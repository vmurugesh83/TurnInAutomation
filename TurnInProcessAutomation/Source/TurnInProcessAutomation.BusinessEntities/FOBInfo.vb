
Imports System.ComponentModel

<Serializable()> _
Public Class FOBInfo

    Private _DTE_MNT_TS As String
    Private _FOB_ID As Integer
    Private _CFG_ID As Integer
    Private _FOB_DESC As String
    Private _FOB_SHRT_DSC As String

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
    Public Property FOB_ID() As Integer
        Get
            Return _FOB_ID
        End Get
        Set(ByVal value As Integer)
            _FOB_ID = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property FOB_DESC() As String
        Get
            Return _FOB_DESC
        End Get
        Set(ByVal value As String)
            _FOB_DESC = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property FOB_SHRT_DSC() As String
        Get
            Return _FOB_SHRT_DSC
        End Get
        Set(ByVal value As String)
            _FOB_SHRT_DSC = value
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
