using Carter;
using Web.Api.Features;
using Web.Api.Infrastructure;
using Web.Api.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFeatures();
builder.Services.AddInfrastructure(builder.Configuration);

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

namespace Web.Api
{
    public partial class Program;
}