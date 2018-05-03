Imports TurnInProcessAutomation.BusinessEntities
Imports System.Text

Public Class CommonBO

    Public Function HandleEmptyString(ByRef strValue As String) As String
        If String.IsNullOrEmpty(Trim(strValue)) Then Return "0" Else Return strValue
    End Function

    Public Shared Function ConvertBooleantoYN(ByVal YesNo As Boolean) As String
        If YesNo Then
            Return "Y"
        Else
            Return "N"
        End If
    End Function

    Public Shared Function FormulateAdminXML(ByVal DB2Results As IList(Of String)) As String
        Dim returnString As New StringBuilder
        returnString.Append("<bonton>")
        For Each ID As String In DB2Results
            returnString.Append("<admin id=""" + ID + """/>")
        Next
        returnString.Append("</bonton>")
        Return returnString.ToString
    End Function

End Class