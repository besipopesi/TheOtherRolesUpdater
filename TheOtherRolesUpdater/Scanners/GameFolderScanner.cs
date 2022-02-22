using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

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
                string gameFolderPath = Path.Combine(steamPath, MagicStrings.AMONG_US_DEFAULT_FOLDER);
                if (File.Exists(Path.Combine(gameFolderPath, MagicStrings.AMONG_US_EXE_FILENAME)))
                {
                    _foundAmongUsFolders.Add(new AmongUsFolder(gameFolderPath));
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
