<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/ContentPage.Master" 
CodeBehind="LogOff.aspx.vb" Inherits="TurnInProcessAutomation.LogOff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentArea" runat="server">
    <br />
    <div style="padding: 5px; margin: 3px; border: 1px solid; width: 98%;">
        
        <asp:Label ID="Label3" runat="server" Text="You are logged off." CssClass="label"></asp:Label>
        <p>
            &nbsp;&nbsp;&nbsp;&nbsp; Close your browser, navigate back to the website, and try
            logging in again.
            <br />
            <br />
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp; If you have any concerns about a pending transactions please
            contact the Bon-Ton Helpdesk at (800) 585-7209.
        </p>
    </div>
</asp:Content>
