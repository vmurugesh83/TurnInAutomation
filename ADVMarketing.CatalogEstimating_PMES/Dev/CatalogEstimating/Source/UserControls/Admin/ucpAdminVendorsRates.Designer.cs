namespace CatalogEstimating.UserControls.Admin
{
    partial class ucpAdminVendorsRates
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
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnNew = new System.Windows.Forms.ToolStripButton();
            this._btnCancel = new System.Windows.Forms.ToolStripButton();
            this._btnDelete = new System.Windows.Forms.ToolStripButton();
            this._lblVendor = new System.Windows.Forms.Label();
            this._lblVendorRateType = new System.Windows.Forms.Label();
            this._lblEffectiveDate = new System.Windows.Forms.Label();
            this._cboVendor = new System.Windows.Forms.ComboBox();
            this._dsAdministration = new CatalogEstimating.Datasets.Administration();
            this._cboVendorRateType = new System.Windows.Forms.ComboBox();
            this._groupVendorRates = new System.Windows.Forms.GroupBox();
            this._lblInformational = new System.Windows.Forms.Label();
            this._panelRateControl = new System.Windows.Forms.Panel();
            this._btnUpdate = new System.Windows.Forms.Button();
            this._cboEffectiveDate = new System.Windows.Forms.ComboBox();
            this._toolStrip.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._dsAdministration ) ).BeginInit();
            this._groupVendorRates.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._btnNew,
            this._btnCancel,
            this._btnDelete} );
            this._toolStrip.Location = new System.Drawing.Point( 0, 452 );
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size( 79, 25 );
            this._toolStrip.TabIndex = 5;
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
            // _btnDelete
            // 
            this._btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnDelete.Image = global::CatalogEstimating.Properties.Resources.Delete;
            this._btnDelete.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnDelete.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this._btnDelete.MergeIndex = 3;
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Size = new System.Drawing.Size( 23, 22 );
            this._btnDelete.Text = "Delete Rate";
            this._btnDelete.Click += new System.EventHandler( this._btnDelete_Click );
            // 
            // _lblVendor
            // 
            this._lblVendor.AutoSize = true;
            this._lblVendor.Location = new System.Drawing.Point( 13, 12 );
            this._lblVendor.Name = "_lblVendor";
            this._lblVendor.Size = new System.Drawing.Size( 41, 13 );
            this._lblVendor.TabIndex = 0;
            this._lblVendor.Text = "Vendor";
            // 
            // _lblVendorRateType
            // 
            this._lblVendorRateType.AutoSize = true;
            this._lblVendorRateType.Location = new System.Drawing.Point( 13, 39 );
            this._lblVendorRateType.Name = "_lblVendorRateType";
            this._lblVendorRateType.Size = new System.Drawing.Size( 94, 13 );
            this._lblVendorRateType.TabIndex = 2;
            this._lblVendorRateType.Text = "Vendor Rate Type";
            // 
            // _lblEffectiveDate
            // 
            this._lblEffectiveDate.AutoSize = true;
            this._lblEffectiveDate.Location = new System.Drawing.Point( 13, 66 );
            this._lblEffectiveDate.Name = "_lblEffectiveDate";
            this._lblEffectiveDate.Size = new System.Drawing.Size( 75, 13 );
            this._lblEffectiveDate.TabIndex = 4;
            this._lblEffectiveDate.Text = "Effective Date";
            // 
            // _cboVendor
            // 
            this._cboVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboVendor.FormattingEnabled = true;
            this._cboVendor.Location = new System.Drawing.Point( 129, 9 );
            this._cboVendor.Name = "_cboVendor";
            this._cboVendor.Size = new System.Drawing.Size( 153, 21 );
            this._cboVendor.TabIndex = 1;
            this._cboVendor.SelectionChangeCommitted += new System.EventHandler( this._cboVendor_SelectedValueChanged );
            // 
            // _dsAdministration
            // 
            this._dsAdministration.DataSetName = "Administration";
            this._dsAdministration.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _cboVendorRateType
            // 
            this._cboVendorRateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboVendorRateType.Enabled = false;
            this._cboVendorRateType.FormattingEnabled = true;
            this._cboVendorRateType.Location = new System.Drawing.Point( 129, 36 );
            this._cboVendorRateType.Name = "_cboVendorRateType";
            this._cboVendorRateType.Size = new System.Drawing.Size( 153, 21 );
            this._cboVendorRateType.TabIndex = 3;
            this._cboVendorRateType.SelectionChangeCommitted += new System.EventHandler( this._cboVendorRateType_SelectedValueChanged );
            // 
            // _groupVendorRates
            // 
            this._groupVendorRates.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._groupVendorRates.Controls.Add( this._lblInformational );
            this._groupVendorRates.Controls.Add( this._panelRateControl );
            this._groupVendorRates.Controls.Add( this._btnUpdate );
            this._groupVendorRates.Enabled = false;
            this._groupVendorRates.Location = new System.Drawing.Point( 16, 97 );
            this._groupVendorRates.Name = "_groupVendorRates";
            this._groupVendorRates.Size = new System.Drawing.Size( 588, 364 );
            this._groupVendorRates.TabIndex = 6;
            this._groupVendorRates.TabStop = false;
            this._groupVendorRates.Text = "Vendor Rates";
            // 
            // _lblInformational
            // 
            this._lblInformational.AutoSize = true;
            this._lblInformational.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblInformational.ForeColor = System.Drawing.Color.Blue;
            this._lblInformational.Location = new System.Drawing.Point( 13, 25 );
            this._lblInformational.Name = "_lblInformational";
            this._lblInformational.Size = new System.Drawing.Size( 0, 13 );
            this._lblInformational.TabIndex = 2;
            // 
            // _panelRateControl
            // 
            this._panelRateControl.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._panelRateControl.Location = new System.Drawing.Point( 16, 41 );
            this._panelRateControl.Name = "_panelRateControl";
            this._panelRateControl.Size = new System.Drawing.Size( 552, 267 );
            this._panelRateControl.TabIndex = 0;
            // 
            // _btnUpdate
            // 
            this._btnUpdate.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._btnUpdate.Location = new System.Drawing.Point( 488, 317 );
            this._btnUpdate.Name = "_btnUpdate";
            this._btnUpdate.Size = new System.Drawing.Size( 80, 29 );
            this._btnUpdate.TabIndex = 1;
            this._btnUpdate.Text = "&Update";
            this._btnUpdate.UseVisualStyleBackColor = true;
            this._btnUpdate.Click += new System.EventHandler( this._btnUpdate_Click );
            // 
            // _cboEffectiveDate
            // 
            this._cboEffectiveDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboEffectiveDate.Enabled = false;
            this._cboEffectiveDate.FormattingEnabled = true;
            this._cboEffectiveDate.Location = new System.Drawing.Point( 129, 63 );
            this._cboEffectiveDate.Name = "_cboEffectiveDate";
            this._cboEffectiveDate.Size = new System.Drawing.Size( 153, 21 );
            this._cboEffectiveDate.TabIndex = 5;
            this._cboEffectiveDate.SelectionChangeCommitted += new System.EventHandler( this._cboEffectiveDate_SelectedValueChanged );
            // 
            // ucpAdminVendorsRates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._toolStrip );
            this.Controls.Add( this._cboEffectiveDate );
            this.Controls.Add( this._groupVendorRates );
            this.Controls.Add( this._cboVendorRateType );
            this.Controls.Add( this._cboVendor );
            this.Controls.Add( this._lblEffectiveDate );
            this.Controls.Add( this._lblVendorRateType );
            this.Controls.Add( this._lblVendor );
            this.Name = "ucpAdminVendorsRates";
            this.Size = new System.Drawing.Size( 618, 479 );
            this.Load += new System.EventHandler( this.ucpAdminVendorsRates_Load );
            this._toolStrip.ResumeLayout( false );
            this._toolStrip.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._dsAdministration ) ).EndInit();
            this._groupVendorRates.ResumeLayout( false );
            this._groupVendorRates.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnNew;
        private System.Windows.Forms.ToolStripButton _btnCancel;
        private System.Windows.Forms.Label _lblVendor;
        private System.Windows.Forms.Label _lblVendorRateType;
        private System.Windows.Forms.Label _lblEffectiveDate;
        private System.Windows.Forms.ComboBox _cboVendor;
        private System.Windows.Forms.ComboBox _cboVendorRateType;
        private System.Windows.Forms.GroupBox _groupVendorRates;
        private System.Windows.Forms.ComboBox _cboEffectiveDate;
        private CatalogEstimating.Datasets.Administration _dsAdministration;
        private System.Windows.Forms.Button _btnUpdate;
        private System.Windows.Forms.Panel _panelRateControl;
        private System.Windows.Forms.ToolStripButton _btnDelete;
        private System.Windows.Forms.Label _lblInformational;
    }
}
