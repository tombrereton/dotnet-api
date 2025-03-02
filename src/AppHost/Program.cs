var builder = DistributedApplication.CreateBuilder(args);

var passwordParameter = builder.AddParameter("sql-password", "Password.1");
var sqlserver = builder
    .AddSqlServer("sqlserver", password: passwordParameter, port: 9988)
    .WithDataVolume() // persists data between restarts
    .WithLifetime(ContainerLifetime.Persistent); // decreases startup time

var database = sqlserver
    .AddDatabase("database");

var messageBroker = builder
    .AddAzureServiceBus("messaging")
    .RunAsEmulator();

builder.AddProject<Projects.Web_Api>("Web-Api")
    .WithReference(database)
    .WithReference(messageBroker)
    .WaitFor(database);

builder.Build().Run();