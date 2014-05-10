using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Trailers.Localisation;

namespace Trailers.Providers
{
    public enum MediaItemType
    {
        Movie,
        Show,
        Season,
        Episode
    }

    /// <summary>
    /// Hold information about the currently selected Media (movie, show, season, episode)
    /// TVShow and Seasons will hold no path details, similarly movies will contain no season/episode info
    /// </summary>
    public class MediaItem
    {
        public MediaItemType MediaType { get; set; }

        public string IMDb { get; set; }

        public string TVDb { get; set; }

        public string TMDb { get; set; }

        public string TVRage { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public int? Season { get; set; }

        public int? Episode { get; set; }

        public string EpisodeName { get; set; }

        public string AirDate { get; set; }

        public string Poster { get; set; }

        public string Plot { get; set; }

        public string FullPath { get; set; }

        public string Directory
        {
            get
            {
                if (string.IsNullOrEmpty(FullPath)) return null;
                return Path.GetDirectoryName(FullPath);
            }
        }

        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(FullPath)) return null;
                return Path.GetFileName(FullPath);
            }
        }

        public string FilenameWOExtension
        {
            get
            {
                if (string.IsNullOrEmpty(FileName)) return null;
                return Path.GetFileNameWithoutExtension(FileName);
            }
        }

        public override string ToString()
        {
            string str = string.Empty;
            switch (this.MediaType)
            {
                case MediaItemType.Movie:
                case MediaItemType.Show:
                    str = this.Title;
                    break;
                case MediaItemType.Season:
                    string seasonLabel = this.Season == 0 ? Translation.Specials : string.Format("{0} {1}", Translation.Season, this.Season.ToString());
                    str = string.Format("{0}: {1}", this.Title, seasonLabel);
                    break;
                case MediaItemType.Episode:
                    str = string.Format("{0}: {1}", this.Title, string.IsNullOrEmpty(this.EpisodeName) ? string.Format("{0}x{1}", this.Season, this.Episode) : this.EpisodeName);
                    break;
            }
            return str;
        }

    }
}
