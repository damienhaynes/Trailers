namespace Trailers.Configuration
{
    partial class MainConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkBoxOnlineVideos = new System.Windows.Forms.CheckBox();
            this.chkBoxTMDbTrailers = new System.Windows.Forms.CheckBox();
            this.chkBoxLocalTrailers = new System.Windows.Forms.CheckBox();
            this.groupBoxLocalTrailerSettings = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chkBoxAggressiveSearch = new System.Windows.Forms.CheckBox();
            this.txtBoxLocalDedicatedDirectorySearchPatterns = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBoxDedicatedSubDirectories = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRemoveDedicatedDirectory = new System.Windows.Forms.Button();
            this.btnAddDedicatedDirectory = new System.Windows.Forms.Button();
            this.listBoxDedicatedDirectories = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkBoxSearchLocalInDedicatedDirectory = new System.Windows.Forms.CheckBox();
            this.txtBoxCurrentFolderSearchPatterns = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkBoxSearchLocalInCurrentMediaFolder = new System.Windows.Forms.CheckBox();
            this.txtBoxLocalAdditionalSubFolders = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkBoxSearchLocalInSubFolder = new System.Windows.Forms.CheckBox();
            this.groupBoxManualSearchSettings = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkBoxOnlineVideosIMDbEnabled = new System.Windows.Forms.CheckBox();
            this.chkBoxOnlineVideosITunesEnabled = new System.Windows.Forms.CheckBox();
            this.txtBoxOnlineVideosYouTubeSearchString = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkBoxOnlineVideosYouTubeEnabled = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkBoxAutoPlayOnSingleLocalOrOnlineTrailer = new System.Windows.Forms.CheckBox();
            this.chkBoxSkipOnlineProvidersIfLocalFound = new System.Windows.Forms.CheckBox();
            this.btnApplySettings = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkBoxAutoDownloadCleanup = new System.Windows.Forms.CheckBox();
            this.spinBoxAutoDownloadUpdateCheck = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.chkBoxAutoDownloadClips = new System.Windows.Forms.CheckBox();
            this.chkBoxAutoDownloadFeaturettes = new System.Windows.Forms.CheckBox();
            this.chkBoxAutoDownloadTeasers = new System.Windows.Forms.CheckBox();
            this.chkBoxAutoDownloadTrailers = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtBoxAutoDownloadSavePath = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.spinBoxAutoDownloadScanInterval = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.spinBoxAutoDownloadStartDelay = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBoxAutoDownloadQuality = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkBoxAutoDownloadMyFilms = new System.Windows.Forms.CheckBox();
            this.chkBoxAutoDownloadMyVideos = new System.Windows.Forms.CheckBox();
            this.chkBoxAutoDownloadMovingPictures = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBoxLocalTrailerSettings.SuspendLayout();
            this.groupBoxManualSearchSettings.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinBoxAutoDownloadUpdateCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinBoxAutoDownloadScanInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinBoxAutoDownloadStartDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkBoxOnlineVideos);
            this.groupBox1.Controls.Add(this.chkBoxTMDbTrailers);
            this.groupBox1.Controls.Add(this.chkBoxLocalTrailers);
            this.groupBox1.Location = new System.Drawing.Point(8, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 107);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Providers";
            // 
            // chkBoxOnlineVideos
            // 
            this.chkBoxOnlineVideos.AutoSize = true;
            this.chkBoxOnlineVideos.Location = new System.Drawing.Point(16, 76);
            this.chkBoxOnlineVideos.Name = "chkBoxOnlineVideos";
            this.chkBoxOnlineVideos.Size = new System.Drawing.Size(169, 17);
            this.chkBoxOnlineVideos.TabIndex = 2;
            this.chkBoxOnlineVideos.Text = "&OnlineVideos (Manual Search)";
            this.chkBoxOnlineVideos.UseVisualStyleBackColor = true;
            this.chkBoxOnlineVideos.Click += new System.EventHandler(this.chkBoxOnlineVideos_Click);
            // 
            // chkBoxTMDbTrailers
            // 
            this.chkBoxTMDbTrailers.AutoSize = true;
            this.chkBoxTMDbTrailers.Location = new System.Drawing.Point(16, 52);
            this.chkBoxTMDbTrailers.Name = "chkBoxTMDbTrailers";
            this.chkBoxTMDbTrailers.Size = new System.Drawing.Size(175, 17);
            this.chkBoxTMDbTrailers.TabIndex = 1;
            this.chkBoxTMDbTrailers.Text = "&TMDb Trailers (themoviedb.org)";
            this.chkBoxTMDbTrailers.UseVisualStyleBackColor = true;
            this.chkBoxTMDbTrailers.Click += new System.EventHandler(this.chkBoxTMDbTrailers_Click);
            // 
            // chkBoxLocalTrailers
            // 
            this.chkBoxLocalTrailers.AutoSize = true;
            this.chkBoxLocalTrailers.Location = new System.Drawing.Point(16, 28);
            this.chkBoxLocalTrailers.Name = "chkBoxLocalTrailers";
            this.chkBoxLocalTrailers.Size = new System.Drawing.Size(89, 17);
            this.chkBoxLocalTrailers.TabIndex = 0;
            this.chkBoxLocalTrailers.Text = "&Local Trailers";
            this.chkBoxLocalTrailers.UseVisualStyleBackColor = true;
            this.chkBoxLocalTrailers.Click += new System.EventHandler(this.chkBoxLocalTrailers_Click);
            // 
            // groupBoxLocalTrailerSettings
            // 
            this.groupBoxLocalTrailerSettings.Controls.Add(this.label9);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.label8);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.chkBoxAggressiveSearch);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.txtBoxLocalDedicatedDirectorySearchPatterns);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.label5);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.txtBoxDedicatedSubDirectories);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.label4);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.btnRemoveDedicatedDirectory);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.btnAddDedicatedDirectory);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.listBoxDedicatedDirectories);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.label3);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.chkBoxSearchLocalInDedicatedDirectory);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.txtBoxCurrentFolderSearchPatterns);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.label2);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.chkBoxSearchLocalInCurrentMediaFolder);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.txtBoxLocalAdditionalSubFolders);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.label1);
            this.groupBoxLocalTrailerSettings.Controls.Add(this.chkBoxSearchLocalInSubFolder);
            this.groupBoxLocalTrailerSettings.Location = new System.Drawing.Point(13, 141);
            this.groupBoxLocalTrailerSettings.Name = "groupBoxLocalTrailerSettings";
            this.groupBoxLocalTrailerSettings.Size = new System.Drawing.Size(528, 540);
            this.groupBoxLocalTrailerSettings.TabIndex = 2;
            this.groupBoxLocalTrailerSettings.TabStop = false;
            this.groupBoxLocalTrailerSettings.Text = "Local Trailer Settings";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(60, 508);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(415, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Files that start with \'sample\' and unsupported MediaPortal file extensions will b" +
    "e ignored";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(11, 508);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Note: ";
            // 
            // chkBoxAggressiveSearch
            // 
            this.chkBoxAggressiveSearch.AutoSize = true;
            this.chkBoxAggressiveSearch.Location = new System.Drawing.Point(11, 475);
            this.chkBoxAggressiveSearch.Name = "chkBoxAggressiveSearch";
            this.chkBoxAggressiveSearch.Size = new System.Drawing.Size(419, 17);
            this.chkBoxAggressiveSearch.TabIndex = 15;
            this.chkBoxAggressiveSearch.Text = "A&ggressive Search (use this to continue looking for trailers after matches are f" +
    "ound).";
            this.chkBoxAggressiveSearch.UseVisualStyleBackColor = true;
            this.chkBoxAggressiveSearch.Click += new System.EventHandler(this.chkBoxAggressiveSearch_Click);
            // 
            // txtBoxLocalDedicatedDirectorySearchPatterns
            // 
            this.txtBoxLocalDedicatedDirectorySearchPatterns.Location = new System.Drawing.Point(34, 439);
            this.txtBoxLocalDedicatedDirectorySearchPatterns.Name = "txtBoxLocalDedicatedDirectorySearchPatterns";
            this.txtBoxLocalDedicatedDirectorySearchPatterns.Size = new System.Drawing.Size(449, 20);
            this.txtBoxLocalDedicatedDirectorySearchPatterns.TabIndex = 14;
            this.txtBoxLocalDedicatedDirectorySearchPatterns.TextChanged += new System.EventHandler(this.txtBoxLocalDedicatedDirectorySearchPatterns_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 400);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(359, 26);
            this.label5.TabIndex = 13;
            this.label5.Text = "Enter search patterns used to find trailers in the list of dedicated directories " +
    "\r\nand their sub-directories:";
            // 
            // txtBoxDedicatedSubDirectories
            // 
            this.txtBoxDedicatedSubDirectories.Location = new System.Drawing.Point(34, 371);
            this.txtBoxDedicatedSubDirectories.Name = "txtBoxDedicatedSubDirectories";
            this.txtBoxDedicatedSubDirectories.Size = new System.Drawing.Size(449, 20);
            this.txtBoxDedicatedSubDirectories.TabIndex = 12;
            this.txtBoxDedicatedSubDirectories.TextChanged += new System.EventHandler(this.txtBoxDedicatedSubDirectories_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 348);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(328, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Enter any sub-directories to check in the list of dedicated directories:";
            // 
            // btnRemoveDedicatedDirectory
            // 
            this.btnRemoveDedicatedDirectory.Image = global::Trailers.Properties.Resources.list_remove;
            this.btnRemoveDedicatedDirectory.Location = new System.Drawing.Point(458, 299);
            this.btnRemoveDedicatedDirectory.Name = "btnRemoveDedicatedDirectory";
            this.btnRemoveDedicatedDirectory.Size = new System.Drawing.Size(25, 23);
            this.btnRemoveDedicatedDirectory.TabIndex = 10;
            this.btnRemoveDedicatedDirectory.UseVisualStyleBackColor = true;
            this.btnRemoveDedicatedDirectory.Click += new System.EventHandler(this.btnRemoveDedicatedDirectory_Click);
            // 
            // btnAddDedicatedDirectory
            // 
            this.btnAddDedicatedDirectory.Image = global::Trailers.Properties.Resources.list_add;
            this.btnAddDedicatedDirectory.Location = new System.Drawing.Point(458, 269);
            this.btnAddDedicatedDirectory.Name = "btnAddDedicatedDirectory";
            this.btnAddDedicatedDirectory.Size = new System.Drawing.Size(25, 23);
            this.btnAddDedicatedDirectory.TabIndex = 9;
            this.btnAddDedicatedDirectory.UseVisualStyleBackColor = true;
            this.btnAddDedicatedDirectory.Click += new System.EventHandler(this.btnAddDedicatedDirectory_Click);
            // 
            // listBoxDedicatedDirectories
            // 
            this.listBoxDedicatedDirectories.FormattingEnabled = true;
            this.listBoxDedicatedDirectories.Location = new System.Drawing.Point(34, 269);
            this.listBoxDedicatedDirectories.Name = "listBoxDedicatedDirectories";
            this.listBoxDedicatedDirectories.Size = new System.Drawing.Size(417, 69);
            this.listBoxDedicatedDirectories.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 243);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(399, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Enter any directories that contain trailers that are not in your current media di" +
    "rectory:";
            // 
            // chkBoxSearchLocalInDedicatedDirectory
            // 
            this.chkBoxSearchLocalInDedicatedDirectory.AutoSize = true;
            this.chkBoxSearchLocalInDedicatedDirectory.Location = new System.Drawing.Point(11, 219);
            this.chkBoxSearchLocalInDedicatedDirectory.Name = "chkBoxSearchLocalInDedicatedDirectory";
            this.chkBoxSearchLocalInDedicatedDirectory.Size = new System.Drawing.Size(224, 17);
            this.chkBoxSearchLocalInDedicatedDirectory.TabIndex = 6;
            this.chkBoxSearchLocalInDedicatedDirectory.Text = "Search for trailers in a &dedicated directory.\r\n";
            this.chkBoxSearchLocalInDedicatedDirectory.UseVisualStyleBackColor = true;
            this.chkBoxSearchLocalInDedicatedDirectory.Click += new System.EventHandler(this.chkBoxSearchLocalInDedicatedDirectory_Click);
            // 
            // txtBoxCurrentFolderSearchPatterns
            // 
            this.txtBoxCurrentFolderSearchPatterns.Location = new System.Drawing.Point(34, 91);
            this.txtBoxCurrentFolderSearchPatterns.Name = "txtBoxCurrentFolderSearchPatterns";
            this.txtBoxCurrentFolderSearchPatterns.Size = new System.Drawing.Size(449, 20);
            this.txtBoxCurrentFolderSearchPatterns.TabIndex = 2;
            this.txtBoxCurrentFolderSearchPatterns.TextChanged += new System.EventHandler(this.txtBoxCurrentFolderSearchPatterns_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(453, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Enter any search patterns that will help identify trailers in the current media f" +
    "older. Dynamic \r\nfields available: %title%, %year%, %imdb%, %filename%  (separat" +
    "e each folder by a \'|\' character):";
            // 
            // chkBoxSearchLocalInCurrentMediaFolder
            // 
            this.chkBoxSearchLocalInCurrentMediaFolder.AutoSize = true;
            this.chkBoxSearchLocalInCurrentMediaFolder.Location = new System.Drawing.Point(11, 28);
            this.chkBoxSearchLocalInCurrentMediaFolder.Name = "chkBoxSearchLocalInCurrentMediaFolder";
            this.chkBoxSearchLocalInCurrentMediaFolder.Size = new System.Drawing.Size(250, 17);
            this.chkBoxSearchLocalInCurrentMediaFolder.TabIndex = 0;
            this.chkBoxSearchLocalInCurrentMediaFolder.Text = "Search for trailers in the &current media directory.";
            this.chkBoxSearchLocalInCurrentMediaFolder.UseVisualStyleBackColor = true;
            this.chkBoxSearchLocalInCurrentMediaFolder.Click += new System.EventHandler(this.chkBoxSearchLocalInCurrentMediaFolder_Click);
            // 
            // txtBoxLocalAdditionalSubFolders
            // 
            this.txtBoxLocalAdditionalSubFolders.Location = new System.Drawing.Point(31, 185);
            this.txtBoxLocalAdditionalSubFolders.Name = "txtBoxLocalAdditionalSubFolders";
            this.txtBoxLocalAdditionalSubFolders.Size = new System.Drawing.Size(452, 20);
            this.txtBoxLocalAdditionalSubFolders.TabIndex = 5;
            this.txtBoxLocalAdditionalSubFolders.TextChanged += new System.EventHandler(this.txtBoxLocalAdditionalSubFolders_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(455, 26);
            this.label1.TabIndex = 4;
            this.label1.Text = "The local trailer provider will first try sub-folders named \'Trailer\' and \'Traile" +
    "rs\', enter any additional\r\nsub-folders to check in the current media folder (sep" +
    "arate each folder by a \'|\' character):";
            // 
            // chkBoxSearchLocalInSubFolder
            // 
            this.chkBoxSearchLocalInSubFolder.AutoSize = true;
            this.chkBoxSearchLocalInSubFolder.Location = new System.Drawing.Point(11, 126);
            this.chkBoxSearchLocalInSubFolder.Name = "chkBoxSearchLocalInSubFolder";
            this.chkBoxSearchLocalInSubFolder.Size = new System.Drawing.Size(255, 17);
            this.chkBoxSearchLocalInSubFolder.TabIndex = 3;
            this.chkBoxSearchLocalInSubFolder.Text = "Search for trailers in &sub-folders of current media.";
            this.chkBoxSearchLocalInSubFolder.UseVisualStyleBackColor = true;
            this.chkBoxSearchLocalInSubFolder.Click += new System.EventHandler(this.chkBoxSearchLocalInSubFolder_Click);
            // 
            // groupBoxManualSearchSettings
            // 
            this.groupBoxManualSearchSettings.Controls.Add(this.label7);
            this.groupBoxManualSearchSettings.Controls.Add(this.chkBoxOnlineVideosIMDbEnabled);
            this.groupBoxManualSearchSettings.Controls.Add(this.chkBoxOnlineVideosITunesEnabled);
            this.groupBoxManualSearchSettings.Controls.Add(this.txtBoxOnlineVideosYouTubeSearchString);
            this.groupBoxManualSearchSettings.Controls.Add(this.label6);
            this.groupBoxManualSearchSettings.Controls.Add(this.chkBoxOnlineVideosYouTubeEnabled);
            this.groupBoxManualSearchSettings.Location = new System.Drawing.Point(547, 141);
            this.groupBoxManualSearchSettings.Name = "groupBoxManualSearchSettings";
            this.groupBoxManualSearchSettings.Size = new System.Drawing.Size(479, 205);
            this.groupBoxManualSearchSettings.TabIndex = 3;
            this.groupBoxManualSearchSettings.TabStop = false;
            this.groupBoxManualSearchSettings.Text = "Manual Search Settings (OnlineVideos)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(376, 26);
            this.label7.TabIndex = 0;
            this.label7.Text = "If no trailers are found from local or online sources, then you can perform a \r\nm" +
    "anual search in the OnlineVideos plugin, enable/disable manual search sites:";
            // 
            // chkBoxOnlineVideosIMDbEnabled
            // 
            this.chkBoxOnlineVideosIMDbEnabled.AutoSize = true;
            this.chkBoxOnlineVideosIMDbEnabled.Location = new System.Drawing.Point(16, 162);
            this.chkBoxOnlineVideosIMDbEnabled.Name = "chkBoxOnlineVideosIMDbEnabled";
            this.chkBoxOnlineVideosIMDbEnabled.Size = new System.Drawing.Size(163, 17);
            this.chkBoxOnlineVideosIMDbEnabled.TabIndex = 5;
            this.chkBoxOnlineVideosIMDbEnabled.Text = "Enable IMD&b manual search.";
            this.chkBoxOnlineVideosIMDbEnabled.UseVisualStyleBackColor = true;
            this.chkBoxOnlineVideosIMDbEnabled.Click += new System.EventHandler(this.chkBoxOnlineVideosIMDbEnabled_Click);
            // 
            // chkBoxOnlineVideosITunesEnabled
            // 
            this.chkBoxOnlineVideosITunesEnabled.AutoSize = true;
            this.chkBoxOnlineVideosITunesEnabled.Location = new System.Drawing.Point(16, 138);
            this.chkBoxOnlineVideosITunesEnabled.Name = "chkBoxOnlineVideosITunesEnabled";
            this.chkBoxOnlineVideosITunesEnabled.Size = new System.Drawing.Size(169, 17);
            this.chkBoxOnlineVideosITunesEnabled.TabIndex = 4;
            this.chkBoxOnlineVideosITunesEnabled.Text = "Enable &iTunes manual search.";
            this.chkBoxOnlineVideosITunesEnabled.UseVisualStyleBackColor = true;
            this.chkBoxOnlineVideosITunesEnabled.Click += new System.EventHandler(this.chkBoxOnlineVideosITunesEnabled_Click);
            // 
            // txtBoxOnlineVideosYouTubeSearchString
            // 
            this.txtBoxOnlineVideosYouTubeSearchString.Location = new System.Drawing.Point(40, 106);
            this.txtBoxOnlineVideosYouTubeSearchString.Name = "txtBoxOnlineVideosYouTubeSearchString";
            this.txtBoxOnlineVideosYouTubeSearchString.Size = new System.Drawing.Size(381, 20);
            this.txtBoxOnlineVideosYouTubeSearchString.TabIndex = 3;
            this.txtBoxOnlineVideosYouTubeSearchString.TextChanged += new System.EventHandler(this.txtBoxOnlineVideosYouTubeSearchString_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(335, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Search pattern used in manual search in OnlineVideos \'YouTube\' site:";
            // 
            // chkBoxOnlineVideosYouTubeEnabled
            // 
            this.chkBoxOnlineVideosYouTubeEnabled.AutoSize = true;
            this.chkBoxOnlineVideosYouTubeEnabled.Location = new System.Drawing.Point(16, 61);
            this.chkBoxOnlineVideosYouTubeEnabled.Name = "chkBoxOnlineVideosYouTubeEnabled";
            this.chkBoxOnlineVideosYouTubeEnabled.Size = new System.Drawing.Size(181, 17);
            this.chkBoxOnlineVideosYouTubeEnabled.TabIndex = 1;
            this.chkBoxOnlineVideosYouTubeEnabled.Text = "Enable &YouTube manual search.";
            this.chkBoxOnlineVideosYouTubeEnabled.UseVisualStyleBackColor = true;
            this.chkBoxOnlineVideosYouTubeEnabled.Click += new System.EventHandler(this.chkBoxOnlineVideosYouTubeEnabled_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkBoxAutoPlayOnSingleLocalOrOnlineTrailer);
            this.groupBox4.Controls.Add(this.chkBoxSkipOnlineProvidersIfLocalFound);
            this.groupBox4.Location = new System.Drawing.Point(255, 22);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(771, 100);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "General Settings";
            // 
            // chkBoxAutoPlayOnSingleLocalOrOnlineTrailer
            // 
            this.chkBoxAutoPlayOnSingleLocalOrOnlineTrailer.AutoSize = true;
            this.chkBoxAutoPlayOnSingleLocalOrOnlineTrailer.Location = new System.Drawing.Point(16, 45);
            this.chkBoxAutoPlayOnSingleLocalOrOnlineTrailer.Name = "chkBoxAutoPlayOnSingleLocalOrOnlineTrailer";
            this.chkBoxAutoPlayOnSingleLocalOrOnlineTrailer.Size = new System.Drawing.Size(311, 17);
            this.chkBoxAutoPlayOnSingleLocalOrOnlineTrailer.TabIndex = 1;
            this.chkBoxAutoPlayOnSingleLocalOrOnlineTrailer.Text = "Auto-Play trailer if only a single local or online match is &found.";
            this.chkBoxAutoPlayOnSingleLocalOrOnlineTrailer.UseVisualStyleBackColor = true;
            this.chkBoxAutoPlayOnSingleLocalOrOnlineTrailer.Click += new System.EventHandler(this.chkBoxAutoPlayOnSingleLocalOrOnlineTrailer_Click);
            // 
            // chkBoxSkipOnlineProvidersIfLocalFound
            // 
            this.chkBoxSkipOnlineProvidersIfLocalFound.AutoSize = true;
            this.chkBoxSkipOnlineProvidersIfLocalFound.Location = new System.Drawing.Point(16, 21);
            this.chkBoxSkipOnlineProvidersIfLocalFound.Name = "chkBoxSkipOnlineProvidersIfLocalFound";
            this.chkBoxSkipOnlineProvidersIfLocalFound.Size = new System.Drawing.Size(212, 17);
            this.chkBoxSkipOnlineProvidersIfLocalFound.TabIndex = 0;
            this.chkBoxSkipOnlineProvidersIfLocalFound.Text = "Ski&p online search if local trailers found.";
            this.chkBoxSkipOnlineProvidersIfLocalFound.UseVisualStyleBackColor = true;
            this.chkBoxSkipOnlineProvidersIfLocalFound.Click += new System.EventHandler(this.chkBoxSkipOnlineProvidersIfLocalFound_Click);
            // 
            // btnApplySettings
            // 
            this.btnApplySettings.Location = new System.Drawing.Point(852, 684);
            this.btnApplySettings.Name = "btnApplySettings";
            this.btnApplySettings.Size = new System.Drawing.Size(174, 23);
            this.btnApplySettings.TabIndex = 5;
            this.btnApplySettings.Text = "&Apply Changes";
            this.btnApplySettings.UseVisualStyleBackColor = true;
            this.btnApplySettings.Click += new System.EventHandler(this.btnApplySettings_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkBoxAutoDownloadCleanup);
            this.groupBox2.Controls.Add(this.spinBoxAutoDownloadUpdateCheck);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.chkBoxAutoDownloadClips);
            this.groupBox2.Controls.Add(this.chkBoxAutoDownloadFeaturettes);
            this.groupBox2.Controls.Add(this.chkBoxAutoDownloadTeasers);
            this.groupBox2.Controls.Add(this.chkBoxAutoDownloadTrailers);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.txtBoxAutoDownloadSavePath);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.spinBoxAutoDownloadScanInterval);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.spinBoxAutoDownloadStartDelay);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.comboBoxAutoDownloadQuality);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.chkBoxAutoDownloadMyFilms);
            this.groupBox2.Controls.Add(this.chkBoxAutoDownloadMyVideos);
            this.groupBox2.Controls.Add(this.chkBoxAutoDownloadMovingPictures);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(548, 353);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(478, 325);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Automatic Download Settings";
            // 
            // chkBoxAutoDownloadCleanup
            // 
            this.chkBoxAutoDownloadCleanup.AutoSize = true;
            this.chkBoxAutoDownloadCleanup.Location = new System.Drawing.Point(29, 292);
            this.chkBoxAutoDownloadCleanup.Name = "chkBoxAutoDownloadCleanup";
            this.chkBoxAutoDownloadCleanup.Size = new System.Drawing.Size(300, 17);
            this.chkBoxAutoDownloadCleanup.TabIndex = 20;
            this.chkBoxAutoDownloadCleanup.Text = "Cleanup trailers from disk when media removed from library";
            this.chkBoxAutoDownloadCleanup.UseVisualStyleBackColor = true;
            this.chkBoxAutoDownloadCleanup.Click += new System.EventHandler(this.chkBoxAutoDownloadCleanup_Click);
            // 
            // spinBoxAutoDownloadUpdateCheck
            // 
            this.spinBoxAutoDownloadUpdateCheck.Location = new System.Drawing.Point(374, 174);
            this.spinBoxAutoDownloadUpdateCheck.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.spinBoxAutoDownloadUpdateCheck.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinBoxAutoDownloadUpdateCheck.Name = "spinBoxAutoDownloadUpdateCheck";
            this.spinBoxAutoDownloadUpdateCheck.Size = new System.Drawing.Size(71, 20);
            this.spinBoxAutoDownloadUpdateCheck.TabIndex = 16;
            this.toolTip.SetToolTip(this.spinBoxAutoDownloadUpdateCheck, "This setting is only for checking movies that previously had no trailer downloads" +
        " already i.e. movies with trailer count = 0");
            this.spinBoxAutoDownloadUpdateCheck.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinBoxAutoDownloadUpdateCheck.ValueChanged += new System.EventHandler(this.spinBoxAutoDownloadUpdateCheck_ValueChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(244, 176);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(114, 13);
            this.label15.TabIndex = 15;
            this.label15.Text = "Update Interval (days):";
            // 
            // chkBoxAutoDownloadClips
            // 
            this.chkBoxAutoDownloadClips.AutoSize = true;
            this.chkBoxAutoDownloadClips.Location = new System.Drawing.Point(283, 144);
            this.chkBoxAutoDownloadClips.Name = "chkBoxAutoDownloadClips";
            this.chkBoxAutoDownloadClips.Size = new System.Drawing.Size(48, 17);
            this.chkBoxAutoDownloadClips.TabIndex = 10;
            this.chkBoxAutoDownloadClips.Text = "Clips";
            this.chkBoxAutoDownloadClips.UseVisualStyleBackColor = true;
            this.chkBoxAutoDownloadClips.Click += new System.EventHandler(this.chkBoxAutoDownloadClips_Click);
            // 
            // chkBoxAutoDownloadFeaturettes
            // 
            this.chkBoxAutoDownloadFeaturettes.AutoSize = true;
            this.chkBoxAutoDownloadFeaturettes.Location = new System.Drawing.Point(187, 144);
            this.chkBoxAutoDownloadFeaturettes.Name = "chkBoxAutoDownloadFeaturettes";
            this.chkBoxAutoDownloadFeaturettes.Size = new System.Drawing.Size(79, 17);
            this.chkBoxAutoDownloadFeaturettes.TabIndex = 9;
            this.chkBoxAutoDownloadFeaturettes.Text = "Featurettes";
            this.chkBoxAutoDownloadFeaturettes.UseVisualStyleBackColor = true;
            this.chkBoxAutoDownloadFeaturettes.Click += new System.EventHandler(this.chkBoxAutoDownloadFeaturettes_Click);
            // 
            // chkBoxAutoDownloadTeasers
            // 
            this.chkBoxAutoDownloadTeasers.AutoSize = true;
            this.chkBoxAutoDownloadTeasers.Location = new System.Drawing.Point(106, 144);
            this.chkBoxAutoDownloadTeasers.Name = "chkBoxAutoDownloadTeasers";
            this.chkBoxAutoDownloadTeasers.Size = new System.Drawing.Size(64, 17);
            this.chkBoxAutoDownloadTeasers.TabIndex = 8;
            this.chkBoxAutoDownloadTeasers.Text = "Teasers";
            this.chkBoxAutoDownloadTeasers.UseVisualStyleBackColor = true;
            this.chkBoxAutoDownloadTeasers.Click += new System.EventHandler(this.chkBoxAutoDownloadTeasers_Click);
            // 
            // chkBoxAutoDownloadTrailers
            // 
            this.chkBoxAutoDownloadTrailers.AutoSize = true;
            this.chkBoxAutoDownloadTrailers.Location = new System.Drawing.Point(29, 144);
            this.chkBoxAutoDownloadTrailers.Name = "chkBoxAutoDownloadTrailers";
            this.chkBoxAutoDownloadTrailers.Size = new System.Drawing.Size(60, 17);
            this.chkBoxAutoDownloadTrailers.TabIndex = 7;
            this.chkBoxAutoDownloadTrailers.Text = "Trailers";
            this.chkBoxAutoDownloadTrailers.UseVisualStyleBackColor = true;
            this.chkBoxAutoDownloadTrailers.Click += new System.EventHandler(this.chkBoxAutoDownloadTrailers_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(15, 123);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(184, 13);
            this.label16.TabIndex = 6;
            this.label16.Text = "Select what video types to download:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(417, 261);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(28, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnFolderBrowseAutoDownloadPath_Click);
            // 
            // txtBoxAutoDownloadSavePath
            // 
            this.txtBoxAutoDownloadSavePath.Location = new System.Drawing.Point(29, 263);
            this.txtBoxAutoDownloadSavePath.Name = "txtBoxAutoDownloadSavePath";
            this.txtBoxAutoDownloadSavePath.ReadOnly = true;
            this.txtBoxAutoDownloadSavePath.Size = new System.Drawing.Size(381, 20);
            this.txtBoxAutoDownloadSavePath.TabIndex = 18;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 236);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(307, 13);
            this.label14.TabIndex = 17;
            this.label14.Text = "Download Directory, local trailer searches will also use this path.";
            // 
            // spinBoxAutoDownloadScanInterval
            // 
            this.spinBoxAutoDownloadScanInterval.Location = new System.Drawing.Point(151, 200);
            this.spinBoxAutoDownloadScanInterval.Maximum = new decimal(new int[] {
            168,
            0,
            0,
            0});
            this.spinBoxAutoDownloadScanInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinBoxAutoDownloadScanInterval.Name = "spinBoxAutoDownloadScanInterval";
            this.spinBoxAutoDownloadScanInterval.Size = new System.Drawing.Size(71, 20);
            this.spinBoxAutoDownloadScanInterval.TabIndex = 14;
            this.spinBoxAutoDownloadScanInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinBoxAutoDownloadScanInterval.ValueChanged += new System.EventHandler(this.spinBoxAutoDownloadScanInterval_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 203);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(108, 13);
            this.label13.TabIndex = 13;
            this.label13.Text = "Scan Interval (hours):";
            // 
            // spinBoxAutoDownloadStartDelay
            // 
            this.spinBoxAutoDownloadStartDelay.Location = new System.Drawing.Point(151, 174);
            this.spinBoxAutoDownloadStartDelay.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.spinBoxAutoDownloadStartDelay.Name = "spinBoxAutoDownloadStartDelay";
            this.spinBoxAutoDownloadStartDelay.Size = new System.Drawing.Size(71, 20);
            this.spinBoxAutoDownloadStartDelay.TabIndex = 12;
            this.spinBoxAutoDownloadStartDelay.ValueChanged += new System.EventHandler(this.spinBoxAutoDownloadStartDelay_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 177);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(105, 13);
            this.label12.TabIndex = 11;
            this.label12.Text = "Startup Delay (secs):";
            // 
            // comboBoxAutoDownloadQuality
            // 
            this.comboBoxAutoDownloadQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAutoDownloadQuality.FormattingEnabled = true;
            this.comboBoxAutoDownloadQuality.Items.AddRange(new object[] {
            "HD - 1280x720",
            "HQ - 640x360",
            "LQ - 320x240"});
            this.comboBoxAutoDownloadQuality.Location = new System.Drawing.Point(29, 92);
            this.comboBoxAutoDownloadQuality.Name = "comboBoxAutoDownloadQuality";
            this.comboBoxAutoDownloadQuality.Size = new System.Drawing.Size(183, 21);
            this.comboBoxAutoDownloadQuality.TabIndex = 5;
            this.comboBoxAutoDownloadQuality.SelectedIndexChanged += new System.EventHandler(this.comboBoxAutoDownloadQuality_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 70);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Select Video Quality Setting: ";
            // 
            // chkBoxAutoDownloadMyFilms
            // 
            this.chkBoxAutoDownloadMyFilms.AutoSize = true;
            this.chkBoxAutoDownloadMyFilms.Location = new System.Drawing.Point(263, 44);
            this.chkBoxAutoDownloadMyFilms.Name = "chkBoxAutoDownloadMyFilms";
            this.chkBoxAutoDownloadMyFilms.Size = new System.Drawing.Size(66, 17);
            this.chkBoxAutoDownloadMyFilms.TabIndex = 3;
            this.chkBoxAutoDownloadMyFilms.Text = "My Films";
            this.chkBoxAutoDownloadMyFilms.UseVisualStyleBackColor = true;
            this.chkBoxAutoDownloadMyFilms.Click += new System.EventHandler(this.chkBoxAutoDownloadMyFilms_Click);
            // 
            // chkBoxAutoDownloadMyVideos
            // 
            this.chkBoxAutoDownloadMyVideos.AutoSize = true;
            this.chkBoxAutoDownloadMyVideos.Location = new System.Drawing.Point(158, 44);
            this.chkBoxAutoDownloadMyVideos.Name = "chkBoxAutoDownloadMyVideos";
            this.chkBoxAutoDownloadMyVideos.Size = new System.Drawing.Size(75, 17);
            this.chkBoxAutoDownloadMyVideos.TabIndex = 2;
            this.chkBoxAutoDownloadMyVideos.Text = "My Videos";
            this.chkBoxAutoDownloadMyVideos.UseVisualStyleBackColor = true;
            this.chkBoxAutoDownloadMyVideos.Click += new System.EventHandler(this.chkBoxAutoDownloadMyVideos_Click);
            // 
            // chkBoxAutoDownloadMovingPictures
            // 
            this.chkBoxAutoDownloadMovingPictures.AutoSize = true;
            this.chkBoxAutoDownloadMovingPictures.Location = new System.Drawing.Point(29, 44);
            this.chkBoxAutoDownloadMovingPictures.Name = "chkBoxAutoDownloadMovingPictures";
            this.chkBoxAutoDownloadMovingPictures.Size = new System.Drawing.Size(99, 17);
            this.chkBoxAutoDownloadMovingPictures.TabIndex = 1;
            this.chkBoxAutoDownloadMovingPictures.Text = "MovingPictures";
            this.chkBoxAutoDownloadMovingPictures.UseVisualStyleBackColor = true;
            this.chkBoxAutoDownloadMovingPictures.Click += new System.EventHandler(this.chkBoxAutoDownloadMovingPictures_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(314, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Select plugins to auto-download trailers for media in local libraries:";
            // 
            // MainConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 714);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnApplySettings);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBoxManualSearchSettings);
            this.Controls.Add(this.groupBoxLocalTrailerSettings);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainConfig";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxLocalTrailerSettings.ResumeLayout(false);
            this.groupBoxLocalTrailerSettings.PerformLayout();
            this.groupBoxManualSearchSettings.ResumeLayout(false);
            this.groupBoxManualSearchSettings.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinBoxAutoDownloadUpdateCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinBoxAutoDownloadScanInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinBoxAutoDownloadStartDelay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkBoxOnlineVideos;
        private System.Windows.Forms.CheckBox chkBoxTMDbTrailers;
        private System.Windows.Forms.CheckBox chkBoxLocalTrailers;
        private System.Windows.Forms.GroupBox groupBoxLocalTrailerSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkBoxSearchLocalInSubFolder;
        private System.Windows.Forms.GroupBox groupBoxManualSearchSettings;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkBoxSearchLocalInCurrentMediaFolder;
        private System.Windows.Forms.TextBox txtBoxLocalAdditionalSubFolders;
        private System.Windows.Forms.TextBox txtBoxCurrentFolderSearchPatterns;
        private System.Windows.Forms.Button btnRemoveDedicatedDirectory;
        private System.Windows.Forms.Button btnAddDedicatedDirectory;
        private System.Windows.Forms.ListBox listBoxDedicatedDirectories;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkBoxSearchLocalInDedicatedDirectory;
        private System.Windows.Forms.TextBox txtBoxLocalDedicatedDirectorySearchPatterns;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBoxDedicatedSubDirectories;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkBoxAggressiveSearch;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkBoxAutoPlayOnSingleLocalOrOnlineTrailer;
        private System.Windows.Forms.CheckBox chkBoxSkipOnlineProvidersIfLocalFound;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkBoxOnlineVideosIMDbEnabled;
        private System.Windows.Forms.CheckBox chkBoxOnlineVideosITunesEnabled;
        private System.Windows.Forms.TextBox txtBoxOnlineVideosYouTubeSearchString;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkBoxOnlineVideosYouTubeEnabled;
        private System.Windows.Forms.Button btnApplySettings;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxAutoDownloadQuality;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkBoxAutoDownloadMyFilms;
        private System.Windows.Forms.CheckBox chkBoxAutoDownloadMyVideos;
        private System.Windows.Forms.CheckBox chkBoxAutoDownloadMovingPictures;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown spinBoxAutoDownloadStartDelay;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown spinBoxAutoDownloadScanInterval;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtBoxAutoDownloadSavePath;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkBoxAutoDownloadClips;
        private System.Windows.Forms.CheckBox chkBoxAutoDownloadFeaturettes;
        private System.Windows.Forms.CheckBox chkBoxAutoDownloadTeasers;
        private System.Windows.Forms.CheckBox chkBoxAutoDownloadTrailers;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown spinBoxAutoDownloadUpdateCheck;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox chkBoxAutoDownloadCleanup;
    }
}