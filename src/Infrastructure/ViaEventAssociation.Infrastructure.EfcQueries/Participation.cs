using System;
using System.Collections.Generic;

namespace ViaEventAssociation.Infrastructure.EfcQueries;

public partial class Participation
{
    public string Id { get; set; } = null!;

    public string GuestId { get; set; } = null!;

    public string EventId { get; set; } = null!;

    public int ParticipationType { get; set; }

    public int ParticipationStatus { get; set; }

    public string Discriminator { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;

    public virtual Guest Guest { get; set; } = null!;
}
