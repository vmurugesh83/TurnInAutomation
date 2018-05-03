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

namespace CatalogEstimating
{
    public partial class AboutForm : CatalogEstimating.SplashForm
    {
        public AboutForm()
        : base( false )
        {
            InitializeComponent();
            _lblVersion.Text = String.Format( "Version {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString() );
        }

        protected override void Form_Click( object sender, EventArgs e )
        {
            // Do nothing
        }

    }
}