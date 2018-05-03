Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports CodeSmith.Engine

Public Class ICCHelper
    Inherits CodeTemplate
    
    Public Function GetDataSet(ByVal connectionString As String, ByVal sql As String) As DataSet
        Dim dataSet As New DataSet
        Dim sda As SqlDataAdapter = Nothing
        Dim cmd As New SqlCommand

        Try
            With cmd
                .CommandType = CommandType.Text
                .CommandText = sql
                .Connection = New SqlConnection(connectionString)
                .Connection.Open()
            End With

            sda = New SqlDataAdapter(cmd)
            sda.Fill(dataSet)
            Return dataSet

        Catch ex as Exception 
            Throw ex
        Finally
            If cmd IsNot Nothing AndAlso cmd.Connection.State <> ConnectionState.Closed Then
                cmd.Connection.Close()
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
            part = part.ToLower
            part = part.Substring(0, 1).ToUpper & part.Substring(1)
            newVal &= part
        Next part

        Return newVal
    End Function

    Public Function ConvertType(ByVal value As String, ByVal nullable As String) As String
        Select Case value.ToUpper.Trim
            Case "SMALLINT"
                If nullable = "Y" Then
                    Return "Integer?"
                Else
                    Return "Integer"
                End If
            Case "INT"
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
            Case "DATETIME"
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
            Case "SMALLINT", "INTEGER", "BIGINT", "INT"
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
            Case "DATE","DATETIME"
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
