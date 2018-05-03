
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm1.aspx.vb" Inherits="TurnInProcessAutomation.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="rsmJS" runat="server" EnablePageMethods="true">
        <Scripts>            
            <asp:ScriptReference Path="~/Javascript/jquery-1.3.2.min.js" />
            <asp:ScriptReference Path="~/Javascript/jquery-ui-1.7.0.min.js" />
            <asp:ScriptReference Path="~/Javascript/jquery.bgiframe-2.1.1.pack.js" />
            <asp:ScriptReference Path="~/Javascript/jshashtable-2.1.js" />
            <asp:ScriptReference Path="~/Javascript/jquery.numberformatter-1.2.1.js" />            
            <asp:ScriptReference Path="~/Javascript/blockUI.js" />
            <asp:ScriptReference Path="~/Javascript/dymo.label.framework.js" />
            <asp:ScriptReference Path="~/Javascript/PrintersAndMultipleLabelsPrinting.js" />
        </Scripts>
    </telerik:RadScriptManager>
        <button id="printButton">Print</button>
    </form>
</body>
</html>
