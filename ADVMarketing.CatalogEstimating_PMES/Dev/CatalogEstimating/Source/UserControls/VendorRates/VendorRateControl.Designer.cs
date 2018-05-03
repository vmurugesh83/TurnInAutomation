namespace CatalogEstimating.UserControls.VendorRates
{
    partial class VendorRateControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._Errors = new System.Windows.Forms.ErrorProvider( this.components );
            ( (System.ComponentModel.ISupportInitialize)( this._Errors ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _Errors
            // 
            this._Errors.ContainerControl = this;
            // 
            // VendorRateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Name = "VendorRateControl";
            ( (System.ComponentModel.ISupportInitialize)( this._Errors ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        protected System.Windows.Forms.ErrorProvider _Errors;
    }
}
