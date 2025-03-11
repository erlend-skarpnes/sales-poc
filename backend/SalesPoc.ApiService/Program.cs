using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using SalesToolPoc.ApiService.Ai;
using SalesToolPoc.ApiService.Ai.Client;
using SalesToolPoc.ApiService.Database;
using SalesToolPoc.ApiService.Email;
using SalesToolPoc.ApiService.Salesforce;
using SalesToolPoc.ApiService.Salesforce.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Add Config
builder.Services.AddOptions<GmailOptions>()
    .BindConfiguration("Gmail");
builder.Services.AddOptions<SalesforceOptions>()
    .BindConfiguration("Salesforce");

// Add Ai related services
builder.Services.AddSingleton<IChatClient>(_ => new StructuredOllamaClient("http://localhost:11434", "gemma2"));
builder.Services.AddSingleton<IAiService, AiService>();

builder.Services.AddSingleton<ISalesforceClient, SalesforceClient>();

// Add MongoDB
builder.AddMongoDBClient(connectionName: "mongodb");
builder.Services.AddDbContextFactory<SalesPocDbContext>();

// Email related services
builder.Services.AddSingleton<IEmailService, GmailService>();
builder.Services.AddSingleton<IUpdateFromEmailService, UpdateFromEmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

string[] summaries =
    ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.MapPost("/ai", async (IAiService client, HttpRequest request) =>
    {
        using var reader = new StreamReader(request.Body);
        var content = await reader.ReadToEndAsync();
        var requestSummary = await client.GetRequestSummary(content);
        return requestSummary;
    })
    .WithName("AiTest");

app.MapGet("/summaries", async (SalesPocDbContext dbContext) =>
{
    var summaries = await dbContext.RequestSummaries.ToListAsync();
    return summaries;
});

app.MapGet("/update", async (IUpdateFromEmailService updateFromEmailService) => { updateFromEmailService.Update(); });

app.MapGet("/form-input", async (ISalesforceClient client) => await client.GetFormOptions());

app.MapGet("/form-search",
    async (ISalesforceClient client, [FromQuery] string q, CancellationToken cancellationToken) =>
    {
        var searchResults = await client.GetSearchResults(q, cancellationToken);
        return searchResults.lookupResults["Account"].records.Select(record => new
        {
            Name = record.fields.Name.value,
            Id = record.fields.Id.value
        });
    });

app.MapPost("/form", async ([FromForm] OpportunityForm form, ISalesforceClient client) =>
{
    var errors = new List<FormError>();

    if (string.IsNullOrWhiteSpace(form.Name)) errors.Add(new FormError("Name", "Name is required"));
    if (string.IsNullOrWhiteSpace(form.Description))
        errors.Add(new FormError("Description", "Description is required"));

    var req = new OpportunityCreateDto()
    {
        Fields = new Dictionary<string, object?>
        {
            ["Name"] = form.Name,
            ["Description"] = form.Description,
            ["AccountId"] = form.Account,
            ["StageName"] = form.Stage,
            ["Opportunity_Type__c"] = form.OpportunityType,
            ["Location__c"] = form.MilesOffice,
            ["Amount"] = form.ContractValue,
            ["CloseDate"] = form.CloseDate,
            ["Assignment_start_date__c"] = form.StartDate,
            ["Assignment_end_date__c"] = form.EndDate
        }
    };

    var res = await client.CreateRecord(req);

    return res ? Results.Ok() : Results.BadRequest(errors);
}).DisableAntiforgery();

app.MapGet("/record-defaults", async (ISalesforceClient client) => { return await client.GetRecordDefaults(); });


app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record OpportunityForm
{
    public string? Name { get; init; }
    public string? Description { get; init; }

    public string? Account { get; init; }

    public string? OpportunityType { get; init; }

    public string? MilesOffice { get; init; }

    public string? Stage { get; init; }

    public int? ContractValue { get; init; }

    public DateOnly? CloseDate { get; init; }

    public DateOnly? StartDate { get; init; }

    public DateOnly? EndDate { get; init; }
};

public record FormError(string Name, string Message);

public partial class Program;