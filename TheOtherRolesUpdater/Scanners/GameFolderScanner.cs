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

    public class GameFolderScanner
    {
        private static readonly Lazy<GameFolderScanner> _instance = new Lazy<GameFolderScanner>(() => new GameFolderScanner());
        public static GameFolderScanner Instance => _instance.Value;
        private GameFolderScanner() { _foundAmongUsFolders = new List<AmongUsFolder>(); }

        private readonly List<AmongUsFolder> _foundAmongUsFolders;

        public async Task<List<AmongUsFolder>> GetGameFolders(GameFolderScannerType scannerType)
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

        private void QuickScan()
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

        private async Task DeepScan()
        {
            foreach (string drive in Directory.GetLogicalDrives())
            {
                await Task.Run(() => SubFolderSearch(drive));
            }
        }

        private void SubFolderSearch(string folder)
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
