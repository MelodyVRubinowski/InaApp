using inaApp.Common.interfaces;
using inaApp.Services;
using inaApp.Repository;
using inaApp.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//inyecciones de dependencias 
//registro contenedor de inyecciones de dependencias 
builder.Services.AddAplicationServices(builder.Configuration);


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
