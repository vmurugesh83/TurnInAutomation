
Imports System.ComponentModel

<Serializable()> _
Public Class NoSampleReportInfo

    Private _BuyerID As String = ""
    Private _BuyerDesc As String = ""
    Private _DeptId As Int16
    Private _Dept_Short_Desc As String
    Private _VendorId As Integer
    Private _VendorName As String
    Private _ISN As Decimal
    Private _ISNDesc As String
    Private _VendorStyleNumber As String
    Private _ColorCode As String
    Private _ColorDesc As String
    Private _TurnInMerchId As Integer
    Private _TurnInMerchStatus As String
    Private _SampleApprovalFlag As String
    Private _OnOrder As Integer
    Private _StartShipDate As Date

    <DataObjectField(True)> _
    Public Property BuyerID() As String
        Get
            Return _BuyerID
        End Get
        Set(ByVal value As String)
            _BuyerID = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BuyerDesc() As String
        Get
            Return _BuyerDesc
        End Get
        Set(ByVal value As String)
            _BuyerDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property DeptId() As Int16
        Get
            Return _DeptId
        End Get
        Set(ByVal value As Int16)
            _DeptId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property Dept_Short_Desc() As String
        Get
            Return _Dept_Short_Desc
        End Get
        Set(ByVal value As String)
            _Dept_Short_Desc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property VendorId() As Integer
        Get
            Return _VendorId
        End Get
        Set(ByVal value As Integer)
            _VendorId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property VendorName() As String
        Get
            Return _VendorName
        End Get
        Set(ByVal value As String)
            _VendorName = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ISN() As Decimal
        Get
            Return _ISN
        End Get
        Set(ByVal value As Decimal)
            _ISN = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ISNDesc() As String
        Get
            Return _ISNDesc
        End Get
        Set(ByVal value As String)
            _ISNDesc = value
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
    Public Property ColorCode() As String
        Get
            Return _ColorCode
        End Get
        Set(ByVal value As String)
            _ColorCode = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ColorDesc() As String
        Get
            Return _ColorDesc
        End Get
        Set(ByVal value As String)
            _ColorDesc = value
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
    Public Property TurnInMerchStatus() As String
        Get
            Return _TurnInMerchStatus
        End Get
        Set(ByVal value As String)
            _TurnInMerchStatus = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SampleApprovalFlag() As String
        Get
            Return _SampleApprovalFlag
        End Get
        Set(ByVal value As String)
            _SampleApprovalFlag = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property OnOrder() As Integer
        Get
            Return _OnOrder
        End Get
        Set(ByVal value As Integer)
            _OnOrder = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property StartShipDate() As String
        Get
            Return _StartShipDate
        End Get
        Set(ByVal value As String)
            _StartShipDate = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property IsVendorImage() As Boolean
    <DataObjectField(True)> _
    Public ReadOnly Property BuyerId_With_Desc() As String
        Get
            Return _BuyerID & " - " & _BuyerDesc
        End Get
    End Property

    <DataObjectField(True)> _
    Public ReadOnly Property DeptId_With_Desc() As String
        Get
            Return _DeptId & " - " & _Dept_Short_Desc
        End Get
    End Property

    <DataObjectField(True)> _
    Public ReadOnly Property VendorId_With_Desc() As String
        Get
            Return _VendorId & " - " & _VendorName
        End Get
    End Property

End Class
