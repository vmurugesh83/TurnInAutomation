#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#endregion

namespace CatalogEstimating.UserControls.Estimate
{
    public partial class ucpSamples : CatalogEstimating.UserControlPanel
    {
        #region Private Variables

        public Datasets.Estimates _dsEstimate;
        private Datasets.Estimates.est_samplesRow _sample;
        private bool _readOnly = true;
        private bool _loaded = false;

        #endregion

        #region Construction

        public ucpSamples(Datasets.Estimates dsEstimate, bool readOnly)
        {
            InitializeComponent();
            Name = "Samples";

            _dsEstimate = dsEstimate;
            _readOnly = readOnly;
        }

        #endregion

        #region Overrides

        public override void LoadData()
        {
            _txtQuantity.ReadOnly = _readOnly;
            _txtFreightCWT.ReadOnly = _readOnly;
            _txtFreightFlat.ReadOnly = _readOnly;

            _loaded = true;

            base.LoadData();
        }

        public override void Reload()
        {
            _sample = _dsEstimate.est_samples[0];

            _txtQuantity.Value    = _sample.quantity;
            _txtFreightCWT.Value  = _sample.freightcwt;
            _txtFreightFlat.Value = _sample.freightflat;

            if (_readOnly || (_sample.RowState == DataRowState.Unchanged))
                this.Dirty = false;

            _txtQuantity.Focus();
        }

        public override void PreSave(CancelEventArgs e)
        {
            if (_loaded)
            {
                if (!ValidateControl())
                {
                    e.Cancel = true;
                }
                else
                {
                    _sample.quantity    = _txtQuantity.Value.Value;
                    _sample.freightcwt  = _txtFreightCWT.Value.Value;
                    _sample.freightflat = _txtFreightFlat.Value.Value;

                    if (_sample.RowState == DataRowState.Modified)
                    {
                        _sample.modifiedby = MainForm.AuthorizedUser.FormattedName;
                        _sample.modifieddate = DateTime.Now;
                    }
                }
            }
        }

        public override void OnLeaving(CancelEventArgs e)
        {
            if (!ValidateControl())
            {
                MessageBox.Show(CatalogEstimating.Properties.Resources.InvalidData, "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
            else
            {
                _sample.quantity    = _txtQuantity.Value.Value;
                _sample.freightcwt  = _txtFreightCWT.Value.Value;
                _sample.freightflat = _txtFreightFlat.Value.Value;

                if (_sample.RowState == DataRowState.Modified)
                {
                    _sample.modifiedby = MainForm.AuthorizedUser.FormattedName;
                    _sample.modifieddate = DateTime.Now;
                }
            }
        }

        public override void Export( ref ExcelWriter writer )
        {
            writer.WriteLine( _lblQuantity.Text, _txtQuantity.Text );
            writer.WriteLine( _lblFreightCWT.Text, _txtFreightCWT.Text );
            writer.WriteLine( _lblFreightFlat.Text, _txtFreightFlat.Text );
        }
         
        #endregion

        #region Event Handlers

        private void _txtQuantity_TextChanged(object sender, EventArgs e)
        {
            this.Dirty = false;
            this.Dirty = CheckForChanges();
        }

        private void _txtFreightCWT_TextChanged(object sender, EventArgs e)
        {
            this.Dirty = false;
            this.Dirty = CheckForChanges();
        }

        private void _txtFreightFlat_TextChanged(object sender, EventArgs e)
        {
            this.Dirty = false;
            this.Dirty = CheckForChanges();
        }

        #endregion

        #region Private Methods

        private bool ValidateControl()
        {
            bool isValid = true;
            _errorProvider.Clear();

            if ((string.IsNullOrEmpty(_txtQuantity.Text)) || (_txtQuantity.Text == "."))
            {
                isValid = false;
                _errorProvider.SetError(_txtQuantity, Properties.Resources.RequiredFieldError);
            }

            if ((string.IsNullOrEmpty(_txtFreightCWT.Text) || (_txtFreightCWT.Text == "."))
                && (string.IsNullOrEmpty(_txtFreightFlat.Text) || (_txtFreightFlat.Text == ".")))
            {
                isValid = false;
                _errorProvider.SetError(_txtFreightCWT, Properties.Resources.RequiredFieldError);
                _errorProvider.SetError(_txtFreightFlat, Properties.Resources.RequiredFieldError);
            }
            else
            {
                if (string.IsNullOrEmpty(_txtFreightCWT.Text) || (_txtFreightCWT.Text == "."))
                {
                    _txtFreightCWT.Text = "0.00";
                }
                else if (string.IsNullOrEmpty(_txtFreightFlat.Text) || (_txtFreightFlat.Text == "."))
                {
                    _txtFreightFlat.Text = "0.00";
                }

                if (((decimal)_txtFreightCWT.Value > 0) && ((decimal)_txtFreightFlat.Value) > 0)
                {
                    isValid = false;
                    _errorProvider.SetError(_txtFreightCWT, Properties.Resources.FreightRateConflict);
                    _errorProvider.SetError(_txtFreightFlat, Properties.Resources.FreightRateConflict);
                }
            }

            return isValid;
        }

        private bool CheckForChanges()
        {
            bool dataChanged = false;

            if (!_readOnly)
            {
                if (_sample.RowState == DataRowState.Added)
                {
                    // new record will be added.
                    dataChanged = true;
                }
                else
                {
                    if ((string.IsNullOrEmpty(_txtQuantity.Text)) || (_txtQuantity.Text == ".") || (_sample.quantity != (int)_txtQuantity.Value))
                    {
                        dataChanged = true;
                    }

                    if ((string.IsNullOrEmpty(_txtFreightCWT.Text)) || (_txtFreightCWT.Text == ".") || (_sample.freightcwt != (int)_txtFreightCWT.Value))
                    {
                        dataChanged = true;
                    }

                    if ((string.IsNullOrEmpty(_txtFreightFlat.Text)) || (_txtFreightFlat.Text == ".") || (_sample.freightflat != (int)_txtFreightFlat.Value))
                    {
                        dataChanged = true;
                    }
                }
            }
            return dataChanged;
        }

        #endregion
    }
}