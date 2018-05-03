namespace CatalogEstimating
{
    partial class EstimateForm
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
            this._estimateControl = new CatalogEstimating.UserControls.Estimate.uctEstimate();
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
            this._toolStripContainer.ContentPanel.Controls.Add(this._estimateControl);
            this._toolStripContainer.ContentPanel.Size = new System.Drawing.Size(888, 566);
            this._toolStripContainer.LeftToolStripPanelVisible = true;
            this._toolStripContainer.RightToolStripPanelVisible = true;
            this._toolStripContainer.Size = new System.Drawing.Size(888, 640);
            this._toolStripContainer.TopToolStripPanelVisible = true;
            // 
            // _estimateControl
            // 
            this._estimateControl.BackColor = System.Drawing.Color.Transparent;
            this._estimateControl.Dirty = false;
            this._estimateControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._estimateControl.IsLoading = false;
            this._estimateControl.Location = new System.Drawing.Point(0, 0);
            this._estimateControl.Name = "_estimateControl";
            this._estimateControl.Size = new System.Drawing.Size(888, 566);
            this._estimateControl.TabIndex = 0;
            // 
            // EstimateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(888, 662);
            this.Name = "EstimateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Estimate";
            this._toolStripContainer.ContentPanel.ResumeLayout(false);
            this._toolStripContainer.ResumeLayout(false);
            this._toolStripContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CatalogEstimating.UserControls.Estimate.uctEstimate _estimateControl;
    }
}
