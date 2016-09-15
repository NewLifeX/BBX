function showhint(iconid, str)
{
	var imgUrl='../images/hint.gif';
	if (iconid != 0)
	{
		imgUrl = '../images/warning.gif';
	}
	document.write('<div style="background:url(' + imgUrl + ') no-repeat 20px 10px;border:1px dotted #DBDDD3; background-color:#FDFFF2; margin-bottom:10px; padding:10px 10px 10px 56px; text-align: left; font-size: 12px;">');
	document.write(str + '</div><div style="clear:both;"></div>');
}

function showloadinghint(divid, str)
{
	if (divid=='')
	{
		divid='PostInfo';
	}
	document.write('<div id="' + divid + ' " style="display:none;position:relative;border:1px dotted #DBDDD3; background-color:#FDFFF2; margin:auto;padding:10px" width="90%"  ><img border="0" src="../images/ajax_loading.gif" /> ' + str + '</div>');
}


function CheckByName(form,tname,checked)
{
    for (var i=0;i<form.elements.length;i++)
    {
        var e = form.elements[i];
        if(e.name == tname)
        {
            e.checked = checked;
        }
    }
}


function CheckAll(form)
{
  for (var i=0;i<form.elements.length;i++)
    {
        var e = form.elements[i];
        if (e.type=="checkbox" && e.name != 'chkall' && e.name !='deleteMode')
        {
           e.checked = form.chkall.checked;
        }
    }
}

//function SH_SelectOne()
//{
//	var obj = window.event.srcElement;
//	if( obj.checked == false)
//	{
//		document.getElementById('chkall').checked = obj.chcked;
//		
//	}
//}


  function   selectall(s)
  {   
  var   obj=document.getElementsByTagName("input");   
  for(i=0;i<obj.length;i++)
  {
  if(obj[i].id=="id"+s)   
 {
  obj[i].checked=window.event.srcElement.checked ; 
 }
  }   
}


function SH_SelectOne(obj)
{
	//var obj = window.event.srcElement;
	if( obj.checked == false)
	{
		document.getElementById('chkall').checked = obj.chcked;
	}
}


//function togetherpi(obj)
//{
//if(document.getElementById("id"+obj).checked == true)
//{
//document.getElementById("pid"+obj).checked =true;
//else
//document.getElementById("pid"+obj).checked =false;
//}



var xmlhttp;
   
function getReturn(Url)  //提交为aspx,aspx页面路径, 返回页面的值
{
    if(typeof XMLHttpRequest != "undefined")
    {
        xmlhttp = new XMLHttpRequest();
    }
    else if(window.ActiveXObject)
    {
        var versions = ["MSXML2.XMLHttp.5.0","MSXML2.XMLHttp.4.0","MSXML2.XMLHttp.3.0","MSXML2.XMLHttp","Microsoft.XMLHttp"];
        for(var i = 0 ; i < versions.length; i++)
        {
            try
            {
                xmlhttp = new ActiveXObject(versions[i]);
                break;
            }
            catch(E)
            {
            }
        }
    }
        
    try 
    {
        xmlhttp.open('GET',Url,false);   
        xmlhttp.setRequestHeader('Content-Type','application/x-www-form-urlencoded')
        xmlhttp.send(null);    
        
        if((xmlhttp.readyState == 4)&&(xmlhttp.status == 200))
        {
            return xmlhttp.responseText;
        }
        else
        {
           return null;
        }
    }
    catch (e) 
    {  
         alert("你的浏览器不支持XMLHttpRequest对象, 请升级"); 
    }

    return null;
}
       

function isMaxLen(o)
{
	var nMaxLen=o.getAttribute? parseInt(o.getAttribute("maxlength")):"";
	if(o.getAttribute && o.value.length>nMaxLen)
	{
		o.value=o.value.substring(0,nMaxLen)
	}
}
    
/*
function Pause(obj,iMinSecond){ 
 if (window.eventList==null) window.eventList=new Array(); 
 var ind=-1; 
 for (var i=0;i<window.eventList.length;i++){ 
  if (window.eventList[i]==null) { 
   window.eventList[i]=obj; 
   ind=i; 
   break; 
  } 
 } 
  
 if (ind==-1){ 
  ind=window.eventList.length; 
  window.eventList[ind]=obj; 
 } 
 setTimeout("GoOn(" + ind + ")",iMinSecond); 
} 


function GoOn(ind){ 
 var obj=window.eventList[ind]; 
 window.eventList[ind]=null; 
 if (obj.NextStep) obj.NextStep(); 
 else obj(); 
} 


function Test(name){ 
 alert(name); 
 Pause(this,10000);//调用暂停函数 
 this.NextStep=function hello(name){ 
  alert('hello'+name); 
} 
} 

Test('dai');
*/

//权限按行选函数
function selectRow(rowId, check) {

    try {
        document.getElementById("viewperm" + rowId).checked = check;
        document.getElementById("postperm" + rowId).checked = check;
        document.getElementById("replyperm" + rowId).checked = check;
        document.getElementById("getattachperm" + rowId).checked = check;
        document.getElementById("postattachperm" + rowId).checked = check;
    }
    catch (e) {
    }
}
//权限按列选函数
function seleCol(colPerfix,check)
{
	var obj;
	var i = 1;
	while(true)
	{
		obj = document.getElementById(colPerfix + i);
		if(obj == null) break;
		obj.checked = check;
		i++;
	}
}

function setSeleColDisplay(colPerfix, display) {
    var obj;
    var i = 1;
    var str = display ? 'none' : '';

    while (true) {
        obj = document.getElementById(colPerfix + i);
        if (obj == null) break;
        obj.style.display = str;
        i++;
    }
}
   

function changeDeleteModeState(item,form)
{
	switch(item)
	{
		case 1:
			document.getElementById("chkall").disabled = false;
			document.getElementById("deleteNum").disabled = document.getElementById("deleteFrom_deleteFrom").disabled = true;
			enableCheckBox(false,form);
			document.getElementById("deleteNum").value = "";
			document.getElementById("deleteFrom_deleteFrom").value = "";
	        Check(form);
			break;
		case 2:
			document.getElementById("deleteNum").disabled = false;
			document.getElementById("chkall").disabled = document.getElementById("deleteFrom_deleteFrom").disabled = true;
			enableCheckBox(true,form);
			document.getElementById("chkall").checked = false;			
			document.getElementById("deleteFrom_deleteFrom").value = "";
			DeleteMode2SetStatus();
			break;
		case 3:
			document.getElementById("deleteFrom_deleteFrom").disabled = false;
			document.getElementById("chkall").disabled = document.getElementById("deleteNum").disabled = true;
			enableCheckBox(true,form);
			document.getElementById("chkall").checked = false;			
			document.getElementById("deleteNum").value = "";
			DeleteMode3SetStatus();
			break;
	}
}  

function enableCheckBox(b,form)
{
	for (var i=0;i<form.elements.length;i++)
	{
		var e = form.elements[i];
		if (e.type == "checkbox")
		{
			e.disabled = b;
			e.checked = false;
		}
	}
} 

function isie()
{
   if(navigator.userAgent.toLowerCase().indexOf('msie') != -1)
   {
       return true;
   }
   else
   {
       return false;
   }
}  


//显示提示层
function showhintinfo(obj, objleftoffset,objtopoffset, title, info , objheight, showtype ,objtopfirefoxoffset)
{
   
   var p = getposition(obj);
   
   if((showtype==null)||(showtype =="")) 
   {
       showtype =="up";
   }
   document.getElementById('hintiframe'+showtype).style.height= objheight + "px";
   document.getElementById('hintinfo'+showtype).innerHTML = info;
   document.getElementById('hintdiv'+showtype).style.display='block';
   
   if(objtopfirefoxoffset != null && objtopfirefoxoffset !=0 && !isie())
   {
        document.getElementById('hintdiv'+showtype).style.top=p['y']+parseInt(objtopfirefoxoffset)+"px";
   }
   else
   {
        if(objtopoffset == 0)
        { 
			if(showtype=="up")
			{
				 document.getElementById('hintdiv'+showtype).style.top=p['y']-document.getElementById('hintinfo'+showtype).offsetHeight-40+"px";
			}
			else
			{
				 document.getElementById('hintdiv'+showtype).style.top=p['y']+obj.offsetHeight+5+"px";
			}
        }
        else
        {
			document.getElementById('hintdiv'+showtype).style.top=p['y']+objtopoffset+"px";
        }
   }
   document.getElementById('hintdiv'+showtype).style.left=p['x']+objleftoffset+"px";
}



//隐藏提示层
function hidehintinfo()
{
    document.getElementById('hintdivup').style.display='none';
    document.getElementById('hintdivdown').style.display='none';
}



//得到字符串长度
function getLen( str) 
{
   var totallength=0;
   
   for (var i=0;i<str.length;i++)
   {
     var intCode=str.charCodeAt(i);   
     if (intCode>=0&&intCode<=128)
     {
        totallength=totallength+1; //非中文单个字符长度加 1
	 }
     else
     {
        totallength=totallength+2; //中文字符长度则加 2
     }
   } 
   return totallength;
}   
   


function getposition(obj)
{
	var r = new Array();
	r['x'] = obj.offsetLeft;
	r['y'] = obj.offsetTop;
	while(obj = obj.offsetParent)
	{
		r['x'] += obj.offsetLeft;
		r['y'] += obj.offsetTop;
	}
	return r;
}

  

function cancelbubble(obj)
{
    //<textarea style="width:400px"></textarea>
    //var log = document.getElementsByTagName('textarea')[0];
	var all = obj.getElementsByTagName('*');
	
	for (var i = 0 ; i < all.length; i++)
	{
	    //log.value +=  all[i].nodeName +":" +all[i].id + "\r\n";
		all[i].onmouseover = function(e)
		{
    		if (e) //停止事件冒泡
	    	    e.stopPropagation();
		    else
			    window.event.cancelBubble = true;
			
			obj.style.display='block';
			//this.style.border = '1px solid white';
			//log.value = '鼠标现在进入的是： ' + this.nodeName + "_" + this.id;
		};
		
		all[i].onmouseout = function(e)
		{
		    if (e) //停止事件冒泡
			    e.stopPropagation();
		    else
			    window.event.cancelBubble = true;
			
	 
			if(this.nodeName == "DIV")
			{
			    obj.style.display='none';
			}
//			else
//			{
//			    obj.style.display='none';
//			}
			//this.style.border = '1px solid white';
			//log.value = '鼠标现在离开的是：' + this.nodeName + "_" + this.id;
	    };
	}

}

//当指定name的复选框选中时，激活相应的按钮
//arguments[0]为指定form，arguments[1]为复选框的name，arguments[2]～arguments[arguments.length - 1]为要激活的按钮
function checkedEnabledButton()
{
    for (var i = 0; i < arguments[0].elements.length; i++)
    {
        var e = arguments[0].elements[i];
        if (e.name == arguments[1] && e.checked)
        {
            for(var j = 2; j < arguments.length; j++)
            {
                document.getElementById(arguments[j]).disabled = false;
            }
            return;
        }
    }
    for(var j = 2; j < arguments.length; j++)
    {
        document.getElementById(arguments[j]).disabled = true;
    }
}

function isNumber(str)
{
    return (/^[+|-]?\d+$/.test(str));
}