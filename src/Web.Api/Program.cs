using Carter;
using MassTransit;
using Teeitup.Core.Application;
using Teeitup.Core.Infrastructure;
using Teeitup.Core.Infrastructure.Database;
using Teeitup.Core.Infrastructure.Extensions;
using Teeitup.ServiceDefaults;
using Teeitup.Web.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddEndpoints();
builder.Services.AddProblemDetailsForTeeitup();

builder.Services.AddMassTransit(x =>
{
    var connectionString = builder.Configuration.GetConnectionString("messaging");
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(connectionString);
        cfg.ConfigureEndpoints(context);
    });
});
var app = builder.Build();
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    builder.Services.EnsureDbCreated<AppointerDbContext>();
}

app.MapCarter();
app.UseHttpsRedirection();
app.Run();

namespace Teeitup.Web.Api
{
    public class Program;
}