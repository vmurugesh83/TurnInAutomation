using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using CatalogEstimating.Datasets;

namespace CatalogEstimating.UserControls.Reports
{
    public partial class rptEstimate : CatalogEstimating.UserControls.Reports.ReportControl
    {
        public rptEstimate(Lookup dsLookup) : base(dsLookup)
        {
            InitializeComponent();
        }

        public override string ReportTemplate
        {
            get
            {
                return "Estimate";
            }
        }

        public override ReportExecutionStatus RunReport(ExcelWriter writer)
        {
            if (_txtEstimateID.Text == String.Empty)
            {
                _errorProvider.SetError(_txtEstimateID, CatalogEstimating.Properties.Resources.RequiredFieldError);
                return ReportExecutionStatus.InvalidSearchCriteria;
            }
            else
                _errorProvider.SetError(_txtEstimateID, String.Empty);

            EstimateReport reportEstimate = new EstimateReport();
            if (reportEstimate.RunReport(writer, Convert.ToInt64(_txtEstimateID.Text.Trim())))
                return ReportExecutionStatus.Success;
            else
                return ReportExecutionStatus.NoDataReturned;
        }
    }
}