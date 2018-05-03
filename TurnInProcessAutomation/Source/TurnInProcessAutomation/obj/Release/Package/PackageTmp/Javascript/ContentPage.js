function RowSelecting(sender, args)
{
    //declare variables
    var domEvent = args.get_domEvent();
    var currentElement = domEvent.srcElement ? domEvent.srcElement : domEvent.target;
    
    //selects grid row only if the checkbox is used
    if(currentElement.tagName.toLowerCase() != "input")
    {
       args.set_cancel(true);
    }
}
function RowDeselecting(sender, args)
{
    //declare variables
    var domEvent = args.get_domEvent();
    var currentElement = domEvent.srcElement ? domEvent.srcElement : domEvent.target;
    
    //deselects grid row only if the checkbox is used        
    if(currentElement.tagName.toLowerCase() != "input")
    {
       args.set_cancel(true);
    }
}

function OnClientDropDownOpeningHandler(sender, eventArgs)
{
    //disables dropdown when page is in inquiry mode
    eventArgs.set_cancel(true);
}

function SetThruDate(dateCtrl, newDate)
{
    //set thru date to start date selection
    dateCtrl.set_selectedDate(newDate);
}

function ValidateOnSave(sender, args)
{
    if (args.get_item().get_text() == "Save")
    {
        //validate page
        Page_ClientValidate();
    
        //display errors if page is invalid
        if (!Page_IsValid)
        {
            //show message panel
            $("*[id$='pnlMessage']").show();
            
            //validation failed, set error title
            $("*[id$='lblMessage']").html("Errors on page");
        }
        else //hide message panel        
        $("*[id$='pnlMessage']").hide();
            
    }        

}
function OnClientImageChanging(sender, args) {

}


