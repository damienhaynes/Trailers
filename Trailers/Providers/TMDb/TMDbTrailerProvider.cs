using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trailers.GUI;
using Trailers.Providers.TMDb.API;
using Trailers.Web;

namespace Trailers.Providers
{
    class TMDbTrailerProvider : IProvider
    {
        #region Constructor

        public TMDbTrailerProvider(bool enableProvider)
        {
            Enabled = enableProvider;
        }

        #endregion

        #region IProvider Members

        public bool Enabled { get; set; }

        public string Name
        {
            get { return "TMDb Trailer Provider"; }
        }

        public bool IsLocal
        {
            get { return false; }
        }

        public List<GUITrailerListItem> Search(MediaItem searchItem)
        {
            List<GUITrailerListItem> listItems = new List<GUITrailerListItem>();

            switch (searchItem.MediaType)
            {
                case MediaItemType.Movie:
                    listItems = SearchMovieTrailers(searchItem);
                    break;

                case MediaItemType.Show:
                case MediaItemType.Season:
                case MediaItemType.Episode:
                    listItems = SearchShowTrailers(searchItem);
                    break;
            }

            return listItems;
        }

        #endregion

        #region Private Methods

        #region Search for Movie Trailers

        private List<GUITrailerListItem> SearchMovieTrailers(MediaItem searchItem)
        {
            string searchTerm = string.Empty;
            var listItems = new List<GUITrailerListItem>();

            if (!string.IsNullOrEmpty(searchItem.TMDb))
            {
                searchTerm = searchItem.TMDb;
            }
            else if (!string.IsNullOrEmpty((searchItem.IMDb ?? string.Empty).Trim()))
            {
                var externalId = PluginSettings.IMDbIds.FirstOrDefault(t => t.ExternalId == searchItem.IMDb);
                if (externalId == null)
                {
                    // use the find method to lookup by external source id
                    FileLog.Debug("Searching themoviedb.org for all objects with IMDb ID '{0}'", searchItem.IMDb);

                    var findResults = TMDbAPI.TMDbFind(searchItem.IMDb, TMDbAPI.ExternalSource.imdb_id);
                    if (findResults == null || findResults.Movies == null || findResults.Movies.Count == 0)
                    {
                        FileLog.Warning("Not enough information to search for trailers from themoviedb.org, no matches found using IMDb ID");
                        return listItems;
                    }

                    // there should only be one
                    searchTerm = findResults.Movies.First().Id.ToString();

                    // cache id so we can use it again
                    PluginSettings.IMDbIds.Add(new PluginSettings.ExternalID { ExternalId = searchItem.IMDb, TmdbId = findResults.Movies.First().Id });
                }
                else
                {
                    FileLog.Debug("Found cached TMDb ID '{0}' for IMDb External ID: '{1}'", externalId.TmdbId.ToString(), searchItem.IMDb);
                    searchTerm = externalId.TmdbId.ToString();
                }
            }
            else if (!string.IsNullOrEmpty(searchItem.Title))
            {
                // we do the best we can with out any proper movie id's
                FileLog.Debug("Searching themoviedb.org for movie: Title: '{0}', Year: '{1}'", searchItem.Title, searchItem.Year);

                var searchResults = TMDbAPI.SearchMovies(searchItem.Title, language: "en", year: searchItem.Year <= 1900 ? null : searchItem.Year.ToString());
                if (searchResults == null || searchResults.TotalResults == 0)
                {
                    FileLog.Warning("No movies found, skipping search from the themoviedb.org.");
                    return listItems;
                }
                else
                {
                    foreach (var movie in searchResults.Results)
                    {
                        FileLog.Debug("Found movie: Title: '{0}', Original Title: '{1}', Release Date: '{2}', TMDb: '{3}', Popularity: '{4}'", movie.Title, movie.OriginalTitle, movie.ReleaseDate, movie.Id, movie.Popularity);
                    }
                }

                // get the movie id of the first result, this would be the most likely match (based on popularity)
                // we can think about providing a menu of choices as well based on demand
                searchTerm = searchResults.Results.First().Id.ToString();
                searchItem.TMDb = searchTerm;
            }
            else
            {
                FileLog.Warning("Not enough information to search for trailers from themoviedb.org, require IMDb ID, TMDb ID or Title+Year.");
                return listItems;
            }

            FileLog.Debug("Searching for movie trailers using search term '{0}' from themoviedb.org...", searchTerm);
            var trailers = TMDbAPI.GetMovieTrailers(searchTerm, PluginSettings.PreferredLanguage, PluginSettings.FallbackToEnglishLanguage, PluginSettings.AlwaysGetEnglishTrailers);
            if (trailers == null || trailers.Results == null)
            {
                FileLog.Error("Error getting movie trailers from themoviedb.org.");
                return listItems;
            }

            foreach (var trailer in trailers.Results)
            {
                var listItem = new GUITrailerListItem();

                string itemName = string.IsNullOrEmpty(trailer.Type) || trailer.Name.ToLowerInvariant().Contains(trailer.Type.ToLowerInvariant()) ? trailer.Name : string.Format("{0} - {1}", trailer.Name, trailer.Type);
                itemName = string.IsNullOrEmpty(trailer.Size) || itemName.ToLowerInvariant().Contains(trailer.Size.ToLowerInvariant()) ? itemName : string.Format("{0} ({1})", itemName, trailer.Size);
                if (PluginSettings.PreferredLanguage != "en")
                {
                    itemName = string.Format("{0} [{1}]", itemName, trailer.LanguageCode);
                }

                listItem.Label = itemName;
                listItem.Label2 = Localisation.Translation.Online;
                listItem.URL = WebUtils.GetYouTubeURL(trailer.Key);
                listItem.TVTag = trailer;
                listItem.IsOnlineItem = true;
                listItem.CurrentMedia = searchItem;

                listItems.Add(listItem);
            }

            FileLog.Info("Found {0} movie trailer(s) from themoviedb.org", trailers.Results.Count.ToString());

            return listItems;
        }

        #endregion

        #region Search for Show Trailers

        private List<GUITrailerListItem> SearchShowTrailers(MediaItem searchItem)
        {
            string searchTerm = string.Empty;
            var listItems = new List<GUITrailerListItem>();

            if (!string.IsNullOrEmpty(searchItem.TMDb))
            {
                searchTerm = searchItem.TMDb;
            }
            else if (!string.IsNullOrEmpty(searchItem.TVDb))
            {
                // check if external id is cached
                var externalId = PluginSettings.TVDbIds.FirstOrDefault(t => t.ExternalId == searchItem.TVDb);
                if (externalId == null)
                {

                    // use the find method to lookup by external source id
                    FileLog.Debug("Searching themoviedb.org for all objects with TVDb ID '{0}'", searchItem.TVDb);

                    var findResults = TMDbAPI.TMDbFind(searchItem.TVDb, TMDbAPI.ExternalSource.tvdb_id);
                    if (findResults == null || findResults.Shows == null || findResults.Shows.Count == 0)
                    {
                        FileLog.Warning("Not enough information to search for trailers from themoviedb.org, no matches found using TVDb ID");
                        return listItems;
                    }

                    // there should only be one
                    searchTerm = findResults.Shows.First().Id.ToString();

                    // cache id so we can use it again
                    PluginSettings.TVDbIds.Add(new PluginSettings.ExternalID { ExternalId = searchItem.TVDb, TmdbId = findResults.Shows.First().Id });
                }
                else
                {
                    FileLog.Debug("Found cached TMDb ID '{0}' for TVDb External ID: '{1}'", externalId.TmdbId.ToString(), searchItem.TVDb);
                    searchTerm = externalId.TmdbId.ToString();
                }
            }
            else if (!string.IsNullOrEmpty(searchItem.IMDb))
            {
                // check if external id is cached
                var externalId = PluginSettings.IMDbIds.FirstOrDefault(t => t.ExternalId == searchItem.IMDb);
                if (externalId == null)
                {
                    FileLog.Debug("Searching themoviedb.org for all objects with IMDb ID '{0}'", searchItem.IMDb);

                    var findResults = TMDbAPI.TMDbFind(searchItem.IMDb, TMDbAPI.ExternalSource.imdb_id);
                    if (findResults == null || findResults.Shows == null || findResults.Shows.Count == 0)
                    {
                        FileLog.Warning("Not enough information to search for trailers from themoviedb.org, no matches found using IMDb ID");
                        return listItems;
                    }

                    // there should only be one
                    searchTerm = findResults.Shows.First().Id.ToString();

                    // cache id so we can use it again
                    PluginSettings.IMDbIds.Add(new PluginSettings.ExternalID { ExternalId = searchItem.IMDb, TmdbId = findResults.Shows.First().Id });
                }
                else
                {
                    FileLog.Debug("Found cached TMDb ID '{0}' for IMDb External ID: '{1}'", externalId.TmdbId.ToString(), searchItem.IMDb);
                    searchTerm = externalId.TmdbId.ToString();
                }
            }
            else if (!string.IsNullOrEmpty(searchItem.TVRage))
            {
                // check if external id is cached
                var externalId = PluginSettings.TVRageIds.FirstOrDefault(t => t.ExternalId == searchItem.TVRage);
                if (externalId == null)
                {
                    FileLog.Debug("Searching themoviedb.org for all objects with TVRage ID '{0}'", searchItem.TVRage);

                    var findResults = TMDbAPI.TMDbFind(searchItem.TVRage, TMDbAPI.ExternalSource.tvrage_id);
                    if (findResults == null || findResults.Shows == null || findResults.Shows.Count == 0)
                    {
                        FileLog.Warning("Not enough information to search for trailers from themoviedb.org, no matches found using TVRage ID");
                        return listItems;
                    }

                    // there should only be one
                    searchTerm = findResults.Shows.First().Id.ToString();

                    // cache id so we can use it again
                    PluginSettings.TVRageIds.Add(new PluginSettings.ExternalID { ExternalId = searchItem.TVRage, TmdbId = findResults.Shows.First().Id });
                }
                else
                {
                    FileLog.Debug("Found cached TMDb ID '{0}' for TVRage External ID: '{1}'", externalId.TmdbId.ToString(), searchItem.TVRage);
                    searchTerm = externalId.TmdbId.ToString();
                }
            }
            else if (!string.IsNullOrEmpty(searchItem.Title))
            {
                // we do the best we can with out any proper show id's
                FileLog.Debug("Searching themoviedb.org for show: Title: '{0}', FirstAired: '{1}'", searchItem.Title, searchItem.AirDate ?? "<empty>");

                var searchResults = TMDbAPI.SearchShows(searchItem.Title, language: "en", firstAirDate: string.IsNullOrEmpty(searchItem.AirDate) ? null : searchItem.AirDate);
                if (searchResults == null || searchResults.TotalResults == 0)
                {
                    FileLog.Warning("No shows found, skipping search from the themoviedb.org.");
                    return listItems;
                }
                else
                {
                    foreach (var show in searchResults.Results)
                    {
                        FileLog.Debug("Found show: Name: '{0}', Original Name: '{1}', Air Date: '{2}', TMDb: '{3}', Popularity: '{4}'", show.Name, show.OriginalName, show.FirstAirDate, show.Id, show.Popularity);
                    }
                }

                // get the show id of the first result, this would be the most likely match (based on popularity)
                // we can think about providing a menu of choices as well based on demand
                searchTerm = searchResults.Results.First().Id.ToString();
                searchItem.TMDb = searchTerm;
            }
            else
            {
                FileLog.Warning("Not enough information to search for trailers from themoviedb.org, require IMDb, TVDb, TMDb or TVRage ID or Title+Year.");
                return listItems;
            }

            FileLog.Debug("Searching for tv {0} trailers using search term '{1}' from themoviedb.org...", searchItem.MediaType.ToString().ToLower(), searchTerm );
            TMDb.DataStructures.TMDbTrailers trailers = null;

            // search for correct type
            switch (searchItem.MediaType)
            {
                case MediaItemType.Show:
                    trailers = TMDbAPI.GetShowTrailers(searchTerm, PluginSettings.PreferredLanguage, PluginSettings.FallbackToEnglishLanguage, PluginSettings.AlwaysGetEnglishTrailers);
                    break;
                case MediaItemType.Season:
                    trailers = TMDbAPI.GetSeasonTrailers(searchTerm, searchItem.Season.ToString(), PluginSettings.PreferredLanguage, PluginSettings.FallbackToEnglishLanguage, PluginSettings.AlwaysGetEnglishTrailers);
                    break;
                case MediaItemType.Episode:
                    trailers = TMDbAPI.GetEpisodeTrailers(searchTerm, searchItem.Season.ToString(), searchItem.Episode.ToString(), PluginSettings.PreferredLanguage, PluginSettings.FallbackToEnglishLanguage, PluginSettings.AlwaysGetEnglishTrailers);
                    break;
            }
            
            if (trailers == null || trailers.Results == null)
            {
                FileLog.Error("Error getting trailers from themoviedb.org.");
                return listItems;
            }

            foreach (var trailer in trailers.Results)
            {
                var listItem = new GUITrailerListItem();

                string itemName = string.IsNullOrEmpty(trailer.Type) || trailer.Name.ToLowerInvariant().Contains(trailer.Type.ToLowerInvariant()) ? trailer.Name : string.Format("{0} - {1}", trailer.Name, trailer.Type);
                itemName = string.IsNullOrEmpty(trailer.Size) || itemName.ToLowerInvariant().Contains(trailer.Size.ToLowerInvariant()) ? itemName : string.Format("{0} ({1})", itemName, trailer.Size);
                if (PluginSettings.PreferredLanguage != "en")
                {
                    itemName = string.Format("{0} [{1}]", itemName, trailer.LanguageCode);
                }

                listItem.Label = itemName;
                listItem.Label2 = Localisation.Translation.Online;
                listItem.URL = WebUtils.GetYouTubeURL(trailer.Key);
                listItem.TVTag = trailer;
                listItem.IsOnlineItem = true;
                listItem.CurrentMedia = searchItem;

                listItems.Add(listItem);
            }

            FileLog.Info("Found {0} tv {1} trailer(s) from themoviedb.org", trailers.Results.Count.ToString(), searchItem.MediaType.ToString().ToLower());

            return listItems;
        }
        #endregion

        #endregion

        #region Public Methods

        public static string GetMovieSearchTerm(string imdbid, string tmdbid, string title, string year)
        {
            string searchTerm = null;

            if (!string.IsNullOrEmpty(tmdbid))
            {
                searchTerm = tmdbid;
            }
            else if (!string.IsNullOrEmpty((imdbid ?? string.Empty).Trim()))
            {
                var externalId = PluginSettings.IMDbIds.FirstOrDefault(t => t.ExternalId == imdbid);
                if (externalId == null)
                {
                    // use the find method to lookup by external source id
                    FileLog.Debug("Searching themoviedb.org for all objects with IMDb ID '{0}'", imdbid);

                    var findResults = TMDbAPI.TMDbFind(imdbid, TMDbAPI.ExternalSource.imdb_id);
                    if (findResults == null || findResults.Movies == null || findResults.Movies.Count == 0)
                    {
                        FileLog.Warning("Not enough information to search for trailers from themoviedb.org, no matches found using IMDb ID");
                        return searchTerm;
                    }

                    // there should only be one
                    searchTerm = findResults.Movies.First().Id.ToString();

                    // cache id so we can use it again
                    PluginSettings.IMDbIds.Add(new PluginSettings.ExternalID { ExternalId = imdbid, TmdbId = findResults.Movies.First().Id });
                }
                else
                {
                    FileLog.Debug("Found cached TMDb ID '{0}' for IMDb External ID: '{1}'", externalId.TmdbId.ToString(), imdbid);
                    searchTerm = externalId.TmdbId.ToString();
                }

            }
            else if (!string.IsNullOrEmpty(title))
            {
                // we do the best we can with out any proper movie id's
                FileLog.Debug("Searching themoviedb.org for movie: Title: '{0}', Year: '{1}'", title, year);
                
                int iYear = 0;
                int.TryParse(year, out iYear);

                var searchResults = TMDbAPI.SearchMovies(title, language: "en", year: iYear <= 1900 ? null : year);
                if (searchResults == null || searchResults.TotalResults == 0)
                {
                    FileLog.Warning("No movies found, skipping search from the themoviedb.org.");
                    return searchTerm;
                }
                else
                {
                    foreach (var movie in searchResults.Results)
                    {
                        FileLog.Debug("Found movie: Title: '{0}', Original Title: '{1}', Release Date: '{2}', TMDb: '{3}', Popularity: '{4}'", movie.Title, movie.OriginalTitle, movie.ReleaseDate, movie.Id, movie.Popularity);
                    }
                }

                // get the movie id of the first result, this would be the most likely match (based on popularity)
                // we can think about providing a menu of choices as well based on demand
                searchTerm = searchResults.Results.First().Id.ToString();
            }
            else
            {
                FileLog.Warning("Not enough information to search for trailers from themoviedb.org, require IMDb ID, TMDb ID or Title+Year.");
            }

            return searchTerm;
        }

        #endregion
    }
}
