<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GXSImageViewerCtrl.ascx.vb" Inherits="TurnInProcessAutomation.GXSImageViewerCtrl" %>

<telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
    <style type="text/css">
        .itemTemplate {
            /*width: 43px; // 11px margin is taken into account*/
            height: 32px;
        }
    </style>
</telerik:RadScriptBlock>
<div>
    <table style="height: 100%; width: 100%;">
        <tr>
            <td colspan="3" style="text-align: center;">
                <asp:Image ID="Image1" runat="server" Width="225px" ImageUrl="" /></td>
        </tr>
        <tr>
            <td>
                <asp:Image ImageUrl="~/WebUserControls/GXS/Images/previousSlide.gif" ID="img_left" AlternateText="left" runat="server" />
            </td>
            <td>
                <div style="margin-top: 10px;"></div>
                <telerik:RadRotator ID="RadRotator1" runat="server" DataSourceID="XmlDataSource1" RenderMode="Lightweight"
                    Width="215px" Height="100px" ItemHeight="32px" FrameDuration="2000" ScrollDuration="2000" RotatorType="Buttons">
                    
                    <ItemTemplate>
                        <div class="itemTemplate">
                            <img src="<%# XPath("SmallURL") %>" alt="" style="height: 100px; width: 100px;" onclick="SetImage('<%# XPath("LargeURL") %>');" />
                        </div>
                    </ItemTemplate>
                    <ControlButtons LeftButtonID="img_left" RightButtonID="img_right"></ControlButtons>
                </telerik:RadRotator>
                <asp:XmlDataSource ID="XmlDataSource1" runat="server" EnableCaching="false"></asp:XmlDataSource>
            </td>
            <td>
                <asp:Image ImageUrl="~/WebUserControls/GXS/Images/nextSlide.gif" ID="img_right" AlternateText="right" runat="server" />
            </td>
        </tr>
    </table>
</div>
<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
    <script type="text/javascript">

        function SetImage(url) {
            var img = $get("<%= Me.Image1.ClientID %>");
            //alert(url);
            img.src = url;
        }

    </script>
</telerik:RadCodeBlock>
