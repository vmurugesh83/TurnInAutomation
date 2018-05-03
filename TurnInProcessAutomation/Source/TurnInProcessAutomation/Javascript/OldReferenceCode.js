// JScript File

//Old promoStyle page code:
    <input id="styleRowIndex" type="hidden" runat="server" value="0" />
    <input id="internalStyleNum" type="hidden" runat="server" value="" />
    <input id="totSKU" type="hidden" runat="server" value="" />
    <input id="skuHeaderCheckboxID" type="hidden" runat="server" value="" />
    <input id="styleHeaderCheckboxID" type="hidden" runat="server" value="" />

            function SKUClicked(pcID, internalStyleNum, vendorStyleNum, itemDes, brandShortDesc, shrtPatternDc)
            {
               var oWindow = radopen("PromoSKU.aspx?pcID=" + pcID + "&internalStyleNum=" + internalStyleNum
               + "&vendorStyleNum=" + vendorStyleNum + "&itemDes=" + itemDes + 
               "&brandShortDesc=" + brandShortDesc,"rwSKUs");
               oWindow.SetSize(GetWidth() - 222, GetHeight() - 50);
               oWindow.MoveTo(222, 50);
               
               //Hack to fix window loading twice when ReloadOnShow="True"
               //Set ReloadOnShow="False" and run this code:  
               oWindow.OnClientClose = function()  
                    {  
                        oWindow.SetUrl("about:blank");  
                    } 
               return false;
            }           
            
            function OnClientCloseWin()
            {          
                alert('Closed');
            }            
   
            function StyleHeaderClicked()
            {
                var checkBoxID = document.getElementById("<%=styleHeaderCheckboxID.ClientID %>").value;
                var checkBox = document.getElementById(checkBoxID);
                
                //alert("Style click: " + checkBox.checked);
                return false;
            }

            function SKUHeaderClicked()
            {
                var checkBoxID = document.getElementById("<%=skuHeaderCheckboxID.ClientID %>").value;
                var checkBox = document.getElementById(checkBoxID);
                
                //alert("SKU click: " + checkBox.checked);
                
                if (checkBox.checked) {
                   //selectAllRows(""); INSERT SKU GRID CLIENT ID
                }
                else {
                    //deSelectAllRows(""); INSERT SKU GRID CLIENT ID
                }
                return false;
            }

            function SKUClickedOld(styleRowIndex, internalStyleNum, totSKU)
            {
                //Place key data values into hidden fields for subsequent use.
                document.getElementById("<%=styleRowIndex.ClientID %>").value = styleRowIndex;
                document.getElementById("<%=internalStyleNum.ClientID %>").value = internalStyleNum;
                document.getElementById("<%=totSKU.ClientID %>").value = totSKU;
                return false;
            }                

            function StyleRowSelected(sender, eventArgs)
            {
                /*
                //Get current and selected isn's.
                var styleRowIndex = document.getElementById("<%=styleRowIndex.ClientID %>").value;
                var curInternalStyleNum = document.getElementById("<%=internalStyleNum.ClientID %>").value;
                var masterTable = $find("<%=rgStyle.ClientID %>").get_masterTableView();
                var selInternalStyleNum = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[styleRowIndex], "INTERNAL_STYLE_NUM").innerHTML;
                
                //If user checked the style level check box, then need to uncheck all sku's.
                if (curInternalStyleNum == selInternalStyleNum) {
                    //Drill into table to get checkbox and image elements.
                    var table = document.getElementById("<%=rgStyle.ClientID %>");  
                    var tbody = table.getElementsByTagName("tbody")[0];
                    var trow = tbody.getElementsByTagName("tr")[styleRowIndex];
                    var chkCell = trow.getElementsByTagName("td")[0].getElementsByTagName('input')[0];

                    var grid = $find(""); INSERT SKU GRID CLIENT ID
                    if (chkCell.checked && grid.get_selectedItems().length > 0) {
                        if (confirm("Selecting this style will reset all selected SKU's.  Continue?")) {
                            deSelectAllRows(grid);
                        }
                        else {
                            chkCell.checked = false;
                        }
                    }
                }
                return false;
                */
            }

            function SKURowSelected(sender, eventArgs)
            {
                //Get style row index which was stored by SKUClicked into hidden field.
                var styleRowIndex = document.getElementById("<%=styleRowIndex.ClientID %>").value

                //Drill into table to get checkbox and image elements.
                var table = document.getElementById("<%=rgStyle.ClientID %>");  
                var tbody = table.getElementsByTagName("tbody")[0];
                var trow = tbody.getElementsByTagName("tr")[styleRowIndex];
                var chkCell = trow.getElementsByTagName("td")[0].getElementsByTagName('input')[0];
                var imgCell = trow.getElementsByTagName("td")[2].getElementsByTagName('img')[0];            
                var totSKU = document.getElementById("<%=totSKU.ClientID %>").value;
                
                //var skuGrid = $find("");  INSERT SKU GRID CLIENT ID
                var totSelected = skuGrid.get_selectedItems().length; 
                
                var styleGrid = $find("<%=rgStyle.ClientID %>");  
                var styleMasterTable = styleGrid.get_masterTableView();    

                if (totSelected == totSKU) {
                    imgCell.src = '../Images/Checked.png';
                    imgCell.title = totSelected + ' of ' + totSKU + ' SKUs Selected';
                    chkCell.checked = false;
                    //if (chkCell.checked) {    
                    //    styleMasterTable.deselectItem(styleMasterTable.get_dataItems()[styleRowIndex].get_element());  
                    //}
                }
                else if (totSelected == 0) {
                    imgCell.src = '../Images/UnChecked.png';
                    imgCell.title = 'Style level selection';
                    chkCell.checked = true;
                    //if (!chkCell.checked) {    
                    //    styleMasterTable.selectItem(styleMasterTable.get_dataItems()[styleRowIndex].get_element());  
                    //}
                }
                else
                {
                    imgCell.src = '../Images/Indeterminate.png';
                    imgCell.title = totSelected + ' of ' + totSKU + ' SKUs Selected';
                    chkCell.checked = false;
                    //if (chkCell.checked) {    
                    //    styleMasterTable.deselectItem(styleMasterTable.get_dataItems()[styleRowIndex].get_element());  
                    //}
                }
                return false;
            }
                          
            function selectAllRows(grid)  
            {                                  
                var masterTable = grid.get_masterTableView();  
    
                masterTable.clearSelectedItems();  
                for(var i =0; i < masterTable.get_dataItems().length; i++)  
                    masterTable.selectItem(masterTable.get_dataItems()[i].get_element());  
                  
                return false;  
            }  
            
            function deSelectAllRows(grid)  
            {  
                var masterTable = grid.get_masterTableView();    
                masterTable.clearSelectedItems();  
                //for(var i =0; i < masterTable.get_dataItems().length; i++)  
                //    masterTable.deselectItem(masterTable.get_dataItems()[i].get_element());  
                  
                return false;  
            }
//End Old promoStyle page code

//sets RadComboBox height based on number of items
function OnClientItemsRequestedHandler(sender, eventArgs) 
{ 
    //set the max allowed height of the combo  
    var MAX_ALLOWED_HEIGHT = 250; 
    //this is the single item's height  
    var SINGLE_ITEM_HEIGHT = 25; 
 
    var calculatedHeight = sender.get_items().get_count() * SINGLE_ITEM_HEIGHT; 
 
    var dropDownDiv = sender.get_dropDownElement(); 
     
    if (calculatedHeight > MAX_ALLOWED_HEIGHT)  
    {  
        setTimeout (function () {dropDownDiv.firstChild.style.height = MAX_ALLOWED_HEIGHT + "px";}, 20);                 
    }     
}