﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Octokit;

namespace TheOtherRolesUpdater.Scanners
{
    public class LatestReleaseVersionScanner
    {
        private static readonly Lazy<LatestReleaseVersionScanner> _instance = new Lazy<LatestReleaseVersionScanner>((() => new LatestReleaseVersionScanner()));
        public static LatestReleaseVersionScanner Instance => _instance.Value;
        private LatestReleaseVersionScanner() { }

        public async Task<string> GetLatestReleaseVersion()
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