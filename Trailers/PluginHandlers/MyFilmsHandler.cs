using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MyFilmsPlugin.MyFilms;
using MyFilmsPlugin.MyFilms.MyFilmsGUI;
using Trailers.Downloader;
using Trailers.Downloader.DataStructures;
using Trailers.Providers;

namespace Trailers.PluginHandlers
{
    class MyFilmsHandler : IDownloader
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

        #region IDownloader Members

        public MyFilmsHandler(bool enabled)
        {
            FileLog.Info("Loading Auto-Downloader: '{0}', Enabled: '{1}'", MoviePluginSource.MyFilms, enabled);

            // check if plugin exists otherwise plugin could accidently get added to list
            string pluginFilename = Path.Combine(Config.GetSubFolder(Config.Dir.Plugins, "Windows"), "MyFilms.dll");
            if (!File.Exists(pluginFilename))
                return;

            this.Enabled = enabled;
        }

        public bool Enabled { get; set; }

        public string Name
        {
            get { return MoviePluginSource.MyFilms.ToString(); }
        }

        public void Download()
        {
            // get all movies in database
            ArrayList myvideos = new ArrayList();
            BaseMesFilms.GetMovies(ref myvideos);
            
            var localMovies = (from MFMovie movie in myvideos select movie).ToList();

            FileLog.Info("{0} movies in My Films database", localMovies.Count);

            // get locally cached trailers
            var cache = TrailerDownloader.LoadMovieList(MoviePluginSource.MyFilms);

            // process local movies
            var movieList = new List<Movie>();

            foreach (var movie in localMovies)
            {
                // add to cache if it doesn't already exist
                if (!cache.Movies.Exists(m => m.IMDbID == movie.IMDBNumber && m.Title == movie.Title && m.Year == movie.Year.ToString()))
                {
                    FileLog.Info("Adding Title='{0}', Year='{1}', IMDb='{2}', TMDb='{3}' to movie trailer cache.", movie.Title, movie.Year, movie.IMDBNumber ?? "<empty>", movie.TMDBNumber ?? "<empty>");

                    movieList.Add(new Movie
                    {
                        File = movie.Path,
                        IMDbID = movie.IMDBNumber,
                        TMDbID = movie.TMDBNumber,
                        Year = movie.Year.ToString(),
                        Title = movie.Title,
                        Poster = movie.Picture,
                        Fanart = movie.Fanart                        
                    });
                }
            }

            // remove any movies from cache that are no longer in local collection
            cache.Movies.RemoveAll(c => !localMovies.Exists(l => l.IMDBNumber == c.IMDbID && l.Title == c.Title && l.Year.ToString() == c.Year));

            // add any new local movies to cache since last time 
            cache.Movies.AddRange(movieList);

            // process the movie cache and download trailers
            TrailerDownloader.ProcessAndDownloadTrailers(cache, MoviePluginSource.MyFilms);

            return;
        }

        #endregion
    }
}
