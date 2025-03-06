using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.AI;

namespace SalesToolPoc.ApiService.Ai.Client;

public class StructuredOllamaClient : IChatClient
{
    private readonly HttpClient _httpClient;
    public ChatClientMetadata Metadata { get; }
    public JsonSerializerOptions OutgoingJsonSerializerOptions { get; } = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

    public StructuredOllamaClient(string baseUrl, string modelId)
    {
        _httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));
        _httpClient.BaseAddress = new Uri(baseUrl);
        Metadata = new ChatClientMetadata("ollama", new Uri(baseUrl), modelId);
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _httpClient.Dispose();
    }

    public async Task<ChatCompletion> CompleteAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var responseFormat = options?.ResponseFormat?.ToString();
        if (options?.ResponseFormat is ChatResponseFormatJson jsonFormat)
        {
            responseFormat = jsonFormat.Schema;
        }
        
        var request = new OllamaRequest()
        {
            Model = Metadata.ModelId,
            Stream = false,
            Messages = ToOllamaMessages(chatMessages),
            Format = JsonDocument.Parse(responseFormat).RootElement
        };

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/chat");
        requestMessage.Content = new StringContent(JsonSerializer.Serialize(request, OutgoingJsonSerializerOptions), Encoding.UTF8, "application/json");
        
        var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
        var parsedResponse = JsonSerializer.Deserialize<OllamaResponse>(await response.Content.ReadAsStreamAsync(cancellationToken), OutgoingJsonSerializerOptions);

        return new ChatCompletion(new ChatMessage
        {
            Role = ChatRole.Assistant,
            Text = parsedResponse?.Message?.Content,
        });
    }

    public IAsyncEnumerable<StreamingChatCompletionUpdate> CompleteStreamingAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException("This client does not support streaming data.");
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        return serviceKey is null && serviceType?.IsInstanceOfType(this) is true ? this : null;
    }
    
    public static IEnumerable<OllamaMessage> ToOllamaMessages(IEnumerable<ChatMessage> chatMessages)
    {
        return chatMessages.Select(message =>
        {
            var role = "user";
            if (message.Role == ChatRole.User)
            {
                role = "user";
            }
            else if (message.Role == ChatRole.System)
            {
                role = "system";
            }

            return new OllamaMessage
            {
                Role = role,
                Content = message.Text
            };
        });
    }

}

public class LoggingHandler : DelegatingHandler
{
    public LoggingHandler(HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Request:");
        Console.WriteLine(request.ToString());
        if (request.Content != null)
        {
            Console.WriteLine(await request.Content.ReadAsStringAsync());
        }
        Console.WriteLine();

        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        Console.WriteLine("Response:");
        Console.WriteLine(response.ToString());
        if (response.Content != null)
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        Console.WriteLine();

        return response;
    }
}