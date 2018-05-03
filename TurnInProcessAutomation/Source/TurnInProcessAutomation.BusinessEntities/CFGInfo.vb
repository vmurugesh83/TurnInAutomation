Imports System.ComponentModel

<Serializable()> _
Public Class CFGInfo

    Private _DTE_MNT_TS As String
    Private _CMG_ID As Integer
    Private _CFG_ID As Integer
    Private _CFG_DESC As String
    Private _CFG_SHRT_DSC As String

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
    Public Property CFG_DESC() As String
        Get
            Return _CFG_DESC
        End Get
        Set(ByVal value As String)
            _CFG_DESC = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property CFG_SHRT_DSC() As String
        Get
            Return _CFG_SHRT_DSC
        End Get
        Set(ByVal value As String)
            _CFG_SHRT_DSC = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property CFG_ID() As Integer
        Get
            Return _CFG_ID
        End Get
        Set(ByVal value As Integer)
            _CFG_ID = value
        End Set
    End Property
End Class
