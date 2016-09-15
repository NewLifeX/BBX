function showgoodsinfo(objid){
     $('showdetail').style.display = 'none';
     $('othermessage').style.display = 'none';
     $('pricelist').style.display = 'none';
     $('messagebox').style.display = 'none';
     
     $('li_showdetail').className = '';
     $('li_othermessage').className = '';
     $('li_pricelist').className = '';
     $('li_messagebox').className = '';
     
     if(objid == 'showdetail'){
        $('othermessage').style.display = 'block';
        $('pricelist').style.display = 'block';
        $('messagebox').style.display = 'block';
        $('othermessage_h3').style.display = 'block';
        $('pricelist_h3').style.display = 'block';
        $('messagebox_h3').style.display = 'block';
     }
     else {
        $(objid +'_h3').style.display = 'none';
        $(objid).style.background = 'none';
     }
     
     $('li_' + objid).className = "current";  
     $(objid).style.display = "block";  
}



function imgzoom(o) {
	if(event.ctrlKey) {
		var zoom = parseInt(o.style.zoom, 10) || 100;
		zoom -= event.wheelDelta / 12;
		if(zoom > 0) {
			o.style.zoom = zoom + '%';
		}
		return false;
	}
	else {
		return true;
	}
}
 

/*************************************************分页函数****************************************************/	

function  ajaxpagination(ajaxfunction, recordcount, pagesize, currentpage, divname) {
    
	var allcurrentpage = 0;
	var next = 0;
	var pre = 0;
	var startcount = 0;
	var endcount = 0;
	var currentpagestr = '<BR />';

	if (currentpage < 1) { 
        currentpage = 1; 
    }

	//计算总页数
	if (pagesize != 0) {
		allcurrentpage = parseInt((recordcount / pagesize));
		allcurrentpage = ((recordcount % pagesize) != 0 ? allcurrentpage + 1 : allcurrentpage);
		allcurrentpage = (allcurrentpage == 0 ? 1 : allcurrentpage);
	}
	next = currentpage + 1;
	pre = currentpage - 1;

    //中间页起始序号
	startcount = (currentpage + 5) > allcurrentpage ? allcurrentpage - 9 : currentpage - 4;
	
    //中间页终止序号
	endcount = currentpage < 5 ? 10 : currentpage + 5;

    //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
	if (startcount < 1) { 
        startcount = 1; 
    }

    //页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
	if (allcurrentpage < endcount) { 
        endcount = allcurrentpage; 
    }
	
	if(startcount>1) {
        currentpagestr += currentpage > 1 ? '&nbsp;&nbsp;<a href="###"  onclick="javascript:'+ajaxfunction+'(' + page_goodsid + ',' + pagesize + ', ' +currentpage +');" title="上一页">上一页</a>' : '';
	}
	
    //当页码数大于1时, 则显示页码
    if (endcount > 1) {
        //中间页处理, 这个增加时间复杂度，减小空间复杂度
        for (i = startcount; i <= endcount; i++) {
            currentpagestr += currentpage == i ? '&nbsp;' + i + '' : '&nbsp;<a href="###"  onclick="javascript:'+ajaxfunction+'(' + page_goodsid + ',' + pagesize + ', ' + i + ');">' + i + '</a>';
        }
    }
	
	if(endcount<allcurrentpage) {
        currentpagestr += currentpage != allcurrentpage ? '&nbsp;&nbsp;<a href="###" onclick="javascript:'+ajaxfunction+'(' + page_goodsid + ',' + pagesize + ', ' + next + ');" title="下一页">下一页</a>&nbsp;&nbsp;' : '';
	}

    if (endcount > 1) {
        currentpagestr += "&nbsp; &nbsp;";
    }

    if(allcurrentpage>1) {
        currentpagestr += currentpage + ' / ' + allcurrentpage + ' 页';// + recordcount + ' 条记录';
    }

	$(divname).innerHTML = (recordcount==0) ? '': currentpagestr;
}

/*************************************************图片点击显示代码****************************************************/

var msgwidth=0;
function attachimg(obj,action)
{
	obj.style.cursor='pointer';			
}

function attachimginfo(obj, infoobj, show, event) {
    
	objinfo = fetchOffset(obj);
	if(show) {
		$(infoobj).style.left = objinfo['left'] + 'px';
		$(infoobj).style.top = obj.offsetHeight < 40 ? (objinfo['top'] + obj.offsetHeight) + 'px' : objinfo['top'] + 'px';
		$(infoobj).style.display = '';
	} else {
	    
		if(is_ie) {
			$(infoobj).style.display = 'none';
			return;
		} else {
			var mousex = document.body.scrollLeft + event.clientX;
			var mousey = document.documentElement.scrollTop + event.clientY;
				$(infoobj).style.display = 'none';
		}
	}
}

var zoomobj = Array();var zoomadjust;var zoomstatus = 1;
function zoom(obj, zimg) {
	if(!zoomstatus) {
		window.open(zimg, '', '');
		return;
	}
	if(!zimg) {
		zimg = obj.src;
	}
	
	if(!$('zoomimglayer_bg')) {
		div = document.createElement('div');div.id = 'zoomimglayer_bg';
		div.style.position = 'absolute';
		div.style.left = div.style.top = '0px';
		div.style.width = '100%';
		div.style.height = document.body.scrollHeight + 'px';
		div.style.backgroundColor = '#000';
		div.style.display = 'none';
		div.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=80,finishOpacity=100,style=0)';
		div.style.opacity = 0.8;
		div.style.zIndex = 998;
		$('append_parent').appendChild(div);
		div = document.createElement('div');div.id = 'zoomimglayer';
		div.style.position = 'absolute';
		div.className = 'popupmenu_popup';
		div.style.padding = 0;
		$('append_parent').appendChild(div);
	}
	zoomobj['srcinfo'] = fetchOffset(obj);
	zoomobj['srcobj'] = obj;
	zoomobj['zimg'] = zimg;
	$('zoomimglayer').style.display = '';
	$('zoomimglayer').style.left = zoomobj['srcinfo']['left'] + 'px';
	$('zoomimglayer').style.top = zoomobj['srcinfo']['top'] + 'px';
	$('zoomimglayer').style.width = zoomobj['srcobj'].width + 'px';
	$('zoomimglayer').style.height = zoomobj['srcobj'].height + 'px';
	$('zoomimglayer').style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=40,finishOpacity=100,style=0)';
	$('zoomimglayer').style.opacity = 0.4;
	$('zoomimglayer').style.zIndex = 999;
	$('zoomimglayer').innerHTML = '<table width="100%" height="100%" cellspacing="0" cellpadding="0"><tr><td align="center" valign="middle"><img src="images/common/loading.gif"></td></tr></table><div style="position:absolute;top:-100000px;visibility:hidden"><img onload="zoomimgresize(this)" src="' + zoomobj['zimg'] + '"></div>';
}
var zoomdragstart = new Array();
var zoomclick = 0;
function zoomdrag(e, op) {
	if(op == 1) {
		zoomclick = 1;
		zoomdragstart = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
		zoomdragstart[2] = parseInt($('zoomimglayer').style.left);
		zoomdragstart[3] = parseInt($('zoomimglayer').style.top);
		doane(e);
	} else if(op == 2 && zoomdragstart[0]) {
		zoomclick = 0;
		var zoomdragnow = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
		$('zoomimglayer').style.left = (zoomdragstart[2] + zoomdragnow[0] - zoomdragstart[0]) + 'px';
		$('zoomimglayer').style.top = (zoomdragstart[3] + zoomdragnow[1] - zoomdragstart[1]) + 'px';
		doane(e);
	} else if(op == 3) {
		if(zoomclick) zoomclose();
		zoomdragstart = [];
		doane(e);
	}
}
function zoomimgresize(obj) {
	zoomobj['zimginfo'] = [obj.width, obj.height];
	var r = obj.width / obj.height;
	var w = document.body.clientWidth * 0.95;
	w = obj.width > w ? w : obj.width;
	var h = w / r;
	var clientHeight = document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight;
	var scrollTop = document.body.scrollTop ? document.body.scrollTop : document.documentElement.scrollTop;
	if(h > clientHeight) {
		h = clientHeight;
		w = h * r;
	}
	var l = (document.body.clientWidth - w) / 2;
	var t = h < clientHeight ? (clientHeight - h) / 2 : 0;
	t += + scrollTop;
	zoomobj['x'] = (l - zoomobj['srcinfo']['left']) / 5;
	zoomobj['y'] = (t - zoomobj['srcinfo']['top']) / 5;
	zoomobj['w'] = (w - zoomobj['srcobj'].width) / 5;
	zoomobj['h'] = (h - zoomobj['srcobj'].height) / 5;
	$('zoomimglayer').style.filter = '';
	$('zoomimglayer').innerHTML = '';
	setTimeout('zoomST(1)', 5);
}

function zoomST(c) {
	if($('zoomimglayer').style.display == '') {
		$('zoomimglayer').style.left = (parseInt($('zoomimglayer').style.left) + zoomobj['x']) + 'px';
		$('zoomimglayer').style.top = (parseInt($('zoomimglayer').style.top) + zoomobj['y']) + 'px';
		$('zoomimglayer').style.width = (parseInt($('zoomimglayer').style.width) + zoomobj['w']) + 'px';
		$('zoomimglayer').style.height = (parseInt($('zoomimglayer').style.height) + zoomobj['h']) + 'px';
		var opacity = c * 20;
		$('zoomimglayer').style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=' + opacity + ',finishOpacity=100,style=0)';
		$('zoomimglayer').style.opacity = opacity / 100;
		c++;
		if(c <= 5) {
			setTimeout('zoomST(' + c + ')', 5);
		} else {
			zoomadjust = 1;
			$('zoomimglayer').style.filter = '';
			$('zoomimglayer_bg').style.display = '';
			$('zoomimglayer').innerHTML = '<table cellspacing="0" cellpadding="2"><tr><td style="text-align: right"><span class="left">鼠标滚轮缩放图片</span> <a href="' + zoomobj['zimg'] + '" target="_blank"><img src="images/common/newwindow.gif" border="0" style="vertical-align: middle" title="在新窗口打开" /></a> <a href="###" onclick="zoomimgadjust(event, 1)"><img src="images/common/resize.gif" border="0" style="vertical-align: middle" title="实际大小" /></a> <a href="###" onclick="zoomclose()"><img style="vertical-align: middle" src="images/common/close.gif" title="关闭" /></a>&nbsp;</td></tr><tr><td align="center" id="zoomimgbox"><img id="zoomimg" style="cursor: move; margin: 5px;" src="' + zoomobj['zimg'] + '" width="' + $('zoomimglayer').style.width + '" height="' + $('zoomimglayer').style.height + '"></td></tr></table>';
			$('zoomimglayer').style.overflow = 'visible';
			$('zoomimglayer').style.width = $('zoomimglayer').style.height = 'auto';
			if(is_ie){
				$('zoomimglayer').onmousewheel = zoomimgadjust;
			} else {
				$('zoomimglayer').addEventListener("DOMMouseScroll", zoomimgadjust, false);
			}
			$('zoomimgbox').onmousedown = function(event) {try{zoomdrag(event, 1);}catch(e){}};
			$('zoomimgbox').onmousemove = function(event) {try{zoomdrag(event, 2);}catch(e){}};
			$('zoomimgbox').onmouseup = function(event) {try{zoomdrag(event, 3);}catch(e){}};
		}
	}
}

function zoomimgadjust(e, a) {
	if(!a) {
		if(!e) e = window.event;
		if(e.altKey || e.shiftKey || e.ctrlKey) return;
		var l = parseInt($('zoomimglayer').style.left);
		var t = parseInt($('zoomimglayer').style.top);
		if(e.wheelDelta <= 0 || e.detail > 0) {
			if($('zoomimg').width <= 200 || $('zoomimg').height <= 200) {
				doane(e);return;
			}
			$('zoomimg').width -= zoomobj['zimginfo'][0] / 10;
			$('zoomimg').height -= zoomobj['zimginfo'][1] / 10;
			l += zoomobj['zimginfo'][0] / 20;
			t += zoomobj['zimginfo'][1] / 20;
		} else {
			if($('zoomimg').width >= zoomobj['zimginfo'][0]) {
				zoomimgadjust(e, 1);return;
			}
			$('zoomimg').width += zoomobj['zimginfo'][0] / 10;
			$('zoomimg').height += zoomobj['zimginfo'][1] / 10;
			l -= zoomobj['zimginfo'][0] / 20;
			t -= zoomobj['zimginfo'][1] / 20;
		}
	} else {
		var clientHeight = document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight;
		var scrollTop = document.body.scrollTop ? document.body.scrollTop : document.documentElement.scrollTop;
		$('zoomimg').width = zoomobj['zimginfo'][0];$('zoomimg').height = zoomobj['zimginfo'][1];
		var l = (document.body.clientWidth - $('zoomimg').clientWidth) / 2;l = l > 0 ? l : 0;
		var t = (clientHeight - $('zoomimg').clientHeight) / 2 + scrollTop;t = t > 0 ? t : 0;
	}
	$('zoomimglayer').style.left = l + 'px';
	$('zoomimglayer').style.top = t + 'px';
	$('zoomimglayer_bg').style.height = t + $('zoomimglayer').clientHeight > $('zoomimglayer_bg').clientHeight ? (t + $('zoomimglayer').clientHeight) + 'px' : $('zoomimglayer_bg').style.height;
	doane(e);
}
function zoomclose() {
	$('zoomimglayer').innerHTML = '';
	$('zoomimglayer').style.display = 'none';
	$('zoomimglayer_bg').style.display = 'none';
}

/*************************************************AJAX加载交易记录列表****************************************************/

function ajaxgettradelog(goodsid, pagesize, pageindex)
{
    $('tradelog_html').innerHTML = '加载数据中...';
    page_currentpage = pageindex;
    _sendRequest('tools/ajax.ashx?t=getgoodstradelog&goodsid=' + goodsid + '&pagesize=' + pagesize + '&pageindex=' + pageindex + '&orderby=lastupdate&ascdesc=1', function(d){
		try{
		eval('tradelog_callback(' + d + ')');}catch(e){};
	});
}

function tradelog_callback(data) {
   
    var tradelog_html = '';
    tradelog_html += '<table cellspacing="0" summary="买家购买记录">';
	tradelog_html += '		<tbody>';
	tradelog_html += '			<tr>';
	tradelog_html += '				<th width="35%">买家</th>';
	tradelog_html += '				<th width="15%">出价</th>';
	tradelog_html += '				<th width="10%">购买数量</th>';
	tradelog_html += '				<th width="30%">时间</th>';
	tradelog_html += '				<th width="10%">状态</th>';
	tradelog_html += '			</tr>';
	tradelog_html += '		</tbody>';
	tradelog_html += '		<tbody >';
		
	for(var i in data) {
		tradelog_html += '<tr class="list" onmouseover="this.className=\'liston\'" onmouseout="this.className=\'list\'">';
		tradelog_html += '<td style="text-align:left; padding-left:10px;">';
		tradelog_html += '<a target="_blank" href="userrate.aspx?uid=' + data[i].buyerid + '">'+ data[i].buyer +'</a>';
		for (j = 0; j< parseInt(data[i].buyercredit,0); j++) {
		   tradelog_html += '<img alt="0" src="'+ path +'/images/1.gif"/>'; //<span rank:params="type=b&uid=1418fcc13383f28e49634d03bfb89380&override=1" class="rank:token"><a href="#" target="_blank" title="11－40个买家信用积分，请点击查看详情" class="tb-rank buyer-rank-2"/></span>
		}
		tradelog_html += '</td>';
		tradelog_html += '<td>' + data[i].price + '</td>';
		tradelog_html += '<td>' + data[i].number + '</td>';
		tradelog_html += '<td>' + data[i].lastupdate + '</td>';
		if(data[i].status == 7) { 
		    tradelog_html += '<td>成交<img width="22" height="18" alt="成交" src="'+ path +'/images/okhank.gif"/></td>';
		}
		else {
		    tradelog_html += '<td>未知</td>';
		}
		tradelog_html += '</tr>';
	}
	//alert(tradelog_html);
	
	tradelog_html += '   </tbody>';
	tradelog_html += '	</table>';
    $('tradelog_html').innerHTML = tradelog_html;
	ajaxpagination("ajaxgettradelog", page_recordcount, page_pagesize, page_currentpage, "listpage");
}


/*************************************************发表留言****************************************************/		
		
function quickpost(event, theform, goodsid, isendpage) {
	if(postSubmited == false && (event.ctrlKey && event.keyCode == 13) || (event.altKey && event.keyCode == 83)) {
		if (!$("postsubmit").disabled) {
		    if(validateform(theform, false,false)){
			    theform.submit();
			    $("postsubmit").disabled = true
			}
		}
		else {
			alert('正在提交, 请稍候...');
		}
	}
}

function validateform(theform, previewpost, switcheditormode) {
    
	var message = theform.message.value;

	if (message == "") {
		alert("请完成内容栏。");
		$("postsubmit").disabled = false;
		return false;
	} 
	
	if(!disablepostctrl && ((postminchars != 0 && mb_strlen(message) < postminchars) || (postmaxchars != 0 && mb_strlen(message) > postmaxchars))) {
		alert("您的帖子长度不符合要求。\n\n当前长度: " + mb_strlen(message) + " 字节\n系统限制: 1 到 200 字节");
		return false;
	}

	if (!switcheditormode && !previewpost) {
		$("postsubmit").disabled = true;
	}

	theform.message.value = message;
	return true;
}

/*************************************************AJAX加载留言列表****************************************************/	

function ajaxgetleaveword(goodsid, pagesize, pageindex)
{
    $('leavewordlist').innerHTML = '加载数据中...';
    leaveword_page_currentpage = pageindex;
    _sendRequest('tools/ajax.ashx?t=getgoodsleaveword&goodsid=' + goodsid + '&pagesize=' + pagesize + '&pageindex=' + pageindex, function(d){
		try{
		eval('leaveword_callback(' + d + ')');}catch(e){};
	});
}

function ajaxgetleavewordbyid(leavewordid)
{
    _sendRequest('tools/ajax.ashx?t=getgoodsleavewordbyid&leavewordid=' + leavewordid, function(d){
		try{
		eval('leavewordmessage_callback(' + d + ')');}catch(e){};
	});
}

function leavewordmessage_callback(data){
    if(data[0].id > 0){
        $("message").value = data[0].message.replace(/<br \/>/g, "\r\n");
        $("leavewordid").value = data[0].id;
        $("postleaveword").value = "edit";
    }
    else {
        alert('当前留言不存在或已被删除!');
    }
}


function leaveword_callback(data) {
    var leaveword_html = '';
  	leaveword_html += '<dl>';
		
	for(var i in data) {
	    if(data[i].isbuyer) {
		    leaveword_html += '<dt>';
        }
        else { 
            leaveword_html += '<dd>[卖家]&nbsp;';
        }   
        
        if(data[i].uid>0) {
            leaveword_html += '<a href="userinfo.aspx?userid=' + data[i].uid + '">' + data[i].username + '</a> : ';
        }
        else {
            leaveword_html += data[i].username + ' : '; 
        }
        
        leaveword_html += '<BR />' + data[i].message + '<span>&nbsp;<BR />';
        //当为管理组身份或卖家或留言发布人时
		if(useradminid == 1 || isseller || (data[i].uid>0 && data[i].uid == userid)) {
		    leaveword_html += '<a href="#" onclick="javascript:if(confirm(\'确认要删除吗?\')){window.location.href= \'showgoods.aspx?goodsid=' + page_goodsid + '&deleteleaveword=1&leavewordid=' + data[i].id + '\';}">[删除]</a> &nbsp;';
		    if(data[i].uid>0 && data[i].uid == userid) {
		        leaveword_html += '<a href="javascript:;" onclick="ajaxgetleavewordbyid(' + data[i].id + ');">[编辑]</a>&nbsp;';
		    }
		}
		
		leaveword_html += '&nbsp;日期:' + data[i].postdatetime + '</span>'; 
		
		if(data[i].isbuyer) {
		    leaveword_html += '</dt>'
        }
        else { 
            leaveword_html += '</dd>'
        }      
	}
	//alert(leaveword_html);

	leaveword_html += '</dl>';
    $('leavewordlist').innerHTML = leaveword_html;
	ajaxpagination('ajaxgetleaveword', leaveword_page_recordcount, leaveword_page_pagesize, leaveword_page_currentpage, "leaveword_listpage");
}

/*************************************************用户信息****************************************************/	

//加载当前用户的评价数据
function loadratedata(startpos,endpos, div_list, span_goodrate)
{
    var goodrate = 0; //好评数
    var sixmonthweekcount = 0;//最近六个月评价数
    var sixmonthagocount = 0;//6个月前评价数
    var ratecount = 0; //评价总数
   
    for(i = startpos; i<endpos ;i++) {		
        sixmonthweekcount += usercredit_data[i].sixmonth;
        sixmonthagocount += usercredit_data[i].sixmonthago;
    
        if(usercredit_data[i].ratetype == 1) {
		    goodrate = usercredit_data[i].sixmonth +usercredit_data[i].sixmonthago;
	    }
    }				
    ratecount = sixmonthweekcount + sixmonthagocount;
    
    if(ratecount > 0) { 
        $(span_goodrate).innerHTML = parseFloat((goodrate / ratecount) * 100).toFixed(2) + '%';
    }
    else {
        $(span_goodrate).innerHTML = '0.00%';
    }
}					

   
function gettradecredit(goodsratenum, isseller, span_credit, uid) {
    var raterank = '';
    for(var i in creditrulesjsondata){
        if(creditrulesjsondata[i].lowerlimit <= goodsratenum && creditrulesjsondata[i].upperlimit > goodsratenum) {
            if(isseller) {
                raterank = creditrulesjsondata[i].sellericon;
            }
            else {
                raterank = creditrulesjsondata[i].buyericon;
            }
            break;
        }
    }
    
    if(raterank == '') {
        $(span_credit).innerHTML = '<a href="eccredit.aspx?uid=' + uid + '" target="_blank">' + goodsratenum + '</a>';
    }
    else {
        $(span_credit).innerHTML = goodsratenum + '</a> <a href="eccredit.aspx?uid=' + uid + '" target="_blank"><img alt="0" src="templates/' + templatepath + '/images/' + raterank + '" /></a>';
    }
}

/*************************************************获取当前时间到截止时间的剩余时间****************************************************/	

function getTime(expiration, obj_id) {

    now = new Date();
    expiration_date = new Date(getDateFromFormat(expiration,'yyyy/MM/dd/ hh:mm:ss'));
    //expiration_date = new Date(expiration);
    days = (expiration_date - now) / 1000 / 60 / 60 / 24;
    daysRound = Math.floor(days);
    hours = (expiration_date - now) / 1000 / 60 / 60 - (24 * daysRound);
    hoursRound = Math.floor(hours);
    minutes = (expiration_date - now) / 1000 /60 - (24 * 60 * daysRound) - (60 * hoursRound);
    minutesRound = Math.floor(minutes);
    seconds = (expiration_date - now) / 1000 - (24 * 60 * 60 * daysRound) - (60 * 60 * hoursRound) - (60 * minutesRound);
    secondsRound = Math.round(seconds);

    var remain = '';
    if(daysRound > 0) {
        remain = daysRound  + '天';
    }
    
    if(hoursRound > 0) {
        remain += hoursRound + '小时';
    }
    
    if(remain =='' || remain.indexOf('天') < 0) {
        remain += minutesRound + '分' + secondsRound + '秒';
    }
    
    $(obj_id).innerHTML =  remain;
    newtime = window.setTimeout("getTime('" + expiration + "','" + obj_id + "');", 5000);
}

function getDateFromFormat(dateString,formatString){
   var regDate = /\d+/g;
   var regFormat = /[YyMmdHhSs]+/g;
   var dateMatches = dateString.match(regDate);
   var formatmatches = formatString.match(regFormat);
   var date = new Date();
    for(var i=0;i<dateMatches.length;i++){
        switch(formatmatches[i].substring(0,1)){
            case 'Y':
            case 'y':
                 date.setFullYear(parseInt(dateMatches[i]));break;
            case 'M':
                 date.setMonth(parseInt(dateMatches[i])-1);break;
            case 'd':
                 date.setDate(parseInt(dateMatches[i]));break;
            case 'H':
            case 'h':
                 date.setHours(parseInt(dateMatches[i]));break;
            case 'm':
                 date.setMinutes(parseInt(dateMatches[i]));break;
            case 's':
                 date.setSeconds(parseInt(dateMatches[i]));break;
         }
     }
    return date;
}

