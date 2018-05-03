#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

#endregion

namespace CatalogEstimating
{
    public partial class UserControlTab : UserControlPanel
    {
        #region Construction

        public UserControlTab()
        {
            InitializeComponent();
        }

        #endregion

        #region Protected Properties

        private List<UserControlPanel> _childControls = new List<UserControlPanel>();
        protected List<UserControlPanel> ChildControls
        {
            get { return _childControls; }
        }

        private UserControlPanel _selectedControl = null;
        protected UserControlPanel SelectedControl
        {
            get { return _selectedControl; }
        }

        public int SelectedTabIndex
        {
            set
            {
                _tabControl.SelectedIndex = value;
            }
        }

        #endregion

        #region Protected Methods
        protected void SetDirtyFalseOnChildControls()
        {
            foreach (UserControlPanel p in _childControls)
            {
                UserControlTab t = p as UserControlTab;
                p.Dirty = false;
                if (t != null)
                    t.SetDirtyFalseOnChildControls();
            }
        }

        protected void SetIsLoadingOnChildControls(bool isLoading)
        {
            foreach (UserControlPanel p in _childControls)
            {
                p.IsLoading = isLoading;
                UserControlTab t = p as UserControlTab;
                if (t != null)
                    t.SetIsLoadingOnChildControls(isLoading);
            }
        }

        #endregion

        #region Protected Overrides

        protected override void  OnCreateControl()
        {
            if ( !DesignMode )
            {
                // Add the ChildControls to the tab
                foreach ( UserControlPanel child in ChildControls )
                {
                    // Subscribe to the appropriate events for each child control
                    child.DirtyChanged += new EventHandler( ChildControl_DirtyChanged );
                    child.ControlActivated += new ControlActivatedHandler(ChildControl_ControlActivated);
                    child.Dock = DockStyle.Fill;

                    // Create the tab for the control and add it to the tab control
                    TabPage childPage = new TabPage( child.Name );
                    childPage.UseVisualStyleBackColor = true;
                    childPage.Controls.Add( child );
                    _tabControl.TabPages.Add( childPage );
                }

                _tabControl.SelectTab( 0 );
                _selectedControl = _tabControl.TabPages[0].Controls[0] as UserControlPanel;
                _selectedControl.Focus();
            }
            base.OnCreateControl();
        }

        protected override void OnLoad(EventArgs e)
        {
            _tabControl.Focus();
            base.OnLoad(e);
        }
        #endregion

        #region Public Overrides

        public override MenuStrip Menubar
        {
            get
            {
                if ( _selectedControl != null )
                    return _selectedControl.Menubar;
                else
                    return null;
            }
        }

        public sealed override void MergeToolstrip( ToolStrip target )
        {
            // Merge my toolstrip
            base.MergeToolstrip( target );

            // Merge my selected child toolstrip
            _selectedControl.MergeToolstrip( target );
        }

        public override void Reload()
        {
            _selectedControl.Reload();
        }

        public override void LoadData()
        {
            _selectedControl.IsLoading = true;
            _selectedControl.LoadData();
            base.LoadData();
        }

        public override void SaveData()
        {
            foreach ( UserControlPanel panel in _childControls )
            {
                if ( panel.Loaded )
                    panel.SaveData();
            }
        }

        public override void Export( ref ExcelWriter writer )
        {
            _selectedControl.Export( ref writer );
        }

        public override void OnLeaving( CancelEventArgs e )
        {
            _selectedControl.OnLeaving( e );
        }

        public override void PreSave( CancelEventArgs e )
        {
            // Don't short circuit.  Always call PreSave on all child controls
            foreach ( UserControlPanel child in ChildControls )
            {
                CancelEventArgs newArg = new CancelEventArgs();
                child.PreSave( newArg );
                e.Cancel = e.Cancel || newArg.Cancel;
            }
            base.PreSave( e );
        }

        #endregion

        #region Event Handlers

        private void ChildControl_DirtyChanged( object sender, EventArgs e )
        {
            // If any children are dirty, so am I
            bool totalDirty = false;

            UserControlPanel sendCtrl = sender as UserControlPanel;
            foreach ( TabPage childPage in _tabControl.TabPages )
            {
                UserControlPanel pageCtrl = childPage.Controls[0] as UserControlPanel;
                totalDirty = totalDirty || pageCtrl.Dirty;

                if ( childPage.Controls[0] == sender )
                {
                    if ( sendCtrl.Dirty && !childPage.Text.Contains( "*" ) )
                        childPage.Text = sendCtrl.Name + " *";
                    else if ( !sendCtrl.Dirty && childPage.Text.Contains( "*" ) )
                        childPage.Text = sendCtrl.Name;
                }
            }

            // Now raise the event to my parent since if a child is dirty, so am I
            Dirty = totalDirty;
        }

        private void ChildControl_ControlActivated( object sender, ControlActivatedArgs e )
        {
            OnControlActivated( sender, e );
        }
        
        private void _tabControl_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( !DesignMode )
            {
                UserControlPanel oldControl = _selectedControl;
                _selectedControl = _tabControl.SelectedTab.Controls[0] as UserControlPanel;

                OnControlActivated( this, new ControlActivatedArgs( oldControl, _selectedControl ) );

                if ( !_selectedControl.Loaded )
                {
                    _selectedControl.IsLoading = true;
                    _selectedControl.LoadData();
                }

                _selectedControl.Reload();
            }
        }

        private void _tabControl_Selecting( object sender, TabControlCancelEventArgs e )
        {
            CancelEventArgs ctrlEvent = new CancelEventArgs();
            _selectedControl.OnLeaving( ctrlEvent );

            if ( ctrlEvent.Cancel )
                e.Cancel = true;
        }

        #endregion
    }
}