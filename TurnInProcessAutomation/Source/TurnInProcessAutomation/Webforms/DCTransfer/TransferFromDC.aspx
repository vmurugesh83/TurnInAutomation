<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ContentPage.Master" ValidateRequest="false"
 CodeBehind="TransferFromDC.aspx.vb" Inherits="TurnInProcessAutomation.TransferFromDC" %>

<%@ MasterType VirtualPath="~/ContentPage.Master" %>
<%@ Reference Control="~/WebUserControls/DCTransfer/DCTransferSearchControl.ascx" %>
 <asp:Content ID="TransferFromDCForm" ContentPlaceHolderID="ContentArea"
    runat="Server">
    <telerik:RadSplitter ID="rseTransferFromDC" runat="server" SkinID="pageSplitter">
        <telerik:RadPane ID="rpHeader" runat="server" Height="90"  Scrolling="None" Font-Bold="True">
            <div id="pageActionBar">
                <telerik:RadToolBar ID="rtbeTransferFromDC" runat="server" 
                    OnClientLoad="clientLoad" OnClientButtonClicking="OnClientRadToolBarClick" CssClass="SeparatedButtons">
                    <Items>
                        <telerik:RadToolBarButton runat="server" CommandName="Back" DisabledImageUrl="~/Images/BackButton.gif"
                            ImageUrl="~/Images/BackButton.gif" Text="Back">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Reset" DisabledImageUrl="~/Images/Reset_d.gif"
                            ImageUrl="~/Images/Reset.gif" Text="Reset" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Export" DisabledImageUrl="~/Images/Export_d.gif"
                            ImageUrl="~/Images/Export.gif" Text="Export" CssClass="rightAligned" Enabled="false" Visible="False">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="CreateBatch" DisabledImageUrl="~/Images/Submit.gif"
                            ImageUrl="~/Images/Submit.gif" Text="Create Turn-in" CssClass="rightAligned"  Visible="False" OnClientClick="ShowContinue();">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="CreateBatch" DisabledImageUrl="~/Images/Retrieve2_d.gif" 
                            ImageUrl="~/Images/Retrieve2.gif" Text="Create Batch" CssClass="rightAligned" Visible="False">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="CancelAll" DisabledImageUrl="~/Images/Cancel_d.gif"
                            ImageUrl="~/Images/Cancel.gif" Text="Cancel All" CssClass="rightAligned" Visible="False">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="SaveAll" DisabledImageUrl="~/Images/Save_d.gif"
                            ImageUrl="~/Images/Save.gif" Text="Save All" CssClass="rightAligned" Visible="False">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="EditAll" DisabledImageUrl="~/Images/Edit3_d.gif"
                            ImageUrl="~/Images/Edit3.gif" Text="Edit All" CssClass="rightAligned" Visible="False">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Retrieve" DisabledImageUrl="~/Images/Retrieve1_d.gif"
                            ImageUrl="~/Images/Retrieve1.gif" Text="Retrieve" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
            </div>
            <div id="pageHeader">
                <asp:Label ID="lblPageHeader" runat="server" Text="Transfer from Distribution Center" />
                <bonton:MessagePanel ID="mpeTransferFromDC" runat="server" />
            </div>
        </telerik:RadPane>
        <telerik:RadPane ID="rpContent" runat="server">
        <p>
        <asp:Panel ID="CreateBatchPanel" runat="server" Visible="false" GroupingText="Create Batch" Width="500px">
                 <table cellpadding="4" >
            <tr>
                <td align="right"><asp:Label ID="lblAdNoLabel" runat="server" class="smallLabel" Text="Ad#:" /></td>
                <td>
                    <telerik:RadComboBox ID="rcbAds" runat="server"
                        OnClientBlur="OnClientBlurHandler" 
                        AutoPostBack="true" EnableLoadOnDemand="true" ShowMoreResultsBox="false" EnableVirtualScrolling="false"
                        DropDownWidth="400" Width="150" HighlightTemplatedItems="true" Filter="Contains">
                        <HeaderTemplate>
                            <table style="width: 350px" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="width: 50px;">
                                        Ad #
                                    </td>
                                    <td style="width: 300px;">
                                        Description
                                    </td>
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
                <td align="right">
                    <asp:Label ID="Label1" runat="server" class="smallLabel" Text="Page:" />
                </td>
                <td>
                    <telerik:RadComboBox ID="rcbPageNumber" runat="server" OnClientBlur="OnClientBlurHandler"
                        DropDownWidth="300" Width="150" Enabled="false" HighlightTemplatedItems="true" Filter="Contains">
                        <HeaderTemplate>
                            <table style="width: 350px" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="width: 50px;">
                                        Page #
                                    </td>
                                    <td style="width: 300px;">
                                        Description
                                    </td>
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
                <td>
                <telerik:RadButton ID="radBtnCreateBatch" runat="server" Text="Create" OnClientClicked="RTS.Common.blockUI" />
                </td>
            </tr>
            </table>
                
                
        </asp:Panel>        
        </p>
       <div>
    <!-- Script Manager for AJAX enabled Grids -->
                   
    <telerik:RadGrid ID="RadUPCGrid" runat="server" AutoGenerateColumns="False" Height="610px" AllowMultiRowSelection="true" 
    ExportSettings-IgnorePaging = "true" Visible = "false" Width="1500px" AllowPaging="true" EnableHeaderContextMenu="true" PageSize="25">
    <MasterTableView EditMode="InPlace" Name="grdUPC" DataKeyNames="ISN, ColorCode,Color, ISNDesc, DepartmentID,SelectedUPC" 
                    ClientDataKeyNames="ISN, ColorCode"  AllowSorting="true"  TableLayout="Fixed">
    <EditFormSettings>
       <EditColumn UniqueName="EditCommandColumn1">
        </EditColumn>
    </EditFormSettings>
    <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
    <Columns>
     <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn1" ItemStyle-Width="30px" HeaderStyle-Width="30px" ></telerik:GridClientSelectColumn>
     <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Images/Edit1.gif"
                                CancelImageUrl="~/Images/Cancel1.gif" UpdateImageUrl="~/Images/CheckMark.gif"
                                HeaderText="" UniqueName="EditColumn" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                                <ItemStyle HorizontalAlign="Center" Width="60" />
    </telerik:GridEditCommandColumn>
    <telerik:GridBoundColumn DataField="Vendor" HeaderText="Vendor" UniqueName="Vendor" ReadOnly="true" ItemStyle-Width="120px" HeaderStyle-Width="120px"/>
    <telerik:GridBoundColumn DataField="ISN" HeaderText="ISN" UniqueName="ISN" ReadOnly="true" ItemStyle-Width="80px" HeaderStyle-Width="80px"/>
    <telerik:GridBoundColumn DataField="ISNDesc" HeaderText="ISN Description" UniqueName="ISNDesc" ReadOnly="true" ItemStyle-Width="150px" HeaderStyle-Width="150px"/>
    <telerik:GridBoundColumn DataField="Color" HeaderText="Color" UniqueName="Color" ReadOnly="true" ItemStyle-Width="120px" HeaderStyle-Width="120px"/>
    <telerik:GridBoundColumn DataField="OH" HeaderText="OH Qty." UniqueName="OH" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-Width="50px"/>
    <telerik:GridBoundColumn DataField="EMMID" HeaderText="IMM ID" UniqueName="EMMID" ReadOnly="true" ItemStyle-Width="40px" HeaderStyle-Width="40px"/>
    <telerik:GridBoundColumn DataField="EMMDesc" HeaderText="Name" UniqueName="EMMDesc" ReadOnly="true"  ItemStyle-Width="130px" HeaderStyle-Width="130px"/>
    <telerik:GridBoundColumn DataField="SellArea" HeaderText="Sell Area" UniqueName="SellArea" ReadOnly="true"  ItemStyle-Width="50px" HeaderStyle-Width="50px"/>
    <telerik:GridBoundColumn DataField="DepartmentID" HeaderText="Dept." UniqueName="DepartmentID" ReadOnly="true"  ItemStyle-Width="50px" HeaderStyle-Width="50px"/>
    <telerik:GridBoundColumn DataField="DepartmentDesc" HeaderText="Dept. Name" UniqueName="DepartmentDesc" ReadOnly="true"   ItemStyle-Width="130px" HeaderStyle-Width="130px"  />
    <telerik:GridBoundColumn DataField="BuyerID" HeaderText="Buyer" UniqueName="BuyerID" ReadOnly="true"   ItemStyle-Width="60px" HeaderStyle-Width="60px"/>
    <telerik:GridBoundColumn DataField="BuyerDesc" HeaderText="Name" UniqueName="BuyerDesc" ReadOnly="true"   ItemStyle-Width="130px" HeaderStyle-Width="130px"/>


    <telerik:GridBoundColumn DataField="CMGID" HeaderText="CMG" UniqueName="CMGID" ReadOnly="true"   ItemStyle-Width="30px" HeaderStyle-Width="30px"/>
    <telerik:GridBoundColumn DataField="CMGDesc" HeaderText="Description" UniqueName="CMGDesc" ReadOnly="true"   ItemStyle-Width="30px" HeaderStyle-Width="30px"/>
    <telerik:GridBoundColumn DataField="ProductID" HeaderText="Product" UniqueName="ProductID" ReadOnly="true"   ItemStyle-Width="30px" HeaderStyle-Width="30px"/>
    <telerik:GridBoundColumn DataField="ProductDesc" HeaderText="Description" UniqueName="ProductDesc" ReadOnly="true"   ItemStyle-Width="30px" HeaderStyle-Width="30px"/>
    <telerik:GridBoundColumn DataField="ProductStatus" HeaderText="Status" UniqueName="ProductStatus" ReadOnly="true"  ItemStyle-Width="30px" HeaderStyle-Width="30px"/>


    <telerik:GridTemplateColumn HeaderText="UPC" ItemStyle-Width="205px" UniqueName="UPC" HeaderStyle-Width ="205px">
    <ItemTemplate>
    <%# DataBinder.Eval(Container.DataItem, "SelectedUPC")%>
    </ItemTemplate>
    <EditItemTemplate>
     <asp:HiddenField ID="hfUPC" runat="server" Value='<%# Eval("SelectedUPC") %>' />
    <telerik:RadComboBox ID="rcbUPC" runat="server" Width="200" MarkFirstMatch="true" class="RadComboBox_Vista" AllowCustomText="false"
                        AppendDataBoundItems="False" Filter="StartsWith">
                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
    </telerik:RadComboBox>    
    </EditItemTemplate>
    </telerik:GridTemplateColumn>
    <telerik:GridBoundColumn DataField="UPCDesc" HeaderText="Description" UniqueName="UPCDesc" ReadOnly="true" ItemStyle-Width="250px" HeaderStyle-Width="250px"/>


    <telerik:GridBoundColumn DataField="UPCStatus" HeaderText="Status" UniqueName="UPCStatus" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-Width="50px"/>
    <telerik:GridBoundColumn DataField="MerchType" HeaderText="Merch Type" UniqueName="MerchType" ReadOnly="true" ItemStyle-Width="120px" HeaderStyle-Width="120px"/>
    <telerik:GridBoundColumn DataField="MerchStatus" HeaderText="Merch Status" UniqueName="MerchStatus" ReadOnly="true" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
    <telerik:GridBoundColumn DataField="Inventory" HeaderText="Inventory" UniqueName="Inventory" ReadOnly="true"/>
    <telerik:GridBoundColumn DataField="SalesLast4Wks" HeaderText="Sales Last 4 Wks" UniqueName="SalesLast4Wks" ReadOnly="true"/>
    <telerik:GridBoundColumn DataField="OO" HeaderText="Ordered Qty" UniqueName="OO" ReadOnly="true" ItemStyle-Width="60px" HeaderStyle-Width="60px"/>
    <telerik:GridNumericColumn DataField="TotalOwnedAmount" HeaderText="Total Owned $" UniqueName="TotalOwnedAmount" ReadOnly="true" DataType="System.Decimal" NumericType="Currency" />
    <telerik:GridBoundColumn DataField="OriginalTicketPrice" HeaderText="Original Tkt Prc" UniqueName="OriginalTicketPrice" ReadOnly="true"/>
    <telerik:GridBoundColumn DataField="OwnedRetailAmount" HeaderText="Original Retail $" UniqueName="OwnedRetailAmount" ReadOnly="true"/>
    <telerik:GridBoundColumn DataField="PurchaseOrderID" HeaderText="PO #" UniqueName="PurchaseOrderID" ReadOnly="true"/>
    <telerik:GridBoundColumn DataField="POShipDate" HeaderText="PO Ship Dte" UniqueName="POShipDate" ReadOnly="true"/>
    <telerik:GridBoundColumn DataField="ReplenishFlag" HeaderText="Replenish Flag" UniqueName="ReplenishFlag" ReadOnly="true"/>
    <telerik:GridBoundColumn DataField="SKU" HeaderText="SKU" UniqueName="SKU" ReadOnly="true"/>
    

    <telerik:GridBoundColumn DataField="VendorStyle" HeaderText="Vendor Style" UniqueName="VendorStyle" ReadOnly="true" ItemStyle-Width="120px" HeaderStyle-Width="120px" />
     <telerik:GridTemplateColumn HeaderText="From Location" ItemStyle-Width="200px"  UniqueName="TransferFromDC" HeaderStyle-Width ="200px">
    <ItemTemplate>
         <%# DataBinder.Eval(Container.DataItem, "TransferFromDC")%>
    </ItemTemplate>
    <EditItemTemplate>
    <asp:HiddenField ID="hfFromLoc" runat="server" Value='<%# Eval("TransferFromDC") %>' />
    <telerik:RadComboBox ID="rcbFromLoc" runat="server" Width="200" MarkFirstMatch="true" class="RadComboBox_Vista" AllowCustomText="false"
                        AppendDataBoundItems="False" Filter="StartsWith">
                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
    </telerik:RadComboBox>
    </EditItemTemplate>
    </telerik:GridTemplateColumn>
    <telerik:GridBoundColumn DataField="ExtraColumn" HeaderText="?" UniqueName="extracolumn" ReadOnly="true"/>
    <telerik:GridTemplateColumn HeaderText="To Location" ItemStyle-Width="205px" UniqueName="TransferToDC" HeaderStyle-Width ="205px">
    <EditItemTemplate>
    <asp:HiddenField ID="hfToLoc" runat="server" Value='<%# Eval("TransferToDC") %>' />
    <telerik:RadComboBox ID="rcbToLoc" runat="server" Width="200" MarkFirstMatch="true" class="RadComboBox_Vista" AllowCustomText="false"
                        AppendDataBoundItems="False" Filter="StartsWith">
                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
    </telerik:RadComboBox>
    </EditItemTemplate>
    <ItemTemplate>
        <%# DataBinder.Eval(Container.DataItem, "TransferToDC")%>
    </ItemTemplate>
    </telerik:GridTemplateColumn>
    <telerik:GridNumericColumn HeaderText="Transfer Qty." DataField="TransferQty"  UniqueName="TransferQtyClm" ItemStyle-Width="80px" HeaderStyle-Width ="80px" EmptyDataText="1" >
    </telerik:GridNumericColumn>
    <telerik:GridTemplateColumn HeaderText="Comments" ItemStyle-Width="250px" HeaderStyle-Width ="80px" UniqueName="Comments">
    <ItemTemplate>
    <%# DataBinder.Eval(Container.DataItem, "Comments")%>
    </ItemTemplate>
    <EditItemTemplate>
    <telerik:RadTextBox Width="255px" ID="rtxtComments" runat="server" TextMode="MultiLine" Resize="Both"  MaxLength="2000">
                </telerik:RadTextBox>
    </EditItemTemplate>

    </telerik:GridTemplateColumn>
     
    </Columns>
    </MasterTableView>
    <ClientSettings>
        <Scrolling AllowScroll="True"></Scrolling>
        <Selecting AllowRowSelect="true"  UseClientSelectColumnOnly="True" ></Selecting>
    </ClientSettings>
    </telerik:RadGrid>
       </div>
            </asp:Panel>
        </telerik:RadPane>
    </telerik:RadSplitter>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">

            function ShowContinue() {
                document.getElementById('<%=  CreateBatchPanel.ClientID %>').style.visibility = "visible";
            }  

            function OnClientRadToolBarClick(sender, eventArgs) {
                var button = eventArgs.get_item();

                if (button.get_text() != "Save All" && button.get_text() != "Cancel All") {

                    if (CheckForRowsInEditMode()) {
                        eventArgs.set_cancel(false);
                    } else {
                        eventArgs.set_cancel(true);
                    }
                }
                else {
                
                if (button.get_text() == "Save All") {
                        eventArgs.set_cancel(!confirm('Are you sure? All editable rows will be saved.'));
                    }
                    if (button.get_text() == "Cancel All") {
                        eventArgs.set_cancel(!confirm('Are you sure? All actions will be cancelled.'));
                    }
                };

            }

            function CheckForRowsInEditMode() {
                var radUPCGrid = $find("<%=RadUPCGrid.ClientID %>");
                if (radUPCGrid) {
                    var MasterTableUPC = radUPCGrid.get_masterTableView();
                    var editItemsUPC = MasterTableUPC.get_editItems();
                    if (editItemsUPC.length > 0) {
                        alert("A row is in Edit mode. Please Save/Cancel changes before performing any action.");
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    return true;
                }
            }

        </script>
    </telerik:RadCodeBlock>
</asp:Content>
