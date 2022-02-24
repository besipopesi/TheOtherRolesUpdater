using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using TheOtherRolesUpdater.Models;

namespace TheOtherRolesUpdater.Services
{
    class SavedFolders
    {
        public Dictionary<string, Platform> AmongUsFoldersPaths { get; set; }
        public int SelectedAmongUsFolderIndex { get; set; }
        public SavedFolders()
        {
            AmongUsFoldersPaths = new Dictionary<string, Platform>();
        }
    }

    public static class SavedFoldersService
    {
        public static void WriteSavedFolders(List<AmongUsFolder> amongUsFolders, AmongUsFolder selectedAmongUsFolder)
        {
            if (amongUsFolders == null)
                return;
            
            SavedFolders savedFolders = new SavedFolders();

            foreach (AmongUsFolder amongUsFolder in amongUsFolders)
            {
                savedFolders.AmongUsFoldersPaths.Add(amongUsFolder.Path, amongUsFolder.Platform);
            }

            if (selectedAmongUsFolder != null)
                savedFolders.SelectedAmongUsFolderIndex = amongUsFolders.IndexOf(selectedAmongUsFolder);

            string jsonOutput = JsonConvert.SerializeObject(savedFolders, Formatting.Indented);
            File.WriteAllText(MagicStrings.SETTINGS_FILE, jsonOutput);
        }

        public static (IEnumerable<AmongUsFolder>, AmongUsFolder) ReadSavedFolders()
        {
            List<AmongUsFolder> amongUsFolders = new List<AmongUsFolder>();
            int selectedAmongUsFolderIndex;
            AmongUsFolder selectedAmongUsFolder = null;

            if (File.Exists(MagicStrings.SETTINGS_FILE))
            {
                SavedFolders jsonInput = JsonConvert.DeserializeObject<SavedFolders>(File.ReadAllText(MagicStrings.SETTINGS_FILE));

                Dictionary<string, Platform> amongUsFoldersPaths = jsonInput.AmongUsFoldersPaths;
                selectedAmongUsFolderIndex = jsonInput.SelectedAmongUsFolderIndex;

                foreach ((string path, Platform platform) in amongUsFoldersPaths.Select(x => (x.Key, x.Value)))
                {
                    if (Directory.Exists(path))
                        amongUsFolders.Add(new AmongUsFolder(path, platform));
                }

                if (selectedAmongUsFolderIndex >= 0 && selectedAmongUsFolderIndex < amongUsFolders.Count)
                    selectedAmongUsFolder = amongUsFolders.ElementAt(selectedAmongUsFolderIndex);
            }

            return (amongUsFolders, selectedAmongUsFolder);
        }
    }
}
