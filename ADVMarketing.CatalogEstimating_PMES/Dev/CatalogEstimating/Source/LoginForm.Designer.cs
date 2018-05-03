namespace CatalogEstimating
{
    partial class LoginForm
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
            if (disposing && (components != null))
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
            this.components = new System.ComponentModel.Container();
            this._txtUserName = new System.Windows.Forms.TextBox();
            this._txtPassword = new System.Windows.Forms.TextBox();
            this._cboDomain = new System.Windows.Forms.ComboBox();
            this._lblUserName = new System.Windows.Forms.Label();
            this._lblPassword = new System.Windows.Forms.Label();
            this._lblDomain = new System.Windows.Forms.Label();
            this._grpLogin = new System.Windows.Forms.GroupBox();
            this._btnLogin = new System.Windows.Forms.Button();
            this._btnQuit = new System.Windows.Forms.Button();
            this._errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            this._imgLogo = new System.Windows.Forms.PictureBox();
            this._grpLogin.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this._imgLogo ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _txtUserName
            // 
            this._txtUserName.Location = new System.Drawing.Point( 93, 24 );
            this._txtUserName.Name = "_txtUserName";
            this._txtUserName.Size = new System.Drawing.Size( 149, 20 );
            this._txtUserName.TabIndex = 1;
            this._txtUserName.Validated += new System.EventHandler( this.txtUserName_Validated );
            this._txtUserName.Validating += new System.ComponentModel.CancelEventHandler( this.txtUserName_Validating );
            // 
            // _txtPassword
            // 
            this._txtPassword.Location = new System.Drawing.Point( 93, 57 );
            this._txtPassword.Name = "_txtPassword";
            this._txtPassword.Size = new System.Drawing.Size( 149, 20 );
            this._txtPassword.TabIndex = 3;
            this._txtPassword.UseSystemPasswordChar = true;
            // 
            // _cboDomain
            // 
            this._cboDomain.FormattingEnabled = true;
            this._cboDomain.Location = new System.Drawing.Point( 93, 92 );
            this._cboDomain.Name = "_cboDomain";
            this._cboDomain.Size = new System.Drawing.Size( 149, 21 );
            this._cboDomain.TabIndex = 5;
            // 
            // _lblUserName
            // 
            this._lblUserName.AutoSize = true;
            this._lblUserName.Location = new System.Drawing.Point( 24, 27 );
            this._lblUserName.Name = "_lblUserName";
            this._lblUserName.Size = new System.Drawing.Size( 63, 13 );
            this._lblUserName.TabIndex = 0;
            this._lblUserName.Text = "User Name:";
            // 
            // _lblPassword
            // 
            this._lblPassword.AutoSize = true;
            this._lblPassword.Location = new System.Drawing.Point( 24, 60 );
            this._lblPassword.Name = "_lblPassword";
            this._lblPassword.Size = new System.Drawing.Size( 56, 13 );
            this._lblPassword.TabIndex = 2;
            this._lblPassword.Text = "Password:";
            // 
            // _lblDomain
            // 
            this._lblDomain.AutoSize = true;
            this._lblDomain.Location = new System.Drawing.Point( 24, 95 );
            this._lblDomain.Name = "_lblDomain";
            this._lblDomain.Size = new System.Drawing.Size( 46, 13 );
            this._lblDomain.TabIndex = 4;
            this._lblDomain.Text = "Domain:";
            // 
            // _grpLogin
            // 
            this._grpLogin.Controls.Add( this._lblUserName );
            this._grpLogin.Controls.Add( this._txtUserName );
            this._grpLogin.Controls.Add( this._txtPassword );
            this._grpLogin.Controls.Add( this._lblDomain );
            this._grpLogin.Controls.Add( this._cboDomain );
            this._grpLogin.Controls.Add( this._lblPassword );
            this._grpLogin.Location = new System.Drawing.Point( 74, 66 );
            this._grpLogin.Name = "_grpLogin";
            this._grpLogin.Size = new System.Drawing.Size( 267, 132 );
            this._grpLogin.TabIndex = 0;
            this._grpLogin.TabStop = false;
            this._grpLogin.Text = "Login";
            // 
            // _btnLogin
            // 
            this._btnLogin.Location = new System.Drawing.Point( 101, 215 );
            this._btnLogin.Name = "_btnLogin";
            this._btnLogin.Size = new System.Drawing.Size( 80, 29 );
            this._btnLogin.TabIndex = 1;
            this._btnLogin.Text = "&Login";
            this._btnLogin.UseVisualStyleBackColor = true;
            this._btnLogin.Click += new System.EventHandler( this.cmdLogin_Click );
            // 
            // _btnQuit
            // 
            this._btnQuit.CausesValidation = false;
            this._btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnQuit.Location = new System.Drawing.Point( 237, 215 );
            this._btnQuit.Name = "_btnQuit";
            this._btnQuit.Size = new System.Drawing.Size( 79, 29 );
            this._btnQuit.TabIndex = 2;
            this._btnQuit.Text = "&Quit";
            this._btnQuit.UseVisualStyleBackColor = true;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // _imgLogo
            // 
            this._imgLogo.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._imgLogo.BackgroundImage = global::CatalogEstimating.Properties.Resources.LogoSmall;
            this._imgLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this._imgLogo.Location = new System.Drawing.Point( 0, 12 );
            this._imgLogo.Name = "_imgLogo";
            this._imgLogo.Size = new System.Drawing.Size( 414, 39 );
            this._imgLogo.TabIndex = 6;
            this._imgLogo.TabStop = false;
            // 
            // LoginForm
            // 
            this.AcceptButton = this._btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 414, 267 );
            this.Controls.Add( this._imgLogo );
            this.Controls.Add( this._btnLogin );
            this.Controls.Add( this._btnQuit );
            this.Controls.Add( this._grpLogin );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login -Print Media Estimating System";
            this._grpLogin.ResumeLayout( false );
            this._grpLogin.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._errorProvider ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this._imgLogo ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.TextBox _txtUserName;
        private System.Windows.Forms.TextBox _txtPassword;
        private System.Windows.Forms.ComboBox _cboDomain;
        private System.Windows.Forms.Label _lblUserName;
        private System.Windows.Forms.Label _lblPassword;
        private System.Windows.Forms.Label _lblDomain;
        private System.Windows.Forms.GroupBox _grpLogin;
        private System.Windows.Forms.Button _btnLogin;
        private System.Windows.Forms.Button _btnQuit;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.PictureBox _imgLogo;

    }
}

