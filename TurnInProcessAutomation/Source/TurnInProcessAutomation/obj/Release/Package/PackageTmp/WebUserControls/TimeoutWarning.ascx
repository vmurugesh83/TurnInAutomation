<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TimeoutWarning.ascx.vb" Inherits="TurnInProcessAutomation.TimeoutWarning" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="200px" Width="300px">
 <asp:Timer ID="TimerTimeout" runat="server">
        </asp:Timer>
        <asp:Panel ID="PanelTimeout" runat="server" Visible="false" 
            CssClass="timeoutMessage">
        Your session has expired.<br />
        <asp:Button ID="Button1" runat="server" Text="Hide" />
        </asp:Panel>
</telerik:RadAjaxPanel>
