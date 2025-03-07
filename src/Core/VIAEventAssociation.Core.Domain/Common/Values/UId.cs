namespace VIAEventAssociation.Core.Domain.Common.Values;

public class UId
{
    public Guid Id { get; init; }
    
    public UId(Guid id)
    {
        Id = id;
        Validate();
    }
    
    private void Validate()
    {
        if (Id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty");
        }
    }
}