using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;
using Trailers.Providers;

namespace Trailers.PluginHandlers
{
    class ShowTimesHandler
    {
        public static bool GetCurrentMediaItem(out MediaItem currentMediaItem, out bool isDetailsView)
        {
            FileLog.Info("Getting selected movie information from ShowTimes.");

            // check if we're in details view
            isDetailsView = GUIWindowManager.GetWindow(GUIWindowManager.ActiveWindow).GetControl(24).IsVisible;            

            // get title
            currentMediaItem = new MediaItem();

            if (isDetailsView)
                currentMediaItem.Title = GUIPropertyManager.GetProperty("#st_title").Trim();
            else
                currentMediaItem.Title = GUIPropertyManager.GetProperty("#selecteditem").Trim();

            // clean the title
            currentMediaItem.Title = currentMediaItem.Title.Replace("3D", string.Empty).Trim();

            // get year
            DateTime releaseDate;
            var strReleaseDate = GUIPropertyManager.GetProperty("#st_releasedate").Trim();
            if (DateTime.TryParse(strReleaseDate, out releaseDate))
            {
                // get the year component
                currentMediaItem.Year = releaseDate.Year;
            }

            // get IMDb ID
            string imdbid = GUIPropertyManager.GetProperty("#st_imdb").Trim();
            if (!string.IsNullOrEmpty(imdbid) && imdbid.Length == 9)
                currentMediaItem.IMDb = imdbid;

            // get TMDb ID
            int iTMDbID;
            if (int.TryParse(GUIPropertyManager.GetProperty("#st_tmdb").Trim(), out iTMDbID))
                currentMediaItem.TMDb = iTMDbID.ToString();

            // get poster
            currentMediaItem.Poster = GUIPropertyManager.GetProperty("#st_poster").Trim();

            // get overview
            currentMediaItem.Plot = GUIPropertyManager.GetProperty("#st_plot").Trim();

            return true;
        }
    }
}
