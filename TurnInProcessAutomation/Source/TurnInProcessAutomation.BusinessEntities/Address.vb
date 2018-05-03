Imports System.ComponentModel

Public Class Address
    Private _Address1 As String = ""
    <DataObjectField(True)> _
    Public Property Address1() As String
        Get
            Return _Address1
        End Get
        Set(ByVal value As String)
            _Address1 = value
        End Set
    End Property

    Private _Address2 As String = ""
    <DataObjectField(True)> _
    Public Property Address2() As String
        Get
            Return _Address2
        End Get
        Set(ByVal value As String)
            _Address2 = value
        End Set
    End Property

    Private _City As String = ""
    <DataObjectField(True)> _
    Public Property City() As String
        Get
            Return _City
        End Get
        Set(ByVal value As String)
            _City = value
        End Set
    End Property

    Private _State As String = ""
    <DataObjectField(True)> _
    Public Property State() As String
        Get
            Return _State
        End Get
        Set(ByVal value As String)
            _State = value
        End Set
    End Property

    Private _Zip As String = ""
    <DataObjectField(True)> _
    Public Property Zip() As String
        Get
            Return _Zip
        End Get
        Set(ByVal value As String)
            _Zip = value
        End Set
    End Property
End Class
