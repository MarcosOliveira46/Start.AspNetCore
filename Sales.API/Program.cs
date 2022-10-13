using Microsoft.EntityFrameworkCore;
using Sales.API;
using Sales.API.DataAccess;
using Sales.API.DataAccessNoSql;
using Sales.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<SalesDatabaseSetting>(builder.Configuration.GetSection("SalesDatabase"));

builder.Services.AddScoped<IDataAccess<Item>, ItemDataNoSql>();
builder.Services.AddScoped<IDataAccess<Customer>, CustomerDataNoSql>();
builder.Services.AddScoped<IDataAccess<Order>, OrderDataNoSql>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
