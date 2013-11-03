using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trailers.GUI;
using Trailers.Localisation;
using Trailers.Providers;

namespace Trailers.Providers
{
    class OnlineVideoSearchProvider : IProvider
    {
        #region Constructor

        public OnlineVideoSearchProvider(bool enableProvider)
        {
            Enabled = enableProvider;
        }

        #endregion

        #region IProvider Members

        public bool Enabled { get; set; }

        public string Name
        {
            get { return "OnlineVideos Trailer Search Provider"; }
        }

        public bool IsLocal
        {
            get { return false; }
        }

        public List<GUITrailerListItem> Search(MediaItem searchItem)
        {
            var listItems = new List<GUITrailerListItem>();

            // Add all trailer search sites available from OnlineVideos
            // Youtube, iTunes and IMDb Trailers
            var listItem = new GUITrailerListItem();

            if (PluginSettings.OnlineVideosYouTubeEnabled)
            {
                listItem.Label = Translation.YouTubeTrailers;
                listItem.Label2 = Translation.Search;
                listItem.URL = string.Format("site:YouTube|search:{0}|return:Locked", GetYouTubeSearchString(searchItem));
                listItem.IsSearchItem = true;
                listItem.CurrentMedia = searchItem;
                listItems.Add(listItem);
            }

            if (PluginSettings.OnlineVideosITunesEnabled)
            {
                listItem = new GUITrailerListItem();
                listItem.Label = Translation.ITunesTrailers;
                listItem.Label2 = Translation.Search;
                listItem.URL = string.Format("site:iTunes Movie Trailers|search:{0}|return:Locked", searchItem.Title);
                listItem.IsSearchItem = true;
                listItem.CurrentMedia = searchItem;
                listItems.Add(listItem);
            }

            if (PluginSettings.OnlineVideosIMDbEnabled)
            {
                listItem = new GUITrailerListItem();
                listItem.Label = Translation.IMDbTrailers;
                listItem.Label2 = Translation.Search;
                listItem.URL = string.Format("site:IMDb Movie Trailers|search:{0}|return:Locked", !string.IsNullOrEmpty(searchItem.IMDb) ? searchItem.IMDb : searchItem.Title);
                listItem.IsSearchItem = true;
                listItem.CurrentMedia = searchItem;
                listItems.Add(listItem);
            }

            return listItems;
        }

        #endregion

        #region Private Methods

        private string GetYouTubeSearchString(MediaItem item)
        {
            string youTubeSearchStr = PluginSettings.OnlineVideosYouTubeSearchString;

            // replace placeholders with actual values
            // only title and year are useful for youtube
            youTubeSearchStr = youTubeSearchStr.Replace("%title%", item.Title);
            youTubeSearchStr = youTubeSearchStr.Replace("%year%", item.Year.ToString());
            
            return youTubeSearchStr;
        }

        #endregion
    }
}
