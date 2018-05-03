Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Text
Imports System.ComponentModel
Imports System.Drawing  ' requires the System.Drawing assembly!
Imports Telerik.Web.UI

Namespace PeterBlum.DES.TelerikWebUI


   '******* RADGRID ****************************************************************
   ' ---- class ButtonColumn -----------------------------------------
   ''' <summary>
   ''' A subclass of the radgrid's Telerik.Web.UI.GridButtonColumn 
   ''' that makes registers its buttons with DES.
   ''' </summary>
   Public Class GridButtonColumn
      Inherits Telerik.Web.UI.GridButtonColumn

      Protected fGroup As String = ""

      Protected fCausesValidation As Boolean = False

      Protected fInAJAXUpdate As Boolean = False

      Protected fDisableOnSubmit As Boolean = False

      Protected fMayMoveOnClick As Boolean = False

      Protected fChangeMonitorGroups As String = ""

      Protected fChangeMonitorEnables As Global.PeterBlum.DES.ChangeMonitorEnablesSubmitControl = Global.PeterBlum.DES.ChangeMonitorEnablesSubmitControl.CausesValidationIsTrue

      ' ---- constructors -----------------------------------------------------------
      ''' <summary>
      ''' Constructor
      ''' </summary>
      Public Sub New()
         MyBase.New()

      End Sub

      ' ---- PROPERTIES -------------------------------------------------------------
      ''' <summary>
      ''' Group determines which validators are invoked when clicked.
      ''' </summary>
      ''' <value>
      ''' <para>Those that match the value in this will be run.</para>
      ''' <para>Group names are blank by default. When left blank, this runs all validators
      ''' with the default group.</para>
      ''' <para>You can also use the string "*" to run every validator on the page.</para>
      ''' <para>When the button is shown on multiple rows (naming containers) of a DataGrid or Repeater,
      ''' you can make each row have a unique group name by adding a plus (+) character as the first
      ''' character of the group name. (This is supported for multiple group names with "+groupname|+groupname2".)
      ''' Just be sure to use an identical name in the validators associated with this button.</para>
      ''' <para>The ValidationGroup property does the same thing as the Group property.
      ''' If this is blank, the ValidationGroup property is used. If this is assigned, it is used.</para>
      ''' </value>
      <DefaultValue(""), _
       Category("Behavior"), _
       Description("Runs validators whose Group property matches this one. Use '*' to run all validators on the page. Thi" & _
      "s overrides the ValidationGroup property.")> _
      Public Overridable Property Group() As String
         Get
            Return fGroup
         End Get
         Set(ByVal value As String)
            fGroup = Value
         End Set
      End Property

      ''' <summary>
      ''' Determines if validation is performed when the button is clicked.
      ''' </summary>
      ''' <value>
      ''' <para>When true, validation is performed.</para>
      ''' <para>Defaults to false since the ancestor class also defaults to false. 
      ''' This is to avoid breaking the default condition.</para>
      ''' </value>
      <DefaultValue(False), _
       Category("Behavior"), _
       Description("Determines if validation is performed when the button is clicked.")> _
      Public Overridable Property CausesValidation() As Boolean
         Get
            Return fCausesValidation
         End Get
         Set(ByVal value As Boolean)
            fCausesValidation = Value
         End Set
      End Property

      ''' <summary>
      ''' When true, the control will be part of a block of HTML that is being updated by an AJAX callback.
      ''' </summary>
      ''' <remarks>
      ''' <para>User is required to tell DES which controls need updates so that DES only 
      ''' outputs the HTML and scripts for this specific controls.</para>
      ''' <para>
      ''' It indicates that the OnPreRender method should call AJAXManager.AddScriptToCallback
      ''' and Render method should call AJAXManager.OutputScriptsFromRender.</para>
      ''' </remarks>
      <DefaultValue(False), _
       Description("When true, the control will be part of a block of HTML that is being updated by an AJAX callback."), _
       Category("Behavior")> _
      Public Overridable Property InAJAXUpdate() As Boolean
         Get
            Return fInAJAXUpdate
         End Get
         Set(ByVal value As Boolean)
            fInAJAXUpdate = Value
         End Set
      End Property

      ''' <summary>
      ''' When true, the control will be disabled after the page submits.
      ''' </summary>
      ''' <remarks>
      ''' <para>Required License: Peter's Interactive Pages.</para>
      ''' </remarks>
      ''' <value>
      ''' <para>When true, the control will disable on submit.</para>
      ''' <para>It defaults to false.</para>
      ''' </value>
      <DefaultValue(False), _
       Category("Behavior"), _
       Description("When true, the control will be disabled after the page submits.")> _
      Public Overridable Property DisableOnSubmit() As Boolean
         Get
            Return fDisableOnSubmit
         End Get
         Set(ByVal value As Boolean)
            fDisableOnSubmit = Value
         End Set
      End Property

      ''' <summary>
      ''' If the button requires an extra click to submit the page, its because it jumped
      ''' as the user clicks on it. Set this to true to avoid that extra click.
      ''' </summary>
      ''' <remarks>
      ''' <para>This solves the following problem:</para>
      ''' <para>When the user edits a control and immediately clicks on the button, the onchange event of the 
      ''' control fires, running validation. If validation removes error message and the ValidationSummary,
      ''' the button may jump. This happens before the button's onclick event, preventing that onclick event to run
      ''' because the mouse button is no longer on the button.</para>
      ''' </remarks>
      <DefaultValue(False), _
       Description("If the button requires an extra click to submit the page, its because its position changes as the use" & _
      "r clicks on it. Set this to true to avoid that extra click."), _
       Category("Behavior")> _
      Public Overridable Property MayMoveOnClick() As Boolean
         Get
            Return fMayMoveOnClick
         End Get
         Set(ByVal value As Boolean)
            fMayMoveOnClick = Value
         End Set
      End Property

      ' --- CHANGE MONITOR PROPERTIES --------------------------------------------
      ''' <summary>
      ''' Used by the ChangeMonitor system when its enabled to determine which group or groups
      ''' this submit control is associated with. 
      ''' </summary>
      ''' <remarks>
      ''' <para>The Group property is merged with this value if the ChangeMonitor is set to
      ''' UseValidationGroups=true. This enables validation groups to deliver its names
      ''' avoiding setting this property if the validation groups are appropriate.</para>
      ''' </remarks>
      ''' <value>
      ''' <para>The value of "" is a valid group name.</para>
      ''' <para>For a list of group names, use the pipe character as a delimiter.
      ''' For example: "GroupName1|GroupName2". If one of the groups has the name "",
      ''' start this string with the pipe character: "|GroupName2".</para>
      ''' <para>Use "*" to indicate all groups apply.</para>
      ''' <para>If a group needs to be different based on its naming container,
      ''' use "+" as the first character of the group name.</para>
      ''' </value>
      <DefaultValue(""), _
       Description("Used by the ChangeMonitor system when its enabled to determine which group or groups this submit cont" & _
      "rol is associated with."), _
       Category("Change Monitor")> _
      Public Overridable Property ChangeMonitorGroups() As String
         Get
            Return fChangeMonitorGroups
         End Get
         Set(ByVal value As String)
            fChangeMonitorGroups = Value.Trim
         End Set
      End Property

      ''' <summary>
      ''' Determines if the ChangeMonitor handles enabling and disabling this submit control
      ''' as the changemonitor group is either set or cleared.
      ''' </summary>
      ''' <remarks>
      ''' <para>Required License: Peter's Interactive Pages.</para>
      ''' <para>Typically this is used on buttons whose CausesValidation property is true
      ''' so that a Cancel button will always be active.</para>
      ''' </remarks>
      ''' <value>
      ''' <para>The enumerated type ChangeMonitorEnablesSubmit has these values:</para>
      ''' <para>* No - The button will not change its enable state.</para>
      ''' <para>* Yes - The button will change its enabled state.</para>
      ''' <para>* CausesValidationIsTrue - When the button's CausesValidation property is true, it will change its enabled state.</para>
      ''' <para>* CausesValidationIsFalse - When the button's CausesValidation property is false, it will change its enabled state.</para>
      ''' <para>It defaults to ChangeMonitorEnablesSubmitControl.CausesValidation.</para>
      ''' </value>
      <DefaultValue(Global.PeterBlum.DES.ChangeMonitorEnablesSubmitControl.CausesValidationIsTrue), _
       Description("Determines if the ChangeMonitor handles enabling and disabling this submit control as the changemonit" & _
      "or group is either set or cleared."), _
       Category("Change Monitor")> _
      Public Overridable Property ChangeMonitorEnables() As Global.PeterBlum.DES.ChangeMonitorEnablesSubmitControl
         Get
            Return fChangeMonitorEnables
         End Get
         Set(ByVal value As Global.PeterBlum.DES.ChangeMonitorEnablesSubmitControl)
            fChangeMonitorEnables = value
         End Set
      End Property

      ' ---- METHODS -------------------------------------------------------------
      ''' <summary>
      ''' The ancestor creates the buttons. This registers them with DES's SubmitPageManager
      ''' to provide client-side functionality.
      ''' </summary>
      Public Overrides Sub InitializeCell(ByVal pCell As TableCell, ByVal pColumnIndex As Integer, ByVal pInItem As GridItem)
         MyBase.InitializeCell(pCell, pColumnIndex, pInItem)
         ' creates the buttons
         If (HttpContext.Current Is Nothing) Then
            Return
         End If
         Dim vDESPage As Global.PeterBlum.DES.DESPage = Global.PeterBlum.DES.Globals.Page
         Dim vDeleteID As String = Owner.ID
         Dim vPos As Integer = 0
         Do While (vPos < pCell.Controls.Count)
            Dim vChild As Control = pCell.Controls(vPos)
            If (TypeOf vChild Is IButtonControl) Then
               If ((vChild.ID Is Nothing) _
                           OrElse (vChild.ID.Length = 0)) Then
                  vChild.ID = vDESPage.GetNextControlID
               End If
               Dim vSubmitBehavior As Global.PeterBlum.DES.SubmitBehavior = New Global.PeterBlum.DES.SubmitBehavior(vChild, CausesValidation, Group, ConfirmText, DisableOnSubmit, MayMoveOnClick, InAJAXUpdate, ChangeMonitorGroups, ChangeMonitorEnables)
               vSubmitBehavior.DeleteID = vDeleteID
               vDESPage.SubmitPageManager.RegisterSubmitControl(vSubmitBehavior)
               CType(vChild, WebControl).Attributes.Remove("onclick")
               ' was originally assigned by base class to handle confirmation
            End If
            vPos = (vPos + 1)
         Loop
         ' attach the DataGrid.DataBinding event to DeleteOldButtons method
         If (vDESPage.Items(("GridSubmit" + vDeleteID)) Is Nothing) Then
            vDESPage.Items.Add(("GridSubmit" + vDeleteID), 1)
            AddHandler Owner.DataBinding, AddressOf GridButtonColumn.DeleteOldButtons
         End If
      End Sub

      ' InitializeCell
      ''' <summary>
      ''' An event handler that calls Globals.Page.DeleteOldButtons using the ID of the datagrid
      ''' </summary>
      ''' <param name="pSender"></param>
      ''' <param name="pArgs"></param>
      Public Shared Sub DeleteOldButtons(ByVal pSender As Object, ByVal pArgs As EventArgs)
         Dim cDeleteID As String = CType(pSender, GridTableView).OwnerGrid.ID
         Global.PeterBlum.DES.Globals.Page.SubmitPageManager.DeleteSubmitControls(cDeleteID)
      End Sub
      ' Clone added in v4.0.4
      Public Overrides Function Clone() As Telerik.Web.UI.GridColumn
         Dim vNew As GridButtonColumn = New GridButtonColumn()
         vNew.CopyBaseProperties(Me)
         Return vNew
      End Function
      ' CopyBaseProperties added in v4.0.4
      Protected Overrides Sub CopyBaseProperties(ByVal pFromColumn As Telerik.Web.UI.GridColumn)
         MyBase.CopyBaseProperties(pFromColumn)
         Dim vFromColumn As GridButtonColumn = CType(pFromColumn, GridButtonColumn)
         CausesValidation = vFromColumn.CausesValidation
         ChangeMonitorEnables = vFromColumn.ChangeMonitorEnables
         ChangeMonitorGroups = vFromColumn.ChangeMonitorGroups
         DisableOnSubmit = vFromColumn.DisableOnSubmit
         Group = vFromColumn.Group
         InAJAXUpdate = vFromColumn.InAJAXUpdate
         MayMoveOnClick = vFromColumn.MayMoveOnClick
      End Sub


   End Class

   ' class GridButtonColumn
   ' ---- class GridEditCommandColumn -----------------------------------------
   ''' <summary>
   ''' A subclass of the RadGrid's GridEditCommandColumn 
   ''' that makes registers its buttons with DES.
   ''' </summary>
   Public Class GridEditCommandColumn
      Inherits Telerik.Web.UI.GridEditCommandColumn

      Protected fGroup As String = ""

      Protected fUpdateConfirmText As String = ""

      Protected fCancelConfirmText As String = ""

      Protected fInAJAXUpdate As Boolean = False

      Protected fDisableOnSubmit As Boolean = False

      Protected fMayMoveOnClick As Boolean = False

      Protected fChangeMonitorGroups As String = ""

      Protected fChangeMonitorEnables As Global.PeterBlum.DES.ChangeMonitorEnablesSubmitControl = Global.PeterBlum.DES.ChangeMonitorEnablesSubmitControl.CausesValidationIsTrue

      ' ---- constructors -----------------------------------------------------------
      ''' <summary>
      ''' Constructor
      ''' </summary>
      Public Sub New()
         MyBase.New()

      End Sub

      ' ---- PROPERTIES -------------------------------------------------------------
      ''' <summary>
      ''' Group determines which validators are invoked when clicked.
      ''' </summary>
      ''' <value>
      ''' <para>Those that match the value in this will be run.</para>
      ''' <para>Group names are blank by default. When left blank, this runs all validators
      ''' with the default group.</para>
      ''' <para>You can also use the string "*" to run every validator on the page.</para>
      ''' <para>When the button is shown on multiple rows (naming containers) of a DataGrid or Repeater,
      ''' you can make each row have a unique group name by adding a plus (+) character as the first
      ''' character of the group name. (This is supported for multiple group names with "+groupname|+groupname2".)
      ''' Just be sure to use an identical name in the validators associated with this button.</para>
      ''' </value>
      <DefaultValue(""), _
       Category("Behavior"), _
       Description("Runs validators whose Group property matches this one. Use '*' to run all validators on the page.")> _
      Public Overridable Property Group() As String
         Get
            Return fGroup
         End Get
         Set(ByVal value As String)
            fGroup = Value
         End Set
      End Property

      ''' <summary>
      ''' A confirmation message for the Update button.
      ''' </summary>
      ''' <remarks>
      ''' <para>Required License: Peter's Interactive Pages.</para>
      ''' </remarks>
      <DefaultValue(""), _
       Category("Behavior"), _
       Description("A confirmation message for the Update button. ")> _
      Public Overridable Property UpdateConfirmText() As String
         Get
            Return fUpdateConfirmText
         End Get
         Set(ByVal value As String)
            fUpdateConfirmText = Value.Trim
         End Set
      End Property

      ''' <summary>
      ''' A confirmation message for the Cancel button.
      ''' </summary>
      ''' <remarks>
      ''' <para>Required License: Peter's Interactive Pages.</para>
      ''' </remarks>
      <DefaultValue(""), _
       Category("Behavior"), _
       Description("A confirmation message for the Cancel button. ")> _
      Public Overridable Property CancelConfirmText() As String
         Get
            Return fCancelConfirmText
         End Get
         Set(ByVal value As String)
            fCancelConfirmText = Value.Trim
         End Set
      End Property

      ''' <summary>
      ''' When true, the control will be part of a block of HTML that is being updated by an AJAX callback.
      ''' </summary>
      ''' <remarks>
      ''' <para>User is required to tell DES which controls need updates so that DES only 
      ''' outputs the HTML and scripts for this specific controls.</para>
      ''' <para>
      ''' It indicates that the OnPreRender method should call AJAXManager.AddScriptToCallback
      ''' and Render method should call AJAXManager.OutputScriptsFromRender.</para>
      ''' </remarks>
      <DefaultValue(False), _
       Description("When true, the control will be part of a block of HTML that is being updated by an AJAX callback."), _
       Category("Behavior")> _
      Public Overridable Property InAJAXUpdate() As Boolean
         Get
            Return fInAJAXUpdate
         End Get
         Set(ByVal value As Boolean)
            fInAJAXUpdate = Value
         End Set
      End Property

      ''' <summary>
      ''' When true, the control will be disabled after the page submits.
      ''' </summary>
      ''' <remarks>
      ''' <para>Required License: Peter's Interactive Pages. </para>
      ''' </remarks>
      ''' <value>
      ''' <para>When true, the control will disable on submit.</para>
      ''' <para>It defaults to false.</para>
      ''' </value>
      <DefaultValue(False), _
       Category("Behavior"), _
       Description("When true, the control will be disabled after the page submits.")> _
      Public Overridable Property DisableOnSubmit() As Boolean
         Get
            Return fDisableOnSubmit
         End Get
         Set(ByVal value As Boolean)
            fDisableOnSubmit = value
         End Set
      End Property

      ''' <summary>
      ''' If the button requires an extra click to submit the page, its because it jumped
      ''' as the user clicks on it. Set this to true to avoid that extra click.
      ''' </summary>
      ''' <remarks>
      ''' <para>This solves the following problem:</para>
      ''' <para>When the user edits a control and immediately clicks on the button, the onchange event of the 
      ''' control fires, running validation. If validation removes error message and the ValidationSummary,
      ''' the button may jump. This happens before the button's onclick event, preventing that onclick event to run
      ''' because the mouse button is no longer on the button.</para>
      ''' </remarks>
      <DefaultValue(False), _
       Description("If the button requires an extra click to submit the page, its because its position changes as the use" & _
      "r clicks on it. Set this to true to avoid that extra click."), _
       Category("Behavior")> _
      Public Overridable Property MayMoveOnClick() As Boolean
         Get
            Return fMayMoveOnClick
         End Get
         Set(ByVal value As Boolean)
            fMayMoveOnClick = Value
         End Set
      End Property

      ' --- CHANGE MONITOR PROPERTIES --------------------------------------------
      ''' <summary>
      ''' Used by the ChangeMonitor system when its enabled to determine which group or groups
      ''' this submit control is associated with. 
      ''' ONLY APPLIED TO THE UPDATE BUTTON.
      ''' </summary>
      ''' <remarks>
      ''' <para>The Group property is merged with this value if the ChangeMonitor is set to
      ''' UseValidationGroups=true. This enables validation groups to deliver its names
      ''' avoiding setting this property if the validation groups are appropriate.</para>
      ''' </remarks>
      ''' <value>
      ''' <para>The value of "" is a valid group name.</para>
      ''' <para>For a list of group names, use the pipe character as a delimiter.
      ''' For example: "GroupName1|GroupName2". If one of the groups has the name "",
      ''' start this string with the pipe character: "|GroupName2".</para>
      ''' <para>Use "*" to indicate all groups apply.</para>
      ''' <para>If a group needs to be different based on its naming container,
      ''' use "+" as the first character of the group name.</para>
      ''' </value>
      <DefaultValue(""), _
       Description("Only applied to the Update Button. Used by the ChangeMonitor system when its enabled to determine whi" & _
      "ch group or groups this submit control is associated with."), _
       Category("Change Monitor")> _
      Public Overridable Property ChangeMonitorGroups() As String
         Get
            Return fChangeMonitorGroups
         End Get
         Set(ByVal value As String)
            fChangeMonitorGroups = Value.Trim
         End Set
      End Property

      ''' <summary>
      ''' Determines if the ChangeMonitor handles enabling and disabling this submit control
      ''' as the changemonitor group is either set or cleared.
      ''' ONLY APPLIED TO THE UPDATE BUTTON. 
      ''' </summary>
      ''' <remarks>
      ''' <para>Required License: Peter's Interactive Pages.</para>
      ''' <para>Typically this is used on buttons whose CausesValidation property is true
      ''' so that a Cancel button will always be active.</para>
      ''' </remarks>
      ''' <value>
      ''' <para>The enumerated type ChangeMonitorEnablesSubmit has these values:</para>
      ''' <para>* No - The button will not change its enable state.</para>
      ''' <para>* Yes - The button will change its enabled state.</para>
      ''' <para>* CausesValidationIsTrue - When the button's CausesValidation property is true, it will change its enabled state.</para>
      ''' <para>* CausesValidationIsFalse - When the button's CausesValidation property is false, it will change its enabled state.</para>
      ''' <para>It defaults to ChangeMonitorEnablesSubmitControl.CausesValidation.</para>
      ''' </value>
      <DefaultValue(Global.PeterBlum.DES.ChangeMonitorEnablesSubmitControl.CausesValidationIsTrue), _
       Description("Only applied to the Update Button. Determines if the ChangeMonitor handles enabling and disabling thi" & _
      "s submit control as the changemonitor group is either set or cleared."), _
       Category("Change Monitor")> _
      Public Overridable Property ChangeMonitorEnables() As Global.PeterBlum.DES.ChangeMonitorEnablesSubmitControl
         Get
            Return fChangeMonitorEnables
         End Get
         Set(ByVal value As Global.PeterBlum.DES.ChangeMonitorEnablesSubmitControl)
            fChangeMonitorEnables = value
         End Set
      End Property

      ' ---- METHODS -------------------------------------------------------------
      ''' <summary>
      '''The ancestor creates the buttons. This sets up client-side functionality.
      ''' </summary>
      Public Overrides Sub InitializeCell(ByVal pCell As TableCell, ByVal pColumnIndex As Integer, ByVal pInItem As GridItem)
         MyBase.InitializeCell(pCell, pColumnIndex, pInItem)
         ' creates the buttons
         If (HttpContext.Current Is Nothing) Then
            Return
         End If
         Dim vDESPage As Global.PeterBlum.DES.DESPage = Global.PeterBlum.DES.Globals.Page
         Dim vDeleteID As String = Owner.ID
         Dim vPos As Integer = 0
         Do While (vPos < pCell.Controls.Count)
            Dim vChild As Control = pCell.Controls(vPos)
            If (TypeOf vChild Is IButtonControl) Then
               If ((vChild.ID Is Nothing) _
                           OrElse (vChild.ID.Length = 0)) Then
                  vChild.ID = vDESPage.GetNextControlID
               End If
               ' updatebutton is known by its CommandName property, which was already setup
               Dim vIsUpdateButton As Boolean = ((CType(vChild, IButtonControl).CommandName = "PerformInsert") _
                           OrElse (CType(vChild, IButtonControl).CommandName = "Update"))
               Dim vIsCancelButton As Boolean = (CType(vChild, IButtonControl).CommandName = "Cancel")
               Dim vConfirmMessage As String = ""
               Dim vCME As Global.PeterBlum.DES.ChangeMonitorEnablesSubmitControl
               If vIsUpdateButton Then
                  vConfirmMessage = UpdateConfirmText
                  vCME = Me.ChangeMonitorEnables
               ElseIf vIsCancelButton Then
                  vConfirmMessage = CancelConfirmText
                  vCME = Global.PeterBlum.DES.ChangeMonitorEnablesSubmitControl.No
               End If
               Dim vSubmitBehavior As Global.PeterBlum.DES.SubmitBehavior = New Global.PeterBlum.DES.SubmitBehavior(vChild, vIsUpdateButton, Group, vConfirmMessage, DisableOnSubmit, MayMoveOnClick, InAJAXUpdate, ChangeMonitorGroups, vCME)
               vSubmitBehavior.DeleteID = vDeleteID
               vDESPage.SubmitPageManager.RegisterSubmitControl(vSubmitBehavior)
            End If
            vPos = (vPos + 1)
         Loop
         ' attach the DataGrid.DataBinding event to DeleteOldButtons method
         If (vDESPage.Items(("GridSubmit" + vDeleteID)) Is Nothing) Then
            vDESPage.Items.Add(("GridSubmit" + vDeleteID), 1)
            AddHandler Owner.DataBinding, AddressOf GridButtonColumn.DeleteOldButtons
         End If
      End Sub
      ' Clone function added in v4.0.4
      Public Overrides Function Clone() As Telerik.Web.UI.GridColumn
         Dim vNew As GridEditCommandColumn = New GridEditCommandColumn()
         vNew.CopyBaseProperties(Me)
         Return vNew
      End Function

      ' CopyBaseProperties function added in v4.0.4
      Protected Overrides Sub CopyBaseProperties(ByVal pFromColumn As Telerik.Web.UI.GridColumn)
         MyBase.CopyBaseProperties(pFromColumn)
         Dim vFromColumn As GridEditCommandColumn = CType(pFromColumn, GridEditCommandColumn)
         CancelConfirmText = vFromColumn.CancelConfirmText
         ChangeMonitorEnables = vFromColumn.ChangeMonitorEnables
         ChangeMonitorGroups = vFromColumn.ChangeMonitorGroups
         DisableOnSubmit = vFromColumn.DisableOnSubmit
         Group = vFromColumn.Group
         InAJAXUpdate = vFromColumn.InAJAXUpdate
         MayMoveOnClick = vFromColumn.MayMoveOnClick
         UpdateConfirmText = vFromColumn.UpdateConfirmText
      End Sub

   End Class

   ' class GridEditCommandColumn
   Public Class Globals

      '******* RADMENU ****************************************************************
      ''' <summary>
      ''' Creates DES validation on a MenuItem of the RadMenu, plus any child MenuGroup and its items.
      ''' It will NOT update any MenuItem whose PostBack property is false.
      ''' See the Using radControls User's Guide for usage directions.
      ''' </summary>
      ''' <remarks>
      ''' <para>The MenuItem.PostBack property must be true because validation requires post back to handle
      ''' the fields.</para>
      ''' <para>Uses the MenuItem.OnClientClick property. If that is already in use, it will not assign any validation,
      ''' leaving you to call DES_ValOnSubWGrp('group') from your function.</para>
      ''' <para>It creates one javascript function DES_RadMenuValidate for each validation group by adding
      ''' the group name to the end of the function name.</para>
      ''' </remarks>
      ''' <param name="pMenuItem">The MenuItem to add validation.</param>
      ''' <param name="pConfirmMessage">When using the {SUBMIT} token, you can show this as a confirmation message
      ''' when that token has the confirm parameter.</param>
      ''' <param name="pUpdateChildren">When true, if there is a child group, update all of its children. Even if this MenuItem's
      ''' PostBack property is false, it will still update the children.</param>
      Public Shared Sub PrepareMenuItem(ByVal pMenuItem As Telerik.Web.UI.RadMenuItem, ByVal pConfirmMessage As String, ByVal pUpdateChildren As Boolean)
         If (pMenuItem Is Nothing) Then
            Throw New ArgumentException("Do not pass null for the MenuItem parameter.")
         End If
         If (pMenuItem.Menu Is Nothing) Then
            Throw New ArgumentException("The RadMenuItem object has not been added to a RadMenu control.")
         End If
         If pMenuItem.NavigateUrl.StartsWith("{SUBMIT") Then
            Dim vArgument As String = ""
            ' used in __doPostback(control, argument). Format [index:index:index, etc] where index is the nested level.
            Dim vItem As RadMenuItem = pMenuItem

            While (Not (vItem) Is Nothing)
               If (vArgument.Length = 0) Then
                  vArgument = vItem.Owner.Items.IndexOf(vItem).ToString
               Else
                  vArgument = (vItem.Owner.Items.IndexOf(vItem).ToString + (":" + vArgument))
               End If
               If (TypeOf vItem.Owner Is RadMenuItem) Then
                  vItem = CType(vItem.Owner, RadMenuItem)
               Else
                  vItem = Nothing
               End If

            End While
            ' while
            pMenuItem.NavigateUrl = ("javascript: var vMenu = $find('" _
                        + (pMenuItem.Menu.ClientID + ("'); vMenu.close();" + Global.PeterBlum.DES.Globals.Page.SubmitPageManager.CreateSubmitTokenScript(pMenuItem.Menu, pMenuItem.NavigateUrl, False, vArgument, pConfirmMessage))))
         End If
         If (pUpdateChildren _
                     AndAlso (Not (pMenuItem.Items) Is Nothing)) Then
            For Each vItem As Telerik.Web.UI.RadMenuItem In pMenuItem.Items
               PrepareMenuItem(vItem, pConfirmMessage, True)
            Next
         End If
         ' may cause recursion
      End Sub

      ' PrepareMenuItem
      ''' <summary>
      ''' Creates DES validation on all menu items in the Menu that have the {SUBMIT}
      ''' token in their NavigateUrl property.
      ''' See the Using Third Party Controls s Guide for usage directions.
      ''' </summary>
      ''' <param name="pRadMenu">The RadMenu to update</param>
      ''' <param name="pConfirmMessage">When using the {SUBMIT} token, you can show this as a confirmation message
      ''' when that token has the confirm parameter.</param>
      Public Shared Sub PrepareMenu(ByVal pRadMenu As Telerik.Web.UI.RadMenu, ByVal pConfirmMessage As String)
         If (Not (pRadMenu.Items) Is Nothing) Then
            For Each vItem As Telerik.Web.UI.RadMenuItem In pRadMenu.Items
               PrepareMenuItem(vItem, pConfirmMessage, True)
            Next
         End If
         ' may cause recursion
      End Sub


      '***** MORE SUPPORT FOR RADGRID ****************************************************
      ''' <summary>
      ''' Call in the Page's Init event when using EditMode=EditForms
      ''' and RadGrid.MasterTableView.EditFormSettings.FormTemplate is in use with DES validators.
      ''' </summary>
      ''' <remarks>
      ''' <para>Needs to be called prior RadGrid calls its ItemCreated event.</para>
      ''' </remarks>
      ''' <param name="pRadGrid"></param>
      ''' <param name="pValidationGroup">The validation group. If your validators use
      ''' the + notation in their Group property, use the same + notation here.</param>
      ''' <param name="pConfirmMessage">Optional confirm message. Leave "" for no message.</param>
      Public Shared Sub SetupRadGridForEditForms(ByVal pRadGrid As Telerik.Web.UI.RadGrid, ByVal pValidationGroup As String, ByVal pConfirmMessage As String, ByVal pDisableOnSubmit As Boolean, ByVal pMayMoveOnClick As Boolean, ByVal pInAJAXUpdate As Boolean)
         Global.PeterBlum.DES.Globals.Page.Items.Add(("RGEM" + pRadGrid.UniqueID), New RadGridInEditMode(pRadGrid, pValidationGroup, pConfirmMessage, pDisableOnSubmit, pMayMoveOnClick, pInAJAXUpdate))
      End Sub

      ' SetupRadGridInForEditMode
      ''' <summary>
      ''' Used by SetupRadGridInForEditMode to store the properties and
      ''' provide the ItemCreated event that will eventually use those properties.
      ''' </summary>
      Class RadGridInEditMode

         Private ValidationGroup As String

         Private ConfirmMessage As String

         Private DisableOnSubmit As Boolean

         Private MayMoveOnClick As Boolean

         Private InAJAXUpdate As Boolean

         Public Sub New(ByVal pRadGrid As Telerik.Web.UI.RadGrid, ByVal pValidationGroup As String, ByVal pConfirmMessage As String, ByVal pDisableOnSubmit As Boolean, ByVal pMayMoveOnClick As Boolean, ByVal pInAJAXUpdate As Boolean)
            MyBase.New()
            If (pRadGrid.MasterTableView.EditMode = GridEditMode.EditForms) Or (pRadGrid.MasterTableView.EditMode = GridEditMode.PopUp) Then
               AddHandler pRadGrid.ItemCreated, AddressOf Me.RadGridItemCreated
               ValidationGroup = pValidationGroup
               ConfirmMessage = pConfirmMessage
               DisableOnSubmit = pDisableOnSubmit
               MayMoveOnClick = pMayMoveOnClick
               InAJAXUpdate = pInAJAXUpdate
            End If
         End Sub

         Protected Sub RadGridItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
            If (TypeOf e.Item Is GridEditFormItem) Then
               'the item is in edit mode
               Dim editedItem As GridEditFormItem = CType(e.Item, GridEditFormItem)
               If True Then
                  ' find the ClientID of the UpdateButton. 
                  ' Look for the namingcontainer of the containing row
                  ' and append "UpdateButton"
                  Dim vClientID As String = (editedItem.UniqueID.Replace(Microsoft.VisualBasic.ChrW(36), Microsoft.VisualBasic.ChrW(95)) + "_UpdateButton")
                  ' register that control with DES
                  Global.PeterBlum.DES.Globals.Page.SubmitPageManager.RegisterSubmitControl(New Global.PeterBlum.DES.SubmitBehavior(vClientID, True, "", "", False, False, False))
               End If
               ' if
            End If
            ' if
         End Sub
      End Class

      ' ****** RADAJAXMANAGER ***********************************************
      ''' <summary>
      ''' Call from Application_Start to let RadAJAXManager controls automatically register
      ''' their InAJAXUpdate properties when UsingRadAJAX is called with them.
      ''' </summary>
      Public Shared Sub AutomaticRadAJAXManager()
         AddHandler Global.PeterBlum.DES.AJAXManager.RadAJAXConnections, AddressOf ProcessRadAJAXManager
      End Sub

      ''' <summary>
      ''' For RadAjaxManager controls to set the InAJAXUpdate on all of the controls it will update.
      ''' </summary>
      ''' <param name="pRadAJAXManager"></param>
      Public Shared Sub SetUpdatedControlsInAJAXUpdate(ByVal pRadAJAXManager As Telerik.Web.UI.RadAjaxManager)
         For Each vSetting As AjaxSetting In pRadAJAXManager.AjaxSettings
            For Each vAjaxUpdatedControl As AjaxUpdatedControl In vSetting.UpdatedControls
               Try
                  Dim vControl As Control = pRadAJAXManager.FindControl(vAjaxUpdatedControl.ControlID)
                  If (Not (vControl) Is Nothing) Then
                     If (TypeOf vControl Is Global.PeterBlum.DES.IControlSupportsAJAXPanels) Then
                        CType(vControl, Global.PeterBlum.DES.IControlSupportsAJAXPanels).InAJAXUpdate = True
                        CType(vControl, Global.PeterBlum.DES.IControlSupportsAJAXPanels).InAJAXUpdateModified = Global.PeterBlum.DES.InAJAXUpdateModified.InAJAXContainerControl
                        CType(vControl, Global.PeterBlum.DES.IControlSupportsAJAXPanels).SetChildrenInAJAXUpdate()
                     End If
                  End If
               Catch e As Exception

               End Try
            Next ' foreach
         Next  ' foreach
      End Sub



      Public Shared Sub ProcessRadAJAXManager(ByVal pSender As Object, ByVal pE As EventArgs)
         If (TypeOf pSender Is RadAjaxManager) Then
            SetUpdatedControlsInAJAXUpdate(CType(pSender, RadAjaxManager))
         Else
            Throw New ArgumentException("Must pass a RadAjaxManager control.")
         End If
      End Sub

   End Class
End Namespace