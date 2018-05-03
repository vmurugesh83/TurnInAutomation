<%@ Page Language="VB" AutoEventWireup="false" Inherits="TurnInProcessAutomation._Default"
    CodeBehind="Default.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Merchandise Turn In System</title>
    
    <script language='Javascript'>
        function mycontextmenu() 
        {
            alert('Right click is disabled. Use File Menu at the top.');
            return false;
        } 
        document.oncontextmenu = mycontextmenu;
            
    </script>
                 
</head>
<body>
    <form id="HomePageForm" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="appHeader">
     <asp:Label id="lblEnvironmentLeft" runat="server" ></asp:Label>
       &nbsp;&nbsp;&nbsp;Merchandise Turn In System &nbsp;&nbsp;&nbsp;
     <asp:Label id="lblEnvironmentRight" runat="server" ></asp:Label>   
        </div>
    <div id="appMenu">
        <bonton:ApplicationMenu ID="TurnInProcessAutomationMenu" runat="server" />
       
        <asp:LoginView ID="LoginView1" runat="server">
        <LoggedInTemplate>
        <span style="padding: 4px; color:White; float: right; font-weight: bold">
        Logged in as: <asp:LoginName ID="LoginName1" runat="server" /></span>
        </LoggedInTemplate>
        <AnonymousTemplate>
            <span style="color:White; float: right; font-weight: bold;"><asp:LoginStatus ID="LoginStatus1" runat="server" /></span>
        </AnonymousTemplate>
        </asp:LoginView>          
               
    </div>
    
       <div style="height:450px;text-align:center;">
           <asp:Image ID="imgHomePageLogo" runat="server" ImageUrl="Images/Bonton_Stores_Logo.jpg" style="margin-top:200px" />
       </div>
    
    
    <div id="appFooter">
        For technical issues with this web site, please contact the <b>Bon Ton Help Desk</b> at
        <b>1-800-585-7209</b>
    </div>
    </form>
</body>
</html>
