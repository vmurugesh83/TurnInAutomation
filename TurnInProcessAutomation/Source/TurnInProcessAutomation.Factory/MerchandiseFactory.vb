Imports TurnInProcessAutomation.BusinessEntities
Imports IBM.Data.DB2
Imports System.Data.Common

Public Class MerchandiseFactory

    Friend Shared Function ReadColumn(ByVal reader As DbDataReader, ByVal ColumnName As String) As Object
        Try
            If HasColumn(reader, ColumnName) Then
                Return reader(ColumnName)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function HasColumn(ByVal Reader As DbDataReader, ByVal ColumnName As String) As Boolean
        Dim i As Integer
        For i = 0 To Reader.FieldCount - 1
            If Reader.GetName(i).ToString().ToUpper() = ColumnName.ToUpper() Then Return True
        Next
        Return False
    End Function

    Public Shared Function ConvertToString(ByVal O As Object) As String
        Dim returnVal As String = ""
        Try
            If Not IsDBNull(O) Then returnVal = CStr(O)
        Catch ex As Exception

        End Try
        Return returnVal
    End Function

    Public Shared Function ConstructList(ByVal reader As DB2DataReader) As List(Of MerchandiseSample)
        Dim merchList As New List(Of MerchandiseSample)

        While reader.Read
            merchList.Add(Construct(reader))
        End While

        Return merchList
    End Function

    Public Shared Function Construct(ByVal reader As DB2DataReader) As MerchandiseSample
        Dim merch As New MerchandiseSample

        With merch
            If (ReadColumn(reader, "ApprovalFlag") IsNot DBNull.Value) Then
                .ApprovalFlag = CBool(IIf(CStr(ReadColumn(reader, "ApprovalFlag")) = "Y", 1, 0))
            End If

            If (ReadColumn(reader, "ApprovalStatus") IsNot DBNull.Value) Then
                .ApprovalStatus = CStr(ReadColumn(reader, "ApprovalStatus"))
            End If
            If (ReadColumn(reader, "BoxNumber") IsNot DBNull.Value) Then
                .BoxNumber = CInt(ReadColumn(reader, "BoxNumber"))
            End If

            If (ReadColumn(reader, "BrandDesc") IsNot DBNull.Value) Then
                .BrandDesc = CStr(ReadColumn(reader, "BrandDesc"))
            End If
            If (ReadColumn(reader, "BrandId") IsNot DBNull.Value) Then
                .BrandId = CShort(CInt(ReadColumn(reader, "BrandId")))
            End If
            If (ReadColumn(reader, "BuyerDesc") IsNot DBNull.Value) Then
                .BuyerDesc = CStr(ReadColumn(reader, "BuyerDesc"))
            End If
            If (ReadColumn(reader, "BuyerGroupEmail") IsNot DBNull.Value) Then
                .BuyerGroupEmail = CStr(ReadColumn(reader, "BuyerGroupEmail"))
            End If
            If (ReadColumn(reader, "BuyerId") IsNot DBNull.Value) Then
                .BuyerId = CInt(ReadColumn(reader, "BuyerId"))
            End If
            If (ReadColumn(reader, "CheckinDate") IsNot DBNull.Value) Then
                .CheckinDate = CDate(ReadColumn(reader, "CheckinDate"))
            End If

            If (ReadColumn(reader, "ClassDesc") IsNot DBNull.Value) Then
                .ClassDesc = CStr(ReadColumn(reader, "ClassDesc"))
            End If
            If (ReadColumn(reader, "ClassId") IsNot DBNull.Value) Then
                .ClassId = CInt(ReadColumn(reader, "ClassId"))
            End If
            If (ReadColumn(reader, "CMGDesc") IsNot DBNull.Value) Then
                .CMGDesc = CStr(ReadColumn(reader, "CMGDesc"))
            End If
            If (ReadColumn(reader, "CMGId") IsNot DBNull.Value) Then
                .CMGID = CInt(ReadColumn(reader, "CMGId"))
            End If
            If (ReadColumn(reader, "CMRNotes") IsNot DBNull.Value) Then
                .CMRNotes = CStr(ReadColumn(reader, "CMRNotes"))
            End If
            If (ReadColumn(reader, "ColorCode") IsNot DBNull.Value) Then
                .ColorCode = CInt(ReadColumn(reader, "ColorCode"))
            End If
            If (ReadColumn(reader, "ColorDesc") IsNot DBNull.Value) Then
                .ColorDesc = CStr(ReadColumn(reader, "ColorDesc"))
            End If
            If (ReadColumn(reader, "CRGDesc") IsNot DBNull.Value) Then
                .CRGDesc = CStr(ReadColumn(reader, "CRGDesc"))
            End If
            If (ReadColumn(reader, "CRGId") IsNot DBNull.Value) Then
                .CRGID = CInt(ReadColumn(reader, "CRGId"))
            End If
            If (ReadColumn(reader, "DeptDesc") IsNot DBNull.Value) Then
                .DeptDesc = CStr(ReadColumn(reader, "DeptDesc"))
            End If
            If (ReadColumn(reader, "DeptId") IsNot DBNull.Value) Then
                .DeptID = CInt(ReadColumn(reader, "DeptId"))
            End If
            If (ReadColumn(reader, "Disposition") IsNot DBNull.Value) Then
                .Disposition = CStr(ReadColumn(reader, "Disposition"))
            End If
            If (ReadColumn(reader, "ExpediteReturn") IsNot DBNull.Value) Then
                .ExpediteReturn = CBool(IIf(CBool(ReadColumn(reader, "ExpediteReturn")), True, False))
            End If
            If (ReadColumn(reader, "ExtensionDate") IsNot DBNull.Value) Then
                .ExtensionDate = CDate(ReadColumn(reader, "ExtensionDate"))
            End If
            If (ReadColumn(reader, "ISN") IsNot DBNull.Value) Then
                .ISN = CDec(ReadColumn(reader, "ISN"))
            End If
            If (ReadColumn(reader, "ISNChanged") IsNot DBNull.Value) Then
                .ISNChanged = CBool(IIf(CStr(ReadColumn(reader, "ISNChanged")) = "Y", True, False))
            End If
            If (ReadColumn(reader, "ISNDescription") IsNot DBNull.Value) Then
                .ISNDescription = CStr(ReadColumn(reader, "ISNDescription"))
            End If
            If (ReadColumn(reader, "IsWebEligible") IsNot DBNull.Value) Then
                .IsWebEligible = CBool(ReadColumn(reader, "IsWebEligible"))
            End If
            If (ReadColumn(reader, "Label") IsNot DBNull.Value) Then
                .LabelDesc = CStr(ReadColumn(reader, "Label"))
            End If
            If (ReadColumn(reader, "LastUsedDate") IsNot DBNull.Value) Then
                .LastUsedDate = CDate(ReadColumn(reader, "LastUsedDate"))
            End If
            If (ReadColumn(reader, "MerchID") IsNot DBNull.Value) Then
                .MerchID = CStr(CInt(ReadColumn(reader, "MerchID")))
            End If
            If (ReadColumn(reader, "PlaceholderAdNumber") IsNot DBNull.Value) Then
                .PlaceholderAdNumber = CInt(ReadColumn(reader, "PlaceholderAdNumber"))
            End If
            If (ReadColumn(reader, "PlaceholderPageNumber") IsNot DBNull.Value) Then
                .PlaceholderPageNumber = CInt(ReadColumn(reader, "PlaceholderPageNumber"))
            End If
            If (ReadColumn(reader, "Quantity") IsNot DBNull.Value) Then
                .Quantity = CInt(ReadColumn(reader, "Quantity"))
            End If
            If (ReadColumn(reader, "Requestor") IsNot DBNull.Value) Then
                .Requestor = CStr(ReadColumn(reader, "Requestor"))
            End If
            If (ReadColumn(reader, "ReturnAddress1") IsNot DBNull.Value) Then
                .ReturnAddress1 = CStr(ReadColumn(reader, "ReturnAddress1"))
            End If
            If (ReadColumn(reader, "ReturnAddress2") IsNot DBNull.Value) Then
                .ReturnAddress2 = CStr(ReadColumn(reader, "ReturnAddress2"))
            End If
            If (ReadColumn(reader, "ReturnCity") IsNot DBNull.Value) Then
                .ReturnCity = CStr(ReadColumn(reader, "ReturnCity"))
            End If
            If (ReadColumn(reader, "ReturnDate") IsNot DBNull.Value) Then
                .ReturnDate = CDate(ReadColumn(reader, "ReturnDate"))
            End If
            If (ReadColumn(reader, "ReturnPhone") IsNot DBNull.Value) Then
                .ReturnPhone = CStr(ReadColumn(reader, "ReturnPhone"))
            End If
            If (ReadColumn(reader, "ReturnState") IsNot DBNull.Value) Then
                .ReturnState = CStr(ReadColumn(reader, "ReturnState"))
            End If
            If (ReadColumn(reader, "ReturnZip") IsNot DBNull.Value) Then
                .ReturnZip = CStr(ReadColumn(reader, "ReturnZip"))
            End If
            If (ReadColumn(reader, "SampleCareNotes") IsNot DBNull.Value) Then
                .SampleCareNotes = CStr(ReadColumn(reader, "SampleCareNotes"))
            End If
            If (ReadColumn(reader, "SampleDueInhouseDate") IsNot DBNull.Value) Then
                '.SampleDueInhouseDate = CDate(ReadColumn(reader, "SampleDueInhouseDate"))
                .SampleDueInhouseDate = CDate(DateTime.Now.Date.ToString("yyyy-MM-ddTHH\:mm\:ss.ffffff"))
            End If
            If (ReadColumn(reader, "SamplePrimaryLocation") IsNot DBNull.Value) Then
                .SamplePrimaryLocation = CStr(ReadColumn(reader, "SamplePrimaryLocation"))
            End If
            If (ReadColumn(reader, "SampleRequestedDate") IsNot DBNull.Value) Then
                .SampleRequestedDate = CDate(CDate(ReadColumn(reader, "SampleRequestedDate")).ToString("yyyy-MM-ddTHH\:mm\:ss.ffffff"))
            End If
            If (ReadColumn(reader, "SampleRequestType") IsNot DBNull.Value) Then
                .SampleRequestType = CStr(ReadColumn(reader, "SampleRequestType"))
            End If
            If (ReadColumn(reader, "SampleSecondaryLocation") IsNot DBNull.Value) Then
                .SampleSecondaryLocation = CStr(ReadColumn(reader, "SampleSecondaryLocation"))
            End If
            If (ReadColumn(reader, "SampleSource") IsNot DBNull.Value) Then
                .SampleSource = CStr(ReadColumn(reader, "SampleSource"))
            End If
            If (ReadColumn(reader, "SampleStatus") IsNot DBNull.Value) Then
                .SampleStatus = CStr(ReadColumn(reader, "SampleStatus"))
            End If
            If (ReadColumn(reader, "SellingLocation") IsNot DBNull.Value) Then
                .SellingLocation = CStr(ReadColumn(reader, "SellingLocation"))
            End If
            If (ReadColumn(reader, "SequenceNumber") IsNot DBNull.Value) Then
                .SequenceNumber = CInt(ReadColumn(reader, "SequenceNumber"))
            End If
            If (ReadColumn(reader, "ShelfNumber") IsNot DBNull.Value) Then
                .ShelfNumber = CInt(ReadColumn(reader, "ShelfNumber"))
            End If
            If (ReadColumn(reader, "SizeCode") IsNot DBNull.Value) Then
                .SizeCode = CInt(ReadColumn(reader, "SizeCode"))
            End If
            If (ReadColumn(reader, "SizeDesc") IsNot DBNull.Value) Then
                .SizeDesc = CStr(ReadColumn(reader, "SizeDesc"))
            End If

            Dim SI As New SnapshotImage
            Dim pURL As New URL
            If (ReadColumn(reader, "PActualURL") IsNot DBNull.Value) Then
                pURL.ActualURL = CStr(ReadColumn(reader, "PActualURL"))
            End If
            If (ReadColumn(reader, "PMediumURL") IsNot DBNull.Value) Then
                pURL.MediumURL = CStr(ReadColumn(reader, "PMediumURL"))
            End If
            If (ReadColumn(reader, "PThumbnailURL") IsNot DBNull.Value) Then
                pURL.ThumbnailURL = CStr(ReadColumn(reader, "PThumbnailURL"))
            End If
            SI.PrimaryView = pURL

            Dim sURL As New URL
            If (ReadColumn(reader, "SActualURL") IsNot DBNull.Value) Then
                sURL.ActualURL = CStr(ReadColumn(reader, "SActualURL"))
            End If
            If (ReadColumn(reader, "SMediumURL") IsNot DBNull.Value) Then
                sURL.MediumURL = CStr(ReadColumn(reader, "SMediumURL"))
            End If
            If (ReadColumn(reader, "SThumbnailURL") IsNot DBNull.Value) Then
                sURL.ThumbnailURL = CStr(ReadColumn(reader, "SThumbnailURL"))
            End If
            SI.SecondaryView = sURL

            .SnapshotImage = SI

            If (ReadColumn(reader, "TurninMerchandiseID") IsNot DBNull.Value) Then
                .TurninMerchandiseID = CInt(ReadColumn(reader, "TurninMerchandiseID"))
            End If
            If (ReadColumn(reader, "UPC") IsNot DBNull.Value) Then
                .UPC = CDec(ReadColumn(reader, "UPC"))
            End If
            If (ReadColumn(reader, "User") IsNot DBNull.Value) Then
                .User = CStr(ReadColumn(reader, "User"))
            End If
            If (ReadColumn(reader, "VendorEmail") IsNot DBNull.Value) Then
                .VendorEmail = CStr(ReadColumn(reader, "VendorEmail"))
            End If
            If (ReadColumn(reader, "VendorId") IsNot DBNull.Value) Then
                .VendorId = CInt(ReadColumn(reader, "VendorId"))
            End If
            If (ReadColumn(reader, "VendorInstructions") IsNot DBNull.Value) Then
                .VendorInstructions = CStr(ReadColumn(reader, "VendorInstructions"))
            End If
            If (ReadColumn(reader, "VendorName") IsNot DBNull.Value) Then
                .VendorName = CStr(ReadColumn(reader, "VendorName"))
            End If
            If (ReadColumn(reader, "VendorPhone") IsNot DBNull.Value) Then
                .VendorPhone = CStr(ReadColumn(reader, "VendorPhone"))
            End If
            If (ReadColumn(reader, "VendorPID") IsNot DBNull.Value) Then
                .VendorPID = CStr(ReadColumn(reader, "VendorPID"))
            End If
            If (ReadColumn(reader, "VendorStyleNumber") IsNot DBNull.Value) Then
                .VendorStyleNumber = CStr(ReadColumn(reader, "VendorStyleNumber"))
            End If

        End With

        Return merch
    End Function



End Class
