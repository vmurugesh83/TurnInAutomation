Imports System.ComponentModel

<Serializable()> _
Public Class DCTransferReportInfo

    Public Sub New()
    End Sub

    Private _EMMID As String = ""
    <DataObjectField(True)> _
    Public Property EMMID() As String
        Get
            Return _EMMID
        End Get
        Set(ByVal value As String)
            _EMMID = value
        End Set
    End Property

    Private _EMMDesc As String = ""
    <DataObjectField(True)> _
    Public Property EMMDesc() As String
        Get
            Return _EMMDesc
        End Get
        Set(ByVal value As String)
            _EMMDesc = value
        End Set
    End Property

    Private _SellArea As String = ""
    <DataObjectField(True)> _
    Public Property SellArea() As String
        Get
            Return _SellArea
        End Get
        Set(ByVal value As String)
            _SellArea = value
        End Set
    End Property

    Private _DepartmentID As String = ""
    <DataObjectField(True)> _
    Public Property DepartmentID() As String
        Get
            Return _DepartmentID
        End Get
        Set(ByVal value As String)
            _DepartmentID = value
        End Set
    End Property

    Private _DepartmentDesc As String = ""
    <DataObjectField(True)> _
    Public Property DepartmentDesc() As String
        Get
            Return _DepartmentDesc
        End Get
        Set(ByVal value As String)
            _DepartmentDesc = value
        End Set
    End Property

    Private _BuyerID As String = ""
    <DataObjectField(True)> _
    Public Property BuyerID() As String
        Get
            Return _BuyerID
        End Get
        Set(ByVal value As String)
            _BuyerID = value
        End Set
    End Property

    Private _BuyerDesc As String = ""
    <DataObjectField(True)> _
    Public Property BuyerDesc() As String
        Get
            Return _BuyerDesc
        End Get
        Set(ByVal value As String)
            _BuyerDesc = value
        End Set
    End Property

    Private _CMGID As String = ""
    <DataObjectField(True)> _
    Public Property CMGID() As String
        Get
            Return _CMGID
        End Get
        Set(ByVal value As String)
            _CMGID = value
        End Set
    End Property

    Private _CMGDesc As String = ""
    <DataObjectField(True)> _
    Public Property CMGDesc() As String
        Get
            Return _CMGDesc
        End Get
        Set(ByVal value As String)
            _CMGDesc = value
        End Set
    End Property

    Private _ProductID As String = ""
    <DataObjectField(True)> _
    Public Property ProductID() As String
        Get
            Return _ProductID
        End Get
        Set(ByVal value As String)
            _ProductID = value
        End Set
    End Property

    Private _ProductDesc As String = ""
    <DataObjectField(True)> _
    Public Property ProductDesc() As String
        Get
            Return _ProductDesc
        End Get
        Set(ByVal value As String)
            _ProductDesc = value
        End Set
    End Property

    Private _ProductStatus As String = ""
    <DataObjectField(True)> _
    Public Property ProductStatus() As String
        Get
            Return _ProductStatus
        End Get
        Set(ByVal value As String)
            _ProductStatus = value
        End Set
    End Property

    Private _UPC As Decimal
    <DataObjectField(True)> _
    Public Property UPC() As Decimal
        Get
            Return _UPC
        End Get
        Set(ByVal value As Decimal)
            _UPC = value
        End Set
    End Property

    ''' <summary>
    ''' Display Size with UPC number in dropdown.
    ''' </summary>
    ''' <remarks></remarks>
    Private _UPCDisplay As String = ""
    <DataObjectField(True)> _
    Public Property UPCDisplay() As String
        Get
            Return _UPCDisplay
        End Get
        Set(ByVal value As String)
            _UPCDisplay = value
        End Set
    End Property

    ''' <summary>
    ''' SelectedUPC
    ''' </summary>
    ''' <remarks></remarks>
    Private _SelectedUPC As String = ""
    <DataObjectField(True)> _
    Public Property SelectedUPC() As String
        Get
            Return _SelectedUPC
        End Get
        Set(ByVal value As String)
            _SelectedUPC = value
        End Set
    End Property



    Private _UPCDesc As String = ""
    <DataObjectField(True)> _
    Public Property UPCDesc() As String
        Get
            Return _UPCDesc
        End Get
        Set(ByVal value As String)
            _UPCDesc = value
        End Set
    End Property

    Private _UPCStatus As String = ""
    <DataObjectField(True)> _
    Public Property UPCStatus() As String
        Get
            Return _UPCStatus
        End Get
        Set(ByVal value As String)
            _UPCStatus = value
        End Set
    End Property

    Private _MerchType As String = ""
    <DataObjectField(True)> _
    Public Property MerchType() As String
        Get
            Return _MerchType
        End Get
        Set(ByVal value As String)
            _MerchType = value
        End Set
    End Property

    Private _MerchStatus As String = ""
    <DataObjectField(True)> _
    Public Property MerchStatus() As String
        Get
            Return _MerchStatus
        End Get
        Set(ByVal value As String)
            _MerchStatus = value
        End Set
    End Property

    Private _Inventory As String = ""
    <DataObjectField(True)> _
    Public Property Inventory() As String
        Get
            Return _Inventory
        End Get
        Set(ByVal value As String)
            _Inventory = value
        End Set
    End Property

    Private _SalesLast4Wks As Integer = 0
    <DataObjectField(True)> _
    Public Property SalesLast4Wks() As Integer
        Get
            Return _SalesLast4Wks
        End Get
        Set(ByVal value As Integer)
            _SalesLast4Wks = value
        End Set
    End Property

    Private _OO As Integer = 0
    <DataObjectField(True)> _
    Public Property OO() As Integer
        Get
            Return _OO
        End Get
        Set(ByVal value As Integer)
            _OO = value
        End Set
    End Property

    Private _TotalOwnedAmount As Decimal = 0
    <DataObjectField(True)> _
    Public Property TotalOwnedAmount() As Decimal
        Get
            Return _TotalOwnedAmount
        End Get
        Set(ByVal value As Decimal)
            _TotalOwnedAmount = value
        End Set
    End Property

    Private _OriginalTktPrc As Decimal = 0
    <DataObjectField(True)> _
    Public Property OriginalTicketPrice() As Decimal
        Get
            Return _OriginalTktPrc
        End Get
        Set(ByVal value As Decimal)
            _OriginalTktPrc = value
        End Set
    End Property

    Private _OwnedRetailAmount As Decimal = 0
    <DataObjectField(True)> _
    Public Property OwnedRetailAmount() As Decimal
        Get
            Return _OwnedRetailAmount
        End Get
        Set(ByVal value As Decimal)
            _OwnedRetailAmount = value
        End Set
    End Property

    Private _PurchaseOrderID As String = ""
    <DataObjectField(True)> _
    Public Property PONumber() As String
        Get
            Return _PurchaseOrderID
        End Get
        Set(ByVal value As String)
            _PurchaseOrderID = value
        End Set
    End Property

    Private _POShipDate As String = ""
    <DataObjectField(True)> _
    Public Property POShipDate() As String
        Get
            Return _POShipDate
        End Get
        Set(ByVal value As String)
            _POShipDate = value
        End Set
    End Property

    Private _ReplenishFlag As String = ""
    <DataObjectField(True)> _
    Public Property ReplenishFlag() As String
        Get
            Return _ReplenishFlag
        End Get
        Set(ByVal value As String)
            _ReplenishFlag = value
        End Set
    End Property

    Private _SKU As String = ""
    <DataObjectField(True)> _
    Public Property SKU() As String
        Get
            Return _SKU
        End Get
        Set(ByVal value As String)
            _SKU = value
        End Set
    End Property

    Private _VendorStyle As String = ""
    <DataObjectField(True)> _
    Public Property VendorStyle() As String
        Get
            Return _VendorStyle
        End Get
        Set(ByVal value As String)
            _VendorStyle = value
        End Set
    End Property

    Private _LocDesc As String = ""
    <DataObjectField(True)> _
    Public Property LocationDesc() As String
        Get
            Return _LocDesc
        End Get
        Set(ByVal value As String)
            _LocDesc = value
        End Set
    End Property

    Private _InStoreDate As String = ""
    <DataObjectField(True)> _
    Public Property InStoreDate() As String
        Get
            Return _InStoreDate
        End Get
        Set(ByVal value As String)
            _InStoreDate = value
        End Set
    End Property

    Private _TransferFromDC As Integer = 0
    <DataObjectField(True)> _
    Public Property TransferFromDC() As Integer
        Get
            Return _TransferFromDC
        End Get
        Set(ByVal value As Integer)
            _TransferFromDC = value
        End Set
    End Property

    Private _TransferToDC As Integer = 0
    <DataObjectField(True)> _
    Public Property TransferToDC() As Integer
        Get
            Return _TransferToDC
        End Get
        Set(ByVal value As Integer)
            _TransferToDC = value
        End Set
    End Property

    Private _TransferQty As Integer = 1
    <DataObjectField(True)> _
    Public Property TransferQty() As Integer
        Get
            Return _TransferQty
        End Get
        Set(ByVal value As Integer)
            _TransferQty = value
        End Set
    End Property

    Private _IsTransferred As String = ""
    <DataObjectField(True)> _
    Public Property IsTransferred() As String
        Get
            Return _IsTransferred
        End Get
        Set(ByVal value As String)
            _IsTransferred = value
        End Set
    End Property

    Private _Comments As String = ""
    <DataObjectField(True)> _
    Public Property Comments() As String
        Get
            Return _Comments
        End Get
        Set(ByVal value As String)
            _Comments = value
        End Set
    End Property

    Private _ColorCode As Decimal = 0
    <DataObjectField(True)> _
    Public Property ColorCode() As Decimal
        Get
            Return _ColorCode
        End Get
        Set(ByVal value As Decimal)
            _ColorCode = value
        End Set
    End Property

    Private _Color As String = ""
    <DataObjectField(True)> _
    Public Property Color() As String
        Get
            Return _Color
        End Get
        Set(ByVal value As String)
            _Color = value
        End Set
    End Property

    Private _OH As Integer = 0
    <DataObjectField(True)> _
    Public Property OH() As Integer
        Get
            Return _OH
        End Get
        Set(ByVal value As Integer)
            _OH = value
        End Set
    End Property

    Private _ISN As Decimal = 0
    <DataObjectField(True)> _
    Public Property ISN() As Decimal
        Get
            Return _ISN
        End Get
        Set(ByVal value As Decimal)
            _ISN = value
        End Set
    End Property

    Private _ISNDesc As String = ""
    <DataObjectField(True)> _
    Public Property ISNDesc() As String
        Get
            Return _ISNDesc
        End Get
        Set(ByVal value As String)
            _ISNDesc = value
        End Set
    End Property

    Private _User As String = ""
    <DataObjectField(True)> _
    Public Property User() As String
        Get
            Return _User
        End Get
        Set(ByVal value As String)
            _User = value
        End Set
    End Property

    Private _Vendor As String = ""
    <DataObjectField(True)> _
    Public Property Vendor() As String
        Get
            Return _Vendor
        End Get
        Set(ByVal value As String)
            _Vendor = value
        End Set
    End Property

    Private _ExtraColumn As String = ""
    <DataObjectField(True)> _
    Public Property ExtraColumn() As String
        Get
            Return _ExtraColumn
        End Get
        Set(ByVal value As String)
            _ExtraColumn = value
        End Set
    End Property

End Class
