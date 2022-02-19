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

namespace TheOtherRolesUpdater.Services
{
    public class PluginInstallService
    {
        private static readonly Lazy<PluginInstallService> instance = new Lazy<PluginInstallService>(() => new PluginInstallService());
        public static PluginInstallService Instance => instance.Value;
        private PluginInstallService() { }

        private readonly string _filename = MagicStrings.THE_OTHER_ROLES_GITHUB_NAME + MagicStrings.THE_OTHER_ROLES_GITHUB_FILE_EXTENSION;

        public async Task InstallPlugin(string gameFolder)
        {
            try
            {
                await DownloadPlugin();
                await CopyPlugin(gameFolder);
            }
            catch { throw; }

        }

        private async Task DownloadPlugin()
        {
            {
                GitHubClient client = new GitHubClient(new ProductHeaderValue(Assembly.GetExecutingAssembly().GetName().Name));
                Release releases = await client.Repository.Release.GetLatest(MagicStrings.THE_OTHER_ROLES_GITHUB_OWNER, MagicStrings.THE_OTHER_ROLES_GITHUB_NAME);
                string downloadUrl = releases.Assets.Where(a => a.Name.Contains(MagicStrings.THE_OTHER_ROLES_GITHUB_FILE_EXTENSION)).First().BrowserDownloadUrl;

                using (HttpClient httpClient = new HttpClient())
                {
                    Stream stream = await httpClient.GetStreamAsync(downloadUrl);
                    using (FileStream fileStream = new FileStream(_filename, System.IO.FileMode.Create))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
            }
        }

        private async Task CopyPlugin(string gameFolder)
        {
            using (ZipFile zipFile = await Task.Run(() => ZipFile.Read(_filename)))
            {
                await Task.Run(() => zipFile.ExtractAll(gameFolder, ExtractExistingFileAction.OverwriteSilently));
            }
        }
    }
}
