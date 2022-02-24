using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Octokit;

namespace TheOtherRolesUpdater.Scanners
{
    public static class LatestReleaseVersionScanner
    {
        public static async Task<string> GetLatestReleaseVersion()
        {
            string latestVersion = MagicStrings.NOT_FOUND;

            try
            {
                GitHubClient client = new GitHubClient(new ProductHeaderValue(Assembly.GetExecutingAssembly().GetName().Name));
                Release releases = await client.Repository.Release.GetLatest(MagicStrings.THE_OTHER_ROLES_GITHUB_OWNER, MagicStrings.THE_OTHER_ROLES_GITHUB_NAME);
                if (releases != null)
                {
                    latestVersion = Regex.Replace(releases.TagName, "[^0-9.]", string.Empty);
                }
                return latestVersion;
            }
            catch
            {
                return latestVersion;
            }
        }
    }
}
