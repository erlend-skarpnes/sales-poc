namespace SalesToolPoc.ApiService.Salesforce.Dtos;

public class OpportunityDefaultsDto
{
    public ObjectInfos objectInfos { get; set; }
}

public class ObjectInfos
{
    public Opportunity2 Opportunity { get; set; }
}

public class Opportunity
{
    public string apiName { get; set; }
    public Fields fields { get; set; }
}

public class Opportunity2
{
    public string apiName { get; set; }
    public Dictionary<string, Field> fields { get; set; }
}

public class Fields
{
    public Field AccountId { get; set; }
    public Field Amount { get; set; }
    public Field Assignment_end_date__c { get; set; }
    public Field Assignment_start_date__c { get; set; }
    public Field CloneSourceId { get; set; }
    public Field CloseDate { get; set; }
    public Field Closing_note__c { get; set; }
    public Field Cobrief__c { get; set; }
    public Field ContactId { get; set; }
    public Field CreatedById { get; set; }
    public Field CreatedDate { get; set; }
    public Field Deadline__c { get; set; }
    public Field Description { get; set; }
    public Field Fiscal { get; set; }
    public Field FiscalQuarter { get; set; }
    public Field FiscalYear { get; set; }
    public Field Flextribe__c { get; set; }
    public Field ForecastCategory { get; set; }
    public Field ForecastCategoryName { get; set; }
    public Field HasOpenActivity { get; set; }
    public Field HasOpportunityLineItem { get; set; }
    public Field HasOverdueTask { get; set; }
    public Field Id { get; set; }
    public Field IsClosed { get; set; }
    public Field IsDeleted { get; set; }
    public Field IsWon { get; set; }
    public Field LastActivityDate { get; set; }
    public Field LastAmountChangedHistoryId { get; set; }
    public Field LastCloseDateChangedHistoryId { get; set; }
    public Field LastModifiedById { get; set; }
    public Field LastModifiedDate { get; set; }
    public Field LastReferencedDate { get; set; }
    public Field LastStageChangeDate { get; set; }
    public Field LastViewedDate { get; set; }
    public Field Location__c { get; set; }
    public Field Loss_Reason__c { get; set; }
    public Field Mercell_EU_Supply__c { get; set; }
    public Field Name { get; set; }
    public Field Opportunity_Type__c { get; set; }
    public Field OwnerId { get; set; }
    public Field Position_fraction__c { get; set; }
    public Field Pricebook2Id { get; set; }
    public Field Probability { get; set; }
    public Field Proposed_Emplyee__c { get; set; }
    public Field PushCount { get; set; }
    public Field Second_Consultant__c { get; set; }
    public Field Skills_area__c { get; set; }
    public Field StageName { get; set; }
    public Field SystemModstamp { get; set; }
    public Field Total_contract_value__c { get; set; }
    public Field Won_Reason__c { get; set; }
    public Field Working_location__c { get; set; }
    public Field calc_24_10_days__c { get; set; }
    public Field calc_24_10_revenue__c { get; set; }
    public Field calc_24_11_days__c { get; set; }
    public Field calc_24_11_revenue__c { get; set; }
    public Field calc_24_12_days__c { get; set; }
    public Field calc_24_12_revenue__c { get; set; }
    public Field calc_25_01_days__c { get; set; }
    public Field calc_25_01_revenue__c { get; set; }
    public Field calc_25_02_days__c { get; set; }
    public Field calc_25_02_revenue__c { get; set; }
    public Field calc_25_03_days__c { get; set; }
    public Field calc_25_03_revenue__c { get; set; }
    public Field calc_25_04_days__c { get; set; }
    public Field calc_25_04_revenue__c { get; set; }
    public Field calc_25_05_days__c { get; set; }
    public Field calc_25_05_revenue__c { get; set; }
    public Field calc_25_06_days__c { get; set; }
    public Field calc_25_06_revenue__c { get; set; }
    public Field calc_Days_total__c { get; set; }
    public Field frame_agreement__c { get; set; }
}

public class Field
{
    public string apiName { get; set; }
    public bool calculated { get; set; }
    public bool compound { get; set; }
    public object compoundComponentName { get; set; }
    public object compoundFieldName { get; set; }
    public object controllerName { get; set; }
    public object[] controllingFields { get; set; }
    public bool createable { get; set; }
    public bool custom { get; set; }
    public string dataType { get; set; }
    public bool externalId { get; set; }
    public object extraTypeInfo { get; set; }
    public bool filterable { get; set; }
    public object filteredLookupInfo { get; set; }
    public bool highScaleNumber { get; set; }
    public bool htmlFormatted { get; set; }
    public object inlineHelpText { get; set; }
    public string label { get; set; }
    public int length { get; set; }
    public object maskType { get; set; }
    public bool nameField { get; set; }
    public bool polymorphicForeignKey { get; set; }
    public int precision { get; set; }
    public bool reference { get; set; }
    public object referenceTargetField { get; set; }
    public ReferenceToInfos[] referenceToInfos { get; set; }
    public string relationshipName { get; set; }
    public bool required { get; set; }
    public int scale { get; set; }
    public bool searchPrefilterable { get; set; }
    public bool sortable { get; set; }
    public bool unique { get; set; }
    public bool updateable { get; set; }
}

public class ReferenceToInfos
{
    public string apiName { get; set; }
    public string[] nameFields { get; set; }
}
