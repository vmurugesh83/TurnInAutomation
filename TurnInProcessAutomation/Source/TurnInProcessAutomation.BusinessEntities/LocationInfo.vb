Imports System.ComponentModel

<Serializable()> _
Public Class LocationInfo

    Private _DTE_MNT_TS As String
    Private _LOC_ID As Integer
    Private _LOC_NME As String
    Private _TYP_CDE As String
    Private _TYP_SUB_CDE As String

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
    Public Property LOC_ID() As Integer
        Get
            Return _LOC_ID
        End Get
        Set(ByVal value As Integer)
            _LOC_ID = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property LOC_NME() As String
        Get
            Return _LOC_NME
        End Get
        Set(ByVal value As String)
            _LOC_NME = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property TYP_CDE() As String
        Get
            Return _TYP_CDE
        End Get
        Set(ByVal value As String)
            _TYP_CDE = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property TYP_SUB_CDE() As String
        Get
            Return _TYP_SUB_CDE
        End Get
        Set(ByVal value As String)
            _TYP_SUB_CDE = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property STOR_ADDR As String

    <DataObjectField(True)> _
    Public Property STOR_CITY_ADDR As String

    <DataObjectField(True)> _
    Public Property STOR_STAT_ADDR As String

    <DataObjectField(True)> _
    Public Property STOR_ZIP_ADDR As String

    <DataObjectField(True)> _
    Public Property COUNTRY As String

End Class
