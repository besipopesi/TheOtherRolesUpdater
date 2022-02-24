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
        public const string SETTINGS_FILE = "SavedFolders.json";

        public const string STEAM_REGISTRY_KEY = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam";
        public const string STEAM_REGISTRY_VALUE = "InstallPath";
        public const string STEAM_AMONG_US_FOLDER = @"steamapps\common\Among Us";

        public const string EPIC_GAMES_REGISTRY_KEY = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Epic Games\EpicGamesLauncher";
        public const string EPIC_GAMES_REGISTRY_VALUE = "AppDataPath";
        public const string EPIC_GAMES_MANIFESTS_FOLDER = "Manifests";
        public const string EPIC_GAMES_MANIFESTS_FILE_EXTENSION = ".item";
        public const string EPIC_GAMES_MANIFEST_ENTRY_LAUNCH_EXECUTABLE = "LaunchExecutable";
        public const string EPIC_GAMES_MANIFEST_ENTRY_INSTALL_LOCATION = "InstallLocation";

        public const string AMONG_US_EXE_FILENAME = "Among Us.exe";

        public const string THE_OTHER_ROLES_PLUGIN_FOLDER = @"BepInEx\plugins";
        public const string THE_OTHER_ROLES_PLUGIN_FILENAME = "TheOtherRoles.dll";

        public const string THE_OTHER_ROLES_GITHUB_OWNER = "Eisbison";
        public const string THE_OTHER_ROLES_GITHUB_NAME = "TheOtherRoles";
        public const string THE_OTHER_ROLES_GITHUB_FILE_EXTENSION = ".zip";

        public const string EXCEPTION_DOWNLOAD_FAILED_MESSAGE = "Unable to download latest release of The Other Roles.\nPlease try again later.";
        public const string EXCEPTION_DOWNLOAD_FAILED_CAPTION = "Download failed!";
        public const string EXCEPTION_NO_ADMIN_RIGHTS_FOR_EPIC_GAMES_MESSAGE = "You need to run this application as administrator to update an Epic Games installation.";
        public const string EXCEPTION_NO_ADMIN_RIGHTS_FOR_EPIC_GAMES_CAPTION = "Can't write to folder!";
    }
}
