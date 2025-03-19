using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public class NameType : ValueObject
{
    private NameType(string value)
    {
        Value = Capitalize(value);
    }
    
    public string Value { get; }

    public static Result<NameType> Create(string value)
    {
        try
        {
            var validation = Validate(value);
            return validation.IsSuccess ? new NameType(value) : validation.Error;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }
    
    private static Result Validate(string value)
    {
        
        var errors = new HashSet<Error>();

        if (string.IsNullOrWhiteSpace(value))
            errors.Add(Error.BlankString);

        if (value.Length < 2 || value.Length > 25)
            errors.Add(Error.InvalidNameLength());

        if (!Regex.IsMatch(value, @"^[a-zA-Z]+$"))
            errors.Add(Error.InvalidName);

        if (errors.Any())
            return Error.Add(errors);
        
        return Result.Ok;
    }
    
    private static string Capitalize(string value)
    {
        return char.ToUpper(value[0]) + value.Substring(1).ToLower();
    }

    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}