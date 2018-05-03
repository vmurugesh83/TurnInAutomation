namespace CatalogEstimating
{
    partial class CopyNumberDialog
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
            this._lblNumCopies = new System.Windows.Forms.Label();
            this._btnOK = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._cboDatabases = new System.Windows.Forms.ComboBox();
            this._lblDatabases = new System.Windows.Forms.Label();
            this._txtNumCopies = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this._txtNumCopies)).BeginInit();
            this.SuspendLayout();
            // 
            // _lblNumCopies
            // 
            this._lblNumCopies.AutoSize = true;
            this._lblNumCopies.Location = new System.Drawing.Point(12, 9);
            this._lblNumCopies.Name = "_lblNumCopies";
            this._lblNumCopies.Size = new System.Drawing.Size(91, 13);
            this._lblNumCopies.TabIndex = 0;
            this._lblNumCopies.Text = "Number of Copies";
            // 
            // _btnOK
            // 
            this._btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOK.Location = new System.Drawing.Point(127, 72);
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size(80, 29);
            this._btnOK.TabIndex = 2;
            this._btnOK.Text = "&OK";
            this._btnOK.UseVisualStyleBackColor = true;
            this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.CausesValidation = false;
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(244, 72);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(79, 29);
            this._btnCancel.TabIndex = 3;
            this._btnCancel.Text = "&Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // _cboDatabases
            // 
            this._cboDatabases.FormattingEnabled = true;
            this._cboDatabases.Location = new System.Drawing.Point(127, 34);
            this._cboDatabases.Name = "_cboDatabases";
            this._cboDatabases.Size = new System.Drawing.Size(196, 21);
            this._cboDatabases.TabIndex = 5;
            // 
            // _lblDatabases
            // 
            this._lblDatabases.AutoSize = true;
            this._lblDatabases.Location = new System.Drawing.Point(12, 34);
            this._lblDatabases.Name = "_lblDatabases";
            this._lblDatabases.Size = new System.Drawing.Size(109, 13);
            this._lblDatabases.TabIndex = 4;
            this._lblDatabases.Text = "Destination Database";
            // 
            // _txtNumCopies
            // 
            this._txtNumCopies.Location = new System.Drawing.Point(127, 7);
            this._txtNumCopies.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._txtNumCopies.Name = "_txtNumCopies";
            this._txtNumCopies.Size = new System.Drawing.Size(80, 20);
            this._txtNumCopies.TabIndex = 6;
            this._txtNumCopies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // CopyNumberDialog
            // 
            this.AcceptButton = this._btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(341, 116);
            this.Controls.Add(this._txtNumCopies);
            this.Controls.Add(this._cboDatabases);
            this.Controls.Add(this._lblDatabases);
            this.Controls.Add(this._btnOK);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._lblNumCopies);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CopyNumberDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Copy Estimate";
            ((System.ComponentModel.ISupportInitialize)(this._txtNumCopies)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblNumCopies;
        private System.Windows.Forms.Button _btnOK;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.ComboBox _cboDatabases;
        private System.Windows.Forms.Label _lblDatabases;
        private System.Windows.Forms.NumericUpDown _txtNumCopies;
    }
}