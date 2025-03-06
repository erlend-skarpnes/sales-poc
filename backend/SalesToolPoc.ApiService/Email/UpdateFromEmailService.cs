using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using SalesToolPoc.ApiService.Ai;
using SalesToolPoc.ApiService.Database;
using SalesToolPoc.ApiService.Database.Models;

namespace SalesToolPoc.ApiService.Email;

public class UpdateFromEmailService : IUpdateFromEmailService
{
    private readonly IDbContextFactory<SalesPocDbContext>  _dbContextFactory;
    private readonly IEmailService _emailService;
    private readonly IAiService _aiService;

    public UpdateFromEmailService(IDbContextFactory<SalesPocDbContext> contextFactory, IEmailService emailService, IAiService aiService)
    {
        _dbContextFactory = contextFactory;
        _emailService = emailService;
        _aiService = aiService;
    }
    
    public async Task Update()
    {
        await using var context = _dbContextFactory.CreateDbContext();
        var alreadyImportedIds = context.Requests.Select(r => r.EmailId).ToArray();
        var newEmails = await _emailService.GetRequestEmails(alreadyImportedIds);

        foreach (var newEmail in newEmails)
        {
            var summary = await _aiService.GetRequestSummary(newEmail.Body, CancellationToken.None);

            var dbRequest = new Request(newEmail.Body, newEmail.Id);
            var dbSummary = new RequestSummary
            {
                _id = new ObjectId(),
                Hash = dbRequest.Hash,
                Title = summary.Title,
                Summary = summary.Summary,
                Role = summary.Role,
                Customer = summary.Customer,
                Deadline = summary.Deadline
            };
            
            context.Requests.Add(dbRequest);
            context.RequestSummaries.Add(dbSummary);
            await context.SaveChangesAsync();
        }
    }
}

public interface IUpdateFromEmailService
{
    Task Update();
}