using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trailers.Web
{
    class WebUtils
    {
        public static WebGrabber GetWebGrabberInstance(string url)
        {
            WebGrabber grabber = new WebGrabber(url);
            grabber.UserAgent = PluginSettings.UserAgent;
            grabber.MaxRetries = PluginSettings.WebMaxRetries;
            grabber.Timeout = PluginSettings.WebTimeout;
            grabber.TimeoutIncrement = PluginSettings.WebTimeoutIncrement;
            return grabber;
        }

        public static string GetYouTubeURL(string source)
        {
            // check it's not already a youtube url
            if (source.ToLowerInvariant().Contains("youtube.com")) 
                return source;

            return string.Format("http://www.youtube.com/watch?v={0}&hd=1", source);
        }
    }
}
