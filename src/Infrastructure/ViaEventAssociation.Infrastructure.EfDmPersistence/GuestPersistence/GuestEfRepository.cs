using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.Agregates.Guests;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.GuestPersistence;

public class GuestEfRepository : RepositoryEfBase<Guest, GuestId>, IGuestRepository {
 public GuestEfRepository(DbContext context) : base(context) {}
}