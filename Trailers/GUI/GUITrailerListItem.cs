using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;
using Trailers.Providers;

namespace Trailers.GUI
{
    public class GUITrailerListItem : GUIListItem 
    {
        public string URL { get; set; }
        public bool IsSearchItem { get; set; }
        public bool IsOnlineItem { get; set; }
        public MediaItem CurrentMedia { get; set; }
    }
}
