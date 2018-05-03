Imports System.ComponentModel

<Serializable()> _
Public Class AdminImageNotesInfo
    'Internal member variables
    Private _adNbrAdminImgNbr As String
    Private _imageNotes As String
    Private _imageSuffix As String
    Private _imageSuffixDesc As String

    'Default constructor
    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property AdNbrAdminImgNbr() As String
        Get
            Return _adNbrAdminImgNbr
        End Get
        Set(ByVal value As String)
            _adNbrAdminImgNbr = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageNotes() As String
        Get
            Return _imageNotes
        End Get
        Set(ByVal value As String)
            _imageNotes = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageSuffix() As String
        Get
            Return _imageSuffix
        End Get
        Set(ByVal value As String)
            _imageSuffix = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageSuffixDesc() As String
        Get
            Return _imageSuffixDesc
        End Get
        Set(ByVal value As String)
            _imageSuffixDesc = value
        End Set
    End Property
End Class
