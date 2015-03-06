using System;
using System.Diagnostics;
using System.IO;
using MediaPortal.Profile;

namespace Trailers
{
    class Utility
    {
        public static bool IsPluginEnabled(string name)
        {
            using (Settings xmlreader = new MPSettings())
            {
                return xmlreader.GetValueAsBool("plugins", name, false);
            }
        }

        public static Version FileVersion(string filename)
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
