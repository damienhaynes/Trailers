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
        public static string NoTrailersFound = "No Trailers Found!";

        // O
        public static string Off = "Off";
        public static string OK = "OK";
        public static string On = "On";
        public static string Online = "Online";
        public static string OnlineVideosNotInstalled = "OnlineVideos plugin is required to play trailers, ensure that you have the plugin installed and it's enabled.";

        // P
        
        // R

        // S
        public static string Search = "Search";

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

        #endregion
    }

}