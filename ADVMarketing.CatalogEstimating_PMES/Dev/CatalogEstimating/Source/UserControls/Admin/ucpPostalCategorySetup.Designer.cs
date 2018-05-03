namespace CatalogEstimating.UserControls.Admin
{
    partial class ucpPostalCategorySetup
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
            this._gridCategories = new System.Windows.Forms.DataGridView();
            this.colCategories = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pstpostalcategoryidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifieddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._ctxtMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._mnuDeleteSelectedRows = new System.Windows.Forms.ToolStripMenuItem();
            this.pstpostalcategoryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.postal = new CatalogEstimating.Datasets.Postal();
            this.pst_postalcategoryTableAdapter = new CatalogEstimating.Datasets.PostalTableAdapters.pst_postalcategoryTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this._gridCategories)).BeginInit();
            this._ctxtMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pstpostalcategoryBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.postal)).BeginInit();
            this.SuspendLayout();
            // 
            // _gridCategories
            // 
            this._gridCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this._gridCategories.AutoGenerateColumns = false;
            this._gridCategories.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridCategories.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCategories,
            this.pstpostalcategoryidDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.createdbyDataGridViewTextBoxColumn,
            this.createddateDataGridViewTextBoxColumn,
            this.modifiedbyDataGridViewTextBoxColumn,
            this.modifieddateDataGridViewTextBoxColumn});
            this._gridCategories.ContextMenuStrip = this._ctxtMenu;
            this._gridCategories.DataSource = this.pstpostalcategoryBindingSource;
            this._gridCategories.Location = new System.Drawing.Point(3, 3);
            this._gridCategories.Name = "_gridCategories";
            this._gridCategories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridCategories.Size = new System.Drawing.Size(352, 255);
            this._gridCategories.TabIndex = 6;
            this._gridCategories.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this._gridCategories_UserDeletingRow);
            this._gridCategories.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this._gridCategories_RowValidating);
            this._gridCategories.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridCategories_CellValidated);
            this._gridCategories.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this._gridCategories_UserDeletedRow);
            this._gridCategories.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this._gridCategories_CellValidating);
            this._gridCategories.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridCategories_RowValidated);
            this._gridCategories.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this._gridCategories_DefaultValuesNeeded);
            this._gridCategories.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridCategories_CellValueChanged);
            // 
            // colCategories
            // 
            this.colCategories.HeaderText = "Categories";
            this.colCategories.Name = "colCategories";
            this.colCategories.Visible = false;
            // 
            // pstpostalcategoryidDataGridViewTextBoxColumn
            // 
            this.pstpostalcategoryidDataGridViewTextBoxColumn.DataPropertyName = "pst_postalcategory_id";
            this.pstpostalcategoryidDataGridViewTextBoxColumn.HeaderText = "pst_postalcategory_id";
            this.pstpostalcategoryidDataGridViewTextBoxColumn.Name = "pstpostalcategoryidDataGridViewTextBoxColumn";
            this.pstpostalcategoryidDataGridViewTextBoxColumn.ReadOnly = true;
            this.pstpostalcategoryidDataGridViewTextBoxColumn.Visible = false;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Category Description";
            this.descriptionDataGridViewTextBoxColumn.MaxInputLength = 35;
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // createdbyDataGridViewTextBoxColumn
            // 
            this.createdbyDataGridViewTextBoxColumn.DataPropertyName = "createdby";
            this.createdbyDataGridViewTextBoxColumn.HeaderText = "createdby";
            this.createdbyDataGridViewTextBoxColumn.Name = "createdbyDataGridViewTextBoxColumn";
            this.createdbyDataGridViewTextBoxColumn.Visible = false;
            // 
            // createddateDataGridViewTextBoxColumn
            // 
            this.createddateDataGridViewTextBoxColumn.DataPropertyName = "createddate";
            this.createddateDataGridViewTextBoxColumn.HeaderText = "createddate";
            this.createddateDataGridViewTextBoxColumn.Name = "createddateDataGridViewTextBoxColumn";
            this.createddateDataGridViewTextBoxColumn.Visible = false;
            // 
            // modifiedbyDataGridViewTextBoxColumn
            // 
            this.modifiedbyDataGridViewTextBoxColumn.DataPropertyName = "modifiedby";
            this.modifiedbyDataGridViewTextBoxColumn.HeaderText = "modifiedby";
            this.modifiedbyDataGridViewTextBoxColumn.Name = "modifiedbyDataGridViewTextBoxColumn";
            this.modifiedbyDataGridViewTextBoxColumn.Visible = false;
            // 
            // modifieddateDataGridViewTextBoxColumn
            // 
            this.modifieddateDataGridViewTextBoxColumn.DataPropertyName = "modifieddate";
            this.modifieddateDataGridViewTextBoxColumn.HeaderText = "modifieddate";
            this.modifieddateDataGridViewTextBoxColumn.Name = "modifieddateDataGridViewTextBoxColumn";
            this.modifieddateDataGridViewTextBoxColumn.Visible = false;
            // 
            // _ctxtMenu
            // 
            this._ctxtMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mnuDeleteSelectedRows});
            this._ctxtMenu.Name = "_ctxtMenu";
            this._ctxtMenu.Size = new System.Drawing.Size(198, 26);
            // 
            // _mnuDeleteSelectedRows
            // 
            this._mnuDeleteSelectedRows.Name = "_mnuDeleteSelectedRows";
            this._mnuDeleteSelectedRows.Size = new System.Drawing.Size(197, 22);
            this._mnuDeleteSelectedRows.Text = "Delete Selected Row(s)";
            this._mnuDeleteSelectedRows.Click += new System.EventHandler(this._mnuDeleteSelectedRows_Click);
            // 
            // pstpostalcategoryBindingSource
            // 
            this.pstpostalcategoryBindingSource.DataMember = "pst_postalcategory";
            this.pstpostalcategoryBindingSource.DataSource = this.postal;
            this.pstpostalcategoryBindingSource.Sort = "description";
            // 
            // postal
            // 
            this.postal.DataSetName = "Postal";
            this.postal.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pst_postalcategoryTableAdapter
            // 
            this.pst_postalcategoryTableAdapter.ClearBeforeFill = true;
            // 
            // ucpPostalCategorySetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._gridCategories);
            this.Name = "ucpPostalCategorySetup";
            this.Size = new System.Drawing.Size(360, 261);
            ((System.ComponentModel.ISupportInitialize)(this._gridCategories)).EndInit();
            this._ctxtMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pstpostalcategoryBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.postal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _gridCategories;
        private System.Windows.Forms.BindingSource pstpostalcategoryBindingSource;
        private CatalogEstimating.Datasets.Postal postal;
        private CatalogEstimating.Datasets.PostalTableAdapters.pst_postalcategoryTableAdapter pst_postalcategoryTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategories;
        private System.Windows.Forms.DataGridViewTextBoxColumn pstpostalcategoryidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createddateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifieddateDataGridViewTextBoxColumn;
        private System.Windows.Forms.ContextMenuStrip _ctxtMenu;
        private System.Windows.Forms.ToolStripMenuItem _mnuDeleteSelectedRows;
    }
}
