using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;
using Trailers.GUI;
using Trailers.Providers;

namespace Trailers.PluginHandlers
{
    class TraktHandler
    {
        internal enum WindowType
        {
            Movie,
            Show,
            Season,
            Episode,
            List,
            Unknown
        }

        public static bool GetCurrentMediaItem(out MediaItem currentMediaItem, WindowType windowType)
        {
            currentMediaItem = new MediaItem();

            switch (windowType)
            {
                case WindowType.Movie:
                    return GetMovieMediaItem(ref currentMediaItem);
                    
                case WindowType.Show:
                    return GetShowMediaItem(ref currentMediaItem);

                case WindowType.Season:
                    return GetSeasonMediaItem(ref currentMediaItem);

                case WindowType.Episode:
                    return GetEpisodeMediaItem(ref currentMediaItem);

                case WindowType.List:
                    return GetListMediaItem(ref currentMediaItem);
                    
            }

            return false;
        }

        private static bool GetListMediaItem(ref MediaItem currentMediaItem)
        {
            FileLog.Info("Getting selected list item information from Trakt.");

            string listType = GUIPropertyManager.GetProperty("#Trakt.List.ItemType").ToLowerInvariant();

            // check if we're in a list which supports movies, shows, seasons and episodes
            switch (listType)
            {
                case "movie":
                    return GetMovieMediaItem(ref currentMediaItem);

                case "show":
                    return GetShowMediaItem(ref currentMediaItem);

                case "season":
                    return GetSeasonMediaItem(ref currentMediaItem);

                case "episode":
                    return GetEpisodeMediaItem(ref currentMediaItem);
            }

            return true;
        }

        private static bool GetMovieMediaItem(ref MediaItem currentMediaItem)
        {
            FileLog.Info("Getting selected movie information from Trakt.");
            
            currentMediaItem.MediaType = MediaItemType.Movie;

            // get title
            currentMediaItem.Title = GUIPropertyManager.GetProperty("#Trakt.Movie.Title").Trim();

            // get year
            int year;
            if (int.TryParse(GUIPropertyManager.GetProperty("#Trakt.Movie.Year").Trim(), out year))
                currentMediaItem.Year = year;

            // get IMDb ID
            string imdbid = GUIPropertyManager.GetProperty("#Trakt.Movie.Imdb").Trim();
            if (!string.IsNullOrEmpty(imdbid) && imdbid.Length == 9)
                currentMediaItem.IMDb = imdbid;

            // get TMDb ID
            int iTMDbID;
            if (int.TryParse(GUIPropertyManager.GetProperty("#Trakt.Movie.Tmdb").Trim(), out iTMDbID))
                currentMediaItem.TMDb = iTMDbID.ToString();

            // get poster
            currentMediaItem.Poster = GUIPropertyManager.GetProperty("#Trakt.Movie.PosterImageFilename").Trim();

            // get overview
            currentMediaItem.Plot = GUIPropertyManager.GetProperty("#Trakt.Movie.Overview").Trim();

            return true;
        }

        private static bool GetShowMediaItem(ref MediaItem currentMediaItem)
        {
            FileLog.Info("Getting selected show information from Trakt.");

            currentMediaItem.MediaType = MediaItemType.Show;

            // get title
            currentMediaItem.Title = GUIPropertyManager.GetProperty("#Trakt.Show.Title").Trim();

            // get year
            int year;
            if (int.TryParse(GUIPropertyManager.GetProperty("#Trakt.Show.Year").Trim(), out year))
                currentMediaItem.Year = year;

            // get air date
            currentMediaItem.AirDate = GUIPropertyManager.GetProperty("#Trakt.Show.FirstAired").Trim();

            // get IMDb ID
            string imdbid = GUIPropertyManager.GetProperty("#Trakt.Show.Imdb").Trim();
            if (!string.IsNullOrEmpty(imdbid) && imdbid.Length == 9)
                currentMediaItem.IMDb = imdbid;

            // get TVDb ID
            int iTVDbID;
            if (int.TryParse(GUIPropertyManager.GetProperty("#Trakt.Show.Tvdb").Trim(), out iTVDbID))
                currentMediaItem.TVDb = iTVDbID.ToString();

            // get TVRage ID
            int iTVRageID;
            if (int.TryParse(GUIPropertyManager.GetProperty("#Trakt.Show.TvRage").Trim(), out iTVRageID))
                currentMediaItem.TVRage = iTVRageID.ToString();

            // get poster
            currentMediaItem.Poster = GUIPropertyManager.GetProperty("#Trakt.Show.PosterImageFilename").Trim();

            // get overview
            currentMediaItem.Plot = GUIPropertyManager.GetProperty("#Trakt.Show.Overview").Trim();

            return true;
        }

        private static bool GetSeasonMediaItem(ref MediaItem currentMediaItem)
        {
            // first get show info
            GetShowMediaItem(ref currentMediaItem);

            FileLog.Info("Getting selected season information from Trakt.");

            currentMediaItem.MediaType = MediaItemType.Season;

            // get season
            currentMediaItem.Season = int.Parse(GUIPropertyManager.GetProperty("#Trakt.Season.Number").Trim());

            // get poster
            currentMediaItem.Poster = GUIPropertyManager.GetProperty("#Trakt.Season.PosterImageFilename").Trim();

            return true;
        }

        private static bool GetEpisodeMediaItem(ref MediaItem currentMediaItem)
        {
            FileLog.Info("Getting selected episode information from Trakt.");

            // first get show info
            GetShowMediaItem(ref currentMediaItem);

            currentMediaItem.MediaType = MediaItemType.Episode;

            // get season
            currentMediaItem.Season = int.Parse(GUIPropertyManager.GetProperty("#Trakt.Episode.Season").Trim());

            // get episode
            currentMediaItem.Episode = int.Parse(GUIPropertyManager.GetProperty("#Trakt.Episode.Number").Trim());

            // get episode name
            currentMediaItem.EpisodeName = GUIPropertyManager.GetProperty("#Trakt.Episode.Title").Trim();

            // get thumb
            currentMediaItem.Poster = GUIPropertyManager.GetProperty("#Trakt.Episode.EpisodeImageFilename").Trim();

            return true;
        }
    }
}
