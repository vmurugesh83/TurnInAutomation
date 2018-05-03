<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebCategories.aspx.vb"
    Inherits="TurnInProcessAutomation.WebCategories" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title ></title>
    <script type="text/javascript" >
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="rsmJS" runat="server" EnablePageMethods="true">
        <Scripts>
            <asp:ScriptReference Path="~/Javascript/CommonJSFunctions.js" />
            <asp:ScriptReference Path="~/Javascript/jquery-1.3.2.min.js" />
            <asp:ScriptReference Path="~/Javascript/jquery-ui-1.7.0.min.js" />
            <asp:ScriptReference Path="~/Javascript/jquery.bgiframe-2.1.1.pack.js" />
            <asp:ScriptReference Path="~/Javascript/jshashtable-2.1.js" />
            <asp:ScriptReference Path="~/Javascript/jquery.numberformatter-1.2.1.js" />
            <asp:ScriptReference Path="~/Javascript/blockUI.js" />
            <asp:ScriptReference Path="~/Javascript/dymo.label.framework.js" />
        </Scripts>
    </telerik:RadScriptManager>
    <%--    <div >--%>
    <div id="pageHeader">
        <asp:Label ID="lblPageHeader" runat="server" Text="E-Comm Turn-In Webcategories " />
        <bonton:MessagePanel ID="MessagePanel1" runat="server" />
        <br />
    </div>
    <telerik:RadSplitter ID="rsPreTurnInCreate" runat="server" SkinID="pageSplitter"
        Width="100%" Height="100%" align="center">
        <telerik:RadPane ID="rpHeader" runat="server" SkinID="pageHeaderAreaPane">
            <div id="pageActionBar">
                <telerik:RadToolBar ID="rtbeCommWebCat" runat="server" CssClass="SeparatedButtons">
                    <Items>
                        <telerik:RadToolBarButton runat="server" CommandName="Save" DisabledImageUrl="~/Images/Save_d.gif"
                            ImageUrl="~/Images/Save.gif" Text="Save" CssClass="centered">
                        </telerik:RadToolBarButton>
                         <telerik:RadToolBarButton runat="server" CommandName="Add" DisabledImageUrl="~/Images/Add_d.gif"
                            ImageUrl="~/Images/Add.gif" Text="Add & Close" CssClass="centered">
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
            </div>
        </telerik:RadPane>
        <telerik:RadPane ID="rpContent" runat="server" Width="60%" Height="100%" align="center">
            <table width="60%" align="center">
                <tr>
                    <%--<td nowrap="nowrap" colspan="4" align="left" width="100%" height="100%">--%>
                    <td nowrap="nowrap" align="right" valign="top" colspan="1">
                        <asp:Label ID="lblWebCategoriesLabel" runat="server" class="label" Text="*Web Categories:" />
                    </td>
                    <td nowrap="nowrap" align="left">
                        <telerik:RadComboBox ID="cmbWebCategoriesLevel1" runat="server" MarkFirstMatch="true"
                            OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                            AutoPostBack="true" AppendDataBoundItems="false" Width="350" TabIndex="2" Filter="Contains">
                            <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                        </telerik:RadComboBox>
                        <telerik:RadButton ID="imgAddWebCategoriesLevel1" runat="server" Image-ImageUrl="~/Images/Add.gif"
                            Image-DisabledImageUrl="~/Images/Add_d.gif" Width="16px" Height="16px" Visible="false" />
                        <bonton:ToolTipValidator ID="ttvWebCat" runat="server" ControlToEvaluate="grdWebCategories"
                            ValidationGroup="ISNLevel" OnServerValidate="ttvWebCat_ServerValidate" />
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
                    </td>
                    <%--</td>--%>
                    <td nowrap="nowrap" rowspan="8" align="left">
                        <telerik:RadGrid ID="grdWebCategories" runat="server" ShowFooter="false" SkinID="CenteredWithScroll"
                            Height="190px" Width="650px" ShowHeader="True" AutoGenerateColumns="False" CellSpacing="0"
                            GridLines="None">
                            <MasterTableView DataKeyNames="CategoryCode" ClientDataKeyNames="CategoryCode">
                                <EditFormSettings>
                                    <EditColumn UniqueName="EditCommandColumn1">
                                    </EditColumn>
                                </EditFormSettings>
                                <NoRecordsTemplate>
                                    no records retrieved</NoRecordsTemplate>
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
                                                AutoPostBack="true" OnCheckedChanged="cbDefaultCategoryFlag_CheckedChanged" /></ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="120" />
                                        <ItemStyle HorizontalAlign="Center" Width="120" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                        </telerik:RadGrid>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </telerik:RadPane>
    </telerik:RadSplitter>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function ClientClose(args) {
                //if (confirm("Are you sure you want to close this window?")) {
                        GetRadWindow().close(args);
                //    }
                }

                // Used by the eCommerce Prioritization Page
                // Returns the webcat code and the web cat description to the parent page.
            function returnValues(webcatcode, webcatdesc) {
                var args = new Object();
                args.WebCatCde = webcatcode;
                args.WebCatDesc = webcatdesc;

                GetRadWindow().close(args);
                return false;
        }
        </script>
    </telerik:RadCodeBlock>
    </form>
</body>
</html>
