<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExportUPCsByCategory.aspx.vb" Inherits="TurnInProcessAutomation.WebForm2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Export UPC information to Excel</title>
</head>

<body>
    <form id="form1" runat="server">
    <div id="Title">
        <h2>Export Turn-in Prioritization results by Web Category.</h2>
    </div>
    <div id="input">
            Status :  <asp:DropDownList ID="ddlStatusDropDown" runat="server" Width="100" AutoPostBack="True"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="ddlTurnInWeek" runat="server" Width="150"></asp:DropDownList>  &nbsp;&nbsp;&nbsp;&nbsp;
            Category : <asp:DropDownList ID="ddlCategoryDropDown" runat="server" Width="300" AutoPostBack="True"></asp:DropDownList>  &nbsp;&nbsp;&nbsp;&nbsp;
            <telerik:RadButton ID="Submit" Text="Submit" runat="server"></telerik:RadButton>
            <telerik:RadButton ID="Clear" Text="Clear" runat="server"></telerik:RadButton>
            <asp:ImageButton ID="ExportToExcelButton" runat="server" ImageUrl="Images/Export.gif" AlternateText="To Excel" />
    </div>
    <div>
        <br />
        <b><asp:Label runat="server" ID="CategoriesLabel"></asp:Label></b>
        <br />
        <b><asp:Label runat="server" ID="ErrorLabel"></asp:Label></b>
    </div>
    <div>
    <br />
    <!-- Script Manager for AJAX enabled Grids -->
    <telerik:RadScriptManager ID="RadScriptManager1" Runat="server"></telerik:RadScriptManager>
    <telerik:RadGrid ID="RadUPCGrid" runat="server" AutoGenerateColumns="False" Width="100%" Height="600px" >
    <MasterTableView EditMode="InPlace" Name="grdUPC" DataKeyNames="ISN,BrandID,StatusFlg,SwatchFlg,ColorFlg,SizeFlg,IsValidFlg,WebCatgyCde,WebCatgyList" 
                    ClientDataKeyNames="ISN,BrandID,StatusFlg,SwatchFlg,ColorFlg,SizeFlg,IsValidFlg" AllowSorting="true">
    <Columns>
    <telerik:GridBoundColumn DataField="Frs" HeaderText="Ad Number" UniqueName="AdNum" ReadOnly="true" />
    <telerik:GridTemplateColumn HeaderText="Web Categories" UniqueName="WebCategories" >
       <ItemTemplate>
          <asp:Literal runat="server" ID="ltrPrimaryWebCategory"></asp:Literal>
       </ItemTemplate>
    </telerik:GridTemplateColumn>
    <telerik:GridBoundColumn DataField="EMMNotes" HeaderText="EMM Notes" UniqueName="EMMNotes" ReadOnly="true" ItemStyle-Width="100px" />     
    <telerik:GridBoundColumn DataField="ProductName" HeaderText="Product Name" UniqueName="ProductName" ReadOnly="true" ItemStyle-Width="100px"  />
    <telerik:GridBoundColumn DataField="VendorStyleNumber" HeaderText="Vendor Style #" UniqueName="VendorStyleNumber" ReadOnly="true" />
    <telerik:GridBoundColumn DataField="UPC" HeaderText="UPC #" UniqueName="UPC" ReadOnly="true" />
    <telerik:GridTemplateColumn HeaderText="Short Copy" UniqueName="ShortCopy" ReadOnly="True">
    <ItemTemplate>    X    </ItemTemplate>
    </telerik:GridTemplateColumn>
    <telerik:GridBoundColumn DataField="MerchantNotes" HeaderText="Long Copy" UniqueName="MerchantNotes" ReadOnly="true" ItemStyle-Width="100px" />
    <telerik:GridTemplateColumn UniqueName="Keywords" HeaderText="Keywords" ReadOnly="True">
    <ItemTemplate> </ItemTemplate>
    </telerik:GridTemplateColumn>
    <telerik:GridBoundColumn DataField="FriendlyColor" HeaderText="Color" UniqueName="WebCatStatus" ReadOnly="true" />
    <telerik:GridBoundColumn DataField="WebCatSizeDesc" HeaderText="Size" UniqueName="WebCatStatus" ReadOnly="true" />
    <telerik:GridBoundColumn DataField="ImageNotes" HeaderText="Admin Notes" UniqueName="ImageNotes" ReadOnly="true" ItemStyle-Width="100px" />
    <telerik:GridBoundColumn DataField="ImageID" HeaderText="Image ID" UniqueName="ImageID" ReadOnly="true" />
    <telerik:GridBoundColumn DataField="FeatureID" HeaderText="Feature ID" UniqueName="FeatureID" ReadOnly="true" />
    <telerik:GridBoundColumn DataField="DropShipID" HeaderText="Drop-Ship Distributor" UniqueName="DropShipID" ReadOnly="true" />
    <telerik:GridBoundColumn DataField="IntReturnInstrct" HeaderText="Internal Return Instructions" UniqueName="IntReturnInstrct" ReadOnly="true" />
    <telerik:GridBoundColumn DataField="ExtReturnInstrct" HeaderText="External Return Instructions" UniqueName="ExtReturnInstrct" ReadOnly="true" />
    <telerik:GridBoundColumn DataField="BrandDesc" HeaderText="Brand" UniqueName="BrandDesc" ReadOnly="true" />
    <telerik:GridBoundColumn DataField="ColorFamily" HeaderText="Color Family" UniqueName="ColorFamily" ReadOnly="true" />
    <telerik:GridBoundColumn DataField="SizeFamily" HeaderText="Size Family" UniqueName="SizeFamily" ReadOnly="true" />
    <telerik:GridBoundColumn DataField="AgeDesc" HeaderText="Age" UniqueName="AgeDesc" ReadOnly="true" />
    <telerik:GridBoundColumn DataField="GenderDesc" HeaderText="Gender" UniqueName="GenderDesc" ReadOnly="true" />
    </Columns>
    </MasterTableView>
    <ClientSettings>
        <Scrolling AllowScroll="True"></Scrolling>
    </ClientSettings>
    </telerik:RadGrid>
    </div>
    </form>
</body>
</html>
