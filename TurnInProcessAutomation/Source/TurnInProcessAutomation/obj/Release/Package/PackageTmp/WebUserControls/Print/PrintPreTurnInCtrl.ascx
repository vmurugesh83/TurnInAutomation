<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PrintPreTurnInCtrl.ascx.vb" Inherits="TurnInProcessAutomation.PrintPreTurnInCtrl" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<br />

 <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="tblPrintPreTurnInCtrl">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblPrintPreTurnInCtrl" LoadingPanelID="ralpLoadPnl" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <table id="tblPrintPreTurnInAdLevelCtrl" runat="server" cellpadding="0" style="width: 80%;">

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
                <asp:Label ID="lblFOB" runat="server" class="smallLabel"  Text="FOB:" />
            </td>
            <td width="70%">
                <telerik:RadComboBox ID="cmbFOB" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                    class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                    Width="200" TabIndex="3" >      
                     <Items>   
                        <telerik:RadComboBoxItem runat="server" Text="Jewelry" Selected="True" />                         
                    </Items>                      
                    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                </telerik:RadComboBox>
            </td>
        </tr>
</table>

<table id="tblPrintPreTurnInCtrl" runat="server" cellpadding="0" style="width: 80%;">

     <tr>
            <td nowrap="nowrap" align="right" style="padding-left: 5px">
                <asp:Label ID="lblDept" runat="server" class="smallLabel"  Text="Dept:" />
            </td>
            <td width="70%">
                <telerik:RadComboBox ID="cmbDept" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                    class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                    Width="80" TabIndex="1" DropDownWidth="205px">                      
                    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                </telerik:RadComboBox>
            </td>
        </tr>

            <tr>
            <td nowrap="nowrap" align="right" style="padding-left: 5px">
                <asp:Label ID="lblClass" runat="server" class="smallLabel"  Text="Class:" />
            </td>
            <td width="70%">
                <telerik:RadComboBox ID="cmbClass" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                    class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                    Width="80" TabIndex="2" DropDownWidth="205px">                      
                    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                </telerik:RadComboBox>
            </td>
        </tr>

         <tr>
            <td nowrap="nowrap" align="right" style="padding-left: 5px">
                <asp:Label ID="lblACode" runat="server" class="smallLabel"  Text="A-CD 1:" />
            </td>
            <td width="70%">
                <telerik:RadComboBox ID="cmbACode" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                    class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                    Width="80" TabIndex="3" DropDownWidth="205px">                      
                    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                </telerik:RadComboBox>
            </td>
        </tr>

       <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblCreatedSince" runat="server" class="smallLabel"  Text="Created Since:" />
        </td>
        <td width="70%">
            <telerik:RadDatePicker ID="dpCreatedSince"  Style="vertical-align: middle;"
                runat="server" Width="100px" TabIndex="4" UseEmbeddedScripts="false" >
                <DateInput ID="diCreatedSince" TabIndex="4" DisplayDateFormat="MM/dd/yyyy" runat="server"></DateInput>
                <Calendar ID="calCreatedSince" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x" runat="server"></Calendar>
                <DatePopupButton ImageUrl="" HoverImageUrl="" TabIndex="4"></DatePopupButton>
            </telerik:RadDatePicker>
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Error.png" Visible="False" />
                                
        </td>
    </tr>

    <tr><td>&nbsp;</td></tr>
         
    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblVendorStyle" runat="server" class="smallLabel"  Text="Vendor Style:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbVendorStyle" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                Width="80" TabIndex="5" DropDownWidth="205px">                      
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
            &nbsp;&nbsp;
              <asp:Image ID="imgAddVendorStyle" runat="server" ImageUrl="~/Images/Add.gif"  />
        </td>
    </tr>

      <tr >
             <td nowrap="nowrap" align="right" style="padding-left: 5px"> &nbsp; </td>
            <td width="70%">
            <asp:ListBox ID="listBoxVendorStyle" runat="server" TabIndex="6" Height="120px"   SelectionMode="Multiple" Width="100" />     
                      &nbsp;&nbsp;
              <asp:Image ID="imgRemoveVendorStyle" runat="server" ImageUrl="~/Images/minus-button.png"  ImageAlign="Top" />       
        </td>       
    </tr>

  <tr>
        <td>&nbsp;&nbsp;&nbsp;
                  <asp:Label ID="lblOR1" runat="server" class="label"  Text="OR" Font-Size="Large" ForeColor="Red" />
       </td>
        <td>&nbsp;</td>
    </tr>

   <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblISN" runat="server" class="smallLabel"  Text="ISN:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbISN" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                Width="80" TabIndex="7" DropDownWidth="205px">                      
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
              &nbsp;&nbsp;
              <asp:Image ID="imgAddISN" runat="server" ImageUrl="~/Images/Add.gif"  />
        </td>
    </tr>

      <tr >
             <td nowrap="nowrap" align="right" style="padding-left: 5px"> &nbsp; </td>
            <td width="70%">
            <asp:ListBox ID="listBoxISN" runat="server" TabIndex="8" Height="120px"   SelectionMode="Multiple" Width="100" />     
             &nbsp;&nbsp;
              <asp:Image ID="imgRemoveISN" runat="server" ImageUrl="~/Images/minus-button.png"  ImageAlign="Top" />       
        </td>       
    </tr>

       <tr>
        <td>&nbsp;&nbsp;&nbsp;
                  <asp:Label ID="lblOR2" runat="server" class="label"  Text="OR" Font-Size="Large" ForeColor="Red" />
       </td>
        <td>&nbsp;</td>
    </tr>

    <tr>
        <td nowrap="nowrap" align="right" style="padding-left: 5px">
            <asp:Label ID="lblReserveISN" runat="server" class="smallLabel"  Text="Reserve ISN:" />
        </td>
        <td width="70%">
            <telerik:RadComboBox ID="cmbReserverISN" runat="server" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                class="RadComboBox_Vista" AllowCustomText="false" AppendDataBoundItems="True"
                Width="80" TabIndex="9" DropDownWidth="205px">                      
                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
            </telerik:RadComboBox>
            &nbsp;&nbsp;
              <asp:Image ID="imgAddReserveISN" runat="server" ImageUrl="~/Images/Add.gif"  />
        </td>
    </tr>

      <tr >
             <td nowrap="nowrap" align="right" style="padding-left: 5px"> &nbsp; </td>
            <td width="70%">
            <asp:ListBox ID="listBoxReserveISN" runat="server" TabIndex="10" Height="120px"   SelectionMode="Multiple" Width="100" />     
             &nbsp;&nbsp;
              <asp:Image ID="imgRemoveReserveISN" runat="server" ImageUrl="~/Images/minus-button.png"  ImageAlign="Top" />       
        </td>       
    </tr>
            
       <tr>
        <td>&nbsp;&nbsp;&nbsp;
                  <asp:Label ID="lblOR3" runat="server" class="label"  Text="OR" Font-Size="Large" ForeColor="Red" />
       </td>
        <td>&nbsp;</td>
    </tr>  

     <tr >
            <td>&nbsp;</td>
             <td colspan="2" >
                       <asp:Button  ID="btnAddTBDItem"  runat="server"  Text="ADD TBD ITEM >>>"  Font-Bold="True" Width="150" />        
             </td>       
    </tr>


</table>