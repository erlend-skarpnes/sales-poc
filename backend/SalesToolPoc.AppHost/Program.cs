using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var mongodb = builder.AddMongoDB("mongo")
    .WithDataVolume("mongo-data")
    .WithMongoExpress()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("mongodb", "salesdb");

builder.AddProject<SalesToolPoc_ApiService>("apiservice")
    .WithReference(mongodb)
    .WaitFor(mongodb);

builder.Build().Run();
