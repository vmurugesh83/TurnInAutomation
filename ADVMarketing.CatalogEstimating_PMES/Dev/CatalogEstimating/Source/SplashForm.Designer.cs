namespace CatalogEstimating
{
    partial class SplashForm
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
            this._timeFade = new System.Windows.Forms.Timer( this.components );
            this._timeSolid = new System.Windows.Forms.Timer( this.components );
            this._pnlBorder = new System.Windows.Forms.Panel();
            this._lblAppName = new System.Windows.Forms.Label();
            this._imgLogo = new System.Windows.Forms.PictureBox();
            this._pnlBorder.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._imgLogo ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _timeFade
            // 
            this._timeFade.Interval = 50;
            this._timeFade.Tick += new System.EventHandler( this._timeFade_Tick );
            // 
            // _timeSolid
            // 
            this._timeSolid.Interval = 2000;
            this._timeSolid.Tick += new System.EventHandler( this._timeSolid_Tick );
            // 
            // _pnlBorder
            // 
            this._pnlBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._pnlBorder.Controls.Add( this._lblAppName );
            this._pnlBorder.Controls.Add( this._imgLogo );
            this._pnlBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlBorder.Location = new System.Drawing.Point( 0, 0 );
            this._pnlBorder.Name = "_pnlBorder";
            this._pnlBorder.Size = new System.Drawing.Size( 495, 275 );
            this._pnlBorder.TabIndex = 1;
            this._pnlBorder.Click += new System.EventHandler( this.Form_Click );
            // 
            // _lblAppName
            // 
            this._lblAppName.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._lblAppName.Font = new System.Drawing.Font( "Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this._lblAppName.Location = new System.Drawing.Point( -1, 96 );
            this._lblAppName.Name = "_lblAppName";
            this._lblAppName.Size = new System.Drawing.Size( 495, 83 );
            this._lblAppName.TabIndex = 1;
            this._lblAppName.Text = "Estimating System";
            this._lblAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._lblAppName.Click += new System.EventHandler( this.Form_Click );
            // 
            // _imgLogo
            // 
            this._imgLogo.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._imgLogo.BackgroundImage = global::CatalogEstimating.Properties.Resources.LogoSmall;
            this._imgLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this._imgLogo.Location = new System.Drawing.Point( -1, 49 );
            this._imgLogo.Name = "_imgLogo";
            this._imgLogo.Size = new System.Drawing.Size( 495, 24 );
            this._imgLogo.TabIndex = 0;
            this._imgLogo.TabStop = false;
            this._imgLogo.Click += new System.EventHandler( this.Form_Click );
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size( 495, 275 );
            this.ControlBox = false;
            this.Controls.Add( this._pnlBorder );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashForm";
            this.Opacity = 0;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Click += new System.EventHandler( this.Form_Click );
            this.Load += new System.EventHandler( this.SplashForm_Load );
            this._pnlBorder.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this._imgLogo ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Timer _timeFade;
        private System.Windows.Forms.Timer _timeSolid;
        private System.Windows.Forms.Label _lblAppName;
        private System.Windows.Forms.PictureBox _imgLogo;
        protected System.Windows.Forms.Panel _pnlBorder;
    }
}