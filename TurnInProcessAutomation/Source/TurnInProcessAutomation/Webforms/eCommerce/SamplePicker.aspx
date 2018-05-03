<%@ Page Language="vb" AutoEventWireup="false"  ClassName = "SamplePicker" CodeBehind="SamplePicker.aspx.vb" Inherits="TurnInProcessAutomation.SamplePicker" %>
<%@ Import Namespace="TurnInProcessAutomation.BusinessEntities" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">        
        <title>Central MerchandiseRoom Sample Picker</title>
    </head>
    <body>
        <form id="frmSamplePicker" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <table width="600px" align="center">
                <tr>
                    <td><p><b>Search by:</b></p></td>
                    <td align="center">
                        <telerik:RadNumericTextBox ID="rtbSampleMerchId" runat="server" MaxLength="7" MinValue="0" MaxValue="9999999"
                                EmptyMessageStyle-Font-Italic="true" EmptyMessage="Enter MerchId" ToolTip="Enter MerchId">
                            <NumberFormat GroupSeparator="" DecimalDigits="0" />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td align="center">
                        <telerik:RadNumericTextBox ID="rtbInternalStyle" runat="server" MaxLength="9" MinValue="0" MaxValue="999999999"
                                EmptyMessageStyle-Font-Italic="true" EmptyMessage="Enter ISN" ToolTip="Enter ISN">
                            <NumberFormat GroupSeparator="" DecimalDigits="0" />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td align="center">
                        <telerik:RadTextBox ID="rtbVendorStyle" runat="server" EmptyMessageStyle-Font-Italic="true" EmptyMessage="Enter Vendor Style">
                            <EmptyMessageStyle Font-Italic="True" />
                        </telerik:RadTextBox>
                    </td>
                    <td align="center">
                        <telerik:RadButton ID="rbRetrieve" runat="server" Text="Retrieve" OnClick="OnRetrieveClicked">
                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="grdAvailableSamples" runat="server" OnItemDataBound="grdAvailableSamples_ItemDataBound"
                SkinID="CenteredWithScroll" AutoGenerateColumns="False" AllowPaging="True" ShowFooter="false" 
                Width="1000px" Height="520px" CellSpacing="0" CellPadding="0">
                <ClientSettings>
                    <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="True" />
                    <ClientEvents OnRowSelected="RowSelected" OnGridCreated="GridCreated" />
                </ClientSettings>
                <MasterTableView DataKeyNames="SampleMerchId,InternalStyleNum,ColorCode,ColorLongDesc,SampleSizeDesc,SampleSize, SampleAltAttrDesc, SampleDueDate,PrimaryThumbnailUrl,PrimaryMediumUrl,SecondaryMediumUrl"                                 ClientDataKeyNames="SampleMerchId,InternalStyleNum,ColorCode,ColorLongDesc,SampleSizeDesc,SampleSize, SampleAltAttrDesc, SampleDueDate,PrimaryThumbnailUrl,PrimaryMediumUrl,SecondaryMediumUrl">
                    <NoRecordsTemplate>No samples available</NoRecordsTemplate>
                    <EditFormSettings>
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                    </EditFormSettings>
                    <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="SampleMerchId" HeaderText="Merch Id" UniqueName="AdminMerchNum">
                            <HeaderStyle Width="72" HorizontalAlign="Center" />
                            <ItemStyle Width="72" HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="InternalStyleNum" HeaderText="ISN" UniqueName="InternalStyleNum">
                            <HeaderStyle Width="80" HorizontalAlign="Center" />
                            <ItemStyle Width="80" HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="VendorStyleNumber" HeaderText="Vendor Style" UniqueName="VendorStyleNum">
                            <HeaderStyle Width="120" HorizontalAlign="Center" />
                            <ItemStyle Width="120" HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Color" UniqueName="Color">
                            <HeaderStyle Width="80" HorizontalAlign="Center" />
                            <ItemStyle Width="80" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <%# CType(Container.DataItem, SampleRequestInfo).ColorCode%>
                                -
                                <%# CType(Container.DataItem, SampleRequestInfo).ColorLongDesc%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="SampleSizeDesc" HeaderText="Size" UniqueName="SampleSizeDesc">
                            <HeaderStyle Width="40" HorizontalAlign="Center" />
                            <ItemStyle Width="40" HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField = "SampleAltAttrDesc" HeaderText = "Alt Attribute" UniqueName = "SampleAltAttrDesc">
                        <HeaderStyle Width="60" HorizontalAlign="Center" />
                            <ItemStyle Width="60" HorizontalAlign="Center" Wrap="true" />
                            </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="PrimaryLocationName" HeaderText="Primary Location Name" UniqueName="PrimaryLocationName">
                            <HeaderStyle Width="80" HorizontalAlign="Center" />
                            <ItemStyle Width="80" HorizontalAlign="Center" />
                        </telerik:GridDateTimeColumn>
                        <telerik:GridDateTimeColumn DataField="SampleStatusDesc" HeaderText="Status Description" UniqueName="SampleStatusDesc">
                            <HeaderStyle Width="80" HorizontalAlign="Center" />
                            <ItemStyle Width="80" HorizontalAlign="Center" />
                        </telerik:GridDateTimeColumn>
                        <telerik:GridDateTimeColumn DataField="SampleApprovalTimestamp" HeaderText="Approval Date" UniqueName="SampleApprovalTimestamp">
                            <HeaderStyle Width="80" HorizontalAlign="Center" />
                            <ItemStyle Width="80" HorizontalAlign="Center" />
                        </telerik:GridDateTimeColumn>
                        <telerik:GridDateTimeColumn DataField="CMRCheckInDate" HeaderText="CMR Check-In Date" UniqueName="CMRCheckInDate">
                            <HeaderStyle Width="80" HorizontalAlign="Center" />
                            <ItemStyle Width="80" HorizontalAlign="Center" />
                        </telerik:GridDateTimeColumn>
                        <telerik:GridDateTimeColumn DataField="SampleDueDate" HeaderText="Request Date" UniqueName="SampleDueDate" PickerType="DatePicker" DataFormatString="{0:d}">
                            <HeaderStyle Width="80" HorizontalAlign="Center" />
                            <ItemStyle Width="80" HorizontalAlign="Center" />
                        </telerik:GridDateTimeColumn>
                        <telerik:GridTemplateColumn UniqueName="Thumbnail" HeaderText="Thumbnail">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:ImageButton ID="ibThumbnail" runat="server" ImageUrl='<%# Eval("PrimaryThumbnailUrl") %>' AlternateText='<%# Eval("SampleMerchId") %>' Width="90px" Height="90px" OnClientClick="ShowTheImage(this); return false;" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <FilterMenu EnableImageSprites="False"></FilterMenu>
            </telerik:RadGrid>
        </form>

        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                // Methods for the RadWindow dialog

                var selectedMerchId = {};

                // Need a convenient place to pull out the current window argument - not currently used
                function GridCreating(sender, args) {
                    var currentWindow = GetRadWindow();
                    selectedMerchId = currentWindow.argument;
                }

                // Bring the selected item into view
                function GridCreated(sender, eventArgs) {
                    //gets the main table scrollArea HTML element  
                    var scrollArea = document.getElementById(sender.get_element().id + "_GridData");
                    var row = sender.get_masterTableView().get_selectedItems()[0];

                    //if the position of the selected row is below the viewable grid area  
                    if (row) {
                        if ((row.get_element().offsetTop - scrollArea.scrollTop) + row.get_element().offsetHeight + 20 > scrollArea.offsetHeight) {
                            //scroll down to selected row  
                            scrollArea.scrollTop = scrollArea.scrollTop + ((row.get_element().offsetTop - scrollArea.scrollTop) +
                                                    row.get_element().offsetHeight - scrollArea.offsetHeight) + row.get_element().offsetHeight;
                        }
                        //if the position of the the selected row is above the viewable grid area  
                        else if ((row.get_element().offsetTop - scrollArea.scrollTop) < 0) {
                            //scroll the selected row to the top  
                            scrollArea.scrollTop = row.get_element().offsetTop;
                        }
                     }
                }

                // Select the default item - not currently used
                function RowCreated(sender, args) {
                    var merchId = args.getDataKeyValue("SampleMerchId");
                    if (selectedMerchId == merchId) {
                        args.get_gridDataItem().set_selected(true);
                    }
                }

                //Close the dialog and return the argument to the OnClientClose event handler
                function RowSelected(sender, args) {
                    var grid = $find("<%= grdAvailableSamples.ClientID %>"); // get grid
                    var MasterTable = grid.get_masterTableView(); // get mastertableview
                    var gridDataItem = MasterTable.get_selectedItems()[0]; // get selected row

                 //if (gridDataItem.getDataKeyValue("ColorCode") != 0) {
                        var oResults = new SelectedMerchSample(
                        gridDataItem.getDataKeyValue("SampleMerchId"),
                        gridDataItem.getDataKeyValue("ColorCode"),
                        gridDataItem.getDataKeyValue("ColorLongDesc"),
                        gridDataItem.getDataKeyValue("SampleSizeDesc"),
                        gridDataItem.getDataKeyValue("SampleSize"),
                        gridDataItem.getDataKeyValue("PrimaryThumbnailUrl"));
                //   }

                    var oWnd = GetRadWindow();
                  oWnd.close(oResults);
                }

                // Find the window
                function GetRadWindow() {
                    var oWindow = null;
                    if (window.radWindow)
                        oWindow = window.radWindow;
                    else if (window.frameElement.radWindow)
                        oWindow = window.frameElement.radWindow;
                    return oWindow;
                }

                //return no argument and close the RadWindow
                function cancelAndClose() {
                    var oWindow = GetRadWindow();
                    oWindow.close(null);
                }

                // show the image viewer dialog, with image
                function ShowTheImage(sender) {
                    var sampleImages = new SampleImageInfo();

                    var grid = $find("<%= grdAvailableSamples.ClientID %>"); // get grid
                    var masterTable = grid.get_masterTableView(); // get mastertableview
                    var dataItems = masterTable.get_dataItems();
                    for (var i = 0; i < dataItems.length; i++) {
                        var knownOffset = sender.src.lastIndexOf('/');
                        var subjectOffset = dataItems[i].getDataKeyValue("PrimaryThumbnailUrl").lastIndexOf('/')
                        if (dataItems[i].getDataKeyValue("PrimaryThumbnailUrl").substring(subjectOffset) === sender.src.substring(knownOffset)) {
                            sampleImages = new SampleImageInfo(dataItems[i].getDataKeyValue("PrimaryMediumUrl"),
                                                                dataItems[i].getDataKeyValue("SecondaryMediumUrl"),
                                                                dataItems[i].getDataKeyValue("SampleMerchId"));
                            break;
                        }
                    }

                    if (sampleImages.primaryImageUrl && sampleImages.primaryImageUrl.length > 0) {
                        var oManager = GetRadWindow().BrowserWindow;
                        setTimeout(function () { oManager.openWin(sampleImages); }, 0);
                    }
                }

                function SampleImageInfo(primaryImageUrl, secondaryImageUrl, merchId) {
                    this.primaryImageUrl = primaryImageUrl;
                    this.secondaryImageUrl = secondaryImageUrl;
                    this.MerchId = merchId;
                }

                function SelectedMerchSample(sampleMerchId, colorCode, colorDesc, sizeDesc, sampleSize, primaryThumbnailUrl) {
                    this.sampleMerchId = sampleMerchId;
                    this.colorCode = colorCode;
                    this.colorDesc = colorDesc;
                    this.sizeDesc = sizeDesc;
                    this.sampleSize = sampleSize;
                  this.primaryThumbnailUrl = primaryThumbnailUrl;                   
                }
                                
            </script>
        </telerik:RadCodeBlock>
    </body>
</html>

