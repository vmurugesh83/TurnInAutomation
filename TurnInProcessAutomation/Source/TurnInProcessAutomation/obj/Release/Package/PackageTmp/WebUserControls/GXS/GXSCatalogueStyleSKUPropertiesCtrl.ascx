<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GXSCatalogueStyleSKUPropertiesCtrl.ascx.vb" Inherits="TurnInProcessAutomation.GXSCatalogueStyleSKUPropertiesCtrl" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <style type="text/css">
        .tdSS {
            text-align: right;
            font: normal 11px calibri;
            display: inline-block;
            width: 100px;
            height: 20px;
            padding-top: 8px;
        }

        .Div3 {
            text-align: center;
            font: normal 20px calibri;
            display: inline-block;
            width: 200px;
            height: 20px;
            padding-top: 8px;
        }

        div div {
            margin-bottom: 5px;
        }
    </style>
</telerik:RadCodeBlock>
    <div id="SSHeader" class="Div3" runat ="server">
        <span>Style SKU Attributes</span>
    </div>
<div style="display: inline-block;">

    <div id="divFabrication" runat="server">
        <span class="tdSS">Fabrication </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtFabrication" runat="server" SkinID="PopupTextBox"/></span>
    </div>
    <div id="divSellingLoc" runat="server">
        <span class="tdSS">Selling Loc </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtSellingLoc" runat="server" SkinID="PopupTextBox" width="150px"></telerik:RadTextBox></span>
    </div>
    <div id="divProdDtl1" runat="server">
        <span class="tdSS">Prod Dtl 1 </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtProdDtl1" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divProdDtl2" runat="server">
        <span class="tdSS">Prod Dtl 2 </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtProdDtl2" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divProdDtl3" runat="server">
        <span class="tdSS">Prod Dtl 3 </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtProdDtl3" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divAssembledIn" runat="server">
        <span class="tdSS">Assembled In </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtAssembledIn" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divGenClass" runat="server">
        <span class="tdSS">Gen Class </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtGenClass" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divGenSubClass" runat="server">
        <span class="tdSS">Gen SubClass </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtGenSubClass" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divBrand" runat="server">
        <span class="tdSS">Brand </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtBrand" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divLabel" runat="server">
        <span class="tdSS">Label </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtLabel" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divFabDtl" runat="server">
        <span class="tdSS">Fab Dtl </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtFabDtl" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divLifestyle" runat="server">
        <span class="tdSS">Lifestyle </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtLifestyle" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divSeason" runat="server">
        <span class="tdSS">Season </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtSeason" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divOccasion" runat="server">
        <span class="tdSS">Occasion </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtOccasion" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divTheme" runat="server">
        <span class="tdSS">Theme </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtTheme" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>

</div>

<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
    <script type="text/javascript">

    </script>
</telerik:RadCodeBlock>
