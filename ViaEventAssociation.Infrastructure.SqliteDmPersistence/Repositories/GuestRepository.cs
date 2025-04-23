using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Infrastructure.SqliteDmPersistence.Repositories
{
    public class GuestRepository : Repository<Guest, GuestId>, IGuestRepository
    {
        public GuestRepository(SqliteDmContext context) : base(context) { }

        public async Task<bool> EmailExists(string email)
        {
            return await _dbSet.AnyAsync(g => g.Email.Value == email.ToLower());
        }

        public async Task<Guest?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(g => g.Participations)
                .FirstOrDefaultAsync(g => g.Email.Value == email.ToLower());
        }
    }
}