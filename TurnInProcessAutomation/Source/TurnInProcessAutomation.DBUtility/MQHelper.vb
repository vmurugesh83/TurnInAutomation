Imports System
Imports System.Configuration
Imports IBM.WMQ


Public Class MQHelper

#Region "Variables"

    Public Shared ReadOnly MQChannel As String = ConfigurationManager.AppSettings("MQChannel").ToString
    Public Shared ReadOnly MQHostName As String = ConfigurationManager.AppSettings("MQHostName").ToString
    Public Shared ReadOnly MQPort As Integer = CInt(ConfigurationManager.AppSettings("MQPort"))

    Private Shared ReadOnly Log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private _mqReqQ As String
    Private _mqRespQ As String
    Private _mqQueueManager As MQQueueManager
    Private _mqQueue As MQQueue

#End Region

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal mqReqQ As String, ByVal mqRespQ As String)
        _mqReqQ = mqReqQ
        _mqRespQ = mqRespQ
    End Sub

#End Region

#Region "Properties"

    Public Property MqReqQ() As String
        Get
            Return _mqReqQ
        End Get
        Set(ByVal value As String)
            _mqReqQ = value
        End Set
    End Property

    Public Property MqRespQ() As String
        Get
            Return _mqRespQ
        End Get
        Set(ByVal value As String)
            _mqRespQ = value
        End Set
    End Property

    Private Property MqQueueManager() As MQQueueManager
        Get
            Return _mqQueueManager
        End Get
        Set(ByVal value As MQQueueManager)
            _mqQueueManager = value
        End Set
    End Property

    Private Property MqQueue() As MQQueue
        Get
            Return _mqQueue
        End Get
        Set(ByVal value As MQQueue)
            _mqQueue = value
        End Set
    End Property

#End Region

#Region "Methods"

    ''' <summary>
    ''' set message connection info
    ''' </summary>	
    Private Sub MQConn()
        Try
            MQEnvironment.Hostname = MQHostName
            MQEnvironment.Port = MQPort
            MQEnvironment.Channel = MQChannel
            MqQueueManager = New MQQueueManager()

        Catch mqEx As MQException
            Log.Fatal("MQ Exception", mqEx)
            Throw
        Catch ex As Exception
            Log.Fatal(ex)
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' open queue access
    ''' </summary>	
    Private Sub MQOpen(ByVal openOptions As Integer, ByVal mqQueueName As String)
        Try
            MqQueue = MqQueueManager.AccessQueue(mqQueueName, openOptions)

        Catch mqEx As MQException
            Log.Fatal("MQ Exception", mqEx)
            Throw
        Catch ex As Exception
            Log.Fatal(ex)
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' write messages to queue
    ''' </summary>	
    Public Sub MQPut(ByVal openOptions As Integer, ByVal mqMessage As MQMessage)
        MQPut(openOptions, New MQPutMessageOptions, mqMessage)
    End Sub

    ''' <summary>
    ''' write messages to queue
    ''' </summary>	
    Public Sub MQPut(ByVal openOptions As Integer, ByVal mqPutMessageOptions As MQPutMessageOptions, ByVal mqMessage As MQMessage)
        Try
            MQConn()
            MQOpen(openOptions, MqReqQ)
            MqQueue.Put(mqMessage, mqPutMessageOptions)
            MQClose()
            MQDisc()
        Catch mqEx As MQException
            Log.Fatal("MQ Exception", mqEx)
            Throw
        Catch ex As Exception
            Log.Fatal(ex)
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' read messages from queue.
    ''' </summary>	
    Public Function MQGet(ByVal openOptions As Integer, ByVal mqGetMessageOptions As MQGetMessageOptions, ByVal mqMessage As MQMessage) As IBM.WMQ.MQMessage
        Try
            MQConn()
            MQOpen(openOptions, MqRespQ)
            MqQueue.Get(mqMessage, mqGetMessageOptions)
            MQClose()
            MQDisc()
            Return mqMessage

        Catch mqEx As MQException
            If mqEx.ReasonCode = 2033 Then
                Throw New ApplicationException("Match response not received, system wait time exceeded. Contact I.S. ")
            Else
                Log.Fatal("MQ Exception", mqEx)
                Throw (mqEx)
            End If
            
        Catch ex As Exception
            Log.Fatal(ex)
            Throw
        End Try
    End Function

    ''' <summary>
    ''' close connection.
    ''' </summary>	
    Private Sub MQClose()
        Try
            MqQueue.Close()

        Catch mqEx As MQException
            Log.Fatal("MQ Exception", mqEx)
            Throw
        Catch ex As Exception
            Log.Fatal(ex)
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' disconnect connection.
    ''' </summary>	
    Private Sub MQDisc()
        Try
            MqQueueManager.Disconnect()

        Catch mqEx As MQException
            Log.Fatal("MQ Exception", mqEx)
            Throw
        Catch ex As Exception
            Log.Fatal(ex)
            Throw
        End Try
    End Sub

#End Region


End Class
