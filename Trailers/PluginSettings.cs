using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using System.Runtime.Serialization;
using System.Web;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Profile;
using Trailers.Extensions;

namespace Trailers
{
    public class PluginSettings
    {
        private static Object lockObject = new object();

        #region Constants

        // MPEI Installer ID
        public const string cGuid = "b4293f64-9e83-4f1f-b2e3-8bdea2a37425";

        private static string cTVDbExternalIDs = Path.Combine(Config.GetFolder(Config.Dir.Config), @"Trailers\tvdb_ids.json");
        private static string cIMDbExternalIDs = Path.Combine(Config.GetFolder(Config.Dir.Config), @"Trailers\imdb_ids.json");
        private static string cTVRageExternalIDs = Path.Combine(Config.GetFolder(Config.Dir.Config), @"Trailers\tvrage_ids.json");

        private const string cTrailers = "Trailers";
        private const string cOnlineVideos = "onlinevideos";
        private const string cShowTimes = "ShowTimes Grabber";
        private const string cOnlineVideosDownloadDir = "downloadDir";
        private const string cShowTimesConfigFile = "grabberCfgXml";
        private const string cSettingsVersion = "SettingsVersion";
        private const string cWebTimeoutIncrement = "WebTimeoutIncrement";
        private const string cWebTimeout = "WebTimeout";
        private const string cWebMaxRetries = "WebMaxRetries";
        private const string cProviderLocalSearch = "ProviderLocalSearch";
        private const string cProviderTMDb = "ProviderTMDbMovies";
        private const string cProviderOnlineVideoSearch = "ProviderOnlineVideoSearch";
        private const string cOnlineVideosYouTubeMovieSearchString = "OnlineVideosYouTubeSearchString";
        private const string cOnlineVideosYouTubeShowSearchString = "OnlineVideosYouTubeShowSearchString";
        private const string cOnlineVideosYouTubeSeasonSearchString = "OnlineVideosYouTubeSeasonSearchString";
        private const string cOnlineVideosYouTubeEpisodeSearchString = "OnlineVideosYouTubeEpisodeSearchString";
        private const string cOnlineVideosYouTubeEpisodeSpecialSearchString = "OnlineVideosYouTubeEpisodeSpecialSearchString";
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
        private const string cSearchLocalCurrentMediaFolderSearchPatterns = "SearchLocalCurrentFolderSearchPatterns";
        private const string cSearchLocalAggressiveSearch = "SearchLocalAggressiveSearch";
        private const string cSkipOnlineProvidersIfLocalFound = "SkipOnlineProvidersIfLocalFound";
        private const string cAutoPlayOnSingleLocalOrOnlineTrailer = "AutoPlayOnSingleLocalOrOnlineTrailer";
        private const string cAutoDownloadTrailersMovingPictures = "AutoDownloadTrailersMovingPictures";
        private const string cAutoDownloadTrailersMyFilms = "AutoDownloadTrailersMyFilms";
        private const string cAutoDownloadTrailersMyVideos = "AutoDownloadTrailersMyVideos";
        private const string cAutoDownloadStartDelay = "AutoDownloadStartDelay";
        private const string cAutoDownloadInterval = "AutoDownloadInterval";
        private const string cAutoDownloadUpdateInterval = "AutoDownloadUpdateInterval";
        private const string cAutoDownloadDirectory = "AutoDownloadDirectory";
        private const string cAutoDownloadQuality = "AutoDownloadQuality";
        private const string cAutoDownloadTrailers = "AutoDownloadTrailers";
        private const string cAutoDownloadTeasers = "AutoDownloadTeasers";
        private const string cAutoDownloadFeaturettes = "AutoDownloadFeaturettes";
        private const string cAutoDownloadClips = "AutoDownloadClips";
        private const string cAutoDownloadCleanup = "AutoDownloadCleanup";

        #endregion

        #region Persisted Settings

        static int SettingsVersion = 1;

        public static List<ExternalID> TVDbIds { get; set; }
        public static List<ExternalID> IMDbIds { get; set; }
        public static List<ExternalID> TVRageIds { get; set; }

        public static int WebTimeoutIncrement { get; set; }
        public static int WebTimeout { get; set; }
        public static int WebMaxRetries { get; set; }
        public static bool ProviderLocalSearch { get; set; }
        public static bool ProviderTMDb { get; set; }
        public static bool ProviderOnlineVideoSearch { get; set; }
        public static string OnlineVideosYouTubeMovieSearchString { get; set; }
        public static string OnlineVideosYouTubeShowSearchString { get; set; }
        public static string OnlineVideosYouTubeSeasonSearchString { get; set; }
        public static string OnlineVideosYouTubeEpisodeSearchString { get; set; }
        public static string OnlineVideosYouTubeEpisodeSpecialSearchString { get; set; }
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
        public static bool AutoDownloadTrailersMovingPictures { get; set; }
        public static bool AutoDownloadTrailersMyFilms { get; set; }
        public static bool AutoDownloadTrailersMyVideos { get; set; }
        public static int AutoDownloadStartDelay { get; set; }
        public static int AutoDownloadInterval { get; set; }
        public static int AutoDownloadUpdateInterval { get; set; }
        public static string AutoDownloadDirectory { get; set; }
        public static string AutoDownloadQuality { get; set; }
        public static bool AutoDownloadTrailers { get; set; }
        public static bool AutoDownloadTeasers { get; set; }
        public static bool AutoDownloadFeaturettes { get; set; }
        public static bool AutoDownloadClips { get; set; }
        public static bool AutoDownloadCleanup { get; set; }

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
        internal static void LoadSettings(bool reload = false)
        {
            FileLog.Info("Loading Local Settings");

            using (Settings xmlreader = new MPSettings())
            {
                LogLevel = xmlreader.GetValueAsInt("general", "loglevel", 1);
                WebTimeout = xmlreader.GetValueAsInt(cTrailers, cWebTimeout, 5000);
                WebTimeoutIncrement = xmlreader.GetValueAsInt(cTrailers, cWebTimeoutIncrement, 1000);
                WebMaxRetries = xmlreader.GetValueAsInt(cTrailers, cWebMaxRetries, 5);
                ProviderLocalSearch = xmlreader.GetValueAsBool(cTrailers, cProviderLocalSearch, true);
                ProviderTMDb = xmlreader.GetValueAsBool(cTrailers, cProviderTMDb, true);
                ProviderOnlineVideoSearch = xmlreader.GetValueAsBool(cTrailers, cProviderOnlineVideoSearch, true);
                OnlineVideosYouTubeMovieSearchString = xmlreader.GetValueAsString(cTrailers, cOnlineVideosYouTubeMovieSearchString, "%title% %year% trailer");
                OnlineVideosYouTubeShowSearchString = xmlreader.GetValueAsString(cTrailers, cOnlineVideosYouTubeShowSearchString, "%title% trailer");
                OnlineVideosYouTubeSeasonSearchString = xmlreader.GetValueAsString(cTrailers, cOnlineVideosYouTubeSeasonSearchString, "%title% season %season%");
                OnlineVideosYouTubeEpisodeSearchString = xmlreader.GetValueAsString(cTrailers, cOnlineVideosYouTubeEpisodeSearchString, "%title% season %season% episode %episode%");
                OnlineVideosYouTubeEpisodeSpecialSearchString = xmlreader.GetValueAsString(cTrailers, cOnlineVideosYouTubeEpisodeSpecialSearchString, "%title% %episodename%");
                OnlineVideosYouTubeEnabled = xmlreader.GetValueAsBool(cTrailers, cOnlineVideosYouTubeEnabled, true);
                OnlineVideosIMDbEnabled = xmlreader.GetValueAsBool(cTrailers, cOnlineVideosIMDbEnabled, true);
                OnlineVideosITunesEnabled = xmlreader.GetValueAsBool(cTrailers, cOnlineVideosITunesEnabled, true);
                SearchLocalInSubFolder = xmlreader.GetValueAsBool(cTrailers, cSearchLocalInSubFolder, true);
                SearchLocalAdditionalSubFolders = xmlreader.GetValueAsString(cTrailers, cSearchLocalAdditionalSubFolders, "Teaser|Extras|Shorts|Featurette|Featurettes");
                SearchLocalInDedicatedDirectory = xmlreader.GetValueAsBool(cTrailers, cSearchLocalInDedicatedDirectory, true);
                SearchLocalDedicatedDirectories = xmlreader.GetValueAsString(cTrailers, cSearchLocalDedicatedDirectories, GetExternalDownloadDirs());
                SearchLocalDedicatedSubDirectories = xmlreader.GetValueAsString(cTrailers, cSearchLocalDedicatedSubDirectories, "%title% (%year%)|%title%");
                SearchLocalDedicatedDirectorySearchPatterns = xmlreader.GetValueAsString(cTrailers, cSearchLocalDedicatedDirectorySearchPatterns, "%filename%*|*%title%*%year%*|*%title%*");
                SearchLocalInCurrentMediaFolder = xmlreader.GetValueAsBool(cTrailers, cSearchLocalInCurrentMediaFolder, true);
                SearchLocalCurrentMediaFolderSearchPatterns = xmlreader.GetValueAsString(cTrailers, cSearchLocalCurrentMediaFolderSearchPatterns, "trailer*|%filename%*|*%title%*%year%*|*%title%*");
                SearchLocalAggressiveSearch = xmlreader.GetValueAsBool(cTrailers, cSearchLocalAggressiveSearch, false);
                SkipOnlineProvidersIfLocalFound = xmlreader.GetValueAsBool(cTrailers, cSkipOnlineProvidersIfLocalFound, false);
                AutoPlayOnSingleLocalOrOnlineTrailer = xmlreader.GetValueAsBool(cTrailers, cAutoPlayOnSingleLocalOrOnlineTrailer, false);
                AutoDownloadTrailersMovingPictures = xmlreader.GetValueAsBool(cTrailers, cAutoDownloadTrailersMovingPictures, false);
                AutoDownloadTrailersMyFilms = xmlreader.GetValueAsBool(cTrailers, cAutoDownloadTrailersMyFilms, false);
                AutoDownloadTrailersMyVideos = xmlreader.GetValueAsBool(cTrailers, cAutoDownloadTrailersMyVideos, false);
                AutoDownloadStartDelay = xmlreader.GetValueAsInt(cTrailers, cAutoDownloadStartDelay, 30000);
                AutoDownloadInterval = xmlreader.GetValueAsInt(cTrailers, cAutoDownloadInterval, 86400000);
                AutoDownloadUpdateInterval = xmlreader.GetValueAsInt(cTrailers, cAutoDownloadUpdateInterval, 7);
                AutoDownloadDirectory = xmlreader.GetValueAsString(cTrailers, cAutoDownloadDirectory, Config.GetSubFolder(Config.Dir.Config, "Trailers"));
                AutoDownloadQuality = xmlreader.GetValueAsString(cTrailers, cAutoDownloadQuality, "HD");
                AutoDownloadTrailers = xmlreader.GetValueAsBool(cTrailers, cAutoDownloadTrailers, true);
                AutoDownloadTeasers = xmlreader.GetValueAsBool(cTrailers, cAutoDownloadTeasers, true);
                AutoDownloadFeaturettes = xmlreader.GetValueAsBool(cTrailers, cAutoDownloadFeaturettes, true);
                AutoDownloadClips = xmlreader.GetValueAsBool(cTrailers, cAutoDownloadClips, true);
                AutoDownloadCleanup = xmlreader.GetValueAsBool(cTrailers, cAutoDownloadCleanup, false);
            }

            if (!reload)
            {
                FileLog.Info("Loading Persisted File Cache");
                TVDbIds = LoadFileCache(cTVDbExternalIDs, "{}").FromJsonArray<ExternalID>().ToList();
                IMDbIds = LoadFileCache(cIMDbExternalIDs, "{}").FromJsonArray<ExternalID>().ToList();
                TVRageIds = LoadFileCache(cTVRageExternalIDs, "{}").FromJsonArray<ExternalID>().ToList();

                if (TVDbIds == null) TVDbIds = new List<ExternalID>();
                if (IMDbIds == null) TVDbIds = new List<ExternalID>();
                if (TVRageIds == null) TVDbIds = new List<ExternalID>();
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
                xmlwriter.SetValueAsBool(cTrailers, cProviderTMDb, ProviderTMDb);
                xmlwriter.SetValueAsBool(cTrailers, cProviderOnlineVideoSearch, ProviderOnlineVideoSearch);
                xmlwriter.SetValue(cTrailers, cOnlineVideosYouTubeMovieSearchString, OnlineVideosYouTubeMovieSearchString);
                xmlwriter.SetValue(cTrailers, cOnlineVideosYouTubeShowSearchString, OnlineVideosYouTubeShowSearchString);
                xmlwriter.SetValue(cTrailers, cOnlineVideosYouTubeSeasonSearchString, OnlineVideosYouTubeSeasonSearchString);
                xmlwriter.SetValue(cTrailers, cOnlineVideosYouTubeEpisodeSearchString, OnlineVideosYouTubeEpisodeSearchString);
                xmlwriter.SetValue(cTrailers, cOnlineVideosYouTubeEpisodeSpecialSearchString, OnlineVideosYouTubeEpisodeSpecialSearchString);
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
                xmlwriter.SetValueAsBool(cTrailers, cAutoDownloadTrailersMovingPictures, AutoDownloadTrailersMovingPictures);
                xmlwriter.SetValueAsBool(cTrailers, cAutoDownloadTrailersMyFilms, AutoDownloadTrailersMyFilms);
                xmlwriter.SetValueAsBool(cTrailers, cAutoDownloadTrailersMyVideos, AutoDownloadTrailersMyVideos);
                xmlwriter.SetValue(cTrailers, cAutoDownloadStartDelay, AutoDownloadStartDelay);
                xmlwriter.SetValue(cTrailers, cAutoDownloadInterval, AutoDownloadInterval);
                xmlwriter.SetValue(cTrailers, cAutoDownloadUpdateInterval, AutoDownloadUpdateInterval);
                xmlwriter.SetValue(cTrailers, cAutoDownloadDirectory, AutoDownloadDirectory);
                xmlwriter.SetValue(cTrailers, cAutoDownloadQuality, AutoDownloadQuality);
                xmlwriter.SetValueAsBool(cTrailers, cAutoDownloadTrailers, AutoDownloadTrailers);
                xmlwriter.SetValueAsBool(cTrailers, cAutoDownloadTeasers, AutoDownloadTeasers);
                xmlwriter.SetValueAsBool(cTrailers, cAutoDownloadFeaturettes, AutoDownloadFeaturettes);
                xmlwriter.SetValueAsBool(cTrailers, cAutoDownloadClips, AutoDownloadClips);
                xmlwriter.SetValueAsBool(cTrailers, cAutoDownloadCleanup, AutoDownloadCleanup);
            }

            Settings.SaveCache();

            FileLog.Info("Saving Persistent File Cache");
            SaveFileCache(cTVDbExternalIDs, (TVDbIds ?? "{}".FromJsonArray<ExternalID>()).ToList().ToJson());
            SaveFileCache(cIMDbExternalIDs, (IMDbIds ?? "{}".FromJsonArray<ExternalID>()).ToList().ToJson());
            SaveFileCache(cTVRageExternalIDs, (TVRageIds ?? "{}".FromJsonArray<ExternalID>()).ToList().ToJson());
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

        static string GetExternalDownloadDirs()
        {
            string trailerDirs = string.Empty;
            
            trailerDirs =  GetOnlineVideosDownloadDirs();
            trailerDirs += string.IsNullOrEmpty(trailerDirs) ? string.Empty : "|";
            trailerDirs += GetShowTimesTrailerDir();

            return trailerDirs;
        }

        /// <summary>
        /// Get ShowTimes Trailer download directory
        /// </summary>
        /// <returns></returns>
        static string GetShowTimesTrailerDir()
        {
            string showTimesConfigFile = string.Empty;
            string trailerDir = string.Empty;

            FileLog.Debug("Getting ShowTimes Config location");

            using (Settings xmlreader = new MPSettings())
            {
                showTimesConfigFile = xmlreader.GetValueAsString(cShowTimes, cShowTimesConfigFile, string.Empty);
            }

            if (!string.IsNullOrEmpty(showTimesConfigFile))
            {
                FileLog.Debug("Getting ShowTimes Trailer folder");

                try
                {
                    if (File.Exists(showTimesConfigFile))
                    {
                        var xmlDoc = new XmlDocument();
                        xmlDoc.Load(showTimesConfigFile);

                        var node = xmlDoc.SelectSingleNode("/ConfigurationSettings/TrailerFolder");
                        if (node != null) trailerDir = node.InnerText;
                    }
                }
                catch(Exception e)
                {
                    FileLog.Error("Unable to get trailer directory for ShowTimes plugin!: {0}", e.Message);
                }
            }

            return trailerDir;
        }

        /// <summary>
        /// Get OnlineVideos download directories for possible source of Trailers
        /// </summary>
        static string GetOnlineVideosDownloadDirs()
        {
            string trailerDirs = string.Empty;
            string downloadDir = string.Empty;

            FileLog.Debug("Getting DownloadDir from OnlineVideos");
            using (Settings xmlreader = new MPSettings())
            {
                downloadDir = xmlreader.GetValueAsString(cOnlineVideos, cOnlineVideosDownloadDir, string.Empty);
            }

            if (!string.IsNullOrEmpty(downloadDir))
            {
                // add iTunes, IMDb and YouTube as possible trailer directories
                trailerDirs =  string.Format("{0}",  Path.Combine(downloadDir, "YouTube"));
                trailerDirs += string.Format("|{0}", Path.Combine(downloadDir, "iTunes Movie Trailers"));
                trailerDirs += string.Format("|{0}", Path.Combine(downloadDir, "IMDb Movie Trailers"));
            }

            return trailerDirs;
        }

        static void SaveFileCache(string file, string value)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file));
                File.WriteAllText(file, value, Encoding.UTF8);
            }
            catch (Exception e)
            {
                FileLog.Error(string.Format("Error saving file: {0}, Error: {1}", file, e.Message));
            }
        }

        static string LoadFileCache(string file, string defaultValue)
        {
            string returnValue = defaultValue;

            try
            {
                if (File.Exists(file))
                {
                    returnValue = File.ReadAllText(file, Encoding.UTF8);
                }
            }
            catch (Exception e)
            {
                FileLog.Error(string.Format("Error loading file: {0}, Error: {1}", file, e.Message));
                return defaultValue;
            }

            return returnValue;
        }

        #endregion

        [DataContract]
        public class ExternalID
        {
            [DataMember(Name = "external_id")]
            public string ExternalId { get; set; }

            [DataMember(Name = "tmdb_id")]
            public int TmdbId { get; set; }
        }

    }

    internal class ExtensionSettings
    {
        #region Init
        public void Init()
        {
            Thread hookThread = new Thread(delegate()
            {
                try
                {
                    FileLog.Info("Adding hooks to MPEI Settings");
                    AddHooksIntoMPEISettings();
                }
                catch
                {
                    FileLog.Warning("Unable to add hooks into MPEI Settings, Extensions plugin not installed or out of date. Install the Extensions plugin to have support to change settings in the GUI and/or auto-update plugin.");
                }
            })
            {
                Name = "Settings",
                IsBackground = true
            };

            hookThread.Start();
        }
        #endregion

        #region Hooks
        private void AddHooksIntoMPEISettings()
        {
            // sleep until we know that there has been enough time
            // for window manager to have loaded extension settings window
            // todo: find a better way...
            Thread.Sleep(10000);

            // get a reference to the extension settings window
            MPEIPlugin.GUISettings extensionSettings = (MPEIPlugin.GUISettings)GUIWindowManager.GetWindow((int)GUI.ExternalPluginWindows.MPEISettings);
            extensionSettings.OnSettingsChanged += new MPEIPlugin.GUISettings.SettingsChangedHandler(Extensions_OnSettingsChanged);
        }

        private void Extensions_OnSettingsChanged(string guid)
        {
            // settings change occured
            if (guid == PluginSettings.cGuid)
            {
                FileLog.Info("Settings updated externally, re-loading trailer settings.");

                // re-load settings
                PluginSettings.LoadSettings(true);
            }
        }
        #endregion
    }
}
