namespace CatalogEstimating.UserControls.Admin
{
    partial class uctAdminPostal
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._btnCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._btnCancel} );
            this.toolStrip1.Location = new System.Drawing.Point( 0, 79 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size( 33, 25 );
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // _btnCancel
            // 
            this._btnCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnCancel.Enabled = false;
            this._btnCancel.Image = global::CatalogEstimating.Properties.Resources.Cancel;
            this._btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnCancel.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this._btnCancel.MergeIndex = 1;
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size( 23, 22 );
            this._btnCancel.Text = "Cancel";
            this._btnCancel.Click += new System.EventHandler( this._btnCancel_Click );
            // 
            // uctAdminPostal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this.toolStrip1 );
            this.Name = "uctAdminPostal";
            this.Controls.SetChildIndex( this._tabControl, 0 );
            this.Controls.SetChildIndex( this.toolStrip1, 0 );
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _btnCancel;

    }
}
