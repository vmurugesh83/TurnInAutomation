function IsDirty()
{                                    
    if (dirtyControls == null)
        return false;
    for (var i = 0, j = dirtyControls.length; i < j; i++)
    {                        
        var control;
        switch(dirtyControls[i].Type)
        {
            case "TextBox":
                control = $get(dirtyControls[i].ID);
                if (control.value != dirtyControls[i].Value)
                    return true;
                break;
            case "RadioButtonList":
                control = $get(dirtyControls[i].ID + "_" + dirtyControls[i].Value );                
                if (!control.checked)
                    return true;              
                break;
            case "CheckBox":
                control = $get(dirtyControls[i].ID);                
                if (control.checked != Boolean(dirtyControls[i].Value))
                    return true;              
                break;
            case "RadTextBox":
                control = $find(dirtyControls[i].ID);                                
                if (control.get_value() != dirtyControls[i].Value)
                    return true;
                break;
            case "RadNumericTextBox":
                control = $find(dirtyControls[i].ID);                                
                if (control.get_value() != dirtyControls[i].Value)
                    return true;
                break;
            case "RadComboBox":
                control = $find(dirtyControls[i].ID);                                
                if (control.get_value() != dirtyControls[i].Value)
                    return true;
                break;
            case "RadTimePicker":
                control = $find(dirtyControls[i].ID);                                               
                if (control.get_dateInput().get_value() != dirtyControls[i].Value)
                    return true;
                break;
            case "RadDatePicker":
                control = $find(dirtyControls[i].ID);                
                if (control.get_dateInput().get_value() != dirtyControls[i].Value)
                    return true;
                break;
                  }
    }
    return false;
}

