using inaApp.Api.Extensions;
using inaApp.Common.Interfaces;
using inaApp.Repository;
using inaApp.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //Devolver el enum como texto
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

//Inyección de dependencias en archivos
builder.Services.AddAplicaServices(builder.Configuration);

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
