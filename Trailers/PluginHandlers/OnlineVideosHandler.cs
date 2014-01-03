using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MediaPortal.Configuration;
using OnlineVideos;
using Trailers.Web;
using Trailers.Player;

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

        public static Dictionary<string, string> GetPlaybackOptionsFromYoutubeUrl(string source)
        {
            var playbackOptions = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(source)) return playbackOptions;

            string url = WebUtils.GetYouTubeURL(source);

            FileLog.Debug("Getting download and playback options for url: '{0}'", url);

            try
            {
                var ovHosterProxy = OnlineVideosAppDomain.Domain.CreateInstanceAndUnwrap(typeof(OnlineVideosTrailersHosterProxy).Assembly.FullName, typeof(OnlineVideosTrailersHosterProxy).FullName) as OnlineVideosTrailersHosterProxy;
                playbackOptions = ovHosterProxy.GetPlaybackOptions(url);
                if (playbackOptions == null) return null;
            }
            catch (Exception e)
            {
                FileLog.Error("Error getting playback options: {0}", e.Message);
                return null;
            }

            if (playbackOptions.Count == 0) return null;

            foreach (var option in playbackOptions)
            {
                FileLog.Debug("Found download option, source url: '{0}', quality: '{1}'", option.Value, option.Key);
            }

            return playbackOptions;
        }
    }
}
