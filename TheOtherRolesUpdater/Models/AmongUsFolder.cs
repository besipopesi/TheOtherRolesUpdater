using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheOtherRolesUpdater.Scanners;

namespace TheOtherRolesUpdater.Models
{
    public class AmongUsFolder
    {
        public string Path { get; set; }
        public string GameVersion { get; set; }

        public AmongUsFolder(string path)
        {
            Path = path;
            Refresh();
        }

        public void Refresh()
        {
            GameVersion = InstalledVersionScanner.Instance.GetInstalledVersion(Path);
        }
    }
}
