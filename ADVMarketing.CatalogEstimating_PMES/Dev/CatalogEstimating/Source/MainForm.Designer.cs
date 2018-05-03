namespace CatalogEstimating
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MainForm ) );
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._statusStripDatabase = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusStripVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this._toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this._tabControl = new System.Windows.Forms.TabControl();
            this._tabEstimateSearch = new System.Windows.Forms.TabPage();
            this._estimateSearchControl = new CatalogEstimating.UserControls.Main.ucpEstimateSearch();
            this._tabComponentSearch = new System.Windows.Forms.TabPage();
            this._componentSearchControl = new CatalogEstimating.UserControls.Main.ucpComponentSearch();
            this._tabPolybagSearch = new System.Windows.Forms.TabPage();
            this._polybagSearchControl = new CatalogEstimating.UserControls.Main.ucpPolybagSearch();
            this._tabEstimateUpload = new System.Windows.Forms.TabPage();
            this._estimateUploadControl = new CatalogEstimating.UserControls.Main.ucpEstimateUpload();
            this._tabReports = new System.Windows.Forms.TabPage();
            this._reportsControl = new CatalogEstimating.UserControls.Main.ucpReports();
            this._tabAdministration = new System.Windows.Forms.TabPage();
            this._adminControl = new CatalogEstimating.UserControls.Admin.uctAdministration();
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileNewEstimate = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileNewPolybag = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileOpenReadOnly = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileView = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileSep0 = new System.Windows.Forms.ToolStripSeparator();
            this._menuFileDelete = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileUpload = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this._menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this._menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this._menuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this._menuEditSync = new System.Windows.Forms.ToolStripMenuItem();
            this._menuEditSyncEstimates = new System.Windows.Forms.ToolStripMenuItem();
            this._menuEditSyncRates = new System.Windows.Forms.ToolStripMenuItem();
            this._menuEditAddtoPolybag = new System.Windows.Forms.ToolStripMenuItem();
            this._menuWindow = new System.Windows.Forms.ToolStripMenuItem();
            this._menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this._menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._statusLastAction = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusStrip.SuspendLayout();
            this._toolStripContainer.ContentPanel.SuspendLayout();
            this._toolStripContainer.TopToolStripPanel.SuspendLayout();
            this._toolStripContainer.SuspendLayout();
            this._tabControl.SuspendLayout();
            this._tabEstimateSearch.SuspendLayout();
            this._tabComponentSearch.SuspendLayout();
            this._tabPolybagSearch.SuspendLayout();
            this._tabEstimateUpload.SuspendLayout();
            this._tabReports.SuspendLayout();
            this._tabAdministration.SuspendLayout();
            this._menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _statusStrip
            // 
            this._statusStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._statusStripDatabase,
            this._statusStripVersion,
            this._statusLastAction} );
            this._statusStrip.Location = new System.Drawing.Point( 0, 644 );
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size( 892, 22 );
            this._statusStrip.TabIndex = 2;
            this._statusStrip.Text = "statusStrip1";
            // 
            // _statusStripDatabase
            // 
            this._statusStripDatabase.BackColor = System.Drawing.Color.Transparent;
            this._statusStripDatabase.Name = "_statusStripDatabase";
            this._statusStripDatabase.Size = new System.Drawing.Size( 81, 17 );
            this._statusStripDatabase.Text = "Connected To: ";
            // 
            // _statusStripVersion
            // 
            this._statusStripVersion.Margin = new System.Windows.Forms.Padding( 15, 3, 0, 2 );
            this._statusStripVersion.Name = "_statusStripVersion";
            this._statusStripVersion.Size = new System.Drawing.Size( 46, 17 );
            this._statusStripVersion.Text = "Version:";
            // 
            // _toolStripContainer
            // 
            // 
            // _toolStripContainer.ContentPanel
            // 
            this._toolStripContainer.ContentPanel.Controls.Add( this._tabControl );
            this._toolStripContainer.ContentPanel.Padding = new System.Windows.Forms.Padding( 5 );
            this._toolStripContainer.ContentPanel.Size = new System.Drawing.Size( 892, 595 );
            this._toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._toolStripContainer.Location = new System.Drawing.Point( 0, 0 );
            this._toolStripContainer.Name = "_toolStripContainer";
            this._toolStripContainer.Size = new System.Drawing.Size( 892, 644 );
            this._toolStripContainer.TabIndex = 5;
            this._toolStripContainer.Text = "toolStripContainer1";
            // 
            // _toolStripContainer.TopToolStripPanel
            // 
            this._toolStripContainer.TopToolStripPanel.Controls.Add( this._menuStrip );
            this._toolStripContainer.TopToolStripPanel.Controls.Add( this._toolStrip );
            // 
            // _tabControl
            // 
            this._tabControl.Controls.Add( this._tabEstimateSearch );
            this._tabControl.Controls.Add( this._tabComponentSearch );
            this._tabControl.Controls.Add( this._tabPolybagSearch );
            this._tabControl.Controls.Add( this._tabEstimateUpload );
            this._tabControl.Controls.Add( this._tabReports );
            this._tabControl.Controls.Add( this._tabAdministration );
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.Location = new System.Drawing.Point( 5, 5 );
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size( 882, 585 );
            this._tabControl.TabIndex = 0;
            this._tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler( this._tabControl_Selecting );
            this._tabControl.SelectedIndexChanged += new System.EventHandler( this._tabControl_SelectedIndexChanged );
            // 
            // _tabEstimateSearch
            // 
            this._tabEstimateSearch.Controls.Add( this._estimateSearchControl );
            this._tabEstimateSearch.Location = new System.Drawing.Point( 4, 22 );
            this._tabEstimateSearch.Name = "_tabEstimateSearch";
            this._tabEstimateSearch.Padding = new System.Windows.Forms.Padding( 3 );
            this._tabEstimateSearch.Size = new System.Drawing.Size( 874, 559 );
            this._tabEstimateSearch.TabIndex = 0;
            this._tabEstimateSearch.Text = "Estimate Search";
            this._tabEstimateSearch.UseVisualStyleBackColor = true;
            // 
            // _estimateSearchControl
            // 
            this._estimateSearchControl.BackColor = System.Drawing.Color.Transparent;
            this._estimateSearchControl.Dirty = false;
            this._estimateSearchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._estimateSearchControl.IsLoading = false;
            this._estimateSearchControl.Location = new System.Drawing.Point( 3, 3 );
            this._estimateSearchControl.Name = "_estimateSearchControl";
            this._estimateSearchControl.Size = new System.Drawing.Size( 868, 553 );
            this._estimateSearchControl.TabIndex = 0;
            // 
            // _tabComponentSearch
            // 
            this._tabComponentSearch.Controls.Add( this._componentSearchControl );
            this._tabComponentSearch.Location = new System.Drawing.Point( 4, 22 );
            this._tabComponentSearch.Name = "_tabComponentSearch";
            this._tabComponentSearch.Padding = new System.Windows.Forms.Padding( 3 );
            this._tabComponentSearch.Size = new System.Drawing.Size( 874, 559 );
            this._tabComponentSearch.TabIndex = 1;
            this._tabComponentSearch.Text = "Component Search";
            this._tabComponentSearch.UseVisualStyleBackColor = true;
            // 
            // _componentSearchControl
            // 
            this._componentSearchControl.BackColor = System.Drawing.Color.Transparent;
            this._componentSearchControl.Dirty = false;
            this._componentSearchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._componentSearchControl.IsLoading = false;
            this._componentSearchControl.Location = new System.Drawing.Point( 3, 3 );
            this._componentSearchControl.Name = "_componentSearchControl";
            this._componentSearchControl.Size = new System.Drawing.Size( 868, 553 );
            this._componentSearchControl.TabIndex = 0;
            // 
            // _tabPolybagSearch
            // 
            this._tabPolybagSearch.Controls.Add( this._polybagSearchControl );
            this._tabPolybagSearch.Location = new System.Drawing.Point( 4, 22 );
            this._tabPolybagSearch.Name = "_tabPolybagSearch";
            this._tabPolybagSearch.Size = new System.Drawing.Size( 874, 559 );
            this._tabPolybagSearch.TabIndex = 5;
            this._tabPolybagSearch.Text = "Polybag Search";
            this._tabPolybagSearch.UseVisualStyleBackColor = true;
            // 
            // _polybagSearchControl
            // 
            this._polybagSearchControl.BackColor = System.Drawing.Color.Transparent;
            this._polybagSearchControl.Dirty = false;
            this._polybagSearchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._polybagSearchControl.IsLoading = false;
            this._polybagSearchControl.Location = new System.Drawing.Point( 0, 0 );
            this._polybagSearchControl.Name = "_polybagSearchControl";
            this._polybagSearchControl.Size = new System.Drawing.Size( 874, 559 );
            this._polybagSearchControl.TabIndex = 0;
            // 
            // _tabEstimateUpload
            // 
            this._tabEstimateUpload.Controls.Add( this._estimateUploadControl );
            this._tabEstimateUpload.Location = new System.Drawing.Point( 4, 22 );
            this._tabEstimateUpload.Name = "_tabEstimateUpload";
            this._tabEstimateUpload.Size = new System.Drawing.Size( 874, 559 );
            this._tabEstimateUpload.TabIndex = 6;
            this._tabEstimateUpload.Text = "Pending Estimate Upload";
            this._tabEstimateUpload.UseVisualStyleBackColor = true;
            // 
            // _estimateUploadControl
            // 
            this._estimateUploadControl.AutoSize = true;
            this._estimateUploadControl.BackColor = System.Drawing.Color.Transparent;
            this._estimateUploadControl.Dirty = false;
            this._estimateUploadControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._estimateUploadControl.IsLoading = false;
            this._estimateUploadControl.Location = new System.Drawing.Point( 0, 0 );
            this._estimateUploadControl.Name = "_estimateUploadControl";
            this._estimateUploadControl.Padding = new System.Windows.Forms.Padding( 5 );
            this._estimateUploadControl.Size = new System.Drawing.Size( 874, 559 );
            this._estimateUploadControl.TabIndex = 0;
            // 
            // _tabReports
            // 
            this._tabReports.Controls.Add( this._reportsControl );
            this._tabReports.Location = new System.Drawing.Point( 4, 22 );
            this._tabReports.Name = "_tabReports";
            this._tabReports.Size = new System.Drawing.Size( 874, 559 );
            this._tabReports.TabIndex = 3;
            this._tabReports.Text = "Reports";
            this._tabReports.UseVisualStyleBackColor = true;
            // 
            // _reportsControl
            // 
            this._reportsControl.BackColor = System.Drawing.Color.Transparent;
            this._reportsControl.Dirty = false;
            this._reportsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._reportsControl.IsLoading = false;
            this._reportsControl.Location = new System.Drawing.Point( 0, 0 );
            this._reportsControl.Name = "_reportsControl";
            this._reportsControl.Size = new System.Drawing.Size( 874, 559 );
            this._reportsControl.TabIndex = 0;
            // 
            // _tabAdministration
            // 
            this._tabAdministration.Controls.Add( this._adminControl );
            this._tabAdministration.Location = new System.Drawing.Point( 4, 22 );
            this._tabAdministration.Name = "_tabAdministration";
            this._tabAdministration.Size = new System.Drawing.Size( 874, 559 );
            this._tabAdministration.TabIndex = 2;
            this._tabAdministration.Text = "Administration";
            this._tabAdministration.UseVisualStyleBackColor = true;
            // 
            // _adminControl
            // 
            this._adminControl.BackColor = System.Drawing.Color.Transparent;
            this._adminControl.Dirty = false;
            this._adminControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._adminControl.IsLoading = false;
            this._adminControl.Location = new System.Drawing.Point( 0, 0 );
            this._adminControl.Name = "_adminControl";
            this._adminControl.Size = new System.Drawing.Size( 874, 559 );
            this._adminControl.TabIndex = 0;
            // 
            // _menuStrip
            // 
            this._menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._menuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this._menuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._menuFile,
            this._menuEdit,
            this._menuWindow,
            this._menuHelp} );
            this._menuStrip.Location = new System.Drawing.Point( 0, 0 );
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Size = new System.Drawing.Size( 892, 24 );
            this._menuStrip.TabIndex = 0;
            // 
            // _menuFile
            // 
            this._menuFile.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._menuFileNew,
            this._menuFileOpen,
            this._menuFileOpenReadOnly,
            this._menuFileView,
            this._menuFileSep0,
            this._menuFileDelete,
            this._menuFileUpload,
            this._menuFileSep1,
            this._menuFileExit} );
            this._menuFile.Name = "_menuFile";
            this._menuFile.Size = new System.Drawing.Size( 35, 20 );
            this._menuFile.Text = "&File";
            // 
            // _menuFileNew
            // 
            this._menuFileNew.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._menuFileNewEstimate,
            this._menuFileNewPolybag} );
            this._menuFileNew.Name = "_menuFileNew";
            this._menuFileNew.Size = new System.Drawing.Size( 171, 22 );
            this._menuFileNew.Text = "&New";
            // 
            // _menuFileNewEstimate
            // 
            this._menuFileNewEstimate.Name = "_menuFileNewEstimate";
            this._menuFileNewEstimate.Size = new System.Drawing.Size( 126, 22 );
            this._menuFileNewEstimate.Text = "&Estimate";
            this._menuFileNewEstimate.Click += new System.EventHandler( this._menuFileNewEstimate_Click );
            // 
            // _menuFileNewPolybag
            // 
            this._menuFileNewPolybag.Name = "_menuFileNewPolybag";
            this._menuFileNewPolybag.Size = new System.Drawing.Size( 126, 22 );
            this._menuFileNewPolybag.Text = "&Polybag";
            this._menuFileNewPolybag.Click += new System.EventHandler( this._menuFileNewPolybag_Click );
            // 
            // _menuFileOpen
            // 
            this._menuFileOpen.Name = "_menuFileOpen";
            this._menuFileOpen.Size = new System.Drawing.Size( 171, 22 );
            this._menuFileOpen.Text = "&Open";
            this._menuFileOpen.Visible = false;
            // 
            // _menuFileOpenReadOnly
            // 
            this._menuFileOpenReadOnly.Name = "_menuFileOpenReadOnly";
            this._menuFileOpenReadOnly.Size = new System.Drawing.Size( 171, 22 );
            this._menuFileOpenReadOnly.Text = "Open - &Read Only";
            this._menuFileOpenReadOnly.Visible = false;
            // 
            // _menuFileView
            // 
            this._menuFileView.Name = "_menuFileView";
            this._menuFileView.Size = new System.Drawing.Size( 171, 22 );
            this._menuFileView.Text = "&View";
            this._menuFileView.Visible = false;
            // 
            // _menuFileSep0
            // 
            this._menuFileSep0.Name = "_menuFileSep0";
            this._menuFileSep0.Size = new System.Drawing.Size( 168, 6 );
            this._menuFileSep0.Visible = false;
            // 
            // _menuFileDelete
            // 
            this._menuFileDelete.Name = "_menuFileDelete";
            this._menuFileDelete.Size = new System.Drawing.Size( 171, 22 );
            this._menuFileDelete.Text = "&Delete";
            this._menuFileDelete.Visible = false;
            // 
            // _menuFileUpload
            // 
            this._menuFileUpload.Name = "_menuFileUpload";
            this._menuFileUpload.Size = new System.Drawing.Size( 171, 22 );
            this._menuFileUpload.Text = "&Upload";
            this._menuFileUpload.Visible = false;
            // 
            // _menuFileSep1
            // 
            this._menuFileSep1.Name = "_menuFileSep1";
            this._menuFileSep1.Size = new System.Drawing.Size( 168, 6 );
            this._menuFileSep1.Visible = false;
            // 
            // _menuFileExit
            // 
            this._menuFileExit.Name = "_menuFileExit";
            this._menuFileExit.Size = new System.Drawing.Size( 171, 22 );
            this._menuFileExit.Text = "&Exit";
            this._menuFileExit.Click += new System.EventHandler( this._menuFileExit_Click );
            // 
            // _menuEdit
            // 
            this._menuEdit.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._menuEditCopy,
            this._menuEditSync,
            this._menuEditAddtoPolybag} );
            this._menuEdit.Name = "_menuEdit";
            this._menuEdit.Size = new System.Drawing.Size( 37, 20 );
            this._menuEdit.Text = "&Edit";
            this._menuEdit.Visible = false;
            // 
            // _menuEditCopy
            // 
            this._menuEditCopy.Name = "_menuEditCopy";
            this._menuEditCopy.Size = new System.Drawing.Size( 158, 22 );
            this._menuEditCopy.Text = "&Copy";
            // 
            // _menuEditSync
            // 
            this._menuEditSync.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._menuEditSyncEstimates,
            this._menuEditSyncRates} );
            this._menuEditSync.Name = "_menuEditSync";
            this._menuEditSync.Size = new System.Drawing.Size( 158, 22 );
            this._menuEditSync.Text = "&Sync";
            // 
            // _menuEditSyncEstimates
            // 
            this._menuEditSyncEstimates.Name = "_menuEditSyncEstimates";
            this._menuEditSyncEstimates.Size = new System.Drawing.Size( 131, 22 );
            this._menuEditSyncEstimates.Text = "&Estimates";
            // 
            // _menuEditSyncRates
            // 
            this._menuEditSyncRates.Name = "_menuEditSyncRates";
            this._menuEditSyncRates.Size = new System.Drawing.Size( 131, 22 );
            this._menuEditSyncRates.Text = "&Rates";
            // 
            // _menuEditAddtoPolybag
            // 
            this._menuEditAddtoPolybag.Name = "_menuEditAddtoPolybag";
            this._menuEditAddtoPolybag.Size = new System.Drawing.Size( 158, 22 );
            this._menuEditAddtoPolybag.Text = "&Add to Polybag";
            // 
            // _menuWindow
            // 
            this._menuWindow.Name = "_menuWindow";
            this._menuWindow.Size = new System.Drawing.Size( 57, 20 );
            this._menuWindow.Text = "&Window";
            // 
            // _menuHelp
            // 
            this._menuHelp.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._menuHelpAbout} );
            this._menuHelp.Name = "_menuHelp";
            this._menuHelp.Size = new System.Drawing.Size( 40, 20 );
            this._menuHelp.Text = "&Help";
            // 
            // _menuHelpAbout
            // 
            this._menuHelpAbout.Name = "_menuHelpAbout";
            this._menuHelpAbout.Size = new System.Drawing.Size( 114, 22 );
            this._menuHelpAbout.Text = "&About";
            this._menuHelpAbout.Click += new System.EventHandler( this._menuHelpAbout_Click );
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Location = new System.Drawing.Point( 3, 24 );
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size( 111, 25 );
            this._toolStrip.TabIndex = 1;
            this._toolStrip.Text = "MainToolbar";
            // 
            // _statusLastAction
            // 
            this._statusLastAction.Margin = new System.Windows.Forms.Padding( 15, 3, 0, 2 );
            this._statusLastAction.Name = "_statusLastAction";
            this._statusLastAction.Size = new System.Drawing.Size( 0, 17 );
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size( 892, 666 );
            this.Controls.Add( this._toolStripContainer );
            this.Controls.Add( this._statusStrip );
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
            this.MainMenuStrip = this._menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bon-Ton Print Media Estimating System";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler( this.MainForm_FormClosed );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.MainForm_FormClosing );
            this.Load += new System.EventHandler( this.MainForm_Load );
            this._statusStrip.ResumeLayout( false );
            this._statusStrip.PerformLayout();
            this._toolStripContainer.ContentPanel.ResumeLayout( false );
            this._toolStripContainer.TopToolStripPanel.ResumeLayout( false );
            this._toolStripContainer.TopToolStripPanel.PerformLayout();
            this._toolStripContainer.ResumeLayout( false );
            this._toolStripContainer.PerformLayout();
            this._tabControl.ResumeLayout( false );
            this._tabEstimateSearch.ResumeLayout( false );
            this._tabComponentSearch.ResumeLayout( false );
            this._tabPolybagSearch.ResumeLayout( false );
            this._tabEstimateUpload.ResumeLayout( false );
            this._tabEstimateUpload.PerformLayout();
            this._tabReports.ResumeLayout( false );
            this._tabAdministration.ResumeLayout( false );
            this._menuStrip.ResumeLayout( false );
            this._menuStrip.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStripMenuItem _menuFile;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripMenuItem _menuWindow;
        private System.Windows.Forms.ToolStripMenuItem _menuHelp;
        private System.Windows.Forms.ToolStripMenuItem _menuFileNew;
        private System.Windows.Forms.ToolStripMenuItem _menuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem _menuFileOpenReadOnly;
        private System.Windows.Forms.ToolStripMenuItem _menuFileNewEstimate;
        private System.Windows.Forms.ToolStripMenuItem _menuFileNewPolybag;
        private System.Windows.Forms.ToolStripMenuItem _menuFileView;
        private System.Windows.Forms.ToolStripSeparator _menuFileSep0;
        private System.Windows.Forms.ToolStripMenuItem _menuFileDelete;
        private System.Windows.Forms.ToolStripMenuItem _menuFileUpload;
        private System.Windows.Forms.ToolStripSeparator _menuFileSep1;
        private System.Windows.Forms.ToolStripMenuItem _menuFileExit;
        private System.Windows.Forms.ToolStripMenuItem _menuEdit;
        private System.Windows.Forms.ToolStripMenuItem _menuEditCopy;
        private System.Windows.Forms.ToolStripMenuItem _menuEditSync;
        private System.Windows.Forms.ToolStripMenuItem _menuEditSyncEstimates;
        private System.Windows.Forms.ToolStripMenuItem _menuEditSyncRates;
        private System.Windows.Forms.ToolStripMenuItem _menuEditAddtoPolybag;
        private System.Windows.Forms.ToolStripMenuItem _menuHelpAbout;
        private System.Windows.Forms.ToolStripContainer _toolStripContainer;
        private System.Windows.Forms.ToolStripStatusLabel _statusStripDatabase;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _tabEstimateSearch;
        private CatalogEstimating.UserControls.Main.ucpEstimateSearch _estimateSearchControl;
        private System.Windows.Forms.TabPage _tabComponentSearch;
        private CatalogEstimating.UserControls.Main.ucpComponentSearch _componentSearchControl;
        private System.Windows.Forms.TabPage _tabAdministration;
        private CatalogEstimating.UserControls.Admin.uctAdministration _adminControl;
        private System.Windows.Forms.TabPage _tabReports;
        private CatalogEstimating.UserControls.Main.ucpReports _reportsControl;
        private System.Windows.Forms.TabPage _tabPolybagSearch;
        private CatalogEstimating.UserControls.Main.ucpPolybagSearch _polybagSearchControl;
        private System.Windows.Forms.TabPage _tabEstimateUpload;
        private CatalogEstimating.UserControls.Main.ucpEstimateUpload _estimateUploadControl;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripStatusLabel _statusStripVersion;
        private System.Windows.Forms.ToolStripStatusLabel _statusLastAction;

    }
}

