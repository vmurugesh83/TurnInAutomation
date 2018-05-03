<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InvalidAccess.aspx.vb" Inherits="TurnInProcessAutomation.InvalidAccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invalid Access</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Invalid Access. Please close the window and try again.
        </div>
    </form>
</body>

<script language="javascript" type="text/javascript">
    alert("You have opened this webpage in a new tab or a new window, this window will close by the system.");
    window.close();
</script>

</html>
