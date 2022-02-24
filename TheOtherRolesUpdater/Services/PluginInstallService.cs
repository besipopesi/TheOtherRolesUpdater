using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Octokit;
using Ionic.Zip;
using System.Security.AccessControl;
using System.Security.Principal;

namespace TheOtherRolesUpdater.Services
{
    public static class PluginInstallService
    {
        //private static readonly string _filename = MagicStrings.THE_OTHER_ROLES_GITHUB_NAME + MagicStrings.THE_OTHER_ROLES_GITHUB_FILE_EXTENSION;
        private static string tempFilename;

        public static async Task InstallPlugin(string gameFolder)
        {
            tempFilename = Path.GetTempFileName();

            try
            {
                await DownloadPlugin();
                await CopyPlugin(gameFolder);
            }
            catch { throw; }

        }

        private static async Task DownloadPlugin()
        {
            {
                GitHubClient client = new GitHubClient(new ProductHeaderValue(Assembly.GetExecutingAssembly().GetName().Name));
                Release releases = await client.Repository.Release.GetLatest(MagicStrings.THE_OTHER_ROLES_GITHUB_OWNER, MagicStrings.THE_OTHER_ROLES_GITHUB_NAME);
                string downloadUrl = releases.Assets.Where(a => a.Name.Contains(MagicStrings.THE_OTHER_ROLES_GITHUB_FILE_EXTENSION)).First().BrowserDownloadUrl;

                using (HttpClient httpClient = new HttpClient())
                {
                    Stream stream = await httpClient.GetStreamAsync(downloadUrl);
                    using (FileStream fileStream = new FileStream(tempFilename, System.IO.FileMode.Create))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
            }
        }

        private static async Task CopyPlugin(string gameFolder)
        {
            using (ZipFile zipFile = await Task.Run(() => ZipFile.Read(tempFilename)))
            {
                await Task.Run(() => zipFile.ExtractAll(gameFolder, ExtractExistingFileAction.OverwriteSilently));
            }
        }
    }
}
