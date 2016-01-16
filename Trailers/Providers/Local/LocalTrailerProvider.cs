using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Trailers.Extensions;
using Trailers.GUI;
using Trailers.Localisation;

namespace Trailers.Providers
{
    class LocalTrailerProvider : IProvider
    {
        #region Private Variables

        private List<string> TrailerFiles = new List<string>();

        #endregion

        #region Constructor

        public LocalTrailerProvider(bool enableProvider)
        {
            Enabled = enableProvider;
        }

        #endregion

        #region IProvider Members

        public bool Enabled { get; set; }

        public string Name
        {
            get { return "Local Trailer Search Provider"; }
        }

        public bool IsLocal
        {
            get { return true; }
        }

        public List<GUITrailerListItem> Search(MediaItem searchItem)
        {
            var listItems = new List<GUITrailerListItem>();

            // Clear trailer file list, we may be 'aggressive' with our search
            // so this can avoid duplicates in a crude way
            TrailerFiles.Clear();

            // Check local auto-download directory first
            if (AutoDownloadEnabled())
            {
                listItems.AddRange(GetTrailersFromAutoDownloadDirectory(searchItem));
            }

            // Search for Trailer(s) in Sub-Folder of current media
            if (!string.IsNullOrEmpty(searchItem.Directory))
            {
                if (listItems.Count == 0 || PluginSettings.SearchLocalAggressiveSearch)
                {
                    listItems.AddRange(GetTrailersFromCurrentMediaSubFolder(searchItem));
                }

                // Search for Trailer(s) in the current media directory
                if (listItems.Count == 0 || PluginSettings.SearchLocalAggressiveSearch)
                {
                    listItems.AddRange(GetTrailersFromLocalMediaFolder(searchItem));
                }
            }

            // Search for Trailer(s) in dedicated directories
            if (listItems.Count == 0 || PluginSettings.SearchLocalAggressiveSearch)
            {
                listItems.AddRange(GetTrailersFromDedicatedFolder(searchItem));
            }

            return listItems;
        }

        #endregion

        #region Private Methods

        private bool AutoDownloadEnabled()
        {
            return  PluginSettings.AutoDownloadTrailersMovingPictures ||
                    PluginSettings.AutoDownloadTrailersMyVideos ||
                    PluginSettings.AutoDownloadTrailersMyFilms;
        }

        private string ReplaceVars(MediaItem searchItem, string searchPattern)
        {
            searchPattern = searchPattern.Replace("%title%", searchItem.Title.ToCleanFileName());
            searchPattern = searchPattern.Replace("%year%", searchItem.Year.ToString());
            searchPattern = searchPattern.Replace("%imdb%", searchItem.IMDb ?? string.Empty);
            searchPattern = searchPattern.Replace("%filename%", searchItem.FilenameWOExtension ?? "null");
            searchPattern = searchPattern.Replace("%episode%", searchItem.Episode.HasValue ? searchItem.Episode.ToString() : string.Empty);
            searchPattern = searchPattern.Replace("%season%", searchItem.Season.HasValue ? searchItem.Season.ToString() : string.Empty);
            searchPattern = searchPattern.Replace("%episodename%", searchItem.EpisodeName ?? string.Empty);
            searchPattern = searchPattern.Replace("%airdate%", searchItem.AirDate ?? string.Empty);

            if (string.IsNullOrEmpty(searchPattern)) 
                searchPattern = "null";

            return searchPattern;
        }

        private List<GUITrailerListItem> SearchForLocalTrailers(MediaItem searchItem, string directory, string searchPattern)
        {
            var listItems = new List<GUITrailerListItem>();

            FileLog.Debug("Searching for local trailers in directory: '{0}', with search pattern: '{1}'", directory, searchPattern);

            // check if string replacements produced a bad search pattern
            if (searchPattern.Contains("**") || searchPattern.Contains("null"))
                return listItems;

            try
            {
                foreach (var file in Directory.GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly))
                {
                    // check it's a video file as defined by MediaPortal and User in MP Configuration
                    if (!MediaPortal.Util.Utils.IsVideo(file)) continue;

                    // check it's not a sample
                    if (Path.GetFileName(file).ToLowerInvariant().StartsWith("sample")) continue;

                    // check it's not the same file as the source
                    if (file.Equals(searchItem.FullPath, StringComparison.InvariantCultureIgnoreCase)) continue;

                    // check we have not already got this trailer matched
                    if (TrailerFiles.Contains(file)) continue;

                    FileLog.Debug("Found local trailer that matched search criteria: '{0}'", file);

                    var listItem = new GUITrailerListItem();

                    listItem.Label = Path.GetFileNameWithoutExtension(file);
                    listItem.Label2 = Translation.Local;
                    listItem.URL = file;
                    listItem.CurrentMedia = searchItem;

                    listItems.Add(listItem);

                    // store trailer file in case we are in 'aggressive' mode
                    TrailerFiles.Add(file);
                }
            }
            catch (Exception e)
            {
                FileLog.Error("Failed to get files from '{0}' with error: {1}", directory, e.Message);
            }

            return listItems;
        }

        private List<GUITrailerListItem> GetTrailersFromLocalMediaFolder(MediaItem searchItem)
        {
            var listItems = new List<GUITrailerListItem>();

            if (!PluginSettings.SearchLocalInCurrentMediaFolder) return listItems;
            FileLog.Debug("Searching for trailers from local media folder...");

            if (string.IsNullOrEmpty(searchItem.Directory))
            {
                FileLog.Info("No associated directory for search item, cancelling search from current media folder");
                return listItems;
            }

            foreach (var pattern in PluginSettings.SearchLocalCurrentMediaFolderSearchPatterns.Split('|').ToList())
            {
                if (listItems.Count > 0 && !PluginSettings.SearchLocalAggressiveSearch) continue;
                listItems.AddRange(SearchForLocalTrailers(searchItem, searchItem.Directory, ReplaceVars(searchItem, pattern)));
            }

            FileLog.Info("Found {0} trailer(s) from local media folder.", listItems.Count.ToString());

            return listItems;
        }

        private List<GUITrailerListItem> GetTrailersFromDedicatedFolder(MediaItem searchItem)
        {
            var listItems = new List<GUITrailerListItem>();

            if (!PluginSettings.SearchLocalInDedicatedDirectory) return listItems;

            FileLog.Debug("Searching for trailers from dedicated trailer directory...");

            foreach (var directory in PluginSettings.SearchLocalDedicatedDirectories.Split('|').ToList())
            {
                FileLog.Debug("Searching for local trailers in directory: '{0}'", directory);

                // if we have found trailers we don't need to continue looking
                if (!Directory.Exists(directory) || (listItems.Count > 0 && !PluginSettings.SearchLocalAggressiveSearch)) continue;

                // first check the base directory, this case is for when 
                // all movie trailers are together i.e. filenames will be unique
                foreach (var pattern in PluginSettings.SearchLocalDedicatedDirectorySearchPatterns.Split('|').ToList())
                {
                    if (listItems.Count > 0 && !PluginSettings.SearchLocalAggressiveSearch) continue;
                    listItems.AddRange(SearchForLocalTrailers(searchItem, directory, ReplaceVars(searchItem, pattern)));
                }

                // if we have not found any trailers by now, check sub-directories
                // since the sub-directories are unique we can search all files '*'.
                foreach (var subDirectory in PluginSettings.SearchLocalDedicatedSubDirectories.Split('|').ToList())
                {
                    if (listItems.Count > 0 && !PluginSettings.SearchLocalAggressiveSearch) continue;

                    string subdir = string.Format(@"{0}\{1}", directory, ReplaceVars(searchItem, subDirectory));
                    FileLog.Debug("Searching for local trailers in sub-directory: '{0}'", subdir);

                    if (!Directory.Exists(subdir)) continue;
                    listItems.AddRange(SearchForLocalTrailers(searchItem, subdir, "*"));
                }
            }

            FileLog.Info("Found {0} trailer(s) from dedicated trailer directories.", listItems.Count.ToString());

            return listItems;
        }

        private List<GUITrailerListItem> GetTrailersFromCurrentMediaSubFolder(MediaItem searchItem)
        {
            var listItems = new List<GUITrailerListItem>();

            if (!PluginSettings.SearchLocalInSubFolder) return listItems;

            FileLog.Debug("Searching for trailers from local media sub-folder(s)...");

            if (string.IsNullOrEmpty(searchItem.Directory))
            {
                FileLog.Info("No associated directory for search item, cancelling search from current media sub-folder");
                return listItems;
            }

            // Get list of sub-folder names to search in from the base path of the media's filename
            // First get list of common names from the Trailer and Trailers translation string
            var subFolders = new HashSet<string>{ Translation.Trailers, Translation.Trailer, "Trailers", "Trailer" };

            // Add any additional folder names defined by the user, multiple names seperated by pipe char: '|'
            foreach (var folder in PluginSettings.SearchLocalAdditionalSubFolders.Split('|').ToList())
            {
                // ignore invalid folder names
                if (!string.IsNullOrEmpty(folder) && folder.IndexOfAny(Path.GetInvalidPathChars()) == -1)
                {
                    subFolders.Add(folder);
                }
            }

            FileLog.Debug("Searching in the following sub-folders: {0}", string.Join(", ", subFolders.Select(a => a.ToString()).ToArray()));

            // Search for any files with-in defined sub-folders
            foreach (var subfolder in subFolders)
            {
                string directory = Path.Combine(searchItem.Directory, subfolder);
                FileLog.Debug("Searching for local trailers in directory: '{0}'", directory);

                if (!Directory.Exists(directory)) continue;

                listItems.AddRange(SearchForLocalTrailers(searchItem, directory, "*"));
            }

            FileLog.Info("Found {0} trailer(s) from local media sub-folders.", listItems.Count.ToString());

            return listItems;
        }

        private List<GUITrailerListItem> GetTrailersFromAutoDownloadDirectory(MediaItem searchItem)
        {
            var listItems = new List<GUITrailerListItem>();

            FileLog.Debug("Searching for trailers from local auto-download directory...");

            // trailers in auto-download directory are grouped by movie folder in the form 'title% (%year%) [%imdbid%]'
            // first check with imdbid
            string folder = string.Format("{0} ({1}) [{2}]", searchItem.Title, searchItem.Year, searchItem.IMDb);
            string directory = Path.Combine(PluginSettings.AutoDownloadDirectory, folder.ToCleanFileName());
            if (Directory.Exists(directory))
            {
                FileLog.Debug("Searching for local trailers in directory: '{0}'", directory);
                listItems.AddRange(SearchForLocalTrailers(searchItem, directory, "*"));
            }
            else
            {
                // check with empty imdb id.
                folder = string.Format("{0} ({1}) []", searchItem.Title, searchItem.Year);
                directory = Path.Combine(PluginSettings.AutoDownloadDirectory, folder.ToCleanFileName());

                if (Directory.Exists(directory))
                {
                    FileLog.Debug("Searching for local trailers in directory: '{0}'", directory);
                    listItems.AddRange(SearchForLocalTrailers(searchItem, directory, "*"));
                }
            }

            FileLog.Info("Found {0} trailer(s) from local auto-download directory.", listItems.Count.ToString());

            return listItems;
        }

        #endregion
    }
}
