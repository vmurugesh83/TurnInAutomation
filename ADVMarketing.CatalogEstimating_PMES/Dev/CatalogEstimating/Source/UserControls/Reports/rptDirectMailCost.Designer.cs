namespace CatalogEstimating.UserControls.Reports
{
    partial class rptDirectMailCost
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
            this._cboEstimateStatus = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._dtEndRunDate = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._dtStartRunDate = new CatalogEstimating.CustomControls.NullableDateTimePicker();
            this._txtHostAdNumber = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._txtEstimateID = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _cboEstimateStatus
            // 
            this._cboEstimateStatus.FormattingEnabled = true;
            this._cboEstimateStatus.Location = new System.Drawing.Point( 129, 76 );
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
            this.label6.Location = new System.Drawing.Point( 4, 54 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 61, 13 );
            this.label6.TabIndex = 7;
            this.label6.Text = "Estimate ID";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 4, 29 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 55, 13 );
            this.label5.TabIndex = 5;
            this.label5.Text = "Host Ad #";
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
            this.label2.Location = new System.Drawing.Point( 126, 4 );
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
            // _txtHostAdNumber
            // 
            this._txtHostAdNumber.AllowNegative = false;
            this._txtHostAdNumber.FlashColor = System.Drawing.Color.Red;
            this._txtHostAdNumber.Location = new System.Drawing.Point( 129, 26 );
            this._txtHostAdNumber.MaxLength = 5;
            this._txtHostAdNumber.Name = "_txtHostAdNumber";
            this._txtHostAdNumber.Size = new System.Drawing.Size( 100, 20 );
            this._txtHostAdNumber.TabIndex = 6;
            this._txtHostAdNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtHostAdNumber.ThousandsSeperator = false;
            this._txtHostAdNumber.Value = null;
            // 
            // _txtEstimateID
            // 
            this._txtEstimateID.AllowNegative = false;
            this._txtEstimateID.FlashColor = System.Drawing.Color.Red;
            this._txtEstimateID.Location = new System.Drawing.Point( 129, 51 );
            this._txtEstimateID.MaxLength = 18;
            this._txtEstimateID.Name = "_txtEstimateID";
            this._txtEstimateID.Size = new System.Drawing.Size( 100, 20 );
            this._txtEstimateID.TabIndex = 8;
            this._txtEstimateID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtEstimateID.ThousandsSeperator = false;
            this._txtEstimateID.Value = null;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // rptDirectMailCost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._txtEstimateID );
            this.Controls.Add( this._txtHostAdNumber );
            this.Controls.Add( this._dtEndRunDate );
            this.Controls.Add( this._dtStartRunDate );
            this.Controls.Add( this._cboEstimateStatus );
            this.Controls.Add( this.label7 );
            this.Controls.Add( this.label6 );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.Name = "rptDirectMailCost";
            this.Size = new System.Drawing.Size( 374, 154 );
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox _cboEstimateStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtEndRunDate;
        private CatalogEstimating.CustomControls.NullableDateTimePicker _dtStartRunDate;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtHostAdNumber;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtEstimateID;
        private System.Windows.Forms.ErrorProvider _errorProvider;
    }
}
