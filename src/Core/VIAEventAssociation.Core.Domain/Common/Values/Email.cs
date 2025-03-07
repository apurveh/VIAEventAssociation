using System.Text.RegularExpressions;
using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Common.Values;

public class Email : ValueObject
{
    public string Value { get; }
    
    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        try
        {
            var validation = Validate(email);
            return validation.IsSuccess ? new Email(email) : validation.Error;
        }
        catch (Exception ex)
        {
            return Error.FromException(ex);
        }
    }
    
    private static Result Validate(string email)
    {
        HashSet<Error> errors = new HashSet<Error>();
        
        if (string.IsNullOrWhiteSpace(email))
            errors.Add(Error.BlankString);

        if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            errors.Add(Error.InvalidEmail);

        if (!Regex.IsMatch(email, @"^([\w\.\-]+)@via\.dk$"))
            errors.Add(Error.InvalidEmailDomain);
        
        var localPart = email.Split('@')[0];
        if (localPart.Length < 3 || localPart.Length > 6)
            errors.Add(Error.InvalidEmail);
        
        if (!(Regex.IsMatch(localPart, @"^[a-zA-Z]{3,4}$") || Regex.IsMatch(localPart, @"^\d{6}$")))
            errors.Add(Error.InvalidEmail);

        if (errors.Any())
            return Error.Add(errors);
        
        return Result.Ok;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}