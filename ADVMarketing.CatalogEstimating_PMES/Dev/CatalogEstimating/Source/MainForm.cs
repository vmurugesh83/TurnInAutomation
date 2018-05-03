#region Using Directives

using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using CatalogEstimating.Properties;

#endregion

namespace CatalogEstimating
{
    public partial class MainForm : Form
    {
        #region PInvoke Structures and Methods

        [DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = false )]
        static extern bool SetProp( IntPtr hWnd, string lpString, IntPtr hData );

        [DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = false )]
        static extern IntPtr SendMessage( IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam );

        private const uint WM_KEYDOWN = 0x100;
        private const uint WM_KEYUP = 0x102;
        private const uint WM_CHAR = 0x102;

        #endregion

        #region Private Variables

        private List<ChildForm> _childForms = new List<ChildForm>();
        private UserControlPanel _selectedControl = null;

        #endregion

        #region Construction

        public MainForm()
        {
            InitializeComponent();
            RefreshWindowList();

            _statusStripVersion.Text = String.Format( "Version: {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString() );

            _selectedControl = _tabEstimateSearch.Controls[0] as UserControlPanel;
            _selectedControl.MergeToolstrip( _toolStrip );

            //if ( _selectedControl.Toolbar != null )
            //    _toolStripContainer.TopToolStripPanel.Controls.Add( _selectedControl.Toolbar );

            if ( _selectedControl.Menubar != null )
                ToolStripManager.Merge( _selectedControl.Menubar, _menuStrip );
        }

        #endregion

        #region Public Properties

        static private AuthorizedUser _user = null;
        static public AuthorizedUser AuthorizedUser
        {
            get { return _user;  }
            set { _user = value; }
        }

        static private CESDatabase _workingDatabase = null;
        static public CESDatabase WorkingDatabase
        {
            get { return _workingDatabase; }
        }

        private List<CESDatabase> _databases;
        public List<CESDatabase> Databases
        {
            get { return _databases;  }
            set 
            { 
                _databases = value;

                // Now iterate through the list and determine our working database
                foreach ( CESDatabase db in value )
                {
                    if ( db.IsWorking )
                    {
                        _workingDatabase = db;
                        bool retVal = SetProp( this.Handle, "WorkingDatabase", new IntPtr( db.Id ) );
                        _statusStripDatabase.Text = string.Format( "Connected To: {0}", db.FriendlyName );
                        break;
                    }
                }
            }
        }

        public string LastAction
        {
            get { return _statusLastAction.Text;  }
            set { _statusLastAction.Text = value; }
        }

        #endregion

        #region Public Methods

        public static void UpdateDateControl( DateTimePicker dt )
        {
            if (dt.Focused)
            {
                IntPtr wParam = (IntPtr)47;     // '/'
                SendMessage(dt.Handle, WM_KEYDOWN, wParam, IntPtr.Zero);
                SendMessage(dt.Handle, WM_KEYUP, wParam, IntPtr.Zero);
            }
        }

        public bool IsEstimateOpen(long estimateID)
        {
            foreach (ChildForm iterForm in _childForms)
            {
                EstimateForm estForm = iterForm as EstimateForm;
                if (estForm != null)
                {
                    if (estForm.DatabaseId == estimateID)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public EstimateForm OpenEstimate( bool readOnly, long? estimateId, long? parentId, int? selectedTabIndex )
        {
            if ( estimateId != null )
            {
                foreach ( ChildForm iterForm in _childForms )
                {
                    EstimateForm estForm = iterForm as EstimateForm;
                    if ( estForm != null )
                    {
                        if ( estForm.DatabaseId == estimateId )
                        {
                            if (selectedTabIndex != null)
                            {
                                estForm.SelectedTabIndex = selectedTabIndex.Value;
                            }

                            estForm.Activate();
                            return estForm;
                        }
                    }
                }
            }

            EstimateForm form = new EstimateForm( this, readOnly, estimateId, parentId );
            ShowChildForm( form );

            if (selectedTabIndex != null)
            {
                form.SelectedTabIndex = selectedTabIndex.Value;
            }

            return form;
        }

        public PolybagEditForm OpenPolybagGroup( bool readOnly, long? polybagGroupId )
        {
            if ( polybagGroupId != null )
            {
                foreach ( ChildForm iterForm in _childForms )
                {
                    PolybagEditForm polybagForm = iterForm as PolybagEditForm;
                    if ( polybagForm != null )
                    {
                        if ( polybagForm.DatabaseId == polybagGroupId )
                        {
                            polybagForm.Activate();
                            return polybagForm;
                        }
                    }
                }
            }

            PolybagEditForm form = new PolybagEditForm(this, readOnly, polybagGroupId);

            ShowChildForm( form );
            return form;
        }

        public void AddEstimatesToPolybagGroup( List<long> estimateIds )
        {
            if ( estimateIds.Count == 0 )
                return;

            List<PolybagEditForm> polybagGroups = new List<PolybagEditForm>();
            foreach ( ChildForm child in _childForms )
            {
                PolybagEditForm polybagForm = child as PolybagEditForm;
                if ( polybagForm != null && !polybagForm.ReadOnly)
                    polybagGroups.Add( polybagForm );
            }

            using ( PolybagPickerForm picker = new PolybagPickerForm( polybagGroups ) )
            {
                try
                {
                    if (picker.ShowDialog(this) == DialogResult.OK)
                    {
                        PolybagEditForm polybagGroup = picker.PolybagGroup;
                        if (polybagGroup == null)
                            polybagGroup = OpenPolybagGroup(false, null);

                        // Add the selected estimates to the polybag group
                        foreach (long estimateId in estimateIds)
                            polybagGroup.AddEstimate(estimateId);

                        // Show the polybag group form
                        polybagGroup.Activate();
                    }
                }
                catch ( CatalogEstimating.Exceptions.PolybagGroupNotExistException )
                {
                    MessageBox.Show("The polybag group has been deleted.", "Cannot Find Polybag Group", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        public void DisplayUploadControl(List<long> estimateIds)
        {
            _estimateUploadControl.UploadEstimates = estimateIds;
            _selectedControl = _tabEstimateUpload.Controls[0] as UserControlPanel;
            _selectedControl.Focus();
            _tabControl.SelectedIndex = 3;
            _estimateUploadControl.ValidateEstimates();
        }

        private void ShowChildForm( ChildForm form )
        {
            form.FormClosed  += new FormClosedEventHandler( ChildForm_Closed );
            form.TextChanged += new EventHandler( ChildForm_TextChanged );
            _childForms.Add( form );
            RefreshWindowList();
            form.Show();
            form.SetChildControlFocus();
        }

        #endregion

        #region Protected Overrides

        protected override void OnClosed( EventArgs e )
        {
            if ( _user != null )
                _user.Dispose();

            base.OnClosed( e );
        }

        protected override void OnCreateControl()
        {
            if ( !DesignMode )
            {
                foreach ( TabPage page in _tabControl.TabPages )
                {
                    UserControlPanel panel = page.Controls[0] as UserControlPanel;
                    panel.ControlActivated += new ControlActivatedHandler( UserControlPanel_ControlActivated );
                    panel.DirtyChanged += new EventHandler( UserControlPanel_DirtyChanged );
                }

                _selectedControl.IsLoading = true;
                _selectedControl.LoadData();
                _selectedControl.Reload();
            }
            base.OnCreateControl();
        }

        #endregion

        #region Menu Item Event Handlers

        private void _menuHelpAbout_Click( object sender, EventArgs e )
        {
            AboutForm about = new AboutForm();
            about.ShowDialog( this );
        }

        private void _menuFileNewEstimate_Click( object sender, EventArgs e )
        {
            EstimateForm newEstimate = new EstimateForm(this, false, null, null);
            ShowChildForm( newEstimate );
        }

        private void _menuFileExit_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void _menuFileNewPolybag_Click( object sender, EventArgs e )
        {
            PolybagEditForm newPolybag = new PolybagEditForm( this, false, null );
            ShowChildForm( newPolybag );
        }

        private void menuWindowItem_Click( object sender, EventArgs e )
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            Form theForm = menu.Tag as Form;
            theForm.Activate();
        }

        #endregion

        #region ChildForm Events

        private void ChildForm_Closed( object sender, FormClosedEventArgs e )
        {
            _childForms.Remove( sender as ChildForm );
            RefreshWindowList();
        }

        private void ChildForm_TextChanged( object sender, EventArgs e )
        {
            RefreshWindowList();
        }

        #endregion

        #region Tab Control Events

        private void UserControlPanel_ControlActivated( object sender, ControlActivatedArgs e )
        {
            RefreshToolStrip();
        }

        private void UserControlPanel_DirtyChanged( object sender, EventArgs e )
        {
            UserControlPanel sendCtrl = sender as UserControlPanel;
            foreach ( TabPage childPage in _tabControl.TabPages )
            {
                if ( childPage.Controls[0] == sender )
                {
                    if ( sendCtrl.Dirty && !childPage.Text.Contains( "*" ) )
                        childPage.Text = childPage.Text + " *";
                    else if ( !sendCtrl.Dirty && childPage.Text.Contains( "*" ) )
                        childPage.Text = childPage.Text.Remove( childPage.Text.Length - 2, 2 );
                }
            }

        }

        private void _tabControl_SelectedIndexChanged( object sender, EventArgs e )
        {
            _selectedControl = _tabControl.SelectedTab.Controls[0] as UserControlPanel;
            RefreshToolStrip();

            if ( !_selectedControl.Loaded )
            {
                _selectedControl.IsLoading = true;
                _selectedControl.LoadData();
            }

            _selectedControl.Reload();
        }

        private void _tabControl_Selecting( object sender, TabControlCancelEventArgs e )
        {
            CancelEventArgs ctrlEvent = new CancelEventArgs();
            _selectedControl.OnLeaving( ctrlEvent );

            if ( ctrlEvent.Cancel )
                e.Cancel = true;
        }

        #endregion

        #region Other Events

        private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            bool dirty = false;
            foreach ( TabPage page in _tabControl.TabPages )
            {
                UserControlPanel pagePanel = page.Controls[0] as UserControlPanel;
                if ( pagePanel.Dirty )
                {
                    dirty = true;
                    break;
                }
            }

            if ( dirty )
            {
                DialogResult result = MessageBox.Show( Resources.ApplicationExitDirtyWarning, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
                if ( result == DialogResult.Yes )
                    e.Cancel = false;   // Force exit because validation may have occurred improperly
                else
                    e.Cancel = true;    // Despite validation, don't allow exit
            }

            if (e.Cancel == false)
            {
                ChildForm child = null;

                for (int i = _childForms.Count - 1; i >= 0; i--)
                {
                    child = _childForms[i];
                    child.Activate();
                    child.Close();
                }

                if (_childForms.Count != 0)
                {
                    e.Cancel = true;
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PerformanceLog.Close();
        }

        private void MainForm_Load( object sender, EventArgs e )
        {
            this.Text = string.Concat( CatalogEstimating.Properties.Resources.ApplicationEnvironment, " ", CatalogEstimating.Properties.Resources.MainFormTitle );
        }

        #endregion

        #region Private Methods

        private void RefreshToolStrip()
        {
            ToolStripManager.RevertMerge( _toolStrip );
            _selectedControl.MergeToolstrip( _toolStrip );

            // Add the new controls
            if ( _selectedControl.Menubar != null )
                ToolStripManager.Merge( _selectedControl.Menubar, _menuStrip );
        }

        private void RefreshWindowList()
        {
            _menuWindow.DropDownItems.Clear();
            List<ToolStripMenuItem> windowMenus = GetWindowList();
            windowMenus[0].Checked = true;
            _menuWindow.DropDownItems.AddRange( windowMenus.ToArray() );

            foreach ( ChildForm form in _childForms )
                form.RefreshWindowList( GetWindowList() );
        }

        private List<ToolStripMenuItem> GetWindowList()
        {
            List<ToolStripMenuItem> windowMenus = new List<ToolStripMenuItem>();

            // Create a menu item for the main form
            ToolStripMenuItem mainMenu = new ToolStripMenuItem();
            mainMenu.Text = "Main Window";
            mainMenu.Click += new EventHandler( menuWindowItem_Click );
            mainMenu.Tag = this;
            windowMenus.Add( mainMenu );

            // Create a menu item for each Child Form
            foreach ( ChildForm form in _childForms )
            {
                ToolStripMenuItem menu = new ToolStripMenuItem();
                menu.Text = form.Text;
                menu.Click += new EventHandler( menuWindowItem_Click );
                menu.Tag = form;
                windowMenus.Add( menu );
            }

            return windowMenus;
        }

        #endregion

       
    }
}