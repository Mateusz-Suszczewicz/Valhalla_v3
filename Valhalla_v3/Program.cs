using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;
using System.Globalization;
using Valhalla_v3.Components;
using Valhalla_v3.Database;
using Valhalla_v3.Services;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Services.ToDo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();
builder.Services.AddScoped<IMudPopoverService, MudPopoverService>();

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

var app = builder.Build();
app.UseResponseCompression();
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
    
    //app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//    endpoints.MapFallbackToFile("");
//});
app.MapControllers();
app.UseStaticFiles();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(Valhalla_v3.Client._Imports).Assembly);
app.Run();
