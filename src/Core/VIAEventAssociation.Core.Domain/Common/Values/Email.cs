using System.Text.RegularExpressions;

namespace VIAEventAssociation.Core.Domain.Common.Values;

public class Email
{
    public string Value { get; init; }
    
    public Email(string value)
    {
        Value = value;
        Validate();
    }
    
    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            throw new ArgumentException("Email cannot be empty");
        }
        
        if (!Regex.IsMatch(Value, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
        {
            throw new ArgumentException("Email is not valid");
        }
        
        if (!Value.Contains("@via.dk", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Email cannot be from VIA");
        }
    }
}