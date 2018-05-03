Imports Microsoft.Reporting.WebForms
Imports System.Security.Principal

Partial Public Class ReportServerCredentials
    Implements IReportServerCredentials

    Private _userName As String
    Private _password As String
    Private _domain As String

    Public Sub New(ByVal UserNme As String, ByVal Pwd As String, ByVal Domain As String)
        _userName = UserNme
        _password = Pwd
        _domain = Domain
    End Sub

    Public Function GetFormsCredentials(ByRef authCookie As System.Net.Cookie, ByRef userName As String, ByRef password As String, ByRef authority As String) As Boolean Implements Microsoft.Reporting.WebForms.IReportServerCredentials.GetFormsCredentials
        authCookie = Nothing
        userName = _userName
        password = _password
        authority = _domain

        Return False
    End Function

    Public ReadOnly Property ImpersonationUser As System.Security.Principal.WindowsIdentity Implements Microsoft.Reporting.WebForms.IReportServerCredentials.ImpersonationUser
        Get
            Return WindowsIdentity.GetCurrent()
        End Get
    End Property

    Public ReadOnly Property NetworkCredentials As System.Net.ICredentials Implements Microsoft.Reporting.WebForms.IReportServerCredentials.NetworkCredentials
        Get
            Return New System.Net.NetworkCredential(_userName, _password, _domain)
        End Get
    End Property
End Class
