Imports System.Configuration

Public Class Common
    Public Shared Function GetDateDiff(ByVal Interval As String, ByVal Date1 As Date, ByVal Date2 As Date, ByVal FirstWeekOfDay As FirstDayOfWeek) As Long
        If (Date1 <= Date.MinValue OrElse Date2 <= Date.MinValue) Then
            Return 0
        Else
            Return DateDiff(Interval, Date1, Date2, FirstWeekOfDay)
        End If

    End Function
    Public Shared Sub GetRecipients(ByVal configurationKeyName As String, recipients As List(Of String))
        Dim emailAddresses As String = ConfigurationManager.AppSettings(configurationKeyName)
        Try

            If Not String.IsNullOrEmpty(emailAddresses) Then
                For Each emailAddress As String In emailAddresses.Split(",").ToList()
                    recipients.Add(emailAddress)
                Next
            Else
                Throw New Exception(String.Format("No email address is configured for the key : {0}.", configurationKeyName))
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Enum AdType
        Ecommerce
        VendorImage
        ExtraHot
        Lift
        INFC
    End Enum

End Class
