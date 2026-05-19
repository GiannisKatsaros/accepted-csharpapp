using CSharpApp.Api.Endpoints.Categories;
using CSharpApp.Api.Endpoints.Products;
using CSharpApp.Api.Middleware;
using CSharpApp.Application;
using CSharpApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<PerformanceLoggingMiddleware>();

//app.UseHttpsRedirection();

var versionedEndpointRouteBuilder = app.NewVersionedApi();

versionedEndpointRouteBuilder.MapProductsEndpoints();
versionedEndpointRouteBuilder.MapCategoriesEndpoints();

app.Run();