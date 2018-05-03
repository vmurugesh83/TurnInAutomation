namespace CatalogEstimating.UserControls.Main
{
    partial class ucpPolybagSearch
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
            this._gridPolybags = new System.Windows.Forms.DataGridView();
            this.EST_Polybag_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRunDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSeason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._menuGridPopup = new System.Windows.Forms.ContextMenuStrip( this.components );
            this._menuContextNew = new System.Windows.Forms.ToolStripMenuItem();
            this._menuContextOpen = new System.Windows.Forms.ToolStripMenuItem();
            this._menuContextOpenReadOnly = new System.Windows.Forms.ToolStripMenuItem();
            this._menuContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            this._groupFilter = new System.Windows.Forms.GroupBox();
            this._txtAdNumber = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._txtPolybagID = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._lblPolybagID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._btnReset = new System.Windows.Forms.Button();
            this._dtEndRunDate = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._dtStartRunDate = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._btnSearch = new System.Windows.Forms.Button();
            this._cboFiscalMonth = new System.Windows.Forms.ComboBox();
            this._cboFiscalYear = new System.Windows.Forms.ComboBox();
            this._txtPolybagComments = new System.Windows.Forms.TextBox();
            this._cboSeason = new System.Windows.Forms.ComboBox();
            this._lblPolybagComments = new System.Windows.Forms.Label();
            this._lblFiscalMonth = new System.Windows.Forms.Label();
            this._lblFiscalYear = new System.Windows.Forms.Label();
            this._lblSeason = new System.Windows.Forms.Label();
            this._lblRunDateRangeStart = new System.Windows.Forms.Label();
            this._cboCreatedBy = new System.Windows.Forms.ComboBox();
            this._cboEstimateStatus = new System.Windows.Forms.ComboBox();
            this._lblCreatedBy = new System.Windows.Forms.Label();
            this._lblAdNumber = new System.Windows.Forms.Label();
            this._lblStatus = new System.Windows.Forms.Label();
            this._txtPolybagDesc = new System.Windows.Forms.TextBox();
            this._lblPolybagDesc = new System.Windows.Forms.Label();
            this._lblPolybags = new System.Windows.Forms.Label();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnNewPolybag = new System.Windows.Forms.ToolStripButton();
            this._btnOpenPolybag = new System.Windows.Forms.ToolStripButton();
            this._btnOpenPolybagReadOnly = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._btnPrint = new System.Windows.Forms.ToolStripButton();
            this._errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            ( (System.ComponentModel.ISupportInitialize)( this._gridPolybags ) ).BeginInit();
            this._menuGridPopup.SuspendLayout();
            this._groupFilter.SuspendLayout();
            this._toolStrip.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _gridPolybags
            // 
            this._gridPolybags.AllowUserToAddRows = false;
            this._gridPolybags.AllowUserToDeleteRows = false;
            this._gridPolybags.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._gridPolybags.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridPolybags.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._gridPolybags.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridPolybags.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.EST_Polybag_ID,
            this.colRunDate,
            this.colDescription,
            this.colComments,
            this.colSeason,
            this.colYear,
            this.colStatus} );
            this._gridPolybags.ContextMenuStrip = this._menuGridPopup;
            this._gridPolybags.Location = new System.Drawing.Point( 12, 215 );
            this._gridPolybags.Name = "_gridPolybags";
            this._gridPolybags.ReadOnly = true;
            this._gridPolybags.RowHeadersVisible = false;
            this._gridPolybags.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridPolybags.Size = new System.Drawing.Size( 694, 237 );
            this._gridPolybags.TabIndex = 2;
            this._gridPolybags.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler( this._gridPolybags_CellDoubleClick );
            this._gridPolybags.SelectionChanged += new System.EventHandler( this._gridPolybags_SelectionChanged );
            // 
            // EST_Polybag_ID
            // 
            this.EST_Polybag_ID.HeaderText = "EST_Polybag_ID";
            this.EST_Polybag_ID.Name = "EST_Polybag_ID";
            this.EST_Polybag_ID.ReadOnly = true;
            this.EST_Polybag_ID.Visible = false;
            // 
            // colRunDate
            // 
            this.colRunDate.HeaderText = "Run Date";
            this.colRunDate.Name = "colRunDate";
            this.colRunDate.ReadOnly = true;
            // 
            // colDescription
            // 
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            // 
            // colComments
            // 
            this.colComments.HeaderText = "Comments";
            this.colComments.Name = "colComments";
            this.colComments.ReadOnly = true;
            // 
            // colSeason
            // 
            this.colSeason.HeaderText = "Season";
            this.colSeason.Name = "colSeason";
            this.colSeason.ReadOnly = true;
            // 
            // colYear
            // 
            this.colYear.HeaderText = "Year";
            this.colYear.Name = "colYear";
            this.colYear.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // _menuGridPopup
            // 
            this._menuGridPopup.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._menuContextNew,
            this._menuContextOpen,
            this._menuContextOpenReadOnly,
            this._menuContextDelete} );
            this._menuGridPopup.Name = "_menuGridPopup";
            this._menuGridPopup.Size = new System.Drawing.Size( 166, 92 );
            // 
            // _menuContextNew
            // 
            this._menuContextNew.Image = global::CatalogEstimating.Properties.Resources.NewEstimate;
            this._menuContextNew.ImageTransparentColor = System.Drawing.Color.Black;
            this._menuContextNew.Name = "_menuContextNew";
            this._menuContextNew.Size = new System.Drawing.Size( 165, 22 );
            this._menuContextNew.Text = "&New";
            this._menuContextNew.Click += new System.EventHandler( this._btnNewPolybag_Click );
            // 
            // _menuContextOpen
            // 
            this._menuContextOpen.Enabled = false;
            this._menuContextOpen.Image = global::CatalogEstimating.Properties.Resources.Open;
            this._menuContextOpen.ImageTransparentColor = System.Drawing.Color.Black;
            this._menuContextOpen.Name = "_menuContextOpen";
            this._menuContextOpen.Size = new System.Drawing.Size( 165, 22 );
            this._menuContextOpen.Text = "&Open";
            this._menuContextOpen.Click += new System.EventHandler( this._btnOpenPolybag_Click );
            // 
            // _menuContextOpenReadOnly
            // 
            this._menuContextOpenReadOnly.Enabled = false;
            this._menuContextOpenReadOnly.Image = global::CatalogEstimating.Properties.Resources.OpenReadOnly;
            this._menuContextOpenReadOnly.ImageTransparentColor = System.Drawing.Color.Black;
            this._menuContextOpenReadOnly.Name = "_menuContextOpenReadOnly";
            this._menuContextOpenReadOnly.Size = new System.Drawing.Size( 165, 22 );
            this._menuContextOpenReadOnly.Text = "&Open Read-Only";
            this._menuContextOpenReadOnly.Click += new System.EventHandler( this._btnOpenPolybagReadOnly_Click );
            // 
            // _menuContextDelete
            // 
            this._menuContextDelete.Enabled = false;
            this._menuContextDelete.Image = global::CatalogEstimating.Properties.Resources.Delete;
            this._menuContextDelete.ImageTransparentColor = System.Drawing.Color.Black;
            this._menuContextDelete.Name = "_menuContextDelete";
            this._menuContextDelete.Size = new System.Drawing.Size( 165, 22 );
            this._menuContextDelete.Text = "&Delete";
            this._menuContextDelete.Click += new System.EventHandler( this._btnDelete_Click );
            // 
            // _groupFilter
            // 
            this._groupFilter.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._groupFilter.Controls.Add( this._txtAdNumber );
            this._groupFilter.Controls.Add( this._txtPolybagID );
            this._groupFilter.Controls.Add( this._lblPolybagID );
            this._groupFilter.Controls.Add( this.label1 );
            this._groupFilter.Controls.Add( this.label2 );
            this._groupFilter.Controls.Add( this._btnReset );
            this._groupFilter.Controls.Add( this._dtEndRunDate );
            this._groupFilter.Controls.Add( this._dtStartRunDate );
            this._groupFilter.Controls.Add( this._btnSearch );
            this._groupFilter.Controls.Add( this._cboFiscalMonth );
            this._groupFilter.Controls.Add( this._cboFiscalYear );
            this._groupFilter.Controls.Add( this._txtPolybagComments );
            this._groupFilter.Controls.Add( this._cboSeason );
            this._groupFilter.Controls.Add( this._lblPolybagComments );
            this._groupFilter.Controls.Add( this._lblFiscalMonth );
            this._groupFilter.Controls.Add( this._lblFiscalYear );
            this._groupFilter.Controls.Add( this._lblSeason );
            this._groupFilter.Controls.Add( this._lblRunDateRangeStart );
            this._groupFilter.Controls.Add( this._cboCreatedBy );
            this._groupFilter.Controls.Add( this._cboEstimateStatus );
            this._groupFilter.Controls.Add( this._lblCreatedBy );
            this._groupFilter.Controls.Add( this._lblAdNumber );
            this._groupFilter.Controls.Add( this._lblStatus );
            this._groupFilter.Controls.Add( this._txtPolybagDesc );
            this._groupFilter.Controls.Add( this._lblPolybagDesc );
            this._groupFilter.Location = new System.Drawing.Point( 12, 13 );
            this._groupFilter.Name = "_groupFilter";
            this._groupFilter.Size = new System.Drawing.Size( 694, 174 );
            this._groupFilter.TabIndex = 0;
            this._groupFilter.TabStop = false;
            this._groupFilter.Text = "Polybag Filter";
            // 
            // _txtAdNumber
            // 
            this._txtAdNumber.AllowNegative = false;
            this._txtAdNumber.FlashColor = System.Drawing.Color.Red;
            this._txtAdNumber.Location = new System.Drawing.Point( 401, 13 );
            this._txtAdNumber.MaxLength = 18;
            this._txtAdNumber.Name = "_txtAdNumber";
            this._txtAdNumber.Size = new System.Drawing.Size( 156, 20 );
            this._txtAdNumber.TabIndex = 13;
            this._txtAdNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtAdNumber.ThousandsSeperator = false;
            this._txtAdNumber.Value = null;
            this._txtAdNumber.TextChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _txtPolybagID
            // 
            this._txtPolybagID.AllowNegative = false;
            this._txtPolybagID.FlashColor = System.Drawing.Color.Red;
            this._txtPolybagID.Location = new System.Drawing.Point( 109, 13 );
            this._txtPolybagID.MaxLength = 18;
            this._txtPolybagID.Name = "_txtPolybagID";
            this._txtPolybagID.Size = new System.Drawing.Size( 156, 20 );
            this._txtPolybagID.TabIndex = 1;
            this._txtPolybagID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtPolybagID.ThousandsSeperator = false;
            this._txtPolybagID.Value = null;
            this._txtPolybagID.TextChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _lblPolybagID
            // 
            this._lblPolybagID.AutoSize = true;
            this._lblPolybagID.Location = new System.Drawing.Point( 6, 16 );
            this._lblPolybagID.Name = "_lblPolybagID";
            this._lblPolybagID.Size = new System.Drawing.Size( 59, 13 );
            this._lblPolybagID.TabIndex = 0;
            this._lblPolybagID.Text = "Polybag ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 567, 42 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 20, 13 );
            this.label1.TabIndex = 17;
            this.label1.Text = "To";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 419, 42 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 30, 13 );
            this.label2.TabIndex = 15;
            this.label2.Text = "From";
            // 
            // _btnReset
            // 
            this._btnReset.Location = new System.Drawing.Point( 619, 141 );
            this._btnReset.Name = "_btnReset";
            this._btnReset.Size = new System.Drawing.Size( 75, 23 );
            this._btnReset.TabIndex = 24;
            this._btnReset.Text = "Reset";
            this._btnReset.UseVisualStyleBackColor = true;
            this._btnReset.Click += new System.EventHandler( this._btnReset_Click );
            // 
            // _dtEndRunDate
            // 
            this._dtEndRunDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEndRunDate.Location = new System.Drawing.Point( 590, 39 );
            this._dtEndRunDate.Name = "_dtEndRunDate";
            this._dtEndRunDate.Size = new System.Drawing.Size( 104, 20 );
            this._dtEndRunDate.TabIndex = 18;
            this._dtEndRunDate.Value = null;
            this._dtEndRunDate.ValueChanged += new System.EventHandler( this.SearchCriteria_Changed );
            this._dtEndRunDate.Validated += new System.EventHandler( this._dtEndRunDate_Validated );
            this._dtEndRunDate.Validating += new System.ComponentModel.CancelEventHandler( this._dtEndRunDate_Validating );
            // 
            // _dtStartRunDate
            // 
            this._dtStartRunDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtStartRunDate.Location = new System.Drawing.Point( 453, 39 );
            this._dtStartRunDate.Name = "_dtStartRunDate";
            this._dtStartRunDate.Size = new System.Drawing.Size( 104, 20 );
            this._dtStartRunDate.TabIndex = 16;
            this._dtStartRunDate.Value = null;
            this._dtStartRunDate.ValueChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _btnSearch
            // 
            this._btnSearch.Location = new System.Drawing.Point( 512, 141 );
            this._btnSearch.Name = "_btnSearch";
            this._btnSearch.Size = new System.Drawing.Size( 75, 23 );
            this._btnSearch.TabIndex = 23;
            this._btnSearch.Text = "Search";
            this._btnSearch.UseVisualStyleBackColor = true;
            this._btnSearch.Click += new System.EventHandler( this._btnSearch_Click );
            // 
            // _cboFiscalMonth
            // 
            this._cboFiscalMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboFiscalMonth.FormattingEnabled = true;
            this._cboFiscalMonth.Location = new System.Drawing.Point( 109, 143 );
            this._cboFiscalMonth.Name = "_cboFiscalMonth";
            this._cboFiscalMonth.Size = new System.Drawing.Size( 156, 21 );
            this._cboFiscalMonth.TabIndex = 11;
            this._cboFiscalMonth.SelectedIndexChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _cboFiscalYear
            // 
            this._cboFiscalYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboFiscalYear.FormattingEnabled = true;
            this._cboFiscalYear.Location = new System.Drawing.Point( 109, 117 );
            this._cboFiscalYear.Name = "_cboFiscalYear";
            this._cboFiscalYear.Size = new System.Drawing.Size( 156, 21 );
            this._cboFiscalYear.TabIndex = 9;
            this._cboFiscalYear.SelectedIndexChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _txtPolybagComments
            // 
            this._txtPolybagComments.Location = new System.Drawing.Point( 109, 65 );
            this._txtPolybagComments.MaxLength = 255;
            this._txtPolybagComments.Name = "_txtPolybagComments";
            this._txtPolybagComments.Size = new System.Drawing.Size( 156, 20 );
            this._txtPolybagComments.TabIndex = 5;
            this._txtPolybagComments.Validated += new System.EventHandler( this._txtPolybagComments_Validated );
            this._txtPolybagComments.TextChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _cboSeason
            // 
            this._cboSeason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboSeason.FormattingEnabled = true;
            this._cboSeason.Location = new System.Drawing.Point( 109, 91 );
            this._cboSeason.Name = "_cboSeason";
            this._cboSeason.Size = new System.Drawing.Size( 156, 21 );
            this._cboSeason.TabIndex = 7;
            this._cboSeason.SelectedIndexChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _lblPolybagComments
            // 
            this._lblPolybagComments.AutoSize = true;
            this._lblPolybagComments.Location = new System.Drawing.Point( 6, 68 );
            this._lblPolybagComments.Name = "_lblPolybagComments";
            this._lblPolybagComments.Size = new System.Drawing.Size( 97, 13 );
            this._lblPolybagComments.TabIndex = 4;
            this._lblPolybagComments.Text = "Polybag Comments";
            // 
            // _lblFiscalMonth
            // 
            this._lblFiscalMonth.AutoSize = true;
            this._lblFiscalMonth.Location = new System.Drawing.Point( 6, 146 );
            this._lblFiscalMonth.Name = "_lblFiscalMonth";
            this._lblFiscalMonth.Size = new System.Drawing.Size( 67, 13 );
            this._lblFiscalMonth.TabIndex = 10;
            this._lblFiscalMonth.Text = "Fiscal Month";
            // 
            // _lblFiscalYear
            // 
            this._lblFiscalYear.AutoSize = true;
            this._lblFiscalYear.Location = new System.Drawing.Point( 6, 120 );
            this._lblFiscalYear.Name = "_lblFiscalYear";
            this._lblFiscalYear.Size = new System.Drawing.Size( 59, 13 );
            this._lblFiscalYear.TabIndex = 8;
            this._lblFiscalYear.Text = "Fiscal Year";
            // 
            // _lblSeason
            // 
            this._lblSeason.AutoSize = true;
            this._lblSeason.Location = new System.Drawing.Point( 6, 94 );
            this._lblSeason.Name = "_lblSeason";
            this._lblSeason.Size = new System.Drawing.Size( 43, 13 );
            this._lblSeason.TabIndex = 6;
            this._lblSeason.Text = "Season";
            // 
            // _lblRunDateRangeStart
            // 
            this._lblRunDateRangeStart.AutoSize = true;
            this._lblRunDateRangeStart.Location = new System.Drawing.Point( 305, 42 );
            this._lblRunDateRangeStart.Name = "_lblRunDateRangeStart";
            this._lblRunDateRangeStart.Size = new System.Drawing.Size( 88, 13 );
            this._lblRunDateRangeStart.TabIndex = 14;
            this._lblRunDateRangeStart.Text = "Run Date Range";
            // 
            // _cboCreatedBy
            // 
            this._cboCreatedBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboCreatedBy.FormattingEnabled = true;
            this._cboCreatedBy.Location = new System.Drawing.Point( 401, 65 );
            this._cboCreatedBy.Name = "_cboCreatedBy";
            this._cboCreatedBy.Size = new System.Drawing.Size( 156, 21 );
            this._cboCreatedBy.TabIndex = 20;
            this._cboCreatedBy.SelectedIndexChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _cboEstimateStatus
            // 
            this._cboEstimateStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboEstimateStatus.FormattingEnabled = true;
            this._cboEstimateStatus.Location = new System.Drawing.Point( 401, 91 );
            this._cboEstimateStatus.Name = "_cboEstimateStatus";
            this._cboEstimateStatus.Size = new System.Drawing.Size( 156, 21 );
            this._cboEstimateStatus.TabIndex = 22;
            this._cboEstimateStatus.SelectedIndexChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _lblCreatedBy
            // 
            this._lblCreatedBy.AutoSize = true;
            this._lblCreatedBy.Location = new System.Drawing.Point( 305, 68 );
            this._lblCreatedBy.Name = "_lblCreatedBy";
            this._lblCreatedBy.Size = new System.Drawing.Size( 59, 13 );
            this._lblCreatedBy.TabIndex = 19;
            this._lblCreatedBy.Text = "Created By";
            // 
            // _lblAdNumber
            // 
            this._lblAdNumber.AutoSize = true;
            this._lblAdNumber.Location = new System.Drawing.Point( 305, 16 );
            this._lblAdNumber.Name = "_lblAdNumber";
            this._lblAdNumber.Size = new System.Drawing.Size( 60, 13 );
            this._lblAdNumber.TabIndex = 12;
            this._lblAdNumber.Text = "Ad Number";
            // 
            // _lblStatus
            // 
            this._lblStatus.AutoSize = true;
            this._lblStatus.Location = new System.Drawing.Point( 305, 94 );
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new System.Drawing.Size( 37, 13 );
            this._lblStatus.TabIndex = 21;
            this._lblStatus.Text = "Status";
            // 
            // _txtPolybagDesc
            // 
            this._txtPolybagDesc.Location = new System.Drawing.Point( 109, 39 );
            this._txtPolybagDesc.MaxLength = 35;
            this._txtPolybagDesc.Name = "_txtPolybagDesc";
            this._txtPolybagDesc.Size = new System.Drawing.Size( 156, 20 );
            this._txtPolybagDesc.TabIndex = 3;
            this._txtPolybagDesc.Validated += new System.EventHandler( this._txtPolybagDesc_Validated );
            this._txtPolybagDesc.TextChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _lblPolybagDesc
            // 
            this._lblPolybagDesc.AutoSize = true;
            this._lblPolybagDesc.Location = new System.Drawing.Point( 6, 42 );
            this._lblPolybagDesc.Name = "_lblPolybagDesc";
            this._lblPolybagDesc.Size = new System.Drawing.Size( 73, 13 );
            this._lblPolybagDesc.TabIndex = 2;
            this._lblPolybagDesc.Text = "Polybag Desc";
            // 
            // _lblPolybags
            // 
            this._lblPolybags.AutoSize = true;
            this._lblPolybags.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblPolybags.ForeColor = System.Drawing.Color.Blue;
            this._lblPolybags.Location = new System.Drawing.Point( 12, 198 );
            this._lblPolybags.Name = "_lblPolybags";
            this._lblPolybags.Size = new System.Drawing.Size( 69, 13 );
            this._lblPolybags.TabIndex = 1;
            this._lblPolybags.Text = "0 Polybags";
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._btnNewPolybag,
            this._btnOpenPolybag,
            this._btnOpenPolybagReadOnly,
            this.toolStripSeparator1,
            this._btnDelete,
            this.toolStripSeparator3,
            this._btnPrint} );
            this._toolStrip.Location = new System.Drawing.Point( 0, 252 );
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size( 139, 25 );
            this._toolStrip.TabIndex = 3;
            // 
            // _btnNewPolybag
            // 
            this._btnNewPolybag.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnNewPolybag.Enabled = false;
            this._btnNewPolybag.Image = global::CatalogEstimating.Properties.Resources.NewPolybag;
            this._btnNewPolybag.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnNewPolybag.Name = "_btnNewPolybag";
            this._btnNewPolybag.Size = new System.Drawing.Size( 23, 22 );
            this._btnNewPolybag.Text = "New Polybag";
            this._btnNewPolybag.Click += new System.EventHandler( this._btnNewPolybag_Click );
            // 
            // _btnOpenPolybag
            // 
            this._btnOpenPolybag.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnOpenPolybag.Enabled = false;
            this._btnOpenPolybag.Image = global::CatalogEstimating.Properties.Resources.Open;
            this._btnOpenPolybag.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnOpenPolybag.Name = "_btnOpenPolybag";
            this._btnOpenPolybag.Size = new System.Drawing.Size( 23, 22 );
            this._btnOpenPolybag.Text = "Open Polybag";
            this._btnOpenPolybag.Click += new System.EventHandler( this._btnOpenPolybag_Click );
            // 
            // _btnOpenPolybagReadOnly
            // 
            this._btnOpenPolybagReadOnly.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnOpenPolybagReadOnly.Enabled = false;
            this._btnOpenPolybagReadOnly.Image = global::CatalogEstimating.Properties.Resources.OpenReadOnly;
            this._btnOpenPolybagReadOnly.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnOpenPolybagReadOnly.Name = "_btnOpenPolybagReadOnly";
            this._btnOpenPolybagReadOnly.Size = new System.Drawing.Size( 23, 22 );
            this._btnOpenPolybagReadOnly.Text = "Open Polybag Read-Only";
            this._btnOpenPolybagReadOnly.Click += new System.EventHandler( this._btnOpenPolybagReadOnly_Click );
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 6, 25 );
            // 
            // _btnDelete
            // 
            this._btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnDelete.Enabled = false;
            this._btnDelete.Image = global::CatalogEstimating.Properties.Resources.Delete;
            this._btnDelete.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Size = new System.Drawing.Size( 23, 22 );
            this._btnDelete.Text = "Delete";
            this._btnDelete.Click += new System.EventHandler( this._btnDelete_Click );
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size( 6, 25 );
            // 
            // _btnPrint
            // 
            this._btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnPrint.Image = global::CatalogEstimating.Properties.Resources.Print;
            this._btnPrint.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnPrint.Name = "_btnPrint";
            this._btnPrint.Size = new System.Drawing.Size( 23, 22 );
            this._btnPrint.Text = "Print";
            this._btnPrint.Click += new System.EventHandler( this._btnPrint_Click );
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // ucpPolybagSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.Controls.Add( this._toolStrip );
            this.Controls.Add( this._lblPolybags );
            this.Controls.Add( this._gridPolybags );
            this.Controls.Add( this._groupFilter );
            this.Name = "ucpPolybagSearch";
            this.Size = new System.Drawing.Size( 718, 463 );
            ( (System.ComponentModel.ISupportInitialize)( this._gridPolybags ) ).EndInit();
            this._menuGridPopup.ResumeLayout( false );
            this._groupFilter.ResumeLayout( false );
            this._groupFilter.PerformLayout();
            this._toolStrip.ResumeLayout( false );
            this._toolStrip.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView _gridPolybags;
        private System.Windows.Forms.GroupBox _groupFilter;
        private System.Windows.Forms.Button _btnSearch;
        private System.Windows.Forms.ComboBox _cboFiscalMonth;
        private System.Windows.Forms.ComboBox _cboFiscalYear;
        private System.Windows.Forms.TextBox _txtPolybagComments;
        private System.Windows.Forms.ComboBox _cboSeason;
        private System.Windows.Forms.Label _lblPolybagComments;
        private System.Windows.Forms.Label _lblFiscalMonth;
        private System.Windows.Forms.Label _lblFiscalYear;
        private System.Windows.Forms.Label _lblSeason;
        private System.Windows.Forms.Label _lblRunDateRangeStart;
        private System.Windows.Forms.ComboBox _cboCreatedBy;
        private System.Windows.Forms.ComboBox _cboEstimateStatus;
        private System.Windows.Forms.Label _lblCreatedBy;
        private System.Windows.Forms.Label _lblAdNumber;
        private System.Windows.Forms.Label _lblStatus;
        private System.Windows.Forms.TextBox _txtPolybagDesc;
        private System.Windows.Forms.Label _lblPolybagDesc;
        private System.Windows.Forms.Label _lblPolybags;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtEndRunDate;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtStartRunDate;
        private System.Windows.Forms.Button _btnReset;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnNewPolybag;
        private System.Windows.Forms.ToolStripButton _btnOpenPolybag;
        private System.Windows.Forms.ToolStripButton _btnOpenPolybagReadOnly;
        private System.Windows.Forms.ToolStripButton _btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton _btnPrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label _lblPolybagID;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtAdNumber;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtPolybagID;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn EST_Polybag_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRunDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComments;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeason;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.ContextMenuStrip _menuGridPopup;
        private System.Windows.Forms.ToolStripMenuItem _menuContextNew;
        private System.Windows.Forms.ToolStripMenuItem _menuContextOpen;
        private System.Windows.Forms.ToolStripMenuItem _menuContextOpenReadOnly;
        private System.Windows.Forms.ToolStripMenuItem _menuContextDelete;
    }
}
