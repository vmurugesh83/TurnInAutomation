<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CopyPrioritizationSearchCtrl.ascx.vb" Inherits="TurnInProcessAutomation.CopyPrioritizationSearchCtrl" %>

<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="cmbDept">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbDept"  LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbGMM">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbGMM"  LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbVendorStyle">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbVendorStyle"  LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="radLBISN">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="radLBISN"  LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Black" MinDisplayTime="1000">
</telerik:RadAjaxLoadingPanel>

<telerik:RadCodeBlock runat="server" ID="rcb">
    <style type="text/css">
        .slPane_disable {
            color: #dcdcdc !important;
        }
    </style>
    <script type="text/javascript">
    </script>
</telerik:RadCodeBlock>

<telerik:RadSplitter runat="server" ID="splM" OnClientLoad="spl_Loaded" SkinID="BlueBT">
    <telerik:RadPane runat="server" ID="pNav" SkinID="pLNav" Height="100%">
        <div style="float: left; clear: none;" class="tabsContainer">
            <%--            <telerik:RadSlidingZone ID="slZone" runat="server" OnClientLoaded="OnClientLoaded" SlideDirection="Right" ClickToOpen="true">--%>
            <telerik:RadSlidingZone ID="slZone" runat="server" SlideDirection="Right" ClickToOpen="true">
                <telerik:RadSlidingPane ID="slPane" Title="Copy Filter" runat="server" Overlay="true" EnableDock="false" Width="300px" SkinID="slPane">
                    <asp:PlaceHolder runat="server" ID="phCopyFilter">
                        <div>
                            <br />
                            <asp:Label ID="lblPriceStatusHeader" runat="server" class="smallLabel" Style="padding-left: 5px; padding-right: 5px;" Text="Price Status:" />
                            <asp:CheckBoxList runat="server" ID="cblPriceStatusCodes" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="ChkPricingStatus" CellSpacing="10" CellPadding="10" Font-Size="X-Small">
                                <asp:ListItem Text="R" Selected="true" />
                                <asp:ListItem Text="V" Selected="true" />
                                <asp:ListItem Text="M" />
                                <asp:ListItem Text="C" />
                                <asp:ListItem Text="F" />
                                <asp:ListItem Text="P" />
                            </asp:CheckBoxList>
                            <br />
                            <br />
                            <asp:Label ID="lblCopyHeader" runat="server" class="smallLabel" Style="padding-left: 5px;">Copy</asp:Label>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkCopyReady" name="chkCopyReady" runat="server" />
                                <label for="chkCopyReady">Ready</label></span><span class="Label">
                                </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkCopyNotReady" name="chkCopyNotReady" runat="server" Checked="true" />
                                <label for="chkCopyNotReady">Not Ready</label></span><span class="Label">
                                </span>
                            <br />
                            <br />
                            <asp:Label ID="lblFinalImageHeader" runat="server" class="smallLabel" Style="padding-left: 5px;">Final Image</asp:Label>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkImageAvailable" name="chkImageAvailable" runat="server" />
                                <label for="chkImageAvailable">Available</label>
                            </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkImageNotAvailable" name="chkImageNotAvailable" runat="server" />
                                <label for="chkImageNotAvailable">Not Available</label></span><span class="Label">
                                </span>
                            <br />
                            <br />
                            <asp:Label ID="lblSKUUseHeader" runat="server" class="smallLabel" Style="padding-left: 5px;">SKU Use</asp:Label>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkBasic" name="chkBasic" runat="server" Text="Basic" CssClass="ChkLabel" />
                            </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkFashion" name="chkFashion" runat="server" Text="Fashion" CssClass="ChkLabel" />
                            </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkPWP" name="chkPWP" runat="server" Text="PWP" CssClass="ChkLabel" />
                            </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkGWP" name="chkGWP" runat="server" Text="GWP" CssClass="ChkLabel" />
                            </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkSpecial" name="chkSpecial" runat="server" Text="Special" CssClass="ChkLabel" />
                            </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkCollateral" name="chkCollateral" runat="server" Text="Collateral" CssClass="ChkLabel" />
                            </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkVirtualGC" name="chkVirtualGC" runat="server" Text="Virtual GC" CssClass="ChkLabel" />
                            </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkPlasticGC" name="chkPlasticGC" runat="server" Text="Plastic GC" CssClass="ChkLabel" />
                            </span>
                            <br />
                            <br />
                            <asp:Label ID="lblMiscHeader" runat="server" class="smallLabel" Style="padding-left: 5px;">Miscellaneous</asp:Label>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkOnHand" name="chkOnHand" runat="server" />
                                <label for="chkOnHand">On Hand(>0)</label></span>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkOnOrder" name="chkOnOrder" runat="server" />
                                <label for="chkOnOrder">On Order(>0)</label></span>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkINFC" name="chkINFC" runat="server" />
                                <label for="chkINFC">Loc 192 only</label></span>
                            <br />
                            <span class="CheckBox" style="padding-left: 15px; padding-right: 5px;">
                                <asp:CheckBox ID="chkDropship" name="chkDropship" runat="server" />
                                <label for="chkDropship">Direct Ship</label></span>

                        </div>
                    </asp:PlaceHolder>
                </telerik:RadSlidingPane>
            </telerik:RadSlidingZone>
        </div>
    </telerik:RadPane>
    <%--    <telerik:RadSplitBar runat="server" ID="sBar1" CollapseMode="Forward" EnableResize="true" />--%>
    <telerik:RadPane ID="pMainNav" runat="server" Scrolling="None">
        <telerik:RadPanelBar runat="server" ID="rpbResultsTab" Height="100%" Width="100%"
            ExpandMode="FullExpandedItem" OnClientItemClicked="rpbResultsTab_OnClientItemClicked">
            <Items>
                <telerik:RadPanelItem Expanded="True" Text="Find By Web Category">
                    <Items>
                        <telerik:RadPanelItem>
                            <ItemTemplate>
                                <br />
                                <table id="tblCriteria" runat="server" style="width: 100%; padding: 0;">
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">
                                            <asp:Label ID="Label3" runat="server" CssClass="smallLabel" Text="Web Category (Top):" />&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <telerik:RadDropDownTree RenderMode="Lightweight"
                                                ID="rddtCategory"
                                                runat="server"
                                                Width="150px"
                                                DefaultMessage="Select Web Category"
                                                ExpandNodeOnSingleClick="false"
                                                EnableFiltering="true"
                                                OnClientEntryAdding="OnClientEntryAdding">
                                                <DropDownSettings OpenDropDownOnLoad="false" AutoWidth="Enabled" CloseDropDownOnSelection="true" />
                                                <FilterSettings Highlight="Matches" EmptyMessage="Type here to filter" />
                                            </telerik:RadDropDownTree>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadPanelItem>
                    </Items>
                </telerik:RadPanelItem>
                <telerik:RadPanelItem Expanded="True" Text="Find By Hierarchy">
                    <Items>
                        <telerik:RadPanelItem>
                            <ItemTemplate>
                                <br />
                                <table id="tblHierarchy" runat="server" style="width: 100%; padding: 0;">
                                    <tr>
                                        <td nowrap="nowrap" align="right" style="padding-right: 5px">
                                            <asp:Label ID="Label1" runat="server" class="smallLabel" Text="Ad: " /></td>
                                        <td width="85%">
                                            <telerik:RadComboBox ID="cmbAdNo" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                class="RadComboBox_Vista" AllowCustomText="false" OnItemsRequested="cmbAdNo_ItemsRequested" Filter="StartsWith"
                                                Width="180" DropDownWidth="326" EnableLoadOnDemand="true"
                                                ShowMoreResultsBox="false" EnableVirtualScrolling="false" HighlightTemplatedItems="true"
                                                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" TabIndex="11">
                                                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                <HeaderTemplate>
                                                    <table style="width: 300px" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="width: 50px;">Ad #
                                                            </td>
                                                            <td style="width: 250px;">Description
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
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" align="right" style="padding-right: 5px">
                                            <asp:Label ID="lblGMM" runat="server" class="smallLabel" Text="GMM: " /></td>
                                        <td width="85%">
                                            <telerik:RadComboBox ID="cmbGMM" runat="server" MarkFirstMatch="true"
                                                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" AutoPostBack="true"
                                                Width="150" TabIndex="1" DropDownWidth="200px" OnItemsRequested="cmbGMM_ItemsRequested" 
                                                OnSelectedIndexChanged="cmbGMM_SelectedIndexChanged"
                                                OnClientSelectedIndexChanged="gmmSelectedIndexChanged"
                                                Filter="Contains">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" align="right" style="padding-right: 5px">
                                            <asp:Label ID="lblDept" runat="server" class="smallLabel" Text="Dept:" /></td>
                                        <td width="85%">
                                            <telerik:RadComboBox ID="cmbDept"
                                                runat="server"
                                                class="RadComboBox_Vista"
                                                AllowCustomText="false"
                                                AppendDataBoundItems="True"
                                                OnItemsRequested="cmbDept_ItemsRequested"
                                                Width="150"
                                                TabIndex="1"
                                                DropDownWidth="250px"
                                                AutoPostBack="true"
                                                OnClientSelectedIndexChanged="deptSelectedIndexChanged"
                                                OnSelectedIndexChanged="cmbDept_SelectedIndexChanged" Enabled="false">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">
                                            <asp:Label ID="lblVendorStyle" runat="server" CssClass="smallLabel" Text="Vendor Style:" />&nbsp;&nbsp;
                                        </td>
                                        <td style="white-space: nowrap; vertical-align: middle; width: 85%;">
                                            <telerik:RadComboBox ID="cmbVendorStyle" runat="server"
                                                MarkFirstMatch="true"
                                                OnClientBlur="OnClientBlurHandler"
                                                class="RadComboBox_Vista"
                                                AllowCustomText="false"
                                                AppendDataBoundItems="True"
                                                Width="80"
                                                TabIndex="5"
                                                AutoPostBack="true"
                                                DropDownWidth="205px" OnItemsRequested="cmbVendorStyle_ItemsRequested" Enabled="false">
                                                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                            </telerik:RadComboBox>
                                            <asp:ImageButton ID="imgAddVendorStyle" runat="server" ImageUrl="~/Images/Add.gif"
                                                OnClick="imgAddVendorStyle_Click" OnClientClick="return addVendorStyle()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right; padding-left: 5px">&nbsp;
                                        </td>
                                        <td style="width: 85%;">
                                            <asp:ListBox runat="server" ID="lbVendorStyles"
                                                Width="130px"
                                                CssClass="ListBox"></asp:ListBox>
                                            <asp:ImageButton ID="btnResetVendorStyles" runat="server"
                                                ToolTip="Reset"
                                                ImageUrl="~/App_Themes/BonTonStandard/Images/Reset.gif"
                                                OnClick="btnResetVendorStyles_Click"
                                                OnClientClick="return clearListbox()"
                                                ImageAlign="Top" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 5px; text-align: right; text-wrap: none; white-space: nowrap;">
                                            <asp:Label ID="lblCreatedSince" runat="server" class="smallLabel" Text="Start Ship Date:" />&nbsp;&nbsp;
                                        </td>
                                        <td style="width: 70%;">
                                            <telerik:RadDatePicker ID="dpStartShipDate"
                                                Style="vertical-align: middle;"
                                                runat="server"
                                                Width="110px"
                                                TabIndex="4"
                                                UseEmbeddedScripts="false">
                                                <DateInput ID="diCreatedSince" TabIndex="4" DisplayDateFormat="MM/dd/yyyy" runat="server"></DateInput>
                                                <Calendar ID="calCreatedSince"
                                                    UseRowHeadersAsSelectors="False"
                                                    UseColumnHeadersAsSelectors="False"
                                                    ViewSelectorText="x" runat="server">
                                                </Calendar>
                                                <DatePopupButton ImageUrl="" HoverImageUrl="" TabIndex="4"></DatePopupButton>
                                            </telerik:RadDatePicker>
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Error.png" Visible="False" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <%--                                    <tr>
                                        <td style="padding-left: 5px; text-align: right; text-wrap: none;">
                                            <asp:Label ID="Label1" runat="server" class="smallLabel" Text="Date Maintained:" />&nbsp;&nbsp;
                                        </td>
                                        <td style="width: 70%;">
                                            <telerik:RadDatePicker ID="dpDateMaintained" Style="vertical-align: middle;"
                                                runat="server" 
                                                Width="100px" 
                                                TabIndex="4" 
                                                UseEmbeddedScripts="false">
                                                <DateInput ID="diDateMaintained" TabIndex="4" DisplayDateFormat="MM/dd/yyyy" runat="server"></DateInput>
                                                <Calendar ID="calDateMaintained" 
                                                    UseRowHeadersAsSelectors="False" 
                                                    UseColumnHeadersAsSelectors="False" 
                                                    ViewSelectorText="x" 
                                                    runat="server"></Calendar>
                                                <DatePopupButton ImageUrl="" HoverImageUrl="" TabIndex="4"></DatePopupButton>
                                            </telerik:RadDatePicker>
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Error.png" Visible="False" />

                                        </td>
                                    </tr>--%>
                                </table>
                            </ItemTemplate>
                        </telerik:RadPanelItem>
                    </Items>
                </telerik:RadPanelItem>
                <telerik:RadPanelItem Text="Find By Item/Image">
                    <Items>
                        <telerik:RadPanelItem>
                            <ItemTemplate>
                                <br />
                                <table id="Table1" runat="server" style="width: 100%; padding: 0;">
                                    <tr>
                                        <td style="padding-left: 5px; text-align: right; text-wrap: none;">
                                            <asp:Label ID="lblISN" runat="server" class="smallLabel" Text="ISN:" />&nbsp;&nbsp;
                                        </td>
                                        <td style="width: 70%;">
                                            <telerik:RadTextBox ID="txtISN" runat="server" Width="80" TabIndex="7" />&#160;&#160;
                                            <asp:ImageButton ID="imgAddISN" runat="server" ImageUrl="~/Images/Add.gif" OnClientClick="return addItemToListBox('ISN');" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 5px; text-align: right; text-wrap: none;">&nbsp; </td>
                                        <td style="width: 70%;">
                                            <telerik:RadListBox RenderMode="Lightweight" SelectionMode="Multiple" runat="server" Height="120px" Width="70px" ID="radLBISN" />
                                            &nbsp;&nbsp;
                                            <asp:ImageButton ID="imgResetISN" runat="server" ImageUrl="~/App_Themes/BonTonStandard/Images/Reset.gif" ImageAlign="Top" OnClientClick="return resetListBox('ISN');" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 5px; text-align: right; text-wrap: none;">
                                            <asp:Label ID="lblUPC" runat="server" class="smallLabel" Text="SKU/UPC:" />&nbsp;&nbsp;
                                        </td>
                                        <td style="width: 70%;">
                                            <telerik:RadTextBox ID="txtUPC" runat="server" Width="80" TabIndex="7" />&#160;&#160;
                                            <asp:ImageButton ID="imgAddUPC" runat="server" ImageUrl="~/Images/Add.gif" OnClientClick="return addItemToListBox('SKU');"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 5px; text-align: right; text-wrap: none;">&nbsp; </td>
                                        <td style="width: 70%;">
                                            <telerik:RadListBox RenderMode="Lightweight" SelectionMode="Multiple" runat="server" Height="120px" Width="70px" ID="radLBSKU" />
                                            &nbsp;&nbsp;
                                            <asp:ImageButton ID="imgResetUPC" runat="server" ImageUrl="~/App_Themes/BonTonStandard/Images/Reset.gif" ImageAlign="Top" OnClientClick="return resetListBox('SKU');" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 5px; text-align: right; text-wrap: none;">
                                            <asp:Label ID="Label2" runat="server" class="smallLabel" Text="Image:" />&nbsp;&nbsp;
                                        </td>
                                        <td style="width: 70%;">
                                            <telerik:RadTextBox ID="txtImageID" runat="server" Width="80" TabIndex="7" />&#160;&#160;
                                            <asp:ImageButton ID="imgAddImage" runat="server" ImageUrl="~/Images/Add.gif" OnClientClick="return addItemToListBox('Image');"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 5px; text-align: right; text-wrap: none;">&nbsp; </td>
                                        <td style="width: 70%;">
                                            <telerik:RadListBox RenderMode="Lightweight" SelectionMode="Multiple" runat="server" Height="120px" Width="70px" ID="radLBImage" />
                                            &nbsp;&nbsp;
                                            <asp:ImageButton ID="imgResetImage" runat="server" ImageUrl="~/App_Themes/BonTonStandard/Images/Reset.gif" ImageAlign="Top" OnClientClick="return resetListBox('Image');" />
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
<asp:HiddenField runat="server" ID="hdnVendorStyles" />
<telerik:RadCodeBlock runat="server" ID="rcb1">
    <script type="text/javascript">
        function spl_Loaded(s, e) {
            s.get_element().style.visibility = 'inherit';
        }
        function OnClientLoaded(sender, args) {
            var slidingPane = $find('<%= slPane.ClientID %>');
            if (slidingPane.get_expanded()) {
                sender.collapsePane(slidingPane.get_id());
            }
        }

    </script>
    <script type="text/javascript">
        function OnClientEntryAdding(sender, eventArgs) {
            //$telerik.$("#WorldMap").css('background-image', 'url(images/' + eventArgs.get_entry().get_value() + '.png)');
        }
    </script>
</telerik:RadCodeBlock>
<script type="text/javascript">

    function enableSearchControls(radioButtonGroup) {
        var radioButtonDept = $("input[id$='rdoDept']");

        if ((!radioButtonDept.is(":checked"))) {
            //enableCategorySearch();
        }
        else {
            //enableDepartmentSearch();
        }
    }
    function enableCategorySearch() {
        var rddtCategory = $find("<%= rddtCategory.ClientID%>");
        rddtCategory.set_enabled(true);
<%--            var cmbDept = $find("<%= cmbDept.ClientID%>");
            cmbDept.disable();--%>
        var cmbVendorStyle = $find("<%= cmbVendorStyle.ClientID%>");
        cmbVendorStyle.disable();
        var dpStartShipDate = $find("<%= dpStartShipDate.ClientID%>");
        dpStartShipDate.set_enabled(false);
<%--            var dpDateMaintained = $find("<%= dpDateMaintained.ClientID%>");
            dpDateMaintained.set_enabled(false);--%>
    }
    function enableDepartmentSearch() {
        var cmbDept = $find("<%= cmbDept.ClientID%>");
        cmbDept.enable();
        var cmbVendorStyle = $find("<%= cmbVendorStyle.ClientID%>");
        cmbVendorStyle.enable();
        var dpStartShipDate = $find("<%= dpStartShipDate.ClientID%>");
        dpStartShipDate.set_enabled(true);
<%--        var dpDateMaintained = $find("<%= dpDateMaintained.ClientID%>");
        dpDateMaintained.set_enabled(true);--%>
        var rddtCategory = $find("<%= rddtCategory.ClientID%>");
        rddtCategory.set_enabled(false);
    }
    function collapseRadPane() {
        var splitter = $find('<%= splM.ClientID%>');
        var pane = splitter.getPaneById("<%= pMainNav.ClientID %>");
        pane.collapse();

        var slidingPane = splitter.getPaneById("<%= pNav.ClientID()%>");
        slidingPane.collapse();

<%--        var resultsTab = $find('<%=rpbResultsTab.ClientID %>');
        if (resultsTab != null) {
            resultsTab.get_items().getItem(0).collapse();
        }--%>
    }
    function removeAllItems(listBoxID) {
        alert("select[id$='" + listBoxID + "']");
        $("select[id$='" + listBoxID + "']").empty();
    }
    function collapseCopyPrioritizationPane() {
<%--        var spl = $find('<%= splM.ClientID %>');
        <%--var pane = spl.getPaneById('<%= pNav.ClientID %>');
        pane.collapse();
        var mainPane = spl.getPaneById('<%= pMainNav.ClientID%>');
        mainPane.collapse();--%>
    }
    function hideSlidingPanes(sender, args) {
        var selectedButtonText = args.get_item().get_text();
        if (selectedButtonText.toUpperCase() == "RETRIEVE") {
            $find('<%= slZone.ClientID%>').collapsePane('<%= slPane.ClientID%>');
            <%--            $find('<%= splM.ClientID%>').getPaneById('<%= pMainNav.ClientID%>').collapse();--%>
        }
    }

    var lastPanel = '';

    function rpbResultsTab_OnClientItemClicked(s, e) {
        if (e.get_item().get_text() == 'Find By Item/Image' && e.get_item().get_text() != lastPanel)
            EnableCopyDetailsPanel(false);
        else
            if ((e.get_item().get_text() != lastPanel) && !(e.get_item().get_text() == 'Find By Web Category' && lastPanel == 'Find By Hierarchy') && !(e.get_item().get_text() == 'Find By Hierarchy' && lastPanel == 'Find By Web Category'))
                EnableCopyDetailsPanel(true);
        lastPanel = e.get_item().get_text();
    }

    function EnableCopyDetailsPanel(enable) {
        var pnl = $find('<%= slPane.ClientID %>');
        if (pnl != null) {
            var paneTab = pnl.getTabContainer();
            var pane = $get("RAD_SLIDING_PANE_TAB_ctl00_CopyPrioritizationSearchCtrl1_slPane");
            if (enable) {
                pane.removeAttribute("class");
                pane.setAttribute("class", "rspPaneTabContainer");
                $addHandlers(paneTab,
                  {
                      "mousedown": pnl._paneTab_OnMouseDown,
                      "mouseover": pnl._paneTab_OnMouseOver,
                      "mouseout": pnl._paneTab_OnMouseOut
                  }, pnl);
            } else {
                pane.setAttribute("class", "slPane_disable");
                $clearHandlers(paneTab);
            }
        }
    }
    function gmmSelectedIndexChanged(sender, args)
    {
       var ddlDept = $find('<%= cmbDept.ClientID%>');
        var ddlVendorStyle = $find('<%= cmbVendorStyle.ClientID%>');
        ddlDept.disable();
        ddlDept.clearSelection();
        ddlVendorStyle.clearSelection();
        ddlVendorStyle.disable();
        if (args.get_item().get_text() != "")
        {
            ddlDept.enable();
        }
    }
    function deptSelectedIndexChanged(sender, args) {
        var ddlVendorStyle = $find('<%= cmbVendorStyle.ClientID%>');
        ddlVendorStyle.disable();
        ddlVendorStyle.clearSelection();
        if (args.get_item().get_text() != "") {
            ddlVendorStyle.enable();
        }
    }
    function addVendorStyle(sender, args) {
        var listboxVendorStyle = document.getElementById('<%= lbVendorStyles.ClientID%>');
        var ddlVendorStyle = $find('<%= cmbVendorStyle.ClientID%>');
        var selValue = ddlVendorStyle.get_text().trim();
        var styleExists = 0;
        var vendorStyles = [];

        if (selValue != "")
        {
            for (var i = 0; i < listboxVendorStyle.options.length; i++) {
                vendorStyles[i] = listboxVendorStyle.options[i].value.trim();
                if (selValue == listboxVendorStyle.options[i].value.trim())
                {
                    styleExists = 1;
                }
            }

            if (styleExists == 0) {
                var ddlOption = document.createElement("option");
                ddlOption.text = selValue;
                ddlOption.value = selValue;
                listboxVendorStyle.options.add(ddlOption);
                vendorStyles.push(selValue);
            }
        }
        document.getElementById('<%= hdnVendorStyles.ClientID%>').value = vendorStyles.join(',');
        ddlVendorStyle.clearSelection();
        return false;
    }

    function clearListbox(sender, args) {
        document.getElementById('<%= hdnVendorStyles.ClientID%>').value = "";
        document.getElementById('<%= lbVendorStyles.ClientID%>').options.length = 0;
            return false;
        }

    function addItemToListBox(itemName)
    {
        var radLB;
        var txtBoxValue;
        if (itemName == 'ISN')
        {
            radLB = $find("<%= radLBISN.ClientID %>");
            txtBoxValue = document.getElementById("<%= txtISN.ClientID%>").value;
        }
        else if (itemName == 'SKU')
        {
            radLB = $find("<%= radLBSKU.ClientID%>");
            txtBoxValue = document.getElementById("<%= txtUPC.ClientID%>").value;
        }
        else if (itemName == 'Image')
        {
            radLB = $find("<%= radLBImage.ClientID %>");
            txtBoxValue = document.getElementById("<%= txtImage.ClientID%>").value;
        }
        if (radLB && txtBoxValue != "" && radLB.findItemByText(txtBoxValue) == null) {
            var item = new Telerik.Web.UI.RadListBoxItem();
            item.set_text(txtBoxValue);
            radLB.trackChanges();
            radLB.get_items().add(item);
            radLB.commitChanges();
        }
        return false;
    }
    function resetListBox(itemName)
    {
        var radLB;
        var txtBoxValue;
        if (itemName == 'ISN') {
            radLB = $find("<%= radLBISN.ClientID %>");
        }
        else if (itemName == 'SKU') {
            radLB = $find("<%= radLBSKU.ClientID%>");
        }
        else if (itemName == 'Image') {
            radLB = $find("<%= radLBImage.ClientID %>");
        }

        if (radLB)
        {
            var items = radLB.get_items();
            radLB.trackChanges();
            items.clear();
            radLB.commitChanges();
        }
        return false;
    }
</script>

