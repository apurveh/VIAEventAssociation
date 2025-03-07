namespace VIAEventAssociation.Core.Domain.Common.Values;

public class PersonName
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string FullName { get; private set; }
    
    public PersonName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        Validate();
        
        FullName = $"{FirstName} {LastName}";
    }
    
    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            throw new ArgumentException("First name cannot be empty");
        }
        
        if (string.IsNullOrWhiteSpace(LastName))
        {
            throw new ArgumentException("Last name cannot be empty");
        }
    }
}