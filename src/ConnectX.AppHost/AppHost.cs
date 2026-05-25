var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var db = builder.AddMongoDB("mongodb").AddDatabase("mydb");

var apiService = builder.AddProject<Projects.ConnectX_Api>("api")
    .WithHttpHealthCheck("/health")
    .WithReference(db);

builder.AddProject<Projects.ConnectX_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
