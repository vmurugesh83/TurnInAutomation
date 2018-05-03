Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL.MiscConstants
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL.Enumerations
Public Class WebCategories
    'Inherits System.Web.UI.Page
    Inherits PageBase
    Dim _TUWebCat As New TUWebCat
    Dim _TUECommTurnInMeetResults As New TUECommTurnInMeetResults
    Dim KEY As String = ""

    Dim isn As Decimal
#Region "Properties"
    Public ReadOnly Property AllWebCats As List(Of WebCat)
        Get
            If Application("ApplWebCatsObject") Is Nothing Then
                Dim objGetApplicationObjectsService As New GetGlobalObjectsService
                objGetApplicationObjectsService.GetAllApplicationObjects()  'this will create/populate all application objects.
            End If
            Return DirectCast(Application("ApplWebCatsObject"), List(Of WebCat))
        End Get
    End Property

    Public Property SelectedWebCats() As List(Of WebCat)
        Get
            If Session("WebCategories.SelectedWebCats") Is Nothing Then
                Session("WebCategories.SelectedWebCats") = New List(Of WebCat)
            End If
            Return CType(Session("WebCategories.SelectedWebCats"), List(Of WebCat))
        End Get
        Set(value As List(Of WebCat))
            Session("WebCategories.SelectedWebCats") = value
        End Set
    End Property

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("ID") Is Nothing Then
            KEY = Request.QueryString("ID")
            isn = CDec(KEY)

            If isn > 0 Then
                rtbeCommWebCat.Items(0).Visible = True
                rtbeCommWebCat.Items(1).Visible = False
            Else
                rtbeCommWebCat.Items(0).Visible = False
                rtbeCommWebCat.Items(1).Visible = True
            End If
        End If

        If Not Page.IsPostBack Then
            BindWebCatData()
        End If
    End Sub
    ''' <summary>
    '''  Save Button from the Meeting Page (shows a warning when window is closed)
    ''' Add Button from the Prioritization Page (returns values back to the page)
    ''' </summary>
    ''' <param name="sender">ToolBar Button</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rtbeCommWebCat_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles rtbeCommWebCat.ButtonClick
        If TypeOf e.Item Is RadToolBarButton Then
            Dim radToolBarButton As RadToolBarButton = DirectCast(e.Item, RadToolBarButton)
            Dim WebCatCode As Integer = 0
            Dim WebCatDesc As String = ""
            Select Case radToolBarButton.CommandName
                Case "Save"
                    SaveWebCategories(isn)
                    ScriptManager.RegisterStartupScript(Page, GetType(Page), "CloseScript", "ClientClose('');", True)
                Case "Add"
                    If SelectedWebCats.Count > 0 Then
                        WebCatCode = SelectedWebCats.Find(Function(x) x.DefaultCategoryFlag = True).CategoryCode
                        WebCatDesc = SelectedWebCats.Find(Function(x) x.DefaultCategoryFlag = True).CategoryLongDesc
                        Dim script As String = "returnValues(""" & WebCatCode & """ , """ & WebCatDesc & """);"
                        ScriptManager.RegisterStartupScript(Page, GetType(Page), "CloseScript", script, True)
                    End If
            End Select
        End If
    End Sub
    Private Sub SaveWebCategories(ByVal isn As Decimal)
        Try
            If isn > 0 Then
                _TUECommTurnInMeetResults.InsertWebcat(isn, SelectedWebCats, SessionWrapper.UserID)
                'BindWebCatData()
                'Else
                '    Session("WebCategories.PrimaryWebCat") = SelectedWebCats
            End If
        Catch ex As Exception
            Throw ex

        End Try

    End Sub
    Private Sub BindWebCatData()
        Try
            BindWebCat(cmbWebCategoriesLevel1, "0")
            DisableWebCatSelections(cmbWebCategoriesLevel2)
            DisableWebCatSelections(cmbWebCategoriesLevel3)
            DisableWebCatSelections(cmbWebCategoriesLevel4)
            DisableWebCatSelections(cmbWebCategoriesLevel5)
            DisableWebCatSelections(cmbWebCategoriesLevel6)
            cmbWebCategoriesLevel1.Focus()
            ' BindWebCategoriesGrid()
            SelectedWebCats.Clear()
            SelectedWebCats = _TUWebCat.GetWebCatByISN(isn).ToList
            SelectedWebCats = SelectedWebCats.Join(AllWebCats, Function(x) x.CategoryCode, Function(y) y.CategoryCode, Function(x, y) New WebCat With {.CategoryCode = x.CategoryCode, .DefaultCategoryFlag = x.DefaultCategoryFlag, .CategoryLongDesc = y.CategoryLongDesc}).ToList

            BindWebCategoriesGrid()
            EnableDisableWebCatAddButtons()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub
    Public Sub ttvWebCat_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Try
            Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
            If grdWebCategories.Items.Count = 0 Then
                validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1014)
                args.IsValid = False
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub imgAddWebCategoriesLevel1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgAddWebCategoriesLevel1.Click
        Try
            If cmbWebCategoriesLevel1.Enabled = True AndAlso cmbWebCategoriesLevel1.SelectedItem.Text <> "" Then
                AddWebCat(CInt(cmbWebCategoriesLevel1.SelectedValue), cmbWebCategoriesLevel1.SelectedItem.Text)
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub imgAddWebCategoriesLevel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgAddWebCategoriesLevel2.Click
        Try
            If cmbWebCategoriesLevel2.Enabled = True AndAlso cmbWebCategoriesLevel2.SelectedItem.Text <> "" Then
                AddWebCat(CInt(cmbWebCategoriesLevel2.SelectedValue), cmbWebCategoriesLevel1.SelectedItem.Text & " > " & cmbWebCategoriesLevel2.SelectedItem.Text)
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub imgAddWebCategoriesLevel3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgAddWebCategoriesLevel3.Click
        Try
            If cmbWebCategoriesLevel3.Enabled = True AndAlso cmbWebCategoriesLevel3.SelectedItem.Text <> "" Then
                AddWebCat(CInt(cmbWebCategoriesLevel3.SelectedValue), cmbWebCategoriesLevel1.SelectedItem.Text & " > " & cmbWebCategoriesLevel2.SelectedItem.Text & " > " & cmbWebCategoriesLevel3.SelectedItem.Text)
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub imgAddWebCategoriesLevel4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgAddWebCategoriesLevel4.Click
        Try
            If cmbWebCategoriesLevel4.Enabled = True AndAlso cmbWebCategoriesLevel4.SelectedItem.Text <> "" Then
                AddWebCat(CInt(cmbWebCategoriesLevel4.SelectedValue), cmbWebCategoriesLevel1.SelectedItem.Text & " > " & cmbWebCategoriesLevel2.SelectedItem.Text & " > " & cmbWebCategoriesLevel3.SelectedItem.Text & " > " & cmbWebCategoriesLevel4.SelectedItem.Text)
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub imgAddWebCategoriesLevel5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgAddWebCategoriesLevel5.Click
        Try
            If cmbWebCategoriesLevel5.Enabled = True AndAlso cmbWebCategoriesLevel5.SelectedItem.Text <> "" Then
                AddWebCat(CInt(cmbWebCategoriesLevel5.SelectedValue), cmbWebCategoriesLevel1.SelectedItem.Text & " > " & cmbWebCategoriesLevel2.SelectedItem.Text & " > " & cmbWebCategoriesLevel3.SelectedItem.Text & " > " & cmbWebCategoriesLevel4.SelectedItem.Text & " > " & cmbWebCategoriesLevel5.SelectedItem.Text)
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub imgAddWebCategoriesLevel6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgAddWebCategoriesLevel6.Click
        Try
            If cmbWebCategoriesLevel6.Enabled = True AndAlso cmbWebCategoriesLevel6.SelectedItem.Text <> "" Then
                AddWebCat(CInt(cmbWebCategoriesLevel6.SelectedValue), cmbWebCategoriesLevel1.SelectedItem.Text & " > " & cmbWebCategoriesLevel2.SelectedItem.Text & " > " & cmbWebCategoriesLevel3.SelectedItem.Text & " > " & cmbWebCategoriesLevel4.SelectedItem.Text & " > " & cmbWebCategoriesLevel5.SelectedItem.Text & " > " & cmbWebCategoriesLevel6.SelectedItem.Text)
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub AddWebCat(ByVal cmbValue As Integer, ByVal cmbText As String)
        Try
            If SelectedWebCats.Count < 6 Then
                Dim webcat As New WebCat
                webcat.CategoryCode = cmbValue
                webcat.CategoryLongDesc = cmbText.Replace(" (Display Only)", "")
                If SelectedWebCats.Where(Function(x) x.DefaultCategoryFlag = True).Count = 0 Then
                    webcat.DefaultCategoryFlag = True
                End If

                If SelectedWebCats.Find(Function(x) x.CategoryCode = webcat.CategoryCode) Is Nothing Then
                    SelectedWebCats.Add(webcat)
                End If
            Else
                MessagePanel1.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1012)
            End If
            BindWebCategoriesGrid()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Protected Sub cbDefaultCategoryFlag_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim rb As RadioButton = CType(sender, RadioButton)
            Dim cc As Integer = CInt(CType(rb.NamingContainer, GridDataItem).GetDataKeyValue("CategoryCode"))
            SelectedWebCats.Find(Function(x) x.DefaultCategoryFlag = True).DefaultCategoryFlag = False
            SelectedWebCats.Find(Function(x) x.CategoryCode = cc).DefaultCategoryFlag = True
            BindWebCategoriesGrid()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub BindWebCategoriesGrid()
        Try
            grdWebCategories.DataSource = SelectedWebCats
            grdWebCategories.DataBind()
            'grdWebCategories.MasterTableView.DataKeyNames = New String() {"CategoryCode"}
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub
    Private Sub grdWebCategories_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdWebCategories.DeleteCommand
        Try
            Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim cc As Integer = CInt(item.GetDataKeyValue("CategoryCode"))
            If SelectedWebCats.Count > 1 And SelectedWebCats.Find(Function(x) x.CategoryCode = cc And x.DefaultCategoryFlag = True) IsNot Nothing Then
                MessagePanel1.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1011)
                e.Canceled = True
            Else
                SelectedWebCats.RemoveAll(Function(x) x.CategoryCode = cc)
                BindWebCategoriesGrid()
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub
    Private Sub cmbWebCategoriesLevel1_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbWebCategoriesLevel1.SelectedIndexChanged
        Try
            If cmbWebCategoriesLevel1.SelectedIndex > 0 Then
                BindWebCat(cmbWebCategoriesLevel2, cmbWebCategoriesLevel1.SelectedValue)
            Else
                DisableWebCatSelections(cmbWebCategoriesLevel2)
            End If
            DisableWebCatSelections(cmbWebCategoriesLevel3)
            DisableWebCatSelections(cmbWebCategoriesLevel4)
            DisableWebCatSelections(cmbWebCategoriesLevel5)
            DisableWebCatSelections(cmbWebCategoriesLevel6)
            EnableDisableWebCatAddButtons()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub cmbWebCategoriesLevel2_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbWebCategoriesLevel2.SelectedIndexChanged
        Try
            If cmbWebCategoriesLevel2.SelectedIndex > 0 Then
                BindWebCat(cmbWebCategoriesLevel3, cmbWebCategoriesLevel2.SelectedValue)
            Else
                DisableWebCatSelections(cmbWebCategoriesLevel3)
            End If
            DisableWebCatSelections(cmbWebCategoriesLevel4)
            DisableWebCatSelections(cmbWebCategoriesLevel5)
            DisableWebCatSelections(cmbWebCategoriesLevel6)
            EnableDisableWebCatAddButtons()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub cmbWebCategoriesLevel3_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbWebCategoriesLevel3.SelectedIndexChanged
        Try
            If cmbWebCategoriesLevel3.SelectedIndex > 0 Then
                BindWebCat(cmbWebCategoriesLevel4, cmbWebCategoriesLevel3.SelectedValue)
            Else
                DisableWebCatSelections(cmbWebCategoriesLevel4)
            End If
            DisableWebCatSelections(cmbWebCategoriesLevel5)
            DisableWebCatSelections(cmbWebCategoriesLevel6)
            EnableDisableWebCatAddButtons()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub cmbWebCategoriesLevel4_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbWebCategoriesLevel4.SelectedIndexChanged
        Try
            If cmbWebCategoriesLevel4.SelectedIndex > 0 Then
                BindWebCat(cmbWebCategoriesLevel5, cmbWebCategoriesLevel4.SelectedValue)
            Else
                DisableWebCatSelections(cmbWebCategoriesLevel5)
            End If
            DisableWebCatSelections(cmbWebCategoriesLevel6)
            EnableDisableWebCatAddButtons()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub cmbWebCategoriesLevel5_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbWebCategoriesLevel5.SelectedIndexChanged
        Try
            If cmbWebCategoriesLevel5.SelectedIndex > 0 Then
                BindWebCat(cmbWebCategoriesLevel6, cmbWebCategoriesLevel5.SelectedValue)
            Else
                DisableWebCatSelections(cmbWebCategoriesLevel6)
            End If
            EnableDisableWebCatAddButtons()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub cmbWebCategoriesLevel6_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbWebCategoriesLevel6.SelectedIndexChanged
        Try
            EnableDisableWebCatAddButtons()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub BindWebCat(ByVal cmb As RadComboBox, ByVal SelectedValue As String)
        Try
            Dim list As IList(Of WebCat) = _TUWebCat.GetWebCatByParentCde(CInt(SelectedValue))
            If list.Count > 0 Then
                With cmb
                    .Text = ""
                    .ClearSelection()
                    .DataSource = list
                    .DataTextField = "CategoryNameDisplayOnlyText"
                    .DataValueField = "CategoryCode"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem())
                    .Enabled = True
                End With
            Else
                cmb.Enabled = False
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub DisableWebCatSelections(ByVal cmb As RadComboBox)
        Try
            With cmb
                .Text = ""
                .ClearSelection()
                .Enabled = False
                .DataSource = ""
                .DataBind()
            End With
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub
    Private Sub EnableDisableWebCatAddButtons()
        Try
            imgAddWebCategoriesLevel1.Visible = cmbWebCategoriesLevel1.Enabled AndAlso (cmbWebCategoriesLevel1.Items.Count - 1 > cmbWebCategoriesLevel1.Items.Where(Function(x) x.Text.ToLower.Contains("display only")).Count) AndAlso cmbWebCategoriesLevel1.SelectedIndex > 0 AndAlso Not cmbWebCategoriesLevel1.SelectedItem.Text.ToLower.Contains("display only")
            imgAddWebCategoriesLevel2.Visible = cmbWebCategoriesLevel2.Enabled AndAlso (cmbWebCategoriesLevel2.Items.Count - 1 > cmbWebCategoriesLevel2.Items.Where(Function(x) x.Text.ToLower.Contains("display only")).Count) AndAlso cmbWebCategoriesLevel2.SelectedIndex > 0 AndAlso Not cmbWebCategoriesLevel2.SelectedItem.Text.ToLower.Contains("display only")
            imgAddWebCategoriesLevel3.Visible = cmbWebCategoriesLevel3.Enabled AndAlso (cmbWebCategoriesLevel3.Items.Count - 1 > cmbWebCategoriesLevel3.Items.Where(Function(x) x.Text.ToLower.Contains("display only")).Count) AndAlso cmbWebCategoriesLevel3.SelectedIndex > 0 AndAlso Not cmbWebCategoriesLevel3.SelectedItem.Text.ToLower.Contains("display only")
            imgAddWebCategoriesLevel4.Visible = cmbWebCategoriesLevel4.Enabled AndAlso (cmbWebCategoriesLevel4.Items.Count - 1 > cmbWebCategoriesLevel4.Items.Where(Function(x) x.Text.ToLower.Contains("display only")).Count) AndAlso cmbWebCategoriesLevel4.SelectedIndex > 0 AndAlso Not cmbWebCategoriesLevel4.SelectedItem.Text.ToLower.Contains("display only")
            imgAddWebCategoriesLevel5.Visible = cmbWebCategoriesLevel5.Enabled AndAlso (cmbWebCategoriesLevel5.Items.Count - 1 > cmbWebCategoriesLevel5.Items.Where(Function(x) x.Text.ToLower.Contains("display only")).Count) AndAlso cmbWebCategoriesLevel5.SelectedIndex > 0 AndAlso Not cmbWebCategoriesLevel5.SelectedItem.Text.ToLower.Contains("display only")
            imgAddWebCategoriesLevel6.Visible = cmbWebCategoriesLevel6.Enabled AndAlso (cmbWebCategoriesLevel6.Items.Count - 1 > cmbWebCategoriesLevel6.Items.Where(Function(x) x.Text.ToLower.Contains("display only")).Count) AndAlso cmbWebCategoriesLevel6.SelectedIndex > 0 AndAlso Not cmbWebCategoriesLevel6.SelectedItem.Text.ToLower.Contains("display only")
            'Alert subcategories
            Dim imgUrl As String = "/Images/Alert.gif"
            If imgAddWebCategoriesLevel1.Visible AndAlso cmbWebCategoriesLevel2.Enabled Then
                imgAddWebCategoriesLevel1.Image.ImageUrl = imgUrl
            End If
            If imgAddWebCategoriesLevel2.Visible AndAlso cmbWebCategoriesLevel3.Enabled Then
                imgAddWebCategoriesLevel2.Image.ImageUrl = imgUrl
            End If
            If imgAddWebCategoriesLevel3.Visible AndAlso cmbWebCategoriesLevel4.Enabled Then
                imgAddWebCategoriesLevel3.Image.ImageUrl = imgUrl
            End If
            If imgAddWebCategoriesLevel4.Visible AndAlso cmbWebCategoriesLevel5.Enabled Then
                imgAddWebCategoriesLevel4.Image.ImageUrl = imgUrl
            End If
            If imgAddWebCategoriesLevel5.Visible AndAlso cmbWebCategoriesLevel6.Enabled Then
                imgAddWebCategoriesLevel5.Image.ImageUrl = imgUrl
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

End Class