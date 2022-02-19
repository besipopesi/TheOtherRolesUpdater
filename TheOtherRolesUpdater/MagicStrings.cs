using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOtherRolesUpdater
{
    public static class MagicStrings
    {
        public const string NOT_FOUND = "not found";
        public const string NOTHING_TO_INSTALL = "nothing to install";
        public const string INSTALL_STRING_BEGIN = "Install";
        public const string INSTALL_STRING_END = "...";
        public const string GAME_FOLDERS_FOUND = "folders found.";
        public const string SETTINGS_FILE = "SavedFolders.json";
        public const string STEAM_REGISTRY_KEY = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam";
        public const string STEAM_REGISTRY_VALUE = "InstallPath";
        public const string AMONG_US_DEFAULT_FOLDER = @"steamapps\common\Among Us";
        public const string AMONG_US_EXE_FILENAME = "Among Us.exe";
        public const string THE_OTHER_ROLES_PLUGIN_FOLDER = @"BepInEx\plugins";
        public const string THE_OTHER_ROLES_PLUGIN_FILENAME = "TheOtherRoles.dll";
        public const string THE_OTHER_ROLES_GITHUB_OWNER = "Eisbison";
        public const string THE_OTHER_ROLES_GITHUB_NAME = "TheOtherRoles";
        public const string THE_OTHER_ROLES_GITHUB_FILE_EXTENSION = ".zip";        
                
        public const string EXCEPTION_DOWNLOAD_FAILED_MESSAGE = "Unable to download latest release of The Other Roles.\nPlease try again later.";
        public const string EXCEPTION_DOWNLOAD_FAILED_CAPTION = "Download failed";
    }
}
