#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Diagnostics;

#endregion

namespace CatalogEstimating
{
    public partial class LoginForm : Form
    {
        #region PInvoke Structures and Methods

        [DllImport( "Netapi32.dll", EntryPoint = "NetApiBufferFree" )]
        private static extern uint NetApiBufferFree( IntPtr buffer );

        [DllImport( "Netapi32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto )]
        private static extern uint DsEnumerateDomainTrusts(
            string ServerName,
            DS_DOMAIN_TRUST_TYPE Flags,
            out IntPtr Domains,
            out uint DomainCount );

        [Flags]
        private enum DS_DOMAIN_TRUST_TYPE : uint
        {
            DS_DOMAIN_IN_FOREST = 0x0001,  // Domain is a member of the forest
            DS_DOMAIN_DIRECT_OUTBOUND = 0x0002,  // Domain is directly trusted
            DS_DOMAIN_TREE_ROOT = 0x0004,  // Domain is root of a tree in the forest
            DS_DOMAIN_PRIMARY = 0x0008,  // Domain is the primary domain of queried server
            DS_DOMAIN_NATIVE_MODE = 0x0010,  // Primary domain is running in native mode
            DS_DOMAIN_DIRECT_INBOUND = 0x0020   // Domain is directly trusting
        }

        [StructLayout( LayoutKind.Sequential )]
        private struct DS_DOMAIN_TRUSTS
        {
            [MarshalAs( UnmanagedType.LPTStr )]
            public string NetbiosDomainName;
            [MarshalAs( UnmanagedType.LPTStr )]
            public string DnsDomainName;
            public uint Flags;
            public uint ParentIndex;
            public uint TrustType;
            public uint TrustAttributes;
            public IntPtr DomainSid;
            public Guid DomainGuid;
        }

        [DllImport( "advapi32.dll", SetLastError = true )]
        static extern bool LogonUser(
            string principal,
            string authority,
            string password,
            LogonSessionType logonType,
            LogonProvider logonProvider,
            out IntPtr token );


        [DllImport( "kernel32.dll", SetLastError = true )]
        static extern bool CloseHandle( IntPtr handle );

        enum LogonSessionType : uint
        {
            Interactive = 2,
            Network,
            Batch,
            Service,
            NetworkCleartext = 8,
            NewCredentials
        }

        enum LogonProvider : uint
        {
            Default = 0, // default for platform (use this!)
            WinNT35,     // sends smoke signals to authority
            WinNT40,     // uses NTLM
            WinNT50      // negotiates Kerb or NTLM
        }

        #endregion

        #region Construction

        public LoginForm()
        {
            InitializeComponent();

            foreach ( string domain in GetDomains() )
            {
                _cboDomain.Items.Add( domain );
            }
            _cboDomain.SelectedIndex = 0;
            _txtUserName.Focus();
        }

        #endregion

        #region Public Properties

        AuthorizedUser _user = null;
        public AuthorizedUser User
        {
            get { return _user; }
        }

        #endregion

        #region Event Handlers

        private void cmdLogin_Click( object sender, EventArgs e )
        {
            DialogResult = DialogResult.None;       // Assume fail

            if ( ValidateChildren() )
            {
                try
                {
                    IntPtr userToken;

                    // Create a token for DomainName\UserName
                    bool result = LogonUser( _txtUserName.Text, _cboDomain.Text, _txtPassword.Text,
                                             LogonSessionType.Interactive, LogonProvider.Default, out userToken );

                    if ( result )
                    {
                        AuthorizedUser user = new AuthorizedUser( userToken );
                        if ( user.Right == UserRights.Exclude )
                        {
                            MessageBox.Show( "User does not have access to this application" );
                            user.Dispose();     // Stop impersonating that user
                        }
                        else
                        {
                            _user = user;
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show( "Login Failed" );
                    }
                }
                catch ( Exception exLogin )
                {
                    MessageBox.Show( exLogin.Message, "Login Failed" );
                }

                _txtPassword.Text = String.Empty;
            }

        }

        private void txtUserName_Validating( object sender, CancelEventArgs e )
        {
            if ( _txtUserName.Text.Trim().Length == 0 )
            {
                _errorProvider.SetError( _txtUserName, "Please provide a user name" );
                e.Cancel = true;
            }
        }

        private void txtUserName_Validated( object sender, EventArgs e )
        {
            _errorProvider.SetError( _txtUserName, null );
        }

        #endregion

        #region Private Methods

        private List<string> GetDomains()
        {
            // What trust types are we interested in ?
            DS_DOMAIN_TRUST_TYPE trustTypes = DS_DOMAIN_TRUST_TYPE.DS_DOMAIN_PRIMARY | DS_DOMAIN_TRUST_TYPE.DS_DOMAIN_DIRECT_OUTBOUND;

            IntPtr buf;
            uint numDomains = 0;
            List<string> returnList = new List<string>();

            // Make the call - not doing anything special with the result value here
            uint result = DsEnumerateDomainTrusts( null, trustTypes, out buf, out numDomains );

            try
            {
                if ( ( numDomains > 0 ) && ( result == 0 ) )
                {
                    // Marshal the received buffer to managed structs
                    DS_DOMAIN_TRUSTS trust;
                    IntPtr iter = buf;

                    for ( int i = 0; i < numDomains; i++ )
                    {
                        trust = (DS_DOMAIN_TRUSTS)Marshal.PtrToStructure( iter, typeof( DS_DOMAIN_TRUSTS ) );
                        returnList.Add( trust.NetbiosDomainName );
                        iter = (IntPtr)( (int)iter + Marshal.SizeOf( typeof( DS_DOMAIN_TRUSTS ) ) );
                    }
                }
            }
            finally
            {
                // Make sure we free the buffer whatever happens
                NetApiBufferFree( buf );
            }

            return returnList;
        }

        private void ToggleLogin()
        {
            if ( _btnLogin.Text == "Login" )
                _btnLogin.Text = "Logoff";
            else
                _btnLogin.Text = "Login";

            _txtUserName.Text = string.Empty;
            _txtPassword.Text = string.Empty;
        }

        #endregion
    }
}