namespace CatalogEstimating.UserControls.Admin
{
    partial class ucpAdminInsertionScenarios
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
            this._lstScenarios = new System.Windows.Forms.ListBox();
            this._lblScenarios = new System.Windows.Forms.Label();
            this._groupMapping = new System.Windows.Forms.GroupBox();
            this._lblScenarioInfo = new System.Windows.Forms.Label();
            this._lblGroupsInScenario = new System.Windows.Forms.Label();
            this._lstGroupsInScenario = new System.Windows.Forms.ListBox();
            this._btnRemoveAll = new System.Windows.Forms.Button();
            this._btnRemove = new System.Windows.Forms.Button();
            this._btnAddAll = new System.Windows.Forms.Button();
            this._btnAdd = new System.Windows.Forms.Button();
            this._lblAvailableGroups = new System.Windows.Forms.Label();
            this._lstAvailableGroups = new System.Windows.Forms.ListBox();
            this._chkActive = new System.Windows.Forms.CheckBox();
            this._txtComments = new System.Windows.Forms.TextBox();
            this._lblComments = new System.Windows.Forms.Label();
            this._txtDescription = new System.Windows.Forms.TextBox();
            this._lblDescription = new System.Windows.Forms.Label();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnNew = new System.Windows.Forms.ToolStripButton();
            this._btnDelete = new System.Windows.Forms.ToolStripButton();
            this._dsPublications = new CatalogEstimating.Datasets.Publications();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._btnClear = new System.Windows.Forms.Button();
            this._groupMapping.SuspendLayout();
            this._toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dsPublications)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // _lstScenarios
            // 
            this._lstScenarios.FormattingEnabled = true;
            this._lstScenarios.Location = new System.Drawing.Point(31, 26);
            this._lstScenarios.Name = "_lstScenarios";
            this._lstScenarios.Size = new System.Drawing.Size(240, 82);
            this._lstScenarios.TabIndex = 0;
            this._lstScenarios.SelectedValueChanged += new System.EventHandler(this._lstScenarios_SelectedValueChanged);
            // 
            // _lblScenarios
            // 
            this._lblScenarios.AutoSize = true;
            this._lblScenarios.Location = new System.Drawing.Point(28, 10);
            this._lblScenarios.Name = "_lblScenarios";
            this._lblScenarios.Size = new System.Drawing.Size(54, 13);
            this._lblScenarios.TabIndex = 1;
            this._lblScenarios.Text = "Scenarios";
            // 
            // _groupMapping
            // 
            this._groupMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._groupMapping.Controls.Add(this._btnClear);
            this._groupMapping.Controls.Add(this._lblScenarioInfo);
            this._groupMapping.Controls.Add(this._lblGroupsInScenario);
            this._groupMapping.Controls.Add(this._lstGroupsInScenario);
            this._groupMapping.Controls.Add(this._btnRemoveAll);
            this._groupMapping.Controls.Add(this._btnRemove);
            this._groupMapping.Controls.Add(this._btnAddAll);
            this._groupMapping.Controls.Add(this._btnAdd);
            this._groupMapping.Controls.Add(this._lblAvailableGroups);
            this._groupMapping.Controls.Add(this._lstAvailableGroups);
            this._groupMapping.Controls.Add(this._chkActive);
            this._groupMapping.Controls.Add(this._txtComments);
            this._groupMapping.Controls.Add(this._lblComments);
            this._groupMapping.Controls.Add(this._txtDescription);
            this._groupMapping.Controls.Add(this._lblDescription);
            this._groupMapping.Location = new System.Drawing.Point(15, 114);
            this._groupMapping.Name = "_groupMapping";
            this._groupMapping.Size = new System.Drawing.Size(678, 396);
            this._groupMapping.TabIndex = 3;
            this._groupMapping.TabStop = false;
            this._groupMapping.Text = "Scenario Mapping";
            // 
            // _lblScenarioInfo
            // 
            this._lblScenarioInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblScenarioInfo.ForeColor = System.Drawing.Color.Blue;
            this._lblScenarioInfo.Location = new System.Drawing.Point(7, 20);
            this._lblScenarioInfo.Name = "_lblScenarioInfo";
            this._lblScenarioInfo.Size = new System.Drawing.Size(665, 23);
            this._lblScenarioInfo.TabIndex = 27;
            // 
            // _lblGroupsInScenario
            // 
            this._lblGroupsInScenario.AutoSize = true;
            this._lblGroupsInScenario.Location = new System.Drawing.Point(418, 167);
            this._lblGroupsInScenario.Name = "_lblGroupsInScenario";
            this._lblGroupsInScenario.Size = new System.Drawing.Size(98, 13);
            this._lblGroupsInScenario.TabIndex = 25;
            this._lblGroupsInScenario.Text = "Groups In Scenario";
            // 
            // _lstGroupsInScenario
            // 
            this._lstGroupsInScenario.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this._lstGroupsInScenario.FormattingEnabled = true;
            this._lstGroupsInScenario.Location = new System.Drawing.Point(421, 183);
            this._lstGroupsInScenario.Name = "_lstGroupsInScenario";
            this._lstGroupsInScenario.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._lstGroupsInScenario.Size = new System.Drawing.Size(240, 199);
            this._lstGroupsInScenario.TabIndex = 26;
            // 
            // _btnRemoveAll
            // 
            this._btnRemoveAll.Enabled = false;
            this._btnRemoveAll.Location = new System.Drawing.Point(290, 312);
            this._btnRemoveAll.Name = "_btnRemoveAll";
            this._btnRemoveAll.Size = new System.Drawing.Size(96, 29);
            this._btnRemoveAll.TabIndex = 24;
            this._btnRemoveAll.Text = "<< Remove All";
            this._btnRemoveAll.UseVisualStyleBackColor = true;
            this._btnRemoveAll.Click += new System.EventHandler(this._btnRemoveAll_Click);
            // 
            // _btnRemove
            // 
            this._btnRemove.Enabled = false;
            this._btnRemove.Location = new System.Drawing.Point(290, 277);
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.Size = new System.Drawing.Size(94, 29);
            this._btnRemove.TabIndex = 23;
            this._btnRemove.Text = "< Remove";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += new System.EventHandler(this._btnRemove_Click);
            // 
            // _btnAddAll
            // 
            this._btnAddAll.Enabled = false;
            this._btnAddAll.Location = new System.Drawing.Point(290, 218);
            this._btnAddAll.Name = "_btnAddAll";
            this._btnAddAll.Size = new System.Drawing.Size(94, 29);
            this._btnAddAll.TabIndex = 22;
            this._btnAddAll.Text = "Add All >>";
            this._btnAddAll.UseVisualStyleBackColor = true;
            this._btnAddAll.Click += new System.EventHandler(this._btnAddAll_Click);
            // 
            // _btnAdd
            // 
            this._btnAdd.Enabled = false;
            this._btnAdd.Location = new System.Drawing.Point(290, 183);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(96, 29);
            this._btnAdd.TabIndex = 21;
            this._btnAdd.Text = "Add >";
            this._btnAdd.UseVisualStyleBackColor = true;
            this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
            // 
            // _lblAvailableGroups
            // 
            this._lblAvailableGroups.AutoSize = true;
            this._lblAvailableGroups.Location = new System.Drawing.Point(13, 167);
            this._lblAvailableGroups.Name = "_lblAvailableGroups";
            this._lblAvailableGroups.Size = new System.Drawing.Size(87, 13);
            this._lblAvailableGroups.TabIndex = 10;
            this._lblAvailableGroups.Text = "Available Groups";
            // 
            // _lstAvailableGroups
            // 
            this._lstAvailableGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this._lstAvailableGroups.FormattingEnabled = true;
            this._lstAvailableGroups.Location = new System.Drawing.Point(16, 183);
            this._lstAvailableGroups.Name = "_lstAvailableGroups";
            this._lstAvailableGroups.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._lstAvailableGroups.Size = new System.Drawing.Size(240, 199);
            this._lstAvailableGroups.TabIndex = 11;
            // 
            // _chkActive
            // 
            this._chkActive.AutoSize = true;
            this._chkActive.Enabled = false;
            this._chkActive.Location = new System.Drawing.Point(421, 45);
            this._chkActive.Name = "_chkActive";
            this._chkActive.Size = new System.Drawing.Size(56, 17);
            this._chkActive.TabIndex = 9;
            this._chkActive.Text = "Active";
            this._chkActive.UseVisualStyleBackColor = true;
            this._chkActive.CheckedChanged += new System.EventHandler(this.Control_Changed);
            // 
            // _txtComments
            // 
            this._txtComments.Location = new System.Drawing.Point(79, 76);
            this._txtComments.MaxLength = 255;
            this._txtComments.Multiline = true;
            this._txtComments.Name = "_txtComments";
            this._txtComments.ReadOnly = true;
            this._txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtComments.Size = new System.Drawing.Size(582, 70);
            this._txtComments.TabIndex = 8;
            this._txtComments.TextChanged += new System.EventHandler(this.Control_Changed);
            // 
            // _lblComments
            // 
            this._lblComments.AutoSize = true;
            this._lblComments.Location = new System.Drawing.Point(13, 76);
            this._lblComments.Name = "_lblComments";
            this._lblComments.Size = new System.Drawing.Size(56, 13);
            this._lblComments.TabIndex = 7;
            this._lblComments.Text = "Comments";
            // 
            // _txtDescription
            // 
            this._txtDescription.Location = new System.Drawing.Point(79, 43);
            this._txtDescription.MaxLength = 35;
            this._txtDescription.Name = "_txtDescription";
            this._txtDescription.ReadOnly = true;
            this._txtDescription.Size = new System.Drawing.Size(305, 20);
            this._txtDescription.TabIndex = 6;
            this._txtDescription.TextChanged += new System.EventHandler(this.Control_Changed);
            // 
            // _lblDescription
            // 
            this._lblDescription.AutoSize = true;
            this._lblDescription.Location = new System.Drawing.Point(13, 46);
            this._lblDescription.Name = "_lblDescription";
            this._lblDescription.Size = new System.Drawing.Size(60, 13);
            this._lblDescription.TabIndex = 5;
            this._lblDescription.Text = "Description";
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnNew,
            this._btnDelete});
            this._toolStrip.Location = new System.Drawing.Point(0, 284);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(58, 25);
            this._toolStrip.TabIndex = 27;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _btnNew
            // 
            this._btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnNew.Enabled = false;
            this._btnNew.Image = global::CatalogEstimating.Properties.Resources.NewEstimate;
            this._btnNew.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnNew.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this._btnNew.MergeIndex = 0;
            this._btnNew.Name = "_btnNew";
            this._btnNew.Size = new System.Drawing.Size(23, 22);
            this._btnNew.Text = "New";
            this._btnNew.Click += new System.EventHandler(this._btnNew_Click);
            // 
            // _btnDelete
            // 
            this._btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnDelete.Image = global::CatalogEstimating.Properties.Resources.Delete;
            this._btnDelete.ImageTransparentColor = System.Drawing.Color.Black;
            this._btnDelete.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this._btnDelete.MergeIndex = 3;
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Size = new System.Drawing.Size(23, 22);
            this._btnDelete.Text = "toolStripButton1";
            this._btnDelete.ToolTipText = "Delete";
            this._btnDelete.Click += new System.EventHandler(this._btnDelete_Click);
            // 
            // _dsPublications
            // 
            this._dsPublications.DataSetName = "Publications";
            this._dsPublications.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // _btnClear
            // 
            this._btnClear.Enabled = false;
            this._btnClear.Location = new System.Drawing.Point(586, 41);
            this._btnClear.Name = "_btnClear";
            this._btnClear.Size = new System.Drawing.Size(75, 23);
            this._btnClear.TabIndex = 28;
            this._btnClear.Text = "Clear";
            this._btnClear.UseVisualStyleBackColor = true;
            this._btnClear.Click += new System.EventHandler(this._btnClear_Click);
            // 
            // ucpAdminInsertionScenarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._toolStrip);
            this.Controls.Add(this._groupMapping);
            this.Controls.Add(this._lblScenarios);
            this.Controls.Add(this._lstScenarios);
            this.Name = "ucpAdminInsertionScenarios";
            this.Size = new System.Drawing.Size(709, 526);
            this.Load += new System.EventHandler(this.ucpAdminInsertionScenarios_Load);
            this._groupMapping.ResumeLayout(false);
            this._groupMapping.PerformLayout();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dsPublications)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox _lstScenarios;
        private System.Windows.Forms.Label _lblScenarios;
        private System.Windows.Forms.GroupBox _groupMapping;
        private System.Windows.Forms.CheckBox _chkActive;
        private System.Windows.Forms.TextBox _txtComments;
        private System.Windows.Forms.Label _lblComments;
        private System.Windows.Forms.TextBox _txtDescription;
        private System.Windows.Forms.Label _lblDescription;
        private System.Windows.Forms.Label _lblAvailableGroups;
        private System.Windows.Forms.ListBox _lstAvailableGroups;
        private System.Windows.Forms.Button _btnRemoveAll;
        private System.Windows.Forms.Button _btnRemove;
        private System.Windows.Forms.Button _btnAddAll;
        private System.Windows.Forms.Button _btnAdd;
        private System.Windows.Forms.Label _lblGroupsInScenario;
        private System.Windows.Forms.ListBox _lstGroupsInScenario;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnNew;
        private CatalogEstimating.Datasets.Publications _dsPublications;
        private System.Windows.Forms.ToolStripButton _btnDelete;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.Label _lblScenarioInfo;
        private System.Windows.Forms.Button _btnClear;
    }
}
