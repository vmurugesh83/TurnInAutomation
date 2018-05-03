Public Class MessageBox

#Region "Members"

    Private Shared _executingPages As New Hashtable()

#End Region



#Region "Constructor"

    Private Sub New()
    End Sub

#End Region



#Region "Methods"

    Public Shared Sub Show(ByVal sMessage As String)
        ' If this is the first time a page has called this method then
        If Not _executingPages.Contains(HttpContext.Current.Handler) Then
            ' Attempt to cast HttpHandler as a Page.
            Dim executingPage As Page = TryCast(HttpContext.Current.Handler, Page)

            If executingPage IsNot Nothing Then
                ' Create a Queue to hold one or more messages.
                Dim messageQueue As New Queue()

                ' Add our message to the Queue
                messageQueue.Enqueue(sMessage)

                ' Add our message queue to the hash table. Use our page reference
                ' (IHttpHandler) as the key.
                _executingPages.Add(HttpContext.Current.Handler, messageQueue)

                ' Wire up Unload event so that we can inject 
                ' some JavaScript for the alerts.
                AddHandler executingPage.Unload, AddressOf ExecutingPage_Unload
            End If
        Else
            ' If were here then the method has allready been 
            ' called from the executing Page.
            ' We have allready created a message queue and stored a
            ' reference to it in our hastable. 
            Dim queue As Queue = DirectCast(_executingPages(HttpContext.Current.Handler), Queue)

            ' Add our message to the Queue
            queue.Enqueue(sMessage)
        End If
    End Sub

    ' Our page has finished rendering so lets output the
    ' JavaScript to produce the alert's
    Private Shared Sub ExecutingPage_Unload(ByVal sender As Object, ByVal e As EventArgs)
        ' Get our message queue from the hashtable
        Dim queue As Queue = DirectCast(_executingPages(HttpContext.Current.Handler), Queue)

        If queue IsNot Nothing Then
            Dim sb As New StringBuilder()

            ' How many messages have been registered?
            Dim iMsgCount As Integer = queue.Count

            ' Use StringBuilder to build up our client slide JavaScript.
            sb.Append("<script language='javascript'>")

            ' Loop round registered messages
            Dim sMsg As String
            While System.Math.Max(System.Threading.Interlocked.Decrement(iMsgCount), iMsgCount + 1) > 0
                sMsg = DirectCast(queue.Dequeue(), String)
                sMsg = sMsg.Replace("" & Chr(10) & "", "\n")
                sMsg = sMsg.Replace("""", "'")
                sb.Append("alert( """ + sMsg + """ );")
            End While

            ' Close our JS
            sb.Append("</script>")

            ' Were done, so remove our page reference from the hashtable
            _executingPages.Remove(HttpContext.Current.Handler)

            ' Write the JavaScript to the end of the response stream.
            HttpContext.Current.Response.Write(sb.ToString())
        End If
    End Sub


#End Region



End Class

