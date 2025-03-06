namespace SalesToolPoc.ApiService.Models;

public class RequestSummary
{
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required string Role { get; init; }
    public required string Customer { get; init; }
    public DateTime Deadline { get; init; }
}