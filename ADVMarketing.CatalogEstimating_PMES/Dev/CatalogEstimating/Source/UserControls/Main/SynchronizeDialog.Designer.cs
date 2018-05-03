namespace CatalogEstimating.UserControls.Main
{
    partial class SynchronizeDialog
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
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnSyncClose = new System.Windows.Forms.Button();
            this._cboDatabases = new System.Windows.Forms.ComboBox();
            this._lblDatabases = new System.Windows.Forms.Label();
            this._progressSync = new System.Windows.Forms.ProgressBar();
            this._lblStatus = new System.Windows.Forms.Label();
            this._TestTimer = new System.Windows.Forms.Timer( this.components );
            this.SuspendLayout();
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point( 205, 121 );
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size( 80, 29 );
            this._btnCancel.TabIndex = 6;
            this._btnCancel.Text = "&Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // _btnSyncClose
            // 
            this._btnSyncClose.CausesValidation = false;
            this._btnSyncClose.Location = new System.Drawing.Point( 111, 121 );
            this._btnSyncClose.Name = "_btnSyncClose";
            this._btnSyncClose.Size = new System.Drawing.Size( 79, 29 );
            this._btnSyncClose.TabIndex = 7;
            this._btnSyncClose.Text = "&Sync";
            this._btnSyncClose.UseVisualStyleBackColor = true;
            this._btnSyncClose.Click += new System.EventHandler( this._btnSyncClose_Click );
            // 
            // _cboDatabases
            // 
            this._cboDatabases.FormattingEnabled = true;
            this._cboDatabases.Location = new System.Drawing.Point( 89, 18 );
            this._cboDatabases.Name = "_cboDatabases";
            this._cboDatabases.Size = new System.Drawing.Size( 196, 21 );
            this._cboDatabases.TabIndex = 5;
            this._cboDatabases.Text = "Test Database";
            // 
            // _lblDatabases
            // 
            this._lblDatabases.AutoSize = true;
            this._lblDatabases.Location = new System.Drawing.Point( 21, 21 );
            this._lblDatabases.Name = "_lblDatabases";
            this._lblDatabases.Size = new System.Drawing.Size( 60, 13 );
            this._lblDatabases.TabIndex = 4;
            this._lblDatabases.Text = "Destination";
            // 
            // _progressSync
            // 
            this._progressSync.ForeColor = System.Drawing.Color.Lime;
            this._progressSync.Location = new System.Drawing.Point( 24, 54 );
            this._progressSync.Maximum = 5;
            this._progressSync.Name = "_progressSync";
            this._progressSync.Size = new System.Drawing.Size( 261, 16 );
            this._progressSync.Step = 1;
            this._progressSync.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._progressSync.TabIndex = 8;
            // 
            // _lblStatus
            // 
            this._lblStatus.Location = new System.Drawing.Point( 21, 82 );
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new System.Drawing.Size( 264, 24 );
            this._lblStatus.TabIndex = 9;
            // 
            // _TestTimer
            // 
            this._TestTimer.Interval = 5000;
            this._TestTimer.Tick += new System.EventHandler( this._TestTimer_Tick );
            // 
            // SynchronizeDialog
            // 
            this.AcceptButton = this._btnSyncClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size( 307, 171 );
            this.Controls.Add( this._lblStatus );
            this.Controls.Add( this._progressSync );
            this.Controls.Add( this._btnCancel );
            this.Controls.Add( this._btnSyncClose );
            this.Controls.Add( this._cboDatabases );
            this.Controls.Add( this._lblDatabases );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SynchronizeDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Synchronize";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.Button _btnSyncClose;
        private System.Windows.Forms.ComboBox _cboDatabases;
        private System.Windows.Forms.Label _lblDatabases;
        private System.Windows.Forms.ProgressBar _progressSync;
        private System.Windows.Forms.Label _lblStatus;
        private System.Windows.Forms.Timer _TestTimer;
    }
}