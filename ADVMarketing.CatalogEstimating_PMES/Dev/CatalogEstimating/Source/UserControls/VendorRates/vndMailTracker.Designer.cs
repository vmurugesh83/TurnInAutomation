namespace CatalogEstimating.UserControls.VendorRates
{
    partial class vndMailTracker
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
            this._dtEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this._lblEffectiveDate = new System.Windows.Forms.Label();
            this._txtMailTrackingRate = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._lblMailTrackingRate = new System.Windows.Forms.Label();
            ( (System.ComponentModel.ISupportInitialize)( this._Errors ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _dtEffectiveDate
            // 
            this._dtEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtEffectiveDate.Location = new System.Drawing.Point( 143, 12 );
            this._dtEffectiveDate.Name = "_dtEffectiveDate";
            this._dtEffectiveDate.Size = new System.Drawing.Size( 153, 20 );
            this._dtEffectiveDate.TabIndex = 1;
            this._dtEffectiveDate.ValueChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _lblEffectiveDate
            // 
            this._lblEffectiveDate.AutoSize = true;
            this._lblEffectiveDate.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblEffectiveDate.Location = new System.Drawing.Point( 13, 16 );
            this._lblEffectiveDate.Name = "_lblEffectiveDate";
            this._lblEffectiveDate.Size = new System.Drawing.Size( 94, 13 );
            this._lblEffectiveDate.TabIndex = 0;
            this._lblEffectiveDate.Text = "Effective Date*";
            // 
            // _txtMailTrackingRate
            // 
            this._txtMailTrackingRate.AllowNegative = false;
            this._txtMailTrackingRate.FlashColor = System.Drawing.Color.Red;
            this._txtMailTrackingRate.Location = new System.Drawing.Point( 143, 38 );
            this._txtMailTrackingRate.MaxLength = 9;
            this._txtMailTrackingRate.Name = "_txtMailTrackingRate";
            this._txtMailTrackingRate.Size = new System.Drawing.Size( 153, 20 );
            this._txtMailTrackingRate.TabIndex = 3;
            this._txtMailTrackingRate.Value = null;
            this._txtMailTrackingRate.TextChanged += new System.EventHandler( this.Control_ValueChanged );
            // 
            // _lblMailTrackingRate
            // 
            this._lblMailTrackingRate.AutoSize = true;
            this._lblMailTrackingRate.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblMailTrackingRate.Location = new System.Drawing.Point( 13, 41 );
            this._lblMailTrackingRate.Name = "_lblMailTrackingRate";
            this._lblMailTrackingRate.Size = new System.Drawing.Size( 120, 13 );
            this._lblMailTrackingRate.TabIndex = 2;
            this._lblMailTrackingRate.Text = "Mail Tracking Rate*";
            // 
            // vndMailTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.Controls.Add( this._dtEffectiveDate );
            this.Controls.Add( this._lblEffectiveDate );
            this.Controls.Add( this._txtMailTrackingRate );
            this.Controls.Add( this._lblMailTrackingRate );
            this.Name = "vndMailTracker";
            this.Size = new System.Drawing.Size( 322, 73 );
            this.Load += new System.EventHandler( this.vndMailTracker_Load );
            this.Validating += new System.ComponentModel.CancelEventHandler( this.vndMailTracker_Validating );
            ( (System.ComponentModel.ISupportInitialize)( this._Errors ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker _dtEffectiveDate;
        private System.Windows.Forms.Label _lblEffectiveDate;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtMailTrackingRate;
        private System.Windows.Forms.Label _lblMailTrackingRate;
    }
}