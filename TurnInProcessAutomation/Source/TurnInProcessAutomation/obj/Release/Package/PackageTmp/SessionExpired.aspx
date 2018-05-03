<%@ Page Title="Session Ended" Language="vb" AutoEventWireup="false" MasterPageFile="~/ContentPage.Master"
    CodeBehind="SessionExpired.aspx.vb" Inherits="TurnInProcessAutomation.SessionExpired" %>

<%@ MasterType VirtualPath="~/ContentPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentArea" runat="server">
    <div style="font-size: large; font-weight: bold; color: #FF0000;">
        Session Ended !</div>
    <br />
    <div style="padding: 5px; margin: 3px; border: 1px solid; width: 98%;">
        <asp:Label ID="Label1" runat="server" Text="What happened:" CssClass="label"></asp:Label>
        <p>
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Literal ID="litErrorMessage" runat="server" Text="Your session has been aborted due to a timeout on the server."></asp:Literal>
        </p>
        <asp:Label ID="Label2" runat="server" Text="How this will affect you:" CssClass="label"></asp:Label>
        <p>
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Literal ID="Literal1" runat="server" Text="Any work in progress not committed to disk has been lost."></asp:Literal>
        </p>
        <asp:Label ID="Label3" runat="server" Text="What you can do about it:" CssClass="label"></asp:Label>
        <p>
            &nbsp;&nbsp;&nbsp;&nbsp;Please <a href="Default.aspx">return to the home page</a>
            and log in again to continue accessing your account.</p>
        <br />
         <p>
        &nbsp;&nbsp;&nbsp;&nbsp; If you have any concerns about a pending transactions please
        contact the Bon-Ton Helpdesk at (800) 585-7209. </p>
    </div>
</asp:Content>
