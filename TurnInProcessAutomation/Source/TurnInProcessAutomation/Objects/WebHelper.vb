Imports Telerik.Web.UI


Public NotInheritable Class WebHelper
    ''' <summary>
    ''' Display tool tip on error message, use this function when setting tooltip from usercontrol.
    ''' </summary>
    ''' <param name="controlName"></param>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Public Shared Sub ShowToolTip(ByVal parentControl As Control, ByVal controlName As String, ByVal message As String)
        Dim toolTipControl As RadToolTip = DirectCast(parentControl.FindControl(controlName), RadToolTip)
        toolTipControl.Text = "<font size='2'><b>" + message + "</b></font>"
    End Sub


    ''' <summary>
    ''' Get pink Color
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetPinkColor() As System.Drawing.Color
        Return Drawing.ColorTranslator.FromHtml("#FFC0CB")
    End Function
End Class
