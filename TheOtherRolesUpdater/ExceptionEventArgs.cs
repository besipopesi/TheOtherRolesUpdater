using System;

using TheOtherRolesUpdater.Exceptions;

namespace TheOtherRolesUpdater
{
    public class ExceptionEventArgs : EventArgs
    {
        public string Message { get; }
        public string Caption { get; }

        public ExceptionEventArgs(Exception ex)
        {
            if (ex.Message.Equals("Not Found") && ex.Source.Equals("Octokit"))
            {
                Message = MagicStrings.EXCEPTION_DOWNLOAD_FAILED_MESSAGE;
                Caption = MagicStrings.EXCEPTION_DOWNLOAD_FAILED_CAPTION;
            }

            if (ex.Source.Equals(nameof(NoAdminRightsForEpicGamesException)))
            {
                Message = MagicStrings.EXCEPTION_NO_ADMIN_RIGHTS_FOR_EPIC_GAMES_MESSAGE;
                Caption = MagicStrings.EXCEPTION_NO_ADMIN_RIGHTS_FOR_EPIC_GAMES_CAPTION;
            }
        }
    }
}
