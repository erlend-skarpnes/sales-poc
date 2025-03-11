using Newtonsoft.Json;
using SalesToolPoc.ApiService.Email;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SalesToolPoc.ApiService.Fakes.Email;

public class EmailFromJsonService : IEmailService
{

    public async Task<ICollection<ApiService.Email.Email>> GetRequestEmails(string[] idsToExclude)
    {
        try
        {
            var jsonString = await File.ReadAllTextAsync("./Fakes/Email/DummyEmails.json");
            var allEmails = JsonSerializer.Deserialize<List<ApiService.Email.Email>>(jsonString);
            var newEmails = allEmails?.Where(x => !idsToExclude.Contains(x.Id)).ToList();
            return newEmails ?? [];
        }
        catch (FileNotFoundException ex)
        {
            throw new FileNotFoundException("File DummyEmails.json not found", ex);
        }
        catch (JsonException ex)
        {
            throw new JsonException("Invalid JSON format in DummyEmails.json", ex);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}