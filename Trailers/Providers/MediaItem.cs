using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace Trailers.Providers
{
    public class MediaItem
    {
        public string IMDb { get; set; }

        public string TVDb { get; set; }

        public string TMDb { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

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

    }
}
