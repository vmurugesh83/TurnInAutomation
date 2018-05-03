'=============================================================================== 
' This file is based on the Microsoft Data Access Application Block for .NET 
' For more information please go to 
' http://msdn.microsoft.com/library/en-us/dnbda/html/daab-rm.asp 
'=============================================================================== 

Imports System
Imports System.Configuration
Imports System.Data
Imports IBM.Data.DB2
Imports System.Collections

Public MustInherit Class DB2Helper

    'Database connection strings 
    Public Shared ReadOnly ConnectionStringLocalTransaction As String = ConfigurationManager.ConnectionStrings("DB2Connect").ToString
    ' Hashtable to store cached parameters 
    Private Shared parmCache As Hashtable = Hashtable.Synchronized(New Hashtable())

    Public Shared Function ExecuteNonQuery(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As DB2Parameter()) As Integer
        Dim cmd As New DB2Command()
        Using conn As New DB2Connection(connectionString)
            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            Dim val As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            Return val
        End Using
    End Function

    Public Shared Function ExecuteNonQuery(ByVal connection As DB2Connection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As DB2Parameter()) As Integer
        Dim cmd As New DB2Command()
        PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
        Dim val As Integer = cmd.ExecuteNonQuery()
        cmd.Parameters.Clear()
        Return val
    End Function

    Public Shared Function ExecuteNonQuery(ByVal trans As DB2Transaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As DB2Parameter()) As Integer
        Dim cmd As New DB2Command()
        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters)
        Dim val As Integer = cmd.ExecuteNonQuery()
        cmd.Parameters.Clear()
        Return val
    End Function

    Public Shared Function ExecuteReader(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As DB2Parameter()) As DB2DataReader
        Dim cmd As New DB2Command()
        cmd.CommandTimeout = 0
        Dim conn As New DB2Connection(connectionString)
        ' we use a try/catch here because if the method throws an exception we want to 
        ' close the connection throw code, because no datareader will exist, hence the 
        ' commandBehaviour.CloseConnection will not work 
        Try
            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            Dim rdr As DB2DataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            cmd.Parameters.Clear()
            Return rdr
        Catch ex As Exception
            conn.Close()
            Throw
        End Try
    End Function

    Public Shared Function ExecuteScalar(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As DB2Parameter()) As Object
        Dim cmd As New DB2Command()
        cmd.CommandTimeout = 0
        Using connection As New DB2Connection(connectionString)
            PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
            Dim val As Object = cmd.ExecuteScalar()
            cmd.Parameters.Clear()
            Return val
        End Using
    End Function

    Public Shared Function ExecuteScalar(ByVal connection As DB2Connection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As DB2Parameter()) As Object
        Dim cmd As New DB2Command()
        cmd.CommandTimeout = 0
        PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
        Dim val As Object = cmd.ExecuteScalar()
        cmd.Parameters.Clear()
        Return val
    End Function

    Public Shared Function ExecuteScalar(ByVal trans As DB2Transaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As DB2Parameter()) As Integer
        Dim cmd As New DB2Command()
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

    Public Shared Sub CacheParameters(ByVal cacheKey As String, ByVal ParamArray commandParameters As DB2Parameter())
        parmCache(cacheKey) = commandParameters
    End Sub

    Public Shared Function GetCachedParameters(ByVal cacheKey As String) As DB2Parameter()
        Dim cachedParms As DB2Parameter() = DirectCast(parmCache(cacheKey), DB2Parameter())
        If cachedParms Is Nothing Then
            Return Nothing
        End If

        Dim clonedParms As DB2Parameter() = New DB2Parameter(cachedParms.Length - 1) {}
        Dim i As Integer = 0, j As Integer = cachedParms.Length
        While i < j
            clonedParms(i) = DirectCast(DirectCast(cachedParms(i), ICloneable).Clone(), DB2Parameter)
            i += 1
        End While
        Return clonedParms
    End Function

    Private Shared Sub PrepareCommand(ByVal cmd As DB2Command, ByVal conn As DB2Connection, ByVal trans As DB2Transaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal cmdParms As DB2Parameter())
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
            For Each parm As DB2Parameter In cmdParms
                'If Not parm.Value Is DBNull.Value AndAlso parm.DB2Type = DB2Type.Char AndAlso parm.Value = CChar("") Then
                    'parm.Value = " "
                'End If
                cmd.Parameters.Add(parm)
            Next
        End If
    End Sub

    'Added by Rob Woehr on 12/2/2016 for debugging purposes only
    Public Shared Function ExecGetDataTable(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As DB2Parameter()) As DataTable

        Dim cmd As New DB2Command()
        cmd.CommandTimeout = 0
        Dim rdr As DB2DataReader
        Dim conn As New DB2Connection(connectionString)
        Try
            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            cmd.Parameters.Clear()
        Catch ex As Exception
            conn.Close()
            Throw
        End Try

        Dim dtReturn As DataTable = New DataTable("dt")
        Dim dtSchemaTable As DataTable = rdr.GetSchemaTable
        Dim cField As Integer = dtSchemaTable.Rows.Count
        Dim iField As Integer = 0
        While (iField < cField)
            dtReturn.Columns.Add(New DataColumn(dtSchemaTable.Rows(iField)("ColumnName").ToLower(), _
                                                dtSchemaTable.Rows(iField)("DataType")))
            iField += 1
        End While
        '
        If (rdr.HasRows) Then
            Dim drRow As DataRow
            While rdr.Read
                drRow = dtReturn.NewRow
                iField = 0
                While (iField < cField)
                    drRow(iField) = rdr(iField)

                    iField += 1
                End While
                dtReturn.Rows.Add(drRow)
            End While
        End If

        Return dtReturn
    End Function

End Class

