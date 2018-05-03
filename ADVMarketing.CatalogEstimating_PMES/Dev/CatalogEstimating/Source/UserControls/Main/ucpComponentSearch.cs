using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using CatalogEstimating;
using CatalogEstimating.CustomControls;
using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.LookupTableAdapters;
using CatalogEstimating.Properties;

namespace CatalogEstimating.UserControls.Main
{
    public partial class ucpComponentSearch : CatalogEstimating.UserControlPanel
    {
        private CatalogEstimating.Datasets.Lookup _dsLookup = new CatalogEstimating.Datasets.Lookup();

        public ucpComponentSearch()
        {
            InitializeComponent();
        }

        #region Overrides

        public override ToolStrip Toolbar
        {
            get { return _toolStrip; }
        }

        public override void LoadData()
        {
            _dsLookup.Clear();

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                using (est_statusTableAdapter adapter = new est_statusTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.est_status);
                }

                using (est_estimatemediatypeTableAdapter adapter = new est_estimatemediatypeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.est_estimatemediatype);
                }

                using (est_componenttypeTableAdapter adapter = new est_componenttypeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.est_componenttype);
                }

                using (ppr_paperweightTableAdapter adapter = new ppr_paperweightTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.ppr_paperweight);
                }

                using (ppr_papergradeTableAdapter adapter = new ppr_papergradeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.ppr_papergrade);
                }

                conn.Close();
            }

            // Populate the dropdowns
            _cboEstimateMediaType.Items.Clear();
            _dsLookup.est_estimatemediatype.DefaultView.Sort = "Description";
            _cboEstimateMediaType.Items.Add(new IntPair(-1, string.Empty));
            foreach (DataRowView drv_row in _dsLookup.est_estimatemediatype.DefaultView)
            {
                Lookup.est_estimatemediatypeRow emt_row = (Lookup.est_estimatemediatypeRow)drv_row.Row;
                _cboEstimateMediaType.Items.Add(new IntPair(emt_row.est_estimatemediatype_id, emt_row.description));
            }

            _cboComponentType.Items.Clear();
            _dsLookup.est_componenttype.DefaultView.Sort = "Description";
            _cboComponentType.Items.Add(new IntPair(-1, string.Empty));
            foreach (DataRowView drv_row in _dsLookup.est_componenttype.DefaultView)
            {
                Lookup.est_componenttypeRow ct_row = (Lookup.est_componenttypeRow)drv_row.Row;
                _cboComponentType.Items.Add(new IntPair(ct_row.est_componenttype_id, ct_row.description));
            }

            _cboPaperWeight.Items.Clear();
            _dsLookup.ppr_paperweight.DefaultView.Sort = "Weight";
            _cboPaperWeight.Items.Add(new IntPair(-1, string.Empty));
            foreach (DataRowView drv_row in _dsLookup.ppr_paperweight.DefaultView)
            {
                Lookup.ppr_paperweightRow pw_row = (Lookup.ppr_paperweightRow)drv_row.Row;
                _cboPaperWeight.Items.Add(new IntPair(pw_row.ppr_paperweight_id, pw_row.weight.ToString()));
            }

            _cboPaperGrade.Items.Clear();
            _dsLookup.ppr_papergrade.DefaultView.Sort = "Grade";
            _cboPaperGrade.Items.Add(new IntPair(-1, string.Empty));
            foreach (DataRowView drv_row in _dsLookup.ppr_papergrade.DefaultView)
            {
                Lookup.ppr_papergradeRow pg_row = (Lookup.ppr_papergradeRow)drv_row.Row;
                _cboPaperGrade.Items.Add(new IntPair(pg_row.ppr_papergrade_id, pg_row.grade));
            }
        }

        public override void Export(ref ExcelWriter writer)
        {
            writer.WriteLine("Component Search");
            writer.WriteLine("Component ID", _txtComponentID.Text, null, null, null, "Paper Weight", _cboPaperWeight.Text);
            writer.WriteLine("Component Desc", _txtDescription.Text, null, null, null, "Paper Grade", _cboPaperGrade.Text);
            writer.WriteLine("Range of Run Dates", "From", _dtStartRunDate.Text, "To", _dtEndRunDate.Text, "Page Count", _txtPageCount.Text);
            writer.WriteLine("Estimate Media Type", _cboEstimateMediaType.Text, null, null, null, _groupVendorSupplied.Text);
            writer.WriteLine("Component Type", _cboComponentType.Text, null, null, null, _radBoth.Checked.ToString(), _radBoth.Text, _radNo.Checked.ToString(), _radNo.Text, _radYes.Checked.ToString(), _radYes.Text);
            writer.WriteTable(_gridEstimates, true);
        }
        #endregion

        #region Event Handlers

        private void _btnReset_Click(object sender, EventArgs e)
        {
            _errorProvider.Clear();

            _txtComponentID.Text = string.Empty;
            _txtDescription.Text = string.Empty;
            _dtStartRunDate.Value = null;
            _dtEndRunDate.Value = null;
            _cboEstimateMediaType.SelectedIndex = 0;
            _cboComponentType.SelectedIndex = 0;
            _cboPaperWeight.SelectedIndex = 0;
            _cboPaperGrade.SelectedIndex = 0;
            _txtPageCount.Text = string.Empty;
            _radBoth.Checked = true;
            _gridEstimates.Rows.Clear();

            _btnSearch.Enabled = false;
        }

        private void _btnSearch_Click(object sender, EventArgs e)
        {
            _gridEstimates.Rows.Clear();

            using (SqlConnection conn = (SqlConnection) MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("EstComponent_Search", conn);
                cmd.CommandTimeout = 7200;
                cmd.CommandType = CommandType.StoredProcedure;

                if ( _txtComponentID.Text.Trim().Length > 0 )
                {
                    cmd.Parameters.AddWithValue( "@EST_Component_ID", _txtComponentID.Text.Trim() );
                    cmd.Parameters.AddWithValue( "@Description", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@RunDateStart", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@RunDateEnd", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@EST_EstimateMediaType_ID", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@EST_ComponentType_ID", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@PaperWeight_ID", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@PaperGrade_ID", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@PageCount", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@VendorSupplied", 1 );
                }
                else
                {
                    cmd.Parameters.AddWithValue( "@EST_Component_ID", DBNull.Value );
                    if ( _txtDescription.Text.Trim().Length > 0 )
                        cmd.Parameters.AddWithValue( "@Description", _txtDescription.Text.Trim() );
                    else
                        cmd.Parameters.AddWithValue( "@Description", DBNull.Value );
                    if ( _dtStartRunDate.Value.HasValue )
                        cmd.Parameters.AddWithValue( "@RunDateStart", _dtStartRunDate.Value.Value.Date );
                    else
                        cmd.Parameters.AddWithValue( "@RunDateStart", DBNull.Value );
                    if ( _dtEndRunDate.Value.HasValue )
                        cmd.Parameters.AddWithValue( "@RunDateEnd", _dtEndRunDate.Value.Value.Date );
                    else
                        cmd.Parameters.AddWithValue( "@RunDateEnd", DBNull.Value );
                    if ( _cboEstimateMediaType.SelectedIndex > 0 )
                        cmd.Parameters.AddWithValue( "@EST_EstimateMediaType_ID", ( (IntPair)_cboEstimateMediaType.SelectedItem ).Value );
                    else
                        cmd.Parameters.AddWithValue( "@EST_EstimateMediaType_ID", DBNull.Value );
                    if ( _cboComponentType.SelectedIndex > 0 )
                        cmd.Parameters.AddWithValue( "@EST_ComponentType_ID", ( (IntPair)_cboComponentType.SelectedItem ).Value );
                    else
                        cmd.Parameters.AddWithValue( "@EST_ComponentType_ID", DBNull.Value );
                    if ( _cboPaperWeight.SelectedIndex > 0 )
                        cmd.Parameters.AddWithValue( "@PaperWeight_ID", ( (IntPair)_cboPaperWeight.SelectedItem ).Value );
                    else
                        cmd.Parameters.AddWithValue( "@PaperWeight_ID", DBNull.Value );
                    if ( _cboPaperGrade.SelectedIndex > 0 )
                        cmd.Parameters.AddWithValue( "@PaperGrade_ID", ( (IntPair)_cboPaperGrade.SelectedItem ).Value );
                    else
                        cmd.Parameters.AddWithValue( "@PaperGrade_ID", DBNull.Value );
                    if ( _txtPageCount.Text.Trim().Length > 0 )
                        cmd.Parameters.AddWithValue( "@PageCount", _txtPageCount.Text.Trim() );
                    else
                        cmd.Parameters.AddWithValue( "@PageCount", DBNull.Value );
                    if ( _radBoth.Checked )
                        cmd.Parameters.AddWithValue( "@VendorSupplied", 1 );
                    else if ( _radYes.Checked )
                        cmd.Parameters.AddWithValue( "@VendorSupplied", 2 );
                    else
                        cmd.Parameters.AddWithValue( "@VendorSupplied", 3 );
                }

                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    dr.Close();
                    conn.Close();
                    MessageBox.Show(Resources.ComponentSearchNoRecords);

                    _lblComponents.Text = string.Concat((_gridEstimates.RowCount).ToString(), " Component(s) Returned on: ", DateTime.Now.ToString());
                    return;
                }

                while (dr.Read())
                {
                    if (dr.IsDBNull(dr.GetOrdinal("Parent_ID")))
                    {
                        _gridEstimates.Rows.Add(dr.GetInt64(dr.GetOrdinal("EST_Component_ID")), dr.GetInt64(dr.GetOrdinal("EST_Estimate_ID")),
                            null, dr.GetInt32(dr.GetOrdinal("EST_Status_ID")),
                            dr.GetDateTime(dr.GetOrdinal("RunDate")).ToShortDateString(), dr["Description"].ToString(),
                            dr["AdNumber"].ToString());
                    }
                    else
                    {
                        _gridEstimates.Rows.Add(dr.GetInt64(dr.GetOrdinal("EST_Component_ID")), dr.GetInt64(dr.GetOrdinal("EST_Estimate_ID")),
                            dr.GetInt64(dr.GetOrdinal("Parent_ID")), dr.GetInt32(dr.GetOrdinal("EST_Status_ID")),
                            dr.GetDateTime(dr.GetOrdinal("RunDate")).ToShortDateString(), dr["Description"].ToString(),
                            dr["AdNumber"].ToString());
                    }
                }

                dr.Close();
                conn.Close();
            }

            _lblComponents.Text = string.Concat((_gridEstimates.RowCount).ToString(), " Component(s) Returned on: ", DateTime.Now.ToString());
        }

        private void SearchCriteria_Changed(object sender, EventArgs e)
        {
            EnableSearchButton();
        }

        private void _gridEstimates_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            OpenSelectedComponents(false);
        }

        private void _btnOpen_Click(object sender, EventArgs e)
        {
            OpenSelectedComponents(false);
        }

        private void _btnOpenReadOnly_Click(object sender, EventArgs e)
        {
            OpenSelectedComponents(true);
        }

        private void _dtEndRunDate_Validating(object sender, CancelEventArgs e)
        {
            if (_dtStartRunDate.Value != null && _dtEndRunDate.Value != null)
            {
                if (_dtEndRunDate.Value < _dtStartRunDate.Value)
                {
                    _errorProvider.SetError(_dtEndRunDate, "End Date must be on or after the Begin Date");
                    e.Cancel = true;
                }
            }
        }

        private void _dtEndRunDate_Validated(object sender, EventArgs e)
        {
            _errorProvider.SetError(_dtEndRunDate, string.Empty);
        }

        private void _btnPrint_Click(object sender, EventArgs e)
        {
            ExcelWriter writer = null;
            try
            {
                writer = new ExcelWriter();
                this.Export(ref writer);
                writer.Show();
            }
            finally
            {
                writer.Dispose();
            }
        }

        private void _gridEstimates_SelectionChanged(object sender, EventArgs e)
        {
            EnableToolbarButtons();
        }

        private void _gridEstimates_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == _gridEstimates.Columns["AdNumber"])
            {
                int v1 = -1;
                int v2 = -1;
                if (!string.IsNullOrEmpty(e.CellValue1.ToString()))
                    v1 = Convert.ToInt32(e.CellValue1);
                if (!string.IsNullOrEmpty(e.CellValue2.ToString()))
                    v2 = Convert.ToInt32(e.CellValue2);

                e.SortResult = v1.CompareTo(v2);
                e.Handled = true;
            }
            else if (e.Column == _gridEstimates.Columns["RunDate"])
            {
                e.SortResult = Convert.ToDateTime(e.CellValue1).CompareTo(Convert.ToDateTime(e.CellValue2));
                e.Handled = true;
            }
        }

        #endregion

        #region Private Methods

        private void EnableSearchButton()
        {
            bool bEnable = false;
            foreach (Control ctrl in _groupFilter.Controls)
            {
                TextBox text = ctrl as TextBox;
                if (text != null)
                {
                    if (text.Text.Trim().Length > 0)
                    {
                        bEnable = true;
                        break;
                    }
                    else
                        continue;
                }

                ComboBox combo = ctrl as ComboBox;
                if (combo != null)
                {
                    if (combo.SelectedIndex > 0)
                    {
                        bEnable = true;
                        break;
                    }
                    else
                        continue;
                }

                NullableDateTimePicker date = ctrl as NullableDateTimePicker;
                if (date != null)
                {
                    if (date.Value != null)
                    {
                        bEnable = true;
                        break;
                    }
                    else
                        continue;
                }
            }

            _btnSearch.Enabled = bEnable;
        }

        private void OpenSelectedComponents(bool bForceReadOnly)
        {
            MainForm main = (MainForm)ParentForm;

            if (MainForm.AuthorizedUser.Right == UserRights.ReadOnly)
                bForceReadOnly = true;

            List<long> selectedEstimates = new List<long>();

            foreach (DataGridViewRow gridViewRow in _gridEstimates.SelectedRows)
            {
                if (!selectedEstimates.Contains((long)gridViewRow.Cells[1].Value))
                {
                    long? parentId = null;
                    if (gridViewRow.Cells[2].Value != null)
                        parentId = (long)gridViewRow.Cells[2].Value;

                    bool readOnly = bForceReadOnly;
                    if (((int)gridViewRow.Cells[3].Value) != 1)
                        readOnly = true;

                    main.OpenEstimate( readOnly, (long)gridViewRow.Cells[1].Value, parentId, 1 );
                }
            }
        }

        private void EnableToolbarButtons()
        {
            bool bEditUser = (MainForm.AuthorizedUser.Right != UserRights.ReadOnly);
            bool bHasKilled = false;
            bool bHasActive = false;
            bool bHasUploaded = false;

            foreach (DataGridViewRow gridViewRow in _gridEstimates.SelectedRows)
            {
                switch ((int)gridViewRow.Cells[3].Value)
                {
                    case 1:
                        bHasActive = true;
                        break;
                    case 2:
                        bHasUploaded = true;
                        break;
                    case 3:
                        bHasKilled = true;
                        break;
                }
            }

            _btnOpen.Enabled = bHasActive && bEditUser;
            _btnOpenReadOnly.Enabled = bHasActive || bHasUploaded || bHasKilled;
        }

        #endregion

        private void _txtDescription_Validated(object sender, EventArgs e)
        {
            _txtDescription.Text = _txtDescription.Text.Trim();
        }
    }
}