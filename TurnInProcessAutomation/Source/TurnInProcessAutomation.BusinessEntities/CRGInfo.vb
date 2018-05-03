
Imports System.ComponentModel

<Serializable()> _
Public Class CRGInfo

    Private _DTE_MNT_TS As String
    Private _CRG_ID As Integer
    Private _CRG_DSC As String
    Private _CRG_SHRT_DSC As String

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
    Public Property CRG_DSC() As String
        Get
            Return _CRG_DSC
        End Get
        Set(ByVal value As String)
            _CRG_DSC = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property CRG_SHRT_DSC() As String
        Get
            Return _CRG_SHRT_DSC
        End Get
        Set(ByVal value As String)
            _CRG_SHRT_DSC = value
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
