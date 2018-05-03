Imports System.ComponentModel

Public Class CMGInfo

    Private _DTE_MNT_TS As String
    Private _CMG_ID As Integer
    Private _CRG_ID As Integer
    Private _CMG_DESC As String
    Private _CMG_SHRT_DSC As String

    <DataObjectField(True)> _
    Public Property DTE_MNT_TS() As String
        Get
            Return _DTE_MNT_TS
        End Get
        Set(ByVal value As String)
            _DTE_MNT_TS = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property CMG_ID() As Integer
        Get
            Return _CMG_ID
        End Get
        Set(ByVal value As Integer)
            _CMG_ID = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property CMG_DESC() As String
        Get
            Return _CMG_DESC
        End Get
        Set(ByVal value As String)
            _CMG_DESC = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property CMG_SHRT_DSC() As String
        Get
            Return _CMG_SHRT_DSC
        End Get
        Set(ByVal value As String)
            _CMG_SHRT_DSC = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property CRG_ID() As Integer
        Get
            Return _CRG_ID
        End Get
        Set(ByVal value As Integer)
            _CRG_ID = value
        End Set
    End Property


End Class
