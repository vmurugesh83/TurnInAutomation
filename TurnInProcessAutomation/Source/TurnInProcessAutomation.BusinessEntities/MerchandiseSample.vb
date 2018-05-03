Imports System.ComponentModel
Imports System.IO
Imports System.Xml.Serialization
Imports System.Text

Public Class MerchandiseSample


    Private _id As String
    ''' <summary>
    ''' MerchandiseID for a sample. 
    ''' Unique identifier for tracing a sample anywhere in the system.
    ''' Generate from WorkHorse / NERVE
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(True)> _
    Public Property MerchID() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _quantity As Integer
    <DataObjectField(False)> _
    Public Property Quantity() As Integer
        Get
            Return _quantity
        End Get
        Set(ByVal value As Integer)
            _quantity = value
        End Set
    End Property

    Private _isn As Decimal
    <DataObjectField(False)> _
    Public Property ISN() As Decimal
        Get
            Return _isn
        End Get
        Set(ByVal value As Decimal)
            _isn = value
        End Set
    End Property

    Private _isndesc As String = String.Empty
    <DataObjectField(False)> _
    Public Property ISNDescription() As String
        Get
            Return _isndesc
        End Get
        Set(ByVal value As String)
            _isndesc = value
        End Set
    End Property

    Private _turninMerchandiseID As Decimal
    <DataObjectField(False)> _
    Public Property TurninMerchandiseID() As Decimal
        Get
            Return _turninMerchandiseID
        End Get
        Set(ByVal value As Decimal)
            _turninMerchandiseID = value
        End Set
    End Property


    Private _colorcode As Integer
    ''' <summary>
    ''' Code for color
    ''' </summary>
    <DataObjectField(False)> _
    Public Property ColorCode() As Integer
        Get
            Return _colorcode
        End Get
        Set(ByVal value As Integer)
            _colorcode = value
        End Set
    End Property

    Private _colordesc As String = String.Empty
    <DataObjectField(False)> _
    Public Property ColorDesc() As String
        Get
            Return _colordesc
        End Get
        Set(ByVal value As String)
            _colordesc = value
        End Set
    End Property

    Private _sizecode As Integer
    ''' <summary>
    ''' Code for size
    ''' </summary>
    <DataObjectField(False)> _
    Public Property SizeCode() As Integer
        Get
            Return _sizecode
        End Get
        Set(ByVal value As Integer)
            _sizecode = value
        End Set
    End Property

    Private _sizedesc As String = String.Empty
    <DataObjectField(False)> _
    Public Property SizeDesc() As String
        Get
            Return _sizedesc
        End Get
        Set(ByVal value As String)
            _sizedesc = value
        End Set
    End Property

    Private _sellingLocation As String = String.Empty
    <DataObjectField(False)> _
    Public Property SellingLocation() As String
        Get
            Return _sellingLocation
        End Get
        Set(ByVal value As String)
            _sellingLocation = value
        End Set
    End Property

    Private _sequenceNumber As Integer
    <DataObjectField(False)> _
    Public Property SequenceNumber() As Integer
        Get
            Return _sequenceNumber
        End Get
        Set(ByVal value As Integer)
            _sequenceNumber = value
        End Set
    End Property

    Private _shelfNumber As Integer
    <DataObjectField(False)> _
    Public Property ShelfNumber() As Integer
        Get
            Return _shelfNumber
        End Get
        Set(ByVal value As Integer)
            _shelfNumber = value
        End Set
    End Property

    Private _status As String = String.Empty
    ''' <summary>
    ''' Status coming from WorkHorse
    ''' This is different from SampleApprovalStatus
    ''' Valid Values : R - Requested, C - Received, I - InTransit, S - Studio
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property SampleStatus() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _source As String = String.Empty
    ''' <summary>
    ''' VendorSample as default, WH can put in Store, fulfillment location etc.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property SampleSource() As String
        Get
            Return _source
        End Get
        Set(ByVal value As String)
            _source = value
        End Set
    End Property

    Private _User As String = ""
    <DataObjectField(False)> _
    Public Property User() As String
        Get
            Return _User
        End Get
        Set(ByVal value As String)
            _User = value
        End Set
    End Property

    Private _DateTimeStamp As DateTime = DateTime.MinValue
    <DataObjectField(False)> _
    Public Property LastUsedDate() As DateTime
        Get
            Return _DateTimeStamp
        End Get
        Set(ByVal value As DateTime)
            _DateTimeStamp = value
        End Set
    End Property

    Private _CheckInDate As DateTime = DateTime.MinValue
    <DataObjectField(False)> _
    Public Property CheckInDate() As DateTime
        Get
            Return _CheckInDate
        End Get
        Set(ByVal value As DateTime)
            _CheckInDate = value
        End Set
    End Property

    Dim _DeptId As Integer
    <DataObjectField(False)> _
    Public Property DeptID() As Integer
        Get
            Return _DeptId
        End Get
        Set(ByVal value As Integer)
            _DeptId = value
        End Set
    End Property

    Dim _DeptDesc As String = String.Empty
    <DataObjectField(False)> _
    Public Property DeptDesc() As String
        Get
            Return _DeptDesc
        End Get
        Set(ByVal value As String)
            _DeptDesc = value
        End Set
    End Property

    Private _SnapshotImage As SnapshotImage
    <DataObjectField(False)> _
    Public Property SnapshotImage As SnapshotImage
        Get
            Return _SnapshotImage
        End Get
        Set(ByVal value As SnapshotImage)
            _SnapshotImage = value
        End Set
    End Property
    Public Sub New()

    End Sub

    Private _VendorInfo As VendorInfo
    <DataObjectField(False)> _
    Public Property Vendor As VendorInfo
        Get
            Return _VendorInfo
        End Get
        Set(value As VendorInfo)
            _VendorInfo = value
        End Set
    End Property

    Private _VendorStyleNumber As String = String.Empty
    <DataObjectField(False)> _
    Public Property VendorStyleNumber As String
        Get
            Return _VendorStyleNumber
        End Get
        Set(value As String)
            _VendorStyleNumber = value
        End Set
    End Property

    Private _PlaceholderAdNumber As Integer
    <DataObjectField(False)> _
    Public Property PlaceholderAdNumber As Integer
        Get
            Return _PlaceholderAdNumber
        End Get
        Set(value As Integer)
            _PlaceholderAdNumber = value
        End Set
    End Property

    Private _PlaceholderPageNumber As Integer
    <DataObjectField(False)> _
    Public Property PlaceholderPageNumber As Integer
        Get
            Return _PlaceholderPageNumber
        End Get
        Set(value As Integer)
            _PlaceholderPageNumber = value
        End Set
    End Property

    Dim _BuyerId As Integer
    <DataObjectField(False)> _
    Public Property BuyerId() As Integer
        Get
            Return _BuyerId
        End Get
        Set(ByVal value As Integer)
            _BuyerId = value
        End Set
    End Property

    Dim _BuyerGroupEmail As String = String.Empty
    <DataObjectField(False)> _
    Public Property BuyerGroupEmail() As String
        Get
            Return _BuyerGroupEmail
        End Get
        Set(ByVal value As String)
            _BuyerGroupEmail = value
        End Set
    End Property

    Dim _BuyerDesc As String = String.Empty
    <DataObjectField(False)> _
    Public Property BuyerDesc() As String
        Get
            Return _BuyerDesc
        End Get
        Set(ByVal value As String)
            _BuyerDesc = value
        End Set
    End Property

    Dim _classId As Integer
    <DataObjectField(False)> _
    Public Property ClassId() As Integer
        Get
            Return _classId
        End Get
        Set(ByVal value As Integer)
            _classId = value
        End Set
    End Property

    Dim _classDesc As String = String.Empty
    <DataObjectField(False)> _
    Public Property ClassDesc() As String
        Get
            Return _classDesc
        End Get
        Set(ByVal value As String)
            _classDesc = value
        End Set
    End Property

    'SampleRequested Datetime
    Private _SampleRequestedDate As Date = DateTime.MinValue
    <DataObjectField(False)> _
    Public Property SampleRequestedDate() As Date
        Get
            Return _SampleRequestedDate
        End Get
        Set(ByVal value As Date)
            _SampleRequestedDate = value
        End Set
    End Property

    'The type of sample requested from the vendor.  
    'It will be used by  the Central merch room to validate the received sample matches the requested sample.
    'Sample Only, Image Only, Image and Sample
    Private _SampleRequestType As String = String.Empty
    <DataObjectField(False)> _
    Public Property SampleRequestType() As String
        Get
            Return _SampleRequestType
        End Get
        Set(ByVal value As String)
            _SampleRequestType = value
        End Set
    End Property

    ' The date the merchants request to have the sample on hand at the Sample Primary Location. 
    'It is generated in the new Style/SKU Sample Request Screen. 
    'It will be used in the Centralized Merch Room to determine missing/late samples.
    Private _SampleDueInhouseDate As Date = DateTime.MinValue
    <DataObjectField(False)> _
    Public Property SampleDueInhouseDate() As Date
        Get
            Return _SampleDueInhouseDate
        End Get
        Set(ByVal value As Date)
            _SampleDueInhouseDate = value
        End Set
    End Property

    'sample requestor
    Private _Requestor As String = String.Empty
    <DataObjectField(False)> _
    Public Property Requestor() As String
        Get
            Return _Requestor
        End Get
        Set(ByVal value As String)
            _Requestor = value
        End Set
    End Property
    'sample care note
    Private _CareNotes As String = String.Empty
    <DataObjectField(False)> _
    Public Property SampleCareNotes() As String
        Get
            Return _CareNotes
        End Get
        Set(ByVal value As String)
            _CareNotes = value
        End Set
    End Property
    'return to vendor date
    Private _ReturnDate As Date = DateTime.MinValue
    <DataObjectField(False)> _
    Public Property ReturnDate() As Date
        Get
            Return _ReturnDate
        End Get
        Set(ByVal value As Date)
            _ReturnDate = value
        End Set
    End Property

    Private _ReturnAddress1 As String = ""
    <DataObjectField(False)> _
    Public Property ReturnAddress1() As String
        Get
            Return _ReturnAddress1
        End Get
        Set(ByVal value As String)
            _ReturnAddress1 = value
        End Set
    End Property

    Private _ReturnAddress2 As String = ""
    <DataObjectField(False)> _
    Public Property ReturnAddress2() As String
        Get
            Return _ReturnAddress2
        End Get
        Set(ByVal value As String)
            _ReturnAddress2 = value
        End Set
    End Property

    Private _ReturnCity As String = ""
    <DataObjectField(False)> _
    Public Property ReturnCity() As String
        Get
            Return _ReturnCity
        End Get
        Set(ByVal value As String)
            _ReturnCity = value
        End Set
    End Property

    Private _ReturnState As String = ""
    <DataObjectField(False)> _
    Public Property ReturnState() As String
        Get
            Return _ReturnState
        End Get
        Set(ByVal value As String)
            _ReturnState = value
        End Set
    End Property

    Private _ReturnZip As String = ""
    <DataObjectField(False)> _
    Public Property ReturnZip() As String
        Get
            Return _ReturnZip
        End Get
        Set(ByVal value As String)
            _ReturnZip = value
        End Set
    End Property

    Private _ReturnPhone As String = String.Empty
    <DataObjectField(False)> _
    Public Property ReturnPhone() As String
        Get
            Return _ReturnPhone
        End Get
        Set(ByVal value As String)
            _ReturnPhone = value
        End Set
    End Property

    'expedite return to vendor 
    Private _ExpediteReturn As Boolean
    <DataObjectField(False)> _
    Public Property ExpediteReturn() As Boolean
        Get
            Return _ExpediteReturn
        End Get
        Set(ByVal value As Boolean)
            _ExpediteReturn = value
        End Set
    End Property

    Private _ExtensionDate As Date = DateTime.MinValue
    <DataObjectField(False)> _
    Public Property ExtensionDate() As Date
        Get
            Return _ExtensionDate
        End Get
        Set(ByVal value As Date)
            _ExtensionDate = value
        End Set
    End Property

    ' Send to Location
    Private _SamplePrimaryLocation As String = String.Empty
    <DataObjectField(False)> _
    Public Property SamplePrimaryLocation() As String
        Get
            Return _SamplePrimaryLocation
        End Get
        Set(ByVal value As String)
            _SamplePrimaryLocation = value
        End Set
    End Property

    ' Location within primary location
    Private _SampleSecondaryLocation As String = String.Empty
    <DataObjectField(False)> _
    Public Property SampleSecondaryLocation() As String
        Get
            Return _SampleSecondaryLocation
        End Get
        Set(ByVal value As String)
            _SampleSecondaryLocation = value
        End Set
    End Property
    ' Disposition
    Private _Disposition As String = String.Empty
    <DataObjectField(False)> _
    Public Property Disposition() As String
        Get
            Return _Disposition
        End Get
        Set(ByVal value As String)
            _Disposition = value
        End Set
    End Property

    'Y, N, NULL
    Private _ApprovalFlag As Boolean = False
    <DataObjectField(False)> _
    Public Property ApprovalFlag() As Boolean
        Get
            Return _ApprovalFlag
        End Get
        Set(ByVal value As Boolean)
            _ApprovalFlag = value
        End Set
    End Property

    Private _BoxNumber As Integer
    ''' <summary>
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property BoxNumber() As Integer
        Get
            Return _BoxNumber
        End Get
        Set(ByVal value As Integer)
            _BoxNumber = value
        End Set
    End Property

    Private _ApprovalStatus As String = String.Empty
    ''' <summary>
    '''  This will map to the SAMPLE_APVL_FLG
    ''' This is different from SampleStatus
    ''' Valid Values : P-Pending, A-Approved (TBD)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property ApprovalStatus() As String
        Get
            Return _ApprovalStatus
        End Get
        Set(ByVal value As String)
            _ApprovalStatus = value
        End Set
    End Property

    Dim _CMGId As Integer
    <DataObjectField(False)> _
    Public Property CMGID() As Integer
        Get
            Return _CMGId
        End Get
        Set(ByVal value As Integer)
            _CMGId = value
        End Set
    End Property

    Dim _CMGDesc As String = String.Empty
    <DataObjectField(False)> _
    Public Property CMGDesc() As String
        Get
            Return _CMGDesc
        End Get
        Set(ByVal value As String)
            _CMGDesc = value
        End Set
    End Property

    Dim _CMRNotes As String = String.Empty
    <DataObjectField(False)> _
    Public Property CMRNotes() As String
        Get
            Return _CMRNotes
        End Get
        Set(ByVal value As String)
            _CMRNotes = value
        End Set
    End Property

    Dim _CRGID As Integer
    <DataObjectField(False)> _
    Public Property CRGID() As Integer
        Get
            Return _CRGID
        End Get
        Set(ByVal value As Integer)
            _CRGID = value
        End Set
    End Property

    Dim _CRGDesc As String = String.Empty
    <DataObjectField(False)> _
    Public Property CRGDesc() As String
        Get
            Return _CRGDesc
        End Get
        Set(ByVal value As String)
            _CRGDesc = value
        End Set
    End Property

    Private _upc As Decimal
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property UPC() As Decimal
        Get
            Return _upc
        End Get
        Set(ByVal value As Decimal)
            _upc = value
        End Set
    End Property

    Dim _BrandId As Short = 0
    <DataObjectField(False)> _
    Public Property BrandId() As Short
        Get
            Return _BrandId
        End Get
        Set(ByVal value As Short)
            _BrandId = value
        End Set
    End Property

    Dim _BrandDesc As String = String.Empty
    <DataObjectField(False)> _
    Public Property BrandDesc() As String
        Get
            Return _BrandDesc
        End Get
        Set(ByVal value As String)
            _BrandDesc = value
        End Set
    End Property

    Dim _labelID As Integer
    <DataObjectField(False)> _
    Public Property LabelID() As Integer
        Get
            Return _labelID
        End Get
        Set(ByVal value As Integer)
            _labelID = value
        End Set
    End Property

    Private _label As String = String.Empty
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property LabelDesc() As String
        Get
            Return _label
        End Get
        Set(ByVal value As String)
            _label = value
        End Set
    End Property

    Private _isISNChanged As Boolean
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property ISNChanged() As Boolean
        Get
            Return _isISNChanged
        End Get
        Set(ByVal value As Boolean)
            _isISNChanged = value
        End Set
    End Property

    Private _webEligibleFlag As Boolean
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataObjectField(False)> _
    Public Property IsWebEligible() As Boolean
        Get
            Return _webEligibleFlag
        End Get
        Set(ByVal value As Boolean)
            _webEligibleFlag = value
        End Set
    End Property

    Private _VendorId As Integer
    <DataObjectField(True)> _
    Public Property VendorId() As Integer
        Get
            Return _VendorId
        End Get
        Set(ByVal value As Integer)
            _VendorId = value
        End Set
    End Property

    Private _VendorName As String
    <DataObjectField(True)> _
    Public Property VendorName() As String
        Get
            Return _VendorName
        End Get
        Set(ByVal value As String)
            _VendorName = value
        End Set
    End Property

    Private _VendorEmail As String
    <DataObjectField(True)> _
    Public Property VendorEmail() As String
        Get
            Return _VendorEmail
        End Get
        Set(ByVal value As String)
            _VendorEmail = value
        End Set
    End Property

    Private _VendorInstructions As String
    <DataObjectField(True)> _
    Public Property VendorInstructions() As String
        Get
            Return _VendorInstructions
        End Get
        Set(ByVal value As String)
            _VendorInstructions = value
        End Set
    End Property

    Private _VendorPhone As String
    <DataObjectField(True)> _
    Public Property VendorPhone() As String
        Get
            Return _VendorPhone
        End Get
        Set(ByVal value As String)
            _VendorPhone = value
        End Set
    End Property

    Private _VendorPID As String
    <DataObjectField(True)> _
    Public Property VendorPID() As String
        Get
            Return _VendorPID
        End Get
        Set(ByVal value As String)
            _VendorPID = value
        End Set
    End Property

    Public Shared Function Serialize(ByVal obj As Object) As String

        Dim stream As New MemoryStream
        Dim ds As New XmlSerializer(GetType(MerchandiseSample), "http://schema.bonton.com/Schema/SampleSchema")
        ds.Serialize(stream, obj)
        Dim xmlString As String = Encoding.UTF8.GetString(stream.ToArray())
        stream.Close()
        Return xmlString

    End Function
End Class
