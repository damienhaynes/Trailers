using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;
using Trailers.Providers;

namespace Trailers.PluginHandlers
{
    class VideoInfoHandler
    {
        public static bool GetCurrentMediaItem(out MediaItem currentMediaItem)
        {
            FileLog.Info("Getting selected movie information from MyVideos.");

            currentMediaItem = new MediaItem();
            currentMediaItem.Title = GUIPropertyManager.GetProperty("#title").Trim();

            int year;
            var strYear = GUIPropertyManager.GetProperty("#year").Trim();
            if (int.TryParse(strYear, out year))
                currentMediaItem.Year = year;

            // Get IMDb ID
            string imdbid = GUIPropertyManager.GetProperty("#imdbnumber").Trim();
            if (!string.IsNullOrEmpty(imdbid) && imdbid.Length == 9)
                currentMediaItem.IMDb = imdbid;

            currentMediaItem.Plot = GUIPropertyManager.GetProperty("#plot").Trim();
            currentMediaItem.Poster = GUIPropertyManager.GetProperty("#thumb").Trim();

            // Get Local File Info
            currentMediaItem.FullPath = GUIPropertyManager.GetProperty("#file").Trim();

            // At the very least we should have a file
            if (string.IsNullOrEmpty(currentMediaItem.FullPath))
            {
                // try get the selected item on the facade
                if (SelectedItem != null && !SelectedItem.IsFolder)
                {
                    currentMediaItem.FullPath = SelectedItem.Path;
                }
                else
                {
                    currentMediaItem = null;
                }
            }

            return true;
        }

        static GUIFacadeControl Facade
        {
            get
            {
                // Get the current window
                var window = GUIWindowManager.GetWindow(GUIWindowManager.ActiveWindow);
                if (window == null) return null;

                // Get the Facade control
                return window.GetControl(50) as GUIFacadeControl;
            }
        }

        static GUIListItem SelectedItem
        {
            get
            {
                if (Facade == null) return null;

                // Get the Selected Item
                return Facade.SelectedListItem;
            }
        }
    }
}
