namespace VIAEventAssociation.Core.Domain.Common.Values;

public class Title
{
    public string Value { get; init; }
    
    public Title(string value)
    {
        Value = value;
        Validate();
    }
    
    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            throw new ArgumentException("Title cannot be empty");
        }
    }
}