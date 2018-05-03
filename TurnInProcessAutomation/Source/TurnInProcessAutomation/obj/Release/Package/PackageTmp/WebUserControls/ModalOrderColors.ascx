<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ModalOrderColors.ascx.vb"
    Inherits="TurnInProcessAutomation.ModalOrderColors" %>
<script type="text/javascript" language="javascript">
    function ShowModalOrderColors() {
        var mPopup = $find('<%= mPopup.ClientID %>')
        var mButton = $get('<%= fakeButton.ClientID %>')
        mButton.click()
    }
    function HideModalOrderColors() {
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
<asp:Panel ID="pnlMessage" runat="server" Height="250px" Width="550px" Style="display: none;
    background-color: AntiqueWhite; text-align: center;">
    <p>Click and drag to change the order.</p>
    <telerik:RadListBox runat="server" ID="rlbColorItems" AutoPostBackOnReorder="false" AllowReorder="True" EnableDragAndDrop="True" Height="150" Width="500" Style="text-align: left;">
        <ButtonSettings ShowReorder="False" />
    </telerik:RadListBox>
    <br />
    <br />
    <asp:LinkButton ID="lnkSave" runat="server" Text="Save" class="label" OnClientClick="HideModalOrderColors()" />
    &nbsp;&nbsp;
    <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" class="label" OnClientClick="HideModalOrderColors();return false;" />
</asp:Panel>
<ajaxtoolkit:ModalPopupExtender ID="mPopup" runat="server" PopupControlID="pnlMessage"
    TargetControlID="fakeButton" BackgroundCssClass="modalProgressGreyBackground"
    DropShadow="false">
</ajaxtoolkit:ModalPopupExtender>
