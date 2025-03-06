namespace SalesToolPoc.ApiService.Email;

public class GmailOptions
{
    public string? ClientSecret { get; set; }
    public string? ClientId { get; set; }
    public string[] Scopes = { Google.Apis.Gmail.v1.GmailService.Scope.GmailReadonly };
}