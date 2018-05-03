namespace CatalogEstimating.UserControls.Admin
{
    partial class uctAdministration
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
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._btnPrint = new System.Windows.Forms.ToolStripButton();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._btnSave,
            this.toolStripSeparator1,
            this._btnPrint} );
            this._toolStrip.Location = new System.Drawing.Point( 0, 190 );
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size( 64, 25 );
            this._toolStrip.TabIndex = 1;
            this._toolStrip.Text = "Administration";
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
            // uctAdministration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._toolStrip );
            this.Name = "Administration";
            this.Load += new System.EventHandler( this.uctAdministration_Load );
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

    }
}
