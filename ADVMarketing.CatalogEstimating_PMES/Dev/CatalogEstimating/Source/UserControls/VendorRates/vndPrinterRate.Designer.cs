namespace CatalogEstimating.UserControls.VendorRates
{
    partial class vndPrinterRate
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this._lblGridTitle = new System.Windows.Forms.Label();
            this._gridPrinterRate = new System.Windows.Forms.DataGridView();
            this.prtprinterrateBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this.administration = new CatalogEstimating.Datasets.Administration();
            this._lblValidationError = new System.Windows.Forms.Label();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rateDataGridViewTextBoxColumn = new CatalogEstimating.CustomControls.DecimalColumn();
            this.defaultDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.prtprinterrateidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vndprinteridDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prtprinterratetypeidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifieddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ( (System.ComponentModel.ISupportInitialize)( this._gridPrinterRate ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.prtprinterrateBindingSource ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.administration ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _lblGridTitle
            // 
            this._lblGridTitle.AutoSize = true;
            this._lblGridTitle.Location = new System.Drawing.Point( 12, 9 );
            this._lblGridTitle.Name = "_lblGridTitle";
            this._lblGridTitle.Size = new System.Drawing.Size( 35, 13 );
            this._lblGridTitle.TabIndex = 0;
            this._lblGridTitle.Text = "label1";
            // 
            // _gridPrinterRate
            // 
            this._gridPrinterRate.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._gridPrinterRate.AutoGenerateColumns = false;
            this._gridPrinterRate.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridPrinterRate.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._gridPrinterRate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridPrinterRate.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.descriptionDataGridViewTextBoxColumn,
            this.rateDataGridViewTextBoxColumn,
            this.defaultDataGridViewCheckBoxColumn,
            this.prtprinterrateidDataGridViewTextBoxColumn,
            this.vndprinteridDataGridViewTextBoxColumn,
            this.prtprinterratetypeidDataGridViewTextBoxColumn,
            this.createdbyDataGridViewTextBoxColumn,
            this.createddateDataGridViewTextBoxColumn,
            this.modifiedbyDataGridViewTextBoxColumn,
            this.modifieddateDataGridViewTextBoxColumn} );
            this._gridPrinterRate.DataSource = this.prtprinterrateBindingSource;
            this._gridPrinterRate.Location = new System.Drawing.Point( 15, 35 );
            this._gridPrinterRate.Name = "_gridPrinterRate";
            this._gridPrinterRate.Size = new System.Drawing.Size( 622, 170 );
            this._gridPrinterRate.TabIndex = 1;
            this._gridPrinterRate.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler( this._gridPrinterRate_UserDeletingRow );
            this._gridPrinterRate.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler( this._gridPrinterRate_RowValidating );
            this._gridPrinterRate.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler( this._gridPrinterRate_RowValidated );
            this._gridPrinterRate.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler( this._gridPrinterRate_DefaultValuesNeeded );
            this._gridPrinterRate.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler( this._gridPrinterRate_CellValueChanged );
            this._gridPrinterRate.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler( this._gridPrinterRate_DataError );
            // 
            // prtprinterrateBindingSource
            // 
            this.prtprinterrateBindingSource.DataMember = "prt_printerrate";
            this.prtprinterrateBindingSource.DataSource = this.administration;
            // 
            // administration
            // 
            this.administration.DataSetName = "Administration";
            this.administration.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _lblValidationError
            // 
            this._lblValidationError.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this._lblValidationError.AutoSize = true;
            this._lblValidationError.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblValidationError.ForeColor = System.Drawing.Color.Red;
            this._lblValidationError.Location = new System.Drawing.Point( 12, 214 );
            this._lblValidationError.Name = "_lblValidationError";
            this._lblValidationError.Size = new System.Drawing.Size( 94, 13 );
            this._lblValidationError.TabIndex = 2;
            this._lblValidationError.Text = "Validation Error";
            this._lblValidationError.Visible = false;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.MaxInputLength = 35;
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // rateDataGridViewTextBoxColumn
            // 
            this.rateDataGridViewTextBoxColumn.AllowNegative = false;
            this.rateDataGridViewTextBoxColumn.DataPropertyName = "rate";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.rateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.rateDataGridViewTextBoxColumn.FillWeight = 65F;
            this.rateDataGridViewTextBoxColumn.HeaderText = "Rate";
            this.rateDataGridViewTextBoxColumn.MaxInputLength = 9;
            this.rateDataGridViewTextBoxColumn.Name = "rateDataGridViewTextBoxColumn";
            // 
            // defaultDataGridViewCheckBoxColumn
            // 
            this.defaultDataGridViewCheckBoxColumn.DataPropertyName = "default";
            this.defaultDataGridViewCheckBoxColumn.FillWeight = 40F;
            this.defaultDataGridViewCheckBoxColumn.HeaderText = "Default";
            this.defaultDataGridViewCheckBoxColumn.Name = "defaultDataGridViewCheckBoxColumn";
            // 
            // prtprinterrateidDataGridViewTextBoxColumn
            // 
            this.prtprinterrateidDataGridViewTextBoxColumn.DataPropertyName = "prt_printerrate_id";
            this.prtprinterrateidDataGridViewTextBoxColumn.HeaderText = "prt_printerrate_id";
            this.prtprinterrateidDataGridViewTextBoxColumn.Name = "prtprinterrateidDataGridViewTextBoxColumn";
            this.prtprinterrateidDataGridViewTextBoxColumn.ReadOnly = true;
            this.prtprinterrateidDataGridViewTextBoxColumn.Visible = false;
            // 
            // vndprinteridDataGridViewTextBoxColumn
            // 
            this.vndprinteridDataGridViewTextBoxColumn.DataPropertyName = "vnd_printer_id";
            this.vndprinteridDataGridViewTextBoxColumn.HeaderText = "vnd_printer_id";
            this.vndprinteridDataGridViewTextBoxColumn.Name = "vndprinteridDataGridViewTextBoxColumn";
            this.vndprinteridDataGridViewTextBoxColumn.Visible = false;
            // 
            // prtprinterratetypeidDataGridViewTextBoxColumn
            // 
            this.prtprinterratetypeidDataGridViewTextBoxColumn.DataPropertyName = "prt_printerratetype_id";
            this.prtprinterratetypeidDataGridViewTextBoxColumn.HeaderText = "prt_printerratetype_id";
            this.prtprinterratetypeidDataGridViewTextBoxColumn.Name = "prtprinterratetypeidDataGridViewTextBoxColumn";
            this.prtprinterratetypeidDataGridViewTextBoxColumn.Visible = false;
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
            // vndPrinterRate
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._lblValidationError );
            this.Controls.Add( this._gridPrinterRate );
            this.Controls.Add( this._lblGridTitle );
            this.Name = "vndPrinterRate";
            this.Size = new System.Drawing.Size( 651, 241 );
            this.Load += new System.EventHandler( this.vndPrinterRate_Load );
            this.Validated += new System.EventHandler( this.vndPrinterRate_Validated );
            this.Validating += new System.ComponentModel.CancelEventHandler( this.vndPrinterRate_Validating );
            ( (System.ComponentModel.ISupportInitialize)( this._gridPrinterRate ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.prtprinterrateBindingSource ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.administration ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblGridTitle;
        private System.Windows.Forms.DataGridView _gridPrinterRate;
        private System.Windows.Forms.BindingSource prtprinterrateBindingSource;
        private CatalogEstimating.Datasets.Administration administration;
        private System.Windows.Forms.Label _lblValidationError;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private CatalogEstimating.CustomControls.DecimalColumn rateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn defaultDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn prtprinterrateidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vndprinteridDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn prtprinterratetypeidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createddateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifieddateDataGridViewTextBoxColumn;

    }
}
