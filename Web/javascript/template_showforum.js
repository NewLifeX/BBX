function addslashes(str) {
	return preg_replace(['\\\\', '\\\'', '\\\/', '\\\(', '\\\)', '\\\[', '\\\]', '\\\{', '\\\}', '\\\^', '\\\$', '\\\?', '\\\.', '\\\*', '\\\+', '\\\|'], ['\\\\', '\\\'', '\\/', '\\(', '\\)', '\\[', '\\]', '\\{', '\\}', '\\^', '\\$', '\\?', '\\.', '\\*', '\\+', '\\|'], str);
}

function preg_replace(search, replace, str) {
	var len = search.length;
	for(var i = 0; i < len; i++) {
		re = new RegExp(search[i], "ig");
		str = str.replace(re, typeof replace == 'string' ? replace : (replace[i] ? replace[i] : replace[0]));
	}
	return str;
}

function fastdalert(s,editorkey) {
		$(editorkey+'validatemessage').className = 'onerror';
		$(editorkey+'validatemessage').innerHTML = s;
}


function fastvalidate(theform, editorkey, previewpost, switcheditormode) {
    var originalmessage = theform.message ? theform.message.value : $(editorkey + "message").value
    var message = !theform.parseurloff.checked ? parseurl(originalmessage) : originalmessage;

    if ((theform.typeid) && (theform.postbytopictype.value == "1") && (theform.typeid.value == "0")) {
        fastdalert("请选择相应的主题分类。", editorkey);
        $(editorkey + "submit").disabled = false;
        return false;
    } else if ($(editorkey + 'title').value == "" || message == "") {
        fastdalert("请完成标题和内容栏。", editorkey);
        $(editorkey + "submit").disabled = false;
        return false;
    } else if ($(editorkey + 'title').value.length > 60) {
        fastdalert("您的标题超过 60 个字符的限制。", editorkey);
        theform.title.focus();
        $(editorkey + "submit").disabled = false;
        return false;
    }

    if (!disablepostctrl && ((postminchars != 0 && mb_strlen(message) < postminchars) || (postmaxchars != 0 && mb_strlen(message) > postmaxchars))) {
        fastdalert("您的帖子长度不符合要求。\n\n当前长度: " + mb_strlen(message) + " 字节\n系统限制: " + postminchars + " 到 " + postmaxchars + " 字节", editorkey);
        return false;
    }

    if (!switcheditormode && !previewpost) {
        $(editorkey + "submit").disabled = true;
    }

    try {
        theform.message.value = message;
    } catch (e) {
        $(editorkey + "message").value = message;
    }
    return true;
}


function gotopage(topicid,index){
	page = parseInt($("divInput_pagenumber_" + topicid).value);
	if (is_ie){
		curpage = parseInt($("div_curpage_" + topicid).innerText);
		total = parseInt($("div_Totalpage_" + topicid).innerText);
	}
	else{
		curpage = parseInt($("div_curpage_" + topicid).textContent);
		total = parseInt($("div_Totalpage_" + topicid).textContent);
	}
	if (!(parseInt(page)==page && page>0)){
		alert("页码无效");
		return false;
	}
	
	if (!(parseInt(curpage)==curpage && curpage>0)){
		curpage = page;
	}
	
	if (!(parseInt(total)==total && total>0)){
		alert("总页码无效");
		return false;
	}
	
	if (curpage>total || curpage < 1){
		alert("页码无效");
		return false;
	}
	
	
	switch (index){
		case 1:
			if (page-1<1){
				alert("已经是第一页");
				return false
			}
			$("divFollow_" + topicid + "_" + curpage).style.display="none";
			$("divFollow_" + topicid + "_" + (curpage-1)).style.display="block";
			$("divInput_pagenumber_" + topicid).value = curpage - 1;
			break;
		case 2:
			if (page+1>total){
				alert("已经到达最后一页");
				return false
			}
			$("divFollow_" + topicid + "_" + curpage).style.display="none";
			$("divFollow_" + topicid + "_" + (parseInt(curpage) +1)).style.display="block";
			$("divInput_pagenumber_" + topicid).value = parseInt(curpage) + 1;
			break;
		case 3:
			if (page>total || page<1){
				alert("页码应在 1-" + total + " 之间");
				return false
			}
			$("divFollow_" + topicid + "_" + curpage).style.display="none";
			$("divFollow_" + topicid + "_" + page).style.display="block";
			break;
	}
	if (is_ie){
		$("div_curpage_" + topicid).innerText = $("divInput_pagenumber_" + topicid).value;
	}
	else{
		$("div_curpage_" + topicid).textContent = $("divInput_pagenumber_" + topicid).value;
	}
}

function writetree(obj,topicid,ppp,aspxrewrite){
	var divTopic = $("divTopic" + topicid);

	var err = obj.getElementsByTagName('error');
	if (err[0] != null && err[0] != undefined)
	{
		var errdiv = document.createElement("DIV");
		errdiv.className='errcontext';

	    if (err[0].childNodes.length > 1) {
		    errdiv.innerHTML = '<div style="margin: 0px 15px 0px 40px;">' + err[0].childNodes[1].nodeValue + '</div>';
		} else {
		    errdiv.innerHTML = '<div style="margin: 0px 15px 0px 40px;">' + err[0].firstChild.nodeValue + '</div>';    		
		}

		divTopic.appendChild(errdiv);
		$("divTopic" + topicid).style.display = "";
		$("divShowContext_" + topicid).style.display = "none";
		return;
	}

	var dataArray = obj.getElementsByTagName('post');
	var dataArrayLen = dataArray.length;
		var page = 0;
		var divIsShow = "block";
		for(var i = 0; i < dataArrayLen; i++){
			page ++;
			div = document.createElement("DIV");
			div.className = "openlist";
			div.id = "divFollow_" + topicid + "_" + page;
			div.style.display = divIsShow;
			div.style.lineHeight = "180%";
			var list = "<ul class=\"listcontent\">";
			for (j=0;j<10;j++){
			    if(aspxrewrite == 1)
			    {
				    list += "<li><a href=\""+forumurl+"showtopic-";
				    list += topicid;
				    if (ppp>0){
					    list += "-";
					    list += Math.ceil((i+2)/ppp);
				    }
				    list += ".aspx#";
				    list += dataArray[i].getAttribute("pid");
				    list +="\">";
				}
				else
				{
				    list += "<li><a href=\"" + forumurl + "showtopic.aspx"
				    list += "?topicid=";
				    list += topicid;
				    if (ppp>0){
					    list += "&page=";
					    list += Math.ceil((i+2)/ppp);
				    }
				    list += "#" +dataArray[i].getAttribute("pid");
				    list +="\">";
				}
				list += dataArray[i].getAttribute("message").substring(0,50);
				list += "</a>&nbsp; --- &nbsp; (";
				if (dataArray[i].getAttribute("poster")!="游客")
				{
					list += "<a target=\"_blank\" href=\""+forumurl+"userinfo.aspx?userid=";
					list += dataArray[i].getAttribute("posterid");
					list += "\">";
					list += dataArray[i].getAttribute("poster");
					list += "</a> 发表于 ";
				}
				else
				{
					list += "游客 发表于";
				}
				
				list += dataArray[i].getAttribute("postdatetime");
				list += ")</li>";
				i++;
				if (i>=dataArrayLen){
					break;
				}
			}
			list += "</ul>";
			if ( list != "<ul class=\"listcontent\"></ul>" )
			{
			
				i--;
				
			}

			div.innerHTML = list;
			
			divIsShow = "none";
			divTopic.appendChild(div);
		}
		
		divpage = document.createElement("DIV");
		divpage.id = "divFollow_page_" + topicid;
		divpage.className = "pagecontent";
		divpage.style.marginTop = "5px"
		divpage.innerHTML = "页数:<span id=\"div_curpage_" + topicid + "\">1</span>/<span id=\"div_Totalpage_" + topicid + "\">" + page + "</span>&nbsp;&nbsp;&nbsp;<span onclick=\"gotopage(" + topicid + ",1);\" style=\"cursor:pointer;\" title=\"上一页\"><img src=\"" + imagedir + "/prev.gif\" /></span>&nbsp;<input type=\"text\" id=\"divInput_pagenumber_" + topicid + "\" value=\"1\" size=\"3\" style=\"text-align:center\" class=\"colorblue\" onKeyDown=\"if(event.keyCode==13) { gotopage(" + topicid + ",3); return false; }\">&nbsp;<span onclick=\"gotopage(" + topicid + ",2);\" style=\"cursor:pointer;\" title=\"下一页\"><img src=\"" + imagedir + "/next.gif\" /></span>"
		divTopic.appendChild(divpage);
		divTopic.className = "pagediv";
		$("divTopic" + topicid).style.display = "";
		$("divShowContext_" + topicid).style.display = "none";
		
}

function showtree(topicid,ppp,aspxrewrite){
	var imgsrcCol = imagedir + "/topItem_col.gif";
	var imgsrcExp = imagedir + "/topItem_exp.gif"
	
	var divTopic = $("divTopic" + topicid);
	if (divTopic.innerHTML =="")
	{	
		div = document.createElement("DIV");
		div.id = "divShowContext_" + topicid;
		div.className = "openlist";
		div.innerHTML = "<img src='" +forumurl+"images/common/loading.gif' />载入中...";
		
		divTopic.appendChild(div);
		if(aspxrewrite == 1)
		{
		    ajaxRead(forumurl + "tools/ajax.ashx?t=topictree&topicid=" + topicid, "writetree(obj," + topicid + "," + ppp + "," + aspxrewrite + ");");
		}
		else
		{
		    ajaxRead(forumurl + "tools/ajax.ashx?t=topictree&topicid=" + topicid, "writetree(obj," + topicid + "," + ppp + ");");
		}
	}

	if ($("imgButton_" + topicid).src.indexOf(imgsrcCol)!=-1){	
		$("imgButton_" + topicid).src = imgsrcExp;
		$("imgButton_" + topicid).alt = "展开帖子列表";
		$("imgButton_" + topicid).title = "展开帖子列表";

		$("divTopic" + topicid).style.display = "none";
	}
	else{
		$("imgButton_" + topicid).src = imgsrcCol;
		$("imgButton_" + topicid).alt = "关闭帖子列表";
		$("imgButton_" + topicid).title = "关闭帖子列表";
		if ($("divTopic" + topicid)){
			$("divTopic" + topicid).style.display = "";
		}
	}

}

modclickcount = 0;
function modclick(obj)
{
    if (obj.checked)
    {
        modclickcount++;
    } else
    {
        modclickcount--;
    }
    $('modcount').innerHTML = modclickcount;
    if (modclickcount > 0)
    {
        var top_offset = obj.offsetTop;
        while ((obj = obj.offsetParent).id != 'threadlist')
        {
            top_offset += obj.offsetTop;
        }
        $('modlayer').style.top = top_offset - 7 + 'px';
        $('modlayer').style.display = '';
    } else
    {
        $('modlayer').style.display = 'none';
    }
}
function modthreads(optgroup, operation)
{
    var checked = 0;
    var operation = !operation ? '' : operation;
//    var windowWidth = 250;
//    var windowHeight = 220;
//    switch (operation)
//    {
//        case "identify":
//            windowWidth = 210;
//            windowHeight = 260;
//            break;
//        /*case "delete":
//            windowWidth = 250;
//            break;
//        case "repair":
//            windowHeight = 260;
//            break;
//        case "merge":
//            windowHeight = 310;
//            break;*/
//        default:
//            windowHeight = 220;
//    }
hideWindow('mods');
    for (var i = 0; i < $('moderate').elements.length; i++)
    {
        if ($('moderate').elements[i].name.match('topicid') && $('moderate').elements[i].checked)
        {
            checked = 1;
            break;
        }
    }
    if (!checked)
    {
        alert('请选择需要操作的帖子');
    } else
    {
        
        $('moderate').optgroup.value = optgroup;
        $('moderate').operat.value = operation;
        showWindow('mods', 'moderate', 'post', 0);

        if (BROWSER.ie) {
            doane(event);
        }	
    }
}

function atarget(obj) {
	obj.target = getcookie('atarget') ? '_blank' : '';
}

function setatarget(v) {
	$('atarget').className = 'y atarget_' + v;
	$('atarget').onclick = function() { setatarget(v ? 0 : 1); };
	setcookie('atarget', v, (v ? 2592000 : -1));
}

window.onload = function () {
    if ($('atarget') != null) {
        if (getcookie('atarget') == '1') {
            $('atarget').className = 'y atarget_1';
            $('atarget').onclick = function () { setatarget(0); };
        }
        else {
            $('atarget').className = 'y atarget_0';
            $('atarget').onclick = function () { setatarget(1); };
        }
    }

    if ($("threadlist") != null) {
        var objs = $("threadlist").childNodes;
        for (var i = 0; i < objs.length; i++) {
            if (objs[i].nodeName == "TBODY")
                objs[i].onmouseover = function () {
                    if (this.className != 'separation')
                        this.className = 'onthread';
                }
            objs[i].onmouseout = function () {
                if (this.className != 'separation')
                    BROWSER.ie ? this.removeAttribute("className", false) : this.removeAttribute("class");
            }
        }
        var obj = document.getElementsByTagName("INPUT")
        for (var i = 0; i < obj.length; i++) {
            if (obj[i].type == "checkbox")
                obj[i].checked = "";
        }
    }
    currenttopictype();
}

function currenttopictype() {
    if ($('headfilter') != null) {
        var typeid = getQueryString('typeid');
        var filter = getQueryString('filter');
        var els = $('headfilter').childNodes;
        if (filter != '') {
            for (i = 2; i < els.length; i++) {
                if (els[i].nodeName == 'A') {
                    var temphref = els[i].href;
                    if (temphref.indexOf('typeid=') != -1) {
                        els[i].className = '';
                    }
                }
            }
        }

        if (filter == '' && typeid == '') {
            if (BROWSER.ie) {
                if (typeof(els[0] )!= 'undefined') 
                    els[0].className = 'current';
                }
            else {
                if(typeof(els[1])!='undefined')
                    els[1].className = 'current';
            }
        }
        else if(typeid != '') {
            for (i = 2; i < els.length; i++) {
                if (els[i].nodeName == 'A') {
                    var temphref = els[i].href;
                    if (temphref.indexOf('typeid=' + typeid) != -1) {
                        els[i].className = 'current';
                        break;
                    }
                }
            }
        }
    }
}