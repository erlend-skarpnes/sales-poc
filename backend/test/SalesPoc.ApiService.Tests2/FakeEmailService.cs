using SalesToolPoc.ApiService.Email;

namespace SalesPoc.ApiService.Tests2;

public class FakeEmailService: IEmailService
{
    private readonly ICollection<Email> _emails;

    public FakeEmailService(ICollection<Email> emails)
    {
        _emails = emails;
    }
    public async Task<ICollection<Email>> GetRequestEmails(string[] idsToExclude)
    {
        return _emails;
    }
}