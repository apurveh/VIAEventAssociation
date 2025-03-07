using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Entities.Participation;

public abstract class Participation : Entity<ParticipationId>
{
    protected Participation(ParticipationId id, ParticipationStatus participationStatus, ParticipationType participationType) : base(id)
    {
        ParticipationStatus = participationStatus;
        ParticipationType = participationType;
    }
    
    public ParticipationStatus ParticipationStatus { get; }
    public ParticipationType ParticipationType { get; }
}