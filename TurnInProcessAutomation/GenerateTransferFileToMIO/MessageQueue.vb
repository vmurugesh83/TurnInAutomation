Imports System.Configuration
Imports BonTon.Common
Public Class MessageQueue
    Public Sub PutTransferMessage(ByVal transferXML As String)
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

            message = transferXML
            mqQueue.Put(message)

            mqChannel.Disconnect()
        Catch ex As IBM.WMQ.MQException
            Throw New Exception("SendSampleRequest Failed " & ex.Message & "( RC : " & ex.Reason.ToString & " Reason : " & ex.Reason.ToString & " Completion Code : " & ex.CompletionCode.ToString & " )")
        End Try
    End Sub

End Class
