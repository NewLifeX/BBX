
//鼠标mouseover事件处理程序
function tabpage_mouseover(e)
{
    if(e.className == "CurrentTabSelect")
	{
		return ;
	}
	
	if(e.className != "OnTabSelect")
	{
		e.className = "OnTabSelect";
	}
}

//鼠标mouseout事件处理程序
function tabpage_mouseout(e)
{
    if(e.className == "CurrentTabSelect")
	{
		return ;
	}
	if(e.className != "TabSelect")
	{
		e.className = "TabSelect";
    }
}

//服务器端选取属性页选项处理程序
function tabpage_selectonserver(e,tabpageid)
{
	e.parentNode.parentNode.childNodes[0].value = tabpageid;
}

function tabpage_selectonclient(e,tabpageid)
{
	tabdiv = e.parentNode;
	
	//此处代码说明详见tabcontrols控件源码
	var tabpagediv = getElementsByClassName('tab-page','div',document);
    var tabareas = getElementsByClassName('tabarea','div',document);
		
	//当前选中节点的所有子节点设置为选中状态	
	for(i=0;i<tabdiv.childNodes.length;i++)
	{  
		tabdiv.childNodes[i].className = "TabSelect";
		tabdiv.childNodes[i].childNodes[0].className = "";
	}
		 
	//除当前选中节点之外的其余节点设置为隐藏	 
	for(i=0;i<tabpagediv.length;i++)
	{
		if(tabpagediv[i].id.indexOf(e.id.split(':')[0])>=0)
	    {
			tabpagediv[i].style.display = "none";
		}
    }
    
    //对当前结点的子结点(所有)设置属性
    for(i=0;i<tabareas.length;i++)
	{
        if(tabareas[i].id.indexOf(e.id.replace('_li',''))>=0)
	    {
		     tabareas[i].style.display = "block";
		     tabareas[i].childNodes[0].style.display = 'block';
    	}
    }
   
    //对当前结点的父节点(所有)设置属性
    var parentnode = document.getElementById(tabpageid);
    while(true)
    {
        parentnode = parentnode.parentNode;
        
        if(parentnode == null)
        {   
            break;
        }
       
        if((parentnode.className =="tab-page")||(parentnode.className =="tabarea"))
        {
            parentnode.style.display = 'block';
        }
    }
    
    //此处代码说明详见tabcontrols控件源码
 	document.getElementById(tabpageid).style.display = 'block';
	document.getElementById(tabpageid+"_li").className = 'CurrentTabSelect';
	document.getElementById(tabpageid+"_li").childNodes[0].className="current";
}

//获取指定样式的元素
function getElementsByClassName(strClassName, strTagName, oElm)
{
    var arrElements = (strTagName == "*" && document.all)? document.all : oElm.getElementsByTagName(strTagName);
    var arrReturnElements = new Array();
    strClassName = strClassName.replace(/\-/g, "\\-");
    var oRegExp = new RegExp("(^|\\s)" + strClassName + "(\\s|$)");
    var oElement;
    for(var i=0; i<arrElements.length; i++)
    {
        oElement = arrElements[i];      
        if(oRegExp.test(oElement.className))
        {
            arrReturnElements.push(oElement);
        }   
    }
    return (arrReturnElements)
}

