<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TurnInQueryCtrl.ascx.vb"
    Inherits="TurnInProcessAutomation.TurnInQueryCtrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<br />
<telerik:RadAjaxManagerProxy ID="rampTurnInQueryCtrl" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="tblTurnInQueryCtrl">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="tblTurnInQueryCtrl" LoadingPanelID="ralpLoadPnl" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="rblViewType">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rblViewType" />
                <telerik:AjaxUpdatedControl ControlID="tblSQfields" />
                <telerik:AjaxUpdatedControl ControlID="tblPMfields" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbAdNoPM">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbAdNoPM" />
                <telerik:AjaxUpdatedControl ControlID="rcbPageNumber" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbDept">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbDept" />
                <telerik:AjaxUpdatedControl ControlID="cmbVendorStyle" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbBuyer">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbBuyer" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbVendorStyle">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbVendorStyle" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbVendor">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbVendor" />
                <telerik:AjaxUpdatedControl ControlID="cmbVendorStyle" />
            </UpdatedControls>
        </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="cmbCRG">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbCRG" />
                <telerik:AjaxUpdatedControl ControlID="cmbCMG" />
                <telerik:AjaxUpdatedControl ControlID="cmbCFG" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>
<%--<telerik:RadPanelBar runat="server" ID="rpbQT" Height="400" Width="100%" ExpandMode="FullExpandedItem">
    <Items>
        <telerik:RadPanelItem Expanded="True" Text="Main Filter">
            <Items>
                <telerik:RadPanelItem>
                    <ItemTemplate>--%>
<table id="tblTurnInQueryCtrl" runat="server" cellpadding="1" style="width: 80%;">
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblView" runat="server" class="smallLabel" Text="View:" />
        </td>
        <td width="70%">
            <asp:RadioButtonList runat="server" ID="rblViewType" RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="true">
                <asp:ListItem Value="1" Selected="True">Standard</asp:ListItem>
                <asp:ListItem Value="2">Pre-Media</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td>
            <br />
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblCRG" runat="server" class="smallLabel" Text="CRG:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbCRG" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" Filter="StartsWith" 
                Width="180" TabIndex="1" DropDownWidth="276" OnItemsRequested="cmbCRG_ItemsRequested"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"  AutoPostBack="true">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <HeaderTemplate>
                    <table style="width: 250px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                Id
                            </td>
                            <td style="width: 200px;">
                                Desc
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 250px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                <%# Eval("CRG_ID")%>
                            </td>
                            <td style="width: 200px; text-align: left">
                                <%# Eval("CRG_DSC")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblCMG" runat="server" class="smallLabel" Text="CMG:" />
        </td>
        <td width="50%">
            <telerik:RadComboBox ID="cmbCMG" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" Filter="StartsWith" 
                Width="180" TabIndex="2" DropDownWidth="276" OnItemsRequested="cmbCMG_ItemsRequested"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" AutoPostBack = "true">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <HeaderTemplate>
                    <table style="width: 250px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                Id
                            </td>
                            <td style="width: 200px;">
                                Desc
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 250px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                <%# Eval("CMG_ID")%>
                            </td>
                            <td style="width: 200px; text-align: left">
                                <%# Eval("CMG_DESC")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
            <bonton:ToolTipValidator ID="ToolTipValidator1" runat="server" ControlToEvaluate="cmbCMG"
                ValidationGroup="Validate" />
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblCFG" runat="server" class="smallLabel" Text="CFG:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbCFG" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" Filter="StartsWith" 
                Width="180" TabIndex="3" DropDownWidth="326" OnItemsRequested="cmbCFG_ItemsRequested"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <HeaderTemplate>
                    <table style="width: 300px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                Id
                            </td>
                            <td style="width: 250px;">
                                Desc
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 300px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                <%# Eval("CFG_ID")%>
                            </td>
                            <td style="width: 250px; text-align: left">
                                <%# Eval("CFG_DESC")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
            <bonton:ToolTipValidator ID="ToolTipValidator2" runat="server" ControlToEvaluate="cmbCFG"
                ValidationGroup="Validate" />
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblBuyer" runat="server" class="smallLabel" Text="Buyer:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbBuyer" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                Filter="StartsWith"
                class="RadComboBox_Vista" AllowCustomText="false" Width="180" TabIndex="4" DropDownWidth="276"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <HeaderTemplate>
                    <table style="width: 250px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                Id
                            </td>
                            <td style="width: 200px;">
                                Name
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 250px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                <%# Eval("BuyerId")%>
                            </td>
                            <td style="width: 200px; text-align: left">
                                <%# Eval("BuyerName")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
            <bonton:ToolTipValidator ID="ToolTipValidator3" runat="server" ControlToEvaluate="cmbBuyer"
                ValidationGroup="Validate" />
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblFOB" runat="server" class="smallLabel" Text="FOB:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbFOB" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                OnItemsRequested="cmbFOB_ItemsRequested" class="RadComboBox_Vista" AllowCustomText="false" Filter="StartsWith"
                AppendDataBoundItems="True" Width="180" TabIndex="5" DropDownWidth="326"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <HeaderTemplate>
                    <table style="width: 300px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                Id
                            </td>
                            <td style="width: 250px;">
                                Desc
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 300px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                <%# Eval("FOB_ID")%>
                            </td>
                            <td style="width: 250px; text-align: left">
                                <%# Eval("FOB_DESC")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblDept" runat="server" class="smallLabel" Text="Dept:" />
        </td>
        <td width="90%">
            <telerik:RadComboBox ID="cmbDept" runat="server" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" OnItemsRequested="cmbDept_ItemsRequested"
                OnSelectedIndexChanged="cmbDept_SelectedIndexChanged" Width="180" DropDownWidth="226" Filter="StartsWith"
                AutoPostBack="true" EnableLoadOnDemand="true" ShowMoreResultsBox="false" EnableVirtualScrolling="false"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" TabIndex="6">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <HeaderTemplate>
                    <table style="width: 200px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 200px;">
                                ID - Desc
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 200px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 200px; text-align: left">
                                <%# Eval("DeptIdDesc")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblClass" runat="server" class="smallLabel" Text="Class:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbClass" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="false" Filter="StartsWith"
                Enabled="false" Width="180" DropDownWidth="226" OnItemsRequested="cmbClass_ItemsRequested"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" TabIndex="7">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <HeaderTemplate>
                    <table style="width: 200px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 200px;">
                                ID - Desc
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 200px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 200px; text-align: left">
                                <%# Eval("ClassIdDesc")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblACode" runat="server" class="smallLabel" Text="A-Code" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbACode" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="false" Filter="StartsWith"
                Enabled="false" Width="180" DropDownWidth="326" OnItemsRequested="cmbACode_ItemsRequested"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" TabIndex="8">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <HeaderTemplate>
                    <table style="width: 300px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 300px;">
                                Acode - Desc
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 300px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 300px; text-align: left">
                                <%# Eval("ACodeCompoundDesc")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblVendor1" runat="server" class="smallLabel" Text="Vendor: " />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbVendor" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="false" Filter="StartsWith"
                Enabled="false" Width="180" DropDownWidth="376" OnItemsRequested="cmbVendor_ItemsRequested"
                AutoPostBack="true" ClientSelectedIndexChanged="OnClientSelectedIndexChanged" TabIndex="9">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <HeaderTemplate>
                    <table style="width: 350px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 350px;">
                                ID - Name
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 350px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 350px; text-align: left">
                                <%# Eval("VendorIdName")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-left: 5px">
            <asp:Label ID="lblVendorStyle" runat="server" class="smallLabel" Text="Vendor Style:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbVendorStyle" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                OnItemsRequested="cmbVendorStyle_ItemsRequested" class="RadComboBox_Vista" 
                AllowCustomText="false" Filter="StartsWith" Width="180" DropDownWidth="200" 
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" TabIndex="10" Enabled="False">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-left: 5px">
            <asp:Label ID="lblTurnInStatus" runat="server" class="smallLabel" Text="Turn-In Status:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbTurnInStatus" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                Width="180" Height="120" TabIndex="12">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <Items>
                    <telerik:RadComboBoxItem Text="" Value=""></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Text="Complete" Value="COMP"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Text="Exported for Internal team" Value="INTR"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Text="Exported to Freelance" Value="EXTR"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Text="Pending" Value="PEND"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Text="Ready for Meeting" Value="RDFM"></telerik:RadComboBoxItem>
                </Items>
            </telerik:RadComboBox>
            <a ID="rbShowDialog" runat="server" href="#">help</a>
            <telerik:RadWindow ID="modalPopup" runat="server" Width="480px" Height="390px" Modal="true">
               <ContentTemplate>
                    <h3 style="padding-left: 10px">Status Definitions</h3>
                   <ul style="padding-right:20px">
                    <li><b>Initial Stage</b> - This should be a temporary status after an ISN is saved and before the actual merchandise is saved on the color/size screen.</li>
                    <li><b>Pending</b> - This is the status that is designated after the merchandise is saved on the color/size screen and before it is submitted for the turn-in meeting.</li>
                    <li><b>Ready For Meeting</b> - Merchant has completed initial data entry and submitted it for the Turn-In meeting.</li>
                    <li><b>Completed</b> - The status assigned after the Creative Coordinator has assured that all data is entered and the meeting is considered complete.</li>
                    <li><b>Exported for Internal team</b> - The status designated within the Query tool upon hitting the Export for Internal (Pre-media) button within the Pre-media view.</li>
                    <li><b>Exported to Freelance</b> - The status designated within the Query tool upon hitting the Export for Free-lance (Pre-media) button within the Pre-media view.</li>
                   </ul>
               </ContentTemplate>
          </telerik:RadWindow>
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-left: 5px">
            <asp:Label ID="lblTurnInType" runat="server" class="smallLabel" Text="Turn-In Type:"  Visible="false"/>
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbTurnInType" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                Width="180" Height="80" OnItemsRequested="cmbTurnInType_ItemsRequested" Visible="false">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-left: 5px">
            <asp:Label ID="Label1" runat="server" class="smallLabel" Text="Turn-In DateFrom:" />
        </td>
        <td width="70%">
            <telerik:RadDatePicker ID="dpTurninFrom" Style="vertical-align: middle;" runat="server"
                Width="180px" TabIndex="13" UseEmbeddedScripts="false">
                <DateInput ID="dITurninFrom" DisplayDateFormat="MM/dd/yyyy" runat="server">
                </DateInput>
                <Calendar ID="calTurninFrom" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                    ViewSelectorText="x" runat="server">
                </Calendar>
                <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
            </telerik:RadDatePicker>
            <asp:Image ID="ImgTurnInFrom" runat="server" ImageUrl="~/Images/Error.png" Visible="False" />
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-left: 5px">
            <asp:Label ID="Label2" runat="server" class="smallLabel" Text="Turn-In DateTo:" />
        </td>
        <td width="70%">
            <telerik:RadDatePicker ID="dpTurninTo" Style="vertical-align: middle;" runat="server"
                Width="180px" TabIndex="14" UseEmbeddedScripts="false">
                <DateInput ID="dITurninTo" DisplayDateFormat="MM/dd/yyyy" runat="server">
                </DateInput>
                <Calendar ID="calTurnInTo" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                    ViewSelectorText="x" runat="server">
                </Calendar>
                <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
            </telerik:RadDatePicker>
            <asp:Image ID="ImgTurnInTo" runat="server" ImageUrl="~/Images/Error.png" Visible="False" />
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-left: 5px"> <asp:Label ID="lblBatch" runat="server" class="smallLabel" Text="Batch Number:" /></td>
        <td width="70%"><asp:TextBox ID="txtBatchNumber" runat="server"></asp:TextBox>
        <bonton:ToolTipValidator ID="ttvBatch" runat="server" ControlToEvaluate="txtBatchNumber" ValidationGroup="Search" /></td>
    </tr>
</table>
<table id="tblSQFields" runat="server" cellpadding="1" style="width: 100%;" visible="true">
    <tr>
        <td align="right" style="padding-left: 5px">
            <asp:Label ID="lblAdNo" runat="server" class="smallLabel" Text="Ad# / Wk# /Date:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbAdNo" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" OnItemsRequested="cmbAdNo_ItemsRequested" Filter="StartsWith"
                Width="180" DropDownWidth="326" EnableLoadOnDemand="true"
                ShowMoreResultsBox="false" EnableVirtualScrolling="false"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" TabIndex="11">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <HeaderTemplate>
                    <table style="width: 300px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                Ad #
                            </td>
                            <td style="width: 250px;">
                                Description
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 300px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                <%# Eval("AdNbr")%>
                            </td>
                            <td style="width: 250px; text-align: left">
                                <%# Eval("AdDesc")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
            <bonton:ToolTipValidator ID="ttvAds" runat="server" ControlToEvaluate="cmbAdNo" ValidationGroup="Search" />
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-left: 5px" width="30%">
            <asp:Label ID="Label3" runat="server" class="smallLabel" Text="Route From Ad:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbRouteFromAd" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false"
                Width="180" Height="60" TabIndex="13">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <Items>
                    <telerik:RadComboBoxItem Value="ALL" Text="ALL" Selected="true"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="WITH" Text="With Routes"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="WITHOUT" Text="Without Routes"></telerik:RadComboBoxItem>
                </Items>
            </telerik:RadComboBox>
        </td>
    </tr>
</table>
<table id="tblPMfields" runat="server" cellpadding="1" style="width: 80%;" visible="false">
    <tr>
        <td align="right" style="padding-left: 5px">
            <asp:Label ID="lblAdNoPM" runat="server" class="smallLabel" Text="Ad# / Wk# /Date:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbAdNoPM" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" OnItemsRequested="cmbAdNoPM_ItemsRequested" Filter="StartsWith"
                Width="180" DropDownWidth="326" AutoPostBack="true" EnableLoadOnDemand="true"
                ShowMoreResultsBox="false" EnableVirtualScrolling="false" OnSelectedIndexChanged="cmbAdNoPM_SelectedIndexChanged"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" TabIndex="11">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <HeaderTemplate>
                    <table style="width: 300px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                Ad #
                            </td>
                            <td style="width: 250px;">
                                Description
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 300px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                <%# Eval("AdNbr")%>
                            </td>
                            <td style="width: 250px; text-align: left">
                                <%# Eval("AdDesc")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
            <bonton:ToolTipValidator ID="ToolTipValidator4" runat="server" ControlToEvaluate="cmbAdNo" ValidationGroup="Search" />
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblPageNo" runat="server" class="smallLabel" Text="Page #:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbPageNumber" runat="server" OnClientBlur="OnClientBlurHandler" Filter="StartsWith"
                DropDownWidth="276" Width="100" Enabled="false" HighlightTemplatedItems="true">
                <HeaderTemplate>
                    <table style="width: 250px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                Page #
                            </td>
                            <td style="width: 200px;">
                                Description
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 250px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                <%# Eval("PgNbr")%>
                            </td>
                            <td style="width: 200px; text-align: left">
                                <%# Eval("PgDesc")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-left: 5px">
            <asp:Label ID="lblInWebCat" runat="server" class="smallLabel" Text="In WebCat:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbInWebCat" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false"
                Width="80" Height="60" TabIndex="13">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <Items>
                    <telerik:RadComboBoxItem Text="" Value=""></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Text="Yes" Value="Y"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Text="No" Value="N"></telerik:RadComboBoxItem>
                </Items>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-left: 5px">
            <asp:Label ID="lblSuffix" runat="server" class="smallLabel" Text="Suffix:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbSuffix" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                OnItemsRequested="cmbSuffix_ItemsRequested" class="RadComboBox_Vista" AllowCustomText="false"
                Width="180" DropDownWidth="250" 
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" TabIndex="10">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-left: 5px">
            <asp:Label ID="lblImageType" runat="server" class="smallLabel" Text="Image Type:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbImageType" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false"
                Width="80" Height="60" TabIndex="13">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                <Items>
                    <telerik:RadComboBoxItem Text="" Value=""></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Text="ON" Value="ON"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Text="OFF" Value="OFF"></telerik:RadComboBoxItem>
                </Items>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-left: 5px">
            <asp:Label ID="lblModelCategory" runat="server" class="smallLabel" Text="Model Category:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbModelCategory" runat="server" Width="180px" DropDownWidth="200" Height="100px"
                MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista"
                OnItemsRequested="rcbModelCategory_ItemsRequested" AllowCustomText="false">
                <CollapseAnimation Duration="200" Type="OutQuint" />
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-left: 5px">
            <asp:Label ID="lblFeatWebCat" runat="server" class="smallLabel" Text="Feature Web Category:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbFeatWebCat" runat="server" Width="180px" DropDownWidth="200" Height="100px"
                MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista"
                OnItemsRequested="rcbFeatWebCat_ItemsRequested" AllowCustomText="false">
                <CollapseAnimation Duration="200" Type="OutQuint" />
            </telerik:RadComboBox>
        </td>
    </tr>
</table>
<%-- </ItemTemplate>
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelItem>
        <telerik:RadPanelItem Expanded="True" Text="Pre-media Filter">
            <Items>
                <telerik:RadPanelItem>
                    <ItemTemplate>
                        <table id="tblTurnInQueryCtrl" runat="server" cellpadding="1" style="width: 80%;">
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelItem>
    </Items>
</telerik:RadPanelBar>
--%>
