<%@ Page Title="Application Error" Language="vb" AutoEventWireup="false" MasterPageFile="~/ContentPage.Master"
    CodeBehind="Error.aspx.vb" Inherits="TurnInProcessAutomation.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentArea" runat="server">
    <div style="font-size: large; font-weight: bold; color: #FF0000;">
        Application Error !</div>
    <br />
    <div style="padding: 5px; margin: 3px; border: 1px solid; width: 98%;">
        <asp:Label ID="Label1" runat="server" Text="What happened:" CssClass="label"></asp:Label>
        <p>
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Literal ID="litErrorMessage" runat="server" Text="The details have been recorded for review by IS personnel."></asp:Literal>
        </p>
        <asp:Label ID="Label2" runat="server" Text="How this will affect you:" CssClass="label"></asp:Label>
        <p>
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Literal ID="Literal1" runat="server" Text="The current page will not load."></asp:Literal>
        </p>
        <asp:Label ID="Label3" runat="server" Text="What you can do about it:" CssClass="label"></asp:Label>
        <p>
            &nbsp;&nbsp;&nbsp;&nbsp; Close your browser, navigate back to the website, and try
            repeating your last action.
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp; Try performing alternative methods of performing the same
            action.
            <br />
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp; If you have any concerns about a pending transactions please
            contact the Bon-Ton Helpdesk at (800) 585-7209.
        </p>
    </div>
</asp:Content>
