function display(id) {
    jQuery("#auditreason_" + id).show();
}

var newCheckAll = function (formId,checked) {
    jQuery("#" + formId + " :checkbox").attr("checked", checked);
}

var optionCheckedTopics = function (option, reason) {
    var tableid = $('tablelist').value;
    var tidlist = "";
    jQuery(":checkbox[name='audittopicid']:checked").each(function (i) {
        tidlist = tidlist + this.value + ','
    });
    tidlist = tidlist.slice(0, tidlist.length - 1);
    if (tidlist == "") {
        alert("请至少选择一条主题!");
    } else {
        _sendRequest('tools/ajax.ashx?t=' + option + 'topic&tid=' + tidlist + "&reason=" + reason + "&tableid=" + tableid, optionCheckedTopics_callback, false);
    }
}

function optionCheckedTopics_callback(doc) {
    if (doc.length == 0) {
        alert("操作失败，请联系管理员");
    } else {
        var count = jQuery("#audittopiccount").html();
        jQuery("#audittopiccount").html(count - jQuery(":checkbox[name='audittopicid']:checked").length);
        _sendRequest(location + '&inajax=1&about=topic', innertopic_callback, false);
    }
}

function innertopic_callback(doc) {
    var str = doc.slice(doc.indexOf('<table id="table_topic"'), -1);//可用JQUERY替代；
    str = str.slice(0, str.indexOf('</table>'));
    if (str == "") {
        jQuery("#audittopic_form").hide();
        jQuery("#forum_topic").after("<div class='hintinfo'>审核完成，没有需要审核的主题!</div>");
        jQuery("#tablelist").show();
    } else {
        $('table_topic').innerHTML = str;
    }
}

var optionCheckedPosts = function (option, reason) {
    var pidlist = "";
    var tableid = $('tablelist').value;
    jQuery(":checkbox[name='auditpostid']:checked").each(function (i) {
        pidlist = pidlist + this.value + ','
    });
    pidlist = pidlist.slice(0, pidlist.length - 1);
    if (pidlist == "") {
        alert("请至少选择一条帖子!");
    } else {
        switch (option) {
            case "pass":
                _sendRequest('tools/ajax.ashx?t=passpost&pid=' + pidlist + "&reason=" + reason + "&tableid=" + tableid, optionCheckedPosts_callback, false);
                break;
            case "delete":
                _sendRequest('tools/ajax.ashx?t=deletepost&pid=' + pidlist + "&reason=" + reason + "&tableid=" + tableid, optionCheckedPosts_callback, false);
                break;
            case "ignore":
                _sendRequest('tools/ajax.ashx?t=ignorepost&pid=' + pidlist + "&reason=" + reason + "&tableid=" + tableid, optionCheckedPosts_callback, false);
                break;
        }
    }
}

function optionCheckedPosts_callback(doc) {
    if (doc.length == 0) {
        alert("操作失败，请联系管理员");
    } else {
        var count = jQuery("#auditpostcount").html();
        jQuery("#auditpostcount").html(count - jQuery(":checkbox[name='auditpostid']:checked").length);
        if (jQuery("#postcount") != null) {
            jQuery("#tablelist").show();
        }
        _sendRequest(location + '&inajax=1&about=post', innerposts_callback, false);
    }
}

function innerposts_callback(doc) {
    var str = doc.slice(doc.indexOf('<table id="table_post"'), -1);
    str = str.slice(0, str.indexOf('</table>'));
    if (str == "") {
        jQuery("#auditpost_form").hide();
        jQuery("#form_post").after("<div class='hintinfo'>审核完成，没有需要审核的主题!</div>");
    } else {
        $('table_post').innerHTML = str;
    }
}

function chageclass(about, id) {
    switch (about) {
        case "topicover":
            jQuery("#tbody_" + id).addClass("onmouse");
            jQuery("#cite_" + id).show();
            break;
        case "topicout":
            if (jQuery("#pm_" + id).val() != "")
                return;
            jQuery("#tbody_" + id).removeClass("onmouse");
            jQuery("#cite_" + id).hide();
            break;
        case "postover":
            jQuery("#postbody_" + id).addClass("onmouse");
            jQuery("#postcite_" + id).show();
            break;
        case "postout":
            if (jQuery("#pm_" + id).val() != "")
                return;
            jQuery("#postbody_" + id).removeClass("onmouse");
            jQuery("#postcite_" + id).hide();
            break;
    }
}

function _auditpost(option, id, reason) {
    var tableid = $('tablelist') == null ? 1 : $('tablelist').value;
    switch (option) {
        case "passpost":
        case "ignorepost":
        case "deletepost":
            jQuery("#postcite_" + id.split('|')[0]).hide();
            _sendRequest('tools/ajax.ashx?t=' + option + '&pid=' + id + "&reason=" + reason + "&tableid=" + tableid, auditpost_callback, false);
            break;
        case "passtopic":
        case "ignoretopic":
        case "deletetopic":
            jQuery("#cite_" + id).hide();
            _sendRequest('tools/ajax.ashx?t=' + option + '&tid=' + id + "&reason=" + reason + "&tableid=" + tableid, audittopic_callback, false);
            break;
            break;
        case "deletepostsbyuidanddays":
            _sendRequest('tools/ajax.ashx?t=deletepostsbyuidanddays&uid=' + id + "&tableid=" + tableid, deletepostsbyuidanddays_callback, false);
            break;
    }
}

function deletepostsbyuidanddays_callback(doc) {
    var err = eval(doc);
    if (err[0] == null || err[0] == undefined || err == "") {
        alert("操作失败，请联系管理员");
    } else {
        removeeffect("deletepostbyuidanddays", err[0].uid);
    }
    window.location.reload();
}
function auditpost_callback(doc) { 
    var err = eval(doc);
    if (err[0] == null && err[0] == undefined) {
        alert("操作失败，请联系管理员");
    } else {
    if (err[0].message)
        alert(err[0].message);
    else
        removeeffect("post", err[0].pid);
    }
}

function audittopic_callback(doc) {
    var err = eval(doc);
    if (err[0] == null && err[0] == undefined) {
        alert("操作失败，请联系管理员");
    } else {
    if (err[0].message)
        alert(err[0].message);
    else
        removeeffect("topic", err[0].tid);
    }
}

function removeeffect(about, id) {
    switch (about) {
        case "topic":
            jQuery("#tbody_" + id).hide("slow", function () {
                jQuery("#table_topic > tbody").remove("#tbody_" + id);
                var count = jQuery("#audittopiccount").html();
                jQuery("#audittopiccount").html(--count);
                if (jQuery("#table_topic > tbody").length == 0) {
                    jQuery("#audittopic_form").hide();
                    jQuery("#forum_topic").after("<div class='hintinfo'>审核完成，没有需要审核的主题!</div>");
                    jQuery("#tablelist").show();
                }
            });
            if ($('count').value > 16 && $('pagecount').value > $('pageid').value)
                _sendRequest(location + '&inajax=1&about=topic', getlasttopic_callback, false);
            break;
        case "post":
            jQuery("#postbody_" + id).hide("slow", function () {
                jQuery("#table_post > tbody").remove("#postbody_" + id);
                var count = jQuery("#auditpostcount").html();
                jQuery("#auditpostcount").html(--count);
                if (jQuery("#table_post > tbody").length == 0) {
                    jQuery("#auditpost_form").hide();
                    jQuery("#form_post").after("<div class='hintinfo'>审核完成，没有需要审核的帖子!</div>");
                    jQuery("#tablelist").show();
                }
             });
            if ($('count').value > 16 && $('pagecount').value > $('pageid').value)
                _sendRequest(location + '&inajax=1&about=post', getlastpost_callback, false);
            break;
        case "deletepostbyuidanddays":
            var parm = location.href.substring(location.href.indexOf("operation=") + 10, location.href.indexOf("&"));
            if (parm == 'audittopic') {
                _sendRequest(location + '&inajax=1&about=topic', deletetopic_callback, false);
            } else if (parm == 'auditpost') {
                _sendRequest(location + '&inajax=1&about=post', deletepost_callback, false);
            } 
            break;
    }
//    if (about == 'topic') {
//        jQuery("#tbody_" + id).hide("slow", function () { jQuery("#table_topic").remove(jQuery("#tbody_" + id)); });
//        if ($('count').value > 16 && $('pagecount').value > $('pageid').value)
//        _sendRequest(location + '&inajax=1&about=topic', getlasttopic_callback, false);
//    }else if (about == 'post') {
//        jQuery("#postbody_" + id).hide("slow", function () { jQuery("#table_post").remove(jQuery("#postbody_" + id)); });
//        if ($('count').value > 16 && $('pagecount').value > $('pageid').value)
//        _sendRequest(location + '&inajax=1&about=post', getlastpost_callback, false);
//    }
}

function deletepost_callback(doc) {
    var str = doc.slice(doc.indexOf('<table id="table_post"'), -1);
    str = str.slice(0, str.indexOf('</table>'));
    if (str == "") {
        jQuery("#auditpost_form").hide();
        jQuery("#form_post").after("<div class='hintinfo'>没有需要审核的回复!</div>");
        location.reload();
    }else {
        $('temp').innerHTML = str;
        $('table_post').appendChild($('temp').childNodes[0]);
        location.reload();
    }
    
}

function deletetopic_callback(doc) {
    var str = doc.slice(doc.indexOf('<table id="table_topic"'), -1);
    str = str.slice(0, str.indexOf('</table>'));
    if (str == "") {
        jQuery("#audittopic_form").hide();
        jQuery("#forum_topic").after("<div class='hintinfo'>审核完成，没有需要审核的主题!</div>");
        location.reload();
    } else {
        $('temp').innerHTML = str;
        $('table_topic').appendChild($('temp').childNodes[0]);
        location.reload();
    }
}

function getlastpost_callback(doc){
    var str = doc.slice(doc.indexOf('<table id="table_post"'), -1);
    str = str.slice(0, str.indexOf('</table>'));
    if (str == "") {
        jQuery("#auditpost_form").hide();
        jQuery("#form_post").after("<div class='hintinfo'>审核完成，没有需要审核的回复!</div>");
    } else {
        var last = str.lastIndexOf('<tbody');
        $('temp').innerHTML = str.slice(last, -1);
        $('table_post').appendChild($('temp').childNodes[0]);
    }
}

function getlasttopic_callback(doc) {
    var str = doc.slice(doc.indexOf('<table id="table_topic"'), -1);
    str = str.slice(0, str.indexOf('</table>'));
    if (str == "") {
        jQuery("#audittopic_form").hide();
        jQuery("#forum_topic").after("<div class='hintinfo'>审核完成，没有需要审核的主题!</div>");
    } else {
        var last = str.lastIndexOf('<tbody');
        $('temp').innerHTML = str.slice(last, -1);
        $('table_topic').appendChild($('temp').childNodes[0]);
    }
}

function getpostinfo(tid)
{
	if($('msgtbody_'+tid).style.display=='none')
	{
	_sendRequest('tools/ajax.ashx?t=getpostinfo', getpostinfo_callback, true, 'tid='+ tid);
	}
	else
	{
	$('msgtbody_'+tid).style.display='none';
	}
}

function getpostinfo_callback(doc)
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
	    {

        $('msgtbody_'+getSingleNodeValue(doc, 'tid')).style.display='';
        $('msg_'+getSingleNodeValue(doc, 'tid')).innerHTML=getSingleNodeValue(doc,'message');
		return;
			}
	
	}
	
	
function audittopic(tidlist,mod,formid,pm){
	var str='modcp.aspx?operation=audittopic&topicidlist='+tidlist +'&mod_'+tidlist+'='+mod+'&pm_'+tidlist+'='+pm;
	postoperation(str,formid)
}

function auditpost(pid, formid, tid) {
    var tableid = $('tablelist').value;
	var mod = getradiovalue('mod_' + pid);
	var pm = $('pm_' + pid).value;
 	var str = ['modcp.aspx?operation=auditpost&tidlist=',tid,'&tableid=',tableid,'&pidlist=',pid,'&mod_',pid,'=',mod,'&pm_',pid,'=',pm].join('');
	postoperation(str,formid)
}


function postoperation(actionstr,formid){
    $(formid).action=actionstr;
	$(formid).submit();
}

function getradiovalue(RadioName){
    var obj;   
    obj=document.getElementsByName(RadioName);
    if(obj!=null){
        var i;
        for(i=0;i<obj.length;i++){
            if(obj[i].checked){
                return obj[i].value;           
            }
        }
    }
    return null;
}
function preg_replace(search, replace, str, regswitch) {
    var regswitch = !regswitch ? 'ig' : regswitch;
    var len = search.length;
    for (var i = 0; i < len; i++) {
        re = new RegExp(search[i], regswitch);
        str = str.replace(re, typeof replace == 'string' ? replace : (replace[i] ? replace[i] : replace[0]));
    }
    return str;
}