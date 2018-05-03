
Imports System.ComponentModel

<Serializable()> _
Public Class ImageGroupingReportInfo

    Private _CmgId As Int16
    Private _ImageGroup As Integer
    Private _ImageID As Integer
    Private _VendorStyleNumber As String
    Private _TurnInMerchId As Integer
    Private _ProductDesc As String

    <DataObjectField(True)> _
    Public Property CmgId() As Int16
        Get
            Return _CmgId
        End Get
        Set(ByVal value As Int16)
            _CmgId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageGroup() As Integer
        Get
            Return _ImageGroup
        End Get
        Set(ByVal value As Integer)
            _ImageGroup = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageID() As Integer
        Get
            Return _ImageId
        End Get
        Set(ByVal value As Integer)
            _ImageId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property VendorStyleNumber() As String
        Get
            Return _VendorStyleNumber
        End Get
        Set(ByVal value As String)
            _VendorStyleNumber = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property TurnInMerchId() As Integer
        Get
            Return _TurnInMerchId
        End Get
        Set(ByVal value As Integer)
            _TurnInMerchId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ProductDesc() As String
        Get
            Return _ProductDesc
        End Get
        Set(ByVal value As String)
            _ProductDesc = value
        End Set
    End Property

End Class
