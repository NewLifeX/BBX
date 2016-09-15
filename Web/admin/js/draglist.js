var dragobj={}
window.onerror=function(){return false}
function on_ini()
{
	String.prototype.inc=function(s){return this.indexOf(s)>-1?true:false}
	var agent=navigator.userAgent
	window.isOpr=agent.inc("Opera")
	window.isIE=agent.inc("IE")&&!isOpr
	window.isMoz=agent.inc("Mozilla")&&!isOpr&&!isIE
	if(isMoz)
	{
		Event.prototype.__defineGetter__("x",function(){return this.clientX+2})
		Event.prototype.__defineGetter__("y",function(){return this.clientY+2})
	}
	basic_ini()
}
function basic_ini()
{
	window.$=function(obj){return typeof(obj)=="string"?document.getElementById(obj):obj}
	window.$$=function(obj)
	{
	    if(typeof(obj) == "string")
	    {
	        var divColl = $("dom0").getElementsByTagName("div");
	        for(i = 0 ; i < divColl.length ; i++)
	        {
	            if(divColl[i].getAttribute("flag") == obj)
	                return divColl[i];
	        }
	        return null;
	    }
	    else
	        return null;
	}
	window.oDel=function(obj){if($(obj)!=null){$(obj).parentNode.removeChild($(obj))}}
}
window.onload=function()
{
	on_ini()
	var o=document.getElementsByTagName("h1")
	for(var i=0;i<o.length;i++){
		o[i].onmousedown=movedownEvent;
	}
}

function movedownEvent(e)
{
	if(dragobj.o!=null)
		return false
	e=e||event
	dragobj.o=this.parentNode
	dragobj.xy=getxy(dragobj.o)
	dragobj.xx=new Array((e.x-dragobj.xy[1]),(e.y-dragobj.xy[0]))
	dragobj.o.style.width=dragobj.xy[2]+"px"
	dragobj.o.style.height=dragobj.xy[3]+"px"
	dragobj.o.style.left=(e.x-dragobj.xx[0])+"px"
	dragobj.o.style.top=(e.y-dragobj.xx[1])+"px"			
	dragobj.o.style.position="absolute"
	var om=document.createElement("div")
	dragobj.otemp=om
	om.style.width=dragobj.xy[2]+"px"
	om.style.height=dragobj.xy[3]+"px"
	dragobj.o.parentNode.insertBefore(om,dragobj.o)
	return false
}

document.onselectstart=function(){return false}
//window.onfocus=function(){document.onmouseup()}
//window.onblur=function(){document.onmouseup()}
document.onmouseup=function()
{  
	if(dragobj.o!=null)
	{   
		dragobj.o.style.width="auto"
		dragobj.o.style.height="auto"
		if(!Obj1OverObj2(dragobj.o, $("dom0")))
		{
		    var checkbox = $("tid" + dragobj.o.getElementsByTagName("input")[0].value)
		    if(checkbox != null)
		        checkbox.checked = false;
		    oDel(dragobj.otemp)
		    oDel(dragobj.o);
		    dragobj={}
		    resetFlag();
		    return;
		}
		if(window.isIE)
		{
            if(event.srcElement.tagName.toLowerCase() == "input")
            {
                event.srcElement.click();
                event.srcElement.parentNode.innerHTML = event.srcElement.parentNode.innerHTML;
            }
        }
		strId = dragobj.o.id;
		check = dragobj.o.firstChild.firstChild.checked;
		dragobj.otemp.parentNode.insertBefore(dragobj.o,dragobj.otemp);
		dragobj.o.style.position=""
		oDel(dragobj.otemp)
		dragobj={}
		$(strId).firstChild.firstChild.checked = check;
	}
}
document.onmousemove=function(e)
{
	e=e||event
	if(dragobj.o!=null)
	{
		dragobj.o.style.left=(e.x-dragobj.xx[0])+"px"
		dragobj.o.style.top=(e.y-dragobj.xx[1])+"px"
		createtmpl(e)
	}
}
function getxy(e)
{
	var a=new Array()
	var t=e.offsetTop;
	var l=e.offsetLeft;
	var w=e.offsetWidth;
	var h=e.offsetHeight;
	while(e=e.offsetParent)
	{
		t+=e.offsetTop;
		l+=e.offsetLeft;
	}
	a[0]=t;a[1]=l;a[2]=w;a[3]=h
  return a;
}
function inner(o,e){
	var a=getxy(o)	
	if(e.x>a[1]&&e.x<(a[1]+a[2])&&e.y>(a[0]-document.body.scrollTop)&&e.y<(a[0]+a[3]-document.body.scrollTop)){
		if(e.y<(a[0]+a[3]/2))
			return 1;
		else
			return 2;
	}else
		return 0;
}

function createtmpl(e)
{
	for(var i=0;i<getDom0ElementByDivLength()-1;i++)
	{
		if($$("f"+i)==dragobj.o)
			continue
		var b=inner($$("f"+i),e)
		if(b==0)
			continue
		dragobj.otemp.style.width=$$("f"+i).offsetWidth
		if(b==1)
		{
			$$("f"+i).parentNode.insertBefore(dragobj.otemp,$$("f"+i))
		}
		else
		{
			if($$("f"+i).nextSibling==null)
			{
				$$("f"+i).parentNode.appendChild(dragobj.otemp)
			}
			else
			{
				$$("f"+i).parentNode.insertBefore(dragobj.otemp,$$("f"+i).nextSibling)
			}
		}
		return
	}
	for(var j=0;j<1;j++)
	{
		if($("dom"+j).innerHTML.inc("div")||$("dom"+j).innerHTML.inc("DIV"))
			continue
		var op=getxy($("dom"+j))
		if(e.x>(op[1]+10)&&e.x<(op[1]+op[2]-10)){
			$("dom"+j).appendChild(dragobj.otemp)
			dragobj.otemp.style.width=(op[2]-10)+"px"
		}
	}
}
function resetFlag()
{
    var divColl = $("dom0").getElementsByTagName("div");
    for(i = 0 ; i < divColl.length ; i++)
    {
        divColl[i].setAttribute("flag","f" + i);
    }
}
function getDom0ElementByDivLength()
{
    return $("dom0").getElementsByTagName("div").length;
}
function findElement(hiddname,hiddvalue)
{
    var hiddColl = $("dom0").getElementsByTagName("input");
    for(i = 0 ; i < hiddColl.length; i++)
    {
        if(hiddColl[i].name == hiddname && hiddColl[i].value == hiddvalue)
            return true;
    }
    return false;
}
function addElement(hiddname,hiddvalue,title)
{
    var oNewNode = document.createElement("div");
    oNewNode.className = "mo";
    oNewNode.id = "m" + getDom0ElementByDivLength();
    oNewNode.setAttribute("flag","f" + getDom0ElementByDivLength());
    oNewNode.innerHTML = "<h1><input type='checkbox' name='" + hiddname + "' value='" + hiddvalue + "' checked>" + title + "</h1>";
    oNewNode.firstChild.onmousedown=movedownEvent;
    $("dom0").appendChild(oNewNode);
}

function removeElement(hiddname,hiddvalue)
{
    var hiddColl = $("dom0").getElementsByTagName("input");
    for(i = 0 ; i < hiddColl.length; i++)
    {
        if(hiddColl[i].name == hiddname && hiddColl[i].value == hiddvalue)
        {
           window.oDel(hiddColl[i].parentNode.parentNode);
           resetFlag();
        }
    }
}

function insertElement(hiddname,hiddvalue,title,check)
{
    if(check)
    {
        if(!findElement(hiddname,hiddvalue))
        {
            addElement(hiddname,hiddvalue,title);
        }
    }
    else
    {
        if(findElement(hiddname,hiddvalue)) 
        {
            removeElement(hiddname,hiddvalue);
        }
    }
}