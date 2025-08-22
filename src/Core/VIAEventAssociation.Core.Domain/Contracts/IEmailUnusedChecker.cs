using ViaEventAssociation.Core.Domain.Common.Values;

namespace ViaEventAssociation.Core.Domain.Contracts;

public interface IEmailUnusedChecker
{
    Result<bool> IsEmailUsed(string email);
}