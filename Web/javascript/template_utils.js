function MinPhotoSize(actual, maxvalue) {
	return Math.min(actual, maxvalue) + "px";
}

function ShowFormatBytesStr(bytes) {
	if(bytes > 1073741824) {
		document.write((Math.round((bytes/1073741824)*100)/100).toString()+' G');
	} else if(bytes > 1048576) {
		document.write((Math.round((bytes/1048576)*100)/100).toString()+' M');
	} else if(bytes > 1024) {
		document.write((Math.round((bytes/1024)*100)/100).toString()+' K');
	} else {
		document.write(bytes.toString()+' Bytes');
	}
}

function MouseCursor(obj) {
	if (is_ie)
		obj.style.cursor = 'hand';
	else
		obj.style.cursor = 'pointer';
}

function convertdate(strdate) {
	strdate = strdate.replace(/-/ig,'/');
	var d = new Date(strdate);
	var now = new Date();
	var result;

	if (d.getYear() == now.getYear() && d.getMonth() == now.getMonth()) {
		var xday = now.getDate() - d.getDate();

		switch (xday) {
			case 0:
				result = "今天 " + d.format("hh") + ":" + d.format("mm");
				break;
			case 1:
				result = "昨天 " + d.format("hh") + ":" + d.format("mm");
				break;
			case 2:
				result = "前天 " + d.format("hh") + ":" + d.format("mm");
				break;
			default:
				result = d.format("yyyy-MM-dd hh:mm");
				break;		
		}
	} else {
		result = d.format("yyyy-MM-dd hh:mm");
	}
	
	return result;
}

function convertdate2(strdate)
{
	strdate = strdate.replace(/-/ig,'/');
	var d = new Date(strdate);
	var now = new Date();
	var result = now - d;
	if (now.getYear() == d.getYear() && now.getMonth() == d.getMonth() && now.getDate() - d.getDate() > 0){
		result = convertdate(strdate);
	} else if (now.getYear() == d.getYear() && now.getMonth() == d.getMonth() && now.getDate() == d.getDate() && now.getHours() - d.getHours() > 0){
		result = convertdate(strdate);
	} else if (now.getYear() == d.getYear() && now.getMonth() == d.getMonth() && now.getDate() == d.getDate() && now.getHours() == d.getHours() && now.getMinutes() - d.getMinutes() > 0){
		result = (now.getMinutes() - d.getMinutes()) + " 分钟前"
	} else if (now.getYear() == d.getYear() && now.getMonth() == d.getMonth() && now.getDate() == d.getDate() && now.getHours() == d.getHours() && now.getMinutes() == d.getMinutes() && now.getSeconds() - d.getSeconds()> 0){
		result = (now.getSeconds() - d.getSeconds()) + " 秒前"
	} else {
		result = d.format("yyyy-MM-dd hh:mm");
	}
	return result;

}

Date.prototype.format = function(format) {
	var o = {
	"M+" : this.getMonth()+1, //month
	"d+" : this.getDate(),    //day
	"h+" : this.getHours(),   //hour
	"m+" : this.getMinutes(), //minute
	"s+" : this.getSeconds(), //second
	"q+" : Math.floor((this.getMonth()+3)/3),  //quarter
	"S" : this.getMilliseconds() //millisecond
	};
	if(/(y+)/.test(format)) {
		format = format.replace(RegExp.$1,
			(this.getFullYear() + "").substr(4 - RegExp.$1.length));
	}
	for(var k in o) {
		if(new RegExp("("+ k +")").test(format))
			format = format.replace(RegExp.$1,
				RegExp.$1.length==1 ? o[k] : ("00"+ o[k]).substr((""+ o[k]).length));
	}
	return format;
}

function findobj(n, d) {
	var p, i, x;
	if(!d) d = document;
	if((p = n.indexOf("?"))>0 && parent.frames.length) {
		d = parent.frames[n.substring(p + 1)].document;
		n = n.substring(0, p);
	}
	if(x != d[n] && d.all) x = d.all[n];
	for(i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
	for(i = 0; !x && d.layers && i < d.layers.length; i++) x = findobj(n, d.layers[i].document);
	if(!x && document.getElementById) x = document.getElementById(n);
	return x;
}


function expandoptions(id) {
	var a = document.getElementById(id);
	if(a.style.display=='')
	{
		a.style.display='none';
	}
	else
	{
		a.style.display='';
	}
}

function cloneObj(oClone, oParent, count)
{
    var elementCount = 0;
    //重新计数HTML元素个数，排除文本节点
    for (var i = 0; i < oParent.childNodes.length; i++)
    {
        if (oParent.childNodes[i].nodeType == 1 && oParent.childNodes[i].tagName.toLowerCase() == "p")
            elementCount++;
    }
    if (elementCount < count) 
	{
		var newNode = oClone.cloneNode(true);
		oParent.appendChild(newNode);
		
		return newNode;
	} 
	return false;	
}

function delObj(oParent, count, currentObj)
{
    var elementCount = 0;
    //重新计数HTML元素个数，排除文本节点
    for (var i = 0; i < oParent.childNodes.length; i++)
    {
        if (oParent.childNodes[i].nodeType == 1 && oParent.childNodes[i].tagName.toLowerCase() == "p")
            elementCount++;
    }
    if (elementCount > count)
    {
        if (currentObj)
            oParent.removeChild(currentObj);
        else {
            var pArray = oParent.getElementsByTagName("p");
            oParent.removeChild(pArray[pArray.length - 1]);
        }
		return true;
	}
	return false;
}

function cloneObj_1(oClone, oParent, i, count, msgtext) {
	var tempcount = 1;
	for(k=0;k<oParent['childNodes'].length;k++){
		if (oParent['childNodes'][k].tagName){
			if (oParent['childNodes'][k].id == oClone.id){
				tempcount ++;
			}
		}
	}

	if(tempcount <= count) {
		for(;i>0;i--) {
			newNode = oClone.cloneNode(true);
			oParent.appendChild(newNode);
		}
	} else {
		alert(msgtext);
	}
}

function clonePoll(maxpoll) {
    var cloneItem = $("polloptions").getElementsByTagName("p")[0];
	var newNode = cloneObj(cloneItem, $('polloptions') ,parseInt(maxpoll));
	if(!newNode){
		alert('投票项不能多于 ' + maxpoll + ' 个');
	}
	var inputs = findtags(newNode, 'input');
	var attach;
	for(i in inputs) {
	    inputs[i].value="";
		if(inputs[i].name == 'pollitemid') {
			
			inputs[i].id = "pollitemid";
		}
	}
}

function delOjb_1(oParent, count, msgtext) {
	var tempcount = 0;
	for(k=0;k<oParent['childNodes'].length;k++){
		if (oParent['childNodes'][k].tagName){
				tempcount ++;
		}
	}
	
	if(tempcount > count) {
		oParent.removeChild(oParent.lastChild);
	} else {
		alert(msgtext);
	}
}

//选择或取消选反列表中全部记录
//与DZ的Js冲突
/*function checkall(form, prefix, checkall) {
	var checkall = checkall ? checkall : 'chkall';
	for(var i = 0; i < form.elements.length; i++) {
		var e = form.elements[i];
		if(e.name != checkall && (!prefix || (prefix && e.name.match(prefix)))) {
			e.checked = form.elements[checkall].checked;
		}
	}
}*/

//显示主题图标
function showicons(icons,iconscount,iconscolcount){
	var row=null;
	var col=null;
	var img=null;
	var rowIndex=0;
	var colIndex=0;
	var iCount = 0;
	
	if(typeof(iconscount) == 'undefined') {
		var iconscount = 0;
	}
	
	if(typeof(iconscolcount) == 'undefined') {
		var iconscolcount = 0;
	}
	

	try{

		var icons_container = findobj('iconsdiv');
		var iconstable = document.createElement('table');
				iconstable.cellPadding="2";
				iconstable.cellSpacing="0";
				iconstable.border=0;
				//iconstable.className="altbg1";
				
				iconstable.style.border="0px";
				
				iconstable.id="topiciconstable";
				
		if (!icons.length){
			iCount = 0;
		}
		else{
			iCount = icons.length
		}
		
		if (iconscount > 0 ){
			if (iCount > iconscount){
				iCount = iconscount			
			}
		}
		
		iCount = iCount + 1;
		if (iconscolcount <1){
			iconscolcount = parseInt((iCount + 1) / 2)
		}

		
		var temp_iCount = 1;
		
		row=iconstable.insertRow(-1);		
		col=row.insertCell(-1);
		col.vAlign="middle";
		col.align = "left";
		col.width = "49";
		col.innerHTML = '<input type="radio" id="icon_0" name="iconid" value="0"> <label for="icon_0">无</label>';
		
		colIndex++;
		
		for(i=0;i<icons.length;i++)
		{
		
			if (icons[i]){
				temp_iCount ++;
				if (temp_iCount>iCount){
					break;
				}
				
				if (colIndex>=iconscolcount || colIndex<1){
					row=iconstable.insertRow(-1);		
					colIndex=0;
					
				}
				col=row.insertCell(-1);
				col.vAlign="middle";
				col.align = "left";
				col.width = "49";
				col.innerHTML = '<input type="radio" id="icon_' + icons[i][0] + '" name="iconid" value="' + icons[i][0] + '"> <img src="images/posticons/' + icons[i][1] + '" width="19" height="19" />';
/* 
				input = document.createElement('input');
				input.type = "radio";
				input.value = icons[i][0];
				input.id = "icon_" + icons[i][0];
				input.name = "iconid";
				col.appendChild(input);
				
				img=document.createElement('img');
				img.src="images/posticons/" + icons[i][1];
				img.alt=smilies[i][1];
				img.border=0;
				col.appendChild(img);
 */	
				colIndex++;
				
			}
		}
		
		for (i=colIndex;i<iconscolcount;i++) {
			if (row!=null){
				col=row.insertCell(-1);
				col.vAlign="top";
				col.innerHTML="&nbsp;";
			}
		}
		
		icons_container.appendChild(iconstable);
		
	}
	catch(e){
		alert(e.message);
	}
}


function getpageurl(url,value){
	return url.replace(/\$page/ig,value);	
}

///
///
function getpagenumbers(extname, recordcount,pagesize,mode,title, topicid, page, url, aspxrewrite ){
	var pagecount = 0;
	var pagenumbers = "";
	if (recordcount<=pagesize || pagesize <= 0){
		return;
	}
	if (!mode){
		mode = 0;
	}
	switch(mode){
		case 0:
			/*
				   <script language="javascript">getpagenumbers({topic[replis]},{config.tpp});</script> 
			*/
			recordcount ++;		//帖子数自动加1(主题帖)
			pagecount = parseInt(Math.ceil(recordcount*1.0/pagesize*1.0));
			pagenumbers = "[" + title;
			for (i=1;i<=pagecount;i++){
				if (i>5){
					pagenumbers = pagenumbers + "...";
					i=pagecount;
				}
				if(aspxrewrite==1) {
				    pagenumbers = pagenumbers + "<a href=\""+url+"showtopic-" + topicid + "-" + i + extname + "\">" + i + "</a>";
				} else {
				    pagenumbers = pagenumbers + "<a href=\""+url+"showtopic.aspx?topicid=" + topicid + "&page=" + i + "\">" + i + "</a>";
				}
			}
			pagenumbers += "]";
			break;
		case 1:
		
			/*
				   <script language="javascript">getpagenumbers({topiccount},{config.tpp},1,'{request[page]}',"showforum-{forumid}-$page.aspx");</script> 
			*/
			
			pagecount = parseInt(Math.ceil(recordcount*1.0/pagesize*1.0));
			if (page=="" || page<=0){
				page = 1;
			}
			page=parseInt(page);
			pagenumbers += '<div class="p_bar">\n';
			pagenumbers += '	<span class="p_total">&nbsp;' + recordcount + '&nbsp;</span>\n';
			pagenumbers += '	<span class="p_pages">&nbsp;' + page + ' / ' + pagecount + '&nbsp;</span>';
			if (page <= 1) {
				pagenumbers += '	<span title="上一页" class="p_redirect">&lsaquo;&lsaquo;</span>\n';
			} else {
				pagenumbers += '	<a href="' + getpageurl(url,page-1) + '" class="p_redirect">&lsaquo;&lsaquo;</a>\n';
			}
			if (page != 1) pagenumbers += '	<a href="' + getpageurl(url,1) + '" class="p_num">1</a>\n';
			if (page >= 5) pagenumbers += '<span class="p_num">...</span>\n';
			if (pagecount > page + 2) {
				var endPage = page + 2;
			} else {
				var endPage = pagecount;
			}
			
			for (var i = page - 2; i <= endPage; i++) {
				if (i > 0) {
					if (i == page) {
						pagenumbers += '<span class="p_curpage">' + i + '</span>';
					} else {
						if (i != 1 && i != pagecount) {
							pagenumbers += '<a href="' + getpageurl(url,i) + '" class="p_num">' + i + '</a>';
						}
					}
				}
			}
			if ((page + 3) < pagecount) pagenumbers += '<span class="p_num">...</span>\n';
			if (page != pagecount) pagenumbers += '<a href="' + getpageurl(url,pagecount) + '" class="p_num">' + pagecount + '</a>';
			
		
			if (page >= pagecount) {
				pagenumbers += '<span class="p_redirect">&rsaquo;&rsaquo;</span>';
			} else {
				pagenumbers += '<a href="' + getpageurl(url,pagecount) + '" class="p_num">&rsaquo;&rsaquo;</a>';
			}
			
			pagenumbers += '<span class="p_num"><input name="gopage" type="text" class="p_input" id="gopage" onKeyDown="if(event.keyCode==13) {window.location=\'' + getpageurl(url,"\'+this.value + \'") + '\';}" size="4" maxlength="9" value="转到" onmouseover="this.select();" /></span>';
			pagenumbers += '</div>';
			break;
	}
	document.write(pagenumbers);
}

function showPopupText(event) {	
	if(event.srcElement) o = event.srcElement; else o = event.target;
	if (!o) return;
	MouseX = event.clientX;
	MouseY = event.clientY;
	if(o.alt != null && o.alt!="") { o.pop = o.alt;o.alt = "" }
	if(o.title != null && o.title != ""){ o.pop = o.title;o.title = "" }
	if(o.pop != sPop) {
		sPop = o.pop;
		if(sPop == null || sPop == "") {
			document.getElementById("popLayer").style.visibility = "hidden";
		} else {
			if(o.dyclass != null) popStyle = o.dyclass; else popStyle = "cPopText";
			document.getElementById("popLayer").style.visibility = "visible";
			showIt();
		}
	}
}

function showIt() {
	document.getElementById("popLayer").className = popStyle;
	document.getElementById("popLayer").innerHTML = sPop.replace(/<(.*)>/g,"&lt;$1&gt;").replace(/\n/g,"<br>");;
	popWidth = document.getElementById("popLayer").clientWidth;
	popHeight = document.getElementById("popLayer").clientHeight;
	if(MouseX + 12 + popWidth > document.body.clientWidth) popLeftAdjust = -popWidth - 24; else popLeftAdjust = 0;
	if(MouseY + 12 + popHeight > document.body.clientHeight) popTopAdjust = -popHeight - 24; else popTopAdjust = 0;
	document.getElementById("popLayer").style.left = MouseX + 12 + document.body.scrollLeft + popLeftAdjust;
	document.getElementById("popLayer").style.top = MouseY + 12 + document.body.scrollTop + popTopAdjust;
}

