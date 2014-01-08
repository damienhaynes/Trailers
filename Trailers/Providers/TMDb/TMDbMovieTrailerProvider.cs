using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trailers.GUI;
using Trailers.Providers.TMDb.API;
using Trailers.Web;

namespace Trailers.Providers
{
    class TMDbMovieTrailerProvider : IProvider
    {
        #region Constructor

        public TMDbMovieTrailerProvider(bool enableProvider)
        {
            Enabled = enableProvider;
        }

        #endregion

        #region IProvider Members

        public bool Enabled { get; set; }

        public string Name
        {
            get { return "TMDb Movie Trailer Provider"; }
        }

        public bool IsLocal
        {
            get { return false; }
        }

        public List<GUITrailerListItem> Search(MediaItem searchItem)
        {
            string searchTerm = string.Empty;
            var listItems = new List<GUITrailerListItem>();

            if (!string.IsNullOrEmpty(searchItem.TMDb))
            {
                searchTerm = searchItem.TMDb;
            }
            else if (!string.IsNullOrEmpty((searchItem.IMDb ?? string.Empty).Trim()))
            {
                searchTerm = searchItem.IMDb;
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
                    foreach(var movie in searchResults.Results)
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

            FileLog.Debug("Searching for trailers using search term '{0}' from themoviedb.org...", searchTerm);
            var trailers = TMDbAPI.GetMovieTrailers(searchTerm);
            if (trailers == null || trailers.YouTubeTrailers == null)
            {
                FileLog.Error("Error getting trailers from themoviedb.org.");
                return listItems;
            }

            // TMDb currently only supports YouTube videos
            // Note: there is open ticket for 'type' field not available in the API

            foreach(var trailer in trailers.YouTubeTrailers)
            {
                var listItem = new GUITrailerListItem();

                string itemName = string.IsNullOrEmpty(trailer.Type) || trailer.Name.ToLowerInvariant().Contains(trailer.Type.ToLowerInvariant()) ? trailer.Name : string.Format("{0} - {1}", trailer.Name, trailer.Type);
                itemName = string.IsNullOrEmpty(trailer.Size) || itemName.ToLowerInvariant().Contains(trailer.Size.ToLowerInvariant()) ? itemName : string.Format("{0} ({1})", itemName, trailer.Size);

                listItem.Label = itemName;
                listItem.Label2 = Localisation.Translation.Online;
                listItem.URL = WebUtils.GetYouTubeURL(trailer.Source);
                listItem.TVTag = trailer;
                listItem.IsOnlineItem = true;
                listItem.CurrentMedia = searchItem;

                listItems.Add(listItem);
            }

            FileLog.Info("Found {0} trailer(s) from themoviedb.org", trailers.YouTubeTrailers.Count.ToString());

            return listItems;
        }

        #endregion

        #region Public Methods

        public static string GetSearchTerm(string imdbid, string tmdbid, string title, string year)
        {
            string searchTerm = null;

            if (!string.IsNullOrEmpty(tmdbid))
            {
                searchTerm = tmdbid;
            }
            else if (!string.IsNullOrEmpty((imdbid ?? string.Empty).Trim()))
            {
                searchTerm = imdbid;
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
