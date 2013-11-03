using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MediaPortal.Configuration;

namespace Trailers.PluginHandlers
{
    class OnlineVideosHandler
    {
        public static bool IsAvailable
        {
            get
            {
                if (!File.Exists(Path.Combine(Config.GetSubFolder(Config.Dir.Plugins, "Windows"), "OnlineVideos.MediaPortal1.dll")))
                    return false;

                return Utility.IsPluginEnabled("Online Videos") || Utility.IsPluginEnabled("OnlineVideos");
            }
        }
    }
}
