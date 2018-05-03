namespace CatalogEstimating.UserControls.Estimate
{
    partial class ucpSamples
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._lblQuantity = new System.Windows.Forms.Label();
            this._lblFreightCWT = new System.Windows.Forms.Label();
            this._lblFreightFlat = new System.Windows.Forms.Label();
            this._txtQuantity = new CatalogEstimating.CustomControls.IntegerTextBox();
            this._txtFreightCWT = new CatalogEstimating.CustomControls.DecimalTextBox();
            this._txtFreightFlat = new CatalogEstimating.CustomControls.DecimalTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // _lblQuantity
            // 
            this._lblQuantity.AutoSize = true;
            this._lblQuantity.Location = new System.Drawing.Point(12, 9);
            this._lblQuantity.Name = "_lblQuantity";
            this._lblQuantity.Size = new System.Drawing.Size(46, 13);
            this._lblQuantity.TabIndex = 2;
            this._lblQuantity.Text = "Quantity";
            // 
            // _lblFreightCWT
            // 
            this._lblFreightCWT.AutoSize = true;
            this._lblFreightCWT.Location = new System.Drawing.Point(12, 36);
            this._lblFreightCWT.Name = "_lblFreightCWT";
            this._lblFreightCWT.Size = new System.Drawing.Size(73, 13);
            this._lblFreightCWT.TabIndex = 3;
            this._lblFreightCWT.Text = "Freight (CWT)";
            // 
            // _lblFreightFlat
            // 
            this._lblFreightFlat.AutoSize = true;
            this._lblFreightFlat.Location = new System.Drawing.Point(12, 62);
            this._lblFreightFlat.Name = "_lblFreightFlat";
            this._lblFreightFlat.Size = new System.Drawing.Size(65, 13);
            this._lblFreightFlat.TabIndex = 4;
            this._lblFreightFlat.Text = "Freight (Flat)";
            // 
            // _txtQuantity
            // 
            this._txtQuantity.AllowNegative = false;
            this._txtQuantity.FlashColor = System.Drawing.Color.Red;
            this._txtQuantity.Location = new System.Drawing.Point(125, 6);
            this._txtQuantity.Name = "_txtQuantity";
            this._txtQuantity.Size = new System.Drawing.Size(144, 20);
            this._txtQuantity.TabIndex = 0;
            this._txtQuantity.Value = null;
            this._txtQuantity.TextChanged += new System.EventHandler(this._txtQuantity_TextChanged);
            // 
            // _txtFreightCWT
            // 
            this._txtFreightCWT.AllowNegative = false;
            this._txtFreightCWT.FlashColor = System.Drawing.Color.Red;
            this._txtFreightCWT.Location = new System.Drawing.Point(125, 33);
            this._txtFreightCWT.Name = "_txtFreightCWT";
            this._txtFreightCWT.Size = new System.Drawing.Size(144, 20);
            this._txtFreightCWT.TabIndex = 1;
            this._txtFreightCWT.Value = null;
            this._txtFreightCWT.TextChanged += new System.EventHandler(this._txtFreightCWT_TextChanged);
            // 
            // _txtFreightFlat
            // 
            this._txtFreightFlat.AllowNegative = false;
            this._txtFreightFlat.FlashColor = System.Drawing.Color.Red;
            this._txtFreightFlat.Location = new System.Drawing.Point(125, 59);
            this._txtFreightFlat.Name = "_txtFreightFlat";
            this._txtFreightFlat.Size = new System.Drawing.Size(144, 20);
            this._txtFreightFlat.TabIndex = 2;
            this._txtFreightFlat.Value = null;
            this._txtFreightFlat.TextChanged += new System.EventHandler(this._txtFreightFlat_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "$";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "$";
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // ucpSamples
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._lblFreightFlat);
            this.Controls.Add(this._lblFreightCWT);
            this.Controls.Add(this._txtFreightFlat);
            this.Controls.Add(this._txtFreightCWT);
            this.Controls.Add(this._txtQuantity);
            this.Controls.Add(this._lblQuantity);
            this.Name = "ucpSamples";
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label _lblQuantity;
        private System.Windows.Forms.Label _lblFreightCWT;
        private System.Windows.Forms.Label _lblFreightFlat;
        private CatalogEstimating.CustomControls.IntegerTextBox _txtQuantity;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtFreightCWT;
        private CatalogEstimating.CustomControls.DecimalTextBox _txtFreightFlat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider _errorProvider;
    }
}
