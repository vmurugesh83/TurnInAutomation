Imports System.ComponentModel

<Serializable()> _
Public Class GXSCFGInfo

    Private _INTERNAL_STYLE_NUM As Decimal
    Private _DEPT_ID As Integer
    'Private _FOB_ID As Integer
    Private _CRG_ID As Integer
    Private _CFG_ID As Integer
    Private _CMG_ID As Integer
    Private _ISN_LONG_DESC As String

    <DataObjectField(False)> _
    Public Property INTERNAL_STYLE_NUM() As Decimal
        Get
            Return _INTERNAL_STYLE_NUM
        End Get
        Set(ByVal value As Decimal)
            _INTERNAL_STYLE_NUM = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property DEPT_ID() As Integer
        Get
            Return _DEPT_ID
        End Get
        Set(ByVal value As Integer)
            _DEPT_ID = value
        End Set
    End Property
    '<DataObjectField(True)> _
    'Public Property FOB_ID() As Integer
    '    Get
    '        Return _FOB_ID
    '    End Get
    '    Set(ByVal value As Integer)
    '        _FOB_ID = value
    '    End Set
    'End Property
    <DataObjectField(True)> _
    Public Property CRG_ID() As Integer
        Get
            Return _CRG_ID
        End Get
        Set(ByVal value As Integer)
            _CRG_ID = value
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
    Public Property ISN_LONG_DESC() As String
        Get
            Return _ISN_LONG_DESC
        End Get
        Set(ByVal value As String)
            _ISN_LONG_DESC = value
        End Set
    End Property
End Class
