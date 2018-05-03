using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CatalogEstimating
{
    public partial class CopyNumberDialog : Form
    {
        private List<long> _EstimateIDs;

        public CopyNumberDialog(List<long> EstimateIDs)
        {
            InitializeComponent();

            _EstimateIDs = EstimateIDs;

            // Populate the List of Databases
            List<CESDatabase> _databases = DatabaseList.GetDatabases();
            foreach (CESDatabase database in _databases)
            {
                if (database.Display && database.Type != DatabaseType.Admin)
                    _cboDatabases.Items.Add(database);
            }

            for (int i = 0; i < _cboDatabases.Items.Count; ++i)
                if (((CESDatabase)_cboDatabases.Items[i]).Id == MainForm.WorkingDatabase.Id)
                {
                    _cboDatabases.SelectedIndex = i;
                    break;
                }

            SetNumberOfCopies();
        }

        private void SetNumberOfCopies()
        {
            if (_EstimateIDs.Count == 1)
            {
                _txtNumCopies.Enabled = true;
            }
            else
            {
                _txtNumCopies.Value = 1;
                _txtNumCopies.Enabled = false;
            }
        }

        private void _btnOK_Click(object sender, EventArgs e)
        {
            MainForm main = (MainForm)ParentForm;

            CopyDescriptionDialog dialogCopyDescription = new CopyDescriptionDialog(((CESDatabase) _cboDatabases.SelectedItem), Convert.ToInt32(_txtNumCopies.Value), _EstimateIDs);
            dialogCopyDescription.ShowDialog();
            dialogCopyDescription.Activate();
            //this.Close();
        }
    }
}