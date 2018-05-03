namespace CatalogEstimating
{
	partial class AboutForm
	{
	        /// <summary>
	        /// Required designer variable.
	        /// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
	        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this._btnOK = new System.Windows.Forms.Button();
            this._lblVersion = new System.Windows.Forms.Label();
            this._pnlBorder.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pnlBorder
            // 
            this._pnlBorder.Controls.Add( this._lblVersion );
            this._pnlBorder.Controls.Add( this._btnOK );
            this._pnlBorder.Controls.SetChildIndex( this._btnOK, 0 );
            this._pnlBorder.Controls.SetChildIndex( this._lblVersion, 0 );
            // 
            // _btnOK
            // 
            this._btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOK.Location = new System.Drawing.Point( 402, 233 );
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size( 80, 29 );
            this._btnOK.TabIndex = 2;
            this._btnOK.Text = "&OK";
            this._btnOK.UseVisualStyleBackColor = true;
            // 
            // _lblVersion
            // 
            this._lblVersion.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._lblVersion.Font = new System.Drawing.Font( "Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblVersion.Location = new System.Drawing.Point( -1, 166 );
            this._lblVersion.Name = "_lblVersion";
            this._lblVersion.Size = new System.Drawing.Size( 495, 24 );
            this._lblVersion.TabIndex = 3;
            this._lblVersion.Text = "Version";
            this._lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AboutForm
            // 
            this.AcceptButton = this._btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.ClientSize = new System.Drawing.Size( 495, 275 );
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this._pnlBorder.ResumeLayout( false );
            this.ResumeLayout( false );

		}

		#endregion

        private System.Windows.Forms.Button _btnOK;
        private System.Windows.Forms.Label _lblVersion;

    }
}
