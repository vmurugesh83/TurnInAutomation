namespace CatalogEstimating.UserControls.Admin
{
    partial class ucpAdminInsertionGroupSetup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this._groupMapping = new System.Windows.Forms.GroupBox();
            this._lblGroupInformation = new System.Windows.Forms.Label();
            this._gridPubsInGroup = new System.Windows.Forms.DataGridView();
            this.pubpubratemapidDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pubNMDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pubidDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publocidDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdbyDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createddateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedbyDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifieddateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._dsPublications = new CatalogEstimating.Datasets.Publications();
            this._lblPubsInGroup = new System.Windows.Forms.Label();
            this._btnRemoveAll = new System.Windows.Forms.Button();
            this._btnRemove = new System.Windows.Forms.Button();
            this._btnAddAll = new System.Windows.Forms.Button();
            this._btnAdd = new System.Windows.Forms.Button();
            this._cboLocationFilter = new System.Windows.Forms.ComboBox();
            this._lblLocationFilter = new System.Windows.Forms.Label();
            this._gridAvailablePubs = new System.Windows.Forms.DataGridView();
            this.pubpubratemapidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pubNMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pubidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publocidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifieddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._lblAvailablePubs = new System.Windows.Forms.Label();
            this._dtEffectiveDateMapping = new System.Windows.Forms.DateTimePicker();
            this._lblEffectiveDateMapping = new System.Windows.Forms.Label();
            this._chkActive = new System.Windows.Forms.CheckBox();
            this._txtComments = new System.Windows.Forms.TextBox();
            this._lblComments = new System.Windows.Forms.Label();
            this._txtDescription = new System.Windows.Forms.TextBox();
            this._lblDescription = new System.Windows.Forms.Label();
            this._cboGroup = new System.Windows.Forms.ComboBox();
            this._lblEffectiveDateSearch = new System.Windows.Forms.Label();
            this._lblGroup = new System.Windows.Forms.Label();
            this._btnSearch = new System.Windows.Forms.Button();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnNew = new System.Windows.Forms.ToolStripButton();
            this._cboEffectiveDate = new System.Windows.Forms.ComboBox();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._btnClear = new System.Windows.Forms.Button();
            this._groupMapping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridPubsInGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dsPublications)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridAvailablePubs)).BeginInit();
            this._toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // _groupMapping
            // 
            this._groupMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._groupMapping.Controls.Add(this._btnClear);
            this._groupMapping.Controls.Add(this._lblGroupInformation);
            this._groupMapping.Controls.Add(this._gridPubsInGroup);
            this._groupMapping.Controls.Add(this._lblPubsInGroup);
            this._groupMapping.Controls.Add(this._btnRemoveAll);
            this._groupMapping.Controls.Add(this._btnRemove);
            this._groupMapping.Controls.Add(this._btnAddAll);
            this._groupMapping.Controls.Add(this._btnAdd);
            this._groupMapping.Controls.Add(this._cboLocationFilter);
            this._groupMapping.Controls.Add(this._lblLocationFilter);
            this._groupMapping.Controls.Add(this._gridAvailablePubs);
            this._groupMapping.Controls.Add(this._lblAvailablePubs);
            this._groupMapping.Controls.Add(this._dtEffectiveDateMapping);
            this._groupMapping.Controls.Add(this._lblEffectiveDateMapping);
            this._groupMapping.Controls.Add(this._chkActive);
            this._groupMapping.Controls.Add(this._txtComments);
            this._groupMapping.Controls.Add(this._lblComments);
            this._groupMapping.Controls.Add(this._txtDescription);
            this._groupMapping.Controls.Add(this._lblDescription);
            this._groupMapping.Location = new System.Drawing.Point(15, 59);
            this._groupMapping.Name = "_groupMapping";
            this._groupMapping.Size = new System.Drawing.Size(702, 439);
            this._groupMapping.TabIndex = 9;
            this._groupMapping.TabStop = false;
            this._groupMapping.Text = "Group Mapping";
            // 
            // _lblGroupInformation
            // 
            this._lblGroupInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblGroupInformation.ForeColor = System.Drawing.Color.Blue;
            this._lblGroupInformation.Location = new System.Drawing.Point(7, 20);
            this._lblGroupInformation.Name = "_lblGroupInformation";
            this._lblGroupInformation.Size = new System.Drawing.Size(689, 23);
            this._lblGroupInformation.TabIndex = 23;
            // 
            // _gridPubsInGroup
            // 
            this._gridPubsInGroup.AllowUserToAddRows = false;
            this._gridPubsInGroup.AllowUserToDeleteRows = false;
            this._gridPubsInGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this._gridPubsInGroup.AutoGenerateColumns = false;
            this._gridPubsInGroup.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridPubsInGroup.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this._gridPubsInGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridPubsInGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pubpubratemapidDataGridViewTextBoxColumn1,
            this.pubNMDataGridViewTextBoxColumn1,
            this.pubidDataGridViewTextBoxColumn1,
            this.publocidDataGridViewTextBoxColumn1,
            this.createdbyDataGridViewTextBoxColumn1,
            this.createddateDataGridViewTextBoxColumn1,
            this.modifiedbyDataGridViewTextBoxColumn1,
            this.modifieddateDataGridViewTextBoxColumn1});
            this._gridPubsInGroup.DataMember = "pub_pubrate_map";
            this._gridPubsInGroup.DataSource = this._dsPublications;
            this._gridPubsInGroup.Location = new System.Drawing.Point(418, 197);
            this._gridPubsInGroup.Name = "_gridPubsInGroup";
            this._gridPubsInGroup.ReadOnly = true;
            this._gridPubsInGroup.RowHeadersVisible = false;
            this._gridPubsInGroup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridPubsInGroup.Size = new System.Drawing.Size(240, 223);
            this._gridPubsInGroup.TabIndex = 22;
            // 
            // pubpubratemapidDataGridViewTextBoxColumn1
            // 
            this.pubpubratemapidDataGridViewTextBoxColumn1.DataPropertyName = "pub_pubrate_map_id";
            this.pubpubratemapidDataGridViewTextBoxColumn1.HeaderText = "pub_pubrate_map_id";
            this.pubpubratemapidDataGridViewTextBoxColumn1.Name = "pubpubratemapidDataGridViewTextBoxColumn1";
            this.pubpubratemapidDataGridViewTextBoxColumn1.ReadOnly = true;
            this.pubpubratemapidDataGridViewTextBoxColumn1.Visible = false;
            // 
            // pubNMDataGridViewTextBoxColumn1
            // 
            this.pubNMDataGridViewTextBoxColumn1.DataPropertyName = "Pub_NM";
            this.pubNMDataGridViewTextBoxColumn1.HeaderText = "Publication";
            this.pubNMDataGridViewTextBoxColumn1.Name = "pubNMDataGridViewTextBoxColumn1";
            this.pubNMDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // pubidDataGridViewTextBoxColumn1
            // 
            this.pubidDataGridViewTextBoxColumn1.DataPropertyName = "pub_id";
            this.pubidDataGridViewTextBoxColumn1.HeaderText = "Pub Code";
            this.pubidDataGridViewTextBoxColumn1.Name = "pubidDataGridViewTextBoxColumn1";
            this.pubidDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // publocidDataGridViewTextBoxColumn1
            // 
            this.publocidDataGridViewTextBoxColumn1.DataPropertyName = "publoc_id";
            this.publocidDataGridViewTextBoxColumn1.HeaderText = "Pub Loc";
            this.publocidDataGridViewTextBoxColumn1.Name = "publocidDataGridViewTextBoxColumn1";
            this.publocidDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // createdbyDataGridViewTextBoxColumn1
            // 
            this.createdbyDataGridViewTextBoxColumn1.DataPropertyName = "createdby";
            this.createdbyDataGridViewTextBoxColumn1.HeaderText = "createdby";
            this.createdbyDataGridViewTextBoxColumn1.Name = "createdbyDataGridViewTextBoxColumn1";
            this.createdbyDataGridViewTextBoxColumn1.ReadOnly = true;
            this.createdbyDataGridViewTextBoxColumn1.Visible = false;
            // 
            // createddateDataGridViewTextBoxColumn1
            // 
            this.createddateDataGridViewTextBoxColumn1.DataPropertyName = "createddate";
            this.createddateDataGridViewTextBoxColumn1.HeaderText = "createddate";
            this.createddateDataGridViewTextBoxColumn1.Name = "createddateDataGridViewTextBoxColumn1";
            this.createddateDataGridViewTextBoxColumn1.ReadOnly = true;
            this.createddateDataGridViewTextBoxColumn1.Visible = false;
            // 
            // modifiedbyDataGridViewTextBoxColumn1
            // 
            this.modifiedbyDataGridViewTextBoxColumn1.DataPropertyName = "modifiedby";
            this.modifiedbyDataGridViewTextBoxColumn1.HeaderText = "modifiedby";
            this.modifiedbyDataGridViewTextBoxColumn1.Name = "modifiedbyDataGridViewTextBoxColumn1";
            this.modifiedbyDataGridViewTextBoxColumn1.ReadOnly = true;
            this.modifiedbyDataGridViewTextBoxColumn1.Visible = false;
            // 
            // modifieddateDataGridViewTextBoxColumn1
            // 
            this.modifieddateDataGridViewTextBoxColumn1.DataPropertyName = "modifieddate";
            this.modifieddateDataGridViewTextBoxColumn1.HeaderText = "modifieddate";
            this.modifieddateDataGridViewTextBoxColumn1.Name = "modifieddateDataGridViewTextBoxColumn1";
            this.modifieddateDataGridViewTextBoxColumn1.ReadOnly = true;
            this.modifieddateDataGridViewTextBoxColumn1.Visible = false;
            // 
            // _dsPublications
            // 
            this._dsPublications.DataSetName = "Publications";
            this._dsPublications.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _lblPubsInGroup
            // 
            this._lblPubsInGroup.AutoSize = true;
            this._lblPubsInGroup.Location = new System.Drawing.Point(415, 181);
            this._lblPubsInGroup.Name = "_lblPubsInGroup";
            this._lblPubsInGroup.Size = new System.Drawing.Size(75, 13);
            this._lblPubsInGroup.TabIndex = 21;
            this._lblPubsInGroup.Text = "Pubs In Group";
            // 
            // _btnRemoveAll
            // 
            this._btnRemoveAll.Enabled = false;
            this._btnRemoveAll.Location = new System.Drawing.Point(300, 326);
            this._btnRemoveAll.Name = "_btnRemoveAll";
            this._btnRemoveAll.Size = new System.Drawing.Size(96, 29);
            this._btnRemoveAll.TabIndex = 20;
            this._btnRemoveAll.Text = "<< Remove All";
            this._btnRemoveAll.UseVisualStyleBackColor = true;
            this._btnRemoveAll.Click += new System.EventHandler(this._btnRemoveAll_Click);
            // 
            // _btnRemove
            // 
            this._btnRemove.Enabled = false;
            this._btnRemove.Location = new System.Drawing.Point(300, 291);
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.Size = new System.Drawing.Size(96, 29);
            this._btnRemove.TabIndex = 19;
            this._btnRemove.Text = "< Remove";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += new System.EventHandler(this._btnRemove_Click);
            // 
            // _btnAddAll
            // 
            this._btnAddAll.Enabled = false;
            this._btnAddAll.Location = new System.Drawing.Point(300, 232);
            this._btnAddAll.Name = "_btnAddAll";
            this._btnAddAll.Size = new System.Drawing.Size(96, 29);
            this._btnAddAll.TabIndex = 18;
            this._btnAddAll.Text = "Add All >>";
            this._btnAddAll.UseVisualStyleBackColor = true;
            this._btnAddAll.Click += new System.EventHandler(this._btnAddAll_Click);
            // 
            // _btnAdd
            // 
            this._btnAdd.Enabled = false;
            this._btnAdd.Location = new System.Drawing.Point(300, 197);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(96, 29);
            this._btnAdd.TabIndex = 17;
            this._btnAdd.Text = "Add >";
            this._btnAdd.UseVisualStyleBackColor = true;
            this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
            // 
            // _cboLocationFilter
            // 
            this._cboLocationFilter.DisplayMember = "PubLoc_ID";
            this._cboLocationFilter.Enabled = false;
            this._cboLocationFilter.FormattingEnabled = true;
            this._cboLocationFilter.Location = new System.Drawing.Point(95, 139);
            this._cboLocationFilter.Name = "_cboLocationFilter";
            this._cboLocationFilter.Size = new System.Drawing.Size(133, 21);
            this._cboLocationFilter.TabIndex = 16;
            this._cboLocationFilter.ValueMember = "PubLoc_ID";
            this._cboLocationFilter.SelectedIndexChanged += new System.EventHandler(this._cboLocationFilter_SelectedIndexChanged);
            // 
            // _lblLocationFilter
            // 
            this._lblLocationFilter.AutoSize = true;
            this._lblLocationFilter.Location = new System.Drawing.Point(17, 142);
            this._lblLocationFilter.Name = "_lblLocationFilter";
            this._lblLocationFilter.Size = new System.Drawing.Size(73, 13);
            this._lblLocationFilter.TabIndex = 15;
            this._lblLocationFilter.Text = "Location Filter";
            // 
            // _gridAvailablePubs
            // 
            this._gridAvailablePubs.AllowUserToAddRows = false;
            this._gridAvailablePubs.AllowUserToDeleteRows = false;
            this._gridAvailablePubs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this._gridAvailablePubs.AutoGenerateColumns = false;
            this._gridAvailablePubs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridAvailablePubs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this._gridAvailablePubs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridAvailablePubs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pubpubratemapidDataGridViewTextBoxColumn,
            this.pubNMDataGridViewTextBoxColumn,
            this.pubidDataGridViewTextBoxColumn,
            this.publocidDataGridViewTextBoxColumn,
            this.createdbyDataGridViewTextBoxColumn,
            this.createddateDataGridViewTextBoxColumn,
            this.modifiedbyDataGridViewTextBoxColumn,
            this.modifieddateDataGridViewTextBoxColumn});
            this._gridAvailablePubs.DataMember = "pub_pubrate_map";
            this._gridAvailablePubs.DataSource = this._dsPublications;
            this._gridAvailablePubs.Location = new System.Drawing.Point(20, 197);
            this._gridAvailablePubs.Name = "_gridAvailablePubs";
            this._gridAvailablePubs.ReadOnly = true;
            this._gridAvailablePubs.RowHeadersVisible = false;
            this._gridAvailablePubs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridAvailablePubs.Size = new System.Drawing.Size(252, 223);
            this._gridAvailablePubs.TabIndex = 14;
            // 
            // pubpubratemapidDataGridViewTextBoxColumn
            // 
            this.pubpubratemapidDataGridViewTextBoxColumn.DataPropertyName = "pub_pubrate_map_id";
            this.pubpubratemapidDataGridViewTextBoxColumn.HeaderText = "pub_pubrate_map_id";
            this.pubpubratemapidDataGridViewTextBoxColumn.Name = "pubpubratemapidDataGridViewTextBoxColumn";
            this.pubpubratemapidDataGridViewTextBoxColumn.ReadOnly = true;
            this.pubpubratemapidDataGridViewTextBoxColumn.Visible = false;
            // 
            // pubNMDataGridViewTextBoxColumn
            // 
            this.pubNMDataGridViewTextBoxColumn.DataPropertyName = "Pub_NM";
            this.pubNMDataGridViewTextBoxColumn.HeaderText = "Publication";
            this.pubNMDataGridViewTextBoxColumn.Name = "pubNMDataGridViewTextBoxColumn";
            this.pubNMDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pubidDataGridViewTextBoxColumn
            // 
            this.pubidDataGridViewTextBoxColumn.DataPropertyName = "pub_id";
            this.pubidDataGridViewTextBoxColumn.HeaderText = "Pub Code";
            this.pubidDataGridViewTextBoxColumn.Name = "pubidDataGridViewTextBoxColumn";
            this.pubidDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // publocidDataGridViewTextBoxColumn
            // 
            this.publocidDataGridViewTextBoxColumn.DataPropertyName = "publoc_id";
            this.publocidDataGridViewTextBoxColumn.HeaderText = "Pub Loc";
            this.publocidDataGridViewTextBoxColumn.Name = "publocidDataGridViewTextBoxColumn";
            this.publocidDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // createdbyDataGridViewTextBoxColumn
            // 
            this.createdbyDataGridViewTextBoxColumn.DataPropertyName = "createdby";
            this.createdbyDataGridViewTextBoxColumn.HeaderText = "createdby";
            this.createdbyDataGridViewTextBoxColumn.Name = "createdbyDataGridViewTextBoxColumn";
            this.createdbyDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdbyDataGridViewTextBoxColumn.Visible = false;
            // 
            // createddateDataGridViewTextBoxColumn
            // 
            this.createddateDataGridViewTextBoxColumn.DataPropertyName = "createddate";
            this.createddateDataGridViewTextBoxColumn.HeaderText = "createddate";
            this.createddateDataGridViewTextBoxColumn.Name = "createddateDataGridViewTextBoxColumn";
            this.createddateDataGridViewTextBoxColumn.ReadOnly = true;
            this.createddateDataGridViewTextBoxColumn.Visible = false;
            // 
            // modifiedbyDataGridViewTextBoxColumn
            // 
            this.modifiedbyDataGridViewTextBoxColumn.DataPropertyName = "modifiedby";
            this.modifiedbyDataGridViewTextBoxColumn.HeaderText = "modifiedby";
            this.modifiedbyDataGridViewTextBoxColumn.Name = "modifiedbyDataGridViewTextBoxColumn";
            this.modifiedbyDataGridViewTextBoxColumn.ReadOnly = true;
            this.modifiedbyDataGridViewTextBoxColumn.Visible = false;
            // 
            // modifieddateDataGridViewTextBoxColumn
            // 
            this.modifieddateDataGridViewTextBoxColumn.DataPropertyName = "modifieddate";
            this.modifieddateDataGridViewTextBoxColumn.HeaderText = "modifieddate";
            this.modifieddateDataGridViewTextBoxColumn.Name = "modifieddateDataGridViewTextBoxColumn";
            this.modifieddateDataGridViewTextBoxColumn.ReadOnly = true;
            this.modifieddateDataGridViewTextBoxColumn.Visible = false;
            // 
            // _lblAvailablePubs
            // 
            this._lblAvailablePubs.AutoSize = true;
            this._lblAvailablePubs.Location = new System.Drawing.Point(17, 181);
            this._lblAvailablePubs.Name = "_lblAvailablePubs";
            this._lblAvailablePubs.Size = new System.Drawing.Size(77, 13);
            this._lblAvailablePubs.TabIndex = 12;
            this._lblAvailablePubs.Text = "Available Pubs";
            // 
            // _dtEffectiveDateMapping
            // 
            this._dtEffectiveDateMapping.Enabled = false;
            this._dtEffectiveDateMapping.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEffectiveDateMapping.Location = new System.Drawing.Point(392, 51);
            this._dtEffectiveDateMapping.Name = "_dtEffectiveDateMapping";
            this._dtEffectiveDateMapping.Size = new System.Drawing.Size(143, 20);
            this._dtEffectiveDateMapping.TabIndex = 10;
            // 
            // _lblEffectiveDateMapping
            // 
            this._lblEffectiveDateMapping.AutoSize = true;
            this._lblEffectiveDateMapping.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblEffectiveDateMapping.Location = new System.Drawing.Point(292, 55);
            this._lblEffectiveDateMapping.Name = "_lblEffectiveDateMapping";
            this._lblEffectiveDateMapping.Size = new System.Drawing.Size(94, 13);
            this._lblEffectiveDateMapping.TabIndex = 5;
            this._lblEffectiveDateMapping.Text = "Effective Date*";
            // 
            // _chkActive
            // 
            this._chkActive.AutoSize = true;
            this._chkActive.Enabled = false;
            this._chkActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._chkActive.Location = new System.Drawing.Point(552, 54);
            this._chkActive.Name = "_chkActive";
            this._chkActive.Size = new System.Drawing.Size(67, 17);
            this._chkActive.TabIndex = 4;
            this._chkActive.Text = "Active*";
            this._chkActive.UseVisualStyleBackColor = true;
            this._chkActive.CheckedChanged += new System.EventHandler(this._chkActive_CheckedChanged);
            // 
            // _txtComments
            // 
            this._txtComments.Location = new System.Drawing.Point(95, 85);
            this._txtComments.MaxLength = 255;
            this._txtComments.Multiline = true;
            this._txtComments.Name = "_txtComments";
            this._txtComments.ReadOnly = true;
            this._txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtComments.Size = new System.Drawing.Size(563, 46);
            this._txtComments.TabIndex = 3;
            this._txtComments.TextChanged += new System.EventHandler(this._txtComments_TextChanged);
            // 
            // _lblComments
            // 
            this._lblComments.AutoSize = true;
            this._lblComments.Location = new System.Drawing.Point(17, 85);
            this._lblComments.Name = "_lblComments";
            this._lblComments.Size = new System.Drawing.Size(56, 13);
            this._lblComments.TabIndex = 2;
            this._lblComments.Text = "Comments";
            // 
            // _txtDescription
            // 
            this._txtDescription.Location = new System.Drawing.Point(95, 52);
            this._txtDescription.MaxLength = 35;
            this._txtDescription.Name = "_txtDescription";
            this._txtDescription.ReadOnly = true;
            this._txtDescription.Size = new System.Drawing.Size(177, 20);
            this._txtDescription.TabIndex = 1;
            this._txtDescription.TextChanged += new System.EventHandler(this._txtDescription_TextChanged);
            // 
            // _lblDescription
            // 
            this._lblDescription.AutoSize = true;
            this._lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblDescription.Location = new System.Drawing.Point(17, 55);
            this._lblDescription.Name = "_lblDescription";
            this._lblDescription.Size = new System.Drawing.Size(76, 13);
            this._lblDescription.TabIndex = 0;
            this._lblDescription.Text = "Description*";
            // 
            // _cboGroup
            // 
            this._cboGroup.DisplayMember = "description";
            this._cboGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboGroup.FormattingEnabled = true;
            this._cboGroup.Location = new System.Drawing.Point(110, 3);
            this._cboGroup.Name = "_cboGroup";
            this._cboGroup.Size = new System.Drawing.Size(177, 21);
            this._cboGroup.TabIndex = 7;
            this._cboGroup.ValueMember = "description";
            this._cboGroup.SelectedValueChanged += new System.EventHandler(this._cboGroup_SelectedValueChanged);
            // 
            // _lblEffectiveDateSearch
            // 
            this._lblEffectiveDateSearch.AutoSize = true;
            this._lblEffectiveDateSearch.Location = new System.Drawing.Point(12, 37);
            this._lblEffectiveDateSearch.Name = "_lblEffectiveDateSearch";
            this._lblEffectiveDateSearch.Size = new System.Drawing.Size(75, 13);
            this._lblEffectiveDateSearch.TabIndex = 6;
            this._lblEffectiveDateSearch.Text = "Effective Date";
            // 
            // _lblGroup
            // 
            this._lblGroup.AutoSize = true;
            this._lblGroup.Location = new System.Drawing.Point(12, 9);
            this._lblGroup.Name = "_lblGroup";
            this._lblGroup.Size = new System.Drawing.Size(36, 13);
            this._lblGroup.TabIndex = 5;
            this._lblGroup.Text = "Group";
            // 
            // _btnSearch
            // 
            this._btnSearch.Location = new System.Drawing.Point(305, 26);
            this._btnSearch.Name = "_btnSearch";
            this._btnSearch.Size = new System.Drawing.Size(68, 27);
            this._btnSearch.TabIndex = 53;
            this._btnSearch.Text = "Search";
            this._btnSearch.UseVisualStyleBackColor = true;
            this._btnSearch.Click += new System.EventHandler(this._btnSearch_Click);
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnNew});
            this._toolStrip.Location = new System.Drawing.Point(0, 308);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(35, 25);
            this._toolStrip.TabIndex = 54;
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
            // _cboEffectiveDate
            // 
            this._cboEffectiveDate.DisplayMember = "pub_pubgroup_id";
            this._cboEffectiveDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboEffectiveDate.FormattingEnabled = true;
            this._cboEffectiveDate.Location = new System.Drawing.Point(110, 30);
            this._cboEffectiveDate.Name = "_cboEffectiveDate";
            this._cboEffectiveDate.Size = new System.Drawing.Size(177, 21);
            this._cboEffectiveDate.TabIndex = 55;
            this._cboEffectiveDate.ValueMember = "pub_pubgroup_id";
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // _btnClear
            // 
            this._btnClear.Enabled = false;
            this._btnClear.Location = new System.Drawing.Point(552, 19);
            this._btnClear.Name = "_btnClear";
            this._btnClear.Size = new System.Drawing.Size(75, 23);
            this._btnClear.TabIndex = 24;
            this._btnClear.Text = "Clear";
            this._btnClear.UseVisualStyleBackColor = true;
            this._btnClear.Click += new System.EventHandler(this._btnClear_Click);
            // 
            // ucpAdminInsertionGroupSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._cboEffectiveDate);
            this.Controls.Add(this._toolStrip);
            this.Controls.Add(this._btnSearch);
            this.Controls.Add(this._groupMapping);
            this.Controls.Add(this._cboGroup);
            this.Controls.Add(this._lblEffectiveDateSearch);
            this.Controls.Add(this._lblGroup);
            this.Name = "ucpAdminInsertionGroupSetup";
            this.Size = new System.Drawing.Size(731, 513);
            this.Load += new System.EventHandler(this.ucpAdminInsertionGroupSetup_Load);
            this._groupMapping.ResumeLayout(false);
            this._groupMapping.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridPubsInGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dsPublications)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridAvailablePubs)).EndInit();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox _groupMapping;
        private System.Windows.Forms.ComboBox _cboGroup;
        private System.Windows.Forms.Label _lblEffectiveDateSearch;
        private System.Windows.Forms.Label _lblGroup;
        private System.Windows.Forms.TextBox _txtComments;
        private System.Windows.Forms.Label _lblComments;
        private System.Windows.Forms.TextBox _txtDescription;
        private System.Windows.Forms.Label _lblDescription;
        private System.Windows.Forms.Label _lblEffectiveDateMapping;
        private System.Windows.Forms.CheckBox _chkActive;
        private System.Windows.Forms.DateTimePicker _dtEffectiveDateMapping;
        private System.Windows.Forms.Label _lblAvailablePubs;
        private System.Windows.Forms.ComboBox _cboLocationFilter;
        private System.Windows.Forms.Label _lblLocationFilter;
        private System.Windows.Forms.DataGridView _gridAvailablePubs;
        private System.Windows.Forms.Button _btnRemoveAll;
        private System.Windows.Forms.Button _btnRemove;
        private System.Windows.Forms.Button _btnAddAll;
        private System.Windows.Forms.Button _btnAdd;
        private System.Windows.Forms.Button _btnSearch;
        private System.Windows.Forms.DataGridView _gridPubsInGroup;
        private System.Windows.Forms.Label _lblPubsInGroup;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnNew;
        private System.Windows.Forms.ComboBox _cboEffectiveDate;
        private CatalogEstimating.Datasets.Publications _dsPublications;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn pubpubratemapidDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn pubNMDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn pubidDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn publocidDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdbyDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn createddateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedbyDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifieddateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn pubpubratemapidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pubNMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pubidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn publocidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createddateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifieddateDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label _lblGroupInformation;
        private System.Windows.Forms.Button _btnClear;
    }
}
