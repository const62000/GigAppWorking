using Carter;
using GigApp.Api.Configurations.JsonConvertor;
using GigApp.Api.Middleware;
using GigApp.Api.Services;
using GigApp.Application;
using GigApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddOption();
builder.Services.AddMappings();
builder.Services.AddInfrastructureLayer().AddApplicationLayer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddCarter();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .WithOrigins(
                "http://localhost:5010",     // Local Blazor development
                "https://localhost:7199",     // Local Blazor HTTPS
                "http://vms.runasp.net",     // Production Blazor
                "https://vms.runasp.net"     // Production Blazor HTTPS
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Configure JSON options to include custom converters

var app = builder.Build();
app.UseStaticFiles();
app.UseExceptionHandler();
// Configure the HTTP request pipeline.

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapCarter();


app.Run();


