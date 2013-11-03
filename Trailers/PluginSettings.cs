using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Profile;

namespace Trailers
{
    public class PluginSettings
    {
        private static Object lockObject = new object();

        #region Constants

        private const string cTrailers = "Trailers";
        private const string cSettingsVersion = "SettingsVersion";
        private const string cWebTimeoutIncrement = "WebTimeoutIncrement";
        private const string cWebTimeout = "WebTimeout";
        private const string cWebMaxRetries = "WebMaxRetries";
        private const string cProviderLocalSearch = "ProviderLocalSearch";
        private const string cProviderTMDbMovies = "ProviderTMDbMovies";
        private const string cProviderOnlineVideoSearch = "ProviderOnlineVideoSearch";
        private const string cOnlineVideosYouTubeSearchString = "OnlineVideosYouTubeSearchString";
        private const string cOnlineVideosYouTubeEnabled = "OnlineVideosYouTubeEnabled";
        private const string cOnlineVideosITunesEnabled = "OnlineVideosITunesEnabled";
        private const string cOnlineVideosIMDbEnabled = "OnlineVideosIMDbEnabled";
        private const string cSearchLocalInSubFolder = "SearchLocalInSubFolder";
        private const string cSearchLocalAdditionalSubFolders = "SearchLocalAdditionalSubFolders";
        private const string cSearchLocalInDedicatedDirectory = "SearchLocalInDedicatedDirectory";
        private const string cSearchLocalDedicatedDirectories = "SearchLocalDedicatedDirectories";
        private const string cSearchLocalDedicatedSubDirectories = "SearchLocalDedicatedSubDirectories";
        private const string cSearchLocalDedicatedDirectorySearchPatterns = "SearchLocalDedicatedDirectorySearchPatterns";
        private const string cSearchLocalInCurrentMediaFolder = "SearchLocalInCurrentMediaFolder";
        private const string cSearchLocalCurrentMediaFolderSearchPatterns = "SearchLocalCurrentvFolderSearchPatterns";
        private const string cSearchLocalAggressiveSearch = "SearchLocalAggressiveSearch";
        private const string cSkipOnlineProvidersIfLocalFound = "SkipOnlineProvidersIfLocalFound";
        private const string cAutoPlayOnSingleLocalOrOnlineTrailer = "AutoPlayOnSingleLocalOrOnlineTrailer";

        #endregion

        #region Persisted Settings

        static int SettingsVersion = 1;

        public static int WebTimeoutIncrement { get; set; }
        public static int WebTimeout { get; set; }
        public static int WebMaxRetries { get; set; }
        public static bool ProviderLocalSearch { get; set; }
        public static bool ProviderTMDbMovies { get; set; }
        public static bool ProviderOnlineVideoSearch { get; set; }
        public static string OnlineVideosYouTubeSearchString { get; set; }
        public static bool OnlineVideosYouTubeEnabled { get; set; }
        public static bool OnlineVideosITunesEnabled { get; set; }
        public static bool OnlineVideosIMDbEnabled { get; set; }
        public static bool SearchLocalInSubFolder { get; set; }
        public static string SearchLocalAdditionalSubFolders { get; set; }
        public static bool SearchLocalInDedicatedDirectory { get; set; }
        public static string SearchLocalDedicatedDirectories { get; set; }
        public static string SearchLocalDedicatedSubDirectories { get; set; }
        public static string SearchLocalDedicatedDirectorySearchPatterns { get; set; }
        public static bool SearchLocalInCurrentMediaFolder { get; set; }
        public static string SearchLocalCurrentMediaFolderSearchPatterns { get; set; }
        public static bool SearchLocalAggressiveSearch { get; set; }
        public static bool SkipOnlineProvidersIfLocalFound { get; set; }
        public static bool AutoPlayOnSingleLocalOrOnlineTrailer { get; set; }
        
        #endregion

        #region Properties

        public static int LogLevel { get; set; }

        /// <summary>
        /// Version of Plugin
        /// </summary>
        public static string Version
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// MediaPortal Version
        /// </summary>
        public static Version MPVersion
        { 
            get
            {
                return Assembly.GetEntryAssembly().GetName().Version;
            }
        }

        /// <summary>
        /// UserAgent used for Web Requests
        /// </summary>
        public static string UserAgent
        {
            get
            {
                return string.Format("TrailersForMediaPortal/{0}", Version);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Loads the Settings
        /// </summary>
        internal static void LoadSettings()
        {
            FileLog.Info("Loading Local Settings");

            using (Settings xmlreader = new MPSettings())
            {
                LogLevel = xmlreader.GetValueAsInt("general", "loglevel", 1);
                WebTimeout = xmlreader.GetValueAsInt(cTrailers, cWebTimeout, 5000);
                WebTimeoutIncrement = xmlreader.GetValueAsInt(cTrailers, cWebTimeoutIncrement, 1000);
                WebMaxRetries = xmlreader.GetValueAsInt(cTrailers, cWebMaxRetries, 5);
                ProviderLocalSearch = xmlreader.GetValueAsBool(cTrailers, cProviderLocalSearch, true);
                ProviderTMDbMovies = xmlreader.GetValueAsBool(cTrailers, cProviderTMDbMovies, true);
                ProviderOnlineVideoSearch = xmlreader.GetValueAsBool(cTrailers, cProviderOnlineVideoSearch, true);
                OnlineVideosYouTubeSearchString = xmlreader.GetValueAsString(cTrailers, cOnlineVideosYouTubeSearchString, "%title% %year% trailer");
                OnlineVideosYouTubeEnabled = xmlreader.GetValueAsBool(cTrailers, cOnlineVideosYouTubeEnabled, true);
                OnlineVideosIMDbEnabled = xmlreader.GetValueAsBool(cTrailers, cOnlineVideosIMDbEnabled, true);
                OnlineVideosITunesEnabled = xmlreader.GetValueAsBool(cTrailers, cOnlineVideosITunesEnabled, true);
                SearchLocalInSubFolder = xmlreader.GetValueAsBool(cTrailers, cSearchLocalInSubFolder, true);
                SearchLocalAdditionalSubFolders = xmlreader.GetValueAsString(cTrailers, cSearchLocalAdditionalSubFolders, "Extras|Shorts|Featurette|Featurettes");
                SearchLocalInDedicatedDirectory = xmlreader.GetValueAsBool(cTrailers, cSearchLocalInDedicatedDirectory, true);
                SearchLocalDedicatedDirectories = xmlreader.GetValueAsString(cTrailers, cSearchLocalDedicatedDirectories, string.Empty);
                SearchLocalDedicatedSubDirectories = xmlreader.GetValueAsString(cTrailers, cSearchLocalDedicatedSubDirectories, "%title% (%year%)|%title%");
                SearchLocalDedicatedDirectorySearchPatterns = xmlreader.GetValueAsString(cTrailers, cSearchLocalDedicatedDirectorySearchPatterns, "%filename%*|*%title%*%year%*|*%title%*");
                SearchLocalInCurrentMediaFolder = xmlreader.GetValueAsBool(cTrailers, cSearchLocalInCurrentMediaFolder, true);
                SearchLocalCurrentMediaFolderSearchPatterns = xmlreader.GetValueAsString(cTrailers, cSearchLocalCurrentMediaFolderSearchPatterns, "trailer*|%filename%*|*%title%*%year%*|*%title%*");
                SearchLocalAggressiveSearch = xmlreader.GetValueAsBool(cTrailers, cSearchLocalAggressiveSearch, false);
                SkipOnlineProvidersIfLocalFound = xmlreader.GetValueAsBool(cTrailers, cSkipOnlineProvidersIfLocalFound, false);
                AutoPlayOnSingleLocalOrOnlineTrailer = xmlreader.GetValueAsBool(cTrailers, cAutoPlayOnSingleLocalOrOnlineTrailer, false);
            }
        }

        /// <summary>
        /// Saves the Settings
        /// </summary>
        internal static void SaveSettings()
        {
            FileLog.Info("Saving Settings");
            using (Settings xmlwriter = new MPSettings())
            {
                xmlwriter.SetValue(cTrailers, cSettingsVersion, SettingsVersion);

                xmlwriter.SetValue(cTrailers, cWebMaxRetries, WebMaxRetries);
                xmlwriter.SetValue(cTrailers, cWebTimeout, WebTimeout);
                xmlwriter.SetValue(cTrailers, cWebTimeoutIncrement, WebTimeoutIncrement);
                xmlwriter.SetValueAsBool(cTrailers, cProviderLocalSearch, ProviderLocalSearch);
                xmlwriter.SetValueAsBool(cTrailers, cProviderTMDbMovies, ProviderTMDbMovies);
                xmlwriter.SetValueAsBool(cTrailers, cProviderOnlineVideoSearch, ProviderOnlineVideoSearch);
                xmlwriter.SetValue(cTrailers, cOnlineVideosYouTubeSearchString, OnlineVideosYouTubeSearchString);
                xmlwriter.SetValueAsBool(cTrailers, cOnlineVideosYouTubeEnabled, OnlineVideosYouTubeEnabled);
                xmlwriter.SetValueAsBool(cTrailers, cOnlineVideosITunesEnabled, OnlineVideosITunesEnabled);
                xmlwriter.SetValueAsBool(cTrailers, cOnlineVideosIMDbEnabled, OnlineVideosIMDbEnabled);
                xmlwriter.SetValueAsBool(cTrailers, cSearchLocalInSubFolder, SearchLocalInSubFolder);
                xmlwriter.SetValue(cTrailers, cSearchLocalAdditionalSubFolders, SearchLocalAdditionalSubFolders);
                xmlwriter.SetValueAsBool(cTrailers, cSearchLocalInDedicatedDirectory, SearchLocalInDedicatedDirectory);
                xmlwriter.SetValue(cTrailers, cSearchLocalDedicatedDirectories, SearchLocalDedicatedDirectories);
                xmlwriter.SetValue(cTrailers, cSearchLocalDedicatedSubDirectories, SearchLocalDedicatedSubDirectories);
                xmlwriter.SetValue(cTrailers, cSearchLocalDedicatedDirectorySearchPatterns, SearchLocalDedicatedDirectorySearchPatterns);
                xmlwriter.SetValueAsBool(cTrailers, cSearchLocalInCurrentMediaFolder, SearchLocalInCurrentMediaFolder);
                xmlwriter.SetValue(cTrailers, cSearchLocalCurrentMediaFolderSearchPatterns, SearchLocalCurrentMediaFolderSearchPatterns);
                xmlwriter.SetValueAsBool(cTrailers, cSearchLocalAggressiveSearch, SearchLocalAggressiveSearch);
                xmlwriter.SetValueAsBool(cTrailers, cSkipOnlineProvidersIfLocalFound, SkipOnlineProvidersIfLocalFound);
                xmlwriter.SetValueAsBool(cTrailers, cAutoPlayOnSingleLocalOrOnlineTrailer, AutoPlayOnSingleLocalOrOnlineTrailer);
            }

            Settings.SaveCache();
        }

        /// <summary>
        /// Perform any maintenance tasks on the settings
        /// </summary>
        internal static void PerformMaintenance()
        {
            using (Settings xmlreader = new MPSettings())
            {
                int currentSettingsVersion = xmlreader.GetValueAsInt(cTrailers, cSettingsVersion, 0);

                // check if any maintenance task is required
                if (currentSettingsVersion >= SettingsVersion) return;

                // upgrade settings for each version
                while (currentSettingsVersion < SettingsVersion)
                {
                    switch (currentSettingsVersion)
                    {
                        case 0:
                            currentSettingsVersion++;
                            break;
                    }
                }
            }
            Settings.SaveCache();
        }

        #endregion
    }
}
