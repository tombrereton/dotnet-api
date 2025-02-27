using System.Reflection.Metadata;

var builder = DistributedApplication.CreateBuilder(args);

var passwordParameter = builder.AddParameter("sql-password", "Password.1");
var sqlserver = builder
    .AddSqlServer("sqlserver", password: passwordParameter, port: 9999)
    .WithDataVolume() // persists data between restarts
    .WithLifetime(ContainerLifetime.Persistent); // decreases startup time

var database = sqlserver
    .AddDatabase("database");

builder.AddProject<Projects.Web_Api>("Web-Api")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();


