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
    public partial class ExceptionForm : Form
    {
        #region Private Variables

        bool _isExpanded = true;

        #endregion

        #region Construction

        public ExceptionForm()
        {
            InitializeComponent();
        }

        public ExceptionForm( Exception ex, string titleBar )
        : this( ex )
        {
            Text = titleBar;
        }

        public ExceptionForm( Exception ex )
        : this()
        {
            InitializeException( ex );

#if !DEBUG
            ToggleDetails();
#endif
        }

        #endregion

        #region Private Methods

        private void InitializeException( Exception ex )
        {
            _txtMessage.Text   = ex.Message;
            _txtCallStack.Text = ex.StackTrace;
        }

        private void ToggleDetails()
        {
            _isExpanded = !_isExpanded;

            _lblCallStack.Visible = _isExpanded;
            _txtCallStack.Visible = _isExpanded;

            Control anchorControl = null;
            if ( _isExpanded )
            {
                anchorControl    = _txtCallStack;
                _btnDetails.Text = "Details <<";
            }
            else
            {
                anchorControl    = _txtMessage;
                _btnDetails.Text = "Details >>";
            }

            _btnDetails.Top = anchorControl.Bottom + 8;
            _btnOK.Top      = anchorControl.Bottom + 8;
            _btnCopy.Top = anchorControl.Bottom + 8;

            Height = _btnOK.Bottom + _btnOK.Height + 8;
        }

        #endregion

        #region Event Handlers

        private void _btnDetails_Click( object sender, EventArgs e )
        {
            ToggleDetails();
        }

        private void _btnOK_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void _btnCopy_Click( object sender, EventArgs e )
        {
            Clipboard.SetDataObject( _txtMessage.Text + "\n" + _txtCallStack.Text, true );
        }

        #endregion
    }
}