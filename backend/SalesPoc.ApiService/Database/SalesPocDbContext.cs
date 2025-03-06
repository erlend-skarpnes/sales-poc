using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using SalesToolPoc.ApiService.Database.Models;

namespace SalesToolPoc.ApiService.Database;

public class SalesPocDbContext : DbContext
{
    private readonly IMongoClient _mongoClient;
    public DbSet<Request> Requests { get; set; }
    public DbSet<RequestSummary> RequestSummaries { get; set; }

    public SalesPocDbContext(DbContextOptions options, IMongoClient mongoClient) : base(options)
    {
        _mongoClient = mongoClient;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Request>().ToCollection("requests");
        modelBuilder.Entity<RequestSummary>().ToCollection("summaries");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMongoDB(_mongoClient, "salesdb");
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }
}