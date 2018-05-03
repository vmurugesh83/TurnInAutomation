<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GXSCopyViewSearchCtrl.ascx.vb" Inherits="TurnInProcessAutomation.GXSCopyViewSearchCtrl" %>

<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <%--<telerik:AjaxSetting AjaxControlID="rcbAds">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rcbAds" />
                <telerik:AjaxUpdatedControl ControlID="rcbPageNumber" LoadingPanelID="ralpLoadPnl" />
            </UpdatedControls>
        </telerik:AjaxSetting>--%>
        <telerik:AjaxSetting AjaxControlID="cmbDept">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbDept" />
                <telerik:AjaxUpdatedControl ControlID="cmbClass" />
                <telerik:AjaxUpdatedControl ControlID="cmbVendor" />
                <telerik:AjaxUpdatedControl ControlID="cmbVendorStyle" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbClass">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbClass" />
                <telerik:AjaxUpdatedControl ControlID="cmbVendor" />
                <telerik:AjaxUpdatedControl ControlID="cmbVendorStyle" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbVendor">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbVendor" />
                <telerik:AjaxUpdatedControl ControlID="cmbVendorStyle" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="btnResetISNs">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="lbISNs" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="btnResetSKUs">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="lbSKUs" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbAds">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbAds" />
                <telerik:AjaxUpdatedControl ControlID="cmbPageNumber" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbPageNumber">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbPageNumber" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbBatch">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbBatch" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbLabel">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbLabel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>

<telerik:RadCodeBlock runat="server" ID="rcb">
    <script type="text/javascript">
    </script>
</telerik:RadCodeBlock>

<telerik:RadSplitter runat="server" ID="splM" OnClientLoad="spl_Loaded" SkinID="BlueBT">
    <%--<telerik:RadPane runat="server" ID="pNav" SkinID="pLNav" Scrolling="None">
        <div style="float: left; clear: none;" class="tabsContainer">
            <telerik:RadSlidingZone ID="slZone" runat="server" OnClientLoaded="OnClientLoaded" SlideDirection="Right" ClickToOpen="true">
                <telerik:RadSlidingPane ID="slPane" Title="TurnIn Filter" runat="server" Overlay="true" EnableDock="false" Width="243px" SkinID="slPane">
                    <asp:PlaceHolder runat="server" ID="phTurnInFilter">
                        <div>
                            <br />
                            <span class="Label" style="font-weight: bold; padding-left: 5px;">Sample Status</span>
                            <br />
                            <span class="CheckBox" style="padding-left: 5px; padding-right: 5px;">
                                <asp:CheckBox ID="chkAvailableAndApproved" name="chkAvailableAndApproved" runat="server" /><label
                                    for="chkAvailableAndApproved">Sample Available and Approved</label>
                            </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 5px; padding-right: 5px;">
                                <asp:CheckBox ID="chkNotAvailable" name="chkNotAvailable" runat="server" /><label
                                    for="chkNotAvailable">Sample Not Available</label></span><span class="Label">
                                    </span>
                            <br />
                            <br />
                            <span class="Label" style="font-weight: bold; padding-left: 5px;">Web Activity</span>
                            <br />
                            <span class="CheckBox" style="padding-left: 5px; padding-right: 5px;">
                                <asp:CheckBox ID="chkActiveOnWeb" name="chkActiveOnWeb" runat="server" /><label for="chkActiveOnWeb">Active
                                            on Web</label>
                            </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 5px; padding-right: 5px;">
                                <asp:CheckBox ID="chkNotActiveOnWeb" name="chkNotActiveOnWeb" runat="server" />
                                <label for="chkNotActiveOnWeb">
                                    Not
                                            Active on Web</label></span><span class="Label"> </span>
                            <br />
                            <br />
                            <span class="Label" style="font-weight: bold; padding-left: 5px;">Turnin Type</span>
                            <br />
                            <span class="CheckBox" style="padding-left: 5px; padding-right: 5px;">
                                <asp:CheckBox ID="chkTurnInType" name="chkTurnInType" runat="server" /><label for="chkTurnInType">Not
                                            in Turn-in</label>
                            </span>
                            <br />
                        </div>
                    </asp:PlaceHolder>
                </telerik:RadSlidingPane>
            </telerik:RadSlidingZone>
        </div>
    </telerik:RadPane>--%>
    <%--            <telerik:RadSplitBar runat="server" ID="sBar1" CollapseMode="Forward" EnableResize="true" />--%>
    <telerik:RadPane ID="pMainNav" runat="server" Scrolling="None">
        <telerik:RadPanelBar runat="server" ID="rpbResultsTab" Height="100%" Width="100%"
            ExpandMode="FullExpandedItem">
            <Items>
                <telerik:RadPanelItem Expanded="True" Text="Find By Hierarchy">
                    <Items>
                        <telerik:RadPanelItem>
                            <ItemTemplate>
                                <br />
                                <table id="tblGXSCopyViewCtrl" runat="server" style="width: 100%; padding: 0;">
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">
                                            <asp:Label ID="lblDept" runat="server" CssClass="smallLabel" Text="Dept:&nbsp;" />
                                        </td>
                                        <td style="width: 85%;">
                                            <telerik:RadComboBox ID="cmbDept" runat="server" CssClass="RadComboBox_Vista" AllowCustomText="false"
                                                AppendDataBoundItems="True" OnItemsRequested="cmbDept_ItemsRequested" Width="150"
                                                TabIndex="1" DropDownWidth="150px" AutoPostBack="true" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"
                                                OnSelectedIndexChanged="cmbDept_SelectedIndexChanged">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">
                                            <asp:Label ID="lblClass" runat="server" CssClass="smallLabel" Text="Class:&nbsp;" />
                                        </td>
                                        <td style="width: 85%;">
                                            <telerik:RadComboBox ID="cmbClass" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                CssClass="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                                OnItemsRequested="cmbClass_ItemsRequested" Width="150" TabIndex="2" DropDownWidth="150px"
                                                Enabled="false" AutoPostBack="true" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"
                                                OnSelectedIndexChanged="cmbClass_SelectedIndexChanged">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">
                                            <asp:Label ID="lblVendor" runat="server" CssClass="smallLabel" Text="Vendor:&nbsp;" />
                                        </td>
                                        <td style="width: 85%;">
                                            <telerik:RadComboBox ID="cmbVendor" runat="server" AutoPostBack="true" CssClass="RadComboBox_Vista"
                                                AllowCustomText="false" AppendDataBoundItems="True" Width="150" TabIndex="2"
                                                DropDownWidth="150px" Enabled="false" OnItemsRequested="cmbVendor_ItemsRequested"
                                                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" OnSelectedIndexChanged="cmbVendor_SelectedIndexChanged">
                                                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">
                                            <asp:Label ID="lblVendorStyle" runat="server" CssClass="smallLabel" Text="Vendor Style:&nbsp;" />
                                        </td>
                                        <td style="white-space: nowrap; vertical-align: middle; width: 85%;">
                                            <telerik:RadComboBox ID="cmbVendorStyle" runat="server" OnItemsRequested="cmbVendorStyle_ItemsRequested"
                                                CssClass="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                                OnClientBlur="OnClientBlurHandler" Width="150px" TabIndex="5" DropDownWidth="150px"
                                                Enabled="false" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                                                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                            </telerik:RadComboBox>
                                            <%--<asp:ImageButton ID="imgAddVendorStyle" runat="server" ImageUrl="~/Images/Add.gif"
                                                OnClick="imgAddVendorStyle_Click" />--%>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">&nbsp;
                                        </td>
                                        <td style="width: 85%;">
                                            <asp:ListBox runat="server" ID="lbVendorStyles" Width="130px" CssClass="ListBox"></asp:ListBox>
                                            <asp:ImageButton ID="btnResetVendorStyles" runat="server" ToolTip="Reset" ImageUrl="~/App_Themes/BonTonStandard/Images/Reset.gif"
                                                OnClick="btnResetVendorStyles_Click" />
                                        </td>
                                    </tr>--%>
                                </table>
                            </ItemTemplate>
                        </telerik:RadPanelItem>
                    </Items>
                </telerik:RadPanelItem>
                <telerik:RadPanelItem Text="Find By Item(s)">
                    <%--/Reserve ISN">--%>
                    <Items>
                        <telerik:RadPanelItem>
                            <ItemTemplate>
                                <br />
                                <table id="Table1" runat="server" style="width: 100%; padding: 0;">
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">
                                            <asp:Label ID="lblISN" runat="server" CssClass="smallLabel" Text="ISN:&nbsp;" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtISN" runat="server" Width="150" TabIndex="7" />
                                            <%--<bonton:ToolTipValidator ID="ttvISN" runat="server" ControlToEvaluate="txtISN" ValidationGroup="AddISN"
                                                OnServerValidate="ttvISN_ServerValidate" />--%>
                                        </td>
                                        <td style="padding-right: 5px;">
                                            <asp:ImageButton ID="imgAddISN" runat="server" ImageUrl="~/Images/Add.gif" OnClick="imgAddISN_Click"
                                                ValidationGroup="AddISN" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-bottom: 10px;"></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">&nbsp;
                                        </td>
                                        <td>
                                            <asp:ListBox runat="server" ID="lbISNs" Width="150px" Height="200px" CssClass="ListBox"></asp:ListBox>
                                        </td>
                                        <td style="vertical-align: top; text-align: left;">
                                            <asp:ImageButton ID="btnResetISNs" runat="server" ToolTip="Reset" ImageUrl="~/App_Themes/BonTonStandard/Images/Reset.gif"
                                                OnClick="btnResetISNs_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">
                                            <asp:Label ID="lblSKU" runat="server" CssClass="smallLabel" Text="SKU/UPC:&nbsp;" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtSKU" runat="server" Width="150" TabIndex="8" />
                                            <%--<bonton:ToolTipValidator ID="ttvSKU" runat="server" ControlToEvaluate="txtSKU" ValidationGroup="AddSKU"
                                                OnServerValidate="ttvSKU_ServerValidate" />--%>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgAddSKU" runat="server" ImageUrl="~/Images/Add.gif" OnClick="imgAddSKU_Click"
                                                ValidationGroup="AddSKU" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-bottom: 10px;"></td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">&nbsp;
                                        </td>
                                        <td>
                                            <asp:ListBox runat="server" ID="lbSKUs" Width="150px" Height="200px" CssClass="ListBox"></asp:ListBox>
                                        </td>
                                        <td style="vertical-align: top; text-align: left;">
                                            <asp:ImageButton ID="btnResetSKUs" runat="server" ToolTip="Reset" ImageUrl="~/App_Themes/BonTonStandard/Images/Reset.gif"
                                                OnClick="btnResetSKUs_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadPanelItem>
                    </Items>
                </telerik:RadPanelItem>
                <telerik:RadPanelItem Expanded="True" Text="Find By Ads">
                    <Items>
                        <telerik:RadPanelItem>
                            <ItemTemplate>
                                <br />
                                <table style="width: 95%; padding: 0;">
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblAdNoLabel" runat="server" CssClass="smallLabel" Text="Ad#:&nbsp;" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="cmbAds" runat="server"
                                                OnClientBlur="OnClientBlurHandler" OnSelectedIndexChanged="cmbAds_SelectedIndexChanged" OnItemsRequested="cmbAds_ItemsRequested"
                                                AutoPostBack="true" EnableLoadOnDemand="true" ShowMoreResultsBox="false" EnableVirtualScrolling="false"
                                                DropDownWidth="150" Width="150" HighlightTemplatedItems="true" Filter="Contains">
                                                <HeaderTemplate>
                                                    <table style="width: 350px; padding: 0; border-collapse: collapse;">
                                                        <tr>
                                                            <td style="width: 50px;">Ad #
                                                            </td>
                                                            <td style="width: 300px;">Description
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table style="width: 350px; padding: 0; border-collapse: collapse;">
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
                                            <%--<bonton:ToolTipValidator ID="ttvAdNbr" runat="server" ControlToEvaluate="rcbAds"
                                                ValidationGroup="Search" OnServerValidate="ttvAdNbr_ServerValidate" />--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label1" runat="server" CssClass="smallLabel" Text="Page:&nbsp;" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="cmbPageNumber" runat="server" OnClientBlur="OnClientBlurHandler" CheckBoxes="true"
                                                DropDownWidth="150" Width="150" Enabled="false" HighlightTemplatedItems="true" Filter="Contains">

                                                <HeaderTemplate>
                                                    <table style="width: 350px; padding: 0; border-collapse: collapse;">
                                                        <tr>
                                                            <td style="width: 100px;">Page #
                                                            </td>
                                                            <td style="width: 300px;">Description
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table style="width: 350px; padding: 0; border-collapse: collapse;">
                                                        <tr>
                                                            <td style="width: 100px;">
                                                                <%# Eval("PgNbr")%>
                                                            </td>
                                                            <td style="width: 200px; text-align: left">
                                                                <%# Eval("PgDesc")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:RadComboBox>
                                            <%--<bonton:ToolTipValidator ID="ttvPageNumber" runat="server" ControlToEvaluate="rcbPageNumber"
                                                ValidationGroup="Search" OnServerValidate="ttvPageNumber_ServerValidate" />--%>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="BatchRow">
                                        <td style="text-align: right; white-space: nowrap; text-align: right; padding-left: 5px">
                                            <asp:Label ID="Label3" runat="server" CssClass="smallLabel" Text="Batch #:&nbsp;" />
                                        </td>
                                        <td style="width: 70%;">
                                            <telerik:RadComboBox ID="cmbBatch" runat="server" CssClass="RadComboBox_Vista" AllowCustomText="false"
                                                OnItemsRequested="cmbBatch_ItemsRequested" DropDownWidth="150" Width="150"
                                                TabIndex="5" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" Filter="Contains">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadPanelItem>
                    </Items>
                </telerik:RadPanelItem>
                <telerik:RadPanelItem Expanded="True" Text="Find By Label">
                    <Items>
                        <telerik:RadPanelItem>
                            <ItemTemplate>
                                <br />
                                <table runat="server" style="width: 100%; padding: 0;">
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">
                                            <asp:Label ID="lblLabel" runat="server" CssClass="smallLabel" Text="Label:&nbsp;" />
                                        </td>
                                        <td style="width: 85%;">
                                            <telerik:RadComboBox ID="cmbLabel" runat="server" CssClass="RadComboBox_Vista" AllowCustomText="false"
                                                AppendDataBoundItems="True" OnItemsRequested="cmbLabel_ItemsRequested" Width="150"
                                                TabIndex="1" DropDownWidth="150px" AutoPostBack="true" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadPanelItem>
                    </Items>
                </telerik:RadPanelItem>
                <telerik:RadPanelItem Expanded="True" Text="Find By PO Start Ship Date">
                    <Items>
                        <telerik:RadPanelItem>
                            <ItemTemplate>
                                <br />
                                <table runat="server" style="width: 100%; padding: 0;">
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">
                                            <asp:Label ID="lblFrom" runat="server" CssClass="smallLabel" Text="From:&nbsp;" />
                                        </td>
                                        <td style="width: 85%;">
                                            <telerik:RadDatePicker ID="dpFrom" Style="vertical-align: middle; width: 150px;" runat="server"
                                                TabIndex="4" Calendar-EnableEmbeddedScripts="false" CssClass="dpWidth">
                                                <DateInput ID="diFrom" TabIndex="4" DisplayDateFormat="MM/dd/yyyy" runat="server">
                                                </DateInput>
                                                <Calendar ID="calCreatedSince" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                    ViewSelectorText="x" runat="server">
                                                </Calendar>
                                                <DatePopupButton ImageUrl="" HoverImageUrl="" TabIndex="4"></DatePopupButton>
                                            </telerik:RadDatePicker>
                                            <asp:Image ID="imgErrorFrom" runat="server" ImageUrl="~/Images/Error.png" Visible="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">
                                            <asp:Label ID="lblTo" runat="server" CssClass="smallLabel" Text="To:&nbsp;" />
                                        </td>
                                        <td style="width: 85%;">
                                            <telerik:RadDatePicker ID="dpTo" Style="vertical-align: middle; width: 150px;" runat="server"
                                                TabIndex="4" Calendar-EnableEmbeddedScripts="false" CssClass="dpWidth">
                                                <DateInput ID="diTo" TabIndex="4" DisplayDateFormat="MM/dd/yyyy" runat="server">
                                                </DateInput>
                                                <Calendar ID="calCreatedTo" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                    ViewSelectorText="x" runat="server">
                                                </Calendar>
                                                <DatePopupButton ImageUrl="" HoverImageUrl="" TabIndex="4"></DatePopupButton>
                                            </telerik:RadDatePicker>
                                            <asp:Image ID="imgErrorTo" runat="server" ImageUrl="~/Images/Error.png" Visible="False" />
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
        function OnClientLoaded(sender, args) {
            <%--var slidingPane = $find('<%= slPane.ClientID %>');
                if (slidingPane.get_expanded()) {
                    sender.collapsePane(slidingPane.get_id());
                }--%>
            
        }

    </script>
</telerik:RadCodeBlock>
