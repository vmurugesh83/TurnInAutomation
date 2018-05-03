namespace CatalogEstimating
{
    partial class UserControlTab
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
            this._tabControl = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // _tabControl
            // 
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.Location = new System.Drawing.Point( 0, 0 );
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size( 470, 350 );
            this._tabControl.TabIndex = 0;
            this._tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler( this._tabControl_Selecting );
            this._tabControl.SelectedIndexChanged += new System.EventHandler( this._tabControl_SelectedIndexChanged );
            // 
            // UserControlTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._tabControl );
            this.Name = "UserControlTab";
            this.Size = new System.Drawing.Size( 470, 350 );
            this.ResumeLayout( false );

        }

        #endregion

        protected System.Windows.Forms.TabControl _tabControl;

    }
}
