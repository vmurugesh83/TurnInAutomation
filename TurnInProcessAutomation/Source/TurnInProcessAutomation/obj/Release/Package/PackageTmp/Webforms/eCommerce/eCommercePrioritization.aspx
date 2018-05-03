<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ContentPage.Master" ValidateRequest="false"
    CodeBehind="eCommercePrioritization.aspx.vb" Inherits="TurnInProcessAutomation.eCommercePrioritization" %>

<%@ MasterType VirtualPath="~/ContentPage.Master" %>
<%@ Reference Control="~/WebUserControls/eCommerce/eCommercePrioritizationCtrl.ascx" %>
<%@ Register TagPrefix="TurnIn" TagName="Modal" Src="~/WebUserControls/ModalPopupControl.ascx" %>
<%@ Register TagPrefix="TurnIn" TagName="SnapshotModal" Src="~/WebUserControls/ModalSnapshotImage.ascx" %>
<asp:Content ID="eCommercePrioritizationForm" ContentPlaceHolderID="ContentArea"
    runat="Server">
    <telerik:RadAjaxManagerProxy ID="RadAjaxMgrProxyEcommPrior" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rtbeCommercePrioritization">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdeCommercePrioritization" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFlood" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodColorOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFloodClrLvl" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodSizeOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFloodSizLvl" />
                    <telerik:AjaxUpdatedControl ControlID="mpeCommercePrioritization" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdeCommercePrioritization">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdeCommercePrioritization" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFlood" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodColorOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFloodClrLvl" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodSizeOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFloodSizLvl" />
                    <telerik:AjaxUpdatedControl ControlID="mpeCommercePrioritization" />
                    <telerik:AjaxUpdatedControl ControlID="rtbeCommercePrioritization" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnFlood">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdeCommercePrioritization" />
                    <telerik:AjaxUpdatedControl ControlID="rcbFloodSwatchFlg" />
                    <telerik:AjaxUpdatedControl ControlID="rcbFloodColorFlg" />
                    <telerik:AjaxUpdatedControl ControlID="rcbFloodSizeFlg" />
                    <telerik:AjaxUpdatedControl ControlID="rcbFloodLabel" />
                    <telerik:AjaxUpdatedControl ControlID="rcbFloodBrand" />
                    <telerik:AjaxUpdatedControl ControlID="rcbFloodAge" />
                    <telerik:AjaxUpdatedControl ControlID="rcbFloodGender" />
                    <telerik:AjaxUpdatedControl ControlID="mpeCommercePrioritization" />
                    <telerik:AjaxUpdatedControl ControlID="rtbeCommercePrioritization" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFlood" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodColorOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFloodClrLvl" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodSizeOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFloodSizLvl" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnFloodClrLvl">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdeCommercePrioritization" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodFeatureId" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodProductName" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodImageGroup" />
                    <telerik:AjaxUpdatedControl ControlID="hdnUpdateButton" />
                    <telerik:AjaxUpdatedControl ControlID="hdnSpellCheck" />
                    <telerik:AjaxUpdatedControl ControlID="mpeCommercePrioritization" />
                    <telerik:AjaxUpdatedControl ControlID="rtbeCommercePrioritization" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFlood" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodColorOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFloodClrLvl" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodSizeOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFloodSizLvl" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodFindVendorSize" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodVendorWCSize" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodVendorWCSizeFamily" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodFindWebCatSize" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodReplaceWebCatSize" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodFindSizeFam" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodReplaceSizeFam" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnFloodSizLvl">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdeCommercePrioritization" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodFindWebCatSize" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodReplaceWebCatSize" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodFindSizeFam" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodReplaceSizeFam" />
                    <telerik:AjaxUpdatedControl ControlID="hdnUpdateButton" />
                    <telerik:AjaxUpdatedControl ControlID="hdnSpellCheck" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodFindVendorSize" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodVendorWCSize" />
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodVendorWCSizeFamily" />
                    <telerik:AjaxUpdatedControl ControlID="mpeCommercePrioritization" />
                    <telerik:AjaxUpdatedControl ControlID="rtbeCommercePrioritization" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFlood" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodColorOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFloodClrLvl" />
                    <telerik:AjaxUpdatedControl ControlID="tblFloodSizeOptions" />
                    <telerik:AjaxUpdatedControl ControlID="btnFloodSizLvl" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnOpenWebCat">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rtxtFloodWebCategories" />
                    <telerik:AjaxUpdatedControl ControlID="hfFloodWebCatCde" />
                    <telerik:AjaxUpdatedControl ControlID="FloodWindow" />
                    <telerik:AjaxUpdatedControl ControlID="rtbeCommercePrioritization" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    <telerik:RadSplitter ID="rseCommercePrioritization" runat="server" SkinID="pageSplitter">
        <telerik:RadPane ID="rpHeader" runat="server" Height="90" Scrolling="None" Font-Bold="True">
            <div id="pageActionBar">

                <telerik:RadToolBar ID="rtbeCommercePrioritization" runat="server" OnClientButtonClicking="OnClientTabButtonClicking"
                    OnClientLoad="clientLoad" OnClientButtonClicked="setHourglass" CssClass="SeparatedButtons">
                    <Items>
                        <telerik:RadToolBarButton runat="server" CommandName="Back" DisabledImageUrl="~/Images/BackButton.gif"
                            ImageUrl="~/Images/BackButton.gif" Text="Back">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Export" DisabledImageUrl="~/Images/Export_d.gif"
                            ImageUrl="~/Images/Export.gif" Text="Export" CssClass="rightAligned" Enabled="false">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Reset" DisabledImageUrl="~/Images/Reset_d.gif"
                            ImageUrl="~/Images/Reset.gif" Text="Reset" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="LevelDown" DisabledImageUrl="~/Images/Retrieve2_d.gif"
                            ImageUrl="~/Images/Retrieve2.gif" Text="Level Down" CssClass="rightAligned" Enabled="false" Visible="true">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="LevelUp" DisabledImageUrl="~/Images/Retrieve2_d.gif"
                            ImageUrl="~/Images/Retrieve2.gif" Text="Level Up" CssClass="rightAligned" Enabled="false">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Reject" DisabledImageUrl="~/Images/Submit.gif"
                            ImageUrl="~/Images/Submit.gif" Text="Reject" CssClass="rightAligned" Enabled="false" Visible="true">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Submit" DisabledImageUrl="~/Images/Submit.gif"
                            ImageUrl="~/Images/Submit.gif" Text="Submit" CssClass="rightAligned" Enabled="false" Visible="true">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Retrieve" DisabledImageUrl="~/Images/Retrieve1_d.gif"
                            ImageUrl="~/Images/Retrieve1.gif" Text="Retrieve" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>

            </div>
            <div id="pageHeader">
                <asp:Label ID="lblPageHeader" runat="server" Text="E-Comm Prioritization" />
                <bonton:MessagePanel ID="mpeCommercePrioritization" runat="server" />
            </div>
        </telerik:RadPane>
        <telerik:RadPane ID="rpContent" runat="server">
            <asp:Panel ID="pnleCommercePrioritization" runat="server" Visible="true">
                <asp:HiddenField ID="hdnUpdateButton" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnSpellCheck" runat="server"></asp:HiddenField>
                <table id="tblFloodOptions" runat="server" visible="false">
                    <tr>
                        <td>
                            <asp:Button ID="btnFlood" runat="server" Text="Flood (ISN)" Font-Bold="true" Width="90px"
                                Height="25px" OnClientClick="javascript: if(!confirm('Are you sure? This action will automatically save data for every row selected.')) return false;" />
                        </td>
                        <td>
                            <telerik:RadComboBox ID="rcbFloodSwatchFlg" runat="server" Width="92px" Height="60px"
                                MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false"
                                EmptyMessage="Swatch Flag" ToolTip="Swatch Flag">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="" Selected="true"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Y" Value="Y"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="N" Value="N"></telerik:RadComboBoxItem>
                                </Items>
                                <CollapseAnimation Duration="200" Type="OutQuint" />
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="rcbFloodColorFlg" runat="server" Width="85px" Height="60px"
                                MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false"
                                EmptyMessage="Color Flag" ToolTip="Color Flag">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="" Selected="true"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Y" Value="Y"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="N" Value="N"></telerik:RadComboBoxItem>
                                </Items>
                                <CollapseAnimation Duration="200" Type="OutQuint" />
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="rcbFloodSizeFlg" runat="server" Width="80px" Height="60px"
                                MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false"
                                EmptyMessage="Size Flag" ToolTip="Size Flag">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="" Selected="true"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Y" Value="Y"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="N" Value="N"></telerik:RadComboBoxItem>
                                </Items>
                                <CollapseAnimation Duration="200" Type="OutQuint" />
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <asp:HiddenField ID="hfFloodWebCatCde" runat="server" Value='0' />
                            <telerik:RadTextBox ID="rtxtFloodWebCategories" runat="server" EmptyMessageStyle-Font-Italic="true"
                                Width="150px" EmptyMessage="Primary Web Category" ToolTip="Primary Web Category">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadButton ID="btnOpenWebCat" runat="server" Width="18px" Image-ImageUrl="~/Images/Add.gif"
                                Height="18px" OnClientClicking="OpenWebCategoriesForAll" ToolTip="Add Primary Web Category">
                            </telerik:RadButton>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="rcbFloodLabel" AppendDataBoundItems="False" runat="server"
                                Width="60px" Height="140px" DropDownWidth="140px" OnItemsRequested="rcbFloodLabel_ItemsRequested"
                                EmptyMessage="Label" ToolTip="Label" Filter="Contains">
                                <CollapseAnimation Duration="200" Type="OutQuint" />
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="rcbFloodBrand" AppendDataBoundItems="False" runat="server"
                                Width="115px" Height="140px" DropDownWidth="140px" OnItemsRequested="rcbFloodBrand_ItemsRequested"
                                EmptyMessage="Web Cat Brand" ToolTip="Web Cat Brand" Filter="Contains">
                                <CollapseAnimation Duration="200" Type="OutQuint" />
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="rcbFloodAge" AppendDataBoundItems="False" runat="server"
                                Width="45px" Height="100px" DropDownWidth="60" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                AllowCustomText="false" OnItemsRequested="rcbFloodAge_ItemsRequested"
                                EmptyMessage="Age" ToolTip="Age">
                                <CollapseAnimation Duration="200" Type="OutQuint" />
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="rcbFloodGender" AppendDataBoundItems="False" runat="server"
                                Width="70px" Height="80px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                AllowCustomText="false" OnItemsRequested="rcbFloodGender_ItemsRequested"
                                EmptyMessage="Gender" ToolTip="Gender">
                                <CollapseAnimation Duration="200" Type="OutQuint" />
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
                <table id="tblFloodColorOptions" runat="server" visible="false">
                    <tr>
                        <td style="padding-right: 80px"></td>
                        <td>
                            <asp:Button ID="btnFloodClrLvl" runat="server" Text="Flood (Color)" Font-Bold="true" Width="100px"
                                Height="25px" />
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="rtxtFloodFeatureId" runat="server" EmptyMessage="Feature Id"
                                Width="80px" MaxLength="9" MinValue="0" MaxValue="999999999" ToolTip="Feature Id">
                                <NumberFormat GroupSeparator="" DecimalDigits="0" />
                            </telerik:RadNumericTextBox>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="rtxtFloodProductName" runat="server" EmptyMessage="Product Name" EmptyMessageStyle-Font-Italic="true"
                                Width="200px" MaxLength="255" ToolTip="Product Name">
                                <ClientEvents OnBlur="ValidateText" />
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="rtxtFloodImageGroup" runat="server" EmptyMessage="Image Group"
                                Width="100px" MaxLength="9" MinValue="0" MaxValue="999999999" ToolTip="Image Group">
                                <NumberFormat GroupSeparator="" DecimalDigits="0" />
                            </telerik:RadNumericTextBox>
                        </td>
                    </tr>
                </table>
                <table id="tblFloodSizeOptions" runat="server" visible="false">
                    <tr>
                        <td style="padding-right: 160px"></td>
                        <td>
                            <asp:Button ID="btnFloodSizLvl" runat="server" Text="Flood (Size)" Font-Bold="true" Width="100px"
                                Height="25px" OnClientClick="javascript: if(!confirm('Are you sure? This action will automatically save data for every row selected.')) return false;" />
                        </td>
                        <td>
                            <telerik:RadTextBox ID="rtxtFloodFindWebCatSize" runat="server" EmptyMessage="Find: Web Cat Size" EmptyMessageStyle-Font-Italic="true"
                                Width="120px" MaxLength="100" ToolTip="Find: Web Cat Size">
                                <ClientEvents OnBlur="ValidateText" />
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="rtxtFloodReplaceWebCatSize" runat="server" EmptyMessage="Replace: Web Cat Size" EmptyMessageStyle-Font-Italic="true"
                                Width="140px" MaxLength="100" ToolTip="Replace: Web Cat Size">
                                <ClientEvents OnBlur="ValidateText" />
                            </telerik:RadTextBox>
                        </td>
                        <td width="20"></td>
                        <td>
                            <telerik:RadTextBox ID="rtxtFloodFindSizeFam" runat="server" EmptyMessage="Find: Size Family"
                                EmptyMessageStyle-Font-Italic="true" Width="105px" MaxLength="100" ToolTip="Find: Size Family">
                                <ClientEvents OnBlur="ValidateText" />
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="rtxtFloodReplaceSizeFam" runat="server" EmptyMessage="Replace: Size Family"
                                EmptyMessageStyle-Font-Italic="true" Width="125px" MaxLength="100" ToolTip="Replace: Size Family">
                                <ClientEvents OnBlur="ValidateText" />
                            </telerik:RadTextBox>
                        </td>
                        <td width="20"></td>
                        <td>
                            <telerik:RadTextBox ID="rtxtFloodFindVendorSize" runat="server" EmptyMessage="Find: Vendor Size"
                                EmptyMessageStyle-Font-Italic="true" Width="105px" MaxLength="100" ToolTip="Find: Vendor Size">
                                <ClientEvents OnBlur="ValidateText" />
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="rtxtFloodVendorWCSize" runat="server" EmptyMessage="Replace: WC Size"
                                EmptyMessageStyle-Font-Italic="true" Width="130px" MaxLength="100" ToolTip="Replace: WC Size">
                                <ClientEvents OnBlur="ValidateText" />
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="rcbFloodVendorWCSizeFamily" runat="server" EmptyMessage="Replace: Size Family"
                                Width="130px" ToolTip="Replace: Size Family"
                                MarkFirstMatch="true" AllowCustomText="false" CheckBoxes="true" Filter="StartsWith">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
                <div runat="server" id="divFScr" style="width: 100%; height: 100%; overflow: auto;">
                    <telerik:RadGrid ID="grdeCommercePrioritization" runat="server" SkinID="CenteredWithScroll"
                        AllowPaging="True" Width="1745" Height="520" EditMode="InPlace" AllowMultiRowSelection="true"
                        Visible="false">
                        <ClientSettings>
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                            <ClientEvents OnCommand="OnEditClick" />
                            <ClientEvents OnRowSelected="OnRowSelect" />
                            <ClientEvents OnRowDeselected="OnRowDeSelect" />
                        </ClientSettings>
                        <MasterTableView EditMode="InPlace" Name="grdFirstLevel" DataKeyNames="ISN,BrandID,StatusFlg,SwatchFlg,ColorFlg,SizeFlg,IsValidFlg,WebCatgyCde,WebCatgyList,TurnInMerchID,AdNbr,BrandId"
                            ClientDataKeyNames="ISN,BrandID,StatusFlg,SwatchFlg,ColorFlg,SizeFlg,IsValidFlg,TurnInMerchID,AdNbr,ISNBrandID" AllowSorting="true">
                            <EditFormSettings>
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                            </EditFormSettings>
                            <NoRecordsTemplate>
                                no records retrieved
                            </NoRecordsTemplate>
                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                            <Columns>
                                <telerik:GridImageColumn UniqueName="MissingData" ImageUrl="" Visible="true">
                                    <HeaderStyle Width="30px" />
                                    <ItemStyle Width="30px" />
                                </telerik:GridImageColumn>
                                <telerik:GridClientSelectColumn UniqueName="selColumn">
                                    <ItemStyle Width="30px" />
                                    <HeaderStyle Width="30px" />
                                </telerik:GridClientSelectColumn>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Images/Edit1.gif"
                                    CancelImageUrl="~/Images/Cancel1.gif" UpdateImageUrl="~/Images/CheckMark.gif"
                                    HeaderText="" UniqueName="EditColumn">
                                    <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="60" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridBoundColumn DataField="WebCatStatus" HeaderText="Prioritization Status" UniqueName="WebCatStatus"
                                    ReadOnly="true" SortExpression="WebCatStatus">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="WebCatAvailableQty" HeaderText="WebCat Available Qty" UniqueName="WebCatAvailableQty"
                                    ReadOnly="true" SortExpression="WebCatAvailableQty">
                                    <HeaderStyle HorizontalAlign="Center" Width="65" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="OnOrder" HeaderText="OO" UniqueName="OnOrder"
                                    ReadOnly="true" SortExpression="OnOrder">
                                    <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DeliverDate" HeaderText="In Store Date"
                                    UniqueName="DeliverDate" ReadOnly="true" SortExpression="DeliverDate">
                                    <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Swatch Flag"
                                    UniqueName="SwatchFlg" SortExpression="SwatchFlg">
                                    <HeaderStyle HorizontalAlign="Center" Width="45px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("SwatchFlg")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="rcbSwatchFlg" runat="server" Width="45px" Height="40px"
                                            Text='<%# Eval("SwatchFlg")%>' MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                            AllowCustomText="false">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Y" Value="Y"></telerik:RadComboBoxItem>
                                                <telerik:RadComboBoxItem Text="N" Value="N"></telerik:RadComboBoxItem>
                                            </Items>
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Color Flag"
                                    UniqueName="ColorFlg" SortExpression="ColorFlg">
                                    <HeaderStyle HorizontalAlign="Center" Width="40px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("ColorFlg")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="rcbColorFlg" runat="server" Width="40px" Height="40px" Text='<%# Eval("ColorFlg")%>'
                                            MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false" OnClientSelectedIndexChanged="ColorFlagChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Y" Value="Y"></telerik:RadComboBoxItem>
                                                <telerik:RadComboBoxItem Text="N" Value="N"></telerik:RadComboBoxItem>
                                            </Items>
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Size Flag"
                                    UniqueName="SizeFlg" SortExpression="SizeFlg">
                                    <HeaderStyle HorizontalAlign="Center" Width="40px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("SizeFlg")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="rcbSizeFlg" runat="server" Width="40px" Height="40px" Text='<%# Eval("SizeFlg")%>'
                                            MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Y" Value="Y"></telerik:RadComboBoxItem>
                                                <telerik:RadComboBoxItem Text="N" Value="N"></telerik:RadComboBoxItem>
                                            </Items>
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Web Categories"
                                    UniqueName="WebCategories" SortExpression="WebCatgyDesc">
                                    <HeaderStyle HorizontalAlign="Center" Width="160px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltrPrimaryWebCategory"></asp:Literal>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="rtxtWebCategories" runat="server"
                                            Width="150px" MaxLength="255">
                                            <ClientEvents OnFocus="OpenWebCategories" />
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Label"
                                    UniqueName="Label" SortExpression="LabelDesc">
                                    <HeaderStyle HorizontalAlign="Center" Width="95px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("LabelDesc")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfLabel" runat="server" Value='<%# Eval("LabelID") %>' />
                                        <telerik:RadComboBox ID="rcbLabel" runat="server" Width="97px" Height="140px" DropDownWidth="140px"
                                            MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false" Filter="contains">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Web Cat Brand"
                                    UniqueName="WebCatBrand" SortExpression="BrandDesc">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("BrandDesc")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfWebCatBrand" runat="server" Value='<%# Eval("BrandDesc") %>' />
                                        <telerik:RadComboBox ID="rcbWebCatBrand" runat="server" Width="102px" Height="140px" DropDownWidth="140px"
                                            MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false" Filter="contains">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="ProductName" HeaderText="Product Name"
                                    UniqueName="ProductName" ReadOnly="true" SortExpression="ProductName">
                                    <HeaderStyle HorizontalAlign="Center" Width="120px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorStyleNumber" HeaderText="Vendor Style"
                                    UniqueName="VendorStyleNumber" ReadOnly="true" SortExpression="VendorStyleNumber">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DropShipFlg" HeaderText="Drop Ship Flag" Visible="false"
                                    UniqueName="DropShipFlg" ReadOnly="true" SortExpression="DropShipFlg">
                                    <HeaderStyle HorizontalAlign="Center" Width="40" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Drop Ship ID" Visible="false"
                                    UniqueName="DropShipID" SortExpression="DropShipDesc">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("DropShipDesc")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfDropShipID" runat="server" Value='<%# Eval("DropShipID") %>' />
                                        <telerik:RadComboBox ID="rcbDropShipID" runat="server" Width="102px" Height="100px" DropDownWidth="150px"
                                            MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false" Filter="contains">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Internal Return Instructions" Visible="false"
                                    UniqueName="IntReturnInstrct" SortExpression="IntReturnInstrct">
                                    <HeaderStyle HorizontalAlign="Center" Width="120px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("IntReturnInstrct")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfIntReturnInstrct" runat="server" Value='<%# Eval("IntRetInsCde") %>' />
                                        <telerik:RadComboBox ID="rcbIntReturnInstrct" runat="server" Width="122px" Height="100px"
                                            MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="External Return Instructions" Visible="false"
                                    UniqueName="ExtReturnInstrct" SortExpression="ExtReturnInstrct">
                                    <HeaderStyle HorizontalAlign="Center" Width="120px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("ExtReturnInstrct")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfExtReturnInstrct" runat="server" Value='<%# Eval("ExtRetInsCde") %>' />
                                        <telerik:RadComboBox ID="rcbExtReturnInstrct" runat="server" Width="122px" Height="100px"
                                            DropDownWidth="150px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                            AllowCustomText="false">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="PrimaryThumbnailURL" HeaderText="Snapshot Image">
                                    <HeaderStyle HorizontalAlign="Center" Width="108" />
                                    <ItemStyle HorizontalAlign="Center" Width="108" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibThumbnail" runat="server" ImageUrl='<%# Eval("PrimaryThumbnailURL")%>' AlternateText="No Images Available"
                                            Width="100px" Height="80px" ImageAlign="Middle" OnClientClick='<%# "return ShowSnapshotImage(&apos;" + Eval("PrimaryActualURL") + "&apos;);"%>' ToolTip='Snapshot Image' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Age" UniqueName="Age"
                                    SortExpression="AgeDesc">
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("AgeDesc")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfAge" runat="server" Value='<%# Eval("AgeCde") %>' />
                                        <telerik:RadComboBox ID="rcbAge" runat="server" Width="62px" Height="100px" MarkFirstMatch="true"
                                            OnClientBlur="OnClientBlurHandler" AllowCustomText="false">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Gender" UniqueName="Gender"
                                    SortExpression="GenderDesc">
                                    <HeaderStyle HorizontalAlign="Center" Width="70px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("GenderDesc")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfGender" runat="server" Value='<%# Eval("GenderCde") %>' />
                                        <telerik:RadComboBox ID="rcbGender" runat="server" Width="72px" Height="80px" MarkFirstMatch="true"
                                            OnClientBlur="OnClientBlurHandler" AllowCustomText="false">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="LastModifiedDate" HeaderText="Last Modified" UniqueName="LastModifiedDate"
                                    ReadOnly="true" SortExpression="LastModifiedDate">
                                    <HeaderStyle HorizontalAlign="Center" Width="75" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AdNbr" HeaderText="AdNbr" UniqueName="AdNbr"
                                    ReadOnly="true" SortExpression="AdNbr">
                                    <HeaderStyle HorizontalAlign="Center" Width="75" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                            </Columns>
                            <DetailTables>
                                <telerik:GridTableView Name="grdSecondLevel" AutoGenerateColumns="false" SkinID="CenteredWithScroll"
                                    HorizontalAlign="Left" ShowFooter="false" AllowSorting="False" EditMode="InPlace" CssClass="detailTableLevel1"
                                    Height="10" Width="650" DataKeyNames="ISN,TurnInMerchID,IsValidFlg,StatusFlg,ImageID,FRS" ClientDataKeyNames="ISN,TurnInMerchID,IsValidFlg,StatusFlg,ImageID">
                                    <NoRecordsTemplate>Color level detail does not exist. There could have been items which are now killed in Admin which are not included in this grid.</NoRecordsTemplate>
                                    <Columns>
                                        <telerik:GridImageColumn UniqueName="MissingData" ImageUrl="" Visible="true">
                                            <HeaderStyle Width="30px" />
                                            <ItemStyle Width="30px" />
                                        </telerik:GridImageColumn>
                                        <telerik:GridClientSelectColumn UniqueName="selColumn">
                                            <ItemStyle Width="30px" />
                                            <HeaderStyle Width="30px" />
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Images/Edit1.gif"
                                            CancelImageUrl="~/Images/Cancel1.gif" UpdateImageUrl="~/Images/CheckMark.gif"
                                            HeaderText="" UniqueName="EditColumn">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                                            <ItemStyle HorizontalAlign="Center" Width="60" />
                                        </telerik:GridEditCommandColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                            UniqueName="DeleteColumn" ImageUrl="~/Images/Delete.gif">
                                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                                        </telerik:GridButtonColumn>
                                        <telerik:GridBoundColumn DataField="ImageSuffix" HeaderText="Killed Item in Admin?"
                                            UniqueName="ImageSuffix" ReadOnly="true">
                                            <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn HeaderText="Feature Id" UniqueName="FeatureId">
                                            <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <%# Eval("FeatureId")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadNumericTextBox ID="rtxtFeatureId" runat="server" Value='<%# CStr(Eval("FeatureId"))%>'
                                                    Width="80px" MaxLength="9" MinValue="0" MaxValue="999999999">
                                                    <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Image Id" UniqueName="ImageId">
                                            <HeaderStyle HorizontalAlign="Center" Width="70px" Font-Bold="True" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <%# Eval("ImageID")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadNumericTextBox ID="rtxtImageId" runat="server" Value='<%# CStr(Eval("ImageID"))%>'
                                                    Width="80px" MaxLength="9" MinValue="0" MaxValue="999999999">
                                                    <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <%--                                    <telerik:GridBoundColumn DataField="ImageID" HeaderText="Image Id"
                                        UniqueName="ImageId" ReadOnly="true">
                                        <HeaderStyle HorizontalAlign="Center" Width="70" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>--%>
                                        <telerik:GridTemplateColumn HeaderText="Product Name" UniqueName="ProductName">
                                            <HeaderStyle HorizontalAlign="Center" Width="200px" Font-Bold="True" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <%# Eval("ProductName")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadTextBox ID="rtxtProductName" runat="server" Text='<%# Eval("ProductName")%>'
                                                    Width="200px" MaxLength="255">
                                                    <ClientEvents OnBlur="ValidateText" />
                                                </telerik:RadTextBox>
                                                <bonton:ToolTipValidator ID="valProductName" runat="server" ControlToEvaluate="rtxtProductName"
                                                    ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                                    ValidationGroup="UpdateClrLvl" />
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Friendly Color" UniqueName="FriendlyColor">
                                            <HeaderStyle HorizontalAlign="Center" Width="100px" Font-Bold="True" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <%# Eval("FriendlyColor") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadTextBox ID="rtxtFriendlyColor" runat="server" Text='<%# Eval("FriendlyColor") %>'
                                                    Width="50px" MaxLength="255">
                                                    <ClientEvents OnBlur="ValidateText" />
                                                </telerik:RadTextBox>
                                                <bonton:ToolTipValidator ID="valFriendlyColor" runat="server" ControlToEvaluate="rtxtFriendlyColor"
                                                    ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                                    ValidationGroup="UpdateColorLvl" />
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Non Swatch Color" UniqueName="NonSwatchColor">
                                            <HeaderStyle HorizontalAlign="Center" Width="100px" Font-Bold="True" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <%# Eval("NonSwatchClrDesc")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:HiddenField ID="hfNonSwatchColor" runat="server" Value='<%# Eval("NonSwatchClrCde") %>' />
                                                <telerik:RadComboBox ID="rcbNonSwatchColor" runat="server" Width="100px" Height="100px" DropDownWidth="150px"
                                                    MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false" Filter="StartsWith">
                                                    <CollapseAnimation Duration="200" Type="OutQuint" />
                                                </telerik:RadComboBox>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Color Family" UniqueName="ColorFamily">
                                            <HeaderStyle HorizontalAlign="Center" Width="90px" Font-Bold="True" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblColorFamily" ToolTip='<%# Eval("ColorFamily")%>'
                                                    Text='<%# Eval("ColorFamily").ToString.Split(","c)(0)%>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:HiddenField ID="hfColorFamily" runat="server" Value='<%# Eval("ColorFamily") %>' />
                                                <telerik:RadComboBox ID="rcbColorFamily" CheckBoxes="true" runat="server" Width="90px"
                                                    DropDownWidth="120px" Filter="StartsWith">
                                                    <CollapseAnimation Duration="200" Type="OutQuint" />
                                                </telerik:RadComboBox>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Image Group" UniqueName="ImageGroup">
                                            <HeaderStyle HorizontalAlign="Center" Width="90px" Font-Bold="True" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblImageGroup" ToolTip='<%# Eval("ImageGroup")%>'
                                                    Text='<%# Eval("ImageGroup")%>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadTextBox ID="rtxtImageGroup" runat="server" Text='<%# Eval("ImageGroup")%>'
                                                    Width="110px" MaxLength="255">
                                                    <ClientEvents OnBlur="ValidateText" />
                                                </telerik:RadTextBox>
                                                <bonton:ToolTipValidator ID="valImageGroup" runat="server" ControlToEvaluate="rtxtImageGroup"
                                                    ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                                    ValidationGroup="UpdateClrLvl" />
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Feature/ Render/ Swatch" UniqueName="FRS">
                                            <HeaderStyle HorizontalAlign="Center" Width="70px" Font-Bold="True" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <%# Eval("FRS")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:HiddenField ID="hfFRS" runat="server" Value='<%# Eval("FRS") %>' />
                                                <telerik:RadComboBox ID="rcbFRS" runat="server" Width="70px" Height="100px" DropDownWidth="100px"
                                                    MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false">
                                                    <CollapseAnimation Duration="200" Type="OutQuint" />
                                                </telerik:RadComboBox>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="ImageNotes" HeaderText="Image Notes"
                                            UniqueName="ImageNotes">
                                            <HeaderStyle HorizontalAlign="Center" Width="120" Font-Bold="True" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <%# Eval("ImageNotes")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadTextBox ID="rtxtImageNotes" runat="server" Text='<%# Eval("ImageNotes") %>'
                                                    Width="120px" MaxLength="35" TextMode="MultiLine">
                                                </telerik:RadTextBox>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="EMM Notes" UniqueName="EMMNotes">
                                            <HeaderStyle HorizontalAlign="Center" Width="150" Font-Bold="True" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <%# Eval("EMMNotes")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadTextBox ID="rtxtEMMNotes" runat="server" Text='<%# Eval("EMMNotes") %>'
                                                    Width="120px" MaxLength="255" TextMode="MultiLine">
                                                </telerik:RadTextBox>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <DetailTables>
                                        <telerik:GridTableView Name="grdThirdLevel" AutoGenerateColumns="false" HorizontalAlign="Left"
                                            ShowFooter="false" AllowSorting="False" EditMode="InPlace" Width="450" DataKeyNames="ISN,UPC,IsValidFlg,StatusFlg"
                                            ClientDataKeyNames="ISN,UPC,IsValidFlg,StatusFlg" CssClass="detailTableLevel2">
                                            <NoRecordsTemplate>
                                                Size Level detail does not exist.
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridImageColumn UniqueName="MissingData" ImageUrl="" Visible="true">
                                                    <HeaderStyle Width="30px" />
                                                    <ItemStyle Width="30px" />
                                                </telerik:GridImageColumn>
                                                <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Images/Edit1.gif"
                                                    CancelImageUrl="~/Images/Cancel1.gif" UpdateImageUrl="~/Images/CheckMark.gif"
                                                    HeaderText="" UniqueName="EditColumn">
                                                    <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                </telerik:GridEditCommandColumn>
                                                <telerik:GridBoundColumn DataFormatString="&nbsp;{0}" DataField="UPC" HeaderText="UPC" UniqueName="UPC" ReadOnly="true">
                                                    <HeaderStyle HorizontalAlign="Center" Width="100px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="VendorSize" HeaderText="Vendor Size" UniqueName="VendorSize" ReadOnly="true">
                                                    <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Webcat Size" UniqueName="WebcatSize">
                                                    <HeaderStyle HorizontalAlign="Center" Width="100px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <%# Eval("WebCatSizeDesc")%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:HiddenField ID="hfWebCatSize" runat="server" Value='<%# Eval("WebCatSizeDesc") %>' />
                                                        <asp:HiddenField ID="hfWebCatSizeID" runat="server" Value='<%# Eval("WebCatSizeID") %>' />
                                                        <telerik:RadComboBox ID="rcbWebCatSize" runat="server" Width="102px" DropDownWidth="120px"
                                                            MarkFirstMatch="true" AllowCustomText="false"
                                                            ShowMoreResultsBox="true" EnableVirtualScrolling="true" EnableLoadOnDemand="true" ItemsPerRequest="10" OnItemsRequested="rcbWebCatSize_ItemsRequested" Filter="StartsWith">
                                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                                        </telerik:RadComboBox>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Size Family" UniqueName="SizeFamily">
                                                    <HeaderStyle HorizontalAlign="Center" Width="300px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblColorFamily" ToolTip='<%# Eval("SizeFamily")%>'
                                                            Text='<%# Eval("SizeFamily")%>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:HiddenField ID="hfSizeFamily" runat="server" Value='<%# Eval("SizeFamily") %>' />
                                                        <telerik:RadComboBox ID="rcbSizeFamily" runat="server" Width="102px" DropDownWidth="120px"
                                                            MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false" CheckBoxes="true" Filter="StartsWith">
                                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                                        </telerik:RadComboBox>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </telerik:GridTableView>
                                    </DetailTables>
                                </telerik:GridTableView>
                            </DetailTables>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                <!-- Grid used only for reporting. It is not displayed on the screen -->
                <telerik:RadWindowManager ID="RadWinMgrPrior" runat="server" Skin="Vista">
                    <Windows>
                        <telerik:RadWindow Skin="Vista" ID="UserDialog" runat="server" Title="Web Categories"
                            Height="400px" Width="1200px" Left="150px" Modal="true"
                            VisibleTitlebar="true" OnClientClose="RefreshGrid" AutoSize="false" />
                        <telerik:RadWindow Skin="Vista" ID="FloodWindow" runat="server" Title="Primary Web Categories"
                            Height="400px" Width="1200px" Left="150px" Modal="true"
                            VisibleTitlebar="true" OnClientClose="AddWebCategory" AutoSize="false" />
                    </Windows>
                </telerik:RadWindowManager>
            </asp:Panel>
            <TurnIn:Modal ID="tuModalExport" runat="server" />
            <TurnIn:SnapshotModal ID="tuSnapshotImage" runat="server" />
        </telerik:RadPane>
    </telerik:RadSplitter>
    <telerik:RadCodeBlock ID="RadCodeBlockEcommPrior" runat="server">
        <style type="text/css">
            body {
                cursor: default !important;
            }
        </style>
        <script type="text/javascript">
            function OnClientTabButtonClicking(sender, args) {
                var button = args.get_item();

                if (button.get_text() == "Export") {
                    var radGrid = $find("<%=grdeCommercePrioritization.ClientID %>");
                    if (radGrid) {
                        var editItems = radGrid.get_editItems();

                        if (editItems.length > 0) {
                            alert("A row is in Edit mode. Please Save/Cancel changes before performing any action.");
                            args.set_cancel(true);
                            return false;
                        }
                    }

                    ShowModalMessage();
                    args.set_cancel(true);
                }

                if (button.get_text() == "Submit") {
                    var radGrid = $find("<%=grdeCommercePrioritization.ClientID %>");
                    if (radGrid) {
                        var MasterTable = radGrid.get_masterTableView();
                        var editItems = radGrid.get_editItems();
                        var selectedRows = MasterTable.get_selectedItems();

                        if (selectedRows.length > 0) {
                            if (!confirm('Are you sure you want to submit? Rows can not be edited once they have been submitted.')) {
                                args.set_cancel(true);
                                return false;
                            }

                            if (editItems.length > 0) {
                                alert("A row is in Edit mode. Please Save/Cancel changes before performing any action.");
                                args.set_cancel(true);
                            }
                        } else {
                            alert("Please select at least one ISN level record for Submission.");
                            args.set_cancel(true);
                        }
                    }
                }

                if (button.get_text() == "Back" || button.get_text() == "Reset" || button.get_text() == "Retrieve" || button.get_text() == "Level Up" || button.get_text() == "Level Down") {
                    var radGrid = $find("<%=grdeCommercePrioritization.ClientID %>");
                    if (radGrid) {
                        var editItems = radGrid.get_editItems();

                        if (editItems.length > 0) {
                            alert("A row is in Edit mode. Please Save/Cancel changes before performing any action.");
                            args.set_cancel(true);
                        }
                    }
                }
            }

            function ValidateText(sender, args) {
                var str = sender.get_value();
                var isValid = true;

                for (var i = 0; i < str.length; i++) {
                    asciiNum = str.charCodeAt(i);
                    if ((asciiNum == 10) || (asciiNum == 13) || (asciiNum > 31 && asciiNum < 94) || (asciiNum > 94 && asciiNum < 127)) {
                    }
                    else {
                        isValid = false;
                    }
                    // quotes not allowed.
                    if (asciiNum == 39) {
                        isValid = false;
                    }
                }


                if (!isValid) {
                    alert("Invalid Character found!");
                    sender.focus();
                }
                sender.set_value(str.replace(/([^a-zA-Z\d\s:]*[^\s:\-])([^\s:\-]*)/g, function ($0, $1, $2) { return $1.toUpperCase() + $2.toLowerCase(); }));
                return isValid;
            }

            function OnEditClick(sender, eventArgs) {
                var command = eventArgs.get_commandName();

                if (command == 'Edit' || command == 'Delete') {
                    var radGrid = $find("<%=grdeCommercePrioritization.ClientID %>");
                    if (radGrid) {
                        var editItems = radGrid.get_editItems();

                        if (editItems.length > 0) {
                            alert("A row is already in Edit mode. Please Save/Cancel changes before performing any action.");
                            eventArgs.set_cancel(true);
                        }
                    }
                }

                if (command == 'Delete') {
                    eventArgs.set_cancel(!confirm('Delete/Activate this record?'));
                }

                if (command == 'Update') {
                    if (eventArgs.get_tableView().get_name() == "grdSecondLevel") {
                        var radGrid = $find("<%=grdeCommercePrioritization.ClientID %>");
                        if (radGrid) {
                            var editItems = radGrid.get_editItems();
                            var combo;
                            var val;
                            var txtFeatureID;
                            var txtImageID;
                            var featureID;
                            var imageID;
                            var bMessage = false;
                            for (var i = 0; i < editItems.length; i++) {
                                txtFeatureID = $telerik.findControl(editItems[i].get_element(), "rtxtFeatureId");
                                txtImageID = $telerik.findControl(editItems[i].get_element(), "rtxtImageId");
                                featureID = txtFeatureID.get_value();
                                imageID = txtImageID.get_value();
                                combo = $telerik.findControl(editItems[i].get_element(), "rcbFRS");
                                val = combo.get_selectedItem().get_value();
                                if ((val == 'SWTCH' || val == 'REND' || val == 'SWTBOX') && featureID == imageID) {
                                    bMessage = true;
                                }
                            }
                            if (bMessage) {
                                alert("Feature ID must be different than Swatch ID.");
                                eventArgs.set_cancel(true);
                            }
                        }
                    }
                }
            }

            function OnRowSelect(sender, args) {
                if (args.get_tableView().get_name() == "grdSecondLevel") {
                    var parRow = args.get_tableView().get_parentRow();
                    args.get_tableView().get_parentView().selectItem(parRow);
                }
            }

            function OnRowDeSelect(sender, args) {
                if (args.get_tableView().get_name() == "grdFirstLevel") {
                    var dataItemFirstLvl = args.get_gridDataItem();
                    if (dataItemFirstLvl.get_nestedViews().length > 0) {
                        var nestedViewFirstLvl = dataItemFirstLvl.get_nestedViews()[0];
                        nestedViewFirstLvl.clearSelectedItems();
                    }
                }
            }

            function OpenWebCategoriesForAll(sender, eventArgs) {
                var oWnd = radopen("WebCategories.aspx?ID=0", "FloodWindow"); //open in radwindow
                eventArgs.set_cancel(true);
            }

            function OpenWebCategories(sender, eventArgs) {
                var grid = $find("<%=grdeCommercePrioritization.ClientID%>"); // get grid
                var MasterTable = grid.get_masterTableView(); // get mastertableview
                var gridDataItem = MasterTable.get_editItems()[0]; // get edititem
                var key = gridDataItem.getDataKeyValue("ISN"); // get key value for selected edititem
                window.focus(); // move the focus away otherwise the window opens again once the webcategories are changed.
                window.radopen("WebCategories.aspx?ID=" + key, "UserDialog"); //open in radwindow
            }

            function RefreshGrid() {
                var masterTable = $find("<%=grdeCommercePrioritization.clientId%>").get_masterTableView();
                masterTable.rebind();
            }

            function AddWebCategory(oWnd, args) {
                var arg = args.get_argument(); //get the transferred arguments
                var txtBoxWebCatDesc = $find("<%= rtxtFloodWebCategories.ClientID %>");
                if (arg) {
                    if (oWnd.get_name() == "FloodWindow") {
                        document.getElementById('<%= hfFloodWebCatCde.ClientID %>').value = arg.WebCatCde;
                        txtBoxWebCatDesc.set_value(arg.WebCatDesc);
                    }
                }
            }

            function ColorFlagChanged(sender, eventArgs) {
                var item = eventArgs.get_item();
                var grid = $find("<%=grdeCommercePrioritization.ClientID%>"); // get grid
                var MasterTable = grid.get_masterTableView(); // get mastertableview
                var gridDataItem = MasterTable.get_editItems()[0]; // get edititem
                var comboSwatchFlg = gridDataItem.findControl("rcbSwatchFlg");

                if (item.get_text() == "Y") {
                    comboSwatchFlg.enable();
                } else {
                    comboSwatchFlg.set_text("N");
                    comboSwatchFlg.disable();
                }
            }

            var s_gList_Id = '<%= Me.grdeCommercePrioritization.ClientID%>';
            var s_divFScr_Id = '<%= Me.divFScr.ClientID%>';

            //$(document).ready(function () {
            //    ResizeWinDelay(0);
            //    var prm = Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            //});

            //window.onresize = function () {
            //    ResizeWinDelay(200);
            //}
            // Page Request Manager
            function EndRequestHandler(s, e) {
                ResizeWinDelay(0);
            }

            // Window Resizing 
            var tOutResize;
            function ResizeWinDelay(timeOut, repaint) {
                if (tOutResize != null)
                    window.clearTimeout(tOutResize);
                tOutResize = setTimeout(function () { ResizeWin(repaint); tOutResize = null; }, timeOut)
            }
            function ResizeWin(repaint) {
                resize(document.getElementById(s_divFScr_Id));

                var grid = $find(s_gList_Id);
                if (grid)
                    (grid).repaint();
            }
            // End Window //
            function resize(element) {

                if (element != null) {
                    var height = document.body.clientHeight - 1;
                    var width = document.body.clientWidth - 1;

                    var parent = element;
                    while (parent.tagName != 'FORM') {
                        height -= parseInt(parent.offsetTop, 10);
                        width -= parseInt(parent.offsetLeft, 10);

                        parent = parent.parentNode;
                    }

                    element.style.height = '600px'; //height + 'px';
                    element.style.width = width + 'px';
                }
            }
            function ShowSnapshotImage(imageURL) {
                if (imageURL.trim() == "") {
                    return false;
                }

                ShowSnapshotModalMessage(imageURL);
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
