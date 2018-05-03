'=============================================================================== 
' This file is based on the Microsoft Data Access Application Block for .NET 
' For more information please go to 
' http://msdn.microsoft.com/library/en-us/dnbda/html/daab-rm.asp 
'=============================================================================== 

Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.OleDb
Imports System.Collections
Imports System.Text

''' <summary> 
''' The OleHelper class is intended to encapsulate high performance, 
''' scalable best practices for common uses of SqlClient. 
''' </summary> 
Public MustInherit Class OleHelper

    'Database connection strings 
    Public Shared ReadOnly ConnectionStringLocalTransaction As String = ConfigurationManager.ConnectionStrings("DB2Connect").ConnectionString

    ' Hashtable to store cached parameters 
    Private Shared parmCache As Hashtable = Hashtable.Synchronized(New Hashtable())

    ''' <summary> 
    ''' Execute a OleDbCommand (that returns no resultset) against the database specified in the connection string 
    ''' using the provided parameters. 
    ''' </summary> 
    ''' <remarks> 
    ''' e.g.: 
    ''' int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24)); 
    ''' </remarks> 
    ''' <param name="connectionString">a valid connection string for a OleDbConnection</param> 
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param> 
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param> 
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param> 
    ''' <returns>an int representing the number of rows affected by the command</returns> 
    Public Shared Function ExecuteNonQuery(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OleDbParameter()) As Integer
        Dim cmd As New OleDbCommand()
        Try
            Using conn As New OleDbConnection(connectionString)
                PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
                Dim val As Integer = cmd.ExecuteNonQuery()
                cmd.Parameters.Clear()
                cmd.Dispose()
                Return val
            End Using
        Catch ex As Exception
            Throw
        Finally
            cmd = Nothing
        End Try
    End Function

    ''' <summary> 
    ''' Execute a OleDbCommand (that returns no resultset) against an existing database connection 
    ''' using the provided parameters. 
    ''' </summary> 
    ''' <remarks> 
    ''' e.g.: 
    ''' int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24)); 
    ''' </remarks> 
    ''' <param name="connection">an existing database connection</param> 
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param> 
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param> 
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param> 
    ''' <returns>an int representing the number of rows affected by the command</returns> 
    Public Shared Function ExecuteNonQuery(ByVal connection As OleDbConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OleDbParameter()) As Integer
        Dim cmd As New OleDbCommand()
        Try
            PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
            Dim val As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            cmd.Dispose()
            Return val
        Catch ex As Exception
            Throw
        Finally
            cmd = Nothing
        End Try
    End Function

    ''' <summary> 
    ''' Execute a OleDbCommand (that returns no resultset) using an existing SQL Transaction 
    ''' using the provided parameters. 
    ''' </summary> 
    ''' <remarks> 
    ''' e.g.: 
    ''' int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24)); 
    ''' </remarks> 
    ''' <param name="trans">an existing sql transaction</param> 
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param> 
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param> 
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param> 
    ''' <returns>an int representing the number of rows affected by the command</returns> 
    Public Shared Function ExecuteNonQuery(ByVal trans As OleDbTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OleDbParameter()) As Integer
        Dim cmd As New OleDbCommand()
        Try
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters)
            Dim val As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            cmd.Dispose()
            Return val
        Catch ex As Exception
            Throw
        Finally
            cmd = Nothing
        End Try
    End Function

    ''' <summary> 
    ''' Execute a OleDbCommand that returns a resultset against the database specified in the connection string 
    ''' using the provided parameters. 
    ''' </summary> 
    ''' <remarks> 
    ''' e.g.: 
    ''' OleDbDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24)); 
    ''' </remarks> 
    ''' <param name="connectionString">a valid connection string for a OleDbConnection</param> 
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param> 
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param> 
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param> 
    ''' <returns>A OleDbDataReader containing the results</returns> 
    Public Shared Function ExecuteReader(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OleDbParameter()) As OleDbDataReader
        Dim cmd As New OleDbCommand()
        Dim conn As New OleDbConnection(connectionString)
        ' we use a try/catch here because if the method throws an exception we want to 
        ' close the connection throw code, because no datareader will exist, hence the 
        ' commandBehaviour.CloseConnection will not work 
        Try
            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            Dim rdr As OleDbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            cmd.Parameters.Clear()
            cmd.Dispose()
            Return rdr
        Catch
            conn.Close()
            Throw
        Finally
            cmd = Nothing
            conn = Nothing
        End Try
    End Function

    ''' <summary> 
    ''' Execute a OleDbCommand that returns the first column of the first record against the database specified in the connection string 
    ''' using the provided parameters. 
    ''' </summary> 
    ''' <remarks> 
    ''' e.g.: 
    ''' Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24)); 
    ''' </remarks> 
    ''' <param name="connectionString">a valid connection string for a OleDbConnection</param> 
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param> 
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param> 
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param> 
    ''' <returns>An object that should be converted to the expected type using Convert.To{Type}</returns> 
    Public Shared Function ExecuteScalar(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OleDbParameter()) As Object
        Dim cmd As New OleDbCommand()
        Try
            Using connection As New OleDbConnection(connectionString)
                PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
                Dim val As Object = cmd.ExecuteScalar()
                cmd.Parameters.Clear()
                cmd.Dispose()
                Return val
            End Using
        Catch ex As Exception
            Throw
        Finally
            cmd = Nothing
        End Try
    End Function

    ''' <summary> 
    ''' Execute a OleDbCommand that returns the first column of the first record against an existing database connection 
    ''' using the provided parameters. 
    ''' </summary> 
    ''' <remarks> 
    ''' e.g.: 
    ''' Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24)); 
    ''' </remarks> 
    ''' <param name="connection">an existing database connection</param> 
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param> 
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param> 
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param> 
    ''' <returns>An object that should be converted to the expected type using Convert.To{Type}</returns> 
    Public Shared Function ExecuteScalar(ByVal connection As OleDbConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OleDbParameter()) As Object
        Dim cmd As New OleDbCommand()
        Try
            PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
            Dim val As Object = cmd.ExecuteScalar()
            cmd.Parameters.Clear()
            cmd.Dispose()
            Return val
        Catch ex As Exception
            Throw
        Finally
            cmd = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Execute a OleDbCommand that returns the first column of the first record using an existing SQL Transaction  
    ''' using the provided parameters. 
    ''' </summary>
    ''' <param name="trans"></param>
    ''' <param name="cmdType"></param>
    ''' <param name="cmdText"></param>
    ''' <param name="commandParameters"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ExecuteScalar(ByVal trans As OleDbTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OleDbParameter()) As Integer
        Dim cmd As New OleDbCommand()
        Try
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters)
            Dim val As Integer = cmd.ExecuteScalar()
            cmd.Parameters.Clear()
            cmd.Dispose()
            Return val
        Catch ex As Exception
            Throw
        Finally
            cmd = Nothing
        End Try
    End Function

    ''' <summary> 
    ''' add parameter array to the cache 
    ''' </summary> 
    ''' <param name="cacheKey">Key to the parameter cache</param> 
    ''' <param name="commandParameters">an array of SqlParamters to be cached</param> 
    Public Shared Sub CacheParameters(ByVal cacheKey As String, ByVal ParamArray commandParameters As OleDbParameter())
        parmCache(cacheKey) = commandParameters
    End Sub

    ''' <summary> 
    ''' Retrieve cached parameters 
    ''' </summary> 
    ''' <param name="cacheKey">key used to lookup parameters</param> 
    ''' <returns>Cached SqlParamters array</returns> 
    Public Shared Function GetCachedParameters(ByVal cacheKey As String) As OleDbParameter()
        Dim cachedParms As OleDbParameter() = DirectCast(parmCache(cacheKey), OleDbParameter())
        If cachedParms Is Nothing Then
            Return Nothing
        End If

        Dim clonedParms As OleDbParameter() = New OleDbParameter(cachedParms.Length - 1) {}

        Dim i As Integer = 0, j As Integer = cachedParms.Length
        While i < j
            clonedParms(i) = DirectCast(DirectCast(cachedParms(i), ICloneable).Clone(), OleDbParameter)
            i += 1
        End While
        Return clonedParms
    End Function

    ''' <summary>
    ''' Formats a List (Of String) into a SQL IN predicate clause.
    ''' </summary>    
    Public Shared Function FormatInListOfString(ByVal collection As IList(Of String)) As String
        Dim sb As New StringBuilder
        For Each value As String In collection
            sb.AppendFormat("'{0}',", value)
        Next
        Return sb.ToString.TrimEnd(","c)
    End Function

    ''' <summary>
    ''' Formats a List (Of Integer) into a SQL IN predicate clause.
    ''' </summary>    
    Public Shared Function FormatInListOfString(ByVal collection As List(Of Integer)) As String
        Dim sb As New StringBuilder
        For Each value As Integer In collection
            sb.AppendFormat("{0},", value)
        Next
        Return sb.ToString.TrimEnd(","c)
    End Function

    ''' <summary>
    ''' Formats a List (Of Decimal) into a SQL IN predicate clause.
    ''' </summary>    
    Public Shared Function FormatInListOfString(ByVal collection As List(Of Decimal)) As String
        Dim sb As New StringBuilder
        For Each value As Decimal In collection
            sb.AppendFormat("{0},", value)
        Next
        Return sb.ToString.TrimEnd(","c)
    End Function

    ''' <summary> 
    ''' Prepare a command for execution 
    ''' </summary> 
    ''' <param name="cmd">OleDbCommand object</param> 
    ''' <param name="conn">OleDbConnection object</param> 
    ''' <param name="trans">OleDbTransaction object</param> 
    ''' <param name="cmdType">Cmd type e.g. stored procedure or text</param> 
    ''' <param name="cmdText">Command text, e.g. Select * from Products</param> 
    ''' <param name="cmdParms">OleDbParameters to use in the command</param> 
    Private Shared Sub PrepareCommand(ByVal cmd As OleDbCommand, ByVal conn As OleDbConnection, ByVal trans As OleDbTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal cmdParms As OleDbParameter())
        If conn.State <> ConnectionState.Open Then
            conn.Open()
        End If
        cmd.Connection = conn
        cmd.CommandText = cmdText

        If trans IsNot Nothing Then
            cmd.Transaction = trans
        End If
        cmd.CommandType = cmdType
        If cmdParms IsNot Nothing Then
            For Each parm As OleDbParameter In cmdParms
                cmd.Parameters.Add(parm)
            Next
        End If
    End Sub
End Class
