using System;
using System.Collections.Generic;

namespace ViaEventAssociation.Infrastructure.EfcQueries;

public partial class Event
{
    public string Id { get; set; } = null!;

    public DateTime? EventStart { get; set; }

    public DateTime? EventEnd { get; set; }

    public string Visibility { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int NumberOfGuests { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Participation> Participations { get; set; } = new List<Participation>();
}
