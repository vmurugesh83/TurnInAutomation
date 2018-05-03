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
        //Scaling Factor
        img.zoom = 30;

        //Get aspect ratio
        if (!aspectRatio) {
            aspectRatio = img.e.height / img.e.width;
        }

        if (img.e.addEventListener) {
            img.e.addEventListener("mousewheel", MouseWheelHandler, false);
        }

        function MouseWheelHandler(e) {

            var delta = Math.max(-1, Math.min(1, e.wheelDelta));
            img.e.style.width = Math.max(800, Math.min(img.nw * 4, img.e.width + (img.zoom * delta))) + "px";
            img.e.style.height = img.e.style.width.replace("px", "") * aspectRatio + "px";
            return false;

        }

        //Prevent mouse wheel scrolling
        $("#divSnapshot").bind("mousewheel", function (event, delta) {
            return false;
        });

    }

    function onRightClick() {

        $('#imgSnapshot').css({
            height: 780,
            width: 800,
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
    <div id="divSnapshot" style="height: 100%; width: 100%; overflow: auto;" oncontextmenu="onRightClick(); return false;">
        <img id="imgSnapshot" src="Snapshot" alt="Snapshot Image" width="800" height="780" />
    </div>
    <div style="background-color: AntiqueWhite;">
        <asp:LinkButton ID="lnkClose" runat="server" Text="Close" class="label" OnClientClick="HideModalMessage(); return false;" />
    </div>
</asp:Panel>

<ajaxtoolkit:ModalPopupExtender ID="mSnapshotPopup" runat="server" PopupControlID="pnlMessage" TargetControlID="fakeButton"
    BackgroundCssClass="modalProgressGreyBackground" DropShadow="false">
</ajaxtoolkit:ModalPopupExtender>

