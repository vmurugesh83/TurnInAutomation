<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ContentPage.Master"
    CodeBehind="PrintPreTurnInSetUpCreate.aspx.vb" Inherits="TurnInProcessAutomation.PrintPreTurnInSetUpCreate" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/ContentPage.Master" %>
<%@ Reference Control="~/WebUserControls/Print/PrintPreTurnInCtrl.ascx" %>
<asp:Content ID="PrintPreTurnInSetUpCreateForm" ContentPlaceHolderID="ContentArea"
    runat="Server">
    <telerik:RadSplitter ID="rsPrintPreTurnInSetUpCreate" runat="server" SkinID="pageSplitter">
        <telerik:RadPane ID="rpHeader" runat="server" SkinID="pageHeaderAreaPane">
            <div id="pageActionBar">
                <telerik:RadToolBar ID="rtbPrintPreTurnInSetUpCreate" runat="server" OnClientButtonClicking="OnClientButtonClickingSave"
                    OnClientLoad="clientLoad" CssClass="SeparatedButtons">
                    <Items>
                        <telerik:RadToolBarButton runat="server" CommandName="Back" DisabledImageUrl="~/Images/BackButton.gif"
                            ImageUrl="~/Images/BackButton.gif" Text="Back" CausesValidation="false" TabIndex="16">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Reset" DisabledImageUrl="~/Images/Reset_d.gif"
                            ImageUrl="~/Images/Reset.gif" Text="Reset" CssClass="rightAligned" CausesValidation="false"
                            TabIndex="15">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Save" DisabledImageUrl="~/Images/Save_d.gif"
                            ImageUrl="~/Images/Save.gif" Text="Save" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Retrieve" DisabledImageUrl="~/Images/Retrieve1_d.gif"
                            ImageUrl="~/Images/Retrieve1.gif" Text="Retrieve" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="NextItem" DisabledImageUrl="~/Images/PagingNext.gif"
                            ImageUrl="~/Images/PagingNext.gif" Text="Next Item" CssClass="rightAligned" Height="26">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Previou Item" DisabledImageUrl="~/Images/PagingPrev.gif"
                            ImageUrl="~/Images/PagingPrev.gif" Text="Previous Item" CssClass="rightAligned"
                            Height="26">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Print Labels" DisabledImageUrl="~/Images/Print_d.gif"
                            ImageUrl="~/Images/Print.gif" Text="Print Labels" CssClass="rightAligned" Height="26">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Print Report" DisabledImageUrl="~/Images/Print_d.gif"
                            ImageUrl="~/Images/Print.gif" Text="Print Report" CssClass="rightAligned" Height="26">
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
            </div>
            <asp:Panel ID="pnlTabStrip" runat="server" CssClass="pageTabStrip" Style="margin: 0;">
                <telerik:RadTabStrip ID="rtsPrintPreTurnInSetUpCreate" runat="server" MultiPageID="rmpPrintPreTurnInSetUpCreate"
                    SelectedIndex="1">
                    <Tabs>
                        <telerik:RadTab runat="server" Text="Ad Level" PageViewID="pvAdLevel" Font-Bold="True"
                            Selected="True" />
                        <telerik:RadTab runat="server" Text="Result List" PageViewID="pvResultList" Font-Bold="True" />
                        <telerik:RadTab runat="server" Text="ISN Level" PageViewID="pvISNLevel" Font-Bold="True" />
                        <telerik:RadTab runat="server" Text="Color/Size Level" PageViewID="pvColorSizeLevel"
                            Font-Bold="True" />
                    </Tabs>
                </telerik:RadTabStrip>
            </asp:Panel>
            <div id="pageHeader">
                <asp:Label ID="lblPageHeader" runat="server" Text="Print TurnIn Setup Create" />
                <br />
            </div>
            <bonton:MessagePanel ID="mpPrintPreTurnInSetUpCreate" runat="server" />
        </telerik:RadPane>
        <telerik:RadPane ID="rpContent" runat="server" Width="100%">
            <telerik:RadMultiPage ID="rmpPrintPreTurnInSetUpCreate" SelectedIndex="0" runat="server"
                Height="100%">
                <telerik:RadPageView ID="pvAdLevel" runat="server" Height="100%">
                    <asp:Panel ID="pnlAdLevel" runat="server" Visible="true">
                        <br />
                        <table align="left" width="700" cellpadding="10">
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblAdNoLabel" runat="server" class="label" Text="Ad #:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblAdNoText" runat="server" Text="1281" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblAdNoDescLabel" runat="server" class="label" Text="Desc:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblAdNoDescText" runat="server" Text="2_5 Fine Jewelry Mailer" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblRunStartLabel" runat="server" class="label" Text="Run Start:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblRunStartText" runat="server" Text="02/05/13" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblPageNoLabel" runat="server" class="label" Text="Page #:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblPageNoText" runat="server" Text="1" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblPageNoDescLabel" runat="server" class="label" Text="Desc:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblPageNoDescText" runat="server" Text="Page Description" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblRunEndLabel" runat="server" class="label" Text="Run End:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblRunEndText" runat="server" Text="02/14/13" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblPrintType" runat="server" class="label" Text="Print Type:" />
                                </td>
                                <td nowrap="nowrap" colspan="7">
                                    <telerik:RadComboBox ID="cmbPrintType" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="120" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="PPM" Selected="True" />
                                            <telerik:RadComboBoxItem runat="server" Text="PPM1" />
                                            <telerik:RadComboBoxItem runat="server" Text="PPM2" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </telerik:RadPageView>
                <telerik:RadPageView ID="pvResultList" runat="server" Height="100%">
                    <asp:Panel ID="pnlResultList" runat="server" Visible="true">
                        <br />
                        <table class="labels-vertical" style="margin: 20px;" width="700">
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblAdNoWorkListLabel" runat="server" class="label" Text="Ad #:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblAdNoWorkListText" runat="server" Text="31248" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblAdNoDescWorkListLabel" runat="server" class="label" Text="Desc:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblAdNoDescWorkListText" runat="server" Text="10_10 Anniversary Sale Mailer" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblRunStartWorkListLabel" runat="server" class="label" Text="Run Start:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblRunStartWorkListText" runat="server" Text="10/10/12" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblPageNoWorkListLabel" runat="server" class="label" Text="Page #:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblPageNoWorkListText" runat="server" Text="1" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblPageNoDescWorkListLabel" runat="server" class="label" Text="Page Desc:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblPageNoDescWorkListText" runat="server" Text="This is Page Description" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblRunEndWorkListLabel" runat="server" class="label" Text="Run End:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblRunEndWorkListText" runat="server" Text="10/18/12" />
                                </td>
                            </tr>
                        </table>
                        <table align="center">
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="grdResultList" runat="server" SkinID="CenteredWithScroll" AllowPaging="True"
                                        Height="400" Width="1200" ShowFooter="False">
                                        <MasterTableView>
                                            <EditFormSettings>
                                                <EditColumn UniqueName="EditCommandColumn1">
                                                </EditColumn>
                                            </EditFormSettings>
                                            <NoRecordsTemplate>
                                                no records retrieved</NoRecordsTemplate>
                                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="" UniqueName="Select">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkAll" runat="server" AllowMultiRowSelection="true" AutoPostBack="true"
                                                            Checked="false" OnCheckedChanged="ToggleSelectDeselectAll" ToolTip="Select/Deselect All" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectRow" runat="server" AutoPostBack="true" Checked="false"
                                                            OnCheckedChanged="ToggleSelectedRow" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="20px" />
                                                    <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="ACode" HeaderText="A-CD 1" UniqueName="ACode">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Vendor" HeaderText="Vendor" UniqueName="Vendor">
                                                    <HeaderStyle HorizontalAlign="Center" Width="150" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ISN" HeaderText="ISN" UniqueName="ISN">
                                                    <HeaderStyle HorizontalAlign="Center" Width="100" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ISNLongDesc" HeaderText="ISN Long Desc" UniqueName="ISNLongDesc">
                                                    <HeaderStyle HorizontalAlign="Center" Width="200" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="VendorStyle" HeaderText="Vendor Style" UniqueName="VendorStyle">
                                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="TurnedInPrint" HeaderText="Turned-In (Print)"
                                                    UniqueName="TurnedInPrint">
                                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="TurnedIneComm" HeaderText="Turned-In (eComm)"
                                                    UniqueName="TurnedIneComm">
                                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="OO" HeaderText="OO" UniqueName="OO">
                                                    <HeaderStyle HorizontalAlign="Center" Width="80" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="OH" HeaderText="OH" UniqueName="OH">
                                                    <HeaderStyle HorizontalAlign="Center" Width="50" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                            <DetailTables>
                                                <telerik:GridTableView Name="grdSecondLevel" AutoGenerateColumns="false" HorizontalAlign="Left"
                                                    ShowFooter="false" AllowSorting="False" Width="400">
                                                    <NoRecordsTemplate>
                                                        Detail does not exist.
                                                    </NoRecordsTemplate>
                                                    <Columns>
                                                        <telerik:GridTemplateColumn HeaderText="" UniqueName="Select">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkAll" runat="server" AllowMultiRowSelection="true" AutoPostBack="true"
                                                                    Checked="false" OnCheckedChanged="ToggleSelectDeselectAll" ToolTip="Select/Deselect All" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkBatchesToApproval" runat="server" AutoPostBack="true" Checked="false"
                                                                    OnCheckedChanged="ToggleSelectedRow" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="20px" />
                                                            <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="VendorColor" HeaderText="Vendor Color" UniqueName="VendorColor">
                                                            <HeaderStyle Width="100" HorizontalAlign="Center" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="TurnedInPrint" HeaderText="Turned-In (Print)"
                                                            UniqueName="TurnedInPrint">
                                                            <HeaderStyle Width="100" HorizontalAlign="Center" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="TurnedIneComm" HeaderText="Turned-In (eComm)"
                                                            UniqueName="TurnedIneComm">
                                                            <HeaderStyle Width="100" HorizontalAlign="Center" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="OO" HeaderText="OO" UniqueName="OO">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="OH" HeaderText="OH" UniqueName="OH">
                                                            <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </telerik:GridTableView>
                                            </DetailTables>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </telerik:RadPageView>
                <telerik:RadPageView ID="pvISNLevel" runat="server" Height="100%">
                    <asp:Panel ID="pnlISNLevel" runat="server" Visible="true">
                        <table align="center">
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblUserCreateDateLabel" runat="server" class="label" Text="User/Create Date:" />
                                </td>
                                <td>
                                    <asp:Label ID="lblUserCreateDateText" runat="server" Text="Stoll, S 10/31/12" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblUserModifyDateLabel" runat="server" class="label" Text="User/Modify Date:" />
                                </td>
                                <td>
                                    <asp:Label ID="lblUserModifyDateText" runat="server" Text="Stoll, S 10/31/12" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblISNLabel" runat="server" class="label" Text="ISN:" />
                                </td>
                                <td nowrap="nowrap">
                                    <telerik:RadComboBox ID="cmbISNText" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="120" TabIndex="2">
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblVendorStyleLabel" runat="server" class="label" Text="Vendor Style:" />
                                </td>
                                <td nowrap="nowrap">
                                    <telerik:RadComboBox ID="cmbVendorStyle" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="120" TabIndex="2" Filter="Contains">
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblDeptLabel" runat="server" class="label" Text="Dept:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblDeptText" runat="server" Text="375 YC KNITS/WOVENS" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblBuyerExtLabel" runat="server" class="label" Text="Buyer/Ext:" />
                                </td>
                                <td nowrap="nowrap" colspan="3">
                                    <asp:Label ID="lblBuyerExtText" runat="server" Text="Hinton/5199" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblVendorLabel" runat="server" class="label" Text="Vendor:" />
                                </td>
                                <td nowrap="nowrap" colspan="3">
                                    <asp:Label ID="lblVendorText" runat="server" Text="26487- CURRANTS" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblBrandLabel" runat="server" class="label" Text="Brand:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblBrandText" runat="server" Text="Grane" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblLabellbl" runat="server" class="label" Text="Label:" />
                                </td>
                                <td nowrap="nowrap" colspan="3">
                                    <telerik:RadComboBox ID="cmbLabel" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="150" TabIndex="2" Filter="Contains">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="Grane" Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblFeatureImageLabel" runat="server" class="label" Text="Feature Image#:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblFeatureImageText" runat="server" Text="12345" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblImageKindLabel" runat="server" class="label" Text="Image Kind:" />
                                </td>
                                <td nowrap="nowrap">
                                    <telerik:RadComboBox ID="cmbImageKind" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="200" TabIndex="2" Filter="Contains">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="New, Pick-up, Vendor supplied" Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" colspan="12">
                                    <asp:Label ID="lblFriendlyProductDescription" runat="server" class="label" Text="Friendly Product Description:" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" colspan="3" rowspan="4">
                                    <telerik:RadTextBox ID="txtFriendlyProductDesc" runat="server" TabIndex="15" Height="100"
                                        Width="400" TextMode="MultiLine" Text="Grane Striped Long Sleeve Tee.">
                                    </telerik:RadTextBox>
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblImageType" runat="server" class="label" Text="Image Type:" />
                                </td>
                                <td nowrap="nowrap">
                                    <telerik:RadComboBox ID="cmbImageType" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="120" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="On/Off figure" Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblSizeAvailable" runat="server" class="label" Text="Size Available:" />
                                </td>
                                <td nowrap="nowrap" colspan="8">
                                    <telerik:RadComboBox ID="cmbSizeAvailable" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="300" TabIndex="2" Filter="Contains">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="Size Range will be available in Stub"
                                                Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblSizeCategory" runat="server" class="label" Text="Size Category:" />
                                </td>
                                <td nowrap="nowrap" colspan="8">
                                    <telerik:RadComboBox ID="cmbSizeCategory" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="300" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="Concat w/Style Desc Ms./Petite/Men's/Jr"
                                                Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblNoOfModelsLabel" runat="server" class="label" Text="Number of Models:" />
                                </td>
                                <td nowrap="nowrap" colspan="8">
                                    <telerik:RadComboBox ID="cmbNoOfModels" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="100" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="1" />
                                            <telerik:RadComboBoxItem runat="server" Text="2" Selected="True" />
                                            <telerik:RadComboBoxItem runat="server" Text="3" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" colspan="12">
                                    <asp:Label ID="lblFriendlyProductFeatures" runat="server" class="label" Text="Friendly Product Features:" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" colspan="3" rowspan="4">
                                    <telerik:RadTextBox ID="txtFriendlyProductFeatures" runat="server" TabIndex="15"
                                        Height="100" Width="400" TextMode="MultiLine" Text="Polyester / Pullover / Long Sleeve /Scoop Neck.">
                                    </telerik:RadTextBox>
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblModelAge" runat="server" class="label" Text="Model Age:" />
                                </td>
                                <td nowrap="nowrap" colspan="8">
                                    <telerik:RadComboBox ID="cmbModelAge" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="100" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="10s" />
                                            <telerik:RadComboBoxItem runat="server" Text="20s" />
                                            <telerik:RadComboBoxItem runat="server" Text="30s" Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblVendorApproval" runat="server" class="label" Text="Vendor Approval:" />
                                </td>
                                <td nowrap="nowrap" colspan="7">
                                    <telerik:RadComboBox ID="cmbVendorApproval" runat="server" MarkFirstMatch="true"
                                        OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                        AppendDataBoundItems="True" Width="100" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="Y/N" Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblModelCategory" runat="server" class="label" Text="Model Category:" />
                                </td>
                                <td nowrap="nowrap" colspan="7">
                                    <telerik:RadComboBox ID="cmbModelCategory" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="100" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="Y/N" Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblColorCorrect" runat="server" class="label" Text="Color Correct:" />
                                </td>
                                <td nowrap="nowrap" colspan="8">
                                    <telerik:RadComboBox ID="cmbColorCorrect" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="100" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="Y/N" Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" colspan="12">
                                    <asp:Label ID="lblMerchantNotesLabel" runat="server" class="label" Text="Merchant Notes:" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" colspan="3" rowspan="3">
                                    <telerik:RadTextBox ID="txtMerchantNotes" runat="server" TabIndex="15" Height="100"
                                        Width="400" TextMode="MultiLine" Text="Is this styling notes?A:more like styling or misc. notes. There will be individual y/n fields for the types of notes  that pertain. This  currently is used as an approval mechanism for coord notes. Prob can keep same approval proc... ">
                                    </telerik:RadTextBox>
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblStoreSample" runat="server" class="label" Text="Store Sample:" />
                                </td>
                                <td nowrap="nowrap" colspan="7">
                                    <telerik:RadComboBox ID="cmbStoreSample" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="200" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="Y for store/N for vendor" Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblSampleStore" runat="server" class="label" Text="Sample Store:" />
                                </td>
                                <td nowrap="nowrap" colspan="8">
                                    <telerik:RadComboBox ID="cmbSampleStore" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                                        Width="150" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="Store#" Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblAdditionalColorsSamples" runat="server" class="label" Text="Additional Colors/Samples:" />
                                </td>
                                <td nowrap="nowrap" colspan="8">
                                    <telerik:RadComboBox ID="cmbAdditionalColorsSamples" runat="server" MarkFirstMatch="true"
                                        OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                        AppendDataBoundItems="True" Width="250" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="Y/N are additional turn-ins coming?"
                                                Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" colspan="12">
                                    <asp:Label ID="lblOfferTextNonPPM" runat="server" class="label" Text="Offer Text (Non-PPM):" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" colspan="3" rowspan="2">
                                    <telerik:RadTextBox ID="RadTextBox1" runat="server" TabIndex="15" Height="100" Width="400"
                                        TextMode="MultiLine" Text="This is for non-PPM ads only. Is this really needed? The spreadsheet layout is going to have to be included anyway and this doesnot get paased anywhere so maybe it should stay on the layout spreadsheet.">
                                    </telerik:RadTextBox>
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblSellingLocationLabel" runat="server" class="label" Text="Selling Location:" />
                                </td>
                                <td nowrap="nowrap" colspan="7">
                                    <telerik:RadComboBox ID="cmbSellingLocation" runat="server" MarkFirstMatch="true"
                                        OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                        AppendDataBoundItems="True" Width="150" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="STORE/WEB - 3" Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblTurnInUsageIndicatorLabel" runat="server" class="label" Text="Turn-In Usage Indicator:" />
                                </td>
                                <td nowrap="nowrap" colspan="8">
                                    <telerik:RadComboBox ID="cmbTurnInUsageIndicator" runat="server" MarkFirstMatch="true"
                                        OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                        AppendDataBoundItems="True" Width="300" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="Print,eComm, Both" Selected="True" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <table align="center">
                            <tr>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblPageNumber" runat="server" Text="Page 1 of 20" class="bigLabel" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </telerik:RadPageView>
                <telerik:RadPageView ID="pvColorSizeLevel" runat="server" Height="100%">
                    <asp:Panel ID="pnlColorSizeLevel" runat="server" Visible="true" Height="100%">
                        <table align="right">
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" class="label" Text="User/Create Date:" />
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Stoll, S 10/31/12" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" class="label" Text="User/Modify Date:" />
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Stoll, S 10/31/12" />
                                </td>
                            </tr>
                        </table>
                        <table class="labels-vertical" style="margin: 20px;" width="700">
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblAdNoColorSizeLabel" runat="server" class="label" Text="Ad #:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblAdNoColorSizeText" runat="server" Text="31248" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblAdNoDescColorSizeLabel" runat="server" class="label" Text="Desc:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblAdNoDescColorSizeText" runat="server" Text="10_10 Anniversary Sale Mailer" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblAdNoRunStartColorSizeLabel" runat="server" class="label" Text="Run Start:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblAdNoRunStartColorSizeText" runat="server" Text="10/10/12" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblAdNoPageNoColorSizeLabel" runat="server" class="label" Text="Page #:" />
                                </td>
                                <td nowrap="nowrap">
                                    <telerik:RadComboBox ID="cmbPageNoColorSize" runat="server" MarkFirstMatch="true"
                                        OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                        AppendDataBoundItems="True" Width="50" TabIndex="2">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="1" />
                                            <telerik:RadComboBoxItem runat="server" Text="2" />
                                            <telerik:RadComboBoxItem runat="server" Text="3" />
                                            <telerik:RadComboBoxItem runat="server" Text="4" />
                                            <telerik:RadComboBoxItem runat="server" Text="5" Selected="True" />
                                            <telerik:RadComboBoxItem runat="server" Text="6" />
                                        </Items>
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblAdNoPageNoDescColorSizeLabel" runat="server" class="label" Text="Desc:" />
                                </td>
                                <td nowrap="nowrap" colspan="5">
                                    <asp:Label ID="lblAdNoPageNoDescColorSizeText" runat="server" Text="Page Description" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblAdNoRunEndColorSizeLabel" runat="server" class="label" Text="Run End:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblAdNoRunEndColorSizeText" runat="server" Text="10/18/12" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblDeptColorSizeLabel" runat="server" class="label" Text="Dept:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblDeptColorSizeText" runat="server" Text="254 - MS RUFF HEWN" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblBuyerColorSizeLabel" runat="server" class="label" Text="Buyer:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblBuyerColorSizeText" runat="server" Text="Ryan" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblBuyerExtColorSizeLabel" runat="server" class="label" Text="Buyer Ext:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblBuyerExtColorSizeText" runat="server" Text="5199" />
                                </td>
                            </tr>
                        </table>
                        <telerik:RadGrid ID="grdColorSizeLevel" runat="server" AllowPaging="True" Height="80%"
                            SkinID="CenteredWithScroll" CssClass="AddBorders" ShowFooter="true">
                            <ClientSettings>
                                <Resizing AllowColumnResize="false" />
                                <Scrolling AllowScroll="true" FrozenColumnsCount="8" />
                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                            </ClientSettings>
                            <AlternatingItemStyle BackColor="White" />
                            <MasterTableView CommandItemDisplay="Top">
                                <NoRecordsTemplate>
                                    no records retrieved</NoRecordsTemplate>
                                <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                <CommandItemTemplate>
                                    <div style="padding: 5px 5px;">
                                        <asp:Button ID="btnFlood" runat="server" CssClass="label" CommandName="Flood" Text="Flood"
                                            ForeColor="White" BackColor="#006699"></asp:Button>
                                            <telerik:RadTextBox runat="server" ID="rtbImgName" Width="100" EmptyMessageStyle-Font-Italic="true" EmptyMessage="Picture Name" ToolTip="Picture Name">
                                            </telerik:RadTextBox>
                                        <telerik:RadComboBox ID="rcbFloodImgType" runat="server" Width="100px" Height="80px"
                                            MarkFirstMatch="true" AllowCustomText="false" EmptyMessage="On/Off Figure" ToolTip="On/Off Figure">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="" Value="" Selected="true"></telerik:RadComboBoxItem>
                                                <telerik:RadComboBoxItem Text="ON" Checked="true" />
                                                <telerik:RadComboBoxItem Text="OFF" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadComboBox ID="cmbFloodImageKind" runat="server" Width="90" EmptyMessage="Image Kind"
                                            ToolTip="Image Kind">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Up" Checked="true" />
                                                <telerik:RadComboBoxItem Text="Vendor" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadTextBox runat="server" ID="rtbFloodModelCount" EmptyMessageStyle-Font-Italic="true"
                                            Width="60" EmptyMessage="# Models" ToolTip="# Models">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox runat="server" ID="rtbFloodModelAge" EmptyMessageStyle-Font-Italic="true"
                                            Width="70" EmptyMessage="Model Age" ToolTip="Model Age">
                                        </telerik:RadTextBox>
                                        <telerik:RadComboBox ID="cmbFloodModelCat" runat="server" Width="90" EmptyMessage="Model Age" ToolTip="Model Age">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Snr Msy" Checked="true" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridClientSelectColumn UniqueName="selColumn">
                                        <ItemStyle Width="25px" />
                                        <HeaderStyle Width="25px" />
                                    </telerik:GridClientSelectColumn>
                                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                        EditImageUrl="~/Images/Edit1.gif" EditText="Edit" CancelImageUrl="~/Images/Delete.gif"
                                        CancelText="Cancel" UpdateImageUrl="~/Images/CheckMark.gif" UpdateText="Update">
                                        <ItemStyle Width="40px" />
                                        <HeaderStyle Width="40px" />
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogHeight="160px"
                                        UniqueName="DeleteColumn" ImageUrl="~/Images/Delete.gif">
                                        <HeaderStyle HorizontalAlign="Center" Width="24px" />
                                        <ItemStyle HorizontalAlign="Center" Width="24px" />
                                    </telerik:GridButtonColumn>
                                    <telerik:GridBoundColumn DataField="Vendor" HeaderText="Vendor">
                                        <HeaderStyle HorizontalAlign="Center" Width="150" Font-Bold="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="VendorStyle" HeaderText="Vendor Style">
                                        <HeaderStyle HorizontalAlign="Center" Width="90" Font-Bold="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Color" HeaderText="Color">
                                        <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn DataField="Size" HeaderText="Size">
                                        <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                                        <ItemTemplate>
                                            <telerik:RadTextBox runat="server" ID="rtbSize" Text='<%# Eval("Size") %>' Width="100%">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Offer" HeaderText="Offer">
                                        <HeaderStyle HorizontalAlign="Center" Width="90" Font-Bold="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn HeaderText="Picture Name">
                                        <ItemTemplate>
                                            <telerik:RadTextBox runat="server" ID="rtbImgName" Text="test" Width="100%" TextMode="MultiLine">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="70" Font-Bold="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="On/Off">
                                        <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                                        <ItemTemplate>
                                            <telerik:RadComboBox ID="cmbSampleStatus" runat="server" Width="100%">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="ON" Checked="true" />
                                                    <telerik:RadComboBoxItem Text="OFF" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Img Kind">
                                        <HeaderStyle HorizontalAlign="Center" Width="70" Font-Bold="True" />
                                        <ItemTemplate>
                                            <telerik:RadComboBox ID="cmbImageType" runat="server" Width="100%">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Up" Checked="true" />
                                                    <telerik:RadComboBoxItem Text="Vendor" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="# Models">
                                        <ItemTemplate>
                                            <telerik:RadTextBox runat="server" ID="rtbModelCount" Text='<%# Eval("ModelCount") %>'
                                                Width="100%">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="55" Font-Bold="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Model<br>Age">
                                        <ItemTemplate>
                                            <telerik:RadTextBox runat="server" ID="rtbModelAge" Text='<%# Eval("ModelAge") %>'
                                                Width="100%">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="65" Font-Bold="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Model Cat">
                                        <ItemTemplate>
                                            <telerik:RadComboBox ID="cmbModelCat" runat="server" Width="100%">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Snr Msy" Checked="true" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="90" Font-Bold="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Product Features" UniqueName="ProductFeatures">
                                        <ItemTemplate>
                                            <telerik:RadTextBox runat="server" ID="rtbProductFeatures" Text='<%# Eval("ProductFeatures") %>'
                                                Width="100%" TextMode="MultiLine">
                                                <ClientEvents OnFocus="FocusTextBox" OnBlur="BlurTextBox" />
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="250" Font-Bold="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Style Desc">
                                        <ItemTemplate>
                                            <telerik:RadTextBox runat="server" ID="rtbStyleDescription" Text='<%# Eval("StyleDescription") %>'
                                                Width="100%" TextMode="MultiLine">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="250" Font-Bold="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Sample Status">
                                        <HeaderStyle HorizontalAlign="Center" Width="95" Font-Bold="True" />
                                        <ItemTemplate>
                                            <telerik:RadComboBox ID="cmbSampleSt" runat="server" Width="100%">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Available" Checked="true" />
                                                    <telerik:RadComboBoxItem Text="UnAvailable" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Sample Status<br>Details">
                                        <ItemTemplate>
                                            <telerik:RadTextBox runat="server" ID="rtbSampleStatusDetails" Text='<%# Eval("SampleStatusDetails") %>'
                                                Width="100%" TextMode="MultiLine">
                                                <ClientEvents OnFocus="FocusTextBox" OnBlur="BlurTextBox" />
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="250" Font-Bold="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Route From Ad">
                                        <ItemTemplate>
                                            <telerik:RadTextBox runat="server" ID="rtbRouteFromAd" Text='<%# Eval("RouteFromAd") %>'
                                                Width="100%">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="70" Font-Bold="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Grp">
                                        <ItemTemplate>
                                            <telerik:RadTextBox runat="server" ID="rtbGroup" Text='<%# Eval("Group") %>' Width="100%">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="35" Font-Bold="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Seq">
                                        <ItemTemplate>
                                            <telerik:RadTextBox runat="server" ID="rtbSequence" Text='<%# Eval("Sequence") %>'
                                                Width="100%">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="35" Font-Bold="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Qty">
                                        <ItemTemplate>
                                            <telerik:RadTextBox runat="server" ID="rtbQty" Text='<%# Eval("Qty") %>' Width="100%">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="35" Font-Bold="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Notes">
                                        <ItemTemplate>
                                            <telerik:RadTextBox runat="server" ID="rtbNotes" Text='<%# Eval("Notes") %>' Width="100%"
                                                TextMode="MultiLine">
                                                <ClientEvents OnFocus="FocusTextBox" OnBlur="BlurTextBox" />
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="250" Font-Bold="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Prop">
                                        <ItemTemplate>
                                            <telerik:RadComboBox ID="cmbProp" runat="server" Width="100%">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Yes" Checked="true" />
                                                    <telerik:RadComboBoxItem Text="No" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </asp:Panel>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
            <asp:HiddenField ID="hdnVendorStyle" runat="server" />
        </telerik:RadPane>
    </telerik:RadSplitter>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function FocusTextBox(sender, eventArgs) {
                var textBox = sender;
                textBox._originalTextBoxCssText = "width:100%;height:240px;";
                textBox.updateCssClass();
            }

            function BlurTextBox(sender, eventArgs) {
                var textBox = sender;
                textBox._originalTextBoxCssText = "width:100%;height:100%;";
                textBox.updateCssClass();
            }

        </script>
    </telerik:RadCodeBlock>
</asp:Content>
