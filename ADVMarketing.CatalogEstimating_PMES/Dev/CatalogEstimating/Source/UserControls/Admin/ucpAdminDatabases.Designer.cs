namespace CatalogEstimating.UserControls.Admin
{
    partial class ucpAdminDatabases
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
            this._gridDatabases = new System.Windows.Forms.DataGridView();
            this.assocDatabasessAllBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._dsAdministration = new CatalogEstimating.Datasets.Administration();
            this._groupPurging = new System.Windows.Forms.GroupBox();
            this._txtCurrentDatabase = new System.Windows.Forms.Label();
            this._lblCurrentDatabase = new System.Windows.Forms.Label();
            this._btnPurge = new System.Windows.Forms.Button();
            this._dtPurgeDate = new System.Windows.Forms.DateTimePicker();
            this._lblPurgeDate = new System.Windows.Forms.Label();
            this.assocDatabases_s_AllTableAdapter = new CatalogEstimating.Datasets.AdministrationTableAdapters.AssocDatabases_s_AllTableAdapter();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnCancel = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this._tabMaintenance = new System.Windows.Forms.TabPage();
            this._tabPurge = new System.Windows.Forms.TabPage();
            this._tabSynch = new System.Windows.Forms.TabPage();
            this._lblDestinationDB = new System.Windows.Forms.Label();
            this._lblSynchStatus = new System.Windows.Forms.Label();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._btnSynch = new System.Windows.Forms.Button();
            this._cboDatabases = new System.Windows.Forms.ComboBox();
            this._errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            this.databaseidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.databasetype_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.displayorderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.databasetype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.connectionstringDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifieddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRateSyncDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstimateSynced = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ( (System.ComponentModel.ISupportInitialize)( this._gridDatabases ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.assocDatabasessAllBindingSource ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this._dsAdministration ) ).BeginInit();
            this._groupPurging.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this._tabMaintenance.SuspendLayout();
            this._tabPurge.SuspendLayout();
            this._tabSynch.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _gridDatabases
            // 
            this._gridDatabases.AllowUserToAddRows = false;
            this._gridDatabases.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._gridDatabases.AutoGenerateColumns = false;
            this._gridDatabases.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridDatabases.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._gridDatabases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridDatabases.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.databaseidDataGridViewTextBoxColumn,
            this.databasetype_id,
            this.descriptionDataGridViewTextBoxColumn,
            this.displayDataGridViewCheckBoxColumn,
            this.displayorderDataGridViewTextBoxColumn,
            this.databasetype,
            this.connectionstringDataGridViewTextBoxColumn,
            this.createdbyDataGridViewTextBoxColumn,
            this.createddateDataGridViewTextBoxColumn,
            this.modifiedbyDataGridViewTextBoxColumn,
            this.modifieddateDataGridViewTextBoxColumn,
            this.colRateSyncDate,
            this.colEstimateSynced} );
            this._gridDatabases.DataSource = this.assocDatabasessAllBindingSource;
            this._gridDatabases.Location = new System.Drawing.Point( 6, 6 );
            this._gridDatabases.Name = "_gridDatabases";
            this._gridDatabases.Size = new System.Drawing.Size( 554, 356 );
            this._gridDatabases.TabIndex = 0;
            this._gridDatabases.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler( this._gridDatabases_CellBeginEdit );
            this._gridDatabases.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler( this._gridDatabases_CellValidating );
            this._gridDatabases.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler( this._gridDatabases_CellEndEdit );
            // 
            // assocDatabasessAllBindingSource
            // 
            this.assocDatabasessAllBindingSource.DataMember = "AssocDatabases_s_All";
            this.assocDatabasessAllBindingSource.DataSource = this._dsAdministration;
            this.assocDatabasessAllBindingSource.Filter = "databasetype_id <> 4";
            // 
            // _dsAdministration
            // 
            this._dsAdministration.DataSetName = "AssocDatabases";
            this._dsAdministration.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _groupPurging
            // 
            this._groupPurging.Controls.Add( this._txtCurrentDatabase );
            this._groupPurging.Controls.Add( this._lblCurrentDatabase );
            this._groupPurging.Controls.Add( this._btnPurge );
            this._groupPurging.Controls.Add( this._dtPurgeDate );
            this._groupPurging.Controls.Add( this._lblPurgeDate );
            this._groupPurging.Location = new System.Drawing.Point( 25, 33 );
            this._groupPurging.Name = "_groupPurging";
            this._groupPurging.Size = new System.Drawing.Size( 422, 115 );
            this._groupPurging.TabIndex = 1;
            this._groupPurging.TabStop = false;
            this._groupPurging.Text = "Purging";
            // 
            // _txtCurrentDatabase
            // 
            this._txtCurrentDatabase.AutoSize = true;
            this._txtCurrentDatabase.Location = new System.Drawing.Point( 112, 74 );
            this._txtCurrentDatabase.Name = "_txtCurrentDatabase";
            this._txtCurrentDatabase.Size = new System.Drawing.Size( 0, 13 );
            this._txtCurrentDatabase.TabIndex = 4;
            // 
            // _lblCurrentDatabase
            // 
            this._lblCurrentDatabase.AutoSize = true;
            this._lblCurrentDatabase.Location = new System.Drawing.Point( 16, 74 );
            this._lblCurrentDatabase.Name = "_lblCurrentDatabase";
            this._lblCurrentDatabase.Size = new System.Drawing.Size( 96, 13 );
            this._lblCurrentDatabase.TabIndex = 3;
            this._lblCurrentDatabase.Text = "Current Database: ";
            // 
            // _btnPurge
            // 
            this._btnPurge.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._btnPurge.Location = new System.Drawing.Point( 324, 66 );
            this._btnPurge.Name = "_btnPurge";
            this._btnPurge.Size = new System.Drawing.Size( 80, 29 );
            this._btnPurge.TabIndex = 2;
            this._btnPurge.Text = "&Purge";
            this._btnPurge.UseVisualStyleBackColor = true;
            this._btnPurge.Click += new System.EventHandler( this._btnPurge_Click );
            // 
            // _dtPurgeDate
            // 
            this._dtPurgeDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtPurgeDate.Location = new System.Drawing.Point( 83, 23 );
            this._dtPurgeDate.Name = "_dtPurgeDate";
            this._dtPurgeDate.Size = new System.Drawing.Size( 132, 20 );
            this._dtPurgeDate.TabIndex = 1;
            // 
            // _lblPurgeDate
            // 
            this._lblPurgeDate.AutoSize = true;
            this._lblPurgeDate.Location = new System.Drawing.Point( 16, 27 );
            this._lblPurgeDate.Name = "_lblPurgeDate";
            this._lblPurgeDate.Size = new System.Drawing.Size( 61, 13 );
            this._lblPurgeDate.TabIndex = 0;
            this._lblPurgeDate.Text = "Purge Date";
            // 
            // assocDatabases_s_AllTableAdapter
            // 
            this.assocDatabases_s_AllTableAdapter.ClearBeforeFill = true;
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._btnCancel} );
            this._toolStrip.Location = new System.Drawing.Point( 0, 397 );
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size( 35, 25 );
            this._toolStrip.TabIndex = 2;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _btnCancel
            // 
            this._btnCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnCancel.Image = global::CatalogEstimating.Properties.Resources.Cancel;
            this._btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnCancel.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this._btnCancel.MergeIndex = 2;
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size( 23, 22 );
            this._btnCancel.Text = "Cancel";
            this._btnCancel.Click += new System.EventHandler( this._btnCancel_Click );
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.tabControl1.Controls.Add( this._tabMaintenance );
            this.tabControl1.Controls.Add( this._tabPurge );
            this.tabControl1.Controls.Add( this._tabSynch );
            this.tabControl1.Location = new System.Drawing.Point( 0, 0 );
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size( 574, 394 );
            this.tabControl1.TabIndex = 3;
            // 
            // _tabMaintenance
            // 
            this._tabMaintenance.Controls.Add( this._gridDatabases );
            this._tabMaintenance.Location = new System.Drawing.Point( 4, 22 );
            this._tabMaintenance.Name = "_tabMaintenance";
            this._tabMaintenance.Padding = new System.Windows.Forms.Padding( 3 );
            this._tabMaintenance.Size = new System.Drawing.Size( 566, 368 );
            this._tabMaintenance.TabIndex = 0;
            this._tabMaintenance.Text = "Order and Naming";
            this._tabMaintenance.UseVisualStyleBackColor = true;
            // 
            // _tabPurge
            // 
            this._tabPurge.Controls.Add( this._groupPurging );
            this._tabPurge.Location = new System.Drawing.Point( 4, 22 );
            this._tabPurge.Name = "_tabPurge";
            this._tabPurge.Padding = new System.Windows.Forms.Padding( 3 );
            this._tabPurge.Size = new System.Drawing.Size( 566, 368 );
            this._tabPurge.TabIndex = 1;
            this._tabPurge.Text = "Purge";
            this._tabPurge.UseVisualStyleBackColor = true;
            // 
            // _tabSynch
            // 
            this._tabSynch.Controls.Add( this._lblDestinationDB );
            this._tabSynch.Controls.Add( this._lblSynchStatus );
            this._tabSynch.Controls.Add( this._progressBar );
            this._tabSynch.Controls.Add( this._btnSynch );
            this._tabSynch.Controls.Add( this._cboDatabases );
            this._tabSynch.Location = new System.Drawing.Point( 4, 22 );
            this._tabSynch.Name = "_tabSynch";
            this._tabSynch.Padding = new System.Windows.Forms.Padding( 3 );
            this._tabSynch.Size = new System.Drawing.Size( 566, 368 );
            this._tabSynch.TabIndex = 2;
            this._tabSynch.Text = "Synch";
            this._tabSynch.UseVisualStyleBackColor = true;
            // 
            // _lblDestinationDB
            // 
            this._lblDestinationDB.AutoSize = true;
            this._lblDestinationDB.Location = new System.Drawing.Point( 6, 9 );
            this._lblDestinationDB.Name = "_lblDestinationDB";
            this._lblDestinationDB.Size = new System.Drawing.Size( 112, 13 );
            this._lblDestinationDB.TabIndex = 4;
            this._lblDestinationDB.Text = "Destination Database:";
            // 
            // _lblSynchStatus
            // 
            this._lblSynchStatus.Location = new System.Drawing.Point( 124, 89 );
            this._lblSynchStatus.Name = "_lblSynchStatus";
            this._lblSynchStatus.Size = new System.Drawing.Size( 231, 20 );
            this._lblSynchStatus.TabIndex = 3;
            // 
            // _progressBar
            // 
            this._progressBar.Location = new System.Drawing.Point( 124, 63 );
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size( 231, 23 );
            this._progressBar.TabIndex = 2;
            // 
            // _btnSynch
            // 
            this._btnSynch.Location = new System.Drawing.Point( 124, 33 );
            this._btnSynch.Name = "_btnSynch";
            this._btnSynch.Size = new System.Drawing.Size( 152, 23 );
            this._btnSynch.TabIndex = 1;
            this._btnSynch.Text = "Synchronize Database";
            this._btnSynch.UseVisualStyleBackColor = true;
            this._btnSynch.Click += new System.EventHandler( this._btnSynch_Click );
            // 
            // _cboDatabases
            // 
            this._cboDatabases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboDatabases.FormattingEnabled = true;
            this._cboDatabases.Location = new System.Drawing.Point( 124, 6 );
            this._cboDatabases.Name = "_cboDatabases";
            this._cboDatabases.Size = new System.Drawing.Size( 231, 21 );
            this._cboDatabases.TabIndex = 0;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // databaseidDataGridViewTextBoxColumn
            // 
            this.databaseidDataGridViewTextBoxColumn.DataPropertyName = "database_id";
            this.databaseidDataGridViewTextBoxColumn.HeaderText = "database_id";
            this.databaseidDataGridViewTextBoxColumn.Name = "databaseidDataGridViewTextBoxColumn";
            this.databaseidDataGridViewTextBoxColumn.ReadOnly = true;
            this.databaseidDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.databaseidDataGridViewTextBoxColumn.Visible = false;
            // 
            // databasetype_id
            // 
            this.databasetype_id.DataPropertyName = "databasetype_id";
            this.databasetype_id.HeaderText = "databasetype_id";
            this.databasetype_id.Name = "databasetype_id";
            this.databasetype_id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.databasetype_id.Visible = false;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Database";
            this.descriptionDataGridViewTextBoxColumn.MaxInputLength = 35;
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // displayDataGridViewCheckBoxColumn
            // 
            this.displayDataGridViewCheckBoxColumn.DataPropertyName = "display";
            this.displayDataGridViewCheckBoxColumn.HeaderText = "Display";
            this.displayDataGridViewCheckBoxColumn.Name = "displayDataGridViewCheckBoxColumn";
            // 
            // displayorderDataGridViewTextBoxColumn
            // 
            this.displayorderDataGridViewTextBoxColumn.DataPropertyName = "displayorder";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.displayorderDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.displayorderDataGridViewTextBoxColumn.HeaderText = "Display Order";
            this.displayorderDataGridViewTextBoxColumn.Name = "displayorderDataGridViewTextBoxColumn";
            this.displayorderDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // databasetype
            // 
            this.databasetype.DataPropertyName = "databasetype";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.databasetype.DefaultCellStyle = dataGridViewCellStyle3;
            this.databasetype.HeaderText = "Type";
            this.databasetype.Name = "databasetype";
            this.databasetype.ReadOnly = true;
            this.databasetype.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // connectionstringDataGridViewTextBoxColumn
            // 
            this.connectionstringDataGridViewTextBoxColumn.DataPropertyName = "connectionstring";
            this.connectionstringDataGridViewTextBoxColumn.HeaderText = "connectionstring";
            this.connectionstringDataGridViewTextBoxColumn.Name = "connectionstringDataGridViewTextBoxColumn";
            this.connectionstringDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.connectionstringDataGridViewTextBoxColumn.Visible = false;
            // 
            // createdbyDataGridViewTextBoxColumn
            // 
            this.createdbyDataGridViewTextBoxColumn.DataPropertyName = "createdby";
            this.createdbyDataGridViewTextBoxColumn.HeaderText = "createdby";
            this.createdbyDataGridViewTextBoxColumn.Name = "createdbyDataGridViewTextBoxColumn";
            this.createdbyDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.createdbyDataGridViewTextBoxColumn.Visible = false;
            // 
            // createddateDataGridViewTextBoxColumn
            // 
            this.createddateDataGridViewTextBoxColumn.DataPropertyName = "createddate";
            this.createddateDataGridViewTextBoxColumn.HeaderText = "createddate";
            this.createddateDataGridViewTextBoxColumn.Name = "createddateDataGridViewTextBoxColumn";
            this.createddateDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.createddateDataGridViewTextBoxColumn.Visible = false;
            // 
            // modifiedbyDataGridViewTextBoxColumn
            // 
            this.modifiedbyDataGridViewTextBoxColumn.DataPropertyName = "modifiedby";
            this.modifiedbyDataGridViewTextBoxColumn.HeaderText = "modifiedby";
            this.modifiedbyDataGridViewTextBoxColumn.Name = "modifiedbyDataGridViewTextBoxColumn";
            this.modifiedbyDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.modifiedbyDataGridViewTextBoxColumn.Visible = false;
            // 
            // modifieddateDataGridViewTextBoxColumn
            // 
            this.modifieddateDataGridViewTextBoxColumn.DataPropertyName = "modifieddate";
            this.modifieddateDataGridViewTextBoxColumn.HeaderText = "modifieddate";
            this.modifieddateDataGridViewTextBoxColumn.Name = "modifieddateDataGridViewTextBoxColumn";
            this.modifieddateDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.modifieddateDataGridViewTextBoxColumn.Visible = false;
            // 
            // colRateSyncDate
            // 
            this.colRateSyncDate.HeaderText = "Last Date Rates Synced";
            this.colRateSyncDate.Name = "colRateSyncDate";
            this.colRateSyncDate.ReadOnly = true;
            this.colRateSyncDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colEstimateSynced
            // 
            this.colEstimateSynced.HeaderText = "Last Date Est Synced";
            this.colEstimateSynced.Name = "colEstimateSynced";
            this.colEstimateSynced.ReadOnly = true;
            this.colEstimateSynced.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ucpAdminDatabases
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this.tabControl1 );
            this.Controls.Add( this._toolStrip );
            this.Name = "ucpAdminDatabases";
            this.Size = new System.Drawing.Size( 574, 422 );
            this.Load += new System.EventHandler( this.ucpAdminDatabases_Load );
            ( (System.ComponentModel.ISupportInitialize)( this._gridDatabases ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.assocDatabasessAllBindingSource ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this._dsAdministration ) ).EndInit();
            this._groupPurging.ResumeLayout( false );
            this._groupPurging.PerformLayout();
            this._toolStrip.ResumeLayout( false );
            this._toolStrip.PerformLayout();
            this.tabControl1.ResumeLayout( false );
            this._tabMaintenance.ResumeLayout( false );
            this._tabPurge.ResumeLayout( false );
            this._tabSynch.ResumeLayout( false );
            this._tabSynch.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView _gridDatabases;
        private System.Windows.Forms.GroupBox _groupPurging;
        private System.Windows.Forms.DateTimePicker _dtPurgeDate;
        private System.Windows.Forms.Label _lblPurgeDate;
        private System.Windows.Forms.Button _btnPurge;
        private System.Windows.Forms.Label _txtCurrentDatabase;
        private System.Windows.Forms.Label _lblCurrentDatabase;
        private System.Windows.Forms.BindingSource assocDatabasessAllBindingSource;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnCancel;
        private CatalogEstimating.Datasets.Administration _dsAdministration;
        private CatalogEstimating.Datasets.AdministrationTableAdapters.AssocDatabases_s_AllTableAdapter assocDatabases_s_AllTableAdapter;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage _tabMaintenance;
        private System.Windows.Forms.TabPage _tabPurge;
        private System.Windows.Forms.TabPage _tabSynch;
        private System.Windows.Forms.Button _btnSynch;
        private System.Windows.Forms.ComboBox _cboDatabases;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.Label _lblSynchStatus;
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Label _lblDestinationDB;
        private System.Windows.Forms.DataGridViewTextBoxColumn databaseidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn databasetype_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn displayDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayorderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn databasetype;
        private System.Windows.Forms.DataGridViewTextBoxColumn connectionstringDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createddateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifieddateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRateSyncDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstimateSynced;
    }
}
