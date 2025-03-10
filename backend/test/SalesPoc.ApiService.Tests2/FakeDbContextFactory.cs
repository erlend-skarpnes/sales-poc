using Microsoft.EntityFrameworkCore;
using SalesToolPoc.ApiService.Database;

namespace SalesPoc.ApiService.Tests2;

public class FakeDbContextFactory: IDbContextFactory<SalesPocDbContext>
{
    private readonly SalesPocDbContext _dbContext;

    public FakeDbContextFactory(SalesPocDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public SalesPocDbContext CreateDbContext()
    {
        return _dbContext;
    }
}