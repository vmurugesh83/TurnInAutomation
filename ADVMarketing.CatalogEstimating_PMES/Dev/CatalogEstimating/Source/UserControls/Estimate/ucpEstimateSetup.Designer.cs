namespace CatalogEstimating.UserControls.Estimate
{
    partial class ucpEstimateSetup
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
            this._lblDescription = new System.Windows.Forms.Label();
            this._txtDescription = new System.Windows.Forms.TextBox();
            this._lblComments = new System.Windows.Forms.Label();
            this._txtComments = new System.Windows.Forms.TextBox();
            this._lblRunDate = new System.Windows.Forms.Label();
            this._dtRunDate = new System.Windows.Forms.DateTimePicker();
            this._lblFiscalMonth = new System.Windows.Forms.Label();
            this._lblFiscalYear = new System.Windows.Forms.Label();
            this._lblSeason = new System.Windows.Forms.Label();
            this._txtFiscalMonth = new System.Windows.Forms.TextBox();
            this._txtFiscalYear = new System.Windows.Forms.TextBox();
            this._txtSeason = new System.Windows.Forms.TextBox();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // _lblDescription
            // 
            this._lblDescription.AutoSize = true;
            this._lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblDescription.Location = new System.Drawing.Point(16, 10);
            this._lblDescription.Name = "_lblDescription";
            this._lblDescription.Size = new System.Drawing.Size(93, 13);
            this._lblDescription.TabIndex = 0;
            this._lblDescription.Text = "Estimate Desc*";
            // 
            // _txtDescription
            // 
            this._txtDescription.Location = new System.Drawing.Point(121, 7);
            this._txtDescription.MaxLength = 35;
            this._txtDescription.Name = "_txtDescription";
            this._txtDescription.ReadOnly = true;
            this._txtDescription.Size = new System.Drawing.Size(265, 20);
            this._txtDescription.TabIndex = 1;
            this._txtDescription.Validated += new System.EventHandler(this._txtDescription_Validated);
            this._txtDescription.Validating += new System.ComponentModel.CancelEventHandler(this._txtDescription_Validating);
            this._txtDescription.TextChanged += new System.EventHandler(this._txtDescription_TextChanged);
            // 
            // _lblComments
            // 
            this._lblComments.AutoSize = true;
            this._lblComments.Location = new System.Drawing.Point(16, 45);
            this._lblComments.Name = "_lblComments";
            this._lblComments.Size = new System.Drawing.Size(99, 13);
            this._lblComments.TabIndex = 2;
            this._lblComments.Text = "Estimate Comments";
            // 
            // _txtComments
            // 
            this._txtComments.Location = new System.Drawing.Point(121, 42);
            this._txtComments.MaxLength = 255;
            this._txtComments.Multiline = true;
            this._txtComments.Name = "_txtComments";
            this._txtComments.ReadOnly = true;
            this._txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtComments.Size = new System.Drawing.Size(534, 91);
            this._txtComments.TabIndex = 3;
            this._txtComments.TextChanged += new System.EventHandler(this._txtDescription_TextChanged);
            // 
            // _lblRunDate
            // 
            this._lblRunDate.AutoSize = true;
            this._lblRunDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblRunDate.Location = new System.Drawing.Point(16, 146);
            this._lblRunDate.Name = "_lblRunDate";
            this._lblRunDate.Size = new System.Drawing.Size(66, 13);
            this._lblRunDate.TabIndex = 4;
            this._lblRunDate.Text = "Run Date*";
            // 
            // _dtRunDate
            // 
            this._dtRunDate.Enabled = false;
            this._dtRunDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtRunDate.Location = new System.Drawing.Point(121, 142);
            this._dtRunDate.Name = "_dtRunDate";
            this._dtRunDate.Size = new System.Drawing.Size(155, 20);
            this._dtRunDate.TabIndex = 5;
            this._dtRunDate.ValueChanged += new System.EventHandler(this._dtRunDate_ValueChanged);
            // 
            // _lblFiscalMonth
            // 
            this._lblFiscalMonth.AutoSize = true;
            this._lblFiscalMonth.Location = new System.Drawing.Point(16, 173);
            this._lblFiscalMonth.Name = "_lblFiscalMonth";
            this._lblFiscalMonth.Size = new System.Drawing.Size(67, 13);
            this._lblFiscalMonth.TabIndex = 6;
            this._lblFiscalMonth.Text = "Fiscal Month";
            // 
            // _lblFiscalYear
            // 
            this._lblFiscalYear.AutoSize = true;
            this._lblFiscalYear.Location = new System.Drawing.Point(16, 198);
            this._lblFiscalYear.Name = "_lblFiscalYear";
            this._lblFiscalYear.Size = new System.Drawing.Size(59, 13);
            this._lblFiscalYear.TabIndex = 7;
            this._lblFiscalYear.Text = "Fiscal Year";
            // 
            // _lblSeason
            // 
            this._lblSeason.AutoSize = true;
            this._lblSeason.Location = new System.Drawing.Point(16, 224);
            this._lblSeason.Name = "_lblSeason";
            this._lblSeason.Size = new System.Drawing.Size(43, 13);
            this._lblSeason.TabIndex = 8;
            this._lblSeason.Text = "Season";
            // 
            // _txtFiscalMonth
            // 
            this._txtFiscalMonth.Location = new System.Drawing.Point(121, 170);
            this._txtFiscalMonth.Name = "_txtFiscalMonth";
            this._txtFiscalMonth.ReadOnly = true;
            this._txtFiscalMonth.Size = new System.Drawing.Size(155, 20);
            this._txtFiscalMonth.TabIndex = 9;
            this._txtFiscalMonth.TabStop = false;
            // 
            // _txtFiscalYear
            // 
            this._txtFiscalYear.Location = new System.Drawing.Point(121, 195);
            this._txtFiscalYear.Name = "_txtFiscalYear";
            this._txtFiscalYear.ReadOnly = true;
            this._txtFiscalYear.Size = new System.Drawing.Size(155, 20);
            this._txtFiscalYear.TabIndex = 10;
            this._txtFiscalYear.TabStop = false;
            // 
            // _txtSeason
            // 
            this._txtSeason.Location = new System.Drawing.Point(121, 221);
            this._txtSeason.Name = "_txtSeason";
            this._txtSeason.ReadOnly = true;
            this._txtSeason.Size = new System.Drawing.Size(155, 20);
            this._txtSeason.TabIndex = 11;
            this._txtSeason.TabStop = false;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // ucpEstimateSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._txtSeason);
            this.Controls.Add(this._txtFiscalYear);
            this.Controls.Add(this._txtFiscalMonth);
            this.Controls.Add(this._lblSeason);
            this.Controls.Add(this._lblFiscalYear);
            this.Controls.Add(this._lblFiscalMonth);
            this.Controls.Add(this._dtRunDate);
            this.Controls.Add(this._lblRunDate);
            this.Controls.Add(this._txtComments);
            this.Controls.Add(this._lblComments);
            this.Controls.Add(this._txtDescription);
            this.Controls.Add(this._lblDescription);
            this.Name = "ucpEstimateSetup";
            this.Size = new System.Drawing.Size(655, 290);
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblDescription;
        private System.Windows.Forms.TextBox _txtDescription;
        private System.Windows.Forms.Label _lblComments;
        private System.Windows.Forms.TextBox _txtComments;
        private System.Windows.Forms.Label _lblRunDate;
        private System.Windows.Forms.DateTimePicker _dtRunDate;
        private System.Windows.Forms.Label _lblFiscalMonth;
        private System.Windows.Forms.Label _lblFiscalYear;
        private System.Windows.Forms.Label _lblSeason;
        private System.Windows.Forms.TextBox _txtFiscalMonth;
        private System.Windows.Forms.TextBox _txtFiscalYear;
        private System.Windows.Forms.TextBox _txtSeason;
        private System.Windows.Forms.ErrorProvider _errorProvider;
    }
}
