using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOtherRolesUpdater.Scanners
{
    public class InstalledVersionScanner
    {
        private static readonly Lazy<InstalledVersionScanner> _instance = new Lazy<InstalledVersionScanner>(() => new InstalledVersionScanner());
        public static InstalledVersionScanner Instance => _instance.Value;
        private InstalledVersionScanner() { }

        public string GetInstalledVersion(string gameFolderPath)
        {
            string pluginFile = Path.Combine(gameFolderPath, MagicStrings.THE_OTHER_ROLES_PLUGIN_FOLDER, MagicStrings.THE_OTHER_ROLES_PLUGIN_FILENAME);
            string installedVersion = MagicStrings.NOT_FOUND;

            if (File.Exists(pluginFile))
            {
                FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(pluginFile);
                installedVersion = $"{fileVersion.FileMajorPart}.{fileVersion.FileMinorPart}.{fileVersion.FileBuildPart}";
            }

            return installedVersion;
        }
    }
}
