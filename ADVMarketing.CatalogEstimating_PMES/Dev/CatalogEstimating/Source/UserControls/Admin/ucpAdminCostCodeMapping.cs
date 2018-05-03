using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CatalogEstimating.UserControls.Admin
{
    public partial class ucpAdminCostCodeMapping : CatalogEstimating.UserControlPanel
    {
        public ucpAdminCostCodeMapping()
        {
            InitializeComponent();
            this.Name = "Cost Code Mapping";
        }

        public override ToolStrip Toolbar
        {
            get { return _toolStrip; }
        }
    }
}