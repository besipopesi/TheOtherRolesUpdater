using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using TheOtherRolesUpdater.Models;

namespace TheOtherRolesUpdater.Services
{
    class SavedFolders
    {
        public List<string> AmongUsFoldersPaths { get; set; }
        public int SelectedAmongUsFolderIndex { get; set; }
        public SavedFolders()
        {
            AmongUsFoldersPaths = new List<string>();
        }
    }

    public class SavedFoldersService
    {
        private static readonly Lazy<SavedFoldersService> _instance = new Lazy<SavedFoldersService>(() => new SavedFoldersService());
        public static SavedFoldersService Instance => _instance.Value;
        private SavedFoldersService() { }


        public void WriteSavedFolders(List<AmongUsFolder> amongUsFolders, AmongUsFolder selectedAmongUsFolder)
        {
            if (amongUsFolders == null)
                return;
            
            SavedFolders savedFolders = new SavedFolders();

            foreach (AmongUsFolder amongUsFolder in amongUsFolders)
            {
                savedFolders.AmongUsFoldersPaths.Add(amongUsFolder.Path);
            }

            if (selectedAmongUsFolder != null)
                savedFolders.SelectedAmongUsFolderIndex = amongUsFolders.IndexOf(selectedAmongUsFolder);

            string jsonOutput = JsonConvert.SerializeObject(savedFolders, Formatting.Indented);
            File.WriteAllText(MagicStrings.SETTINGS_FILE, jsonOutput);
        }

        public (List<AmongUsFolder>, AmongUsFolder) ReadSavedFolders()
        {
            List<AmongUsFolder> amongUsFolders = new List<AmongUsFolder>();
            int selectedAmongUsFolderIndex;
            AmongUsFolder selectedAmongUsFolder = null;

            if (File.Exists(MagicStrings.SETTINGS_FILE))
            {
                SavedFolders jsonInput = JsonConvert.DeserializeObject<SavedFolders>(File.ReadAllText(MagicStrings.SETTINGS_FILE));

                List<string> amongUsFoldersPaths = jsonInput.AmongUsFoldersPaths;
                selectedAmongUsFolderIndex = jsonInput.SelectedAmongUsFolderIndex;

                foreach (string path in amongUsFoldersPaths)
                {
                    if (Directory.Exists(path))
                        amongUsFolders.Add(new AmongUsFolder(path));
                }

                if (selectedAmongUsFolderIndex >= 0 && selectedAmongUsFolderIndex < amongUsFolders.Count)
                    selectedAmongUsFolder = amongUsFolders.ElementAt(selectedAmongUsFolderIndex);
            }

            return (amongUsFolders, selectedAmongUsFolder);
        }
    }
}
