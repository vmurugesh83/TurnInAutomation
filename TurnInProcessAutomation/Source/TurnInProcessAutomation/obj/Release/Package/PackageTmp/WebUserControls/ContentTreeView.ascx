<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ContentTreeView.ascx.vb" Inherits="TurnInProcessAutomation.WebUserControls_ContentTreeView" %>

<table cellpadding="4" style="width:99%;">
    <tr>
        <td>
            <asp:Label ID="lblStatusId" runat="server" CssClass="label" Text="Status:&nbsp;&nbsp;" Visible="False" />
            <asp:Label ID="lblStatus" runat="server" CssClass="label" Visible="False" />
        </td>
    </tr>
    <tr>
        <td>
            <telerik:RadTreeView ID="rtvContent" runat="server" />
            &nbsp;
        </td>
    </tr>
</table>