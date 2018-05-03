// Functions are used with RadComboBox and CheckBox ItemTemplate 
// to allow multiple selections


function OnCheckedChanged(e, comboID)
{   
    //stop propagation to allow multiple checkbox selections
    StopPropagation(e);
    
    //get checkbox id that caused event to fire        
    var chk = e.srcElement || e.target;
    var id = chk.id ;    
    
    //create regular expression to get item index association
    var re = /rcb\w+_i([\d]+)_cb\w+/g;
    
    //test regular expression selection
    re.test(id);    
    var index = parseInt(RegExp.$1);   
    
    //get collection of radcombobox items
    var combo = $find(comboID);
    var items = combo.get_items();    
    
    //first item acts as "select all" for all other items
    if (index == 0) 
    {   
        //uncheck all other items
        for (var i = 1; i < items.get_count(); i++)
        {
            var checkBox = GetItemCheckBox(items.getItem(i));
            checkBox.checked = false;
        }           
    }
    else
    {   
        //uncheck the first item
        var checkBox = GetItemCheckBox(items.getItem(0));
        checkBox.checked = false;       
    }       
}

function OnClientDropDownClosing(sender, eventArgs)
{   
    //declare variables 
    var inputArea = sender.get_inputDomElement();
    var items = sender.get_items();
    var text = "";
    var toolTip = "Selected Items:\n";
                  
    
    
    //cycle through item collection for checked items
    for (var i = 0; i < items.get_count(); i++)
    {            
        var checkBox = GetItemCheckBox(items.getItem(i));            
        if (checkBox.checked)
        {   
            //get checked items text value
            var itemText = items.getItem(i).get_text()
            
            //append item to combo Text and ToolTip properties
            text += itemText + ";";
            toolTip += itemText + "\n";
        }
    } 
    //set combo Text and ToolTip properties    
    sender.set_text(RTrim(text,";"));    
    inputArea.title = RTrim(toolTip, "\n");
}


function OnClientSelectedIndexChanging(sender, eventArgs)
{
    //does not allow combobox item selection when RadComboBox is using CheckBox ItemTemplate
    eventArgs.set_cancel(true);
}

function StopPropagation(e)
{
    //stop propagation to allow multiple checkbox selections
    e.cancelBubble = true;
    if (e.stopPropagation)
    {
        e.stopPropagation();
    }
}  
           
function GetItemCheckBox(item)
{
    //get the 'div' representing the current RadComboBox Item.
    var itemDiv = item.get_element();
   
    //get the collection of all 'input' elements in the 'div' (which are contained in the Item).
    var inputs = itemDiv.getElementsByTagName("input");
   
    for (var inputIndex = 0; inputIndex < inputs.length; inputIndex++)
    {  
        var input = inputs[inputIndex];
       
        //gheck the type of the current 'input' element.
        if (input.type == "checkbox")
        {
            return input;
        }
    }
   
    return null;
}



