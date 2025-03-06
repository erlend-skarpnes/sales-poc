using System.Text.Json.Serialization;

namespace SalesToolPoc.ApiService.Salesforce.Dtos;

public class OpportunityPicklistOptionsDto
{
    [JsonPropertyName("picklistFieldValues")]
    public required PicklistFieldValues PicklistFieldValues { get; init; }
}

public class PicklistFieldValues
{
    [JsonPropertyName("ForecastCategory")] public Category ForecastCategory { get; init; }

    [JsonPropertyName("ForecastCategoryName")]
    public Category ForecastCategoryName { get; init; }

    [JsonPropertyName("Location__c")] public Category Location { get; init; }
    [JsonPropertyName("Loss_Reason__c")] public Category LossReason { get; init; }

    [JsonPropertyName("Opportunity_Type__c")]
    public Category OpportunityType { get; init; }

    [JsonPropertyName("Skills_area__c")] public Category SkillsArea { get; init; }
    [JsonPropertyName("StageName")] public Category StageName { get; init; }
    [JsonPropertyName("Won_Reason__c")] public Category WonReason { get; init; }

    [JsonPropertyName("Working_location__c")]
    public Category WorkingLocation { get; init; }
}

public class Category
{
    [JsonPropertyName("defaultValue")] public PicklistValue DefaultValue { get; init; }
    [JsonPropertyName("values")] public PicklistValue[] Values { get; init; }
}

public class PicklistValue
{
    [JsonPropertyName("label")] public string Label { get; init; }
    [JsonPropertyName("value")] public string Value { get; init; }
}