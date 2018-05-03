String.prototype.toBoolean = function () {
    // Convert string value to boolean.
    switch (this.toUpperCase()) {
        case "1":
        case "TRUE":
        case "YES":
            return true;
        case "0":
        case "FALSE":
        case "NO":
            return false;
        default:
            return Boolean(value);
    }
}

// Define an RTS module object.
if (!RTS) {
    var RTS = {};
}

// Define a 'common' namespace.
if (!RTS.Common) {
    RTS.Common = {};
}

if (!RTS.Common.resetCursorPosition) {
    // Resets the cursor of the control to the beginning of its value.  This is
    // commonly used for left-aligning lengthy items in a dropdown once selected.
    RTS.Common.resetCursorPosition = function (sender, args) {
        sender.selectText(0, 1);
    };
}

if (!RTS.Common.blockUI) {
    // Block the user interface when the form is submitted or an AJAX request is submitted.
    // http://malsup.com/jquery/block/#overview
    RTS.Common.blockUI = function () {
        $.blockUI({
            message: '<p>Processing...</p>',
            css: {
                padding: '2px',
                width: '100px',
                top: '',
                left: '15px',
                bottom: '15px',
                right: '',
                textAlign: 'center',
                color: '#000',
                border: '1px solid #aaa',
                backgroundColor: '#fff',
                cursor: 'wait'
            },
            showOverlay: true,
            overlayCSS: {
                backgroundColor: '#000',
                opacity: 0.0
            },
            baseZ: 99999
        });
    }
}

if (!RTS.Common.unBlockUI) {
    // Unblock the user interface when AJAX request is complete.
    RTS.Common.unBlockUI = function () {
        $.unblockUI();
    }
}

if (!RTS.WebControls) {
    RTS.WebControls = {};
}

if (!RTS.WebControls.RadComboBox) {
    RTS.WebControls.RadComboBox = {};
}

if (!RTS.WebControls.RadComboBox.findItemTemplateCheckBox) {
    // Find and return the checkbox within the RadComboBox item.
    RTS.WebControls.RadComboBox.findItemTemplateCheckBox = function (item) {
        // Get the collection of all 'input' elements in the 'div' (which are contained in the Item).
        var inputs = item.get_element().getElementsByTagName('input');

        for (var i = 0, input = inputs[0]; i < inputs.length; input = inputs[i++]) {
            // Check the type of the current 'input' element.
            if (input.type == "checkbox") {
                return input;
            }
        }

        return null;
    }
}

if (!RTS.WebControls.RadComboBox.cancelItemSelectingBehavior) {
    RTS.WebControls.RadComboBox.cancelItemSelectingBehavior = function (sender, args) {
        args.set_cancel(true);
    };
}

if (!RTS.WebControls.RadComboBox.updateDisplayTextAndToolTip) {
    // Captures each selected item in the RadComboBox and updates the combo box text display and tooltip with the selected items' values.
    RTS.WebControls.RadComboBox.updateDisplayTextAndToolTip = function (sender, args) {
        var inputArea = sender.get_inputDomElement(),
            items = sender.get_items(), text = "", toolTip = "Selected Items:\n";

        if (!items || !items.get_count || items.get_count() === 0) { return; }

        // Append checked items text to combo text and tooltip attributes.
        for (var i = 0, item = items.getItem(0); i < items.get_count(); i = i + 1, item = items.getItem(i)) {
            var checkBox = RTS.WebControls.RadComboBox.findItemTemplateCheckBox(item);

            if (checkBox.checked) {
                var itemText = item.get_text();

                text += itemText + ";";
                toolTip += itemText + "\n";
            }
        }

        sender.clearSelection();
        sender.set_text(RTrim(text, ";"));
        inputArea.title = RTrim(toolTip, "\n");
    };
}


// Javascript trim, ltrim, rtrim

function Trim(str, chars) {
    return ltrim(rtrim(str, chars), chars);
}

function LTrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
}

function RTrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
}

//*********************************************************************


function OnClientButtonClicking(sender, args) {
    var button = args.get_item();
    if (button.get_text() == "Delete") {
        args.set_cancel(!confirm('Confirm Delete?'));
    }
    if (button.get_text() == "EOD") {
        args.set_cancel(!confirm('Confirm EOD?'));
    }
    if (button.get_text() == "Export") {
        ShowModalMessage();
        args.set_cancel(true);
    }

    if (button.get_text() == "Flood") {
        ShowModalMessageFlood();
        args.set_cancel(true);
    }

}


function OnClientButtonClickingSave(sender, args) {
    var button = args.get_item();
    if (button.get_text() == "Delete") {
        args.set_cancel(!confirm('Confirm Delete?'));
    }
    if (button.get_text() == "Save") {
        args.set_cancel(!confirm('Confirm Save?'));
    }
    if (button.get_text() == "Next Item") {
        args.set_cancel(!confirm('Click OK to proceed to the next item without saving. Any changes will be lost. To save, click CANCEL, then click the SAVE button.'));
    }
    if (button.get_text() == "Previous Item") {
        args.set_cancel(!confirm('Click OK to proceed to the previous item without saving. Any changes will be lost. To save, click CANCEL, then click the SAVE button.'));
    }
    if (button.get_text() == "Export") {
        ShowModalMessage();
        args.set_cancel(true);
    }
}

function clientLoad(toolbar) {
    for (var i = 0; i < toolbar.get_items().get_count(); i++) {
        var item = toolbar.get_items().getItem(i);
        if (item.get_linkElement().className.indexOf('rightAligned') >= 0) {
            item.get_element().className += " rightAlignedWrapper";
        }
    }

    // Block interface when any button is clicked.
    // Exclude Export.
    toolbar.add_buttonClicked(function (sender, args) {
        if (args.get_item().get_text().toUpperCase() !== "EXPORT")
            RTS.Common.blockUI();
    })
}

function setHourglass() {
    document.body.style.cursor = 'wait';
}


function KeyPress(sender, args) {
    if (args.get_keyCharacter() == sender.get_numberFormat().DecimalSeparator) {
        args.set_cancel(true);
    }
    if (args.get_keyCharacter() == sender.get_numberFormat().NegativeSign) {
        args.set_cancel(true);
    }
}


function NoDecimal(sender, eventArgs) {
    var c = eventArgs.get_keyCharacter();
    if (c == '.')
        eventArgs.set_cancel(true);
}

function OnClientBlurHandler(sender, eventArgs) {
    var textInTheCombo = sender.get_text();
    var item = sender.findItemByText(textInTheCombo);
    //if there is no item with that text
    if (!item) {
        sender.set_text("");
        setFocus();
    }
}


function OnClientBlurHandlerDC(sender, eventArgs) {
    var textInTheCombo = sender.get_text();
    var item = sender.findItemByText(textInTheCombo);
    //if there is no item with that text
    if (!item) {
        sender.set_text("");
        setTimeout(function () {
            var inputElement = sender.get_inputDomElement();
            inputElement.focus();
        }, 20);
    }
}


function CostMutChkList(chk) {

    var chkList = chk.parentNode.parentNode.parentNode;
    var chks = chkList.getElementsByTagName("input");

    for (var i = 0; i < chks.length; i++) {
        if (chks[i] != chk && chk.checked) {
            chks[i].checked = false;
        }
    }
}

function RetailMutChkList(chk) {

    var chkList = chk.parentNode.parentNode.parentNode;
    var chks = chkList.getElementsByTagName("input");

    for (var i = 0; i < chks.length; i++) {
        if (chks[i] != chk && chk.checked) {
            chks[i].checked = false;
        }
    }
}

function DiscountMutChkList(chk) {

    var chkList = chk.parentNode.parentNode.parentNode;
    var chks = chkList.getElementsByTagName("input");

    for (var i = 0; i < chks.length; i++) {
        if (chks[i] != chk && chk.checked) {
            chks[i].checked = false;
        }
    }
}

function FreightMutChkList(chk) {

    var chkList = chk.parentNode.parentNode.parentNode;
    var chks = chkList.getElementsByTagName("input");

    for (var i = 0; i < chks.length; i++) {
        if (chks[i] != chk && chk.checked) {
            chks[i].checked = false;
        }
    }
}


function setFocus() {
    try {
        var inputElement = sender.get_inputDomElement();
        inputElement.focus();
    }
    catch (er) {
        //do nothing
    }
}

//***************************************************************
//Switching Calender Window to Top - Batch Control List
function BeginPopupAboveBCL(e, pickerID) {
    var datePicker;
    if (pickerID == null) {
        datePicker = $find("<%= dpModifiedStartDate.ClientID %>");
    }
    else {
        datePicker = $find(pickerID);
    }
    var textBox = datePicker.get_textBox();
    var popupElement = datePicker.get_popupContainer();
    var dimensions = datePicker.getElementDimensions(popupElement);
    var position = datePicker.getElementPosition(textBox);
    datePicker.showPopup(position.x, position.y - dimensions.height);
}


function EndPopupAboveBCL(e, pickerID) {
    var datePicker;
    if (pickerID == null) {
        datePicker = $find("<%= dpModifiedEndDate.ClientID %>");
    }
    else {
        datePicker = $find(pickerID);
    }
    var textBox = datePicker.get_textBox();
    var popupElement = datePicker.get_popupContainer();
    var dimensions = datePicker.getElementDimensions(popupElement);
    var position = datePicker.getElementPosition(textBox);
    datePicker.showPopup(position.x, position.y - dimensions.height);
}

function BeginExtractPopupAboveBCL(e, pickerID) {
    var datePicker;
    if (pickerID == null) {
        datePicker = $find("<%= dpBatchExtractStartDate.ClientID %>");
    }
    else {
        datePicker = $find(pickerID);
    }
    var textBox = datePicker.get_textBox();
    var popupElement = datePicker.get_popupContainer();
    var dimensions = datePicker.getElementDimensions(popupElement);
    var position = datePicker.getElementPosition(textBox);
    datePicker.showPopup(position.x, position.y - dimensions.height);
}


function EndExtractPopupAboveBCL(e, pickerID) {
    var datePicker;
    if (pickerID == null) {
        datePicker = $find("<%= dpBatchExtractEndDate.ClientID %>");
    }
    else {
        datePicker = $find(pickerID);
    }
    var textBox = datePicker.get_textBox();
    var popupElement = datePicker.get_popupContainer();
    var dimensions = datePicker.getElementDimensions(popupElement);
    var position = datePicker.getElementPosition(textBox);
    datePicker.showPopup(position.x, position.y - dimensions.height);
}

//***************************************************************
//Switching Calender Window to Top - Document List

function EndPopupAboveDL(e, pickerID) {
    var datePicker;
    if (pickerID == null) {
        datePicker = $find("<%= dpModifiedEndDate.ClientID %>");
    }
    else {
        datePicker = $find(pickerID);
    }
    var textBox = datePicker.get_textBox();
    var popupElement = datePicker.get_popupContainer();
    var dimensions = datePicker.getElementDimensions(popupElement);
    var position = datePicker.getElementPosition(textBox);
    datePicker.showPopup(position.x, position.y - dimensions.height);
}

function BeginPopupAboveDL(e, pickerID) {
    var datePicker;
    if (pickerID == null) {
        datePicker = $find("<%= dpModifiedStartDate.ClientID %>");
    }
    else {
        datePicker = $find(pickerID);
    }
    var textBox = datePicker.get_textBox();
    var popupElement = datePicker.get_popupContainer();
    var dimensions = datePicker.getElementDimensions(popupElement);
    var position = datePicker.getElementPosition(textBox);
    datePicker.showPopup(position.x, position.y - dimensions.height);
}

function BeginInvPopupAboveDL(e, pickerID) {
    var datePicker;
    if (pickerID == null) {
        datePicker = $find("<%= dpStartInvoiceDate.ClientID %>");
    }
    else {
        datePicker = $find(pickerID);
    }
    var textBox = datePicker.get_textBox();
    var popupElement = datePicker.get_popupContainer();
    var dimensions = datePicker.getElementDimensions(popupElement);
    var position = datePicker.getElementPosition(textBox);
    datePicker.showPopup(position.x, position.y - dimensions.height);
}

function EndInvPopupAboveDL(e, pickerID) {
    var datePicker;
    if (pickerID == null) {
        datePicker = $find("<%= dpEndInvoiceDate.ClientID %>");
    }
    else {
        datePicker = $find(pickerID);
    }
    var textBox = datePicker.get_textBox();
    var popupElement = datePicker.get_popupContainer();
    var dimensions = datePicker.getElementDimensions(popupElement);
    var position = datePicker.getElementPosition(textBox);
    datePicker.showPopup(position.x, position.y - dimensions.height);
}

//***************************************************************

//***************************************************************
//Switching Calender Window to Top - Invoice List Page

function BeginInvoiceList(e, pickerID) {
    var datePicker;
    if (pickerID == null) {
        datePicker = $find("<%= dpStartInvoiceDate.ClientID %>");
    }
    else {
        datePicker = $find(pickerID);
    }
    var textBox = datePicker.get_textBox();
    var popupElement = datePicker.get_popupContainer();
    var dimensions = datePicker.getElementDimensions(popupElement);
    var position = datePicker.getElementPosition(textBox);
    datePicker.showPopup(position.x, position.y - dimensions.height);
}

function EndInvoiceList(e, pickerID) {
    var datePicker;
    if (pickerID == null) {
        datePicker = $find("<%= dpEndInvoiceDate.ClientID %>");
    }
    else {
        datePicker = $find(pickerID);
    }
    var textBox = datePicker.get_textBox();
    var popupElement = datePicker.get_popupContainer();
    var dimensions = datePicker.getElementDimensions(popupElement);
    var position = datePicker.getElementPosition(textBox);
    datePicker.showPopup(position.x, position.y - dimensions.height);
}



//***************************************************************


function onToolBarClientButtonClicking(sender, args) {
    var button = args.get_item();
    if (button.get_commandName() == "DeleteSelected") {
        args.set_cancel(!confirm('Delete all selected records?'));
    }
}




function MoveCursor(sender, args) {
    sender.set_caretPosition(4);

}

//***************************************************************

////Modal PopUp control
//function ShowModalMessage() {
//    var mPopup = $find('<%= mPopup.ClientID %>')
//    var mButton = $get('<%= fakeButton.ClientID %>')
//    mButton.click()
//}

//function HideModalMessage() {
//    var mPopup = $find('<%= mPopup.ClientID %>')
//    mPopup.hide()
//}

//***************************************************************

//Switching Calender Window to Top - Purchase Journal List
function EndPopupAbovePJ(e, pickerID) {
    var datePicker;
    if (pickerID == null) {
        datePicker = $find("<%= dpDocumentDateTo.ClientID %>");
    }
    else {
        datePicker = $find(pickerID);
    }
    var textBox = datePicker.get_textBox();
    var popupElement = datePicker.get_popupContainer();
    var dimensions = datePicker.getElementDimensions(popupElement);
    var position = datePicker.getElementPosition(textBox);
    datePicker.showPopup(position.x, position.y - dimensions.height);
}

function BeginPopupAbovePJ(e, pickerID) {
    var datePicker;
    if (pickerID == null) {
        datePicker = $find("<%= dpDocumentDateFrom.ClientID %>");
    }
    else {
        datePicker = $find(pickerID);
    }
    var textBox = datePicker.get_textBox();
    var popupElement = datePicker.get_popupContainer();
    var dimensions = datePicker.getElementDimensions(popupElement);
    var position = datePicker.getElementPosition(textBox);
    datePicker.showPopup(position.x, position.y - dimensions.height);
}
//***************************************************************

//Multi Selection DropDown List Control
function stopPropagation(e) {
    e.cancelBubble = true;
    if (e.stopPropagation) {
        e.stopPropagation();
    }
}

function OnClientDropDownClosing(sender) {
    document.getElementById(sender.get_id() + "Helper").click();
}
//***************************************************************

function disableCtrlKeyCombination(e) {
    //list all CTRL + key combinations you want to disable   
    var forbiddenKeys = new Array('a', 'n', 'c', 'x', 'v', 'j', 't');
    var key;
    var isCtrl;

    if (window.event) {
        key = window.event.keyCode;     //IE   
        if (window.event.ctrlKey)
            isCtrl = true;
        else
            isCtrl = false;
    }
    else {
        key = e.which;     //firefox   
        if (e.ctrlKey)
            isCtrl = true;
        else
            isCtrl = false;
    }

    //if ctrl is pressed check if other key is in forbidenKeys array   
    if (isCtrl) {
        for (i = 0; i < forbiddenkeys.length; i++) {
            //case-insensitive comparation   
            if (forbiddenKeys[i].toLowerCase() == String.fromCharCode(key).toLowerCase()) {
                alert('Key combination CTRL + '
                                        + String.fromCharCode(key)
                                        + ' has been disabled.');
                return false;
            }
        }
    }
    return true;
}

//***************************************************************
//RadWindow Methods
function getRadWindow() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement && window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    return oWindow;
}

function closeRadWindow() {
    var oArg = new Object();
    var oWindow = getRadWindow();
    if (oWindow) {
        oWindow.Close(oArg);
    }
}

//***************************************************************
function centerElementOnScreen(element) {
    var scrollTop = document.body.scrollTop;
    var scrollLeft = document.body.scrollLeft;
    var viewPortHeight = document.body.clientHeight;
    var viewPortWidth = document.body.clientWidth;
    if (document.compatMode == "CSS1Compat") {
        viewPortHeight = document.documentElement.clientHeight;
        viewPortWidth = document.documentElement.clientWidth;
    }
    var topOffset = Math.ceil(viewPortHeight / 2 - element.offsetHeight / 2);
    var leftOffset = Math.ceil(viewPortWidth / 2 - element.offsetWidth / 2);
    var top = scrollTop + topOffset - 40;
    var left = scrollLeft + leftOffset - 70;
    element.style.position = "absolute";
    element.style.top = top + "px";
    element.style.left = left + "px";
}

function ValidateAsciiChars(sender, args) {
    var str = sender.get_value();
    var isValid = true;

    for (var i = 0; i < str.length; i++) {
        asciiNum = str.charCodeAt(i);
        if ((asciiNum == 10) || (asciiNum == 13) || (asciiNum > 31 && asciiNum < 94) || (asciiNum > 94 && asciiNum < 127)) {
        }
        else {
            isValid = false;
        }
    }
    //sender.get_id()
    if (!isValid) {
        alert("Invalid Character found!");
        sender.focus();
    }
    return isValid;
}

function OnClientSelectedIndexChanged(sender, args) {
    var combo = sender;
    var input = combo.get_inputDomElement();

    if (input.setSelectionRange) {
        input.setSelectionRange(0, 0);
    }
    else if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', 0);
        range.moveStart('character', 0);
        range.select();
    }
}
