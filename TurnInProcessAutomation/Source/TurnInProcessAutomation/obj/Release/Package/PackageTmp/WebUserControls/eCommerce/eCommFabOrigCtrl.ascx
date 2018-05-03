<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="eCommFabOrigCtrl.ascx.vb"
    Inherits="TurnInProcessAutomation.eCommFabOrigCtrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<br />
<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="rcbAds">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rcbAds" />
                <telerik:AjaxUpdatedControl ControlID="rcbPageNumber" LoadingPanelID="ralpLoadPnl" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbDept">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbDept" />
                <telerik:AjaxUpdatedControl ControlID="cmbClass" />
                <telerik:AjaxUpdatedControl ControlID="cmbSubClass" />
                <telerik:AjaxUpdatedControl ControlID="cmbVendor" />
                <telerik:AjaxUpdatedControl ControlID="cmbVendorStyle" />
                <telerik:AjaxUpdatedControl ControlID="cmbACode1" />
                <telerik:AjaxUpdatedControl ControlID="cmbACode2" />
                <telerik:AjaxUpdatedControl ControlID="cmbACode3" />
                <telerik:AjaxUpdatedControl ControlID="cmbACode4" />
                <telerik:AjaxUpdatedControl ControlID="cmbSellSeason" />
                <telerik:AjaxUpdatedControl ControlID="cmbSellYear" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbClass">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbClass" />
                <telerik:AjaxUpdatedControl ControlID="cmbSubClass" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbVendor">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbVendor" />
                <telerik:AjaxUpdatedControl ControlID="cmbVendorStyle" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbDeptMaint">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbDeptMaint" LoadingPanelID="ralpLoadPnl" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbBuyer">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbBuyer" LoadingPanelID="ralpLoadPnl" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbBatch">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbBatch" LoadingPanelID="ralpLoadPnl" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbDMM">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbDMM"/>
                <telerik:AjaxUpdatedControl ControlID="cmbPOBuyer" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbPODept">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbPODept" />
                <telerik:AjaxUpdatedControl ControlID="cmbPOClass" />
                <telerik:AjaxUpdatedControl ControlID="cmbPOVendor" />
                <telerik:AjaxUpdatedControl ControlID="cmbPOVendorStyle" />
                <telerik:AjaxUpdatedControl ControlID="dpStartShipDate" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>
<asp:MultiView ID="mvSearchOptions" runat="server" ActiveViewIndex="0">
    <asp:View ID="View1" runat="server">
        <table cellpadding="0" style="width: 95%;">
            <tr>
                <td align="right">
                    <asp:Label ID="lblAdNoLabel" runat="server" class="smallLabel" Text="Ad#:" />
                </td>
                <td>
                    <telerik:RadComboBox ID="rcbAds" runat="server"
                        OnClientBlur="OnClientBlurHandler" OnSelectedIndexChanged="rcbAds_SelectedIndexChanged"
                        AutoPostBack="true" EnableLoadOnDemand="true" ShowMoreResultsBox="false" EnableVirtualScrolling="false"
                        DropDownWidth="400" Width="150" HighlightTemplatedItems="true" Filter="Contains">
                        <HeaderTemplate>
                            <table style="width: 350px" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="width: 50px;">Ad # </td>
                                    <td style="width: 300px;">Description </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 350px" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="width: 50px;">
                                        <%# Eval("AdNbr")%>
                                    </td>
                                    <td style="width: 300px; text-align: left">
                                        <%# Eval("AdDesc")%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:RadComboBox>
                    <bonton:ToolTipValidator ID="ttvAdNbr" runat="server" ControlToEvaluate="rcbAds"
                        ValidationGroup="Search" OnServerValidate="ttvAdNbr_ServerValidate" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label1" runat="server" class="smallLabel" Text="Page:" />
                </td>
                <td>
                    <telerik:RadComboBox ID="rcbPageNumber" runat="server" OnClientBlur="OnClientBlurHandler"
                        DropDownWidth="300" Width="150" Enabled="false" HighlightTemplatedItems="true" Filter="Contains">
                        <HeaderTemplate>
                            <table style="width: 350px" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="width: 50px;">Page # </td>
                                    <td style="width: 300px;">Description </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 350px" cellspacing="0" cellpadding="0">
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
                    <bonton:ToolTipValidator ID="ttvPageNumber" runat="server" ControlToEvaluate="rcbPageNumber"
                        ValidationGroup="Search" OnServerValidate="ttvPageNumber_ServerValidate" />
                </td>
            </tr>
            <tr runat="server" id="BuyerRow">
                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                    <asp:Label ID="lblBuyer" runat="server" class="smallLabel" Text="Buyer:" />
                </td>
                <td width="70%">
                    <telerik:RadComboBox ID="cmbBuyer" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                        class="RadComboBox_Vista" AllowCustomText="false" DropDownWidth="300" Width="150"
                        TabIndex="3" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" Filter="Contains">
                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr runat="server" id="DeptRow">
                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                    <asp:Label ID="lblDept" runat="server" class="smallLabel" Text="Department #:" />
                </td>
                <td width="70%">
                    <telerik:RadComboBox ID="cmbDeptMaint" runat="server" class="RadComboBox_Vista" AllowCustomText="false"
                        OnItemsRequested="cmbDeptMaint_ItemsRequested" DropDownWidth="200" Width="150"
                        TabIndex="4" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" Filter="Contains">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr runat="server" id="BatchRow">
                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                    <asp:Label ID="Label3" runat="server" class="smallLabel" Text="Batch #:" />
                </td>
                <td width="70%">
                    <telerik:RadComboBox ID="cmbBatch" runat="server" class="RadComboBox_Vista" AllowCustomText="false"
                        OnItemsRequested="cmbBatch_ItemsRequested" DropDownWidth="150" Width="150"
                        TabIndex="5" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" Filter="Contains">
                    </telerik:RadComboBox>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="View2" runat="server">
        <telerik:RadSplitter runat="server" ID="splM" OnClientLoad="spl_Loaded" SkinID="BlueBT">
            <telerik:RadPane ID="pMainNav" runat="server" Scrolling="None">
                <telerik:RadPanelBar runat="server" ID="rpbResultsTab" Height="470px" Width="100%"
                    ExpandMode="FullExpandedItem" AllowCollapseAllItems="true">
                    <Items>
                        <telerik:RadPanelItem Text="Filter by PO Start Ship Date" Expanded="true">
                            <Items>
                                <telerik:RadPanelItem Height="200px">
                                    <ItemTemplate>
                                        <table id="tblPreTurnInSetUpCtrl" runat="server" cellpadding="0" style="width: 100%;">
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblDMM" runat="server" class="smallLabel" Text="DMM:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbDMM" runat="server" MarkFirstMatch="true"
                                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" AutoPostBack="true"
                                                        Width="150" TabIndex="1" DropDownWidth="200px" OnItemsRequested="cmbDMM_ItemsRequested" OnSelectedIndexChanged="cmbDMM_SelectedIndexChanged"
                                                        Filter="Contains">
                                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblBuyer" runat="server" class="smallLabel" Text="Buyer:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbPOBuyer" runat="server" MarkFirstMatch="true"
                                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" AutoPostBack="true"
                                                        Width="150" TabIndex="2" DropDownWidth="200px" OnItemsRequested="cmbPOBuyer_ItemsRequested" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" 
                                                        Filter="Contains" OnSelectedIndexChanged="cmbPOBuyer_SelectedIndexChanged">
                                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblDept" runat="server" class="smallLabel" Text="Deparment:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbPODept" runat="server" class="RadComboBox_Vista" AllowCustomText="false"
                                                        AppendDataBoundItems="True" Width="150" OnItemsRequested="cmbPODept_ItemsRequested"
                                                        TabIndex="3" DropDownWidth="250px" AutoPostBack="true" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" OnSelectedIndexChanged="cmbPODept_SelectedIndexChanged">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblClass" runat="server" class="smallLabel" Text="Class:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbPOClass" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" OnItemsRequested="cmbPOClass_ItemsRequested"
                                                         Width="150" TabIndex="4" DropDownWidth="205px"
                                                        Enabled="false" AutoPostBack="true" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" OnSelectedIndexChanged="cmbPOClass_SelectedIndexChanged">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblVendor" runat="server" class="smallLabel" Text="Vendor:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbPOVendor" runat="server" AutoPostBack="true" class="RadComboBox_Vista"
                                                        AllowCustomText="false" AppendDataBoundItems="True" Width="150" TabIndex="5"
                                                        DropDownWidth="350px" Enabled="false" OnItemsRequested="cmbPOVendor_ItemsRequested"
                                                        OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" OnSelectedIndexChanged="cmbPOVendor_SelectedIndexChanged">
                                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblVendorStyle" runat="server" class="smallLabel" Text="Vendor Style:" /></td>
                                                <td width="85%" nowrap="nowrap" valign="middle">
                                                    <telerik:RadComboBox ID="cmbPOVendorStyle" runat="server" OnItemsRequested="cmbPOVendorStyle_ItemsRequested"
                                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" AutoPostBack="true"
                                                        OnClientBlur="OnClientBlurHandler" Width="130px" TabIndex="6" DropDownWidth="205px"
                                                        Enabled="false" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" OnSelectedIndexChanged="cmbPOVendorStyle_SelectedIndexChanged">
                                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                    </telerik:RadComboBox>
                                                    <asp:ImageButton ID="imgAddPOVendorStyle" runat="server" ImageUrl="~/Images/Add.gif"
                                                        OnClick="imgAddPOVendorStyle_Click" /></td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">&#160;&nbsp;</td>
                                                <td width="85%">
                                                    <asp:ListBox runat="server" ID="lbPOVendorStyles" Width="130px" CssClass="ListBox"></asp:ListBox><asp:ImageButton ID="btnPOResetVendorStyles" runat="server" ToolTip="Reset" ImageUrl="~/App_Themes/BonTonStandard/Images/Reset.gif" OnClick="btnPOResetVendorStyles_Click"/></td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblStartShipDate" runat="server" class="smallLabel" Text="Start Ship Date:" /></td>
                                                <td width="85%">
                                                    <telerik:RadDatePicker ID="dpStartShipDate" Style="vertical-align: middle; width: 120px;" runat="server" AutoPostBack="true"
                                                        TabIndex="4" UseEmbeddedScripts="false" CssClass="dpWidth" OnSelectedDateChanged="dpStartShipDate_SelectedDateChanged">
                                                        <DateInput ID="diCreatedSince" TabIndex="4" DisplayDateFormat="MM/dd/yyyy" runat="server"></DateInput><Calendar ID="calCreatedSince" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                            ViewSelectorText="x" runat="server">
                                                        </Calendar>
                                                        <DatePopupButton ImageUrl="" HoverImageUrl="" TabIndex="4"></DatePopupButton>
                                                    </telerik:RadDatePicker>
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Error.png" Visible="False" /></td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelItem>
                        <telerik:RadPanelItem Text="Filter by Hierarchy">
                            <Items>
                                <telerik:RadPanelItem>
                                    <ItemTemplate>
                                        <table id="tblPreTurnInSetUpCtrl" runat="server" cellpadding="0" style="width: 100%;">
                                            <tr>
                                                <td nowrap="nowrap" align="right">
                                                    <asp:Label ID="Label2" runat="server" class="smallLabel" Text="Price Status:" /></td>
                                                <td width="85%">
                                                    <asp:CheckBoxList runat="server" ID="cblPriceStatusCodes" RepeatDirection="Horizontal"
                                                        RepeatLayout="Flow" CssClass="ChkLabel" AutoPostBack="true" OnSelectedIndexChanged="cblPriceStatusCodes_SelectedIndexChanged" >
                                                        <asp:ListItem Text="R" Selected="true" />
                                                        <asp:ListItem Text="V" Selected="true" />
                                                        <asp:ListItem Text="M" />
                                                        <asp:ListItem Text="C" />
                                                        <asp:ListItem Text="F" />
                                                        <asp:ListItem Text="P" />
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblDept" runat="server" class="smallLabel" Text="Dept:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbDept" runat="server" class="RadComboBox_Vista" AllowCustomText="false"
                                                        AppendDataBoundItems="True" OnItemsRequested="cmbDept_ItemsRequested" Width="150"
                                                        TabIndex="1" DropDownWidth="250px" AutoPostBack="true" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"
                                                        OnSelectedIndexChanged="cmbDept_SelectedIndexChanged">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblClass" runat="server" class="smallLabel" Text="Class:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbClass" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                                        OnItemsRequested="cmbClass_ItemsRequested" Width="150" TabIndex="2" DropDownWidth="205px"
                                                        Enabled="false" AutoPostBack="true" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"
                                                        OnSelectedIndexChanged="cmbClass_SelectedIndexChanged">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblSubClass" runat="server" class="smallLabel" Text="Sub-Class:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbSubClass" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                                        OnItemsRequested="cmbSubClass_ItemsRequested" Width="150" TabIndex="2" DropDownWidth="205px"
                                                        Enabled="false" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblVendor" runat="server" class="smallLabel" Text="Vendor:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbVendor" runat="server" AutoPostBack="true" class="RadComboBox_Vista"
                                                        AllowCustomText="false" AppendDataBoundItems="True" Width="150" TabIndex="2"
                                                        DropDownWidth="350px" Enabled="false" OnItemsRequested="cmbVendor_ItemsRequested"
                                                        OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" OnSelectedIndexChanged="cmbVendor_SelectedIndexChanged">
                                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblVendorStyle" runat="server" class="smallLabel" Text="Vendor Style:" /></td>
                                                <td width="85%" nowrap="nowrap" valign="middle">
                                                    <telerik:RadComboBox ID="cmbVendorStyle" runat="server" OnItemsRequested="cmbVendorStyle_ItemsRequested"
                                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                                        OnClientBlur="OnClientBlurHandler" Width="130px" TabIndex="5" DropDownWidth="205px" AutoPostBack="true"
                                                        Enabled="false" OnSelectedIndexChanged="cmbVendorStyle_SelectedIndexChanged">
                                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                    </telerik:RadComboBox>
                                                    <asp:ImageButton ID="imgAddVendorStyle" runat="server" ImageUrl="~/Images/Add.gif"
                                                        OnClick="imgAddVendorStyle_Click" /></td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">&#160;&nbsp;</td>
                                                <td width="85%">
                                                    <asp:ListBox runat="server" ID="lbVendorStyles" Width="130px" CssClass="ListBox"></asp:ListBox><asp:ImageButton ID="btnResetVendorStyles" runat="server" ToolTip="Reset" ImageUrl="~/App_Themes/BonTonStandard/Images/Reset.gif"
                                                        OnClick="btnResetVendorStyles_Click" /></td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblACode1" runat="server" class="smallLabel" Text="A-CD 1:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbACode1" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" AutoPostBack="true"
                                                        OnItemsRequested="cmbACode1_ItemsRequested" Width="150" TabIndex="3" DropDownWidth="205px"
                                                        Enabled="false" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" OnSelectedIndexChanged="cmbACode1_SelectedIndexChanged">
                                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblACode2" runat="server" class="smallLabel" Text="A-CD 2:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbACode2" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" AutoPostBack="true"
                                                        OnItemsRequested="cmbACode2_ItemsRequested" Width="150" TabIndex="3" DropDownWidth="205px"
                                                        Enabled="false" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" OnSelectedIndexChanged="cmbACode2_SelectedIndexChanged">
                                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblACode3" runat="server" class="smallLabel" Text="A-CD 3:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbACode3" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" AutoPostBack="true"
                                                        OnItemsRequested="cmbACode3_ItemsRequested" Width="150" TabIndex="3" DropDownWidth="205px"
                                                        Enabled="false" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" OnSelectedIndexChanged="cmbACode3_SelectedIndexChanged">
                                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblACode4" runat="server" class="smallLabel" Text="A-CD 4:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbACode4" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" AutoPostBack="true"
                                                        OnItemsRequested="cmbACode4_ItemsRequested" Width="150" TabIndex="3" DropDownWidth="205px"
                                                        Enabled="false" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" OnSelectedIndexChanged="cmbACode4_SelectedIndexChanged">
                                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblSellYear" runat="server" class="smallLabel" Text="Sell Year:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbSellYear" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" AutoPostBack="true"
                                                        Enabled="false" OnItemsRequested="cmbSellYear_ItemsRequested" Width="100" TabIndex="3"
                                                        DropDownWidth="100px" OnSelectedIndexChanged="cmbSellYear_SelectedIndexChanged">
                                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblSellSeason" runat="server" class="smallLabel" Text="Sell Season:" /></td>
                                                <td width="85%">
                                                    <telerik:RadComboBox ID="cmbSellSeason" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" AutoPostBack="true"
                                                        Enabled="false" OnItemsRequested="cmbSellSeason_ItemsRequested" Width="150" TabIndex="3"
                                                        DropDownWidth="150px" OnSelectedIndexChanged="cmbSellSeason_SelectedIndexChanged">
                                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblCreatedSince" runat="server" class="smallLabel" Text="Created Since:" /></td>
                                                <td width="85%">
                                                    <telerik:RadDatePicker ID="dpCreatedSince" Style="vertical-align: middle; width: 100px;" runat="server" AutoPostBack="true"
                                                        TabIndex="4" UseEmbeddedScripts="false" CssClass="dpWidth" OnSelectedDateChanged="dpCreatedSince_SelectedDateChanged">
                                                        <DateInput ID="diCreatedSince" TabIndex="4" DisplayDateFormat="MM/dd/yyyy" runat="server"></DateInput><Calendar ID="calCreatedSince" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                            ViewSelectorText="x" runat="server">
                                                        </Calendar>
                                                        <DatePopupButton ImageUrl="" HoverImageUrl="" TabIndex="4"></DatePopupButton>
                                                    </telerik:RadDatePicker>
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Error.png" Visible="False" /></td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelItem>
                        <telerik:RadPanelItem Text="Filter by ISN">
                            <Items>
                                <telerik:RadPanelItem>
                                    <ItemTemplate>
                                        <table id="Table1" runat="server" cellpadding="0" style="width: 100%;">
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblISN" runat="server" class="smallLabel" Text="ISN:" /></td>
                                                <td width="75%">
                                                    <telerik:RadTextBox ID="txtISN" runat="server" Width="80" TabIndex="7" />&#160;&#160;
                                                    <asp:ImageButton ID="imgAddISN" runat="server" ImageUrl="~/Images/Add.gif" OnClick="imgAddISN_Click"
                                                        ValidationGroup="AddISN" /><bonton:ToolTipValidator ID="ttvISN" runat="server" ControlToEvaluate="txtISN" ValidationGroup="AddISN"
                                                            OnServerValidate="ttvISN_ServerValidate" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">&#160;&nbsp;</td>
                                                <td width="75%">
                                                    <asp:ListBox runat="server" ID="lbISNs" Width="130px" CssClass="ListBox"></asp:ListBox><asp:ImageButton ID="btnResetISNs" runat="server" ToolTip="Reset" ImageUrl="~/App_Themes/BonTonStandard/Images/Reset.gif"
                                                        OnClick="btnResetISNs_Click" /><br /><br /></td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                                    <asp:Label ID="lblSku" runat="server" class="smallLabel" Text="SKU/UPC:" /></td>
                                                <td width="75%">
                                                    <telerik:RadTextBox ID="txtSku" runat="server" Width="80" TabIndex="8" AutoPostBack="true" OnTextChanged="txtSku_TextChanged" />&#160;&#160;
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelItem>
                    </Items>
                </telerik:RadPanelBar>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadCodeBlock runat="server" ID="rcb1">
            <script type="text/javascript">
                function spl_Loaded(s, e) {
                    s.get_element().style.visibility = 'inherit';
                }
           </script>         
        </telerik:RadCodeBlock>
    </asp:View>
</asp:MultiView>
