''' <summary>
'''  POCO for Admin Data returned by the CMR_ stored procedures on the Admin Database.
''' </summary>
''' <remarks></remarks>
Public Class AdminFullData

    Public Property AdNumber As Integer
    Public Property AdDescription As String
    Public Property AdEnd As Date
    Public Property AdStart As Date
    Public Property AdStatus As Nullable(Of Char)
    Public Property AdVersion As Nullable(Of Decimal)
    Public Property AssocFirst As String
    Public Property AssocId As String
    Public Property AssocLast As String
    Public Property AssocPhone As String
    Public Property EventEnd As Date
    Public Property EventStart As Date
    Public Property EventName As String
    Public Property MediaDescription As String
    Public Property MediaType As String
    Public Property PhotoStart As Date
    Public Property PhotoEnd As Date
    Public Property TurnInDate As Date
    Public Property PageNumber As Integer
    Public Property PageDescription As String
    Public Property CoverPage As String
    Public Property PageActiveIndicator As Nullable(Of Char)
    Public Property ShotNumber As Nullable(Of Integer)
    Public Property ImageNumber As Integer
    Public Property ImageDescription As String
    Public Property ImageClass As String
    Public Property ImageNotes As String
    Public Property ImageSource As String
    Public Property ImageSuffixType As String
    Public Property ImageMediaType As String
    Public Property ImageActiveIndicator As Nullable(Of Char)
    Public Property MerchId As Nullable(Of Integer)
    Public Property MerchActiveIndicator As Nullable(Of Char)
    Public Property StylingNotes As String
    'Public Property MerchGroup As Integer
End Class
