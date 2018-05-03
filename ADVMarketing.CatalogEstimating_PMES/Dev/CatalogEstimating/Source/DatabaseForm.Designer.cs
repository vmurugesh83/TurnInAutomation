namespace CatalogEstimating
{
    partial class DatabaseForm
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
            this._lblDatabases = new System.Windows.Forms.Label();
            this._cboDatabases = new System.Windows.Forms.ComboBox();
            this._btnOK = new System.Windows.Forms.Button();
            this._btnQuit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _lblDatabases
            // 
            this._lblDatabases.AutoSize = true;
            this._lblDatabases.Location = new System.Drawing.Point( 12, 27 );
            this._lblDatabases.Name = "_lblDatabases";
            this._lblDatabases.Size = new System.Drawing.Size( 53, 13 );
            this._lblDatabases.TabIndex = 0;
            this._lblDatabases.Text = "Database";
            // 
            // _cboDatabases
            // 
            this._cboDatabases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboDatabases.FormattingEnabled = true;
            this._cboDatabases.Location = new System.Drawing.Point( 80, 24 );
            this._cboDatabases.Name = "_cboDatabases";
            this._cboDatabases.Size = new System.Drawing.Size( 196, 21 );
            this._cboDatabases.TabIndex = 1;
            // 
            // _btnOK
            // 
            this._btnOK.Location = new System.Drawing.Point( 80, 69 );
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size( 80, 29 );
            this._btnOK.TabIndex = 2;
            this._btnOK.Text = "&OK";
            this._btnOK.UseVisualStyleBackColor = true;
            this._btnOK.Click += new System.EventHandler( this._btnOK_Click );
            // 
            // _btnQuit
            // 
            this._btnQuit.CausesValidation = false;
            this._btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnQuit.Location = new System.Drawing.Point( 197, 69 );
            this._btnQuit.Name = "_btnQuit";
            this._btnQuit.Size = new System.Drawing.Size( 79, 29 );
            this._btnQuit.TabIndex = 3;
            this._btnQuit.Text = "&Quit";
            this._btnQuit.UseVisualStyleBackColor = true;
            // 
            // DatabaseForm
            // 
            this.AcceptButton = this._btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnQuit;
            this.ClientSize = new System.Drawing.Size( 304, 117 );
            this.Controls.Add( this._btnOK );
            this.Controls.Add( this._btnQuit );
            this.Controls.Add( this._cboDatabases );
            this.Controls.Add( this._lblDatabases );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Selection";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblDatabases;
        private System.Windows.Forms.ComboBox _cboDatabases;
        private System.Windows.Forms.Button _btnOK;
        private System.Windows.Forms.Button _btnQuit;
    }
}