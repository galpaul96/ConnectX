var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var mongoDb = builder.AddMongoDB("mongodb").AddDatabase("mydb");
var postgres = builder.AddPostgres("pgsql")
    .WithDataVolume("connectx-postgres-data");

var apiDb = postgres.AddDatabase("apidb");
var webDb = postgres.AddDatabase("webdb");
var identityDb = postgres.AddDatabase("identitydb");


var apiService = builder.AddProject<Projects.ConnectX_Api>("api")
    .WithHttpHealthCheck("/health")
    .WithReference(apiDb)
    .WaitFor(apiDb)
    .WithReference(mongoDb)
    .WaitFor(mongoDb);

builder.AddProject<Projects.ConnectX_Web>("web")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithReference(webDb)
    .WaitFor(webDb)
    .WithReference(identityDb)
    .WaitFor(identityDb);

builder.Build().Run();
