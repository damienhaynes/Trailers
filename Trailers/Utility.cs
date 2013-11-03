using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
