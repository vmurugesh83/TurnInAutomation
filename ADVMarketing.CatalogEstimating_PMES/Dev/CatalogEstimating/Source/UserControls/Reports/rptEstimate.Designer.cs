namespace CatalogEstimating.UserControls.Reports
{
    partial class rptEstimate
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
            this.label1 = new System.Windows.Forms.Label();
            this._txtEstimateID = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.label1.Location = new System.Drawing.Point( 4, 4 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 77, 13 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Estimate ID*";
            // 
            // _txtEstimateID
            // 
            this._txtEstimateID.AllowNegative = false;
            this._txtEstimateID.FlashColor = System.Drawing.Color.Red;
            this._txtEstimateID.Location = new System.Drawing.Point( 87, 1 );
            this._txtEstimateID.MaxLength = 18;
            this._txtEstimateID.Name = "_txtEstimateID";
            this._txtEstimateID.Size = new System.Drawing.Size( 100, 20 );
            this._txtEstimateID.TabIndex = 1;
            this._txtEstimateID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtEstimateID.ThousandsSeperator = false;
            this._txtEstimateID.Value = null;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // rptEstimate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._txtEstimateID );
            this.Controls.Add( this.label1 );
            this.Name = "rptEstimate";
            this.Size = new System.Drawing.Size( 579, 208 );
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtEstimateID;
        private System.Windows.Forms.ErrorProvider _errorProvider;

    }
}
