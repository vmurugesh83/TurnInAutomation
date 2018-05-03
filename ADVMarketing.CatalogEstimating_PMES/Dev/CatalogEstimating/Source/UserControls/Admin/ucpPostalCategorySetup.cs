#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.PostalTableAdapters;
using CatalogEstimating.Properties;

#endregion

namespace CatalogEstimating.UserControls.Admin
{
    public partial class ucpPostalCategorySetup : CatalogEstimating.UserControlPanel
    {
        #region Private Variables

        private Postal _postal = null;

        #endregion

        #region Construction

        public ucpPostalCategorySetup()
        {
            InitializeComponent();

            Name = "Category Setup";
        }

        public ucpPostalCategorySetup(Postal ds)
            : this()
        {
            _postal = ds;
        }

        #endregion

        #region Overrrides

        public override void Export( ref ExcelWriter writer)
        {
            writer.WriteTable(_gridCategories, true);
        }

        public override void SaveData()
        {
            LoadData();
            base.SaveData();
        }

        public override void PreSave(CancelEventArgs e)
        {
            if (!ValidateChildren())
                e.Cancel = true;
            else
            {
                DateTime modTime = DateTime.Now;

                DataView dvChanged = new DataView(_postal.pst_postalcategory);
                dvChanged.RowStateFilter = DataViewRowState.ModifiedCurrent;

                foreach (DataRowView r in dvChanged)
                {
                    Postal.pst_postalcategoryRow catRow = (Postal.pst_postalcategoryRow)r.Row;

                    catRow.modifiedby = MainForm.AuthorizedUser.FormattedName;
                    catRow.modifieddate = modTime;
                }
            }
        }

        public override void OnLeaving(CancelEventArgs e)
        {
            if (!UniqueCategories())
                e.Cancel = true;
            else
            {
                DateTime modTime = DateTime.Now;

                DataView dvChanged = new DataView(_postal.pst_postalcategory);
                dvChanged.RowStateFilter = DataViewRowState.ModifiedCurrent;

                foreach (DataRowView r in dvChanged)
                {
                    Postal.pst_postalcategoryRow catRow = (Postal.pst_postalcategoryRow)r.Row;

                    catRow.modifiedby = MainForm.AuthorizedUser.FormattedName;
                    catRow.modifieddate = modTime;
                }
            }
        }

        public override void LoadData()
        {
            IsLoading = true;
            base.LoadData();

            pstpostalcategoryBindingSource.DataSource = _postal;
            Dirty = false;

            if ((MainForm.AuthorizedUser.Right == UserRights.SuperAdmin) ||
                (MainForm.AuthorizedUser.Right == UserRights.Admin))
            {
                _gridCategories.ReadOnly = false;
            }
            else
            {
                _gridCategories.ReadOnly = true;
                _gridCategories.AllowUserToAddRows = false;
                _gridCategories.AllowUserToDeleteRows = false;
                _gridCategories.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
            }

            IsLoading = false;
        }

        #endregion

        #region Event Handlers

        private void _gridCategories_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["createdbyDataGridViewTextBoxColumn"].Value = MainForm.AuthorizedUser.FormattedName;
            e.Row.Cells["createddateDataGridViewTextBoxColumn"].Value = DateTime.Now;
        }

        private void _gridCategories_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!IsLoading && (e.RowIndex < _gridCategories.RowCount - 1))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString().Trim()))
                {
                    _gridCategories.Rows[e.RowIndex].ErrorText = Resources.RequiredFieldError;
                    e.Cancel = true;
                }
            }
        }

        private void _gridCategories_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            _gridCategories.Rows[e.RowIndex].ErrorText = string.Empty;
        }

        private void _gridCategories_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!IsLoading)
            {
                Dirty = true;
            }
        }

        private void _gridCategories_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DataRowView drv = (DataRowView)e.Row.DataBoundItem;

            if (drv != null)
            {
                if (IsUsedByRates(drv))
                {
                    MessageBox.Show(Resources.PostalRateDeleteError, "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                }
            }
        }

        private void _gridCategories_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            Dirty = true;
        }

        #endregion

        #region Private Methods

        private bool UniqueCategories()
        {
            bool isUnique = true;

            DataView dups = new DataView(_postal.pst_postalcategory);

            for (Int32 r = 0; r < _gridCategories.Rows.Count-1; r++)
            {
                Postal.pst_postalcategoryRow cr = (Postal.pst_postalcategoryRow)((DataRowView)_gridCategories.Rows[r].DataBoundItem).Row;

                dups.RowFilter = string.Concat("description = '", cr.description, "'");

                if (dups.Count > 1)
                {
                    _gridCategories.Rows[r].ErrorText = string.Concat("The Postal Category description exists multiple times.");
                    isUnique = false;
                }
                else
                {
                    _gridCategories.Rows[r].ErrorText = string.Empty;
                }
            }

            return isUnique;
        }

        private int CategoryCount(string category)
        {
            int cnt = 0;

            for (Int32 r = 0; r < _gridCategories.Rows.Count - 1; r++)
            {
                if (_gridCategories["descriptionDataGridViewTextBoxColumn", r].Value.ToString() == category)
                    cnt++;
            }

            return cnt;
        }

        private bool IsUsedByRates(DataRowView dataRow)
        {
            bool used = false;
            Postal.pst_postalcategoryRow pcr = (Postal.pst_postalcategoryRow)dataRow.Row;

            DataView dv = new DataView(_postal.pst_postalcategoryrate_map);
            dv.RowFilter = string.Concat("pst_postalcategory_id = ", pcr.pst_postalcategory_id);

            if (dv.Count > 0)
                used = true;

            return used;
        }

        #endregion

        private void _mnuDeleteSelectedRows_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow drv in _gridCategories.SelectedRows)
            {
                if (IsUsedByRates((DataRowView)drv.DataBoundItem))
                {
                    MessageBox.Show(Resources.PostalRateDeleteError, "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    Postal.pst_postalcategoryRow pcr = (Postal.pst_postalcategoryRow)((DataRowView)drv.DataBoundItem).Row;
                    pcr.Delete();
                    Dirty = true;
                }
            }
        }

        private void _gridCategories_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
                _gridCategories.Rows[e.RowIndex].ErrorText = string.Empty;
        }

        private void _gridCategories_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!IsLoading && !_gridCategories.ReadOnly)
            {
                try
                {
                    object o = _gridCategories[e.ColumnIndex, e.RowIndex].Value;
                    int cc = CategoryCount(o.ToString());

                    if (cc > 1)
                    {
                        _gridCategories.Rows[e.RowIndex].ErrorText = Resources.DuplicateItemError;
                        e.Cancel = true;
                    }
                }
                catch
                {
                    // ignore if the row is being removed.
                }
            }
        }

    }
}

