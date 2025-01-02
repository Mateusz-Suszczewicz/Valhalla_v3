using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using System.Globalization;
using System.Text;
using Valhalla_v3.Client.Service;
using Valhalla_v3.Components;
using Valhalla_v3.Database;
using Valhalla_v3.Services;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Services.ToDo;
using Valhalla_v3.Shared;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IGasStationService, GasStationService>();
builder.Services.AddScoped<ICarHistoryFuelService, CarHistoryFuelService>();
builder.Services.AddScoped<IMechanicService, MechanicService>();
builder.Services.AddScoped<ICarHistoryRepairService, CarHistoryRepairService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddBlazorBootstrap(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient("MyHttpClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7070"); // Ustaw w³aœciwy adres bazowy
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("Connection");
}
else
{
    connection = Environment.GetEnvironmentVariable("Connection");
}

builder.Services.AddDbContext<ValhallaContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddIdentity<Operator, IdentityRole<int>>()
    .AddEntityFrameworkStores<ValhallaContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Cookies["AuthCookie"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddTransient<AuthorizationMessageHandler>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuthenticationStateProvider,
    CustomAuthenticationStateProvider>();

builder.Services.AddAuthorizationCore(config =>
{
    config.AddPolicy(Policies.IsAdmin, Policies.IsUserLogged());
    config.AddPolicy(Policies.IsUserLog, Policies.IsUserLogged());
    config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
    config.AddPolicy(Policies.IsClaim, Policies.IsClaimed());
});



builder.Services.Configure<CircuitOptions>(options => options.DetailedErrors = true);

var app = builder.Build();
app.UseResponseCompression();


if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
    
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}
var defaultCulture = new CultureInfo("pl-PL");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture }
};
app.UseRequestLocalization(localizationOptions);
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization(); 
app.UseAntiforgery();
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ValhallaContext>();
        dbContext.Database.Migrate();
    }
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}

app.MapControllers();
app.UseStaticFiles();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(Valhalla_v3.Client._Imports).Assembly);



// SeedRoles podczas uruchamiania aplikacji
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    await SeedRoles(roleManager);
}

async Task SeedRoles(RoleManager<IdentityRole<int>> roleManager)
{
    var roles = new[] { "Admin", "User", "Manager" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
        }
    }
}



app.Run();



