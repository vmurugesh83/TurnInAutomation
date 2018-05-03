Imports System.Text.RegularExpressions
Imports System.Net
Imports System.Web

Namespace Validations

    Public Class Validation

#Region "Constants"
        Private Const COMP_LEN = 4
        Private Const ACCT_UNIT_LEN = 8
        Private Const ACCT_LEN = 6
        Private Const SUB_ACCT = 4
#End Region

#Region "Conversion Routines"
        ''' <summary>
        ''' Converts the string expression to the System.Nullable(Of Date) data type.
        ''' </summary>        
        Public Shared Function ConvertToNullableDate(ByVal value As String) As Date?
            If String.IsNullOrEmpty(value) Then
                Return Nothing
            Else
                Dim dte As Date
                If Date.TryParse(value, dte) Then
                    Return dte
                Else
                    Return Nothing
                End If
            End If
        End Function

        ''' <summary>
        ''' Converts the string expression to the System.Nullable(Of Decimal) data type.
        ''' </summary>        
        Public Shared Function ConvertToNullableDecimal(ByVal value As String) As Decimal?
            If String.IsNullOrEmpty(value) Then
                Return Nothing
            Else
                Dim x As Decimal
                If Decimal.TryParse(value, x) Then
                    Return x
                Else
                    Return Nothing
                End If
            End If
        End Function

        ''' <summary>
        ''' Converts the string expression to the System.Nullable(Of Integer) data type.
        ''' </summary>        
        Public Shared Function ConvertToNullableInteger(ByVal value As String) As Integer?
            If String.IsNullOrEmpty(value) Then
                Return Nothing
            Else
                Dim x As Integer
                If Integer.TryParse(value, x) Then
                    Return x
                Else
                    Return Nothing
                End If
            End If
        End Function

        'Converts a DB2 DateTime Format to Standard US Date Format(MM/DD/YYYY)
        Public Shared Function ConvertDateTime(ByVal strDateTime As String) As String

            Dim strDate As String
            strDate = strDateTime.Substring(0, 10)
            ConvertDateTime = Format(CDate(strDate), "MM/dd/yyyy")

        End Function

        'Converts a RAD DatePicker DateTime Format to DB2 Date Format(YYYY-MM-DD).
        Public Shared Function ConvertDateTime1(ByVal strDateTime As String) As String
            Dim strDate() As String
            strDate = strDateTime.Split(" ")
            ConvertDateTime1 = Format(CDate(strDate(0)), "yyyy-MM-dd")
        End Function
        'Converts Standard Data format to DB2 Data format
        Public Shared Function ConvertDBDatFmt(ByVal dteDate As Date) As String
            ConvertDBDatFmt = Format(dteDate, "yyyy-MM-dd")
        End Function

        Public Shared Function DBTimeStampMinValue(ByVal dteDate As Date) As String
            Dim strMinTimeStamp As String = ""
            If dteDate <> Nothing Then
                strMinTimeStamp = dteDate.ToString("yyyy-MM-dd") & "-00.00.00.000000"
            End If

            Return strMinTimeStamp

        End Function

        Public Shared Function DBTimeStampMaxValue(ByVal dteDate As Date) As String
            Dim strMaxTimeStamp As String = ""
            If dteDate <> Nothing Then
                strMaxTimeStamp = dteDate.ToString("yyyy-MM-dd") & "-24.00.00.000000"
            End If

            Return strMaxTimeStamp

        End Function

        Public Shared Function AccountComposite(ByVal strCompany As String, ByVal strAcctUnit As String, ByVal strAccount As String, ByVal strSubAccount As String) As String

            strCompany = strCompany.ToString().PadLeft(COMP_LEN, "0")
            strAcctUnit = strAcctUnit.Trim.PadLeft(ACCT_UNIT_LEN, "0")
            strAccount = strAccount.PadLeft(ACCT_LEN, "0")
            strSubAccount = strSubAccount.ToString().PadLeft(SUB_ACCT, "0")

            AccountComposite = strCompany + strAcctUnit + strAccount + strSubAccount

        End Function

        'Invoice date should be within 24 months prior to current date and nothing ahead of current date.
        Public Shared Function RTSDatesValidation(ByVal selectedDate As Date) As Boolean
            Dim minDate As DateTime = Date.Today
            minDate = minDate.AddMonths(-24)
            Dim maxDate As DateTime = Date.Today
            If selectedDate < minDate OrElse selectedDate > maxDate Then
                Return False
            Else
                Return True
            End If
            Return True
        End Function

        'Invoice date should be 12 months prior to current date and up to 30 days ahead of current date.
        Public Shared Function InvoiceDatesValidation(ByVal selectedDate As Date) As Boolean
            Dim minDate As DateTime = Date.Today
            minDate = minDate.AddMonths(-12)
            Dim maxDate As DateTime = Date.Today
            maxDate = maxDate.AddMonths(1)
            If selectedDate < minDate OrElse selectedDate > maxDate Then
                Return False
            Else
                Return True
            End If
            Return True
        End Function

#End Region

#Region "Validation Routines"
        'This function matches Integer, Double and Floats
        Public Shared Function IsNum(ByVal textToCheck As String) As Boolean
            Return Regex.IsMatch(textToCheck, "\b\d+(\.\d+)?\b")
        End Function


        Public Shared Function IsItNumber(ByVal inputvalue As String) As Boolean
            Dim isnumber As New Regex("[^0-9]")
            Return Not isnumber.IsMatch(inputvalue)
        End Function

        Public Shared Function YesNoMatch(ByVal textToCheck As String) As Boolean
            Return Regex.IsMatch(textToCheck, "^(YES|NO)$")
        End Function

        'This function validates US Date 
        Public Shared Function IsDate(ByVal textToCheck As Date) As Boolean
            Return Regex.IsMatch(CStr(textToCheck), "^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{4}$")
        End Function

        Public Shared Function CheckValidDate(ByVal textToCheck As Date) As Boolean
            Return Regex.IsMatch(CStr(textToCheck), "^(((((((0?[13578])|(1[02]))[\.\-/]?((0?[1-9])|([12]\d)|(3[01])))|(((0?[469])|(11))[\.\-/]?((0?[1-9])|([12]\d)|(30)))|((0?2)[\.\-/]?((0?[1-9])|(1\d)|(2[0-8]))))[\.\-/]?(((19)|(20))?([\d][\d]))))|((0?2)[\.\-/]?(29)[\.\-/]?(((19)|(20))?(([02468][048])|([13579][26])))))$")
        End Function

        Public Shared Function IsDateValid(ByVal strDate As String) As Boolean
            Dim regDate As New System.Text.RegularExpressions.Regex("^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{4}$")
            If regDate.IsMatch(strDate) Then
                Return True
            Else
                Return False
            End If
        End Function

        'function removes special characters
        Public Shared Function RemoveSpecialChar(ByVal textToCheck As String) As String
            Return Regex.Replace(textToCheck, "[^A-Za-z0-9\-/]", " ")
        End Function


#End Region

#Region "Global Routines"


#End Region

    End Class
End Namespace

