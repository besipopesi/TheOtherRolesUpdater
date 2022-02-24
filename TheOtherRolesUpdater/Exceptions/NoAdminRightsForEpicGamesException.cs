using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
