namespace CatalogEstimating.UserControls.Reports
{
    partial class rptPostageCategoryUsage
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
            this._txtPolybagID = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._txtHostAdNumber = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._txtEstimateID = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._dtEndRunDate = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._dtStartRunDate = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._radExcludeVS = new System.Windows.Forms.RadioButton();
            this._radOnlyVS = new System.Windows.Forms.RadioButton();
            this._radAll = new System.Windows.Forms.RadioButton();
            this._lstEstMediaType = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this._cboEstimateStatus = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this._errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            this.groupBox1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 4, 54 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 59, 13 );
            this.label5.TabIndex = 7;
            this.label5.Text = "Polybag ID";
            // 
            // _txtPolybagID
            // 
            this._txtPolybagID.AllowNegative = false;
            this._txtPolybagID.FlashColor = System.Drawing.Color.Red;
            this._txtPolybagID.Location = new System.Drawing.Point( 128, 51 );
            this._txtPolybagID.Name = "_txtPolybagID";
            this._txtPolybagID.Size = new System.Drawing.Size( 100, 20 );
            this._txtPolybagID.TabIndex = 8;
            this._txtPolybagID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtPolybagID.ThousandsSeperator = false;
            this._txtPolybagID.Value = null;
            // 
            // _txtHostAdNumber
            // 
            this._txtHostAdNumber.AllowNegative = false;
            this._txtHostAdNumber.FlashColor = System.Drawing.Color.Red;
            this._txtHostAdNumber.Location = new System.Drawing.Point( 128, 76 );
            this._txtHostAdNumber.MaxLength = 5;
            this._txtHostAdNumber.Name = "_txtHostAdNumber";
            this._txtHostAdNumber.Size = new System.Drawing.Size( 100, 20 );
            this._txtHostAdNumber.TabIndex = 10;
            this._txtHostAdNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtHostAdNumber.ThousandsSeperator = false;
            this._txtHostAdNumber.Value = null;
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
            // _dtEndRunDate
            // 
            this._dtEndRunDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEndRunDate.Location = new System.Drawing.Point( 278, 0 );
            this._dtEndRunDate.Name = "_dtEndRunDate";
            this._dtEndRunDate.Size = new System.Drawing.Size( 88, 20 );
            this._dtEndRunDate.TabIndex = 4;
            this._dtEndRunDate.Value = null;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 4, 79 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 55, 13 );
            this.label1.TabIndex = 9;
            this.label1.Text = "Host Ad #";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this._radExcludeVS );
            this.groupBox1.Controls.Add( this._radOnlyVS );
            this.groupBox1.Controls.Add( this._radAll );
            this.groupBox1.Location = new System.Drawing.Point( 4, 154 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 246, 42 );
            this.groupBox1.TabIndex = 13;
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
            // _lstEstMediaType
            // 
            this._lstEstMediaType.FormattingEnabled = true;
            this._lstEstMediaType.Location = new System.Drawing.Point( 473, 0 );
            this._lstEstMediaType.Name = "_lstEstMediaType";
            this._lstEstMediaType.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this._lstEstMediaType.Size = new System.Drawing.Size( 120, 69 );
            this._lstEstMediaType.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 386, 4 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 81, 13 );
            this.label4.TabIndex = 14;
            this.label4.Text = "Est Media Type";
            // 
            // _cboEstimateStatus
            // 
            this._cboEstimateStatus.FormattingEnabled = true;
            this._cboEstimateStatus.Location = new System.Drawing.Point( 128, 101 );
            this._cboEstimateStatus.Name = "_cboEstimateStatus";
            this._cboEstimateStatus.Size = new System.Drawing.Size( 121, 21 );
            this._cboEstimateStatus.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 4, 104 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 80, 13 );
            this.label7.TabIndex = 11;
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
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point( 4, 4 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 105, 13 );
            this.label8.TabIndex = 0;
            this.label8.Text = "Range of Run Dates";
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // rptPostageCategoryUsage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._txtHostAdNumber );
            this.Controls.Add( this._txtEstimateID );
            this.Controls.Add( this._dtEndRunDate );
            this.Controls.Add( this._dtStartRunDate );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.groupBox1 );
            this.Controls.Add( this._lstEstMediaType );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this._cboEstimateStatus );
            this.Controls.Add( this.label7 );
            this.Controls.Add( this.label6 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label8 );
            this.Controls.Add( this._txtPolybagID );
            this.Controls.Add( this.label5 );
            this.Name = "rptPostageCategoryUsage";
            this.Size = new System.Drawing.Size( 616, 219 );
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtPolybagID;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtHostAdNumber;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtEstimateID;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtEndRunDate;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtStartRunDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton _radExcludeVS;
        private System.Windows.Forms.RadioButton _radOnlyVS;
        private System.Windows.Forms.RadioButton _radAll;
        private System.Windows.Forms.ListBox _lstEstMediaType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox _cboEstimateStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ErrorProvider _errorProvider;
    }
}
