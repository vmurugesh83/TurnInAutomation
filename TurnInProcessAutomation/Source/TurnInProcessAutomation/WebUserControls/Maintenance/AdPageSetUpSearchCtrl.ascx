<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdPageSetUpSearchCtrl.ascx.vb" Inherits="TurnInProcessAutomation.AdPageSetUpSearchCtrl" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<br />

 <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="tblAdPageSetupCtrl">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblAdPageSetupCtrl" LoadingPanelID="ralpLoadPnl" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <table id="tblAdPageSetupCtrl" runat="server" cellpadding="0" style="width: 80%;">
                    <tr>                       
                         <td align="right">   
                                    <asp:Label ID="lblAdNoLabel" runat="server" class = "label" Text="Ad#:" />             
                        </td>
                         <td>       
                                <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">   
                                    <telerik:RadComboBox 
                                    ID="rcbAds" runat="server" 
                                    OnItemsRequested="rcbAds_ItemsRequested"
                                    EnableLoadOnDemand="true" 
                                    ShowMoreResultsBox="true" 
                                    EnableVirtualScrolling="true"
                                    DropDownWidth="400"
                                    Height="218"
                                    HighlightTemplatedItems="true"
                                    >
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
                                                <td style="width: 50px;"><%# DataBinder.Eval(Container.DataItem, "AdNbr")%></td>
                                                <td style="width: 300px; text-align:left"><%# DataBinder.Eval(Container.DataItem, "AdDesc")%></td>
                                            </tr>
                                       </table>
                                      </ItemTemplate>
                                    </telerik:RadComboBox>
                                </telerik:RadAjaxPanel>
                         </td>
                     </tr>
</table>