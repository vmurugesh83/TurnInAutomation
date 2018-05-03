#region Using Directives

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Security.Principal;
using System.Threading;

#endregion

namespace CatalogEstimating
{
    static class Program
    {
        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy( PrincipalPolicy.WindowsPrincipal );

            // Setup the unhandled exception handler
            UnhandledExceptionHandler eh = new UnhandledExceptionHandler();
            AppDomain.CurrentDomain.UnhandledException +=new UnhandledExceptionEventHandler( eh.OnUnhandledException );
            Application.ThreadException +=new ThreadExceptionEventHandler( eh.OnThreadException );

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

#if !DEBUG
            // Only show the splash screen in release mode
            SplashForm splash = new SplashForm();
            splash.ShowDialog();
#endif

            // First check to see if the current principal is authorized to use this program
            AuthorizedUser user = new AuthorizedUser();
            if ( user.Right == UserRights.Exclude )
            {
                LoginForm login = new LoginForm();
                try
                {
                    // Prompt user for new login
                    if ( login.ShowDialog() == DialogResult.OK )
                        user = login.User;
                    else
                        return;
                }
                catch ( Exception ex )
                {
                    ExceptionForm exForm = new ExceptionForm( ex, "Error Impersonating User" );
                    exForm.ShowDialog();
                    exForm.Dispose();
                }
                finally
                {
                    login.Dispose();
                }
            }

            DatabaseForm dbForm = null;
            try
            {
                // Now ask them which database they'd like to connect to
                dbForm = new DatabaseForm();
                if ( dbForm.ShowDialog() == DialogResult.Cancel )
                {
                    // Clean up any potentially allocated resources
                    user.Dispose();
                    dbForm.Dispose();
                    return;
                }
            }
            catch ( Exception ex )
            {
                ExceptionForm exForm = new ExceptionForm( ex, "Error Connecting to Live Database" );
                user.Dispose();
                exForm.ShowDialog();
                exForm.Dispose();
                return;
            }

            // Create the primary form and go
            MainForm mainForm       = new MainForm();
            MainForm.AuthorizedUser = user;
            mainForm.Databases      = dbForm.Databases;

            Application.Run( mainForm );
        }

        #region Unhandled Exception Handler

        private class UnhandledExceptionHandler
        {
            public void OnThreadException( object sender, ThreadExceptionEventArgs t )
            {
                HandleException( t.Exception );
            }

            public void OnUnhandledException( object sender, UnhandledExceptionEventArgs args )
            {
                HandleException( (Exception)args.ExceptionObject );
            }

            private void HandleException( Exception ex )
            {
                // TODO: NLS - Log exception here
                ExceptionForm exForm = new ExceptionForm( ex );
                exForm.ShowDialog();
            }
        }

        #endregion
    }
}