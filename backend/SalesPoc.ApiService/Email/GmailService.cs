using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SalesToolPoc.ApiService.Database;

namespace SalesToolPoc.ApiService.Email;

public interface IEmailService
{
    public Task<ICollection<Email>> GetRequestEmails(string[] idsToExclude);
}

public record Email(string Id, string Body);

public class GmailService : IEmailService
{
    private readonly GmailOptions _options;
    private readonly SalesPocDbContext _dbContext;
    static string ApplicationName = "Gmail API .NET Quickstart";

    public GmailService(IOptions<GmailOptions> options)
    {
        _options = options.Value;
    }

    public async Task<ICollection<Email>> GetRequestEmails(string[] idsToExclude)
    {
        var credentials =
            await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets()
                {
                    ClientSecret = _options.ClientSecret,
                    ClientId = _options.ClientId
                },
                _options.Scopes,
                "user",
                CancellationToken.None
            );

        // Create Gmail API service.
        var service = new Google.Apis.Gmail.v1.GmailService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credentials,
            ApplicationName = ApplicationName
        });

        // Define parameters of request.
        var emailRequest = service.Users.Messages.List("me");
        emailRequest.Q = "to:avrop@miles.no";
        var messages = await emailRequest.ExecuteAsync();

        var notImportedMessages = messages.Messages.Where(message => !idsToExclude.Contains(message.Id));

        var newEmails = new List<Email>();
        
        foreach (var message in notImportedMessages)
        {
            var req = service.Users.Messages.Get("me", message.Id);
            req.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;
            var res = await req.ExecuteAsync();
            newEmails.Add(new Email(res.Id, ExtractEmailPlainText(res.Payload)));
        }

        return newEmails;
    }

    private static string ExtractEmailPlainText(MessagePart payload)
    {
        var stringBuilder = new StringBuilder();

        return ParsePartTree(payload, stringBuilder).ToString();
    }

    private static StringBuilder ParsePartTree(MessagePart messagePart, StringBuilder builder)
    {
        if (messagePart.MimeType == "text/plain")
        {
            var utf8String = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(messagePart.Body.Data));
            builder.Append(utf8String);
        }

        if (messagePart.Parts == null) return builder;

        foreach (var part in messagePart.Parts)
        {
            ParsePartTree(part, builder);
        }

        return builder;
    }
}

