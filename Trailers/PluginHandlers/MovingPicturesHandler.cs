using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaPortal.Configuration;
using MediaPortal.Plugins.MovingPictures;
using MediaPortal.Plugins.MovingPictures.Database;
using MediaPortal.Plugins.MovingPictures.MainUI;
using Trailers.Downloader;
using Trailers.Downloader.DataStructures;
using Trailers.Providers;

namespace Trailers.PluginHandlers
{
    internal class MovingPicturesHandler : IDownloader
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

        #region IDownloader Members

        public MovingPicturesHandler(bool enabled)
        {
            FileLog.Info("Loading Auto-Downloader: '{0}', Enabled: '{1}'", MoviePluginSource.MovingPictures, enabled);

            // check if plugin exists otherwise plugin could accidently get added to list
            string pluginFilename = Path.Combine(Config.GetSubFolder(Config.Dir.Plugins, "Windows"), "MovingPictures.dll");
            if (!File.Exists(pluginFilename))
                return;

            this.Enabled = enabled;
        }

        public bool Enabled { get; set; }

        public string Name
        { 
            get { return MoviePluginSource.MovingPictures.ToString(); }
        }

        public void Download()
        {
            // get all movies in database
            var localMovies = DBMovieInfo.GetAll();

            FileLog.Info("{0} movies in MovingPictures database", localMovies.Count);

            // get locally cached trailers
            var cache = TrailerDownloader.LoadMovieList(MoviePluginSource.MovingPictures);

            // process local movies
            var movieList = new List<Movie>();

            foreach (var movie in localMovies)
            {
                // add to cache if it doesn't already exist
                if (!cache.Movies.Exists(m => m.IMDbID == (movie.ImdbID ?? string.Empty).Trim() && m.Title == movie.Title && m.Year == movie.Year.ToString()))
                {
                    FileLog.Info("Adding Title='{0}', Year='{1}', IMDb='{2}', TMDb='{3}' to movie trailer cache.", movie.Title, movie.Year, movie.ImdbID ?? "<empty>", GetTmdbID(movie) ?? "<empty>");

                    movieList.Add(new Movie
                    {
                        File = movie.LocalMedia.First().FullPath,
                        IMDbID = movie.ImdbID,
                        TMDbID = GetTmdbID(movie),
                        Plot = movie.Summary,
                        Cast = movie.Actors.ToString(),
                        Directors = movie.Directors.ToString(),
                        Writers = movie.Writers.ToString(),
                        Year = movie.Year.ToString(),
                        Title = movie.Title,
                        Poster = movie.CoverFullPath,
                        Fanart = movie.BackdropFullPath,
                        Genres = movie.Genres.ToString()
                    });
                }
            }
            
            // remove any movies from cache that are no longer in local collection
            cache.Movies.RemoveAll(c => !localMovies.Exists(l => (l.ImdbID ?? string.Empty).Trim() == c.IMDbID && l.Title == c.Title && l.Year.ToString() == c.Year));

            // add any new local movies to cache since last time 
            cache.Movies.AddRange(movieList);

            // process the movie cache and download trailers
            TrailerDownloader.ProcessAndDownloadTrailers(cache, MoviePluginSource.MovingPictures);

            return;
        }

        #endregion
    }
}
