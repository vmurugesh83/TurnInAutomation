Imports System.ComponentModel
Imports System.Xml.Serialization

Public Class WorkhorseRequest
    Private _header As New RequestHeader
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property RequestHeader() As RequestHeader
        Get
            Return _header
        End Get
        Set(ByVal value As RequestHeader)
            _header = value
        End Set
    End Property

    Private _detail As New RequestDetail
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property RequestDetail() As RequestDetail
        Get
            Return _detail
        End Get
        Set(ByVal value As RequestDetail)
            _detail = value
        End Set
    End Property
End Class

Public Class RequestHeader
    Private _security As New SecurityInfo
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property SecurityInfo() As SecurityInfo
        Get
            Return _security
        End Get
        Set(ByVal value As SecurityInfo)
            _security = value
        End Set
    End Property

    Private _Operation As String = "MerchCreate"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property Operation() As String
        Get
            Return _Operation
        End Get
        Set(ByVal value As String)
            _Operation = value
        End Set
    End Property

End Class

Public Class SecurityInfo
    Private _workhorseLogin As String = "msg"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property workhorseLogin() As String
        Get
            Return _workhorseLogin
        End Get
        Set(ByVal value As String)
            _workhorseLogin = value
        End Set
    End Property

    Private _encryptedPassword As String = "341A10747C4533634CA4F0588767AECDB39F2F5B5CD77C6640258CB8D47DF149"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property encryptedPassword() As String
        Get
            Return _encryptedPassword
        End Get
        Set(ByVal value As String)
            _encryptedPassword = value
        End Set
    End Property

    Private _mutualToken As String = "bonton"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property mutualToken() As String
        Get
            Return _mutualToken
        End Get
        Set(ByVal value As String)
            _mutualToken = value
        End Set
    End Property
End Class

Public Class RequestDetail
    Private _merchandise As New List(Of MerchandiseSample)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property MerchInfo() As List(Of MerchandiseSample)
        Get
            Return _merchandise
        End Get
        Set(ByVal value As List(Of MerchandiseSample))
            _merchandise = value
        End Set
    End Property
End Class