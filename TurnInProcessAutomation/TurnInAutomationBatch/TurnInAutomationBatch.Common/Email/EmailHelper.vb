Imports Microsoft.VisualBasic
Imports System.Net.Mail
Imports System.IO

Public Class EmailHelper

    Public Shared Function Send(ByVal Recipients As List(Of String), ByVal Sender As String,
                                ByVal Subject As String, ByVal Body As String,
                                ByVal AttachmentFileName As String,
                                Optional ByVal shouldAddCCEmailAddress As Boolean = True) As Boolean
        Dim ret As Boolean = True

        Dim client As New SmtpClient("smtp.internal.bonton.com")
        Dim objEmail As New MailMessage
        With objEmail
            .From = New MailAddress(Sender)
            For Each recip As String In Recipients
                .To.Add(New MailAddress(recip))
            Next
            If shouldAddCCEmailAddress Then
                .CC.Add(New MailAddress(Sender))
            End If
            .Subject = Subject
            .Body = Body
            .IsBodyHtml = True
        End With

        If Not String.IsNullOrEmpty(AttachmentFileName) Then
            'Use a semicolon separated list for multiple attachments
            If AttachmentFileName.IndexOf(";") = -1 Then
                objEmail.Attachments.Add(New Attachment(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output", AttachmentFileName)))
            Else
                Dim attachments() As String = AttachmentFileName.Split(";")
                For i As Integer = 0 To attachments.Length - 1
                    objEmail.Attachments.Add(New Attachment(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output", attachments(i))))
                Next
            End If
        End If

        client.Send(objEmail)
        objEmail.Dispose()

        Return ret
    End Function

    Public Shared Sub SendDebugEmail(ByVal debugInfo As String, ByVal exceptionMessage As String, ByVal stackTrace As String, ByVal pdfDebugInfo As String)

        Dim client As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient("smtp.internal.bonton.com")
        Dim strServer As String = System.Web.HttpContext.Current.Session("ServerLocationText")
        Dim objEmail As New System.Net.Mail.MailMessage
        With objEmail
            .From = New System.Net.Mail.MailAddress(System.Web.HttpContext.Current.Session("EmailFrom"))
            .To.Add(New System.Net.Mail.MailAddress(System.Web.HttpContext.Current.Session("EmailFrom")))
            .Subject = "Sample Request Error" & " - " & strServer
            .Body = debugInfo & vbCrLf & vbCrLf & exceptionMessage & vbCrLf & vbCrLf & stackTrace & IIf(pdfDebugInfo IsNot Nothing, vbCrLf & vbCrLf & pdfDebugInfo, "")
            .IsBodyHtml = False
        End With

        client.Send(objEmail)

    End Sub

End Class
