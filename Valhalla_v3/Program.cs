using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using MudBlazor.Services;
using Valhalla_v3.Components;
using Valhalla_v3.Controller;
using Valhalla_v3.Database;
using Valhalla_v3.Services;
using Valhalla_v3.Services.CarHistory;
using MudBlazor.Services;
using MudBlazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();
builder.Services.AddScoped<IMudPopoverService, MudPopoverService>();

builder.Services.AddMudServices();
builder.Services.AddDbContext<ValhallaComtext>();
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IGasStationService, GasStationService>();
builder.Services.AddScoped<ICarHistoryFuelService, CarHistoryFuelService>();
builder.Services.AddScoped<IMechanicService, MechanicService>();
builder.Services.AddScoped<ICarHistoryRepairService, CarHistoryRepairService>();
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
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization(); 
app.UseAntiforgery();

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
