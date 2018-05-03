<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.Page"  %>
<script runat="server">

   protected void Page_Init(object sender, EventArgs e)
   {
      PeterBlum.DES.CombinedFilesManager.OutputToPage(Page);
   }
</script>
