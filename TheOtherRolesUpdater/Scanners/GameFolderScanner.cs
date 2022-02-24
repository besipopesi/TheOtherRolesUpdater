using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Win32;

using Newtonsoft.Json.Linq;

using TheOtherRolesUpdater.Models;

namespace TheOtherRolesUpdater.Scanners
{
    public enum GameFolderScannerType { Quick, Deep }

    public static class GameFolderScanner
    {
        private static readonly List<AmongUsFolder> _foundAmongUsFolders = new List<AmongUsFolder>();

        public static async Task<IEnumerable<AmongUsFolder>> GetGameFolders(GameFolderScannerType scannerType)
        {
            _foundAmongUsFolders.Clear();

            switch (scannerType)
            {
                case GameFolderScannerType.Quick:
                    QuickScan();
                    break;
                case GameFolderScannerType.Deep:
                    await DeepScan();
                    break;
                default:
                    break;
            }

            return _foundAmongUsFolders;
        }

        private static void QuickScan()
        {
            string steamPath = Registry.GetValue(MagicStrings.STEAM_REGISTRY_KEY, MagicStrings.STEAM_REGISTRY_VALUE, string.Empty).ToString() ?? string.Empty;

            if (!string.IsNullOrEmpty(steamPath))
            {
                string gameFolderPath = Path.Combine(steamPath, MagicStrings.STEAM_AMONG_US_FOLDER);
                if (File.Exists(Path.Combine(gameFolderPath, MagicStrings.AMONG_US_EXE_FILENAME)))
                {
                    _foundAmongUsFolders.Add(new AmongUsFolder(gameFolderPath, Platform.Steam));
                }
            }

            string epicGamesPath = Registry.GetValue(MagicStrings.EPIC_GAMES_REGISTRY_KEY, MagicStrings.EPIC_GAMES_REGISTRY_VALUE, string.Empty).ToString() ?? string.Empty;

            if (!string.IsNullOrEmpty(epicGamesPath))
            {
                epicGamesPath = Path.Combine(epicGamesPath, MagicStrings.EPIC_GAMES_MANIFESTS_FOLDER);
                foreach (string manifestFile in Directory.EnumerateFiles(epicGamesPath, "*" + MagicStrings.EPIC_GAMES_MANIFESTS_FILE_EXTENSION))
                {
                    JObject fileContent = JObject.Parse(File.ReadAllText(manifestFile));
                    if (fileContent[MagicStrings.EPIC_GAMES_MANIFEST_ENTRY_LAUNCH_EXECUTABLE].ToString().Contains(MagicStrings.AMONG_US_EXE_FILENAME))
                    {
                        string gameFolderPath = fileContent[MagicStrings.EPIC_GAMES_MANIFEST_ENTRY_INSTALL_LOCATION].ToString();
                        if (File.Exists(Path.Combine(gameFolderPath, MagicStrings.AMONG_US_EXE_FILENAME)))
                        {
                            _foundAmongUsFolders.Add(new AmongUsFolder(gameFolderPath, Platform.EpicGames));
                            break;
                        }
                    }
                }
            }
        }

        private static async Task DeepScan()
        {
            foreach (string drive in Directory.GetLogicalDrives())
            {
                await Task.Run(() => SubFolderSearch(drive));
            }
        }

        private static void SubFolderSearch(string folder)
        {
            foreach (string subfolder in Directory.EnumerateDirectories(folder))
            {
                try
                {
                    SubFolderSearch(subfolder);
                }
                catch { }
            }
            if (File.Exists(Path.Combine(folder, MagicStrings.AMONG_US_EXE_FILENAME)))
            {
                _foundAmongUsFolders.Add(new AmongUsFolder(folder));
            }
        }
    }
}
