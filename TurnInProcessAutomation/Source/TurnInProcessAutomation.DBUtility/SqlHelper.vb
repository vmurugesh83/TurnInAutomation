'=============================================================================== 
' This file is based on the Microsoft Data Access Application Block for .NET 
' For more information please go to 
' http://msdn.microsoft.com/library/en-us/dnbda/html/daab-rm.asp 
'=============================================================================== 

Imports System
Imports System.Configuration
Imports System.Data
Imports System.Collections
Imports System.Text
Imports System.Data.SqlClient

''' <summary> 
''' The OleHelper class is intended to encapsulate high performance, 
''' scalable best practices for common uses of SqlClient. 
''' </summary> 
Public MustInherit Class SqlHelper

    'Database connection strings 
    Public Shared ReadOnly ConnectionStringLocalTransaction As String = ConfigurationManager.ConnectionStrings("SQLServer").ConnectionString
    Public Shared ReadOnly ConnectionStringVirtualTicket As String = ConfigurationManager.ConnectionStrings("VirtualTicketSQLServer").ConnectionString

    ' Hashtable to store cached parameters 
    Private Shared parmCache As Hashtable = Hashtable.Synchronized(New Hashtable())

    ''' <summary> 
    ''' Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
    ''' using the provided parameters. 
    ''' </summary> 
    ''' <remarks> 
    ''' e.g.: 
    ''' int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24)); 
    ''' </remarks> 
    ''' <param name="connectionString">a valid connection string for a SqlConnection</param> 
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param> 
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param> 
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param> 
    ''' <returns>an int representing the number of rows affected by the command</returns> 
    Public Shared Function ExecuteNonQuery(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Integer
        Dim cmd As New SqlCommand()
        Try
            Using conn As New SqlConnection(connectionString)
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
    ''' Execute a SqlCommand (that returns no resultset) against an existing database connection 
    ''' using the provided parameters. 
    ''' </summary> 
    ''' <remarks> 
    ''' e.g.: 
    ''' int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24)); 
    ''' </remarks> 
    ''' <param name="connection">an existing database connection</param> 
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param> 
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param> 
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param> 
    ''' <returns>an int representing the number of rows affected by the command</returns> 
    Public Shared Function ExecuteNonQuery(ByVal connection As SqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Integer
        Dim cmd As New SqlCommand()
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
    ''' Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
    ''' using the provided parameters. 
    ''' </summary> 
    ''' <remarks> 
    ''' e.g.: 
    ''' int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24)); 
    ''' </remarks> 
    ''' <param name="trans">an existing sql transaction</param> 
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param> 
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param> 
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param> 
    ''' <returns>an int representing the number of rows affected by the command</returns> 
    Public Shared Function ExecuteNonQuery(ByVal trans As SqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Integer
        Dim cmd As New SqlCommand()
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
    ''' Execute a SqlCommand that returns a resultset against the database specified in the connection string 
    ''' using the provided parameters. 
    ''' </summary> 
    ''' <remarks> 
    ''' e.g.: 
    ''' SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24)); 
    ''' </remarks> 
    ''' <param name="connectionString">a valid connection string for a SqlConnection</param> 
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param> 
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param> 
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param> 
    ''' <returns>A SqlDataReader containing the results</returns> 
    Public Shared Function ExecuteReader(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As SqlDataReader
        Dim cmd As New SqlCommand()
        Dim conn As New SqlConnection(connectionString)
        ' we use a try/catch here because if the method throws an exception we want to 
        ' close the connection throw code, because no datareader will exist, hence the 
        ' commandBehaviour.CloseConnection will not work 
        Try
            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
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
    ''' Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
    ''' using the provided parameters. 
    ''' </summary> 
    ''' <remarks> 
    ''' e.g.: 
    ''' Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24)); 
    ''' </remarks> 
    ''' <param name="connectionString">a valid connection string for a SqlConnection</param> 
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param> 
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param> 
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param> 
    ''' <returns>An object that should be converted to the expected type using Convert.To{Type}</returns> 
    Public Shared Function ExecuteScalar(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Object
        Dim cmd As New SqlCommand()
        Try
            Using connection As New SqlConnection(connectionString)
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
    ''' Execute a SqlCommand that returns the first column of the first record against an existing database connection 
    ''' using the provided parameters. 
    ''' </summary> 
    ''' <remarks> 
    ''' e.g.: 
    ''' Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24)); 
    ''' </remarks> 
    ''' <param name="connection">an existing database connection</param> 
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param> 
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param> 
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param> 
    ''' <returns>An object that should be converted to the expected type using Convert.To{Type}</returns> 
    Public Shared Function ExecuteScalar(ByVal connection As SqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Object
        Dim cmd As New SqlCommand()
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
    ''' Execute a SqlCommand that returns the first column of the first record using an existing SQL Transaction  
    ''' using the provided parameters. 
    ''' </summary>
    ''' <param name="trans"></param>
    ''' <param name="cmdType"></param>
    ''' <param name="cmdText"></param>
    ''' <param name="commandParameters"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ExecuteScalar(ByVal trans As SqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Integer
        Dim cmd As New SqlCommand()
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
    Public Shared Sub CacheParameters(ByVal cacheKey As String, ByVal ParamArray commandParameters As SqlParameter())
        parmCache(cacheKey) = commandParameters
    End Sub

    ''' <summary> 
    ''' Retrieve cached parameters 
    ''' </summary> 
    ''' <param name="cacheKey">key used to lookup parameters</param> 
    ''' <returns>Cached SqlParamters array</returns> 
    Public Shared Function GetCachedParameters(ByVal cacheKey As String) As SqlParameter()
        Dim cachedParms As SqlParameter() = DirectCast(parmCache(cacheKey), SqlParameter())
        If cachedParms Is Nothing Then
            Return Nothing
        End If

        Dim clonedParms As SqlParameter() = New SqlParameter(cachedParms.Length - 1) {}

        Dim i As Integer = 0, j As Integer = cachedParms.Length
        While i < j
            clonedParms(i) = DirectCast(DirectCast(cachedParms(i), ICloneable).Clone(), SqlParameter)
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
    ''' <param name="cmd">SqlCommand object</param> 
    ''' <param name="conn">SqlConnection object</param> 
    ''' <param name="trans">SqlTransaction object</param> 
    ''' <param name="cmdType">Cmd type e.g. stored procedure or text</param> 
    ''' <param name="cmdText">Command text, e.g. Select * from Products</param> 
    ''' <param name="cmdParms">SqlParameters to use in the command</param> 
    Private Shared Sub PrepareCommand(ByVal cmd As SqlCommand, ByVal conn As SqlConnection, ByVal trans As SqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal cmdParms As SqlParameter())
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
            For Each parm As SqlParameter In cmdParms
                cmd.Parameters.Add(parm)
            Next
        End If
    End Sub
End Class
