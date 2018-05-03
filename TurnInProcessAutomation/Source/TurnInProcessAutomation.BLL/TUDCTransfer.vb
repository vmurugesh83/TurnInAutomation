Imports TurnInProcessAutomation.BLL.CommonBO
Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.SqlDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient
Imports System.Text
Imports System.Configuration
Imports BonTon.Common


Public Class TUDCTransfer
    Private dalDB2 As MainframeDAL.DCTransferReportDAO = New MainframeDAL.DCTransferReportDAO

    Public Sub Save(ByVal transferRecordData As DCTransferReportInfo)
        dalDB2.InsertDCTransferRecord(transferRecordData.ISN, transferRecordData.ColorCode, transferRecordData.SelectedUPC, transferRecordData.TransferFromDC, _
                                          transferRecordData.TransferToDC, transferRecordData.IsTransferred, transferRecordData.Comments, transferRecordData.User, transferRecordData.TransferQty)
    End Sub

    Dim BatchId As Integer = 0
    Public Function CreateBatchInTurnIn(ByVal transferRecordData As List(Of DCTransferReportInfo), ByVal AdNumber As Decimal, ByVal PageNumber As Integer) As Integer
        Dim dalTurnInDB2 As MainframeDAL.EcommSetupCreateDao = New MainframeDAL.EcommSetupCreateDao
        For Each transferRecord As DCTransferReportInfo In transferRecordData
            Dim ISNDescription As String = (transferRecord.ColorCode & " " & transferRecord.ISNDesc)
            If ISNDescription.Length > 51 Then
                ISNDescription = ISNDescription.Substring(0, 51)
            End If
            BatchId = dalTurnInDB2.InsertISNDataColorLevel(transferRecord.ISN, _
                                                    transferRecord.DepartmentID, _
                                                    transferRecord.ColorCode, _
                                                    2, _
                                                    ISNDescription, _
                                                    AdNumber, _
                                                    PageNumber, _
                                                    "N", _
                                                    "N", _
                                                    "", _
                                                    "N", _
                                                    transferRecord.User, _
                                                    BatchId)
        Next
        Return BatchId
    End Function

    Public Function GetDCTransferReportData(ByVal DepartmentId As String, ByVal BuyerId As String, ByVal VendorId As String, ByVal PriceStausCodes As String) As IList(Of DCTransferReportInfo)
        Dim DB2Results As IList(Of DCTransferReportInfo) = dalDB2.GetData(DepartmentId, BuyerId, VendorId, PriceStausCodes)
        Return DB2Results
    End Function

    Public Sub UpdateDCTransferRecordAfterSubmit(ByVal ISN As Decimal, ByVal ColorCode As Decimal, ByVal User As String)
        dalDB2.UpdateDCTransferRecordAfterSubmit(ISN, ColorCode, User)
    End Sub

    Public Sub SendSampleRequest(ByVal TransferUPCs As String)

        Dim SampleRequestList As New List(Of MerchandiseSample)
        SampleRequestList = dalDB2.ReadSampleRequestData(TransferUPCs)
        If SampleRequestList.Count > 0 Then
            For Each merch As MerchandiseSample In SampleRequestList
                Try
                    Dim message As String = String.Empty
                    Dim mqChannel As New MQSeries.MQChannel
                    mqChannel.Server = ConfigurationManager.AppSettings("Server")
                    mqChannel.Port = ConfigurationManager.AppSettings("Port")
                    mqChannel.Channel = ConfigurationManager.AppSettings("Channel")

                    mqChannel.Connect()

                    Dim mqQueue As New MQSeries.MQQueue
                    mqQueue.Name = ConfigurationManager.AppSettings("QueueName")
                    mqQueue.Channel = mqChannel

                    message = MerchandiseSample.Serialize(merch)
                    mqQueue.Put(message)

                    mqChannel.Disconnect()
                Catch ex As IBM.WMQ.MQException
                    Throw New Exception("SendSampleRequest Failed " & ex.Message & "( RC : " & ex.Reason.ToString & " Reason : " & ex.Reason.ToString & " Completion Code : " & ex.CompletionCode.ToString & " )")
                End Try
            Next
        End If
    End Sub
End Class
