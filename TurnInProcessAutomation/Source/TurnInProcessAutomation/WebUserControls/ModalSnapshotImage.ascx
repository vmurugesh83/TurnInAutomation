<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ModalSnapshotImage.ascx.vb" Inherits="TurnInProcessAutomation.ModalSnapshotImage" %>
<script type="text/javascript">
    function ShowSnapshotModalMessage(snapshotImageURL) {
        var mPopup = $find('<%= mSnapshotPopup.ClientID%>');
        var mButton = $get('<%= fakeButton.ClientID %>');
        $("#imgSnapshot").attr("src", snapshotImageURL);
        InitZoom();
        mButton.click();
    }
    function HideModalMessage() {
        var mPopup = $find('<%= mSnapshotPopup.ClientID%>');
        mPopup.hide();
        //Reset image size
        img.e.style.width = "800px";
        img.e.style.height = "780px";
        $("#imgSnapshot").attr("src", '');
    }

    var aspectRatio;
    var img;

    function InitZoom() {

        img = {};
        img.e = document.getElementById("imgSnapshot");

        if (img.e.naturalWidth) {
            img.nw = img.e.naturalWidth;
        } else {
            var i = new Image();
            i.src = img.e.src;
            img.nw = i.width;
        }

        //Prevent mouse wheel scrolling
        $("#divSnapshot").bind("mousewheel", function (event, delta) {
            return false;
        });

    }

    function zoom(factor) {
        $('#imgSnapshot').css({
            height: 780 * factor,
            width: 800 * factor,
            left: 0,
            top: 0
        });
        return false;
    }

</script>

<style>
    .modalProgressGreyBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }
</style>
<asp:Button ID="fakeButton" runat="server" Style="display: none" Visible="true" />

<asp:Panel ID="pnlMessage" runat="server" Height="783px"
    Width="803px" Style="display: none; background-color: AntiqueWhite; text-align: center;">
    <div id="divSnapshot" style="height: 100%; width: 100%; overflow: auto;">
        <img id="imgSnapshot" src="Snapshot" alt="Snapshot Image" width="800" height="780" />
    </div>
    <div style="background-color: AntiqueWhite;">
        <asp:LinkButton ID="lnkClose" runat="server" Text="Close" class="label" OnClientClick="HideModalMessage(); return false;" />
    </div>
    <div style="position: absolute; left: 700px; top: 785px;"><a href="#" onclick="zoom(1); return false;">x1</a>&nbsp;<a href="#" onclick="zoom(2); return false;">x2</a>&nbsp;<a href="#" onclick="zoom(3); return false;">x3</a>&nbsp;<a href="#" onclick="zoom(4); return false;">x4</a></div>
</asp:Panel>

<ajaxtoolkit:ModalPopupExtender ID="mSnapshotPopup" runat="server" PopupControlID="pnlMessage" TargetControlID="fakeButton"
    BackgroundCssClass="modalProgressGreyBackground" DropShadow="false">
</ajaxtoolkit:ModalPopupExtender>

