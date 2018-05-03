#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#endregion

namespace CatalogEstimating
{
    public partial class EstimateForm : CatalogEstimating.ChildForm
    {
        #region Private Data

        private long? _EstimateID = null;
        private long? _EstimateParentID = null;

        #endregion

        #region Construction

        public EstimateForm()
        : this( null, false )
        {
        }

        public EstimateForm(MainForm parent, bool readOnly)
            : base(parent, readOnly)
        {
            InitializeComponent();
            _mainControl = _estimateControl;
            _menuFileSave.Click += new EventHandler( _menuFileSave_Click );
        }

        public EstimateForm( MainForm parent, bool readOnly, long? EstimateID, long? EstimateParentID )
        : base(parent, readOnly)
        {
            _EstimateID = EstimateID;
            _EstimateParentID = EstimateParentID;

            InitializeComponent();
            _estimateControl.Initialize(_EstimateID, _EstimateParentID, readOnly);
            _mainControl = _estimateControl;
            _menuFileSave.Click += new EventHandler( _menuFileSave_Click );
        }

        #endregion

        #region Public Properties

        public override long? DatabaseId
        {
            get { return _EstimateID; }
        }

        public int SelectedTabIndex
        {
            set { _estimateControl.SelectedTabIndex = value; }
        }

        #endregion

        #region Event Handlers

        private void _menuFileSave_Click( object sender, EventArgs e )
        {
            CancelEventArgs ctrlEvent = new CancelEventArgs();
            _estimateControl.PreSave( ctrlEvent );

            if ( !ctrlEvent.Cancel )
                _estimateControl.SaveData();
        }

        #endregion

        public override void SetChildControlFocus()
        {
            ((CatalogEstimating.UserControls.Estimate.uctEstimate)_mainControl).EstimateSetupPanel.SetChildControlFocus();
        }
    }
}