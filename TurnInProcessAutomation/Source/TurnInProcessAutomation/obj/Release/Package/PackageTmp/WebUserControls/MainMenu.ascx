<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MainMenu.ascx.vb" Inherits="TurnInProcessAutomation.WebUserControls_MainMenu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadMenu ID="rmMain" runat="server">
    <collapseanimation type="OutQuint" duration="200"></collapseanimation>
    <databindings>
        <telerik:RadMenuItemBinding DataMember="MenuItem" EnabledField="Enabled" NavigateUrlField="NavigateUrl"
            TextField="Text" ToolTipField="ToolTip" ValueField="SeqNum" />
    </databindings>
</telerik:RadMenu>
<asp:XmlDataSource ID="xmlMenuItems" runat="server" XPath="MenuItems/MenuItem" EnableCaching="False">
</asp:XmlDataSource>



