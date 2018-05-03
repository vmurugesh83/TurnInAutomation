<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ContentPage.Master"
    CodeBehind="TurnInQuery.aspx.vb" Inherits="TurnInProcessAutomation.TurnInQuery" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/ContentPage.Master" %>
<%@ Reference Control="~/WebUserControls/TurnInQuery/TurnInQueryCtrl.ascx" %>
<asp:Content ID="TurnInQueryForm" ContentPlaceHolderID="ContentArea" runat="Server">
<%--    <telerik:RadAjaxManagerProxy ID="rAjaxMgrPxyTurnInQuery" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rtbTurnInQuery">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdTurnInQuery" />
                    <telerik:AjaxUpdatedControl ControlID="grdPreMedia" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>--%>
    <telerik:RadSplitter ID="rsTurnInQuery" runat="server" SkinID="pageSplitter">
        <telerik:RadPane ID="rpHeader" runat="server" Height="90" Scrolling="None" Font-Bold="True">
            <div id="pageActionBar">
                <telerik:RadToolBar ID="rtbTurnInQuery" runat="server" OnClientButtonClicking="OnClientButtonClickingQry"
                    OnClientLoad="clientLoad" CssClass="SeparatedButtons">
                    <Items>
                        <telerik:RadToolBarButton runat="server" CommandName="Back" DisabledImageUrl="~/Images/BackButton.gif"
                            ImageUrl="~/Images/BackButton.gif" Text="Back" Value="">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Export" DisabledImageUrl="~/Images/Export_d.gif"
                            ImageUrl="~/Images/Export.gif" Text="Export" CssClass="rightAligned" Enabled="false">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="ExportExtr" DisabledImageUrl="~/Images/Export_d.gif"
                            ImageUrl="~/Images/Export.gif" Text="Export External" CssClass="rightAligned" Enabled="false">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="UPCreport" DisabledImageUrl="~/Images/Export_d.gif"
                            ImageUrl="~/Images/Export.gif" Text="UPC Report" CssClass="rightAligned" Enabled="true">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Reset" DisabledImageUrl="~/Images/Reset_d.gif"
                            ImageUrl="~/Images/Reset.gif" Text="Reset" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Retrieve" DisabledImageUrl="~/Images/Retrieve1_d.gif"
                            ImageUrl="~/Images/Retrieve1.gif" Text="Retrieve" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Print" DisabledImageUrl="~/Images/Export_d.gif"
                            ImageUrl="~/Images/Export.gif" Text="Print" CssClass="rightAligned" Enabled="false">
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
            </div>
            <div id="pageHeader">
                <asp:Label ID="lblPageHeader" runat="server" Text="Turn-In Query" />
            </div>
            <bonton:MessagePanel ID="mpeCommTurnInQuery" runat="server" />
        </telerik:RadPane>
        <telerik:RadPane ID="rpContent" runat="server">
            <telerik:RadGrid ID="grdTurnInQuery" runat="server" SkinID="CenteredWithScroll" AllowPaging="True"
                Height="550" Width="98%" Visible="false" AllowMultiRowSelection="true" ClientDataKeyNames="ImageSuffix" DataKeyNames="ImageSuffix">                
                <ClientSettings>
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true"  UseClientSelectColumnOnly="true" />
                </ClientSettings>
                <MasterTableView>
                    <EditFormSettings>
                        <EditColumn UniqueName="EditCommandColumn1">
                        </EditColumn>
                    </EditFormSettings>        
                    <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Next" PrevPageText="Previous" AlwaysVisible="true" 
            PagerTextFormat="Change page: {4} Displaying page {0} of {1}, items {2} to {3} of {5}." />
                    <NoRecordsTemplate>
                        no records retrieved</NoRecordsTemplate>
                    <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                    <Columns>
                        <telerik:GridClientSelectColumn UniqueName="selColumn">
                            <ItemStyle Width="30" HorizontalAlign="Center"/>
                            <HeaderStyle Width="30" HorizontalAlign="Center"/>
                        </telerik:GridClientSelectColumn>
                        <telerik:GridBoundColumn DataField="TIStatus" HeaderText="Status" UniqueName="TIStatus">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BatchNum" HeaderText="Batch #" UniqueName="BatchId">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AD_NUM" HeaderText="Ad #" UniqueName="AD_NUM">
                            <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Turn_in_Indicator" HeaderText="Turn In Indicator (Ecommerce/ Print)"
                            UniqueName="Turn_in_Indicator">
                            <HeaderStyle HorizontalAlign="Center" Width="100" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Turn_in_Date" HeaderText="Turn In Date" 
                            DataFormatString="{0:MM/dd/yy}" UniqueName="Turn_in_Date">
                            <HeaderStyle HorizontalAlign="Center" Width="70" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="OO" HeaderText="OO" UniqueName="OO">
                            <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="OH" HeaderText="OH" UniqueName="OH">
                            <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Ship_Date" HeaderText="In Store Date" UniqueName="Ship_Date">
                            <HeaderStyle HorizontalAlign="Center" Width="70" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ReserveFlag" HeaderText="Reserve Flag" UniqueName="ReserveFlag">
                            <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Department" HeaderText="Department" UniqueName="Department">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Buyer" HeaderText="Buyer/ Extension" UniqueName="Buyer">
                            <HeaderStyle HorizontalAlign="Center" Width="90" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Vendor" HeaderText="Vendor" UniqueName="Vendor">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Vendor_Style" HeaderText="Vendor Style" UniqueName="Vendor_Style">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ISN" HeaderText="ISN/Reserve ISN #" UniqueName="ISN">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Style_Desc" HeaderText="Style Description" UniqueName="Style_Desc">
                            <HeaderStyle HorizontalAlign="Center" Width="100" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Friendly_Product_Description" HeaderText="Friendly Description"
                            UniqueName="Friendly_Product_Description">
                            <HeaderStyle HorizontalAlign="Center" Width="100" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Color" HeaderText="Color" UniqueName="Color">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Friendly_color" HeaderText="Friendly Color Description"
                            UniqueName="Friendly_color">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SampleSize" HeaderText="Sample Size" UniqueName="SampleSize">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Feature_Render_Swatch" HeaderText="Feature/ Swatch/ Render"
                            UniqueName="Feature_Render_Swatch">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MerchId" HeaderText="Merch ID" UniqueName="MerchId">
                            <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Image_ID" HeaderText="Image ID" UniqueName="Image_ID">
                            <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RouteFromAd" HeaderText="Route From Ad" UniqueName="RouteFromAd">
                            <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="VT_Path" HeaderText="Virtual Ticket Image Path"
                            UniqueName="VT_Path">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Model_Category" HeaderText="Model Category" UniqueName="Model_Category">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="OnOff_Figure" HeaderText="On/ Off Figure" UniqueName="OnOff_Figure">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Feature_Web_Cat" HeaderText="Feature Web Category"
                            UniqueName="Feature_Web_Cat">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Feature_Image_ID" HeaderText="Feature Img ID"
                            UniqueName="Feature_Image_ID">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Hot_Rushed" HeaderText="Rush Sample " UniqueName="Hot_Rushed">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MerchantNotes" HeaderText="Merchant Notes" UniqueName="MerchantNotes"
                            ReadOnly="true" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="80px" />
                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ImageSuffix" HeaderText="Suffix" UniqueName="ImageSuffix" HtmlEncode="true" Visible="False">
                            <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RemoveMerchFlg" HeaderText="Killed" UniqueName="RemoveMerchFlg" HtmlEncode="true" Visible="False">
                            <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <telerik:RadGrid ID="grdPreMedia" runat="server" SkinID="CenteredWithScroll" AllowPaging="True"
                Height="550" Width="98%" Visible="false" AllowMultiRowSelection="true">
                <ClientSettings>
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                </ClientSettings>
                <MasterTableView DataKeyNames="TurnInMerchID" ClientDataKeyNames="TurnInMerchID,TIStatus,ImageSuffix">
                    <EditFormSettings>
                        <EditColumn UniqueName="EditCommandColumn1">
                        </EditColumn>
                    </EditFormSettings>
                    <NoRecordsTemplate>
                        no records retrieved</NoRecordsTemplate>
                    <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                    <Columns>
                        <telerik:GridClientSelectColumn UniqueName="selColumn">
                            <ItemStyle Width="30" HorizontalAlign="Center"/>
                            <HeaderStyle Width="30" HorizontalAlign="Center"/>
                        </telerik:GridClientSelectColumn>
                        <telerik:GridBoundColumn DataField="InWebCat" HeaderText="In WebCat" UniqueName="InWebCat">
                            <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TIStatus" HeaderText="Status" UniqueName="TIStatus">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AD_NUM" HeaderText="Ad #" UniqueName="AD_NUM">
                            <HeaderStyle HorizontalAlign="Center" Width="40" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PAGE_NUM" HeaderText="Page #" UniqueName="PAGE_NUM">
                            <HeaderStyle HorizontalAlign="Center" Width="40" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Image_ID" HeaderText="Image Number" UniqueName="" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ImageSuffix" HeaderText="Suffix" UniqueName="" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Friendly_Product_Description" HeaderText="Friendly Description" UniqueName="" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Width="120" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="OnOff_Figure" HeaderText="Image Type (On/Off)" UniqueName="" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Width="55" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Model_Category" HeaderText="Model Category" UniqueName="" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Friendly_color" HeaderText="Friendly Color Description" UniqueName="" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Width="100" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ImageNotes" HeaderText="Image Notes" UniqueName="ImageNotes" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Width="70" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AltView" HeaderText="Alt View" UniqueName="" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ClrCorrectFlg" HeaderText="Color Correct?" UniqueName="" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Feature_Render_Swatch" HeaderText="Feature/ Render / Swatch"
                            UniqueName="" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Feature_Image_ID" HeaderText="Feature Image Number"
                            UniqueName="" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Feature_Web_Cat" HeaderText="Feature Web Category"
                            UniqueName="" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMMNotes" HeaderText="EMM Notes" UniqueName="" HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ImageSuffix" HeaderText="Suffix" UniqueName="ImageSuffix" HtmlEncode="true" Visible="False">
                            <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RemoveMerchFlg" HeaderText="Killed" UniqueName="RemoveMerchFlg" HtmlEncode="true" Visible="False">
                            <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="True" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView></telerik:RadGrid>
            <telerik:RadGrid ID="grdUPCGrid" runat="server" AutoGenerateColumns="False" Width="100%" Height="600px" >
                <MasterTableView EditMode="InPlace" Name="grdUPC" DataKeyNames="adnbr,PgNbr,VendorISNNumber,UPCNumber" 
                    ClientDataKeyNames="adnbr,PgNbr,VendorISNNumber,UPCNumber" AllowSorting="false" Visible="True">
                <Columns>
                        <telerik:GridBoundColumn DataField="adnbr" DataType="System.Int64" HeaderText="Ad Number" UniqueName="Ad" ReadOnly="true" />
                        <telerik:GridBoundColumn DataField="PgNbr" DataType="System.Int64" HeaderText="Page Number" UniqueName="Page" ReadOnly="true" />
                        <telerik:GridBoundColumn DataField="VendorISNNumber" DataType="System.String" HeaderText="ISN Number" UniqueName="ISN" ReadOnly="true" />
                        <telerik:GridBoundColumn DataField="UPCNumber" DataType="System.Int64" HeaderText="UPC Number" UniqueName="UPC" ReadOnly="true" />
                </Columns>
                </MasterTableView>
                <ClientSettings>
                    <Scrolling AllowScroll="True"></Scrolling>
                </ClientSettings>
            </telerik:RadGrid>

            <asp:HiddenField ID="hdnExport" runat="server"></asp:HiddenField>
        </telerik:RadPane>
    </telerik:RadSplitter>
    <style>
.modalProgressGreyBackground
{
	background-color: Gray;
	filter: alpha(opacity=70);
	opacity: 0.7;	
}
</style>
<asp:Button ID="fakeButton" runat="server" Style="display: none" Visible="true" />

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
    <asp:LinkButton ID="lnkOK" runat="server" Text="OK" class="label" OnClientClick="HideExportModalMessage()" />
    &nbsp;&nbsp;
    <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" class="label" OnClientClick="CancelExportModalMessage();return false;"/>
</asp:Panel>

<ajaxtoolkit:ModalPopupExtender ID="mPopup" runat="server" PopupControlID="pnlMessage" TargetControlID="fakeButton"
    BackgroundCssClass="modalProgressGreyBackground" DropShadow="false">
</ajaxtoolkit:ModalPopupExtender>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function ShowExportModalMessage() {
                var mPopup = $find('<%= mPopup.ClientID %>')
                var mButton = $get('<%= fakeButton.ClientID %>')
                mButton.click()
            }
            function HideExportModalMessage() {
                var mPopup = $find('<%= mPopup.ClientID %>')
                mPopup.hide()
            }

            function CancelExportModalMessage() {
                var mPopup = $find('<%= mPopup.ClientID %>')
                mPopup.hide()
                if ($find("<%=grdPreMedia.ClientID%>") != null) {

                    var masterTable = $find("<%=grdPreMedia.ClientID%>").get_masterTableView();
                    var count = masterTable.get_dataItems().length;
                    var checkbox;
                    var item;
                    for (var i = 0; i < count; i++) {
                        item = masterTable.get_dataItems()[i];
                        checkbox = item.findElement("selColumnSelectCheckBox");
                        if (checkbox.checked) {
                            var oldStatus = item.getDataKeyValue("TIStatus");
                            masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[i], "TIStatus").innerHTML = oldStatus;
                        }
                    }
                }
            }

            function OnClientButtonClickingQry(sender, args) {
                var button = args.get_item();

                if (button.get_text() == "Export") {
                    if ($find("<%=grdPreMedia.ClientID%>") != null) {
                        UpdateStatus("Exported for Internal team");
                    }
                    document.getElementById('<%= hdnExport.ClientID %>').value = 'Export';
                    ShowExportModalMessage();
                    args.set_cancel(true);
                }

                if (button.get_text() == "UPC Report") {
                    document.getElementById('<%= hdnExport.ClientID %>').value = 'Report';
                    ShowExportModalMessage();
                    args.set_cancel(true);
                }

                if (button.get_text() == "Export External") {
                    UpdateStatus("Exported to Freelance");
                    document.getElementById('<%= hdnExport.ClientID %>').value = 'ExportExtr';
                    ShowExportModalMessage();
                    args.set_cancel(true);
                }

                if (button.get_text() == "Print") {
                    document.getElementById('<%= hdnExport.ClientID %>').value = 'Print';
                    ShowExportModalMessage();
                    args.set_cancel(true);
                }

                function UpdateStatus(msg) {
                    var masterTable;
                    if ($find("<%=grdTurnInQuery.ClientID%>") != null) {
                        masterTable = $find("<%=grdTurnInQuery.ClientID%>").get_masterTableView();
                    }
                    else if ($find("<%=grdPreMedia.ClientID%>") != null) {
                        masterTable = $find("<%=grdPreMedia.ClientID%>").get_masterTableView();
                    }
                    else { return; }
                    var count = masterTable.get_dataItems().length;
                    var checkbox;
                    var item;
                    for (var i = 0; i < count; i++) {
                        item = masterTable.get_dataItems()[i];
                        checkbox = item.findElement("selColumnSelectCheckBox");
                        if (checkbox.checked) {
                            masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[i], "TIStatus").innerHTML = msg;
                        }
                    }
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
