<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GXSCatalogue.aspx.vb" Inherits="TurnInProcessAutomation.GXSCatalogue" MasterPageFile="~/PopupPage.Master" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/PopupPage.Master" %>

<%@ Register Src="~/WebUserControls/GXS/GXSImageViewerCtrl.ascx" TagPrefix="bonton" TagName="GXSImageViewerCtrl" %>

<asp:Content ID="GXSCatalogForm" ContentPlaceHolderID="ContentArea" runat="Server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style type="text/css">
            ::-ms-clear {
                display: none;
            }

            .tdFiller {
                padding-left: 10px;
                padding-right: 10px;
            }
        </style>
    </telerik:RadCodeBlock>
    <telerik:RadToolBar ID="rtbGXSCopyView" runat="server" OnClientButtonClicking="OnClientButtonClicking"
        OnClientLoad="clientLoad" CssClass="SeparatedButtons">
        <Items>
            <telerik:RadToolBarButton runat="server" CommandName="Save" DisabledImageUrl="~/Images/Save_d.gif"
                ImageUrl="~/Images/Save.gif" Text="Save" CssClass="rightAligned">
            </telerik:RadToolBarButton>
        </Items>
    </telerik:RadToolBar>
    <div id="divMessage" runat="server" style="text-align: right; height: 20px;">
    </div>
    <div style="margin: 60px 20px 40px 20px; padding-top: 20px; padding-left: 20px; border: 1px solid black;">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDescription" runat="server" Text="" Font-Bold="true" Font-Size="13pt" Font-Names="Calibri"></asp:Label></td>
                <td rowspan="9">
                    <telerik:RadTextBox ID="txtWebCatCopy" runat="server" Width="400px" TextMode="MultiLine" Rows="20" BorderColor="Black" DisplayText="Web Cat Product Copy (Phase II)" Visible="False"></telerik:RadTextBox></td>
                <td rowspan="9">
                    <bonton:GXSImageViewerCtrl runat="server" ID="GXSImageViewerCtrl1" />
                </td>
            </tr>
            <tr>
                <td style="font: normal 8pt calibri;">Mouse over will provide the name in the GXS Catalogue</td>
            </tr>
            <tr>
                <td style="font: bold 11pt calibri;">Available Colors</td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="litAvailColors" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td style="font: bold 11pt calibri;">Available Sizes</td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="litAvailSizes" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td style="font: bold 11pt calibri;">Product Details / Copy</td>
            </tr>
            <tr>
                <td style="padding-right: 80px;">
                    <telerik:RadTextBox ID="txtProductDetails" runat="server" Width="438px" TextMode="MultiLine" Rows="6" BorderColor="Black"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td style="padding-top: 10px;">
                    <br />
                </td>
                <td>&nbsp;</td>
                <td>
                    <asp:Literal ID="litCatUrl" runat="server"></asp:Literal></td>

            </tr>
        </table>
        <table>
            <colgroup>
                <col style="width: 280px;" />
                <col style="width: 960px;" />
            </colgroup>
            <tr>
                <td style="vertical-align: top;">
                    <asp:PlaceHolder ID="phStyleSKUProperties" runat="server"></asp:PlaceHolder>
                </td>
                <td style="vertical-align: top;">
                    <asp:PlaceHolder ID="phExtendedProperties" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </table>
    </div>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <script type="text/javascript">
            window.moveTo(0, 0);
            window.resizeTo(1350, screen.height);

            function OnKeyPress(sender, args) {
                args.set_cancel(true);
            }

            function RadTextBox_OnFocus(sender, args) {
                var txt = sender;
                var input = txt.get_element();
                if (input)
                    input.setSelectionRange(0, input.value.length);
            }

            function SetMessage(message, color, duration) {
                var div = $get("<%= Me.divMessage.ClientID %>");
                div.innerHTML = message;
                div.style.color = color;
                if (duration > 0) {
                    setTimeout(function () {
                        div.innerHTML = '';
                    }, duration * 1000);
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
