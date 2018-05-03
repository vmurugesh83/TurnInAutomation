namespace CatalogEstimating.UserControls.VendorRates
{
    partial class vndPrinter
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this._dtEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this._lblEffectiveDate = new System.Windows.Forms.Label();
            this._txtPaperHandling = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblPaperHandling = new System.Windows.Forms.Label();
            this._txtPolybagBagWeight = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblPolybagBagWeight = new System.Windows.Forms.Label();
            this._gridDefaultRates = new System.Windows.Forms.DataGridView();
            this._tabControl = new System.Windows.Forms.TabControl();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Default = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Rate = new CatalogEstimating.CustomControls.DecimalColumn();
            ((System.ComponentModel.ISupportInitialize)(this._Errors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridDefaultRates)).BeginInit();
            this.SuspendLayout();
            // 
            // _dtEffectiveDate
            // 
            this._dtEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEffectiveDate.Location = new System.Drawing.Point(150, 9);
            this._dtEffectiveDate.Name = "_dtEffectiveDate";
            this._dtEffectiveDate.Size = new System.Drawing.Size(153, 20);
            this._dtEffectiveDate.TabIndex = 1;
            this._dtEffectiveDate.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblEffectiveDate
            // 
            this._lblEffectiveDate.AutoSize = true;
            this._lblEffectiveDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblEffectiveDate.Location = new System.Drawing.Point(13, 13);
            this._lblEffectiveDate.Name = "_lblEffectiveDate";
            this._lblEffectiveDate.Size = new System.Drawing.Size(94, 13);
            this._lblEffectiveDate.TabIndex = 0;
            this._lblEffectiveDate.Text = "Effective Date*";
            // 
            // _txtPaperHandling
            // 
            this._txtPaperHandling.AllowNegative = false;
            this._txtPaperHandling.FlashColor = System.Drawing.Color.Red;
            this._txtPaperHandling.Location = new System.Drawing.Point(150, 35);
            this._txtPaperHandling.MaxLength = 9;
            this._txtPaperHandling.Name = "_txtPaperHandling";
            this._txtPaperHandling.Size = new System.Drawing.Size(153, 20);
            this._txtPaperHandling.TabIndex = 3;
            this._txtPaperHandling.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtPaperHandling.Value = null;
            this._txtPaperHandling.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblPaperHandling
            // 
            this._lblPaperHandling.AutoSize = true;
            this._lblPaperHandling.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPaperHandling.Location = new System.Drawing.Point(13, 38);
            this._lblPaperHandling.Name = "_lblPaperHandling";
            this._lblPaperHandling.Size = new System.Drawing.Size(99, 13);
            this._lblPaperHandling.TabIndex = 2;
            this._lblPaperHandling.Text = "Paper Handling*";
            // 
            // _txtPolybagBagWeight
            // 
            this._txtPolybagBagWeight.AllowNegative = false;
            this._txtPolybagBagWeight.DecimalPrecision = 4;
            this._txtPolybagBagWeight.FlashColor = System.Drawing.Color.Red;
            this._txtPolybagBagWeight.Location = new System.Drawing.Point(150, 61);
            this._txtPolybagBagWeight.MaxLength = 6;
            this._txtPolybagBagWeight.Name = "_txtPolybagBagWeight";
            this._txtPolybagBagWeight.Size = new System.Drawing.Size(153, 20);
            this._txtPolybagBagWeight.TabIndex = 5;
            this._txtPolybagBagWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtPolybagBagWeight.Value = null;
            this._txtPolybagBagWeight.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblPolybagBagWeight
            // 
            this._lblPolybagBagWeight.AutoSize = true;
            this._lblPolybagBagWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPolybagBagWeight.Location = new System.Drawing.Point(13, 64);
            this._lblPolybagBagWeight.Name = "_lblPolybagBagWeight";
            this._lblPolybagBagWeight.Size = new System.Drawing.Size(127, 13);
            this._lblPolybagBagWeight.TabIndex = 4;
            this._lblPolybagBagWeight.Text = "Polybag Bag Weight*";
            // 
            // _gridDefaultRates
            // 
            this._gridDefaultRates.AllowUserToAddRows = false;
            this._gridDefaultRates.AllowUserToDeleteRows = false;
            this._gridDefaultRates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridDefaultRates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._gridDefaultRates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridDefaultRates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Description,
            this.Default,
            this.Rate});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._gridDefaultRates.DefaultCellStyle = dataGridViewCellStyle3;
            this._gridDefaultRates.Location = new System.Drawing.Point(313, 9);
            this._gridDefaultRates.Name = "_gridDefaultRates";
            this._gridDefaultRates.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this._gridDefaultRates.Size = new System.Drawing.Size(431, 111);
            this._gridDefaultRates.TabIndex = 6;
            this._gridDefaultRates.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this._gridDefaultRates_RowValidating);
            this._gridDefaultRates.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridDefaultRates_CellValueChanged);
            // 
            // _tabControl
            // 
            this._tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tabControl.Location = new System.Drawing.Point(16, 126);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(732, 232);
            this._tabControl.TabIndex = 7;
            this._tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this._tabControl_Selecting);
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.MaxInputLength = 35;
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Default
            // 
            this.Default.FillWeight = 50F;
            this.Default.HeaderText = "Default";
            this.Default.Name = "Default";
            this.Default.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Rate
            // 
            this.Rate.AllowNegative = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.Rate.DefaultCellStyle = dataGridViewCellStyle2;
            this.Rate.FillWeight = 75F;
            this.Rate.HeaderText = "Rate";
            this.Rate.MaxInputLength = 9;
            this.Rate.Name = "Rate";
            // 
            // vndPrinter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this._tabControl);
            this.Controls.Add(this._gridDefaultRates);
            this.Controls.Add(this._txtPolybagBagWeight);
            this.Controls.Add(this._lblPolybagBagWeight);
            this.Controls.Add(this._dtEffectiveDate);
            this.Controls.Add(this._lblEffectiveDate);
            this.Controls.Add(this._txtPaperHandling);
            this.Controls.Add(this._lblPaperHandling);
            this.Name = "vndPrinter";
            this.Size = new System.Drawing.Size(765, 361);
            this.Load += new System.EventHandler(this.vndPrinter_Load);
            this.Validating += new System.ComponentModel.CancelEventHandler(this.vndPrinter_Validating);
            ((System.ComponentModel.ISupportInitialize)(this._Errors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridDefaultRates)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker _dtEffectiveDate;
        private System.Windows.Forms.Label _lblEffectiveDate;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtPaperHandling;
        private System.Windows.Forms.Label _lblPaperHandling;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtPolybagBagWeight;
        private System.Windows.Forms.Label _lblPolybagBagWeight;
        private System.Windows.Forms.DataGridView _gridDefaultRates;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Default;
        private CatalogEstimating.CustomControls.DecimalColumn Rate;
    }
}