namespace CatalogEstimating.UserControls.Estimate
{
    partial class ucpComponents
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
            this._gridComponents = new CatalogEstimating.CustomGrids.Component.ComponentGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._btnCut = new System.Windows.Forms.ToolStripButton();
            this._btnCopy = new System.Windows.Forms.ToolStripButton();
            this._btnPaste = new System.Windows.Forms.ToolStripButton();
            this._btnDelete = new System.Windows.Forms.ToolStripButton();
            this._gridComponents.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gridComponents
            // 
            this._gridComponents.Controls.Add(this.toolStrip1);
            this._gridComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridComponents.EstimateDS = null;
            this._gridComponents.Location = new System.Drawing.Point(0, 0);
            this._gridComponents.Name = "_gridComponents";
            this._gridComponents.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this._gridComponents.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this._gridComponents.Size = new System.Drawing.Size(421, 273);
            this._gridComponents.TabIndex = 1;
            this._gridComponents.TabStop = true;
            this._gridComponents.ToolTipText = "";
            this._gridComponents.Validating += new System.ComponentModel.CancelEventHandler(this.componentGrid1_Validating);
            this._gridComponents.HScrollPositionChanged += new SourceGrid.ScrollPositionChangedEventHandler(this._gridComponents_HScrollPositionChanged);
            this._gridComponents.VScrollPositionChanged += new SourceGrid.ScrollPositionChangedEventHandler(this._gridComponents_VScrollPositionChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnCut,
            this._btnCopy,
            this._btnPaste,
            this._btnDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 126);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(104, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // _btnCut
            // 
            this._btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnCut.Image = global::CatalogEstimating.Properties.Resources.Cut;
            this._btnCut.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnCut.Name = "_btnCut";
            this._btnCut.Size = new System.Drawing.Size(23, 22);
            this._btnCut.Text = "Cut Component(s)";
            this._btnCut.Click += new System.EventHandler(this._btnCut_Click);
            // 
            // _btnCopy
            // 
            this._btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnCopy.Image = global::CatalogEstimating.Properties.Resources.Copy;
            this._btnCopy.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnCopy.Name = "_btnCopy";
            this._btnCopy.Size = new System.Drawing.Size(23, 22);
            this._btnCopy.Text = "Copy Component(s)";
            this._btnCopy.Click += new System.EventHandler(this._btnCopy_Click);
            // 
            // _btnPaste
            // 
            this._btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnPaste.Image = global::CatalogEstimating.Properties.Resources.Paste;
            this._btnPaste.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnPaste.Name = "_btnPaste";
            this._btnPaste.Size = new System.Drawing.Size(23, 22);
            this._btnPaste.Text = "Paste Component(s)";
            this._btnPaste.Click += new System.EventHandler(this._btnPaste_Click);
            // 
            // _btnDelete
            // 
            this._btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnDelete.Image = global::CatalogEstimating.Properties.Resources.Delete;
            this._btnDelete.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Size = new System.Drawing.Size(23, 22);
            this._btnDelete.Text = "Delete Component(s)";
            this._btnDelete.Click += new System.EventHandler(this._btnDelete_Click);
            // 
            // ucpComponents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._gridComponents);
            this.Name = "ucpComponents";
            this._gridComponents.ResumeLayout(false);
            this._gridComponents.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CatalogEstimating.CustomGrids.Component.ComponentGrid _gridComponents;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _btnCut;
        private System.Windows.Forms.ToolStripButton _btnCopy;
        private System.Windows.Forms.ToolStripButton _btnPaste;
        private System.Windows.Forms.ToolStripButton _btnDelete;
    }
}
