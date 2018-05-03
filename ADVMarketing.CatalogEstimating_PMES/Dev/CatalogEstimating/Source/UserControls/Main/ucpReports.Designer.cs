namespace CatalogEstimating.UserControls.Main
{
    partial class ucpReports
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
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnRefresh = new System.Windows.Forms.ToolStripButton();
            this._listReportType = new System.Windows.Forms.ListBox();
            this.rptreporttypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._dsReports = new CatalogEstimating.Datasets.RptReport();
            this._groupReportDetails = new System.Windows.Forms.GroupBox();
            this._panelReportDetails = new System.Windows.Forms.Panel();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this._groupReportHistory = new System.Windows.Forms.GroupBox();
            this._lblHistoryInstructions = new System.Windows.Forms.Label();
            this._gridReportHistory = new System.Windows.Forms.DataGridView();
            this.rptreportidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rptReportHistoryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._lblReportList = new System.Windows.Forms.Label();
            this.rpt_ReportHistoryTableAdapter = new CatalogEstimating.Datasets.RptReportTableAdapters.Rpt_ReportHistoryTableAdapter();
            this.rpt_reporttypeTableAdapter = new CatalogEstimating.Datasets.RptReportTableAdapters.rpt_reporttypeTableAdapter();
            this._toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rptreporttypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dsReports)).BeginInit();
            this._groupReportDetails.SuspendLayout();
            this._panelReportDetails.SuspendLayout();
            this._groupReportHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridReportHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptReportHistoryBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnRefresh});
            this._toolStrip.Location = new System.Drawing.Point(0, 418);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(35, 25);
            this._toolStrip.TabIndex = 54;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _btnRefresh
            // 
            this._btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnRefresh.Image = global::CatalogEstimating.Properties.Resources.Refresh;
            this._btnRefresh.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.Size = new System.Drawing.Size(23, 22);
            this._btnRefresh.Text = "Refresh";
            this._btnRefresh.Click += new System.EventHandler(this._btnRefresh_Click);
            // 
            // _listReportType
            // 
            this._listReportType.ColumnWidth = 200;
            this._listReportType.DataSource = this.rptreporttypeBindingSource;
            this._listReportType.DisplayMember = "description";
            this._listReportType.FormattingEnabled = true;
            this._listReportType.Location = new System.Drawing.Point(23, 25);
            this._listReportType.MultiColumn = true;
            this._listReportType.Name = "_listReportType";
            this._listReportType.Size = new System.Drawing.Size(411, 69);
            this._listReportType.TabIndex = 1;
            this._listReportType.ValueMember = "rpt_reporttype_id";
            this._listReportType.SelectedValueChanged += new System.EventHandler(this._listReportType_SelectedValueChanged);
            // 
            // rptreporttypeBindingSource
            // 
            this.rptreporttypeBindingSource.DataMember = "rpt_reporttype";
            this.rptreporttypeBindingSource.DataSource = this._dsReports;
            // 
            // _dsReports
            // 
            this._dsReports.DataSetName = "RptReport";
            this._dsReports.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _groupReportDetails
            // 
            this._groupReportDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._groupReportDetails.Controls.Add(this._panelReportDetails);
            this._groupReportDetails.Location = new System.Drawing.Point(14, 100);
            this._groupReportDetails.Name = "_groupReportDetails";
            this._groupReportDetails.Size = new System.Drawing.Size(578, 248);
            this._groupReportDetails.TabIndex = 2;
            this._groupReportDetails.TabStop = false;
            this._groupReportDetails.Text = "Report Filter && Selection Criteria";
            // 
            // _panelReportDetails
            // 
            this._panelReportDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._panelReportDetails.AutoScroll = true;
            this._panelReportDetails.BackColor = System.Drawing.Color.Transparent;
            this._panelReportDetails.Controls.Add(this.btnGenerateReport);
            this._panelReportDetails.Location = new System.Drawing.Point(10, 16);
            this._panelReportDetails.Name = "_panelReportDetails";
            this._panelReportDetails.Size = new System.Drawing.Size(557, 226);
            this._panelReportDetails.TabIndex = 0;
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateReport.Location = new System.Drawing.Point(450, 193);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(107, 30);
            this.btnGenerateReport.TabIndex = 1;
            this.btnGenerateReport.Text = "Generate Report";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this._btnRunReport_Click);
            // 
            // _groupReportHistory
            // 
            this._groupReportHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._groupReportHistory.Controls.Add(this._lblHistoryInstructions);
            this._groupReportHistory.Controls.Add(this._gridReportHistory);
            this._groupReportHistory.Location = new System.Drawing.Point(14, 354);
            this._groupReportHistory.Name = "_groupReportHistory";
            this._groupReportHistory.Size = new System.Drawing.Size(578, 137);
            this._groupReportHistory.TabIndex = 0;
            this._groupReportHistory.TabStop = false;
            this._groupReportHistory.Text = "Report History";
            // 
            // _lblHistoryInstructions
            // 
            this._lblHistoryInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._lblHistoryInstructions.AutoSize = true;
            this._lblHistoryInstructions.Location = new System.Drawing.Point(6, 183);
            this._lblHistoryInstructions.Name = "_lblHistoryInstructions";
            this._lblHistoryInstructions.Size = new System.Drawing.Size(517, 13);
            this._lblHistoryInstructions.TabIndex = 1;
            this._lblHistoryInstructions.Text = "Double Click the row to download and open the report in Excel (must have Excel in" +
                "stalled for report to open).";
            // 
            // _gridReportHistory
            // 
            this._gridReportHistory.AllowUserToAddRows = false;
            this._gridReportHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._gridReportHistory.AutoGenerateColumns = false;
            this._gridReportHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridReportHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridReportHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rptreportidDataGridViewTextBoxColumn,
            this.createddateDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.createdbyDataGridViewTextBoxColumn});
            this._gridReportHistory.DataSource = this.rptReportHistoryBindingSource;
            this._gridReportHistory.Location = new System.Drawing.Point(10, 19);
            this._gridReportHistory.Name = "_gridReportHistory";
            this._gridReportHistory.ReadOnly = true;
            this._gridReportHistory.RowHeadersVisible = false;
            this._gridReportHistory.Size = new System.Drawing.Size(559, 112);
            this._gridReportHistory.TabIndex = 0;
            this._gridReportHistory.TabStop = false;
            this._gridReportHistory.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridReportHistory_CellDoubleClick);
            // 
            // rptreportidDataGridViewTextBoxColumn
            // 
            this.rptreportidDataGridViewTextBoxColumn.DataPropertyName = "rpt_report_id";
            this.rptreportidDataGridViewTextBoxColumn.HeaderText = "rpt_report_id";
            this.rptreportidDataGridViewTextBoxColumn.Name = "rptreportidDataGridViewTextBoxColumn";
            this.rptreportidDataGridViewTextBoxColumn.ReadOnly = true;
            this.rptreportidDataGridViewTextBoxColumn.Visible = false;
            // 
            // createddateDataGridViewTextBoxColumn
            // 
            this.createddateDataGridViewTextBoxColumn.DataPropertyName = "createddate";
            this.createddateDataGridViewTextBoxColumn.HeaderText = "Date/Time";
            this.createddateDataGridViewTextBoxColumn.Name = "createddateDataGridViewTextBoxColumn";
            this.createddateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Name";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // createdbyDataGridViewTextBoxColumn
            // 
            this.createdbyDataGridViewTextBoxColumn.DataPropertyName = "createdby";
            this.createdbyDataGridViewTextBoxColumn.HeaderText = "Created By";
            this.createdbyDataGridViewTextBoxColumn.Name = "createdbyDataGridViewTextBoxColumn";
            this.createdbyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // rptReportHistoryBindingSource
            // 
            this.rptReportHistoryBindingSource.DataMember = "Rpt_ReportHistory";
            this.rptReportHistoryBindingSource.DataSource = this._dsReports;
            // 
            // _lblReportList
            // 
            this._lblReportList.AutoSize = true;
            this._lblReportList.Location = new System.Drawing.Point(20, 9);
            this._lblReportList.Name = "_lblReportList";
            this._lblReportList.Size = new System.Drawing.Size(44, 13);
            this._lblReportList.TabIndex = 0;
            this._lblReportList.Text = "Reports";
            // 
            // rpt_ReportHistoryTableAdapter
            // 
            this.rpt_ReportHistoryTableAdapter.ClearBeforeFill = true;
            // 
            // rpt_reporttypeTableAdapter
            // 
            this.rpt_reporttypeTableAdapter.ClearBeforeFill = true;
            // 
            // ucpReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._lblReportList);
            this.Controls.Add(this._groupReportHistory);
            this.Controls.Add(this._groupReportDetails);
            this.Controls.Add(this._listReportType);
            this.Controls.Add(this._toolStrip);
            this.Name = "ucpReports";
            this.Size = new System.Drawing.Size(606, 497);
            this.Load += new System.EventHandler(this.ucpReports_Load);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rptreporttypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dsReports)).EndInit();
            this._groupReportDetails.ResumeLayout(false);
            this._panelReportDetails.ResumeLayout(false);
            this._groupReportHistory.ResumeLayout(false);
            this._groupReportHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridReportHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptReportHistoryBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnRefresh;
        private System.Windows.Forms.ListBox _listReportType;
        private System.Windows.Forms.GroupBox _groupReportDetails;
        private System.Windows.Forms.GroupBox _groupReportHistory;
        private System.Windows.Forms.Label _lblReportList;
        private System.Windows.Forms.DataGridView _gridReportHistory;
        private System.Windows.Forms.Label _lblHistoryInstructions;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.Panel _panelReportDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn rptreportidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createddateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource rptReportHistoryBindingSource;
        private CatalogEstimating.Datasets.RptReport _dsReports;
        private CatalogEstimating.Datasets.RptReportTableAdapters.Rpt_ReportHistoryTableAdapter rpt_ReportHistoryTableAdapter;
        private System.Windows.Forms.BindingSource rptreporttypeBindingSource;
        private CatalogEstimating.Datasets.RptReportTableAdapters.rpt_reporttypeTableAdapter rpt_reporttypeTableAdapter;
    }
}
