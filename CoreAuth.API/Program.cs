using CoreAuth.Application.Extensions;
using CoreAuth.Persistence.Extensions;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CoreAuth API",
        Description = "An ASP.NET Core Web API for managing authentication and authorization."
    });
});

builder.Services.AddControllers();
builder.Services
    .AddPersistenceServices(builder.Configuration)
    .AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
