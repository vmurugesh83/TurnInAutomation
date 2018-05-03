Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class DCTransferReportFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As DCTransferReportInfo
        Dim DCTransferReportInfo As New DCTransferReportInfo()
        Try

            With DCTransferReportInfo
                .EMMID = CStr(ReadColumn(reader, "EMM_ID"))
                .EMMDesc = CStr(ReadColumn(reader, "EMM_NAME"))
                .BuyerID = CStr(ReadColumn(reader, "BUYER_ID"))
                .BuyerDesc = CStr(ReadColumn(reader, "BUYER_NAME"))
                .InStoreDate = CStr(ReadColumn(reader, "IN_STORE_DATE"))
                .DepartmentID = CStr(ReadColumn(reader, "DEPARTMENT_ID"))
                .DepartmentDesc = CStr(ReadColumn(reader, "DEPARTMENT"))
                .VendorStyle = CStr(ReadColumn(reader, "VENDOR_STYLE_NUMBER"))
                .ISNDesc = CStr(ReadColumn(reader, "ISN_DESCRIPTION"))
                .Color = CStr(ReadColumn(reader, "COLOR"))
                If .Color.Length > 0 And .Color.Contains("-") Then
                    .ColorCode = CDec(CDec(ReadColumn(reader, "COLOR").ToString.Split(CChar("-"))(0)))
                Else
                    .ColorCode = 0
                End If

                .OO = CInt(ReadColumn(reader, "ON_ORDER"))
                .OH = CInt(ReadColumn(reader, "ON_HAND"))
                .OwnedRetailAmount = CDec(ReadColumn(reader, "RETAIL_OWNED_COST"))
                .TransferFromDC = CInt(ReadColumn(reader, "UPC_TRFRFRM_LOC_ID"))
                .TransferToDC = CInt(ReadColumn(reader, "UPC_TRFR_TO_LOC_ID"))
                .UPCDisplay = CStr(ReadColumn(reader, "UPC"))
                'Get the UPC string form the UPC Display
                Dim str As String = CStr(ReadColumn(reader, "UPC"))
                .UPC = CDec(str.Substring(str.ToString.LastIndexOf(CChar("-")) + 1).Trim)
                .ISN = CDec(ReadColumn(reader, "ISN"))
                If CStr(ReadColumn(reader, "UPC_SELECT")) = "0" Then
                    .SelectedUPC = ""
                    .UPCDesc = ""
                Else
                    .SelectedUPC = CStr(ReadColumn(reader, "UPC_SELECT"))
                    .UPCDesc = CStr(ReadColumn(reader, "UPC_DESC"))
                End If

                .IsTransferred = CStr(ReadColumn(reader, "UPC_TRANSFER_FLG"))
                .Comments = CStr(ReadColumn(reader, "UPC_TRFR_CMNT_TXT"))
                .MerchStatus = CStr(ReadColumn(reader, "CUR_STATUS"))
                .TransferQty = CInt(ReadColumn(reader, "UPC_TRANSFER_QTY"))
                .SKU = CStr(ReadColumn(reader, "SKU_NUM"))
                .Vendor = CStr(ReadColumn(reader, "VENDOR"))
            End With
        Catch ex As Exception
            Throw New Exception(CStr(ReadColumn(reader, "ISN")) & " has an issue. Color - " & CStr(ReadColumn(reader, "COLOR")))
        End Try
        Return DCTransferReportInfo
    End Function


End Class
