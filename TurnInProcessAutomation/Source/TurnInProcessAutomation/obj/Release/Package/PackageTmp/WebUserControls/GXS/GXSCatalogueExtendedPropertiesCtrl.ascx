<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GXSCatalogueExtendedPropertiesCtrl.ascx.vb" Inherits="TurnInProcessAutomation.GXSCatalogueExtendedPropertiesCtrl" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <style type="text/css">
        .tdGXS {
            text-align: right;
            font: normal 11px calibri;
            display: inline-block;
            width: 225px;
            height: 20px;
            padding-top: 8px;
        }
        .tdGXSBlue {
            background-color: #b0c4de;
        }
        .Div2 {
            text-align: center;
            font: normal 20px calibri;
            display: inline-block;
            width: 450px;
            height: 20px;
            padding-top: 8px;
        }

        div div {
            margin-bottom: 5px;
        }
    </style>
</telerik:RadCodeBlock>
<div id="SSHeader" runat ="server">
        <span class="Div2">GXS External Attributes</span>
    </div>
<div style="display: inline-block;">
        
    <div id="divVendorStyle" runat="server">
        <span id="spVendorStyle" runat="server">Product (Vendor Style) </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtVendorStyle" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divFullProductName" runat="server">
        <span id="spFullProductName" runat="server">Full Product Name </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtFullProductName" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divUPC" runat="server">
        <span id="spUPC" runat="server">UPC </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtUPC" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divNRFColorCode" runat="server">
        <span id="spNRFColorCode" runat="server">NRF Color Code </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtNRFColorCode" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divColorDescription" runat="server">
        <span id="spColorDescription" runat="server">Color Description </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtColorDescription" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divNRFSizeCode" runat="server">
        <span id="spNRFSizeCode" runat="server">NRF Size Code </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtNRFSizeCode" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divSizeDescription" runat="server">
        <span id="spSizeDescription" runat="server">Size Description </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtSizeDescription" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divBrandName" runat="server">
        <span id="spBrandName" runat="server">Brand Name </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtBrandName" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divVendorCollectionName" runat="server">
        <span id="spVendorCollectionName" runat="server">Vendor Collection Name </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtVendorCollectionName" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divTeamName" runat="server">
        <span id="spTeamName" runat="server">Team Name </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtTeamName" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divConsumerQtyOfUnitsInPkg" runat="server">
        <span id="spConsumerQtyOfUnitsInPkg" runat="server">Consumer Qty of Units in Pkg </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtConsumerQtyOfUnitsInPkg" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divCountryOfOrigin" runat="server">
        <span id="spCountryOfOrigin" runat="server">Country of Origin </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtCountryOfOrigin" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divMarketingMessage" runat="server">
        <span id="spMarketingMessage" runat="server">Features - Benefits - Marketing Message </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtMarketingMessage" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divFabricOrMaterialDescription" runat="server">
        <span id="spFabricOrMaterialDescription" runat="server">Fabric or Material Description </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtFabricOrMaterialDescription" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
    <div id="divLiningMaterial" runat="server">
        <span id="spLiningMaterials" runat="server">Lining Material </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtLiningMaterial" runat="server" SkinID="PopupTextBoxWide"></telerik:RadTextBox></span>
    </div>
</div>
<div style="display: inline-block; vertical-align: top;">
    <div id="Div2" runat ="server">
        <span class="tdSS"></span>
    </div>
    <div id="divGoldCarat" runat="server">
        <span id="spGoldCarat" runat="server">Gold Karat </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtGoldCarat" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divStoneDetails" runat="server">
        <span id="spStoneDetails" runat="server">Stone Details </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtStoneDetails" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divClosure" runat="server">
        <span id="spClosure" runat="server">Closure </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtClosure" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divCareInfo" runat="server">
        <span id="spCareInfo" runat="server">Care Info </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtCareInfo" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divConsumerItemLength" runat="server">
        <span id="spConsumerItemLength" runat="server">Consumer Item Length </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtConsumerItemLength" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divConsumerItemWidth" runat="server">
        <span id="spConsumerItemWidth" runat="server">Consumer Item Width </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtConsumerItemWidth" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divConsumerItemDepth" runat="server">
        <span id="spConsumerItemDepth" runat="server">Consumer Item Depth </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtConsumerItemDepth" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divConsumerItemHeight" runat="server">
        <span id="spConsumerItemHeight" runat="server">Consumer Item Height </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtConsumerItemHeight" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divHeelHeight" runat="server">
        <span id="spHeelHeight" runat="server">Heel Height </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtHeelHeight" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divPlatformHeight" runat="server">
        <span id="spPlatformHeight" runat="server">Platform Height </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtPlatformHeight" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divSoleType" runat="server">
        <span id="spSoleType" runat="server">Sole Type </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtSoleType" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divBootLegCircumference" runat="server">
        <span id="spBootLegCircumference" runat="server">Boot Leg Circumference </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtBootLegCircumference" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divBootShaftHeight" runat="server">
        <span id="spBootShaftHeight" runat="server">Boot Shaft Height </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtBootShaftHeight" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>    
    <div id="divEarringsDrop" runat="server">
        <span id="spEarringsDrop" runat="server">Earrings Drop </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtEarringsDrop" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divWatchBandWidth" runat="server">
        <span id="spWatchBandWidth" runat="server">Watch Band Width </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtWatchBandWidth" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divWatchCaseSize" runat="server">
        <span id="spWatchCaseSize" runat="server">Watch Case Size </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtWatchCaseSize" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divHandbagShoulderDrop" runat="server">
        <span id="spHandbagShoulderDrop" runat="server">Handbag Shoulder Drop </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtHandbagShoulderDrop" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divConsumerPackageDepth" runat="server">
        <span id="spConsumerPackageDepth" runat="server">Consumer Package Depth </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtConsumerPackageDepth" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divConsumerPackageHeight" runat="server">
        <span id="spConsumerPackageHeight" runat="server">Consumer Package Height </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtConsumerPackageHeight" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divConsumerPackageWidth" runat="server">
        <span id="spConsumerPackageWidth" runat="server">Consumer Package Width </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtConsumerPackageWidth" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divConsumerPackageGrossWeight" runat="server">
        <span id="spConsumerPackageGrossWeight" runat="server">Consumer Package Gross Weight </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtConsumerPackageGrossWeight" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divClosure2" runat="server">
        <span id="spClosure2" runat="server">Closure </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtClosure2" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divCollarType" runat="server">
        <span id="spCollarType" runat="server">Collar Type </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtCollarType" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divPantInseamLength" runat="server">
        <span id="spPantInseamLength" runat="server">Pant Inseam Length </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtPantInseamLength" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divSleeveMeasurement" runat="server">
        <span id="spSleeveMeasurement" runat="server">Sleeve Measurement </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtSleeveMeasurement" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divSleeveType" runat="server">
        <span id="spSleeveType" runat="server">Sleeve Type </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtSleeveType" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divCareInfo2" runat="server">
        <span id="spCareInfo2" runat="server">Care Info </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtCareInfo2" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divWarrantyDescription" runat="server">
        <span id="spWarrantyDescription" runat="server">Warranty Description </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtWarrantyDescription" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divConsumerProductCapacity" runat="server">
        <span id="spConsumerProductCapacity" runat="server">Consumer Product Capacity or Volume </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtConsumerProductCapacity" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divKeyActiveIngredient" runat="server">
        <span id="spKeyActiveIngredient" runat="server">Key Active Ingredient </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtKeyActiveIngredient" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divDoesNotContain" runat="server">
        <span id="spDoesNotContain" runat="server">Does not contain </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtDoesNotContain" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divAerosolProduct" runat="server">
        <span id="spAerosolProduct" runat="server">Aerosol Product </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtAerosolProduct" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divClosure3" runat="server">
        <span id="spClosure3" runat="server">Closure </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtClosure3" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divFauxFur" runat="server">
        <span id="spFauxFur" runat="server">Faux Fur </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtFauxFur" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divFurAnimalName" runat="server">
        <span id="spFurAnimalName" runat="server">Fur Animal Name </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtFurAnimalName" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divFurCountryOfOrigin" runat="server">
        <span id="spFurCountryOfOrigin" runat="server">Fur Country of Origin </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtFurCountryOfOrigin" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
    <div id="divFurTreatment" runat="server">
        <span id="spFurTreatment" runat="server">Fur Treatment </span><span class="tdFiller">
            <telerik:RadTextBox ID="txtFurTreatment" runat="server" SkinID="PopupTextBox"></telerik:RadTextBox></span>
    </div>
</div>
<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
    <script type="text/javascript">

    </script>
</telerik:RadCodeBlock>
