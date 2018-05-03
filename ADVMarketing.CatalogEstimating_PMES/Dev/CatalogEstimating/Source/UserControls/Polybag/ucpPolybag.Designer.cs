namespace CatalogEstimating.UserControls.Polybag
{
    partial class ucpPolybag
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this._cboBagRate = new System.Windows.Forms.ComboBox();
            this._lblBagRate = new System.Windows.Forms.Label();
            this._cboPrinterVendor = new System.Windows.Forms.ComboBox();
            this.printersPrinterIDandDescriptionByRunDateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._dsPolybagGroup = new CatalogEstimating.Datasets.PolybagGroup();
            this._lblPrinterVendor = new System.Windows.Forms.Label();
            this._txtComments = new System.Windows.Forms.TextBox();
            this._lblComments = new System.Windows.Forms.Label();
            this._txtDescription = new System.Windows.Forms.TextBox();
            this._lblDescription = new System.Windows.Forms.Label();
            this._lblMakereadyRate = new System.Windows.Forms.Label();
            this._gridPolybag = new System.Windows.Forms.DataGridView();
            this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.estpolybagBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._txtTotal = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._groupMessages = new System.Windows.Forms.GroupBox();
            this._txtMessageMakereadyRate = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._chkUseMessage = new System.Windows.Forms.CheckBox();
            this._lblMessageMakereadyRate = new System.Windows.Forms.Label();
            this._txtMessageRate = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblMessageRate = new System.Windows.Forms.Label();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnNew = new System.Windows.Forms.ToolStripButton();
            this._btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._btnRefresh = new System.Windows.Forms.ToolStripButton();
            this._btnDelete = new System.Windows.Forms.ToolStripButton();
            this._btnUpload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._btnPrint = new System.Windows.Forms.ToolStripButton();
            this._btnHome = new System.Windows.Forms.ToolStripButton();
            this._cboMakereadyRate = new System.Windows.Forms.ComboBox();
            this._lblTotal = new System.Windows.Forms.Label();
            this._menuColumnHeader = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._menuRemoveEstimate = new System.Windows.Forms.ToolStripMenuItem();
            this._toolTipProvider = new System.Windows.Forms.ToolTip(this.components);
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.est_polybagTableAdapter = new CatalogEstimating.Datasets.PolybagGroupTableAdapters.est_polybagTableAdapter();
            this.AllocateByPercent = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PostalScenario = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Quantity = new CatalogEstimating.CustomControls.IntegerColumn();
            this.PolybagWeight = new CatalogEstimating.CustomControls.DecimalColumn();
            this.estpolybagidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estpolybaggroupidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifieddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.printersPrinterIDandDescriptionByRunDateBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dsPolybagGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridPolybag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.estpolybagBindingSource)).BeginInit();
            this._groupMessages.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this._menuColumnHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // _cboBagRate
            // 
            this._cboBagRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboBagRate.FormattingEnabled = true;
            this._cboBagRate.Location = new System.Drawing.Point(461, 38);
            this._cboBagRate.Name = "_cboBagRate";
            this._cboBagRate.Size = new System.Drawing.Size(248, 21);
            this._cboBagRate.TabIndex = 7;
            this._cboBagRate.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblBagRate
            // 
            this._lblBagRate.AutoSize = true;
            this._lblBagRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblBagRate.Location = new System.Drawing.Point(386, 41);
            this._lblBagRate.Name = "_lblBagRate";
            this._lblBagRate.Size = new System.Drawing.Size(69, 13);
            this._lblBagRate.TabIndex = 6;
            this._lblBagRate.Text = "Bag Rate *";
            // 
            // _cboPrinterVendor
            // 
            this._cboPrinterVendor.DataSource = this.printersPrinterIDandDescriptionByRunDateBindingSource;
            this._cboPrinterVendor.DisplayMember = "Description";
            this._cboPrinterVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboPrinterVendor.FormattingEnabled = true;
            this._cboPrinterVendor.Location = new System.Drawing.Point(461, 9);
            this._cboPrinterVendor.Name = "_cboPrinterVendor";
            this._cboPrinterVendor.Size = new System.Drawing.Size(248, 21);
            this._cboPrinterVendor.TabIndex = 5;
            this._cboPrinterVendor.ValueMember = "VND_Printer_ID";
            this._cboPrinterVendor.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            this._cboPrinterVendor.SelectedValueChanged += new System.EventHandler(this._cboPrinterVendor_SelectedValueChanged);
            // 
            // printersPrinterIDandDescriptionByRunDateBindingSource
            // 
            this.printersPrinterIDandDescriptionByRunDateBindingSource.DataMember = "Printer_s_PrinterIDandDescription_ByRunDate";
            this.printersPrinterIDandDescriptionByRunDateBindingSource.DataSource = this._dsPolybagGroup;
            this.printersPrinterIDandDescriptionByRunDateBindingSource.Sort = "Description ASC";
            // 
            // _dsPolybagGroup
            // 
            this._dsPolybagGroup.DataSetName = "PolybagGroup";
            this._dsPolybagGroup.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _lblPrinterVendor
            // 
            this._lblPrinterVendor.AutoSize = true;
            this._lblPrinterVendor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPrinterVendor.Location = new System.Drawing.Point(358, 12);
            this._lblPrinterVendor.Name = "_lblPrinterVendor";
            this._lblPrinterVendor.Size = new System.Drawing.Size(97, 13);
            this._lblPrinterVendor.TabIndex = 4;
            this._lblPrinterVendor.Text = "Printer Vendor *";
            // 
            // _txtComments
            // 
            this._txtComments.Location = new System.Drawing.Point(98, 41);
            this._txtComments.MaxLength = 255;
            this._txtComments.Multiline = true;
            this._txtComments.Name = "_txtComments";
            this._txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtComments.Size = new System.Drawing.Size(233, 108);
            this._txtComments.TabIndex = 3;
            this._txtComments.Validated += new System.EventHandler(this._txtComments_Validated);
            this._txtComments.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblComments
            // 
            this._lblComments.AutoSize = true;
            this._lblComments.Location = new System.Drawing.Point(12, 41);
            this._lblComments.Name = "_lblComments";
            this._lblComments.Size = new System.Drawing.Size(56, 13);
            this._lblComments.TabIndex = 2;
            this._lblComments.Text = "Comments";
            // 
            // _txtDescription
            // 
            this._txtDescription.Location = new System.Drawing.Point(98, 9);
            this._txtDescription.MaxLength = 35;
            this._txtDescription.Name = "_txtDescription";
            this._txtDescription.Size = new System.Drawing.Size(233, 20);
            this._txtDescription.TabIndex = 1;
            this._txtDescription.Validated += new System.EventHandler(this._txtDescription_Validated);
            this._txtDescription.Validating += new System.ComponentModel.CancelEventHandler(this._txtDescription_Validating);
            this._txtDescription.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblDescription
            // 
            this._lblDescription.AutoSize = true;
            this._lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblDescription.Location = new System.Drawing.Point(12, 12);
            this._lblDescription.Name = "_lblDescription";
            this._lblDescription.Size = new System.Drawing.Size(80, 13);
            this._lblDescription.TabIndex = 0;
            this._lblDescription.Text = "Description *";
            // 
            // _lblMakereadyRate
            // 
            this._lblMakereadyRate.AutoSize = true;
            this._lblMakereadyRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblMakereadyRate.Location = new System.Drawing.Point(346, 69);
            this._lblMakereadyRate.Name = "_lblMakereadyRate";
            this._lblMakereadyRate.Size = new System.Drawing.Size(109, 13);
            this._lblMakereadyRate.TabIndex = 8;
            this._lblMakereadyRate.Text = "Makeready Rate *";
            // 
            // _gridPolybag
            // 
            this._gridPolybag.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._gridPolybag.AutoGenerateColumns = false;
            this._gridPolybag.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridPolybag.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._gridPolybag.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridPolybag.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AllocateByPercent,
            this.PostalScenario,
            this.Quantity,
            this.PolybagWeight,
            this.estpolybagidDataGridViewTextBoxColumn,
            this.estpolybaggroupidDataGridViewTextBoxColumn,
            this.createdbyDataGridViewTextBoxColumn,
            this.createddateDataGridViewTextBoxColumn,
            this.modifiedbyDataGridViewTextBoxColumn,
            this.modifieddateDataGridViewTextBoxColumn});
            this._gridPolybag.DataSource = this.estpolybagBindingSource;
            this._gridPolybag.Location = new System.Drawing.Point(6, 223);
            this._gridPolybag.Name = "_gridPolybag";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridPolybag.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this._gridPolybag.Size = new System.Drawing.Size(703, 140);
            this._gridPolybag.TabIndex = 11;
            this._gridPolybag.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this._gridPolybag_CellBeginEdit);
            this._gridPolybag.KeyDown += new System.Windows.Forms.KeyEventHandler(this._gridPolybag_KeyDown);
            this._gridPolybag.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this._gridPolybag_RowValidating);
            this._gridPolybag.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridPolybag_RowValidated);
            this._gridPolybag.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this._gridPolybag_DefaultValuesNeeded);
            this._gridPolybag.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridPolybag_CellValueChanged);
            this._gridPolybag.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this._gridPolybag_DataError);
            this._gridPolybag.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this._gridPolybag_ColumnWidthChanged);
            // 
            // postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource
            // 
            this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource.DataMember = "PostalScenario_s_PostalScenarioIDandDescription_ByRunDate";
            this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource.DataSource = this._dsPolybagGroup;
            this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource.Sort = "description ASC";
            // 
            // estpolybagBindingSource
            // 
            this.estpolybagBindingSource.DataMember = "est_polybag";
            this.estpolybagBindingSource.DataSource = this._dsPolybagGroup;
            // 
            // _txtTotal
            // 
            this._txtTotal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._txtTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._txtTotal.FlashColor = System.Drawing.Color.Red;
            this._txtTotal.Location = new System.Drawing.Point(646, 376);
            this._txtTotal.Name = "_txtTotal";
            this._txtTotal.ReadOnly = true;
            this._txtTotal.Size = new System.Drawing.Size(60, 20);
            this._txtTotal.TabIndex = 30;
            this._txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtTotal.Value = null;
            // 
            // _groupMessages
            // 
            this._groupMessages.Controls.Add(this._txtMessageMakereadyRate);
            this._groupMessages.Controls.Add(this._chkUseMessage);
            this._groupMessages.Controls.Add(this._lblMessageMakereadyRate);
            this._groupMessages.Controls.Add(this._txtMessageRate);
            this._groupMessages.Controls.Add(this._lblMessageRate);
            this._groupMessages.Location = new System.Drawing.Point(418, 93);
            this._groupMessages.Name = "_groupMessages";
            this._groupMessages.Size = new System.Drawing.Size(291, 96);
            this._groupMessages.TabIndex = 10;
            this._groupMessages.TabStop = false;
            this._groupMessages.Text = "Messages";
            // 
            // _txtMessageMakereadyRate
            // 
            this._txtMessageMakereadyRate.Enabled = false;
            this._txtMessageMakereadyRate.FlashColor = System.Drawing.Color.Red;
            this._txtMessageMakereadyRate.Location = new System.Drawing.Point(161, 66);
            this._txtMessageMakereadyRate.Name = "_txtMessageMakereadyRate";
            this._txtMessageMakereadyRate.ReadOnly = true;
            this._txtMessageMakereadyRate.Size = new System.Drawing.Size(121, 20);
            this._txtMessageMakereadyRate.TabIndex = 4;
            this._txtMessageMakereadyRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtMessageMakereadyRate.Value = null;
            // 
            // _chkUseMessage
            // 
            this._chkUseMessage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkUseMessage.Checked = true;
            this._chkUseMessage.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkUseMessage.Location = new System.Drawing.Point(4, 10);
            this._chkUseMessage.Name = "_chkUseMessage";
            this._chkUseMessage.Size = new System.Drawing.Size(171, 24);
            this._chkUseMessage.TabIndex = 0;
            this._chkUseMessage.Text = "Use Inkjet Message on Bag";
            this._chkUseMessage.UseVisualStyleBackColor = true;
            this._chkUseMessage.CheckedChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblMessageMakereadyRate
            // 
            this._lblMessageMakereadyRate.AutoSize = true;
            this._lblMessageMakereadyRate.Location = new System.Drawing.Point(10, 69);
            this._lblMessageMakereadyRate.Name = "_lblMessageMakereadyRate";
            this._lblMessageMakereadyRate.Size = new System.Drawing.Size(132, 13);
            this._lblMessageMakereadyRate.TabIndex = 3;
            this._lblMessageMakereadyRate.Text = "Message Makeready Rate";
            // 
            // _txtMessageRate
            // 
            this._txtMessageRate.Enabled = false;
            this._txtMessageRate.FlashColor = System.Drawing.Color.Red;
            this._txtMessageRate.Location = new System.Drawing.Point(161, 40);
            this._txtMessageRate.Name = "_txtMessageRate";
            this._txtMessageRate.ReadOnly = true;
            this._txtMessageRate.Size = new System.Drawing.Size(121, 20);
            this._txtMessageRate.TabIndex = 2;
            this._txtMessageRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtMessageRate.Value = null;
            // 
            // _lblMessageRate
            // 
            this._lblMessageRate.AutoSize = true;
            this._lblMessageRate.Location = new System.Drawing.Point(66, 43);
            this._lblMessageRate.Name = "_lblMessageRate";
            this._lblMessageRate.Size = new System.Drawing.Size(76, 13);
            this._lblMessageRate.TabIndex = 1;
            this._lblMessageRate.Text = "Message Rate";
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnNew,
            this._btnSave,
            this.toolStripSeparator2,
            this._btnRefresh,
            this._btnDelete,
            this._btnUpload,
            this.toolStripSeparator1,
            this._btnPrint,
            this._btnHome});
            this._toolStrip.Location = new System.Drawing.Point(0, 371);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(185, 25);
            this._toolStrip.TabIndex = 35;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _btnNew
            // 
            this._btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnNew.Image = global::CatalogEstimating.Properties.Resources.NewPolybag;
            this._btnNew.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnNew.Name = "_btnNew";
            this._btnNew.Size = new System.Drawing.Size(23, 22);
            this._btnNew.Text = "New";
            this._btnNew.Click += new System.EventHandler(this._btnNew_Click);
            // 
            // _btnSave
            // 
            this._btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnSave.Image = global::CatalogEstimating.Properties.Resources.Save;
            this._btnSave.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size(23, 22);
            this._btnSave.Text = "Save";
            this._btnSave.Click += new System.EventHandler(this._btnSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // _btnDelete
            // 
            this._btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnDelete.Image = global::CatalogEstimating.Properties.Resources.Delete;
            this._btnDelete.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Size = new System.Drawing.Size(23, 22);
            this._btnDelete.Text = "Delete";
            this._btnDelete.Click += new System.EventHandler(this._btnDelete_Click);
            // 
            // _btnUpload
            // 
            this._btnUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnUpload.Image = global::CatalogEstimating.Properties.Resources.Upload;
            this._btnUpload.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnUpload.Name = "_btnUpload";
            this._btnUpload.Size = new System.Drawing.Size(23, 22);
            this._btnUpload.Text = "Upload";
            this._btnUpload.Click += new System.EventHandler(this._btnUpload_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            // _btnHome
            // 
            this._btnHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnHome.Image = global::CatalogEstimating.Properties.Resources.Home;
            this._btnHome.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnHome.Name = "_btnHome";
            this._btnHome.Size = new System.Drawing.Size(23, 22);
            this._btnHome.Text = "Home";
            this._btnHome.Click += new System.EventHandler(this._btnHome_Click);
            // 
            // _cboMakereadyRate
            // 
            this._cboMakereadyRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboMakereadyRate.FormattingEnabled = true;
            this._cboMakereadyRate.Location = new System.Drawing.Point(461, 66);
            this._cboMakereadyRate.Name = "_cboMakereadyRate";
            this._cboMakereadyRate.Size = new System.Drawing.Size(248, 21);
            this._cboMakereadyRate.TabIndex = 9;
            this._cboMakereadyRate.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // _lblTotal
            // 
            this._lblTotal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._lblTotal.AutoSize = true;
            this._lblTotal.Location = new System.Drawing.Point(609, 378);
            this._lblTotal.Name = "_lblTotal";
            this._lblTotal.Size = new System.Drawing.Size(31, 13);
            this._lblTotal.TabIndex = 37;
            this._lblTotal.Text = "Total";
            // 
            // _menuColumnHeader
            // 
            this._menuColumnHeader.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuRemoveEstimate});
            this._menuColumnHeader.Name = "_menuColumnHeader";
            this._menuColumnHeader.Size = new System.Drawing.Size(169, 26);
            // 
            // _menuRemoveEstimate
            // 
            this._menuRemoveEstimate.Name = "_menuRemoveEstimate";
            this._menuRemoveEstimate.Size = new System.Drawing.Size(168, 22);
            this._menuRemoveEstimate.Text = "&Remove Estimate";
            this._menuRemoveEstimate.Click += new System.EventHandler(this._menuRemoveEstimate_Click);
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // est_polybagTableAdapter
            // 
            this.est_polybagTableAdapter.ClearBeforeFill = true;
            // 
            // AllocateByPercent
            // 
            this.AllocateByPercent.FillWeight = 40F;
            this.AllocateByPercent.HeaderText = "Allocate By %";
            this.AllocateByPercent.IndeterminateValue = "false";
            this.AllocateByPercent.Name = "AllocateByPercent";
            // 
            // PostalScenario
            // 
            this.PostalScenario.DataPropertyName = "pst_postalscenario_id";
            this.PostalScenario.DataSource = this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource;
            this.PostalScenario.DisplayMember = "description";
            this.PostalScenario.HeaderText = "Postal Scenario";
            this.PostalScenario.Name = "PostalScenario";
            this.PostalScenario.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.PostalScenario.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.PostalScenario.ValueMember = "pst_postalscenario_id";
            // 
            // Quantity
            // 
            this.Quantity.DataPropertyName = "quantity";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle2;
            this.Quantity.FillWeight = 50F;
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.MaxInputLength = 32767;
            this.Quantity.Name = "Quantity";
            // 
            // PolybagWeight
            // 
            this.PolybagWeight.AllowNegative = true;
            this.PolybagWeight.DataPropertyName = "PolybagWeight";
            this.PolybagWeight.DecimalPrecision = 4;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Format = "N4";
            dataGridViewCellStyle3.NullValue = null;
            this.PolybagWeight.DefaultCellStyle = dataGridViewCellStyle3;
            this.PolybagWeight.FillWeight = 50F;
            this.PolybagWeight.HeaderText = "Polybag Weight";
            this.PolybagWeight.MaxInputLength = 32767;
            this.PolybagWeight.Name = "PolybagWeight";
            this.PolybagWeight.ReadOnly = true;
            // 
            // estpolybagidDataGridViewTextBoxColumn
            // 
            this.estpolybagidDataGridViewTextBoxColumn.DataPropertyName = "est_polybag_id";
            this.estpolybagidDataGridViewTextBoxColumn.HeaderText = "est_polybag_id";
            this.estpolybagidDataGridViewTextBoxColumn.Name = "estpolybagidDataGridViewTextBoxColumn";
            this.estpolybagidDataGridViewTextBoxColumn.ReadOnly = true;
            this.estpolybagidDataGridViewTextBoxColumn.Visible = false;
            // 
            // estpolybaggroupidDataGridViewTextBoxColumn
            // 
            this.estpolybaggroupidDataGridViewTextBoxColumn.DataPropertyName = "est_polybaggroup_id";
            this.estpolybaggroupidDataGridViewTextBoxColumn.HeaderText = "est_polybaggroup_id";
            this.estpolybaggroupidDataGridViewTextBoxColumn.Name = "estpolybaggroupidDataGridViewTextBoxColumn";
            this.estpolybaggroupidDataGridViewTextBoxColumn.ReadOnly = true;
            this.estpolybaggroupidDataGridViewTextBoxColumn.Visible = false;
            // 
            // createdbyDataGridViewTextBoxColumn
            // 
            this.createdbyDataGridViewTextBoxColumn.DataPropertyName = "createdby";
            this.createdbyDataGridViewTextBoxColumn.HeaderText = "createdby";
            this.createdbyDataGridViewTextBoxColumn.Name = "createdbyDataGridViewTextBoxColumn";
            this.createdbyDataGridViewTextBoxColumn.Visible = false;
            // 
            // createddateDataGridViewTextBoxColumn
            // 
            this.createddateDataGridViewTextBoxColumn.DataPropertyName = "createddate";
            this.createddateDataGridViewTextBoxColumn.HeaderText = "createddate";
            this.createddateDataGridViewTextBoxColumn.Name = "createddateDataGridViewTextBoxColumn";
            this.createddateDataGridViewTextBoxColumn.Visible = false;
            // 
            // modifiedbyDataGridViewTextBoxColumn
            // 
            this.modifiedbyDataGridViewTextBoxColumn.DataPropertyName = "modifiedby";
            this.modifiedbyDataGridViewTextBoxColumn.HeaderText = "modifiedby";
            this.modifiedbyDataGridViewTextBoxColumn.Name = "modifiedbyDataGridViewTextBoxColumn";
            this.modifiedbyDataGridViewTextBoxColumn.Visible = false;
            // 
            // modifieddateDataGridViewTextBoxColumn
            // 
            this.modifieddateDataGridViewTextBoxColumn.DataPropertyName = "modifieddate";
            this.modifieddateDataGridViewTextBoxColumn.HeaderText = "modifieddate";
            this.modifieddateDataGridViewTextBoxColumn.Name = "modifieddateDataGridViewTextBoxColumn";
            this.modifieddateDataGridViewTextBoxColumn.Visible = false;
            // 
            // ucpPolybag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this._lblTotal);
            this.Controls.Add(this._toolStrip);
            this.Controls.Add(this._txtTotal);
            this.Controls.Add(this._gridPolybag);
            this.Controls.Add(this._cboMakereadyRate);
            this.Controls.Add(this._lblComments);
            this.Controls.Add(this._groupMessages);
            this.Controls.Add(this._txtComments);
            this.Controls.Add(this._cboBagRate);
            this.Controls.Add(this._lblMakereadyRate);
            this.Controls.Add(this._lblBagRate);
            this.Controls.Add(this._cboPrinterVendor);
            this.Controls.Add(this._lblDescription);
            this.Controls.Add(this._lblPrinterVendor);
            this.Controls.Add(this._txtDescription);
            this.Name = "ucpPolybag";
            this.Size = new System.Drawing.Size(717, 404);
            this.Validating += new System.ComponentModel.CancelEventHandler(this.ucpPolybag_Validating);
            this.Load += new System.EventHandler(this.ucpPolybag_Load);
            this.Resize += new System.EventHandler(this.ucpPolybag_Resize);
            this.Validated += new System.EventHandler(this.ucpPolybag_Validated);
            ((System.ComponentModel.ISupportInitialize)(this.printersPrinterIDandDescriptionByRunDateBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dsPolybagGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridPolybag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.estpolybagBindingSource)).EndInit();
            this._groupMessages.ResumeLayout(false);
            this._groupMessages.PerformLayout();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this._menuColumnHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox _cboBagRate;
        private System.Windows.Forms.Label _lblBagRate;
        private System.Windows.Forms.ComboBox _cboPrinterVendor;
        private System.Windows.Forms.Label _lblPrinterVendor;
        private System.Windows.Forms.TextBox _txtComments;
        private System.Windows.Forms.Label _lblComments;
        private System.Windows.Forms.TextBox _txtDescription;
        private System.Windows.Forms.Label _lblDescription;
        private System.Windows.Forms.Label _lblMakereadyRate;
        private System.Windows.Forms.DataGridView _gridPolybag;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtTotal;
        private System.Windows.Forms.GroupBox _groupMessages;
        private System.Windows.Forms.CheckBox _chkUseMessage;
        private System.Windows.Forms.Label _lblMessageMakereadyRate;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtMessageRate;
        private System.Windows.Forms.Label _lblMessageRate;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton _btnDelete;
        private System.Windows.Forms.ToolStripButton _btnUpload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton _btnPrint;
        private System.Windows.Forms.ToolStripButton _btnHome;
        private System.Windows.Forms.ToolStripButton _btnNew;
        private System.Windows.Forms.ToolStripButton _btnRefresh;
        private System.Windows.Forms.ComboBox _cboMakereadyRate;
        private System.Windows.Forms.Label _lblTotal;
        private System.Windows.Forms.ContextMenuStrip _menuColumnHeader;
        private System.Windows.Forms.ToolStripMenuItem _menuRemoveEstimate;
        private System.Windows.Forms.ToolTip _toolTipProvider;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.BindingSource printersPrinterIDandDescriptionByRunDateBindingSource;
        private CatalogEstimating.Datasets.PolybagGroup _dsPolybagGroup;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtMessageMakereadyRate;
        private System.Windows.Forms.BindingSource postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource;
        private System.Windows.Forms.BindingSource estpolybagBindingSource;
        private CatalogEstimating.Datasets.PolybagGroupTableAdapters.est_polybagTableAdapter est_polybagTableAdapter;
        private System.Windows.Forms.DataGridViewCheckBoxColumn AllocateByPercent;
        private System.Windows.Forms.DataGridViewComboBoxColumn PostalScenario;
        private CatalogEstimating.CustomControls.IntegerColumn Quantity;
        private CatalogEstimating.CustomControls.DecimalColumn PolybagWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn estpolybagidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn estpolybaggroupidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createddateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifieddateDataGridViewTextBoxColumn;
    }
}
