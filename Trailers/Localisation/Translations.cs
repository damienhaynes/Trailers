using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Text.RegularExpressions;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Localisation;
using Trailers.GUI;

namespace Trailers.Localisation
{
    static class Translation
    {
        #region Private variables
        
        private static Dictionary<string, string> translations;
        private static Regex translateExpr = new Regex(@"\$\{([^\}]+)\}");
        private static string path = string.Empty;

        #endregion

        #region Constructor

        static Translation()
        {
            
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the translated strings collection in the active language
        /// </summary>
        public static Dictionary<string, string> Strings
        {
            get
            {
                if (translations == null)
                {
                    translations = new Dictionary<string, string>();
                    Type transType = typeof(Translation);
                    FieldInfo[] fields = transType.GetFields(BindingFlags.Public | BindingFlags.Static);
                    foreach (FieldInfo field in fields)
                    {
                        translations.Add(field.Name, field.GetValue(transType).ToString());
                    }
                }
                return translations;
            }
        }

        public static string CurrentLanguage
        {
            get
            {
                string language = string.Empty;
                try
                {
                    language = GUILocalizeStrings.GetCultureName(GUILocalizeStrings.CurrentLanguage());
                }
                catch (Exception)
                {
                    language = CultureInfo.CurrentUICulture.Name;
                }
                return language;
            }
        }
        public static string PreviousLanguage { get; set; }

        #endregion

        #region Public Methods

        public static void Init()
        {
            translations = null;
            FileLog.Info("Using language: " + CurrentLanguage);

            path = Config.GetSubFolder(Config.Dir.Language, "Trailers");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string lang = PreviousLanguage = CurrentLanguage;
            LoadTranslations(lang);

            // publish all available translation strings
            // so skins have access to them
            foreach (string name in Strings.Keys)
            {
                GUIUtils.SetProperty("#Trailers.Translation." + name + ".Label", Translation.Strings[name]);
            }
        }

        public static int LoadTranslations(string lang)
        {
            XmlDocument doc = new XmlDocument();
            Dictionary<string, string> TranslatedStrings = new Dictionary<string, string>();
            string langPath = string.Empty;
            try
            {
                langPath = Path.Combine(path, lang + ".xml");
                doc.Load(langPath);
            }
            catch (Exception e)
            {
                if (lang == "en")
                    return 0; // otherwise we are in an endless loop!

                if (e.GetType() == typeof(FileNotFoundException))
                    FileLog.Warning("Cannot find translation file {0}. Falling back to English", langPath);
                else
                    FileLog.Error("Error in translation xml file: {0}. Falling back to English", lang);

                return LoadTranslations("en");
            }
            foreach (XmlNode stringEntry in doc.DocumentElement.ChildNodes)
            {
                if (stringEntry.NodeType == XmlNodeType.Element)
                {
                    try
                    {
                        string key = stringEntry.Attributes.GetNamedItem("name").Value;
                        if (!TranslatedStrings.ContainsKey(key))
                        {
                            TranslatedStrings.Add(key, stringEntry.InnerText.NormalizeTranslation());
                        }
                        else
                        {
                            FileLog.Error("Error in Translation Engine, the translation key '{0}' already exists.", key);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        FileLog.Error("Error in Translation Engine: {0}", ex.Message);
                    }
                }
            }

            Type TransType = typeof(Translation);
            FieldInfo[] fieldInfos = TransType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo fi in fieldInfos)
            {
                if (TranslatedStrings != null && TranslatedStrings.ContainsKey(fi.Name))
                    TransType.InvokeMember(fi.Name, BindingFlags.SetField, null, TransType, new object[] { TranslatedStrings[fi.Name] });
                else
                    FileLog.Info("Translation not found for field: {0}. Using hard-coded English default.", fi.Name);
            }
            return TranslatedStrings.Count;
        }

        public static string GetByName(string name)
        {
            if (!Strings.ContainsKey(name))
                return name;

            return Strings[name];
        }

        public static string GetByName(string name, params object[] args)
        {
            return String.Format(GetByName(name), args);
        }

        /// <summary>
        /// Takes an input string and replaces all ${named} variables with the proper translation if available
        /// </summary>
        /// <param name="input">a string containing ${named} variables that represent the translation keys</param>
        /// <returns>translated input string</returns>
        public static string ParseString(string input)
        {
            MatchCollection matches = translateExpr.Matches(input);
            foreach (Match match in matches)
            {
                input = input.Replace(match.Value, GetByName(match.Groups[1].Value));
            }
            return input;
        }

        /// <summary>
        /// Temp workaround to remove unwanted chars from Transifex
        /// </summary>
        public static string NormalizeTranslation(this string input)
        {
            input = input.Replace("\\'", "'");
            input = input.Replace("\\\"", "\"");
            return input;
        }
        #endregion

        #region Translations / Strings

        /// <summary>
        /// These will be loaded with the language files content
        /// if the selected lang file is not found, it will first try to load en(us).xml as a backup
        /// if that also fails it will use the hardcoded strings as a last resort.
        /// </summary>

        // A

        // B
        public static string BufferingTrailer = "Buffering Trailer";

        // C

        // D

        // E
        public static string Error = "Trailers Error";

        // F

        // G
        public static string GettingTrailers = "Getting Trailers";
        public static string GettingTrailerUrls = "Getting Trailer Urls";

        // H

        // I
        public static string IMDbTrailers = "IMDb Trailers";
        public static string ITunesTrailers = "iTunes Trailers";

        // J

        // K

        // L
        public static string Local = "Local";

        // M
        public static string MultiSelectDialog = "Multi-Select Dialog";

        // N
        public static string No = "No";
        public static string NoTrailersFound = "No Trailers Found!";
        
        // O
        public static string Off = "Off";
        public static string OK = "OK";
        public static string On = "On";
        public static string Online = "Online";
        public static string OnlineVideosNotInstalled = "OnlineVideos plugin is required to play trailers, ensure that you have the plugin installed and it's enabled.";

        // P
        public static string Play = "Play";

        // R

        // S
        public static string Search = "Search";
        public static string SettingPluginEnabledName = "Plugin Enabled";
        public static string SettingPluginEnabledDescription = "Enable / Disable this setting to control if the Trailers plugin is loaded with MediaPortal.";
        public static string SettingListedHomeName = "Listed in Home";
        public static string SettingListedHomeDescription = "Enable this setting for the Trailers plugin to appear in the main Home screen menu items.";
        public static string SettingListedPluginsName = "Listed in My Plugins";
        public static string SettingListedPluginsDescription = "Enable this setting for the Trailers plugin to appear in the My Plugins screen menu items.";
        public static string Settings = "Settings";
        public static string SettingWebMaxRetriesName = "Max Retries for Web Requests";
        public static string SettingWebMaxRetriesDescription = "The maximum number of retries to perform a repeated web request if an attempt fails e.g. times-out.";
        public static string SettingWebTimeoutName = "Web Request Timeout";
        public static string SettingWebTimeoutDescription = "The amount of time to wait for a response from a web-request.";
        public static string SettingWebTimeoutIncrementName = "Web Request Timeout Increment";
        public static string SettingWebTimeoutIncrementDescription = "The amount of time to wait after a failed web-request before retrying.";
        public static string SettingProviderLocalSearchName = "Enable Local Trailer Provider";
        public static string SettingProviderLocalSearchDescription = "The Local Trailer Provider will search your local drives for trailers, trailers can be stored either with your current media or from a dedicated directory / directories.";
        public static string SettingProviderTMDbMoviesName = "Enable TheMovieDb Trailer Provider";
        public static string SettingProviderTMDbMoviesDescription = "TheMovieDb Trailer Provider will return a list of trailers, featurettes, clips and teasers for the specified movie. It's the preferred source for online search of trailers.";
        public static string SettingProviderOnlineVideoSearchName = "Enable OnlineVideos Trailer Search Provider";
        public static string SettingProviderOnlineVideoSearchDescription = "The OnlineVideos Trailer Search Provider acts as a backup so that a manual search can be triggered if no online or local trailers were found.";
        public static string SettingSearchLocalInCurrentMediaFolderName = "Search Local Trailers in Current Folder";
        public static string SettingSearchLocalInCurrentMediaFolderDescription = "Enable to search for trailers in the current media's folder, typically files be be-side the current media with 'trailer' in the filename.";
        public static string SettingSearchLocalInSubFolderName = "Search Local Trailers in Current SubFolder";
        public static string SettingSearchLocalInSubFolderDescription = "Enable to search for trailers in a list of specified sub-directories of the current menu e.g. in a 'Trailer' sub-directory.";
        public static string SettingSearchLocalInDedicatedDirectoryName = "Search Local Trailers in Dedicated Directories";
        public static string SettingSearchLocalInDedicatedDirectoryDescription = "Enable to search for trailers in a list of specified dedicated directories, these would be outside of the current media path. By default the OnlineVideos and ShowTimes download directories are used for search.";
        public static string SettingOnlineVideosYouTubeEnabledName = "Enable YouTube Site for Manual Search";
        public static string SettingOnlineVideosYouTubeEnabledDescription = "The YouTube site is probably the best site in OnlineVideos to perform a manual search, there is a large range of movies trailers available at good quality.";
        public static string SettingOnlineVideosITunesEnabledName = "Enable iTunes Site for Manual Search";
        public static string SettingOnlineVideosITunesEnabledDescription = "The iTunes Movie Trailer site is a dedicated trailer site, this is a good choice for more recent movies.";
        public static string SettingOnlineVideosIMDbEnabledName = "Enable IMDb Site for Manual Search";
        public static string SettingOnlineVideosIMDbEnabledDescription = "The IMDb Movie Trailer site is a dedicated trailer site, this is a good choice for more recent movies and also allows for exact trailer searches using the IMDb ID.";
        public static string SettingSearchLocalAggressiveSearchName = "Enable Aggressive Search for Local Trailers";
        public static string SettingSearchLocalAggressiveSearchDescription = "Aggressive search will continue to find trailers from the various enabled local search methods even when there has already been match(es) found. This setting is not needed unless you have trailers for the same movie scattered in more than one place.";
        public static string SettingSkipOnlineProvidersIfLocalFoundName = "Skip Online Providers if Local Trailers Found";
        public static string SettingSkipOnlineProvidersIfLocalFoundDescription = "If there are local trailers found in search, then immediately show/play result or continue to search for online trailers and present both results.";
        public static string SettingAutoPlayOnSingleLocalOrOnlineTrailerName = "Auto Play on Single Local or Online Trailer";
        public static string SettingAutoPlayOnSingleLocalOrOnlineTrailerDescription = "If there is only a single local or online trailer found, then automatically play trailer. When disabled (default) a menu of results will be shown including manual search providers.";
        public static string SettingAutoDownloadTrailersMovingPicturesName = "Enable Auto Download Trailers for MovingPictures Library";
        public static string SettingAutoDownloadTrailersMovingPicturesDescription = "Enable to automatically download trailers for movies in your MovingPictures library.";
        public static string SettingAutoDownloadTrailersMyFilmsName = "Enable Auto Download Trailers for My Films Library";
        public static string SettingAutoDownloadTrailersMyFilmsDescription = "Enable to automatically download trailers for movies in your My Films library.";
        public static string SettingAutoDownloadTrailersMyVideosName = "Enable Auto Download Trailers for My Videos Library";
        public static string SettingAutoDownloadTrailersMyVideosDescription = "Enable to automatically download trailers for movies in your My Videos library.";
        public static string SettingAutoDownloadStartDelayName = "Auto Download Startup Delay";
        public static string SettingAutoDownloadStartDelayDescription = "Sets the delay on startup before checking your plugin libraries for new trailers to download.";
        public static string SettingAutoDownloadIntervalName = "Auto Download Interval";
        public static string SettingAutoDownloadIntervalDescription = "Sets the interval for library checks to see if new movies have been added for automatic trailer download.";
        public static string SettingAutoDownloadUpdateIntervalName = "Auto Download Update Interval";
        public static string SettingAutoDownloadUpdateIntervalDescription = "Sets the interval to re-check movies previously checked that have no trailers. New trailers may have been added online between this value.";
        public static string SettingAutoDownloadQualityName = "Auto Download Video Quality";
        public static string SettingAutoDownloadQualityDescription = "Set the quality settings for video download from youtube trailers, if the desired quality is not available then the next lowest quality will be downloaded.";
        public static string SettingAutoDownloadTrailersName = "Enable Auto Download of Trailers";
        public static string SettingAutoDownloadTrailersDescription = "Checks if the video type is a trailer for automatic trailer downloads.";
        public static string SettingAutoDownloadTeasersName = "Enable Auto Download of Teasers";
        public static string SettingAutoDownloadTeasersDescription = "Checks if the video type is a teaser for automatic trailer downloads.";
        public static string SettingAutoDownloadFeaturettesName = "Enable Auto Download of Featurettes";
        public static string SettingAutoDownloadFeaturettesDescription = "Checks if the video type is a featurette for automatic trailer downloads.";
        public static string SettingAutoDownloadClipsName = "Enable Auto Download of Clips";
        public static string SettingAutoDownloadClipsDescription = "Checks if the video type is a clip for automatic trailer downloads.";
        public static string SettingAutoDownloadCleanupName = "Enable Auto Download Cleanup";
        public static string SettingAutoDownloadCleanupDescription = "Enable this to remove any trailers from disk if the associated movie is no longer in your plugin libraries.";

        // T
        public static string Timeout = "Timeout";
        public static string Trailer = "Trailer";
        public static string Trailers = "Trailers";

        // U
        public static string UnableToPlayTrailer = "Unable to play trailer.";

        // V

        // W

        // Y
        public static string YouTubeTrailers = "YouTube Trailers";
        public static string Yes = "Yes";

        #endregion
    }

}