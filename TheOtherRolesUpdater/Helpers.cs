using System.Security.Principal;

namespace TheOtherRolesUpdater
{
    internal static class Helpers
    {
        public static bool HasApplicationAdminRights()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }
}
