<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GXSCopyViewCtrl.ascx.vb"
    Inherits="TurnInProcessAutomation.GXSCopyViewCtrl" %>

<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="grdGXSCopyView">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdGXSCopyView" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="rtbGXSCopyView">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdGXSCopyView" />
                <telerik:AjaxUpdatedControl ControlID="MessagePanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>

<telerik:RadSplitter ID="rsGXSCopyView" runat="server" SkinID="pageSplitter">
    <telerik:RadPane ID="rpHeader" runat="server" Height="70" Scrolling="None" Font-Bold="True">
        <div id="pageActionBar">
            <telerik:RadToolBar ID="rtbGXSCopyView" runat="server" OnClientButtonClicking="OnClientButtonClicking"
                OnClientLoad="clientLoad" CssClass="SeparatedButtons">
                <Items>
                    <telerik:RadToolBarButton runat="server" CommandName="Back" DisabledImageUrl="~/Images/BackButton.gif"
                        ImageUrl="~/Images/BackButton.gif" Text="Back">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton runat="server" CommandName="Reset" DisabledImageUrl="~/Images/Reset_d.gif"
                        ImageUrl="~/Images/Reset.gif" Text="Reset" CssClass="rightAligned">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton runat="server" CommandName="Retrieve" DisabledImageUrl="~/Images/Retrieve1_d.gif"
                        ImageUrl="~/Images/Retrieve1.gif" Text="Retrieve" CssClass="rightAligned">
                    </telerik:RadToolBarButton>
                </Items>
            </telerik:RadToolBar>
        </div>
        <div id="pageHeader">
            <asp:Label ID="lblPageHeader" runat="server" Text="GXS Copy View" />
            <bonton:MessagePanel ID="MessagePanel1" runat="server" />
        </div>
    </telerik:RadPane>
    <telerik:RadPane ID="rpContent" runat="server">
        <table class="labels-vertical" style="margin: 20px; width: 700px;">
            <tr>
                <td colspan="6" style="text-align: left;">
                    <div id="divFScr" style="width: 100%; height: 100%; float: left;">
                        <telerik:RadGrid ID="grdGXSCopyView" runat="server" SkinID="CenteredWithScroll" AllowPaging="True"
                            Height="100%" Width="100%" ShowFooter="false" Visible="false">
                            <MasterTableView DataKeyNames="INTERNAL_STYLE_NUM,PO_STARTSHIPDT,FEATUREDCOLOR,COLOR" TableLayout="Fixed" Width="1310px">
                                <NoRecordsTemplate>
                                    no records retrieved
                                </NoRecordsTemplate>
                                <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="IMAGE_ID " HeaderText="ImageId">
                                        <HeaderStyle Width="90px" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="INTERNAL_STYLE_NUM " HeaderText="ISN" UniqueName="INTERNAL_STYLE_NUM">
                                        <HeaderStyle Width="90px" HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="LABEL" HeaderText="Label">
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="PRODUCT_NAME" HeaderText="Product Name">
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="OO" HeaderText="OO Qty">
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="OH" HeaderText="OH Qty">
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="PO_STARTSHIPDT" HeaderText="PO Start Ship">
                                        <HeaderStyle Width="90px" HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="PRICESTATUS" HeaderText="Price Status">
                                        <HeaderStyle Width="90px" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FEATUREDCOLOR" HeaderText="Featured Color">
                                        <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="COLOR" HeaderText="Color">
                                        <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <DetailTables>
                                    <telerik:GridTableView Name="UPC" AutoGenerateColumns="false" HorizontalAlign="Left" TableLayout="Fixed" Width="980px"
                                        ShowFooter="false" AllowSorting="False" DataKeyNames="INTERNAL_STYLE_NUM,COLOR">
                                        <ParentTableRelation>
                                            <telerik:GridRelationFields MasterKeyField="INTERNAL_STYLE_NUM" DetailKeyField="INTERNAL_STYLE_NUM" />
                                        </ParentTableRelation>
                                        <NoRecordsTemplate>
                                            Detail does not exist.
                                        </NoRecordsTemplate>
                                        <Columns>
                                            <telerik:GridTemplateColumn>
                                                <HeaderStyle Width="75px" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <HeaderStyle Width="77px" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="INTERNAL_STYLE_NUM " HeaderText="ISN" UniqueName="INTERNAL_STYLE_NUM" Visible="false">
                                                <HeaderStyle Width="90px" HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridHyperLinkColumn DataTextField="UPC_NUM" HeaderText="UPC Num" Target="_blank" UniqueName="UPC_NUM"
                                                DataNavigateUrlFormatString="~\Webforms\GXS\GXSCatalogue.aspx?upc={0}&isn={1}&imageId={2}" DataNavigateUrlFields="UPC_NUM,INTERNAL_STYLE_NUM,IMAGE_ID">
                                                <HeaderStyle Width="168px" HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </telerik:GridHyperLinkColumn>
                                            <telerik:GridBoundColumn DataField="FEATURE" HeaderText="Feature">
                                                <HeaderStyle Width="170px" HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="COLOR" HeaderText="Color">
                                                <HeaderStyle Width="170px" HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SIZE" HeaderText="Size">
                                                <HeaderStyle Width="170px" HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </telerik:GridTableView>
                                </DetailTables>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </td>
            </tr>
        </table>
    </telerik:RadPane>
</telerik:RadSplitter>

<telerik:RadCodeBlock runat="server" ID="rcb2">
    <script type="text/javascript">

        $(document).ready(function () {
            ResizeWinDelay(0);
        });

        window.onresize = function () {
            ResizeWinDelay(200);
        }

        var tOutResize;
        function ResizeWinDelay(timeOut) {
            if (tOutResize != null)
                window.clearTimeout(tOutResize);
            tOutResize = setTimeout(function () { ResizeWin(); tOutResize = null; }, timeOut)
        }

        function ResizeWin() {
            resize(document.getElementById('divFScr'));

            var grid = $find("<%= grdGXSCopyView.ClientID %>");
            if (grid)
                grid.repaint();
        }

        function resize(element) {

            if (element != null) {
                var height = document.body.clientHeight - 1;
                var width = document.body.clientWidth - 1;

                var parent = element;
                while (parent.tagName != 'FORM') {
                    height -= parseInt(parent.offsetTop, 10);
                    width -= parseInt(parent.offsetLeft, 10);

                    parent = parent.parentNode;
                }

                element.style.height = height + 'px';
                element.style.width = width + 'px';
            }
        }

    </script>
</telerik:RadCodeBlock>

