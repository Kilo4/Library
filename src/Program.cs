using Microsoft.EntityFrameworkCore;
using Webapi.Models.DbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Webapi;
using Microsoft.OpenApi.Models;
using System.Reflection;
using RabbitMQ.Client;
using Webapi.Helpers;
using Webapi.Services;
using Webapi.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
// Add postgres
var connectionString = builder.Configuration.GetConnectionString("Default");
var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
// AddScope
builder.Services.AddScoped<ICalculationService, CalculationService>();
// AddSingleton
builder.Services.AddSingleton<ConnectionFactory>(_ => new ConnectionFactory() { Uri = new Uri(rabbitMqConfig.GetValue<string>("ConnectionStrings"))});
builder.Services.AddSingleton<IRabbitMqHelper, RabbitMqHelper>();
builder.Services.AddSingleton<IKeyValueStorage, KeyValueStorage>();
builder.Services.AddHostedService(provider =>
{
    var service = new RabbitMqConsumerService(rabbitMqConfig);
    return service;
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Library API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


var app = builder.Build();

// Run migration
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapDefaultControllerRoute();
app.Run();
