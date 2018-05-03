namespace CatalogEstimating.UserControls.Reports
{
    partial class rptVendorCommitment
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
            this._txtEstimateID = new CatalogEstimating.CustomControls.IntegerTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._lstVendor = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this._lstCostCodes = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this._cboEstimateStatus = new System.Windows.Forms.ComboBox();
            this._dtStartRunDate = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._dtEndRunDate = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._txtHostAdNumber = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            this.label9 = new System.Windows.Forms.Label();
            this._lstPublisher = new System.Windows.Forms.ListBox();
            this._btnClearPubs = new System.Windows.Forms.Button();
            this._btnSelectAllPubs = new System.Windows.Forms.Button();
            this._btnClearVendors = new System.Windows.Forms.Button();
            this._btnSelectAllVendors = new System.Windows.Forms.Button();
            this._btnSelectAllCostCodes = new System.Windows.Forms.Button();
            this._btnClearCostCodes = new System.Windows.Forms.Button();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _txtEstimateID
            // 
            this._txtEstimateID.AllowNegative = false;
            this._txtEstimateID.FlashColor = System.Drawing.Color.Red;
            this._txtEstimateID.Location = new System.Drawing.Point( 321, 197 );
            this._txtEstimateID.Name = "_txtEstimateID";
            this._txtEstimateID.Size = new System.Drawing.Size( 120, 20 );
            this._txtEstimateID.TabIndex = 18;
            this._txtEstimateID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtEstimateID.ThousandsSeperator = false;
            this._txtEstimateID.Value = null;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 254, 200 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 61, 13 );
            this.label6.TabIndex = 17;
            this.label6.Text = "Estimate ID";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 40, 200 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 55, 13 );
            this.label5.TabIndex = 15;
            this.label5.Text = "Host Ad #";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 242, 4 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 16, 13 );
            this.label3.TabIndex = 3;
            this.label3.Text = "to";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 112, 4 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 30, 13 );
            this.label2.TabIndex = 1;
            this.label2.Text = "From";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 1, 4 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 105, 13 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Range of Run Dates";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 10, 44 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 41, 13 );
            this.label4.TabIndex = 5;
            this.label4.Text = "Vendor";
            // 
            // _lstVendor
            // 
            this._lstVendor.FormattingEnabled = true;
            this._lstVendor.Location = new System.Drawing.Point( 57, 68 );
            this._lstVendor.Name = "_lstVendor";
            this._lstVendor.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this._lstVendor.Size = new System.Drawing.Size( 162, 108 );
            this._lstVendor.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 464, 44 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 56, 13 );
            this.label7.TabIndex = 13;
            this.label7.Text = "Cost Code";
            // 
            // _lstCostCodes
            // 
            this._lstCostCodes.FormattingEnabled = true;
            this._lstCostCodes.Location = new System.Drawing.Point( 526, 68 );
            this._lstCostCodes.Name = "_lstCostCodes";
            this._lstCostCodes.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this._lstCostCodes.Size = new System.Drawing.Size( 162, 108 );
            this._lstCostCodes.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point( 482, 200 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 80, 13 );
            this.label8.TabIndex = 19;
            this.label8.Text = "Estimate Status";
            // 
            // _cboEstimateStatus
            // 
            this._cboEstimateStatus.FormattingEnabled = true;
            this._cboEstimateStatus.Location = new System.Drawing.Point( 568, 197 );
            this._cboEstimateStatus.Name = "_cboEstimateStatus";
            this._cboEstimateStatus.Size = new System.Drawing.Size( 120, 21 );
            this._cboEstimateStatus.TabIndex = 20;
            // 
            // _dtStartRunDate
            // 
            this._dtStartRunDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtStartRunDate.Location = new System.Drawing.Point( 148, 0 );
            this._dtStartRunDate.Name = "_dtStartRunDate";
            this._dtStartRunDate.Size = new System.Drawing.Size( 88, 20 );
            this._dtStartRunDate.TabIndex = 2;
            this._dtStartRunDate.Value = null;
            // 
            // _dtEndRunDate
            // 
            this._dtEndRunDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEndRunDate.Location = new System.Drawing.Point( 264, 0 );
            this._dtEndRunDate.Name = "_dtEndRunDate";
            this._dtEndRunDate.Size = new System.Drawing.Size( 88, 20 );
            this._dtEndRunDate.TabIndex = 4;
            this._dtEndRunDate.Value = null;
            // 
            // _txtHostAdNumber
            // 
            this._txtHostAdNumber.AllowNegative = false;
            this._txtHostAdNumber.FlashColor = System.Drawing.Color.Red;
            this._txtHostAdNumber.Location = new System.Drawing.Point( 101, 197 );
            this._txtHostAdNumber.MaxLength = 5;
            this._txtHostAdNumber.Name = "_txtHostAdNumber";
            this._txtHostAdNumber.Size = new System.Drawing.Size( 118, 20 );
            this._txtHostAdNumber.TabIndex = 16;
            this._txtHostAdNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtHostAdNumber.ThousandsSeperator = false;
            this._txtHostAdNumber.Value = null;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point( 242, 44 );
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size( 31, 13 );
            this.label9.TabIndex = 9;
            this.label9.Text = "Pubs";
            // 
            // _lstPublisher
            // 
            this._lstPublisher.FormattingEnabled = true;
            this._lstPublisher.Location = new System.Drawing.Point( 279, 68 );
            this._lstPublisher.Name = "_lstPublisher";
            this._lstPublisher.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this._lstPublisher.Size = new System.Drawing.Size( 162, 108 );
            this._lstPublisher.TabIndex = 12;
            // 
            // _btnClearPubs
            // 
            this._btnClearPubs.Location = new System.Drawing.Point( 279, 39 );
            this._btnClearPubs.Name = "_btnClearPubs";
            this._btnClearPubs.Size = new System.Drawing.Size( 75, 23 );
            this._btnClearPubs.TabIndex = 10;
            this._btnClearPubs.Text = "Clear";
            this._btnClearPubs.UseVisualStyleBackColor = true;
            this._btnClearPubs.Click += new System.EventHandler( this._btnClearPubs_Click );
            // 
            // _btnSelectAllPubs
            // 
            this._btnSelectAllPubs.Location = new System.Drawing.Point( 366, 39 );
            this._btnSelectAllPubs.Name = "_btnSelectAllPubs";
            this._btnSelectAllPubs.Size = new System.Drawing.Size( 75, 23 );
            this._btnSelectAllPubs.TabIndex = 11;
            this._btnSelectAllPubs.Text = "Select All";
            this._btnSelectAllPubs.UseVisualStyleBackColor = true;
            this._btnSelectAllPubs.Click += new System.EventHandler( this._btnSelectAllPubs_Click );
            // 
            // _btnClearVendors
            // 
            this._btnClearVendors.Location = new System.Drawing.Point( 57, 39 );
            this._btnClearVendors.Name = "_btnClearVendors";
            this._btnClearVendors.Size = new System.Drawing.Size( 75, 23 );
            this._btnClearVendors.TabIndex = 6;
            this._btnClearVendors.Text = "Clear";
            this._btnClearVendors.UseVisualStyleBackColor = true;
            this._btnClearVendors.Click += new System.EventHandler( this._btnClearVendors_Click );
            // 
            // _btnSelectAllVendors
            // 
            this._btnSelectAllVendors.Location = new System.Drawing.Point( 144, 39 );
            this._btnSelectAllVendors.Name = "_btnSelectAllVendors";
            this._btnSelectAllVendors.Size = new System.Drawing.Size( 75, 23 );
            this._btnSelectAllVendors.TabIndex = 7;
            this._btnSelectAllVendors.Text = "Select All";
            this._btnSelectAllVendors.UseVisualStyleBackColor = true;
            this._btnSelectAllVendors.Click += new System.EventHandler( this._btnSelectAllVendors_Click );
            // 
            // _btnSelectAllCostCodes
            // 
            this._btnSelectAllCostCodes.Location = new System.Drawing.Point( 613, 39 );
            this._btnSelectAllCostCodes.Name = "_btnSelectAllCostCodes";
            this._btnSelectAllCostCodes.Size = new System.Drawing.Size( 75, 23 );
            this._btnSelectAllCostCodes.TabIndex = 22;
            this._btnSelectAllCostCodes.Text = "Select All";
            this._btnSelectAllCostCodes.UseVisualStyleBackColor = true;
            this._btnSelectAllCostCodes.Click += new System.EventHandler( this._btnSelectAllCostCodes_Click );
            // 
            // _btnClearCostCodes
            // 
            this._btnClearCostCodes.Location = new System.Drawing.Point( 526, 39 );
            this._btnClearCostCodes.Name = "_btnClearCostCodes";
            this._btnClearCostCodes.Size = new System.Drawing.Size( 75, 23 );
            this._btnClearCostCodes.TabIndex = 21;
            this._btnClearCostCodes.Text = "Clear";
            this._btnClearCostCodes.UseVisualStyleBackColor = true;
            this._btnClearCostCodes.Click += new System.EventHandler( this._btnClearCostCodes_Click );
            // 
            // rptVendorCommitment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._btnSelectAllCostCodes );
            this.Controls.Add( this._btnClearCostCodes );
            this.Controls.Add( this._btnSelectAllVendors );
            this.Controls.Add( this._btnClearVendors );
            this.Controls.Add( this._btnSelectAllPubs );
            this.Controls.Add( this._btnClearPubs );
            this.Controls.Add( this._lstPublisher );
            this.Controls.Add( this.label9 );
            this.Controls.Add( this._txtHostAdNumber );
            this.Controls.Add( this._dtEndRunDate );
            this.Controls.Add( this._dtStartRunDate );
            this.Controls.Add( this._cboEstimateStatus );
            this.Controls.Add( this.label8 );
            this.Controls.Add( this._lstCostCodes );
            this.Controls.Add( this.label7 );
            this.Controls.Add( this._lstVendor );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this._txtEstimateID );
            this.Controls.Add( this.label6 );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.Name = "rptVendorCommitment";
            this.Size = new System.Drawing.Size( 702, 246 );
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private CatalogEstimating.CustomControls.IntegerTextBox _txtEstimateID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox _lstVendor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox _lstCostCodes;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox _cboEstimateStatus;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtStartRunDate;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtEndRunDate;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtHostAdNumber;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.ListBox _lstPublisher;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button _btnClearPubs;
        private System.Windows.Forms.Button _btnSelectAllPubs;
        private System.Windows.Forms.Button _btnSelectAllVendors;
        private System.Windows.Forms.Button _btnClearVendors;
        private System.Windows.Forms.Button _btnSelectAllCostCodes;
        private System.Windows.Forms.Button _btnClearCostCodes;
    }
}
