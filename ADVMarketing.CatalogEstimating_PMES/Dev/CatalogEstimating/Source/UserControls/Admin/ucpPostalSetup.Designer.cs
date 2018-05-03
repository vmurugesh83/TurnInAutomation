namespace CatalogEstimating.UserControls.Admin
{
    partial class ucpPostalSetup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this._lblVendor = new System.Windows.Forms.Label();
            this._cboVendor = new System.Windows.Forms.ComboBox();
            this.postal = new CatalogEstimating.Datasets.Postal();
            this._lblEffectiveDate = new System.Windows.Forms.Label();
            this._cboEffectiveDate = new System.Windows.Forms.ComboBox();
            this._groupPostalRates = new System.Windows.Forms.GroupBox();
            this._lblInfoText = new System.Windows.Forms.Label();
            this._btnUpdate = new System.Windows.Forms.Button();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnNew = new System.Windows.Forms.ToolStripButton();
            this._btnDelete = new System.Windows.Forms.ToolStripButton();
            this._gridPostalRates = new System.Windows.Forms.DataGridView();
            this._dtEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this._lblSetupEffectiveDate = new System.Windows.Forms.Label();
            this._txtStandardOverweightLimit = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblStandardOverweightLimit = new System.Windows.Forms.Label();
            this._txtFirstClassOverweightLimit = new CatalogEstimating.CustomControls.DecimalTextBox();
            this.lblFirstClassOverweightLimit = new System.Windows.Forms.Label();
            this._errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pst_postalcategory_id = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.pst_postalclass_id = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.pst_postalmailertype_id = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.underweightpiecerate = new CatalogEstimating.CustomControls.DecimalColumn();
            this.overweightpiecerate = new CatalogEstimating.CustomControls.DecimalColumn();
            this.overweightpoundrate = new CatalogEstimating.CustomControls.DecimalColumn();
            this.active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pst_postalcategoryrate_map_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pst_postalweights_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdby = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createddate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedby = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifieddate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ( (System.ComponentModel.ISupportInitialize)( this.postal ) ).BeginInit();
            this._groupPostalRates.SuspendLayout();
            this._toolStrip.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._gridPostalRates ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _lblVendor
            // 
            this._lblVendor.AutoSize = true;
            this._lblVendor.Location = new System.Drawing.Point( 12, 13 );
            this._lblVendor.Name = "_lblVendor";
            this._lblVendor.Size = new System.Drawing.Size( 41, 13 );
            this._lblVendor.TabIndex = 0;
            this._lblVendor.Text = "Vendor";
            // 
            // _cboVendor
            // 
            this._cboVendor.DisplayMember = "vnd_vendor_id";
            this._cboVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboVendor.FormattingEnabled = true;
            this._cboVendor.Location = new System.Drawing.Point( 120, 10 );
            this._cboVendor.Name = "_cboVendor";
            this._cboVendor.Size = new System.Drawing.Size( 200, 21 );
            this._cboVendor.TabIndex = 1;
            this._cboVendor.ValueMember = "vnd_vendor_id";
            this._cboVendor.SelectedIndexChanged += new System.EventHandler( this._cboVendor_SelectedIndexChanged );
            // 
            // postal
            // 
            this.postal.DataSetName = "Postal";
            this.postal.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _lblEffectiveDate
            // 
            this._lblEffectiveDate.AutoSize = true;
            this._lblEffectiveDate.Location = new System.Drawing.Point( 12, 45 );
            this._lblEffectiveDate.Name = "_lblEffectiveDate";
            this._lblEffectiveDate.Size = new System.Drawing.Size( 75, 13 );
            this._lblEffectiveDate.TabIndex = 2;
            this._lblEffectiveDate.Text = "Effective Date";
            // 
            // _cboEffectiveDate
            // 
            this._cboEffectiveDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboEffectiveDate.FormattingEnabled = true;
            this._cboEffectiveDate.Location = new System.Drawing.Point( 120, 42 );
            this._cboEffectiveDate.Name = "_cboEffectiveDate";
            this._cboEffectiveDate.Size = new System.Drawing.Size( 200, 21 );
            this._cboEffectiveDate.TabIndex = 3;
            this._cboEffectiveDate.SelectedIndexChanged += new System.EventHandler( this._cboEffectiveDate_SelectedIndexChanged );
            // 
            // _groupPostalRates
            // 
            this._groupPostalRates.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._groupPostalRates.Controls.Add( this._lblInfoText );
            this._groupPostalRates.Controls.Add( this._btnUpdate );
            this._groupPostalRates.Controls.Add( this._toolStrip );
            this._groupPostalRates.Controls.Add( this._gridPostalRates );
            this._groupPostalRates.Controls.Add( this._dtEffectiveDate );
            this._groupPostalRates.Controls.Add( this._lblSetupEffectiveDate );
            this._groupPostalRates.Controls.Add( this._txtStandardOverweightLimit );
            this._groupPostalRates.Controls.Add( this._lblStandardOverweightLimit );
            this._groupPostalRates.Controls.Add( this._txtFirstClassOverweightLimit );
            this._groupPostalRates.Controls.Add( this.lblFirstClassOverweightLimit );
            this._groupPostalRates.Location = new System.Drawing.Point( 3, 75 );
            this._groupPostalRates.Name = "_groupPostalRates";
            this._groupPostalRates.Size = new System.Drawing.Size( 762, 304 );
            this._groupPostalRates.TabIndex = 4;
            this._groupPostalRates.TabStop = false;
            this._groupPostalRates.Text = "Postal Rate Setup";
            // 
            // _lblInfoText
            // 
            this._lblInfoText.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._lblInfoText.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblInfoText.ForeColor = System.Drawing.Color.Blue;
            this._lblInfoText.Location = new System.Drawing.Point( 8, 20 );
            this._lblInfoText.Name = "_lblInfoText";
            this._lblInfoText.Size = new System.Drawing.Size( 748, 20 );
            this._lblInfoText.TabIndex = 0;
            // 
            // _btnUpdate
            // 
            this._btnUpdate.Enabled = false;
            this._btnUpdate.Location = new System.Drawing.Point( 654, 45 );
            this._btnUpdate.Name = "_btnUpdate";
            this._btnUpdate.Size = new System.Drawing.Size( 91, 26 );
            this._btnUpdate.TabIndex = 8;
            this._btnUpdate.Text = "&Update";
            this._btnUpdate.UseVisualStyleBackColor = true;
            this._btnUpdate.Click += new System.EventHandler( this._btnUpdate_Click );
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._btnNew,
            this._btnDelete} );
            this._toolStrip.Location = new System.Drawing.Point( 0, 236 );
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size( 58, 25 );
            this._toolStrip.TabIndex = 9;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _btnNew
            // 
            this._btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnNew.Image = global::CatalogEstimating.Properties.Resources.NewEstimate;
            this._btnNew.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnNew.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this._btnNew.MergeIndex = 0;
            this._btnNew.Name = "_btnNew";
            this._btnNew.Size = new System.Drawing.Size( 23, 22 );
            this._btnNew.Text = "New";
            this._btnNew.Click += new System.EventHandler( this._btnNew_Click );
            // 
            // _btnDelete
            // 
            this._btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnDelete.Image = global::CatalogEstimating.Properties.Resources.Delete;
            this._btnDelete.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnDelete.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this._btnDelete.MergeIndex = 3;
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Size = new System.Drawing.Size( 23, 22 );
            this._btnDelete.Text = "Delete Rate";
            this._btnDelete.Click += new System.EventHandler( this._btnDelete_Click );
            // 
            // _gridPostalRates
            // 
            this._gridPostalRates.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._gridPostalRates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridPostalRates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._gridPostalRates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridPostalRates.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.pst_postalcategory_id,
            this.pst_postalclass_id,
            this.pst_postalmailertype_id,
            this.underweightpiecerate,
            this.overweightpiecerate,
            this.overweightpoundrate,
            this.active,
            this.pst_postalcategoryrate_map_id,
            this.pst_postalweights_id,
            this.createdby,
            this.createddate,
            this.modifiedby,
            this.modifieddate} );
            this._gridPostalRates.Location = new System.Drawing.Point( 7, 96 );
            this._gridPostalRates.Name = "_gridPostalRates";
            this._gridPostalRates.Size = new System.Drawing.Size( 749, 202 );
            this._gridPostalRates.TabIndex = 7;
            this._gridPostalRates.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler( this._gridPostalRates_UserDeletingRow );
            this._gridPostalRates.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler( this._gridPostalRates_CellBeginEdit );
            this._gridPostalRates.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler( this._gridPostalRates_RowValidating );
            this._gridPostalRates.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler( this._gridPostalRates_UserDeletedRow );
            this._gridPostalRates.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler( this._gridPostalRates_RowValidated );
            this._gridPostalRates.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler( this._gridPostalRates_DefaultValuesNeeded );
            this._gridPostalRates.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler( this._gridPostalRates_CellValueChanged );
            this._gridPostalRates.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler( this._gridPostalRates_DataError );
            // 
            // _dtEffectiveDate
            // 
            this._dtEffectiveDate.Enabled = false;
            this._dtEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEffectiveDate.Location = new System.Drawing.Point( 186, 70 );
            this._dtEffectiveDate.Name = "_dtEffectiveDate";
            this._dtEffectiveDate.Size = new System.Drawing.Size( 121, 20 );
            this._dtEffectiveDate.TabIndex = 6;
            // 
            // _lblSetupEffectiveDate
            // 
            this._lblSetupEffectiveDate.AutoSize = true;
            this._lblSetupEffectiveDate.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblSetupEffectiveDate.Location = new System.Drawing.Point( 7, 74 );
            this._lblSetupEffectiveDate.Name = "_lblSetupEffectiveDate";
            this._lblSetupEffectiveDate.Size = new System.Drawing.Size( 98, 13 );
            this._lblSetupEffectiveDate.TabIndex = 5;
            this._lblSetupEffectiveDate.Text = "Effective Date *";
            // 
            // _txtStandardOverweightLimit
            // 
            this._txtStandardOverweightLimit.AllowNegative = false;
            this._txtStandardOverweightLimit.DecimalPrecision = 4;
            this._txtStandardOverweightLimit.FlashColor = System.Drawing.Color.Red;
            this._txtStandardOverweightLimit.Location = new System.Drawing.Point( 495, 45 );
            this._txtStandardOverweightLimit.MaxLength = 12;
            this._txtStandardOverweightLimit.Name = "_txtStandardOverweightLimit";
            this._txtStandardOverweightLimit.ReadOnly = true;
            this._txtStandardOverweightLimit.Size = new System.Drawing.Size( 121, 20 );
            this._txtStandardOverweightLimit.TabIndex = 4;
            this._txtStandardOverweightLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtStandardOverweightLimit.Value = null;
            this._txtStandardOverweightLimit.Leave += new System.EventHandler( this._txtStandardOverweightLimit_Leave );
            this._txtStandardOverweightLimit.TextChanged += new System.EventHandler( this._txtStandardOverweightLimit_TextChanged );
            // 
            // _lblStandardOverweightLimit
            // 
            this._lblStandardOverweightLimit.AutoSize = true;
            this._lblStandardOverweightLimit.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblStandardOverweightLimit.Location = new System.Drawing.Point( 324, 48 );
            this._lblStandardOverweightLimit.Name = "_lblStandardOverweightLimit";
            this._lblStandardOverweightLimit.Size = new System.Drawing.Size( 165, 13 );
            this._lblStandardOverweightLimit.TabIndex = 3;
            this._lblStandardOverweightLimit.Text = "Standard Overweight Limit *";
            // 
            // _txtFirstClassOverweightLimit
            // 
            this._txtFirstClassOverweightLimit.AllowNegative = false;
            this._txtFirstClassOverweightLimit.DecimalPrecision = 4;
            this._txtFirstClassOverweightLimit.FlashColor = System.Drawing.Color.Red;
            this._txtFirstClassOverweightLimit.Location = new System.Drawing.Point( 186, 45 );
            this._txtFirstClassOverweightLimit.MaxLength = 12;
            this._txtFirstClassOverweightLimit.Name = "_txtFirstClassOverweightLimit";
            this._txtFirstClassOverweightLimit.ReadOnly = true;
            this._txtFirstClassOverweightLimit.Size = new System.Drawing.Size( 121, 20 );
            this._txtFirstClassOverweightLimit.TabIndex = 2;
            this._txtFirstClassOverweightLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtFirstClassOverweightLimit.Value = null;
            this._txtFirstClassOverweightLimit.Leave += new System.EventHandler( this._txtFirstClassOverweightLimit_Leave );
            this._txtFirstClassOverweightLimit.TextChanged += new System.EventHandler( this._txtFirstClassOverweightLimit_TextChanged );
            // 
            // lblFirstClassOverweightLimit
            // 
            this.lblFirstClassOverweightLimit.AutoSize = true;
            this.lblFirstClassOverweightLimit.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lblFirstClassOverweightLimit.Location = new System.Drawing.Point( 8, 48 );
            this.lblFirstClassOverweightLimit.Name = "lblFirstClassOverweightLimit";
            this.lblFirstClassOverweightLimit.Size = new System.Drawing.Size( 172, 13 );
            this.lblFirstClassOverweightLimit.TabIndex = 1;
            this.lblFirstClassOverweightLimit.Text = "First Class Overweight Limit *";
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "pst_postalcategoryrate_map_id";
            this.dataGridViewTextBoxColumn1.HeaderText = "pst_postalcategoryrate_map_id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "pst_postalweights_id";
            this.dataGridViewTextBoxColumn2.HeaderText = "pst_postalweights_id";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "createdby";
            this.dataGridViewTextBoxColumn3.HeaderText = "createdby";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "createddate";
            this.dataGridViewTextBoxColumn4.HeaderText = "createddate";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "modifiedby";
            this.dataGridViewTextBoxColumn5.HeaderText = "modifiedby";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "modifieddate";
            this.dataGridViewTextBoxColumn6.HeaderText = "modifieddate";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // pst_postalcategory_id
            // 
            this.pst_postalcategory_id.DataPropertyName = "pst_postalcategory_id";
            this.pst_postalcategory_id.HeaderText = "Postage Category";
            this.pst_postalcategory_id.Name = "pst_postalcategory_id";
            // 
            // pst_postalclass_id
            // 
            this.pst_postalclass_id.DataPropertyName = "pst_postalclass_id";
            this.pst_postalclass_id.FillWeight = 45F;
            this.pst_postalclass_id.HeaderText = "Postage Class";
            this.pst_postalclass_id.Name = "pst_postalclass_id";
            // 
            // pst_postalmailertype_id
            // 
            this.pst_postalmailertype_id.DataPropertyName = "pst_postalmailertype_id";
            this.pst_postalmailertype_id.FillWeight = 45F;
            this.pst_postalmailertype_id.HeaderText = "Mailer Type";
            this.pst_postalmailertype_id.Name = "pst_postalmailertype_id";
            // 
            // underweightpiecerate
            // 
            this.underweightpiecerate.AllowNegative = false;
            this.underweightpiecerate.DataPropertyName = "underweightpiecerate";
            this.underweightpiecerate.DecimalPrecision = 4;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N4";
            dataGridViewCellStyle2.NullValue = null;
            this.underweightpiecerate.DefaultCellStyle = dataGridViewCellStyle2;
            this.underweightpiecerate.FillWeight = 45F;
            this.underweightpiecerate.HeaderText = "Under Piece Rate";
            this.underweightpiecerate.MaxInputLength = 6;
            this.underweightpiecerate.Name = "underweightpiecerate";
            // 
            // overweightpiecerate
            // 
            this.overweightpiecerate.AllowNegative = false;
            this.overweightpiecerate.DataPropertyName = "overweightpiecerate";
            this.overweightpiecerate.DecimalPrecision = 4;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N4";
            dataGridViewCellStyle3.NullValue = null;
            this.overweightpiecerate.DefaultCellStyle = dataGridViewCellStyle3;
            this.overweightpiecerate.FillWeight = 45F;
            this.overweightpiecerate.HeaderText = "Over Piece Rate";
            this.overweightpiecerate.MaxInputLength = 6;
            this.overweightpiecerate.Name = "overweightpiecerate";
            // 
            // overweightpoundrate
            // 
            this.overweightpoundrate.AllowNegative = false;
            this.overweightpoundrate.DataPropertyName = "overweightpoundrate";
            this.overweightpoundrate.DecimalPrecision = 4;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N4";
            dataGridViewCellStyle4.NullValue = null;
            this.overweightpoundrate.DefaultCellStyle = dataGridViewCellStyle4;
            this.overweightpoundrate.FillWeight = 45F;
            this.overweightpoundrate.HeaderText = "Over Pound Rate";
            this.overweightpoundrate.MaxInputLength = 6;
            this.overweightpoundrate.Name = "overweightpoundrate";
            // 
            // active
            // 
            this.active.DataPropertyName = "active";
            this.active.FillWeight = 25F;
            this.active.HeaderText = "Active";
            this.active.IndeterminateValue = "false";
            this.active.Name = "active";
            // 
            // pst_postalcategoryrate_map_id
            // 
            this.pst_postalcategoryrate_map_id.DataPropertyName = "pst_postalcategoryrate_map_id";
            this.pst_postalcategoryrate_map_id.HeaderText = "pst_postalcategoryrate_map_id";
            this.pst_postalcategoryrate_map_id.Name = "pst_postalcategoryrate_map_id";
            this.pst_postalcategoryrate_map_id.Visible = false;
            // 
            // pst_postalweights_id
            // 
            this.pst_postalweights_id.DataPropertyName = "pst_postalweights_id";
            this.pst_postalweights_id.HeaderText = "pst_postalweights_id";
            this.pst_postalweights_id.Name = "pst_postalweights_id";
            this.pst_postalweights_id.Visible = false;
            // 
            // createdby
            // 
            this.createdby.DataPropertyName = "createdby";
            this.createdby.HeaderText = "createdby";
            this.createdby.Name = "createdby";
            this.createdby.Visible = false;
            // 
            // createddate
            // 
            this.createddate.DataPropertyName = "createddate";
            this.createddate.HeaderText = "createddate";
            this.createddate.Name = "createddate";
            this.createddate.Visible = false;
            // 
            // modifiedby
            // 
            this.modifiedby.DataPropertyName = "modifiedby";
            this.modifiedby.HeaderText = "modifiedby";
            this.modifiedby.Name = "modifiedby";
            this.modifiedby.Visible = false;
            // 
            // modifieddate
            // 
            this.modifieddate.DataPropertyName = "modifieddate";
            this.modifieddate.HeaderText = "modifieddate";
            this.modifieddate.Name = "modifieddate";
            this.modifieddate.Visible = false;
            // 
            // ucpPostalSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._groupPostalRates );
            this.Controls.Add( this._cboEffectiveDate );
            this.Controls.Add( this._lblEffectiveDate );
            this.Controls.Add( this._cboVendor );
            this.Controls.Add( this._lblVendor );
            this.Name = "ucpPostalSetup";
            this.Size = new System.Drawing.Size( 768, 382 );
            this.Load += new System.EventHandler( this.ucpPostalSetup_Load );
            ( (System.ComponentModel.ISupportInitialize)( this.postal ) ).EndInit();
            this._groupPostalRates.ResumeLayout( false );
            this._groupPostalRates.PerformLayout();
            this._toolStrip.ResumeLayout( false );
            this._toolStrip.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._gridPostalRates ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblVendor;
        private System.Windows.Forms.ComboBox _cboVendor;
        private System.Windows.Forms.Label _lblEffectiveDate;
        private System.Windows.Forms.ComboBox _cboEffectiveDate;
        private System.Windows.Forms.GroupBox _groupPostalRates;
        private System.Windows.Forms.Label lblFirstClassOverweightLimit;
        private System.Windows.Forms.DataGridView _gridPostalRates;
        private System.Windows.Forms.DateTimePicker _dtEffectiveDate;
        private System.Windows.Forms.Label _lblSetupEffectiveDate;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtStandardOverweightLimit;
        private System.Windows.Forms.Label _lblStandardOverweightLimit;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtFirstClassOverweightLimit;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnNew;
        private CatalogEstimating.Datasets.Postal postal;
        private System.Windows.Forms.Button _btnUpdate;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.ToolStripButton _btnDelete;
        private System.Windows.Forms.Label _lblInfoText;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewComboBoxColumn pst_postalcategory_id;
        private System.Windows.Forms.DataGridViewComboBoxColumn pst_postalclass_id;
        private System.Windows.Forms.DataGridViewComboBoxColumn pst_postalmailertype_id;
        private CatalogEstimating.CustomControls.DecimalColumn underweightpiecerate;
        private CatalogEstimating.CustomControls.DecimalColumn overweightpiecerate;
        private CatalogEstimating.CustomControls.DecimalColumn overweightpoundrate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn active;
        private System.Windows.Forms.DataGridViewTextBoxColumn pst_postalcategoryrate_map_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn pst_postalweights_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdby;
        private System.Windows.Forms.DataGridViewTextBoxColumn createddate;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedby;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifieddate;
    }
}
