using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;
using MyFilmsPlugin.MyFilms;
using MyFilmsPlugin.MyFilms.MyFilmsGUI;
using Trailers.Providers;

namespace Trailers.PluginHandlers
{
    class MyFilmsHandler
    {
        private static MFMovie selectedMovie;

        public static bool GetCurrentMediaItem(out MediaItem currentMediaItem)
        {
            FileLog.Info("Getting selected movie information from My Films.");

            currentMediaItem = new MediaItem();
            selectedMovie = MyFilmsDetail.GetCurrentMovie();
            
            currentMediaItem.Title = selectedMovie.Title;
            currentMediaItem.Year = selectedMovie.Year;
            currentMediaItem.Poster = selectedMovie.Picture;
            currentMediaItem.Plot = GUIPropertyManager.GetProperty("#myfilms.db.description.value").Trim();

            // Get local file information
            currentMediaItem.FullPath = selectedMovie.File;

            // Check if TMDb ID is available
            int tmdbid = 0;
            if (int.TryParse(selectedMovie.TMDBNumber, out tmdbid))
            {
                if (tmdbid > 0) currentMediaItem.TMDb = tmdbid.ToString();
            }

            // Next best ID to use
            string imdbid = selectedMovie.IMDBNumber;
            if (!string.IsNullOrEmpty(imdbid) && imdbid.Length == 9) currentMediaItem.IMDb = imdbid;

            return true;
        }
    }
}
