<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GXSCopyView.aspx.vb" Inherits="TurnInProcessAutomation.GXSCopyView"
    ValidateRequest="false" MasterPageFile="~/ContentPage.Master" %>

<%@ MasterType VirtualPath="~/ContentPage.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/WebUserControls/GXS/GXSCopyViewCtrl.ascx" TagPrefix="bonton" TagName="GXSCopyViewCtrl" %>

<asp:Content ID="GXSCopyViewForm" ContentPlaceHolderID="ContentArea" runat="Server">
    <bonton:GXSCopyViewCtrl runat="server" ID="GXSCopyViewCtrl1" />
</asp:Content>
