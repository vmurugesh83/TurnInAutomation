namespace CatalogEstimating.UserControls.Admin
{
    partial class ucpAdminInsertionRates
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this._lblPublication = new System.Windows.Forms.Label();
            this._cboPublication = new System.Windows.Forms.ComboBox();
            this._cboLocation = new System.Windows.Forms.ComboBox();
            this._lblLocation = new System.Windows.Forms.Label();
            this._lblEffectiveDate = new System.Windows.Forms.Label();
            this._lblEffectiveDateRates = new System.Windows.Forms.Label();
            this._lblEffectiveDateQuantities = new System.Windows.Forms.Label();
            this._dsPublications = new CatalogEstimating.Datasets.Publications();
            this._btnSearch = new System.Windows.Forms.Button();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnNew = new System.Windows.Forms.ToolStripButton();
            this._btnDelete = new System.Windows.Forms.ToolStripButton();
            this._cboEffectiveDate = new System.Windows.Forms.ComboBox();
            this._cboPubRateEffectiveDate = new System.Windows.Forms.ComboBox();
            this._cboPubQuantityEffectiveDate = new System.Windows.Forms.ComboBox();
            this._tabControl = new System.Windows.Forms.TabControl();
            this._groupRateActivation = new System.Windows.Forms.TabPage();
            this._lblPubLocInfo = new System.Windows.Forms.Label();
            this._chkActive = new System.Windows.Forms.CheckBox();
            this._dtEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this._lblPubEffectiveDate = new System.Windows.Forms.Label();
            this._tabRates = new System.Windows.Forms.TabPage();
            this._lblPubRateInfo = new System.Windows.Forms.Label();
            this._dtPubRateEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this._lblPubRateEffectiveDate = new System.Windows.Forms.Label();
            this._gridInsertsPerDayDiscount = new System.Windows.Forms.DataGridView();
            this.insert = new CatalogEstimating.CustomControls.IntegerColumn();
            this.discount = new CatalogEstimating.CustomControls.DecimalColumn();
            this._lblInsertsPerDayDiscount = new System.Windows.Forms.Label();
            this._groupPublicationRates = new System.Windows.Forms.GroupBox();
            this._gridPubRates = new System.Windows.Forms.DataGridView();
            this.pub_dayofweekratetypes_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ratetypedescription = new CatalogEstimating.CustomControls.DecimalColumn();
            this.pub_dayofweekrates_id0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pub_dayofweekrates_id1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pub_dayofweekrates_id2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pub_dayofweekrates_id3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pub_dayofweekrates_id4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pub_dayofweekrates_id5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pub_dayofweekrates_id6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRatesSunday = new CatalogEstimating.CustomControls.DecimalColumn();
            this.colRatesMonday = new CatalogEstimating.CustomControls.DecimalColumn();
            this.colRatesTuesday = new CatalogEstimating.CustomControls.DecimalColumn();
            this.colRatesWednesday = new CatalogEstimating.CustomControls.DecimalColumn();
            this.colRatesThursday = new CatalogEstimating.CustomControls.DecimalColumn();
            this.colRatesFriday = new CatalogEstimating.CustomControls.DecimalColumn();
            this.colRatesSaturday = new CatalogEstimating.CustomControls.DecimalColumn();
            this._groupChargeForQuantity = new System.Windows.Forms.GroupBox();
            this._txtBilledPctLess = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblBilledPctLess = new System.Windows.Forms.Label();
            this._radioSent = new System.Windows.Forms.RadioButton();
            this._radioBilled = new System.Windows.Forms.RadioButton();
            this._cboBlowInRateType = new System.Windows.Forms.ComboBox();
            this._lblBlowInRateType = new System.Windows.Forms.Label();
            this._chkChargeForBlowIn = new System.Windows.Forms.CheckBox();
            this._cboRateType = new System.Windows.Forms.ComboBox();
            this._lblRateType = new System.Windows.Forms.Label();
            this._tabQuantities = new System.Windows.Forms.TabPage();
            this._txtNewYears = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._txtChristmas = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._txtThanksgiving = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._lblNewYears = new System.Windows.Forms.Label();
            this._lblChristmas = new System.Windows.Forms.Label();
            this._lblThanksgiving = new System.Windows.Forms.Label();
            this._gridPubQuantitiesRates = new System.Windows.Forms.DataGridView();
            this._dtPubRateQuantitiesEffDate = new System.Windows.Forms.DateTimePicker();
            this._lblPubRateQuantitiesEffDate = new System.Windows.Forms.Label();
            this._lblPubQtyInfo = new System.Windows.Forms.Label();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pub_dayofweekquantity_id0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pub_dayofweekquantity_id1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pub_dayofweekquantity_id2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pub_dayofweekquantity_id3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pub_dayofweekquantity_id4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pub_dayofweekquantity_id5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pub_dayofweekquantity_id6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantitiesSunday = new CatalogEstimating.CustomControls.IntegerColumn();
            this.colQuantitiesMonday = new CatalogEstimating.CustomControls.IntegerColumn();
            this.colQuantitiesTuesday = new CatalogEstimating.CustomControls.IntegerColumn();
            this.colQuantitiesWednesday = new CatalogEstimating.CustomControls.IntegerColumn();
            this.colQuantitiesThursday = new CatalogEstimating.CustomControls.IntegerColumn();
            this.colQuantitiesFriday = new CatalogEstimating.CustomControls.IntegerColumn();
            this.colQuantitiesSaturday = new CatalogEstimating.CustomControls.IntegerColumn();
            ((System.ComponentModel.ISupportInitialize)(this._dsPublications)).BeginInit();
            this._toolStrip.SuspendLayout();
            this._tabControl.SuspendLayout();
            this._groupRateActivation.SuspendLayout();
            this._tabRates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridInsertsPerDayDiscount)).BeginInit();
            this._groupPublicationRates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridPubRates)).BeginInit();
            this._groupChargeForQuantity.SuspendLayout();
            this._tabQuantities.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridPubQuantitiesRates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // _lblPublication
            // 
            this._lblPublication.AutoSize = true;
            this._lblPublication.Location = new System.Drawing.Point(12, 13);
            this._lblPublication.Name = "_lblPublication";
            this._lblPublication.Size = new System.Drawing.Size(59, 13);
            this._lblPublication.TabIndex = 0;
            this._lblPublication.Text = "Publication";
            // 
            // _cboPublication
            // 
            this._cboPublication.CausesValidation = false;
            this._cboPublication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboPublication.FormattingEnabled = true;
            this._cboPublication.Location = new System.Drawing.Point(77, 10);
            this._cboPublication.Name = "_cboPublication";
            this._cboPublication.Size = new System.Drawing.Size(278, 21);
            this._cboPublication.TabIndex = 1;
            this._cboPublication.SelectedValueChanged += new System.EventHandler(this._cboPublication_SelectedValueChanged);
            // 
            // _cboLocation
            // 
            this._cboLocation.CausesValidation = false;
            this._cboLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboLocation.Enabled = false;
            this._cboLocation.FormattingEnabled = true;
            this._cboLocation.Location = new System.Drawing.Point(208, 36);
            this._cboLocation.Name = "_cboLocation";
            this._cboLocation.Size = new System.Drawing.Size(147, 21);
            this._cboLocation.TabIndex = 3;
            this._cboLocation.SelectedValueChanged += new System.EventHandler(this._cboLocation_SelectedValueChanged);
            // 
            // _lblLocation
            // 
            this._lblLocation.AutoSize = true;
            this._lblLocation.Location = new System.Drawing.Point(12, 39);
            this._lblLocation.Name = "_lblLocation";
            this._lblLocation.Size = new System.Drawing.Size(48, 13);
            this._lblLocation.TabIndex = 2;
            this._lblLocation.Text = "Location";
            // 
            // _lblEffectiveDate
            // 
            this._lblEffectiveDate.AutoSize = true;
            this._lblEffectiveDate.Location = new System.Drawing.Point(369, 13);
            this._lblEffectiveDate.Name = "_lblEffectiveDate";
            this._lblEffectiveDate.Size = new System.Drawing.Size(168, 13);
            this._lblEffectiveDate.TabIndex = 4;
            this._lblEffectiveDate.Text = "Pub Loc Activation Effective Date";
            // 
            // _lblEffectiveDateRates
            // 
            this._lblEffectiveDateRates.AutoSize = true;
            this._lblEffectiveDateRates.Location = new System.Drawing.Point(412, 39);
            this._lblEffectiveDateRates.Name = "_lblEffectiveDateRates";
            this._lblEffectiveDateRates.Size = new System.Drawing.Size(106, 13);
            this._lblEffectiveDateRates.TabIndex = 6;
            this._lblEffectiveDateRates.Text = "Rates Effective Date";
            // 
            // _lblEffectiveDateQuantities
            // 
            this._lblEffectiveDateQuantities.AutoSize = true;
            this._lblEffectiveDateQuantities.Location = new System.Drawing.Point(412, 65);
            this._lblEffectiveDateQuantities.Name = "_lblEffectiveDateQuantities";
            this._lblEffectiveDateQuantities.Size = new System.Drawing.Size(125, 13);
            this._lblEffectiveDateQuantities.TabIndex = 8;
            this._lblEffectiveDateQuantities.Text = "Quantities Effective Date";
            // 
            // _dsPublications
            // 
            this._dsPublications.DataSetName = "Publications";
            this._dsPublications.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _btnSearch
            // 
            this._btnSearch.Enabled = false;
            this._btnSearch.Location = new System.Drawing.Point(721, 59);
            this._btnSearch.Name = "_btnSearch";
            this._btnSearch.Size = new System.Drawing.Size(75, 23);
            this._btnSearch.TabIndex = 52;
            this._btnSearch.Text = "Search";
            this._btnSearch.UseVisualStyleBackColor = true;
            this._btnSearch.Click += new System.EventHandler(this._btnSearch_Click);
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnNew,
            this._btnDelete});
            this._toolStrip.Location = new System.Drawing.Point(3, 392);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(58, 25);
            this._toolStrip.TabIndex = 53;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _btnNew
            // 
            this._btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnNew.Enabled = false;
            this._btnNew.Image = global::CatalogEstimating.Properties.Resources.NewEstimate;
            this._btnNew.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnNew.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this._btnNew.MergeIndex = 0;
            this._btnNew.Name = "_btnNew";
            this._btnNew.Size = new System.Drawing.Size(23, 22);
            this._btnNew.Text = "New";
            this._btnNew.Click += new System.EventHandler(this._btnNew_Click);
            // 
            // _btnDelete
            // 
            this._btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnDelete.Enabled = false;
            this._btnDelete.Image = global::CatalogEstimating.Properties.Resources.Delete;
            this._btnDelete.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnDelete.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this._btnDelete.MergeIndex = 3;
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Size = new System.Drawing.Size(23, 22);
            this._btnDelete.Text = "Delete";
            this._btnDelete.ToolTipText = "Delete";
            this._btnDelete.Click += new System.EventHandler(this._btnDelete_Click);
            // 
            // _cboEffectiveDate
            // 
            this._cboEffectiveDate.CausesValidation = false;
            this._cboEffectiveDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboEffectiveDate.Enabled = false;
            this._cboEffectiveDate.FormattingEnabled = true;
            this._cboEffectiveDate.Location = new System.Drawing.Point(543, 10);
            this._cboEffectiveDate.Name = "_cboEffectiveDate";
            this._cboEffectiveDate.Size = new System.Drawing.Size(121, 21);
            this._cboEffectiveDate.TabIndex = 54;
            // 
            // _cboPubRateEffectiveDate
            // 
            this._cboPubRateEffectiveDate.CausesValidation = false;
            this._cboPubRateEffectiveDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboPubRateEffectiveDate.Enabled = false;
            this._cboPubRateEffectiveDate.FormattingEnabled = true;
            this._cboPubRateEffectiveDate.Location = new System.Drawing.Point(543, 36);
            this._cboPubRateEffectiveDate.Name = "_cboPubRateEffectiveDate";
            this._cboPubRateEffectiveDate.Size = new System.Drawing.Size(121, 21);
            this._cboPubRateEffectiveDate.TabIndex = 55;
            // 
            // _cboPubQuantityEffectiveDate
            // 
            this._cboPubQuantityEffectiveDate.CausesValidation = false;
            this._cboPubQuantityEffectiveDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboPubQuantityEffectiveDate.Enabled = false;
            this._cboPubQuantityEffectiveDate.FormattingEnabled = true;
            this._cboPubQuantityEffectiveDate.Location = new System.Drawing.Point(543, 62);
            this._cboPubQuantityEffectiveDate.Name = "_cboPubQuantityEffectiveDate";
            this._cboPubQuantityEffectiveDate.Size = new System.Drawing.Size(121, 21);
            this._cboPubQuantityEffectiveDate.TabIndex = 56;
            // 
            // _tabControl
            // 
            this._tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tabControl.Controls.Add(this._groupRateActivation);
            this._tabControl.Controls.Add(this._tabRates);
            this._tabControl.Controls.Add(this._tabQuantities);
            this._tabControl.Location = new System.Drawing.Point(7, 89);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(789, 453);
            this._tabControl.TabIndex = 57;
            this._tabControl.SelectedIndexChanged += new System.EventHandler(this._tabControl_SelectedIndexChanged);
            // 
            // _groupRateActivation
            // 
            this._groupRateActivation.Controls.Add(this._lblPubLocInfo);
            this._groupRateActivation.Controls.Add(this._chkActive);
            this._groupRateActivation.Controls.Add(this._dtEffectiveDate);
            this._groupRateActivation.Controls.Add(this._lblPubEffectiveDate);
            this._groupRateActivation.Location = new System.Drawing.Point(4, 22);
            this._groupRateActivation.Name = "_groupRateActivation";
            this._groupRateActivation.Padding = new System.Windows.Forms.Padding(3);
            this._groupRateActivation.Size = new System.Drawing.Size(781, 427);
            this._groupRateActivation.TabIndex = 2;
            this._groupRateActivation.Text = "Pub Loc Activation";
            this._groupRateActivation.UseVisualStyleBackColor = true;
            // 
            // _lblPubLocInfo
            // 
            this._lblPubLocInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPubLocInfo.ForeColor = System.Drawing.Color.Blue;
            this._lblPubLocInfo.Location = new System.Drawing.Point(8, 5);
            this._lblPubLocInfo.Name = "_lblPubLocInfo";
            this._lblPubLocInfo.Size = new System.Drawing.Size(767, 23);
            this._lblPubLocInfo.TabIndex = 13;
            // 
            // _chkActive
            // 
            this._chkActive.AutoSize = true;
            this._chkActive.Enabled = false;
            this._chkActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._chkActive.Location = new System.Drawing.Point(6, 43);
            this._chkActive.Name = "_chkActive";
            this._chkActive.Size = new System.Drawing.Size(67, 17);
            this._chkActive.TabIndex = 0;
            this._chkActive.Text = "Active*";
            this._chkActive.UseVisualStyleBackColor = true;
            this._chkActive.CheckedChanged += new System.EventHandler(this.Control_Changed);
            // 
            // _dtEffectiveDate
            // 
            this._dtEffectiveDate.Enabled = false;
            this._dtEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEffectiveDate.Location = new System.Drawing.Point(321, 40);
            this._dtEffectiveDate.Name = "_dtEffectiveDate";
            this._dtEffectiveDate.Size = new System.Drawing.Size(147, 20);
            this._dtEffectiveDate.TabIndex = 12;
            // 
            // _lblPubEffectiveDate
            // 
            this._lblPubEffectiveDate.AutoSize = true;
            this._lblPubEffectiveDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPubEffectiveDate.Location = new System.Drawing.Point(98, 44);
            this._lblPubEffectiveDate.Name = "_lblPubEffectiveDate";
            this._lblPubEffectiveDate.Size = new System.Drawing.Size(217, 13);
            this._lblPubEffectiveDate.TabIndex = 11;
            this._lblPubEffectiveDate.Text = "Activate/Deactivate  Effective Date*";
            // 
            // _tabRates
            // 
            this._tabRates.Controls.Add(this._lblPubRateInfo);
            this._tabRates.Controls.Add(this._dtPubRateEffectiveDate);
            this._tabRates.Controls.Add(this._lblPubRateEffectiveDate);
            this._tabRates.Controls.Add(this._gridInsertsPerDayDiscount);
            this._tabRates.Controls.Add(this._lblInsertsPerDayDiscount);
            this._tabRates.Controls.Add(this._groupPublicationRates);
            this._tabRates.Controls.Add(this._groupChargeForQuantity);
            this._tabRates.Controls.Add(this._cboBlowInRateType);
            this._tabRates.Controls.Add(this._lblBlowInRateType);
            this._tabRates.Controls.Add(this._chkChargeForBlowIn);
            this._tabRates.Controls.Add(this._cboRateType);
            this._tabRates.Controls.Add(this._lblRateType);
            this._tabRates.Location = new System.Drawing.Point(4, 22);
            this._tabRates.Name = "_tabRates";
            this._tabRates.Padding = new System.Windows.Forms.Padding(5);
            this._tabRates.Size = new System.Drawing.Size(781, 427);
            this._tabRates.TabIndex = 0;
            this._tabRates.Text = "Rates";
            this._tabRates.UseVisualStyleBackColor = true;
            // 
            // _lblPubRateInfo
            // 
            this._lblPubRateInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPubRateInfo.ForeColor = System.Drawing.Color.Blue;
            this._lblPubRateInfo.Location = new System.Drawing.Point(8, 5);
            this._lblPubRateInfo.Name = "_lblPubRateInfo";
            this._lblPubRateInfo.Size = new System.Drawing.Size(765, 23);
            this._lblPubRateInfo.TabIndex = 0;
            // 
            // _dtPubRateEffectiveDate
            // 
            this._dtPubRateEffectiveDate.Enabled = false;
            this._dtPubRateEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtPubRateEffectiveDate.Location = new System.Drawing.Point(150, 39);
            this._dtPubRateEffectiveDate.Name = "_dtPubRateEffectiveDate";
            this._dtPubRateEffectiveDate.Size = new System.Drawing.Size(147, 20);
            this._dtPubRateEffectiveDate.TabIndex = 1;
            // 
            // _lblPubRateEffectiveDate
            // 
            this._lblPubRateEffectiveDate.AutoSize = true;
            this._lblPubRateEffectiveDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPubRateEffectiveDate.Location = new System.Drawing.Point(13, 43);
            this._lblPubRateEffectiveDate.Name = "_lblPubRateEffectiveDate";
            this._lblPubRateEffectiveDate.Size = new System.Drawing.Size(131, 13);
            this._lblPubRateEffectiveDate.TabIndex = 0;
            this._lblPubRateEffectiveDate.Text = "Rates Effective Date*";
            // 
            // _gridInsertsPerDayDiscount
            // 
            this._gridInsertsPerDayDiscount.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridInsertsPerDayDiscount.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._gridInsertsPerDayDiscount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridInsertsPerDayDiscount.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.insert,
            this.discount});
            this._gridInsertsPerDayDiscount.Location = new System.Drawing.Point(392, 72);
            this._gridInsertsPerDayDiscount.Name = "_gridInsertsPerDayDiscount";
            this._gridInsertsPerDayDiscount.ReadOnly = true;
            this._gridInsertsPerDayDiscount.Size = new System.Drawing.Size(269, 150);
            this._gridInsertsPerDayDiscount.TabIndex = 9;
            this._gridInsertsPerDayDiscount.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this._gridInsertsPerDayDiscount_RowValidating);
            this._gridInsertsPerDayDiscount.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this._gridInsertsPerDayDiscount_UserDeletedRow);
            this._gridInsertsPerDayDiscount.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridInsertsPerDayDiscount_CellValueChanged);
            this._gridInsertsPerDayDiscount.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this._gridInsertsPerDayDiscount_DataError);
            // 
            // insert
            // 
            this.insert.AllowNegative = false;
            this.insert.DataPropertyName = "insert";
            this.insert.HeaderText = "Insert Nbr";
            this.insert.MaxInputLength = 6;
            this.insert.Name = "insert";
            this.insert.ReadOnly = true;
            // 
            // discount
            // 
            this.discount.AllowNegative = false;
            this.discount.DataPropertyName = "discount";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.discount.DefaultCellStyle = dataGridViewCellStyle2;
            this.discount.HeaderText = "Discount %";
            this.discount.MaxInputLength = 6;
            this.discount.Name = "discount";
            this.discount.ReadOnly = true;
            // 
            // _lblInsertsPerDayDiscount
            // 
            this._lblInsertsPerDayDiscount.AutoSize = true;
            this._lblInsertsPerDayDiscount.Location = new System.Drawing.Point(389, 45);
            this._lblInsertsPerDayDiscount.Name = "_lblInsertsPerDayDiscount";
            this._lblInsertsPerDayDiscount.Size = new System.Drawing.Size(124, 13);
            this._lblInsertsPerDayDiscount.TabIndex = 8;
            this._lblInsertsPerDayDiscount.Text = "Inserts Per Day Discount";
            // 
            // _groupPublicationRates
            // 
            this._groupPublicationRates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._groupPublicationRates.Controls.Add(this._gridPubRates);
            this._groupPublicationRates.Location = new System.Drawing.Point(16, 237);
            this._groupPublicationRates.Name = "_groupPublicationRates";
            this._groupPublicationRates.Size = new System.Drawing.Size(757, 182);
            this._groupPublicationRates.TabIndex = 10;
            this._groupPublicationRates.TabStop = false;
            this._groupPublicationRates.Text = "Publication Rates";
            // 
            // _gridPubRates
            // 
            this._gridPubRates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._gridPubRates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridPubRates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this._gridPubRates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridPubRates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pub_dayofweekratetypes_id,
            this.ratetypedescription,
            this.pub_dayofweekrates_id0,
            this.pub_dayofweekrates_id1,
            this.pub_dayofweekrates_id2,
            this.pub_dayofweekrates_id3,
            this.pub_dayofweekrates_id4,
            this.pub_dayofweekrates_id5,
            this.pub_dayofweekrates_id6,
            this.colRatesSunday,
            this.colRatesMonday,
            this.colRatesTuesday,
            this.colRatesWednesday,
            this.colRatesThursday,
            this.colRatesFriday,
            this.colRatesSaturday});
            this._gridPubRates.Location = new System.Drawing.Point(9, 19);
            this._gridPubRates.Name = "_gridPubRates";
            this._gridPubRates.ReadOnly = true;
            this._gridPubRates.Size = new System.Drawing.Size(742, 157);
            this._gridPubRates.TabIndex = 0;
            this._gridPubRates.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this._gridPubRates_UserDeletedRow);
            this._gridPubRates.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridPubRates_CellValueChanged);
            // 
            // pub_dayofweekratetypes_id
            // 
            this.pub_dayofweekratetypes_id.HeaderText = "pub_dayofweekratetypes_id";
            this.pub_dayofweekratetypes_id.Name = "pub_dayofweekratetypes_id";
            this.pub_dayofweekratetypes_id.ReadOnly = true;
            this.pub_dayofweekratetypes_id.Visible = false;
            // 
            // ratetypedescription
            // 
            this.ratetypedescription.AllowNegative = false;
            this.ratetypedescription.HeaderText = "Tab Page Count";
            this.ratetypedescription.MaxInputLength = 6;
            this.ratetypedescription.Name = "ratetypedescription";
            this.ratetypedescription.ReadOnly = true;
            // 
            // pub_dayofweekrates_id0
            // 
            this.pub_dayofweekrates_id0.HeaderText = "pub_dayofweekrates_id0";
            this.pub_dayofweekrates_id0.Name = "pub_dayofweekrates_id0";
            this.pub_dayofweekrates_id0.ReadOnly = true;
            this.pub_dayofweekrates_id0.Visible = false;
            // 
            // pub_dayofweekrates_id1
            // 
            this.pub_dayofweekrates_id1.HeaderText = "pub_dayofweekrates_id1";
            this.pub_dayofweekrates_id1.Name = "pub_dayofweekrates_id1";
            this.pub_dayofweekrates_id1.ReadOnly = true;
            this.pub_dayofweekrates_id1.Visible = false;
            // 
            // pub_dayofweekrates_id2
            // 
            this.pub_dayofweekrates_id2.HeaderText = "pub_dayofweekrates_id2";
            this.pub_dayofweekrates_id2.Name = "pub_dayofweekrates_id2";
            this.pub_dayofweekrates_id2.ReadOnly = true;
            this.pub_dayofweekrates_id2.Visible = false;
            // 
            // pub_dayofweekrates_id3
            // 
            this.pub_dayofweekrates_id3.HeaderText = "pub_dayofweekrates_id3";
            this.pub_dayofweekrates_id3.Name = "pub_dayofweekrates_id3";
            this.pub_dayofweekrates_id3.ReadOnly = true;
            this.pub_dayofweekrates_id3.Visible = false;
            // 
            // pub_dayofweekrates_id4
            // 
            this.pub_dayofweekrates_id4.HeaderText = "pub_dayofweekrates_id4";
            this.pub_dayofweekrates_id4.Name = "pub_dayofweekrates_id4";
            this.pub_dayofweekrates_id4.ReadOnly = true;
            this.pub_dayofweekrates_id4.Visible = false;
            // 
            // pub_dayofweekrates_id5
            // 
            this.pub_dayofweekrates_id5.HeaderText = "pub_dayofweekrates_id5";
            this.pub_dayofweekrates_id5.Name = "pub_dayofweekrates_id5";
            this.pub_dayofweekrates_id5.ReadOnly = true;
            this.pub_dayofweekrates_id5.Visible = false;
            // 
            // pub_dayofweekrates_id6
            // 
            this.pub_dayofweekrates_id6.HeaderText = "pub_dayofweekrates_id6";
            this.pub_dayofweekrates_id6.Name = "pub_dayofweekrates_id6";
            this.pub_dayofweekrates_id6.ReadOnly = true;
            this.pub_dayofweekrates_id6.Visible = false;
            // 
            // colRatesSunday
            // 
            this.colRatesSunday.AllowNegative = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.colRatesSunday.DefaultCellStyle = dataGridViewCellStyle4;
            this.colRatesSunday.HeaderText = "Sunday";
            this.colRatesSunday.MaxInputLength = 9;
            this.colRatesSunday.Name = "colRatesSunday";
            this.colRatesSunday.ReadOnly = true;
            // 
            // colRatesMonday
            // 
            this.colRatesMonday.AllowNegative = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            this.colRatesMonday.DefaultCellStyle = dataGridViewCellStyle5;
            this.colRatesMonday.HeaderText = "Monday";
            this.colRatesMonday.MaxInputLength = 9;
            this.colRatesMonday.Name = "colRatesMonday";
            this.colRatesMonday.ReadOnly = true;
            // 
            // colRatesTuesday
            // 
            this.colRatesTuesday.AllowNegative = false;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            this.colRatesTuesday.DefaultCellStyle = dataGridViewCellStyle6;
            this.colRatesTuesday.HeaderText = "Tuesday";
            this.colRatesTuesday.MaxInputLength = 9;
            this.colRatesTuesday.Name = "colRatesTuesday";
            this.colRatesTuesday.ReadOnly = true;
            // 
            // colRatesWednesday
            // 
            this.colRatesWednesday.AllowNegative = false;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            this.colRatesWednesday.DefaultCellStyle = dataGridViewCellStyle7;
            this.colRatesWednesday.HeaderText = "Wednesday";
            this.colRatesWednesday.MaxInputLength = 9;
            this.colRatesWednesday.Name = "colRatesWednesday";
            this.colRatesWednesday.ReadOnly = true;
            // 
            // colRatesThursday
            // 
            this.colRatesThursday.AllowNegative = false;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N2";
            this.colRatesThursday.DefaultCellStyle = dataGridViewCellStyle8;
            this.colRatesThursday.HeaderText = "Thursday";
            this.colRatesThursday.MaxInputLength = 9;
            this.colRatesThursday.Name = "colRatesThursday";
            this.colRatesThursday.ReadOnly = true;
            // 
            // colRatesFriday
            // 
            this.colRatesFriday.AllowNegative = false;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N2";
            this.colRatesFriday.DefaultCellStyle = dataGridViewCellStyle9;
            this.colRatesFriday.HeaderText = "Friday";
            this.colRatesFriday.MaxInputLength = 9;
            this.colRatesFriday.Name = "colRatesFriday";
            this.colRatesFriday.ReadOnly = true;
            // 
            // colRatesSaturday
            // 
            this.colRatesSaturday.AllowNegative = false;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N2";
            this.colRatesSaturday.DefaultCellStyle = dataGridViewCellStyle10;
            this.colRatesSaturday.HeaderText = "Saturday";
            this.colRatesSaturday.MaxInputLength = 9;
            this.colRatesSaturday.Name = "colRatesSaturday";
            this.colRatesSaturday.ReadOnly = true;
            // 
            // _groupChargeForQuantity
            // 
            this._groupChargeForQuantity.Controls.Add(this._txtBilledPctLess);
            this._groupChargeForQuantity.Controls.Add(this._lblBilledPctLess);
            this._groupChargeForQuantity.Controls.Add(this._radioSent);
            this._groupChargeForQuantity.Controls.Add(this._radioBilled);
            this._groupChargeForQuantity.Enabled = false;
            this._groupChargeForQuantity.Location = new System.Drawing.Point(16, 156);
            this._groupChargeForQuantity.Name = "_groupChargeForQuantity";
            this._groupChargeForQuantity.Size = new System.Drawing.Size(346, 75);
            this._groupChargeForQuantity.TabIndex = 7;
            this._groupChargeForQuantity.TabStop = false;
            this._groupChargeForQuantity.Text = "Charge For Quantity";
            // 
            // _txtBilledPctLess
            // 
            this._txtBilledPctLess.AllowNegative = false;
            this._txtBilledPctLess.Enabled = false;
            this._txtBilledPctLess.FlashColor = System.Drawing.Color.Red;
            this._txtBilledPctLess.Location = new System.Drawing.Point(104, 45);
            this._txtBilledPctLess.MaxLength = 6;
            this._txtBilledPctLess.Name = "_txtBilledPctLess";
            this._txtBilledPctLess.Size = new System.Drawing.Size(74, 20);
            this._txtBilledPctLess.TabIndex = 3;
            this._txtBilledPctLess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtBilledPctLess.Value = null;
            this._txtBilledPctLess.TextChanged += new System.EventHandler(this.Control_Changed);
            // 
            // _lblBilledPctLess
            // 
            this._lblBilledPctLess.AutoSize = true;
            this._lblBilledPctLess.Location = new System.Drawing.Point(6, 48);
            this._lblBilledPctLess.Name = "_lblBilledPctLess";
            this._lblBilledPctLess.Size = new System.Drawing.Size(74, 13);
            this._lblBilledPctLess.TabIndex = 2;
            this._lblBilledPctLess.Text = "Billed (% Less)";
            // 
            // _radioSent
            // 
            this._radioSent.AutoSize = true;
            this._radioSent.Checked = true;
            this._radioSent.Location = new System.Drawing.Point(66, 19);
            this._radioSent.Name = "_radioSent";
            this._radioSent.Size = new System.Drawing.Size(47, 17);
            this._radioSent.TabIndex = 1;
            this._radioSent.TabStop = true;
            this._radioSent.Text = "Sent";
            this._radioSent.UseVisualStyleBackColor = true;
            this._radioSent.CheckedChanged += new System.EventHandler(this._radioSent_CheckedChanged);
            // 
            // _radioBilled
            // 
            this._radioBilled.AutoSize = true;
            this._radioBilled.Location = new System.Drawing.Point(6, 19);
            this._radioBilled.Name = "_radioBilled";
            this._radioBilled.Size = new System.Drawing.Size(50, 17);
            this._radioBilled.TabIndex = 0;
            this._radioBilled.Text = "Billed";
            this._radioBilled.UseVisualStyleBackColor = true;
            this._radioBilled.CheckedChanged += new System.EventHandler(this._radioBilled_CheckedChanged);
            // 
            // _cboBlowInRateType
            // 
            this._cboBlowInRateType.Enabled = false;
            this._cboBlowInRateType.FormattingEnabled = true;
            this._cboBlowInRateType.Items.AddRange(new object[] {
            "None",
            "Charge Full Page Count",
            "Charge 1/2 Page Count"});
            this._cboBlowInRateType.Location = new System.Drawing.Point(150, 126);
            this._cboBlowInRateType.Name = "_cboBlowInRateType";
            this._cboBlowInRateType.Size = new System.Drawing.Size(212, 21);
            this._cboBlowInRateType.TabIndex = 6;
            this._cboBlowInRateType.SelectedValueChanged += new System.EventHandler(this.Control_Changed);
            // 
            // _lblBlowInRateType
            // 
            this._lblBlowInRateType.AutoSize = true;
            this._lblBlowInRateType.Location = new System.Drawing.Point(13, 129);
            this._lblBlowInRateType.Name = "_lblBlowInRateType";
            this._lblBlowInRateType.Size = new System.Drawing.Size(95, 13);
            this._lblBlowInRateType.TabIndex = 5;
            this._lblBlowInRateType.Text = "Blow-In Rate Type";
            // 
            // _chkChargeForBlowIn
            // 
            this._chkChargeForBlowIn.AutoSize = true;
            this._chkChargeForBlowIn.Enabled = false;
            this._chkChargeForBlowIn.Location = new System.Drawing.Point(16, 104);
            this._chkChargeForBlowIn.Name = "_chkChargeForBlowIn";
            this._chkChargeForBlowIn.Size = new System.Drawing.Size(113, 17);
            this._chkChargeForBlowIn.TabIndex = 4;
            this._chkChargeForBlowIn.Text = "Charge for Blow-In";
            this._chkChargeForBlowIn.UseVisualStyleBackColor = true;
            this._chkChargeForBlowIn.CheckedChanged += new System.EventHandler(this._chkChargeForBlowIn_CheckedChanged);
            // 
            // _cboRateType
            // 
            this._cboRateType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this._cboRateType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this._cboRateType.DisplayMember = "pub_ratetype_id";
            this._cboRateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboRateType.Enabled = false;
            this._cboRateType.FormattingEnabled = true;
            this._cboRateType.Location = new System.Drawing.Point(150, 72);
            this._cboRateType.Name = "_cboRateType";
            this._cboRateType.Size = new System.Drawing.Size(212, 21);
            this._cboRateType.TabIndex = 3;
            this._cboRateType.ValueMember = "pub_ratetype_id";
            this._cboRateType.SelectedValueChanged += new System.EventHandler(this._cboRateType_SelectedValueChanged);
            // 
            // _lblRateType
            // 
            this._lblRateType.AutoSize = true;
            this._lblRateType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblRateType.Location = new System.Drawing.Point(13, 75);
            this._lblRateType.Name = "_lblRateType";
            this._lblRateType.Size = new System.Drawing.Size(71, 13);
            this._lblRateType.TabIndex = 2;
            this._lblRateType.Text = "Rate Type*";
            // 
            // _tabQuantities
            // 
            this._tabQuantities.Controls.Add(this._txtNewYears);
            this._tabQuantities.Controls.Add(this._txtChristmas);
            this._tabQuantities.Controls.Add(this._txtThanksgiving);
            this._tabQuantities.Controls.Add(this._lblNewYears);
            this._tabQuantities.Controls.Add(this._lblChristmas);
            this._tabQuantities.Controls.Add(this._lblThanksgiving);
            this._tabQuantities.Controls.Add(this._gridPubQuantitiesRates);
            this._tabQuantities.Controls.Add(this._dtPubRateQuantitiesEffDate);
            this._tabQuantities.Controls.Add(this._lblPubRateQuantitiesEffDate);
            this._tabQuantities.Controls.Add(this._lblPubQtyInfo);
            this._tabQuantities.Location = new System.Drawing.Point(4, 22);
            this._tabQuantities.Name = "_tabQuantities";
            this._tabQuantities.Padding = new System.Windows.Forms.Padding(3);
            this._tabQuantities.Size = new System.Drawing.Size(781, 427);
            this._tabQuantities.TabIndex = 1;
            this._tabQuantities.Text = "Quantities";
            this._tabQuantities.UseVisualStyleBackColor = true;
            // 
            // _txtNewYears
            // 
            this._txtNewYears.AllowNegative = false;
            this._txtNewYears.FlashColor = System.Drawing.Color.Red;
            this._txtNewYears.Location = new System.Drawing.Point(106, 226);
            this._txtNewYears.MaxLength = 9;
            this._txtNewYears.Name = "_txtNewYears";
            this._txtNewYears.ReadOnly = true;
            this._txtNewYears.Size = new System.Drawing.Size(175, 20);
            this._txtNewYears.TabIndex = 14;
            this._txtNewYears.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtNewYears.Value = null;
            this._txtNewYears.TextChanged += new System.EventHandler(this.Control_Changed);
            // 
            // _txtChristmas
            // 
            this._txtChristmas.AllowNegative = false;
            this._txtChristmas.FlashColor = System.Drawing.Color.Red;
            this._txtChristmas.Location = new System.Drawing.Point(106, 200);
            this._txtChristmas.MaxLength = 9;
            this._txtChristmas.Name = "_txtChristmas";
            this._txtChristmas.ReadOnly = true;
            this._txtChristmas.Size = new System.Drawing.Size(175, 20);
            this._txtChristmas.TabIndex = 12;
            this._txtChristmas.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtChristmas.Value = null;
            this._txtChristmas.TextChanged += new System.EventHandler(this.Control_Changed);
            // 
            // _txtThanksgiving
            // 
            this._txtThanksgiving.AllowNegative = false;
            this._txtThanksgiving.FlashColor = System.Drawing.Color.Red;
            this._txtThanksgiving.Location = new System.Drawing.Point(106, 174);
            this._txtThanksgiving.MaxLength = 9;
            this._txtThanksgiving.Name = "_txtThanksgiving";
            this._txtThanksgiving.ReadOnly = true;
            this._txtThanksgiving.Size = new System.Drawing.Size(175, 20);
            this._txtThanksgiving.TabIndex = 10;
            this._txtThanksgiving.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtThanksgiving.Value = null;
            this._txtThanksgiving.TextChanged += new System.EventHandler(this.Control_Changed);
            // 
            // _lblNewYears
            // 
            this._lblNewYears.AutoSize = true;
            this._lblNewYears.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblNewYears.Location = new System.Drawing.Point(10, 229);
            this._lblNewYears.Name = "_lblNewYears";
            this._lblNewYears.Size = new System.Drawing.Size(73, 13);
            this._lblNewYears.TabIndex = 13;
            this._lblNewYears.Text = "New Years*";
            // 
            // _lblChristmas
            // 
            this._lblChristmas.AutoSize = true;
            this._lblChristmas.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblChristmas.Location = new System.Drawing.Point(10, 203);
            this._lblChristmas.Name = "_lblChristmas";
            this._lblChristmas.Size = new System.Drawing.Size(66, 13);
            this._lblChristmas.TabIndex = 11;
            this._lblChristmas.Text = "Christmas*";
            // 
            // _lblThanksgiving
            // 
            this._lblThanksgiving.AutoSize = true;
            this._lblThanksgiving.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblThanksgiving.Location = new System.Drawing.Point(10, 177);
            this._lblThanksgiving.Name = "_lblThanksgiving";
            this._lblThanksgiving.Size = new System.Drawing.Size(88, 13);
            this._lblThanksgiving.TabIndex = 9;
            this._lblThanksgiving.Text = "Thanksgiving*";
            // 
            // _gridPubQuantitiesRates
            // 
            this._gridPubQuantitiesRates.AllowUserToAddRows = false;
            this._gridPubQuantitiesRates.AllowUserToDeleteRows = false;
            this._gridPubQuantitiesRates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._gridPubQuantitiesRates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridPubQuantitiesRates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridPubQuantitiesRates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDescription,
            this.pub_dayofweekquantity_id0,
            this.pub_dayofweekquantity_id1,
            this.pub_dayofweekquantity_id2,
            this.pub_dayofweekquantity_id3,
            this.pub_dayofweekquantity_id4,
            this.pub_dayofweekquantity_id5,
            this.pub_dayofweekquantity_id6,
            this.colQuantitiesSunday,
            this.colQuantitiesMonday,
            this.colQuantitiesTuesday,
            this.colQuantitiesWednesday,
            this.colQuantitiesThursday,
            this.colQuantitiesFriday,
            this.colQuantitiesSaturday});
            this._gridPubQuantitiesRates.Location = new System.Drawing.Point(13, 71);
            this._gridPubQuantitiesRates.Name = "_gridPubQuantitiesRates";
            this._gridPubQuantitiesRates.ReadOnly = true;
            this._gridPubQuantitiesRates.RowHeadersVisible = false;
            this._gridPubQuantitiesRates.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this._gridPubQuantitiesRates.Size = new System.Drawing.Size(745, 89);
            this._gridPubQuantitiesRates.TabIndex = 8;
            this._gridPubQuantitiesRates.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this._gridPubQuantitiesRates_CellValidating);
            this._gridPubQuantitiesRates.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridPubQuantitiesRates_CellValueChanged);
            this._gridPubQuantitiesRates.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this._gridPubQuantitiesRates_RowsRemoved);
            // 
            // _dtPubRateQuantitiesEffDate
            // 
            this._dtPubRateQuantitiesEffDate.Enabled = false;
            this._dtPubRateQuantitiesEffDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtPubRateQuantitiesEffDate.Location = new System.Drawing.Point(173, 39);
            this._dtPubRateQuantitiesEffDate.Name = "_dtPubRateQuantitiesEffDate";
            this._dtPubRateQuantitiesEffDate.Size = new System.Drawing.Size(147, 20);
            this._dtPubRateQuantitiesEffDate.TabIndex = 7;
            // 
            // _lblPubRateQuantitiesEffDate
            // 
            this._lblPubRateQuantitiesEffDate.AutoSize = true;
            this._lblPubRateQuantitiesEffDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPubRateQuantitiesEffDate.Location = new System.Drawing.Point(12, 43);
            this._lblPubRateQuantitiesEffDate.Name = "_lblPubRateQuantitiesEffDate";
            this._lblPubRateQuantitiesEffDate.Size = new System.Drawing.Size(155, 13);
            this._lblPubRateQuantitiesEffDate.TabIndex = 6;
            this._lblPubRateQuantitiesEffDate.Text = "Quantities Effective Date*";
            // 
            // _lblPubQtyInfo
            // 
            this._lblPubQtyInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPubQtyInfo.ForeColor = System.Drawing.Color.Blue;
            this._lblPubQtyInfo.Location = new System.Drawing.Point(8, 5);
            this._lblPubQtyInfo.Name = "_lblPubQtyInfo";
            this._lblPubQtyInfo.Size = new System.Drawing.Size(767, 23);
            this._lblPubQtyInfo.TabIndex = 0;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // colDescription
            // 
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            this.colDescription.DefaultCellStyle = dataGridViewCellStyle11;
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            this.colDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // pub_dayofweekquantity_id0
            // 
            this.pub_dayofweekquantity_id0.HeaderText = "pub_dayofweekquantity_id0";
            this.pub_dayofweekquantity_id0.Name = "pub_dayofweekquantity_id0";
            this.pub_dayofweekquantity_id0.ReadOnly = true;
            this.pub_dayofweekquantity_id0.Visible = false;
            // 
            // pub_dayofweekquantity_id1
            // 
            this.pub_dayofweekquantity_id1.HeaderText = "pub_dayofweekquantity_id1";
            this.pub_dayofweekquantity_id1.Name = "pub_dayofweekquantity_id1";
            this.pub_dayofweekquantity_id1.ReadOnly = true;
            this.pub_dayofweekquantity_id1.Visible = false;
            // 
            // pub_dayofweekquantity_id2
            // 
            this.pub_dayofweekquantity_id2.HeaderText = "pub_dayofweekquantity_id2";
            this.pub_dayofweekquantity_id2.Name = "pub_dayofweekquantity_id2";
            this.pub_dayofweekquantity_id2.ReadOnly = true;
            this.pub_dayofweekquantity_id2.Visible = false;
            // 
            // pub_dayofweekquantity_id3
            // 
            this.pub_dayofweekquantity_id3.HeaderText = "pub_dayofweekquantity_id3";
            this.pub_dayofweekquantity_id3.Name = "pub_dayofweekquantity_id3";
            this.pub_dayofweekquantity_id3.ReadOnly = true;
            this.pub_dayofweekquantity_id3.Visible = false;
            // 
            // pub_dayofweekquantity_id4
            // 
            this.pub_dayofweekquantity_id4.HeaderText = "pub_dayofweekquantity_id4";
            this.pub_dayofweekquantity_id4.Name = "pub_dayofweekquantity_id4";
            this.pub_dayofweekquantity_id4.ReadOnly = true;
            this.pub_dayofweekquantity_id4.Visible = false;
            // 
            // pub_dayofweekquantity_id5
            // 
            this.pub_dayofweekquantity_id5.HeaderText = "pub_dayofweekquantity_id5";
            this.pub_dayofweekquantity_id5.Name = "pub_dayofweekquantity_id5";
            this.pub_dayofweekquantity_id5.ReadOnly = true;
            this.pub_dayofweekquantity_id5.Visible = false;
            // 
            // pub_dayofweekquantity_id6
            // 
            this.pub_dayofweekquantity_id6.HeaderText = "pub_dayofweekquantity_id6";
            this.pub_dayofweekquantity_id6.Name = "pub_dayofweekquantity_id6";
            this.pub_dayofweekquantity_id6.ReadOnly = true;
            this.pub_dayofweekquantity_id6.Visible = false;
            // 
            // colQuantitiesSunday
            // 
            this.colQuantitiesSunday.AllowNegative = false;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Format = "N0";
            dataGridViewCellStyle12.NullValue = null;
            this.colQuantitiesSunday.DefaultCellStyle = dataGridViewCellStyle12;
            this.colQuantitiesSunday.HeaderText = "Sunday";
            this.colQuantitiesSunday.MaxInputLength = 9;
            this.colQuantitiesSunday.Name = "colQuantitiesSunday";
            this.colQuantitiesSunday.ReadOnly = true;
            // 
            // colQuantitiesMonday
            // 
            this.colQuantitiesMonday.AllowNegative = false;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.Format = "N0";
            this.colQuantitiesMonday.DefaultCellStyle = dataGridViewCellStyle13;
            this.colQuantitiesMonday.HeaderText = "Monday";
            this.colQuantitiesMonday.MaxInputLength = 9;
            this.colQuantitiesMonday.Name = "colQuantitiesMonday";
            this.colQuantitiesMonday.ReadOnly = true;
            // 
            // colQuantitiesTuesday
            // 
            this.colQuantitiesTuesday.AllowNegative = false;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.Format = "N0";
            this.colQuantitiesTuesday.DefaultCellStyle = dataGridViewCellStyle14;
            this.colQuantitiesTuesday.HeaderText = "Tuesday";
            this.colQuantitiesTuesday.MaxInputLength = 9;
            this.colQuantitiesTuesday.Name = "colQuantitiesTuesday";
            this.colQuantitiesTuesday.ReadOnly = true;
            // 
            // colQuantitiesWednesday
            // 
            this.colQuantitiesWednesday.AllowNegative = false;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.Format = "N0";
            this.colQuantitiesWednesday.DefaultCellStyle = dataGridViewCellStyle15;
            this.colQuantitiesWednesday.HeaderText = "Wednesday";
            this.colQuantitiesWednesday.MaxInputLength = 9;
            this.colQuantitiesWednesday.Name = "colQuantitiesWednesday";
            this.colQuantitiesWednesday.ReadOnly = true;
            // 
            // colQuantitiesThursday
            // 
            this.colQuantitiesThursday.AllowNegative = false;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Format = "N0";
            this.colQuantitiesThursday.DefaultCellStyle = dataGridViewCellStyle16;
            this.colQuantitiesThursday.HeaderText = "Thursday";
            this.colQuantitiesThursday.MaxInputLength = 9;
            this.colQuantitiesThursday.Name = "colQuantitiesThursday";
            this.colQuantitiesThursday.ReadOnly = true;
            // 
            // colQuantitiesFriday
            // 
            this.colQuantitiesFriday.AllowNegative = false;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle17.Format = "N0";
            this.colQuantitiesFriday.DefaultCellStyle = dataGridViewCellStyle17;
            this.colQuantitiesFriday.HeaderText = "Friday";
            this.colQuantitiesFriday.MaxInputLength = 9;
            this.colQuantitiesFriday.Name = "colQuantitiesFriday";
            this.colQuantitiesFriday.ReadOnly = true;
            // 
            // colQuantitiesSaturday
            // 
            this.colQuantitiesSaturday.AllowNegative = false;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle18.Format = "N0";
            this.colQuantitiesSaturday.DefaultCellStyle = dataGridViewCellStyle18;
            this.colQuantitiesSaturday.HeaderText = "Saturday";
            this.colQuantitiesSaturday.MaxInputLength = 9;
            this.colQuantitiesSaturday.Name = "colQuantitiesSaturday";
            this.colQuantitiesSaturday.ReadOnly = true;
            // 
            // ucpAdminInsertionRates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._toolStrip);
            this.Controls.Add(this._tabControl);
            this.Controls.Add(this._cboPubQuantityEffectiveDate);
            this.Controls.Add(this._cboPubRateEffectiveDate);
            this.Controls.Add(this._cboEffectiveDate);
            this.Controls.Add(this._btnSearch);
            this.Controls.Add(this._lblEffectiveDateQuantities);
            this.Controls.Add(this._lblEffectiveDateRates);
            this.Controls.Add(this._lblEffectiveDate);
            this.Controls.Add(this._cboLocation);
            this.Controls.Add(this._lblLocation);
            this.Controls.Add(this._cboPublication);
            this.Controls.Add(this._lblPublication);
            this.Name = "ucpAdminInsertionRates";
            this.Size = new System.Drawing.Size(814, 565);
            this.Load += new System.EventHandler(this.ucpAdminInsertionRates_Load);
            ((System.ComponentModel.ISupportInitialize)(this._dsPublications)).EndInit();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this._tabControl.ResumeLayout(false);
            this._groupRateActivation.ResumeLayout(false);
            this._groupRateActivation.PerformLayout();
            this._tabRates.ResumeLayout(false);
            this._tabRates.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridInsertsPerDayDiscount)).EndInit();
            this._groupPublicationRates.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridPubRates)).EndInit();
            this._groupChargeForQuantity.ResumeLayout(false);
            this._groupChargeForQuantity.PerformLayout();
            this._tabQuantities.ResumeLayout(false);
            this._tabQuantities.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridPubQuantitiesRates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblPublication;
        private System.Windows.Forms.ComboBox _cboPublication;
        private System.Windows.Forms.ComboBox _cboLocation;
        private System.Windows.Forms.Label _lblLocation;
        private System.Windows.Forms.Label _lblEffectiveDate;
        private System.Windows.Forms.Label _lblEffectiveDateRates;
        private System.Windows.Forms.Label _lblEffectiveDateQuantities;
        private System.Windows.Forms.Button _btnSearch;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnNew;
        private System.Windows.Forms.ComboBox _cboEffectiveDate;
        private System.Windows.Forms.ComboBox _cboPubRateEffectiveDate;
        private System.Windows.Forms.ComboBox _cboPubQuantityEffectiveDate;
        private CatalogEstimating.Datasets.Publications _dsPublications;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _groupRateActivation;
        private System.Windows.Forms.CheckBox _chkActive;
        private System.Windows.Forms.DateTimePicker _dtEffectiveDate;
        private System.Windows.Forms.Label _lblPubEffectiveDate;
        private System.Windows.Forms.TabPage _tabRates;
        private System.Windows.Forms.DateTimePicker _dtPubRateEffectiveDate;
        private System.Windows.Forms.Label _lblPubRateEffectiveDate;
        private System.Windows.Forms.DataGridView _gridInsertsPerDayDiscount;
        private System.Windows.Forms.Label _lblInsertsPerDayDiscount;
        private System.Windows.Forms.GroupBox _groupPublicationRates;
        private System.Windows.Forms.DataGridView _gridPubRates;
        private System.Windows.Forms.GroupBox _groupChargeForQuantity;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtBilledPctLess;
        private System.Windows.Forms.Label _lblBilledPctLess;
        private System.Windows.Forms.RadioButton _radioSent;
        private System.Windows.Forms.RadioButton _radioBilled;
        private System.Windows.Forms.ComboBox _cboBlowInRateType;
        private System.Windows.Forms.Label _lblBlowInRateType;
        private System.Windows.Forms.CheckBox _chkChargeForBlowIn;
        private System.Windows.Forms.ComboBox _cboRateType;
        private System.Windows.Forms.Label _lblRateType;
        private System.Windows.Forms.TabPage _tabQuantities;
        private System.Windows.Forms.ToolStripButton _btnDelete;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.Label _lblPubRateInfo;
        private System.Windows.Forms.Label _lblPubQtyInfo;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtNewYears;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtChristmas;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtThanksgiving;
        private System.Windows.Forms.Label _lblNewYears;
        private System.Windows.Forms.Label _lblChristmas;
        private System.Windows.Forms.Label _lblThanksgiving;
        private System.Windows.Forms.DataGridView _gridPubQuantitiesRates;
        private System.Windows.Forms.DateTimePicker _dtPubRateQuantitiesEffDate;
        private System.Windows.Forms.Label _lblPubRateQuantitiesEffDate;
        private System.Windows.Forms.Label _lblPubLocInfo;
        private CatalogEstimating.CustomControls.IntegerColumn insert;
        private CatalogEstimating.CustomControls.DecimalColumn discount;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekratetypes_id;
        private CatalogEstimating.CustomControls.DecimalColumn ratetypedescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekrates_id0;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekrates_id1;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekrates_id2;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekrates_id3;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekrates_id4;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekrates_id5;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekrates_id6;
        private CatalogEstimating.CustomControls.DecimalColumn colRatesSunday;
        private CatalogEstimating.CustomControls.DecimalColumn colRatesMonday;
        private CatalogEstimating.CustomControls.DecimalColumn colRatesTuesday;
        private CatalogEstimating.CustomControls.DecimalColumn colRatesWednesday;
        private CatalogEstimating.CustomControls.DecimalColumn colRatesThursday;
        private CatalogEstimating.CustomControls.DecimalColumn colRatesFriday;
        private CatalogEstimating.CustomControls.DecimalColumn colRatesSaturday;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekquantity_id0;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekquantity_id1;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekquantity_id2;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekquantity_id3;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekquantity_id4;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekquantity_id5;
        private System.Windows.Forms.DataGridViewTextBoxColumn pub_dayofweekquantity_id6;
        private CatalogEstimating.CustomControls.IntegerColumn colQuantitiesSunday;
        private CatalogEstimating.CustomControls.IntegerColumn colQuantitiesMonday;
        private CatalogEstimating.CustomControls.IntegerColumn colQuantitiesTuesday;
        private CatalogEstimating.CustomControls.IntegerColumn colQuantitiesWednesday;
        private CatalogEstimating.CustomControls.IntegerColumn colQuantitiesThursday;
        private CatalogEstimating.CustomControls.IntegerColumn colQuantitiesFriday;
        private CatalogEstimating.CustomControls.IntegerColumn colQuantitiesSaturday;
    }
}
