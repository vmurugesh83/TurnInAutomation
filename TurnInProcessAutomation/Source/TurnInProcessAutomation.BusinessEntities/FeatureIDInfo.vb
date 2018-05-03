Imports System.ComponentModel
<Serializable()> _
Public Class FeatureIDInfo
    Private _ISN As Decimal
    Private _ImageIdNum As Integer
    Private _upcNum As Decimal

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
    Public Property ImageIdNum() As Integer
        Get
            Return _ImageIdNum
        End Get
        Set(ByVal value As Integer)
            _ImageIdNum = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property upcNum() As Decimal
        Get
            Return _upcNum
        End Get
        Set(ByVal value As Decimal)
            _upcNum = value
        End Set
    End Property

End Class
