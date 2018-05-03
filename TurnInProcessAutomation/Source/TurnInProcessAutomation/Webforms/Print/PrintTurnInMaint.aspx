<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ContentPage.Master" 
CodeBehind="PrintTurnInMaint.aspx.vb" Inherits="TurnInProcessAutomation.PrintTurnInMaint" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/ContentPage.Master" %>
<%@ Reference Control="~/WebUserControls/Print/PrintTurnInMaintctrl.ascx" %>
<%@ Register TagPrefix="TurnIn" TagName="Modal" Src="~/WebUserControls/ModalPopupControl.ascx" %>

<asp:Content ID="PrintTurnInMaintForm" ContentPlaceHolderID="ContentArea" runat="Server">
    <telerik:RadSplitter ID="rsPrintTurnInMaint" runat="server" SkinID="pageSplitter">
        <telerik:RadPane ID="rpHeader" runat="server" Height="90" Scrolling="None" 
            Font-Bold="True">
            <div id="pageActionBar">
                <telerik:RadToolBar ID="rtbPrintTurnInMaint" runat="server" OnClientButtonClicking="OnClientButtonClicking"
                    OnClientLoad="clientLoad"  OnClientButtonClicked="setHourglass"  CssClass="SeparatedButtons">                    
                    <Items>                                                              
                        <telerik:RadToolBarButton runat="server" CommandName="Back" DisabledImageUrl="~/Images/BackButton.gif" 
                            ImageUrl="~/Images/BackButton.gif" Text="Back" >
                        </telerik:RadToolBarButton>   
                        
                        <telerik:RadToolBarButton runat="server" CommandName="Export" DisabledImageUrl="~/Images/Export_d.gif"
                            ImageUrl="~/Images/Export.gif" Text="Export" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        
                         <telerik:RadToolBarButton runat="server" CommandName="Reset" DisabledImageUrl="~/Images/Reset_d.gif"
                            ImageUrl="~/Images/Reset.gif" Text="Reset" CssClass="rightAligned">
                        </telerik:RadToolBarButton>                   
                        
                         <telerik:RadToolBarButton runat="server" CommandName="Submit" DisabledImageUrl="~/Images/Save_d.gif"
                            ImageUrl="~/Images/Save.gif" Text="Submit" CssClass="rightAligned">
                        </telerik:RadToolBarButton>               
                        
                             <telerik:RadToolBarButton runat="server" CommandName="Retrieve" DisabledImageUrl="~/Images/Retrieve1_d.gif"
                            ImageUrl="~/Images/Retrieve1.gif" Text="Retrieve" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                                         
                    </Items>
                </telerik:RadToolBar>
            </div>
                  
            <div id="pageHeader">                          
                <asp:Label ID="lblPageHeader" runat="server" Text="Print Turn-In Meeting" />                
            </div>                         
        </telerik:RadPane>        
        <telerik:RadPane ID="rpContent" runat="server">   
                 
               <asp:Panel ID="pnlPrintTurnInMaint" runat="server" Visible="true" >

               <table align="right">           
                          <tr>
                                <td colspan="6">&nbsp;</td>
                                <td> <asp:Label ID="lblUserCrtDateLabel" runat="server"  class= "label"  Text="User/Create Date:" /></td>
                                <td> <asp:Label ID="lblUserCrtDateText" runat="server" Text="Stoll, S 10/31/12" /></td>
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                          </tr>   
                          <tr>  
                                <td colspan="6">&nbsp;</td>
                                 <td> <asp:Label ID="lblUserModDateLabel" runat="server"  class= "label"  Text="User/Modify Date:" /></td>
                                <td> <asp:Label ID="lblUserModDateText" runat="server" Text="Stoll, S 10/31/12" /></td>
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                          </tr> 
                    </table>

                <table class="labels-vertical" style="margin:20px;" width="700" >
                       <tr>
                                <td nowrap="nowrap" align="right" >
                                    <asp:Label ID="lblAdNoLabel" runat="server" class="label"  Text="Ad#:" />
                                </td>
                                <td>
                                     <asp:Label ID="lblAdNoText" runat="server"  Text="31248" />
                                </td>             
                                  <td nowrap="nowrap" align="right" >
                                    <asp:Label ID="lblAdNoDescLabel" runat="server" class="label"  Text="Desc:" />
                                </td>
                                <td>
                                    <asp:Label ID="lblAdNoDescText" runat="server"  Text="10_10 Anniversary Sale Mailer" />
                                </td>
                                  <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblRunStartLabel" runat="server" class="label"  Text="Run Start:" />
                                  </td>
                                <td>
                                    <asp:Label ID="lblRunStartText" runat="server"  Text="10/10/2012" />
                                </td>                                                          
                        </tr>    
                        <tr>
                                <td nowrap="nowrap" align="right" >
                                    <asp:Label ID="lblPageNoLabel" runat="server" class="label"  Text="Page #:" />
                               </td>
                                <td>
                                    <asp:Label ID="lblPageNoText" runat="server"  Text="005" />
                                </td>        
                                 <td nowrap="nowrap"  align="right" >
                                    <asp:Label ID="lblBookVersionLabel" runat="server" class="label"  Text="Book Version:" />
                                </td>
                                <td>
                                    <asp:Label ID="lblBookVersionText" runat="server"  Text="2712" />
                                </td>    
                                  <td nowrap="nowrap" align="right">
                                    <asp:Label ID="lblRunEndLabel" runat="server" class="label"  Text="Run End:" />
                                 </td>
                                <td>
                                    <asp:Label ID="lblRunEndText" runat="server"  Text="10/18/2012" />
                                </td>  
                        </tr>
               </table>  

             <asp:Panel ID="pnlTabStrip" runat="server"  CssClass="pageTabStrip" style="margin:0;" width = "220">                
                <telerik:RadTabStrip ID="rtsPrintTurnInMeeting" runat="server" MultiPageID="rmpPrintTurnInMeeting"
                    SelectedIndex="1">
                    <Tabs>                        
                        <telerik:RadTab runat="server" Text="Merch Coord" PageViewID="pvMerchCoord"  Selected="True" Font-Bold="True" />
                        <telerik:RadTab runat="server" Text="Media Coord" PageViewID="pvMediaCoord"  Font-Bold="True" />
                    </Tabs>
                </telerik:RadTabStrip>                
            </asp:Panel>   
                
               <telerik:RadMultiPage ID="rmpPrintTurnInMeeting" SelectedIndex="0" runat="server" Height="100%" >
                <telerik:RadPageView ID="pvMerchCoord" runat="server" Height="100%">
                    <asp:Panel ID="pnlMerchCoord" runat="server" Visible="true" >
                
                <table align="center">                                                
                        <tr>                        
                             <td>                                       
                                    <telerik:RadGrid ID="grdMerchCoord" runat="server" SkinID="CenteredWithScroll" AllowPaging="True"  Height="400" Width="1100"  >
                                                <MasterTableView >
                                                            <EditFormSettings>                   
                                                                    <EditColumn UniqueName="EditCommandColumn1">
                                                                    </EditColumn>
                                                            </EditFormSettings>                                           
                                                                                                                                               
                                                                <NoRecordsTemplate>no records retrieved</NoRecordsTemplate>
                                                                <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                                                                                                
                                                         <Columns>                                                                     
                                                                                                                                                                                                          
                                                            <telerik:GridBoundColumn DataField="PageName" HeaderText="Page Name" UniqueName="PageName" >      
                                                                        <HeaderStyle HorizontalAlign="Center" Width="200"   />
                                                                        <ItemStyle HorizontalAlign="Center" Font-Bold="True"  BackColor="LightGray"  />                            
                                                            </telerik:GridBoundColumn>                                                                             
                                                                                                                        
                                                             <telerik:GridBoundColumn DataField="OfferType" HeaderText="Offer Type" UniqueName="OfferType"  > 
                                                                            <HeaderStyle HorizontalAlign="Center"  />
                                                                            <ItemStyle HorizontalAlign="Center" Font-Bold="True"  BackColor="LightGray" />                                                                                                               
                                                            </telerik:GridBoundColumn>            

                                                              <telerik:GridBoundColumn DataField="OfferName" HeaderText="Offer Name" UniqueName="OfferName"  > 
                                                                            <HeaderStyle HorizontalAlign="Center"  />
                                                                            <ItemStyle HorizontalAlign="Center" Font-Bold="True"  BackColor="LightGray" />                                                                                                               
                                                            </telerik:GridBoundColumn>                                                                                                                                                                                                     
                                                                                                                                                                                                                                                                                                   
                                                            </Columns>    

                                                         <DetailTables >
                                                                    <telerik:GridTableView Name="grdSecondLevel"  AutoGenerateColumns="false" SkinID="CenteredWithScroll" 
                                                                                    HorizontalAlign="Left" ShowFooter="false" AllowSorting="False"   EditMode="InPlace" Width="900">
                                                                      <NoRecordsTemplate>Detail does not exist. </NoRecordsTemplate>
                                                                <Columns>         
                                                                          <telerik:GridTemplateColumn HeaderText="" UniqueName="Select">
                                                                                <HeaderTemplate>
                                                                                    <asp:CheckBox ID="chkAll" runat="server" AllowMultiRowSelection="true" AutoPostBack="true"
                                                                                        Checked="false" ToolTip="Select/Deselect All" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSingleRow" runat="server" AutoPostBack="true" Checked="false"     />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="20px" />
                                                                                <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                                            </telerik:GridTemplateColumn>

                                                                              <telerik:GridButtonColumn ConfirmDialogType="RadWindow" ConfirmTitle="Drill" ButtonType="ImageButton"
                                                                                CommandName="Drill" Text="Drill" UniqueName="DrillColumn" ImageUrl="~/Images/drilldown.gif">
                                                                                <HeaderStyle Width="20" />                            
                                                                            </telerik:GridButtonColumn>
                                                                                                        
                                                                            <telerik:GridEditCommandColumn 
                                                                                            ButtonType="ImageButton"  EditImageUrl="~/Images/Edit1.gif"   CancelImageUrl="~/Images/Cancel1.gif"
                                                                                            UpdateImageUrl="~/Images/CheckMark.gif"   HeaderText="" UniqueName="EditColumn">
                                                                                                <HeaderStyle HorizontalAlign="Center" Width="20" />       
                                                                                                <ItemStyle  HorizontalAlign="Center" Width="20" />                                                                            
                                                                                        </telerik:GridEditCommandColumn>
                                                              
                                                                             <telerik:GridBoundColumn DataField="VendorStyle" HeaderText="Vendor Style" UniqueName="VendorStyle" >      
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="true"   />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>                                                                                               
                                                                            <telerik:GridBoundColumn DataField="StyleDesc" HeaderText="Style Desc" UniqueName="StyleDesc" >      
                                                                                        <HeaderStyle HorizontalAlign="Center"  Width="150" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>                 
                                                                            <telerik:GridBoundColumn DataField="FriendlyColor" HeaderText="Friendly Color" UniqueName="FriendlyColor"  > 
                                                                                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>                                                                                                       
                                                                            <telerik:GridBoundColumn DataField="StylingNotes" HeaderText="Styling Notes" UniqueName="StylingNotes"  >   
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Right" />                            
                                                                            </telerik:GridBoundColumn>    
                                                                                <telerik:GridBoundColumn DataField="RouteFromAd" HeaderText="Route From Ad" UniqueName="RouteFromAd"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Right" />                            
                                                                            </telerik:GridBoundColumn>     
                                                                                <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="Status"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>     
                                                                                 <telerik:GridBoundColumn DataField="StatusDate" HeaderText="StatusDate" UniqueName="StatusDate"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>                                                                                                                                                                                                                                                                                                           
                                                            </Columns>       
                                                </telerik:GridTableView>
                                                    </DetailTables>
                                                                        
                                                </MasterTableView>
                                        </telerik:RadGrid>       
                                       </td>                         
                           
                       </tr>
               </table>

                 </asp:Panel>

                </telerik:RadPageView>

                <telerik:RadPageView ID="pvMediaCoord" runat="server" Height="100%">
                    <asp:Panel ID="pnlMediaCoord" runat="server" Visible="true" >

                      <table align="center">                                                
                        <tr>                        
                             <td>                                       
                                    <telerik:RadGrid ID="grdMediaCoord" runat="server" SkinID="CenteredWithScroll" AllowPaging="True"  Height="400" Width="1400"  >
                                                <MasterTableView>
                                                            <EditFormSettings>                   
                                                                    <EditColumn UniqueName="EditCommandColumn1">
                                                                    </EditColumn>
                                                            </EditFormSettings>                                           
                                                                                                                                               
                                                                <NoRecordsTemplate>no records retrieved</NoRecordsTemplate>
                                                                <FooterStyle HorizontalAlign="Right" Font-Bold="True" />                                                                                                                              
                                                        
                                                         <Columns>                                                                     
                                                                                                                                                                                                          
                                                            <telerik:GridBoundColumn DataField="PageName" HeaderText="Page Name" UniqueName="PageName" >      
                                                                        <HeaderStyle HorizontalAlign="Center" Width="200"   />
                                                                        <ItemStyle HorizontalAlign="Center" Font-Bold="True"  BackColor="LightGray"  />                            
                                                            </telerik:GridBoundColumn>                                                                             
                                                                                                                      
                                                             <telerik:GridBoundColumn DataField="OfferType" HeaderText="Offer Type" UniqueName="OfferType"  > 
                                                                            <HeaderStyle HorizontalAlign="Center"  />
                                                                            <ItemStyle HorizontalAlign="Center" Font-Bold="True"  BackColor="LightGray" />                                                                                                               
                                                            </telerik:GridBoundColumn>            

                                                              <telerik:GridBoundColumn DataField="OfferName" HeaderText="Offer Name" UniqueName="OfferName"  > 
                                                                            <HeaderStyle HorizontalAlign="Center"  />
                                                                            <ItemStyle HorizontalAlign="Center" Font-Bold="True"  BackColor="LightGray" />                                                                                                               
                                                            </telerik:GridBoundColumn>                                                                                                                                                                                                     
                                                                                                                                                                                                                                                                                                   
                                                            </Columns>    

                                                         <DetailTables >
                                                                    <telerik:GridTableView Name="grdSecondLevel"  AutoGenerateColumns="false" SkinID="CenteredWithScroll" 
                                                                                    HorizontalAlign="Left" ShowFooter="false" AllowSorting="False"   EditMode="InPlace" Width="1200">
                                                                      <NoRecordsTemplate>Detail does not exist. </NoRecordsTemplate>
                                                                <Columns>         
                                                                          <telerik:GridTemplateColumn HeaderText="" UniqueName="Select">
                                                                                <HeaderTemplate>
                                                                                    <asp:CheckBox ID="chkAll" runat="server" AllowMultiRowSelection="true" AutoPostBack="true"
                                                                                        Checked="false" ToolTip="Select/Deselect All" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSingleRow" runat="server" AutoPostBack="true" Checked="false"     />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="20px" />
                                                                                <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                                            </telerik:GridTemplateColumn>

                                                                              <telerik:GridButtonColumn ConfirmDialogType="RadWindow" ConfirmTitle="Drill" ButtonType="ImageButton"
                                                                                CommandName="Drill" Text="Drill" UniqueName="DrillColumn" ImageUrl="~/Images/drilldown.gif">
                                                                                <HeaderStyle Width="20" />                            
                                                                            </telerik:GridButtonColumn>
                                                                                                        
                                                                            <telerik:GridEditCommandColumn 
                                                                                            ButtonType="ImageButton"  EditImageUrl="~/Images/Edit1.gif"   CancelImageUrl="~/Images/Cancel1.gif"
                                                                                            UpdateImageUrl="~/Images/CheckMark.gif"   HeaderText="" UniqueName="EditColumn">
                                                                                                <HeaderStyle HorizontalAlign="Center" Width="20" />       
                                                                                                <ItemStyle  HorizontalAlign="Center" Width="20" />                                                                            
                                                                                        </telerik:GridEditCommandColumn>
                                                              
                                                                             <telerik:GridBoundColumn DataField="VendorStyle" HeaderText="Vendor Style" UniqueName="VendorStyle" >      
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="true"   />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>                                                                                               
                                                                            <telerik:GridBoundColumn DataField="StyleDesc" HeaderText="Style Desc" UniqueName="StyleDesc" >      
                                                                                        <HeaderStyle HorizontalAlign="Center"  Width="150" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>                 
                                                                            <telerik:GridBoundColumn DataField="FriendlyColor" HeaderText="Friendly Color" UniqueName="FriendlyColor"  > 
                                                                                            <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>                     
                                                                            <telerik:GridBoundColumn DataField="StylingNotes" HeaderText="Styling Notes" UniqueName="StylingNotes"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>                                                                                 
                                                                                <telerik:GridBoundColumn DataField="OnOff" HeaderText="On/Off" UniqueName="OnOff"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>     
                                                                               <telerik:GridBoundColumn DataField="PU" HeaderText="P/U?" UniqueName="PU"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>   
                                                                                <telerik:GridBoundColumn DataField="PUImageID" HeaderText="P/U Image ID" UniqueName="PUImageID"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="60"  Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Right" />                            
                                                                            </telerik:GridBoundColumn>       
                                                                                 <telerik:GridBoundColumn DataField="ModelCatg" HeaderText="Model Catg" UniqueName="ModelCatg"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="30" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>   
                                                                             <telerik:GridBoundColumn DataField="ModelAge" HeaderText="Model Age" UniqueName="ModelAge"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>   
                                                                             <telerik:GridBoundColumn DataField="NoOfModels" HeaderText="# Of Models" UniqueName="NoOfModels"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="50" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>                                                                           
                                                                            <telerik:GridBoundColumn DataField="ImageName" HeaderText="Image Name" UniqueName="ImageName"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="80"  Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Center" />                            
                                                                            </telerik:GridBoundColumn>     
                                                                                <telerik:GridBoundColumn DataField="Group" HeaderText="Grp" UniqueName="Group"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Right"   />                            
                                                                            </telerik:GridBoundColumn>                                                                                     
                                                                                <telerik:GridBoundColumn DataField="Seq" HeaderText="Seq" UniqueName="Seq"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="60"  Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Right" />                            
                                                                            </telerik:GridBoundColumn>      
                                                                             <telerik:GridBoundColumn DataField="Shot" HeaderText="Shot" UniqueName="Shot"  >       
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="60"  Font-Bold="true" />
                                                                                        <ItemStyle HorizontalAlign="Right" />                            
                                                                            </telerik:GridBoundColumn>                                                                                                                                                                                                                                      
                                                            </Columns>       
                                                </telerik:GridTableView>
                                                    </DetailTables>
                                                                        
                                                </MasterTableView>
                                        </telerik:RadGrid>       
                                       </td>                         
                           
                               </tr>
                       </table>

                      </asp:Panel>

                </telerik:RadPageView>
            </telerik:RadMultiPage>       
                              
                    </asp:Panel>                            
        </telerik:RadPane>
    </telerik:RadSplitter>
 </asp:Content>






