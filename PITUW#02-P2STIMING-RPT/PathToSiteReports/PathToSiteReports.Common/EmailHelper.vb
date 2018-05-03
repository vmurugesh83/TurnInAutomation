Imports Microsoft.VisualBasic
Imports System.Net.Mail
Imports System.IO

Public Class EmailHelper

    Public Shared Function Send(ByVal Recipients As List(Of String), ByVal Sender As String, ByVal Subject As String, ByVal Body As String, ByVal AttachmentFileName As String) As Boolean
        Dim ret As Boolean = True

        Dim client As New SmtpClient("smtp.internal.bonton.com")
        Dim objEmail As New MailMessage
        With objEmail
            .From = New MailAddress(Sender)
            For Each recip As String In Recipients
                .To.Add(New MailAddress(recip))
            Next
            .CC.Add(New MailAddress(Sender))
            .Subject = Subject
            .Body = Body
            .IsBodyHtml = True
        End With

        If Not String.IsNullOrEmpty(AttachmentFileName) Then
            'Use a semicolon separated list for multiple attachments
            If AttachmentFileName.IndexOf(";") = -1 Then
                objEmail.Attachments.Add(New Attachment(AttachmentFileName))
            Else
                Dim attachments() As String = AttachmentFileName.Split(";")
                For i As Integer = 0 To attachments.Length - 1
                    objEmail.Attachments.Add(New Attachment(attachments(i)))
                Next
            End If
        End If

        client.Send(objEmail)
        objEmail.Dispose()

        Return ret
    End Function

End Class
