namespace CatalogEstimating.UserControls.VendorRates
{
    partial class vndMailHouse
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this._txtTimeValueSlips = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblTimeValueSlips = new System.Windows.Forms.Label();
            this._txtInkJetRate = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblInkJetRate = new System.Windows.Forms.Label();
            this._txtInkJetMakeready = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblInkJetMakeready = new System.Windows.Forms.Label();
            this._txtAdminFee = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblAdminFee = new System.Windows.Forms.Label();
            this._gridDefaultRates = new System.Windows.Forms.DataGridView();
            this._dtEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this._lblEffectiveDate = new System.Windows.Forms.Label();
            this._txtPostalDrop = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblPostalDrop = new System.Windows.Forms.Label();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Default = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Rate = new CatalogEstimating.CustomControls.DecimalColumn();
            ((System.ComponentModel.ISupportInitialize)(this._Errors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridDefaultRates)).BeginInit();
            this.SuspendLayout();
            // 
            // _txtTimeValueSlips
            // 
            this._txtTimeValueSlips.AllowNegative = false;
            this._txtTimeValueSlips.FlashColor = System.Drawing.Color.Red;
            this._txtTimeValueSlips.Location = new System.Drawing.Point(140, 40);
            this._txtTimeValueSlips.MaxLength = 9;
            this._txtTimeValueSlips.Name = "_txtTimeValueSlips";
            this._txtTimeValueSlips.Size = new System.Drawing.Size(153, 20);
            this._txtTimeValueSlips.TabIndex = 3;
            this._txtTimeValueSlips.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtTimeValueSlips.Value = null;
            this._txtTimeValueSlips.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblTimeValueSlips
            // 
            this._lblTimeValueSlips.AutoSize = true;
            this._lblTimeValueSlips.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblTimeValueSlips.Location = new System.Drawing.Point(13, 43);
            this._lblTimeValueSlips.Name = "_lblTimeValueSlips";
            this._lblTimeValueSlips.Size = new System.Drawing.Size(106, 13);
            this._lblTimeValueSlips.TabIndex = 2;
            this._lblTimeValueSlips.Text = "Time Value Slips*";
            // 
            // _txtInkJetRate
            // 
            this._txtInkJetRate.AllowNegative = false;
            this._txtInkJetRate.FlashColor = System.Drawing.Color.Red;
            this._txtInkJetRate.Location = new System.Drawing.Point(140, 66);
            this._txtInkJetRate.MaxLength = 9;
            this._txtInkJetRate.Name = "_txtInkJetRate";
            this._txtInkJetRate.Size = new System.Drawing.Size(153, 20);
            this._txtInkJetRate.TabIndex = 5;
            this._txtInkJetRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtInkJetRate.Value = null;
            this._txtInkJetRate.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblInkJetRate
            // 
            this._lblInkJetRate.AutoSize = true;
            this._lblInkJetRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblInkJetRate.Location = new System.Drawing.Point(13, 69);
            this._lblInkJetRate.Name = "_lblInkJetRate";
            this._lblInkJetRate.Size = new System.Drawing.Size(82, 13);
            this._lblInkJetRate.TabIndex = 4;
            this._lblInkJetRate.Text = "Ink Jet Rate*";
            // 
            // _txtInkJetMakeready
            // 
            this._txtInkJetMakeready.AllowNegative = false;
            this._txtInkJetMakeready.FlashColor = System.Drawing.Color.Red;
            this._txtInkJetMakeready.Location = new System.Drawing.Point(140, 92);
            this._txtInkJetMakeready.MaxLength = 9;
            this._txtInkJetMakeready.Name = "_txtInkJetMakeready";
            this._txtInkJetMakeready.Size = new System.Drawing.Size(153, 20);
            this._txtInkJetMakeready.TabIndex = 7;
            this._txtInkJetMakeready.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtInkJetMakeready.Value = null;
            this._txtInkJetMakeready.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblInkJetMakeready
            // 
            this._lblInkJetMakeready.AutoSize = true;
            this._lblInkJetMakeready.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblInkJetMakeready.Location = new System.Drawing.Point(13, 95);
            this._lblInkJetMakeready.Name = "_lblInkJetMakeready";
            this._lblInkJetMakeready.Size = new System.Drawing.Size(117, 13);
            this._lblInkJetMakeready.TabIndex = 6;
            this._lblInkJetMakeready.Text = "Ink Jet Makeready*";
            // 
            // _txtAdminFee
            // 
            this._txtAdminFee.AllowNegative = false;
            this._txtAdminFee.FlashColor = System.Drawing.Color.Red;
            this._txtAdminFee.Location = new System.Drawing.Point(140, 118);
            this._txtAdminFee.MaxLength = 9;
            this._txtAdminFee.Name = "_txtAdminFee";
            this._txtAdminFee.Size = new System.Drawing.Size(153, 20);
            this._txtAdminFee.TabIndex = 9;
            this._txtAdminFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtAdminFee.Value = null;
            this._txtAdminFee.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblAdminFee
            // 
            this._lblAdminFee.AutoSize = true;
            this._lblAdminFee.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblAdminFee.Location = new System.Drawing.Point(13, 121);
            this._lblAdminFee.Name = "_lblAdminFee";
            this._lblAdminFee.Size = new System.Drawing.Size(71, 13);
            this._lblAdminFee.TabIndex = 8;
            this._lblAdminFee.Text = "Admin Fee*";
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
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._gridDefaultRates.DefaultCellStyle = dataGridViewCellStyle4;
            this._gridDefaultRates.Location = new System.Drawing.Point(319, 14);
            this._gridDefaultRates.Name = "_gridDefaultRates";
            this._gridDefaultRates.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this._gridDefaultRates.Size = new System.Drawing.Size(320, 89);
            this._gridDefaultRates.TabIndex = 12;
            this._gridDefaultRates.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this._gridDefaultRates_CellValidating);
            this._gridDefaultRates.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridDefaultRates_CellEndEdit);
            this._gridDefaultRates.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridDefaultRates_CellValueChanged);
            // 
            // _dtEffectiveDate
            // 
            this._dtEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEffectiveDate.Location = new System.Drawing.Point(140, 14);
            this._dtEffectiveDate.Name = "_dtEffectiveDate";
            this._dtEffectiveDate.Size = new System.Drawing.Size(153, 20);
            this._dtEffectiveDate.TabIndex = 1;
            this._dtEffectiveDate.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblEffectiveDate
            // 
            this._lblEffectiveDate.AutoSize = true;
            this._lblEffectiveDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblEffectiveDate.Location = new System.Drawing.Point(13, 18);
            this._lblEffectiveDate.Name = "_lblEffectiveDate";
            this._lblEffectiveDate.Size = new System.Drawing.Size(98, 13);
            this._lblEffectiveDate.TabIndex = 0;
            this._lblEffectiveDate.Text = "Effective Date *";
            // 
            // _txtPostalDrop
            // 
            this._txtPostalDrop.AllowNegative = false;
            this._txtPostalDrop.FlashColor = System.Drawing.Color.Red;
            this._txtPostalDrop.Location = new System.Drawing.Point(140, 144);
            this._txtPostalDrop.MaxLength = 9;
            this._txtPostalDrop.Name = "_txtPostalDrop";
            this._txtPostalDrop.Size = new System.Drawing.Size(153, 20);
            this._txtPostalDrop.TabIndex = 11;
            this._txtPostalDrop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtPostalDrop.Value = null;
            this._txtPostalDrop.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblPostalDrop
            // 
            this._lblPostalDrop.AutoSize = true;
            this._lblPostalDrop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPostalDrop.Location = new System.Drawing.Point(13, 147);
            this._lblPostalDrop.Name = "_lblPostalDrop";
            this._lblPostalDrop.Size = new System.Drawing.Size(110, 13);
            this._lblPostalDrop.TabIndex = 10;
            this._lblPostalDrop.Text = "Postal Drop CWT*";
            // 
            // Description
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.Description.DefaultCellStyle = dataGridViewCellStyle2;
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
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.Rate.DefaultCellStyle = dataGridViewCellStyle3;
            this.Rate.FillWeight = 75F;
            this.Rate.HeaderText = "Rate";
            this.Rate.MaxInputLength = 9;
            this.Rate.Name = "Rate";
            // 
            // vndMailHouse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this._txtPostalDrop);
            this.Controls.Add(this._lblPostalDrop);
            this.Controls.Add(this._dtEffectiveDate);
            this.Controls.Add(this._lblEffectiveDate);
            this.Controls.Add(this._gridDefaultRates);
            this.Controls.Add(this._txtAdminFee);
            this.Controls.Add(this._lblAdminFee);
            this.Controls.Add(this._txtInkJetMakeready);
            this.Controls.Add(this._lblInkJetMakeready);
            this.Controls.Add(this._txtInkJetRate);
            this.Controls.Add(this._lblInkJetRate);
            this.Controls.Add(this._txtTimeValueSlips);
            this.Controls.Add(this._lblTimeValueSlips);
            this.Name = "vndMailHouse";
            this.Size = new System.Drawing.Size(665, 178);
            this.Load += new System.EventHandler(this.vndMailHouse_Load);
            this.Validating += new System.ComponentModel.CancelEventHandler(this.vndMailHouse_Validating);
            ((System.ComponentModel.ISupportInitialize)(this._Errors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridDefaultRates)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CatalogEstimating.CustomControls.DecimalTextBox _txtTimeValueSlips;
        private System.Windows.Forms.Label _lblTimeValueSlips;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtInkJetRate;
        private System.Windows.Forms.Label _lblInkJetRate;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtInkJetMakeready;
        private System.Windows.Forms.Label _lblInkJetMakeready;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtAdminFee;
        private System.Windows.Forms.Label _lblAdminFee;
        private System.Windows.Forms.DataGridView _gridDefaultRates;
        private System.Windows.Forms.DateTimePicker _dtEffectiveDate;
        private System.Windows.Forms.Label _lblEffectiveDate;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtPostalDrop;
        private System.Windows.Forms.Label _lblPostalDrop;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Default;
        private CatalogEstimating.CustomControls.DecimalColumn Rate;
    }
}