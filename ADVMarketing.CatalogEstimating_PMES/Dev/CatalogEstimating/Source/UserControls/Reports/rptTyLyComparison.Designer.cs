namespace CatalogEstimating.UserControls.Reports
{
    partial class rptTyLyComparison
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._radExcludeVS = new System.Windows.Forms.RadioButton();
            this._radOnlyVS = new System.Windows.Forms.RadioButton();
            this._radAll = new System.Windows.Forms.RadioButton();
            this._lstComponentType = new System.Windows.Forms.ListBox();
            this.lblComponentType = new System.Windows.Forms.Label();
            this._lstEstMediaType = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this._cboSeason = new System.Windows.Forms.ComboBox();
            this._cboFiscalYear = new System.Windows.Forms.ComboBox();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fiscal Season";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fiscal Year*";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._radExcludeVS);
            this.groupBox1.Controls.Add(this._radOnlyVS);
            this.groupBox1.Controls.Add(this._radAll);
            this.groupBox1.Location = new System.Drawing.Point(4, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 42);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vendor Supplied?";
            // 
            // _radExcludeVS
            // 
            this._radExcludeVS.AutoSize = true;
            this._radExcludeVS.Location = new System.Drawing.Point(131, 20);
            this._radExcludeVS.Name = "_radExcludeVS";
            this._radExcludeVS.Size = new System.Drawing.Size(80, 17);
            this._radExcludeVS.TabIndex = 2;
            this._radExcludeVS.Text = "Exclude VS";
            this._radExcludeVS.UseVisualStyleBackColor = true;
            // 
            // _radOnlyVS
            // 
            this._radOnlyVS.AutoSize = true;
            this._radOnlyVS.Location = new System.Drawing.Point(61, 20);
            this._radOnlyVS.Name = "_radOnlyVS";
            this._radOnlyVS.Size = new System.Drawing.Size(63, 17);
            this._radOnlyVS.TabIndex = 1;
            this._radOnlyVS.Text = "Only VS";
            this._radOnlyVS.UseVisualStyleBackColor = true;
            // 
            // _radAll
            // 
            this._radAll.AutoSize = true;
            this._radAll.Checked = true;
            this._radAll.Location = new System.Drawing.Point(7, 20);
            this._radAll.Name = "_radAll";
            this._radAll.Size = new System.Drawing.Size(36, 17);
            this._radAll.TabIndex = 0;
            this._radAll.TabStop = true;
            this._radAll.Text = "All";
            this._radAll.UseVisualStyleBackColor = true;
            // 
            // _lstComponentType
            // 
            this._lstComponentType.FormattingEnabled = true;
            this._lstComponentType.Location = new System.Drawing.Point(377, 79);
            this._lstComponentType.Name = "_lstComponentType";
            this._lstComponentType.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this._lstComponentType.Size = new System.Drawing.Size(120, 95);
            this._lstComponentType.TabIndex = 8;
            // 
            // lblComponentType
            // 
            this.lblComponentType.AutoSize = true;
            this.lblComponentType.Location = new System.Drawing.Point(283, 83);
            this.lblComponentType.Name = "lblComponentType";
            this.lblComponentType.Size = new System.Drawing.Size(88, 13);
            this.lblComponentType.TabIndex = 7;
            this.lblComponentType.Text = "Component Type";
            // 
            // _lstEstMediaType
            // 
            this._lstEstMediaType.FormattingEnabled = true;
            this._lstEstMediaType.Location = new System.Drawing.Point(377, 4);
            this._lstEstMediaType.Name = "_lstEstMediaType";
            this._lstEstMediaType.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this._lstEstMediaType.Size = new System.Drawing.Size(120, 69);
            this._lstEstMediaType.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(290, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Est Media Type";
            // 
            // _cboSeason
            // 
            this._cboSeason.FormattingEnabled = true;
            this._cboSeason.Location = new System.Drawing.Point(83, 1);
            this._cboSeason.Name = "_cboSeason";
            this._cboSeason.Size = new System.Drawing.Size(121, 21);
            this._cboSeason.TabIndex = 1;
            // 
            // _cboFiscalYear
            // 
            this._cboFiscalYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboFiscalYear.FormattingEnabled = true;
            this._cboFiscalYear.Location = new System.Drawing.Point(83, 26);
            this._cboFiscalYear.Name = "_cboFiscalYear";
            this._cboFiscalYear.Size = new System.Drawing.Size(121, 21);
            this._cboFiscalYear.TabIndex = 3;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // rptTyLyComparison
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._cboFiscalYear);
            this.Controls.Add(this._cboSeason);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._lstComponentType);
            this.Controls.Add(this.lblComponentType);
            this.Controls.Add(this._lstEstMediaType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "rptTyLyComparison";
            this.Size = new System.Drawing.Size(511, 211);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton _radExcludeVS;
        private System.Windows.Forms.RadioButton _radOnlyVS;
        private System.Windows.Forms.RadioButton _radAll;
        private System.Windows.Forms.ListBox _lstComponentType;
        private System.Windows.Forms.Label lblComponentType;
        private System.Windows.Forms.ListBox _lstEstMediaType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox _cboSeason;
        private System.Windows.Forms.ComboBox _cboFiscalYear;
        private System.Windows.Forms.ErrorProvider _errorProvider;
    }
}
