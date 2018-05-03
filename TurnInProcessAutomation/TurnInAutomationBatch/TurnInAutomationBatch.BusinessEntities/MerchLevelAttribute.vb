Imports System.ComponentModel

<Serializable>
Public Class MerchLevelAttribute
    <DataObjectField(True)> _
    Public Property MerchLevelNumber() As Integer

    <DataObjectField(True)> _
    Public Property ImageURL() As String

    <DataObjectField(True)> _
    Public Property FabricationDetailID() As String

    <DataObjectField(True)> _
    Public Property FabricationDetailDesc() As String

    <DataObjectField(True)>
    Public Property WebCatVendorStyleNumber() As String

    <DataObjectField(True)>
    Public Property WebCatFabricationDetailDesc() As String

    <DataObjectField(True)>
    Public Property FeatureImageID() As Integer

End Class
