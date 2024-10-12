using Microsoft.AspNetCore.ResponseCompression;
using Valhalla_v3.Components;
using Valhalla_v3.Database;
using Valhalla_v3.Hubs;
using Valhalla_v3.Services;
using Valhalla_v3.Services.CarHistory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();
builder.Services.AddDbContext<ValhallaComtext>();
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddBlazorBootstrap();
builder.Services.AddSignalR();

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
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.MapBlazorHub();
app.MapHub<CarHub>("/carhub");
//app.MapFallbackToPage("/_Host");
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(Valhalla_v3.Client._Imports).Assembly);

app.Run();
