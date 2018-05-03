using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CatalogEstimating.UserControls.Main
{
    public partial class SynchronizeDialog : Form
    {
        private int _step = 0;

        public SynchronizeDialog()
        {
            InitializeComponent();
        }

        private void _btnSyncClose_Click( object sender, EventArgs e )
        {
            if ( _step == 0 )
            {
                _cboDatabases.Enabled = false;
                _btnCancel.Enabled = false;
                _btnSyncClose.Enabled = false;

                _TestTimer.Enabled = true;
            }
            else
            {
                Close();
            }
        }

        private void _TestTimer_Tick( object sender, EventArgs e )
        {
            _step++;

            if ( _step == 1 )
                _lblStatus.Text = "Purging Destination...";
            else if ( _step == 5 )
            {
                _lblStatus.Text = "Synchronization Complete";
                _TestTimer.Enabled = false;
                _btnSyncClose.Enabled = true;
                _btnSyncClose.Text = "&Close";
            }

            _progressSync.Value++;
        }
    }
}