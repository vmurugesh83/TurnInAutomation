<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="eCommTurnInMeetCtrl.ascx.vb"
    Inherits="TurnInProcessAutomation.eCommTurnInMeetCtrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<br />

<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="rcbAds">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rcbAds" />
                <telerik:AjaxUpdatedControl ControlID="rcbPageNumber" />
                <telerik:AjaxUpdatedControl ControlID="cmbBatchNum" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="rcbEMM">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rcbEMM" />
                <telerik:AjaxUpdatedControl ControlID="cmbBatchNum" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbDept">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbDept" />
                <telerik:AjaxUpdatedControl ControlID="cmbVendorStyle" />
                <telerik:AjaxUpdatedControl ControlID="cmbBatchNum" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbBuyer">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbBuyer" />
                <telerik:AjaxUpdatedControl ControlID="cmbBatchNum" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbVendorStyle">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbVendorStyle" />
                <telerik:AjaxUpdatedControl ControlID="cmbBatchNum" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbLabel">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbLabel" />
                <telerik:AjaxUpdatedControl ControlID="cmbBatchNum" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmbBatchNum">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="cmbBatchNum" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>
<table id="tbleCommTurnInMaintCtrl" runat="server" cellpadding="0" style="width: 80%;">
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblAdNo" runat="server" class="smallLabel" Text="Ad #:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbAds" runat="server" OnItemsRequested="rcbAds_ItemsRequested"
              OnSelectedIndexChanged="rcbAds_SelectedIndexChanged"
                AutoPostBack="true" EnableLoadOnDemand="true" ShowMoreResultsBox="false" EnableVirtualScrolling="false"
                DropDownWidth="400" Width="100" HighlightTemplatedItems="true">
                <HeaderTemplate>
                    <table style="width: 350px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                Ad #
                            </td>
                            <td style="width: 300px;">
                                Description
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 350px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                <%# Eval("AdNbr")%>
                            </td>
                            <td style="width: 300px; text-align: left">
                                <%# Eval("AdDesc")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblPageNo" runat="server" class="smallLabel" Text="Page #:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbPageNumber" runat="server" OnClientBlur="OnClientBlurHandler"
                DropDownWidth="400" Width="100" Enabled="false" HighlightTemplatedItems="true" AutoPostBack="true">
                <HeaderTemplate>
                    <table style="width: 350px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                Page #
                            </td>
                            <td style="width: 300px;">
                                Description
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 350px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50px;">
                                <%# Eval("PgNbr")%>
                            </td>
                            <td style="width: 200px; text-align: left">
                                <%# Eval("PgDesc")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
    <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblEMM" runat="server" class="smallLabel" Text="EMM:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="rcbEMM" runat="server" 
                class="RadComboBox_Vista" AutoPostBack="true"
                OnItemsRequested="rcbEMM_ItemsRequested" OnSelectedIndexChanged="rcbEMM_SelectedIndexChanged" Width="100" TabIndex="1" EnableLoadOnDemand="true" DropDownWidth="140px"
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
            <telerik:RadComboBox ID="cmbBuyer" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" DropDownWidth="400" Width="100"
                TabIndex="3" AutoPostBack="true">
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblDept" runat="server" class="smallLabel" Text="Dept #:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbDept" runat="server" class="RadComboBox_Vista" AllowCustomText="false"
                OnItemsRequested="cmbDept_ItemsRequested" DropDownWidth="400" Width="100" TabIndex="4" AutoPostBack="true">
            </telerik:RadComboBox>
        </td>
    </tr>
      <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblLabel" runat="server" class="smallLabel" Text="Label:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbLabel" runat="server" class="RadComboBox_Vista" OnClientBlur="OnClientBlurHandler"  
            OnItemsRequested="cmbLabel_ItemsRequested" AllowCustomText="false" DropDownWidth="400" Width="100" TabIndex="5" AutoPostBack="true">
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="Label1" runat="server" class="smallLabel" Text="Vendor Style:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbVendorStyle" runat="server" class="RadComboBox_Vista"
                AllowCustomText="false" DropDownWidth="400" Width="100" TabIndex="6" OnClientBlur="OnClientBlurHandler"  
            OnItemsRequested="cmbVendorStyle_ItemsRequested" AutoPostBack="true" Enabled="False">
            </telerik:RadComboBox>
        </td>
    </tr>
        <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblBatchNum" runat="server" class="smallLabel" Text="Batch #:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbBatchNum" runat="server" class="RadComboBox_Vista"
                AllowCustomText="false" Width="100" TabIndex="7" OnClientBlur="OnClientBlurHandler"  
            OnItemsRequested="cmbBatchNum_ItemsRequested">
            </telerik:RadComboBox>
        </td>
    </tr>
</table>
