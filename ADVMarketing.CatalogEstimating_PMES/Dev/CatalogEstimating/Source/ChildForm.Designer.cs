namespace CatalogEstimating
{
    partial class ChildForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ChildForm ) );
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this._menuFileSep = new System.Windows.Forms.ToolStripSeparator();
            this._menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this._menuWindow = new System.Windows.Forms.ToolStripMenuItem();
            this._menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this._menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._statusStripDatabase = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusStripVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this._toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._statusLastAction = new System.Windows.Forms.ToolStripStatusLabel();
            this._menuStrip.SuspendLayout();
            this._statusStrip.SuspendLayout();
            this._toolStripContainer.TopToolStripPanel.SuspendLayout();
            this._toolStripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // _menuStrip
            // 
            this._menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._menuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._menuFile,
            this._menuWindow,
            this._menuHelp} );
            this._menuStrip.Location = new System.Drawing.Point( 0, 0 );
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Size = new System.Drawing.Size( 610, 24 );
            this._menuStrip.TabIndex = 0;
            // 
            // _menuFile
            // 
            this._menuFile.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._menuFileSave,
            this._menuFileSep,
            this._menuFileExit} );
            this._menuFile.Name = "_menuFile";
            this._menuFile.Size = new System.Drawing.Size( 35, 20 );
            this._menuFile.Text = "&File";
            // 
            // _menuFileSave
            // 
            this._menuFileSave.Name = "_menuFileSave";
            this._menuFileSave.Size = new System.Drawing.Size( 109, 22 );
            this._menuFileSave.Text = "&Save";
            // 
            // _menuFileSep
            // 
            this._menuFileSep.Name = "_menuFileSep";
            this._menuFileSep.Size = new System.Drawing.Size( 106, 6 );
            // 
            // _menuFileExit
            // 
            this._menuFileExit.Name = "_menuFileExit";
            this._menuFileExit.Size = new System.Drawing.Size( 109, 22 );
            this._menuFileExit.Text = "E&xit";
            this._menuFileExit.Click += new System.EventHandler( this._menuFileExit_Click );
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
            // _statusStrip
            // 
            this._statusStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._statusStripDatabase,
            this._statusStripVersion,
            this._statusLastAction} );
            this._statusStrip.Location = new System.Drawing.Point( 0, 390 );
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size( 610, 22 );
            this._statusStrip.TabIndex = 2;
            this._statusStrip.Text = "statusStrip1";
            // 
            // _statusStripDatabase
            // 
            this._statusStripDatabase.BackColor = System.Drawing.Color.Transparent;
            this._statusStripDatabase.Name = "_statusStripDatabase";
            this._statusStripDatabase.Size = new System.Drawing.Size( 78, 17 );
            this._statusStripDatabase.Text = "Connected To:";
            // 
            // _statusStripVersion
            // 
            this._statusStripVersion.BackColor = System.Drawing.Color.Transparent;
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
            this._toolStripContainer.ContentPanel.Size = new System.Drawing.Size( 610, 341 );
            this._toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._toolStripContainer.Location = new System.Drawing.Point( 0, 0 );
            this._toolStripContainer.Name = "_toolStripContainer";
            this._toolStripContainer.Size = new System.Drawing.Size( 610, 390 );
            this._toolStripContainer.TabIndex = 3;
            this._toolStripContainer.Text = "toolStripContainer1";
            // 
            // _toolStripContainer.TopToolStripPanel
            // 
            this._toolStripContainer.TopToolStripPanel.Controls.Add( this._menuStrip );
            this._toolStripContainer.TopToolStripPanel.Controls.Add( this._toolStrip );
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Location = new System.Drawing.Point( 3, 24 );
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size( 111, 25 );
            this._toolStrip.TabIndex = 2;
            this._toolStrip.Text = "MainToolbar";
            // 
            // _statusLastAction
            // 
            this._statusLastAction.Margin = new System.Windows.Forms.Padding( 15, 3, 0, 2 );
            this._statusLastAction.Name = "_statusLastAction";
            this._statusLastAction.Size = new System.Drawing.Size( 0, 17 );
            // 
            // ChildForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 610, 412 );
            this.Controls.Add( this._toolStripContainer );
            this.Controls.Add( this._statusStrip );
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
            this.MainMenuStrip = this._menuStrip;
            this.Name = "ChildForm";
            this.Text = "ChildForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.ChildForm_FormClosing );
            this._menuStrip.ResumeLayout( false );
            this._menuStrip.PerformLayout();
            this._statusStrip.ResumeLayout( false );
            this._statusStrip.PerformLayout();
            this._toolStripContainer.TopToolStripPanel.ResumeLayout( false );
            this._toolStripContainer.TopToolStripPanel.PerformLayout();
            this._toolStripContainer.ResumeLayout( false );
            this._toolStripContainer.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripMenuItem _menuFile;
        private System.Windows.Forms.ToolStripMenuItem _menuWindow;
        private System.Windows.Forms.ToolStripMenuItem _menuHelp;
        protected System.Windows.Forms.ToolStripContainer _toolStripContainer;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripStatusLabel _statusStripDatabase;
        private System.Windows.Forms.ToolStripStatusLabel _statusStripVersion;
        private System.Windows.Forms.ToolStripMenuItem _menuHelpAbout;
        protected System.Windows.Forms.ToolStripMenuItem _menuFileSave;
        private System.Windows.Forms.ToolStripSeparator _menuFileSep;
        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStripMenuItem _menuFileExit;
        private System.Windows.Forms.ToolStripStatusLabel _statusLastAction;
    }
}