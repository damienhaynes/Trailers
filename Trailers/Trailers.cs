using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Player;
using Trailers.Downloader;
using Trailers.GUI;
using Trailers.Localisation;
using Trailers.Player;
using Trailers.PluginHandlers;
using Trailers.Providers;

namespace Trailers
{
    [PluginIcons("Trailers.Images.icon_normal.png", "Trailers.Images.icon_faded.png")]
    public class Trailers : GUIInternalWindow, ISetupForm
    {
        #region Private Variables
        ExtensionSettings ExtensionSettings = new ExtensionSettings();
        static MediaItem CurrentMediaItem = new MediaItem();
        static bool ReShowTrailerMenu = false;
        #endregion

        #region Public Variables

        internal static List<IProvider> TrailerProviders = new List<IProvider>();
        
        #endregion

        #region GUIInternalWindow Overrides

        /// <summary>
        /// will set #currentmodule property on pageload
        /// </summary>
        public override string GetModuleName()
        {
            return PluginName();
        }

        public override int GetID
        {
            get
            {
                return GetWindowId();
            }
        }

        /// <summary>
        /// Starting Point
        /// </summary>
        public override bool Init()
        {
            FileLog.Info("Starting Trailers v{0}", PluginSettings.Version);

            PluginSettings.PerformMaintenance();
            PluginSettings.LoadSettings();
            
            // Initialize Extension Settings
            ExtensionSettings.Init();

            // Load Trailer Providers
            LoadTrailerProviders();

            // Listen to this event to detect skin\language changes in GUI
            GUIWindowManager.OnDeActivateWindow += new GUIWindowManager.WindowActivationHandler(GUIWindowManager_OnDeActivateWindow);
            GUIWindowManager.OnActivateWindow += new GUIWindowManager.WindowActivationHandler(GUIWindowManager_OnActivateWindow);
            GUIWindowManager.Receivers += new SendMessageHandler(GUIWindowManager_Receivers);

            // Listen to player events
            g_Player.PlayBackEnded += new g_Player.EndedHandler(g_Player_PlayBackEnded);
            g_Player.PlayBackStopped += new g_Player.StoppedHandler(g_Player_PlayBackStopped);

            // Initialize translations
            Translation.Init();

            // Initilize plugins for auto trailer download
            TrailerDownloader.Init();

            // Load main skin window
            // this is a launching pad to all other windows
            string xmlSkin = GUIGraphicsContext.Skin + @"\Trailers.xml";
            FileLog.Info("Loading main skin window: " + xmlSkin);
            return Load(xmlSkin);
        }

        /// <summary>
        /// End Point (Clean up)
        /// </summary>
        public override void DeInit()
        {
            FileLog.Debug("Removing Mediaportal Hooks");
            GUIWindowManager.OnDeActivateWindow -= GUIWindowManager_OnDeActivateWindow;
            GUIWindowManager.OnActivateWindow -= GUIWindowManager_OnActivateWindow;
            GUIWindowManager.Receivers -= GUIWindowManager_Receivers;

            // UnLoad Trailer Providers
            UnLoadTrailerProviders();

            // save settings
            PluginSettings.SaveSettings();

            FileLog.Info("Goodbye");
            base.DeInit();
        }

        protected override void OnPageLoad()
        {
            base.OnPageLoad();
        }

        #endregion

        #region ISetupForm Members

        public string Author()
        {
            return "Damien Haynes (ltfearme)";
        }

        public bool CanEnable()
        {
            return true;
        }

        public bool DefaultEnabled()
        {
            return true;
        }

        public string Description()
        {
            return "Adds trailer support for all media plugins in MediaPortal.";
        }

        public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus, out string strPictureImage)
        {
            strButtonText = PluginName();
            strButtonImage = string.Empty;
            strButtonImageFocus = string.Empty;
            strPictureImage = "hover_trailers.png";

            // don't display on home screen if skin doesn't exist.
            return File.Exists(GUIGraphicsContext.Skin + @"\Trailers.xml");
        }

        public int GetWindowId()
        {
            return 1189;
        }

        public bool HasSetup()
        {
            return true;
        }

        public string PluginName()
        {
            return "Trailers";
        }

        public void ShowPlugin()
        {
            Configuration.MainConfig config = new Configuration.MainConfig();
            config.ShowDialog();
        }

        #endregion

        #region Private Methods

        private void LoadTrailerProviders()
        {
            FileLog.Info("Loading Trailer Providers");

            if (!TrailerProviders.Exists(t => t.Name == "Local Trailer Search Provider"))
            {
                TrailerProviders.Add(new LocalTrailerProvider(PluginSettings.ProviderLocalSearch));
            }

            if (!TrailerProviders.Exists(t => t.Name == "TMDb Trailer Provider"))
            {
                TrailerProviders.Add(new TMDbTrailerProvider(PluginSettings.ProviderTMDb));
            }

            if (!TrailerProviders.Exists(t => t.Name == "OnlineVideos Trailer Search Provider"))
            {
                TrailerProviders.Add(new OnlineVideoSearchProvider(PluginSettings.ProviderOnlineVideoSearch && Utility.IsPluginAvailable("OnlineVideos")));
            }
        }

        private void UnLoadTrailerProviders()
        {
            FileLog.Info("Unloading Trailer Providers");

            if (TrailerProviders.Exists(t => t.Name == "Local Trailer Search Provider"))
            {
                var item = TrailerProviders.FirstOrDefault(t => t.Name == "Local Trailer Search Provider");
                TrailerProviders.Remove(item);
            }

            if (TrailerProviders.Exists(t => t.Name == "TMDb Trailer Provider"))
            {
                var item = TrailerProviders.FirstOrDefault(t => t.Name == "TMDb Trailer Provider");
                TrailerProviders.Remove(item);
            }

            if (TrailerProviders.Exists(t => t.Name == "OnlineVideos Trailer Search Provider"))
            {
                var item = TrailerProviders.FirstOrDefault(t => t.Name == "OnlineVideos Trailer Search Provider");
                TrailerProviders.Remove(item);
            }
        }

        #endregion

        #region MediaPortal Hooks

        void GUIWindowManager_OnDeActivateWindow(int windowID)
        {
            // Settings/General window
            // this is where a user can change skins\languages from GUI
            if (windowID == (int)ExternalPluginWindows.MPSkinSettings)
            {
                //did language change?
                if (Translation.CurrentLanguage != Translation.PreviousLanguage)
                {
                    FileLog.Info("Language Changed to '{0}' from GUI, initializing translations.", Translation.CurrentLanguage);
                    Translation.Init();
                }
            }
        }

        void GUIWindowManager_OnActivateWindow(int windowID)
        {

        }

        void GUIWindowManager_Receivers(GUIMessage message)
        {
            // check event was fired from a trailer button
            // buttons allocated are 11899, 11900 and 11901
            if (message.SenderControlId < 11899 || message.SenderControlId > 11901) return;

            bool isDetailsView = true;
            MediaItem currentMediaItem = null;

            switch (message.Message)
            {
                case GUIMessage.MessageType.GUI_MSG_CLICKED:
                    switch (GUIWindowManager.ActiveWindow)
                    {
                        case (int)ExternalPluginWindows.VideoFile:
                        case (int)ExternalPluginWindows.VideoTitle:
                            VideoInfoHandler.GetCurrentMediaItem(out currentMediaItem);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 50);
                            break;

                        case (int)ExternalPluginWindows.VideoInfo:
                            VideoInfoHandler.GetCurrentMediaItem(out currentMediaItem);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 2);
                            break;

                        case (int)ExternalPluginWindows.MovingPictures:
                            MovingPicturesHandler.GetCurrentMediaItem(out currentMediaItem, out isDetailsView);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, isDetailsView ? 6 : 50);
                            break;

                        case (int)ExternalPluginWindows.MyFilmsDetails:
                            MyFilmsHandler.GetCurrentMediaItem(out currentMediaItem);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 10000);
                            break;

                        case (int)ExternalPluginWindows.ShowTimes:
                            ShowTimesHandler.GetCurrentMediaItem(out currentMediaItem, out isDetailsView);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, isDetailsView ? 42 : 50);
                            break;

                        case (int)ExternalPluginWindows.TVSeries:
                            MPTVSeriesHandler.GetCurrentMediaItem(out currentMediaItem);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 50);
                            break;

                        // trakt movie windows
                        case (int)ExternalPluginWindows.TraktRecentAddedMovies:
                        case (int)ExternalPluginWindows.TraktRecentWatchedMovies:
                        case (int)ExternalPluginWindows.TraktRecommendationsMovies:
                        case (int)ExternalPluginWindows.TraktRelatedMovies:
                        case (int)ExternalPluginWindows.TraktSearchMovies:
                        case (int)ExternalPluginWindows.TraktTrendingMovies:
                        case (int)ExternalPluginWindows.TraktWatchedListMovies:
                        case (int)ExternalPluginWindows.TraktPopularMovies:
                        case (int)ExternalPluginWindows.TraktAnticipatedMovies:
                        case (int)ExternalPluginWindows.TraktBoxOffice:
                        case (int)ExternalPluginWindows.TraktPersonCreditMovies:
                            TraktHandler.GetCurrentMediaItem(out currentMediaItem, TraktHandler.WindowType.Movie);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 50);
                            break;

                        // trakt show windows
                        case (int)ExternalPluginWindows.TraktRecommendationsShows:
                        case (int)ExternalPluginWindows.TraktRelatedShows:
                        case (int)ExternalPluginWindows.TraktTrendingShows:
                        case (int)ExternalPluginWindows.TraktWatchedListShows:
                        case (int)ExternalPluginWindows.TraktSearchShows:
                        case (int)ExternalPluginWindows.TraktPopularShows:
                        case (int)ExternalPluginWindows.TraktAnticipatedShows:
                        case (int)ExternalPluginWindows.TraktPersonCreditShows:
                            TraktHandler.GetCurrentMediaItem(out currentMediaItem, TraktHandler.WindowType.Show);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 50);
                            break;

                        // trakt season windows
                        case (int)ExternalPluginWindows.TraktShowSeasons:
                            TraktHandler.GetCurrentMediaItem(out currentMediaItem, TraktHandler.WindowType.Season);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 50);
                            break;

                        // trakt episode windows
                        case (int)ExternalPluginWindows.TraktCalendar:
                        case (int)ExternalPluginWindows.TraktRecentAddedEpisodes:
                        case (int)ExternalPluginWindows.TraktRecentWatchedEpisodes:
                        case (int)ExternalPluginWindows.TraktSearchEpisodes:
                        case (int)ExternalPluginWindows.TraktSeasonEpisodes:
                        case (int)ExternalPluginWindows.TraktWatchedListEpisodes:
                            TraktHandler.GetCurrentMediaItem(out currentMediaItem, TraktHandler.WindowType.Episode);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 50);
                            break;

                        // trakt list window
                        case (int)ExternalPluginWindows.TraktListItems:
                            TraktHandler.GetCurrentMediaItem(out currentMediaItem, TraktHandler.WindowType.List);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 50);
                            break;

                        case (int)ExternalPluginWindows.NetflixAlpha:
                            NetflixHandler.GetCurrentMediaItem(out currentMediaItem, out isDetailsView);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, isDetailsView ? 6 : 50);
                            break;
                    }
                    break;
            }

            if (currentMediaItem != null)
            {
                switch (currentMediaItem.MediaType)
                {
                    case MediaItemType.Movie:
                        FileLog.Info("Searching for movie trailers on: Title='{0}', Year='{1}', IMDb='{2}', TMDb='{3}', Filename='{4}'", currentMediaItem.Title, currentMediaItem.Year.ToString(), currentMediaItem.IMDb ?? "<empty>", currentMediaItem.TMDb ?? "<empty>", currentMediaItem.FilenameWOExtension ?? "<empty>");
                        break;

                    case MediaItemType.Show:
                        FileLog.Info("Searching for tv show trailers on: Title='{0}', Year='{1}', FirstAired='{2}', IMDb='{3}', TVDb='{4}', TMDb='{5}'", currentMediaItem.Title, currentMediaItem.Year.ToString(), currentMediaItem.AirDate ?? "<empty>", currentMediaItem.IMDb ?? "<empty>", currentMediaItem.TVDb ?? "<empty>", currentMediaItem.TMDb ?? "<empty>");
                        break;

                    case MediaItemType.Season:
                        FileLog.Info("Searching for tv season trailers on: Title='{0}', Season='{1}', Year='{2}', FirstAired='{3}', IMDb='{4}', TVDb='{5}', TMDb='{6}'", currentMediaItem.Title, currentMediaItem.Season, currentMediaItem.Year.ToString(), currentMediaItem.AirDate ?? "<empty>", currentMediaItem.IMDb ?? "<empty>", currentMediaItem.TVDb ?? "<empty>", currentMediaItem.TMDb ?? "<empty>");
                        break;

                    case MediaItemType.Episode:
                        FileLog.Info("Searching for tv episode trailers on: Title='{0}', Season='{1}', Episode='{2}', Year='{3}', FirstAired='{4}', IMDb='{5}', TVDb='{6}', TMDb='{7}', Filename='{8}'", currentMediaItem.Title, currentMediaItem.Season, currentMediaItem.Episode, currentMediaItem.Year.ToString(), currentMediaItem.AirDate ?? "<empty>", currentMediaItem.IMDb ?? "<empty>", currentMediaItem.TVDb ?? "<empty>", currentMediaItem.TMDb ?? "<empty>", currentMediaItem.FilenameWOExtension ?? "<empty>");
                        break;
                }
                
                SearchForTrailers(currentMediaItem);
            }
        }

        #endregion

        #region MediaPortal Playback Hooks
        
        private void g_Player_PlayBackStopped(g_Player.MediaType type, int stoptime, string filename)
        {
            if (ReShowTrailerMenu && (OnlinePlayer.CurrentFileName == filename || LocalPlayer.CurrentFileName == filename))
            {
                SearchForTrailers(CurrentMediaItem);
            }
        }

        private void g_Player_PlayBackEnded(g_Player.MediaType type, string filename)
        {
            if (ReShowTrailerMenu && (OnlinePlayer.CurrentFileName == filename || LocalPlayer.CurrentFileName == filename))
            {
                SearchForTrailers(CurrentMediaItem);
            }
        }

        #endregion

        #region Public Methods

        public static void SearchForTrailers(MediaItem searchItem)
        {
            ReShowTrailerMenu = false;

            GUIBackgroundTask.Instance.ExecuteInBackgroundAndCallback(() =>
            {
                var menuItems = new List<GUITrailerListItem>();

                // add enabled local sources
                foreach (var trailerProvider in TrailerProviders.Where(t => t.IsLocal && t.Enabled))
                {
                    menuItems.AddRange(trailerProvider.Search(searchItem));
                }

                // add enabled online sources
                if (menuItems.Count == 0 || !PluginSettings.SkipOnlineProvidersIfLocalFound)
                {
                    foreach (var trailerProvider in TrailerProviders.Where(t => !t.IsLocal && t.Enabled))
                    {
                        menuItems.AddRange(trailerProvider.Search(searchItem));
                    }
                }

                return menuItems;
            },
            delegate(bool success, object result)
            {
                if (success)
                {
                    FileLog.Debug("Showing Trailer Menu for selection.");
                    var menuItems = result as List<GUITrailerListItem>;
                    if (menuItems.Count > 0)
                    {
                        #region Auto-Play
                        // only one local trailer
                        if (PluginSettings.AutoPlayOnSingleLocalOrOnlineTrailer && menuItems.Where(t => !t.IsOnlineItem && !t.IsSearchItem).Count() == 1)
                        {
                            var localTrailer = menuItems.Find(t => !t.IsOnlineItem);
                            LocalPlayer.Play(localTrailer.URL, localTrailer.CurrentMedia);
                            return;
                        }
                        // only one online trailer
                        else if (PluginSettings.AutoPlayOnSingleLocalOrOnlineTrailer && menuItems.Where(t => t.IsOnlineItem && !t.IsSearchItem).Count() == 1)
                        {
                            var onlineTrailer = menuItems.Find(t => t.IsOnlineItem && !t.IsSearchItem);
                            OnlinePlayer.Play(onlineTrailer.URL, onlineTrailer.CurrentMedia);
                            return;
                        }
                        // only one of anything - doesn't matter if AutoPlay is enabled just do it.
                        else if (menuItems.Count == 1)
                        {
                            FileLog.Info("Only a single result to show, skipping GUI menu selection dialog.");

                            if (menuItems.First().IsSearchItem)
                            {
                                FileLog.Info("Performing online lookup for trailer: {0}", menuItems.First().URL);
                                GUIWindowManager.ActivateWindow((int)ExternalPluginWindows.OnlineVideos, menuItems.First().URL);
                            }
                            else if (menuItems.First().IsOnlineItem)
                            {
                                FileLog.Info("Performing online lookup for trailer: {0}", menuItems.First().URL);
                                GUIWindowManager.ActivateWindow((int)ExternalPluginWindows.OnlineVideos, menuItems.First().URL);
                            }
                            else
                            {
                                LocalPlayer.Play(menuItems.First().URL, menuItems.First().CurrentMedia);
                            }
                            return;
                        }
                        #endregion

                        #region Show Menu
                        int selectedItem = GUIUtils.ShowMenuDialog(menuItems.First().CurrentMedia.ToString(), menuItems);
                        if (selectedItem >= 0)
                        {
                            // re-show menu after playback is enabled and there is more than one local/online trailer to select.
                            ReShowTrailerMenu = PluginSettings.ReShowMenuAfterTrailerPlay && (menuItems.Count(t => !t.IsSearchItem) > 1);
                            CurrentMediaItem = menuItems[selectedItem].CurrentMedia;

                            // Search or Play?
                            if (menuItems[selectedItem].IsSearchItem)
                            {
                                FileLog.Info("Performing online lookup for trailer: {0}", menuItems[selectedItem].URL);
                                GUIWindowManager.ActivateWindow((int)ExternalPluginWindows.OnlineVideos, menuItems[selectedItem].URL);
                            }
                            else if (menuItems[selectedItem].IsOnlineItem)
                            {
                                // play the selected trailer
                                OnlinePlayer.Play(menuItems[selectedItem].URL, menuItems[selectedItem].CurrentMedia);
                            }
                            else
                            {
                                // play local media
                                LocalPlayer.Play(menuItems[selectedItem].URL, menuItems[selectedItem].CurrentMedia);
                            }
                        }
                        else
                        {
                            FileLog.Debug("No Trailer selected for playback or search.");
                        }
                        #endregion
                    }
                    else
                    {
                        GUIUtils.ShowNotifyDialog(Translation.Trailers, Translation.NoTrailersFound);
                    }
                }
            }, Translation.GettingTrailers, true);
        }

        #endregion
    }
}
