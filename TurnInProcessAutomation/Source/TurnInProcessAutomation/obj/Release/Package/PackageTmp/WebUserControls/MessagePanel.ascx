<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MessagePanel.ascx.vb"
    Inherits="TurnInProcessAutomation.MessagePanel" %>
<ajaxtoolkit:CollapsiblePanelExtender ID="cpeMessage" runat="server" Collapsed="False"
    SuppressPostBack="True" TargetControlID="pnlDetails" ExpandControlID="pnlTitle"
    CollapseControlID="pnlTitle" ImageControlID="imgDetails" ExpandedImage="~/Images/Expanded_red.gif"
    CollapsedImage="~/Images/Collapsed_red.gif" AutoExpand="true" />
<asp:Panel ID="pnlMessage" runat="server" CssClass="messagePanel" Width="100%">
    <div class="collapsiblePanel">
        <asp:Panel ID="pnlTitle" runat="server" CssClass="title">
            <asp:Label ID="lblMessage" runat="server" />
        </asp:Panel>
        <asp:Panel ID="pnlDetails" runat="server" CssClass="details">
            <asp:ValidationSummary ID="vsDetails" runat="server" />
            <asp:BulletedList ID="blDetails" runat="server">
                
            </asp:BulletedList>
        </asp:Panel>
    </div>
</asp:Panel>
