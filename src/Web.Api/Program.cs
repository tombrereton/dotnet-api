using Carter;
using Teeitup.ServiceDefaults;
using Teeitup.Web.Api.Features;
using Teeitup.Web.Api.Infrastructure;
using Teeitup.Web.Api.Infrastructure.Database;
using Teeitup.Web.Api.Infrastructure.Extensions;

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

namespace Teeitup.Web.Api
{
    public partial class Program;
}