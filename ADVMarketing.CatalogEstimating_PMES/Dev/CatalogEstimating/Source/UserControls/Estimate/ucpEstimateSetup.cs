using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using CatalogEstimating.Datasets;

namespace CatalogEstimating.UserControls.Estimate
{
    public partial class ucpEstimateSetup : CatalogEstimating.UserControlPanel
    {
        private Datasets.Estimates _dsEstimate;
        private bool _readOnly = true;

        #region Construction

        public ucpEstimateSetup(Datasets.Estimates dsEstimate, bool readOnly)
        {
            InitializeComponent();
            Name = "Estimate Setup";

            _dsEstimate = dsEstimate;
            _readOnly = readOnly;

            if (!readOnly)
            {
                _dtRunDate.Enabled = true;
                _txtDescription.ReadOnly = false;
                _txtComments.ReadOnly = false;
            }
        }

        #endregion

        #region Overrides
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _txtDescription.Focus();
        }

        
        public override void Reload()
        {
            this.IsLoading = true;

            base.Reload();

            _dtRunDate.Value = _dsEstimate.est_estimate[0].rundate;
            _txtDescription.Text = _dsEstimate.est_estimate[0].description;
            if (_dsEstimate.est_estimate[0].IscommentsNull())
                _txtComments.Text = String.Empty;
            else
                _txtComments.Text = _dsEstimate.est_estimate[0].comments;

            _txtDescription.Focus();

            this.IsLoading = false;
        }

        public override void PreSave(CancelEventArgs e)
        {
            string msgWarning = string.Empty;

            // Force value to be sync'd with what may have been typed into date control.
            MainForm.UpdateDateControl(_dtRunDate);

            if (ValidateChildren())
            {
                bool runDateChanged = RunDateChanged();
                if (runDateChanged)
                {
                    DialogResult result = MessageBox.Show("You have changed the estimate run date. " + System.Environment.NewLine + "If you continue, some Components, Assembly and Distribution Option, and Distribution Mapping may lose information and require modifications.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                WriteToDataset();
                if (runDateChanged)
                {
                    if (_dsEstimate.est_component.Count > 0)
                    {
                        WriteComponentToDataset();
                        ((uctEstimate)this.Parent.Parent.Parent)._ucpComponents.Reload();
                        ((uctEstimate)this.Parent.Parent.Parent)._ucpComponents.Dirty = true;
                    }

                    WriteADToDataset();
                    ((uctEstimate)this.Parent.Parent.Parent)._ucpAssemblyDistributionOptions.Reload();
                    if (_dsEstimate.est_assemdistriboptions.Count > 0)
                    {
                        ((uctEstimate)this.Parent.Parent.Parent)._ucpAssemblyDistributionOptions.Dirty = true;
                    }
                }

                if (runDateChanged && !ValidateComponents())
                {
                    msgWarning = msgWarning + "At least one component cannot be saved." + System.Environment.NewLine;
                    e.Cancel = true;
                }

                if (runDateChanged && !ValidateAssemblyAndDistribution())
                {
                    msgWarning = msgWarning + "An assembly & distribution rate cannot be found." + System.Environment.NewLine;
                    e.Cancel = true;
                }

                if (runDateChanged && !ValidateDistributionMapping())
                {
                    msgWarning = msgWarning + "The insert setup portion of distribution mapping has conflicts and cannot be saved." + System.Environment.NewLine;
                    e.Cancel = true;
                }

                if (msgWarning != string.Empty)
                {
                    DialogResult result = MessageBox.Show(msgWarning, "Cannot Save", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
                e.Cancel = true;

            base.PreSave(e);
        }

        public override void OnLeaving(CancelEventArgs e)
        {
            string msgWarning = string.Empty;

            if (ValidateChildren())
            {
                bool runDateChanged = RunDateChanged();
                if (runDateChanged)
                {
                    DialogResult result = MessageBox.Show("You have changed the estimate run date. " + System.Environment.NewLine + "If you continue, some Components, Assembly and Distribution Option, and Distribution Mapping may lose information and require modifications.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                WriteToDataset();
                if (runDateChanged)
                {
                    WriteComponentToDataset();
                    ((uctEstimate)this.Parent.Parent.Parent)._ucpComponents.Reload();
                    ((uctEstimate)this.Parent.Parent.Parent)._ucpComponents.Dirty = true;

                    WriteADToDataset();
                    ((uctEstimate)this.Parent.Parent.Parent)._ucpAssemblyDistributionOptions.Reload();
                    ((uctEstimate)this.Parent.Parent.Parent)._ucpAssemblyDistributionOptions.Dirty = true;
                }

                if (runDateChanged && !ValidateComponents())
                {
                    msgWarning = msgWarning + "At least one component cannot be saved." + System.Environment.NewLine;
                }

                if (runDateChanged && !ValidateAssemblyAndDistribution())
                {
                    msgWarning = msgWarning + "An assembly & distribution rate cannot be found." + System.Environment.NewLine;
                }

                if (runDateChanged && !ValidateDistributionMapping())
                {
                    msgWarning = msgWarning + "Conflicts exist on the insert setup portion of distribution mapping." + System.Environment.NewLine;
                }

                if (msgWarning != string.Empty)
                {
                    DialogResult result = MessageBox.Show(msgWarning, "Cannot Continue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
                e.Cancel = true;

            base.OnLeaving(e);
        }

        public override void Export(ref ExcelWriter writer)
        {
            writer.WriteLine("Estimate Setup");
            writer.WriteLine(_lblDescription.Text, _txtDescription.Text.Trim());
            writer.WriteLine(_lblComments.Text, _txtComments.Text.Trim());
            writer.WriteLine(_lblRunDate.Text, _dtRunDate.Text);
            writer.WriteLine(_lblFiscalMonth.Text, _txtFiscalMonth.Text);
            writer.WriteLine(_lblFiscalYear.Text, _txtFiscalYear.Text);
            writer.WriteLine(_lblSeason.Text, _txtSeason.Text);

            base.Export(ref writer);
        }
        #endregion

        #region Event Handlers

        private void _dtRunDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.IsLoading)
                this.Dirty = true;

            _txtFiscalYear.Text = CatalogEstimating.FiscalCalculator.FiscalYear(_dtRunDate.Value).ToString();

            _txtFiscalMonth.Text = ((FiscalMonths)CatalogEstimating.FiscalCalculator.FiscalMonth(_dtRunDate.Value)).ToString();
            _txtSeason.Text = ((FiscalSeasons)CatalogEstimating.FiscalCalculator.SeasonID(_dtRunDate.Value)).ToString();
        }

        private void _txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (_txtDescription.Text.Trim() == String.Empty)
            {
                _errorProvider.SetError(_txtDescription, Properties.Resources.RequiredFieldError);
                e.Cancel = true;
            }
        }

        private void _txtDescription_Validated(object sender, EventArgs e)
        {
            _errorProvider.SetError(_txtDescription, String.Empty);
        }

        private void _txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
                this.Dirty = true;
        }

        #endregion

        #region Private Methods
        private void WriteToDataset()
        {
            _dsEstimate.est_estimate[0].description = _txtDescription.Text.Trim();
            if (_txtComments.Text.Trim() == String.Empty)
                _dsEstimate.est_estimate[0].SetcommentsNull();
            else
                _dsEstimate.est_estimate[0].comments = _txtComments.Text.Trim();
            _dsEstimate.est_estimate[0].rundate = _dtRunDate.Value.Date;
            _dsEstimate.est_estimate[0].fiscalmonth = FiscalCalculator.FiscalMonth(_dtRunDate.Value.Date);
            _dsEstimate.est_estimate[0].fiscalyear = FiscalCalculator.FiscalYear(_dtRunDate.Value.Date);
            _dsEstimate.est_estimate[0].est_season_id = FiscalCalculator.SeasonID(_dtRunDate.Value.Date);
            if (_dsEstimate.est_estimate[0].RowState != DataRowState.Added)
            {
                _dsEstimate.est_estimate[0].modifiedby = MainForm.AuthorizedUser.FormattedName;
                _dsEstimate.est_estimate[0].modifieddate = DateTime.Now;
            }
        }

        private void WriteComponentToDataset()
        {
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                foreach (Estimates.est_componentRow c_row in _dsEstimate.est_component)
                {
                    if (!c_row.Isprinter_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("Printer_s_PrinterID_ByOldPrinterIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@VND_Printer_ID", c_row.printer_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    c_row.printer_id = dr.GetInt64(dr.GetOrdinal("VND_Printer_ID"));
                                else
                                    c_row.Setprinter_idNull();
                                dr.Close();
                            }
                        }
                    }

                    if (!c_row.Isplatecost_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PRT_PrinterRate_ID", c_row.platecost_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    c_row.platecost_id = dr.GetInt64(dr.GetOrdinal("PRT_PrinterRate_ID"));
                                else
                                    c_row.Setplatecost_idNull();
                                dr.Close();
                            }
                        }
                    }

                    if (!c_row.Isdigitalhandlenprepare_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PRT_PrinterRate_ID", c_row.digitalhandlenprepare_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    c_row.digitalhandlenprepare_id = dr.GetInt64(dr.GetOrdinal("PRT_PrinterRate_ID"));
                                else
                                    c_row.Setdigitalhandlenprepare_idNull();
                                dr.Close();
                            }
                        }
                    }

                    if (!c_row.Isstitchin_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PRT_PrinterRate_ID", c_row.stitchin_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    c_row.stitchin_id = dr.GetInt64(dr.GetOrdinal("PRT_PrinterRate_ID"));
                                else
                                    c_row.Setstitchin_idNull();
                                dr.Close();
                            }
                        }
                    }

                    if (!c_row.Isblowin_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PRT_PrinterRate_ID", c_row.blowin_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    c_row.blowin_id = dr.GetInt64(dr.GetOrdinal("PRT_PrinterRate_ID"));
                                else
                                    c_row.Setblowin_idNull();
                                dr.Close();
                            }
                        }
                    }

                    if (!c_row.Isonsert_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PRT_PrinterRate_ID", c_row.onsert_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    c_row.onsert_id = dr.GetInt64(dr.GetOrdinal("PRT_PrinterRate_ID"));
                                else
                                    c_row.Setonsert_idNull();
                                dr.Close();
                            }
                        }
                    }

                    if (!c_row.Isstitchermakeready_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PRT_PrinterRate_ID", c_row.stitchermakeready_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    c_row.stitchermakeready_id = dr.GetInt64(dr.GetOrdinal("PRT_PrinterRate_ID"));
                                else
                                    c_row.Setstitchermakeready_idNull();
                                dr.Close();
                            }
                        }
                    }

                    if (!c_row.Ispressmakeready_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PRT_PrinterRate_ID", c_row.pressmakeready_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    c_row.pressmakeready_id = dr.GetInt64(dr.GetOrdinal("PRT_PrinterRate_ID"));
                                else
                                    c_row.Setpressmakeready_idNull();
                                dr.Close();
                            }
                        }
                    }

                    if (!c_row.Ispaper_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("Paper_s_PaperID_ByOldPaperIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@VND_Paper_ID", c_row.paper_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    c_row.paper_id = dr.GetInt64(dr.GetOrdinal("VND_Paper_ID"));
                                else
                                    c_row.Setpaper_idNull();
                                dr.Close();
                            }
                        }
                    }

                    if (!c_row.Ispaper_map_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PPR_Paper_Map_ID", c_row.paper_map_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    c_row.paper_map_id = dr.GetInt64(dr.GetOrdinal("PPR_Paper_Map_ID"));
                                else
                                    c_row.Setpaper_map_idNull();
                                dr.Close();
                            }
                        }
                    }

                    if (!c_row.Isassemblyvendor_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("Printer_s_PrinterID_ByOldPrinterIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@VND_Printer_ID", c_row.assemblyvendor_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    c_row.assemblyvendor_id = dr.GetInt64(dr.GetOrdinal("VND_Printer_ID"));
                                else
                                    c_row.Setassemblyvendor_idNull();
                                dr.Close();
                            }
                        }
                    }
                }
                conn.Close();
            }
        }

        private void WriteADToDataset()
        {
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                if (_dsEstimate.est_assemdistriboptions.Count > 0)
                {
                    DayOfWeek dtDOW = _dsEstimate.est_estimate[0].rundate.DayOfWeek;
                    _dsEstimate.est_assemdistriboptions[0].insertdow = Convert.ToInt32(dtDOW) + 1;

                    if (!_dsEstimate.est_assemdistriboptions[0].Ispst_postalscenario_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("PstPostalScenario_s_ByOldIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PST_PostalScenario_ID", _dsEstimate.est_assemdistriboptions[0].pst_postalscenario_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    _dsEstimate.est_assemdistriboptions[0].pst_postalscenario_id = dr.GetInt64(dr.GetOrdinal("PST_PostalScenario_ID"));
                                else
                                    _dsEstimate.est_assemdistriboptions[0].Setpst_postalscenario_idNull();
                                dr.Close();
                            }
                        }
                    }

                    if (!_dsEstimate.est_assemdistriboptions[0].Ismailhouse_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("VndMailHouseRate_s_ByOldIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@VND_MailHouseRate_ID", _dsEstimate.est_assemdistriboptions[0].mailhouse_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    _dsEstimate.est_assemdistriboptions[0].mailhouse_id = dr.GetInt64(dr.GetOrdinal("VND_MailHouseRate_ID"));
                                else
                                    _dsEstimate.est_assemdistriboptions[0].Setmailhouse_idNull();
                                dr.Close();
                            }
                        }
                    }

                    if (!_dsEstimate.est_assemdistriboptions[0].Ismailtracking_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("VndMailTrackingRate_s_ByOldIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@VND_MailTrackingRate_ID", _dsEstimate.est_assemdistriboptions[0].mailtracking_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    _dsEstimate.est_assemdistriboptions[0].mailtracking_id = dr.GetInt64(dr.GetOrdinal("VND_MailTrackingRate_ID"));
                                else
                                    _dsEstimate.est_assemdistriboptions[0].Setmailtracking_idNull();
                                dr.Close();
                            }
                        }
                    }

                    if (!_dsEstimate.est_assemdistriboptions[0].Ismaillistresource_idNull())
                    {
                        using (SqlCommand cmd = new SqlCommand("VndMailListResourceRate_s_ByOldIDandRunDate", conn))
                        {
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@VND_MailListResourceRate_ID", _dsEstimate.est_assemdistriboptions[0].maillistresource_id);
                            cmd.Parameters.AddWithValue("@RunDate", _dsEstimate.est_estimate[0].rundate);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                    _dsEstimate.est_assemdistriboptions[0].maillistresource_id = dr.GetInt64(dr.GetOrdinal("VND_MailListResourceRate_ID"));
                                else
                                    _dsEstimate.est_assemdistriboptions[0].Setmaillistresource_idNull();
                                dr.Close();
                            }
                        }
                    }
                }
                conn.Close();
            }
        }

        private bool RunDateChanged()
        {
            if (_dsEstimate.est_estimate[0].rundate != _dtRunDate.Value.Date)
            {
                if ((_dsEstimate.est_component.Count > 0)
                    || (_dsEstimate.est_assemdistriboptions.Count > 0)
                    || (_dsEstimate.est_pubissuedates.Count > 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// If the user changes the Estimate Run Date some component fields have to be validated again.
        /// </summary>
        /// <returns></returns>
        private bool ValidateComponents()
        {
            foreach(Estimates.est_componentRow c_row in _dsEstimate.est_component)
            {
                // Printer Vendor is required
                if (c_row.Isprinter_idNull())
                {
                    return false;
                }

                // If Calc Print Cost is checked many of the print cost fields are required
                if (c_row.calculateprintcost)
                {
                    // Plate Cost
                    if (c_row.Isplatecost_idNull())
                    {
                        return false;
                    }

                    // Digital Handle & Prepare Rate
                    if (c_row.Isdigitalhandlenprepare_idNull())
                    {
                        return false;
                    }

                    // Stitch-In (required if ComponentType is Stitch-In)
                    if (c_row.est_componenttype_id == 3 && c_row.Isstitchin_idNull())
                    {
                        return false;
                    }
                    // Blow-In (required if ComponentType is Blow-In)
                    if (c_row.est_componenttype_id == 4 && c_row.Isblowin_idNull())
                    {
                        return false;
                    }
                    // Onsert (required if ComponentType is Onsert)
                    if (c_row.est_componenttype_id == 2 && c_row.Isonsert_idNull())
                    {
                        return false;
                    }
                    // Stitcher Makeready
                    if (c_row.Isstitchermakeready_idNull())
                    {
                        return false;
                    }
                    // Press Makeready
                    if (c_row.Ispressmakeready_idNull())
                    {
                        return false;
                    }
                }

                // Paper Vendor
                if (c_row.Ispaper_idNull())
                {
                    return false;
                }

                // If the Component is not VS, the Paper Description (Paper Map) is required.
                if (!c_row.vendorsupplied && c_row.Ispaper_map_idNull())
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidateAssemblyAndDistribution()
        {
            uctEstimate e = (uctEstimate)this.Parent.Parent.Parent;

            if (_dsEstimate.est_assemdistriboptions.Count == 0)
                return true;
            else if (_dsEstimate.est_assemdistriboptions[0].Ispst_postalscenario_idNull())
                return false;
            else if (_dsEstimate.est_assemdistriboptions[0].Ismailhouse_idNull())
                return false;
            else if ((_dsEstimate.est_assemdistriboptions[0].Ismailtracking_idNull()) && (_dsEstimate.est_assemdistriboptions[0].usemailtracking))
                return false;
            else if (_dsEstimate.est_assemdistriboptions[0].Ismaillistresource_idNull())
                return false;
            else
                return true;
        }

        private bool ValidateDistributionMapping()
        {
            if (((uctEstimate)this.Parent.Parent.Parent)._uctDistributionMapping.ValidateInsertSetup())
                return true;
            else
                return false;
        }

        #endregion

        public void SetChildControlFocus()
        {
            _txtDescription.Focus();
        }
    }
}