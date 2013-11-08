using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.Plugins.MovingPictures;
using MediaPortal.Plugins.MovingPictures.Database;
using MediaPortal.Plugins.MovingPictures.MainUI;
using Trailers.Providers;

namespace Trailers.PluginHandlers
{
    class MovingPicturesHandler
    {
        private static DBSourceInfo tmdbSource;
        private static MovieBrowser browser;
        private static DBMovieInfo selectedMovie;

        public static bool GetCurrentMediaItem(out MediaItem currentMediaItem, out bool isDetailsView)
        {
            FileLog.Info("Getting selected movie information from MovingPictures.");
            
            currentMediaItem = new MediaItem();
            browser = MovingPicturesCore.Browser;
            isDetailsView = browser.CurrentView.ToString().Equals("DETAILS");
            
            selectedMovie = browser.SelectedMovie;
            
            currentMediaItem.Title = selectedMovie.Title;
            currentMediaItem.Year = selectedMovie.Year;
            currentMediaItem.Plot = selectedMovie.Summary;
            currentMediaItem.Poster = selectedMovie.CoverFullPath;

            // Get local file information
            currentMediaItem.FullPath = selectedMovie.LocalMedia.First().FullPath;
            
            // Check if TMDb ID is available
            string tmdbid = GetTmdbID(selectedMovie);
            if (!string.IsNullOrEmpty(tmdbid)) currentMediaItem.TMDb = tmdbid;

            // Next best ID to use
            string imdbid = selectedMovie.ImdbID;
            if (!string.IsNullOrEmpty(imdbid) && imdbid.Length == 9) currentMediaItem.IMDb = imdbid;

            return true;
        }

        private static string GetTmdbID(DBMovieInfo movie)
        {
            if (tmdbSource == null)
            {
                tmdbSource = GetTmdbSourceInfo();
                if (tmdbSource == null) return null;
            }

            string id = movie.GetSourceMovieInfo(tmdbSource).Identifier;
            if (id == null || id.Trim() == string.Empty) return null;
            if (int.Parse(id.Trim()) <= 0) return null;

            return id;
        }

        private static DBSourceInfo GetTmdbSourceInfo()
        {
            return DBSourceInfo.GetAll().Find(s => s.ToString() == "themoviedb.org");
        }
    }
}
