using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace CatalogEstimating
{
    public partial class CopyDescriptionDialog : Form
    {
        private CESDatabase _destinationDB;
        private List<CopyEstimate> _estimateCopies = new List<CopyEstimate>();

        public CopyDescriptionDialog(CESDatabase destinationDB, int numCopies, List<long> EstimateIDs)
        {
            InitializeComponent();

            _destinationDB = destinationDB;

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                foreach (long EstimateID in EstimateIDs)
                {
                    for (int i = 0; i < numCopies; ++i)
                    {
                        CopyEstimate estimateCopy = new CopyEstimate();
                        estimateCopy.LoadData(EstimateID);
                        _estimateCopies.Add(estimateCopy);
                        _gridCopies.Rows.Add(estimateCopy.Description, estimateCopy.Comments, estimateCopy.RunDate);
                    }
                }
            }
        }

        private void _btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateGrid())
                return;

            int totalRecords = _estimateCopies.Count;
            int failedRecords = 0;

            // Write any changes made to the grid back to the List of Estimates
            for (int i = 0; i < _gridCopies.Rows.Count; ++i)
            {
                _estimateCopies[i].Description = _gridCopies.Rows[i].Cells[0].Value.ToString().Trim();
                if (_gridCopies.Rows[i].Cells[1].Value == null || _gridCopies.Rows[i].Cells[1].Value.ToString().Trim() == string.Empty)
                    _estimateCopies[i].Comments = null;
                else
                    _estimateCopies[i].Comments = _gridCopies.Rows[i].Cells[1].Value.ToString().Trim();
                if (_estimateCopies[i].RunDate != (DateTime)_gridCopies.Rows[i].Cells[2].Value
                    || MainForm.WorkingDatabase.Id != _destinationDB.Id)
                {
                    _estimateCopies[i].ClearIssueDateOverrides = true;
                }
                _estimateCopies[i].RunDate = (DateTime)_gridCopies.Rows[i].Cells[2].Value;
                _estimateCopies[i].StatusID = 1;
                _estimateCopies[i].ParentID = null;
                _estimateCopies[i].UploadDate = null;
            }

            foreach (CopyEstimate estimateCopy in _estimateCopies)
                estimateCopy.LoadDetails();

            for (int i = 0; i < _estimateCopies.Count; ++i)
            {
                string errorDescription;
                if (!_estimateCopies[i].SaveData(_destinationDB, out errorDescription))
                {
                    ++failedRecords;
                    _gridCopies.Rows[i].Cells[0].ErrorText = errorDescription;
                }
            }

            _btnSave.Visible = false;
            _btnQuit.Visible = true;

            if (failedRecords > 0)
            {
                MessageBox.Show("Failed to copy " + failedRecords.ToString() + " record(s).");
            }
            else
                MessageBox.Show("Successfully copied " + totalRecords.ToString() + " record(s).");
        }

        private bool ValidateGrid()
        {
            bool retval = true;

            for (int i = 0; i < _gridCopies.Rows.Count; ++i)
            {
                if (_gridCopies.Rows[i].Cells[0].Value == null || _gridCopies.Rows[i].Cells[0].Value.ToString().Trim() == string.Empty)
                {
                    _gridCopies.Rows[i].Cells[0].ErrorText = CatalogEstimating.Properties.Resources.RequiredFieldError;
                    retval = false;
                }
                else
                {
                    _gridCopies.Rows[i].Cells[0].ErrorText = string.Empty;
                }
            }

            return retval;
        }

        private void _gridCopies_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (_gridCopies.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue == null || _gridCopies.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Trim() == string.Empty)
                {
                    _gridCopies.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = CatalogEstimating.Properties.Resources.RequiredFieldError;
                }
                else
                {
                    _gridCopies.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = string.Empty;
                }
            }
        }
    }
}