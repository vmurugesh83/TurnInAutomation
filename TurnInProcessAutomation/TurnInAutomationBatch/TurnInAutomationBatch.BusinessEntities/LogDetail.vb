Imports System.ComponentModel

<Serializable>
Public Class LogDetail
    <DataObjectField(True)>
    Public Property TurnInBatchID As Integer

    <DataObjectField(True)>
    Public Property TurnInItemTypeID As Integer

    <DataObjectField(True)>
    Public Property TurnInItemType As String

    <DataObjectField(True)>
    Public Property BatchStatusCode As String

    <DataObjectField(True)>
    Public Property BatchStatusMessage As String

    <DataObjectField(True)>
    Public Property LastModifiedBy As String
End Class
