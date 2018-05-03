<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ContentPage.Master"
    CodeBehind="AdPageSetup.aspx.vb" Inherits="TurnInProcessAutomation.AdPageSetup" %>

<%@ MasterType VirtualPath="~/ContentPage.Master" %>
<%@ Reference Control="~/WebUserControls/Maintenance/AdPageSetUpSearchCtrl.ascx" %>
<%@ Register TagPrefix="TurnIn" TagName="Modal" Src="~/WebUserControls/ModalPopupControl.ascx" %>
<asp:Content ID="AdPageSetupForm" ContentPlaceHolderID="ContentArea" runat="Server">
    <bonton:AdPageSetupCtrl runat="server" id="AdPageSetupCtrl1"></bonton:AdPageSetupCtrl>
</asp:Content>
