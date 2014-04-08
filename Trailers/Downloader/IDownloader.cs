using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trailers.Downloader
{
    public interface IDownloader
    {
        /// <summary>
        /// Whether or not trailer downloads are enabled for plugin
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Name of Plugin to download from
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Downloads trailers for media in local library
        /// </summary>
        void Download();
    }
}
