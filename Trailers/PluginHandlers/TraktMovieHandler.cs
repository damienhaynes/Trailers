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
    class TraktMovieHandler
    {
        public static bool GetCurrentMediaItem(out MediaItem currentMediaItem)
        {
            FileLog.Info("Getting selected movie information from Trakt.");
            
            currentMediaItem = new MediaItem();

            // check if we're in a list, currently we only support movie trailers...
            if (GUIWindowManager.ActiveWindow == (int)ExternalPluginWindows.TraktListItems)
            {
                if (GUIPropertyManager.GetProperty("#Trakt.List.ItemType").ToLowerInvariant() != "movie")
                {
                    currentMediaItem = null;
                    return false;
                }
            }

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
    }
}
