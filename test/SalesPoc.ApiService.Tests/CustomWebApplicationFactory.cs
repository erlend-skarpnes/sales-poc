using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SalesPoc.ApiService.Tests.AI;
using SalesToolPoc.ApiService.Models;

namespace SalesPoc.ApiService.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string TestContent = "TEST CONTENT";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => { services.AddSingleton<IChatClient, DummyChatClient>(); });
    }

    [Fact]
    public async void FirstTest()
    {
        JsonSerializerOptions options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await using var app = new CustomWebApplicationFactory();
        var client = app.CreateClient();
        var aiResponse = await client.PostAsync("/ai", new StringContent(TestContent));
        aiResponse.EnsureSuccessStatusCode();
        var stringResponse = await aiResponse.Content.ReadAsStringAsync();
        var model = JsonSerializer.Deserialize<RequestSummary>(stringResponse, options);
        Assert.NotNull(model);
    }
}