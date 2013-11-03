using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;
using MediaPortal.Player;
using Trailers.GUI;
using Trailers.Providers;

namespace Trailers.Player
{
    class LocalPlayer
    {
        internal static void Play(string filename, MediaItem mediaItem)
        {
            FileLog.Info("Starting playback of local file: {0}", filename);

            GUIGraphicsContext.IsFullScreenVideo = true;
            GUIWindowManager.ActivateWindow((int)GUIWindow.Window.WINDOW_FULLSCREEN_VIDEO);

            try
            {
                g_Player.Play(filename, g_Player.MediaType.Video);
                GUIUtils.SetPlayProperties(mediaItem);
            }
            catch (Exception e)
            {
                FileLog.Info("Failed to play local file with error: {0}", e.Message);
            }
        }
    }
}
