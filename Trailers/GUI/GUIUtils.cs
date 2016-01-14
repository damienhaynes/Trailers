using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using Trailers.Localisation;
using Trailers.Providers;

namespace Trailers.GUI
{
    enum ExternalPluginWindows
    {
        VideoInfo = 2003,
        VideoTitle = 25,
        VideoFile = 6,
        OnlineVideos = 4755,
        MyFilmsDetails = 7987,
        TVSeries = 9811,
        MovingPictures = 96742,
        ShowTimes = 7111992,
        MPSkinSettings = 705,
        TraktCalendar = 87259,
        TraktRecommendationsShows = 87262,
        TraktRecommendationsMovies = 87263,
        TraktTrendingShows = 87265,
        TraktTrendingMovies = 87266,
        TraktWatchedListShows = 87268,
        TraktWatchedListEpisodes = 87269,
        TraktWatchedListMovies = 87270,
        TraktRelatedMovies = 87277,
        TraktRelatedShows = 87278,
        TraktRecentWatchedMovies = 87284,
        TraktRecentWatchedEpisodes = 87285,
        TraktRecentAddedMovies = 87286,
        TraktRecentAddedEpisodes = 87287,
        TraktSearchEpisodes = 874002,
        TraktSearchShows = 874003,
        TraktSearchMovies = 874004,
        TraktListItems = 87276,
        TraktShowSeasons = 87281,
        TraktSeasonEpisodes = 87282,
        TraktPopularMovies = 87101,
        TraktPopularShows = 87102,
        TraktAnticipatedMovies = 87605,
        TraktAnticipatedShows = 87606,
        TraktBoxOffice = 87607,
        TraktPersonCreditMovies = 87601,
        TraktPersonCreditShows = 87602,
        NetflixAlpha = 18764,
        MPEISettings = 803
    }

    public static class GUIUtils
    {
        private delegate bool ShowCustomYesNoDialogDelegate(string heading, string lines, string yesLabel, string noLabel, bool defaultYes);
        private delegate void ShowOKDialogDelegate(string heading, string lines);
        private delegate void ShowNotifyDialogDelegate(string heading, string text, string image, string buttonText, int timeOut);
        private delegate int ShowTrailerMenuDialogDelegate(string heading, List<GUITrailerListItem> items, int selectedIdx);
        private delegate int ShowMenuDialogDelegate(string heading, List<GUITrailerListItem> items, int selectedIdx);
        private delegate List<MultiSelectionItem> ShowMultiSelectionDialogDelegate(string heading, List<MultiSelectionItem> items);
        private delegate void ShowTextDialogDelegate(string heading, string text);
        private delegate bool GetStringFromKeyboardDelegate(ref string strLine, bool isPassword);

        public static readonly string TrailersLogo = GUIGraphicsContext.Skin + "\\Media\\Logos\\trailers.png";

        public static string PluginName()
        {
            return "Trailers";
        }

        public static string GetProperty(string property)
        {
            string propertyVal = GUIPropertyManager.GetProperty(property);
            return propertyVal ?? string.Empty;
        }

        public static void SetProperty(string property, string value)
        {
            SetProperty(property, value, false);
        }

        public static void SetProperty(string property, string value, bool log)
        {
            // prevent ugly display of property names
            if (string.IsNullOrEmpty(value))
                value = " ";

            GUIPropertyManager.SetProperty(property, value);

            if (log)
            {
                if (GUIPropertyManager.Changed)
                    FileLog.Debug("Set property \"" + property + "\" to \"" + value + "\" successful");
                else
                    FileLog.Warning("Set property \"" + property + "\" to \"" + value + "\" failed");
            }
        }

        public static void SetPlayProperties(MediaItem item, bool onlinevideoplayer = false)
        {
            var playThread = new Thread((obj) =>
            {
                Thread.Sleep(2000);

                var playItem = obj as MediaItem;
                if (playItem == null) return;

                SetProperty("#Play.Current.Title", playItem.Title);
                SetProperty("#Play.Current.Plot", playItem.Plot);
                SetProperty("#Play.Current.Thumb", playItem.Poster);
                SetProperty("#Play.Current.Year", playItem.Year.ToString());
                SetProperty("#Play.Current.IMDBNumber", playItem.IMDb);
                SetProperty("#Play.Current.TMDBNumber", playItem.TMDb);

                // check if we should set any online video specific properties
                if (onlinevideoplayer)
                {
                    SetProperty("#Play.Current.OnlineVideos.SiteName", "YouTube Trailers");
                    SetProperty("#Play.Current.OnlineVideos.SiteIcon", TrailersLogo);
                }
            })
            {
                IsBackground = true,
                Name = "TrailerPlay"
            };

            playThread.Start(item);
        }

        /// <summary>
        /// Displays a yes/no dialog.
        /// </summary>
        /// <returns>True if yes was clicked, False if no was clicked</returns>
        public static bool ShowYesNoDialog(string heading, string lines)
        {
            return ShowCustomYesNoDialog(heading, lines, null, null, false);
        }

        /// <summary>
        /// Displays a yes/no dialog.
        /// </summary>
        /// <returns>True if yes was clicked, False if no was clicked</returns>
        public static bool ShowYesNoDialog(string heading, string lines, bool defaultYes)
        {
            return ShowCustomYesNoDialog(heading, lines, null, null, defaultYes);
        }

        /// <summary>
        /// Displays a yes/no dialog with custom labels for the buttons.
        /// This method may become obsolete in the future if media portal adds more dialogs.
        /// </summary>
        /// <returns>True if yes was clicked, False if no was clicked</returns>
        public static bool ShowCustomYesNoDialog(string heading, string lines, string yesLabel, string noLabel)
        {
            return ShowCustomYesNoDialog(heading, lines, yesLabel, noLabel, false);
        }

        /// <summary>
        /// Displays a yes/no dialog with custom labels for the buttons.
        /// This method may become obsolete in the future if media portal adds more dialogs.
        /// </summary>
        /// <returns>True if yes was clicked, False if no was clicked</returns>
        public static bool ShowCustomYesNoDialog(string heading, string lines, string yesLabel, string noLabel, bool defaultYes)
        {
            if (GUIGraphicsContext.form.InvokeRequired)
            {
                ShowCustomYesNoDialogDelegate d = ShowCustomYesNoDialog;
                return (bool)GUIGraphicsContext.form.Invoke(d, heading, lines, yesLabel, noLabel, defaultYes);
            }

            GUIDialogYesNo dlgYesNo = (GUIDialogYesNo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_YES_NO);

            try
            {
                dlgYesNo.Reset();
                dlgYesNo.SetHeading(heading);
                string[] linesArray = lines.Split(new string[] { "\\n", "\n" }, StringSplitOptions.None);
                if (linesArray.Length > 0) dlgYesNo.SetLine(1, linesArray[0]);
                if (linesArray.Length > 1) dlgYesNo.SetLine(2, linesArray[1]);
                if (linesArray.Length > 2) dlgYesNo.SetLine(3, linesArray[2]);
                if (linesArray.Length > 3) dlgYesNo.SetLine(4, linesArray[3]);
                dlgYesNo.SetDefaultToYes(defaultYes);

                foreach (GUIControl item in dlgYesNo.Children)
                {
                    if (item is GUIButtonControl)
                    {
                        GUIButtonControl btn = (GUIButtonControl)item;
                        if (btn.GetID == 11 && !string.IsNullOrEmpty(yesLabel)) // Yes button
                            btn.Label = yesLabel;
                        else if (btn.GetID == 10 && !string.IsNullOrEmpty(noLabel)) // No button
                            btn.Label = noLabel;
                    }
                }
                dlgYesNo.DoModal(GUIWindowManager.ActiveWindow);
                return dlgYesNo.IsConfirmed;
            }
            finally
            {
                // set the standard yes/no dialog back to it's original state (yes/no buttons)
                if (dlgYesNo != null)
                {
                    dlgYesNo.ClearAll();
                }
            }
        }

        /// <summary>
        /// Displays a OK dialog with heading and up to 4 lines.
        /// </summary>
        public static void ShowOKDialog(string heading, string line1, string line2, string line3, string line4)
        {
            ShowOKDialog(heading, string.Concat(line1, line2, line3, line4));
        }

        /// <summary>
        /// Displays a OK dialog with heading and up to 4 lines split by \n in lines string.
        /// </summary>
        public static void ShowOKDialog(string heading, string lines)
        {
            if (GUIGraphicsContext.form.InvokeRequired)
            {
                ShowOKDialogDelegate d = ShowOKDialog;
                GUIGraphicsContext.form.Invoke(d, heading, lines);
                return;
            }

            GUIDialogOK dlgOK = (GUIDialogOK)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_OK);            

            dlgOK.Reset();
            dlgOK.SetHeading(heading);

            int lineid = 1;
            foreach (string line in lines.Split(new string[] { "\\n", "\n" }, StringSplitOptions.None))
            {
                dlgOK.SetLine(lineid, line);
                lineid++;
            }
            for (int i = lineid; i <= 4; i++)
                dlgOK.SetLine(i, string.Empty);

            dlgOK.DoModal(GUIWindowManager.ActiveWindow);
        }

        /// <summary>
        /// Displays a notification dialog.
        /// </summary>
        public static void ShowNotifyDialog(string heading, string text)
        {
            ShowNotifyDialog(heading, text, TrailersLogo, Translation.OK, -1);
        }

        /// <summary>
        /// Displays a notification dialog.
        /// </summary>
        public static void ShowNotifyDialog(string heading, string text, int timeOut)
        {
            ShowNotifyDialog(heading, text, TrailersLogo, Translation.OK, timeOut);
        }

        /// <summary>
        /// Displays a notification dialog.
        /// </summary>
        public static void ShowNotifyDialog(string heading, string text, string image)
        {
            ShowNotifyDialog(heading, text, image, Translation.OK, -1);
        }

        /// <summary>
        /// Displays a notification dialog.
        /// </summary>
        public static void ShowNotifyDialog(string heading, string text, string image, string buttonText, int timeout)
        {
            if (GUIGraphicsContext.form.InvokeRequired)
            {
                ShowNotifyDialogDelegate d = ShowNotifyDialog;
                GUIGraphicsContext.form.Invoke(d, heading, text, image, buttonText, timeout);
                return;
            }

            GUIDialogNotify pDlgNotify = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
            if (pDlgNotify == null) return;

            try
            {
                pDlgNotify.Reset();
                pDlgNotify.SetHeading(heading);
                pDlgNotify.SetImage(image);
                pDlgNotify.SetText(text);
                if (timeout >= 0) pDlgNotify.TimeOut = timeout;
                    
                foreach (GUIControl item in pDlgNotify.Children)
                {
                    if (item is GUIButtonControl)
                    {
                        GUIButtonControl btn = (GUIButtonControl)item;
                        if (btn.GetID == 4 && !string.IsNullOrEmpty(buttonText) && !string.IsNullOrEmpty(btn.Label))
                        {
                            // Only if ID is 4 and we have our custom text and if button already has label (in case the skin "hides" the button by emtying the label)
                            btn.Label = buttonText;
                        }
                    }
                }

                pDlgNotify.DoModal(GUIWindowManager.ActiveWindow);
            }
            finally
            {
                if (pDlgNotify != null)
                    pDlgNotify.ClearAll();
            }
        }

        /// <summary>
        /// Displays a menu dialog from list of items
        /// </summary>
        /// <returns>Selected item index, -1 if exited</returns>
        public static int ShowMenuDialog(string heading, List<GUITrailerListItem> items)
        {
            if (File.Exists(GUIGraphicsContext.Skin + @"\Trailers.Selection.Menu.xml"))
            {
                return ShowTrailerMenuDialog(heading, items, -1);
            }
            else
            {
                return ShowMenuDialog(heading, items, -1);
            }
        }

        /// <summary>
        /// Displays a menu dialog from list of items
        /// </summary>
        /// <returns>Selected item index, -1 if exited</returns>
        public static int ShowMenuDialog(string heading, List<GUITrailerListItem> items, int selectedItemIndex)
        {
            if (GUIGraphicsContext.form.InvokeRequired)
            {
                ShowMenuDialogDelegate d = ShowMenuDialog;
                return (int)GUIGraphicsContext.form.Invoke(d, heading, items, selectedItemIndex);
            }

            if (items == null || items.Count == 0) return -1;

            var dlgMenu = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);            

            dlgMenu.Reset();
            dlgMenu.SetHeading(heading);           

            foreach (GUIListItem item in items)
            {
                dlgMenu.Add(item);
            }

            if (selectedItemIndex >= 0)
                dlgMenu.SelectedLabel = selectedItemIndex;

            dlgMenu.DoModal(GUIWindowManager.ActiveWindow);

            if (dlgMenu.SelectedLabel < 0)
            {
                return -1;
            }

            return dlgMenu.SelectedLabel;
        }

        /// <summary>
        /// Displays a trailer menu dialog from list of items
        /// </summary>
        /// <returns>Selected item index, -1 if exited</returns>
        public static int ShowTrailerMenuDialog(string heading, List<GUITrailerListItem> items, int selectedItemIndex)
        {
            if (GUIGraphicsContext.form.InvokeRequired)
            {
                ShowTrailerMenuDialogDelegate d = ShowTrailerMenuDialog;
                return (int)GUIGraphicsContext.form.Invoke(d, heading, items, selectedItemIndex);
            }

            if (items == null || items.Count == 0) return -1;

            var dlgMenu = (GUIDialogTrailers)GUIWindowManager.GetWindow(11898);

            dlgMenu.Reset();
            dlgMenu.SetHeading(heading);

            foreach (GUIListItem item in items)
            {
                dlgMenu.Add(item);
            }

            if (selectedItemIndex >= 0)
                dlgMenu.SelectedLabel = selectedItemIndex;

            dlgMenu.DoModal(GUIWindowManager.ActiveWindow);

            if (dlgMenu.SelectedLabel < 0)
            {
                return -1;
            }

            return dlgMenu.SelectedLabel;
        }

        /// <summary>
        /// Displays a menu dialog from list of items
        /// </summary>
        public static List<MultiSelectionItem> ShowMultiSelectionDialog(string heading, List<MultiSelectionItem> items)
        {
            List<MultiSelectionItem> result = new List<MultiSelectionItem>();
            if (items == null) return result;

            if (GUIGraphicsContext.form.InvokeRequired)
            {
                ShowMultiSelectionDialogDelegate d = ShowMultiSelectionDialog;
                return (List<MultiSelectionItem>)GUIGraphicsContext.form.Invoke(d, heading, items);
            }

            GUIWindow dlgMultiSelectOld = (GUIWindow)GUIWindowManager.GetWindow(2100);
            GUIDialogMultiSelect dlgMultiSelect = new GUIDialogMultiSelect();
            dlgMultiSelect.Init();
            GUIWindowManager.Replace(2100, dlgMultiSelect);

            try
            {
                dlgMultiSelect.Reset();
                dlgMultiSelect.SetHeading(heading);

                foreach (MultiSelectionItem multiSelectionItem in items)
                {
                    GUIListItem item = new GUIListItem();
                    item.Label = multiSelectionItem.ItemTitle;
                    item.Label2 = multiSelectionItem.ItemTitle2;
                    item.MusicTag = multiSelectionItem.Tag;
                    item.TVTag = multiSelectionItem.IsToggle;
                    item.Selected = multiSelectionItem.Selected;
                    dlgMultiSelect.Add(item);
                }

                dlgMultiSelect.DoModal(GUIWindowManager.ActiveWindow);

                if (dlgMultiSelect.DialogModalResult == ModalResult.OK)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        MultiSelectionItem item = items[i];
                        MultiSelectionItem newMultiSelectionItem = new MultiSelectionItem();
                        newMultiSelectionItem.ItemTitle = item.ItemTitle;
                        newMultiSelectionItem.ItemTitle2 = item.ItemTitle2;
                        newMultiSelectionItem.ItemID = item.ItemID;
                        newMultiSelectionItem.Tag = item.Tag;
                        try
                        {
                            newMultiSelectionItem.Selected = dlgMultiSelect.ListItems[i].Selected;
                        }
                        catch
                        {
                            newMultiSelectionItem.Selected = item.Selected;
                        }

                        result.Add(newMultiSelectionItem);
                    }
                }
                else
                    return null;

                return result;
            }
            finally
            {
                GUIWindowManager.Replace(2100, dlgMultiSelectOld);
            }
        }

        /// <summary>
        /// Displays a text dialog.
        /// </summary>
        public static void ShowTextDialog(string heading, List<string> text)
        {
            if (text == null || text.Count == 0) return;
            ShowTextDialog(heading, string.Join("\n", text.ToArray()));
        }

        /// <summary>
        /// Displays a text dialog.
        /// </summary>
        public static void ShowTextDialog(string heading, string text)
        {
            if (GUIGraphicsContext.form.InvokeRequired)
            {
                ShowTextDialogDelegate d = ShowTextDialog;
                GUIGraphicsContext.form.Invoke(d, heading, text);
                return;
            }

            GUIDialogText dlgText = (GUIDialogText)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_TEXT);

            dlgText.Reset();
            dlgText.SetHeading(heading);
            dlgText.SetText(text);

            dlgText.DoModal(GUIWindowManager.ActiveWindow);
        }

        public static bool GetStringFromKeyboard(ref string strLine)
        {
            return GetStringFromKeyboard(ref strLine, false);
        }

        /// <summary>
        /// Gets the input from the virtual keyboard window.
        /// </summary>
        public static bool GetStringFromKeyboard(ref string strLine, bool isPassword)
        {
            if (GUIGraphicsContext.form.InvokeRequired)
            {
                GetStringFromKeyboardDelegate d = GetStringFromKeyboard;
                object[] args = { strLine, isPassword };
                bool result = (bool)GUIGraphicsContext.form.Invoke(d, args);
                strLine = (string)args[0];
                return result;
            }

            VirtualKeyboard keyboard = (VirtualKeyboard)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_VIRTUAL_KEYBOARD);
            if (keyboard == null) return false;

            keyboard.Reset();
            keyboard.Text = strLine;
            keyboard.Password = isPassword;
            keyboard.DoModal(GUIWindowManager.ActiveWindow);
            
            if (keyboard.IsConfirmed)
            {
                strLine = keyboard.Text;
                return true;
            }

            return false;
        }
    }    
}
