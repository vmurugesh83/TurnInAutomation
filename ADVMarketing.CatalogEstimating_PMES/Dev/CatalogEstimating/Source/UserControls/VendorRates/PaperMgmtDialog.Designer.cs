namespace CatalogEstimating.UserControls.VendorRates
{
    partial class PaperMgmtDialog
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
            this._groupWeights = new System.Windows.Forms.GroupBox();
            this._btnAddWeight = new System.Windows.Forms.Button();
            this._txtWeight = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._listWeights = new System.Windows.Forms.ListBox();
            this.pprpaperweightBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._dsAdministration = new CatalogEstimating.Datasets.Administration();
            this._btnSave = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._groupGrades = new System.Windows.Forms.GroupBox();
            this._btnAddGrade = new System.Windows.Forms.Button();
            this._txtGrade = new System.Windows.Forms.TextBox();
            this._listGrades = new System.Windows.Forms.ListBox();
            this.pprpapergradeBindingSource = new System.Windows.Forms.BindingSource( this.components );
            this._Errors = new System.Windows.Forms.ErrorProvider( this.components );
            this._groupWeights.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.pprpaperweightBindingSource ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this._dsAdministration ) ).BeginInit();
            this._groupGrades.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.pprpapergradeBindingSource ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this._Errors ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _groupWeights
            // 
            this._groupWeights.Controls.Add( this._btnAddWeight );
            this._groupWeights.Controls.Add( this._txtWeight );
            this._groupWeights.Controls.Add( this._listWeights );
            this._groupWeights.Location = new System.Drawing.Point( 12, 12 );
            this._groupWeights.Name = "_groupWeights";
            this._groupWeights.Size = new System.Drawing.Size( 200, 212 );
            this._groupWeights.TabIndex = 0;
            this._groupWeights.TabStop = false;
            this._groupWeights.Text = "Weights";
            // 
            // _btnAddWeight
            // 
            this._btnAddWeight.Enabled = false;
            this._btnAddWeight.Location = new System.Drawing.Point( 130, 177 );
            this._btnAddWeight.Name = "_btnAddWeight";
            this._btnAddWeight.Size = new System.Drawing.Size( 59, 29 );
            this._btnAddWeight.TabIndex = 3;
            this._btnAddWeight.Text = "&Add";
            this._btnAddWeight.UseVisualStyleBackColor = true;
            this._btnAddWeight.Click += new System.EventHandler( this._btnAddWeight_Click );
            // 
            // _txtWeight
            // 
            this._txtWeight.AllowNegative = false;
            this._txtWeight.FlashColor = System.Drawing.Color.Red;
            this._txtWeight.Location = new System.Drawing.Point( 8, 182 );
            this._txtWeight.MaxLength = 9;
            this._txtWeight.Name = "_txtWeight";
            this._txtWeight.Size = new System.Drawing.Size( 99, 20 );
            this._txtWeight.TabIndex = 1;
            this._txtWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtWeight.Value = null;
            this._txtWeight.TextChanged += new System.EventHandler( this._txtWeight_TextChanged );
            // 
            // _listWeights
            // 
            this._listWeights.DataSource = this.pprpaperweightBindingSource;
            this._listWeights.DisplayMember = "weight";
            this._listWeights.FormatString = "N0";
            this._listWeights.FormattingEnabled = true;
            this._listWeights.Location = new System.Drawing.Point( 8, 16 );
            this._listWeights.Name = "_listWeights";
            this._listWeights.Size = new System.Drawing.Size( 181, 160 );
            this._listWeights.TabIndex = 0;
            this._listWeights.ValueMember = "ppr_paperweight_id";
            // 
            // pprpaperweightBindingSource
            // 
            this.pprpaperweightBindingSource.DataMember = "ppr_paperweight";
            this.pprpaperweightBindingSource.DataSource = this._dsAdministration;
            this.pprpaperweightBindingSource.Sort = "weight ASC";
            // 
            // _dsAdministration
            // 
            this._dsAdministration.DataSetName = "Administration";
            this._dsAdministration.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _btnSave
            // 
            this._btnSave.CausesValidation = false;
            this._btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnSave.Location = new System.Drawing.Point( 240, 239 );
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size( 80, 29 );
            this._btnSave.TabIndex = 1;
            this._btnSave.Text = "&Save";
            this._btnSave.UseVisualStyleBackColor = true;
            this._btnSave.Click += new System.EventHandler( this._btnSave_Click );
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point( 344, 239 );
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size( 80, 29 );
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "&Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // _groupGrades
            // 
            this._groupGrades.Controls.Add( this._btnAddGrade );
            this._groupGrades.Controls.Add( this._txtGrade );
            this._groupGrades.Controls.Add( this._listGrades );
            this._groupGrades.Location = new System.Drawing.Point( 224, 12 );
            this._groupGrades.Name = "_groupGrades";
            this._groupGrades.Size = new System.Drawing.Size( 200, 212 );
            this._groupGrades.TabIndex = 4;
            this._groupGrades.TabStop = false;
            this._groupGrades.Text = "Grades";
            // 
            // _btnAddGrade
            // 
            this._btnAddGrade.Enabled = false;
            this._btnAddGrade.Location = new System.Drawing.Point( 130, 177 );
            this._btnAddGrade.Name = "_btnAddGrade";
            this._btnAddGrade.Size = new System.Drawing.Size( 59, 29 );
            this._btnAddGrade.TabIndex = 3;
            this._btnAddGrade.Text = "&Add";
            this._btnAddGrade.UseVisualStyleBackColor = true;
            this._btnAddGrade.Click += new System.EventHandler( this._btnAddGrade_Click );
            // 
            // _txtGrade
            // 
            this._txtGrade.Location = new System.Drawing.Point( 8, 182 );
            this._txtGrade.Name = "_txtGrade";
            this._txtGrade.Size = new System.Drawing.Size( 99, 20 );
            this._txtGrade.TabIndex = 1;
            this._txtGrade.TextChanged += new System.EventHandler( this._txtGrade_TextChanged );
            // 
            // _listGrades
            // 
            this._listGrades.DataSource = this.pprpapergradeBindingSource;
            this._listGrades.DisplayMember = "grade";
            this._listGrades.FormattingEnabled = true;
            this._listGrades.Location = new System.Drawing.Point( 8, 16 );
            this._listGrades.Name = "_listGrades";
            this._listGrades.Size = new System.Drawing.Size( 181, 160 );
            this._listGrades.TabIndex = 0;
            this._listGrades.ValueMember = "ppr_papergrade_id";
            // 
            // pprpapergradeBindingSource
            // 
            this.pprpapergradeBindingSource.DataMember = "ppr_papergrade";
            this.pprpapergradeBindingSource.DataSource = this._dsAdministration;
            this.pprpapergradeBindingSource.Sort = "grade ASC";
            // 
            // _Errors
            // 
            this._Errors.ContainerControl = this;
            // 
            // PaperMgmtDialog
            // 
            this.AcceptButton = this._btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size( 436, 278 );
            this.Controls.Add( this._groupGrades );
            this.Controls.Add( this._btnCancel );
            this.Controls.Add( this._btnSave );
            this.Controls.Add( this._groupWeights );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PaperMgmtDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Paper Management";
            this._groupWeights.ResumeLayout( false );
            this._groupWeights.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.pprpaperweightBindingSource ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this._dsAdministration ) ).EndInit();
            this._groupGrades.ResumeLayout( false );
            this._groupGrades.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.pprpapergradeBindingSource ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this._Errors ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox _groupWeights;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtWeight;
        private System.Windows.Forms.ListBox _listWeights;
        private System.Windows.Forms.Button _btnAddWeight;
        private System.Windows.Forms.Button _btnSave;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.GroupBox _groupGrades;
        private System.Windows.Forms.Button _btnAddGrade;
        private System.Windows.Forms.TextBox _txtGrade;
        private System.Windows.Forms.ListBox _listGrades;
        private CatalogEstimating.Datasets.Administration _dsAdministration;
        private System.Windows.Forms.BindingSource pprpaperweightBindingSource;
        private System.Windows.Forms.BindingSource pprpapergradeBindingSource;
        private System.Windows.Forms.ErrorProvider _Errors;
    }
}