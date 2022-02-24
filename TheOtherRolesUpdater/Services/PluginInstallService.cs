using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

using Ionic.Zip;

using Octokit;

namespace TheOtherRolesUpdater.Services
{
    public static class PluginInstallService
    {
        private static string _tempFilename;

        public static async Task InstallPlugin(string gameFolder)
        {
            _tempFilename = Path.GetTempFileName();

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
                    using (FileStream fileStream = new FileStream(_tempFilename, System.IO.FileMode.OpenOrCreate))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
            }
        }

        private static async Task CopyPlugin(string gameFolder)
        {
            using (ZipFile zipFile = await Task.Run(() => ZipFile.Read(_tempFilename)))
            {
                await Task.Run(() => zipFile.ExtractAll(gameFolder, ExtractExistingFileAction.OverwriteSilently));
            }

            if (File.Exists(_tempFilename))
                File.Delete(_tempFilename);
        }
    }
}
