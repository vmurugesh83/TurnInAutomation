namespace CatalogEstimating.UserControls.Estimate
{
    partial class ucpMappings
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
            this._gridDistMapping = new CatalogEstimating.CustomGrids.Component.DistMappingGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._btnDelete = new System.Windows.Forms.ToolStripButton();
            this._lblInfoText = new System.Windows.Forms.Label();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._gridPopupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._mnuDelPackagesItem = new System.Windows.Forms.ToolStripMenuItem();
            this._gridTotals = new SourceGrid.Grid();
            this._gridDistMapping.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this._gridPopupMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gridDistMapping
            // 
            this._gridDistMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._gridDistMapping.Controls.Add(this.toolStrip1);
            this._gridDistMapping.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._errorProvider.SetIconAlignment(this._gridDistMapping, System.Windows.Forms.ErrorIconAlignment.TopLeft);
            this._errorProvider.SetIconPadding(this._gridDistMapping, -120);
            this._gridDistMapping.Location = new System.Drawing.Point(4, 17);
            this._gridDistMapping.Name = "_gridDistMapping";
            this._gridDistMapping.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this._gridDistMapping.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this._gridDistMapping.Size = new System.Drawing.Size(808, 204);
            this._gridDistMapping.TabIndex = 0;
            this._gridDistMapping.TabStop = true;
            this._gridDistMapping.ToolTipText = "";
            this._gridDistMapping.VScrollPositionChanged += new SourceGrid.ScrollPositionChangedEventHandler(this._gridDistMapping_VScrollPositionChanged);
            this._gridDistMapping.HScrollPositionChanged += new SourceGrid.ScrollPositionChangedEventHandler(this._gridDistMapping_HScrollPositionChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnDelete});
            this.toolStrip1.Location = new System.Drawing.Point(200, 120);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(35, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // _btnDelete
            // 
            this._btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnDelete.Image = global::CatalogEstimating.Properties.Resources.Delete;
            this._btnDelete.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Size = new System.Drawing.Size(23, 22);
            this._btnDelete.Text = "Delete Packages";
            this._btnDelete.Click += new System.EventHandler(this._btnDelete_Click);
            // 
            // _lblInfoText
            // 
            this._lblInfoText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lblInfoText.AutoSize = true;
            this._lblInfoText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblInfoText.ForeColor = System.Drawing.Color.Blue;
            this._lblInfoText.Location = new System.Drawing.Point(1, 0);
            this._lblInfoText.Name = "_lblInfoText";
            this._lblInfoText.Size = new System.Drawing.Size(348, 13);
            this._lblInfoText.TabIndex = 9;
            this._lblInfoText.Text = "At least one component must be entered to enable mapping.";
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // _gridPopupMenu
            // 
            this._gridPopupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mnuDelPackagesItem});
            this._gridPopupMenu.Name = "_gridPopupMenu";
            this._gridPopupMenu.Size = new System.Drawing.Size(209, 26);
            // 
            // _mnuDelPackagesItem
            // 
            this._mnuDelPackagesItem.Image = global::CatalogEstimating.Properties.Resources.Delete;
            this._mnuDelPackagesItem.Name = "_mnuDelPackagesItem";
            this._mnuDelPackagesItem.Size = new System.Drawing.Size(208, 22);
            this._mnuDelPackagesItem.Text = "Delete Selected Packages";
            this._mnuDelPackagesItem.Click += new System.EventHandler(this._btnDelete_Click);
            // 
            // _gridTotals
            // 
            this._gridTotals.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._gridTotals.Enabled = false;
            this._gridTotals.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._gridTotals.Location = new System.Drawing.Point(4, 228);
            this._gridTotals.Name = "_gridTotals";
            this._gridTotals.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this._gridTotals.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this._gridTotals.Size = new System.Drawing.Size(808, 40);
            this._gridTotals.TabIndex = 10;
            this._gridTotals.TabStop = true;
            this._gridTotals.ToolTipText = "";
            // 
            // ucpMappings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._gridTotals);
            this.Controls.Add(this._lblInfoText);
            this.Controls.Add(this._gridDistMapping);
            this.Name = "ucpMappings";
            this.Size = new System.Drawing.Size(821, 269);
            this.Load += new System.EventHandler(this.ucpMappings_Load);
            this.Resize += new System.EventHandler(this.ucpMappings_Resize);
            this._gridDistMapping.ResumeLayout(false);
            this._gridDistMapping.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this._gridPopupMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CatalogEstimating.CustomGrids.Component.DistMappingGrid _gridDistMapping;
        private System.Windows.Forms.Label _lblInfoText;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _btnDelete;
        private System.Windows.Forms.ContextMenuStrip _gridPopupMenu;
        private System.Windows.Forms.ToolStripMenuItem _mnuDelPackagesItem;
        private SourceGrid.Grid _gridTotals;
    }
}
