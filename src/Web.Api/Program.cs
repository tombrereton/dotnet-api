using Appointer.ServiceDefaults;
using Appointer.Web.Api.Features;
using Appointer.Web.Api.Infrastructure;
using Appointer.Web.Api.Infrastructure.Database;
using Appointer.Web.Api.Infrastructure.Extensions;
using Carter;

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

namespace Appointer.Web.Api
{
    public partial class Program;
}