namespace CatalogEstimating.UserControls.Admin
{
    partial class ucpAdminVendorsSetup
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
            this._gridVendors = new System.Windows.Forms.DataGridView();
            this.vendorcodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vndvendoridDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.createdbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifieddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vndvendorBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._dsAdministration = new CatalogEstimating.Datasets.Administration();
            this._groupSetup = new System.Windows.Forms.GroupBox();
            this._btnUpdate = new System.Windows.Forms.Button();
            this._chkInsertFreight = new System.Windows.Forms.CheckBox();
            this._chkVendorSupplied = new System.Windows.Forms.CheckBox();
            this._chkPostal = new System.Windows.Forms.CheckBox();
            this._chkMailTracker = new System.Windows.Forms.CheckBox();
            this._chkMailList = new System.Windows.Forms.CheckBox();
            this._chkMailHouse = new System.Windows.Forms.CheckBox();
            this._chkSeparator = new System.Windows.Forms.CheckBox();
            this._chkCreative = new System.Windows.Forms.CheckBox();
            this._chkPaper = new System.Windows.Forms.CheckBox();
            this._chkPrinter = new System.Windows.Forms.CheckBox();
            this._chkActive = new System.Windows.Forms.CheckBox();
            this._txtVendorID = new System.Windows.Forms.TextBox();
            this._txtVendorName = new System.Windows.Forms.TextBox();
            this._lblVendorID = new System.Windows.Forms.Label();
            this._lblVendorName = new System.Windows.Forms.Label();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnNew = new System.Windows.Forms.ToolStripButton();
            this._btnCancel = new System.Windows.Forms.ToolStripButton();
            this.vnd_vendorTableAdapter = new CatalogEstimating.Datasets.AdministrationTableAdapters.vnd_vendorTableAdapter();
            this._Errors = new System.Windows.Forms.ErrorProvider( this.components );
            ( (System.ComponentModel.ISupportInitialize)( this._gridVendors ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this.vndvendorBindingSource ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this._dsAdministration ) ).BeginInit();
            this._groupSetup.SuspendLayout();
            this._toolStrip.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._Errors ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _gridVendors
            // 
            this._gridVendors.AllowUserToAddRows = false;
            this._gridVendors.AllowUserToDeleteRows = false;
            this._gridVendors.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._gridVendors.AutoGenerateColumns = false;
            this._gridVendors.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridVendors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._gridVendors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridVendors.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.vendorcodeDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.vndvendoridDataGridViewTextBoxColumn,
            this.Type,
            this.activeDataGridViewCheckBoxColumn,
            this.createdbyDataGridViewTextBoxColumn,
            this.createddateDataGridViewTextBoxColumn,
            this.modifiedbyDataGridViewTextBoxColumn,
            this.modifieddateDataGridViewTextBoxColumn} );
            this._gridVendors.DataSource = this.vndvendorBindingSource;
            this._gridVendors.Location = new System.Drawing.Point( 12, 12 );
            this._gridVendors.MultiSelect = false;
            this._gridVendors.Name = "_gridVendors";
            this._gridVendors.ReadOnly = true;
            this._gridVendors.RowHeadersVisible = false;
            this._gridVendors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridVendors.Size = new System.Drawing.Size( 584, 181 );
            this._gridVendors.TabIndex = 0;
            this._gridVendors.SelectionChanged += new System.EventHandler( this._gridVendors_SelectionChanged );
            // 
            // vendorcodeDataGridViewTextBoxColumn
            // 
            this.vendorcodeDataGridViewTextBoxColumn.DataPropertyName = "vendorcode";
            this.vendorcodeDataGridViewTextBoxColumn.HeaderText = "vendorcode";
            this.vendorcodeDataGridViewTextBoxColumn.Name = "vendorcodeDataGridViewTextBoxColumn";
            this.vendorcodeDataGridViewTextBoxColumn.ReadOnly = true;
            this.vendorcodeDataGridViewTextBoxColumn.Visible = false;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Vendor";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // vndvendoridDataGridViewTextBoxColumn
            // 
            this.vndvendoridDataGridViewTextBoxColumn.DataPropertyName = "vnd_vendor_id";
            this.vndvendoridDataGridViewTextBoxColumn.HeaderText = "vnd_vendor_id";
            this.vndvendoridDataGridViewTextBoxColumn.Name = "vndvendoridDataGridViewTextBoxColumn";
            this.vndvendoridDataGridViewTextBoxColumn.ReadOnly = true;
            this.vndvendoridDataGridViewTextBoxColumn.Visible = false;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // activeDataGridViewCheckBoxColumn
            // 
            this.activeDataGridViewCheckBoxColumn.DataPropertyName = "active";
            this.activeDataGridViewCheckBoxColumn.FillWeight = 50F;
            this.activeDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.activeDataGridViewCheckBoxColumn.Name = "activeDataGridViewCheckBoxColumn";
            this.activeDataGridViewCheckBoxColumn.ReadOnly = true;
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
            // vndvendorBindingSource
            // 
            this.vndvendorBindingSource.DataMember = "vnd_vendor";
            this.vndvendorBindingSource.DataSource = this._dsAdministration;
            this.vndvendorBindingSource.Sort = "active DESC, description ASC";
            // 
            // _dsAdministration
            // 
            this._dsAdministration.DataSetName = "Administration";
            this._dsAdministration.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _groupSetup
            // 
            this._groupSetup.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._groupSetup.Controls.Add( this._btnUpdate );
            this._groupSetup.Controls.Add( this._chkInsertFreight );
            this._groupSetup.Controls.Add( this._chkVendorSupplied );
            this._groupSetup.Controls.Add( this._chkPostal );
            this._groupSetup.Controls.Add( this._chkMailTracker );
            this._groupSetup.Controls.Add( this._chkMailList );
            this._groupSetup.Controls.Add( this._chkMailHouse );
            this._groupSetup.Controls.Add( this._chkSeparator );
            this._groupSetup.Controls.Add( this._chkCreative );
            this._groupSetup.Controls.Add( this._chkPaper );
            this._groupSetup.Controls.Add( this._chkPrinter );
            this._groupSetup.Controls.Add( this._chkActive );
            this._groupSetup.Controls.Add( this._txtVendorID );
            this._groupSetup.Controls.Add( this._txtVendorName );
            this._groupSetup.Controls.Add( this._lblVendorID );
            this._groupSetup.Controls.Add( this._lblVendorName );
            this._groupSetup.Location = new System.Drawing.Point( 12, 212 );
            this._groupSetup.Name = "_groupSetup";
            this._groupSetup.Size = new System.Drawing.Size( 584, 216 );
            this._groupSetup.TabIndex = 1;
            this._groupSetup.TabStop = false;
            this._groupSetup.Text = "Vendor Setup";
            // 
            // _btnUpdate
            // 
            this._btnUpdate.CausesValidation = false;
            this._btnUpdate.Location = new System.Drawing.Point( 279, 169 );
            this._btnUpdate.Name = "_btnUpdate";
            this._btnUpdate.Size = new System.Drawing.Size( 80, 29 );
            this._btnUpdate.TabIndex = 15;
            this._btnUpdate.Text = "&Update";
            this._btnUpdate.UseVisualStyleBackColor = true;
            this._btnUpdate.Click += new System.EventHandler( this._btnUpdate_Click );
            // 
            // _chkInsertFreight
            // 
            this._chkInsertFreight.AutoSize = true;
            this._chkInsertFreight.Location = new System.Drawing.Point( 156, 181 );
            this._chkInsertFreight.Name = "_chkInsertFreight";
            this._chkInsertFreight.Size = new System.Drawing.Size( 87, 17 );
            this._chkInsertFreight.TabIndex = 14;
            this._chkInsertFreight.Tag = "10";
            this._chkInsertFreight.Text = "Insert Freight";
            this._chkInsertFreight.UseVisualStyleBackColor = true;
            this._chkInsertFreight.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkVendorSupplied
            // 
            this._chkVendorSupplied.AutoSize = true;
            this._chkVendorSupplied.Location = new System.Drawing.Point( 156, 158 );
            this._chkVendorSupplied.Name = "_chkVendorSupplied";
            this._chkVendorSupplied.Size = new System.Drawing.Size( 104, 17 );
            this._chkVendorSupplied.TabIndex = 13;
            this._chkVendorSupplied.Tag = "9";
            this._chkVendorSupplied.Text = "Vendor Supplied";
            this._chkVendorSupplied.UseVisualStyleBackColor = true;
            this._chkVendorSupplied.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkPostal
            // 
            this._chkPostal.AutoSize = true;
            this._chkPostal.Location = new System.Drawing.Point( 156, 135 );
            this._chkPostal.Name = "_chkPostal";
            this._chkPostal.Size = new System.Drawing.Size( 55, 17 );
            this._chkPostal.TabIndex = 12;
            this._chkPostal.Tag = "8";
            this._chkPostal.Text = "Postal";
            this._chkPostal.UseVisualStyleBackColor = true;
            this._chkPostal.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkMailTracker
            // 
            this._chkMailTracker.AutoSize = true;
            this._chkMailTracker.Location = new System.Drawing.Point( 156, 112 );
            this._chkMailTracker.Name = "_chkMailTracker";
            this._chkMailTracker.Size = new System.Drawing.Size( 85, 17 );
            this._chkMailTracker.TabIndex = 11;
            this._chkMailTracker.Tag = "7";
            this._chkMailTracker.Text = "Mail Tracker";
            this._chkMailTracker.UseVisualStyleBackColor = true;
            this._chkMailTracker.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkMailList
            // 
            this._chkMailList.AutoSize = true;
            this._chkMailList.Location = new System.Drawing.Point( 156, 88 );
            this._chkMailList.Name = "_chkMailList";
            this._chkMailList.Size = new System.Drawing.Size( 113, 17 );
            this._chkMailList.TabIndex = 10;
            this._chkMailList.Tag = "6";
            this._chkMailList.Text = "Mail List Resource";
            this._chkMailList.UseVisualStyleBackColor = true;
            this._chkMailList.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkMailHouse
            // 
            this._chkMailHouse.AutoSize = true;
            this._chkMailHouse.Location = new System.Drawing.Point( 18, 181 );
            this._chkMailHouse.Name = "_chkMailHouse";
            this._chkMailHouse.Size = new System.Drawing.Size( 79, 17 );
            this._chkMailHouse.TabIndex = 9;
            this._chkMailHouse.Tag = "5";
            this._chkMailHouse.Text = "Mail House";
            this._chkMailHouse.UseVisualStyleBackColor = true;
            this._chkMailHouse.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkSeparator
            // 
            this._chkSeparator.AutoSize = true;
            this._chkSeparator.Location = new System.Drawing.Point( 18, 158 );
            this._chkSeparator.Name = "_chkSeparator";
            this._chkSeparator.Size = new System.Drawing.Size( 72, 17 );
            this._chkSeparator.TabIndex = 8;
            this._chkSeparator.Tag = "4";
            this._chkSeparator.Text = "Separator";
            this._chkSeparator.UseVisualStyleBackColor = true;
            this._chkSeparator.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkCreative
            // 
            this._chkCreative.AutoSize = true;
            this._chkCreative.Location = new System.Drawing.Point( 18, 135 );
            this._chkCreative.Name = "_chkCreative";
            this._chkCreative.Size = new System.Drawing.Size( 130, 17 );
            this._chkCreative.TabIndex = 7;
            this._chkCreative.Tag = "3";
            this._chkCreative.Text = "Creative/Photography";
            this._chkCreative.UseVisualStyleBackColor = true;
            this._chkCreative.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkPaper
            // 
            this._chkPaper.AutoSize = true;
            this._chkPaper.Location = new System.Drawing.Point( 18, 112 );
            this._chkPaper.Name = "_chkPaper";
            this._chkPaper.Size = new System.Drawing.Size( 54, 17 );
            this._chkPaper.TabIndex = 6;
            this._chkPaper.Tag = "2";
            this._chkPaper.Text = "Paper";
            this._chkPaper.UseVisualStyleBackColor = true;
            this._chkPaper.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkPrinter
            // 
            this._chkPrinter.AutoSize = true;
            this._chkPrinter.Location = new System.Drawing.Point( 18, 88 );
            this._chkPrinter.Name = "_chkPrinter";
            this._chkPrinter.Size = new System.Drawing.Size( 56, 17 );
            this._chkPrinter.TabIndex = 5;
            this._chkPrinter.Tag = "1";
            this._chkPrinter.Text = "Printer";
            this._chkPrinter.UseVisualStyleBackColor = true;
            this._chkPrinter.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _chkActive
            // 
            this._chkActive.AutoSize = true;
            this._chkActive.Location = new System.Drawing.Point( 299, 25 );
            this._chkActive.Name = "_chkActive";
            this._chkActive.Size = new System.Drawing.Size( 56, 17 );
            this._chkActive.TabIndex = 4;
            this._chkActive.Text = "Active";
            this._chkActive.UseVisualStyleBackColor = true;
            this._chkActive.CheckedChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _txtVendorID
            // 
            this._txtVendorID.Location = new System.Drawing.Point( 113, 48 );
            this._txtVendorID.MaxLength = 10;
            this._txtVendorID.Name = "_txtVendorID";
            this._txtVendorID.Size = new System.Drawing.Size( 157, 20 );
            this._txtVendorID.TabIndex = 3;
            this._txtVendorID.Validated += new System.EventHandler( this._txtVendorID_Validated );
            this._txtVendorID.Validating += new System.ComponentModel.CancelEventHandler( this._txtVendorID_Validating );
            this._txtVendorID.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _txtVendorName
            // 
            this._txtVendorName.Location = new System.Drawing.Point( 113, 22 );
            this._txtVendorName.MaxLength = 35;
            this._txtVendorName.Name = "_txtVendorName";
            this._txtVendorName.Size = new System.Drawing.Size( 157, 20 );
            this._txtVendorName.TabIndex = 1;
            this._txtVendorName.Validated += new System.EventHandler( this._txtVendorName_Validated );
            this._txtVendorName.Validating += new System.ComponentModel.CancelEventHandler( this._txtVendorName_Validating );
            this._txtVendorName.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _lblVendorID
            // 
            this._lblVendorID.AutoSize = true;
            this._lblVendorID.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblVendorID.Location = new System.Drawing.Point( 15, 51 );
            this._lblVendorID.Name = "_lblVendorID";
            this._lblVendorID.Size = new System.Drawing.Size( 69, 13 );
            this._lblVendorID.TabIndex = 2;
            this._lblVendorID.Text = "Vendor ID*";
            // 
            // _lblVendorName
            // 
            this._lblVendorName.AutoSize = true;
            this._lblVendorName.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblVendorName.Location = new System.Drawing.Point( 15, 25 );
            this._lblVendorName.Name = "_lblVendorName";
            this._lblVendorName.Size = new System.Drawing.Size( 88, 13 );
            this._lblVendorName.TabIndex = 0;
            this._lblVendorName.Text = "Vendor Name*";
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._btnNew,
            this._btnCancel} );
            this._toolStrip.Location = new System.Drawing.Point( 0, 403 );
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size( 58, 25 );
            this._toolStrip.TabIndex = 4;
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
            // vnd_vendorTableAdapter
            // 
            this.vnd_vendorTableAdapter.ClearBeforeFill = true;
            // 
            // _Errors
            // 
            this._Errors.ContainerControl = this;
            // 
            // ucpAdminVendorsSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._toolStrip );
            this.Controls.Add( this._groupSetup );
            this.Controls.Add( this._gridVendors );
            this.Name = "ucpAdminVendorsSetup";
            this.Size = new System.Drawing.Size( 611, 443 );
            this.Load += new System.EventHandler( this.ucpAdminVendorsSetup_Load );
            ( (System.ComponentModel.ISupportInitialize)( this._gridVendors ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this.vndvendorBindingSource ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this._dsAdministration ) ).EndInit();
            this._groupSetup.ResumeLayout( false );
            this._groupSetup.PerformLayout();
            this._toolStrip.ResumeLayout( false );
            this._toolStrip.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._Errors ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView _gridVendors;
        private System.Windows.Forms.GroupBox _groupSetup;
        private System.Windows.Forms.Label _lblVendorID;
        private System.Windows.Forms.Label _lblVendorName;
        private System.Windows.Forms.TextBox _txtVendorID;
        private System.Windows.Forms.TextBox _txtVendorName;
        private System.Windows.Forms.CheckBox _chkActive;
        private System.Windows.Forms.CheckBox _chkInsertFreight;
        private System.Windows.Forms.CheckBox _chkVendorSupplied;
        private System.Windows.Forms.CheckBox _chkPostal;
        private System.Windows.Forms.CheckBox _chkMailTracker;
        private System.Windows.Forms.CheckBox _chkMailList;
        private System.Windows.Forms.CheckBox _chkMailHouse;
        private System.Windows.Forms.CheckBox _chkSeparator;
        private System.Windows.Forms.CheckBox _chkCreative;
        private System.Windows.Forms.CheckBox _chkPaper;
        private System.Windows.Forms.CheckBox _chkPrinter;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnNew;
        private System.Windows.Forms.ToolStripButton _btnCancel;
        private System.Windows.Forms.Button _btnUpdate;
        private System.Windows.Forms.BindingSource vndvendorBindingSource;
        private CatalogEstimating.Datasets.Administration _dsAdministration;
        private CatalogEstimating.Datasets.AdministrationTableAdapters.vnd_vendorTableAdapter vnd_vendorTableAdapter;
        private System.Windows.Forms.ErrorProvider _Errors;
        private System.Windows.Forms.DataGridViewTextBoxColumn vendorcodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vndvendoridDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createddateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifieddateDataGridViewTextBoxColumn;
    }
}