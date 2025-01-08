var builder = DistributedApplication.CreateBuilder(args);

var sqlserver = builder
    .AddSqlServer("sqlserver")
    .WithLifetime(ContainerLifetime.Persistent); // persist container between restarts

var database = sqlserver.AddDatabase("database");

builder.AddProject<Projects.Web_Api>("Web-Api")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();


