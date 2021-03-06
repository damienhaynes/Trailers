﻿using System;
using System.Diagnostics;
using System.IO;
using MediaPortal.Configuration;
using MediaPortal.Profile;

namespace Trailers
{
    class Utility
    {
        internal static string OnlineVideosPlugin = Path.Combine(Config.GetSubFolder(Config.Dir.Plugins, "Windows"), "OnlineVideos.MediaPortal1.dll");

        internal static bool IsPluginAvailable(string name)
        {
            switch (name)
            {
                case "Online Videos":
                case "OnlineVideos":
                    if (!File.Exists(OnlineVideosPlugin))
                        return false;

                    return Utility.IsPluginEnabled("Online Videos") || Utility.IsPluginEnabled("OnlineVideos");

                default:
                    return false;
            }
        }

        internal static bool IsPluginEnabled(string name)
        {
            using (Settings xmlreader = new MPSettings())
            {
                return xmlreader.GetValueAsBool("plugins", name, false);
            }
        }

        internal static Version FileVersion(string filename)
        {
            if (!File.Exists(filename))
                return new Version();

            var versionInfo = FileVersionInfo.GetVersionInfo(filename);
            return new Version(versionInfo.FileMajorPart,
                               versionInfo.FileMinorPart,
                               versionInfo.FilePrivatePart,
                               versionInfo.FileBuildPart);
        }
    }
}
