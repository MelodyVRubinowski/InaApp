 using inaApp.ProyectoINAApp.Extensions;
using inaApp.ProyectoINAApp.Mapping;
using inaApp.Services;
using System.Xml.Linq;

using inaApp.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddAplicationServices(builder.Configuration);
builder.Services.AddScoped<FacturaService, FacturaService>();
builder.Services.AddScoped<ClienteService, ClienteService>();
builder.Services.AddScoped<IFacturaService, FacturaService>();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
