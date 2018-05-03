Imports System.Data
Imports System.Data.OleDb
Imports System.ComponentModel
Imports CodeSmith.Engine

Public Class ICCHelper
    Inherits CodeTemplate
    
    Public Function GetDataSet(ByVal connectionString As String, ByVal sql As String) As DataSet
        Dim dataSet As New DataSet
        Dim oledbDataAdatpter As OleDbDataAdapter = Nothing
        Dim oledbCommand As New OleDbCommand

        Try
            With oledbCommand
                .CommandType = CommandType.Text
                .CommandText = sql
                .Connection = New OleDbConnection(connectionString)
                .Connection.Open()
            End With

            oledbDataAdatpter = New OleDbDataAdapter(oledbCommand)
            oledbDataAdatpter.Fill(dataSet)
            Return dataSet

        Catch
            Throw
        Finally
            If oledbCommand IsNot Nothing AndAlso oledbCommand.Connection.State <> ConnectionState.Closed Then
                oledbCommand.Connection.Close()
            End If
        End Try
    End Function

    Public Function ToPascalCase(ByVal value As String) As String
        Dim newVal As String = ""

        For Each part As String In value.Split("_")
            part = part.Substring(0, 1) & part.Substring(1).ToLower
            newVal &= part
        Next part

        Return newVal
    End Function

    Public Function ToCamelCase(ByVal value As String) As String
        Dim newVal As String = ""

        For Each part As String In value.Split("_")
            Dim i As Integer
            i += 1
            part = part.ToLower
            If i > 1 Then
                part = part.Substring(0, 1).ToUpper & part.Substring(1)
            End If
            newVal &= part
        Next part

        Return newVal
    End Function

    Public Function FileClassName(ByVal table As String) As String
        Select Case table.ToUpper
            Case "TCC001AREA"
                Return "Area"
            Case "TCC002AREA_CNTREGN"
                Return "AreaCountRegion"
            Case "TCC003AREA_CNTLOC"
                Return "AreaCountLocation"
            Case "TCC004SCHED_CORP"
                Return "ScheduleCorp"
            Case "TCC005SCHED_REGN"
                Return "ScheduleRegion"
            Case "TCC006COUNTS"
                Return "Counts"
            Case "TCC007SPCL_COUNTS"
                Return "SpecialCounts"
            Case "TCC008LOC_CNTSTAT"
                Return "LocationCountStatus"
            Case "TSS100ISN"
                Return "Isn"
            Case "TSS200SKU"
                Return "Sku"
            Case "TPM190CUR_SKU_PRC"
                Return "CurrentSkuPrice"
            Case "TMI151SKU_WK_OH_OO"
                Return "SkuWeekOnHandOnOrder"
            Case Else
                Return ""
        End Select
    End Function

    Public Function ConvertType(ByVal value As String, ByVal nullable As String) As String
        Select Case value.ToUpper.Trim
            Case "SMALLINT"
                If nullable = "Y" Then
                    Return "Integer?"
                Else
                    Return "Integer"
                End If
            Case "INTEGER"
                If nullable = "Y" Then
                    Return "Integer?"
                Else
                    Return "Integer"
                End If
            Case "BIGINT"
                If nullable = "Y" Then
                    Return "Integer?"
                Else
                    Return "Integer"
                End If
            Case "REAL"
                If nullable = "Y" Then
                    Return "Single?"
                Else
                    Return "Single"
                End If
            Case "DOUBLE"
                If nullable = "Y" Then
                    Return "Double?"
                Else
                    Return "Double"
                End If
            Case "FLOAT"
                If nullable = "Y" Then
                    Return "Double?"
                Else
                    Return "Double"
                End If
            Case "DECIMAL"
                If nullable = "Y" Then
                    Return "Decimal?"
                Else
                    Return "Decimal"
                End If
            Case "NUMERIC"
                If nullable = "Y" Then
                    Return "Decimal?"
                Else
                    Return "Decimal"
                End If
            Case "DATE"
                If nullable = "Y" Then
                    Return "DateTime?"
                Else
                    Return "DateTime"
                End If
            Case "TIME"
                If nullable = "Y" Then
                    Return "TimeSpan?"
                Else
                    Return "TimeSpan"
                End If
            Case "TIMESTMP"
                If nullable = "Y" Then
                    Return "DateTime?"
                Else
                    Return "DateTime"
                End If
            Case "CHAR"
                If nullable = "Y" Then
                    Return "String"
                Else
                    Return "String"
                End If
            Case "VARCHAR"
                If nullable = "Y" Then
                    Return "String"
                Else
                    Return "String"
                End If
            Case "LONGVARCHAR(1)"
                If nullable = "Y" Then
                    Return "String"
                Else
                    Return "String"
                End If
            Case "BINARY"
                If nullable = "Y" Then
                    Return "Byte()?"
                Else
                    Return "Byte()"
                End If
            Case "VARBINARY"
                If nullable = "Y" Then
                    Return "Byte()?"
                Else
                    Return "Byte()"
                End If
            Case "LONGVARBINARY(1)"
                If nullable = "Y" Then
                    Return "Byte()?"
                Else
                    Return "Byte()"
                End If
            Case "GRAPHIC"
                If nullable = "Y" Then
                    Return "String"
                Else
                    Return "String"
                End If
            Case "VARGRAPHIC"
                If nullable = "Y" Then
                    Return "String"
                Else
                    Return "String"
                End If
            Case "LONGVARGRAPHIC(1)"
                If nullable = "Y" Then
                    Return "String"
                Else
                    Return "String"
                End If
            Case "CLOB"
                If nullable = "Y" Then
                    Return "String"
                Else
                    Return "String"
                End If
            Case "BLOB"
                If nullable = "Y" Then
                    Return "Byte()?"
                Else
                    Return "Byte()"
                End If
            Case "DBCLOB"
                If nullable = "Y" Then
                    Return "String"
                Else
                    Return "String"
                End If
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function ConvertTypeNoNullable(ByVal value As String) As String
        Select Case value.ToUpper.Trim
            Case "INTEGER"
                Return "Integer"
            Case "SMALLINT"
                Return "Integer"
            Case "CHAR"
                Return "String"
            Case "VARCHAR"
                Return "String"
            Case "DECIMAL"
                Return "Decimal"
            Case "TIMESTMP"
                Return "DateTime"
            Case "NUMERIC"
                Return "Decimal"
            Case "DATE"
                Return "Date"
            Case "GRAPHIC"
                Return "String"
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function TypeCast(ByVal expr As String, ByVal type As String, ByVal nullable As String) As String
        Select Case type.ToUpper.Trim
            Case "SMALLINT", "INTEGER", "BIGINT"
                If nullable = "Y" Then
                    Return "NothingIfIntNotHasValue(" & expr & ")"
                Else
                    Return "CInt(" & expr & ")"
                End If            
            Case "DOUBLE", "FLOAT"
                If nullable = "Y" Then
                    Return "NothingIfDblNotHasValue(" & expr & ")"
                Else
                    Return "CDbl(" & expr & ")"
                End If            
            Case "DECIMAL", "NUMERIC"
                If nullable = "Y" Then
                    Return "NothingIfDecNotHasValue(" & expr & ")"
                Else
                    Return "CDec(" & expr & ")"
                End If           
            Case "DATE"
                If nullable = "Y" Then
                    return "NothingIfDateNotHasValue(" & expr & ")"
                Else
                    return "CDate(" & expr & ")"
                End If
            Case "TIME"
                If nullable = "Y" Then
                    return "NothingIfTimeNotHasValue(" & expr & ")"
                Else
                    return "CType(" & expr & ", TimeSpan)"
                End If
            Case "TIMESTMP"
                If nullable = "Y" Then
                    return "NothingIfDateTimeNotHasValue(" & expr & ")"
                Else
                    return "CType(" & expr & ", DateTime)"
                End If
            Case "CHAR", "VARCHAR", "LONGVARCHAR(1)"
                return "CStr(" & expr & ")"                                                                
            Case Else
                Return Nothing
        End Select
    End Function
End Class
