#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

#endregion

using CatalogEstimating.Properties;

namespace CatalogEstimating
{
    public partial class ChildForm : Form
    {
        #region Construction

        public ChildForm()
        : this( null, false )
        {
        }

        public ChildForm( MainForm parent, bool readOnly )
        {
            InitializeComponent();

            _readOnly = readOnly;
            _parentForm = parent;
        }

        #endregion

        #region Protected Variables

        protected bool _readOnly = false;
        protected MainForm _parentForm = null;
        protected UserControlPanel _mainControl = null;

        #endregion

        #region Public Properties

        public bool ReadOnly
        {
            get { return _readOnly;  }
            set { _readOnly = value; }
        }

        public MainForm MainForm
        {
            get { return _parentForm; }
        }

        public virtual long? DatabaseId
        {
            get { return null; }
        }

        public string LastAction
        {
            get { return _statusLastAction.Text;  }
            set { _statusLastAction.Text = value; }
        }

        #endregion

        #region Public Methods

        public void RefreshWindowList( List<ToolStripMenuItem> windowMenus )
        {
            _menuWindow.DropDownItems.Clear();
            _menuWindow.DropDownItems.AddRange( windowMenus.ToArray() );

            foreach ( ToolStripMenuItem menu in windowMenus )
                menu.Checked = ( menu.Tag == this );
        }

        public virtual void SetChildControlFocus()
        { }

        #endregion

        #region Private Methods

        private void RefreshToolStrip()
        {
            ToolStripManager.RevertMerge( _toolStrip );
            _mainControl.MergeToolstrip( _toolStrip );

            // Add the new controls
            if ( _mainControl.Menubar != null )
                ToolStripManager.Merge( _mainControl.Menubar, _menuStrip );
        }

        #endregion

        #region Event Handlers

        private void _mainControl_ControlActivated( object sender, ControlActivatedArgs e )
        {
            RefreshToolStrip();
        }

        private void _mainControl_DirtyChanged( object sender, EventArgs e )
        {
            if ( _mainControl.Dirty && !Text.Contains( "*" ) )
                Text = Text + " *";
            else if ( !_mainControl.Dirty && Text.Contains( "*" ) )
                Text = Text.Substring( 0, Text.Length - 2 );
        }

        private void ChildForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            if ( _mainControl.Dirty )
            {
                DialogResult result = MessageBox.Show( Resources.ApplicationExitDirtyWarning, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
                if ( result == DialogResult.Yes )
                    e.Cancel = false;   // Force exit because validation may have occurred improperly
                else
                    e.Cancel = true;    // Despite validation, don't allow exit
            }
            else
                e.Cancel = false;
        }

        private void _menuHelpAbout_Click( object sender, EventArgs e )
        {
            AboutForm about = new AboutForm();
            about.ShowDialog( this );
        }

        private void _menuFileExit_Click( object sender, EventArgs e )
        {
            Close();
        }

        #endregion

        #region Protected Overrides

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if ( _mainControl != null )
            {
                _mainControl.ControlActivated +=new ControlActivatedHandler( _mainControl_ControlActivated );
                _mainControl.DirtyChanged     += new EventHandler( _mainControl_DirtyChanged );
                _toolStripContainer.ContentPanel.Controls.Add( _mainControl );

                RefreshToolStrip();

                _mainControl.IsLoading = true;
                _mainControl.LoadData();
                _mainControl.Reload();

                _statusStripVersion.Text = String.Format( "Version: {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString() );
                if ( _parentForm != null )
                    _statusStripDatabase.Text = string.Format( "Connected To: {0}", MainForm.WorkingDatabase.FriendlyName );
            }
        }

        #endregion
    }
}