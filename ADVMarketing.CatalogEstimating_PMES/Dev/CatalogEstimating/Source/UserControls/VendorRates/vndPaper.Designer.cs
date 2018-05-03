namespace CatalogEstimating.UserControls.VendorRates
{
    partial class vndPaper
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this._dtEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this._lblEffectiveDate = new System.Windows.Forms.Label();
            this._btnManagePaper = new System.Windows.Forms.Button();
            this._gridPaper = new System.Windows.Forms.DataGridView();
            this.pprpaperweightBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._dsAdministration = new CatalogEstimating.Datasets.Administration();
            this.pprpapergradeBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this.pprpapermapBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._lblValidationError = new System.Windows.Forms.Label();
            this.pprpapermapidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pprpaperweightidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.pprpapergradeidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cwtDataGridViewTextBoxColumn = new CatalogEstimating.CustomControls.DecimalColumn();
            this.defaultDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.vndpaperidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifieddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ( (System.ComponentModel.ISupportInitialize)( this._Errors ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this._gridPaper ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pprpaperweightBindingSource ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this._dsAdministration ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pprpapergradeBindingSource ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pprpapermapBindingSource ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _dtEffectiveDate
            // 
            this._dtEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEffectiveDate.Location = new System.Drawing.Point( 118, 10 );
            this._dtEffectiveDate.Name = "_dtEffectiveDate";
            this._dtEffectiveDate.Size = new System.Drawing.Size( 153, 20 );
            this._dtEffectiveDate.TabIndex = 1;
            this._dtEffectiveDate.ValueChanged += new System.EventHandler( this._dtEffectiveDate_ValueChanged );
            // 
            // _lblEffectiveDate
            // 
            this._lblEffectiveDate.AutoSize = true;
            this._lblEffectiveDate.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblEffectiveDate.Location = new System.Drawing.Point( 14, 14 );
            this._lblEffectiveDate.Name = "_lblEffectiveDate";
            this._lblEffectiveDate.Size = new System.Drawing.Size( 94, 13 );
            this._lblEffectiveDate.TabIndex = 0;
            this._lblEffectiveDate.Text = "Effective Date*";
            // 
            // _btnManagePaper
            // 
            this._btnManagePaper.Location = new System.Drawing.Point( 299, 3 );
            this._btnManagePaper.Name = "_btnManagePaper";
            this._btnManagePaper.Size = new System.Drawing.Size( 89, 29 );
            this._btnManagePaper.TabIndex = 3;
            this._btnManagePaper.Text = "&Manage Paper";
            this._btnManagePaper.UseVisualStyleBackColor = true;
            this._btnManagePaper.Click += new System.EventHandler( this._btnManagePaper_Click );
            // 
            // _gridPaper
            // 
            this._gridPaper.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._gridPaper.AutoGenerateColumns = false;
            this._gridPaper.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridPaper.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._gridPaper.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridPaper.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.pprpapermapidDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.pprpaperweightidDataGridViewTextBoxColumn,
            this.pprpapergradeidDataGridViewTextBoxColumn,
            this.cwtDataGridViewTextBoxColumn,
            this.defaultDataGridViewCheckBoxColumn,
            this.vndpaperidDataGridViewTextBoxColumn,
            this.createdbyDataGridViewTextBoxColumn,
            this.createddateDataGridViewTextBoxColumn,
            this.modifiedbyDataGridViewTextBoxColumn,
            this.modifieddateDataGridViewTextBoxColumn} );
            this._gridPaper.DataSource = this.pprpapermapBindingSource;
            this._gridPaper.Location = new System.Drawing.Point( 17, 41 );
            this._gridPaper.Name = "_gridPaper";
            this._gridPaper.Size = new System.Drawing.Size( 541, 275 );
            this._gridPaper.TabIndex = 2;
            this._gridPaper.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler( this._gridPaper_UserDeletingRow );
            this._gridPaper.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler( this._gridPaper_RowValidating );
            this._gridPaper.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler( this._gridPaper_RowValidated );
            this._gridPaper.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler( this._gridPaper_DefaultValuesNeeded );
            this._gridPaper.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler( this._gridPaper_CellValueChanged );
            this._gridPaper.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler( this._gridPaper_DataError );
            this._gridPaper.EnabledChanged += new System.EventHandler( this._gridPaper_EnabledChanged );
            // 
            // pprpaperweightBindingSource
            // 
            this.pprpaperweightBindingSource.DataMember = "ppr_paperweight";
            this.pprpaperweightBindingSource.DataSource = this._dsAdministration;
            this.pprpaperweightBindingSource.Sort = "weight ASC";
            // 
            // _dsAdministration
            // 
            this._dsAdministration.DataSetName = "Administration";
            this._dsAdministration.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pprpapergradeBindingSource
            // 
            this.pprpapergradeBindingSource.DataMember = "ppr_papergrade";
            this.pprpapergradeBindingSource.DataSource = this._dsAdministration;
            this.pprpapergradeBindingSource.Sort = "grade ASC";
            // 
            // pprpapermapBindingSource
            // 
            this.pprpapermapBindingSource.DataMember = "ppr_paper_map";
            this.pprpapermapBindingSource.DataSource = this._dsAdministration;
            this.pprpapermapBindingSource.Sort = "description ASC";
            // 
            // _lblValidationError
            // 
            this._lblValidationError.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this._lblValidationError.AutoSize = true;
            this._lblValidationError.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblValidationError.ForeColor = System.Drawing.Color.Red;
            this._lblValidationError.Location = new System.Drawing.Point( 18, 328 );
            this._lblValidationError.Name = "_lblValidationError";
            this._lblValidationError.Size = new System.Drawing.Size( 94, 13 );
            this._lblValidationError.TabIndex = 4;
            this._lblValidationError.Text = "Validation Error";
            this._lblValidationError.Visible = false;
            // 
            // pprpapermapidDataGridViewTextBoxColumn
            // 
            this.pprpapermapidDataGridViewTextBoxColumn.DataPropertyName = "ppr_paper_map_id";
            this.pprpapermapidDataGridViewTextBoxColumn.HeaderText = "ppr_paper_map_id";
            this.pprpapermapidDataGridViewTextBoxColumn.Name = "pprpapermapidDataGridViewTextBoxColumn";
            this.pprpapermapidDataGridViewTextBoxColumn.ReadOnly = true;
            this.pprpapermapidDataGridViewTextBoxColumn.Visible = false;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.MaxInputLength = 35;
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // pprpaperweightidDataGridViewTextBoxColumn
            // 
            this.pprpaperweightidDataGridViewTextBoxColumn.DataPropertyName = "ppr_paperweight_id";
            this.pprpaperweightidDataGridViewTextBoxColumn.DataSource = this.pprpaperweightBindingSource;
            this.pprpaperweightidDataGridViewTextBoxColumn.DisplayMember = "weight";
            this.pprpaperweightidDataGridViewTextBoxColumn.FillWeight = 60F;
            this.pprpaperweightidDataGridViewTextBoxColumn.HeaderText = "Paper Weight";
            this.pprpaperweightidDataGridViewTextBoxColumn.Name = "pprpaperweightidDataGridViewTextBoxColumn";
            this.pprpaperweightidDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.pprpaperweightidDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.pprpaperweightidDataGridViewTextBoxColumn.ValueMember = "ppr_paperweight_id";
            // 
            // pprpapergradeidDataGridViewTextBoxColumn
            // 
            this.pprpapergradeidDataGridViewTextBoxColumn.DataPropertyName = "ppr_papergrade_id";
            this.pprpapergradeidDataGridViewTextBoxColumn.DataSource = this.pprpapergradeBindingSource;
            this.pprpapergradeidDataGridViewTextBoxColumn.DisplayMember = "grade";
            this.pprpapergradeidDataGridViewTextBoxColumn.FillWeight = 60F;
            this.pprpapergradeidDataGridViewTextBoxColumn.HeaderText = "Paper Grade";
            this.pprpapergradeidDataGridViewTextBoxColumn.Name = "pprpapergradeidDataGridViewTextBoxColumn";
            this.pprpapergradeidDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.pprpapergradeidDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.pprpapergradeidDataGridViewTextBoxColumn.ValueMember = "ppr_papergrade_id";
            // 
            // cwtDataGridViewTextBoxColumn
            // 
            this.cwtDataGridViewTextBoxColumn.AllowNegative = false;
            this.cwtDataGridViewTextBoxColumn.DataPropertyName = "cwt";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.cwtDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.cwtDataGridViewTextBoxColumn.FillWeight = 65F;
            this.cwtDataGridViewTextBoxColumn.HeaderText = "CWT Rate";
            this.cwtDataGridViewTextBoxColumn.MaxInputLength = 9;
            this.cwtDataGridViewTextBoxColumn.Name = "cwtDataGridViewTextBoxColumn";
            // 
            // defaultDataGridViewCheckBoxColumn
            // 
            this.defaultDataGridViewCheckBoxColumn.DataPropertyName = "default";
            this.defaultDataGridViewCheckBoxColumn.FalseValue = "false";
            this.defaultDataGridViewCheckBoxColumn.FillWeight = 50F;
            this.defaultDataGridViewCheckBoxColumn.HeaderText = "Default";
            this.defaultDataGridViewCheckBoxColumn.Name = "defaultDataGridViewCheckBoxColumn";
            this.defaultDataGridViewCheckBoxColumn.TrueValue = "true";
            // 
            // vndpaperidDataGridViewTextBoxColumn
            // 
            this.vndpaperidDataGridViewTextBoxColumn.DataPropertyName = "vnd_paper_id";
            this.vndpaperidDataGridViewTextBoxColumn.HeaderText = "vnd_paper_id";
            this.vndpaperidDataGridViewTextBoxColumn.Name = "vndpaperidDataGridViewTextBoxColumn";
            this.vndpaperidDataGridViewTextBoxColumn.Visible = false;
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
            // vndPaper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.Controls.Add( this._lblValidationError );
            this.Controls.Add( this._gridPaper );
            this.Controls.Add( this._btnManagePaper );
            this.Controls.Add( this._dtEffectiveDate );
            this.Controls.Add( this._lblEffectiveDate );
            this.Name = "vndPaper";
            this.Size = new System.Drawing.Size( 571, 351 );
            this.Load += new System.EventHandler( this.vndPaper_Load );
            this.Validated += new System.EventHandler( this.vndPaper_Validated );
            this.Validating += new System.ComponentModel.CancelEventHandler( this.vndPaper_Validating );
            ( (System.ComponentModel.ISupportInitialize)( this._Errors ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this._gridPaper ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pprpaperweightBindingSource ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this._dsAdministration ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pprpapergradeBindingSource ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pprpapermapBindingSource ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker _dtEffectiveDate;
        private System.Windows.Forms.Label _lblEffectiveDate;
        private System.Windows.Forms.Button _btnManagePaper;
        private System.Windows.Forms.DataGridView _gridPaper;
        private System.Windows.Forms.BindingSource pprpapermapBindingSource;
        private CatalogEstimating.Datasets.Administration _dsAdministration;
        private System.Windows.Forms.BindingSource pprpapergradeBindingSource;
        private System.Windows.Forms.BindingSource pprpaperweightBindingSource;
        private System.Windows.Forms.Label _lblValidationError;
        private System.Windows.Forms.DataGridViewTextBoxColumn pprpapermapidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn pprpaperweightidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn pprpapergradeidDataGridViewTextBoxColumn;
        private CatalogEstimating.CustomControls.DecimalColumn cwtDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn defaultDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vndpaperidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createddateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifieddateDataGridViewTextBoxColumn;
    }
}
