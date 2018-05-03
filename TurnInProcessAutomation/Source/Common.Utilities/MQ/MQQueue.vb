Imports IBM.WMQ

Namespace MQSeries
    Public Class MQQueue

        Property _queueName As String = False
        Public Property Name() As String
            Get
                Return _queueName
            End Get
            Set(ByVal value As String)
                _queueName = value
            End Set
        End Property

        Property _channel As MQChannel
        Public Property Channel() As MQChannel
            Get
                Return _channel
            End Get
            Set(ByVal value As MQChannel)
                _channel = value
            End Set
        End Property

        Public Sub Put(ByVal messageText As String)
            Dim myQueue As IBM.WMQ.MQQueue = _channel.QueueManager.AccessQueue(_queueName, MQC.MQOO_OUTPUT)

            Dim pmo As New MQPutMessageOptions()
            Dim message As MQMessage

            message = New MQMessage
            message.Format = MQC.MQFMT_STRING
            message.MessageId = MQC.MQMI_NONE
            message.CorrelationId = MQC.MQCI_NONE

            Dim strMessage As String = messageText
            message.WriteString(strMessage)

            myQueue.Put(message, pmo)
        End Sub
    End Class
End Namespace
