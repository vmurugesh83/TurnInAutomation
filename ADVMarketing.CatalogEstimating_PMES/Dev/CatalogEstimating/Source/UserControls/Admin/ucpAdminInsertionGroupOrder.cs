using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CatalogEstimating.Datasets;

namespace CatalogEstimating.UserControls.Admin
{
    public partial class ucpAdminInsertionGroupOrder : CatalogEstimating.UserControlPanel
    {
        private bool _ReorderFlag = false;
        private DataView _DataView_DistinctGroups = new DataView();

        public ucpAdminInsertionGroupOrder(Publications ds)
        {
            InitializeComponent();
            Name = "Group Order";
            _dsPublications = ds;
            _DataView_DistinctGroups.Table = new Publications.pub_pubgroupDataTable();
        }

        public override void LoadData()
        {
            this.Reload();
            base.LoadData();
        }

        public override void Reload()
        {
            // Populate the Lists
            /*Do a deep copy of the pub groups.*/
            DataView tmpPubGroups = new DataView();
            tmpPubGroups.Table = _dsPublications.pub_pubgroup.Copy();
            tmpPubGroups.RowFilter = "CustomGroupForPackage = false";
            tmpPubGroups.Sort = "SortOrder";

            // Copy the distinct pub group descriptions into the dataview
            _DataView_DistinctGroups.Table = new Publications.pub_pubgroupDataTable();
            string prevDescription = null;
            foreach (DataRowView r in tmpPubGroups)
            {
                Publications.pub_pubgroupRow pg_row = (Publications.pub_pubgroupRow)r.Row;
                if (prevDescription == null || pg_row.description != prevDescription)
                {
                    // Add the row to the data view
                    prevDescription = pg_row.description;
                    _DataView_DistinctGroups.Table.ImportRow(pg_row);
                }
            }

            // Update the sort order in the dataview and the underlying dataset
            int SortOrder = 0;
            foreach (DataRowView r in _DataView_DistinctGroups)
            {
                Publications.pub_pubgroupRow pg_row = (Publications.pub_pubgroupRow)r.Row;
                pg_row.sortorder = SortOrder++;
                _dsPublications.pub_pubgroup.FindBypub_pubgroup_id(pg_row.pub_pubgroup_id).sortorder = pg_row.sortorder;
            }

            _gridOrder.DataSource = _DataView_DistinctGroups;
            _gridOrder.CurrentCell = null;
            _cmdTop.Enabled = false;
            _cmdUp10.Enabled = false;
            _cmdUp1.Enabled = false;
            _cmdDown1.Enabled = false;
            _cmdDown10.Enabled = false;
            _cmdBottom.Enabled = false;
            base.Reload();
        }

        public override void Export(ref ExcelWriter writer)
        {
            writer.WriteLine(new object[] { "Admin Insertion Group Order" });
            writer.WriteTable(_gridOrder, true);
        }

        private void _cmdTop_Click(object sender, EventArgs e)
        {
            _ReorderFlag = true;

            string selectedGroupDescription = null;

            DataView tmpPubGroups = new DataView();
            tmpPubGroups.Table = new Publications.pub_pubgroupDataTable();

            tmpPubGroups.Table.ImportRow(((DataRowView)_gridOrder.SelectedRows[0].DataBoundItem).Row);

            foreach (DataGridViewRow r in _gridOrder.Rows)
                if (!r.Selected)
                    tmpPubGroups.Table.ImportRow(((DataRowView)r.DataBoundItem).Row);
                else
                    selectedGroupDescription = ((Publications.pub_pubgroupRow)((DataRowView)r.DataBoundItem).Row).description;

            // Update the sort order in the dataview and the underlying dataset
            int SortOrder = 0;
            foreach (DataRowView r in tmpPubGroups)
            {
                Publications.pub_pubgroupRow pg_row = (Publications.pub_pubgroupRow) r.Row;
                pg_row.sortorder = SortOrder++;
                _dsPublications.pub_pubgroup.FindBypub_pubgroup_id(pg_row.pub_pubgroup_id).sortorder = pg_row.sortorder;
            }

            // Set the bound DataView
            _DataView_DistinctGroups.Table = tmpPubGroups.Table;

            _ReorderFlag = false;

            // Set the selected items back to the original selection
            foreach (DataGridViewRow r in _gridOrder.Rows)
                if (selectedGroupDescription == ((Publications.pub_pubgroupRow)((DataRowView)r.DataBoundItem).Row).description)
                {
                    _gridOrder.CurrentCell = r.Cells[1];
                    _gridOrder_SelectionChanged(sender, e);
                    break;
                }

            this.Dirty = true;
        }

        private void MoveSelectedRecord(int numPositions)
        {
            _ReorderFlag = true;

            string selectedGroupDescription = null;

            if (_gridOrder.SelectedRows.Count == 0)
                return;

            int direction = -1;

            if (numPositions > 0)
                direction = 1;

            DataView tmpPubGroups = new DataView();
            tmpPubGroups.Table = new Publications.pub_pubgroupDataTable();

            // Copy each record into the temporary dataview with a new sort order.
            foreach (DataGridViewRow r in _gridOrder.Rows)
            {
                Publications.pub_pubgroupRow pg_row = (Publications.pub_pubgroupRow)((DataRowView)r.DataBoundItem).Row;
                if (r.Selected)
                {
                    pg_row.sortorder = (r.Index * 2) + direction + numPositions * 2;
                    selectedGroupDescription = ((Publications.pub_pubgroupRow)((DataRowView)r.DataBoundItem).Row).description;
                }
                else
                    pg_row.sortorder = (r.Index * 2);

                tmpPubGroups.Table.ImportRow(pg_row);
            }

            // Order the temporary data view on the new sort order
            tmpPubGroups.Sort = "SortOrder";

            // Copy each record into the dataview.
            _DataView_DistinctGroups.Table = new Publications.pub_pubgroupDataTable();
            for (int i = 0; i < tmpPubGroups.Count; ++i)
                _DataView_DistinctGroups.Table.ImportRow((DataRow)tmpPubGroups[i].Row);


            // Update the sort order in the dataview and the underlying dataset
            int SortOrder = 0;
            foreach (DataRowView r in _DataView_DistinctGroups)
            {
                Publications.pub_pubgroupRow pg_row = (Publications.pub_pubgroupRow)r.Row;
                pg_row.sortorder = SortOrder++;

                foreach (DataRow ds_row in _dsPublications.pub_pubgroup.Select("description = '" + pg_row.description.Replace("'", "''") + "'"))
                    ((Publications.pub_pubgroupRow)ds_row).sortorder = pg_row.sortorder;
            }

            _ReorderFlag = false;

            // Set the selected items back to the original selection
            foreach (DataGridViewRow r in _gridOrder.Rows)
                if (selectedGroupDescription == ((Publications.pub_pubgroupRow)((DataRowView)r.DataBoundItem).Row).description)
                {
                    _gridOrder.CurrentCell = r.Cells[1];
                    _gridOrder_SelectionChanged(this, new EventArgs());
                    break;
                }
        }

        private void _cmdUp10_Click(object sender, EventArgs e)
        {
            MoveSelectedRecord(-10);
            this.Dirty = true;
        }

        private void _cmdUp1_Click(object sender, EventArgs e)
        {
            MoveSelectedRecord(-1);
            this.Dirty = true;
        }

        private void _cmdDown1_Click(object sender, EventArgs e)
        {
            MoveSelectedRecord(1);
            this.Dirty = true;
        }

        private void _cmdDown10_Click(object sender, EventArgs e)
        {
            MoveSelectedRecord(10);
            this.Dirty = true;
        }

        private void _cmdBottom_Click(object sender, EventArgs e)
        {
            _ReorderFlag = true;

            string selectedGroupDescription = null;

            DataView tmpPubGroups = new DataView();
            tmpPubGroups.Table = new Publications.pub_pubgroupDataTable();

            foreach (DataGridViewRow r in _gridOrder.Rows)
                if (!r.Selected)
                    tmpPubGroups.Table.ImportRow(((DataRowView)r.DataBoundItem).Row);
                else
                    selectedGroupDescription = ((Publications.pub_pubgroupRow)((DataRowView)r.DataBoundItem).Row).description;

            tmpPubGroups.Table.ImportRow(((DataRowView)_gridOrder.SelectedRows[0].DataBoundItem).Row);

            // Update the sort order in the dataview and the underlying dataset
            int SortOrder = 0;
            foreach (DataRowView r in tmpPubGroups)
            {
                Publications.pub_pubgroupRow pg_row = (Publications.pub_pubgroupRow)r.Row;
                pg_row.sortorder = SortOrder++;
                _dsPublications.pub_pubgroup.FindBypub_pubgroup_id(pg_row.pub_pubgroup_id).sortorder = pg_row.sortorder;
            }

            // Set the bound DataView
            _DataView_DistinctGroups.Table = tmpPubGroups.Table;
            _ReorderFlag = false;

            // Set the selected items back to the original selection
            foreach (DataGridViewRow r in _gridOrder.Rows)
                if (selectedGroupDescription == ((Publications.pub_pubgroupRow)((DataRowView)r.DataBoundItem).Row).description)
                {
                    _gridOrder.CurrentCell = r.Cells[1];
                    _gridOrder_SelectionChanged(sender, e);
                    break;
                }

            this.Dirty = true;
        }

        public override void SaveData()
        {
            base.SaveData();

            this.Reload();
        }

        private void _gridOrder_SelectionChanged(object sender, EventArgs e)
        {
            if (_ReorderFlag)
                return;

            if (_gridOrder.CurrentRow == null)
            {
                _cmdTop.Enabled = false;
                _cmdUp10.Enabled = false;
                _cmdUp1.Enabled = false;
                _cmdDown1.Enabled = false;
                _cmdDown10.Enabled = false;
                _cmdBottom.Enabled = false;
                return;
            }

            if (MainForm.AuthorizedUser.Right == UserRights.SuperAdmin || MainForm.AuthorizedUser.Right == UserRights.Admin)
            {
                if (_gridOrder.CurrentRow.Index == 0)
                {
                    _cmdTop.Enabled = false;
                    _cmdUp1.Enabled = false;
                    _cmdUp10.Enabled = false;
                }

                else if (_gridOrder.CurrentRow.Index < 10)
                {
                    _cmdTop.Enabled = true;
                    _cmdUp10.Enabled = false;
                    _cmdUp1.Enabled = true;
                }

                else
                {
                    _cmdTop.Enabled = true;
                    _cmdUp10.Enabled = true;
                    _cmdUp1.Enabled = true;
                }

                if (_gridOrder.CurrentRow.Index == (_gridOrder.Rows.Count - 1))
                {
                    _cmdBottom.Enabled = false;
                    _cmdDown1.Enabled = false;
                    _cmdDown10.Enabled = false;
                }
                else if (_gridOrder.CurrentRow.Index > (_gridOrder.Rows.Count - 11))
                {
                    _cmdBottom.Enabled = true;
                    _cmdDown10.Enabled = false;
                    _cmdDown1.Enabled = true;
                }
                else
                {
                    _cmdBottom.Enabled = true;
                    _cmdDown10.Enabled = true;
                    _cmdDown1.Enabled = true;
                }
            }
        }
    }
}