using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trailers.GUI;

namespace Trailers.Providers
{
    interface IProvider
    {
        /// <summary>
        /// Wether or not provider is enabled for lookup
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Name of Trailer Provider
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Is a Local or a Online Trailer provider
        /// </summary>
        bool IsLocal { get; }

        /// <summary>
        /// Gets a list of Trailer sources
        /// </summary>
        /// <param name="itemId">ID of Movie, Show, Episode for online lookup</param>
        /// <returns>List of Trailers to present in a MediaPortal GUI Menu</returns>
        List<GUITrailerListItem> Search(MediaItem searchItem);
    }
}
