using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

using TheOtherRolesUpdater.Scanners;

namespace TheOtherRolesUpdater.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Platform
    {
        Unknown, Steam, EpicGames
    }

    public class AmongUsFolder
    {
        public string Path { get; set; }
        public string GameVersion { get; set; }
        public Platform Platform { get; private set; }


        public AmongUsFolder(string path, Platform platform = Platform.Unknown)
        {
            Path = path;
            Platform = platform;
            Refresh();
        }

        public void Refresh()
        {
            GameVersion = InstalledVersionScanner.GetInstalledVersion(Path);
        }
    }
}
