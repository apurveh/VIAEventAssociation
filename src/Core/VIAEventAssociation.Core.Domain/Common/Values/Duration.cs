namespace VIAEventAssociation.Core.Domain.Common.Values;

public class Duration
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    
    public Duration(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
        Validate();
    }
    
    private void Validate()
    {
        if (Start > End)
        {
            throw new ArgumentException("Start date cannot be greater than end date");
        }
    }
}