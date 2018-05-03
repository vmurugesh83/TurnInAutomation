<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ModalMoveBatch.ascx.vb"
    Inherits="TurnInProcessAutomation.ModalMoveBatch" %>
<script type="text/javascript" language="javascript">
    function ShowModalMoveBatch() {
        var mPopup = $find('<%= mPopup.ClientID %>');
        var mButton = $get('<%= fakeButton.ClientID %>');
        mButton.click();
    }
    function HideModalMoveBatch() {
        var mPopup = $find('<%= mPopup.ClientID %>');
        mPopup.hide();
    }
</script>
<style type="text/css">
    .modalProgressGreyBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
        z-index: 1000 !important;
    }
</style>
<asp:Button ID="fakeButton" runat="server" Style="display: none" Visible="true" />
<asp:Panel ID="pnlMessage" runat="server" Height="250px" Width="550px" Style="display: none;
    background-color: AntiqueWhite; text-align: center;">
    <h2>Move Batch</h2>
    <p><b>Batch Number:</b>
    <asp:Label runat="server" ID="lblBatchNum" /><br />
    <b>Current Ad#/Turn-in Week:</b>
    <asp:Label runat="server" ID="lblAd" /> - <asp:Label runat="server" ID="lblWeek" /></p><br />
    Move Batch to:
    <telerik:RadComboBox ID="rcbAds" runat="server" OnItemsRequested="rcbAds_ItemsRequested"
        OnClientBlur="OnClientBlurHandler" EnableLoadOnDemand="true"
        ShowMoreResultsBox="false" EnableVirtualScrolling="false" DropDownWidth="300"
        Width="150" HighlightTemplatedItems="true" Filter="Contains" Style="z-index: 12345">
        <HeaderTemplate>
            <table style="width: 300px" cellspacing="0" cellpadding="0">
                <tr>
                    <td style="width: 50px;">
                        Ad #
                    </td>
                    <td style="width: 300px;">
                        Description
                    </td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <table style="width: 350px" cellspacing="0" cellpadding="0">
                <tr>
                    <td style="width: 50px;">
                        <%# Eval("AdNbr")%>
                    </td>
                    <td style="width: 300px; text-align: left">
                        <%# Eval("AdDesc")%>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </telerik:RadComboBox>
    <br />
    <br />
    <asp:LinkButton ID="lnkSave" runat="server" Text="Save" class="label" OnClientClick="HideModalMoveBatch()" />
    &nbsp;&nbsp;
    <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" class="label" OnClientClick="HideModalMoveBatch();return false;" />
</asp:Panel>
<ajaxtoolkit:ModalPopupExtender ID="mPopup" runat="server" PopupControlID="pnlMessage"
    TargetControlID="fakeButton" BackgroundCssClass="modalProgressGreyBackground"
    DropShadow="false">
</ajaxtoolkit:ModalPopupExtender>
