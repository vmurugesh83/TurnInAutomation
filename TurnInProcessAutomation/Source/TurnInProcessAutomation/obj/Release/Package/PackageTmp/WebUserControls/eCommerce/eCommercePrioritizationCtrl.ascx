<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="eCommercePrioritizationCtrl.ascx.vb"
    Inherits="TurnInProcessAutomation.eCommercePrioritizationCtrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<br />
<telerik:RadAjaxManagerProxy ID="rampeCommercePrioritizationCtrl" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="tbleCommercePrioritizationCtrl">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="tbleCommercePrioritizationCtrl" LoadingPanelID="ralpLoadPnl" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>
<table id="tbleCommercePrioritizationCtrl" runat="server" cellpadding="1" style="width: 80%;">
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblEMM" runat="server" class="smallLabel" Text="EMM:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbEMM" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                OnItemsRequested="rcbEMM_ItemsRequested" Width="100" TabIndex="1" DropDownWidth="140px"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblCMG" runat="server" class="smallLabel" Text="CMG:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbCMG" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True" AutoPostBack="true"
                OnItemsRequested="rcbCMG_ItemsRequested" Width="100" TabIndex="2" DropDownWidth="140px"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblBuyer" runat="server" class="smallLabel" Text="Buyer:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbBuyer" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                OnItemsRequested="rcbBuyer_ItemsRequested" Width="100" TabIndex="3" DropDownWidth="200px"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" Filter="Contains">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblLabel" runat="server" class="smallLabel" Text="Label:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbLabel" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                OnItemsRequested="rcbLabel_ItemsRequested" Width="100" TabIndex="4" DropDownWidth="140px"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" Filter="Contains">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblTurnInWeek" runat="server" class="smallLabel" Text="Turn-In Week" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbTUWeek" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                OnItemsRequested="rcbTUWeek_ItemsRequested" Width="100" DropDownWidth="100px"
                TabIndex="5" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" Filter="Contains">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblImageId" runat="server" class="smallLabel" Text="Image ID:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbImageId" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                Width="100" TabIndex="6" DropDownWidth="140px"
                OnItemsRequested="rcbImageId_ItemsRequested" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" Filter="StartsWith">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblVendorStyle" runat="server" class="smallLabel" Text="Vendor Style:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbVndSty" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                OnItemsRequested="rcbVndSty_ItemsRequested" Width="100" TabIndex="7" DropDownWidth="140px"
                OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" Filter="Contains">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-left: 5px">
            <asp:Label ID="lblStatus" runat="server" class="smallLabel" Text="Prioritization Status:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbStatus" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                Width="100" Height="80" TabIndex="8" DropDownWidth="140px" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" AutoPostBack="true">
                <Items>
                    <telerik:RadComboBoxItem Text="Deleted" Value="D"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Text="Failed" Value="F"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Text="Pending" Value="P" Selected="true"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Text="Uploaded (Success)" Value="U"></telerik:RadComboBoxItem>                    
                </Items>
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
</table>
