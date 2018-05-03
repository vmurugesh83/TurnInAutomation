Imports IBM.WMQ
Imports System.IO
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Text
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports BonTon.DBUtility
Imports System.Xml
Imports TurnInProcessAutomation.BLL

Module Export

    Dim ExportData As New DataSet
    Private _MIOTransferPath As String = ConfigurationManager.AppSettings("MIOTransferPath")
    Private _recordThreshold As String = ConfigurationManager.AppSettings("recordThreshold")
    Private _logPath As String = ConfigurationManager.AppSettings("LogPath")
    Private _TransferType As String = "FC XFER"
    Private _DocumentType As String = "0010"
    Private _WaveType As String = "CMR"
    Private _ToStore As String = "955"

    Dim _appDB As New ApplicationDBLayer

    Dim _transferUPCs As String
    Private ReadOnly Property TransferUPCs As String
        Get
            'Transpose the comma separated values of UPC_NUM into XML records.
            Dim _transferUPCs As String = String.Empty

            _transferUPCs &= "<transferRecord>"
            For Each record As DataRow In ExportData.Tables(0).Rows
                _transferUPCs &= "<UPC num=""" & Trim(record(0).ToString) & """ />"
            Next
            _transferUPCs &= "</transferRecord>"
            Return _transferUPCs
        End Get
    End Property

    Sub Main()
        My.Application.Log.DefaultFileLogWriter.AutoFlush = True
        My.Application.Log.DefaultFileLogWriter.LogFileCreationSchedule = Logging.LogFileCreationScheduleOption.Weekly
        My.Application.Log.DefaultFileLogWriter.CustomLocation = _logPath
        My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - ***** BEGIN :: Generate Transfer File To MIO ***** ", TraceEventType.Information)
        My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - LogPath :: " & _logPath, TraceEventType.Information)
        My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - Message Queue Settings  ", TraceEventType.Information)
        My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - Server :: " & ConfigurationManager.AppSettings("Server"), TraceEventType.Information)
        My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - Port :: " & ConfigurationManager.AppSettings("Port"), TraceEventType.Information)
        My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - Channel :: " & ConfigurationManager.AppSettings("Channel"), TraceEventType.Information)
        My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - Queue :: " & ConfigurationManager.AppSettings("QueueName"), TraceEventType.Information)




        My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - Start Export to MIO ", TraceEventType.Start)
        Try

            Dim SRDataTable As New DataTable("RequestData")
            ExportData.Tables.Add(SRDataTable)

            Dim ReportDataTable As New DataTable("TransferData")
            ExportData.Tables.Add(ReportDataTable)

            _appDB.ReadINDCTransferData(ExportData)

            If ExportData.Tables(0).Rows.Count > 0 Then

                PutINDCTransferMessage()

                'GenerateExcel()

                _appDB.UpdateTransferRecord(TransferUPCs)

                My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - TTU760 Updated " & ExportData.Tables(0).Rows.Count.ToString & " items.", TraceEventType.Information)
            Else
                My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - 0 records found. Execute TTU1117 to find out why.", TraceEventType.Information)
            End If

        Catch ex As Exception
            My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - ERROR : " & ex.Message.ToString & vbCrLf & " STACK TRACE : " & ex.StackTrace.ToString, TraceEventType.Error)
        Finally
            My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - ***** END :: Generate Transfer File To MIO ***** " & vbCrLf & vbCrLf, TraceEventType.Information)
        End Try
    End Sub

    Private Sub GenerateExcel()
        If ExportData.Tables(0).Rows.Count > 0 Then
            ' Create a file to write to.
            Directory.SetCurrentDirectory(_MIOTransferPath)
            Dim [date] As String = String.Format("{0:dd_MM_yyyy}", DateTime.Now)
            Dim targetFilename As String = (Convert.ToString("TransferToPhotoStudio" & Convert.ToString("_")) & [date])

            Dim noOfRecordsPerFile As Integer = CInt(_recordThreshold)
            Dim numberOfFiles As Integer = CInt(ExportData.Tables(0).Rows.Count / noOfRecordsPerFile)


            Dim divisor = ExportData.Tables(0).Rows.Count / numberOfFiles ' needed to identify each group '
            Dim tables = ExportData.Tables(0).AsEnumerable().
                        Select(Function(r, j) New With {.Row = r, .Index = j}).
                        GroupBy(Function(x) Math.Floor(x.Index / divisor)).
                        Select(Function(g) g.Select(Function(x) x.Row).CopyToDataTable())

            Dim a As Integer = 1
            For Each table As DataTable In tables
                targetFilename = targetFilename
                CreateExcelFile.CreateExcelDocument(table, targetFilename & "_" & a.ToString & ".xls")
                My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - File Created : " & _MIOTransferPath & targetFilename & "_" & a.ToString & ".xls", TraceEventType.Information)

                a += 1
            Next
        End If

        My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - Exported " & ExportData.Tables(0).Rows.Count.ToString & " items.", TraceEventType.Information)

    End Sub
    Private Sub PutINDCTransferMessage()
        Dim transferXML As XmlDocument = Nothing
        Dim namespaceElement As XmlElement = Nothing
        Dim childElement As XmlElement = Nothing
        Dim fromLocationDetail As XmlElement = Nothing
        Dim childNode As XmlNode = Nothing
        Dim locationBAO As TULocation = Nothing
        Dim transferDetailElement As XmlElement = Nothing
        Dim transferDetailsElement As XmlElement = Nothing
        Dim transferDetailRecordElement As XmlElement = Nothing
        Dim messageQueue As MessageQueue = Nothing
        Dim sheetNameElement As XmlElement = Nothing

        Try
            transferXML = New XmlDocument()
            namespaceElement = transferXML.CreateElement("ns1", "XFER_REQUEST", "http://www.bonton.com/order/transferOrder")
            transferXML.AppendChild(namespaceElement)
            transferDetailRecordElement = transferXML.CreateElement(String.Empty, "XFER_DETAIL_RECORD", Nothing)
            namespaceElement.AppendChild(transferDetailRecordElement)
            childElement = transferXML.CreateElement(String.Empty, "DOCUMENT_TYPE", Nothing)
            childElement.InnerText = _DocumentType
            transferDetailRecordElement.AppendChild(childElement)
            childElement = transferXML.CreateElement(String.Empty, "TRANSFER_TYPE", Nothing)
            childElement.InnerText = _TransferType
            transferDetailRecordElement.AppendChild(childElement)
            childElement = transferXML.CreateElement(String.Empty, "WAVE_TYPE", Nothing)
            childElement.InnerText = _WaveType
            transferDetailRecordElement.AppendChild(childElement)
            childElement = transferXML.CreateElement(String.Empty, "XFER_ID", Nothing)
            childElement.InnerText = _appDB.GetTransferID()
            transferDetailRecordElement.AppendChild(childElement)
            childElement = transferXML.CreateElement(String.Empty, "XFER_DATE", Nothing)
            childElement.InnerText = Date.Today().ToString("yyyy-MM-dd")
            transferDetailRecordElement.AppendChild(childElement)
            childElement = transferXML.CreateElement(String.Empty, "MOD_ID", Nothing)
            childElement.InnerText = "Turnin-INDC Transfer"
            transferDetailRecordElement.AppendChild(childElement)
            childElement = Nothing
            sheetNameElement = transferXML.CreateElement(String.Empty, "SHEET_NAME", Nothing)
            transferDetailRecordElement.AppendChild(sheetNameElement)

            'from location detail
            CreateLocationDetailsElement(192, True, transferXML, transferDetailRecordElement)
            For Each row As DataRow In ExportData.Tables(0).Rows
                transferDetailsElement = transferXML.CreateElement(String.Empty, "XFER_DETAILS", Nothing)
                transferDetailRecordElement.AppendChild(transferDetailsElement)
                CreateLocationDetailsElement(CInt(row("Transfer location")), False, transferXML, transferDetailsElement)
                childElement = transferXML.CreateElement(String.Empty, "SKU_NUM", Nothing)
                childElement.InnerText = row("SKU").ToString()
                transferDetailsElement.AppendChild(childElement)
                childElement = transferXML.CreateElement(String.Empty, "UPC_NUM", Nothing)
                childElement.InnerText = row("UPC").ToString()
                transferDetailsElement.AppendChild(childElement)

                transferDetailElement = transferXML.CreateElement("XFER_DETAIL")
                childElement = transferXML.CreateElement(String.Empty, "UNIT_PRICE_AMT", Nothing)
                childElement.InnerText = row("OWN_PRICE_AMT")
                transferDetailElement.AppendChild(childElement)
                childElement = transferXML.CreateElement(String.Empty, "UNIT_COST_AMT", Nothing)
                childElement.InnerText = row("MRKT_COST_AMT")
                transferDetailElement.AppendChild(childElement)
                childElement = transferXML.CreateElement(String.Empty, "TRANSFER_QTY", Nothing)
                childElement.InnerText = row("Transfer Qty.")
                transferDetailElement.AppendChild(childElement)

                transferDetailsElement.AppendChild(transferDetailElement)

            Next
            messageQueue = New MessageQueue()
            messageQueue.PutTransferMessage(transferXML.InnerXml.ToString())
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CreateLocationDetailsElement(ByVal locationID As Integer, ByVal isFromLocation As Boolean,
                                                  ByRef transferXML As XmlDocument, ByRef transferDetailsNode As XmlElement)
        Dim locationDetailsElement As XmlElement = Nothing
        Dim locationBAO As TULocation = Nothing
        Dim locationInfo As LocationInfo = Nothing
        Dim childNode As XmlNode = Nothing
        Try
            locationBAO = New TULocation()
            locationInfo = locationBAO.GetLocationByLocationID(locationID)

            If isFromLocation Then
                locationDetailsElement = transferXML.CreateElement(String.Empty, "FROM_LOC_DETAIL", Nothing)
                childNode = transferXML.CreateElement(String.Empty, "FROM_LOC_ID", Nothing)
                childNode.InnerText = locationInfo.LOC_ID
                locationDetailsElement.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "FROM_LOC_NM_FIRST", Nothing)
                childNode.InnerText = locationInfo.LOC_NME
                locationDetailsElement.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "FROM_LOC_NM_LAST", Nothing)
                childNode.InnerText = locationID.ToString()
                locationDetailsElement.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "FROM_ADDR_LINE_1", Nothing)
                childNode.InnerText = locationInfo.STOR_ADDR
                locationDetailsElement.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "FROM_CITY", Nothing)
                childNode.InnerText = locationInfo.STOR_CITY_ADDR
                locationDetailsElement.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "FROM_ST", Nothing)
                childNode.InnerText = locationInfo.STOR_STAT_ADDR
                locationDetailsElement.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "FROM_ZIP_CODE", Nothing)
                childNode.InnerText = locationInfo.STOR_ZIP_ADDR
                locationDetailsElement.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "FROM_COUNTRY", Nothing)
                childNode.InnerText = locationInfo.COUNTRY
                locationDetailsElement.AppendChild(childNode)
                transferDetailsNode.AppendChild(locationDetailsElement)
            Else
                childNode = transferXML.CreateElement(String.Empty, "TO_LOC_ID", Nothing)
                childNode.InnerText = locationInfo.LOC_ID
                transferDetailsNode.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "TO_LOC_NM_FIRST", Nothing)
                childNode.InnerText = locationInfo.LOC_NME
                transferDetailsNode.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "TO_LOC_NM_LAST", Nothing)
                childNode.InnerText = locationID.ToString()
                transferDetailsNode.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "TO_ADDR_LINE_1", Nothing)
                childNode.InnerText = locationInfo.STOR_ADDR
                transferDetailsNode.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "TO_CITY", Nothing)
                childNode.InnerText = locationInfo.STOR_CITY_ADDR
                transferDetailsNode.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "TO_ST", Nothing)
                childNode.InnerText = locationInfo.STOR_STAT_ADDR
                transferDetailsNode.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "TO_ZIP_CODE", Nothing)
                childNode.InnerText = locationInfo.STOR_ZIP_ADDR
                transferDetailsNode.AppendChild(childNode)
                childNode = transferXML.CreateElement(String.Empty, "TO_COUNTRY", Nothing)
                childNode.InnerText = locationInfo.COUNTRY
                transferDetailsNode.AppendChild(childNode)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Module
