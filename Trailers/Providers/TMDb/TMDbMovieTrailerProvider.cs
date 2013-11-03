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
            else if (!string.IsNullOrEmpty(searchItem.IMDb))
            {
                searchTerm = searchItem.IMDb;
            }
            else
            {
                FileLog.Warning("Not enough information to search for trailers from themoviedb.org, require IMDb or TMDb ID.");
                return listItems;
            }

            FileLog.Debug("Searching for trailers from themoviedb.org...");
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
    }
}
