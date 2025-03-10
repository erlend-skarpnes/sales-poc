using SalesToolPoc.ApiService.Ai;
using SalesToolPoc.ApiService.Models;

namespace SalesPoc.ApiService.Tests2;

public class FakeAiService: IAiService
{
    private readonly RequestSummary _requestSummary;
    public string EmailBody;

    public FakeAiService(RequestSummary requestSummary)
    {
        _requestSummary = requestSummary;
    }

    public Task<RequestSummary> GetRequestSummary(string emailBody, CancellationToken cancellationToken = default)
    {
        EmailBody = emailBody;
        return Task.FromResult(_requestSummary);
    }
}