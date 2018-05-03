#region Using Directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace CatalogEstimating
{
    public partial class SplashForm : Form
    {
        #region Private Variables

        private const int FadeTimeMs = 500;
        private double _increment = 0.01;
        private bool _fadeOut = true;

        #endregion

        #region Construction

        public SplashForm()
        {
            InitializeComponent();
            Opacity = _increment;
            _lblAppName.Text = Application.ProductName;

            // Determine how many intervals it will take to get to 100 opacity given the increment size
            _timeFade.Interval = (int)((double)FadeTimeMs * _increment);
        }

        public SplashForm( bool fadeOut )
        : this()
        {
            _fadeOut = fadeOut;
        }

        #endregion

        #region Event Handlers

        private void _timeFade_Tick( object sender, EventArgs e )
        {
            if ( Opacity >= 0.99 )
            {
                _timeFade.Stop();

                if ( _fadeOut )
                    _timeSolid.Start();
            }
            else if ( Opacity == 0 )
            {
                _timeFade.Stop();
                Close();
            }
            else
            {
                Opacity += _increment;
            }
        }

        private void SplashForm_Load( object sender, EventArgs e )
        {
            if ( !DesignMode )
                _timeFade.Start();
        }

        private void _timeSolid_Tick( object sender, EventArgs e )
        {
            _timeSolid.Stop(); 
            _increment *= -1;
            Opacity += _increment;
            _timeFade.Start();
        }

        protected virtual void Form_Click( object sender, EventArgs e )
        {
            _timeFade.Stop();
            _timeSolid.Stop();
            Close();
        }

        #endregion
    }
}