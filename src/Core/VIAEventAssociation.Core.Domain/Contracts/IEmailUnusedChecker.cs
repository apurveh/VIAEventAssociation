using ViaEventAssociation.Core.Domain.Common.Values;

namespace ViaEventAssociation.Core.Domain.Contracts;

internal static class IEmailUnusedChecker {
    public static Result<bool> IsEmailUsed(Email email) {
        return false;
    }
}