
Imports System.ComponentModel

<Serializable()> _
Partial Public Class VendorStyleInfo

    'Internal member variables
    Private _VendorStyleNumber As String
    Private _IsReserve As Boolean


    'Default constructor
    Public Sub New()
    End Sub

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
    Public Property IsReserve() As Boolean
        Get
            Return _IsReserve
        End Get
        Set(ByVal value As Boolean)
            _IsReserve = value
        End Set
    End Property

End Class

