<ServiceContract()>
Public Interface IAd

    <OperationContract()>
    Function GetAdChangeInfo(ByVal Id As Integer) As List(Of Ad)

    ' TODO: Add your service operations here

End Interface

' Use a data contract as illustrated in the sample below to add composite types to service operations.
Public Enum ActiveIndicatorType
    A
    K
End Enum

<DataContract()>
Public Class Ad

    <DataMember()>
    Public Property AdNumber() As Integer
    <DataMember()>
    Public Property AdDescription() As String
    <DataMember()>
    Public Property AdEnd() As DateTime
    <DataMember()>
    Public Property AdStart() As DateTime
    <DataMember()>
    Public Property AdStatus() As String
    <DataMember()>
    Public Property AdVersion() As String
    <DataMember()>
    Public Property AssocFirst() As String
    <DataMember()>
    Public Property AssocId() As Integer
    <DataMember()>
    Public Property AssocLast() As String
    <DataMember()>
    Public Property AssocPhone() As String
    <DataMember()>
    Public Property EventEnd() As DateTime
    <DataMember()>
    Public Property EventStart() As DateTime
    <DataMember()>
    Public Property EventName() As String
    <DataMember()>
    Public Property MediaDescription() As String
    <DataMember()>
    Public Property MediaType() As String
    <DataMember()>
    Public Property PhotoStart() As DateTime
    <DataMember()>
    Public Property PhotoEnd() As DateTime
    <DataMember()>
    Public Property TurninDate() As DateTime
    <DataMember(Name:="Page")>
    Public Property Page() As Page
End Class
Public Class Page
    <DataMember()>
    Public Property CoverPage() As String
    <DataMember()>
    Public Property PageNumber() As Integer
    <DataMember()>
    Public Property PageDescription() As String
    <DataMember()>
    Public Property AdNumber() As String
    <DataMember()>
    Public Property ActiveIndicator() As ActiveIndicatorType = ActiveIndicatorType.A
    <DataMember(Name:="ShotGroup")>
    Public Property ShotGroup() As List(Of ShotGroup)
End Class
Public Class ShotGroup
    <DataMember()>
    Public Property ShotNumber() As Integer
    <DataMember(Name:="Image")>
    Public Property Images() As List(Of Image)
End Class
Public Class Image
    <DataMember()>
    Public Property ImageNumber() As Integer
    <DataMember()>
    Public Property Description() As String
    <DataMember()>
    Public Property ImageClass() As String
    <DataMember()>
    Public Property ImageNotes() As String
    <DataMember()>
    Public Property ImageSource() As String
    <DataMember()>
    Public Property ImageSuffixType() As String
    <DataMember()>
    Public Property MediaType() As String
    <DataMember()>
    Public Property ActiveIndicator() As ActiveIndicatorType = ActiveIndicatorType.A
    <DataMember(Name:="MerchForImage")>
    Public Property Samples() As List(Of MerchForImage)
End Class
Public Class MerchForImage
    <DataMember()>
    Public Property MerchID() As String
    <DataMember()>
    Public Property ActiveIndicator() As ActiveIndicatorType = ActiveIndicatorType.A
    <DataMember()>
    Public Property StylingNotes() As String
    '<DataMember()>
    'Public Property MerchGroup() As Integer
End Class

