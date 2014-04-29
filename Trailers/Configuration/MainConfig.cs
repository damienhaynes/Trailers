using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Trailers.Configuration.Popups;

namespace Trailers.Configuration
{
    public partial class MainConfig : Form
    {
        public MainConfig()
        {
            InitializeComponent();
            this.Text = "Trailers v" + PluginSettings.Version;

            PluginSettings.PerformMaintenance();
            PluginSettings.LoadSettings();

            // Populate settings
            PopulateSearchProviderSettings();
            PopulateGeneralSettings();
            PopulateLocalTrailerSettings();
            PopulateManualSearchSettings();
            PopulateAutoDownloadSettings();

            // Enable / Disable Controls States
            SetLocalSearchControlsEnabledState();
            SetManualSearchControlsEnabledState();
        }

        #region Populate Settings

        private void PopulateSearchProviderSettings()
        {
            chkBoxLocalTrailers.Checked = PluginSettings.ProviderLocalSearch;
            chkBoxTMDbTrailers.Checked = PluginSettings.ProviderTMDb;
            chkBoxOnlineVideos.Checked = PluginSettings.ProviderOnlineVideoSearch;
        }

        private void PopulateGeneralSettings()
        {
            chkBoxSkipOnlineProvidersIfLocalFound.Checked = PluginSettings.SkipOnlineProvidersIfLocalFound;
            chkBoxAutoPlayOnSingleLocalOrOnlineTrailer.Checked = PluginSettings.AutoPlayOnSingleLocalOrOnlineTrailer;

            int selectedItem = 0;
            int i = 0;
            foreach (var language in PluginSettings.Languages)
            {
                cboPreferredLanguage.Items.Add(language);

                if (language.TwoLetterCode == PluginSettings.PreferredLanguage)
                    selectedItem = i;

                i++;
            }
            cboPreferredLanguage.SelectedIndex = selectedItem;

            chkboxFallbackToEnglish.Checked = PluginSettings.FallbackToEnglishLanguage;
            chkboxAlwaysGetEnglish.Checked = PluginSettings.AlwaysGetEnglishTrailers;
        }

        private void PopulateLocalTrailerSettings()
        {
            chkBoxSearchLocalInCurrentMediaFolder.Checked = PluginSettings.SearchLocalInCurrentMediaFolder;
            txtBoxCurrentFolderSearchPatterns.Text = PluginSettings.SearchLocalCurrentMediaFolderSearchPatterns;

            chkBoxSearchLocalInSubFolder.Checked = PluginSettings.SearchLocalInSubFolder;
            txtBoxLocalAdditionalSubFolders.Text = PluginSettings.SearchLocalAdditionalSubFolders;

            chkBoxSearchLocalInDedicatedDirectory.Checked = PluginSettings.SearchLocalInDedicatedDirectory;
            if (!string.IsNullOrEmpty(PluginSettings.SearchLocalDedicatedDirectories))
            {
                listBoxDedicatedDirectories.Items.AddRange(PluginSettings.SearchLocalDedicatedDirectories.Split('|'));
            }
            txtBoxDedicatedSubDirectories.Text = PluginSettings.SearchLocalDedicatedSubDirectories;
            txtBoxLocalDedicatedDirectorySearchPatterns.Text = PluginSettings.SearchLocalDedicatedDirectorySearchPatterns;

            chkBoxAggressiveSearch.Checked = PluginSettings.SearchLocalAggressiveSearch;
        }

        private void PopulateManualSearchSettings()
        {
            chkBoxOnlineVideosYouTubeEnabled.Checked = PluginSettings.OnlineVideosYouTubeEnabled;
            txtBoxOnlineVideosYouTubeMovieSearchString.Text = PluginSettings.OnlineVideosYouTubeMovieSearchString;
            txtBoxOnlineVideosYouTubeShowSearchString.Text = PluginSettings.OnlineVideosYouTubeShowSearchString;
            txtBoxOnlineVideosYouTubeSeasonSearchString.Text = PluginSettings.OnlineVideosYouTubeSeasonSearchString;
            txtBoxOnlineVideosYouTubeEpisodeSearchString.Text = PluginSettings.OnlineVideosYouTubeEpisodeSearchString;
            txtBoxOnlineVideosYouTubeEpisodeSpecialSearchString.Text = PluginSettings.OnlineVideosYouTubeEpisodeSpecialSearchString;

            chkBoxOnlineVideosITunesEnabled.Checked = PluginSettings.OnlineVideosITunesEnabled;
            chkBoxOnlineVideosIMDbEnabled.Checked = PluginSettings.OnlineVideosIMDbEnabled;
        }

        private void PopulateAutoDownloadSettings()
        {
            chkBoxAutoDownloadMovingPictures.Checked = PluginSettings.AutoDownloadTrailersMovingPictures;
            chkBoxAutoDownloadMyVideos.Checked = PluginSettings.AutoDownloadTrailersMyVideos;
            chkBoxAutoDownloadMyFilms.Checked = PluginSettings.AutoDownloadTrailersMyFilms;

            chkBoxAutoDownloadTrailers.Checked = PluginSettings.AutoDownloadTrailers;
            chkBoxAutoDownloadTeasers.Checked = PluginSettings.AutoDownloadTeasers;
            chkBoxAutoDownloadFeaturettes.Checked = PluginSettings.AutoDownloadFeaturettes;
            chkBoxAutoDownloadClips.Checked = PluginSettings.AutoDownloadClips;

            switch(PluginSettings.AutoDownloadQuality)
            {
                case "HD":
                    comboBoxAutoDownloadQuality.SelectedIndex = 0;
                    break;
                case "HQ":
                    comboBoxAutoDownloadQuality.SelectedIndex = 1;
                    break;
                case "LQ":
                    comboBoxAutoDownloadQuality.SelectedIndex = 2;
                    break;
                default:
                    comboBoxAutoDownloadQuality.SelectedIndex = 0;
                    break;
            }

            spinBoxAutoDownloadStartDelay.Value = PluginSettings.AutoDownloadStartDelay / 1000;             // milliseconds -> seconds
            spinBoxAutoDownloadScanInterval.Value = PluginSettings.AutoDownloadInterval / 1000 / 60 / 60;   // milliseconds -> hours
            spinBoxAutoDownloadUpdateCheck.Value = PluginSettings.AutoDownloadUpdateInterval;

            txtBoxAutoDownloadSavePath.Text = PluginSettings.AutoDownloadDirectory;
            chkBoxAutoDownloadCleanup.Checked = PluginSettings.AutoDownloadCleanup;
        }

        #endregion

        #region Save Settings

        private void btnApplySettings_Click(object sender, EventArgs e)
        {
            // Save Settings
            PluginSettings.SaveSettings();
            this.Close();
        }

        #endregion

        #region Event Handlers (Search Providers)
        private void chkBoxLocalTrailers_Click(object sender, EventArgs e)
        {
            PluginSettings.ProviderLocalSearch = !PluginSettings.ProviderLocalSearch;
            SetLocalSearchControlsEnabledState();
        }

        private void chkBoxTMDbTrailers_Click(object sender, EventArgs e)
        {
            PluginSettings.ProviderTMDb = !PluginSettings.ProviderTMDb;
        }

        private void chkBoxOnlineVideos_Click(object sender, EventArgs e)
        {
            PluginSettings.ProviderOnlineVideoSearch = !PluginSettings.ProviderOnlineVideoSearch;
            SetManualSearchControlsEnabledState();
        }
        #endregion

        #region Event Handlers (General Settings)
        private void chkBoxSkipOnlineProvidersIfLocalFound_Click(object sender, EventArgs e)
        {
            PluginSettings.SkipOnlineProvidersIfLocalFound = !PluginSettings.SkipOnlineProvidersIfLocalFound;
        }

        private void chkBoxAutoPlayOnSingleLocalOrOnlineTrailer_Click(object sender, EventArgs e)
        {
            PluginSettings.AutoPlayOnSingleLocalOrOnlineTrailer = !PluginSettings.AutoPlayOnSingleLocalOrOnlineTrailer;
        }

        private void chkboxFallbackToEnglish_Click(object sender, EventArgs e)
        {
            PluginSettings.FallbackToEnglishLanguage = !PluginSettings.FallbackToEnglishLanguage;
        }

        private void chkboxAlwaysGetEnglish_Click(object sender, EventArgs e)
        {
            PluginSettings.AlwaysGetEnglishTrailers = !PluginSettings.AlwaysGetEnglishTrailers;
        }

        private void cboPreferredLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedLanguage = cboPreferredLanguage.SelectedItem as PluginSettings.LanguageISO;
            PluginSettings.PreferredLanguage = selectedLanguage.TwoLetterCode;
        }
        #endregion

        #region Event Handlers (Local Trailer Settings
        private void chkBoxSearchLocalInCurrentMediaFolder_Click(object sender, EventArgs e)
        {
            PluginSettings.SearchLocalInCurrentMediaFolder = !PluginSettings.SearchLocalInCurrentMediaFolder;
            SetLocalSearchControlsEnabledState();
        }

        private void txtBoxCurrentFolderSearchPatterns_TextChanged(object sender, EventArgs e)
        {
            PluginSettings.SearchLocalCurrentMediaFolderSearchPatterns = txtBoxCurrentFolderSearchPatterns.Text;
        }

        private void chkBoxSearchLocalInSubFolder_Click(object sender, EventArgs e)
        {
            PluginSettings.SearchLocalInSubFolder = !PluginSettings.SearchLocalInSubFolder;
            SetLocalSearchControlsEnabledState();
        }

        private void txtBoxLocalAdditionalSubFolders_TextChanged(object sender, EventArgs e)
        {
            PluginSettings.SearchLocalAdditionalSubFolders = txtBoxLocalAdditionalSubFolders.Text;
        }

        private void chkBoxSearchLocalInDedicatedDirectory_Click(object sender, EventArgs e)
        {
            PluginSettings.SearchLocalInDedicatedDirectory = !PluginSettings.SearchLocalInDedicatedDirectory;
            SetLocalSearchControlsEnabledState();
        }

        private void btnAddDedicatedDirectory_Click(object sender, EventArgs e)
        {
            var pathPopup = new AddPathPopup();
            if (pathPopup.ShowDialog() == DialogResult.OK)
            {
                string path = pathPopup.SelectedPath;

                if (!Directory.Exists(path))
                {
                    string message = "The path entered does not exist!";
                    MessageBox.Show(message, "Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                listBoxDedicatedDirectories.Items.Add(path);

                // save as pipe seperated string
                PluginSettings.SearchLocalDedicatedDirectories = string.Join("|", listBoxDedicatedDirectories.Items.OfType<string>().ToArray());
            }
        }

        private void btnRemoveDedicatedDirectory_Click(object sender, EventArgs e)
        {
            if (listBoxDedicatedDirectories.SelectedIndex < 0) return;

            listBoxDedicatedDirectories.Items.Remove(listBoxDedicatedDirectories.SelectedItem);

            // save as pipe seperated string
            PluginSettings.SearchLocalDedicatedDirectories = string.Join("|", listBoxDedicatedDirectories.Items.OfType<string>().ToArray());
        }

        private void txtBoxDedicatedSubDirectories_TextChanged(object sender, EventArgs e)
        {
            PluginSettings.SearchLocalDedicatedSubDirectories = txtBoxDedicatedSubDirectories.Text;
        }

        private void txtBoxLocalDedicatedDirectorySearchPatterns_TextChanged(object sender, EventArgs e)
        {
            PluginSettings.SearchLocalDedicatedDirectorySearchPatterns = txtBoxLocalDedicatedDirectorySearchPatterns.Text;
        }

        private void chkBoxAggressiveSearch_Click(object sender, EventArgs e)
        {
            PluginSettings.SearchLocalAggressiveSearch = !PluginSettings.SearchLocalAggressiveSearch;
        }
        #endregion

        #region Event Handlers (Manual Search Settings)
        private void chkBoxOnlineVideosYouTubeEnabled_Click(object sender, EventArgs e)
        {
            PluginSettings.OnlineVideosYouTubeEnabled = !PluginSettings.OnlineVideosYouTubeEnabled;
            SetManualSearchControlsEnabledState();
        }

        private void txtBoxOnlineVideosYouTubeMovieSearchString_TextChanged(object sender, EventArgs e)
        {
            PluginSettings.OnlineVideosYouTubeMovieSearchString = txtBoxOnlineVideosYouTubeMovieSearchString.Text;
        }
        private void txtBoxOnlineVideosYouTubeShowSearchString_TextChanged(object sender, EventArgs e)
        {
            PluginSettings.OnlineVideosYouTubeShowSearchString = txtBoxOnlineVideosYouTubeShowSearchString.Text;
        }

        private void txtBoxOnlineVideosYouTubeSeasonSearchString_TextChanged(object sender, EventArgs e)
        {
            PluginSettings.OnlineVideosYouTubeSeasonSearchString = txtBoxOnlineVideosYouTubeSeasonSearchString.Text;
        }

        private void txtBoxOnlineVideosYouTubeEpisodeSearchString_TextChanged(object sender, EventArgs e)
        {
            PluginSettings.OnlineVideosYouTubeEpisodeSearchString = txtBoxOnlineVideosYouTubeEpisodeSearchString.Text;
        }

        private void txtBoxOnlineVideosYouTubeEpisodeSpecialSearchString_TextChanged(object sender, EventArgs e)
        {
            PluginSettings.OnlineVideosYouTubeEpisodeSpecialSearchString = txtBoxOnlineVideosYouTubeEpisodeSpecialSearchString.Text;
        }

        private void chkBoxOnlineVideosITunesEnabled_Click(object sender, EventArgs e)
        {
            PluginSettings.OnlineVideosITunesEnabled = !PluginSettings.OnlineVideosITunesEnabled;
        }

        private void chkBoxOnlineVideosIMDbEnabled_Click(object sender, EventArgs e)
        {
            PluginSettings.OnlineVideosIMDbEnabled = !PluginSettings.OnlineVideosIMDbEnabled;
        }
        #endregion

        #region Event Handlers (Auto Download Settings)
        private void chkBoxAutoDownloadMovingPictures_Click(object sender, EventArgs e)
        {
            PluginSettings.AutoDownloadTrailersMovingPictures = !PluginSettings.AutoDownloadTrailersMovingPictures;
        }

        private void chkBoxAutoDownloadMyVideos_Click(object sender, EventArgs e)
        {
            PluginSettings.AutoDownloadTrailersMyVideos = !PluginSettings.AutoDownloadTrailersMyVideos;
        }

        private void chkBoxAutoDownloadMyFilms_Click(object sender, EventArgs e)
        {
            PluginSettings.AutoDownloadTrailersMyFilms = !PluginSettings.AutoDownloadTrailersMyFilms;
        }

        private void chkBoxAutoDownloadTrailers_Click(object sender, EventArgs e)
        {
            PluginSettings.AutoDownloadTrailers = !PluginSettings.AutoDownloadTrailers;
        }

        private void chkBoxAutoDownloadTeasers_Click(object sender, EventArgs e)
        {
            PluginSettings.AutoDownloadTeasers = !PluginSettings.AutoDownloadTeasers;
        }

        private void chkBoxAutoDownloadFeaturettes_Click(object sender, EventArgs e)
        {
            PluginSettings.AutoDownloadFeaturettes = !PluginSettings.AutoDownloadFeaturettes;
        }

        private void chkBoxAutoDownloadClips_Click(object sender, EventArgs e)
        {
            PluginSettings.AutoDownloadClips = !PluginSettings.AutoDownloadClips;
        }

        private void comboBoxAutoDownloadQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxAutoDownloadQuality.SelectedIndex)
            {
                case 0:
                    PluginSettings.AutoDownloadQuality = "HD";
                    break;
                case 1:
                    PluginSettings.AutoDownloadQuality = "HQ";
                    break;
                case 2:
                    PluginSettings.AutoDownloadQuality = "LQ";
                    break;
                default:
                    PluginSettings.AutoDownloadQuality = "HD";
                    break;
            }
        }

        private void spinBoxAutoDownloadStartDelay_ValueChanged(object sender, EventArgs e)
        {
            // from seconds -> milliseconds
            PluginSettings.AutoDownloadStartDelay = Convert.ToInt32(spinBoxAutoDownloadStartDelay.Value * 1000);
        }

        private void spinBoxAutoDownloadScanInterval_ValueChanged(object sender, EventArgs e)
        {
            // from hours -> milliseconds
            PluginSettings.AutoDownloadInterval = Convert.ToInt32(spinBoxAutoDownloadScanInterval.Value * 60 * 60 * 1000);
        }

        private void spinBoxAutoDownloadUpdateCheck_ValueChanged(object sender, EventArgs e)
        {
            PluginSettings.AutoDownloadUpdateInterval = Convert.ToInt32(spinBoxAutoDownloadUpdateCheck.Value);
        }

        private void btnFolderBrowseAutoDownloadPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select a directory for automatic trailer downloads:";
                dialog.SelectedPath = PluginSettings.AutoDownloadDirectory;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    PluginSettings.AutoDownloadDirectory = dialog.SelectedPath;
                    txtBoxAutoDownloadSavePath.Text = dialog.SelectedPath;
                }
            }
        }

        private void chkBoxAutoDownloadCleanup_Click(object sender, EventArgs e)
        {
            PluginSettings.AutoDownloadCleanup = !PluginSettings.AutoDownloadCleanup;
        }
        #endregion

        #region Enable / Disable Controls
        private void SetLocalSearchControlsEnabledState()
        {
            txtBoxCurrentFolderSearchPatterns.Enabled = PluginSettings.SearchLocalInCurrentMediaFolder;
            txtBoxLocalAdditionalSubFolders.Enabled = PluginSettings.SearchLocalInSubFolder;
            listBoxDedicatedDirectories.Enabled = PluginSettings.SearchLocalInDedicatedDirectory;
            btnAddDedicatedDirectory.Enabled = PluginSettings.SearchLocalInDedicatedDirectory;
            btnRemoveDedicatedDirectory.Enabled = PluginSettings.SearchLocalInDedicatedDirectory;
            txtBoxDedicatedSubDirectories.Enabled = PluginSettings.SearchLocalInDedicatedDirectory;
            txtBoxLocalDedicatedDirectorySearchPatterns.Enabled = PluginSettings.SearchLocalInDedicatedDirectory;

            gbxLocalTrailerSettings.Enabled = PluginSettings.ProviderLocalSearch;
        }

        private void SetManualSearchControlsEnabledState()
        {
            txtBoxOnlineVideosYouTubeMovieSearchString.Enabled = PluginSettings.OnlineVideosYouTubeEnabled;
            txtBoxOnlineVideosYouTubeShowSearchString.Enabled = PluginSettings.OnlineVideosYouTubeEnabled;
            txtBoxOnlineVideosYouTubeSeasonSearchString.Enabled = PluginSettings.OnlineVideosYouTubeEnabled;
            txtBoxOnlineVideosYouTubeEpisodeSearchString.Enabled = PluginSettings.OnlineVideosYouTubeEnabled;
            txtBoxOnlineVideosYouTubeEpisodeSpecialSearchString.Enabled = PluginSettings.OnlineVideosYouTubeEnabled;

            gbxOnlineVideoSettings.Enabled = PluginSettings.ProviderOnlineVideoSearch;
        }
        #endregion

    }
}
