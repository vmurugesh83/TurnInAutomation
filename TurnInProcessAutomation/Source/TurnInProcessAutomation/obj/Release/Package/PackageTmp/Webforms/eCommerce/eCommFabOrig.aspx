<%@ Page Title="Fabrication and Origination" Language="vb" AutoEventWireup="false" MasterPageFile="~/ContentPage.Master" CodeBehind="eCommFabOrig.aspx.vb"
    ValidateRequest="false" Inherits="TurnInProcessAutomation.eCommFabOrig" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/ContentPage.Master" %>
<%@ Reference Control="~/WebUserControls/eCommerce/eCommFabOrigCtrl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentArea" runat="server">

    <telerik:RadAjaxManagerProxy ID="RadAjaxMgrProxyFabricationOrigination" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ajxFabOrig">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdFabOrig" LoadingPanelID="ralpLoadPnl" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <telerik:RadSplitter ID="rsFabricationOrigination" runat="server" SkinID="pageSplitter">
        <telerik:RadPane ID="rpHeader" runat="server" SkinID="pageHeaderAreaPane" Height="20%">

            <div id="pageActionBar">
                <telerik:RadToolBar ID="rtbFabricationOrigination" runat="server"
                    OnClientLoad="clientLoad" CssClass="SeparatedButtons" OnClientButtonClicking="OnClientButtonClicking">
                    <Items>
                        <telerik:RadToolBarButton runat="server" CommandName="Reset" DisabledImageUrl="~/Images/Reset_d.gif"
                            ImageUrl="~/Images/Reset.gif" Text="Reset" CssClass="rightAligned" CausesValidation="false"
                            TabIndex="15">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Retrieve" DisabledImageUrl="~/Images/Retrieve1_d.gif"
                            ImageUrl="~/Images/Retrieve1.gif" Text="Retrieve" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="CancelAll" DisabledImageUrl="~/Images/Cancel_d.gif"
                            ImageUrl="~/Images/Cancel.gif" Text="Cancel" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Edit" DisabledImageUrl="~/Images/Edit3_d.gif"
                            ImageUrl="~/Images/Edit3.gif" Text="Edit" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Save" DisabledImageUrl="~/Images/Save_d.gif"
                            ImageUrl="~/Images/Save.gif" Text="Save" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Export" DisabledImageUrl="~/Images/Export_d.gif"
                            ImageUrl="~/Images/Export.gif" Text="Export" CssClass="rightAligned" Enabled="false">
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
            </div>
            <asp:Panel ID="pnlTabStrip" runat="server" CssClass="pageTabStrip" Style="margin: 0;">
                <telerik:RadTabStrip ID="rtsAPAdjustment" runat="server" MultiPageID="rmpPreTurnInCreate"
                    SelectedIndex="1" AutoPostBack="true">
                    <Tabs>
                        <telerik:RadTab runat="server" Text="Fabrication/Origination" PageViewID="pvFabOrig" Font-Bold="True"
                            Visible="false" />
                    </Tabs>
                </telerik:RadTabStrip>
            </asp:Panel>
            <div id="pageHeader" style="height: 100px;">
                <asp:Label ID="lblPageHeader" runat="server" Text="E-Comm Fabrication & Origination " />
                <bonton:MessagePanel ID="mpFabricationOrigination" runat="server" />
                <br />
            </div>

        </telerik:RadPane>
        <telerik:RadPane ID="rpContent" runat="server">
            <telerik:RadMultiPage ID="rmpFabricationOrigination" SelectedIndex="0" runat="server" Height="90%">

                <telerik:RadPageView ID="pvFabOrig" runat="server" Height="100%">
                    <table align="center">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlFlood" runat="server" Visible="false">
                                    <table align="center" width="1348">
                                        <tr>
                                            <td>
                                                <br />
                                                <br />
                                                <table id="Table1" runat="server">
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="btnResetFlood" runat="server" ToolTip="Reset" ImageUrl="~/Images/Reset.gif" />
                                                            <br />
                                                        </td>
                                                        <td>
                                                            <telerik:RadButton ID="btnFlood" runat="server" Text="Flood" Font-Bold="true" Width="60px"
                                                                Height="25px" />
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="rtxtFloodFabrication" ToolTip="Fabrication Description"
                                                                EmptyMessageStyle-Font-Italic="true" EmptyMessage="Fabrication Description"
                                                                runat="server" MaxLength="255" Width="180px">
                                                                <ClientEvents OnBlur="ValidateText" />
                                                            </telerik:RadTextBox>
                                                            <bonton:ToolTipValidator ID="ttvFabrication" runat="server" ControlToEvaluate="rtxtFloodFabrication"
                                                                ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                                                ValidationGroup="FloodUpdate" />
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="rtxtFloodOrigination" ToolTip="Origination Description"
                                                                EmptyMessageStyle-Font-Italic="true" EmptyMessage="Origination Description"
                                                                runat="server" MaxLength="2000" Width="170px">
                                                                <ClientEvents OnBlur="ValidateAsciiChars" />
                                                            </telerik:RadTextBox>
                                                            <bonton:ToolTipValidator ID="ttvOrigination" runat="server" ControlToEvaluate="rtxtFloodOrigination"
                                                                ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                                                ValidationGroup="FloodUpdate" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                                    <table align="center">
                                        <tr>
                                            <td>
                                                <telerik:RadAjaxPanel runat="server" EnableAJAX="true">
                                                    <telerik:RadGrid ID="grdFabOrig" runat="server"
                                                        AllowPaging="True"
                                                        SkinID="CenteredWithScrollInlineEdit"
                                                        Height="680" Width="1348"
                                                        ShowFooter="False"
                                                        AllowSorting="true"
                                                        Visible="true"
                                                        AllowMultiRowEdit="true"
                                                        OnItemCreated="grdFabOrig_ItemCreated"
                                                        OnItemDataBound="grdFabOrig_ItemDataBound"
                                                        GridLines="None"
                                                        AutoGenerateColumns="false"
                                                        AllowMultiRowSelection="true">
                                                        <MasterTableView DataKeyNames="ISN,UPC"
                                                            ClientDataKeyNames="ISN,UPC"
                                                            EditMode="InPlace"
                                                            PageSize="50"
                                                            CommandItemDisplay="None"
                                                            TableLayout="Fixed" CellPadding="7"
                                                            EditFormSettings-FormMainTableStyle-CellPadding="7"
                                                            EditFormSettings-FormTableStyle-CellPadding="7">

                                                            <EditFormSettings>
                                                                <EditColumn UniqueName="EditCommandColumn1">
                                                                </EditColumn>
                                                            </EditFormSettings>
                                                            <NoRecordsTemplate>
                                                                no records retrieved
                                                            </NoRecordsTemplate>
                                                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="StartShipDate" HeaderText="PO Start Ship Date"
                                                                    UniqueName="StartShipDate" ReadOnly="true">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="70" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="ISN" HeaderText="ISN" UniqueName="ISN" ReadOnly="true">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="40" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="VendorStyleNumber" HeaderText="Vendor Style"
                                                                    UniqueName="VendorStyleNumber" ReadOnly="true">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="70" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="ISNDesc" HeaderText="ISN Desc" UniqueName="ISNDesc" ReadOnly="true">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="130" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="FabricationSource" HeaderText="Fab Source" UniqueName="FabSource" ReadOnly="true">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="40" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="Fabrication" HeaderText="Fabrication" UniqueName="Fabrication">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="180" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="OriginationSource" HeaderText="Orig Source" UniqueName="OrigSource" ReadOnly="true">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="40" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="Origination" HeaderText="Origination" UniqueName="Origination">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="60" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="LastModBy" HeaderText="Modified By" UniqueName="LastModBy" ReadOnly="true">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="70" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridDateTimeColumn DataField="LastModTs" HeaderText="Last Modified Date" UniqueName="LastModDate" ReadOnly="true"
                                                                    DataFormatString="{0:MM/dd/yyyy}" DataType="System.DateTime">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="70" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridDateTimeColumn>
                                                                <telerik:GridBoundColumn DataField="UPC" HeaderText="UPC" UniqueName="UPC" ReadOnly="true" Display="false">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="70" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="StyleSkuFabDescription" HeaderText="StyleSkuFabDescription" UniqueName="StyleSkuFabDescription" ReadOnly="true" Display="false">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="70" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="StyleSkuOrigDescription" HeaderText="StyleSkuOrigDescription" UniqueName="StyleSkuOrigDescription" ReadOnly="true" Display="false">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="70" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="LabelId" HeaderText="Label Id" UniqueName="LabelId" ReadOnly="true" Display="false">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="10" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </telerik:GridBoundColumn>
                                                            </Columns>

                                                        </MasterTableView>
                                                        <ClientSettings EnableRowHoverStyle="false" EnableAlternatingItems="false">
                                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="100%" />
                                                            <Selecting AllowRowSelect="True" EnableDragToSelectRows="false"></Selecting>
                                                        </ClientSettings>
                                                    </telerik:RadGrid>
                                                </telerik:RadAjaxPanel>
                                            </td>
                                        </tr>
                                    </table>

                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </telerik:RadPane>
    </telerik:RadSplitter>

    <asp:Button ID="btnExport" runat="server" Style="display: none" Visible="true" />

    <asp:Panel ID="pnlMessage" runat="server"  Height="150px"
        Width="300px" Style="display: none; background-color: AntiqueWhite; text-align: center;">
        <br />
        <asp:Label ID="lblProgressMessage" runat="server" class="label" Text="Choose your export format" Font-Underline="True" />
        <br />
        <br />
        <asp:RadioButtonList ID="rblExport" class="label" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Text="Excel" Value="Excel" Selected="True" />
            <asp:ListItem Text="PDF" Value="PDF" />
        </asp:RadioButtonList>
        <br />
        <asp:LinkButton ID="lnkOK" runat="server" Text="OK" class="label" OnClientClick="HideModalMessage()" />
        &nbsp;&nbsp;
        <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" class="label" OnClientClick="CancelExportModalMessage();return false;"/>
    </asp:Panel>

    <ajaxtoolkit:ModalPopupExtender ID="mPopup" runat="server" PopupControlID="pnlMessage" TargetControlID="btnExport"
        BackgroundCssClass="modalProgressGreyBackground" DropShadow="false">
    </ajaxtoolkit:ModalPopupExtender>

    <telerik:RadCodeBlock ID="rcbSaveChanges" runat="server">
        
        <style type="text/css">
            .modalProgressGreyBackground {
                background-color: Gray;
                filter: alpha(opacity=70);
                opacity: 0.7;
            }
        </style>
        <script type="text/javascript">

            function OnClientFloodClick(sender, eventArgs) {
                if (CheckForRowsInEditMode(0)) {
                    eventArgs.set_cancel(true);
                } else {
                    eventArgs.set_cancel(!confirm('Are you sure? This action will automatically save data for every row selected.'));
                }
            }

            function CheckForRowsInEditMode(expectedRowCount) {
                var grdFabOrig = $find("<%=grdFabOrig.ClientID%>");
                if (grdFabOrig) {
                    var MasterTable = grdFabOrig.get_masterTableView();
                    var editItems = MasterTable.get_editItems();

                    if (editItems.length > expectedRowCount) {
                        if (expectedRowCount > 0) {
                            alert("One or more rows are in Edit mode. Please Save All/Cancel All changes before performing any other action.");
                        } else {
                            alert("A row is in Edit mode. Please Save/Cancel changes before performing any other action.");
                        }
                        return true;
                    }
                }

                return false;
            }

            function ValidateText(sender, eventArgs) {
                var str = sender.get_value();
                var isValid = true;

                for (var i = 0; i < str.length; i++) {
                    asciiNum = str.charCodeAt(i);
                    if ((asciiNum == 10) || (asciiNum == 13) || (asciiNum > 31 && asciiNum < 94) || (asciiNum > 94 && asciiNum < 127)) {
                    }
                    else {
                        isValid = false;
                    }
                }
                //sender.get_id()
                if (!isValid) {
                    alert("Invalid Character found!");
                    sender.focus();
                }
                sender.set_value(str.replace(/([^a-zA-Z\d\s:]*[^\s:\-])([^\s:\-]*)/g, function ($0, $1, $2) { return $1.toUpperCase() + $2.toLowerCase(); }));
                return isValid;
            }

            function DisableEditButton() {
                var toolBar = $find("<%=rtbFabricationOrigination.ClientID%>");
                if (toolBar) {
                    var button = toolBar.findItemByText("Edit");
                    button.disable();
                }
            }

            function ShowModalMessage() {
                var mPopup = $find('<%= mPopup.ClientID %>')
                var mButton = $get('<%= btnExport.ClientID%>')
                mButton.click()
            }

            function HideModalMessage() {
                var mPopup = $find('<%= mPopup.ClientID %>')
                mPopup.hide()
            }
        </script>

    </telerik:RadCodeBlock>
</asp:Content>
