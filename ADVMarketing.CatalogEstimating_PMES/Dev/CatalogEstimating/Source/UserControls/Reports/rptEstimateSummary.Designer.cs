namespace CatalogEstimating.UserControls.Reports
{
    partial class rptEstimateSummary
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
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._radExcludeVS = new System.Windows.Forms.RadioButton();
            this._radOnlyVS = new System.Windows.Forms.RadioButton();
            this._radAll = new System.Windows.Forms.RadioButton();
            this._lstComponentType = new System.Windows.Forms.ListBox();
            this.lblComponentType = new System.Windows.Forms.Label();
            this._lstEstMediaType = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this._cboVendorType = new System.Windows.Forms.ComboBox();
            this._cboVendor = new System.Windows.Forms.ComboBox();
            this.lblVendorType = new System.Windows.Forms.Label();
            this.lblVendor = new System.Windows.Forms.Label();
            this._cboEstimateStatus = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._dtStartRunDate = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._dtEndRunDate = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            this._txtEstimateID = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._txtHostAdNumber = new CatalogEstimating.CustomControls.IntegerTextBox();
            this.groupBox1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 4, 54 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 55, 13 );
            this.label5.TabIndex = 7;
            this.label5.Text = "Host Ad #";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this._radExcludeVS );
            this.groupBox1.Controls.Add( this._radOnlyVS );
            this.groupBox1.Controls.Add( this._radAll );
            this.groupBox1.Location = new System.Drawing.Point( 4, 154 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 246, 42 );
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vendor Supplied?";
            // 
            // _radExcludeVS
            // 
            this._radExcludeVS.AutoSize = true;
            this._radExcludeVS.Location = new System.Drawing.Point( 131, 20 );
            this._radExcludeVS.Name = "_radExcludeVS";
            this._radExcludeVS.Size = new System.Drawing.Size( 80, 17 );
            this._radExcludeVS.TabIndex = 2;
            this._radExcludeVS.Text = "Exclude VS";
            this._radExcludeVS.UseVisualStyleBackColor = true;
            // 
            // _radOnlyVS
            // 
            this._radOnlyVS.AutoSize = true;
            this._radOnlyVS.Location = new System.Drawing.Point( 61, 20 );
            this._radOnlyVS.Name = "_radOnlyVS";
            this._radOnlyVS.Size = new System.Drawing.Size( 63, 17 );
            this._radOnlyVS.TabIndex = 1;
            this._radOnlyVS.Text = "Only VS";
            this._radOnlyVS.UseVisualStyleBackColor = true;
            // 
            // _radAll
            // 
            this._radAll.AutoSize = true;
            this._radAll.Checked = true;
            this._radAll.Location = new System.Drawing.Point( 7, 20 );
            this._radAll.Name = "_radAll";
            this._radAll.Size = new System.Drawing.Size( 36, 17 );
            this._radAll.TabIndex = 0;
            this._radAll.TabStop = true;
            this._radAll.Text = "All";
            this._radAll.UseVisualStyleBackColor = true;
            // 
            // _lstComponentType
            // 
            this._lstComponentType.FormattingEnabled = true;
            this._lstComponentType.Location = new System.Drawing.Point( 473, 75 );
            this._lstComponentType.Name = "_lstComponentType";
            this._lstComponentType.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this._lstComponentType.Size = new System.Drawing.Size( 120, 95 );
            this._lstComponentType.TabIndex = 19;
            // 
            // lblComponentType
            // 
            this.lblComponentType.AutoSize = true;
            this.lblComponentType.Location = new System.Drawing.Point( 379, 79 );
            this.lblComponentType.Name = "lblComponentType";
            this.lblComponentType.Size = new System.Drawing.Size( 88, 13 );
            this.lblComponentType.TabIndex = 18;
            this.lblComponentType.Text = "Component Type";
            // 
            // _lstEstMediaType
            // 
            this._lstEstMediaType.FormattingEnabled = true;
            this._lstEstMediaType.Location = new System.Drawing.Point( 473, 0 );
            this._lstEstMediaType.Name = "_lstEstMediaType";
            this._lstEstMediaType.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this._lstEstMediaType.Size = new System.Drawing.Size( 120, 69 );
            this._lstEstMediaType.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 386, 4 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 81, 13 );
            this.label4.TabIndex = 16;
            this.label4.Text = "Est Media Type";
            // 
            // _cboVendorType
            // 
            this._cboVendorType.FormattingEnabled = true;
            this._cboVendorType.Location = new System.Drawing.Point( 128, 126 );
            this._cboVendorType.Name = "_cboVendorType";
            this._cboVendorType.Size = new System.Drawing.Size( 121, 21 );
            this._cboVendorType.TabIndex = 14;
            // 
            // _cboVendor
            // 
            this._cboVendor.FormattingEnabled = true;
            this._cboVendor.Location = new System.Drawing.Point( 128, 101 );
            this._cboVendor.Name = "_cboVendor";
            this._cboVendor.Size = new System.Drawing.Size( 121, 21 );
            this._cboVendor.TabIndex = 12;
            // 
            // lblVendorType
            // 
            this.lblVendorType.AutoSize = true;
            this.lblVendorType.Location = new System.Drawing.Point( 4, 129 );
            this.lblVendorType.Name = "lblVendorType";
            this.lblVendorType.Size = new System.Drawing.Size( 68, 13 );
            this.lblVendorType.TabIndex = 13;
            this.lblVendorType.Text = "Vendor Type";
            // 
            // lblVendor
            // 
            this.lblVendor.AutoSize = true;
            this.lblVendor.Location = new System.Drawing.Point( 4, 104 );
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size( 41, 13 );
            this.lblVendor.TabIndex = 11;
            this.lblVendor.Text = "Vendor";
            // 
            // _cboEstimateStatus
            // 
            this._cboEstimateStatus.FormattingEnabled = true;
            this._cboEstimateStatus.Location = new System.Drawing.Point( 128, 76 );
            this._cboEstimateStatus.Name = "_cboEstimateStatus";
            this._cboEstimateStatus.Size = new System.Drawing.Size( 121, 21 );
            this._cboEstimateStatus.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 4, 79 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 80, 13 );
            this.label7.TabIndex = 9;
            this.label7.Text = "Estimate Status";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 4, 29 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 61, 13 );
            this.label6.TabIndex = 5;
            this.label6.Text = "Estimate ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 256, 4 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 16, 13 );
            this.label3.TabIndex = 3;
            this.label3.Text = "to";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 125, 4 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 30, 13 );
            this.label2.TabIndex = 1;
            this.label2.Text = "From";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 4, 4 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 105, 13 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Range of Run Dates";
            // 
            // _dtStartRunDate
            // 
            this._dtStartRunDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtStartRunDate.Location = new System.Drawing.Point( 162, 0 );
            this._dtStartRunDate.Name = "_dtStartRunDate";
            this._dtStartRunDate.Size = new System.Drawing.Size( 88, 20 );
            this._dtStartRunDate.TabIndex = 2;
            this._dtStartRunDate.Value = null;
            // 
            // _dtEndRunDate
            // 
            this._dtEndRunDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEndRunDate.Location = new System.Drawing.Point( 278, 0 );
            this._dtEndRunDate.Name = "_dtEndRunDate";
            this._dtEndRunDate.Size = new System.Drawing.Size( 88, 20 );
            this._dtEndRunDate.TabIndex = 4;
            this._dtEndRunDate.Value = null;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // _txtEstimateID
            // 
            this._txtEstimateID.AllowNegative = false;
            this._txtEstimateID.FlashColor = System.Drawing.Color.Red;
            this._txtEstimateID.Location = new System.Drawing.Point( 128, 26 );
            this._txtEstimateID.MaxLength = 18;
            this._txtEstimateID.Name = "_txtEstimateID";
            this._txtEstimateID.Size = new System.Drawing.Size( 100, 20 );
            this._txtEstimateID.TabIndex = 6;
            this._txtEstimateID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtEstimateID.ThousandsSeperator = false;
            this._txtEstimateID.Value = null;
            // 
            // _txtHostAdNumber
            // 
            this._txtHostAdNumber.AllowNegative = false;
            this._txtHostAdNumber.FlashColor = System.Drawing.Color.Red;
            this._txtHostAdNumber.Location = new System.Drawing.Point( 128, 51 );
            this._txtHostAdNumber.MaxLength = 5;
            this._txtHostAdNumber.Name = "_txtHostAdNumber";
            this._txtHostAdNumber.Size = new System.Drawing.Size( 100, 20 );
            this._txtHostAdNumber.TabIndex = 8;
            this._txtHostAdNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtHostAdNumber.ThousandsSeperator = false;
            this._txtHostAdNumber.Value = null;
            // 
            // rptEstimateSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._txtHostAdNumber );
            this.Controls.Add( this._txtEstimateID );
            this.Controls.Add( this._dtEndRunDate );
            this.Controls.Add( this._dtStartRunDate );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.groupBox1 );
            this.Controls.Add( this._lstComponentType );
            this.Controls.Add( this.lblComponentType );
            this.Controls.Add( this._lstEstMediaType );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this._cboVendorType );
            this.Controls.Add( this._cboVendor );
            this.Controls.Add( this.lblVendorType );
            this.Controls.Add( this.lblVendor );
            this.Controls.Add( this._cboEstimateStatus );
            this.Controls.Add( this.label7 );
            this.Controls.Add( this.label6 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.Name = "rptEstimateSummary";
            this.Size = new System.Drawing.Size( 616, 219 );
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label label5;
        protected System.Windows.Forms.GroupBox groupBox1;
        protected System.Windows.Forms.RadioButton _radExcludeVS;
        protected System.Windows.Forms.RadioButton _radOnlyVS;
        protected System.Windows.Forms.RadioButton _radAll;
        protected System.Windows.Forms.ListBox _lstComponentType;
        protected System.Windows.Forms.Label lblComponentType;
        protected System.Windows.Forms.ListBox _lstEstMediaType;
        protected System.Windows.Forms.Label label4;
        protected System.Windows.Forms.ComboBox _cboVendorType;
        protected System.Windows.Forms.ComboBox _cboVendor;
        protected System.Windows.Forms.Label lblVendorType;
        protected System.Windows.Forms.Label lblVendor;
        protected System.Windows.Forms.ComboBox _cboEstimateStatus;
        protected System.Windows.Forms.Label label7;
        protected System.Windows.Forms.Label label6;
        protected System.Windows.Forms.Label label3;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Label label1;
        protected CatalogEstimating.CustomControls.NullableDateTimePicker _dtStartRunDate;
        protected CatalogEstimating.CustomControls.NullableDateTimePicker _dtEndRunDate;
        protected System.Windows.Forms.ErrorProvider _errorProvider;
        protected CatalogEstimating.CustomControls.IntegerTextBox _txtHostAdNumber;
        protected CatalogEstimating.CustomControls.IntegerTextBox _txtEstimateID;


    }
}
