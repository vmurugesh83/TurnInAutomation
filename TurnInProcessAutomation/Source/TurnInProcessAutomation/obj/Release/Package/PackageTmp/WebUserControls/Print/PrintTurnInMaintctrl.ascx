<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PrintTurnInMaintctrl.ascx.vb" 
    Inherits="TurnInProcessAutomation.PrintTurnInMaintctrl" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<br />

 <telerik:RadAjaxManagerProxy ID="rAPMPrintTurnInMaintctrl" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="tblPrintTurnInMaintctrl">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblPrintTurnInMaintctrl" LoadingPanelID="ralpLoadPnl" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <table id="tblPrintTurnInMaintctrl" runat="server" cellpadding="0" style="width: 80%;">

     <tr>
            <td nowrap="nowrap" align="right" style="padding-left: 5px">
                <asp:Label ID="lblAdNo" runat="server" class="smallLabel"  Text="Ad #:" />
            </td>
            <td width="70%">
                <telerik:RadComboBox ID="cmbAdNo" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                    class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                    Width="80" TabIndex="1" >              
                      <Items>   
                        <telerik:RadComboBoxItem runat="server" Text="1281" Selected="True" />   
                        <telerik:RadComboBoxItem runat="server" Text="1282" />   
                        <telerik:RadComboBoxItem runat="server" Text="1283" /> 
                    </Items>           
                    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                </telerik:RadComboBox>
            </td>
        </tr>

         <tr>
            <td nowrap="nowrap" align="right" style="padding-left: 5px">
                <asp:Label ID="lblPageNo" runat="server" class="smallLabel"  Text="Page #:" />
            </td>
            <td width="70%">
                <telerik:RadComboBox ID="cmbPageNo" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                    class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                    Width="80" TabIndex="2" >       
                     <Items>   
                        <telerik:RadComboBoxItem runat="server" Text="1" Selected="True" />   
                        <telerik:RadComboBoxItem runat="server" Text="2" />   
                        <telerik:RadComboBoxItem runat="server" Text="3" /> 
                    </Items>                    
                    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                </telerik:RadComboBox>
            </td>
        </tr>

      <tr>
            <td nowrap="nowrap" align="right" style="padding-left: 5px">
                <asp:Label ID="lblCRG" runat="server" class="smallLabel"  Text="CRG:" />
            </td>
            <td width="70%">
                <telerik:RadComboBox ID="cmbCRG" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                    class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                    Width="150" TabIndex="3" >      
                     <Items>   
                        <telerik:RadComboBoxItem runat="server" Text="Jewelry" Selected="True" />                         
                    </Items>                      
                    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                </telerik:RadComboBox>
            </td>
        </tr>

     <tr>
            <td nowrap="nowrap" align="right" style="padding-left: 5px">
                <asp:Label ID="lblCMG" runat="server" class="smallLabel"  Text="CMG:" />
            </td>
            <td width="70%">
                <telerik:RadComboBox ID="cmbCMG" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                    class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                    Width="150" TabIndex="4" DropDownWidth="200px">                      
                    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                </telerik:RadComboBox>
            </td>
        </tr>

         <tr>
            <td nowrap="nowrap" align="right" style="padding-left: 5px">
                <asp:Label ID="lblFOB" runat="server" class="smallLabel"  Text="FOB:" />
            </td>
            <td width="70%">
                <telerik:RadComboBox ID="cmbFOB" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                    class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                    Width="150" TabIndex="5" DropDownWidth="200px">                      
                    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                </telerik:RadComboBox>
            </td>
        </tr>
        </table>
