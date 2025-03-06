using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SalesToolPoc.ApiService.Salesforce.Dtos;

namespace SalesToolPoc.ApiService.Salesforce;

public interface ISalesforceClient
{
    public Task<OpportunityDefaultsDto> GetRecordDefaults();
    public Task<OpportunityPicklistOptionsDto?> GetFormOptions();
    public Task<bool> CreateRecord(OpportunityCreateDto record);
    public Task<OpportunityLookupDto?> GetSearchResults(string query, CancellationToken cancellationToken);
}

public class SalesforceClient : ISalesforceClient
{
    private readonly SalesforceOptions _options;
    private readonly HttpClient _client;
    private volatile string? _accessToken;

    public SalesforceClient(IOptions<SalesforceOptions> options)
    {
        _options = options.Value;
        _client = new HttpClient();
    }

    public async Task<OpportunityDefaultsDto> GetRecordDefaults()
    {
        var token = await GetAccessToken();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("https://miles-no.my.salesforce.com/services/data/v62.0/ui-api/record-defaults/template/create/Opportunity");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<OpportunityDefaultsDto>(responseBody);
        return responseObject;
    }
    
    public async Task<bool> CreateRecord(OpportunityCreateDto record)
    {
        var token = await GetAccessToken();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var content = new StringContent(JsonSerializer.Serialize(record), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("https://miles-no.my.salesforce.com/services/data/v62.0/ui-api/records", content);
        var responseBody = await response.Content.ReadAsStringAsync();
        var isSuccess = response.IsSuccessStatusCode;
        return isSuccess;
    }

    public async Task<OpportunityPicklistOptionsDto?> GetFormOptions()
    {
        var token = await GetAccessToken();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("https://miles-no.my.salesforce.com/services/data/v62.0/ui-api/object-info/Opportunity/picklist-values/012000000000000AAA");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<OpportunityPicklistOptionsDto>(responseBody);
        return responseObject;
    }

    public async Task<OpportunityLookupDto?> GetSearchResults(string query, CancellationToken cancellationToken)
    {
        var token = await GetAccessToken();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        const string url = "https://miles-no.my.salesforce.com/services/data/v62.0/ui-api/lookups/Opportunity/AccountId";
        var param = new Dictionary<string, string> { { "searchType", "TypeAhead" }, {"q", query } };
        var uri = new Uri(QueryHelpers.AddQueryString(url, param));
        
        var response = await _client.GetAsync(uri, cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OpportunityLookupDto>(responseBody);;
    }

    private async Task<string> GetAccessToken()
    {
        if (_accessToken != null) return _accessToken;

        var requestBody = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", _options.ClientId),
            new KeyValuePair<string, string>("client_secret", _options.ClientSecret)
        });

        var response = await _client.PostAsync(_options.TokenUrl, requestBody);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        // Parse the response to extract the access token
        _accessToken = ExtractAccessToken(responseBody);;
        return _accessToken;
    }

    private string ExtractAccessToken(string responseBody)
    {
        return JsonDocument.Parse(responseBody).RootElement.GetProperty("access_token").GetString() ?? 
               throw new Exception("Could not extract the access token");
    }
}

