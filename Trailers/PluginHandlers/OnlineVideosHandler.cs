using System;
using System.Collections.Generic;
using OnlineVideos;
using OnlineVideos.Hoster;
using OnlineVideos.Sites;
using Trailers.Web;

namespace Trailers.PluginHandlers
{
    class OnlineVideosHandler
    {
        internal static SiteUtilBase YouTubeSiteUtil
        {
            get
            {
                if (_YouTubeSiteUtil == null)
                {
                    FileLog.Info("Getting YouTube site util from OnlineVideos.");
                    SiteUtilBase siteUtil;
                    OnlineVideoSettings.Instance.SiteUtilsList.TryGetValue("YouTube", out siteUtil);
                    _YouTubeSiteUtil = siteUtil;

                    FileLog.Info("Finished getting YouTube site util, Success = '{0}'", siteUtil != null);
                }
                return _YouTubeSiteUtil;
            }
        }
        static SiteUtilBase _YouTubeSiteUtil = null;

        internal static Dictionary<string, string> GetPlaybackOptionsFromYoutubeUrl(string source)
        {
            var playbackOptions = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(source))
                return playbackOptions;

            string url = WebUtils.GetYouTubeURL(source);

            FileLog.Debug("Getting download and playback options, URL = '{0}'", url);

            try
            {
                var hosterBase = HosterFactory.GetHoster("Youtube");
                playbackOptions = hosterBase.GetPlaybackOptions(url);

                if (playbackOptions == null) return null;
            }
            catch (Exception e)
            {
                FileLog.Error("Error getting playback options, Reason = '{0}'", e.Message);
                return null;
            }

            if (playbackOptions.Count == 0) return null;

            foreach (var option in playbackOptions)
            {
                FileLog.Debug("Found download option, Source URL = '{0}', Quality = '{1}'", option.Value, option.Key);
            }

            return playbackOptions;
        }
    }
}
