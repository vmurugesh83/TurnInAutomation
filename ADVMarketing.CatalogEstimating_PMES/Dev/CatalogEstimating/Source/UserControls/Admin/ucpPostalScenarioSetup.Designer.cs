namespace CatalogEstimating.UserControls.Admin
{
    partial class ucpPostalScenarioSetup
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
            this._lblVendor = new System.Windows.Forms.Label();
            this._cboVendor = new System.Windows.Forms.ComboBox();
            this.vendorListforScenariosBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this.postal = new CatalogEstimating.Datasets.Postal();
            this.vndvendorBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._lblPostalScenario = new System.Windows.Forms.Label();
            this._cboPostalScenario = new System.Windows.Forms.ComboBox();
            this._lblEffectiveDate = new System.Windows.Forms.Label();
            this._cboEffectiveDate = new System.Windows.Forms.ComboBox();
            this._groupScenarioSetup = new System.Windows.Forms.GroupBox();
            this._lblInfoText = new System.Windows.Forms.Label();
            this._groupPostalCategoryScenarioSetup = new System.Windows.Forms.GroupBox();
            this._gridPostalCategoryScenarioSetup = new System.Windows.Forms.DataGridView();
            this._lblCategoriesInScenario = new System.Windows.Forms.Label();
            this._btnUpdate = new System.Windows.Forms.Button();
            this._lblAvailableCategories = new System.Windows.Forms.Label();
            this._txtTotalPercentage = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblTotalPercentage = new System.Windows.Forms.Label();
            this._btnRemoveAll = new System.Windows.Forms.Button();
            this._btnRemove = new System.Windows.Forms.Button();
            this._btnAddAll = new System.Windows.Forms.Button();
            this._btnAdd = new System.Windows.Forms.Button();
            this._lbPostalCategory = new System.Windows.Forms.ListBox();
            this._dtEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this._lblSetupEffectiveDate = new System.Windows.Forms.Label();
            this._cboPostalMailerType = new System.Windows.Forms.ComboBox();
            this.pstpostalmailertypeBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._lblPostalMailerType = new System.Windows.Forms.Label();
            this._cboPostageClass = new System.Windows.Forms.ComboBox();
            this.pstpostalclassBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._lblPostalClass = new System.Windows.Forms.Label();
            this._txtComments = new System.Windows.Forms.TextBox();
            this._lblComments = new System.Windows.Forms.Label();
            this._txtDescription = new System.Windows.Forms.TextBox();
            this._lblDescription = new System.Windows.Forms.Label();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnNew = new System.Windows.Forms.ToolStripButton();
            this._btnDelete = new System.Windows.Forms.ToolStripButton();
            this.vnd_vendorTableAdapter = new CatalogEstimating.Datasets.PostalTableAdapters.vnd_vendorTableAdapter();
            this.pst_postalclassTableAdapter = new CatalogEstimating.Datasets.PostalTableAdapters.pst_postalclassTableAdapter();
            this.pst_postalmailertypeTableAdapter = new CatalogEstimating.Datasets.PostalTableAdapters.pst_postalmailertypeTableAdapter();
            this._errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            this.pst_postalcategoryrate_map_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Percentage = new CatalogEstimating.CustomControls.DecimalColumn();
            ( (System.ComponentModel.ISupportInitialize)( this.vendorListforScenariosBindingSource ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.postal ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.vndvendorBindingSource ) ).BeginInit();
            this._groupScenarioSetup.SuspendLayout();
            this._groupPostalCategoryScenarioSetup.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._gridPostalCategoryScenarioSetup ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pstpostalmailertypeBindingSource ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pstpostalclassBindingSource ) ).BeginInit();
            this._toolStrip.SuspendLayout();
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
            // vendorListforScenariosBindingSource
            // 
            this.vendorListforScenariosBindingSource.DataMember = "VendorList_for_Scenarios";
            this.vendorListforScenariosBindingSource.DataSource = this.postal;
            // 
            // postal
            // 
            this.postal.DataSetName = "Postal";
            this.postal.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // vndvendorBindingSource
            // 
            this.vndvendorBindingSource.DataMember = "vnd_vendor";
            this.vndvendorBindingSource.DataSource = this.postal;
            // 
            // _lblPostalScenario
            // 
            this._lblPostalScenario.AutoSize = true;
            this._lblPostalScenario.Location = new System.Drawing.Point( 12, 42 );
            this._lblPostalScenario.Name = "_lblPostalScenario";
            this._lblPostalScenario.Size = new System.Drawing.Size( 49, 13 );
            this._lblPostalScenario.TabIndex = 2;
            this._lblPostalScenario.Text = "Scenario";
            // 
            // _cboPostalScenario
            // 
            this._cboPostalScenario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboPostalScenario.FormattingEnabled = true;
            this._cboPostalScenario.Location = new System.Drawing.Point( 120, 39 );
            this._cboPostalScenario.Name = "_cboPostalScenario";
            this._cboPostalScenario.Size = new System.Drawing.Size( 200, 21 );
            this._cboPostalScenario.Sorted = true;
            this._cboPostalScenario.TabIndex = 3;
            this._cboPostalScenario.SelectedIndexChanged += new System.EventHandler( this._cboPostalScenario_SelectedIndexChanged );
            // 
            // _lblEffectiveDate
            // 
            this._lblEffectiveDate.AutoSize = true;
            this._lblEffectiveDate.Location = new System.Drawing.Point( 12, 71 );
            this._lblEffectiveDate.Name = "_lblEffectiveDate";
            this._lblEffectiveDate.Size = new System.Drawing.Size( 75, 13 );
            this._lblEffectiveDate.TabIndex = 4;
            this._lblEffectiveDate.Text = "Effective Date";
            // 
            // _cboEffectiveDate
            // 
            this._cboEffectiveDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboEffectiveDate.FormattingEnabled = true;
            this._cboEffectiveDate.Location = new System.Drawing.Point( 120, 68 );
            this._cboEffectiveDate.Name = "_cboEffectiveDate";
            this._cboEffectiveDate.Size = new System.Drawing.Size( 200, 21 );
            this._cboEffectiveDate.TabIndex = 5;
            this._cboEffectiveDate.SelectedIndexChanged += new System.EventHandler( this._cboEffectiveDate_SelectedIndexChanged );
            // 
            // _groupScenarioSetup
            // 
            this._groupScenarioSetup.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._groupScenarioSetup.Controls.Add( this._lblInfoText );
            this._groupScenarioSetup.Controls.Add( this._groupPostalCategoryScenarioSetup );
            this._groupScenarioSetup.Controls.Add( this._dtEffectiveDate );
            this._groupScenarioSetup.Controls.Add( this._lblSetupEffectiveDate );
            this._groupScenarioSetup.Controls.Add( this._cboPostalMailerType );
            this._groupScenarioSetup.Controls.Add( this._lblPostalMailerType );
            this._groupScenarioSetup.Controls.Add( this._cboPostageClass );
            this._groupScenarioSetup.Controls.Add( this._lblPostalClass );
            this._groupScenarioSetup.Controls.Add( this._txtComments );
            this._groupScenarioSetup.Controls.Add( this._lblComments );
            this._groupScenarioSetup.Controls.Add( this._txtDescription );
            this._groupScenarioSetup.Controls.Add( this._lblDescription );
            this._groupScenarioSetup.Location = new System.Drawing.Point( 7, 95 );
            this._groupScenarioSetup.Name = "_groupScenarioSetup";
            this._groupScenarioSetup.Size = new System.Drawing.Size( 653, 406 );
            this._groupScenarioSetup.TabIndex = 6;
            this._groupScenarioSetup.TabStop = false;
            this._groupScenarioSetup.Text = "Postal Scenario Setup";
            // 
            // _lblInfoText
            // 
            this._lblInfoText.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._lblInfoText.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblInfoText.ForeColor = System.Drawing.Color.Blue;
            this._lblInfoText.Location = new System.Drawing.Point( 8, 20 );
            this._lblInfoText.Name = "_lblInfoText";
            this._lblInfoText.Size = new System.Drawing.Size( 639, 20 );
            this._lblInfoText.TabIndex = 0;
            // 
            // _groupPostalCategoryScenarioSetup
            // 
            this._groupPostalCategoryScenarioSetup.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._groupPostalCategoryScenarioSetup.Controls.Add( this._gridPostalCategoryScenarioSetup );
            this._groupPostalCategoryScenarioSetup.Controls.Add( this._lblCategoriesInScenario );
            this._groupPostalCategoryScenarioSetup.Controls.Add( this._btnUpdate );
            this._groupPostalCategoryScenarioSetup.Controls.Add( this._lblAvailableCategories );
            this._groupPostalCategoryScenarioSetup.Controls.Add( this._txtTotalPercentage );
            this._groupPostalCategoryScenarioSetup.Controls.Add( this._lblTotalPercentage );
            this._groupPostalCategoryScenarioSetup.Controls.Add( this._btnRemoveAll );
            this._groupPostalCategoryScenarioSetup.Controls.Add( this._btnRemove );
            this._groupPostalCategoryScenarioSetup.Controls.Add( this._btnAddAll );
            this._groupPostalCategoryScenarioSetup.Controls.Add( this._btnAdd );
            this._groupPostalCategoryScenarioSetup.Controls.Add( this._lbPostalCategory );
            this._groupPostalCategoryScenarioSetup.Location = new System.Drawing.Point( 12, 162 );
            this._groupPostalCategoryScenarioSetup.Name = "_groupPostalCategoryScenarioSetup";
            this._groupPostalCategoryScenarioSetup.Size = new System.Drawing.Size( 635, 238 );
            this._groupPostalCategoryScenarioSetup.TabIndex = 11;
            this._groupPostalCategoryScenarioSetup.TabStop = false;
            this._groupPostalCategoryScenarioSetup.Text = "Postal Category Scenario Setup";
            // 
            // _gridPostalCategoryScenarioSetup
            // 
            this._gridPostalCategoryScenarioSetup.AllowUserToAddRows = false;
            this._gridPostalCategoryScenarioSetup.AllowUserToDeleteRows = false;
            this._gridPostalCategoryScenarioSetup.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._gridPostalCategoryScenarioSetup.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridPostalCategoryScenarioSetup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridPostalCategoryScenarioSetup.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.pst_postalcategoryrate_map_id,
            this.Category,
            this.Percentage} );
            this._gridPostalCategoryScenarioSetup.Location = new System.Drawing.Point( 325, 44 );
            this._gridPostalCategoryScenarioSetup.Name = "_gridPostalCategoryScenarioSetup";
            this._gridPostalCategoryScenarioSetup.RowHeadersVisible = false;
            this._gridPostalCategoryScenarioSetup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._gridPostalCategoryScenarioSetup.Size = new System.Drawing.Size( 304, 160 );
            this._gridPostalCategoryScenarioSetup.TabIndex = 7;
            this._gridPostalCategoryScenarioSetup.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler( this._gridPostalCategoryScenarioSetup_CellValueChanged );
            // 
            // _lblCategoriesInScenario
            // 
            this._lblCategoriesInScenario.AutoSize = true;
            this._lblCategoriesInScenario.Location = new System.Drawing.Point( 325, 27 );
            this._lblCategoriesInScenario.Name = "_lblCategoriesInScenario";
            this._lblCategoriesInScenario.Size = new System.Drawing.Size( 114, 13 );
            this._lblCategoriesInScenario.TabIndex = 6;
            this._lblCategoriesInScenario.Text = "Categories In Scenario";
            // 
            // _btnUpdate
            // 
            this._btnUpdate.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this._btnUpdate.Enabled = false;
            this._btnUpdate.Location = new System.Drawing.Point( 538, 209 );
            this._btnUpdate.Name = "_btnUpdate";
            this._btnUpdate.Size = new System.Drawing.Size( 91, 26 );
            this._btnUpdate.TabIndex = 12;
            this._btnUpdate.Text = "&Update";
            this._btnUpdate.UseVisualStyleBackColor = true;
            this._btnUpdate.Click += new System.EventHandler( this._btnUpdate_Click );
            // 
            // _lblAvailableCategories
            // 
            this._lblAvailableCategories.AutoSize = true;
            this._lblAvailableCategories.Location = new System.Drawing.Point( 7, 27 );
            this._lblAvailableCategories.Name = "_lblAvailableCategories";
            this._lblAvailableCategories.Size = new System.Drawing.Size( 103, 13 );
            this._lblAvailableCategories.TabIndex = 0;
            this._lblAvailableCategories.Text = "Available Categories";
            // 
            // _txtTotalPercentage
            // 
            this._txtTotalPercentage.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this._txtTotalPercentage.FlashColor = System.Drawing.Color.Red;
            this._txtTotalPercentage.Location = new System.Drawing.Point( 420, 213 );
            this._txtTotalPercentage.Name = "_txtTotalPercentage";
            this._txtTotalPercentage.ReadOnly = true;
            this._txtTotalPercentage.Size = new System.Drawing.Size( 83, 20 );
            this._txtTotalPercentage.TabIndex = 9;
            this._txtTotalPercentage.TabStop = false;
            this._txtTotalPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtTotalPercentage.Value = null;
            // 
            // _lblTotalPercentage
            // 
            this._lblTotalPercentage.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this._lblTotalPercentage.AutoSize = true;
            this._lblTotalPercentage.Location = new System.Drawing.Point( 375, 216 );
            this._lblTotalPercentage.Name = "_lblTotalPercentage";
            this._lblTotalPercentage.Size = new System.Drawing.Size( 34, 13 );
            this._lblTotalPercentage.TabIndex = 8;
            this._lblTotalPercentage.Text = "Total:";
            this._lblTotalPercentage.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _btnRemoveAll
            // 
            this._btnRemoveAll.Location = new System.Drawing.Point( 213, 179 );
            this._btnRemoveAll.Name = "_btnRemoveAll";
            this._btnRemoveAll.Size = new System.Drawing.Size( 94, 30 );
            this._btnRemoveAll.TabIndex = 5;
            this._btnRemoveAll.Text = "<< Remove All";
            this._btnRemoveAll.UseVisualStyleBackColor = true;
            this._btnRemoveAll.Click += new System.EventHandler( this._btnRemoveAll_Click );
            // 
            // _btnRemove
            // 
            this._btnRemove.Location = new System.Drawing.Point( 213, 144 );
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.Size = new System.Drawing.Size( 94, 30 );
            this._btnRemove.TabIndex = 4;
            this._btnRemove.Text = "< Remove";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += new System.EventHandler( this._btnRemove_Click );
            // 
            // _btnAddAll
            // 
            this._btnAddAll.Location = new System.Drawing.Point( 213, 79 );
            this._btnAddAll.Name = "_btnAddAll";
            this._btnAddAll.Size = new System.Drawing.Size( 94, 30 );
            this._btnAddAll.TabIndex = 3;
            this._btnAddAll.Text = "Add All >>";
            this._btnAddAll.UseVisualStyleBackColor = true;
            this._btnAddAll.Click += new System.EventHandler( this._btnAddAll_Click );
            // 
            // _btnAdd
            // 
            this._btnAdd.Location = new System.Drawing.Point( 213, 44 );
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size( 94, 30 );
            this._btnAdd.TabIndex = 2;
            this._btnAdd.Text = "Add >";
            this._btnAdd.UseVisualStyleBackColor = true;
            this._btnAdd.Click += new System.EventHandler( this._btnAdd_Click );
            // 
            // _lbPostalCategory
            // 
            this._lbPostalCategory.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left ) ) );
            this._lbPostalCategory.DisplayMember = "Value";
            this._lbPostalCategory.FormattingEnabled = true;
            this._lbPostalCategory.Location = new System.Drawing.Point( 6, 44 );
            this._lbPostalCategory.Name = "_lbPostalCategory";
            this._lbPostalCategory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._lbPostalCategory.Size = new System.Drawing.Size( 189, 160 );
            this._lbPostalCategory.TabIndex = 1;
            this._lbPostalCategory.ValueMember = "Key";
            this._lbPostalCategory.Enter += new System.EventHandler( this._lbPostalCategory_Enter );
            // 
            // _dtEffectiveDate
            // 
            this._dtEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEffectiveDate.Location = new System.Drawing.Point( 495, 45 );
            this._dtEffectiveDate.Name = "_dtEffectiveDate";
            this._dtEffectiveDate.Size = new System.Drawing.Size( 121, 20 );
            this._dtEffectiveDate.TabIndex = 6;
            this._dtEffectiveDate.ValueChanged += new System.EventHandler( this._dtEffectiveDate_ValueChanged );
            // 
            // _lblSetupEffectiveDate
            // 
            this._lblSetupEffectiveDate.AutoSize = true;
            this._lblSetupEffectiveDate.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblSetupEffectiveDate.Location = new System.Drawing.Point( 391, 49 );
            this._lblSetupEffectiveDate.Name = "_lblSetupEffectiveDate";
            this._lblSetupEffectiveDate.Size = new System.Drawing.Size( 98, 13 );
            this._lblSetupEffectiveDate.TabIndex = 5;
            this._lblSetupEffectiveDate.Text = "Effective Date *";
            // 
            // _cboPostalMailerType
            // 
            this._cboPostalMailerType.DataSource = this.pstpostalmailertypeBindingSource;
            this._cboPostalMailerType.DisplayMember = "description";
            this._cboPostalMailerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboPostalMailerType.FormattingEnabled = true;
            this._cboPostalMailerType.Location = new System.Drawing.Point( 495, 95 );
            this._cboPostalMailerType.Name = "_cboPostalMailerType";
            this._cboPostalMailerType.Size = new System.Drawing.Size( 152, 21 );
            this._cboPostalMailerType.TabIndex = 10;
            this._cboPostalMailerType.ValueMember = "pst_postalmailertype_id";
            this._cboPostalMailerType.SelectedIndexChanged += new System.EventHandler( this._cboPostalMailerType_SelectedIndexChanged );
            // 
            // pstpostalmailertypeBindingSource
            // 
            this.pstpostalmailertypeBindingSource.DataMember = "pst_postalmailertype";
            this.pstpostalmailertypeBindingSource.DataSource = this.postal;
            // 
            // _lblPostalMailerType
            // 
            this._lblPostalMailerType.AutoSize = true;
            this._lblPostalMailerType.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblPostalMailerType.Location = new System.Drawing.Point( 407, 98 );
            this._lblPostalMailerType.Name = "_lblPostalMailerType";
            this._lblPostalMailerType.Size = new System.Drawing.Size( 82, 13 );
            this._lblPostalMailerType.TabIndex = 9;
            this._lblPostalMailerType.Text = "Mailer Type *";
            // 
            // _cboPostageClass
            // 
            this._cboPostageClass.DataSource = this.pstpostalclassBindingSource;
            this._cboPostageClass.DisplayMember = "description";
            this._cboPostageClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboPostageClass.FormattingEnabled = true;
            this._cboPostageClass.Location = new System.Drawing.Point( 495, 70 );
            this._cboPostageClass.Name = "_cboPostageClass";
            this._cboPostageClass.Size = new System.Drawing.Size( 152, 21 );
            this._cboPostageClass.TabIndex = 8;
            this._cboPostageClass.ValueMember = "pst_postalclass_id";
            this._cboPostageClass.SelectedIndexChanged += new System.EventHandler( this._cboPostageClass_SelectedIndexChanged );
            // 
            // pstpostalclassBindingSource
            // 
            this.pstpostalclassBindingSource.DataMember = "pst_postalclass";
            this.pstpostalclassBindingSource.DataSource = this.postal;
            // 
            // _lblPostalClass
            // 
            this._lblPostalClass.AutoSize = true;
            this._lblPostalClass.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblPostalClass.Location = new System.Drawing.Point( 393, 73 );
            this._lblPostalClass.Name = "_lblPostalClass";
            this._lblPostalClass.Size = new System.Drawing.Size( 96, 13 );
            this._lblPostalClass.TabIndex = 7;
            this._lblPostalClass.Text = "Postage Class *";
            // 
            // _txtComments
            // 
            this._txtComments.Location = new System.Drawing.Point( 113, 70 );
            this._txtComments.MaxLength = 255;
            this._txtComments.Multiline = true;
            this._txtComments.Name = "_txtComments";
            this._txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtComments.Size = new System.Drawing.Size( 233, 86 );
            this._txtComments.TabIndex = 4;
            this._txtComments.TextChanged += new System.EventHandler( this._txtComments_TextChanged );
            // 
            // _lblComments
            // 
            this._lblComments.AutoSize = true;
            this._lblComments.Location = new System.Drawing.Point( 9, 74 );
            this._lblComments.Name = "_lblComments";
            this._lblComments.Size = new System.Drawing.Size( 56, 13 );
            this._lblComments.TabIndex = 3;
            this._lblComments.Text = "Comments";
            // 
            // _txtDescription
            // 
            this._txtDescription.Location = new System.Drawing.Point( 113, 45 );
            this._txtDescription.MaxLength = 35;
            this._txtDescription.Name = "_txtDescription";
            this._txtDescription.Size = new System.Drawing.Size( 233, 20 );
            this._txtDescription.TabIndex = 2;
            this._txtDescription.TextChanged += new System.EventHandler( this._txtDescription_TextChanged );
            // 
            // _lblDescription
            // 
            this._lblDescription.AutoSize = true;
            this._lblDescription.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblDescription.Location = new System.Drawing.Point( 9, 49 );
            this._lblDescription.Name = "_lblDescription";
            this._lblDescription.Size = new System.Drawing.Size( 80, 13 );
            this._lblDescription.TabIndex = 1;
            this._lblDescription.Text = "Description *";
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._btnNew,
            this._btnDelete} );
            this._toolStrip.Location = new System.Drawing.Point( 0, 465 );
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size( 58, 25 );
            this._toolStrip.TabIndex = 7;
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
            this._btnDelete.Text = "Delete Postal Scenario";
            this._btnDelete.Click += new System.EventHandler( this._btnDelete_Click );
            // 
            // vnd_vendorTableAdapter
            // 
            this.vnd_vendorTableAdapter.ClearBeforeFill = true;
            // 
            // pst_postalclassTableAdapter
            // 
            this.pst_postalclassTableAdapter.ClearBeforeFill = true;
            // 
            // pst_postalmailertypeTableAdapter
            // 
            this.pst_postalmailertypeTableAdapter.ClearBeforeFill = true;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // pst_postalcategoryrate_map_id
            // 
            this.pst_postalcategoryrate_map_id.HeaderText = "pst_postalcategoryrate_map_id";
            this.pst_postalcategoryrate_map_id.Name = "pst_postalcategoryrate_map_id";
            this.pst_postalcategoryrate_map_id.ReadOnly = true;
            this.pst_postalcategoryrate_map_id.Visible = false;
            // 
            // Category
            // 
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            // 
            // Percentage
            // 
            this.Percentage.AllowNegative = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.Percentage.DefaultCellStyle = dataGridViewCellStyle1;
            this.Percentage.HeaderText = "Percentage";
            this.Percentage.MaxInputLength = 6;
            this.Percentage.Name = "Percentage";
            // 
            // ucpPostalScenarioSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._toolStrip );
            this.Controls.Add( this._groupScenarioSetup );
            this.Controls.Add( this._cboEffectiveDate );
            this.Controls.Add( this._lblEffectiveDate );
            this.Controls.Add( this._cboPostalScenario );
            this.Controls.Add( this._lblPostalScenario );
            this.Controls.Add( this._cboVendor );
            this.Controls.Add( this._lblVendor );
            this.Name = "ucpPostalScenarioSetup";
            this.Size = new System.Drawing.Size( 663, 504 );
            ( (System.ComponentModel.ISupportInitialize)( this.vendorListforScenariosBindingSource ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.postal ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.vndvendorBindingSource ) ).EndInit();
            this._groupScenarioSetup.ResumeLayout( false );
            this._groupScenarioSetup.PerformLayout();
            this._groupPostalCategoryScenarioSetup.ResumeLayout( false );
            this._groupPostalCategoryScenarioSetup.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._gridPostalCategoryScenarioSetup ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pstpostalmailertypeBindingSource ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.pstpostalclassBindingSource ) ).EndInit();
            this._toolStrip.ResumeLayout( false );
            this._toolStrip.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblVendor;
        private System.Windows.Forms.ComboBox _cboVendor;
        private System.Windows.Forms.Label _lblPostalScenario;
        private System.Windows.Forms.ComboBox _cboPostalScenario;
        private System.Windows.Forms.Label _lblEffectiveDate;
        private System.Windows.Forms.ComboBox _cboEffectiveDate;
        private System.Windows.Forms.GroupBox _groupScenarioSetup;
        private System.Windows.Forms.ComboBox _cboPostalMailerType;
        private System.Windows.Forms.Label _lblPostalMailerType;
        private System.Windows.Forms.ComboBox _cboPostageClass;
        private System.Windows.Forms.Label _lblPostalClass;
        private System.Windows.Forms.TextBox _txtComments;
        private System.Windows.Forms.Label _lblComments;
        private System.Windows.Forms.TextBox _txtDescription;
        private System.Windows.Forms.Label _lblDescription;
        private System.Windows.Forms.GroupBox _groupPostalCategoryScenarioSetup;
        private System.Windows.Forms.DateTimePicker _dtEffectiveDate;
        private System.Windows.Forms.Label _lblSetupEffectiveDate;
        private System.Windows.Forms.ListBox _lbPostalCategory;
        private System.Windows.Forms.Button _btnRemoveAll;
        private System.Windows.Forms.Button _btnRemove;
        private System.Windows.Forms.Button _btnAddAll;
        private System.Windows.Forms.Button _btnAdd;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtTotalPercentage;
        private System.Windows.Forms.Label _lblTotalPercentage;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnNew;
        private System.Windows.Forms.Label _lblCategoriesInScenario;
        private System.Windows.Forms.Label _lblAvailableCategories;
        private CatalogEstimating.Datasets.Postal postal;
        private System.Windows.Forms.BindingSource vndvendorBindingSource;
        private CatalogEstimating.Datasets.PostalTableAdapters.vnd_vendorTableAdapter vnd_vendorTableAdapter;
        private System.Windows.Forms.Button _btnUpdate;
        private System.Windows.Forms.BindingSource vendorListforScenariosBindingSource;
        private System.Windows.Forms.BindingSource pstpostalmailertypeBindingSource;
        private System.Windows.Forms.BindingSource pstpostalclassBindingSource;
        private CatalogEstimating.Datasets.PostalTableAdapters.pst_postalclassTableAdapter pst_postalclassTableAdapter;
        private CatalogEstimating.Datasets.PostalTableAdapters.pst_postalmailertypeTableAdapter pst_postalmailertypeTableAdapter;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.ToolStripButton _btnDelete;
        private System.Windows.Forms.DataGridView _gridPostalCategoryScenarioSetup;
        private System.Windows.Forms.Label _lblInfoText;
        private System.Windows.Forms.DataGridViewTextBoxColumn pst_postalcategoryrate_map_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private CatalogEstimating.CustomControls.DecimalColumn Percentage;
    }
}
