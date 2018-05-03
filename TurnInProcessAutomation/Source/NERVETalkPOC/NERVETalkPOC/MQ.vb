Imports IBM.WMQ
Imports System.Data.OleDb
Imports System.Web
Imports System.Text
Imports System.Configuration
Imports System.IO
Imports System.Xml

Imports System.Xml.Schema



Public Class MQ

#Region "WriteToMQseries"

    Private _qMgr As MQQueueManager
    Public ReadOnly Property QueueManager() As MQQueueManager
        Get
            QueueManager = _qMgr
        End Get
    End Property

    Public Sub connectToMQ(ByVal MQHost As String, ByVal MQPort As Integer, ByVal MQChannel As String)
        MQEnvironment.Hostname = MQHost
        MQEnvironment.Port = MQPort
        MQEnvironment.Channel = MQChannel
        _qMgr = New MQQueueManager
    End Sub
    Public Sub disconnectFromMQ()
        _qMgr.Commit()
        _qMgr.Disconnect()
    End Sub

    Public Function PutToMQ(ByVal QueueName As String, ByVal messageText As String, ByVal appNme As String, ByVal timeStamp As String) As String
        Try

            Dim myQueue As MQQueue = _qMgr.AccessQueue(QueueName, MQC.MQOO_OUTPUT)

            Dim pmo As New MQPutMessageOptions()
            Dim message As MQMessage

            message = New MQMessage
            message.Format = MQC.MQFMT_STRING
            message.MessageId = MQC.MQMI_NONE
            message.CorrelationId = MQC.MQCI_NONE

            Dim strMessage As String = messageText
            message.WriteString(strMessage)

            myQueue.Put(message, pmo)

        Catch ex As Exception
            Dim sbMessageText As New StringBuilder
            sbMessageText.AppendLine(ex.Message)
            sbMessageText.AppendLine(appNme & timeStamp & messageText & ";")

            'LogStoredProcSQL(sbMessageText.ToString, "Exception", "MQ")

            Return ex.Message
        End Try
        Return String.Empty
    End Function

    Public Function PutToMQ(ByVal connection As MQQueueManager, ByVal QueueName As String, ByVal messageText As String, ByVal maintNme As String, ByVal timeStamp As String) As String
        Try

            Dim myQueue As MQQueue = connection.AccessQueue(QueueName, MQC.MQOO_OUTPUT)

            Dim pmo As New MQPutMessageOptions()
            Dim message As MQMessage

            message = New MQMessage
            message.Format = MQC.MQFMT_STRING
            message.MessageId = MQC.MQMI_NONE
            message.CorrelationId = MQC.MQCI_NONE

            Dim strMessage As String = messageText
            message.WriteString(strMessage)

            myQueue.Put(message, pmo)

        Catch ex As Exception
            Dim sbMessageText As New StringBuilder
            sbMessageText.AppendLine(ex.Message)
            sbMessageText.AppendLine(maintNme & timeStamp & messageText & ";")

            'LogStoredProcSQL(sbMessageText.ToString, "Exception", "MQ")

            Return ex.Message
        End Try
        Return String.Empty
    End Function

    Public Shared Function GetMicroTimeStamp() As String
        Dim nowDate As Date = Now
        Return String.Format("{0}-{1}-{2}-{3}.{4}.{5}.{6}", nowDate.Year.ToString.PadLeft(4, "0"), nowDate.Month.ToString.PadLeft(2, "0"), nowDate.Day.ToString.PadLeft(2, "0"), nowDate.Hour.ToString.PadLeft(2, "0"), nowDate.Minute.ToString.PadLeft(2, "0"), nowDate.Second.ToString.PadLeft(2, "0"), nowDate.Millisecond.ToString.PadRight(6, "0"))
    End Function

    'Private Shared Function LogStoredProcSQL(ByVal Query As String, ByVal LogType As String, ByVal Operation As String) As String
    '    Dim fileName As String = HttpContext.Current.Server.MapPath("~/_Logs/")

    '    If UCase(ConfigurationManager.AppSettings(String.Format("LogStoredProc{0}", LogType))) = "TRUE" Then
    '        fileName = String.Format("{0}{1}\{2}\", fileName, LogType, Operation)

    '        If Not Directory.Exists(fileName) Then
    '            Directory.CreateDirectory(fileName)
    '        End If

    '        fileName = String.Format("{0}U{1} {2}.txt", fileName, GetExecUserID, GetExecDateString)

    '        Dim fs As New FileStream(fileName, FileMode.Create, FileAccess.Write)
    '        Dim s As New StreamWriter(fs)
    '        s.BaseStream.Seek(0, SeekOrigin.End)
    '        s.Write(Query)
    '        s.Close()
    '    End If

    '    Return String.Empty
    'End Function

    'Private Shared Function GetExecUserID() As String
    '    If HttpContext.Current.Session("UserName") IsNot Nothing Then
    '        Return HttpContext.Current.Session("UserName")
    '    Else
    '        Dim cUserID As String = HttpContext.Current.Session("UserName")
    '        If cUserID.Contains("\") Then
    '            cUserID = cUserID.Substring(cUserID.IndexOf("\") + 1, cUserID.Length - (cUserID.IndexOf("\") + 1))
    '        End If
    '        Return cUserID
    '    End If
    'End Function

    Private Shared Function GetExecDateString() As String
        Dim nowDate As Date = Now
        Return String.Format("D{0} {1} {2} T{3} {4} {5}", nowDate.Month, nowDate.Day, nowDate.Year, nowDate.Hour, nowDate.Minute, nowDate.Second)
    End Function

#End Region
End Class

