Imports System.ComponentModel

Public Class ImageSuffixInfo
    'Internal member variables
    Private _ImgSfxCd As String
    Private _ImgSfxDesc As String

    <DataObjectField(True)> _
    Public Property imgsfxcd() As String
        Get
            Return _ImgSfxCd
        End Get
        Set(ByVal value As String)
            _ImgSfxCd = value
        End Set
    End Property

    Public Property imgsfxdesc() As String
        Get
            Return _ImgSfxDesc
        End Get
        Set(ByVal value As String)
            _ImgSfxDesc = value
        End Set
    End Property

    Public ReadOnly Property imgsfxANDdesc() As String
        Get
            Return _ImgSfxCd & " - " & _ImgSfxDesc
        End Get
    End Property
End Class
