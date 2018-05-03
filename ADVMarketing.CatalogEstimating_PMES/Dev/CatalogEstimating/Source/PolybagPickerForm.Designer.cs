namespace CatalogEstimating
{
    partial class PolybagPickerForm
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
            this._btnOK = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._cboPolybagGroups = new System.Windows.Forms.ComboBox();
            this._lblDatabases = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _btnOK
            // 
            this._btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOK.Location = new System.Drawing.Point( 80, 71 );
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size( 80, 29 );
            this._btnOK.TabIndex = 6;
            this._btnOK.Text = "&OK";
            this._btnOK.UseVisualStyleBackColor = true;
            this._btnOK.Click += new System.EventHandler( this._btnOK_Click );
            // 
            // _btnCancel
            // 
            this._btnCancel.CausesValidation = false;
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point( 197, 71 );
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size( 79, 29 );
            this._btnCancel.TabIndex = 7;
            this._btnCancel.Text = "&Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler( this._btnCancel_Click );
            // 
            // _cboPolybagGroups
            // 
            this._cboPolybagGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboPolybagGroups.FormattingEnabled = true;
            this._cboPolybagGroups.Location = new System.Drawing.Point( 95, 26 );
            this._cboPolybagGroups.Name = "_cboPolybagGroups";
            this._cboPolybagGroups.Size = new System.Drawing.Size( 181, 21 );
            this._cboPolybagGroups.TabIndex = 5;
            // 
            // _lblDatabases
            // 
            this._lblDatabases.AutoSize = true;
            this._lblDatabases.Location = new System.Drawing.Point( 12, 29 );
            this._lblDatabases.Name = "_lblDatabases";
            this._lblDatabases.Size = new System.Drawing.Size( 77, 13 );
            this._lblDatabases.TabIndex = 4;
            this._lblDatabases.Text = "Polybag Group";
            // 
            // PolybagPickerForm
            // 
            this.AcceptButton = this._btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size( 302, 115 );
            this.Controls.Add( this._btnOK );
            this.Controls.Add( this._btnCancel );
            this.Controls.Add( this._cboPolybagGroups );
            this.Controls.Add( this._lblDatabases );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PolybagPickerForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Polybag Group Selection";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnOK;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.ComboBox _cboPolybagGroups;
        private System.Windows.Forms.Label _lblDatabases;
    }
}