using Carter;
using Microsoft.AspNetCore.Http.Features;
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
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        var httpContext = context.HttpContext;
        context.ProblemDetails.Instance = $"{httpContext.Request.Method}-{httpContext.Request.Path}";
        context.ProblemDetails.Extensions.TryAdd("requestId", httpContext.TraceIdentifier);
        var activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
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
    public partial class Program;
}