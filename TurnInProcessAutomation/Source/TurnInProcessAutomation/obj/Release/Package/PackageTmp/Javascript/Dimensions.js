// Universal functions that controls height and width dimesions of the browser 
// and other DOM elements

function ResizeElement(element) 
{     
    //declare variables
    var elementId
    var myHeight
    var truePos
    
    //get id of element to resize
    elementId = document.getElementById(element)
    if (elementId != null)
    {
        //get height of window
        myHeight = GetHeight();
        //get true position of element by offset
        truePos = GetPosition(elementId);
        
        //ensure top position is less than window height
        if (truePos <= myHeight)
        {
            //resize element height
            elementId.style.height = (myHeight - truePos) + 'px';
        }
    }    
}

function GetHeight () 
{
	if (typeof(window.innerWidth) == 'number') 
	{ 
	    return window.innerHeight; 
	}
	else if (document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight) ) 
	{
		return document.documentElement.clientHeight;
	}
	else if (document.body && ( document.body.clientWidth || document.body.clientHeight ) ) 
	{
		return document.body.clientHeight;
	}
}

function GetWidth () 
{
	if (typeof(window.innerWidth) == 'number') 
	{ 
	    return window.innerWidth; 
	}
	else if (document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight) ) 
	{
		return document.documentElement.clientWidth;
	}
	else if (document.body && ( document.body.clientWidth || document.body.clientHeight ) ) 
	{
		return document.body.clientWidth;
	}
}

function GetPosition (element) 
{    
    //declare variables
    var topPos = 0;
    
    //recursive check for all parent elements to get true position in window
    if (element.offsetParent) 
    {
        do {
            topPos += element.offsetTop;
           } 
        while (element = element.offsetParent);
    }
    return topPos
}



