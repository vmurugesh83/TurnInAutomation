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
    public partial class uctDistributionMapping : CatalogEstimating.UserControlTab
    {
        #region Private Variables

        public Datasets.Estimates _dsEstimate;
        public Datasets.DistributionMapping _dsDistMapping;
        internal ucpInsertSetup _ucpInsertSetup = null;
        internal ucpMappings _ucpMappings = null;

        #endregion

        #region Construction

        public uctDistributionMapping(Datasets.Estimates dsEstimate, Datasets.DistributionMapping dsDistMapping, bool readOnly)
        {
            InitializeComponent();

            Name = "Distribution Mapping";
            _dsEstimate = dsEstimate;
            _dsDistMapping = dsDistMapping;

            _ucpInsertSetup = new ucpInsertSetup(_dsEstimate, _dsDistMapping, readOnly);
            _ucpMappings = new ucpMappings(_dsEstimate, _dsDistMapping, readOnly);

            ChildControls.Add(_ucpInsertSetup);
            ChildControls.Add(_ucpMappings);
        }

        #endregion

        #region Overrides

        public override void PreSave(CancelEventArgs e)
        {
            base.PreSave(e);
        }

        public override void OnLeaving(CancelEventArgs e)
        {
            base.OnLeaving(e);
        }

        #endregion

        #region Event Handlers

        #endregion

        #region Public Methods

        public bool ValidateInsertSetup()
        {
            bool isValid = true;

            isValid = _ucpInsertSetup.ValidateInsertSetup();

            return isValid;
        }

        #endregion

        #region Public Properties
        public UserControlPanel Mappings
        {
            get
            {
                return ChildControls[1];
            }
        }
        #endregion
    }
}