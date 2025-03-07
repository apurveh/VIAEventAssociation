namespace VIAEventAssociation.Core.Domain.Common.Values;

public class Description
{
    public string Value { get; init; }
    
    public Description(string value)
    {
        Value = value;
    }
}