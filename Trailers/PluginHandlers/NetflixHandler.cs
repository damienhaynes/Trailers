using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;
using Trailers.Providers;

namespace Trailers.PluginHandlers
{
    class NetflixHandler
    {
        public static bool GetCurrentMediaItem(out MediaItem currentMediaItem, out bool isDetailsView)
        {
            ///
            /// Netflix currently uses MovingPictures skin properties, this may change in future.
            ///

            FileLog.Info("Getting selected movie information from Netflix.");

            isDetailsView = true;
            currentMediaItem = new MediaItem();
            
            // check if we're in details view or main list
            var facade = GUIWindowManager.GetWindow(GUIWindowManager.ActiveWindow).GetControl(50);
            if (facade != null && facade.IsVisible) isDetailsView = false;

            // get all movie properties
            currentMediaItem.Title = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.title").Trim();
            currentMediaItem.Plot = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.summary").Trim();
            currentMediaItem.Poster = GUIPropertyManager.GetProperty("#MovingPictures.Coverart").Trim();

            int iYear;
            if (int.TryParse(GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.year").Trim(), out iYear))
            {
                currentMediaItem.Year = iYear;
            }

            return true;
        }

    }
}
