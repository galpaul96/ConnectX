using k8s.Models;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("redis-cache");

var postgres = builder.AddPostgres("pgsql")
    .WithDataVolume("connectx-postgres-data");

var idDb = postgres.AddDatabase("web-identity");
var catalogDb = postgres.AddDatabase("api-db");

builder.AddProject<Projects.ConnectX_Web>("connectx-web")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(idDb)
    .WaitFor(idDb)
    .WithReference(catalogDb)
    .WaitFor(catalogDb);

builder.Build().Run();
