using System.Text.Json;
using Microsoft.Extensions.AI;
using SalesToolPoc.ApiService.Models;

namespace SalesPoc.ApiService.Tests.AI;

public class DummyChatClient : IChatClient
{
    public void Dispose()
    {
        // throw new NotImplementedException();
    }

    public Task<ChatCompletion> CompleteAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        JsonSerializerOptions jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var chatMessage = chatMessages.Where(r => r.Role == ChatRole.User).Select(c => c.Text!).First();

        if (options == null )
            return Task.FromResult(new ChatCompletion(new ChatMessage(ChatRole.User, chatMessage)));
        var res = new RequestSummary
        {
            Title = chatMessage,
            Customer = "customer",
            Role = "role",
            Summary = "Summary",
            Deadline = DateTime.Now,
        };
        var requestSummary = JsonSerializer.Serialize(res, jsonSerializerOptions);
        return Task.FromResult(new ChatCompletion(new ChatMessage(ChatRole.Assistant, requestSummary)));
    }

    public IAsyncEnumerable<StreamingChatCompletionUpdate> CompleteStreamingAsync(IList<ChatMessage> chatMessages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        throw new NotImplementedException();
    }

    public ChatClientMetadata Metadata { get; }
}