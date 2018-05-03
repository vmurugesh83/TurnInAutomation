Option Infer On

Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL
Imports Telerik.Web.UI

Public Class GXSCatalogue
    Inherits System.Web.UI.Page

    Private _gxsStyleSkuInfo As IList(Of GXSStyleSkuInfo)
    Private _gxsCatalogInfo As GXSCatalogInfo

    Private ctrlSS_ID As String = "GXSCatalogueStyleSKUPropertiesCtrl1"
    Private ctrlExtd_ID As String = "GXSCatalogueExtendedPropertiesCtrl1"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GetDatasources()
        SetTitle()
        BuildColorTable()
        BuildSizeTable()
        SetCatalogueLink()
        PopulatePropertiesControls()

        If Not Me.Page.IsPostBack Then
            SetProductDetails()
        End If

    End Sub

    Private Sub SetTitle()

        ' TODO: Rob - Description is Label Description + ISN Description
        Me.lblDescription.Text = _gxsStyleSkuInfo(0).Label + "  " + _gxsCatalogInfo.GXSCFG(0).ISN_LONG_DESC

    End Sub

    Private Sub GetDatasources()

        Dim _TUGXStyleSku As New TUGXSStyleSku
        Dim UPC_NUM As Decimal = CDec(Me.Request.QueryString("upc"))
        _gxsStyleSkuInfo = _TUGXStyleSku.GetStyleSKUData(UPC_NUM)

        Dim _TUGXSCatalog As New TUGXSCatalog
        Dim INTERNAL_STYLE_NUM As Decimal = CDec(Me.Request.QueryString("isn"))
        _gxsCatalogInfo = _TUGXSCatalog.GetAllFromTU1135SP(INTERNAL_STYLE_NUM)

    End Sub

    Private Function GroupColors() As IList(Of String)
        Dim colorInfo As New List(Of String)
        If _gxsCatalogInfo.GXSUPC.Count > 0 Then
            Dim colors = From a In _gxsCatalogInfo.GXSUPC _
                         Group a By a.CLR_LONG_DESC Into Group _
                         Order By CLR_LONG_DESC _
                         Select New With {
                            .CLR_LONG_DESC = CLR_LONG_DESC
                         }
            For Each color In colors
                colorInfo.Add(color.CLR_LONG_DESC.Trim)
            Next
        End If
        Return colorInfo
    End Function

    Private Function GroupSizes() As IList(Of String)
        Dim sizeInfo As New List(Of String)
        If _gxsCatalogInfo.GXSUPC.Count > 0 Then
            Dim sizes = From a In _gxsCatalogInfo.GXSUPC _
                         Group a By a.SIZE Into Group _
                         Order By SIZE _
                         Select New With {
                            .SIZE = SIZE
                         }
            For Each size In sizes
                sizeInfo.Add(size.SIZE.Trim)
            Next
        End If
        Return sizeInfo
    End Function

    Private Sub BuildColorTable()

        Dim colorInfo As IList(Of String) = GroupColors()

        Dim sbTable As New StringBuilder
        sbTable.AppendLine("<table>")
        If colorInfo.Count > 0 Then
            Dim colCount As Integer = 4
            Dim rowCount As Integer = CInt(colorInfo.Count \ colCount)

            If colorInfo.Count Mod colCount > 0 Then
                rowCount += 1
            End If

            'Filler column between size columns

            sbTable.AppendLine("     <colgroup>")

            For i As Integer = 0 To colCount - 1
                sbTable.AppendLine("         <col style='width: 125px;' />")
                If i < colCount - 1 Then
                    sbTable.AppendLine("         <col style='width: 5px;' />")
                End If
            Next

            sbTable.AppendLine("     </colgroup>")

            'Set width in <span>
            Dim sbImage As New StringBuilder
            'Dim leftImage As String = "<img src='' alt='' style='width: 100%; height: 15px; background-color: "
            'Dim rightImage As String = ";' />"
            Dim sbSpan As New StringBuilder
            Dim leftSpan As String = "<span style='display: inline-block; width: 125px;'>"
            Dim rightSpan As String = "</span>"
            Dim index As Integer = 0

            For i As Integer = 0 To rowCount - 1

                sbImage.AppendLine("     <tr>")
                sbSpan.AppendLine("     <tr style='font: normal 11px calibri;'>")

                For j As Integer = 1 To colCount
                    If i <= colorInfo.Count - 1 Then
                        sbSpan.Append("         <td>")
                        sbSpan.Append(leftSpan)
                        sbSpan.Append(colorInfo(i))
                        sbSpan.Append(rightSpan)
                        sbSpan.Append("</td>")

                        i += 1
                    End If
                Next

                sbImage.AppendLine("     </tr>")
                sbSpan.AppendLine("     </tr>")

                sbTable.Append(sbImage.ToString)
                sbTable.Append(sbSpan.ToString)

                sbImage.Clear()
                sbSpan.Clear()

            Next
        Else
            sbTable.AppendLine("<tr><td><span style='display: inline-block; width: 125px;'>None</span></td></tr>")
        End If


        sbTable.AppendLine("</table>")

        Me.litAvailColors.Text = sbTable.ToString

    End Sub

    Private Sub BuildSizeTable()

        Dim sizeInfo As IList(Of String) = GroupSizes()
        Dim colCount As Integer = 4
        Dim rowCount As Integer = CInt(sizeInfo.Count \ colCount)

        If sizeInfo.Count Mod colCount > 0 Then
            rowCount += 1
        End If

        'Filler column between size columns
        Dim sb As New StringBuilder
        sb.AppendLine("<table>")
        sb.AppendLine("     <colgroup>")

        For i As Integer = 0 To colCount - 1
            sb.AppendLine("         <col style='width: 125px;' />")
            If i < colCount - 1 Then
                sb.AppendLine("         <col style='width: 5px;' />")
            End If
        Next

        sb.AppendLine("     </colgroup>")

        'Set width in <span>
        Dim leftSpan As String = "<span style='display: inline-block; width: 125px; border: 1px solid black; text-align: center;'>"
        Dim rightSpan As String = "</span>"
        Dim index As Integer = 0

        For i As Integer = 0 To rowCount - 1

            sb.AppendLine("     <tr>")

            For j As Integer = 1 To colCount

                sb.Append("     <td>")
                If index < sizeInfo.Count Then
                    sb.Append(leftSpan)
                    sb.Append(sizeInfo(index))
                    sb.Append(rightSpan)
                End If
                sb.Append("</td>")

                index += 1

            Next

            sb.AppendLine("     </tr>")

        Next

        sb.AppendLine("</table>")

        Me.litAvailSizes.Text = sb.ToString

    End Sub

    Private Sub SetProductDetails()
        Dim ctrl As GXSCatalogueExtendedPropertiesCtrl = CType(Me.phExtendedProperties.FindControl(ctrlExtd_ID), GXSCatalogueExtendedPropertiesCtrl)
        Dim _TUEcommPrioritization As New TUEcommPrioritization
        Me.txtProductDetails.Text = _TUEcommPrioritization.GetWebCatImportProductDetailCopy(CInt(Me.Request.QueryString("imageId")))
    End Sub

    Private Sub UpdateProductDetails()
        Dim ctrl As GXSCatalogueExtendedPropertiesCtrl = CType(Me.phExtendedProperties.FindControl(ctrlExtd_ID), GXSCatalogueExtendedPropertiesCtrl)
        Dim _TUEcommPrioritization As New TUEcommPrioritization
        _TUEcommPrioritization.UpdateWebCatImportProductDetailCopy(CInt(Me.Request.QueryString("imageId")), Me.txtProductDetails.Text)
    End Sub

    Private Sub SetCatalogueLink()

        ' if the catalogue URL is populated, this will be shown else, it will not be shown.
        Me.litCatUrl.Text = "<a href='' target='_blank'>Catalogue URL</a>"
        Me.litCatUrl.Text = ""
    End Sub

    Private Sub PopulatePropertiesControls()

        Dim ctrlSS As Control = LoadUserControl("~/WebUserControls/GXS/GXSCatalogueStyleSKUPropertiesCtrl.ascx", _gxsStyleSkuInfo)
        If Not ctrlSS Is Nothing Then
            ctrlSS.ID = ctrlSS_ID
            Me.phStyleSKUProperties.Controls.Add(ctrlSS)
        End If

        Dim ctrlExtd As Control = LoadUserControl("~/WebUserControls/GXS/GXSCatalogueExtendedPropertiesCtrl.ascx", _gxsCatalogInfo)
        If Not ctrlExtd Is Nothing Then
            ctrlExtd.ID = ctrlExtd_ID
            Me.phExtendedProperties.Controls.Add(ctrlExtd)
        End If

    End Sub

    Private Function LoadUserControl(ByVal UserControlPath As String, ByVal ParamArray constructorParameters As Object()) As UserControl
        Dim constParamTypes As New List(Of Type)()

        For Each constParam As Object In constructorParameters
            constParamTypes.Add(constParam.[GetType]())
        Next

        Dim ctl As UserControl = TryCast(Page.LoadControl(UserControlPath), UserControl)

        Dim constructor As System.Reflection.ConstructorInfo = ctl.[GetType]().BaseType.GetConstructor(constParamTypes.ToArray())
        If constructor Is Nothing Then
            Throw New MemberAccessException("The requested constructor was not found on : " & ctl.[GetType]().BaseType.ToString())
        Else
            constructor.Invoke(ctl, constructorParameters)
        End If

        Return ctl
    End Function

    Private Sub rtbGXSCopyView_ButtonClick(sender As Object, e As RadToolBarEventArgs) Handles rtbGXSCopyView.ButtonClick
        Dim btn As RadToolBarButton = CType(e.Item, RadToolBarButton)
        If btn.CommandName = "Save" Then
            UpdateProductDetails()
            RadAjaxManager.GetCurrent(Page).ResponseScripts.Add("SetMessage('Saved Successfully','Green', 5);")
        End If
    End Sub
End Class