using System;
using MediaPortal.Player;
using OnlineVideos;
using OnlineVideos.MediaPortal1.Player;
using Trailers.GUI;
using Trailers.PluginHandlers;
using Trailers.Providers;
using PlayerFactory = OnlineVideos.MediaPortal1.Player.PlayerFactory;

namespace Trailers.Player
{
    class OnlinePlayer
    {
        internal static string CurrentFileName { get; set; }

        static MediaItem CurrentMedia { get; set; }

        static void GetTrailerUrl(string htmlPage)
        {
            // get playback url from stream
            FileLog.Debug("Getting playback url from page, URL = '{0}'", htmlPage);

            GUIBackgroundTask.Instance.ExecuteInBackgroundAndCallback(() =>
            {
                var hosterBase = OnlineVideos.Hoster.HosterFactory.GetHoster("Youtube");
                return hosterBase.GetVideoUrl(htmlPage);
            },
            delegate(bool success, object result)
            {
                string url = result as string;

                if (success)
                {
                    FileLog.Debug("Successfully found playback url.");

                    if (!string.IsNullOrEmpty(url))
                    {
                        BufferTrailer(url);
                    }
                    else
                    {
                        FileLog.Info("Unable to get url for trailer playback.");
                        GUIUtils.ShowNotifyDialog(Localisation.Translation.Error, Localisation.Translation.UnableToPlayTrailer);
                    }
                }
            },
            Localisation.Translation.GettingTrailerUrls, false);
        }

        static void BufferTrailer(string url)
        {
            // stop player if currently playing some other video
            if (g_Player.Playing) g_Player.Stop();

            // prepare graph must be done on the MP main thread
            FileLog.Debug("Preparing graph for playback, URL = '{0}'.", url);
            var factory = new PlayerFactory(PlayerType.Internal, url);
            bool? prepareResult = ((OnlineVideosPlayer)factory.PreparedPlayer).PrepareGraph();
            FileLog.Debug("Graph is now prepared.");

            switch (prepareResult)
            {
                case true:
                    GUIBackgroundTask.Instance.ExecuteInBackgroundAndCallback(() =>
                    {
                        FileLog.Info("OnlineVideo pre-buffering started.");

                        if (((OnlineVideosPlayer)factory.PreparedPlayer).BufferFile(OnlineVideosHandler.YouTubeSiteUtil))
                        {
                            FileLog.Info("OnlineVideo pre-buffering complete.");
                            return true;
                        }
                        else
                        {
                            FileLog.Error("Error pre-buffering trailer.");
                            return null;
                        }
                    },
                    delegate(bool success, object result)
                    {
                        PlayTrailer(url, factory, result as bool?);
                    },
                    Localisation.Translation.BufferingTrailer, false);
                    break;

                case false:
                    // play without buffering
                    PlayTrailer(url, factory, prepareResult);
                    break;

                default:
                    FileLog.Error("Failed to create player graph.");
                    GUIUtils.ShowNotifyDialog(Localisation.Translation.Error, Localisation.Translation.UnableToPlayTrailer);
                    break;
            }
        }

        static void PlayTrailer(string url, PlayerFactory factory, bool? preparedPlayerResult)
        {
            if (preparedPlayerResult != null)
            {
                (factory.PreparedPlayer as OVSPLayer).GoFullscreen = true;

                var savedFactory = g_Player.Factory;
                g_Player.Factory = factory;

                try
                {
                    CurrentFileName = factory.PreparedUrl;
                    g_Player.Play(factory.PreparedUrl, g_Player.MediaType.Video);

                    // make the OSD pretty with poster, title etc
                    GUIUtils.SetPlayProperties(CurrentMedia, true);
                }
                catch (Exception e)
                {
                    FileLog.Warning("Exception while playing trailer, Reason = '{0}'", e.Message);
                }

                g_Player.Factory = savedFactory;
            }
            else
            {
                factory.PreparedPlayer.Dispose();
                GUIUtils.ShowNotifyDialog(Localisation.Translation.Error, Localisation.Translation.UnableToPlayTrailer);
            }
        }

        internal static void Play(string url, MediaItem mediaItem)
        {
            if (string.IsNullOrEmpty(url)) return;

            if (!Utility.IsPluginAvailable("OnlineVideos"))
            {
                GUIUtils.ShowNotifyDialog(Localisation.Translation.Trailers, Localisation.Translation.OnlineVideosNotInstalled);
                return;
            }
            else if (Utility.FileVersion(Utility.OnlineVideosPlugin) < new Version(2, 0, 0, 0))
            {
                GUIUtils.ShowNotifyDialog(Localisation.Translation.Trailers, Localisation.Translation.OnlineVideosMinVersionNotInstalled);
                return;
            }

            CurrentMedia = mediaItem;

            if (url.ToLowerInvariant().Contains("youtube.com"))
            {
                // use onlinevideo youtube siteutils to get
                // playback urls from youtube url
                GetTrailerUrl(url);
            }
            else
            {
                BufferTrailer(url);
            }
        }
    }
}
