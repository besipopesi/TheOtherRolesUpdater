using System;

namespace TheOtherRolesUpdater.Exceptions
{
    internal class NoAdminRightsForEpicGamesException : Exception
    {
        public NoAdminRightsForEpicGamesException()
        {
            Source = nameof(NoAdminRightsForEpicGamesException);
        }
    }
}
