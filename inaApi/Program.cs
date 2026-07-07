using inaApp.Common.Interfaces;
using inaApp.Services;
using inaApp.Repository;
using inaApp.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAplicationServices(builder.Configuration);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
