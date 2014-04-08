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

            if (PluginSettings.OnlineVideosYouTubeEnabled && !string.IsNullOrEmpty(searchItem.Title))
            {
                listItem.Label = Translation.YouTubeTrailers;
                listItem.Label2 = Translation.Search;
                listItem.URL = string.Format("site:YouTube|search:{0}|return:Locked", GetYouTubeSearchString(searchItem));
                listItem.IsSearchItem = true;
                listItem.CurrentMedia = searchItem;
                listItems.Add(listItem);
            }

            // iTunes only supports movies
            if (PluginSettings.OnlineVideosITunesEnabled && !string.IsNullOrEmpty(searchItem.Title) && searchItem.MediaType == MediaItemType.Movie)
            {
                listItem = new GUITrailerListItem();
                listItem.Label = Translation.ITunesTrailers;
                listItem.Label2 = Translation.Search;
                listItem.URL = string.Format("site:iTunes Movie Trailers|search:{0}|return:Locked", searchItem.Title);
                listItem.IsSearchItem = true;
                listItem.CurrentMedia = searchItem;
                listItems.Add(listItem);
            }

            // IMDb only supports movies and shows
            if (PluginSettings.OnlineVideosIMDbEnabled && !string.IsNullOrEmpty(searchItem.Title) && (searchItem.MediaType == MediaItemType.Movie || searchItem.MediaType == MediaItemType.Show))
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
            string youTubeSearchStr = string.Empty;

            switch (item.MediaType)
            {
                case MediaItemType.Movie:
                    youTubeSearchStr = PluginSettings.OnlineVideosYouTubeMovieSearchString;
                    break;
                case MediaItemType.Show:
                    youTubeSearchStr = PluginSettings.OnlineVideosYouTubeShowSearchString;
                    break;
                case MediaItemType.Season:
                    youTubeSearchStr = PluginSettings.OnlineVideosYouTubeSeasonSearchString;
                    break;
                case MediaItemType.Episode:
                    youTubeSearchStr = PluginSettings.OnlineVideosYouTubeEpisodeSearchString;
                    break;
            }
            
            // replace placeholders with actual values
            // only title and year are useful for youtube movies and shows
            youTubeSearchStr = youTubeSearchStr.Replace("%title%", item.Title);
            youTubeSearchStr = youTubeSearchStr.Replace("%year%", item.Year.ToString());

            if (item.MediaType == MediaItemType.Season || item.MediaType == MediaItemType.Episode)
            {
                youTubeSearchStr = youTubeSearchStr.Replace("%season%", item.Season.ToString());
            }
            if (item.MediaType == MediaItemType.Episode)
            {
                youTubeSearchStr = youTubeSearchStr.Replace("%episode%", item.Episode.ToString());
                youTubeSearchStr = youTubeSearchStr.Replace("%episodename%", item.EpisodeName.ToString());
            }
            
            return youTubeSearchStr;
        }

        #endregion
    }
}
