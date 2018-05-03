namespace CatalogEstimating.UserControls.Estimate
{
    partial class uctEstimate
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
            System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnAddToPolybag = new System.Windows.Forms.ToolStripButton();
            this._btnCopy = new System.Windows.Forms.ToolStripButton();
            this._btnKill = new System.Windows.Forms.ToolStripButton();
            this._btnUpload = new System.Windows.Forms.ToolStripButton();
            this._btnHome = new System.Windows.Forms.ToolStripButton();
            this._btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._btnPrint = new System.Windows.Forms.ToolStripButton();
            this._btnCancel = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tabControl
            // 
            this._tabControl.Size = new System.Drawing.Size( 422, 311 );
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size( 6, 25 );
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size( 6, 25 );
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size( 6, 25 );
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._btnAddToPolybag,
            toolStripSeparator2,
            this._btnCopy,
            toolStripSeparator3,
            this._btnKill,
            this._btnUpload,
            toolStripSeparator4,
            this._btnHome,
            this._btnSave,
            this.toolStripSeparator1,
            this._btnPrint,
            this._btnCancel} );
            this._toolStrip.Location = new System.Drawing.Point( 0, 125 );
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size( 251, 25 );
            this._toolStrip.TabIndex = 1;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _btnAddToPolybag
            // 
            this._btnAddToPolybag.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnAddToPolybag.Image = global::CatalogEstimating.Properties.Resources.NewPolybag;
            this._btnAddToPolybag.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnAddToPolybag.Name = "_btnAddToPolybag";
            this._btnAddToPolybag.Size = new System.Drawing.Size( 23, 22 );
            this._btnAddToPolybag.Text = "Add to Polybag Group";
            this._btnAddToPolybag.Click += new System.EventHandler( this._btnAddToPolybag_Click );
            // 
            // _btnCopy
            // 
            this._btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnCopy.Image = global::CatalogEstimating.Properties.Resources.CopyDialog;
            this._btnCopy.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnCopy.Name = "_btnCopy";
            this._btnCopy.Size = new System.Drawing.Size( 23, 22 );
            this._btnCopy.Text = "Copy Estimate";
            this._btnCopy.Click += new System.EventHandler( this._btnCopy_Click );
            // 
            // _btnKill
            // 
            this._btnKill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnKill.Image = global::CatalogEstimating.Properties.Resources.Kill;
            this._btnKill.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnKill.Name = "_btnKill";
            this._btnKill.Size = new System.Drawing.Size( 23, 22 );
            this._btnKill.Text = "Kill";
            this._btnKill.Click += new System.EventHandler( this._btnKill_Click );
            // 
            // _btnUpload
            // 
            this._btnUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnUpload.Image = global::CatalogEstimating.Properties.Resources.Upload;
            this._btnUpload.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnUpload.Name = "_btnUpload";
            this._btnUpload.Size = new System.Drawing.Size( 23, 22 );
            this._btnUpload.Text = "Upload";
            this._btnUpload.Click += new System.EventHandler( this._btnUpload_Click );
            // 
            // _btnHome
            // 
            this._btnHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnHome.Image = global::CatalogEstimating.Properties.Resources.Home;
            this._btnHome.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnHome.Name = "_btnHome";
            this._btnHome.Size = new System.Drawing.Size( 23, 22 );
            this._btnHome.Text = "Home";
            this._btnHome.Click += new System.EventHandler( this._btnHome_Click );
            // 
            // _btnSave
            // 
            this._btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnSave.Image = global::CatalogEstimating.Properties.Resources.Save;
            this._btnSave.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnSave.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this._btnSave.MergeIndex = 1;
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size( 23, 22 );
            this._btnSave.Text = "Save";
            this._btnSave.ToolTipText = "Save";
            this._btnSave.Click += new System.EventHandler( this._btnSave_Click );
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripSeparator1.MergeIndex = 9;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 6, 25 );
            // 
            // _btnPrint
            // 
            this._btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnPrint.Image = global::CatalogEstimating.Properties.Resources.Print;
            this._btnPrint.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnPrint.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this._btnPrint.MergeIndex = 10;
            this._btnPrint.Name = "_btnPrint";
            this._btnPrint.Size = new System.Drawing.Size( 23, 22 );
            this._btnPrint.Text = "Print";
            this._btnPrint.Click += new System.EventHandler( this._btnPrint_Click );
            // 
            // _btnCancel
            // 
            this._btnCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnCancel.Image = global::CatalogEstimating.Properties.Resources.Cancel;
            this._btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnCancel.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this._btnCancel.MergeIndex = 2;
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size( 23, 22 );
            this._btnCancel.Text = "Cancel";
            this._btnCancel.Click += new System.EventHandler( this._btnCancel_Click );
            // 
            // uctEstimate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.Controls.Add( this._toolStrip );
            this.Name = "uctEstimate";
            this.Size = new System.Drawing.Size( 422, 311 );
            this.Controls.SetChildIndex( this._tabControl, 0 );
            this.Controls.SetChildIndex( this._toolStrip, 0 );
            this._toolStrip.ResumeLayout( false );
            this._toolStrip.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton _btnPrint;
        private System.Windows.Forms.ToolStripButton _btnAddToPolybag;
        private System.Windows.Forms.ToolStripButton _btnCopy;
        private System.Windows.Forms.ToolStripButton _btnKill;
        private System.Windows.Forms.ToolStripButton _btnUpload;
        private System.Windows.Forms.ToolStripButton _btnHome;
        private System.Windows.Forms.ToolStripButton _btnCancel;

    }
}
