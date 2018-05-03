namespace CatalogEstimating.UserControls.Estimate
{
    partial class ucpCostSummary
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
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this._gridCostSummary = new SourceGrid.Grid();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this._toolStrip.Location = new System.Drawing.Point(0, 158);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(35, 25);
            this._toolStrip.TabIndex = 1;
            this._toolStrip.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::CatalogEstimating.Properties.Resources.Refresh;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Refresh";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // _gridCostSummary
            // 
            this._gridCostSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridCostSummary.Location = new System.Drawing.Point(0, 0);
            this._gridCostSummary.Name = "_gridCostSummary";
            this._gridCostSummary.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this._gridCostSummary.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this._gridCostSummary.Size = new System.Drawing.Size(491, 272);
            this._gridCostSummary.TabIndex = 2;
            this._gridCostSummary.TabStop = true;
            this._gridCostSummary.ToolTipText = "";
            // 
            // ucpCostSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this._toolStrip);
            this.Controls.Add(this._gridCostSummary);
            this.Name = "ucpCostSummary";
            this.Size = new System.Drawing.Size(491, 272);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private SourceGrid.Grid _gridCostSummary;
    }
}
