<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CopyPrioritizationSearchCtrl.ascx.vb" Inherits="TurnInProcessAutomation.CopyPrioritizationSearchCtrl" %>

<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="cmbDept">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbDept" />
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
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>

<telerik:RadCodeBlock runat="server" ID="rcb">
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
                            <br />
                            <span class="CheckBox" style="padding-left: 5px; padding-right: 5px;">
                                <asp:CheckBox ID="chkImageAvailable" name="chkImageAvailable" runat="server" />
                                <label for="chkImageAvailable">Image Available</label>
                            </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 5px; padding-right: 5px;">
                                <asp:CheckBox ID="chkImageNotAvailable" name="chkImageNotAvailable" runat="server" />
                                <label for="chkImageNotAvailable">Image Not Available</label></span><span class="Label">
                            </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 5px; padding-right: 5px;">
                                <asp:CheckBox ID="chkCopyReady" name="chkCopyReady" runat="server" />
                                <label for="chkCopyReady">Copy Ready</label></span><span class="Label">
                            </span>
                            <br />
                            <span class="CheckBox" style="padding-left: 5px; padding-right: 5px;">
                                <asp:CheckBox ID="chkCopyNotReady" name="chkCopyNotReady" runat="server" Checked="true" />
                                <label for="chkCopyNotReady">Copy Not Ready</label></span><span class="Label">
                            </span>
                        </div>
                    </asp:PlaceHolder>
                </telerik:RadSlidingPane>
            </telerik:RadSlidingZone>
        </div>
    </telerik:RadPane>
<%--    <telerik:RadSplitBar runat="server" ID="sBar1" CollapseMode="Forward" EnableResize="true" />--%>
    <telerik:RadPane ID="pMainNav" runat="server" Scrolling="None">
        <telerik:RadPanelBar runat="server" ID="rpbResultsTab" Height="100%" Width="100%"
            ExpandMode="FullExpandedItem">
            <Items>
                <telerik:RadPanelItem Expanded="True" Text="Find By Hierarchy">
                    <Items>
                        <telerik:RadPanelItem>
                            <ItemTemplate>
                                <br />
                                <table id="tblHierarchy" runat="server" style="width: 100%; padding: 0;">
                                    <tr>
                                        <td colspan="2">
                                            <asp:RadioButton runat="server" ID="rdoCategory" GroupName="radioHierarchy" Checked="true" Text="Search by Category" onclick="enableSearchControls(this);" />
                                        </td>
                                    </tr>
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
                                                ExpandNodeOnSingleClick="true"
                                                EnableFiltering="true"
                                                OnLoad="rddtCategory_Load" 
                                                OnClientEntryAdding="OnClientEntryAdding">
                                                <DropDownSettings OpenDropDownOnLoad="false" AutoWidth="Enabled"  />
                                                <FilterSettings Highlight="Matches" EmptyMessage="Type here to filter" />
                                            </telerik:RadDropDownTree>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            
                                        </td>
                                        <td>
                                            <br />
                                            <b><i>OR</i></b>
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:RadioButton runat="server" ID="rdoDept" GroupName="radioHierarchy" Text="Search by Department" onclick="enableSearchControls(this);" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" align="right" style="padding-left: 5px">
                                            <asp:Label ID="lblDept" runat="server" class="smallLabel" Text="Dept: " /></td>
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
                                                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"
                                                OnSelectedIndexChanged="cmbDept_SelectedIndexChanged">
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
                                                DropDownWidth="205px" OnItemsRequested="cmbVendorStyle_ItemsRequested">
                                                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                            </telerik:RadComboBox>
                                            <asp:ImageButton ID="imgAddVendorStyle" runat="server" ImageUrl="~/Images/Add.gif"
                                                OnClick="imgAddVendorStyle_Click" />
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
                                                ImageAlign="Top" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 5px; text-align: right; text-wrap: none;">
                                            <asp:Label ID="lblCreatedSince" runat="server" class="smallLabel" Text="Start Ship Date:" />&nbsp;&nbsp;
                                        </td>
                                        <td style="width: 70%;">
                                            <telerik:RadDatePicker ID="dpStartShipDate" 
                                                Style="vertical-align: middle;"
                                                runat="server" 
                                                Width="100px" 
                                                TabIndex="4" 
                                                UseEmbeddedScripts="false">
                                                <DateInput ID="diCreatedSince" TabIndex="4" DisplayDateFormat="MM/dd/yyyy" runat="server"></DateInput>
                                                <Calendar ID="calCreatedSince" 
                                                    UseRowHeadersAsSelectors="False" 
                                                    UseColumnHeadersAsSelectors="False" 
                                                    ViewSelectorText="x" runat="server"></Calendar>
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
                                                    <asp:ImageButton ID="imgAddISN" runat="server" ImageUrl="~/Images/Add.gif" OnClick="imgAddISN_Click"
                                                        ValidationGroup="AddISN" /><bonton:ToolTipValidator ID="ttvISN" runat="server" ControlToEvaluate="txtISN" ValidationGroup="AddISN"
                                                            OnServerValidate="ttvISN_ServerValidate" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 5px; text-align: right; text-wrap: none;">&nbsp; </td>
                                        <td style="width: 70%;">
                                            <asp:ListBox ID="listBoxISN" runat="server" 
                                                TabIndex="8" 
                                                Height="120px" 
                                                SelectionMode="Multiple" 
                                                Width="100" />
                                            &nbsp;&nbsp;
                                            <asp:ImageButton ID="imgResetISN" runat="server" ImageUrl="~/App_Themes/BonTonStandard/Images/Reset.gif" ImageAlign="Top" OnClick="imgResetISN_Click"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 5px; text-align: right; text-wrap: none;">
                                            <asp:Label ID="lblUPC" runat="server" class="smallLabel" Text="UPC:" />&nbsp;&nbsp;
                                        </td>
                                        <td style="width: 70%;">
                                                    <telerik:RadTextBox ID="txtUPC" runat="server" Width="80" TabIndex="7" />&#160;&#160;
                                                    <asp:ImageButton ID="imgAddUPC" runat="server" ImageUrl="~/Images/Add.gif" OnClick="imgAddUPC_Click"
                                                        ValidationGroup="AddUPC" /><bonton:ToolTipValidator ID="ttvUPC" runat="server" ControlToEvaluate="txtUPC" ValidationGroup="AddUPC"
                                                            OnServerValidate="ttvUPC_ServerValidate" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 5px; text-align: right; text-wrap: none;">&nbsp; </td>
                                        <td style="width: 70%;">
                                            <asp:ListBox ID="listBoxUPC" runat="server" TabIndex="10" Height="120px" SelectionMode="Multiple" Width="100" />
                                            &nbsp;&nbsp;
                                            <asp:ImageButton ID="imgResetUPC" runat="server" ImageUrl="~/App_Themes/BonTonStandard/Images/Reset.gif" ImageAlign="Top" OnClick="imgResetUPC_Click"/>
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
                                                    <asp:ImageButton ID="imgAddImage" runat="server" ImageUrl="~/Images/Add.gif" OnClick="imgAddImage_Click"
                                                        ValidationGroup="AddImage" /><bonton:ToolTipValidator ID="ttvImage" runat="server" ControlToEvaluate="txtImageID" ValidationGroup="AddImage"
                                                            OnServerValidate="ttvImage_ServerValidate" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 5px; text-align: right; text-wrap: none;">&nbsp; </td>
                                        <td style="width: 70%;">
                                            <asp:ListBox ID="listBoxImage" runat="server" 
                                                TabIndex="10" 
                                                Height="120px" 
                                                SelectionMode="Multiple" 
                                                Width="100" />
                                            &nbsp;&nbsp;
                                            <asp:ImageButton ID="imgResetImage" runat="server" ImageUrl="~/App_Themes/BonTonStandard/Images/Reset.gif" ImageAlign="Top" OnClick="imgResetImage_Click"/>
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
                 enableCategorySearch();
        }
        else {
                 enableDepartmentSearch();
        }
    }
    function enableCategorySearch()
    {
            var rddtCategory = $find("<%= rddtCategory.ClientID%>");
            rddtCategory.set_enabled(true);
            var cmbDept = $find("<%= cmbDept.ClientID%>");
            cmbDept.disable();
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
    function collapseRadPane()
    {
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
    function removeAllItems(listBoxID)
    {
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
</script>

