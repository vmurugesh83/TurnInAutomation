<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PreTurnInSetUp.aspx.vb"
    ValidateRequest="false" MasterPageFile="~/ContentPage.Master" Inherits="TurnInProcessAutomation.PreTurnInSetUpCreate" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/ContentPage.Master" %>
<%@ Reference Control="~/WebUserControls/eCommerce/eCommPreTurnInSetUpCtrl.ascx" %>
<%@ Register TagPrefix="TurnIn" TagName="ModalOrderISN" Src="~/WebUserControls/ModalOrderISN.ascx" %>
<%@ Register TagPrefix="TurnIn" TagName="ModalMoveBatch" Src="~/WebUserControls/ModalMoveBatch.ascx" %>
<asp:Content ID="PreTurnInCreateForm" ContentPlaceHolderID="ContentArea" runat="Server">
    <telerik:RadAjaxManagerProxy ID="RadAjaxMgrProxyPreTurnInSetup" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdResultList">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdResultList" LoadingPanelID="ralpLoadPnl" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdKilled">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdKilled" LoadingPanelID="ralpLoadPnl" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbWebCategoriesLevel1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel1" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel2" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel3" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel4" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel5" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel6" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel1" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel2" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel3" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel4" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel5" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel6" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbWebCategoriesLevel2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel1" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel2" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel3" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel4" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel5" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel6" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel1" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel2" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel3" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel4" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel5" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel6" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbWebCategoriesLevel3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel1" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel2" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel3" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel4" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel5" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel6" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel1" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel2" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel3" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel4" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel5" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel6" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbWebCategoriesLevel4">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel1" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel2" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel3" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel4" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel5" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel6" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel1" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel2" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel3" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel4" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel5" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel6" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbWebCategoriesLevel5">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel1" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel2" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel3" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel4" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel5" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel6" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel1" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel2" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel3" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel4" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel5" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel6" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbWebCategoriesLevel6">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel1" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel2" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel3" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel4" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel5" />
                    <telerik:AjaxUpdatedControl ControlID="cmbWebCategoriesLevel6" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel1" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel2" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel3" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel4" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel5" />
                    <telerik:AjaxUpdatedControl ControlID="imgAddWebCategoriesLevel6" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="imgAddWebCategoriesLevel1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdWebCategories" LoadingPanelID="RadAjaxLoadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="imgAddWebCategoriesLevel2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdWebCategories" LoadingPanelID="RadAjaxLoadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="imgAddWebCategoriesLevel3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdWebCategories" LoadingPanelID="RadAjaxLoadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="imgAddWebCategoriesLevel4">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdWebCategories" LoadingPanelID="RadAjaxLoadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="imgAddWebCategoriesLevel5">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdWebCategories" LoadingPanelID="RadAjaxLoadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="imgAddWebCategoriesLevel6">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdWebCategories" LoadingPanelID="RadAjaxLoadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdWebCategories">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="MessagePanel1" LoadingPanelID="RadAjaxLoadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="mpPreTurnInCreate" LoadingPanelID="RadAjaxLoadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    <telerik:RadSplitter ID="rsPreTurnInCreate" runat="server" SkinID="pageSplitter">
        <telerik:RadPane ID="rpHeader" runat="server" SkinID="pageHeaderAreaPane" Height="20%">
            <TurnIn:ModalOrderISN ID="tuModalOrderISN" runat="server" />
            <div id="pageActionBar">
                <telerik:RadToolBar ID="rtbPreTurnInCreate" runat="server" OnClientButtonClicking="OnClientRadToolBarClick"
                    OnClientLoad="clientLoad" CssClass="SeparatedButtons">
                    <Items>
                        <telerik:RadToolBarButton runat="server" CommandName="Back" DisabledImageUrl="~/Images/BackButton.gif"
                            ImageUrl="~/Images/BackButton.gif" Text="Back" CausesValidation="false" TabIndex="16">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Reset" DisabledImageUrl="~/Images/Reset_d.gif"
                            ImageUrl="~/Images/Reset.gif" Text="Reset" CssClass="rightAligned" CausesValidation="false"
                            TabIndex="15">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Sort" DisabledImageUrl="~/Images/List_d.gif"
                            ImageUrl="~/Images/List.gif" Text="Sort" CssClass="rightAligned" Visible="false">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="LevelDown" Text="Level Down"
                            CssClass="rightAligned" DisabledImageUrl="~/Images/Retrieve2_d.gif" ImageUrl="~/Images/Retrieve2.gif"
                            Visible="false" />
                        <telerik:RadToolBarButton runat="server" CommandName="LevelUp" Text="Level Up" CssClass="rightAligned"
                            DisabledImageUrl="~/Images/Retrieve2_d.gif" ImageUrl="~/Images/Retrieve2.gif"
                            Visible="false" />
                        <telerik:RadToolBarButton runat="server" CommandName="Save" DisabledImageUrl="~/Images/Save_d.gif"
                            ImageUrl="~/Images/Save.gif" Text="Save" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="AddToBatch" DisabledImageUrl="~/Images/Add_d.gif"
                            ImageUrl="~/Images/Add.gif" Text="Add To Batch" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Retrieve" DisabledImageUrl="~/Images/Retrieve1_d.gif"
                            ImageUrl="~/Images/Retrieve1.gif" Text="Retrieve" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="CancelAll" DisabledImageUrl="~/Images/Cancel_d.gif"
                            ImageUrl="~/Images/Cancel.gif" Text="Cancel All" CssClass="rightAligned" Visible="false">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="SaveAll" DisabledImageUrl="~/Images/Save_d.gif"
                            ImageUrl="~/Images/Save.gif" Text="Save All" CssClass="rightAligned" Visible="false">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="EditAll" DisabledImageUrl="~/Images/Edit3_d.gif"
                            ImageUrl="~/Images/Edit3.gif" Text="Edit All" CssClass="rightAligned" Visible="false">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Submit" Text="Submit" DisabledImageUrl="~/Images/Submit.gif"
                            ImageUrl="~/Images/Submit.gif" CssClass="rightAligned" Height="26" ToolTip="Submit to TurnIn Meeting">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="PrintReport" DisabledImageUrl="~/Images/Print_d.gif"
                            ImageUrl="~/Images/Print.gif" Text="Print Report" CssClass="rightAligned" Height="26">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="PrintLabel" DisabledImageUrl="~/Images/Print_d.gif"
                            ImageUrl="~/Images/Print.gif" Text="Print Labels" CssClass="rightAligned" Height="26">
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
            </div>
            <asp:Panel ID="pnlTabStrip" runat="server" CssClass="pageTabStrip" Style="margin: 0;">
                <telerik:RadTabStrip ID="rtsAPAdjustment" runat="server" MultiPageID="rmpPreTurnInCreate"
                    SelectedIndex="1" AutoPostBack="true" OnClientTabSelecting="OnClientTabClicking"
                    OnClientTabUnSelected="OnClientTabOut">
                    <Tabs>
                        <telerik:RadTab runat="server" Text="Ad List" PageViewID="pvAdList" Font-Bold="True"
                            Visible="false" />
                        <telerik:RadTab runat="server" Text="Ad Level" PageViewID="pvAdLevel" Font-Bold="True"
                            Visible="false" />
                        <telerik:RadTab runat="server" Text="Result List" PageViewID="pvResultList" Font-Bold="True"
                            Visible="false" />
                        <telerik:RadTab runat="server" Text="Killed Items" PageViewID="pvKilled" Font-Bold="True"
                            Visible="false" />
                        <telerik:RadTab runat="server" Text="ISN Level" PageViewID="pvISNLevel" Font-Bold="True"
                            Visible="false" />
                        <telerik:RadTab runat="server" Text="Color/Size Level" PageViewID="pvColorSizeLevel"
                            Font-Bold="True" Visible="false" />
                    </Tabs>
                </telerik:RadTabStrip>
            </asp:Panel>
            <div id="pageHeader" style="height: 100px;">
                <asp:Label ID="lblPageHeader" runat="server" Text="E-Comm Turn-In Setup " />
                <bonton:MessagePanel ID="MessagePanel1" runat="server" />
                <br />
            </div>
            <bonton:MessagePanel ID="mpPreTurnInCreate" runat="server" />
        </telerik:RadPane>
        <telerik:RadPane ID="rpContent" runat="server">
            <asp:Panel ID="pnlAdLevel" runat="server" Visible="false">
                <table class="labels-vertical" style="margin: 20px 0 0 20px;" width="980">
                    <tr>
                        <td style="white-space: nowrap; width: 60px;" align="right">
                            <asp:Label ID="lblBatchId" runat="server" CssClass="label" Text="Batch Id:" />
                        </td>
                        <td style="white-space: nowrap; width: 100;">
                            <asp:Label ID="lblBatchIdText" runat="server" />
                        </td>
                        <td style="white-space: nowrap; width: 140px;" align="right">
                            <asp:Label ID="lblAdNoLabel" runat="server" CssClass="label" Text="Ad #:" />
                        </td>
                        <td style="white-space: nowrap; width: 250px;">
                            <asp:Label ID="lblAdNoText" runat="server" />
                            -
                            <asp:Label ID="lblAdNoDescText" runat="server" />
                            <asp:HiddenField runat="server" ID="hdnAdStartDate" />
                            <asp:HiddenField ID="hdnAdEndDate" runat="server" />
                        </td>
                        <td style="white-space: nowrap; width: 140px;" align="right">
                            <asp:Label ID="lblPageNbr" runat="server" CssClass="label" Text="Page #:" />
                        </td>
                        <td style="white-space: nowrap; width: 165px;">
                            <asp:Label ID="lblPageNumberText" runat="server" />
                            <asp:HiddenField runat="server" ID="hdnSysPgNbr" />
                            <asp:HiddenField runat="server" ID="hdnBuyer" />
                            <asp:HiddenField runat="server" ID="hdnISNTabChanges" />
                            <asp:HiddenField runat="server" ID="hdnISNTabSaveFlg" Value="N" />
                        </td>
                        <td style="white-space: nowrap; width: 140px;" align="right">
                            <asp:Label ID="lblTurnInDate" runat="server" CssClass="label" Text="Turn-In Date:" />
                        </td>
                        <td style="white-space: nowrap; width: 145px;">
                            <asp:Label ID="lblTurnInDateText" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <telerik:RadMultiPage ID="rmpPreTurnInCreate" SelectedIndex="0" runat="server" Height="90%">
                <telerik:RadPageView ID="pvAdList" runat="server" Height="100%">
                    <telerik:RadGrid ID="grdMaint" runat="server" SkinID="CenteredWithScroll" AllowPaging="True"
                        Visible="false">
                        <ClientSettings>
                            <Resizing AllowColumnResize="true" />
                        </ClientSettings>
                        <MasterTableView>
                            <EditFormSettings>
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                            </EditFormSettings>
                            <NoRecordsTemplate>
                                no records retrieved
                            </NoRecordsTemplate>
                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="" UniqueName="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkItem" runat="server" AutoPostBack="true" Checked="false" OnCheckedChanged="AdListToggleSelectItem" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="30px" />
                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Batch #" UniqueName="BatchId">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <% If (Request.QueryString("Action").ToLower() = "maintenance") Then%>
                                        <a href="#" onclick="javascript:ShowModalMoveBatch();"><%#Eval("BatchId")%></a>
                                        <% Else%>
                                        <%#Eval("BatchId")%>
                                        <% End If%>
                                        <TurnIn:ModalMoveBatch ID="tuModalMoveBatch" runat="server" BatchNum='<%#Eval("BatchId")%>'
                                            oldAdNum='<%#Eval("AdNumber")%>' Week='<%# GetAdWeek(Eval("AdNumber"))%>' OnMoveBatch="tuModalMoveBatch_MoveBatch" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="AdNumber" HeaderText="Ad #" UniqueName="AdNumber">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Ad Desc" UniqueName="AdDescription">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Turn-In Date" UniqueName="TurnInDate">
                                    <HeaderStyle HorizontalAlign="Center" Width="120" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PageNumber" HeaderText="Page #" UniqueName="PageNumber">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Page Desc" UniqueName="PageDescription">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Buyer" HeaderText="Buyer" UniqueName="Buyer">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Departments" HeaderText="Dept" UniqueName="Depts">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UserId" HeaderText="User" UniqueName="User">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </telerik:RadPageView>
                <telerik:RadPageView ID="pvAdLevel" runat="server" Height="100%"></telerik:RadPageView>
                <telerik:RadPageView ID="pvResultList" runat="server" Height="100%">
                    <asp:Panel ID="Panel1" runat="server" Visible="true">
                        <table align="center">
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="grdResultList" runat="server" SkinID="CenteredWithScroll" AllowPaging="True"
                                        Height="520" Width="1048" ShowFooter="False" AllowSorting="true" Visible="false">
                                        <MasterTableView>
                                            <EditFormSettings>
                                                <EditColumn UniqueName="EditCommandColumn1">
                                                </EditColumn>
                                            </EditFormSettings>
                                            <NoRecordsTemplate>
                                                no records retrieved
                                            </NoRecordsTemplate>
                                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                            <Columns>
                                                <telerik:GridImageColumn UniqueName="TUWarning" ImageUrl="" Visible="true">
                                                    <HeaderStyle Width="30px" />
                                                    <ItemStyle Width="30px" />
                                                </telerik:GridImageColumn>
                                                <telerik:GridTemplateColumn HeaderText="" UniqueName="Select">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkAll" runat="server" AllowMultiRowSelection="true" AutoPostBack="true"
                                                            Checked="false" OnCheckedChanged="ToggleSelectALL" ToolTip="Select/Deselect All" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkISNLevel" runat="server" AutoPostBack="true" Checked="false"
                                                            OnCheckedChanged="ToggleSelectISN" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="32px" />
                                                    <ItemStyle Width="32px" HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="ACode" HeaderText="A-CD 1" UniqueName="ACode">
                                                    <HeaderStyle HorizontalAlign="Center" Width="50" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn DataField="Vendor" HeaderText="Vendor" UniqueName="Vendor"
                                                    SortExpression="VendorName">
                                                    <HeaderStyle HorizontalAlign="Center" Width="130" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <%# Eval("VendorId") %>
                                                        -
                                                        <%# Eval("VendorName") %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="IsReserve" HeaderText="RSV?" UniqueName="IsReserve">
                                                    <HeaderStyle HorizontalAlign="Center" Width="40" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ISN" HeaderText="ISN" UniqueName="ISN">
                                                    <HeaderStyle HorizontalAlign="Center" Width="70" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ISNDesc" HeaderText="ISN Desc" UniqueName="ISNDesc">
                                                    <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="VendorStyleNumber" HeaderText="Vendor Style"
                                                    UniqueName="VendorStyleNumber">
                                                    <HeaderStyle HorizontalAlign="Center" Width="90" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="StartShipDate" HeaderText="Start Ship Date"
                                                    UniqueName="StartShipDate">
                                                    <HeaderStyle HorizontalAlign="Center" Width="70" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="SellYear" HeaderText="Sell Year" UniqueName="SellYear">
                                                    <HeaderStyle HorizontalAlign="Center" Width="40" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="SellSeason" HeaderText="Sell Season" UniqueName="SellSeason">
                                                    <HeaderStyle HorizontalAlign="Center" Width="50" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Turned-In (Print)" SortExpression="TurnedInPrintAdNos"
                                                    UniqueName="IsTurnedInPrint">
                                                    <HeaderStyle HorizontalAlign="Center" Width="60" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <%# Eval("IsTurnedInPrint")%><asp:HiddenField runat="server" ID="hdnAdNoP" Value='<%# Eval("TurnedInPrintAdNos")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Turned-In (eComm)" SortExpression="TurnedInEcommAdNos"
                                                    UniqueName="IsTurnedInEcomm">
                                                    <HeaderStyle HorizontalAlign="Center" Width="60" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <%# Eval("IsTurnedInEcomm")%><asp:HiddenField runat="server" ID="hdnAdNoE" Value='<%# Eval("TurnedInEcommAdNos")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="OnOrder" HeaderText="OO" UniqueName="OO">
                                                    <HeaderStyle HorizontalAlign="Center" Width="40" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="WebCatAvailableQty" HeaderText="WebCat Available Qty" UniqueName="WebCatAvailableQty">
                                                    <HeaderStyle HorizontalAlign="Center" Width="60" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="OnHand" HeaderText="OH" UniqueName="OH">
                                                    <HeaderStyle HorizontalAlign="Center" Width="40" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Available For Turn-in" UniqueName="IsAvailableForTurnIn">
                                                    <HeaderStyle HorizontalAlign="Center" Width="70" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <%# Eval("AvailableForTurnIn")%><asp:HiddenField runat="server" ID="hdnIsAvailableForTurnIn" Value='<%# Eval("AvailableForTurnIn")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Active On Web" UniqueName="IsActiveOnWeb">
                                                    <HeaderStyle HorizontalAlign="Center" Width="60" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <%# Eval("ActiveOnWeb")%><asp:HiddenField runat="server" ID="hdnIsActiveOnWeb" Value='<%# Eval("ActiveOnWeb")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="AlreadyExistsInBatch" Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="AlreadyProcessed" Visible="false" UniqueName="AlreadyProcessed">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                            <DetailTables>
                                                <telerik:GridTableView Name="grdSecondLevel" AutoGenerateColumns="false" HorizontalAlign="Left"
                                                    ShowFooter="false" AllowSorting="False" Width="400">
                                                    <NoRecordsTemplate>
                                                        Detail does not exist.
                                                    </NoRecordsTemplate>
                                                    <Columns>
                                                        <telerik:GridImageColumn UniqueName="TUWarning" ImageUrl="" Visible="true">
                                                            <HeaderStyle Width="30px" />
                                                            <ItemStyle Width="30px" />
                                                        </telerik:GridImageColumn>
                                                        <telerik:GridTemplateColumn HeaderText="" UniqueName="Select">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkAll" runat="server" AllowMultiRowSelection="true" AutoPostBack="true"
                                                                    Checked="false" ToolTip="Select/Deselect All" OnCheckedChanged="ToggleSelectISNFromNestedGrid" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkColorLevel" runat="server" AutoPostBack="true" Checked="false"
                                                                    OnCheckedChanged="ToggleColorLevel" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="32px" />
                                                            <ItemStyle Width="32px" HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn DataField="VendorColor" HeaderText="Vendor Color" UniqueName="VendorColor">
                                                            <HeaderStyle Width="100" HorizontalAlign="Center" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <ItemTemplate>
                                                                <%# Eval("ColorCode")%>
                                                                -
                                                                <%# Eval("ColorDesc") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="SellYear" HeaderText="Sell Year" UniqueName="SellYear">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="SellSeason" HeaderText="Sell Season" UniqueName="SellSeason">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Turned-In (Print)" UniqueName="IsTurnedInPrint">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <%# Eval("IsTurnedInPrint")%><asp:HiddenField runat="server" ID="hdnAdNoP" Value='<%# Eval("TurnedInPrintAdNos")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Turned-In (eComm)" UniqueName="IsTurnedInEcomm">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <%# Eval("IsTurnedInEcomm")%><asp:HiddenField runat="server" ID="hdnAdNoE" Value='<%# Eval("TurnedInEcommAdNos")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="StartShipDate" HeaderText="Start Ship Date" UniqueName="POStartShipDate">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="OnOrder" HeaderText="OO" UniqueName="OnOrder">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="OnOrderByShipDate" HeaderText="OO" UniqueName="OnOrderByShipDate">
                                                            <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="WebCatAvailableQty" HeaderText="WebCat Available Qty" UniqueName="WebCatAvailableQuantity">
                                                            <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="OnHand" HeaderText="OH" UniqueName="OnHand">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Available For Turn-in" UniqueName="SampleAvailableForTurnIn">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <%# Eval("AvailableForTurnIn")%><asp:HiddenField runat="server" ID="hdnSampleAvailable" Value='<%# Eval("AvailableForTurnIn")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Active On Web" UniqueName="SampleApproved">
                                                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <%# Eval("ActiveOnWeb")%><asp:HiddenField runat="server" ID="hdnSampleApproved" Value='<%# Eval("ActiveOnWeb")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
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
                <telerik:RadPageView ID="pvKilled" runat="server" Height="100%">
                    <asp:Panel ID="pnlKilled" runat="server" Visible="true">
                        <table align="center">
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="grdKilled" runat="server" AllowPaging="True" ShowFooter="False"
                                        Visible="false" AllowMultiRowSelection="True" SkinID="CenteredWithScroll" AutoGenerateColumns="false"
                                        GroupingEnabled="true" CssClass="AddBorders">
                                        <ClientSettings EnableRowHoverStyle="false">
                                            <Scrolling FrozenColumnsCount="0" AllowScroll="false" />
                                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                        </ClientSettings>
                                        <AlternatingItemStyle BackColor="White" />
                                        <MasterTableView ClientDataKeyNames="DeptID,ISN,VendorStyleNum,TurnInMerchID,AdminMerchNum,UPC,VendorName,IsnDesc,RemoveMerchFlag,IsReserve,Status"
                                            DataKeyNames="DeptID,ISN,VendorStyleNum,TurnInMerchID,AdminMerchNum,UPC,VendorName,IsnDesc,RemoveMerchFlag,IsReserve,Status"
                                            GroupsDefaultExpanded="true" EditMode="InPlace">
                                            <NoRecordsTemplate>
                                                no records retrieved
                                            </NoRecordsTemplate>
                                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                            <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="Sequence" SortOrder="Ascending"></telerik:GridGroupByField>
                                                        <telerik:GridGroupByField FieldName="DeptID" SortOrder="Ascending"></telerik:GridGroupByField>
                                                    </GroupByFields>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldAlias="Dept" FieldName="DeptIdDesc"></telerik:GridGroupByField>
                                                    </SelectFields>
                                                </telerik:GridGroupByExpression>
                                                <telerik:GridGroupByExpression>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="ISN" SortOrder="Ascending"></telerik:GridGroupByField>
                                                    </GroupByFields>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField HeaderText="-ISN" FieldAlias="ISN" FieldName="ISN"></telerik:GridGroupByField>
                                                        <telerik:GridGroupByField HeaderText="Vendor Name" FieldAlias="VendorName" FieldName="VendorName"></telerik:GridGroupByField>
                                                        <telerik:GridGroupByField HeaderText="Vendor Style" FieldAlias="VendorStyleNum" FieldName="VendorStyleNum"></telerik:GridGroupByField>
                                                        <telerik:GridGroupByField HeaderText=" " FieldAlias="IsnDesc" FieldName="IsnDesc"
                                                            HeaderValueSeparator=""></telerik:GridGroupByField>
                                                    </SelectFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
                                            <Columns>
                                                <telerik:GridClientSelectColumn UniqueName="selColumn">
                                                    <ItemStyle Width="20px" />
                                                    <HeaderStyle Width="20px" />
                                                </telerik:GridClientSelectColumn>
                                                <telerik:GridBoundColumn DataField="AdNumber" HeaderText="Ad" UniqueName="Ad" ReadOnly="true">
                                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="30px" />
                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="PageNumber" HeaderText="Page" UniqueName="Page"
                                                    ReadOnly="true">
                                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="30px" />
                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="BatchNumber" HeaderText="Batch" UniqueName="Batch"
                                                    ReadOnly="true">
                                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="30px" />
                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="IsReserve" HeaderText="Rsv?" UniqueName="Reserve"
                                                    ReadOnly="true">
                                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="30px" />
                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Rush Sample" UniqueName="HotItem">
                                                    <HeaderStyle HorizontalAlign="Center" Width="40px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                    <ItemTemplate>
                                                        <%# Eval("IsHotItem") %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Friendly Product Desc" UniqueName="FriendlyProdDesc">
                                                    <HeaderStyle HorizontalAlign="Center" Width="200px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltrFriendlyProdDesc" Text='<%# Eval("FriendlyProdDesc")%>'></asp:Literal>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Friendly Product Features" UniqueName="FriendlyProdFeatures">
                                                    <HeaderStyle HorizontalAlign="Center" Width="200px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                    <ItemTemplate>
                                                        <%# Eval("FriendlyProdFeatures").Replace(vbCrLf, "<br />")%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="Color" HeaderText="Vendor Color" UniqueName="Color"
                                                    ReadOnly="true">
                                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="100px" />
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Friendly Color" UniqueName="FriendlyColor">
                                                    <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                    <ItemTemplate>
                                                        <%# Eval("FriendlyColor") %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Color Family" UniqueName="ColorFamily">
                                                    <HeaderStyle HorizontalAlign="Center" Width="90px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblColorFamily" ToolTip='<%# Eval("ColorFamily")%>'
                                                            Text='<%# Eval("ColorFamily").ToString.Split(","c)(0)%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Sample Size" UniqueName="SampleSize">
                                                    <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                    <ItemTemplate>
                                                        <%# If(Eval("SampleSize") <> "0",Eval("SampleSize"),"")  %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Color Correct" UniqueName="ColorCorrect">
                                                    <HeaderStyle HorizontalAlign="Center" Width="40px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                    <ItemTemplate>
                                                        <%# Eval("ColorCorrect")%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <%--                                                <telerik:GridTemplateColumn HeaderText="Image Kind" UniqueName="ImageKind">
                                                    <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                    <ItemTemplate>
                                                        <%# Eval("ImageKind")%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="P/U ImageID" UniqueName="PUImageID">
                                                    <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                    <ItemTemplate>
                                                        <%# Eval("PuImageID")%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Route from Ad" UniqueName="RouteFromAd">
                                                    <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                    <ItemTemplate>
                                                        <%# Eval("RouteFromAD") %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Image Group #" UniqueName="GroupNum">
                                                    <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    <ItemTemplate>
                                                        <%# Eval("GroupNum")%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Feature/ Render/ Swatch" UniqueName="FeatureRenderSwatch">
                                                    <HeaderStyle HorizontalAlign="Center" Width="70px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                    <ItemTemplate>
                                                        <%# Eval("FeatureRenderSwatch") %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Image Type" UniqueName="ImageType">
                                                    <HeaderStyle HorizontalAlign="Center" Width="40px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                    <ItemTemplate>
                                                        <%# Eval("ImageType") %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Alt. View" UniqueName="AlternateView">
                                                    <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    <ItemTemplate>
                                                        <%# Eval("AltView")%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Sample Store #" UniqueName="SampleStoreNum">
                                                    <HeaderStyle HorizontalAlign="Center" Width="100px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    <ItemTemplate>
                                                        <%# Eval("SampleStore")%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Merchant Notes" UniqueName="MerchantNotes">
                                                    <HeaderStyle HorizontalAlign="Center" Width="150px" Font-Bold="True" />
                                                    <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    <ItemTemplate>
                                                        <%# Eval("MerchantNotes") %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                --%>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </telerik:RadPageView>
                <telerik:RadPageView ID="pvISNLevel" runat="server" Height="100%">
                    <asp:Panel ID="pnlISNLevel" runat="server" Visible="true">
                        <table class="labels-vertical" style="margin: 20px 0 0 20px;" width="1005">
                            <tr>
                                <td style="white-space: nowrap;" align="right" valign="top">
                                    <asp:Label ID="lblISNLabel" runat="server" class="label" Text="ISN:" />
                                </td>
                                <td style="white-space: nowrap;">
                                    <asp:DropDownList ID="cmbISNLabel" runat="server" class="RadComboBox_Vista" AutoPostBack="true"
                                        onChange="CheckISNTabContentChange()">
                                    </asp:DropDownList>
                                    <br />
                                    <span style="font-weight: normal;">* = ISN has been saved</span>
                                </td>
                                <td style="white-space: nowrap;" align="right">
                                    <asp:Label ID="lblISNLongDesc" runat="server" class="label" Text="ISN Long Desc:" />
                                </td>
                                <td style="white-space: nowrap;">
                                    <asp:Label ID="lblISNLongDescText" runat="server" />
                                </td>
                                <td style="white-space: nowrap;" align="right">
                                    <asp:Label ID="lblUserModifyDateLabel" runat="server" class="label" Text="User/Modify Date:" />
                                </td>
                                <td style="white-space: nowrap;">
                                    <asp:Label ID="lblUserModifyDateText" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblVendorLabel" runat="server" class="label" Text="Vendor:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblVendorText" runat="server" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblGenericClass" runat="server" class="label" Text="Generic Class:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblGenericClassTxt" runat="server" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblProductDetail1" runat="server" class="label" Text="Product Detail 1:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblProductDetail1Text" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblVendorStyleLabel" runat="server" class="label" Text="Vendor Style:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblVendorStyleLabelText" runat="server" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblGenericSubclass" runat="server" class="label" Text="Generic Subclass:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblGenericSubclassText" runat="server" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblProductDetail2" runat="server" class="label" Text="Product Detail 2:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblProductDetail2Text" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblBrandLabel" runat="server" class="label" Text="Brand:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblBrandText" runat="server" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblPattern" runat="server" class="label" Text="Pattern:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblPatternText" runat="server" />
                                </td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblProductDetail3" runat="server" class="label" Text="Product Detail 3:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblProductDetail3Text" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblLabellbl" runat="server" class="label" Text="Label:" />
                                </td>
                                <td nowrap="nowrap">
                                    <telerik:RadComboBox ID="cmbLabel" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="False"
                                        Width="150" TabIndex="2" Filter="Contains">
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                    <bonton:ToolTipValidator ID="ttvISNTabLabel" runat="server" ControlToEvaluate="cmbLabel"
                                        ValidationGroup="ISNLevel" OnServerValidate="ttvcmbLabel_ServerValidate" />
                                </td>
                                <td style="white-space: nowrap;"></td>
                                <td style="white-space: nowrap;"></td>
                                <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblFeatureImageLabel" runat="server" class="label" Text="Feature Image#:" />
                                </td>
                                <td nowrap="nowrap">
                                    <asp:Literal ID="litFeatureImageText" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap="nowrap" align="right" valign="top">
                                    <asp:Label ID="lblWebCategoriesLabel" runat="server" class="label" Text="*Web Categories:" />
                                </td>
                                <td nowrap="nowrap" align="left" valign="top" colspan="3">
                                    <asp:Label ID="lblWebCatText" runat="server" Font-Bold="false" Text="(Please select at least one Category beyond/below a Display Only Category)" />
                                </td>
                            </tr>
                            <tr>
                                <td style="white-space: nowrap;"></td>
                                <td nowrap="nowrap" colspan="3">
                                    <telerik:RadComboBox ID="cmbWebCategoriesLevel1" runat="server" MarkFirstMatch="true"
                                        OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                        AutoPostBack="true" AppendDataBoundItems="false" Width="350" TabIndex="2" Filter="Contains">
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                    <telerik:RadButton ID="imgAddWebCategoriesLevel1" runat="server" Image-ImageUrl="~/Images/Add.gif"
                                        Image-DisabledImageUrl="~/Images/Add_d.gif" Width="16px" Height="16px" Visible="false" />
                                    <br />
                                    <telerik:RadComboBox ID="cmbWebCategoriesLevel2" runat="server" MarkFirstMatch="true"
                                        OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                        Enabled="false" AutoPostBack="true" AppendDataBoundItems="false" Width="350"
                                        TabIndex="2" Filter="Contains">
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                    <telerik:RadButton ID="imgAddWebCategoriesLevel2" runat="server" Image-ImageUrl="~/Images/Add.gif"
                                        Image-DisabledImageUrl="~/Images/Add_d.gif" Width="16px" Height="16px" Visible="false" />
                                    <br />
                                    <telerik:RadComboBox ID="cmbWebCategoriesLevel3" runat="server" MarkFirstMatch="true"
                                        OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                        Enabled="false" AutoPostBack="true" AppendDataBoundItems="false" Width="350"
                                        TabIndex="2" Filter="Contains">
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                    <telerik:RadButton ID="imgAddWebCategoriesLevel3" runat="server" Image-ImageUrl="~/Images/Add.gif"
                                        Image-DisabledImageUrl="~/Images/Add_d.gif" Width="16px" Height="16px" Visible="false" />
                                    <br />
                                    <telerik:RadComboBox ID="cmbWebCategoriesLevel4" runat="server" MarkFirstMatch="true"
                                        OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                        Enabled="false" AutoPostBack="true" AppendDataBoundItems="false" Width="350"
                                        TabIndex="2" Filter="Contains">
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                    <telerik:RadButton ID="imgAddWebCategoriesLevel4" runat="server" Image-ImageUrl="~/Images/Add.gif"
                                        Image-DisabledImageUrl="~/Images/Add_d.gif" Width="16px" Height="16px" Visible="false" />
                                    <br />
                                    <telerik:RadComboBox ID="cmbWebCategoriesLevel5" runat="server" MarkFirstMatch="true"
                                        OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                        Enabled="false" AutoPostBack="true" AppendDataBoundItems="false" Width="350"
                                        TabIndex="2" Filter="Contains">
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                    <telerik:RadButton ID="imgAddWebCategoriesLevel5" runat="server" Image-ImageUrl="~/Images/Add.gif"
                                        Image-DisabledImageUrl="~/Images/Add_d.gif" Width="16px" Height="16px" Visible="false" />
                                    <br />
                                    <telerik:RadComboBox ID="cmbWebCategoriesLevel6" runat="server" MarkFirstMatch="true"
                                        OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                        Enabled="false" AutoPostBack="true" AppendDataBoundItems="false" Width="350"
                                        TabIndex="2" Filter="Contains">
                                        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                    </telerik:RadComboBox>
                                    <telerik:RadButton ID="imgAddWebCategoriesLevel6" runat="server" Image-ImageUrl="~/Images/Add.gif"
                                        Image-DisabledImageUrl="~/Images/Add_d.gif" Width="16px" Height="16px" Visible="false" />
                                    <br />
                                    OR Copy from:
                                    <asp:DropDownList ID="ddlCopyWebCatsFromISN" runat="server" class="RadComboBox_Vista"
                                        AppendDataBoundItems="false">
                                    </asp:DropDownList>
                                    <telerik:RadButton ID="btnCopyWebCatsFromISN" runat="server" Text="Copy" />
                                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server">
                                    </telerik:RadAjaxLoadingPanel>
                                    <telerik:RadGrid ID="grdWebCategories" runat="server" ShowFooter="false" SkinID="CenteredWithScroll"
                                        Height="190px" Width="650px" ShowHeader="True" AutoGenerateColumns="False" CellSpacing="0"
                                        GridLines="None">
                                        <MasterTableView DataKeyNames="CategoryCode">
                                            <EditFormSettings>
                                                <EditColumn UniqueName="EditCommandColumn1">
                                                </EditColumn>
                                            </EditFormSettings>
                                            <NoRecordsTemplate>
                                                no records retrieved
                                            </NoRecordsTemplate>
                                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                            <CommandItemSettings ExportToPdfText="Export to PDF" />
                                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                            </RowIndicatorColumn>
                                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                            </ExpandCollapseColumn>
                                            <Columns>
                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogType="RadWindow"
                                                    ConfirmText="Delete this record?" ConfirmTitle="Delete" ImageUrl="~/Images/Delete.gif"
                                                    Text="Delete" UniqueName="DeleteColumn">
                                                    <HeaderStyle Width="24" />
                                                    <ItemStyle Width="24" />
                                                </telerik:GridButtonColumn>
                                                <telerik:GridBoundColumn DataField="CategoryLongDesc" HeaderText="WebCategories"
                                                    UniqueName="WebCategories">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Primary Category" UniqueName="WebCategories">
                                                    <ItemTemplate>
                                                        <asp:RadioButton runat="server" ID="cbDefaultCategoryFlag" Checked='<%# Eval("DefaultCategoryFlag") %>'
                                                            AutoPostBack="true" OnCheckedChanged="cbDefaultCategoryFlag_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="120" />
                                                    <ItemStyle HorizontalAlign="Center" Width="120" />
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <FilterMenu EnableImageSprites="False">
                                        </FilterMenu>
                                    </telerik:RadGrid>
                                </td>
                                <td colspan="2" valign="top" align="left" style="padding-left: 62px;">
                                    <table class="labels-vertical" style="margin: 3px;">
                                        <tr>
                                            <td nowrap="nowrap" align="right">
                                                <asp:Label ID="lblDeptLabel" runat="server" class="label" Text="Dept:" />
                                            </td>
                                            <td nowrap="nowrap">
                                                <asp:Label ID="lblDeptText" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap" align="right">
                                                <asp:Label ID="lblBuyerExtLabel" runat="server" class="label" Text="Buyer/Ext:" />
                                            </td>
                                            <td nowrap="nowrap" colspan="3">
                                                <asp:Label ID="lblBuyerExtText" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap" align="right">
                                                <asp:Label ID="lblExistingWebStyleLabel" runat="server" class="label" Text="Existing Web Style:" />
                                            </td>
                                            <td nowrap="nowrap">
                                                <asp:Label ID="lblExistingWebStyleText" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap" align="right">
                                                <asp:Label ID="lblSellingLoc" runat="server" class="label" Text="Selling Location:" />
                                            </td>
                                            <td nowrap="nowrap">
                                                <asp:Label ID="lblSellingLocText" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap" align="right">
                                                <asp:Label ID="lblFabrication" runat="server" class="label" Text="Fabrication:" />
                                            </td>
                                            <td nowrap="nowrap">
                                                <asp:Label ID="lblFabricationText" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap" align="right">
                                                <asp:Label ID="lblMadeIn" runat="server" class="label" Text="Imported/USA:" />
                                            </td>
                                            <td nowrap="nowrap">
                                                <asp:Label ID="lblMadeInText" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap" align="right">
                                                <asp:Label ID="lblDropShip" runat="server" class="label" Text="Drop/Ship:" />
                                            </td>
                                            <td nowrap="nowrap">
                                                <asp:Label ID="lblDropShipText" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap" align="right">
                                                <asp:Label ID="lblSizeCategory" runat="server" class="label" Text="Size Category:" />
                                            </td>
                                            <td nowrap="nowrap" colspan="8">
                                                <telerik:RadComboBox ID="cmbSizeCategory" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                    class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="False"
                                                    Width="200" TabIndex="2" Filter="StartsWith">
                                                    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap" align="right">
                                                <asp:Label ID="lblVendorApproval" runat="server" class="label" Text="Vendor Approval:" />
                                            </td>
                                            <td nowrap="nowrap" colspan="8">
                                                <asp:RadioButtonList runat="server" ID="rblVendorApproval" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap" align="right">
                                                <asp:Label ID="lblModelCategory" runat="server" class="label" Text="Model Category:" />
                                            </td>
                                            <td nowrap="nowrap" colspan="8">
                                                <telerik:RadComboBox ID="cmbModelCategory" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                    class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="False"
                                                    Width="200" TabIndex="2" Filter="StartsWith">
                                                    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap" align="right">
                                                <asp:Label ID="lblAdditionalColorsSamples" runat="server" class="label" Text="Additional Colors/Samples:" />
                                            </td>
                                            <td nowrap="nowrap" colspan="8">
                                                <asp:RadioButtonList runat="server" ID="rblAdditionalColorsSamples" RepeatLayout="Flow"
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap" align="right">
                                                <asp:Label ID="lblTurnInUsageIndicator" runat="server" class="label" Text="Turn-In Usage Indicator:" />
                                            </td>
                                            <td nowrap="nowrap" colspan="8">
                                                <asp:CheckBoxList ID="cmbTurnInUsageIndicator" runat="server" RepeatLayout="Flow"
                                                    RepeatDirection="Horizontal">
                                                    <Items>
                                                        <asp:ListItem Value="1" Text="Print" />
                                                        <asp:ListItem Value="2" Text="Ecomm" />
                                                    </Items>
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table align="center">
                            <tr>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblPageNumber" runat="server" Text="ISN 1 of 1" class="bigLabel" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </telerik:RadPageView>
                <telerik:RadPageView ID="pvColorSizeLevel" runat="server" Height="100%">
                    <asp:Panel ID="pnlColorSizeLevel" runat="server" Visible="true" Height="100%">
                        <table id="tblFloodOptions" runat="server">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="btnResetFlood" runat="server" ToolTip="Reset" ImageUrl="~/Images/Reset.gif" />
                                    <br />
                                </td>
                                <td>
                                    <telerik:RadButton ID="btnFlood" runat="server" Text="Flood" Font-Bold="true" Width="60px"
                                        Height="25px" OnClientClicking="OnClientFloodClick" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="rtxtFloodFrndPrdDesc" ToolTip="Friendly Product Description"
                                        EmptyMessageStyle-Font-Italic="true" EmptyMessage="Friendly Product Description"
                                        runat="server" MaxLength="255" Width="180px">
                                        <ClientEvents OnBlur="ValidateText" />
                                    </telerik:RadTextBox>
                                    <bonton:ToolTipValidator ID="valFloodFrndPrdDesc" runat="server" ControlToEvaluate="rtxtFloodFrndPrdDesc"
                                        ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                        ValidationGroup="FloodUpdate" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="rtxtFloodFrndPrdFeat" ToolTip="Friendly Product Features"
                                        EmptyMessageStyle-Font-Italic="true" EmptyMessage="Friendly Product Features"
                                        runat="server" MaxLength="2000" Width="170px">
                                        <ClientEvents OnBlur="ValidateAsciiChars" />
                                    </telerik:RadTextBox>
                                    <bonton:ToolTipValidator ID="valFloodFrndPrdFeat" runat="server" ControlToEvaluate="rtxtFloodFrndPrdFeat"
                                        ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                        ValidationGroup="FloodUpdate" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="rtxtFloodFriendlyColor" ToolTip="Friendly Color" EmptyMessageStyle-Font-Italic="true"
                                        EmptyMessage="Friendly Color" runat="server" Width="80px" MaxLength="20">
                                        <ClientEvents OnBlur="ValidateText" />
                                    </telerik:RadTextBox>
                                    <bonton:ToolTipValidator ID="valFriendlyColor" runat="server" ControlToEvaluate="rtxtFloodFriendlyColor"
                                        ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                        ValidationGroup="FloodUpdate" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="rcbFloodColorFamily" runat="server" Width="90px" Height="150px"
                                        DropDownWidth="120px" EmptyMessage="Color Family" ToolTip="Color Correct" Filter="StartsWith"
                                        MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false"
                                        CheckBoxes="true">
                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                    </telerik:RadComboBox>
                                </td>
                                <%--                                <td>
                                    <telerik:RadComboBox ID="rcbFloodSampleSize" runat="server" Width="90px" Height="100px" DropDownWidth="120px"
                                        EmptyMessage="Sample Size" ToolTip="Sample Size" Filter="StartsWith" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        AllowCustomText="false">
                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                    </telerik:RadComboBox>
                                </td>
                                --%>
                                <td>
                                    <telerik:RadComboBox ID="rcbFloodClrCorrect" runat="server" Width="96px" Height="50px"
                                        EmptyMessage="Color Correct" ToolTip="Color Correct" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        AllowCustomText="false">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="" Value="" Selected="true"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Y" Value="Y"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="N" Value="N"></telerik:RadComboBoxItem>
                                        </Items>
                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="rcbFloodImgKind" AppendDataBoundItems="False" runat="server"
                                        Width="86px" Height="100px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        AllowCustomText="false" EmptyMessage="Image Kind" ToolTip="Image Kind" OnClientSelectedIndexChanged="OnChangeImageKindFlood">
                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="rtxtFloodGroupNum" runat="server" Width="90px" MaxLength="5"
                                        MinValue="0" MaxValue="99999" EmptyMessage="Image Group #" ToolTip="Image Group #">
                                        <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                    </telerik:RadNumericTextBox>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="rcbFloodImgType" AppendDataBoundItems="False" runat="server"
                                        Width="90px" Height="60px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        AllowCustomText="false" EmptyMessage="On/Off Figure" ToolTip="On/Off Figure"
                                        OnClientSelectedIndexChanged="OnChangeOnOff">
                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="rcbFloodAltView" AppendDataBoundItems="False" runat="server"
                                        Width="76px" DropDownWidth="120px" Height="100px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        AllowCustomText="false" EmptyMessage="Alt. View" ToolTip="Alt. View">
                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="rcbFloodSampleStr" AppendDataBoundItems="False" runat="server"
                                        Width="100px" DropDownWidth="150px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                        AllowCustomText="false" EmptyMessage="Sample Store #" ToolTip="Sample Store #">
                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="rtxtFloodMerchNotes" runat="server" Width="140px" MaxLength="80"
                                        EmptyMessage="Merchant Notes" ToolTip="Merchant Notes" EmptyMessageStyle-Font-Italic="true">
                                        <ClientEvents OnBlur="ValidateAsciiChars" />
                                    </telerik:RadTextBox>
                                    <bonton:ToolTipValidator ID="valFloodMerchNotes" runat="server" ControlToEvaluate="rtxtFloodMerchNotes"
                                        ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                        ValidationGroup="FloodUpdate" />
                                </td>
                            </tr>
                        </table>
                        <telerik:RadGrid ID="grdColorSizeLevel" runat="server" AllowPaging="True" ShowFooter="False"
                            Visible="false" AllowMultiRowSelection="True" AllowMultiRowEdit="true" SkinID="CenteredWithScroll"
                            AutoGenerateColumns="false" GroupingEnabled="true" CssClass="AddBorders" Height="96%">
                            <ClientSettings EnableRowHoverStyle="false">
                                <Resizing AllowColumnResize="true" />
                                <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                <ClientEvents OnCommand="OnEditClick" />
                            </ClientSettings>
                            <AlternatingItemStyle BackColor="White" />
                            <MasterTableView DataKeyNames="DeptID,ISN,VendorStyleNum,TurnInMerchID,AdminMerchNum,UPC,VendorName,IsnDesc,RemoveMerchFlag,IsReserve,Status,VendorColorCode,LabelName,SampleDescription,SampleSize,SampleMerchId" ClientDataKeyNames="DeptID,ISN,VendorStyleNum,TurnInMerchID,AdminMerchNum,UPC,VendorName,IsnDesc,RemoveMerchFlag,IsReserve,Status,VendorColorCode,LabelName,SampleDescription,SampleSize,SampleMerchId"
                                GroupsDefaultExpanded="true" EditMode="InPlace" EnableHeaderContextMenu="true">
                                <EditFormSettings>
                                    <EditColumn UniqueName="EditCommandColumn1">
                                    </EditColumn>
                                </EditFormSettings>
                                <CommandItemTemplate>
                                    test
                                </CommandItemTemplate>
                                <NoRecordsTemplate>
                                    no records retrieved
                                </NoRecordsTemplate>
                                <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                <%--<GroupHeaderTemplate>
                                                <asp:Label runat="server" ID="lblDept" Text='<%# "Department: " + Cstr(Eval("DeptID")) %>'>
                                                </asp:Label>
                                            </GroupHeaderTemplate>--%>
                                <GroupByExpressions>
                                    <telerik:GridGroupByExpression>
                                        <GroupByFields>
                                            <telerik:GridGroupByField FieldName="Sequence" SortOrder="Ascending"></telerik:GridGroupByField>
                                            <telerik:GridGroupByField FieldName="DeptID" SortOrder="Ascending"></telerik:GridGroupByField>
                                        </GroupByFields>
                                        <SelectFields>
                                            <telerik:GridGroupByField FieldAlias="Dept" FieldName="DeptIdDesc"></telerik:GridGroupByField>
                                        </SelectFields>
                                    </telerik:GridGroupByExpression>
                                    <telerik:GridGroupByExpression>
                                        <GroupByFields>
                                            <telerik:GridGroupByField FieldName="ISN" SortOrder="Ascending"></telerik:GridGroupByField>
                                        </GroupByFields>
                                        <SelectFields>
                                            <telerik:GridGroupByField HeaderText="-ISN" FieldAlias="ISN" FieldName="ISN"></telerik:GridGroupByField>
                                            <telerik:GridGroupByField HeaderText="Vendor Name" FieldAlias="VendorName" FieldName="VendorName"></telerik:GridGroupByField>
                                            <telerik:GridGroupByField HeaderText="Vendor Style" FieldAlias="VendorStyleNum" FieldName="VendorStyleNum"></telerik:GridGroupByField>
                                            <telerik:GridGroupByField HeaderText=" " FieldAlias="IsnDesc" FieldName="IsnDesc"
                                                HeaderValueSeparator=""></telerik:GridGroupByField>
                                        </SelectFields>
                                    </telerik:GridGroupByExpression>
                                </GroupByExpressions>
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
                                    <telerik:GridBoundColumn DataField="IsReserve" HeaderText="Rsv?" UniqueName="Reserve"
                                        ReadOnly="true">
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="36px" />
                                        <ItemStyle HorizontalAlign="Center" Width="36px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn HeaderText="Turned In" UniqueName="TurnedIn" ReadOnly="true">
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="50px" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblIsTurnedIn" ToolTip='<%# Eval("TurnedInAdNos")%>'></asp:Label>
                                            <%# Eval("IsTurnedIn") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Rush Sample" UniqueName="HotItem">
                                        <HeaderStyle HorizontalAlign="Center" Width="52px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemTemplate>
                                            <%# Eval("IsHotItem") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox ID="rcbHotItem" runat="server" Width="36px" Height="60px" Text='<%# Eval("IsHotItem")%>'
                                                MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Y" Value="Y"></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Text="N" Value="N"></telerik:RadComboBoxItem>
                                                </Items>
                                                <CollapseAnimation Duration="200" Type="OutQuint" />
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Friendly Product Desc" UniqueName="FriendlyProdDesc">
                                        <HeaderStyle HorizontalAlign="Center" Width="120px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="120px" />
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltrFriendlyProdDesc" Text='<%# Eval("FriendlyProdDesc")%>'></asp:Literal>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="rtxtFriendlyProdDesc" runat="server" Text='<%# Eval("FriendlyProdDesc")%>'
                                                Width="120px" MaxLength="255">
                                                <ClientEvents OnBlur="ValidateText" />
                                            </telerik:RadTextBox>
                                            <bonton:ToolTipValidator ID="valFriendlyProdDesc" runat="server" ControlToEvaluate="rtxtFriendlyProdDesc"
                                                ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                                ValidationGroup="Update" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Friendly Product Features" UniqueName="FriendlyProdFeatures">
                                        <HeaderStyle HorizontalAlign="Center" Width="120px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Left" Width="120px" />
                                        <ItemTemplate>
                                            <%# Regex.Replace(Eval("FriendlyProdFeatures"), "<.*?>", "").Replace(vbCrLf, "<br />").Trim()%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="rtxtFriendlyProdFeatures" runat="server" Text='<%#Regex.Replace(Regex.Replace(Eval("FriendlyProdFeatures"), "<.*?>", ""),"&amp;.*;", " ").Trim()%>'
                                                Width="120px" Height="120px" TextMode="MultiLine" MaxLength="2000">
                                                <ClientEvents OnBlur="ValidateAsciiChars" />
                                            </telerik:RadTextBox>
                                            <bonton:ToolTipValidator ID="valFriendlyProdFeatures" runat="server" ControlToEvaluate="rtxtFriendlyProdFeatures"
                                                ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                                ValidationGroup="Update" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Vendor Color" UniqueName="Color">
                                        <HeaderStyle Width="60" HorizontalAlign="Center" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# Eval("VendorColorCode")%>
                                            -
                                            <%# Eval("Color") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Friendly Color" UniqueName="FriendlyColor">
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
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
                                                ValidationGroup="Update" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Color Family" UniqueName="ColorFamily">
                                        <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblColorFamily" ToolTip='<%# Eval("ColorFamily")%>' Text='<%# Eval("ColorFamily")%>' />
                                            <asp:Image ID="imgCFError" runat="server" ImageUrl="~/Images/Error.png" ToolTip="One or more Color Family could not be flooded."
                                                Visible="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="hfColorFamily" runat="server" Value='<%# Eval("ColorFamily") %>' />
                                            <asp:Literal runat="server" ID="ltrError" Visible="false"></asp:Literal>
                                            <telerik:RadComboBox ID="rcbColorFamily" CheckBoxes="true" runat="server" Width="60px"
                                                Filter="StartsWith" DropDownWidth="120px">
                                                <CollapseAnimation Duration="200" Type="OutQuint" />
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Sample" UniqueName="Sample">
                                        <HeaderStyle HorizontalAlign="Center" Width="120px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="120px" />
                                        <ItemTemplate>
                                            <%# Eval("SampleDescription") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <table>
                                                <tr>
                                                    <td style="border: none">
                                                        <telerik:RadTextBox ID="radtxtSample" runat="server" Width="110px" MaxLength="255" Text='<%# Eval("SampleDescription") %>'>
                                                            <ClientEvents OnFocus="PickSample" />
                                                        </telerik:RadTextBox>
                                                    </td>
                                                    <td style="border: none">
                                                        <telerik:RadButton ID="trbRemoveSample" runat="server" OnClientClicked="RemoveSample" AutoPostBack="false"
                                                            Height="16px" Width="16px" ToolTip="Clear the Sample" Text="Clear Sample" Enabled='<%# Eval("AdminMerchNum") %>'>
                                                            <Image EnableImageButton="true" ImageUrl="~/Images/Delete.gif" />
                                                        </telerik:RadButton>
                                                    </td>
                                                </tr>
                                            </table>
                                            <telerik:RadTextBox ID="hfMerchId" runat="server" Width="1" Text='<%# Eval("SampleMerchId") %>' Display="false" />
                                            <telerik:RadTextBox ID="hfSampleSize" runat="server" Width="1" Text='<%# Eval("SampleSize") %>' Display="false" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Color Correct" UniqueName="ColorCorrect">
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemTemplate>
                                            <%# Eval("ColorCorrect")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox ID="rcbColorCorrect" runat="server" Width="36px" Height="60px"
                                                Text='<%# Eval("ColorCorrect")%>' MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                AllowCustomText="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="" Value=""></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Text="Y" Value="Y"></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Text="N" Value="N"></telerik:RadComboBoxItem>
                                                </Items>
                                                <CollapseAnimation Duration="200" Type="OutQuint" />
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Image Kind" UniqueName="ImageKind">
                                        <HeaderStyle HorizontalAlign="Center" Width="62px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="62px" />
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltrlImageKind" Text='<%# Eval("ImageKind") %>'></asp:Literal>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="hfImageKind" runat="server" Value='<%# Eval("ImageKind") %>' />
                                            <telerik:RadComboBox ID="rcbImageKind" runat="server" Width="62px" DropDownWidth="120"
                                                Height="80px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false">
                                                <CollapseAnimation Duration="200" Type="OutQuint" />
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
<%--                                    <telerik:GridHyperLinkColumn DataNavigateUrlFields="ImageURL" DataTextField="VTImageAvailable" HeaderText="Image Available" UniqueName="VTImageAvailable" Target="_blank" NavigateUrl="ImageURL">
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </telerik:GridHyperLinkColumn>--%>
                                    <telerik:GridTemplateColumn HeaderText="P/U ImageID" UniqueName="PUImageID">
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemTemplate>
                                            <%# Eval("PuImageID")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadNumericTextBox ID="rtxtPUImgID" runat="server" Value='<%# CStr(Eval("PuImageID"))%>'
                                                Width="50px" MaxLength="6" MinValue="0" MaxValue="999999">
                                                <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                            </telerik:RadNumericTextBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Route from Ad" UniqueName="RouteFromAd">
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltrlRouteFromAd" Text='<%# Eval("RouteFromAD") %>'></asp:Literal>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="hfRouteFromAd" runat="server" Value='<%# Eval("RouteFromAD") %>' />
                                            <telerik:RadComboBox ID="rcbRouteFromAd" runat="server" Width="50px" DropDownWidth="70px"
                                                MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false">
                                                <CollapseAnimation Duration="200" Type="OutQuint" />
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Image Group #" UniqueName="GroupNum">
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemTemplate>
                                            <%# Eval("GroupNum")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadNumericTextBox ID="rtxtGroupNum" runat="server" Value='<%# Cstr(Eval("GroupNum"))%>'
                                                Width="40px" MaxLength="5" MinValue="0" MaxValue="99999">
                                                <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                            </telerik:RadNumericTextBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Feature/ Render/ Swatch" UniqueName="FeatureRenderSwatch">
                                        <HeaderStyle HorizontalAlign="Center" Width="70px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        <ItemTemplate>
                                            <%# Eval("FeatureRenderSwatch") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="hfFeatureRenderSwatch" runat="server" Value='<%# Eval("FeatureRenderSwatch") %>' />
                                            <telerik:RadComboBox ID="rcbFeatureRenderSwatch" runat="server" Width="70px" Height="100px"
                                                DropDownWidth="100px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                                AllowCustomText="false">
                                                <CollapseAnimation Duration="200" Type="OutQuint" />
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="On/ off Figure" UniqueName="ImageType">
                                        <HeaderStyle HorizontalAlign="Center" Width="40px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                        <ItemTemplate>
                                            <%# Eval("ImageType") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="hfOnOff" runat="server" Value='<%# Eval("ImageType") %>' />
                                            <telerik:RadComboBox ID="rcbOnOff" runat="server" Width="50px" Height="100px" MarkFirstMatch="true"
                                                OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                                OnClientSelectedIndexChanged="OnChangeOnOff">
                                                <CollapseAnimation Duration="200" Type="OutQuint" />
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Alt. View" UniqueName="AlternateView">
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltrlAltView" Text='<%# Eval("AltView") %>'></asp:Literal>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="hfAlternateView" runat="server" Value='<%# Eval("AltView") %>' />
                                            <telerik:RadComboBox ID="rcbAlternateView" runat="server" Width="45px" DropDownWidth="120px"
                                                Height="100px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false">
                                                <CollapseAnimation Duration="200" Type="OutQuint" />
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Sample Store #" UniqueName="SampleStoreNum">
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemTemplate>
                                            <%# Eval("SampleStore")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="hfSampleStore" runat="server" Value='<%# Eval("SampleStoreNum") %>' />
                                            <telerik:RadComboBox ID="rcbSampleStore" runat="server" Width="50px" DropDownWidth="150px"
                                                MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false">
                                                <CollapseAnimation Duration="200" Type="OutQuint" />
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Merchant Notes" UniqueName="MerchantNotes">
                                        <HeaderStyle HorizontalAlign="Center" Width="150px" Font-Bold="True" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                        <ItemTemplate>
                                            <%# Eval("MerchantNotes") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="rtxtMerchantNotes" runat="server" Text='<%# Eval("MerchantNotes") %>'
                                                Width="140px" Height="50px" TextMode="MultiLine" MaxLength="80">
                                                <ClientEvents OnBlur="ValidateAsciiChars" />
                                            </telerik:RadTextBox>
                                            <bonton:ToolTipValidator ID="valMerchantNotes" runat="server" ControlToEvaluate="rtxtMerchantNotes"
                                                ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                                ValidationGroup="Update" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </asp:Panel>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false">
                <Windows>
                    <telerik:RadWindow ID="SamplePicker" runat="server" NavigateUrl="SamplePicker.aspx" Skin="Vista" Behaviors="Close, Move"
                        Modal="True" MinWidth="1000px" MinHeight="600px" OnClientClose="clientClose" OnClientShow="clientShow" ReloadOnShow="true">
                    </telerik:RadWindow>
                    <telerik:RadWindow ID="PreviewWindow" runat="server" VisibleStatusbar="false" VisibleTitlebar="true" Behaviors="Close, Move" Modal="true"
                        AutoSize="false" Width="624px" Height="680px" KeepInScreenBounds="true" Skin="Vista">
                        <Shortcuts>
                            <telerik:WindowShortcut CommandName="Close" Shortcut="Esc" />
                        </Shortcuts>
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td align="center">
                                        <img src="javascript:void(0);" alt="Image holder" id="imageHolder" width="600px" height="600px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <telerik:RadButton ID="btnImageFlipper" ToggleType="CustomToggle" ButtonType="StandardButton" runat="server"
                                            Width="24px" Height="24px" AutoPostBack="false" OnClientClicked="flipImage">
                                            <ToggleStates>
                                                <telerik:RadButtonToggleState ImageUrl="../../Images/arrowright.gif" Text="Next" Selected="true" />
                                                <telerik:RadButtonToggleState ImageUrl="../../Images/arrowleft.gif" Text="Prev" />
                                            </ToggleStates>
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
        </telerik:RadPane>
    </telerik:RadSplitter>
<%--    <asp:Button ID="fakeImageButton" runat="server" Style="display: none" Visible="true" />
    <asp:Panel ID="pnlMessage" runat="server" Height="400px" Width="400px" Style="display: none; background-color: AntiqueWhite; text-align: center;">
        <table border="0">
            <tr>
                <td style="padding-left: 25px; padding-right: 25px; padding-top: 25px;">
                    <asp:Image ID="vtImage" runat="server" ImageUrl="" Height="350px" Width="350px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="lnkOK" runat="server" Text="OK" class="label" OnClientClick="HideImageModal();return false;" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <ajaxtoolkit:ModalPopupExtender ID="imagePopup" runat="server" PopupControlID="pnlMessage"
        TargetControlID="fakeImageButton" BackgroundCssClass="modalProgressGreyBackground"
        DropShadow="false">
    </ajaxtoolkit:ModalPopupExtender>--%>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            (function (global, undefined) {
                var demo = {};
                var $ = $telerik.$;

                function sizePreviewWindow() {
                    demo.previewWin.center();
                }

                function openWin(sampleImages) {
                    mySampleImages = sampleImages;

                    if (sampleImages.secondaryImageUrl.length > 0) {
                        demo.btnFlipper.set_selectedToggleStateIndex(0);
                        demo.btnFlipper.set_visible(true);
                    } else {
                        demo.btnFlipper.set_visible(false);
                    }

                    demo.previewWin.show();

                    demo.imgHolder.src = sampleImages.primaryImageUrl;
                    window.focus();
                }

                var mySampleImages = {};
                function flipImage(sender, args) {
                    if (demo.imgHolder.src == mySampleImages.primaryImageUrl) {
                        demo.imgHolder.src = mySampleImages.secondaryImageUrl;
                    } else {
                        demo.imgHolder.src = mySampleImages.primaryImageUrl;
                    }
                }

                function clickFlip(sender, args) {
                    if (demo.btnFlipper.get_visible() == true) {
                        demo.btnFlipper.set_checked(!demo.btnFlipper.get_checked());
                        demo.btnFlipper.click();
                    }
                }

                global.$autoSizeDemo = demo;
                global.sizePreviewWindow = sizePreviewWindow;
                global.openWin = openWin;
                global.flipImage = flipImage;
                global.clickFlip = clickFlip;
            })(window);

            Sys.Application.add_load(function () {
                $autoSizeDemo.previewWin = $find("<%= PreviewWindow.ClientID %>");
                //get a reference to the image tag in the preview window
                $autoSizeDemo.imgHolder = $get("imageHolder");
                $autoSizeDemo.btnFlipper = $find("<%= btnImageFlipper.ClientID %>");
                //add onload event for the image in the preview window
                $addHandler($autoSizeDemo.imgHolder, "load", sizePreviewWindow);
                $addHandler($autoSizeDemo.imgHolder, "click", clickFlip);
            })
        </script>
        <script type="text/javascript">

            // Methods for the RadWindow dialog
            function RemoveSample(sender, args) {
                var grid = $find("<%= grdColorSizeLevel.ClientID %>");
                var MasterTable = grid.get_masterTableView();
                var gridDataItems = MasterTable.get_editItems();

                var myRow = null;
                for (var i = 0; i < gridDataItems.length, myRow == null; i++) {
                    if (gridDataItems[i].findControl(sender.get_element().id) != null) {
                        myRow = gridDataItems[i];

                        myRow.findControl("radtxtSample").set_value("");
                        myRow.findControl("hfMerchId").set_value("0");
                        myRow.findControl("hfSampleSize").set_value("0");
                        myRow.findControl("trbRemoveSample").set_visible(false);
                    }
                }
            }

            var samplePickinRow = null;

            function PickSample(sender, eventArgs) {
                var grid = $find("<%= grdColorSizeLevel.ClientID %>"); // get grid
                var MasterTable = grid.get_masterTableView(); // get mastertableview
                var gridDataItems = MasterTable.get_editItems();
                samplePickinRow = null;
                for (var i = 0; i < gridDataItems.length -1, samplePickinRow == null; i++) {
                    if (gridDataItems[i].findControl(sender.get_element().id) != null) {
                        samplePickinRow = gridDataItems[i];
                    }
                }

                var isn = samplePickinRow.getDataKeyValue("ISN"); // get ISN value for selected edititem
                var colorId = samplePickinRow.getDataKeyValue("VendorColorCode"); // get color code value for selected edititem
                var adminMerchNum = samplePickinRow.getDataKeyValue("AdminMerchNum"); // get SampleMerchId value for selected edititem
                window.focus(); // move the focus away otherwise the window opens again once the webcategories are changed.
                var oWnd = window.radopen("SamplePicker.aspx?ISN=" + isn + "&ColorId=" + colorId + "&MerchId=" + adminMerchNum, "SamplePicker", 1000, 600);
                oWnd.center();
            }

            function clientShow(sender, eventArgs) {
                var hfMerchId = samplePickinRow.findControl("hfMerchId");
                sender.argument = hfMerchId.get_value();
            }

            
            function clientClose(sender, args) {
                if (args.get_argument() != null) {
                    var selectedMerchSample = args.get_argument();

                    samplePickinRow.findControl("radtxtSample").set_value(selectedMerchSample.colorCode + "-" + selectedMerchSample.colorDesc + ", " + selectedMerchSample.sizeDesc);
                    samplePickinRow.findControl("hfMerchId").set_value(selectedMerchSample.sampleMerchId);
                    samplePickinRow.findControl("hfSampleSize").set_value(selectedMerchSample.sampleSize);
                    samplePickinRow.findControl("trbRemoveSample").set_visible(true);
                }

                samplePickinRow = null;
            }

            // page global
            var fireOnOff = true;

            function OnChangeOnOff(sender, eventArgs) {
                if (fireOnOff == true) { 
                    var selectedText = sender.get_text();

                    if (selectedText == 'OFF') {
                        alert('Based on your selection of OFF for "On/Off Figure", Model Category will be blank.');
                    }
                }
                else {
                    fireOnOff == true
                }
            }

            function OnChangeImageKindFlood(sender, eventArgs) {
                var selectedValue = sender.get_value();
                var rcbOnOffFlood = $find("<%=rcbFloodImgType.ClientID%>");

                if (selectedValue == "CR8" || selectedValue == "DUP" || selectedValue == "VND" || selectedValue == "NOMER" || selectedValue == "PU") {
                    rcbOnOffFlood.set_text("OFF");
                    return alert('Based on your selection of ' + selectedValue + ' for "Image Kind", we have defaulted "On/Off Figure" to OFF and Model Category will be blank.');
                }
            }

            function OnChangeImageKind(sender, eventArgs, rowId) {
                var selectedValue = sender.get_value();

                var grid = $find("<%=grdColorSizeLevel.ClientID%>"); // get grid
                var MasterTable = grid.get_masterTableView(); // get mastertableview
                var gridDataItem = MasterTable.get_dataItems()[rowId]; // get edititem
                var rcbFeatureRenderSwatch = gridDataItem.findControl("rcbFeatureRenderSwatch");
                var rcbOnOff = gridDataItem.findControl("rcbOnOff");
                var rcbSampleSize = gridDataItem.findControl("rcbSampleSize");
                var onOff = rcbOnOff.get_text();

                //5-20-2014 3:05:25 PM	ISSUE 459 - Vendor Images should always be OFF 
                if (selectedValue == "VND"){
                    fireOnOff = false;
                    rcbOnOff.findItemByText("OFF").select();
                    rcbOnOff.disable();
                } else {
                    rcbOnOff.enable();
                }

                if (selectedValue == "CR8" || selectedValue == "DUP" || selectedValue == "VND" || selectedValue == "NOMER" || selectedValue == "PU") {
                    //Only change the selection to OFF and alert the user if OFF isn't already selected.
                    if (onOff != "OFF") {
                        alert('Based on your selection of ' + selectedValue + ' for "Image Kind", we have defaulted "On/Off Figure" to OFF and Model Category will be blank.');
                        fireOnOff = false;
                        rcbOnOff.findItemByText("OFF").select();
                    }
                }
                else{
                    if(rcbFeatureRenderSwatch.get_value() != "SWTCH" && rcbFeatureRenderSwatch.get_value() != "SWTBOX" && selectedValue == "NEW") {
                        var pageNumber = document.getElementById('<%=lblPageNumberText.ClientID%>').innerText.split(" - ")[0];

                        //Change the selection to ON if Image Kind is New and FRS is NOT Swatch or Static Swatch and if page number is 1-4 and ON isn't already selected.
                        if ((onOff != "ON") && (pageNumber < 5)) {
                            rcbOnOff.findItemByText("ON").select();
                            alert('Based on your selection of NEW for "Image Kind" and because page number is ' + pageNumber + ', we have defaulted "On/Off Figure" to ON.');
                        }
                        //Change the selection to OFF if Image Kind is New and FRS is NOT Swatch or Static Swatch and if page number is 5+ and OFF isn't already selected.
                        if ((onOff != "OFF") && (pageNumber >= 5)) {
                            rcbOnOff.findItemByText("OFF").select();
                            alert('Based on your selection of NEW for "Image Kind" and because page number is ' + pageNumber + ', we have defaulted "On/Off Figure" to OFF.');
                        }
                    }
                }
            }

            function OnChangeFRS(sender, eventArgs, rowId) {
                var selectedValue = sender.get_value();

                var grid = $find("<%=grdColorSizeLevel.ClientID%>"); // get grid
                var MasterTable = grid.get_masterTableView(); // get mastertableview
                var gridDataItem = MasterTable.get_dataItems()[rowId]; // get edititem
                var rcbImageKind = gridDataItem.findControl("rcbImageKind");
                var rcbOnOff = gridDataItem.findControl("rcbOnOff");
                var onOff = rcbOnOff.get_text();
                rcbOnOff.enable();
                if (rcbImageKind.get_value() != "VND")
                {
                    if (selectedValue == "SWTCH" || selectedValue == "SWTBOX") {
                        if (onOff != "OFF") {
                            alert('Based on your selection of ' + selectedValue + ' for "Feature/Render/Swatch", we have defaulted "On/Off Figure" to OFF and Model Category will be blank.');
                            fireOnOff = false;
                            rcbOnOff.findItemByText("OFF").select();
                        }
                    }
                    else{
                        if (onOff != "ON" && rcbImageKind != "CR8" && rcbImageKind != "DUP" && rcbImageKind != "VND" && rcbImageKind != "NOMER" && rcbImageKind != "PU") {
                            rcbOnOff.findItemByText("ON").select();
                            return alert('Based on your selection of ' + selectedValue + ' for "Feature/Render/Swatch", we have defaulted "On/Off Figure" to ON.');
                        }
                    }
                }
                else
                {
                    if (onOff != "OFF") {
                        rcbOnOff.findItemByText("OFF").select();
                        return alert('Based on your selection of ' + rcbImageKind.get_value() + ' for "Image Kind", we have defaulted "On/Off Figure" to OFF.');
                    }   
                    rcbOnOff.disable();
                }
            }

            function OnClientFloodClick(sender, eventArgs) {
                if (CheckForRowsInEditMode(0)) {
                    eventArgs.set_cancel(true);
                } else {
                    eventArgs.set_cancel(!confirm('Are you sure? This action will automatically save data for every row selected.'));
                }
            }

            function ValidateFriendlyColor(sender, eventArgs) {
                var str = sender.get_value();
                var isValid = true;
                var replaceTitleCase = str.replace(/\w\S*/g, function(txt){return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();});

                if (str.match(/[^a-zA-Z ]+/)) {
                    alert("Invalid character(s) in Friendly Color.");
                    isValid = false;
                }
                else if (str != replaceTitleCase)
                {
                    sender.set_value(replaceTitleCase);
                    //alert("The Friendly Color you entered has been changed to Title Case.");
                } 
                return isValid;
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

            function OnEditClick(sender, eventArgs) {
                var command = eventArgs.get_commandName();
                if (command == "Edit") {
                    if (CheckForRowsInEditMode(0)) {
                        eventArgs.set_cancel(true);
                    }
                }
                if (command == "Update") {
                    var radGrid = $find("<%=grdColorSizeLevel.ClientID %>");
                    if (radGrid) {
                        var MasterTable = radGrid.get_masterTableView();
                        var editItems = MasterTable.get_editItems();
                        if (editItems.length > 1) {
                            eventArgs.set_cancel(!confirm('Are you sure you want to save? Changes made to other rows will be lost. If you have made changes to more than one row, use the Save All button at the top.'));
                        }
                    }
                }
                if (command == "CancelUpdate") {
                    if (CheckForRowsInEditMode(1)) {
                        eventArgs.set_cancel(true);
                    }
                }
                if (command == "Delete") {
                    if (CheckForRowsInEditMode(0)) {
                        eventArgs.set_cancel(true);
                    } else {
                        eventArgs.set_cancel(!confirm('Delete/Activate this record?'));
                    }
                }
                if (command == "Page" || command == "PageSize") {
                    if (CheckForRowsInEditMode(0)) {
                        eventArgs.set_cancel(true);
                    }
                }
            }

            function CheckISNTabContentChange() {
                var ISNTabOldValues = document.getElementById("<%=hdnISNTabChanges.ClientID %>").value
                document.getElementById("<%=hdnISNTabSaveFlg.ClientID %>").value = 'N';

                var ISNTabNewValues = '';
                var cmbLabelValue = document.getElementById("ctl00_ContentArea_cmbLabel_Input").value;
                var cmbSizeCategoryValue = document.getElementById("ctl00_ContentArea_cmbSizeCategory_Input").value;
                var cmbModelCategoryValue = document.getElementById("ctl00_ContentArea_cmbModelCategory_Input").value;

                var rblVndAppValue;

                if (document.getElementById("ctl00_ContentArea_rblVendorApproval_0").checked) {
                    rblVndAppValue = document.getElementById("ctl00_ContentArea_rblVendorApproval_0").value;
                } else {
                    rblVndAppValue = document.getElementById("ctl00_ContentArea_rblVendorApproval_1").value;
                }

                var rblAdClrValue;

                if (document.getElementById("ctl00_ContentArea_rblAdditionalColorsSamples_0").checked) {
                    rblAdClrValue = document.getElementById("ctl00_ContentArea_rblAdditionalColorsSamples_0").value;
                } else {
                    rblAdClrValue = document.getElementById("ctl00_ContentArea_rblAdditionalColorsSamples_1").value;
                }

                var cmbTurnInUsage0 = '';
                var cmbTurnInUsage1 = '';
                if (document.getElementById("ctl00_ContentArea_cmbTurnInUsageIndicator_0").checked) {
                    cmbTurnInUsage0 = '1';
                }

                if (document.getElementById("ctl00_ContentArea_cmbTurnInUsageIndicator_1").checked) {
                    cmbTurnInUsage1 = '2';
                }

                ISNTabNewValues = cmbLabelValue + cmbSizeCategoryValue + rblVndAppValue + cmbModelCategoryValue + rblAdClrValue + cmbTurnInUsage0 + cmbTurnInUsage1;

                if (ISNTabNewValues != ISNTabOldValues) {
                    if (confirm('Data not saved. Click Ok to save and proceed or Cancel to ignore the changes and proceed.')) {
                        document.getElementById("<%=hdnISNTabSaveFlg.ClientID %>").value = 'Y';
                    }
                }
            }

            function OnClientTabOut(sender, eventArgs) {
                var tab = eventArgs.get_tab();
                if (tab.get_text() == "ISN Level") {
                    CheckISNTabContentChange();
                }
                if (tab.get_text() == "Color/Size Level") {
                    if (CheckForRowsInEditMode(0)) {
                        eventArgs.set_cancel(true);
                    }
                }
            }

<%--            function HideImageModal() {
                var imagePopup = $find('<%= imagePopup.ClientID %>');
                imagePopup.hide();
            }
            function DisplayVTImage(imageURL)
            {
                var vtImage = $get('<%= vtImage.ClientID %>')
                vtImage.src = imageURL; 
                var imagePopup = $find('<%= imagePopup.ClientID %>');
                var imageButton = $get('<%= fakeImageButton.ClientID()%>');
                imageButton.click();
            }--%>

            function OnClientTabClicking(sender, eventArgs) {
                var tab = eventArgs.get_tab();
                if ((tab.get_text() == "ISN Level" || tab.get_text() == "Color/Size Level") && <%= CurrentBatch.ColorSizeItems.Count() %> == 0) {
                    var radGrid = $find("<%= grdResultList.ClientID %>");
                    if (radGrid) {
                        var MasterTable = radGrid.get_masterTableView();
                        var dataItems = MasterTable.get_dataItems();
                        var selectedRows = 0;
                        for (var i = 0; i < dataItems.length; i++) {
                            var gridItemElement = dataItems[i].findElement("chkISNLevel");
                            if (gridItemElement.checked) {
                                selectedRows++;
                                break;
                            }
                        }

                        if (selectedRows == 0) {
                            for (var i = 0; i < dataItems.length; i++) {
                                if (dataItems[i].get_nestedViews().length > 0) {
                                    var nestedView = dataItems[i].get_nestedViews()[0];
                                    var nestedDataItems = nestedView.get_dataItems();
                                    for (var j = 0; j < nestedDataItems.length; j++) {
                                        var gridNestedItemElement = nestedDataItems[j].findElement("chkColorLevel");
                                        if (gridNestedItemElement.checked) {
                                            selectedRows++;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (selectedRows == 0) {
                            alert("Please select at least one record from Results Tab.");
                            eventArgs.set_cancel(true);
                        }
                    } else {
                        alert("Please navigate to Results tab and select at least one record.");
                        eventArgs.set_cancel(true);
                    }
                }
            }

            function CheckForRowsInEditMode(expectedRowCount) {
                var grdColorSizeLevel = $find("<%=grdColorSizeLevel.ClientID %>");
                if (grdColorSizeLevel) {
                    var MasterTable = grdColorSizeLevel.get_masterTableView();
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

            function OnClientRadToolBarClick(sender, eventArgs) {
                var button = eventArgs.get_item();
                if (button.get_text() != "Save All" && button.get_text() != "Cancel All") {
                    if (CheckForRowsInEditMode(0)) {
                        eventArgs.set_cancel(true);
                    } else {
                        if (button.get_text() == "Delete") {
                            eventArgs.set_cancel(!confirm('Confirm Delete?'));
                        }

                        if (button.get_text() == "Submit") {
                            eventArgs.set_cancel(!confirm('Are you sure you want to submit? The batch cannot be edited once it has been submitted for a meeting.'));
                        }

                        if (button.get_text() == "Back") {
                            var radTabStrip = $find("<%=rtsAPAdjustment.ClientID %>");

                            if (radTabStrip.get_selectedTab().get_text() == "ISN Level") {
                                CheckISNTabContentChange();
                            }
                        }

                        if (button.get_text() == "Export") {
                            ShowModalMessage();
                            eventArgs.set_cancel(true);
                        }

                        if (button.get_text() == "Sort") {
                            ShowModalOrderISN();
                            eventArgs.set_cancel(true);
                        }

                        if (button.get_text() == "Print Labels") {
                            //                    var jqResponse = $.post("/Webforms/LabelTestCode/testhandler.ashx",
                            //                        {
                            //                            MerchID: "66"
                            //                        },
                            //                        function (data, status) {
                            //                              print("DYMO LabelWriter 450", data);
                            //                              alert("Data: " + data + "\nStatus: " + status);
                            //                        });

                            var lblAdNbr = document.getElementById("<%=lblAdNoText.ClientID %>");
                            var lblAdNbrDesc = document.getElementById("<%=lblAdNoDescText.ClientID %>");
                            var lblPgNbr = document.getElementById("<%=lblPageNumberText.ClientID %>");
                            var lblSysPgNbr = document.getElementById("<%=hdnSysPgNbr.ClientID %>");
                            var lblBatchID = document.getElementById("<%=lblBatchIdText.ClientID %>");

                            var radGrid = $find("<%=grdColorSizeLevel.ClientID %>");
                            var MasterTable = radGrid.get_masterTableView();
                            var selectedRows = MasterTable.get_selectedItems();
                            var areRecordsSaved = 'Y';

                            //                    for (var i = 0; i < selectedRows.length; i++) {
                            //                        var dataItem = selectedRows[i];
                            //                        var AdminMerchID = dataItem.getDataKeyValue("AdminMerchNum");

                            //                        if (AdminMerchID == '0') {
                            //                            areRecordsSaved = 'N';
                            //                        }
                            //                    }

                            //                    if (areRecordsSaved == 'N') {
                            //                        alert("Print Label not allowed. At least one record is pending Save."); 
                            //                        eventArgs.set_cancel(true);
                            //                        return false;
                            //                    }

                            if (selectedRows.length > 0) {
                                for (var i = 0; i < selectedRows.length; i++) {
                                    var row = selectedRows[i];
                                    var AdminMerchNum = row.getDataKeyValue("AdminMerchNum");

                                    if (AdminMerchNum != '0') {
                                        var cellMerchID = row.getDataKeyValue("AdminMerchNum");
                                        var cellVndNme = row.getDataKeyValue("VendorName");
                                        var cellLblNme = row.getDataKeyValue("LabelName");
                                        var cellFPD = cellLblNme + " - " + MasterTable.getCellByColumnUniqueName(row, "FriendlyProdDesc").innerHTML;
                                        var cellSiz = MasterTable.getCellByColumnUniqueName(row, "SampleSize").innerHTML;
                                        var cellOnOff = MasterTable.getCellByColumnUniqueName(row, "ImageType").innerHTML;
                                        var cellDept = row.getDataKeyValue("DeptID");
                                        var cellClr = MasterTable.getCellByColumnUniqueName(row, "FriendlyColor").innerHTML;
                                        var cellSty = row.getDataKeyValue("VendorStyleNum");
                                        var cellAdNbr = lblAdNbr.innerText + " - " + lblAdNbrDesc.innerText;
                                        var cellSysPg = lblSysPgNbr.value;
                                        var cellPgNbr = lblPgNbr.innerText;
                                        var cellUpc = row.getDataKeyValue("UPC");
                                        var cellBatchID = lblBatchID.innerText;

                                        print("DYMO LabelWriter 450", cellMerchID, cellVndNme, cellFPD.substring(0, 80), cellSiz, cellOnOff, cellDept, cellClr.substring(0, 30), cellSty, cellAdNbr, cellSysPg, cellPgNbr, cellUpc, cellBatchID);
                                    }                            
                                }
                            } else {
                                alert("Please select at least one record for Printing a Label.");
                                eventArgs.set_cancel(true);
                            }
                        }

                        if (button.get_text() == "Print Report") {
                            var lblBuyer = document.getElementById("<%=hdnBuyer.ClientID %>").value;
                            var TurnInDate = document.getElementById("<%=lblTurnInDateText.ClientID %>").innerText;

                            window.location.href('CreateSSRSReport.ashx?Buyer=' + lblBuyer + '&Ad=' + <%=CurrentBatch.AdNumber%> + '&Pg=' + <%=CurrentBatch.PageNumber%> + '&Dt=' + TurnInDate + '&BatchId=' + <%=CurrentBatch.BatchId%>);                              
                            eventArgs.set_cancel(true);
                        }
                    }
                } else {
                    if (button.get_text() == "Save All") {
                        eventArgs.set_cancel(!confirm('Are you sure? This action will save all rows.'));
                    }
                    if (button.get_text() == "Cancel All") {
                        eventArgs.set_cancel(!confirm('Are you sure? This action will cancel all rows.'));
                    }
                }
            }

            function print(printerName, merchID, vndName, frndProdDesc, size, onOff, dept, color, style, adNbr, sysPg, pgNbr, upc, batchID) {
                var printers = dymo.label.framework.getPrinters();
                var printer = printers[printerName];
                if (!printer) {
                    alert("Printer '" + printerName + "' not found");
                    return;
                }

                // select label layout/template based on printer type
                var labelXml;
                if (printer.printerType == "LabelWriterPrinter")
                    labelXml = dieCutLabelLayout;
                else {
                    alert("Unsupported printer type");
                    throw "Unsupported printer type";
                }

                // create label set to print printers' data
                var labelSetBuilder = new dymo.label.framework.LabelSetBuilder();
                for (var i = 0; i < printers.length; i++) {
                    var printer = printers[i];

                    // process each printer info as a separate label
                    var record = labelSetBuilder.addRecord();

                    record.setTextMarkup("BARCODE", merchID);
                    record.setTextMarkup("MerchID", merchID);
                    record.setTextMarkup("VendorName", vndName.replace("&", "&amp;"));
                    record.setTextMarkup("ItemDesc", frndProdDesc.substring(0, 40).replace("&", "&amp;"));
                    record.setTextMarkup("ItemDesc_1", frndProdDesc.substring(40, 80).replace("&", "&amp;"));
                    record.setTextMarkup("Size", size.replace("&", "&amp;"));
                    record.setTextMarkup("Figure", onOff);
                    record.setTextMarkup("Dept", dept);
                    record.setTextMarkup("Color", color);
                    record.setTextMarkup("Style", style);
                    record.setTextMarkup("AdNoDesc", adNbr);
                    record.setTextMarkup("SysPage", sysPg);
                    record.setTextMarkup("Batch", batchID);
                    record.setTextMarkup("UPC", upc);
                }

                // finally print label with default printing parameters
                dymo.label.framework.printLabel(printerName, "", labelXml, labelSetBuilder);
            }

            // any label layout is a simple layout with one Text object
            var dieCutLabelLayout = '<?xml version="1.0" encoding="utf-8"?>\
<DieCutLabel Version="8.0" Units="twips">\
	<PaperOrientation>Landscape</PaperOrientation>\
	<Id>LargeAddress</Id>\
	<PaperName>30321 Large Address</PaperName>\
	<DrawCommands>\
		<RoundRectangle X="0" Y="0" Width="2025" Height="5020" Rx="270" Ry="270" />\
	</DrawCommands>\
	<ObjectInfo>\
		<TextObject>\
			<Name>TEXT</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Center</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Bon Ton, Inc.</String>\
					<Attributes>\
						<Font Family="Arial" Size="8" Bold="True" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="1792" Y="58" Width="1008" Height="145.687789916992" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<BarcodeObject>\
			<Name>BARCODE</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>True</IsVariable>\
			<Text>514922</Text>\
			<Type>Code39</Type>\
			<Size>Small</Size>\
			<TextPosition>None</TextPosition>\
			<TextFont Family="Arial" Size="8" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
			<CheckSumFont Family="Arial" Size="8" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
			<TextEmbedding>None</TextEmbedding>\
			<ECLevel>0</ECLevel>\
			<HorizontalAlignment>Center</HorizontalAlignment>\
			<QuietZonesPadding Left="0" Top="0" Right="0" Bottom="0" />\
		</BarcodeObject>\
		<Bounds X="1188" Y="240" Width="2376" Height="408" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>MerchID</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Merch ID</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="322" Y="84" Width="855" Height="240" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>VendorName</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Vendor Name</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="1672" Y="645" Width="3180" Height="200" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>ItemDesc</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Friendly Product Description XXXXXXXXXXXXXXXXX</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="322" Y="800" Width="4419" Height="200" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>MerchID_1</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Dept:</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="322" Y="497" Width="420" Height="240" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>MerchID_2</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Ad Nbr:</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="322" Y="1515" Width="630" Height="240" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>MerchID_3</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Sys Pg:</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="322" Y="1700" Width="606" Height="240" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>Dept</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Middle</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>XXX</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="795" Y="497" Width="465" Height="200" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>ColorHeader</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Color:</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="324.449188232422" Y="1339.60424804688" Width="465.404327392578" Height="240" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>TEXT___1</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Style:</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="322" Y="1155" Width="525" Height="240" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>Style</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>9999999999999999(20)</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="864" Y="1155" Width="1959.80847167969" Height="200" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>AdNoDesc</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Bottom</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Ad Nbr Description</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="987" Y="1523.2138671875" Width="3608" Height="190" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>SizeHeader</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Size:</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="3240.38500976563" Y="1352.69445800781" Width="421.459899902344" Height="240" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>Figure</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Bottom</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>ON OFF</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="322" Y="315" Width="840" Height="200" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>SysPage</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Bottom</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>XX</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="960" Y="1715" Width="222" Height="200" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>MerchID___1</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>UPC:</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="2550" Y="1700" Width="420" Height="240" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>UPC</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Bottom</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>XXXXXXXXXXXX</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="2991" Y="1700" Width="1584" Height="200" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>ItemDesc_1</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX(80)</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="322" Y="980" Width="4419" Height="200" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>Color</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>XXXXXXXXXXXXXXXX(20)</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="864" Y="1348.02673339844" Width="2239.08129882813" Height="200" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>Size</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>XXXX(10)</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="3692.841796875" Y="1360.25744628906" Width="1175.69665527344" Height="200" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>BatchLabel</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Batch:</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="1200" Y="1700" Width="580" Height="240" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>Batch</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Bottom</VerticalAlignment>\
			<TextFitMode>ShrinkToFit</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>XXXXX</String>\
					<Attributes>\
						<Font Family="Arial" Size="9" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="1800" Y="1715" Width="700" Height="200" />\
	</ObjectInfo>\
</DieCutLabel>';
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
