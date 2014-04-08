using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;
using Trailers.Downloader;
using Trailers.Downloader.DataStructures;
using Trailers.GUI;
using Trailers.Providers;
using WindowPlugins.GUITVSeries;

namespace Trailers.PluginHandlers
{
    internal class MPTVSeriesHandler
    {
        internal enum ViewLevel
        {
            Series,
            Season,
            Episode,
            Unknown
        }

        internal static bool GetCurrentMediaItem(out MediaItem currentMediaItem)
        {
            FileLog.Info("Getting selected item information from MP-TVSeries.");

            currentMediaItem = new MediaItem();

            // get the object (series, season, episode assigned to the TVTag of the selected facade item)
            Object obj = SelectedObject;
            if (obj == null)
            {
                FileLog.Error("Error getting selected item from MP-TVSeries");
                currentMediaItem = null;
                return false;
            }

            // store the selected item into the MediaItem object
            switch (GetViewLevel(obj))
            {
                #region Series
                case ViewLevel.Series:
                    var series = obj as DBSeries;
                    currentMediaItem.MediaType = MediaItemType.Show;
                    currentMediaItem.Title = series[DBOnlineSeries.cOriginalName];
                    currentMediaItem.Plot = series[DBOnlineSeries.cSummary];
                    currentMediaItem.IMDb = series[DBOnlineSeries.cIMDBID];
                    currentMediaItem.TVDb = series[DBOnlineSeries.cID];
                    currentMediaItem.AirDate = series[DBOnlineSeries.cFirstAired];
                    currentMediaItem.Poster = series.Poster;

                    DateTime airDate;
                    if (DateTime.TryParse(series[DBOnlineSeries.cFirstAired], out airDate))
                    {
                        currentMediaItem.Year = airDate.Year;
                    }

                    break;
                #endregion

                #region Season
                case ViewLevel.Season:
                    var season = obj as DBSeason;
                    series = Helper.getCorrespondingSeries(season[DBSeason.cSeriesID]);
                    if (series == null)
                    {
                        FileLog.Error("Error getting current series from season view level in MP-TVSeries.");
                        currentMediaItem = null;
                        return false;
                    }

                    currentMediaItem.MediaType = MediaItemType.Season;
                    currentMediaItem.Season = season[DBSeason.cIndex];
                    currentMediaItem.Title = series[DBOnlineSeries.cOriginalName];
                    currentMediaItem.Plot = series[DBOnlineSeries.cSummary];
                    currentMediaItem.IMDb = series[DBOnlineSeries.cIMDBID];
                    currentMediaItem.TVDb = series[DBOnlineSeries.cID];
                    currentMediaItem.AirDate = series[DBOnlineSeries.cFirstAired];
                    currentMediaItem.Poster = series.Poster;

                    if (DateTime.TryParse(series[DBOnlineSeries.cFirstAired], out airDate))
                    {
                        currentMediaItem.Year = airDate.Year;
                    }

                    if (currentMediaItem.Season == null)
                    {
                        FileLog.Error("Error getting season index from current selected item in MP-TVSeries.");
                        currentMediaItem = null;
                        return false;
                    }
                    break;
                #endregion

                #region Episode
                case ViewLevel.Episode:
                    var episode = obj as DBEpisode;
                    series = Helper.getCorrespondingSeries(episode[DBOnlineEpisode.cSeriesID]);
                    if (series == null)
                    {
                        FileLog.Error("Error getting current series from season view level in MP-TVSeries.");
                        currentMediaItem = null;
                        return false;
                    }

                    currentMediaItem.MediaType = MediaItemType.Episode;
                    currentMediaItem.Season = episode[DBOnlineEpisode.cSeasonIndex];
                    currentMediaItem.Episode = episode[DBOnlineEpisode.cEpisodeIndex];
                    currentMediaItem.EpisodeName = episode[DBOnlineEpisode.cEpisodeName];
                    currentMediaItem.Title = series[DBOnlineSeries.cOriginalName];
                    currentMediaItem.Plot = series[DBOnlineSeries.cSummary];
                    currentMediaItem.IMDb = series[DBOnlineSeries.cIMDBID];
                    currentMediaItem.TVDb = series[DBOnlineSeries.cID];
                    currentMediaItem.AirDate = series[DBOnlineSeries.cFirstAired];
                    currentMediaItem.Poster = series.Poster;

                    if (DateTime.TryParse(series[DBOnlineSeries.cFirstAired], out airDate))
                    {
                        currentMediaItem.Year = airDate.Year;
                    }

                    if (currentMediaItem.Season == null || currentMediaItem.Episode == null)
                    {
                        FileLog.Error("Error getting season/episode index from current selected item in MP-TVSeries.");
                        currentMediaItem = null;
                        return false;
                    }
                    break;
                #endregion

                case ViewLevel.Unknown:
                    FileLog.Error("Error getting current view level from MP-TVSeries.");
                    currentMediaItem = null;
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Get the current viewlevel selected in TVSeries
        /// </summary>
        /// <param name="obj">TVTag object</param>
        /// <returns>Returns the selected viewlevel</returns>
        static ViewLevel GetViewLevel(Object obj)
        {
            if ((obj as DBEpisode) != null) return ViewLevel.Episode;
            if ((obj as DBSeries) != null) return ViewLevel.Series;
            if ((obj as DBSeason) != null) return ViewLevel.Season;
            return ViewLevel.Unknown;
        }

        /// <summary>
        /// Get the TVTag of the selected facade item in TVseries window
        /// </summary>
        static Object SelectedObject
        {
            get
            {
                if (Facade == null) return null;

                // Get the Selected Item
                GUIListItem currentitem = Facade.SelectedListItem;
                if (currentitem == null) return null;

                // Get the series/episode object
                return currentitem.TVTag;
            }
        }

        static GUIFacadeControl Facade
        {
            get
            {
                // Ensure we are in TVSeries window
                GUIWindow window = GUIWindowManager.GetWindow((int)ExternalPluginWindows.TVSeries);
                if (window == null) return null;

                // Get the Facade control
                return window.GetControl(50) as GUIFacadeControl;
            }
        }
    }
}
