using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CatalogEstimating.UserControls.Admin
{
    public partial class uctAdminInsertionGroups : CatalogEstimating.UserControlTab
    {
        public uctAdminInsertionGroups(Datasets.Publications ds)
        {
            InitializeComponent();
            Name = "Groups";
            ChildControls.Add( new ucpAdminInsertionGroupSetup(ds) );
            ChildControls.Add( new ucpAdminInsertionGroupOrder(ds) );
        }

        public bool Dirty_AdminInsertionGroupSetup
        {
            get { return ChildControls[0].Dirty; }
            set { ChildControls[0].Dirty = value; }
        }

        public bool Dirty_AdminInsertionGroupOrder
        {
            get { return ChildControls[1].Dirty; }
            set { ChildControls[1].Dirty = value; }
        }
    }
}