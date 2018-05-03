
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL
Imports Telerik.Web.UI

Public Class GXSImageViewerCtrl
    Inherits System.Web.UI.UserControl

    Private _gxsImageInfo As IList(Of GXSImageInfo)
    Private axM As RadAjaxManager

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '*** The WebUserControls\GXS\Images folder Should be deleted once the below line of code gets replaced by Image URL's.
        '*** During cleanup, the color images can be deleted from the WebUserControls/GXS/Images/ folder, but don't delete the previous and next images for the RadRotator. They should be moved.
        'XmlDataSource1.Data = "<Data><DataItem><Image>blue.png</Image></DataItem><DataItem><Image>red.png</Image></DataItem><DataItem><Image>yellow.png</Image></DataItem><DataItem><Image>green.png</Image></DataItem><DataItem><Image>orange.png</Image></DataItem></Data>"
        '
        GetImageURLs()

    End Sub

    Private Sub GetImageURLs()

        Dim sb As New StringBuilder
        Dim _TUGXSImage As New TUGXSImage
        Dim UPC_NUM As Decimal = CDec(Me.Request.QueryString("upc"))
        _gxsImageInfo = _TUGXSImage.GetImageData(UPC_NUM)

        If _gxsImageInfo.Count > 0 Then
            sb.Append("<Data>")

            For Each imageInfo As GXSImageInfo In _gxsImageInfo
                sb.Append("<DataItem><SmallURL>")
                sb.Append(imageInfo.SmallURL)
                sb.Append("</SmallURL><LargeURL>")
                sb.Append(imageInfo.LargeURL)
                sb.Append("</LargeURL></DataItem>")
            Next

            sb.Append("</Data>")

            XmlDataSource1.Data = sb.ToString

            Image1.ImageUrl = _gxsImageInfo(0).LargeURL

        Else

            XmlDataSource1.Data = "<Data><DataItem><SmallURL></SmallURL><LargeURL></LargeURL></DataItem></Data>"

            Image1.ImageUrl = "~\Images\noimagefound_690x460.jpg"

        End If

    End Sub

End Class