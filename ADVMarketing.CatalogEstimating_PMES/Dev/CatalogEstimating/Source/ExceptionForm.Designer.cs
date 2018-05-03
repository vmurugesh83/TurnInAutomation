namespace CatalogEstimating
{
    partial class ExceptionForm
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
            this._lblMessage = new System.Windows.Forms.Label();
            this._txtMessage = new System.Windows.Forms.Label();
            this._lblCallStack = new System.Windows.Forms.Label();
            this._btnOK = new System.Windows.Forms.Button();
            this._btnDetails = new System.Windows.Forms.Button();
            this._txtCallStack = new System.Windows.Forms.TextBox();
            this._btnCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _lblMessage
            // 
            this._lblMessage.AutoSize = true;
            this._lblMessage.Location = new System.Drawing.Point( 12, 9 );
            this._lblMessage.Name = "_lblMessage";
            this._lblMessage.Size = new System.Drawing.Size( 53, 13 );
            this._lblMessage.TabIndex = 0;
            this._lblMessage.Text = "Message:";
            // 
            // _txtMessage
            // 
            this._txtMessage.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._txtMessage.Location = new System.Drawing.Point( 71, 9 );
            this._txtMessage.Name = "_txtMessage";
            this._txtMessage.Size = new System.Drawing.Size( 483, 55 );
            this._txtMessage.TabIndex = 1;
            // 
            // _lblCallStack
            // 
            this._lblCallStack.AutoSize = true;
            this._lblCallStack.Location = new System.Drawing.Point( 7, 75 );
            this._lblCallStack.Name = "_lblCallStack";
            this._lblCallStack.Size = new System.Drawing.Size( 58, 13 );
            this._lblCallStack.TabIndex = 2;
            this._lblCallStack.Text = "Call Stack:";
            // 
            // _btnOK
            // 
            this._btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOK.Location = new System.Drawing.Point( 479, 281 );
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size( 75, 34 );
            this._btnOK.TabIndex = 3;
            this._btnOK.Text = "&OK";
            this._btnOK.UseVisualStyleBackColor = true;
            this._btnOK.Click += new System.EventHandler( this._btnOK_Click );
            // 
            // _btnDetails
            // 
            this._btnDetails.Location = new System.Drawing.Point( 384, 281 );
            this._btnDetails.Name = "_btnDetails";
            this._btnDetails.Size = new System.Drawing.Size( 78, 34 );
            this._btnDetails.TabIndex = 4;
            this._btnDetails.Text = "Details <<";
            this._btnDetails.UseVisualStyleBackColor = true;
            this._btnDetails.Click += new System.EventHandler( this._btnDetails_Click );
            // 
            // _txtCallStack
            // 
            this._txtCallStack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._txtCallStack.Location = new System.Drawing.Point( 71, 75 );
            this._txtCallStack.Multiline = true;
            this._txtCallStack.Name = "_txtCallStack";
            this._txtCallStack.ReadOnly = true;
            this._txtCallStack.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtCallStack.Size = new System.Drawing.Size( 483, 200 );
            this._txtCallStack.TabIndex = 5;
            // 
            // _btnCopy
            // 
            this._btnCopy.Location = new System.Drawing.Point( 71, 281 );
            this._btnCopy.Name = "_btnCopy";
            this._btnCopy.Size = new System.Drawing.Size( 105, 34 );
            this._btnCopy.TabIndex = 6;
            this._btnCopy.Text = "Copy to Clipboard";
            this._btnCopy.UseVisualStyleBackColor = true;
            this._btnCopy.Click += new System.EventHandler( this._btnCopy_Click );
            // 
            // ExceptionForm
            // 
            this.AcceptButton = this._btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 566, 325 );
            this.Controls.Add( this._btnCopy );
            this.Controls.Add( this._txtCallStack );
            this.Controls.Add( this._btnDetails );
            this.Controls.Add( this._btnOK );
            this.Controls.Add( this._lblCallStack );
            this.Controls.Add( this._txtMessage );
            this.Controls.Add( this._lblMessage );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExceptionForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "An Unhandled Exception Occurred";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblMessage;
        private System.Windows.Forms.Label _txtMessage;
        private System.Windows.Forms.Label _lblCallStack;
        private System.Windows.Forms.Button _btnOK;
        private System.Windows.Forms.Button _btnDetails;
        private System.Windows.Forms.TextBox _txtCallStack;
        private System.Windows.Forms.Button _btnCopy;
    }
}