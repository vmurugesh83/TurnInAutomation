namespace CatalogEstimating
{
    partial class CopyDescriptionDialog
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
            this._gridCopies = new System.Windows.Forms.DataGridView();
            this._btnSave = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.calendarColumn1 = new CatalogEstimating.CustomControls.CalendarColumn();
            this._btnQuit = new System.Windows.Forms.Button();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRunDate = new CatalogEstimating.CustomControls.CalendarColumn();
            ((System.ComponentModel.ISupportInitialize)(this._gridCopies)).BeginInit();
            this.SuspendLayout();
            // 
            // _gridCopies
            // 
            this._gridCopies.AllowUserToAddRows = false;
            this._gridCopies.AllowUserToDeleteRows = false;
            this._gridCopies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._gridCopies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridCopies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridCopies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDescription,
            this.colComments,
            this.colRunDate});
            this._gridCopies.Location = new System.Drawing.Point(12, 12);
            this._gridCopies.Name = "_gridCopies";
            this._gridCopies.RowHeadersVisible = false;
            this._gridCopies.Size = new System.Drawing.Size(490, 299);
            this._gridCopies.TabIndex = 0;
            this._gridCopies.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this._gridCopies_CellValidating);
            // 
            // _btnSave
            // 
            this._btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnSave.Location = new System.Drawing.Point(306, 324);
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size(80, 29);
            this._btnSave.TabIndex = 4;
            this._btnSave.Text = "&Save";
            this._btnSave.UseVisualStyleBackColor = true;
            this._btnSave.Click += new System.EventHandler(this._btnSave_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCancel.CausesValidation = false;
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(423, 324);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(79, 29);
            this._btnCancel.TabIndex = 5;
            this._btnCancel.Text = "&Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Description";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 162;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Comments";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 163;
            // 
            // calendarColumn1
            // 
            this.calendarColumn1.HeaderText = "Run Date";
            this.calendarColumn1.Name = "calendarColumn1";
            this.calendarColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.calendarColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.calendarColumn1.Width = 162;
            // 
            // _btnQuit
            // 
            this._btnQuit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnQuit.Location = new System.Drawing.Point(306, 324);
            this._btnQuit.Name = "_btnQuit";
            this._btnQuit.Size = new System.Drawing.Size(80, 29);
            this._btnQuit.TabIndex = 6;
            this._btnQuit.Text = "&Quit";
            this._btnQuit.UseVisualStyleBackColor = true;
            this._btnQuit.Visible = false;
            // 
            // colDescription
            // 
            this.colDescription.HeaderText = "Description";
            this.colDescription.MaxInputLength = 35;
            this.colDescription.Name = "colDescription";
            // 
            // colComments
            // 
            this.colComments.HeaderText = "Comments";
            this.colComments.MaxInputLength = 255;
            this.colComments.Name = "colComments";
            // 
            // colRunDate
            // 
            this.colRunDate.HeaderText = "Run Date";
            this.colRunDate.Name = "colRunDate";
            this.colRunDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRunDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // CopyDescriptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 364);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._gridCopies);
            this.Controls.Add(this._btnSave);
            this.Controls.Add(this._btnQuit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CopyDescriptionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Copy Estimate";
            ((System.ComponentModel.ISupportInitialize)(this._gridCopies)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _gridCopies;
        private System.Windows.Forms.Button _btnSave;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private CatalogEstimating.CustomControls.CalendarColumn calendarColumn1;
        private System.Windows.Forms.Button _btnQuit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComments;
        private CatalogEstimating.CustomControls.CalendarColumn colRunDate;
    }
}