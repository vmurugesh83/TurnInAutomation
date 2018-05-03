namespace CatalogEstimating.UserControls.Admin
{
    partial class ucpAdminCostCodeMapping
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
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnSave = new System.Windows.Forms.ToolStripButton();
            this._btnCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._btnRefresh = new System.Windows.Forms.ToolStripButton();
            this._btnPrint = new System.Windows.Forms.ToolStripButton();
            this._lblCostCode = new System.Windows.Forms.Label();
            this._lblVendorType = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this._gridCostCodes = new System.Windows.Forms.DataGridView();
            this.colCostCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendorType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._btnAdd = new System.Windows.Forms.Button();
            this._toolStrip.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._gridCostCodes ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._btnSave,
            this._btnCancel,
            this.toolStripSeparator1,
            this._btnRefresh,
            this._btnPrint} );
            this._toolStrip.Location = new System.Drawing.Point( 0, 242 );
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size( 108, 25 );
            this._toolStrip.TabIndex = 3;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _btnSave
            // 
            this._btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnSave.Image = global::CatalogEstimating.Properties.Resources.Save;
            this._btnSave.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size( 23, 22 );
            this._btnSave.Text = "toolStripButton1";
            this._btnSave.ToolTipText = "Save";
            // 
            // _btnCancel
            // 
            this._btnCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnCancel.Image = global::CatalogEstimating.Properties.Resources.Cancel;
            this._btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size( 23, 22 );
            this._btnCancel.Text = "Cancel";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 6, 25 );
            // 
            // _btnRefresh
            // 
            this._btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnRefresh.Image = global::CatalogEstimating.Properties.Resources.Refresh;
            this._btnRefresh.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.Size = new System.Drawing.Size( 23, 22 );
            this._btnRefresh.Text = "Refresh";
            // 
            // _btnPrint
            // 
            this._btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnPrint.Image = global::CatalogEstimating.Properties.Resources.Print;
            this._btnPrint.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnPrint.Name = "_btnPrint";
            this._btnPrint.Size = new System.Drawing.Size( 23, 22 );
            this._btnPrint.Text = "Print";
            // 
            // _lblCostCode
            // 
            this._lblCostCode.AutoSize = true;
            this._lblCostCode.Location = new System.Drawing.Point( 12, 13 );
            this._lblCostCode.Name = "_lblCostCode";
            this._lblCostCode.Size = new System.Drawing.Size( 56, 13 );
            this._lblCostCode.TabIndex = 4;
            this._lblCostCode.Text = "Cost Code";
            // 
            // _lblVendorType
            // 
            this._lblVendorType.AutoSize = true;
            this._lblVendorType.Location = new System.Drawing.Point( 242, 13 );
            this._lblVendorType.Name = "_lblVendorType";
            this._lblVendorType.Size = new System.Drawing.Size( 68, 13 );
            this._lblVendorType.TabIndex = 5;
            this._lblVendorType.Text = "Vendor Type";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point( 74, 10 );
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size( 139, 21 );
            this.comboBox1.TabIndex = 6;
            this.comboBox1.Text = "850";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point( 316, 10 );
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size( 139, 21 );
            this.comboBox2.TabIndex = 7;
            this.comboBox2.Text = "Printer";
            // 
            // _gridCostCodes
            // 
            this._gridCostCodes.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left ) ) );
            this._gridCostCodes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridCostCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridCostCodes.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.colCostCode,
            this.colVendorType} );
            this._gridCostCodes.Location = new System.Drawing.Point( 15, 46 );
            this._gridCostCodes.Name = "_gridCostCodes";
            this._gridCostCodes.RowHeadersVisible = false;
            this._gridCostCodes.Size = new System.Drawing.Size( 540, 223 );
            this._gridCostCodes.TabIndex = 8;
            // 
            // colCostCode
            // 
            this.colCostCode.HeaderText = "Cost Code";
            this.colCostCode.Name = "colCostCode";
            // 
            // colVendorType
            // 
            this.colVendorType.HeaderText = "VendorType";
            this.colVendorType.Name = "colVendorType";
            // 
            // _btnAdd
            // 
            this._btnAdd.Location = new System.Drawing.Point( 475, 5 );
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size( 80, 29 );
            this._btnAdd.TabIndex = 9;
            this._btnAdd.Text = "&Add";
            this._btnAdd.UseVisualStyleBackColor = true;
            // 
            // ucpAdminCostCodeMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._btnAdd );
            this.Controls.Add( this._gridCostCodes );
            this.Controls.Add( this.comboBox2 );
            this.Controls.Add( this.comboBox1 );
            this.Controls.Add( this._lblVendorType );
            this.Controls.Add( this._lblCostCode );
            this.Controls.Add( this._toolStrip );
            this.Name = "ucpAdminCostCodeMapping";
            this.Size = new System.Drawing.Size( 678, 282 );
            this._toolStrip.ResumeLayout( false );
            this._toolStrip.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this._gridCostCodes ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnSave;
        private System.Windows.Forms.ToolStripButton _btnCancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton _btnPrint;
        private System.Windows.Forms.ToolStripButton _btnRefresh;
        private System.Windows.Forms.Label _lblCostCode;
        private System.Windows.Forms.Label _lblVendorType;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.DataGridView _gridCostCodes;
        private System.Windows.Forms.Button _btnAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCostCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendorType;
    }
}
