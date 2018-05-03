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
    public partial class PolybagPickerForm : Form
    {
        #region Private Variables

        private List<PolybagEditForm> _listPolybagGroups;

        #endregion

        #region Construction

        public PolybagPickerForm()
        {
            InitializeComponent();
        }

        public PolybagPickerForm( List<PolybagEditForm> openGroups )
        : this()
        {
            SetGroupList( openGroups );
        }

        #endregion

        #region Private Methods

        private void SetGroupList( List<PolybagEditForm> openGroups )
        {
            _listPolybagGroups = openGroups;

            // Add a window in the list for a brand new polybag
            _cboPolybagGroups.Items.Add( "(New Polybag Group)" );

            foreach ( PolybagEditForm form in openGroups )
                _cboPolybagGroups.Items.Add( form.Text );

            // Set the default selected item
            _cboPolybagGroups.SelectedIndex = 0;
        }

        private void _btnOK_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void _btnCancel_Click( object sender, EventArgs e )
        {
            Close();
        }

        #endregion

        #region Public Properties

        public PolybagEditForm PolybagGroup
        {
            get
            {
                if ( _cboPolybagGroups.SelectedIndex > 0 )
                    return _listPolybagGroups[_cboPolybagGroups.SelectedIndex - 1];
                else
                    return null;
            }
        }

        #endregion
    }
}