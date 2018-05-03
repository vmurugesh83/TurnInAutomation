namespace CatalogEstimating.UserControls.Estimate
{
    partial class ucpAssemblyDistributionOptions
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
            this._groupInsertOptions = new System.Windows.Forms.GroupBox();
            this._txtTotalInsertWeight = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblTotalInsertWeight = new System.Windows.Forms.Label();
            this._txtFuelSurcharge = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblFuelSurcharge = new System.Windows.Forms.Label();
            this._txtFreightCWT = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblFreightCWT = new System.Windows.Forms.Label();
            this._cboFreightVendor = new System.Windows.Forms.ComboBox();
            this._lblFreightVendor = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._radPMAM = new System.Windows.Forms.RadioButton();
            this._radAMPM = new System.Windows.Forms.RadioButton();
            this._chkSkids = new System.Windows.Forms.CheckBox();
            this._chkCornerGuards = new System.Windows.Forms.CheckBox();
            this._cboInsertDOW = new System.Windows.Forms.ComboBox();
            this._lblInsertDOW = new System.Windows.Forms.Label();
            this._groupMailOptions = new System.Windows.Forms.GroupBox();
            this._txtTotalMailingWeight = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblTotalMailingWeight = new System.Windows.Forms.Label();
            this._txtNumberOfCartons = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._lblNumberOfCartons = new System.Windows.Forms.Label();
            this._groupMailList = new System.Windows.Forms.GroupBox();
            this._cboMailListResource = new System.Windows.Forms.ComboBox();
            this.mailListsMailListIDandDescriptionByRunDateBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._dsEstimate = new CatalogEstimating.Datasets.Estimates();
            this._lblMailListResource = new System.Windows.Forms.Label();
            this._txtExternalCPM = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._txtExternalQty = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._lblExternalCPM = new System.Windows.Forms.Label();
            this._lblExternalQty = new System.Windows.Forms.Label();
            this._chkUseExternalMailList = new System.Windows.Forms.CheckBox();
            this._groupMailTracking = new System.Windows.Forms.GroupBox();
            this._cboMailTracker = new System.Windows.Forms.ComboBox();
            this.mailTrackingsMailTrackingIDandDescriptionByRunDateBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._chkUseMailTracking = new System.Windows.Forms.CheckBox();
            this._lblMailTracker = new System.Windows.Forms.Label();
            this._groupMailhouse = new System.Windows.Forms.GroupBox();
            this._txtPostalDropFlat = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblPostalDropFlat = new System.Windows.Forms.Label();
            this._txtPostalDropCWT = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblPostalDropCWT = new System.Windows.Forms.Label();
            this._txtOtherHandling = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._cboMailHouseVendor = new System.Windows.Forms.ComboBox();
            this.mailhousesMailhouseIDandDescriptionByRunDateBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._lblOtherHandling = new System.Windows.Forms.Label();
            this._lblMailHouseVendor = new System.Windows.Forms.Label();
            this._chkLetterInsertion = new System.Windows.Forms.CheckBox();
            this._chkGlueTack = new System.Windows.Forms.CheckBox();
            this._chkTabbing = new System.Windows.Forms.CheckBox();
            this._groupPostage = new System.Windows.Forms.GroupBox();
            this._txtPostalType = new System.Windows.Forms.TextBox();
            this._txtPostalFuelSurcharge = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._txtPostalClass = new System.Windows.Forms.TextBox();
            this._cboPostalCategoryScenario = new System.Windows.Forms.ComboBox();
            this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._lblPostalFuelSurcharge = new System.Windows.Forms.Label();
            this._lblPostalType = new System.Windows.Forms.Label();
            this._lblPostalClass = new System.Windows.Forms.Label();
            this._lblPostalCategoryScenario = new System.Windows.Forms.Label();
            this._groupOtherOptions = new System.Windows.Forms.GroupBox();
            this._txtOtherFreight = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblOtherFreight = new System.Windows.Forms.Label();
            this._errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            this.estimatesBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._groupInsertOptions.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this._groupMailOptions.SuspendLayout();
            this._groupMailList.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.mailListsMailListIDandDescriptionByRunDateBindingSource ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this._dsEstimate ) ).BeginInit();
            this._groupMailTracking.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.mailTrackingsMailTrackingIDandDescriptionByRunDateBindingSource ) ).BeginInit();
            this._groupMailhouse.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.mailhousesMailhouseIDandDescriptionByRunDateBindingSource ) ).BeginInit();
            this._groupPostage.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource ) ).BeginInit();
            this._groupOtherOptions.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.estimatesBindingSource ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _groupInsertOptions
            // 
            this._groupInsertOptions.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._groupInsertOptions.Controls.Add( this._txtTotalInsertWeight );
            this._groupInsertOptions.Controls.Add( this._lblTotalInsertWeight );
            this._groupInsertOptions.Controls.Add( this._txtFuelSurcharge );
            this._groupInsertOptions.Controls.Add( this._lblFuelSurcharge );
            this._groupInsertOptions.Controls.Add( this._txtFreightCWT );
            this._groupInsertOptions.Controls.Add( this._lblFreightCWT );
            this._groupInsertOptions.Controls.Add( this._cboFreightVendor );
            this._groupInsertOptions.Controls.Add( this._lblFreightVendor );
            this._groupInsertOptions.Controls.Add( this.groupBox1 );
            this._groupInsertOptions.Controls.Add( this._chkSkids );
            this._groupInsertOptions.Controls.Add( this._chkCornerGuards );
            this._groupInsertOptions.Controls.Add( this._cboInsertDOW );
            this._groupInsertOptions.Controls.Add( this._lblInsertDOW );
            this._groupInsertOptions.Location = new System.Drawing.Point( 4, 4 );
            this._groupInsertOptions.Name = "_groupInsertOptions";
            this._groupInsertOptions.Size = new System.Drawing.Size( 704, 122 );
            this._groupInsertOptions.TabIndex = 0;
            this._groupInsertOptions.TabStop = false;
            this._groupInsertOptions.Text = "Insert Options";
            // 
            // _txtTotalInsertWeight
            // 
            this._txtTotalInsertWeight.FlashColor = System.Drawing.Color.Red;
            this._txtTotalInsertWeight.Location = new System.Drawing.Point( 566, 67 );
            this._txtTotalInsertWeight.Name = "_txtTotalInsertWeight";
            this._txtTotalInsertWeight.ReadOnly = true;
            this._txtTotalInsertWeight.Size = new System.Drawing.Size( 122, 20 );
            this._txtTotalInsertWeight.TabIndex = 10;
            this._txtTotalInsertWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtTotalInsertWeight.Value = null;
            // 
            // _lblTotalInsertWeight
            // 
            this._lblTotalInsertWeight.AutoSize = true;
            this._lblTotalInsertWeight.Location = new System.Drawing.Point( 463, 70 );
            this._lblTotalInsertWeight.Name = "_lblTotalInsertWeight";
            this._lblTotalInsertWeight.Size = new System.Drawing.Size( 97, 13 );
            this._lblTotalInsertWeight.TabIndex = 9;
            this._lblTotalInsertWeight.Text = "Total Insert Weight";
            // 
            // _txtFuelSurcharge
            // 
            this._txtFuelSurcharge.AllowNegative = false;
            this._txtFuelSurcharge.FlashColor = System.Drawing.Color.Red;
            this._txtFuelSurcharge.Location = new System.Drawing.Point( 332, 96 );
            this._txtFuelSurcharge.Name = "_txtFuelSurcharge";
            this._txtFuelSurcharge.Size = new System.Drawing.Size( 121, 20 );
            this._txtFuelSurcharge.TabIndex = 12;
            this._txtFuelSurcharge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtFuelSurcharge.Value = null;
            this._txtFuelSurcharge.Validated += new System.EventHandler( this.Control_ValidatedRequiredField );
            this._txtFuelSurcharge.Validating += new System.ComponentModel.CancelEventHandler( this.Control_ValidatingRequiredPercent );
            this._txtFuelSurcharge.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _lblFuelSurcharge
            // 
            this._lblFuelSurcharge.AutoSize = true;
            this._lblFuelSurcharge.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblFuelSurcharge.Location = new System.Drawing.Point( 210, 99 );
            this._lblFuelSurcharge.Name = "_lblFuelSurcharge";
            this._lblFuelSurcharge.Size = new System.Drawing.Size( 123, 13 );
            this._lblFuelSurcharge.TabIndex = 11;
            this._lblFuelSurcharge.Text = "Fuel Surcharge (%) *";
            // 
            // _txtFreightCWT
            // 
            this._txtFreightCWT.AllowNegative = false;
            this._txtFreightCWT.FlashColor = System.Drawing.Color.Red;
            this._txtFreightCWT.Location = new System.Drawing.Point( 332, 67 );
            this._txtFreightCWT.Name = "_txtFreightCWT";
            this._txtFreightCWT.Size = new System.Drawing.Size( 121, 20 );
            this._txtFreightCWT.TabIndex = 8;
            this._txtFreightCWT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtFreightCWT.Value = null;
            this._txtFreightCWT.Validated += new System.EventHandler( this.Control_ValidatedRequiredField );
            this._txtFreightCWT.Validating += new System.ComponentModel.CancelEventHandler( this.Control_ValidatingRequiredText );
            this._txtFreightCWT.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _lblFreightCWT
            // 
            this._lblFreightCWT.AutoSize = true;
            this._lblFreightCWT.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblFreightCWT.Location = new System.Drawing.Point( 210, 70 );
            this._lblFreightCWT.Name = "_lblFreightCWT";
            this._lblFreightCWT.Size = new System.Drawing.Size( 83, 13 );
            this._lblFreightCWT.TabIndex = 7;
            this._lblFreightCWT.Text = "Freight CWT*";
            // 
            // _cboFreightVendor
            // 
            this._cboFreightVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboFreightVendor.FormattingEnabled = true;
            this._cboFreightVendor.Location = new System.Drawing.Point( 332, 17 );
            this._cboFreightVendor.Name = "_cboFreightVendor";
            this._cboFreightVendor.Size = new System.Drawing.Size( 121, 21 );
            this._cboFreightVendor.TabIndex = 4;
            this._cboFreightVendor.Validating += new System.ComponentModel.CancelEventHandler( this.Control_ValidatingRequiredCombo );
            this._cboFreightVendor.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _lblFreightVendor
            // 
            this._lblFreightVendor.AutoSize = true;
            this._lblFreightVendor.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblFreightVendor.Location = new System.Drawing.Point( 210, 20 );
            this._lblFreightVendor.Name = "_lblFreightVendor";
            this._lblFreightVendor.Size = new System.Drawing.Size( 95, 13 );
            this._lblFreightVendor.TabIndex = 3;
            this._lblFreightVendor.Text = "Freight Vendor*";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this._radPMAM );
            this.groupBox1.Controls.Add( this._radAMPM );
            this.groupBox1.Location = new System.Drawing.Point( 10, 40 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 156, 30 );
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // _radPMAM
            // 
            this._radPMAM.AutoSize = true;
            this._radPMAM.Location = new System.Drawing.Point( 75, 10 );
            this._radPMAM.Name = "_radPMAM";
            this._radPMAM.Size = new System.Drawing.Size( 62, 17 );
            this._radPMAM.TabIndex = 1;
            this._radPMAM.Text = "PM/AM";
            this._radPMAM.UseVisualStyleBackColor = true;
            this._radPMAM.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _radAMPM
            // 
            this._radAMPM.AutoSize = true;
            this._radAMPM.Checked = true;
            this._radAMPM.Location = new System.Drawing.Point( 7, 10 );
            this._radAMPM.Name = "_radAMPM";
            this._radAMPM.Size = new System.Drawing.Size( 62, 17 );
            this._radAMPM.TabIndex = 0;
            this._radAMPM.TabStop = true;
            this._radAMPM.Text = "AM/PM";
            this._radAMPM.UseVisualStyleBackColor = true;
            this._radAMPM.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkSkids
            // 
            this._chkSkids.AutoSize = true;
            this._chkSkids.Location = new System.Drawing.Point( 432, 44 );
            this._chkSkids.Name = "_chkSkids";
            this._chkSkids.Size = new System.Drawing.Size( 52, 17 );
            this._chkSkids.TabIndex = 6;
            this._chkSkids.Text = "Skids";
            this._chkSkids.UseVisualStyleBackColor = true;
            this._chkSkids.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkCornerGuards
            // 
            this._chkCornerGuards.AutoSize = true;
            this._chkCornerGuards.Location = new System.Drawing.Point( 332, 44 );
            this._chkCornerGuards.Name = "_chkCornerGuards";
            this._chkCornerGuards.Size = new System.Drawing.Size( 94, 17 );
            this._chkCornerGuards.TabIndex = 5;
            this._chkCornerGuards.Text = "Corner Guards";
            this._chkCornerGuards.UseVisualStyleBackColor = true;
            this._chkCornerGuards.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _cboInsertDOW
            // 
            this._cboInsertDOW.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboInsertDOW.FormattingEnabled = true;
            this._cboInsertDOW.Location = new System.Drawing.Point( 85, 17 );
            this._cboInsertDOW.Name = "_cboInsertDOW";
            this._cboInsertDOW.Size = new System.Drawing.Size( 98, 21 );
            this._cboInsertDOW.TabIndex = 1;
            this._cboInsertDOW.Validating += new System.ComponentModel.CancelEventHandler( this.Control_ValidatingRequiredCombo );
            this._cboInsertDOW.Validated += new System.EventHandler( this.Control_ValidatedRequiredField );
            this._cboInsertDOW.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _lblInsertDOW
            // 
            this._lblInsertDOW.AutoSize = true;
            this._lblInsertDOW.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblInsertDOW.Location = new System.Drawing.Point( 7, 20 );
            this._lblInsertDOW.Name = "_lblInsertDOW";
            this._lblInsertDOW.Size = new System.Drawing.Size( 78, 13 );
            this._lblInsertDOW.TabIndex = 0;
            this._lblInsertDOW.Text = "Insert DOW*";
            // 
            // _groupMailOptions
            // 
            this._groupMailOptions.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._groupMailOptions.Controls.Add( this._txtTotalMailingWeight );
            this._groupMailOptions.Controls.Add( this._lblTotalMailingWeight );
            this._groupMailOptions.Controls.Add( this._txtNumberOfCartons );
            this._groupMailOptions.Controls.Add( this._lblNumberOfCartons );
            this._groupMailOptions.Controls.Add( this._groupMailList );
            this._groupMailOptions.Controls.Add( this._groupMailTracking );
            this._groupMailOptions.Controls.Add( this._groupMailhouse );
            this._groupMailOptions.Controls.Add( this._groupPostage );
            this._groupMailOptions.Location = new System.Drawing.Point( 4, 131 );
            this._groupMailOptions.Name = "_groupMailOptions";
            this._groupMailOptions.Size = new System.Drawing.Size( 704, 307 );
            this._groupMailOptions.TabIndex = 1;
            this._groupMailOptions.TabStop = false;
            this._groupMailOptions.Text = "Mail Options";
            // 
            // _txtTotalMailingWeight
            // 
            this._txtTotalMailingWeight.FlashColor = System.Drawing.Color.Red;
            this._txtTotalMailingWeight.Location = new System.Drawing.Point( 536, 185 );
            this._txtTotalMailingWeight.Name = "_txtTotalMailingWeight";
            this._txtTotalMailingWeight.ReadOnly = true;
            this._txtTotalMailingWeight.Size = new System.Drawing.Size( 122, 20 );
            this._txtTotalMailingWeight.TabIndex = 6;
            this._txtTotalMailingWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtTotalMailingWeight.Value = null;
            // 
            // _lblTotalMailingWeight
            // 
            this._lblTotalMailingWeight.AutoSize = true;
            this._lblTotalMailingWeight.Location = new System.Drawing.Point( 369, 185 );
            this._lblTotalMailingWeight.Name = "_lblTotalMailingWeight";
            this._lblTotalMailingWeight.Size = new System.Drawing.Size( 104, 13 );
            this._lblTotalMailingWeight.TabIndex = 5;
            this._lblTotalMailingWeight.Text = "Total Mailing Weight";
            // 
            // _txtNumberOfCartons
            // 
            this._txtNumberOfCartons.AllowNegative = false;
            this._txtNumberOfCartons.FlashColor = System.Drawing.Color.Red;
            this._txtNumberOfCartons.Location = new System.Drawing.Point( 536, 156 );
            this._txtNumberOfCartons.Name = "_txtNumberOfCartons";
            this._txtNumberOfCartons.Size = new System.Drawing.Size( 121, 20 );
            this._txtNumberOfCartons.TabIndex = 4;
            this._txtNumberOfCartons.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtNumberOfCartons.Value = null;
            this._txtNumberOfCartons.Validated += new System.EventHandler( this.Control_ValidatedRequiredField );
            this._txtNumberOfCartons.Validating += new System.ComponentModel.CancelEventHandler( this.Control_ValidatingRequiredText );
            this._txtNumberOfCartons.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _lblNumberOfCartons
            // 
            this._lblNumberOfCartons.AutoSize = true;
            this._lblNumberOfCartons.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblNumberOfCartons.Location = new System.Drawing.Point( 369, 159 );
            this._lblNumberOfCartons.Name = "_lblNumberOfCartons";
            this._lblNumberOfCartons.Size = new System.Drawing.Size( 82, 13 );
            this._lblNumberOfCartons.TabIndex = 3;
            this._lblNumberOfCartons.Text = "# of Cartons*";
            // 
            // _groupMailList
            // 
            this._groupMailList.Controls.Add( this._cboMailListResource );
            this._groupMailList.Controls.Add( this._lblMailListResource );
            this._groupMailList.Controls.Add( this._txtExternalCPM );
            this._groupMailList.Controls.Add( this._txtExternalQty );
            this._groupMailList.Controls.Add( this._lblExternalCPM );
            this._groupMailList.Controls.Add( this._lblExternalQty );
            this._groupMailList.Controls.Add( this._chkUseExternalMailList );
            this._groupMailList.Location = new System.Drawing.Point( 360, 19 );
            this._groupMailList.Name = "_groupMailList";
            this._groupMailList.Size = new System.Drawing.Size( 322, 120 );
            this._groupMailList.TabIndex = 1;
            this._groupMailList.TabStop = false;
            this._groupMailList.Text = "Mail List";
            // 
            // _cboMailListResource
            // 
            this._cboMailListResource.DataSource = this.mailListsMailListIDandDescriptionByRunDateBindingSource;
            this._cboMailListResource.DisplayMember = "Description";
            this._cboMailListResource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboMailListResource.FormattingEnabled = true;
            this._cboMailListResource.Location = new System.Drawing.Point( 176, 14 );
            this._cboMailListResource.Name = "_cboMailListResource";
            this._cboMailListResource.Size = new System.Drawing.Size( 121, 21 );
            this._cboMailListResource.TabIndex = 1;
            this._cboMailListResource.ValueMember = "vnd_maillistresourcerate_id";
            this._cboMailListResource.Validating += new System.ComponentModel.CancelEventHandler( this.Control_ValidatingRequiredCombo );
            this._cboMailListResource.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // mailListsMailListIDandDescriptionByRunDateBindingSource
            // 
            this.mailListsMailListIDandDescriptionByRunDateBindingSource.DataMember = "MailList_s_MailListIDandDescription_ByRunDate";
            this.mailListsMailListIDandDescriptionByRunDateBindingSource.DataSource = this._dsEstimate;
            // 
            // _dsEstimate
            // 
            this._dsEstimate.DataSetName = "Estimates";
            this._dsEstimate.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _lblMailListResource
            // 
            this._lblMailListResource.AutoSize = true;
            this._lblMailListResource.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblMailListResource.Location = new System.Drawing.Point( 9, 17 );
            this._lblMailListResource.Name = "_lblMailListResource";
            this._lblMailListResource.Size = new System.Drawing.Size( 161, 13 );
            this._lblMailListResource.TabIndex = 0;
            this._lblMailListResource.Text = "Mail List Resource Vendor*";
            // 
            // _txtExternalCPM
            // 
            this._txtExternalCPM.AllowNegative = false;
            this._txtExternalCPM.Enabled = false;
            this._txtExternalCPM.FlashColor = System.Drawing.Color.Red;
            this._txtExternalCPM.Location = new System.Drawing.Point( 176, 92 );
            this._txtExternalCPM.Name = "_txtExternalCPM";
            this._txtExternalCPM.Size = new System.Drawing.Size( 121, 20 );
            this._txtExternalCPM.TabIndex = 6;
            this._txtExternalCPM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtExternalCPM.Value = null;
            this._txtExternalCPM.Validated += new System.EventHandler( this.Control_ValidatedRequiredField );
            this._txtExternalCPM.Validating += new System.ComponentModel.CancelEventHandler( this.Control_ValidatingRequiredMailList );
            this._txtExternalCPM.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _txtExternalQty
            // 
            this._txtExternalQty.AllowNegative = false;
            this._txtExternalQty.Enabled = false;
            this._txtExternalQty.FlashColor = System.Drawing.Color.Red;
            this._txtExternalQty.Location = new System.Drawing.Point( 176, 66 );
            this._txtExternalQty.Name = "_txtExternalQty";
            this._txtExternalQty.Size = new System.Drawing.Size( 121, 20 );
            this._txtExternalQty.TabIndex = 4;
            this._txtExternalQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtExternalQty.Value = null;
            this._txtExternalQty.Validated += new System.EventHandler( this.Control_ValidatedRequiredField );
            this._txtExternalQty.Validating += new System.ComponentModel.CancelEventHandler( this.Control_ValidatingRequiredMailList );
            this._txtExternalQty.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _lblExternalCPM
            // 
            this._lblExternalCPM.AutoSize = true;
            this._lblExternalCPM.Location = new System.Drawing.Point( 29, 95 );
            this._lblExternalCPM.Name = "_lblExternalCPM";
            this._lblExternalCPM.Size = new System.Drawing.Size( 71, 13 );
            this._lblExternalCPM.TabIndex = 5;
            this._lblExternalCPM.Text = "External CPM";
            // 
            // _lblExternalQty
            // 
            this._lblExternalQty.AutoSize = true;
            this._lblExternalQty.Location = new System.Drawing.Point( 29, 69 );
            this._lblExternalQty.Name = "_lblExternalQty";
            this._lblExternalQty.Size = new System.Drawing.Size( 64, 13 );
            this._lblExternalQty.TabIndex = 3;
            this._lblExternalQty.Text = "External Qty";
            // 
            // _chkUseExternalMailList
            // 
            this._chkUseExternalMailList.AutoSize = true;
            this._chkUseExternalMailList.Location = new System.Drawing.Point( 12, 43 );
            this._chkUseExternalMailList.Name = "_chkUseExternalMailList";
            this._chkUseExternalMailList.Size = new System.Drawing.Size( 127, 17 );
            this._chkUseExternalMailList.TabIndex = 2;
            this._chkUseExternalMailList.Text = "Use External Mail List";
            this._chkUseExternalMailList.UseVisualStyleBackColor = true;
            this._chkUseExternalMailList.CheckedChanged += new System.EventHandler( this._chkUseExternalMailList_CheckedChanged );
            // 
            // _groupMailTracking
            // 
            this._groupMailTracking.Controls.Add( this._cboMailTracker );
            this._groupMailTracking.Controls.Add( this._chkUseMailTracking );
            this._groupMailTracking.Controls.Add( this._lblMailTracker );
            this._groupMailTracking.Location = new System.Drawing.Point( 360, 220 );
            this._groupMailTracking.Name = "_groupMailTracking";
            this._groupMailTracking.Size = new System.Drawing.Size( 322, 73 );
            this._groupMailTracking.TabIndex = 7;
            this._groupMailTracking.TabStop = false;
            this._groupMailTracking.Text = "Mail Tracking";
            // 
            // _cboMailTracker
            // 
            this._cboMailTracker.DataSource = this.mailTrackingsMailTrackingIDandDescriptionByRunDateBindingSource;
            this._cboMailTracker.DisplayMember = "Description";
            this._cboMailTracker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboMailTracker.Enabled = false;
            this._cboMailTracker.FormattingEnabled = true;
            this._cboMailTracker.Location = new System.Drawing.Point( 176, 42 );
            this._cboMailTracker.Name = "_cboMailTracker";
            this._cboMailTracker.Size = new System.Drawing.Size( 121, 21 );
            this._cboMailTracker.TabIndex = 2;
            this._cboMailTracker.ValueMember = "vnd_mailtrackingrate_id";
            this._cboMailTracker.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // mailTrackingsMailTrackingIDandDescriptionByRunDateBindingSource
            // 
            this.mailTrackingsMailTrackingIDandDescriptionByRunDateBindingSource.DataMember = "MailTracking_s_MailTrackingIDandDescription_ByRunDate";
            this.mailTrackingsMailTrackingIDandDescriptionByRunDateBindingSource.DataSource = this._dsEstimate;
            // 
            // _chkUseMailTracking
            // 
            this._chkUseMailTracking.AutoSize = true;
            this._chkUseMailTracking.Location = new System.Drawing.Point( 12, 19 );
            this._chkUseMailTracking.Name = "_chkUseMailTracking";
            this._chkUseMailTracking.Size = new System.Drawing.Size( 112, 17 );
            this._chkUseMailTracking.TabIndex = 0;
            this._chkUseMailTracking.Text = "Use Mail Tracking";
            this._chkUseMailTracking.UseVisualStyleBackColor = true;
            this._chkUseMailTracking.CheckedChanged += new System.EventHandler( this._chkUseMailTracking_CheckedChanged );
            // 
            // _lblMailTracker
            // 
            this._lblMailTracker.AutoSize = true;
            this._lblMailTracker.Location = new System.Drawing.Point( 29, 45 );
            this._lblMailTracker.Name = "_lblMailTracker";
            this._lblMailTracker.Size = new System.Drawing.Size( 108, 13 );
            this._lblMailTracker.TabIndex = 1;
            this._lblMailTracker.Text = "Mail Tracking Vendor";
            // 
            // _groupMailhouse
            // 
            this._groupMailhouse.Controls.Add( this._txtPostalDropFlat );
            this._groupMailhouse.Controls.Add( this._lblPostalDropFlat );
            this._groupMailhouse.Controls.Add( this._txtPostalDropCWT );
            this._groupMailhouse.Controls.Add( this._lblPostalDropCWT );
            this._groupMailhouse.Controls.Add( this._txtOtherHandling );
            this._groupMailhouse.Controls.Add( this._cboMailHouseVendor );
            this._groupMailhouse.Controls.Add( this._lblOtherHandling );
            this._groupMailhouse.Controls.Add( this._lblMailHouseVendor );
            this._groupMailhouse.Controls.Add( this._chkLetterInsertion );
            this._groupMailhouse.Controls.Add( this._chkGlueTack );
            this._groupMailhouse.Controls.Add( this._chkTabbing );
            this._groupMailhouse.Location = new System.Drawing.Point( 10, 145 );
            this._groupMailhouse.Name = "_groupMailhouse";
            this._groupMailhouse.Size = new System.Drawing.Size( 330, 148 );
            this._groupMailhouse.TabIndex = 2;
            this._groupMailhouse.TabStop = false;
            this._groupMailhouse.Text = "Mailhouse";
            // 
            // _txtPostalDropFlat
            // 
            this._txtPostalDropFlat.AllowNegative = false;
            this._txtPostalDropFlat.FlashColor = System.Drawing.Color.Red;
            this._txtPostalDropFlat.Location = new System.Drawing.Point( 179, 92 );
            this._txtPostalDropFlat.Name = "_txtPostalDropFlat";
            this._txtPostalDropFlat.Size = new System.Drawing.Size( 121, 20 );
            this._txtPostalDropFlat.TabIndex = 7;
            this._txtPostalDropFlat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtPostalDropFlat.Value = null;
            // 
            // _lblPostalDropFlat
            // 
            this._lblPostalDropFlat.AutoSize = true;
            this._lblPostalDropFlat.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblPostalDropFlat.Location = new System.Drawing.Point( 6, 95 );
            this._lblPostalDropFlat.Name = "_lblPostalDropFlat";
            this._lblPostalDropFlat.Size = new System.Drawing.Size( 82, 13 );
            this._lblPostalDropFlat.TabIndex = 6;
            this._lblPostalDropFlat.Text = "Postal Drop Flat";
            // 
            // _txtPostalDropCWT
            // 
            this._txtPostalDropCWT.FlashColor = System.Drawing.Color.Red;
            this._txtPostalDropCWT.Location = new System.Drawing.Point( 179, 66 );
            this._txtPostalDropCWT.Name = "_txtPostalDropCWT";
            this._txtPostalDropCWT.ReadOnly = true;
            this._txtPostalDropCWT.Size = new System.Drawing.Size( 122, 20 );
            this._txtPostalDropCWT.TabIndex = 5;
            this._txtPostalDropCWT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtPostalDropCWT.Value = null;
            // 
            // _lblPostalDropCWT
            // 
            this._lblPostalDropCWT.AutoSize = true;
            this._lblPostalDropCWT.Location = new System.Drawing.Point( 7, 69 );
            this._lblPostalDropCWT.Name = "_lblPostalDropCWT";
            this._lblPostalDropCWT.Size = new System.Drawing.Size( 90, 13 );
            this._lblPostalDropCWT.TabIndex = 4;
            this._lblPostalDropCWT.Text = "Postal Drop CWT";
            // 
            // _txtOtherHandling
            // 
            this._txtOtherHandling.AllowNegative = false;
            this._txtOtherHandling.FlashColor = System.Drawing.Color.Red;
            this._txtOtherHandling.Location = new System.Drawing.Point( 180, 40 );
            this._txtOtherHandling.Name = "_txtOtherHandling";
            this._txtOtherHandling.Size = new System.Drawing.Size( 121, 20 );
            this._txtOtherHandling.TabIndex = 3;
            this._txtOtherHandling.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtOtherHandling.Value = null;
            this._txtOtherHandling.Validated += new System.EventHandler( this.Control_ValidatedRequiredField );
            this._txtOtherHandling.Validating += new System.ComponentModel.CancelEventHandler( this.Control_ValidatingRequiredText );
            this._txtOtherHandling.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _cboMailHouseVendor
            // 
            this._cboMailHouseVendor.DataSource = this.mailhousesMailhouseIDandDescriptionByRunDateBindingSource;
            this._cboMailHouseVendor.DisplayMember = "Description";
            this._cboMailHouseVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboMailHouseVendor.FormattingEnabled = true;
            this._cboMailHouseVendor.Location = new System.Drawing.Point( 180, 13 );
            this._cboMailHouseVendor.Name = "_cboMailHouseVendor";
            this._cboMailHouseVendor.Size = new System.Drawing.Size( 121, 21 );
            this._cboMailHouseVendor.TabIndex = 1;
            this._cboMailHouseVendor.ValueMember = "vnd_mailhouserate_id";
            this._cboMailHouseVendor.Validating += new System.ComponentModel.CancelEventHandler( this.Control_ValidatingRequiredCombo );
            this._cboMailHouseVendor.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            this._cboMailHouseVendor.SelectedValueChanged += new System.EventHandler( this._cboMailHouseVendor_SelectedValueChanged );
            // 
            // mailhousesMailhouseIDandDescriptionByRunDateBindingSource
            // 
            this.mailhousesMailhouseIDandDescriptionByRunDateBindingSource.DataMember = "Mailhouse_s_MailhouseIDandDescription_ByRunDate";
            this.mailhousesMailhouseIDandDescriptionByRunDateBindingSource.DataSource = this._dsEstimate;
            // 
            // _lblOtherHandling
            // 
            this._lblOtherHandling.AutoSize = true;
            this._lblOtherHandling.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblOtherHandling.Location = new System.Drawing.Point( 7, 43 );
            this._lblOtherHandling.Name = "_lblOtherHandling";
            this._lblOtherHandling.Size = new System.Drawing.Size( 164, 13 );
            this._lblOtherHandling.TabIndex = 2;
            this._lblOtherHandling.Text = "Mail House Other Handling*";
            // 
            // _lblMailHouseVendor
            // 
            this._lblMailHouseVendor.AutoSize = true;
            this._lblMailHouseVendor.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblMailHouseVendor.Location = new System.Drawing.Point( 7, 17 );
            this._lblMailHouseVendor.Name = "_lblMailHouseVendor";
            this._lblMailHouseVendor.Size = new System.Drawing.Size( 119, 13 );
            this._lblMailHouseVendor.TabIndex = 0;
            this._lblMailHouseVendor.Text = "Mail House Vendor*";
            // 
            // _chkLetterInsertion
            // 
            this._chkLetterInsertion.AutoSize = true;
            this._chkLetterInsertion.Location = new System.Drawing.Point( 179, 118 );
            this._chkLetterInsertion.Name = "_chkLetterInsertion";
            this._chkLetterInsertion.Size = new System.Drawing.Size( 96, 17 );
            this._chkLetterInsertion.TabIndex = 10;
            this._chkLetterInsertion.Text = "Letter Insertion";
            this._chkLetterInsertion.UseVisualStyleBackColor = true;
            this._chkLetterInsertion.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkGlueTack
            // 
            this._chkGlueTack.AutoSize = true;
            this._chkGlueTack.Location = new System.Drawing.Point( 10, 118 );
            this._chkGlueTack.Name = "_chkGlueTack";
            this._chkGlueTack.Size = new System.Drawing.Size( 76, 17 );
            this._chkGlueTack.TabIndex = 8;
            this._chkGlueTack.Text = "Glue Tack";
            this._chkGlueTack.UseVisualStyleBackColor = true;
            this._chkGlueTack.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkTabbing
            // 
            this._chkTabbing.AutoSize = true;
            this._chkTabbing.Location = new System.Drawing.Point( 106, 118 );
            this._chkTabbing.Name = "_chkTabbing";
            this._chkTabbing.Size = new System.Drawing.Size( 65, 17 );
            this._chkTabbing.TabIndex = 9;
            this._chkTabbing.Text = "Tabbing";
            this._chkTabbing.UseVisualStyleBackColor = true;
            this._chkTabbing.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _groupPostage
            // 
            this._groupPostage.Controls.Add( this._txtPostalType );
            this._groupPostage.Controls.Add( this._txtPostalFuelSurcharge );
            this._groupPostage.Controls.Add( this._txtPostalClass );
            this._groupPostage.Controls.Add( this._cboPostalCategoryScenario );
            this._groupPostage.Controls.Add( this._lblPostalFuelSurcharge );
            this._groupPostage.Controls.Add( this._lblPostalType );
            this._groupPostage.Controls.Add( this._lblPostalClass );
            this._groupPostage.Controls.Add( this._lblPostalCategoryScenario );
            this._groupPostage.Location = new System.Drawing.Point( 10, 19 );
            this._groupPostage.Name = "_groupPostage";
            this._groupPostage.Size = new System.Drawing.Size( 330, 120 );
            this._groupPostage.TabIndex = 0;
            this._groupPostage.TabStop = false;
            this._groupPostage.Text = "Postage";
            // 
            // _txtPostalType
            // 
            this._txtPostalType.Location = new System.Drawing.Point( 179, 66 );
            this._txtPostalType.Name = "_txtPostalType";
            this._txtPostalType.ReadOnly = true;
            this._txtPostalType.Size = new System.Drawing.Size( 121, 20 );
            this._txtPostalType.TabIndex = 5;
            this._txtPostalType.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _txtPostalFuelSurcharge
            // 
            this._txtPostalFuelSurcharge.AllowNegative = false;
            this._txtPostalFuelSurcharge.FlashColor = System.Drawing.Color.Red;
            this._txtPostalFuelSurcharge.Location = new System.Drawing.Point( 179, 92 );
            this._txtPostalFuelSurcharge.Name = "_txtPostalFuelSurcharge";
            this._txtPostalFuelSurcharge.Size = new System.Drawing.Size( 122, 20 );
            this._txtPostalFuelSurcharge.TabIndex = 7;
            this._txtPostalFuelSurcharge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtPostalFuelSurcharge.Value = null;
            this._txtPostalFuelSurcharge.Validated += new System.EventHandler( this.Control_ValidatedRequiredField );
            this._txtPostalFuelSurcharge.Validating += new System.ComponentModel.CancelEventHandler( this.Control_ValidatingRequiredPercent );
            this._txtPostalFuelSurcharge.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _txtPostalClass
            // 
            this._txtPostalClass.Location = new System.Drawing.Point( 179, 40 );
            this._txtPostalClass.Name = "_txtPostalClass";
            this._txtPostalClass.ReadOnly = true;
            this._txtPostalClass.Size = new System.Drawing.Size( 121, 20 );
            this._txtPostalClass.TabIndex = 3;
            this._txtPostalClass.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _cboPostalCategoryScenario
            // 
            this._cboPostalCategoryScenario.DataSource = this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource;
            this._cboPostalCategoryScenario.DisplayMember = "description";
            this._cboPostalCategoryScenario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboPostalCategoryScenario.FormattingEnabled = true;
            this._cboPostalCategoryScenario.Location = new System.Drawing.Point( 179, 14 );
            this._cboPostalCategoryScenario.Name = "_cboPostalCategoryScenario";
            this._cboPostalCategoryScenario.Size = new System.Drawing.Size( 121, 21 );
            this._cboPostalCategoryScenario.TabIndex = 1;
            this._cboPostalCategoryScenario.ValueMember = "pst_postalscenario_id";
            this._cboPostalCategoryScenario.Validating += new System.ComponentModel.CancelEventHandler( this.Control_ValidatingRequiredCombo );
            this._cboPostalCategoryScenario.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            this._cboPostalCategoryScenario.SelectedValueChanged += new System.EventHandler( this._cboPostalCategoryScenario_SelectedValueChanged );
            // 
            // postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource
            // 
            this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource.DataMember = "PostalScenario_s_PostalScenarioIDandDescription_ByRunDate";
            this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource.DataSource = this._dsEstimate;
            this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource.Sort = "description ASC";
            // 
            // _lblPostalFuelSurcharge
            // 
            this._lblPostalFuelSurcharge.AutoSize = true;
            this._lblPostalFuelSurcharge.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblPostalFuelSurcharge.Location = new System.Drawing.Point( 7, 95 );
            this._lblPostalFuelSurcharge.Name = "_lblPostalFuelSurcharge";
            this._lblPostalFuelSurcharge.Size = new System.Drawing.Size( 123, 13 );
            this._lblPostalFuelSurcharge.TabIndex = 6;
            this._lblPostalFuelSurcharge.Text = "Fuel Surcharge (%) *";
            // 
            // _lblPostalType
            // 
            this._lblPostalType.AutoSize = true;
            this._lblPostalType.Location = new System.Drawing.Point( 7, 69 );
            this._lblPostalType.Name = "_lblPostalType";
            this._lblPostalType.Size = new System.Drawing.Size( 63, 13 );
            this._lblPostalType.TabIndex = 4;
            this._lblPostalType.Text = "Postal Type";
            // 
            // _lblPostalClass
            // 
            this._lblPostalClass.AutoSize = true;
            this._lblPostalClass.Location = new System.Drawing.Point( 7, 43 );
            this._lblPostalClass.Name = "_lblPostalClass";
            this._lblPostalClass.Size = new System.Drawing.Size( 64, 13 );
            this._lblPostalClass.TabIndex = 2;
            this._lblPostalClass.Text = "Postal Class";
            // 
            // _lblPostalCategoryScenario
            // 
            this._lblPostalCategoryScenario.AutoSize = true;
            this._lblPostalCategoryScenario.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblPostalCategoryScenario.Location = new System.Drawing.Point( 7, 17 );
            this._lblPostalCategoryScenario.Name = "_lblPostalCategoryScenario";
            this._lblPostalCategoryScenario.Size = new System.Drawing.Size( 166, 13 );
            this._lblPostalCategoryScenario.TabIndex = 0;
            this._lblPostalCategoryScenario.Text = "Postage Category Scenario*";
            // 
            // _groupOtherOptions
            // 
            this._groupOtherOptions.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._groupOtherOptions.Controls.Add( this._txtOtherFreight );
            this._groupOtherOptions.Controls.Add( this._lblOtherFreight );
            this._groupOtherOptions.Location = new System.Drawing.Point( 4, 443 );
            this._groupOtherOptions.Name = "_groupOtherOptions";
            this._groupOtherOptions.Size = new System.Drawing.Size( 704, 48 );
            this._groupOtherOptions.TabIndex = 2;
            this._groupOtherOptions.TabStop = false;
            this._groupOtherOptions.Text = "Other Options";
            // 
            // _txtOtherFreight
            // 
            this._txtOtherFreight.AllowNegative = false;
            this._txtOtherFreight.FlashColor = System.Drawing.Color.Red;
            this._txtOtherFreight.Location = new System.Drawing.Point( 168, 17 );
            this._txtOtherFreight.Name = "_txtOtherFreight";
            this._txtOtherFreight.Size = new System.Drawing.Size( 121, 20 );
            this._txtOtherFreight.TabIndex = 1;
            this._txtOtherFreight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtOtherFreight.Value = null;
            this._txtOtherFreight.Validated += new System.EventHandler( this.Control_ValidatedRequiredField );
            this._txtOtherFreight.Validating += new System.ComponentModel.CancelEventHandler( this.Control_ValidatingRequiredText );
            this._txtOtherFreight.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _lblOtherFreight
            // 
            this._lblOtherFreight.AutoSize = true;
            this._lblOtherFreight.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblOtherFreight.Location = new System.Drawing.Point( 8, 20 );
            this._lblOtherFreight.Name = "_lblOtherFreight";
            this._lblOtherFreight.Size = new System.Drawing.Size( 51, 13 );
            this._lblOtherFreight.TabIndex = 0;
            this._lblOtherFreight.Text = "Freight*";
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // estimatesBindingSource
            // 
            this.estimatesBindingSource.DataSource = this._dsEstimate;
            this.estimatesBindingSource.Position = 0;
            // 
            // ucpAssemblyDistributionOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._groupOtherOptions );
            this.Controls.Add( this._groupMailOptions );
            this.Controls.Add( this._groupInsertOptions );
            this.Name = "ucpAssemblyDistributionOptions";
            this.Size = new System.Drawing.Size( 724, 509 );
            this._groupInsertOptions.ResumeLayout( false );
            this._groupInsertOptions.PerformLayout();
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            this._groupMailOptions.ResumeLayout( false );
            this._groupMailOptions.PerformLayout();
            this._groupMailList.ResumeLayout( false );
            this._groupMailList.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.mailListsMailListIDandDescriptionByRunDateBindingSource ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this._dsEstimate ) ).EndInit();
            this._groupMailTracking.ResumeLayout( false );
            this._groupMailTracking.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.mailTrackingsMailTrackingIDandDescriptionByRunDateBindingSource ) ).EndInit();
            this._groupMailhouse.ResumeLayout( false );
            this._groupMailhouse.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.mailhousesMailhouseIDandDescriptionByRunDateBindingSource ) ).EndInit();
            this._groupPostage.ResumeLayout( false );
            this._groupPostage.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource ) ).EndInit();
            this._groupOtherOptions.ResumeLayout( false );
            this._groupOtherOptions.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.estimatesBindingSource ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox _groupInsertOptions;
        private System.Windows.Forms.GroupBox _groupMailOptions;
        private System.Windows.Forms.GroupBox _groupOtherOptions;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtFreightCWT;
        private System.Windows.Forms.Label _lblFreightCWT;
        private System.Windows.Forms.ComboBox _cboFreightVendor;
        private System.Windows.Forms.Label _lblFreightVendor;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton _radPMAM;
        private System.Windows.Forms.RadioButton _radAMPM;
        private System.Windows.Forms.CheckBox _chkSkids;
        private System.Windows.Forms.CheckBox _chkCornerGuards;
        private System.Windows.Forms.ComboBox _cboInsertDOW;
        private System.Windows.Forms.Label _lblInsertDOW;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtFuelSurcharge;
        private System.Windows.Forms.Label _lblFuelSurcharge;
        private System.Windows.Forms.CheckBox _chkLetterInsertion;
        private System.Windows.Forms.CheckBox _chkTabbing;
        private System.Windows.Forms.CheckBox _chkGlueTack;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtOtherFreight;
        private System.Windows.Forms.Label _lblOtherFreight;
        private System.Windows.Forms.GroupBox _groupPostage;
        private System.Windows.Forms.TextBox _txtPostalType;
        private System.Windows.Forms.TextBox _txtPostalClass;
        private System.Windows.Forms.ComboBox _cboPostalCategoryScenario;
        private System.Windows.Forms.Label _lblPostalType;
        private System.Windows.Forms.Label _lblPostalClass;
        private System.Windows.Forms.Label _lblPostalCategoryScenario;
        private System.Windows.Forms.GroupBox _groupMailTracking;
        private System.Windows.Forms.ComboBox _cboMailTracker;
        private System.Windows.Forms.CheckBox _chkUseMailTracking;
        private System.Windows.Forms.Label _lblMailTracker;
        private System.Windows.Forms.GroupBox _groupMailhouse;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtOtherHandling;
        private System.Windows.Forms.ComboBox _cboMailHouseVendor;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtPostalFuelSurcharge;
        private System.Windows.Forms.Label _lblOtherHandling;
        private System.Windows.Forms.Label _lblMailHouseVendor;
        private System.Windows.Forms.Label _lblPostalFuelSurcharge;
        private System.Windows.Forms.GroupBox _groupMailList;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtExternalCPM;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtExternalQty;
        private System.Windows.Forms.Label _lblExternalCPM;
        private System.Windows.Forms.Label _lblExternalQty;
        private System.Windows.Forms.CheckBox _chkUseExternalMailList;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtNumberOfCartons;
        private System.Windows.Forms.Label _lblNumberOfCartons;
        private System.Windows.Forms.ComboBox _cboMailListResource;
        private System.Windows.Forms.Label _lblMailListResource;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtPostalDropCWT;
        private System.Windows.Forms.Label _lblPostalDropCWT;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.BindingSource mailListsMailListIDandDescriptionByRunDateBindingSource;
        private CatalogEstimating.Datasets.Estimates _dsEstimate;
        private System.Windows.Forms.BindingSource mailhousesMailhouseIDandDescriptionByRunDateBindingSource;
        private System.Windows.Forms.BindingSource estimatesBindingSource;
        private System.Windows.Forms.BindingSource mailTrackingsMailTrackingIDandDescriptionByRunDateBindingSource;
        private System.Windows.Forms.BindingSource postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtPostalDropFlat;
        private System.Windows.Forms.Label _lblPostalDropFlat;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtTotalInsertWeight;
        private System.Windows.Forms.Label _lblTotalInsertWeight;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtTotalMailingWeight;
        private System.Windows.Forms.Label _lblTotalMailingWeight;
    }
}
