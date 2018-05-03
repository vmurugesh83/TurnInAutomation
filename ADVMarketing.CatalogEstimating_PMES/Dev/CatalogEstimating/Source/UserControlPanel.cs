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
    public partial class UserControlPanel : UserControl
    {
        #region Construction

        public UserControlPanel()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        protected bool _dirty = false;
        [Browsable( false )]
        public virtual bool Dirty
        {
            get { return _dirty; }
            set
            {
                if ( _dirty != value )
                {
                    _dirty = value;
                    OnDirtyChanged( this, EventArgs.Empty );
                }
            }
        }

        [Browsable( false )]
        public virtual ToolStrip Toolbar
        {
            get { return null; }
        }

        [Browsable( false )]
        public virtual MenuStrip Menubar
        {
            get { return null; }
        }

        private bool _loaded = false;
        [Browsable( false )]
        public bool Loaded
        {
            get { return _loaded; }
        }

        private bool _isLoading = false;
        [Browsable( false )]
        public bool IsLoading
        {
            get { return _isLoading;  }
            set { _isLoading = value; }
        }

        #endregion

        #region Protected Overrides

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if ( Menubar != null )
                Controls.Remove( Menubar );
        }

        #endregion

        #region Public Events

        public event EventHandler DirtyChanged;
        protected virtual void OnDirtyChanged( object sender, EventArgs e )
        {
            if ( DirtyChanged != null )
                DirtyChanged( sender, e );
        }

        public event ControlActivatedHandler ControlActivated;
        protected virtual void OnControlActivated( object sender, ControlActivatedArgs e )
        {
            if ( ControlActivated != null )
                ControlActivated( sender, e );
        }

        #endregion

        #region Public Methods

        public virtual void MergeToolstrip( ToolStrip target )
        {
            if ( Toolbar != null )
            {
                ToolStripManager.Merge( Toolbar, target );
                Toolbar.Visible = false;
            }
        }

        public virtual void Reload()
        { }

        public virtual void Export( ref ExcelWriter writer )
        { }

        public virtual void LoadData()
        {
            _isLoading = false;
            _loaded = true;
        }

        public virtual void SaveData()
        {
            Dirty = false;
        }

        public virtual void OnLeaving( CancelEventArgs e )
        { }

        public virtual void PreSave( CancelEventArgs e )
        { }

        #endregion
    }
}