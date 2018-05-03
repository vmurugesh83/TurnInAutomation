<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ContentPage.Master" CodeBehind="CopyPrioritization.aspx.vb" Inherits="TurnInProcessAutomation.CopyPrioritization" %>

<%@ MasterType VirtualPath="~/ContentPage.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ExtraStylesAndScripts" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <style type="text/css">
        /* Set height of the grid so .sidenav can be 100% (adjust as needed) */
        .row.content {
            height: 550px;
        }

        /* Set gray background color and 100% height */
        .sidenav {
            background-color: #f1f1f1;
            height: 100%;
        }

        /* On small screens, set height to 'auto' for the grid */
        @media screen and (max-width: 767px) {
            .row.content {
                height: auto;
            }
        }

        .productInfo {
            height: 155px !important;
            overflow: scroll !important;
        }

        div.RadListBox .rlbText {
            white-space: nowrap;
            display: inline-block;
        }

        div.RadListBox .rlbGroup {
            overflow: auto;
        }

        div.RadListBox .rlbList {
            display: inline-block;
            min-width: 100%;
        }

        * + html div.RadListBox .rlbList {
            display: inline;
        }

        * html div.RadListBox .rlbList {
            display: inline;
        }

        ul {
            list-style-type: none;
        }

        .ChkPricingStatus input {
            font: 9pt arial;
            text-align: left;
            font-weight: bold;
            margin-left: 5px;
        }

        .ChkPricingStatus td {
            padding-left: 5px;
        }

        .ChkLabel label
        {
            font: 8pt arial;            
            padding-left: 3px;
            font-weight: bold;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentArea" runat="server">

    <telerik:RadAjaxLoadingPanel ID="ralpCopyPriLoadPnl" runat="server" IsSticky="false" Transparency="2" Skin="Windows7">
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadAjaxPanel ID="rapCopyPrioritization" runat="server" EnableAJAX="true" LoadingPanelID="ralpCopyPriLoadPnl">
        <div id="pageActionBar">
            <telerik:RadToolBar ID="rtbCopyPrioritization" runat="server" CssClass="SeparatedButtons" OnClientLoad="clientLoad">
                <Items>
                    <telerik:RadToolBarButton runat="server" CommandName="Reset" DisabledImageUrl="~/Images/Reset_d.gif"
                        ImageUrl="~/Images/Reset.gif" Text="Reset" CssClass="rightAligned" CausesValidation="false"
                        TabIndex="15">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton runat="server" CommandName="Submit" DisabledImageUrl="~/Images/Retrieve1_d.gif"
                        ImageUrl="~/Images/Save.gif" Text="Submit" CssClass="rightAligned">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton runat="server" CommandName="Retrieve" DisabledImageUrl="~/Images/Retrieve1_d.gif"
                        ImageUrl="~/Images/Retrieve1.gif" Text="Retrieve" CssClass="rightAligned">
                    </telerik:RadToolBarButton>
                </Items>
            </telerik:RadToolBar>
        </div>
        <div id="pageHeader">
            <asp:Label ID="lblPageHeader" runat="server" Text="Copy Prioritization" />
            <bonton:MessagePanel ID="MessagePanel1" runat="server" />
        </div>
        <telerik:RadTabStrip ID="rtsCopyPrioritization" runat="server"
            MultiPageID="rmpCopyPrioritization"
            SelectedIndex="0"
            OnClientTabSelected="OnClientTabSelected">
            <Tabs>
                <telerik:RadTab runat="server"
                    Text="Copy Prioritization Result List"
                    PageViewID="pvCopyPrioritization"
                    Font-Bold="True"
                    Selected="True" />
                <telerik:RadTab runat="server"
                    Text="Edit Copy"
                    PageViewID="pvEditCopy"
                    Visible="false"
                    Font-Bold="True" PerTabScrolling="true" />
            </Tabs>
        </telerik:RadTabStrip>

        <telerik:RadMultiPage ID="rmpCopyPrioritization" ScrollBars="Vertical"
            SelectedIndex="0"
            runat="server"
            Height="92%"
            BorderWidth="1">
            <telerik:RadPageView ID="pvCopyPrioritization" runat="server" Height="98%">

                <telerik:RadGrid ID="grdCopyPrioritization"
                    EnableViewState="true"
                    runat="server"
                    SkinID="CenteredWithScroll"
                    AllowPaging="true"
                    Height="760px"
                    Visible="true"
                    ShowFooter="true">
                    <MasterTableView DataKeyNames="ImageId, CategoryCode"
                        ClientDataKeyNames="ImageId, CategoryCode">
                        <NoRecordsTemplate>
                            No records available.
                        </NoRecordsTemplate>
                        <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="" UniqueName="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkItem" runat="server" AutoPostBack="true" Checked="false" OnCheckedChanged="chkItem_CheckedChanged" />
                                </ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                            </telerik:GridTemplateColumn>
                            <%--                                    <telerik:GridTemplateColumn UniqueName="ImageIdLink" HeaderText="Image Id">
                                        <HeaderStyle HorizontalAlign="Center" Width="108" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="108" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbImageId" runat="server" AlternateText='<%# Eval("ImageId")%>'
                                                                Text='<%# Eval("ImageId") %>' PostBackUrl='<%# String.Format("~/Webforms/GXS/CopyPrioritizationDetails.aspx?ImageId={0}", Eval("ImageId"))%>' ToolTip='<%# Eval("ImageId") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>
                            <telerik:GridBoundColumn DataField="WeightedInventory" HeaderText="Weighted Inventory"
                                ReadOnly="true" UniqueName="WeightedInventory">
                                <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ImageId" HeaderText="Image Id" DataField="ImageId">
                                <HeaderStyle HorizontalAlign="Center" Width="108" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="108" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="Image" HeaderText="Image">
                                <HeaderStyle HorizontalAlign="Center" Width="108" />
                                <ItemStyle HorizontalAlign="Center" Width="108" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibThumbnail" runat="server" ImageUrl='<%# Eval("PrimaryThumbnailURL") %>' AlternateText='<%# Eval("ImageId")%>'
                                        Width="100px" Height="80px" ImageAlign="Middle" ToolTip='<%# Eval("ImageId") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--                            <telerik:GridBoundColumn DataField="ImageNotes" HeaderText="Image Notes" UniqueName="ImageNotes"
                                Visible="true" ReadOnly="true">
                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                <ItemStyle HorizontalAlign="Left" Width="60px" />
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="Hierarchy" HeaderText="Primary Web Category" UniqueName="Hierarchy"
                                Visible="true" HtmlEncode="false">
                                <HeaderStyle HorizontalAlign="Left" Width="80" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ProductName" HeaderText="ProductName"
                                UniqueName="ProductName" ReadOnly="true">
                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="160px" />
                                <ItemStyle HorizontalAlign="Left" Width="160px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BrandDesc" HeaderText="Brand"
                                ReadOnly="true" UniqueName="BrandDesc">
                                <HeaderStyle HorizontalAlign="Center" Width="100px" Font-Bold="True" />
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="POStartShipDate" HeaderText="PO Start Ship Date" ReadOnly="true" UniqueName="POStartShipDate">
                                <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                <ItemStyle HorizontalAlign="Right" Width="80px" />
                                <ItemTemplate>
                                    <%# CDate(Eval("POStartShipDate")).ToShortDateString()%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="WebCatAvailableQty" HeaderText="Webcat Available Qty"
                                UniqueName="WebCatAvailableQty">
                                <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                <ItemTemplate>
                                    <%# Server.HtmlEncode(Eval("WebCatAvailableQty"))%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--                                                        <telerik:GridTemplateColumn HeaderText="Details" UniqueName="ProductDetails">
                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="250px" />
                                <ItemStyle HorizontalAlign="Left" Width="250px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblDetails" runat="server" Width="250px"><%# Eval("ProductDetails").ToString.Replace(",", "<br />")%></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="rtxtDetails" runat="server" Text='<%# Regex.Replace((CStr(Eval("ProductDetails")).Replace(",", vbCrLf)), "<(.|\n)*?>", "")%>'
                                        Rows="8" TextMode="MultiLine" MaxLength="2000" Width="250px" Height="200px">
                                    </telerik:RadTextBox>
                                    <bonton:ToolTipValidator ID="valDetails" runat="server" ControlToEvaluate="rtxtDetails"
                                        ErrorMessage="Invalid Character Found."
                                        ValidationGroup="Update" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="FeaturedColorSize" HeaderText="FeaturedColorSize" UniqueName="FeaturedColorSize"
                                ReadOnly="true">
                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="96px" />
                                <ItemStyle HorizontalAlign="Left" Width="96px" />
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="CategoryCode" HeaderText="CategoryCode" UniqueName="CategoryCode"
                                ReadOnly="true" Visible="false">
                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="96px" />
                                <ItemStyle HorizontalAlign="Left" Width="96px" />
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings Selecting-AllowRowSelect="true">
                        <ClientEvents OnGridCreated="collapseCopyPrioritizationPane" />
                    </ClientSettings>
                </telerik:RadGrid>

            </telerik:RadPageView>
            <telerik:RadPageView ID="pvEditCopy"
                runat="server"
                Height="90%">

                <div class="container-fluid">
                    <div class="row" style="margin-top: 10px; margin-left: -30px;">
                        <div class="col-sm-3" style="padding-left: 20px;">
                            <table>
                                <tr>
                                    <td>
                                        <label id="lblProductNameHeader">Product Name:</label>
                                    </td>
                                    <td align="right" style="padding-bottom:2px;">
                                        <input type="button" id="btnInsertR" value="&reg;" name="btnInsertR" onclick="addSpecialSymbol('R')" />&nbsp;
                                        <input type="button" id="btnInsertC" value="&copy;" name="btnInsertC" onclick="addSpecialSymbol('C')" />&nbsp;
                                        <input type="button" id="btnInsertTM" value="&trade;" name="btnInsertTM" onclick="addSpecialSymbol('TM')" />&nbsp;
                                        <input type="button" id="btnInsertSM" value="sm" name="btnInsertSM" onclick="addSpecialSymbol('SM')" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox runat="server" ID="txtProductName" Width="385px" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <h5></h5>
                        </div>
                        <div class="col-sm-9" style="padding-left: 20px;">
                            <label id="lblProductNotesHeader">Product Notes:</label>
                            <br />
                            <asp:Label runat="server" ID="lblProductNotes"></asp:Label>
                            <h5></h5>
                            <%--                            <div class="panel-group">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" href="#collapse1"><span runat="server" id="productName">
                                                <asp:Label runat="server" ID="lblProductName"></asp:Label></span></a>
                                        </h4>
                                    </div>
                                    <div id="collapse1" class="panel-collapse collapse">
                                        <div class="panel-body">

                                            <h5><span runat="server" id="productDetails">
                                                <asp:Label runat="server" ID="lblProductDetails"></asp:Label></span></h5>
                                        </div>
                                        <div class="panel-footer">
                                            <asp:Label runat="server" ID="lblCategoryPath"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>--%>
                        </div>
                    </div>
                    <div class="row content">
                        <div class="col-sm-3 sidenav hidden-xs well" style="margin-top: -5px;">
                            <telerik:RadImageGallery RenderMode="Lightweight"
                                runat="server"
                                ID="rigProduct"
                                DisplayAreaMode="Image"
                                Width="300px"
                                Visible="true"
                                DataImageField="imageURL"
                                OnNeedDataSource="rigProduct_NeedDataSource"
                                BackColor="White">
                                <ImageAreaSettings Height="250px" />
                                <ThumbnailsAreaSettings Mode="ImageSlider" />
                            </telerik:RadImageGallery>
                            <br />
                            <label id="lblColorSizeHeader">Color & Size Information</label>
                            <h5>Available Colors</h5>
                            <h5>
                                <asp:Label ID="lblAvailableColors" runat="server"></asp:Label></h5>
                            <br />
                            <label id="lblImageNotesHeader">Image Notes</label>
                            <h5>
                                <asp:Label ID="lblImageNotes" runat="server"></asp:Label>
                            </h5>
                        </div>
                        <br />

                        <div class="col-sm-9" style="margin-top: -21px;">
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="well">
                                                <label id="lblCopyPreviewHeader">Copy Preview</label>
                                                <asp:TextBox ID="txtCopyPreview" TextMode="MultiLine" runat="server" Columns="80" Rows="10" Font-Size="Small" Width="300px" Height="260px"></asp:TextBox>
                                                <asp:HiddenField ID="hdnCopy" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="well">
                                                <telerik:RadListBox RenderMode="Lightweight"
                                                    ID="rlbCopy"
                                                    runat="server"
                                                    SelectionMode="Multiple"
                                                    AllowTransfer="false"
                                                    AutoPostBackOnTransfer="true"
                                                    AllowReorder="true"
                                                    AutoPostBackOnReorder="false"
                                                    EnableDragAndDrop="true"
                                                    RenderButtonText="true"
                                                    AllowDelete="true"
                                                    Height="312px"
                                                    Width="325px"
                                                    Skin="Office2010Blue"
                                                    TransferMode="Copy"
                                                    AllowTransferOnDoubleClick="false"
                                                    OnClientReordered="rlbCopyOnClientReordered"
                                                    OnClientDeleted="rlbCopyOnClientDeleted"
                                                    OnClientItemDoubleClicked="rlbCopyOnClientItemDoubleClicked"
                                                    OnClientDropped="rlbCopyOnClientDropped">

                                                    <Items>
                                                    </Items>
                                                </telerik:RadListBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-8">
                                    <div class="col-md-6">
                                        <div class="well">
                                            <label id="lblGXS">GXS Attributes</label>
                                            <telerik:RadListBox RenderMode="Lightweight"
                                                ID="rlbGxs"
                                                runat="server"
                                                SelectionMode="Multiple"
                                                AllowTransfer="true"
                                                TransferToID="rlbCopy"
                                                AllowReorder="false"
                                                AutoPostBackOnReorder="false"
                                                EnableDragAndDrop="true"
                                                Height="652px"
                                                Width="325px"
                                                Skin="Office2010Blue"
                                                TransferMode="Copy"
                                                AllowTransferOnDoubleClick="true"
                                                OnClientDropped="rlbGxsOnClientDropped"
                                                OnClientLoad="rlbGxsOnClientLoad"
                                                OnClientTransferred="rlbGxsOnClientTransferred">
                                                <ButtonSettings AreaHeight="30" Position="Left" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <Items>
                                                </Items>
                                            </telerik:RadListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="well">
                                            <label id="lblSSKU">Style Sku Attributes</label>
                                            <telerik:RadListBox RenderMode="Lightweight"
                                                ID="rlbSsku"
                                                runat="server"
                                                SelectionMode="Multiple"
                                                AllowTransfer="true"
                                                TransferToID="rlbCopy"
                                                AllowReorder="false"
                                                AutoPostBackOnReorder="false"
                                                EnableDragAndDrop="true"
                                                Height="652px"
                                                Width="325px"
                                                Skin="Office2010Blue"
                                                TransferMode="Copy"
                                                AllowTransferOnDoubleClick="true"
                                                OnClientDropped="rlbSskuOnClientDropped"
                                                OnClientTransferred="rlbSskuOnClientTransferred">
                                                <ButtonSettings AreaHeight="30" Position="Left" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <Items>
                                                </Items>
                                            </telerik:RadListBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--                            <div class="col-md-8">
                                <%--                                <div class="row">
<%--                                    <div class="col-md-6">
                                        <div class="well">
                                            <textarea rows="17" id="txtBlurb" cols="45" name="txtBlurb"></textarea>
                                            <br />
                                            <input type="button" id="btnBlurb" value="Add Custom Copy" name="btnBlurb" onclick="addCustomBlurb()" style="margin-top: 10px" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                </div>
                            

                            <%--                                    <div class="row">
                                        <div class="col-sm-4">
                                            <div class="well">

                                            </div>
                                        </div>

                                        <div class="col-sm-4">
                                            <div class="well">

                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="well">

                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="well">

                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="well">

                                            </div>
                                        </div>
                                    </div>--%>

                        <%--                                    <div class="row">
                                        <div class="col-sm-8">
                                            <div>

                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div>

                                            </div>
                                        </div>
                                    </div>--%>
                    </div>
                </div>
                </div>
            </telerik:RadPageView>
        </telerik:RadMultiPage>
    </telerik:RadAjaxPanel>
    <telerik:RadScriptBlock runat="server">
        <script type="text/javascript">
            function OnClientTabSelected(sender, eventArgs) {
                var tab = eventArgs.get_tab();
                var toolbar = $find("<%=rtbCopyPrioritization.ClientID%>");
                var saveButton = toolbar.findItemByText("Submit");
                var retrieveButton = toolbar.findItemByText("Retrieve");
                if (tab.get_text() == "Edit Copy") {

                    saveButton.enable();
                    retrieveButton.disable();

                }
                else {
                    saveButton.disable();
                    if (retrieveButton) {
                        retrieveButton.enable();
                    }
                }
            }

            function rlbCopyOnClientReordered(sender, args) {
                createPreview(sender, args,"reorder");
            }

            function rlbSskuOnClientDropped(sender, args) {
                transferManager.performTransfer(sender, args);

                createPreview(sender, args,"drop");
            }

            function rlbGxsOnClientDropped(sender, args) {

                rlbGxsChangeBackgroundColor(sender, args);

                transferManager.performTransfer(sender, args);

                createPreview(sender, args,"drop");
            }

            function rlbSskuOnClientTransferred(sender, args) {
                createPreview(sender, args,"transfer");
            }

            function rlbGxsOnClientTransferred(sender, args) {
                rlbCopyChangeBackgroundColor(sender, args);

                createPreview(sender, args,"transfer");
            }

            function rlbCopyChangeBackgroundColor(sender, args) {
                var copyItems = $find("<%= rlbCopy.ClientID%>");
                var sourceItems = args.get_items();
                var destItem = null;
                if (sourceItems != null) {
                    for (var i = 0; i < sourceItems.length; i++) {
                        destItem = copyItems.findItemByText(sourceItems[i].get_text());

                        if (destItem != null) {
                            destItem.get_element().style.backgroundColor = "LightGray";
                        }
                    }
                }
            }

            function createPreview(sender, args, action) {

                //var dvCopyPreview = document.getElementById("dvCopyPreview");
                var textCopyPreview = document.getElementById("<%= txtCopyPreview.ClientID%>");
                var copyItems = $find("<%= rlbCopy.ClientID%>");

                var items = copyItems.get_items();
                var blurb = '';
                var txt = '';
                var htmlString = '';
                //for (var i = 0; i < items.get_count() ; i++) {
                //    if (i == 0) {
                //        blurb = "<div style='color:black;margin-bottom: -10px;'><b>" + getTextAfterColon(items.getItem(i).get_text()) + "</b></div>";
                //    }
                //    else {
                //        txt = txt + '<li>' + getTextAfterColon(items.getItem(i).get_text()) + '</li>';
                //    }
                //}
                //dvCopyPreview.innerHTML = blurb + '<br/><ul>' + txt + '</ul>';
                var updatedValues = textCopyPreview.value.split("\n");
                if (action == "delete" && updatedValues.length > 0 && args.get_item().get_text() != ''
                    && updatedValues.indexOf(getTextAfterColon(args.get_item().get_text())) > -1)
                {
                    updatedValues.splice(updatedValues.indexOf(getTextAfterColon(args.get_item().get_text())), 1);
                }

                var selectedValues = [];
                for (var i = 0; i < items.get_count() ; i++) {
                    selectedValues[i] = getTextAfterColon(items.getItem(i).get_text());
                }

                for (var j = 0; j < updatedValues.length; j++)
                {
                    if(updatedValues[j] != '' && selectedValues.indexOf(updatedValues[j]) < 0)
                    {
                        selectedValues.insert(j, updatedValues[j]);
                    }
                }

                for (var i = 0; i < selectedValues.length ; i++) {
                    txt = txt + selectedValues[i] + '\n';
                    htmlString = htmlString + '<li>' + selectedValues[i] + '</li>';
                }

                //for (var i = 0; i < items.get_count() ; i++) {
                //    if (updatedValues.length > 0 && updatedValues[i] != '' && updatedValues[i].toUpperCase() != getTextAfterColon(items.getItem(i).get_text()).toUpperCase())
                //    {
                //        txt = txt + updatedValues[i] + '\n';
                //    }

                //    txt = txt + getTextAfterColon(items.getItem(i).get_text()) + '\n';
                //}
                textCopyPreview.value = txt;
                htmlString = '<ul>' + htmlString + '</ul>';
                var safeString = String(htmlString).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
                var hdnCopy = document.getElementById('<%= hdnCopy.ClientID%>');
                hdnCopy.value = safeString;

            }

            Array.prototype.insert = function (index, item) {
                this.splice(index, 0, item);
            };

            function rlbCopyOnClientDropped(sender, args) {
                transferManager.performTransfer(sender, args);
                var id = args.get_destinationItem().get_listBox().get_id();
                var destinationListBox = $find(id);
                if (destinationListBox != null && id.indexOf("rlbGxs") >= 0) {
                    rlbGxsChangeBackgroundColor(destinationListBox, null);
                }

            }

            function getTextAfterColon(itemText) {
                var colonIndex = itemText.indexOf(": ");
                var resultText = itemText
                if (colonIndex > 0) {
                    resultText = itemText.substr(colonIndex + 1);
                }
                return resultText;
            }
            function rlbCopyOnClientDeleted(sender, args) {
                createPreview(sender, args, "delete");
            }

            function rlbGxsOnClientLoad(sender, args) {
                rlbGxsChangeBackgroundColor(sender, args);
            }
            function rlbGxsChangeBackgroundColor(sender, args) {
                var itemCount = sender.get_items().get_count();
                if (itemCount > 0) {
                    for (var i = 0; i < itemCount; i++) {
                        sender.get_items().getItem(i).get_element().style.backgroundColor = "LightGray";
                    }
                }
            }

            function rlbCopyOnClientItemDoubleClicked(sender, args) {
                var lstCopy = $find("<%= rlbCopy.ClientID%>");
                var txtCustomCopy = document.getElementById("txtBlurb");
                var item = lstCopy.get_selectedItem();
                txtCustomCopy.value = item.get_text();
                lstCopy.get_items().remove(item);
            }

            function addCustomBlurb() {
                var txt = document.getElementById("txtBlurb");
                var list = $find("<%= rlbCopy.ClientID%>");
                var items = list.get_items();
                list.trackChanges();
                var item = new Telerik.Web.UI.RadListBoxItem();
                item.set_text(txt.value);
                item.set_value(txt.value);
                items.add(item);
                list.commitChanges();
                txt.value = '';
                createPreview();
            }

            function addSpecialSymbol(symbolName) {
                //Initially I was passing the encoded characters (For ex: &reg;) directly to this method from the button click event
                //but when I encoded it, it was getting converted to numbers (for ex: &#174;) instead of the entity (for ex: &reg;).
                //Webcat needs these as entities, so created a method to return the entities.
                var specialSymbol = getHtmlSymbol(symbolName);
                var txtProdName = document.getElementById("<%=txtProductName.ClientID%>");
                if (txtProdName.value.indexOf(specialSymbol) <= 0) {
                    txtProdName.value = txtProdName.value + specialSymbol;
                }
            }

            function getHtmlSymbol(symbolName) {
                if (symbolName == "R") {
                    return "&reg;";
                }
                if (symbolName == "TM") {
                    return "&copy;";
                }
                if (symbolName == "C") {
                    return "&trade;";
                }
                if (symbolName == "SM") {
                    return "&#8480;";
                }
            }

            (function ($) {
                transferManager = function () { }
                transferManager.performTransfer = function (sender, args) {
                    var destinationItemIndex = this._getDestinationIndex(args);
                    var destinationListBox = this._getDestinationListBox(args);

                    if (destinationListBox == null)
                        return;

                    var reorderIndex = args.get_dropPosition() == 0 ?
                        destinationItemIndex : destinationItemIndex + 1;
                    var items = args.get_sourceItems();

                    this._transfer(items, destinationListBox, reorderIndex);
                }

                transferManager._transfer = function (items, destination, reorderIndex) {
                    $.each(items, function (index, item) {
                        item.unselect();
                        destination.get_items().insert(reorderIndex, item);
                    });
                }

                transferManager._getDestinationIndex = function (args) {
                    var destinationItem = args.get_destinationItem();

                    if (destinationItem)
                        return destinationItem.get_index();

                    return 0;
                }

                transferManager._getDestinationListBox = function (args) {
                    var destinationItem = args.get_destinationItem();

                    if (destinationItem) {
                        var id = destinationItem.get_listBox().get_id();
                        return $find(id);
                    }

                    var parent = $(args.get_htmlElement()).parent();
                    if (parent.is(".RadListBox")) {
                        var id = parent[0].id;
                        return $find(id);
                    }
                    else if (parent.is(".rlbGroup")) {
                        var id = parent[0].parentNode.id;
                        return $find(id);
                    }
                    return null;
                }
            })($telerik.$);

            function collapseLeftHandNav(sender, args) {
                var grid = $find("<%=grdCopyPrioritization.ClientID %>");
                var masterTable = grid.get_masterTableView();
                var rows = masterTable.get_dataItems();
                //if (rows.length >0)
                //{
                //    collapseRadPane();
                //}
            }

            Sys.Application.add_load(enableSearchControls);
        </script>
    </telerik:RadScriptBlock>
</asp:Content>
