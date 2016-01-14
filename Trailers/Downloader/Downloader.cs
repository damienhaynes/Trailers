using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using MediaPortal.Configuration;
using Trailers.Downloader.DataStructures;
using Trailers.Extensions;
using Trailers.PluginHandlers;
using Trailers.Providers;
using Trailers.Providers.TMDb.API;
using Trailers.Web;

namespace Trailers.Downloader
{
    enum MoviePluginSource
    {
        MovingPictures,
        MyFilms,
        MyVideos
    }

    internal class TrailerDownloader
    {
        internal static List<IDownloader> PluginsForTrailerDownloads = new List<IDownloader>();
        
        static Timer DownloadTimer;

        internal static void Init()
        {
            if (!Utility.IsPluginAvailable("OnlineVideos"))
            {
                FileLog.Info("Aborting trailer auto-downloads as OnlineVideos is not available or minimum version not installed.");
                return;
            }
            else if (Utility.FileVersion(Utility.OnlineVideosPlugin) < new Version(2, 0, 0, 0))
            {
                FileLog.Info("Aborting trailer auto-downloads as OnlineVideos minimum version is not installed.");
                return;
            }

            // get list of enabled plugins for auto trailer download
            GetPluginsForTrailerDownloads();

            // start and initiate timer for downloading trailers from local media libraries
            DownloadTimer = new Timer(new TimerCallback((o) => { Download(); }), null, PluginSettings.AutoDownloadStartDelay, PluginSettings.AutoDownloadInterval);
        }

        static void Download()
        {
            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "AutoDownload";

            foreach (var plugin in PluginsForTrailerDownloads.Where(p => p.Enabled))
            {
                FileLog.Info("Starting auto-trailer download for '{0}'", plugin.Name);

                plugin.Download();
              
                FileLog.Info("Finished auto-trailer download for '{0}'", plugin.Name);
            }

            // disk cleanup
            Cleanup();
        }

        static void GetPluginsForTrailerDownloads()
        {
            // check if downloader is enabled and add to list of downloaders
            PluginsForTrailerDownloads.Add(new MovingPicturesHandler(PluginSettings.AutoDownloadTrailersMovingPictures));
            PluginsForTrailerDownloads.Add(new MyFilmsHandler(PluginSettings.AutoDownloadTrailersMyFilms));
            PluginsForTrailerDownloads.Add(new VideoInfoHandler(PluginSettings.AutoDownloadTrailersMyVideos));         
        }

        static void Cleanup()
        {
            if (!PluginSettings.AutoDownloadCleanup) return;

            FileLog.Info("Cleaning up trailers from disk");

            // read in all the config's
            var movPicsMovies = LoadMovieList(MoviePluginSource.MovingPictures).Movies;
            var myVideosMovies = LoadMovieList(MoviePluginSource.MyVideos).Movies;
            var myFilmsMovies = LoadMovieList(MoviePluginSource.MyFilms).Movies;

            var moviesInCache = movPicsMovies.Union(myVideosMovies).Union(myFilmsMovies);

            // cleanup
            try
            {
                // get directory listing of trailers on disk (folder names are enough)
                var directories = Directory.GetDirectories(PluginSettings.AutoDownloadDirectory);

                foreach (var directory in directories)
                {
                    // get the folder name which contains the movie info
                    var folder = directory.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }).Last();

                    // check if folder is in cache, if not, delete from disk
                    try
                    {
                        if (moviesInCache.Where(m => m.Trailers.Exists(t => t.Path != null && t.Path.Contains(folder))).Count() == 0)
                        {
                            FileLog.Info("Removing trailers from directory '{0}'", directory);
                            Directory.Delete(directory, true);
                        }
                    }
                    catch
                    {
                        FileLog.Error("Failed to remove trailers from directory '{0}'", directory);
                    }
                }
            }
            catch (Exception e)
            {
                FileLog.Error("Failed to cleanup trailers from disk, {0}", e.Message);
            }

            FileLog.Info("Finished trailer cleanup from disk.");
        }

        internal static DownloadDetail GetDownloadDetails(MovieInfo movie, Trailer trailer, Dictionary<string, string> downloadOptions)
        {
            var downloadDetails = new DownloadDetail();

            // get preferred quality for download
            var qualityOptions = GetPreferredQualityOption(downloadOptions, PluginSettings.AutoDownloadQuality);
            if (qualityOptions.Key == null)
                return null;

            // set source url for download
            downloadDetails.SourceUrl = qualityOptions.Key;

            // create local filename for download
            string folder = string.Format("{0} ({1}) [{2}]", movie.Title, movie.Year ?? string.Empty, movie.IMDbID ?? string.Empty);
            string directory = Path.Combine(PluginSettings.AutoDownloadDirectory, folder.ToCleanFileName());
            string filename = string.Format("{0}{1} [{2}]{3}.mp4", trailer.Name.ReplaceMultiSpaceWithSingleWhiteSpace(), trailer.Name.Contains(trailer.Type) ? string.Empty : " " + trailer.Type, qualityOptions.Value, PluginSettings.PreferredLanguage != "en" ? " [" + trailer.Language + "]" : string.Empty);

            downloadDetails.DestinationFilename = string.Format(@"{0}\{1}", directory, filename.ToCleanFileName());
            return downloadDetails;
        }

        internal static void ProcessAndDownloadTrailers(MovieTrailers cache, MoviePluginSource pluginSource)
        {
            // get a list of movies from the cache that require trailer searches i.e.
            // where trailer count equals zero and update interval is greater than (now - lastupdate)
            // typically this will be for any new movies added to library
            int onlineOps = 0;
            int saveThreshold = 10;

            var moviesWithoutTrailers = cache.Movies.Where(m => m.Trailers.Count == 0);
            foreach (var movie in moviesWithoutTrailers)
            {
                // check the date last processed is longer than max period for update (default 7 days)
                DateTime lastUpdate = DateTime.MinValue;
                DateTime.TryParse(movie.UpdateTime, out lastUpdate);
                if (DateTime.Now.Subtract(new TimeSpan(PluginSettings.AutoDownloadUpdateInterval, 0, 0, 0)) < lastUpdate)
                    continue;

                // update the last update time
                movie.UpdateTime = DateTime.Now.ToString();

                // get the search term for trailer search
                // if there is no 'id' we will get one from a movie lookup
                FileLog.Info("Searching for trailers from themoviedb.org, Title: '{0}', Year: '{1}', IMDb: '{2}', TMDb: '{3}'", movie.Title, movie.Year, movie.IMDbID ?? "<empty>", movie.TMDbID ?? "<empty>");

                string searchTerm = TMDbTrailerProvider.GetMovieSearchTerm(movie.IMDbID, movie.TMDbID, movie.Title, movie.Year);
                if (searchTerm == null) continue;

                // search for trailers
                var trailers = TMDbAPI.GetMovieTrailers(searchTerm, PluginSettings.PreferredLanguage, PluginSettings.FallbackToEnglishLanguage, PluginSettings.AlwaysGetEnglishTrailers);
                if (trailers == null || trailers.Results == null || trailers.Results.Count == 0)
                    continue;

                // save the list of trailers to the cached movie
                var trailersFound = new List<Trailer>();
                foreach (var trailer in trailers.Results)
                {
                    FileLog.Info("Found Youtube video, Name: '{0}', Quality: '{1}', Source: '{2}', Type: '{3}'", trailer.Name, trailer.Size, trailer.Key, trailer.Type);

                    trailersFound.Add(new Trailer
                    {
                        Name = trailer.Name,
                        Quality = trailer.Size,
                        Source = trailer.Key,
                        Type = trailer.Type,
                        Language = trailer.LanguageCode,
                        IsValid = true
                    });

                    onlineOps++;
                }

                // persist trailers found and update time
                movie.Trailers = trailersFound;

                // save cache semi-regularly in case we shutdown/enter stand-by
                if (onlineOps >= saveThreshold)
                {
                    TrailerDownloader.SaveMovieList(pluginSource, cache);
                    saveThreshold += 10;
                }
            }

            // now download trailers that have not been processed yet i.e. don't have a physical file path and are valid
            moviesWithoutTrailers = cache.Movies.Where(m => m.Trailers.Count(t => string.IsNullOrEmpty(t.Path) && t.IsValid) > 0);

            onlineOps = 0;
            saveThreshold = 10;
            foreach (var movie in moviesWithoutTrailers)
            {
                FileLog.Info("Checking for trailer downloads, Title: {0}, Year: {1}, IMDb: {2}, TMDb: {3}", movie.Title, movie.Year, movie.IMDbID ?? "<empty>", movie.TMDbID ?? "<empty>");

                // only process valid trailers
                foreach (var trailer in movie.Trailers.Where(t => t.IsValid))
                {
                    // if the file already exists skip
                    if (!string.IsNullOrEmpty(trailer.Path))
                    {
                        if (File.Exists(trailer.Path))
                        {
                            FileLog.Info("Skipping trailer '{0}' download for '{1}', file already exists", trailer.Name, trailer.Path);
                            continue;
                        }
                    }

                    // check user wants video
                    if (!CheckAllowedTrailerTypes(trailer))
                    {
                        FileLog.Info("Skipping trailer download for, Name='{0}', Type='{1}', Reason='Unwanted Type'", trailer.Name, trailer.Type);
                        continue;
                    }

                    FileLog.Info("Getting download options for video, Name: {0}, Quality: {1}, Source: {2}, Type: {3}", trailer.Name, trailer.Quality, trailer.Source, trailer.Type);
                    var options = OnlineVideosHandler.GetPlaybackOptionsFromYoutubeUrl(trailer.Source);
                    if (options == null)
                    {
                        FileLog.Warning("Unable to get download options for trailer, marking as invalid");
                        trailer.IsValid = false;
                        continue;
                    }

                    // get the preferred (best) video codec and type                    
                    var downloadDetails = TrailerDownloader.GetDownloadDetails(movie, trailer, options);
                    if (downloadDetails == null)
                    {
                        FileLog.Warning("Could not find any matching resolution or lower for download, skipping trailer download");
                        continue;
                    }

                    // download trailer
                    trailer.IsValid = WebUtils.DownloadFile(downloadDetails.SourceUrl, downloadDetails.DestinationFilename);
                    trailer.Path = downloadDetails.DestinationFilename;

                    onlineOps++;
                }

                // save cache semi-regularly in case we shutdown/enter stand-by
                if (onlineOps >= saveThreshold)
                {
                    TrailerDownloader.SaveMovieList(pluginSource, cache);
                    saveThreshold += 10;
                }
            }

            // save cache
            TrailerDownloader.SaveMovieList(pluginSource, cache);
        }

        /// <summary>
        /// checks if the trailer is wanted by the user
        /// </summary>
        /// <param name="trailer"></param>
        /// <returns></returns>
        static bool CheckAllowedTrailerTypes(Trailer trailer)
        {
            bool returnVal = true;

            switch (trailer.Type)
            {
                case "Trailer":
                    returnVal = PluginSettings.AutoDownloadTrailers;
                    break;

                case "Teaser":
                    returnVal = PluginSettings.AutoDownloadTeasers;
                    break;

                case "Featurette":
                    returnVal = PluginSettings.AutoDownloadFeaturettes;
                    break;

                case "Clip":
                    returnVal = PluginSettings.AutoDownloadClips;
                    break;
            }

            return returnVal;
        }

        /// <summary>
        /// returns the highest quality download options for a given preferred quality
        /// </summary>
        static KeyValuePair<string, string> GetPreferredQualityOption(Dictionary<string, string> downloadOptions, string preferredQuality)
        {
            // available mp4 options are:
            // 1. 320x240
            // 2. 426x240
            // 3. 640x360
            // 4. 1280x720

            if (preferredQuality == "HD")
            {
                var options = downloadOptions.Where(o => o.Key.Contains("1280x720 | mp4"));
                if (options.Count() > 0)
                {
                    // get the low audio bitrate version as the high bit rate seems to fail
                    return new KeyValuePair<string, string>(options.First().Value, "HD");
                }
                else
                {
                    // no HD available, check next highest quality
                    return GetPreferredQualityOption(downloadOptions, "HQ");
                }
            }
            else if (preferredQuality == "HQ")
            {
                // get the next highest quality
                var options = downloadOptions.Where(o => o.Key.Contains("640x360 | mp4"));
                if (options.Count() > 0)
                {
                    // get the low audio bitrate version as the high bit rate seems to fail
                    return new KeyValuePair<string, string>(options.First().Value, "HQ");
                }
                else
                {
                    // no HQ available, check next highest quality
                    return GetPreferredQualityOption(downloadOptions, "LQ");
                }
            }
            else if (preferredQuality == "LQ")
            {
                var options = downloadOptions.Where(o => o.Key.Contains("426x240 | mp4"));
                if (options.Count() > 0)
                {
                    return new KeyValuePair<string, string>(options.First().Value, "LQ");
                }
                else
                {
                    options = downloadOptions.Where(o => o.Key.Contains("320x240 | mp4"));
                    if (options.Count() > 0)
                    {
                        return new KeyValuePair<string, string>(options.First().Value, "LQ");
                    }
                }
            }

            // no mp4 videos available
            return new KeyValuePair<string, string>(null, null);
        }

        /// <summary>
        /// loads the list of trailers already processed for a plugin's media library
        /// </summary>
        /// <param name="source">plugin source</param>
        internal static MovieTrailers LoadMovieList(MoviePluginSource source)
        {
            // get the cached file from disk
            string cacheFile = Config.GetFile(Config.Dir.Config, "Trailers", source.ToString() + ".xml");

            // load cached file from disk
            string memFile = LoadFile(cacheFile);
            if (memFile == null) 
                return new MovieTrailers();
            
            // de-serialize the response into a MovieTrailers object            
            var movieTrailers = memFile.FromXML<MovieTrailers>();
            if (movieTrailers == null)
                return new MovieTrailers();

            return movieTrailers;
        }

        static string LoadFile(string filename)
        {
            if (!File.Exists(filename)) return null;

            FileLog.Info("Loading '{0}' movie trailer cache from disk.", filename);

            try
            {
                return File.ReadAllText(filename);
            }
            catch (Exception e)
            {
                FileLog.Error("Error loading cache file: {0}', {1}",filename, e.Message);
                return null;
            }
        }

        internal static void SaveMovieList(MoviePluginSource source, MovieTrailers movies)
        {
            string cacheFile = Config.GetFile(Config.Dir.Config, "Trailers", source.ToString() + ".xml");
            SaveFile(cacheFile, movies.ToXML<MovieTrailers>());
        }

        static void SaveFile(string filename, string data)
        {
            if (data == null) return;

            FileLog.Debug("Saving movie/trailer cache '{0}' to disk.", filename);

            try
            {
                string dir = Path.GetDirectoryName(filename);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText(filename, data);
            }
            catch (Exception e)
            {
                FileLog.Error("Error saving cache '{0}', {1}", filename, e.Message);
                return;
            }
        }
    }
}
