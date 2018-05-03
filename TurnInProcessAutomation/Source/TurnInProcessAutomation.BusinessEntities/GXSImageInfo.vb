Imports System.ComponentModel
Imports System.IO
Imports System.Xml.Serialization
Imports System.Text

Public Class GXSImageInfo

    Private _ID As Decimal
    <DataObjectField(False)> _
    Public Property ID() As Decimal
        Get
            Return _ID
        End Get
        Set(ByVal value As Decimal)
            _ID = value
        End Set
    End Property

    Dim _LargeURL As String = String.Empty
    <DataObjectField(False)> _
    Public Property LargeURL() As String
        Get
            Return _LargeURL
        End Get
        Set(ByVal value As String)
            _LargeURL = value
        End Set
    End Property

    Private _SmallURL As String
    <DataObjectField(False)> _
    Public Property SmallURL() As String
        Get
            Return _SmallURL
        End Get
        Set(ByVal value As String)
            _SmallURL = value
        End Set
    End Property

    Public Shared Function Serialize(ByVal obj As Object) As String

        Dim stream As New MemoryStream
        Dim ds As New XmlSerializer(GetType(GXSImageInfo), "http://schema.bonton.com/Schema/SampleSchema")
        ds.Serialize(stream, obj)
        Dim xmlString As String = Encoding.UTF8.GetString(stream.ToArray())
        stream.Close()
        Return xmlString

    End Function
End Class
