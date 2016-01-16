using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MediaPortal.GUI.Library;
using MediaPortal.Video.Database;
using Trailers.Downloader;
using Trailers.Downloader.DataStructures;
using Trailers.Providers;

namespace Trailers.PluginHandlers
{
    class VideoInfoHandler : IDownloader
    {
        public static bool GetCurrentMediaItem(out MediaItem currentMediaItem)
        {
            FileLog.Info("Getting selected movie information from MyVideos");

            currentMediaItem = new MediaItem();
            currentMediaItem.Title = GUIPropertyManager.GetProperty("#title").Trim();

            int year;
            var strYear = GUIPropertyManager.GetProperty("#year").Trim();
            if (int.TryParse(strYear, out year))
                currentMediaItem.Year = year;

            // Get IMDb ID
            string imdbid = GUIPropertyManager.GetProperty("#imdbnumber").Trim();
            if (!string.IsNullOrEmpty(imdbid) && imdbid.Length == 9)
                currentMediaItem.IMDb = imdbid;

            currentMediaItem.Plot = GUIPropertyManager.GetProperty("#plot").Trim();
            currentMediaItem.Poster = GUIPropertyManager.GetProperty("#thumb").Trim();

            // Get Local File Info
            currentMediaItem.FullPath = GUIPropertyManager.GetProperty("#file").Trim();

            // At the very least we should have a file
            if (string.IsNullOrEmpty(currentMediaItem.FullPath))
            {
                // try get the selected item on the facade
                if (SelectedItem != null && !SelectedItem.IsFolder)
                {
                    currentMediaItem.FullPath = SelectedItem.Path;
                }
            }

            return true;
        }

        static GUIFacadeControl Facade
        {
            get
            {
                // Get the current window
                var window = GUIWindowManager.GetWindow(GUIWindowManager.ActiveWindow);
                if (window == null) return null;

                // Get the Facade control
                return window.GetControl(50) as GUIFacadeControl;
            }
        }

        static GUIListItem SelectedItem
        {
            get
            {
                if (Facade == null) return null;

                // Get the Selected Item
                return Facade.SelectedListItem;
            }
        }

        #region IDownloader Members

        public VideoInfoHandler(bool enabled)
        {
            FileLog.Info("Loading Auto-Downloader: '{0}', Enabled: '{1}'", MoviePluginSource.MyVideos, enabled);

            this.Enabled = enabled;
        }

        public bool Enabled { get; set; }

        public string Name
        {
            get { return MoviePluginSource.MyVideos.ToString(); }
        }

        public void Download()
        {
            // get all movies in database
            ArrayList myvideos = new ArrayList();
            VideoDatabase.GetMovies(ref myvideos);

            var localMovies = (from IMDBMovie movie in myvideos select movie).ToList();

            FileLog.Info("{0} movies in My Videos database", localMovies.Count);

            // get locally cached trailers
            var cache = TrailerDownloader.LoadMovieList(MoviePluginSource.MyVideos);

            // process local movies
            var movieList = new List<Movie>();

            foreach (var movie in localMovies)
            {
                // add to cache if it doesn't already exist
                if (!cache.Movies.Exists(m => m.IMDbID == movie.IMDBNumber && m.Title == movie.Title && m.Year == movie.Year.ToString()))
                {
                    string fanart = string.Empty;
                    MediaPortal.Util.FanArt.GetFanArtfilename(movie.ID, 0, out fanart);

                    string poster = MediaPortal.Util.Utils.GetLargeCoverArtName(MediaPortal.Util.Thumbs.MovieTitle, movie.Title + "{" + movie.ID + "}");

                    FileLog.Info("Adding Title='{0}', Year='{1}', IMDb='{2}', TMDb='{3}' to movie trailer cache.", movie.Title, movie.Year, movie.IMDBNumber ?? "<empty>", "<empty>");

                    movieList.Add(new Movie
                    {
                        File = movie.VideoFileName,
                        IMDbID = movie.IMDBNumber,
                        Year = movie.Year.ToString(),
                        Title = movie.Title,
                        Cast = Regex.Replace(movie.Cast, @"\n|\r", "|"),
                        Directors = movie.Director,
                        Writers = movie.WritingCredits.Replace(" / ", "|"),
                        Genres = movie.Genre.Replace(" / ", "|"),
                        Plot = movie.Plot,
                        Fanart = fanart,
                        Poster = poster
                    });
                }
            }

            // remove any movies from cache that are no longer in local collection
            cache.Movies.RemoveAll(c => !localMovies.Exists(l => l.IMDBNumber == c.IMDbID && l.Title == c.Title && l.Year.ToString() == c.Year));

            // add any new local movies to cache since last time 
            cache.Movies.AddRange(movieList);

            // process the movie cache and download trailers
            TrailerDownloader.ProcessAndDownloadTrailers(cache, MoviePluginSource.MyVideos);

            return;
        }

        #endregion
    }
}
