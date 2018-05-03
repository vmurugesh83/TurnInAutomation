<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ToolTipValidator.ascx.vb" Inherits="TurnInProcessAutomation.ToolTipValidator" %>
<asp:CustomValidator ID="cvToolTipValidator" runat="server" Display="Dynamic" EnableClientScript="false">
    <asp:Image runat="server" ImageUrl="~/Images/Error.png" />    
</asp:CustomValidator>
<telerik:RadToolTip ID="rttError" runat="server" TargetControlID="cvToolTipValidator" Font-Size="Small" Font-Bold="true" />