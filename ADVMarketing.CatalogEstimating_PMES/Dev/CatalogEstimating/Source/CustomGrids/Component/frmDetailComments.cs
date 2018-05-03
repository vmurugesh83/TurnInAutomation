using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CatalogEstimating.CustomGrids.Component
{
    public partial class frmDetailComments : Form
    {
        public frmDetailComments(bool readOnly, string windowCaption, string DetailsString)
        {
            InitializeComponent();

            _txtDetails.ReadOnly = readOnly;
            this.Text = windowCaption;
            _txtDetails.Text = DetailsString;
        }

        public event WorkComplete WorkComplete;

        private void _btnSave_Click(object sender, EventArgs e)
        {
            if (WorkComplete != null)
            {
                WorkComplete(this, new WorkCompleteEventArgs(_txtDetails.Text.Trim()));
            }

            this.Close();
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}