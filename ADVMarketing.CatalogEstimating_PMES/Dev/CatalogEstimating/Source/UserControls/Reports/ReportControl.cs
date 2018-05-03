#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.LookupTableAdapters;

#endregion

namespace CatalogEstimating.UserControls.Reports
{
    public partial class ReportControl : UserControl
    {
        protected Lookup _dsLookup;

        public ReportControl()
        {
            InitializeComponent();
        }

        public ReportControl(Lookup dsLookup) : this()
        {
            _dsLookup = dsLookup;
        }

        /// <summary>
        /// Returns 1 if criteria returned data, 0 if not, -1 if search criteria is not valid
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public virtual ReportExecutionStatus RunReport( ExcelWriter writer )
        {
            throw new NotImplementedException();
        }

        public virtual string ReportTemplate
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}