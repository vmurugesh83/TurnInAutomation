namespace CatalogEstimating.UserControls.Main
{
    partial class ucpEstimateUpload
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnValidate = new System.Windows.Forms.ToolStripButton();
            this._btnUpload = new System.Windows.Forms.ToolStripButton();
            this._btnRefresh = new System.Windows.Forms.ToolStripButton();
            this._btnPrint = new System.Windows.Forms.ToolStripButton();
            this._gridUpload = new System.Windows.Forms.DataGridView();
            this.EST_Estimate_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Parent_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EST_RunDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EST_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ESTc_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AdNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._lblUploadStatus = new System.Windows.Forms.Label();
            this._toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridUpload)).BeginInit();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnValidate,
            this._btnUpload,
            this._btnRefresh,
            this._btnPrint});
            this._toolStrip.Location = new System.Drawing.Point(77, 508);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(104, 25);
            this._toolStrip.TabIndex = 3;
            // 
            // _btnValidate
            // 
            this._btnValidate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnValidate.Image = global::CatalogEstimating.Properties.Resources.Validate;
            this._btnValidate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnValidate.Name = "_btnValidate";
            this._btnValidate.Size = new System.Drawing.Size(23, 22);
            this._btnValidate.Text = "Validate";
            this._btnValidate.Click += new System.EventHandler(this._btnValidate_Click);
            // 
            // _btnUpload
            // 
            this._btnUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnUpload.Image = global::CatalogEstimating.Properties.Resources.Upload;
            this._btnUpload.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnUpload.Name = "_btnUpload";
            this._btnUpload.Size = new System.Drawing.Size(23, 22);
            this._btnUpload.Text = "Upload";
            this._btnUpload.Click += new System.EventHandler(this._btnUpload_Click);
            // 
            // _btnRefresh
            // 
            this._btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnRefresh.Image = global::CatalogEstimating.Properties.Resources.Refresh;
            this._btnRefresh.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.Size = new System.Drawing.Size(23, 22);
            this._btnRefresh.Text = "Refresh";
            // 
            // _btnPrint
            // 
            this._btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnPrint.Image = global::CatalogEstimating.Properties.Resources.Print;
            this._btnPrint.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnPrint.Name = "_btnPrint";
            this._btnPrint.Size = new System.Drawing.Size(23, 22);
            this._btnPrint.Text = "Print";
            // 
            // _gridUpload
            // 
            this._gridUpload.AllowUserToAddRows = false;
            this._gridUpload.AllowUserToDeleteRows = false;
            this._gridUpload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._gridUpload.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridUpload.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._gridUpload.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridUpload.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EST_Estimate_ID,
            this.Parent_ID,
            this.EST_RunDate,
            this.EST_Description,
            this.ESTc_Description,
            this.AdNumber,
            this.StatusCode,
            this.Message});
            this._gridUpload.Location = new System.Drawing.Point(5, 5);
            this._gridUpload.Name = "_gridUpload";
            this._gridUpload.ReadOnly = true;
            this._gridUpload.RowHeadersVisible = false;
            this._gridUpload.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridUpload.Size = new System.Drawing.Size(725, 500);
            this._gridUpload.TabIndex = 4;
            this._gridUpload.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._gridUpload_CellDoubleClick);
            // 
            // EST_Estimate_ID
            // 
            this.EST_Estimate_ID.HeaderText = "EST_Estimate_ID";
            this.EST_Estimate_ID.Name = "EST_Estimate_ID";
            this.EST_Estimate_ID.ReadOnly = true;
            this.EST_Estimate_ID.Visible = false;
            // 
            // Parent_ID
            // 
            this.Parent_ID.HeaderText = "Parent_ID";
            this.Parent_ID.Name = "Parent_ID";
            this.Parent_ID.ReadOnly = true;
            this.Parent_ID.Visible = false;
            // 
            // EST_RunDate
            // 
            this.EST_RunDate.FillWeight = 75F;
            this.EST_RunDate.HeaderText = "Est Run Date";
            this.EST_RunDate.Name = "EST_RunDate";
            this.EST_RunDate.ReadOnly = true;
            // 
            // EST_Description
            // 
            this.EST_Description.HeaderText = "Est Desc";
            this.EST_Description.Name = "EST_Description";
            this.EST_Description.ReadOnly = true;
            // 
            // ESTc_Description
            // 
            this.ESTc_Description.HeaderText = "Comp Desc";
            this.ESTc_Description.Name = "ESTc_Description";
            this.ESTc_Description.ReadOnly = true;
            // 
            // AdNumber
            // 
            this.AdNumber.FillWeight = 50F;
            this.AdNumber.HeaderText = "Ad Number";
            this.AdNumber.Name = "AdNumber";
            this.AdNumber.ReadOnly = true;
            // 
            // StatusCode
            // 
            this.StatusCode.HeaderText = "StatusCode";
            this.StatusCode.Name = "StatusCode";
            this.StatusCode.ReadOnly = true;
            this.StatusCode.Visible = false;
            // 
            // Message
            // 
            this.Message.HeaderText = "Message";
            this.Message.Name = "Message";
            this.Message.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "EST_Estimate_ID";
            this.dataGridViewTextBoxColumn1.HeaderText = "Estimate ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 120;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "EST_Description";
            this.dataGridViewTextBoxColumn2.FillWeight = 75F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Estimate Desc";
            this.dataGridViewTextBoxColumn2.MaxInputLength = 35;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 121;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "EST_Rundate";
            this.dataGridViewTextBoxColumn3.HeaderText = "Estimate Run Date";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 120;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "ESTc_Description";
            this.dataGridViewTextBoxColumn4.HeaderText = "Component Desc";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 120;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "AdNumber";
            this.dataGridViewTextBoxColumn5.FillWeight = 50F;
            this.dataGridViewTextBoxColumn5.HeaderText = "Ad Number";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 121;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "StatusCode";
            this.dataGridViewTextBoxColumn6.HeaderText = "Status";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 120;
            // 
            // _progressBar
            // 
            this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._progressBar.Location = new System.Drawing.Point(8, 511);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(722, 22);
            this._progressBar.TabIndex = 5;
            // 
            // _lblUploadStatus
            // 
            this._lblUploadStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lblUploadStatus.Location = new System.Drawing.Point(8, 536);
            this._lblUploadStatus.Name = "_lblUploadStatus";
            this._lblUploadStatus.Size = new System.Drawing.Size(721, 22);
            this._lblUploadStatus.TabIndex = 6;
            // 
            // ucpEstimateUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoSize = true;
            this.Controls.Add(this._lblUploadStatus);
            this.Controls.Add(this._toolStrip);
            this.Controls.Add(this._progressBar);
            this.Controls.Add(this._gridUpload);
            this.Name = "ucpEstimateUpload";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(763, 563);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridUpload)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnValidate;
        private System.Windows.Forms.ToolStripButton _btnUpload;
        private System.Windows.Forms.ToolStripButton _btnRefresh;
        private System.Windows.Forms.ToolStripButton _btnPrint;
        private System.Windows.Forms.DataGridView _gridUpload;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn EST_Estimate_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parent_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn EST_RunDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn EST_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn ESTc_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn AdNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Label _lblUploadStatus;
    }
}
