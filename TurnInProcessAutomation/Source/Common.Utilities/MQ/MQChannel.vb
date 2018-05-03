Imports IBM.WMQ
Imports System.Data.OleDb
Imports System.Web
Imports System.Text
Imports System.Configuration
Imports System.IO
Imports System.Xml

Imports System.Xml.Schema


Namespace MQSeries
    Public Class MQChannel

#Region "WriteToMQseries"

        Private _qMgr As MQQueueManager

        Public ReadOnly Property QueueManager() As MQQueueManager
            Get
                QueueManager = _qMgr
            End Get
        End Property

        Private _host As String = False
        Public Property Server() As String
            Get
                Return _host
            End Get
            Set(ByVal value As String)
                _host = value
            End Set
        End Property

        Private _port As Integer = 0
        Public Property Port() As Integer
            Get
                Return _port
            End Get
            Set(ByVal value As Integer)
                _port = value
            End Set
        End Property

        Private _channel As String = String.Empty
        Public Property Channel() As String
            Get
                Return _channel
            End Get
            Set(ByVal value As String)
                _channel = value
            End Set
        End Property

        Public Sub Connect()
            MQEnvironment.Hostname = _host
            MQEnvironment.Port = _port
            MQEnvironment.Channel = _channel
            _qMgr = New MQQueueManager
        End Sub

        Public Sub Disconnect()
            _qMgr.Commit()
            _qMgr.Disconnect()
        End Sub

#End Region
    End Class
End Namespace


