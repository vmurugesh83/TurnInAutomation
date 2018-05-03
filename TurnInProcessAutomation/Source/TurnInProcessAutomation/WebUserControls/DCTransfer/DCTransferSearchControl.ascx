<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DCTransferSearchControl.ascx.vb" Inherits="TurnInProcessAutomation.DCTransferSearchControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<br />
<telerik:RadAjaxManagerProxy ID="rampDCTransferSearchControl" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="rcbDept">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rcbVendor" />
                <telerik:AjaxUpdatedControl ControlID="tblDCTransferSearchControl" LoadingPanelID="ralpLoadPnl" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="tblDCTransferSearchControl">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="tblDCTransferSearchControl" LoadingPanelID="ralpLoadPnl" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>
<table id="tblDCTransferSearchControl" runat="server" cellpadding="1" style="width: 80%;">
    <tr>
        <td nowrap="nowrap" align="right">
            <asp:Label ID="Label2" runat="server" class="smallLabel" Text="Price Status:" /></td>
        <td width="85%">
            <asp:CheckBoxList runat="server" ID="cblPriceStatusCodes" RepeatDirection="Horizontal"
                RepeatLayout="Flow" CssClass="ChkLabel">
                <asp:ListItem Text="R" Selected="true" />
                <asp:ListItem Text="V" Selected="true" />
                <asp:ListItem Text="M" />
                <asp:ListItem Text="C" />
                <asp:ListItem Text="F" />
                <asp:ListItem Text="P" />
            </asp:CheckBoxList></td>
    </tr>

    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblDepartment" runat="server" class="smallLabel" Text="Department:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbDept" runat="server" MarkFirstMatch="true"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                Width="160" TabIndex="1" DropDownWidth="140px"
                Filter="Contains">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblBuyer" runat="server" class="smallLabel" Text="Buyer:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbBuyer" runat="server" MarkFirstMatch="true"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" AutoPostBack="true"
                Width="160" TabIndex="2" DropDownWidth="140px"
                Filter="Contains">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="Label1" runat="server" class="smallLabel" Text="Vendor:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbVendor" runat="server" MarkFirstMatch="true"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" AutoPostBack="true"
                Width="160" TabIndex="2" DropDownWidth="140px"
                Filter="Contains">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
</table>
