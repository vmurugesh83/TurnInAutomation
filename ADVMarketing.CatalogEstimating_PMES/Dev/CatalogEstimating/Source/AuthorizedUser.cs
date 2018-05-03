#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security;
using System.Threading;

#endregion

namespace CatalogEstimating
{
    public class AuthorizedUser : IDisposable
    {
        #region PInvoke Declarations

        [DllImport( "kernel32.dll", SetLastError = true )]
        static extern bool CloseHandle( IntPtr handle );

        [DllImport( "advapi32.dll", SetLastError = true )]
        public extern static bool DuplicateToken(
            IntPtr ExistingTokenHandle,
            SecurityImpersonationLevel SECURITY_IMPERSONATION_LEVEL,
            ref IntPtr DuplicateTokenHandle );

        [DllImport( "advapi32.dll", SetLastError = true )]
        public extern static bool CheckTokenMembership(
            IntPtr TokenHandle,
            IntPtr SidToCheck,
            out bool IsMember );

        [DllImport( "advapi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
        private extern static bool LookupAccountName( 
            string lpSystemName,
            string lpAccountName,
            IntPtr Sid, ref int cbSid,
            IntPtr ReferencedDomainName, ref int cbReferencedDomainName,
            out int peUse );

        public enum SecurityImpersonationLevel : int
        {
            /// <summary>
            /// The server process cannot obtain identification information about the client, 
            /// and it cannot impersonate the client. It is defined with no value given, and thus, 
            /// by ANSI C rules, defaults to a value of zero. 
            /// </summary>
            SecurityAnonymous = 0,

            /// <summary>
            /// The server process can obtain information about the client, such as security identifiers and privileges, 
            /// but it cannot impersonate the client. This is useful for servers that export their own objects, 
            /// for example, database products that export tables and views. 
            /// Using the retrieved client-security information, the server can make access-validation decisions without 
            /// being able to use other services that are using the client's security context. 
            /// </summary>
            SecurityIdentification = 1,

            /// <summary>
            /// The server process can impersonate the client's security context on its local system. 
            /// The server cannot impersonate the client on remote systems. 
            /// </summary>
            SecurityImpersonation = 2,

            /// <summary>
            /// The server process can impersonate the client's security context on remote systems. 
            /// NOTE: Windows NT:  This impersonation level is not supported.
            /// </summary>
            SecurityDelegation = 3,
        }

        private const int ERROR_INSUFFICIENT_BUFFER = 122;  //from winerror.h

        #endregion

        #region Private Data

        private WindowsImpersonationContext _impersonationContext;
        private WindowsIdentity _id;
        private IntPtr _userToken = IntPtr.Zero;
        private IntPtr _duplicateToken = IntPtr.Zero;

        #endregion

        #region Construction and Destruction

        /// <summary>Default constructor uses the current principle to determine access rights.</summary>
        public AuthorizedUser()
        {
            WindowsIdentity current = (WindowsIdentity)Thread.CurrentPrincipal.Identity;
            IntPtr duplicate = IntPtr.Zero;

            bool retVal = DuplicateToken( current.Token, SecurityImpersonationLevel.SecurityImpersonation,
                ref duplicate );

            if ( retVal )
                CheckRights(duplicate);
            else
                Marshal.ThrowExceptionForHR( Marshal.GetHRForLastWin32Error() );

            CloseHandle( duplicate );
        }

        /// <summary>Constructor which uses a Win32 HANDLE created from LoginUser to impersonate an 
        /// external user and determine their access rights.</summary>
        /// <param name="token">HANDLE to a user to impersonate.</param>
        public AuthorizedUser( IntPtr token )
        {
            _userToken = token;

            bool retVal = DuplicateToken( _userToken, SecurityImpersonationLevel.SecurityImpersonation,
                ref _duplicateToken );

            if ( retVal )
            {
                if ( CheckRights( _duplicateToken ) )
                {
                    _id = new WindowsIdentity( token, "Kerberos",
                        WindowsAccountType.Normal, true );
                    
                    // Begin impersonation
                    _impersonationContext = _id.Impersonate();
                    System.Threading.Thread.CurrentPrincipal = new WindowsPrincipal(_id);
                }
            }
            else
                Marshal.ThrowExceptionForHR( Marshal.GetHRForLastWin32Error() );
        }

        ~AuthorizedUser()
        {
            Dispose( false );
        }

        private void Dispose( bool disposing )
        {
            // Stop impersonation and revert to the process identity
            if ( _impersonationContext != null )
            {
                _impersonationContext.Undo();
                _impersonationContext.Dispose();
                _impersonationContext = null;
            }

            if ( _id != null )
            {
                _id.Dispose();
                _id = null;
            }

            // Free the token
            if ( _userToken != IntPtr.Zero )
            {
                CloseHandle( _userToken );
                _userToken = IntPtr.Zero;
            }

            // Free the token
            if ( _duplicateToken != IntPtr.Zero )
            {
                CloseHandle( _duplicateToken );
                _userToken = IntPtr.Zero;
            }

            if ( disposing )
                GC.SuppressFinalize( this );
        }

        #endregion

        #region Public Properties

        public string FormattedName
        {
            get
            {
                string userName = WindowsIdentity.GetCurrent().Name;
                int nSlash = userName.IndexOf( "\\" );
                if ( nSlash == -1 )
                    return userName;
                else
                    return userName.Substring( nSlash + 1 );
            }
        }

        private UserRights _rights = UserRights.Exclude;
        public UserRights Right
        {
            get { return _rights; }
        }

        #endregion

        #region Private Methods
        
        private bool IsInRole( IntPtr token, string role )
        {
            Sid sid = null;
            bool ret = false;
            try
            {
                sid = Sid.GetSid( role );
                if ( sid != null && sid.Value != IntPtr.Zero )
                {
                    ret = IsSidInToken( token, sid );
                }
            }
            catch {/*Don't allow exceptions to bubble back up*/}
            finally
            {
                if ( sid != null )
                {
                    sid.Dispose();
                }
            }

            return ret;
        }

        private bool IsSidInToken( IntPtr token, Sid sid )
        {
            IntPtr impersonationToken = IntPtr.Zero;
            bool inToken = false;
            CheckTokenMembership( token, sid.Value, out inToken );
            return inToken;
        }

        private bool CheckRights( IntPtr token )
        {
            if (IsInRole(token, "PMES_SuperAdmin"))
            {
                _rights = UserRights.SuperAdmin;
                return true;
            }
            else if (IsInRole(token, "PMES_RateAdmin"))
            {
                _rights = UserRights.Admin;
                return true;
            }
            else if (IsInRole(token, "PMES_Create"))
            {
                _rights = UserRights.Create;
                return true;
            }
            else if (IsInRole(token, "PMES_ReadOnly"))
            {
                _rights = UserRights.ReadOnly;
                return true;
            }
#if DEBUG
            else if (IsInRole(token, "Bon-Ton"))
            {
                _rights = UserRights.SuperAdmin;
                return true;
            }
#endif
            else
            {
                _rights = UserRights.Exclude;
                return false;
            }               
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose( true );
        }

        #endregion

        #region Sid Class

        private class Sid : IDisposable
        {
            #region Private Variables

            IntPtr _sidvalue;
            int _length;

            #endregion

            #region Construction and Destruction

            public Sid( IntPtr sid, int length )
            {
                _sidvalue = sid;
                _length = length;
            }

            ~Sid()
            {
                Dispose();
            }

            #endregion

            #region Public Properties

            public int Length 
            { 
                get { return _length; } 
            }

            public IntPtr Value 
            { 
                get { return _sidvalue; }
            }

            #endregion

            #region Public Methods

            public Sid Copy()
            {
                Sid newSid = new Sid( IntPtr.Zero, 0 );
                byte[] buffer = new byte[this.Length];
                newSid._sidvalue = Marshal.AllocHGlobal( this.Length );
                newSid._length = this.Length;
                Marshal.Copy( this.Value, buffer, 0, this.Length );
                Marshal.Copy( buffer, 0, newSid._sidvalue, this.Length );

                return newSid;
            }

            public void Dispose()
            {
                if ( _sidvalue != IntPtr.Zero )
                {
                    Marshal.FreeHGlobal( _sidvalue );
                    _sidvalue = IntPtr.Zero;
                    GC.SuppressFinalize( this );
                }
            }

            #endregion

            #region Static Methods

            public static Sid GetSid( string name )
            {
                bool ret = false;
                Sid sid = null;
                IntPtr psid = IntPtr.Zero;
                IntPtr domain = IntPtr.Zero;
                int sidLength = 0;
                int domainLength = 0;
                int sidType = 0;

                try
                {
                    ret = LookupAccountName( null,
                                              name,
                                              psid,
                                              ref sidLength,
                                              domain,
                                              ref domainLength,
                                              out sidType );


                    if ( ret == false &&
                        Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER )
                    {
                        psid = Marshal.AllocHGlobal( sidLength );

                        //LookupAccountName only works on Unicode systems so to ensure
                        //we allocate a LPWSTR
                        domain = Marshal.AllocHGlobal( domainLength * 2 );

                        ret = LookupAccountName( null,
                                                  name,
                                                  psid,
                                                  ref sidLength,
                                                  domain,
                                                  ref domainLength,
                                                  out sidType );



                    }

                    if ( ret == true )
                    {
                        sid = new Sid( psid, sidLength );
                    }

                }
                finally
                {
                    if ( domain != IntPtr.Zero )
                    {
                        Marshal.FreeHGlobal( domain );
                    }
                }

                return sid;
            }

            #endregion
        }

        #endregion
    }
}