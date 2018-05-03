namespace CatalogEstimating.UserControls.Main
{
    partial class ucpEstimateSearch
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
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this._groupFilter = new System.Windows.Forms.GroupBox();
            this._chkCalculateCost = new System.Windows.Forms.CheckBox();
            this._txtEstimateID = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._lblEstimateID = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._btnReset = new System.Windows.Forms.Button();
            this._dtModifiedDateEnd = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._dtModifiedDateStart = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._dtRunDateRangeEnd = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._dtRunDateRangeStart = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._btnSearch = new System.Windows.Forms.Button();
            this._cboFiscalMonth = new System.Windows.Forms.ComboBox();
            this._cboFiscalYear = new System.Windows.Forms.ComboBox();
            this.estEstimatesFiscalYearBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._dsLookup = new CatalogEstimating.Datasets.Lookup();
            this._txtEstimateComments = new System.Windows.Forms.TextBox();
            this._cboSeason = new System.Windows.Forms.ComboBox();
            this.estseasonBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._cboInsertDOW = new System.Windows.Forms.ComboBox();
            this._lblEstimateComments = new System.Windows.Forms.Label();
            this._lblInsertDOW = new System.Windows.Forms.Label();
            this._lblFiscalMonth = new System.Windows.Forms.Label();
            this._lblFiscalYear = new System.Windows.Forms.Label();
            this._lblSeason = new System.Windows.Forms.Label();
            this._lblModifiedDateStart = new System.Windows.Forms.Label();
            this._lblRunDateRangeStart = new System.Windows.Forms.Label();
            this._cboHostEstMedia = new System.Windows.Forms.ComboBox();
            this.estestimatemediatypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._cboCreatedBy = new System.Windows.Forms.ComboBox();
            this.estEstimatesCreatedByBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._cboInsertScenario = new System.Windows.Forms.ComboBox();
            this.estPackagesInsertScenarioBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._cboEstimateStatus = new System.Windows.Forms.ComboBox();
            this.eststatusBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._txtHostMediaQtyEnd = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._lblHostMediaQtyEnd = new System.Windows.Forms.Label();
            this._lblCreatedBy = new System.Windows.Forms.Label();
            this._lblInsertScenario = new System.Windows.Forms.Label();
            this._txtHostMediaQtyStart = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._lblHostMediaQty = new System.Windows.Forms.Label();
            this._txtHostPageCount = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._lblHostPageCount = new System.Windows.Forms.Label();
            this._lblHostEstMedia = new System.Windows.Forms.Label();
            this._txtComponentDesc = new System.Windows.Forms.TextBox();
            this._lblComponentDesc = new System.Windows.Forms.Label();
            this._txtAdNumber = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._lblAdNumber = new System.Windows.Forms.Label();
            this._lblEstimateStatus = new System.Windows.Forms.Label();
            this._txtEstimateDesc = new System.Windows.Forms.TextBox();
            this._lblEstimateDesc = new System.Windows.Forms.Label();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnNew = new System.Windows.Forms.ToolStripButton();
            this._btnOpen = new System.Windows.Forms.ToolStripButton();
            this._btnOpenReadOnly = new System.Windows.Forms.ToolStripButton();
            this._btnNewPolybag = new System.Windows.Forms.ToolStripButton();
            this._btnCopyEstimates = new System.Windows.Forms.ToolStripButton();
            this._btnKill = new System.Windows.Forms.ToolStripButton();
            this._btnUnkill = new System.Windows.Forms.ToolStripButton();
            this._btnUpload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._btnPrint = new System.Windows.Forms.ToolStripButton();
            this._gridEstimates = new System.Windows.Forms.DataGridView();
            this.colPlusMinus = new CatalogEstimating.CustomControls.OptionalButtonColumn();
            this.SortOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eSTEstimateIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.runDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hostMediaDescDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seasonDescDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDescDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uploadKillDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._menuGridPopup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._menuContextNew = new System.Windows.Forms.ToolStripMenuItem();
            this._menuContextOpen = new System.Windows.Forms.ToolStripMenuItem();
            this._menuContextOpenReadOnly = new System.Windows.Forms.ToolStripMenuItem();
            this._menuContextAddPolybagGroup = new System.Windows.Forms.ToolStripMenuItem();
            this._menuContextCopy = new System.Windows.Forms.ToolStripMenuItem();
            this._menuContextKill = new System.Windows.Forms.ToolStripMenuItem();
            this._menuContextUnkill = new System.Windows.Forms.ToolStripMenuItem();
            this._menuContextUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.estEstimateSearchBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._dsSearchResults = new CatalogEstimating.Datasets.EstimateSearch();
            this._lblEstimates = new System.Windows.Forms.Label();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._groupFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.estEstimatesFiscalYearBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dsLookup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.estseasonBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.estestimatemediatypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.estEstimatesCreatedByBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.estPackagesInsertScenarioBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eststatusBindingSource)).BeginInit();
            this._toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridEstimates)).BeginInit();
            this._menuGridPopup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.estEstimateSearchBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dsSearchResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _groupFilter
            // 
            this._groupFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._groupFilter.Controls.Add(this._chkCalculateCost);
            this._groupFilter.Controls.Add(this._txtEstimateID);
            this._groupFilter.Controls.Add(this._lblEstimateID);
            this._groupFilter.Controls.Add(this.label3);
            this._groupFilter.Controls.Add(this.label4);
            this._groupFilter.Controls.Add(this.label1);
            this._groupFilter.Controls.Add(this.label2);
            this._groupFilter.Controls.Add(this._btnReset);
            this._groupFilter.Controls.Add(this._dtModifiedDateEnd);
            this._groupFilter.Controls.Add(this._dtModifiedDateStart);
            this._groupFilter.Controls.Add(this._dtRunDateRangeEnd);
            this._groupFilter.Controls.Add(this._dtRunDateRangeStart);
            this._groupFilter.Controls.Add(this._btnSearch);
            this._groupFilter.Controls.Add(this._cboFiscalMonth);
            this._groupFilter.Controls.Add(this._cboFiscalYear);
            this._groupFilter.Controls.Add(this._txtEstimateComments);
            this._groupFilter.Controls.Add(this._cboSeason);
            this._groupFilter.Controls.Add(this._cboInsertDOW);
            this._groupFilter.Controls.Add(this._lblEstimateComments);
            this._groupFilter.Controls.Add(this._lblInsertDOW);
            this._groupFilter.Controls.Add(this._lblFiscalMonth);
            this._groupFilter.Controls.Add(this._lblFiscalYear);
            this._groupFilter.Controls.Add(this._lblSeason);
            this._groupFilter.Controls.Add(this._lblModifiedDateStart);
            this._groupFilter.Controls.Add(this._lblRunDateRangeStart);
            this._groupFilter.Controls.Add(this._cboHostEstMedia);
            this._groupFilter.Controls.Add(this._cboCreatedBy);
            this._groupFilter.Controls.Add(this._cboInsertScenario);
            this._groupFilter.Controls.Add(this._cboEstimateStatus);
            this._groupFilter.Controls.Add(this._txtHostMediaQtyEnd);
            this._groupFilter.Controls.Add(this._lblHostMediaQtyEnd);
            this._groupFilter.Controls.Add(this._lblCreatedBy);
            this._groupFilter.Controls.Add(this._lblInsertScenario);
            this._groupFilter.Controls.Add(this._txtHostMediaQtyStart);
            this._groupFilter.Controls.Add(this._lblHostMediaQty);
            this._groupFilter.Controls.Add(this._txtHostPageCount);
            this._groupFilter.Controls.Add(this._lblHostPageCount);
            this._groupFilter.Controls.Add(this._lblHostEstMedia);
            this._groupFilter.Controls.Add(this._txtComponentDesc);
            this._groupFilter.Controls.Add(this._lblComponentDesc);
            this._groupFilter.Controls.Add(this._txtAdNumber);
            this._groupFilter.Controls.Add(this._lblAdNumber);
            this._groupFilter.Controls.Add(this._lblEstimateStatus);
            this._groupFilter.Controls.Add(this._txtEstimateDesc);
            this._groupFilter.Controls.Add(this._lblEstimateDesc);
            this._groupFilter.Location = new System.Drawing.Point(12, 12);
            this._groupFilter.Name = "_groupFilter";
            this._groupFilter.Size = new System.Drawing.Size(732, 251);
            this._groupFilter.TabIndex = 0;
            this._groupFilter.TabStop = false;
            this._groupFilter.Text = "Estimate Filter";
            // 
            // _chkCalculateCost
            // 
            this._chkCalculateCost.AutoSize = true;
            this._chkCalculateCost.Location = new System.Drawing.Point(308, 228);
            this._chkCalculateCost.Name = "_chkCalculateCost";
            this._chkCalculateCost.Size = new System.Drawing.Size(121, 17);
            this._chkCalculateCost.TabIndex = 42;
            this._chkCalculateCost.Text = "Calculate Total Cost";
            this._chkCalculateCost.UseVisualStyleBackColor = true;
            // 
            // _txtEstimateID
            // 
            this._txtEstimateID.AllowNegative = false;
            this._txtEstimateID.FlashColor = System.Drawing.Color.Red;
            this._txtEstimateID.Location = new System.Drawing.Point(102, 13);
            this._txtEstimateID.Name = "_txtEstimateID";
            this._txtEstimateID.Size = new System.Drawing.Size(156, 20);
            this._txtEstimateID.TabIndex = 1;
            this._txtEstimateID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtEstimateID.ThousandsSeperator = false;
            this._txtEstimateID.Value = null;
            this._txtEstimateID.Validated += new System.EventHandler(this.TextBox_Validated);
            this._txtEstimateID.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // _lblEstimateID
            // 
            this._lblEstimateID.AutoSize = true;
            this._lblEstimateID.Location = new System.Drawing.Point(6, 17);
            this._lblEstimateID.Name = "_lblEstimateID";
            this._lblEstimateID.Size = new System.Drawing.Size(61, 13);
            this._lblEstimateID.TabIndex = 0;
            this._lblEstimateID.Text = "Estimate ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(592, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "To";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(438, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "From";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(592, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "To";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(438, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "From";
            // 
            // _btnReset
            // 
            this._btnReset.CausesValidation = false;
            this._btnReset.Location = new System.Drawing.Point(648, 224);
            this._btnReset.Name = "_btnReset";
            this._btnReset.Size = new System.Drawing.Size(75, 23);
            this._btnReset.TabIndex = 44;
            this._btnReset.TabStop = false;
            this._btnReset.Text = "Reset";
            this._btnReset.UseVisualStyleBackColor = true;
            this._btnReset.Click += new System.EventHandler(this._btnReset_Click);
            // 
            // _dtModifiedDateEnd
            // 
            this._dtModifiedDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtModifiedDateEnd.Location = new System.Drawing.Point(618, 39);
            this._dtModifiedDateEnd.Name = "_dtModifiedDateEnd";
            this._dtModifiedDateEnd.Size = new System.Drawing.Size(104, 20);
            this._dtModifiedDateEnd.TabIndex = 29;
            this._dtModifiedDateEnd.Value = null;
            this._dtModifiedDateEnd.ValueChanged += new System.EventHandler(this.SearchDate_ValueChanged);
            this._dtModifiedDateEnd.Validated += new System.EventHandler(this.Control_Validated);
            this._dtModifiedDateEnd.Validating += new System.ComponentModel.CancelEventHandler(this._dtModifiedDateEnd_Validating);
            // 
            // _dtModifiedDateStart
            // 
            this._dtModifiedDateStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtModifiedDateStart.Location = new System.Drawing.Point(474, 39);
            this._dtModifiedDateStart.Name = "_dtModifiedDateStart";
            this._dtModifiedDateStart.Size = new System.Drawing.Size(104, 20);
            this._dtModifiedDateStart.TabIndex = 27;
            this._dtModifiedDateStart.Value = null;
            this._dtModifiedDateStart.ValueChanged += new System.EventHandler(this.SearchDate_ValueChanged);
            // 
            // _dtRunDateRangeEnd
            // 
            this._dtRunDateRangeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtRunDateRangeEnd.Location = new System.Drawing.Point(618, 13);
            this._dtRunDateRangeEnd.Name = "_dtRunDateRangeEnd";
            this._dtRunDateRangeEnd.Size = new System.Drawing.Size(104, 20);
            this._dtRunDateRangeEnd.TabIndex = 24;
            this._dtRunDateRangeEnd.Value = null;
            this._dtRunDateRangeEnd.ValueChanged += new System.EventHandler(this.SearchDate_ValueChanged);
            this._dtRunDateRangeEnd.Validated += new System.EventHandler(this.Control_Validated);
            this._dtRunDateRangeEnd.Validating += new System.ComponentModel.CancelEventHandler(this._dtRunDateRangeEnd_Validating);
            // 
            // _dtRunDateRangeStart
            // 
            this._dtRunDateRangeStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtRunDateRangeStart.Location = new System.Drawing.Point(474, 13);
            this._dtRunDateRangeStart.Name = "_dtRunDateRangeStart";
            this._dtRunDateRangeStart.Size = new System.Drawing.Size(104, 20);
            this._dtRunDateRangeStart.TabIndex = 22;
            this._dtRunDateRangeStart.Value = null;
            this._dtRunDateRangeStart.ValueChanged += new System.EventHandler(this.SearchDate_ValueChanged);
            // 
            // _btnSearch
            // 
            this._btnSearch.Enabled = false;
            this._btnSearch.Location = new System.Drawing.Point(537, 224);
            this._btnSearch.Name = "_btnSearch";
            this._btnSearch.Size = new System.Drawing.Size(75, 23);
            this._btnSearch.TabIndex = 43;
            this._btnSearch.Text = "Search";
            this._btnSearch.UseVisualStyleBackColor = true;
            this._btnSearch.Click += new System.EventHandler(this._btnSearch_Click);
            // 
            // _cboFiscalMonth
            // 
            this._cboFiscalMonth.FormattingEnabled = true;
            this._cboFiscalMonth.Location = new System.Drawing.Point(422, 143);
            this._cboFiscalMonth.Name = "_cboFiscalMonth";
            this._cboFiscalMonth.Size = new System.Drawing.Size(156, 21);
            this._cboFiscalMonth.TabIndex = 37;
            this._cboFiscalMonth.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // _cboFiscalYear
            // 
            this._cboFiscalYear.DataSource = this.estEstimatesFiscalYearBindingSource;
            this._cboFiscalYear.DisplayMember = "fiscalyear";
            this._cboFiscalYear.FormattingEnabled = true;
            this._cboFiscalYear.Location = new System.Drawing.Point(422, 117);
            this._cboFiscalYear.Name = "_cboFiscalYear";
            this._cboFiscalYear.Size = new System.Drawing.Size(156, 21);
            this._cboFiscalYear.TabIndex = 35;
            this._cboFiscalYear.ValueMember = "fiscalyear";
            this._cboFiscalYear.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // estEstimatesFiscalYearBindingSource
            // 
            this.estEstimatesFiscalYearBindingSource.DataMember = "EstEstimate_s_FiscalYear";
            this.estEstimatesFiscalYearBindingSource.DataSource = this._dsLookup;
            // 
            // _dsLookup
            // 
            this._dsLookup.DataSetName = "Lookup";
            this._dsLookup.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _txtEstimateComments
            // 
            this._txtEstimateComments.Location = new System.Drawing.Point(422, 195);
            this._txtEstimateComments.MaxLength = 255;
            this._txtEstimateComments.Name = "_txtEstimateComments";
            this._txtEstimateComments.Size = new System.Drawing.Size(301, 20);
            this._txtEstimateComments.TabIndex = 41;
            this._txtEstimateComments.Validated += new System.EventHandler(this.TextBox_Validated);
            this._txtEstimateComments.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // _cboSeason
            // 
            this._cboSeason.DataSource = this.estseasonBindingSource;
            this._cboSeason.DisplayMember = "description";
            this._cboSeason.FormattingEnabled = true;
            this._cboSeason.Location = new System.Drawing.Point(422, 91);
            this._cboSeason.Name = "_cboSeason";
            this._cboSeason.Size = new System.Drawing.Size(156, 21);
            this._cboSeason.TabIndex = 33;
            this._cboSeason.ValueMember = "est_season_id";
            this._cboSeason.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // estseasonBindingSource
            // 
            this.estseasonBindingSource.DataMember = "est_season";
            this.estseasonBindingSource.DataSource = this._dsLookup;
            // 
            // _cboInsertDOW
            // 
            this._cboInsertDOW.FormattingEnabled = true;
            this._cboInsertDOW.Location = new System.Drawing.Point(422, 169);
            this._cboInsertDOW.Name = "_cboInsertDOW";
            this._cboInsertDOW.Size = new System.Drawing.Size(156, 21);
            this._cboInsertDOW.TabIndex = 39;
            this._cboInsertDOW.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // _lblEstimateComments
            // 
            this._lblEstimateComments.AutoSize = true;
            this._lblEstimateComments.Location = new System.Drawing.Point(305, 199);
            this._lblEstimateComments.Name = "_lblEstimateComments";
            this._lblEstimateComments.Size = new System.Drawing.Size(99, 13);
            this._lblEstimateComments.TabIndex = 40;
            this._lblEstimateComments.Text = "Estimate Comments";
            // 
            // _lblInsertDOW
            // 
            this._lblInsertDOW.AutoSize = true;
            this._lblInsertDOW.Location = new System.Drawing.Point(305, 173);
            this._lblInsertDOW.Name = "_lblInsertDOW";
            this._lblInsertDOW.Size = new System.Drawing.Size(63, 13);
            this._lblInsertDOW.TabIndex = 38;
            this._lblInsertDOW.Text = "Insert DOW";
            // 
            // _lblFiscalMonth
            // 
            this._lblFiscalMonth.AutoSize = true;
            this._lblFiscalMonth.Location = new System.Drawing.Point(305, 147);
            this._lblFiscalMonth.Name = "_lblFiscalMonth";
            this._lblFiscalMonth.Size = new System.Drawing.Size(67, 13);
            this._lblFiscalMonth.TabIndex = 36;
            this._lblFiscalMonth.Text = "Fiscal Month";
            // 
            // _lblFiscalYear
            // 
            this._lblFiscalYear.AutoSize = true;
            this._lblFiscalYear.Location = new System.Drawing.Point(305, 121);
            this._lblFiscalYear.Name = "_lblFiscalYear";
            this._lblFiscalYear.Size = new System.Drawing.Size(59, 13);
            this._lblFiscalYear.TabIndex = 34;
            this._lblFiscalYear.Text = "Fiscal Year";
            // 
            // _lblSeason
            // 
            this._lblSeason.AutoSize = true;
            this._lblSeason.Location = new System.Drawing.Point(305, 95);
            this._lblSeason.Name = "_lblSeason";
            this._lblSeason.Size = new System.Drawing.Size(43, 13);
            this._lblSeason.TabIndex = 32;
            this._lblSeason.Text = "Season";
            // 
            // _lblModifiedDateStart
            // 
            this._lblModifiedDateStart.AutoSize = true;
            this._lblModifiedDateStart.Location = new System.Drawing.Point(305, 43);
            this._lblModifiedDateStart.Name = "_lblModifiedDateStart";
            this._lblModifiedDateStart.Size = new System.Drawing.Size(125, 13);
            this._lblModifiedDateStart.TabIndex = 25;
            this._lblModifiedDateStart.Text = "Range of Modified Dates";
            // 
            // _lblRunDateRangeStart
            // 
            this._lblRunDateRangeStart.AutoSize = true;
            this._lblRunDateRangeStart.Location = new System.Drawing.Point(305, 17);
            this._lblRunDateRangeStart.Name = "_lblRunDateRangeStart";
            this._lblRunDateRangeStart.Size = new System.Drawing.Size(105, 13);
            this._lblRunDateRangeStart.TabIndex = 20;
            this._lblRunDateRangeStart.Text = "Range of Run Dates";
            // 
            // _cboHostEstMedia
            // 
            this._cboHostEstMedia.DataSource = this.estestimatemediatypeBindingSource;
            this._cboHostEstMedia.DisplayMember = "description";
            this._cboHostEstMedia.FormattingEnabled = true;
            this._cboHostEstMedia.Location = new System.Drawing.Point(102, 143);
            this._cboHostEstMedia.Name = "_cboHostEstMedia";
            this._cboHostEstMedia.Size = new System.Drawing.Size(156, 21);
            this._cboHostEstMedia.TabIndex = 11;
            this._cboHostEstMedia.ValueMember = "est_estimatemediatype_id";
            this._cboHostEstMedia.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // estestimatemediatypeBindingSource
            // 
            this.estestimatemediatypeBindingSource.DataMember = "est_estimatemediatype";
            this.estestimatemediatypeBindingSource.DataSource = this._dsLookup;
            // 
            // _cboCreatedBy
            // 
            this._cboCreatedBy.DataSource = this.estEstimatesCreatedByBindingSource;
            this._cboCreatedBy.DisplayMember = "createdby";
            this._cboCreatedBy.FormattingEnabled = true;
            this._cboCreatedBy.Location = new System.Drawing.Point(422, 65);
            this._cboCreatedBy.Name = "_cboCreatedBy";
            this._cboCreatedBy.Size = new System.Drawing.Size(156, 21);
            this._cboCreatedBy.TabIndex = 31;
            this._cboCreatedBy.ValueMember = "createdby";
            this._cboCreatedBy.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // estEstimatesCreatedByBindingSource
            // 
            this.estEstimatesCreatedByBindingSource.DataMember = "EstEstimate_s_CreatedBy";
            this.estEstimatesCreatedByBindingSource.DataSource = this._dsLookup;
            // 
            // _cboInsertScenario
            // 
            this._cboInsertScenario.DataSource = this.estPackagesInsertScenarioBindingSource;
            this._cboInsertScenario.DisplayMember = "description";
            this._cboInsertScenario.FormattingEnabled = true;
            this._cboInsertScenario.Location = new System.Drawing.Point(102, 221);
            this._cboInsertScenario.Name = "_cboInsertScenario";
            this._cboInsertScenario.Size = new System.Drawing.Size(156, 21);
            this._cboInsertScenario.TabIndex = 19;
            this._cboInsertScenario.ValueMember = "pub_insertscenario_id";
            this._cboInsertScenario.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // estPackagesInsertScenarioBindingSource
            // 
            this.estPackagesInsertScenarioBindingSource.DataMember = "EstPackage_s_InsertScenario";
            this.estPackagesInsertScenarioBindingSource.DataSource = this._dsLookup;
            // 
            // _cboEstimateStatus
            // 
            this._cboEstimateStatus.DataSource = this.eststatusBindingSource;
            this._cboEstimateStatus.DisplayMember = "description";
            this._cboEstimateStatus.FormattingEnabled = true;
            this._cboEstimateStatus.Location = new System.Drawing.Point(102, 65);
            this._cboEstimateStatus.Name = "_cboEstimateStatus";
            this._cboEstimateStatus.Size = new System.Drawing.Size(156, 21);
            this._cboEstimateStatus.TabIndex = 5;
            this._cboEstimateStatus.ValueMember = "est_status_id";
            this._cboEstimateStatus.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // eststatusBindingSource
            // 
            this.eststatusBindingSource.DataMember = "est_status";
            this.eststatusBindingSource.DataSource = this._dsLookup;
            // 
            // _txtHostMediaQtyEnd
            // 
            this._txtHostMediaQtyEnd.AllowNegative = false;
            this._txtHostMediaQtyEnd.FlashColor = System.Drawing.Color.Red;
            this._txtHostMediaQtyEnd.Location = new System.Drawing.Point(196, 195);
            this._txtHostMediaQtyEnd.Name = "_txtHostMediaQtyEnd";
            this._txtHostMediaQtyEnd.Size = new System.Drawing.Size(62, 20);
            this._txtHostMediaQtyEnd.TabIndex = 17;
            this._txtHostMediaQtyEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtHostMediaQtyEnd.Value = null;
            this._txtHostMediaQtyEnd.Validated += new System.EventHandler(this.Control_Validated);
            this._txtHostMediaQtyEnd.Validating += new System.ComponentModel.CancelEventHandler(this._txtHostMediaQtyEnd_Validating);
            this._txtHostMediaQtyEnd.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // _lblHostMediaQtyEnd
            // 
            this._lblHostMediaQtyEnd.AutoSize = true;
            this._lblHostMediaQtyEnd.Location = new System.Drawing.Point(170, 198);
            this._lblHostMediaQtyEnd.Name = "_lblHostMediaQtyEnd";
            this._lblHostMediaQtyEnd.Size = new System.Drawing.Size(20, 13);
            this._lblHostMediaQtyEnd.TabIndex = 16;
            this._lblHostMediaQtyEnd.Text = "To";
            // 
            // _lblCreatedBy
            // 
            this._lblCreatedBy.AutoSize = true;
            this._lblCreatedBy.Location = new System.Drawing.Point(305, 69);
            this._lblCreatedBy.Name = "_lblCreatedBy";
            this._lblCreatedBy.Size = new System.Drawing.Size(59, 13);
            this._lblCreatedBy.TabIndex = 30;
            this._lblCreatedBy.Text = "Created By";
            // 
            // _lblInsertScenario
            // 
            this._lblInsertScenario.AutoSize = true;
            this._lblInsertScenario.Location = new System.Drawing.Point(6, 225);
            this._lblInsertScenario.Name = "_lblInsertScenario";
            this._lblInsertScenario.Size = new System.Drawing.Size(78, 13);
            this._lblInsertScenario.TabIndex = 18;
            this._lblInsertScenario.Text = "Insert Scenario";
            // 
            // _txtHostMediaQtyStart
            // 
            this._txtHostMediaQtyStart.AllowNegative = false;
            this._txtHostMediaQtyStart.FlashColor = System.Drawing.Color.Red;
            this._txtHostMediaQtyStart.Location = new System.Drawing.Point(102, 195);
            this._txtHostMediaQtyStart.Name = "_txtHostMediaQtyStart";
            this._txtHostMediaQtyStart.Size = new System.Drawing.Size(62, 20);
            this._txtHostMediaQtyStart.TabIndex = 15;
            this._txtHostMediaQtyStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtHostMediaQtyStart.Value = null;
            this._txtHostMediaQtyStart.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // _lblHostMediaQty
            // 
            this._lblHostMediaQty.AutoSize = true;
            this._lblHostMediaQty.Location = new System.Drawing.Point(6, 199);
            this._lblHostMediaQty.Name = "_lblHostMediaQty";
            this._lblHostMediaQty.Size = new System.Drawing.Size(80, 13);
            this._lblHostMediaQty.TabIndex = 14;
            this._lblHostMediaQty.Text = "Host Media Qty";
            // 
            // _txtHostPageCount
            // 
            this._txtHostPageCount.AllowNegative = false;
            this._txtHostPageCount.FlashColor = System.Drawing.Color.Red;
            this._txtHostPageCount.Location = new System.Drawing.Point(102, 169);
            this._txtHostPageCount.Name = "_txtHostPageCount";
            this._txtHostPageCount.Size = new System.Drawing.Size(156, 20);
            this._txtHostPageCount.TabIndex = 13;
            this._txtHostPageCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtHostPageCount.Value = null;
            this._txtHostPageCount.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // _lblHostPageCount
            // 
            this._lblHostPageCount.AutoSize = true;
            this._lblHostPageCount.Location = new System.Drawing.Point(6, 173);
            this._lblHostPageCount.Name = "_lblHostPageCount";
            this._lblHostPageCount.Size = new System.Drawing.Size(88, 13);
            this._lblHostPageCount.TabIndex = 12;
            this._lblHostPageCount.Text = "Host Page Count";
            // 
            // _lblHostEstMedia
            // 
            this._lblHostEstMedia.AutoSize = true;
            this._lblHostEstMedia.Location = new System.Drawing.Point(6, 147);
            this._lblHostEstMedia.Name = "_lblHostEstMedia";
            this._lblHostEstMedia.Size = new System.Drawing.Size(79, 13);
            this._lblHostEstMedia.TabIndex = 10;
            this._lblHostEstMedia.Text = "Estimate Media";
            // 
            // _txtComponentDesc
            // 
            this._txtComponentDesc.Location = new System.Drawing.Point(102, 117);
            this._txtComponentDesc.MaxLength = 35;
            this._txtComponentDesc.Name = "_txtComponentDesc";
            this._txtComponentDesc.Size = new System.Drawing.Size(156, 20);
            this._txtComponentDesc.TabIndex = 9;
            this._txtComponentDesc.Validated += new System.EventHandler(this.TextBox_Validated);
            this._txtComponentDesc.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // _lblComponentDesc
            // 
            this._lblComponentDesc.AutoSize = true;
            this._lblComponentDesc.Location = new System.Drawing.Point(6, 121);
            this._lblComponentDesc.Name = "_lblComponentDesc";
            this._lblComponentDesc.Size = new System.Drawing.Size(89, 13);
            this._lblComponentDesc.TabIndex = 8;
            this._lblComponentDesc.Text = "Component Desc";
            // 
            // _txtAdNumber
            // 
            this._txtAdNumber.AllowNegative = false;
            this._txtAdNumber.FlashColor = System.Drawing.Color.Red;
            this._txtAdNumber.Location = new System.Drawing.Point(102, 91);
            this._txtAdNumber.MaxLength = 5;
            this._txtAdNumber.Name = "_txtAdNumber";
            this._txtAdNumber.Size = new System.Drawing.Size(156, 20);
            this._txtAdNumber.TabIndex = 7;
            this._txtAdNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtAdNumber.ThousandsSeperator = false;
            this._txtAdNumber.Value = null;
            this._txtAdNumber.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // _lblAdNumber
            // 
            this._lblAdNumber.AutoSize = true;
            this._lblAdNumber.Location = new System.Drawing.Point(6, 95);
            this._lblAdNumber.Name = "_lblAdNumber";
            this._lblAdNumber.Size = new System.Drawing.Size(60, 13);
            this._lblAdNumber.TabIndex = 6;
            this._lblAdNumber.Text = "Ad Number";
            // 
            // _lblEstimateStatus
            // 
            this._lblEstimateStatus.AutoSize = true;
            this._lblEstimateStatus.Location = new System.Drawing.Point(6, 69);
            this._lblEstimateStatus.Name = "_lblEstimateStatus";
            this._lblEstimateStatus.Size = new System.Drawing.Size(80, 13);
            this._lblEstimateStatus.TabIndex = 4;
            this._lblEstimateStatus.Text = "Estimate Status";
            // 
            // _txtEstimateDesc
            // 
            this._txtEstimateDesc.Location = new System.Drawing.Point(102, 39);
            this._txtEstimateDesc.MaxLength = 35;
            this._txtEstimateDesc.Name = "_txtEstimateDesc";
            this._txtEstimateDesc.Size = new System.Drawing.Size(156, 20);
            this._txtEstimateDesc.TabIndex = 3;
            this._txtEstimateDesc.Validated += new System.EventHandler(this.TextBox_Validated);
            this._txtEstimateDesc.TextChanged += new System.EventHandler(this.SearchField_TextChanged);
            // 
            // _lblEstimateDesc
            // 
            this._lblEstimateDesc.AutoSize = true;
            this._lblEstimateDesc.Location = new System.Drawing.Point(6, 43);
            this._lblEstimateDesc.Name = "_lblEstimateDesc";
            this._lblEstimateDesc.Size = new System.Drawing.Size(75, 13);
            this._lblEstimateDesc.TabIndex = 2;
            this._lblEstimateDesc.Text = "Estimate Desc";
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnNew,
            this._btnOpen,
            this._btnOpenReadOnly,
            this._btnNewPolybag,
            toolStripSeparator1,
            this._btnCopyEstimates,
            this._btnKill,
            this._btnUnkill,
            this._btnUpload,
            this.toolStripSeparator3,
            this._btnPrint});
            this._toolStrip.Location = new System.Drawing.Point(0, 355);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(231, 25);
            this._toolStrip.TabIndex = 53;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _btnNew
            // 
            this._btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnNew.Image = global::CatalogEstimating.Properties.Resources.NewEstimate;
            this._btnNew.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnNew.Name = "_btnNew";
            this._btnNew.Size = new System.Drawing.Size(23, 22);
            this._btnNew.Text = "New";
            this._btnNew.Click += new System.EventHandler(this._btnNew_Click);
            // 
            // _btnOpen
            // 
            this._btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnOpen.Enabled = false;
            this._btnOpen.Image = global::CatalogEstimating.Properties.Resources.Open;
            this._btnOpen.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnOpen.Name = "_btnOpen";
            this._btnOpen.Size = new System.Drawing.Size(23, 22);
            this._btnOpen.Text = "Open";
            this._btnOpen.Click += new System.EventHandler(this._btnOpen_Click);
            // 
            // _btnOpenReadOnly
            // 
            this._btnOpenReadOnly.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnOpenReadOnly.Enabled = false;
            this._btnOpenReadOnly.Image = global::CatalogEstimating.Properties.Resources.OpenReadOnly;
            this._btnOpenReadOnly.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnOpenReadOnly.Name = "_btnOpenReadOnly";
            this._btnOpenReadOnly.Size = new System.Drawing.Size(23, 22);
            this._btnOpenReadOnly.Text = "Open Read-Only";
            this._btnOpenReadOnly.Click += new System.EventHandler(this._btnOpenReadOnly_Click);
            // 
            // _btnNewPolybag
            // 
            this._btnNewPolybag.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnNewPolybag.Enabled = false;
            this._btnNewPolybag.Image = global::CatalogEstimating.Properties.Resources.NewPolybag;
            this._btnNewPolybag.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnNewPolybag.Name = "_btnNewPolybag";
            this._btnNewPolybag.Size = new System.Drawing.Size(23, 22);
            this._btnNewPolybag.Text = "Add to Polybag Group";
            this._btnNewPolybag.Click += new System.EventHandler(this._menuContextAddPolybagGroup_Click);
            // 
            // _btnCopyEstimates
            // 
            this._btnCopyEstimates.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnCopyEstimates.Enabled = false;
            this._btnCopyEstimates.Image = global::CatalogEstimating.Properties.Resources.CopyDialog;
            this._btnCopyEstimates.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnCopyEstimates.Name = "_btnCopyEstimates";
            this._btnCopyEstimates.Size = new System.Drawing.Size(23, 22);
            this._btnCopyEstimates.Text = "Copy Estimates";
            this._btnCopyEstimates.Click += new System.EventHandler(this._btnCopyEstimates_Click);
            // 
            // _btnKill
            // 
            this._btnKill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnKill.Enabled = false;
            this._btnKill.Image = global::CatalogEstimating.Properties.Resources.Kill;
            this._btnKill.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnKill.Name = "_btnKill";
            this._btnKill.Size = new System.Drawing.Size(23, 22);
            this._btnKill.Text = "Kill";
            this._btnKill.Click += new System.EventHandler(this._btnKill_Click);
            // 
            // _btnUnkill
            // 
            this._btnUnkill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnUnkill.Enabled = false;
            this._btnUnkill.Image = global::CatalogEstimating.Properties.Resources.Unkill;
            this._btnUnkill.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnUnkill.Name = "_btnUnkill";
            this._btnUnkill.Size = new System.Drawing.Size(23, 22);
            this._btnUnkill.Text = "Unkill";
            this._btnUnkill.Click += new System.EventHandler(this._btnUnkill_Click);
            // 
            // _btnUpload
            // 
            this._btnUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnUpload.Enabled = false;
            this._btnUpload.Image = global::CatalogEstimating.Properties.Resources.Upload;
            this._btnUpload.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnUpload.Name = "_btnUpload";
            this._btnUpload.Size = new System.Drawing.Size(23, 22);
            this._btnUpload.Text = "Upload";
            this._btnUpload.Click += new System.EventHandler(this._btnUpload_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // _btnPrint
            // 
            this._btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnPrint.Image = global::CatalogEstimating.Properties.Resources.Print;
            this._btnPrint.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnPrint.Name = "_btnPrint";
            this._btnPrint.Size = new System.Drawing.Size(23, 22);
            this._btnPrint.Text = "Print";
            this._btnPrint.Click += new System.EventHandler(this._btnPrint_Click);
            // 
            // _gridEstimates
            // 
            this._gridEstimates.AllowUserToAddRows = false;
            this._gridEstimates.AllowUserToDeleteRows = false;
            this._gridEstimates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._gridEstimates.AutoGenerateColumns = false;
            this._gridEstimates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridEstimates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._gridEstimates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridEstimates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPlusMinus,
            this.SortOrder,
            this.eSTEstimateIDDataGridViewTextBoxColumn,
            this.ParentId,
            this.runDateDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.hostMediaDescDataGridViewTextBoxColumn,
            this.seasonDescDataGridViewTextBoxColumn,
            this.statusDescDataGridViewTextBoxColumn,
            this.uploadKillDateDataGridViewTextBoxColumn,
            this.totalCostDataGridViewTextBoxColumn,
            this.displayedDataGridViewTextBoxColumn});
            this._gridEstimates.ContextMenuStrip = this._menuGridPopup;
            this._gridEstimates.DataSource = this.estEstimateSearchBindingSource;
            this._gridEstimates.Location = new System.Drawing.Point(12, 292);
            this._gridEstimates.Name = "_gridEstimates";
            this._gridEstimates.ReadOnly = true;
            this._gridEstimates.RowHeadersVisible = false;
            this._gridEstimates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridEstimates.Size = new System.Drawing.Size(732, 236);
            this._gridEstimates.TabIndex = 2;
            this._gridEstimates.TabStop = false;
            this._gridEstimates.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridEstimates_CellDoubleClick);
            this._gridEstimates.SelectionChanged += new System.EventHandler(this._gridEstimates_SelectionChanged);
            this._gridEstimates.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridEstimates_CellContentClick);
            // 
            // colPlusMinus
            // 
            this.colPlusMinus.FillWeight = 15F;
            this.colPlusMinus.HeaderText = "+/-";
            this.colPlusMinus.Name = "colPlusMinus";
            this.colPlusMinus.ReadOnly = true;
            // 
            // SortOrder
            // 
            this.SortOrder.DataPropertyName = "SortOrder";
            this.SortOrder.HeaderText = "SortOrder";
            this.SortOrder.Name = "SortOrder";
            this.SortOrder.ReadOnly = true;
            this.SortOrder.Visible = false;
            // 
            // eSTEstimateIDDataGridViewTextBoxColumn
            // 
            this.eSTEstimateIDDataGridViewTextBoxColumn.DataPropertyName = "EST_Estimate_ID";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.eSTEstimateIDDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.eSTEstimateIDDataGridViewTextBoxColumn.FillWeight = 35F;
            this.eSTEstimateIDDataGridViewTextBoxColumn.HeaderText = "Estimate ID";
            this.eSTEstimateIDDataGridViewTextBoxColumn.Name = "eSTEstimateIDDataGridViewTextBoxColumn";
            this.eSTEstimateIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.eSTEstimateIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ParentId
            // 
            this.ParentId.DataPropertyName = "ParentId";
            this.ParentId.HeaderText = "ParentId";
            this.ParentId.Name = "ParentId";
            this.ParentId.ReadOnly = true;
            this.ParentId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ParentId.Visible = false;
            // 
            // runDateDataGridViewTextBoxColumn
            // 
            this.runDateDataGridViewTextBoxColumn.DataPropertyName = "RunDate";
            this.runDateDataGridViewTextBoxColumn.FillWeight = 40F;
            this.runDateDataGridViewTextBoxColumn.HeaderText = "Run Date";
            this.runDateDataGridViewTextBoxColumn.Name = "runDateDataGridViewTextBoxColumn";
            this.runDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.runDateDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // hostMediaDescDataGridViewTextBoxColumn
            // 
            this.hostMediaDescDataGridViewTextBoxColumn.DataPropertyName = "HostMediaDesc";
            this.hostMediaDescDataGridViewTextBoxColumn.FillWeight = 55F;
            this.hostMediaDescDataGridViewTextBoxColumn.HeaderText = "Estimate Media";
            this.hostMediaDescDataGridViewTextBoxColumn.Name = "hostMediaDescDataGridViewTextBoxColumn";
            this.hostMediaDescDataGridViewTextBoxColumn.ReadOnly = true;
            this.hostMediaDescDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // seasonDescDataGridViewTextBoxColumn
            // 
            this.seasonDescDataGridViewTextBoxColumn.DataPropertyName = "SeasonDesc";
            this.seasonDescDataGridViewTextBoxColumn.FillWeight = 35F;
            this.seasonDescDataGridViewTextBoxColumn.HeaderText = "Season";
            this.seasonDescDataGridViewTextBoxColumn.Name = "seasonDescDataGridViewTextBoxColumn";
            this.seasonDescDataGridViewTextBoxColumn.ReadOnly = true;
            this.seasonDescDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // statusDescDataGridViewTextBoxColumn
            // 
            this.statusDescDataGridViewTextBoxColumn.DataPropertyName = "StatusDesc";
            this.statusDescDataGridViewTextBoxColumn.FillWeight = 35F;
            this.statusDescDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDescDataGridViewTextBoxColumn.Name = "statusDescDataGridViewTextBoxColumn";
            this.statusDescDataGridViewTextBoxColumn.ReadOnly = true;
            this.statusDescDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // uploadKillDateDataGridViewTextBoxColumn
            // 
            this.uploadKillDateDataGridViewTextBoxColumn.DataPropertyName = "UploadKillDate";
            dataGridViewCellStyle3.Format = "g";
            dataGridViewCellStyle3.NullValue = null;
            this.uploadKillDateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.uploadKillDateDataGridViewTextBoxColumn.FillWeight = 50F;
            this.uploadKillDateDataGridViewTextBoxColumn.HeaderText = "Upload/Kill Date";
            this.uploadKillDateDataGridViewTextBoxColumn.Name = "uploadKillDateDataGridViewTextBoxColumn";
            this.uploadKillDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.uploadKillDateDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // totalCostDataGridViewTextBoxColumn
            // 
            this.totalCostDataGridViewTextBoxColumn.DataPropertyName = "TotalCost";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "C2";
            dataGridViewCellStyle4.NullValue = null;
            this.totalCostDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.totalCostDataGridViewTextBoxColumn.FillWeight = 45F;
            this.totalCostDataGridViewTextBoxColumn.HeaderText = "Total Cost";
            this.totalCostDataGridViewTextBoxColumn.Name = "totalCostDataGridViewTextBoxColumn";
            this.totalCostDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalCostDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // displayedDataGridViewTextBoxColumn
            // 
            this.displayedDataGridViewTextBoxColumn.DataPropertyName = "displayed";
            this.displayedDataGridViewTextBoxColumn.HeaderText = "displayed";
            this.displayedDataGridViewTextBoxColumn.Name = "displayedDataGridViewTextBoxColumn";
            this.displayedDataGridViewTextBoxColumn.ReadOnly = true;
            this.displayedDataGridViewTextBoxColumn.Visible = false;
            // 
            // _menuGridPopup
            // 
            this._menuGridPopup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuContextNew,
            this._menuContextOpen,
            this._menuContextOpenReadOnly,
            this._menuContextAddPolybagGroup,
            this._menuContextCopy,
            this._menuContextKill,
            this._menuContextUnkill,
            this._menuContextUpload});
            this._menuGridPopup.Name = "_menuGridPopup";
            this._menuGridPopup.Size = new System.Drawing.Size(191, 180);
            // 
            // _menuContextNew
            // 
            this._menuContextNew.Image = global::CatalogEstimating.Properties.Resources.NewEstimate;
            this._menuContextNew.ImageTransparentColor = System.Drawing.Color.Black;
            this._menuContextNew.Name = "_menuContextNew";
            this._menuContextNew.Size = new System.Drawing.Size(190, 22);
            this._menuContextNew.Text = "&New";
            this._menuContextNew.Click += new System.EventHandler(this._btnNew_Click);
            // 
            // _menuContextOpen
            // 
            this._menuContextOpen.Enabled = false;
            this._menuContextOpen.Image = global::CatalogEstimating.Properties.Resources.Open;
            this._menuContextOpen.ImageTransparentColor = System.Drawing.Color.Black;
            this._menuContextOpen.Name = "_menuContextOpen";
            this._menuContextOpen.Size = new System.Drawing.Size(190, 22);
            this._menuContextOpen.Text = "&Open";
            this._menuContextOpen.Click += new System.EventHandler(this._btnOpen_Click);
            // 
            // _menuContextOpenReadOnly
            // 
            this._menuContextOpenReadOnly.Enabled = false;
            this._menuContextOpenReadOnly.Image = global::CatalogEstimating.Properties.Resources.OpenReadOnly;
            this._menuContextOpenReadOnly.ImageTransparentColor = System.Drawing.Color.Black;
            this._menuContextOpenReadOnly.Name = "_menuContextOpenReadOnly";
            this._menuContextOpenReadOnly.Size = new System.Drawing.Size(190, 22);
            this._menuContextOpenReadOnly.Text = "&Open Read-Only";
            this._menuContextOpenReadOnly.Click += new System.EventHandler(this._btnOpenReadOnly_Click);
            // 
            // _menuContextAddPolybagGroup
            // 
            this._menuContextAddPolybagGroup.Enabled = false;
            this._menuContextAddPolybagGroup.Image = global::CatalogEstimating.Properties.Resources.NewPolybag;
            this._menuContextAddPolybagGroup.ImageTransparentColor = System.Drawing.Color.Black;
            this._menuContextAddPolybagGroup.Name = "_menuContextAddPolybagGroup";
            this._menuContextAddPolybagGroup.Size = new System.Drawing.Size(190, 22);
            this._menuContextAddPolybagGroup.Text = "Add to Polybag Group";
            this._menuContextAddPolybagGroup.Click += new System.EventHandler(this._menuContextAddPolybagGroup_Click);
            // 
            // _menuContextCopy
            // 
            this._menuContextCopy.Enabled = false;
            this._menuContextCopy.Image = global::CatalogEstimating.Properties.Resources.CopyDialog;
            this._menuContextCopy.ImageTransparentColor = System.Drawing.Color.Black;
            this._menuContextCopy.Name = "_menuContextCopy";
            this._menuContextCopy.Size = new System.Drawing.Size(190, 22);
            this._menuContextCopy.Text = "&Copy";
            this._menuContextCopy.Click += new System.EventHandler(this._btnCopyEstimates_Click);
            // 
            // _menuContextKill
            // 
            this._menuContextKill.Enabled = false;
            this._menuContextKill.Image = global::CatalogEstimating.Properties.Resources.Kill;
            this._menuContextKill.ImageTransparentColor = System.Drawing.Color.Black;
            this._menuContextKill.Name = "_menuContextKill";
            this._menuContextKill.Size = new System.Drawing.Size(190, 22);
            this._menuContextKill.Text = "&Kill";
            this._menuContextKill.Click += new System.EventHandler(this._btnKill_Click);
            // 
            // _menuContextUnkill
            // 
            this._menuContextUnkill.Enabled = false;
            this._menuContextUnkill.Image = global::CatalogEstimating.Properties.Resources.Unkill;
            this._menuContextUnkill.ImageTransparentColor = System.Drawing.Color.Black;
            this._menuContextUnkill.Name = "_menuContextUnkill";
            this._menuContextUnkill.Size = new System.Drawing.Size(190, 22);
            this._menuContextUnkill.Text = "&Unkill";
            this._menuContextUnkill.Click += new System.EventHandler(this._btnUnkill_Click);
            // 
            // _menuContextUpload
            // 
            this._menuContextUpload.Enabled = false;
            this._menuContextUpload.Image = global::CatalogEstimating.Properties.Resources.Upload;
            this._menuContextUpload.ImageTransparentColor = System.Drawing.Color.Black;
            this._menuContextUpload.Name = "_menuContextUpload";
            this._menuContextUpload.Size = new System.Drawing.Size(190, 22);
            this._menuContextUpload.Text = "&Upload";
            this._menuContextUpload.Click += new System.EventHandler(this._btnUpload_Click);
            // 
            // estEstimateSearchBindingSource
            // 
            this.estEstimateSearchBindingSource.DataMember = "EstEstimate_Search";
            this.estEstimateSearchBindingSource.DataSource = this._dsSearchResults;
            this.estEstimateSearchBindingSource.Filter = "displayed = 1";
            this.estEstimateSearchBindingSource.Sort = "SortOrder ASC";
            // 
            // _dsSearchResults
            // 
            this._dsSearchResults.DataSetName = "EstimateSearch";
            this._dsSearchResults.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _lblEstimates
            // 
            this._lblEstimates.AutoSize = true;
            this._lblEstimates.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblEstimates.ForeColor = System.Drawing.Color.Blue;
            this._lblEstimates.Location = new System.Drawing.Point(12, 275);
            this._lblEstimates.Name = "_lblEstimates";
            this._lblEstimates.Size = new System.Drawing.Size(72, 13);
            this._lblEstimates.TabIndex = 1;
            this._lblEstimates.Text = "0 Estimates";
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // ucpEstimateSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this._toolStrip);
            this.Controls.Add(this._lblEstimates);
            this.Controls.Add(this._gridEstimates);
            this.Controls.Add(this._groupFilter);
            this.Name = "ucpEstimateSearch";
            this.Size = new System.Drawing.Size(747, 531);
            this.Load += new System.EventHandler(this.ucpEstimateSearch_Load);
            this._groupFilter.ResumeLayout(false);
            this._groupFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.estEstimatesFiscalYearBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dsLookup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.estseasonBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.estestimatemediatypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.estEstimatesCreatedByBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.estPackagesInsertScenarioBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eststatusBindingSource)).EndInit();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridEstimates)).EndInit();
            this._menuGridPopup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.estEstimateSearchBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dsSearchResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox _groupFilter;
        private System.Windows.Forms.TextBox _txtEstimateDesc;
        private System.Windows.Forms.Label _lblEstimateDesc;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtHostMediaQtyEnd;
        private System.Windows.Forms.Label _lblHostMediaQtyEnd;
        private System.Windows.Forms.Label _lblCreatedBy;
        private System.Windows.Forms.Label _lblInsertScenario;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtHostMediaQtyStart;
        private System.Windows.Forms.Label _lblHostMediaQty;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtHostPageCount;
        private System.Windows.Forms.Label _lblHostPageCount;
        private System.Windows.Forms.Label _lblHostEstMedia;
        private System.Windows.Forms.TextBox _txtComponentDesc;
        private System.Windows.Forms.Label _lblComponentDesc;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtAdNumber;
        private System.Windows.Forms.Label _lblAdNumber;
        private System.Windows.Forms.Label _lblEstimateStatus;
        private System.Windows.Forms.ComboBox _cboCreatedBy;
        private System.Windows.Forms.ComboBox _cboInsertScenario;
        private System.Windows.Forms.ComboBox _cboEstimateStatus;
        private System.Windows.Forms.ComboBox _cboHostEstMedia;
        private System.Windows.Forms.ComboBox _cboSeason;
        private System.Windows.Forms.ComboBox _cboInsertDOW;
        private System.Windows.Forms.Label _lblEstimateComments;
        private System.Windows.Forms.Label _lblInsertDOW;
        private System.Windows.Forms.Label _lblFiscalMonth;
        private System.Windows.Forms.Label _lblFiscalYear;
        private System.Windows.Forms.Label _lblSeason;
        private System.Windows.Forms.Label _lblModifiedDateStart;
        private System.Windows.Forms.Label _lblRunDateRangeStart;
        private System.Windows.Forms.TextBox _txtEstimateComments;
        private System.Windows.Forms.ComboBox _cboFiscalMonth;
        private System.Windows.Forms.ComboBox _cboFiscalYear;
        private System.Windows.Forms.Button _btnSearch;
        private System.Windows.Forms.DataGridView _gridEstimates;
        private System.Windows.Forms.Label _lblEstimates;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtModifiedDateEnd;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtModifiedDateStart;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtRunDateRangeEnd;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtRunDateRangeStart;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnNew;
        private System.Windows.Forms.ToolStripButton _btnOpen;
        private System.Windows.Forms.ToolStripButton _btnOpenReadOnly;
        private System.Windows.Forms.ToolStripButton _btnNewPolybag;
        private System.Windows.Forms.ToolStripButton _btnKill;
        private System.Windows.Forms.ToolStripButton _btnUpload;
        private System.Windows.Forms.Button _btnReset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton _btnPrint;
        private System.Windows.Forms.ToolStripButton _btnCopyEstimates;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtEstimateID;
        private System.Windows.Forms.Label _lblEstimateID;
        private System.Windows.Forms.BindingSource estestimatemediatypeBindingSource;
        private CatalogEstimating.Datasets.Lookup _dsLookup;
        private System.Windows.Forms.BindingSource eststatusBindingSource;
        private System.Windows.Forms.BindingSource estseasonBindingSource;
        private System.Windows.Forms.BindingSource estEstimatesCreatedByBindingSource;
        private System.Windows.Forms.BindingSource estEstimatesFiscalYearBindingSource;
        private System.Windows.Forms.BindingSource estEstimateSearchBindingSource;
        private CatalogEstimating.Datasets.EstimateSearch _dsSearchResults;
        private System.Windows.Forms.ToolStripButton _btnUnkill;
        private System.Windows.Forms.ContextMenuStrip _menuGridPopup;
        private System.Windows.Forms.ToolStripMenuItem _menuContextNew;
        private System.Windows.Forms.ToolStripMenuItem _menuContextOpen;
        private System.Windows.Forms.ToolStripMenuItem _menuContextOpenReadOnly;
        private System.Windows.Forms.ToolStripMenuItem _menuContextAddPolybagGroup;
        private System.Windows.Forms.ToolStripMenuItem _menuContextCopy;
        private System.Windows.Forms.ToolStripMenuItem _menuContextKill;
        private System.Windows.Forms.ToolStripMenuItem _menuContextUnkill;
        private System.Windows.Forms.ToolStripMenuItem _menuContextUpload;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.CheckBox _chkCalculateCost;
        private System.Windows.Forms.BindingSource estPackagesInsertScenarioBindingSource;
        private CatalogEstimating.CustomControls.OptionalButtonColumn colPlusMinus;
        private System.Windows.Forms.DataGridViewTextBoxColumn SortOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn eSTEstimateIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn runDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostMediaDescDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seasonDescDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDescDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uploadKillDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalCostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayedDataGridViewTextBoxColumn;
    }
}
