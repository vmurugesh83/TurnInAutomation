Imports System.ComponentModel
Imports System.IO
Imports System.Xml.Serialization
Imports System.Text

Public Class GXSUPCInfo

    Private _UPC_NUM As Decimal
    <DataObjectField(False)> _
    Public Property UPC_NUM() As Decimal
        Get
            Return _UPC_NUM
        End Get
        Set(ByVal value As Decimal)
            _UPC_NUM = value
        End Set
    End Property

    Dim _CLR_LONG_DESC As String = String.Empty
    <DataObjectField(False)> _
    Public Property CLR_LONG_DESC() As String
        Get
            Return _CLR_LONG_DESC
        End Get
        Set(ByVal value As String)
            _CLR_LONG_DESC = value
        End Set
    End Property

    Private _SIZE As String
    <DataObjectField(False)> _
    Public Property SIZE() As String
        Get
            Return _SIZE
        End Get
        Set(ByVal value As String)
            _SIZE = value
        End Set
    End Property

    Public Shared Function Serialize(ByVal obj As Object) As String

        Dim stream As New MemoryStream
        Dim ds As New XmlSerializer(GetType(GXSUPCInfo), "http://schema.bonton.com/Schema/SampleSchema")
        ds.Serialize(stream, obj)
        Dim xmlString As String = Encoding.UTF8.GetString(stream.ToArray())
        stream.Close()
        Return xmlString

    End Function
End Class
