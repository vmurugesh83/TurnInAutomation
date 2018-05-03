namespace CatalogEstimating.UserControls.VendorRates
{
    partial class vndMailList
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
            this._lblInternalListRate = new System.Windows.Forms.Label();
            this._txtInternalListRate = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._dtEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this._lblEffectiveDate = new System.Windows.Forms.Label();
            ( (System.ComponentModel.ISupportInitialize)( this._Errors ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _lblInternalListRate
            // 
            this._lblInternalListRate.AutoSize = true;
            this._lblInternalListRate.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblInternalListRate.Location = new System.Drawing.Point( 15, 46 );
            this._lblInternalListRate.Name = "_lblInternalListRate";
            this._lblInternalListRate.Size = new System.Drawing.Size( 110, 13 );
            this._lblInternalListRate.TabIndex = 2;
            this._lblInternalListRate.Text = "Internal List Rate*";
            // 
            // _txtInternalListRate
            // 
            this._txtInternalListRate.AllowNegative = false;
            this._txtInternalListRate.FlashColor = System.Drawing.Color.Red;
            this._txtInternalListRate.Location = new System.Drawing.Point( 135, 43 );
            this._txtInternalListRate.MaxLength = 9;
            this._txtInternalListRate.Name = "_txtInternalListRate";
            this._txtInternalListRate.Size = new System.Drawing.Size( 153, 20 );
            this._txtInternalListRate.TabIndex = 3;
            this._txtInternalListRate.Value = null;
            this._txtInternalListRate.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _dtEffectiveDate
            // 
            this._dtEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEffectiveDate.Location = new System.Drawing.Point( 135, 17 );
            this._dtEffectiveDate.Name = "_dtEffectiveDate";
            this._dtEffectiveDate.Size = new System.Drawing.Size( 153, 20 );
            this._dtEffectiveDate.TabIndex = 1;
            this._dtEffectiveDate.ValueChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _lblEffectiveDate
            // 
            this._lblEffectiveDate.AutoSize = true;
            this._lblEffectiveDate.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblEffectiveDate.Location = new System.Drawing.Point( 15, 21 );
            this._lblEffectiveDate.Name = "_lblEffectiveDate";
            this._lblEffectiveDate.Size = new System.Drawing.Size( 94, 13 );
            this._lblEffectiveDate.TabIndex = 0;
            this._lblEffectiveDate.Text = "Effective Date*";
            // 
            // vndMailList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.Controls.Add( this._dtEffectiveDate );
            this.Controls.Add( this._lblEffectiveDate );
            this.Controls.Add( this._txtInternalListRate );
            this.Controls.Add( this._lblInternalListRate );
            this.Name = "vndMailList";
            this.Size = new System.Drawing.Size( 308, 85 );
            this.Load += new System.EventHandler( this.vndMailList_Load );
            this.Validating += new System.ComponentModel.CancelEventHandler( this.vndMailList_Validating );
            ( (System.ComponentModel.ISupportInitialize)( this._Errors ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblInternalListRate;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtInternalListRate;
        private System.Windows.Forms.DateTimePicker _dtEffectiveDate;
        private System.Windows.Forms.Label _lblEffectiveDate;
    }
}