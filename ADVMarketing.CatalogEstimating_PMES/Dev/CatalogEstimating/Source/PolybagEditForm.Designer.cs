namespace CatalogEstimating
{
    partial class PolybagEditForm
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
            this._polybagControl = new CatalogEstimating.UserControls.Polybag.ucpPolybag();
            this._toolStripContainer.ContentPanel.SuspendLayout();
            this._toolStripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStripContainer
            // 
            this._toolStripContainer.BottomToolStripPanelVisible = true;
            // 
            // _toolStripContainer.ContentPanel
            // 
            this._toolStripContainer.ContentPanel.Controls.Add(this._polybagControl);
            this._toolStripContainer.ContentPanel.Size = new System.Drawing.Size(888, 566);
            this._toolStripContainer.LeftToolStripPanelVisible = true;
            this._toolStripContainer.RightToolStripPanelVisible = true;
            this._toolStripContainer.Size = new System.Drawing.Size(888, 640);
            this._toolStripContainer.TopToolStripPanelVisible = true;
            // 
            // _polybagControl
            // 
            this._polybagControl.BackColor = System.Drawing.Color.Transparent;
            this._polybagControl.Dirty = false;
            this._polybagControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._polybagControl.IsLoading = false;
            this._polybagControl.Location = new System.Drawing.Point(0, 0);
            this._polybagControl.Name = "_polybagControl";
            this._polybagControl.Size = new System.Drawing.Size(888, 566);
            this._polybagControl.TabIndex = 0;
            // 
            // PolybagEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(888, 662);
            this.Name = "PolybagEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Polybag Group";
            this._toolStripContainer.ContentPanel.ResumeLayout(false);
            this._toolStripContainer.ResumeLayout(false);
            this._toolStripContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CatalogEstimating.UserControls.Polybag.ucpPolybag _polybagControl;
    }
}
