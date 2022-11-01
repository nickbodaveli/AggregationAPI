using AggregationAPI.Context;
using AggregationAPI.Interfaces;
using AggregationAPI.Repository;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


// Add services to the container.

var st = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(st, ServerVersion.AutoDetect(st));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAggregationRepository, AggregationRepository>();
builder.Services.AddHttpClient<IAggregationRepository, AggregationRepository>();
builder.Services.AddScoped<WebClient, WebClient>();
builder.Services.AddScoped<HtmlDocument, HtmlDocument>();


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
