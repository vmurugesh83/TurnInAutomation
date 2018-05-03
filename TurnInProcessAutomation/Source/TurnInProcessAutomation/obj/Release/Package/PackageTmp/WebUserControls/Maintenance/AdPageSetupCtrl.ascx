<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdPageSetupCtrl.ascx.vb"
    Inherits="TurnInProcessAutomation.AdPageSetupCtrl" %>
<telerik:RadSplitter ID="rsAdPageSetup" runat="server" SkinID="pageSplitter">
    <telerik:RadPane ID="rpHeader" runat="server" Height="70" Scrolling="None" Font-Bold="True">
        <div id="pageActionBar">
            <telerik:RadToolBar ID="rtbAdPageSetup" runat="server" OnClientButtonClicking="OnClientButtonClicking"
                OnClientLoad="clientLoad" OnClientButtonClicked="setHourglass" CssClass="SeparatedButtons">
                <Items>
                    <telerik:RadToolBarButton runat="server" CommandName="Back" DisabledImageUrl="~/Images/BackButton.gif"
                        ImageUrl="~/Images/BackButton.gif" Text="Back">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton id="btnSave" runat="server" CommandName="Save" DisabledImageUrl="~/Images/Save_d.gif"
                        ImageUrl="~/Images/Save.gif" Text="Save" CssClass="rightAligned" Enabled="false">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton runat="server" CommandName="Reset" DisabledImageUrl="~/Images/Reset_d.gif"
                        ImageUrl="~/Images/Reset.gif" Text="Reset" CssClass="rightAligned">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton runat="server" CommandName="Retrieve" DisabledImageUrl="~/Images/Retrieve1_d.gif"
                        ImageUrl="~/Images/Retrieve1.gif" Text="Retrieve" CssClass="rightAligned">
                    </telerik:RadToolBarButton>
                </Items>
            </telerik:RadToolBar>
        </div>
        <div id="pageHeader">
            <asp:Label ID="lblPageHeader" runat="server" Text="Ad Page Setup" />
            <bonton:MessagePanel ID="MessagePanel1" runat="server" />
        </div>
    </telerik:RadPane>
    <telerik:RadPane ID="rpContent" runat="server">
        <table class="labels-vertical" style="margin: 20px;" width="700">
            <tr>
                <td style="white-space: nowrap" align="right">
                    <asp:Label ID="lblAdNoLabel" runat="server" CssClass="label" Text="Ad#:" Visible="false" />
                </td>
                <td>
                    <asp:Label ID="lblAdNoText" runat="server" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblAdNoDescText" runat="server" />
                </td>
                <td style="white-space: nowrap" align="right">
                    <asp:Label ID="lblRunStartLabel" runat="server" CssClass="label" Text="Run Start"
                        Visible="false" />
                </td>
                <td>
                    <asp:Label ID="lblRunStartText" runat="server" />
                </td>
                <td style="white-space: nowrap" align="right">
                    <asp:Label ID="lblBasePageLabel" runat="server" CssClass="label" Text="Base Pages:"
                        Visible="false" />
                </td>
                <td>
                    <asp:Label ID="lblBasePageText" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap" colspan="2">
                    &nbsp;
                </td>
                <td style="white-space: nowrap" align="right">
                    <asp:Label ID="lblRunEndLabel" runat="server" CssClass="label" Text="Run End:" Visible="false" />
                </td>
                <td>
                    <asp:Label ID="lblRunEndText" runat="server" />
                </td>
                <td style="white-space: nowrap" align="right">
                    <asp:Label ID="lblMasterPageLabel" runat="server" CssClass="label" Text="Master Pages:"
                        Visible="false" />
                </td>
                <td>
                    <asp:Label ID="lblMasterPageText" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="6" align="left">
                <div style="float:left">
                    <telerik:RadGrid ID="grdAdPageSetup" runat="server" SkinID="CenteredWithScroll" AllowPaging="True"
                        Height="400" Width="700" EditMode="InPlace" ShowFooter="false" Visible="false">
                        <ValidationSettings ValidationGroup="PerformInsert" CommandsToValidate="PerformInsert,Update"
                            EnableValidation="true" />
                        <MasterTableView CommandItemDisplay="Top" EditMode="InPlace">
                            <EditFormSettings>
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                            </EditFormSettings>
                            <CommandItemTemplate>
                                            <div style="float:left; padding: 2px">
                                            <asp:Button ID="btnAddRecord" runat="server" CommandName="InitInsert" Text="Add Page"
                                                Font-Bold="True" Width="100" />
                                            </div>
                            </CommandItemTemplate>
                            <NoRecordsTemplate>
                                no records retrieved</NoRecordsTemplate>
                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                            <Columns>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Images/Edit1.gif"
                                    CancelImageUrl="~/Images/Cancel1.gif" UpdateImageUrl="~/Images/CheckMark.gif"
                                    HeaderText="" UniqueName="EditColumn">
                                    <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="50" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridTemplateColumn HeaderText="System Page" UniqueName="SystemPage">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="SystemPageNumber" Text='<%# Eval("syspgnbr")%>'></asp:Label></ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label runat="server" ID="SystemPageNumber" Text='<%# IIf(DataBinder.Eval(Container, "OwnerTableView.IsItemInserted") And TypeOf Container Is GridDataInsertItem, GetIncrementedSysPage(), Eval("syspgnbr"))%>'></asp:Label>
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="false" Width="85" />
                                    <ItemStyle HorizontalAlign="Right" Width="85" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Page Description" UniqueName="PageDescription">
                                    <ItemTemplate>
                                        <%# Eval("pgdesc")%></ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox runat="server" ID="txtDescription" Text='<%# Eval("pgdesc")%>' style="text-align:left;" Width="450" /><bonton:ToolTipValidator
                                            ID="ttvValidateDescription" runat="server" ControlToEvaluate="txtDescription"
                                            ValidationGroup="PerformInsert" OnServerValidate="ttvValidateDescription_ServerValidate" />
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="false" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Page Order" UniqueName="PageOrder">
                                    <ItemTemplate>
                                        <%# Eval("pgnbr")%></ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox runat="server" ID="txtPageOrder" Text='<%# Eval("pgnbr")%>' style="text-align:left;" Width="50"
                                             /><bonton:ToolTipValidator ID="ttvValidatePageOrder" runat="server"
                                                ControlToEvaluate="txtPageOrder" ValidationGroup="PerformInsert" OnServerValidate="ttvValidatePageOrder_ServerValidate" />
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="85" Wrap="false" />
                                    <ItemStyle HorizontalAlign="Right" Width="85" />
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                </td>
            </tr>
        </table>
    </telerik:RadPane>
</telerik:RadSplitter>
