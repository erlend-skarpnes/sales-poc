namespace SalesToolPoc.ApiService.Salesforce.Dtos;

public class OpportunityLookupDto
{
    public IDictionary<string, SearchResult> lookupResults { get; set; }
}

public class SearchResult
{
    public Records[] records { get; set; }
}

public class Records
{
    public LookupFields fields { get; set; }
}

public class LookupFields
{
    public Id Id { get; set; }
    public Name Name { get; set; }
}

public class Id
{
    public string value { get; set; }
}

public class Name
{
    public string value { get; set; }
}
