<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ModalPopupControl.ascx.vb" Inherits="TurnInProcessAutomation.ModalPopupControl" %>
<script type="text/javascript" language="javascript">
    function ShowModalMessage() {
        var mPopup = $find('<%= mPopup.ClientID %>')
        var mButton = $get('<%= fakeButton.ClientID %>')
        mButton.click()
    }
    function HideModalMessage() {
        var mPopup = $find('<%= mPopup.ClientID %>')
        mPopup.hide()
    }
</script>

<style>
.modalProgressGreyBackground
{
	background-color: Gray;
	filter: alpha(opacity=70);
	opacity: 0.7;	
}
</style>
<asp:Button ID="fakeButton" runat="server" Style="display: none" Visible="true" />

<asp:Panel ID="pnlMessage" runat="server"  Height="150px"
    Width="300px" Style="display: none; background-color: AntiqueWhite; text-align: center;">
    <br />
    <asp:Label ID="lblProgressMessage" runat="server" class="label" Text="Choose your export format" Font-Underline="True" />
    <br />
    <br />
    <asp:RadioButtonList ID="rblExport" class="label" runat="server" RepeatDirection="Horizontal">
        <asp:ListItem Text="Excel" Value="Excel" Selected="True" />
        <asp:ListItem Text="PDF" Value="PDF" />
    </asp:RadioButtonList>
    <br />
    <asp:LinkButton ID="lnkOK" runat="server" Text="OK" class="label" OnClientClick="HideModalMessage()" />
    &nbsp;&nbsp;
    <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" class="label" OnClientClick="HideModalMessage();return false;"/>
</asp:Panel>

<ajaxtoolkit:ModalPopupExtender ID="mPopup" runat="server" PopupControlID="pnlMessage" TargetControlID="fakeButton"
    BackgroundCssClass="modalProgressGreyBackground" DropShadow="false">
</ajaxtoolkit:ModalPopupExtender>

