namespace CatalogEstimating.UserControls.Main
{
    partial class ucpComponentSearch
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
            this._gridEstimates = new System.Windows.Forms.DataGridView();
            this.EST_Component_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EST_Estimate_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Parent_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EST_Status_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RunDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AdNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnOpen = new System.Windows.Forms.ToolStripButton();
            this._btnOpenReadOnly = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._btnPrint = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._txtDescription = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this._cboComponentType = new System.Windows.Forms.ComboBox();
            this._groupVendorSupplied = new System.Windows.Forms.GroupBox();
            this._radBoth = new System.Windows.Forms.RadioButton();
            this._radNo = new System.Windows.Forms.RadioButton();
            this._radYes = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this._cboEstimateMediaType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this._cboPaperWeight = new System.Windows.Forms.ComboBox();
            this._cboPaperGrade = new System.Windows.Forms.ComboBox();
            this._btnReset = new System.Windows.Forms.Button();
            this._btnSearch = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this._dtStartRunDate = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._dtEndRunDate = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._txtComponentID = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._txtPageCount = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            this._lblComponents = new System.Windows.Forms.Label();
            this._groupFilter = new System.Windows.Forms.GroupBox();
            ( (System.ComponentModel.ISupportInitialize)( this._gridEstimates ) ).BeginInit();
            this._toolStrip.SuspendLayout();
            this._groupVendorSupplied.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).BeginInit();
            this._groupFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gridEstimates
            // 
            this._gridEstimates.AllowUserToAddRows = false;
            this._gridEstimates.AllowUserToDeleteRows = false;
            this._gridEstimates.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._gridEstimates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridEstimates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._gridEstimates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridEstimates.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.EST_Component_ID,
            this.EST_Estimate_ID,
            this.Parent_ID,
            this.EST_Status_ID,
            this.RunDate,
            this.Column2,
            this.AdNumber} );
            this._gridEstimates.Location = new System.Drawing.Point( 12, 215 );
            this._gridEstimates.Name = "_gridEstimates";
            this._gridEstimates.ReadOnly = true;
            this._gridEstimates.RowHeadersVisible = false;
            this._gridEstimates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridEstimates.Size = new System.Drawing.Size( 703, 241 );
            this._gridEstimates.TabIndex = 2;
            this._gridEstimates.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler( this._gridEstimates_CellDoubleClick );
            this._gridEstimates.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler( this._gridEstimates_SortCompare );
            this._gridEstimates.SelectionChanged += new System.EventHandler( this._gridEstimates_SelectionChanged );
            // 
            // EST_Component_ID
            // 
            this.EST_Component_ID.HeaderText = "EST_Component_ID";
            this.EST_Component_ID.Name = "EST_Component_ID";
            this.EST_Component_ID.ReadOnly = true;
            this.EST_Component_ID.Visible = false;
            // 
            // EST_Estimate_ID
            // 
            this.EST_Estimate_ID.HeaderText = "EST_Estimate_ID";
            this.EST_Estimate_ID.Name = "EST_Estimate_ID";
            this.EST_Estimate_ID.ReadOnly = true;
            this.EST_Estimate_ID.Visible = false;
            // 
            // Parent_ID
            // 
            this.Parent_ID.HeaderText = "Parent_ID";
            this.Parent_ID.Name = "Parent_ID";
            this.Parent_ID.ReadOnly = true;
            this.Parent_ID.Visible = false;
            // 
            // EST_Status_ID
            // 
            this.EST_Status_ID.HeaderText = "EST_Status_ID";
            this.EST_Status_ID.Name = "EST_Status_ID";
            this.EST_Status_ID.ReadOnly = true;
            this.EST_Status_ID.Visible = false;
            // 
            // RunDate
            // 
            this.RunDate.HeaderText = "Run Date";
            this.RunDate.Name = "RunDate";
            this.RunDate.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Component Desc";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // AdNumber
            // 
            this.AdNumber.HeaderText = "Ad #";
            this.AdNumber.Name = "AdNumber";
            this.AdNumber.ReadOnly = true;
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._btnOpen,
            this._btnOpenReadOnly,
            this.toolStripSeparator3,
            this._btnPrint} );
            this._toolStrip.Location = new System.Drawing.Point( 0, 438 );
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size( 87, 25 );
            this._toolStrip.TabIndex = 3;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _btnOpen
            // 
            this._btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnOpen.Enabled = false;
            this._btnOpen.Image = global::CatalogEstimating.Properties.Resources.Open;
            this._btnOpen.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnOpen.Name = "_btnOpen";
            this._btnOpen.Size = new System.Drawing.Size( 23, 22 );
            this._btnOpen.Text = "Open";
            this._btnOpen.Click += new System.EventHandler( this._btnOpen_Click );
            // 
            // _btnOpenReadOnly
            // 
            this._btnOpenReadOnly.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnOpenReadOnly.Enabled = false;
            this._btnOpenReadOnly.Image = global::CatalogEstimating.Properties.Resources.OpenReadOnly;
            this._btnOpenReadOnly.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnOpenReadOnly.Name = "_btnOpenReadOnly";
            this._btnOpenReadOnly.Size = new System.Drawing.Size( 23, 22 );
            this._btnOpenReadOnly.Text = "Open Read-Only";
            this._btnOpenReadOnly.Click += new System.EventHandler( this._btnOpenReadOnly_Click );
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 6, 68 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 105, 13 );
            this.label1.TabIndex = 4;
            this.label1.Text = "Range of Run Dates";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 126, 68 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 30, 13 );
            this.label2.TabIndex = 5;
            this.label2.Text = "From";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 286, 68 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 20, 13 );
            this.label3.TabIndex = 7;
            this.label3.Text = "To";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 497, 68 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 63, 13 );
            this.label4.TabIndex = 17;
            this.label4.Text = "Page Count";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 6, 42 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 89, 13 );
            this.label5.TabIndex = 2;
            this.label5.Text = "Component Desc";
            // 
            // _txtDescription
            // 
            this._txtDescription.Location = new System.Drawing.Point( 110, 39 );
            this._txtDescription.MaxLength = 35;
            this._txtDescription.Name = "_txtDescription";
            this._txtDescription.Size = new System.Drawing.Size( 306, 20 );
            this._txtDescription.TabIndex = 3;
            this._txtDescription.Validated += new System.EventHandler( this._txtDescription_Validated );
            this._txtDescription.TextChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 6, 120 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 88, 13 );
            this.label6.TabIndex = 11;
            this.label6.Text = "Component Type";
            // 
            // _cboComponentType
            // 
            this._cboComponentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboComponentType.FormattingEnabled = true;
            this._cboComponentType.Location = new System.Drawing.Point( 110, 117 );
            this._cboComponentType.Name = "_cboComponentType";
            this._cboComponentType.Size = new System.Drawing.Size( 156, 21 );
            this._cboComponentType.TabIndex = 12;
            this._cboComponentType.SelectedIndexChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _groupVendorSupplied
            // 
            this._groupVendorSupplied.Controls.Add( this._radBoth );
            this._groupVendorSupplied.Controls.Add( this._radNo );
            this._groupVendorSupplied.Controls.Add( this._radYes );
            this._groupVendorSupplied.Location = new System.Drawing.Point( 460, 91 );
            this._groupVendorSupplied.Name = "_groupVendorSupplied";
            this._groupVendorSupplied.Size = new System.Drawing.Size( 236, 43 );
            this._groupVendorSupplied.TabIndex = 19;
            this._groupVendorSupplied.TabStop = false;
            this._groupVendorSupplied.Text = "Vendor Supplied?";
            // 
            // _radBoth
            // 
            this._radBoth.AutoSize = true;
            this._radBoth.Checked = true;
            this._radBoth.Location = new System.Drawing.Point( 6, 20 );
            this._radBoth.Name = "_radBoth";
            this._radBoth.Size = new System.Drawing.Size( 36, 17 );
            this._radBoth.TabIndex = 0;
            this._radBoth.TabStop = true;
            this._radBoth.Text = "All";
            this._radBoth.UseVisualStyleBackColor = true;
            // 
            // _radNo
            // 
            this._radNo.AutoSize = true;
            this._radNo.Location = new System.Drawing.Point( 137, 19 );
            this._radNo.Name = "_radNo";
            this._radNo.Size = new System.Drawing.Size( 80, 17 );
            this._radNo.TabIndex = 2;
            this._radNo.TabStop = true;
            this._radNo.Text = "Exclude VS";
            this._radNo.UseVisualStyleBackColor = true;
            // 
            // _radYes
            // 
            this._radYes.AutoSize = true;
            this._radYes.Location = new System.Drawing.Point( 62, 20 );
            this._radYes.Name = "_radYes";
            this._radYes.Size = new System.Drawing.Size( 63, 17 );
            this._radYes.TabIndex = 1;
            this._radYes.TabStop = true;
            this._radYes.Text = "Only VS";
            this._radYes.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 6, 94 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 79, 13 );
            this.label7.TabIndex = 9;
            this.label7.Text = "Estimate Media";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _cboEstimateMediaType
            // 
            this._cboEstimateMediaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboEstimateMediaType.FormattingEnabled = true;
            this._cboEstimateMediaType.Location = new System.Drawing.Point( 110, 91 );
            this._cboEstimateMediaType.Name = "_cboEstimateMediaType";
            this._cboEstimateMediaType.Size = new System.Drawing.Size( 156, 21 );
            this._cboEstimateMediaType.TabIndex = 10;
            this._cboEstimateMediaType.SelectedIndexChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point( 497, 16 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 72, 13 );
            this.label8.TabIndex = 13;
            this.label8.Text = "Paper Weight";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point( 497, 42 );
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size( 67, 13 );
            this.label9.TabIndex = 15;
            this.label9.Text = "Paper Grade";
            // 
            // _cboPaperWeight
            // 
            this._cboPaperWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboPaperWeight.FormattingEnabled = true;
            this._cboPaperWeight.Location = new System.Drawing.Point( 587, 13 );
            this._cboPaperWeight.Name = "_cboPaperWeight";
            this._cboPaperWeight.Size = new System.Drawing.Size( 109, 21 );
            this._cboPaperWeight.TabIndex = 14;
            this._cboPaperWeight.SelectedIndexChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _cboPaperGrade
            // 
            this._cboPaperGrade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboPaperGrade.FormattingEnabled = true;
            this._cboPaperGrade.Location = new System.Drawing.Point( 587, 39 );
            this._cboPaperGrade.Name = "_cboPaperGrade";
            this._cboPaperGrade.Size = new System.Drawing.Size( 109, 21 );
            this._cboPaperGrade.TabIndex = 16;
            this._cboPaperGrade.SelectedIndexChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _btnReset
            // 
            this._btnReset.CausesValidation = false;
            this._btnReset.Location = new System.Drawing.Point( 621, 143 );
            this._btnReset.Name = "_btnReset";
            this._btnReset.Size = new System.Drawing.Size( 75, 23 );
            this._btnReset.TabIndex = 21;
            this._btnReset.Text = "Reset";
            this._btnReset.UseVisualStyleBackColor = true;
            this._btnReset.Click += new System.EventHandler( this._btnReset_Click );
            // 
            // _btnSearch
            // 
            this._btnSearch.Enabled = false;
            this._btnSearch.Location = new System.Drawing.Point( 500, 143 );
            this._btnSearch.Name = "_btnSearch";
            this._btnSearch.Size = new System.Drawing.Size( 75, 23 );
            this._btnSearch.TabIndex = 20;
            this._btnSearch.Text = "Search";
            this._btnSearch.UseVisualStyleBackColor = true;
            this._btnSearch.Click += new System.EventHandler( this._btnSearch_Click );
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point( 6, 16 );
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size( 75, 13 );
            this.label10.TabIndex = 0;
            this.label10.Text = "Component ID";
            // 
            // _dtStartRunDate
            // 
            this._dtStartRunDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtStartRunDate.Location = new System.Drawing.Point( 162, 65 );
            this._dtStartRunDate.Name = "_dtStartRunDate";
            this._dtStartRunDate.Size = new System.Drawing.Size( 104, 20 );
            this._dtStartRunDate.TabIndex = 6;
            this._dtStartRunDate.Value = null;
            this._dtStartRunDate.ValueChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _dtEndRunDate
            // 
            this._dtEndRunDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEndRunDate.Location = new System.Drawing.Point( 312, 65 );
            this._dtEndRunDate.Name = "_dtEndRunDate";
            this._dtEndRunDate.Size = new System.Drawing.Size( 104, 20 );
            this._dtEndRunDate.TabIndex = 8;
            this._dtEndRunDate.Value = null;
            this._dtEndRunDate.ValueChanged += new System.EventHandler( this.SearchCriteria_Changed );
            this._dtEndRunDate.Validated += new System.EventHandler( this._dtEndRunDate_Validated );
            this._dtEndRunDate.Validating += new System.ComponentModel.CancelEventHandler( this._dtEndRunDate_Validating );
            // 
            // _txtComponentID
            // 
            this._txtComponentID.AllowNegative = false;
            this._txtComponentID.FlashColor = System.Drawing.Color.Red;
            this._txtComponentID.Location = new System.Drawing.Point( 110, 13 );
            this._txtComponentID.MaxLength = 18;
            this._txtComponentID.Name = "_txtComponentID";
            this._txtComponentID.Size = new System.Drawing.Size( 156, 20 );
            this._txtComponentID.TabIndex = 1;
            this._txtComponentID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtComponentID.ThousandsSeperator = false;
            this._txtComponentID.Value = null;
            this._txtComponentID.TextChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _txtPageCount
            // 
            this._txtPageCount.AllowNegative = false;
            this._txtPageCount.FlashColor = System.Drawing.Color.Red;
            this._txtPageCount.Location = new System.Drawing.Point( 587, 65 );
            this._txtPageCount.MaxLength = 9;
            this._txtPageCount.Name = "_txtPageCount";
            this._txtPageCount.Size = new System.Drawing.Size( 100, 20 );
            this._txtPageCount.TabIndex = 18;
            this._txtPageCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtPageCount.Value = null;
            this._txtPageCount.TextChanged += new System.EventHandler( this.SearchCriteria_Changed );
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // _lblComponents
            // 
            this._lblComponents.AutoSize = true;
            this._lblComponents.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblComponents.ForeColor = System.Drawing.Color.Blue;
            this._lblComponents.Location = new System.Drawing.Point( 12, 198 );
            this._lblComponents.Name = "_lblComponents";
            this._lblComponents.Size = new System.Drawing.Size( 87, 13 );
            this._lblComponents.TabIndex = 1;
            this._lblComponents.Text = "0 Components";
            // 
            // _groupFilter
            // 
            this._groupFilter.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._groupFilter.Controls.Add( this._txtPageCount );
            this._groupFilter.Controls.Add( this._txtComponentID );
            this._groupFilter.Controls.Add( this._dtEndRunDate );
            this._groupFilter.Controls.Add( this._dtStartRunDate );
            this._groupFilter.Controls.Add( this.label10 );
            this._groupFilter.Controls.Add( this._btnSearch );
            this._groupFilter.Controls.Add( this._btnReset );
            this._groupFilter.Controls.Add( this._cboPaperGrade );
            this._groupFilter.Controls.Add( this._cboPaperWeight );
            this._groupFilter.Controls.Add( this.label9 );
            this._groupFilter.Controls.Add( this.label8 );
            this._groupFilter.Controls.Add( this._cboEstimateMediaType );
            this._groupFilter.Controls.Add( this.label7 );
            this._groupFilter.Controls.Add( this._groupVendorSupplied );
            this._groupFilter.Controls.Add( this._cboComponentType );
            this._groupFilter.Controls.Add( this.label6 );
            this._groupFilter.Controls.Add( this._txtDescription );
            this._groupFilter.Controls.Add( this.label5 );
            this._groupFilter.Controls.Add( this.label4 );
            this._groupFilter.Controls.Add( this.label3 );
            this._groupFilter.Controls.Add( this.label2 );
            this._groupFilter.Controls.Add( this.label1 );
            this._groupFilter.Location = new System.Drawing.Point( 12, 12 );
            this._groupFilter.Name = "_groupFilter";
            this._groupFilter.Size = new System.Drawing.Size( 703, 175 );
            this._groupFilter.TabIndex = 0;
            this._groupFilter.TabStop = false;
            this._groupFilter.Text = "Component Filter";
            // 
            // ucpComponentSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.Controls.Add( this._groupFilter );
            this.Controls.Add( this._lblComponents );
            this.Controls.Add( this._toolStrip );
            this.Controls.Add( this._gridEstimates );
            this.Name = "ucpComponentSearch";
            this.Size = new System.Drawing.Size( 726, 463 );
            ( (System.ComponentModel.ISupportInitialize)( this._gridEstimates ) ).EndInit();
            this._toolStrip.ResumeLayout( false );
            this._toolStrip.PerformLayout();
            this._groupVendorSupplied.ResumeLayout( false );
            this._groupVendorSupplied.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).EndInit();
            this._groupFilter.ResumeLayout( false );
            this._groupFilter.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView _gridEstimates;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnOpen;
        private System.Windows.Forms.ToolStripButton _btnOpenReadOnly;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton _btnPrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _txtDescription;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox _cboComponentType;
        private System.Windows.Forms.GroupBox _groupVendorSupplied;
        private System.Windows.Forms.RadioButton _radBoth;
        private System.Windows.Forms.RadioButton _radNo;
        private System.Windows.Forms.RadioButton _radYes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox _cboEstimateMediaType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox _cboPaperWeight;
        private System.Windows.Forms.ComboBox _cboPaperGrade;
        private System.Windows.Forms.Button _btnReset;
        private System.Windows.Forms.Button _btnSearch;
        private System.Windows.Forms.Label label10;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtStartRunDate;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtEndRunDate;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtComponentID;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtPageCount;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn EST_Component_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn EST_Estimate_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parent_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn EST_Status_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn RunDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn AdNumber;
        private System.Windows.Forms.Label _lblComponents;
        private System.Windows.Forms.GroupBox _groupFilter;
    }
}
