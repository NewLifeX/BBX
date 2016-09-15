function commentdebates(tid,messageid)
{

	
	//sendRequest_commentdebates('tools/ajax.ashx?tid=' + tid + '&t=addcommentdebates',messageid);			
	_sendRequest('tools/ajax.ashx?t=addcommentdebates', commentdebate_callback, true, 'commentdebates='+ '[area=点评内容]' + $('commentdebatesmsg').value + '[/area]&tid='+tid);
	
	}
         
function commentdebate_callback(doc)
{
	   var err = doc.getElementsByTagName('error');
		if (err[0] != null && err[0] != undefined)
		{
			if (err[0].childNodes.length > 1) {
				alert(err[0].childNodes[1].nodeValue);
			} else {
				alert(err[0].firstChild.nodeValue);    		
			}
			return;
		}
		else
		{$('firstpost').innerHTML = $('firstpost').innerHTML+ '<br /><br /><div class="msgheader">点评内容</div><div class="msgborder">'+$('commentdebatesmsg').value+'</div>';		
	    $('commentdebates_menu').style.display='none';
		$('commentdebatesmsg').value='';
	    alert('辩论点评成功');
		return;
			}
	
	}
	

function setcomment(divid)
{
	var commentid=$(divid);
	commentid.style.left =($('commentlink').offsetLeft)+ 'px';
	commentid.style.top = ($('commentlink').offsetTop)+ 'px';
	commentid.style.display = '';
	commentid.style.position='absolute';

	//var commentid=$(divid);
	//commentid.style.left =($('commentlink').offsetLeft)+ 'px';
	//commentid.style.top = (($('topicforumbox').offsetTop)+$('commentlink').offsetTop+50)+ 'px';
	//commentid.style.display = '';
	//commentid.style.position='absolute';
}
var currentpid=0;

function digg(pid,tid,type)
{
	//sendRequest_countenance('tools/ajax.ashx?pid=' + pid + '&t=countenancedebates&tid='+tid,pid);
	cureentpid=pid;

	_sendRequest('tools/ajax.ashx?t=diggdebates', diggdebate_callback, true, 'pid=' + pid + '&tid='+tid +'&type='+type);
		
	
//	switch (type){
//		   case 1:
//		  $('positivediggs').innerHTML=parseInt($('positivediggs').innerHTML)+1; 
//		  break;
//		   case 2:
//		  $('negativediggs').innerHTML=parseInt($('negativediggs').innerHTML)+1;
//		  break;
//		   default: 
//	       alert("error");
//		   break;
//                 }
	
	
}

function diggdebate_callback(doc) {
	
	var err = doc.getElementsByTagName('error');
		if (err[0] != null && err[0] != undefined)
		{
			if (err[0].childNodes.length > 1) {
				alert(err[0].childNodes[1].nodeValue);
			} else {
				alert(err[0].firstChild.nodeValue);    		
			}
			return ;
		}
		else {
			$('diggs'+cureentpid).innerHTML = "支持 " + (parseInt($('diggs'+cureentpid).innerHTML.replace("支持 ","")) + 1);
			$('diggs' + cureentpid).onclick = function() { alert("投过票了"); }
			return ;
		}
}

var currentform;

function textareachange(thisform)
{
currentform=thisform;	
	
	}

function debatequickreply(event)
{
	
	if(currentform!='')
	{
    	if((event.ctrlKey && event.keyCode == 13) || (event.altKey && event.keyCode == 83))
	    {
		 $(currentform).submit();
		
		}
	}
}

function showDebatReplyBox(tid, pid, opinion, parseurloff, smileyoff, bbcodeoff, processtime, olid, antispampostmessage) {

    if (antispampostmessage == null || antispampostmessage == "") {
        antispampostmessage = 'message';
    }
	var html = '<div id="reply_box" style="margin-top: 10px;" class="debatemessage">';
	html += '	<form method="post" name="postform'+pid+'" id="postform'+pid+'" action="postreply.aspx?topicid=' + tid + '"	enctype="multipart/form-data" onsubmit="return fastvalidate(this);" >';
	html += '	<table>';
	html += '		<tr><td>我要提出';
	if (opinion == 1)
	{
		html += '正方意见';
	}
	else if (opinion == 2)
	{
		html += '反方意见';
	}
	html += '	</td></tr>';
	html += '		<tr>';
	html += '			<td>';
	html += '				<input type="hidden" id="title" name="title"  tabindex="1" value=""/><input type="hidden" id="postid" name="postid" value=' + pid + ' />';
	html += '				<input name="debateopinion" type="hidden" value="' + opinion + '" />';
	html += '				<input type="hidden" name="parseurloff" value="' + parseurloff + '" />';
	html += '				<input type="hidden" name="smileyoff" value="' + smileyoff + '" />';
	html += '				<input type="hidden" name="bbcodeoff" value="' + bbcodeoff + '" />';
	html += '				<input type="hidden" name="usesig" value="0" />';
	html += '';
	html += '			</td>';
	html += '		</tr>';
	html += '		<tr>';
	html += '			<td>';
	html += '		<textarea name="' + antispampostmessage + '" cols="40" rows="7" class="txtarea autosave"  tabindex="2" onkeydown="debatequickreply(event, this.form);"  onfocus='; 
    html += '      "textareachange(this.form.id);">[area=反对对方' + $('poster' + pid).innerHTML + '的意见]' + $('hiddendpid'+pid).value + '[/area]</textarea>';
	html += '			</td>';
	html += '		</tr>';
	html += '		<tr>';
	html += '			<td>';
	if($('debate_vcode')!=undefined)
	{
	    html += '<div id="js_vcode' + pid + '" name="js_vcode" style="position: relative;"></div>';
	
		}
	html += '				<input  type="submit" name="replysubmit" value="我要发表" tabindex="3"/>';
	html += ' <input type="button" onclick="closeDebatReplyBox('+pid+');" value="放弃" /> ';
	html += '			</td>';
	html += '		</tr>';
	html += '	</table>';
	html += '	</form>'
	html += '</div>';

	if (typeof vcodeimgid != 'undefined')
	    vcodeimgid ++;
	
	$('reply_box_owner_' + pid).innerHTML = html;
	if($('debate_vcode')!=undefined) {
	    $('js_vcode' + pid).innerHTML = '<input name="vcodetext" tabindex="1" size="20" onkeyup="changevcode(this.form, this.value);" class="txt" style="width:90px;" id="vcodetext' + vcodeimgid + '"  onblur="if(!seccodefocus) {display(this.id + \'_menu\')};"  onclick="opensecwin(' + vcodeimgid + ',1)"   value="验证码" autocomplete="off"/>' +
	                                       '<div class="seccodecontent"  style="display:none;cursor: pointer;width: 124px; height: 44px;top:256px;z-index:10009;padding:0;" id="vcodetext' + vcodeimgid + '_menu" onmouseout="seccodefocus = 0" onmouseover="seccodefocus = 1"><img src="' + forumpath + 'tools/VerifyImagePage.aspx?time=' + processtime + '" class="cursor" id="vcodeimg' + vcodeimgid + '" onclick="this.src=\'' + forumpath + 'tools/VerifyImagePage.aspx?id=' + olid + '&time=\' + Math.random();"/></div>';
	}
}

function closeDebatReplyBox(pid)
{
		$('reply_box_owner_' + pid).innerHTML = "";
		$('reply_btn_' + pid).style.display = "";
}

var postSubmited = false;


function ctlent(event, theform) {
	if(postSubmited == false && (event.ctrlKey && event.keyCode == 13) || (event.altKey && event.keyCode == 83) && $('postsubmit')) {
	    try {
		    if(in_array($('postsubmit').name, ['topicsubmit', 'replysubmit', 'editsubmit', 'pmsubmit']) && !fastvalidate(theform)) {
			    doane(event);
			    return;
		    }
		    postSubmited = true;
		    $('postsubmit').disabled = true;
		    theform.submit();
		}
		catch(e)
		{
		    return;
		}
	}
}

function getParms(str, queryname) {
    var qKeys = {};
    var re = /[?&]([^=]+)(?:=([^&]*))?/g;
    var matchInfo;
    while(matchInfo = re.exec(str)){
	    qKeys[matchInfo[1]] = matchInfo[2];
    }
    return typeof(qKeys[queryname])=='undefined'?'':qKeys[queryname];
}

function debaterefresh() {
    window.setTimeout(function () {
        if (jQuery("#positivebutton_menu>.popupmenu_option").text() == "投票成功" || jQuery("#negabutton_menu>.popupmenu_option").text() == "投票成功")
            window.location.href = window.location.href;
    }, 2000);
}

function showdebatepage(dataSource, parseurloff, smileyoff, bbcodeoff,isenddebate,type,userid,tid)
{
	showloading();
	_sendRequest(dataSource, function (responseText){
		var varlist;
		var opinion = getParms(dataSource, 'opinion');
		
		try
		{
			varlist = eval("(" + responseText + ")");
		}
		catch (e)
		{
			varlist = new Array();
		}
		
		var html = '';
		
		var postlist = varlist.postlist;
		var debateexpand = varlist.debateexpand;
		var pagenumbers = varlist.pagenumbers;
		
		//var bordercolor = debateexpand.Positivebordercolor;
		//var color = debateexpand.Positivecolor;
		var pagenumbers_top = 'positive_pagenumbers_top';
		var pagenumbers_buttom = 'positive_pagenumbers_buttom';
		var page_owner = 'positivepage_owner';
		var htmlshow=isenddebate.toLowerCase( )=="false";
		if (opinion == 2)
		{
			//bordercolor = debateexpand.Negativebordercolor;
			//color = debateexpand.Negativecolor;
			pagenumbers_top = 'negative_pagenumbers_top';
			page_owner = 'negativepage_owner';
			pagenumbers_buttom = 'negative_pagenumbers_buttom';
		}
        
		for (var i = 0; i < postlist.length; i++)
		{
			html += '<div class="square" >'
			html += '	<table cellspacing="0" cellpadding="0">';
			html += '		<tbody>';
			html += '			<tr>';
			html += '				<td class="supportbox">';
			html += '					<p style="background: rgb(255, 255, 255) none repeat scroll 0% 0%; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;">';
			html += '						支持度';
			html += '						<span class="talknum" id="diggs' + postlist[i].Pid + '">' + postlist[i].Diggs + '</span>';
			if(htmlshow && postlist[i].Posterid!=userid)
			{
			if(!postlist[i].Digged)
			{
			html += '						<span class="cliktalk" id="cliktalk' + postlist[i].Pid + '"><a href="javascript:void(0);" onclick="digg(' + postlist[i].Pid + ',' + debateexpand.Tid + ','+type+')">支持</a></span>';
			}
			}
			html += '					</p>';
			html += '				</td>';
			html += '				<td class="comment">';
			html += '					<h3><span>时间:' + ((postlist[i].Postdatetime).toString()).replace(":00","") + '</span>发表者:<a id="poster' + postlist[i].Pid + '" href="userinfo.aspx?userid='+ postlist[i].Posterid + '">' + postlist[i].Poster + '</a>';
			if (ismoder || (postlist[i].Posterid != -1 && postlist[i].Posterid == userid))
			{
				html += '&nbsp;';
				html += '<a href="editpost.aspx?topicid=' + tid + '&postid=' + postlist[i].Pid + '&debate=1">编辑</a> | ';
				html += '<a href="delpost.aspx?topicid=' + tid + '&postid=' + postlist[i].Pid + '&opinion='+type+'" onclick="return confirm(\'确定要删除吗?\');">删除</a>';
			}
			html += '</h3>';
			html += '					<div class="debatemessage" id="message' + postlist[i].Pid + '">' + postlist[i].Message + '</div>';
			if(htmlshow  && postlist[i].Posterid!=userid)
			{
				html +=' <input name="hiddendpid'+postlist[i].Pid+'" type="hidden" id="hiddendpid'+postlist[i].Pid+'" value="'+postlist[i].Ubbmessage+'" \/>';
				html += '					<p class="othertalk"><a id="reply_btn_' + postlist[i].Pid + '" href="###" onclick="showDebatReplyBox(' + debateexpand.Tid + ', ' + postlist[i].Pid + ', 2, ' + parseurloff + ', ' + smileyoff + ', '+ bbcodeoff + ');this.style.display=\'\';">我不同意</a>';
				html += '                   <div id="reply_box_owner_' + postlist[i].Pid + '"></div></p>';
			}
			html += '				</td>';
			html += '			</tr>';
			html += '		</tbody>';
			html += '	</table>';
			html += '</div>';
		}
		$('positivediggs').innerHTML = debateexpand.Positivediggs;
		$('negativediggs').innerHTML = debateexpand.Negativediggs;
		var positivepercent = 0.0;
		if (debateexpand.Negativediggs + debateexpand.Positivediggs == 0)
		{
			positivepercent = 50.0;
		}
		else
		{
			positivepercent = debateexpand.Positivediggs / (debateexpand.Negativediggs + debateexpand.Positivediggs) * 100;
		}
		$('positivepercent').style.width = positivepercent + '%';
		$(pagenumbers_top).innerHTML = pagenumbers;
		$(pagenumbers_buttom).innerHTML = pagenumbers;
		$(page_owner).innerHTML = html;
		showloading('none');
	}, false);
}
