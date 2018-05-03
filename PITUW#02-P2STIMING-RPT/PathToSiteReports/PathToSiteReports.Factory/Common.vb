Imports IBM.Data.DB2
Imports System.Data.Common

Public Class Common

    Friend Shared Function ReadColumn(ByVal reader As DbDataReader, ByVal ColumnName As String) As Object
        Try
            If HasColumn(reader, ColumnName) Then
                Return reader(ColumnName)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function HasColumn(ByVal Reader As DbDataReader, ByVal ColumnName As String) As Boolean
        Dim i As Integer
        For i = 0 To Reader.FieldCount - 1
            'If ColumnName.ToUpper = "CTGY_LIST" Then
            '    MsgBox(ColumnName, MsgBoxStyle.OkOnly)
            'End If
            If Reader.GetName(i).ToString().ToUpper() = ColumnName.ToUpper() Then
                Return True
            End If

        Next
        Return False
    End Function

    Public Shared Function ConvertToString(ByVal O As Object) As String
        Dim returnVal As String = ""
        Try
            If Not IsDBNull(O) Then returnVal = CStr(O)
        Catch ex As Exception

        End Try
        Return returnVal
    End Function

    Public Shared Function ConvertToDate(ByVal inputValue As Object) As Date
        If Not IsDBNull(inputValue) AndAlso CDate(inputValue) > Date.MinValue Then
            Return CDate(inputValue)
        Else
            Return Date.MinValue
        End If

    End Function
    Public Shared Function ConvertToInt(ByVal obj As Object) As Integer
        Dim returnVal As Integer = 0
        Try
            If Not IsDBNull(obj) Then returnVal = CInt(obj)
        Catch ex As Exception

        End Try
        Return returnVal
    End Function

End Class
