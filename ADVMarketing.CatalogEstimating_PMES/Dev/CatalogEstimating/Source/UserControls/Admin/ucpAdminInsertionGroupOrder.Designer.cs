namespace CatalogEstimating.UserControls.Admin
{
    partial class ucpAdminInsertionGroupOrder
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this._gridOrder = new System.Windows.Forms.DataGridView();
            this.pubpubgroupidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.effectivedateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sortorderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customgroupforpackageDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.createdbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedbyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifieddateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._dsPublications = new CatalogEstimating.Datasets.Publications();
            this._cmdTop = new System.Windows.Forms.Button();
            this._cmdUp10 = new System.Windows.Forms.Button();
            this._cmdUp1 = new System.Windows.Forms.Button();
            this._cmdDown1 = new System.Windows.Forms.Button();
            this._cmdDown10 = new System.Windows.Forms.Button();
            this._cmdBottom = new System.Windows.Forms.Button();
            ( (System.ComponentModel.ISupportInitialize)( this._gridOrder ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)( this._dsPublications ) ).BeginInit();
            this.SuspendLayout();
            // 
            // _gridOrder
            // 
            this._gridOrder.AllowUserToAddRows = false;
            this._gridOrder.AllowUserToDeleteRows = false;
            this._gridOrder.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._gridOrder.AutoGenerateColumns = false;
            this._gridOrder.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridOrder.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._gridOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridOrder.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.pubpubgroupidDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.commentsDataGridViewTextBoxColumn,
            this.activeDataGridViewCheckBoxColumn,
            this.effectivedateDataGridViewTextBoxColumn,
            this.sortorderDataGridViewTextBoxColumn,
            this.customgroupforpackageDataGridViewCheckBoxColumn,
            this.createdbyDataGridViewTextBoxColumn,
            this.createddateDataGridViewTextBoxColumn,
            this.modifiedbyDataGridViewTextBoxColumn,
            this.modifieddateDataGridViewTextBoxColumn} );
            this._gridOrder.DataMember = "pub_pubgroup";
            this._gridOrder.DataSource = this._dsPublications;
            this._gridOrder.Location = new System.Drawing.Point( 12, 12 );
            this._gridOrder.MultiSelect = false;
            this._gridOrder.Name = "_gridOrder";
            this._gridOrder.ReadOnly = true;
            this._gridOrder.RowHeadersVisible = false;
            this._gridOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridOrder.Size = new System.Drawing.Size( 514, 430 );
            this._gridOrder.TabIndex = 0;
            this._gridOrder.SelectionChanged += new System.EventHandler( this._gridOrder_SelectionChanged );
            // 
            // pubpubgroupidDataGridViewTextBoxColumn
            // 
            this.pubpubgroupidDataGridViewTextBoxColumn.DataPropertyName = "pub_pubgroup_id";
            this.pubpubgroupidDataGridViewTextBoxColumn.HeaderText = "pub_pubgroup_id";
            this.pubpubgroupidDataGridViewTextBoxColumn.Name = "pubpubgroupidDataGridViewTextBoxColumn";
            this.pubpubgroupidDataGridViewTextBoxColumn.ReadOnly = true;
            this.pubpubgroupidDataGridViewTextBoxColumn.Visible = false;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Pub Group Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // commentsDataGridViewTextBoxColumn
            // 
            this.commentsDataGridViewTextBoxColumn.DataPropertyName = "comments";
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.commentsDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.commentsDataGridViewTextBoxColumn.FillWeight = 125F;
            this.commentsDataGridViewTextBoxColumn.HeaderText = "Pub Group Comments";
            this.commentsDataGridViewTextBoxColumn.Name = "commentsDataGridViewTextBoxColumn";
            this.commentsDataGridViewTextBoxColumn.ReadOnly = true;
            this.commentsDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // activeDataGridViewCheckBoxColumn
            // 
            this.activeDataGridViewCheckBoxColumn.DataPropertyName = "active";
            this.activeDataGridViewCheckBoxColumn.FillWeight = 50F;
            this.activeDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.activeDataGridViewCheckBoxColumn.Name = "activeDataGridViewCheckBoxColumn";
            this.activeDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // effectivedateDataGridViewTextBoxColumn
            // 
            this.effectivedateDataGridViewTextBoxColumn.DataPropertyName = "effectivedate";
            this.effectivedateDataGridViewTextBoxColumn.HeaderText = "effectivedate";
            this.effectivedateDataGridViewTextBoxColumn.Name = "effectivedateDataGridViewTextBoxColumn";
            this.effectivedateDataGridViewTextBoxColumn.ReadOnly = true;
            this.effectivedateDataGridViewTextBoxColumn.Visible = false;
            // 
            // sortorderDataGridViewTextBoxColumn
            // 
            this.sortorderDataGridViewTextBoxColumn.DataPropertyName = "sortorder";
            this.sortorderDataGridViewTextBoxColumn.HeaderText = "sortorder";
            this.sortorderDataGridViewTextBoxColumn.Name = "sortorderDataGridViewTextBoxColumn";
            this.sortorderDataGridViewTextBoxColumn.ReadOnly = true;
            this.sortorderDataGridViewTextBoxColumn.Visible = false;
            // 
            // customgroupforpackageDataGridViewCheckBoxColumn
            // 
            this.customgroupforpackageDataGridViewCheckBoxColumn.DataPropertyName = "customgroupforpackage";
            this.customgroupforpackageDataGridViewCheckBoxColumn.HeaderText = "customgroupforpackage";
            this.customgroupforpackageDataGridViewCheckBoxColumn.Name = "customgroupforpackageDataGridViewCheckBoxColumn";
            this.customgroupforpackageDataGridViewCheckBoxColumn.ReadOnly = true;
            this.customgroupforpackageDataGridViewCheckBoxColumn.Visible = false;
            // 
            // createdbyDataGridViewTextBoxColumn
            // 
            this.createdbyDataGridViewTextBoxColumn.DataPropertyName = "createdby";
            this.createdbyDataGridViewTextBoxColumn.HeaderText = "createdby";
            this.createdbyDataGridViewTextBoxColumn.Name = "createdbyDataGridViewTextBoxColumn";
            this.createdbyDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdbyDataGridViewTextBoxColumn.Visible = false;
            // 
            // createddateDataGridViewTextBoxColumn
            // 
            this.createddateDataGridViewTextBoxColumn.DataPropertyName = "createddate";
            this.createddateDataGridViewTextBoxColumn.HeaderText = "createddate";
            this.createddateDataGridViewTextBoxColumn.Name = "createddateDataGridViewTextBoxColumn";
            this.createddateDataGridViewTextBoxColumn.ReadOnly = true;
            this.createddateDataGridViewTextBoxColumn.Visible = false;
            // 
            // modifiedbyDataGridViewTextBoxColumn
            // 
            this.modifiedbyDataGridViewTextBoxColumn.DataPropertyName = "modifiedby";
            this.modifiedbyDataGridViewTextBoxColumn.HeaderText = "modifiedby";
            this.modifiedbyDataGridViewTextBoxColumn.Name = "modifiedbyDataGridViewTextBoxColumn";
            this.modifiedbyDataGridViewTextBoxColumn.ReadOnly = true;
            this.modifiedbyDataGridViewTextBoxColumn.Visible = false;
            // 
            // modifieddateDataGridViewTextBoxColumn
            // 
            this.modifieddateDataGridViewTextBoxColumn.DataPropertyName = "modifieddate";
            this.modifieddateDataGridViewTextBoxColumn.HeaderText = "modifieddate";
            this.modifieddateDataGridViewTextBoxColumn.Name = "modifieddateDataGridViewTextBoxColumn";
            this.modifieddateDataGridViewTextBoxColumn.ReadOnly = true;
            this.modifieddateDataGridViewTextBoxColumn.Visible = false;
            // 
            // _dsPublications
            // 
            this._dsPublications.DataSetName = "Publications";
            this._dsPublications.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _cmdTop
            // 
            this._cmdTop.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._cmdTop.Enabled = false;
            this._cmdTop.Location = new System.Drawing.Point( 541, 12 );
            this._cmdTop.Name = "_cmdTop";
            this._cmdTop.Size = new System.Drawing.Size( 75, 29 );
            this._cmdTop.TabIndex = 1;
            this._cmdTop.Text = "Top";
            this._cmdTop.UseVisualStyleBackColor = true;
            this._cmdTop.Click += new System.EventHandler( this._cmdTop_Click );
            // 
            // _cmdUp10
            // 
            this._cmdUp10.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._cmdUp10.Enabled = false;
            this._cmdUp10.Location = new System.Drawing.Point( 541, 50 );
            this._cmdUp10.Name = "_cmdUp10";
            this._cmdUp10.Size = new System.Drawing.Size( 75, 29 );
            this._cmdUp10.TabIndex = 2;
            this._cmdUp10.Text = "Up 10";
            this._cmdUp10.UseVisualStyleBackColor = true;
            this._cmdUp10.Click += new System.EventHandler( this._cmdUp10_Click );
            // 
            // _cmdUp1
            // 
            this._cmdUp1.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._cmdUp1.Enabled = false;
            this._cmdUp1.Location = new System.Drawing.Point( 541, 88 );
            this._cmdUp1.Name = "_cmdUp1";
            this._cmdUp1.Size = new System.Drawing.Size( 75, 29 );
            this._cmdUp1.TabIndex = 3;
            this._cmdUp1.Text = "Up 1";
            this._cmdUp1.UseVisualStyleBackColor = true;
            this._cmdUp1.Click += new System.EventHandler( this._cmdUp1_Click );
            // 
            // _cmdDown1
            // 
            this._cmdDown1.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._cmdDown1.Enabled = false;
            this._cmdDown1.Location = new System.Drawing.Point( 541, 337 );
            this._cmdDown1.Name = "_cmdDown1";
            this._cmdDown1.Size = new System.Drawing.Size( 75, 29 );
            this._cmdDown1.TabIndex = 4;
            this._cmdDown1.Text = "Down 1";
            this._cmdDown1.UseVisualStyleBackColor = true;
            this._cmdDown1.Click += new System.EventHandler( this._cmdDown1_Click );
            // 
            // _cmdDown10
            // 
            this._cmdDown10.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._cmdDown10.Enabled = false;
            this._cmdDown10.Location = new System.Drawing.Point( 541, 375 );
            this._cmdDown10.Name = "_cmdDown10";
            this._cmdDown10.Size = new System.Drawing.Size( 75, 29 );
            this._cmdDown10.TabIndex = 5;
            this._cmdDown10.Text = "Down 10";
            this._cmdDown10.UseVisualStyleBackColor = true;
            this._cmdDown10.Click += new System.EventHandler( this._cmdDown10_Click );
            // 
            // _cmdBottom
            // 
            this._cmdBottom.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this._cmdBottom.Enabled = false;
            this._cmdBottom.Location = new System.Drawing.Point( 541, 413 );
            this._cmdBottom.Name = "_cmdBottom";
            this._cmdBottom.Size = new System.Drawing.Size( 75, 29 );
            this._cmdBottom.TabIndex = 6;
            this._cmdBottom.Text = "Bottom";
            this._cmdBottom.UseVisualStyleBackColor = true;
            this._cmdBottom.Click += new System.EventHandler( this._cmdBottom_Click );
            // 
            // ucpAdminInsertionGroupOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add( this._cmdBottom );
            this.Controls.Add( this._cmdDown10 );
            this.Controls.Add( this._cmdDown1 );
            this.Controls.Add( this._cmdUp1 );
            this.Controls.Add( this._cmdUp10 );
            this.Controls.Add( this._cmdTop );
            this.Controls.Add( this._gridOrder );
            this.Name = "ucpAdminInsertionGroupOrder";
            this.Size = new System.Drawing.Size( 631, 457 );
            ( (System.ComponentModel.ISupportInitialize)( this._gridOrder ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)( this._dsPublications ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.DataGridView _gridOrder;
        private System.Windows.Forms.Button _cmdTop;
        private System.Windows.Forms.Button _cmdUp10;
        private System.Windows.Forms.Button _cmdUp1;
        private System.Windows.Forms.Button _cmdDown1;
        private System.Windows.Forms.Button _cmdDown10;
        private System.Windows.Forms.Button _cmdBottom;
        private CatalogEstimating.Datasets.Publications _dsPublications;
        private System.Windows.Forms.DataGridViewTextBoxColumn pubpubgroupidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn effectivedateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sortorderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn customgroupforpackageDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createddateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedbyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifieddateDataGridViewTextBoxColumn;

    }
}
