Public Class WebForm1
    Inherits System.Web.UI.Page

    'Private Label As DYMO.Label.Framework.ILabel

    'Public Sub SetupLabelObject()
    '    Label.SetObjectText("BARCODE", "525")
    '    Label.SetObjectText("MerchID", "")
    '    Label.SetObjectText("VendorName", "")
    '    Label.SetObjectText("ItemDesc", "")
    '    Label.SetObjectText("Size", "")
    '    Label.SetObjectText("Figure", "")
    '    Label.SetObjectText("Dept", "")
    '    Label.SetObjectText("Color", "")
    '    Label.SetObjectText("AdNoDesc", "")
    '    Label.SetObjectText("Style", "")
    '    Label.SetObjectText("SysPage", "")
    '    Label.SetObjectText("Page", "")
    '    Label.SetObjectText("UPC", "")
    'End Sub

    'Private Sub WebForm1_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    '    Label = DYMO.Label.Framework.Framework.Open(Server.MapPath("~/PrintLabels/UPC.label"))
    '    If Label IsNot Nothing Then
    '        SetupLabelObject()

    '        Dim printer As DYMO.Label.Framework.IPrinter
    '        printer = DYMO.Label.Framework.Framework.GetPrinters("DYMO LabelWriter 450")
    '        Label.Print(printer)
    '    End If
    'End Sub
End Class