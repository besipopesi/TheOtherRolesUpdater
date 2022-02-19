using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
