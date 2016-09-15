function sendRequest(action, isendpage, imagedir, redirect, editorkey, hidevalue) {
    var oForm = $(editorkey+"form");
    /* if (!isendpage || !isendpage == true){
        oForm.submit();
        return;
    } */
    //var redirect = (event.ctrlKey && event.altKey && event.keyCode == 13);
    var sBody = getRequestBody(oForm);
    var oXmlHttp = createXMLHttp();
    oXmlHttp.open("post", (action && action != '') ? action : oForm.action, true);
    oXmlHttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    oXmlHttp.onreadystatechange = function () {
        if (oXmlHttp.readyState == 4) {
            if (oXmlHttp.status == 200) {
                if (hidevalue > 0) {
                    window.location.reload();
                }
                var xml;
                if (oXmlHttp.responseXML == null && oXmlHttp.responseText != null) {
                    var parser = new DOMParser();
                    xml = parser.parseFromString(oXmlHttp.responseText, "text/xml");
                }
                else {
                    xml = oXmlHttp.responseXML;
                }
                saveResult(xml, imagedir, redirect);
                oForm.replysubmit.disabled = false;
                if ($("reloadvcade")) {
                    $("reloadvcade").click();
                }
                //bind current post;
            } else {
                alert("An error occurred: " + oXmlHttp.statusText);
            }
        }
    };
    oXmlHttp.send(sBody);
    //$(editorkey+'message').innerHTML='';
}
function getRequestBody(oForm) {
    var aParams = new Array();
    for (var i=0 ; i < oForm.elements.length; i++) {
        if (oForm.elements[i].type == "checkbox" && oForm.elements[i].checked == false)
            continue;
        var sParam = encodeURIComponent(oForm.elements[i].name);
        sParam += "=";
        sParam += encodeURIComponent(oForm.elements[i].value);
        aParams.push(sParam);
    }
    return aParams.join("&");
}

function ajaxctlent(event, objfrm, topicid, isendpage, imagedir,editorkey,hidevalue) {
    if(postSubmited == false && (event.ctrlKey && event.keyCode == 13) || (event.altKey && event.keyCode == 83)) {
        var redirect = (event.ctrlKey && event.altKey && event.keyCode == 13);
        ajaxreply(objfrm, topicid, isendpage, imagedir, redirect,editorkey,hidevalue);
    }
}

function ajaxreply(objfrm, topicid, isendpage, imagedir, redirect, editorkey, hidevalue) {
        if (!$(editorkey+"submit").disabled){
            if (fastvalidate(objfrm, editorkey, false, false)) {
                sendRequest('tools/ajax.ashx?topicid=' + topicid + '&postid=' + $(editorkey + 'form').postid.value + '&postreplynotice=' + $(editorkey + 'form').postreplynotice.checked + '&t=quickreply', isendpage, imagedir, redirect, editorkey, hidevalue);
                window.setTimeout(
                    function () {
                        if ($(editorkey + "submit") != null) {
                            if ($(editorkey + "submit").disabled) {
                                var message = $(editorkey + "message").innerHTML;
                                $(editorkey + "message").innerHTML = "提交时间似乎比平时要长，请耐心等待...";
                                if ($(editorkey + "submit") != null) {
                                    window.setTimeout(
                                    function () {
                                        $(editorkey + "message").innerHTML = $(editorkey + "submit").disabled ? message : "";
                                    }, 1000);
                                }
                            }
                        }
                       // hideWindow('reply');
                    }, 1000);
            }
        }else{
        alert('正在提交, 请稍候...');
        }
}

function getStars(n, t, path) {
    var s = '';
    for (var i = 3; i > 0; i--) {
        var level = parseInt(n / Math.pow(t, i-1));
        n = n % Math.pow(t, i-1);
        for (var j = 0; j < level; j++) {
            s += '<img src="' + path + '/star_level' + i + '.gif" />';
        }
    }
    return s;
}

function getInPostad(index){
    try{
        if (inpostad){
                var adstr = '';
                adstr += "<div class=\"line category\"><div style='float: left;'>[广告]&nbsp;</div><div>";
                var tempstr = inpostad[index];
                var ad = tempstr.split("\\r\\n");
                for (var i = 0; i < ad.length; i++)
                {
                    adstr += ("\r\n" + ad[i]);
                }
                adstr += "\r\n</div></div>";
                return adstr;
            }
        }catch(e){}
    return "";
}
String.prototype.trim = function(){return this.replace(/(^[\s\t\xa0\u3000]+)|([\u3000\xa0\s\t]+$)/g, "")};
function saveResult(doc, imagedir, redirect) {
    var err = doc.getElementsByTagName('error');
    if (err[0] != null && err[0] != undefined) {
        if (err[0].childNodes.length > 1) {
            alert(err[0].childNodes[1].nodeValue);
        } else {
            alert(err[0].firstChild.nodeValue);
        }
        return;
    }


    var invisible = getSingleNodeValue(doc, 'invisible');
    if (invisible == 1) {
        alert("回复成功，请等待审核");
        return;
    }

    var ismoder = getSingleNodeValue(doc, 'ismoder');
    var adindex = getSingleNodeValue(doc, 'adindex');
    var status = getSingleNodeValue(doc, 'status');
    var stars = getSingleNodeValue(doc, 'stars');
    var id = getSingleNodeValue(doc, 'id');
    var fid = getSingleNodeValue(doc, 'fid');

    var ip = getSingleNodeValue(doc, 'ip');
    var lastedit = getSingleNodeValue(doc, 'lastedit');
    var layer = getSingleNodeValue(doc, 'layer');
    var message = escape(getSingleNodeValue(doc, 'message'));
    var parentid = getSingleNodeValue(doc, 'parentid');
    var pid = getSingleNodeValue(doc, 'pid');
    var postdatetime = getSingleNodeValue(doc, 'postdatetime');
    var poster = getSingleNodeValue(doc, 'poster');
    var posterid = getSingleNodeValue(doc, 'posterid');
    var smileyoff = getSingleNodeValue(doc, 'smileyoff');
    var topicid = getSingleNodeValue(doc, 'topicid');
    var title = getSingleNodeValue(doc, 'title');
    var usesig = getSingleNodeValue(doc, 'usesig');
    var uid = getSingleNodeValue(doc, 'uid');
    var accessmasks = getSingleNodeValue(doc, 'accessmasks');
    var adminid = getSingleNodeValue(doc, 'adminid');
    //var avatar = getSingleNodeValue(doc, 'avatar');
    var avatarheight = getSingleNodeValue(doc, 'avatarheight');
    var avatarshowid = getSingleNodeValue(doc, 'avatarshowid');
    var avatarwidth = getSingleNodeValue(doc, 'avatarwidth');
    var credits = getSingleNodeValue(doc, 'credits');
    var digestposts = getSingleNodeValue(doc, 'digestposts');
    var email = getSingleNodeValue(doc, 'email');
    var score1 = getSingleNodeValue(doc, 'score1');
    var score2 = getSingleNodeValue(doc, 'score2');
    var score3 = getSingleNodeValue(doc, 'score3');
    var score4 = getSingleNodeValue(doc, 'score4');
    var score5 = getSingleNodeValue(doc, 'score5');
    var score6 = getSingleNodeValue(doc, 'score6');
    var score7 = getSingleNodeValue(doc, 'score7');
    var score8 = getSingleNodeValue(doc, 'score8');
    var scoreunit1 = getSingleNodeValue(doc, 'scoreunit1');
    var scoreunit2 = getSingleNodeValue(doc, 'scoreunit2');
    var scoreunit3 = getSingleNodeValue(doc, 'scoreunit3');
    var scoreunit4 = getSingleNodeValue(doc, 'scoreunit4');
    var scoreunit5 = getSingleNodeValue(doc, 'scoreunit5');
    var scoreunit6 = getSingleNodeValue(doc, 'scoreunit6');
    var scoreunit7 = getSingleNodeValue(doc, 'scoreunit7');
    var scoreunit8 = getSingleNodeValue(doc, 'scoreunit8');
    var extcredits1 = getSingleNodeValue(doc, 'extcredits1');
    var extcredits2 = getSingleNodeValue(doc, 'extcredits2');
    var extcredits3 = getSingleNodeValue(doc, 'extcredits3');
    var extcredits4 = getSingleNodeValue(doc, 'extcredits4');
    var extcredits5 = getSingleNodeValue(doc, 'extcredits5');
    var extcredits6 = getSingleNodeValue(doc, 'extcredits6');
    var extcredits7 = getSingleNodeValue(doc, 'extcredits7');
    var extcredits8 = getSingleNodeValue(doc, 'extcredits8');
    var extgroupids = getSingleNodeValue(doc, 'extgroupids');
    var gender = getSingleNodeValue(doc, 'gender');
    var bday = getSingleNodeValue(doc, 'bday');
    var icq = getSingleNodeValue(doc, 'icq');
    var joindate = getSingleNodeValue(doc, 'joindate');
    var lastactivity = getSingleNodeValue(doc, 'lastactivity');
    var medals = getSingleNodeValue(doc, 'medals');
    var nickname = getSingleNodeValue(doc, 'nickname');
    var oltime = getSingleNodeValue(doc, 'oltime');
    var onlinestate = getSingleNodeValue(doc, 'onlinestate');
    var showemail = getSingleNodeValue(doc, 'showemail');
    var signature = getSingleNodeValue(doc, 'signature');
    var sigstatus = getSingleNodeValue(doc, 'sigstatus');
    var skype = getSingleNodeValue(doc, 'skype');
    var website = getSingleNodeValue(doc, 'website');
    var yahoo = getSingleNodeValue(doc, 'yahoo');
    var qq = getSingleNodeValue(doc, 'qq');
    var msn = getSingleNodeValue(doc, 'msn');
    var posts = getSingleNodeValue(doc, 'posts');
    var footerad = getSingleNodeValue(doc, 'ad_thread1');
    var topad = getSingleNodeValue(doc, 'ad_thread2');
    var rightad = getSingleNodeValue(doc, 'ad_thread3');

    var theLocation = getSingleNodeValue(doc, 'location');

    var showavatars = getSingleNodeValue(doc, 'showavatars');
    var userstatusby = getSingleNodeValue(doc, 'userstatusby');
    var starthreshold = getSingleNodeValue(doc, 'starthreshold');
    var forumtitle = getSingleNodeValue(doc, 'forumtitle');
    var showsignatures = getSingleNodeValue(doc, 'showsignatures');
    var maxsigrows = getSingleNodeValue(doc, 'maxsigrows');
    var medals = getSingleNodeValue(doc, 'medals');
    var debateopinion = getSingleNodeValue(doc, 'debateopinion');
    var onlyauthor = getSingleNodeValue(doc, 'onlyauthor');
    var olimg = getSingleNodeValue(doc, 'olimg');
    var container = $("postsContainer");
    var postleftshow = getSingleNodeValue(doc, 'postleftshow').split(',');
    var userfaceshow = getSingleNodeValue(doc, 'userfaceshow').split(',');
    var lastvisit = getSingleNodeValue(doc, 'lastvisit');
    var divDetailnav = document.createElement("DIV");

    container.appendChild(divDetailnav);

    divDetailnav.className = 'viewthread';

    var html = '';
    html += '		<table cellpadding="0" cellspacing="0" border="0" id="' + pid + '" name="' + pid + '">';
    html += '			<tbody>';
    html += '			<tr>';
    html += '			<td rowspan="3" class="postauthor" id="' + pid + '">';

    html += '		<div class="popupmenu_popup userinfopanel" id="' + posterid + '' + id + '" style="display:none; clip: rect(auto auto auto auto); position absolute;" initialized ctrlkey="userinfo2">';
    html += '		  <div class="popavatar">';
    html += '			<div id="' + posterid + '' + id + '_ma"></div>';
    html += '				<ul class="profile_side">';
    html += '					<li class="post_pm"><a href="usercppostpm.aspx?msgtoid=' + posterid + '" target="_blank">发送短消息</a></li>';
    html += '			    </ul>';
    html += '			</div>';
    html += '			<div class="popuserinfo">';
    html += '			    <dl class="cl">';
    if (in_array("uid", userfaceshow)) {
        html += '<dt>UID</dt><dd>' + posterid + '</dd>';
    }

    if (in_array("bday", userfaceshow)) {
        html += '<dt>生日</dt><dd>' + bday + '</dd>';
    }

    if (in_array("posts", userfaceshow)) {
        html += '<dt>帖子</dt><dd>' + posts + '</dd>';
    }
    if (in_array("digestposts", userfaceshow)) {
        html += '<dt>精华</dt><dd>' + digestposts + '</dd>';
    }
    if (in_array("credits", userfaceshow)) {
        html += '<dt>积分</dt><dd>' + credits + '</dd>';
    }
    if (score1 != "" && (in_array("extcredits1", userfaceshow))) {
        html += '<dt>' + score1 + '</dt><dd>' + extcredits1 + scoreunit1 + '</dd>';
    }

    if (score2 != "" && (in_array("extcredits2", userfaceshow))) {
        html += '<dt>' + score2 + '</dt><dd>' + extcredits2 + scoreunit2 + '</dd>';
    }
    if (score3 != "" && (in_array("extcredits3", userfaceshow))) {
        html += '<dt>' + score3 + '</dt><dd>' + extcredits3 + scoreunit3 + '</dd>';
    }
    if (score4 != "" && (in_array("extcredits4", userfaceshow))) {
        html += '<dt>' + score4 + '</dt><dd>' + extcredits4 + scoreunit4 + '</dd>';
    }

    if (score5 != "" && (in_array("extcredits5", userfaceshow))) {
        html += '<dt>' + score5 + '</dt><dd>' + extcredits5 + scoreunit5 + '</dd>';
    }

    if (score6 != "" && (in_array("extcredits6", userfaceshow))) {
        html += '<dt>' + score6 + '</dt><dd>' + extcredits6 + scoreunit6 + '</dd>';
    }

    if (score7 != "" && (in_array("extcredits7", userfaceshow))) {
        html += '<dt>' + score7 + '</dt><dd>' + extcredits7 + scoreunit7 + '</dd>';
    }
    if (score8 != "" && (in_array("extcredits8", userfaceshow))) {
        html += '<dt>' + score8 + '</dt><dd>' + extcredits8 + scoreunit8 + '</dd>';
    }
    if (in_array("gender", userfaceshow)) {
        html += '<dt>性别</dt><dd>' + displayGender(gender) + '</dd>';
    }
    if (in_array("location", userfaceshow)) {
        html += '<dt>来自</dt><dd>' + theLocation + '</dd>';
    }
    if (in_array("oltime", userfaceshow)) {
        html += '<dt>在线时间</dt><dd>' + oltime + '</dd>';
    }

    if (in_array("joindate", userfaceshow)) {
        html += '<dt>注册时间</dt><dd>' + new Date(joindate.replace(/-/ig, '/')).format("yyyy-MM-dd") + '</dd>';
    }
    if (in_array("lastvisit", userfaceshow)) {
        html += '<dt>最后登录</dt><dd>' + new Date(lastvisit.replace(/-/ig, '/')).format("yyyy-MM-dd") + '</dd>';
    }

    html += '				</dl>';
    html += '				<div class="imicons cl">';
    if (msn != '') {
        html += '					<a href="mailto:' + msn + '" target="_blank" class="msn">' + msn + '</a>';
    }
    if (skype != '') {
        html += '					<a href="skype:' + skype + '" target="_blank" class="skype">' + skype + '</a>';
    }
    if (icq != '') {
        html += '					<a href="http://wwp.icq.com/scripts/search.dll?to=' + icq + '" target="_blank" class="icq">' + icq + '</a>';
    }
    if (qq != '') {
        html += '					<a href="http://wpa.qq.com/msgrd?V=1&Uin=' + qq + '&Site=' + forumtitle + '&Menu=yes" target="_blank" class="qq">' + qq + '</a>';
    }
    if (yahoo != '') {
        html += '					<a href="http://edit.yahoo.com/config/send_webmesg?.target=' + yahoo + '&.src=pg" target="_blank" class="yahoo">' + yahoo + '</a>';
    }
    html += '				</div>';
    html += '			</div>';
    html += '		</div>';

    if (uid != "") {
        html += '					<div class="poster"><span class="onlineyes">';
        html += poster;
        html += '					</span></div>';
        html += '					<div id="' + posterid + '' + id + '_a">';
        html += '				<div class="avatar">';
        html += '	<img onerror="this.onerror=null;this.src=\'/images/common/noavatar_medium.gif\';" src="tools/avatar.aspx?uid=' + posterid + '&size=medium" onmouseover="showauthor(this,\'' + posterid + '' + id + '\')" id="memberinfo_' + id + '"/>';
        html += '				</div>';
        if (nickname != "") {
            html += '			<p><em>';
            html += nickname;
            html += '			</em></p>';
        }
        html += '				</div><p>';
        html += getStars(stars, starthreshold, imagedir);
        html += '				</p>';


        html += '				<ul class="otherinfo">';
        if (userstatusby == 1) {
            html += '				<li><label>组别</label>' + status + '</li>';
        }
        if (in_array("uid", postleftshow)) {
            html += '<li><label>UID</label>' + posterid + '</li>';
        }

        if (in_array("bday", postleftshow)) {
            html += '<li><label>生日</label>' + bday + '</li>';
        }

        if (in_array("posts", postleftshow)) {
            html += '<li><label>帖子</label>' + posts + '</li>';
        }
        if (in_array("digestposts", postleftshow)) {
            html += '<li><label>精华</label>' + digestposts + '</li>';
        }
        if (in_array("credits", postleftshow)) {
            html += '<li><label>积分</label>' + credits + '</li>';
        }
        if (score1 != "" && (in_array("extcredits1", postleftshow))) {
            html += '<li><label>' + score1 + '</label>' + extcredits1 + scoreunit1 + '</li>';
        }

        if (score2 != "" && (in_array("extcredits2", postleftshow))) {
            html += '<li><label>' + score2 + '</label>' + extcredits2 + scoreunit2 + '</li>';
        }
        if (score3 != "" && (in_array("extcredits3", postleftshow))) {
            html += '<li><label>' + score3 + '</label>' + extcredits3 + scoreunit3 + '</li>';
        }
        if (score4 != "" && (in_array("extcredits4", postleftshow))) {
            html += '<li><label>' + score4 + '</label>' + extcredits4 + scoreunit4 + '</li>';
        }

        if (score5 != "" && (in_array("extcredits5", postleftshow))) {
            html += '<li><label>' + score5 + '</label>' + extcredits5 + scoreunit5 + '</li>';
        }

        if (score6 != "" && (in_array("extcredits6", postleftshow))) {
            html += '<li><label>' + score6 + '</label>' + extcredits6 + scoreunit6 + '</li>';
        }

        if (score7 != "" && (in_array("extcredits7", postleftshow))) {
            html += '<li><label>' + score7 + '</label>' + extcredits7 + scoreunit7 + '</li>';
        }
        if (score8 != "" && (in_array("extcredits8", postleftshow))) {
            html += '<li><label>' + score8 + '</label>' + extcredits8 + scoreunit8 + '</li>';
        }
        if (in_array("gender", postleftshow)) {
            html += '<li><label>性别</label>' + displayGender(gender) + '</li>';
        }
        if (in_array("location", postleftshow)) {
            html += '<li><label>来自</label>' + theLocation + '</li>';
        }
        if (in_array("oltime", postleftshow)) {
            html += '<li><label>在线时间</label>' + oltime + '</li>';
        }

        if (in_array("joindate", postleftshow)) {
            html += '<li><label>注册时间</label>' + new Date(joindate.replace(/-/ig, '/')).format("yyyy-MM-dd") + '</li>';
        }
        if (in_array("lastvisit", postleftshow)) {
            html += '<li><label>最后登录</label>' + new Date(lastvisit.replace(/-/ig, '/')).format("yyyy-MM-dd") + '</li>';
        }

        html += '				</ul>';
    }
    else {
        html += '<cite style="padding-left: 15px;">';
        html += '<em>' + theLocation + '</em>';
        html += '</cite>';
        html += '<p><em>未注册</em></p>'
    }
    html += '                       <div class="medals">';
    html += medals;
    html += '                       </div>';
    html += '			</td>';
    html += '			<td class="postcontent">';
    html += ' <div class="pi">';
    html += ' <strong>';
    html += '	<a href="###" class="floor" title="复制帖子链接到剪贴板" onclick="setcopy(window.location.toString().replace(/#(.*?)$/ig, \'\') + \'#' + pid + '\', \'已经复制到剪贴板\')">';
    html += id + '<sup>#</sup>';
    html += ' 	</a>';
    html += ' </strong>';
    html += ' <div class="postinfo">';
    html += ' 	<div class="msgfsize y">';
    html += ' 		<label>字体大小: </label>';
    html += ' 		<small title="正常" onclick="$(\'message' + pid + '\').className=\'t_msgfont\'"><b>t</b></small>';
    html += ' 		<big title="放大" onclick="$(\'message' + pid + '\').className=\'t_bigfont\'"><b>T</b></big>';
    html += ' 	</div>';
    html += ' 	<em>' + olimg + '   ' + postdatetime + '</em>	';
    var link = '';
    if (uid != "") {
        if (onlyauthor == 1) {
            link = '<a href="showtopic.aspx?topicid=' + topicid + '&onlyauthor=1">只看楼主</a>';
        }
        else {
            link = '<a href="showtopic.aspx?topicid=' + topicid + '&onlyauthor=2&posterid=' + posterid + '">只看该用户</a>'
        }
        html += '   <em>|' + link + '</em>'
    }
    html += ' </div>';
    html += ' </div>';
    html += ' <div id="ad_thread2_' + id + '"></div>';
    html += ' <div id="ad_thread3_' + id + '"></div>';
    html += ' <div class="postmessage defaultpost">';
    html += ' 		<h2>' + title + '</h2><div id="topictag"></div>';
    html += ' 		<div id="message' + pid + '" class="t_msgfont">' + unescape(message) + '</div>';
    html += ' </div>';
    if (debateopinion == 1) {
        html += '正方';
    } else if (debateopinion == 2) {
        html += '反方';
    }
    html += '			</div>';
    html += '			</td>';
    html += '			</tr>';
    html += '			<tr>';
    html += '			<td class="plc">';
    if (usesig == 1 && signature != "" && showsignatures == 1) {
        html += '			<div class="postertext">';
        if (maxsigrows > 0) {
            var ieheight = maxsigrows * 12;
            html += '			<div class="t_signature" style="overflow: hidden; max-height: ' + maxsigrows * 1.5 + 'em;maxHeightIE:' + ieheight + 'px;">' + signature + '</div>';
        } else
            html += signature;
        html += '			</div>';
    }
    html += '			</td>';
    html += '			</tr>';
    html += '			<tr>';
    html += '			<td class="plc">';
    html += '			    <div id="ad_thread1_' + id + '"></div>';
    html += '			</td>';
    html += '			</tr>';
    html += '			<tr>';
    html += '			<td class="postauthor"></td>';
    html += '			<td class="postactions">';
    html += '			<div class="p_control">';
    html += '			<cite class="y">';
    html += '              <a href="###" onclick="window.scrollTo(0,0)">TOP</a></p>';
    html += '			</cite>';

    if (uid != "") {
        html += '	<a class="fastreply" onclick="showWindow(\'reply\', \'showtopic.aspx?poster=' + poster + '&postlayer=' + id + '&postid=' + pid + '&topicid=' + topicid + '\')" href="showtopic.aspx?poster=' + poster + '&postlayer=' + id + '&postid=' + pid + '&topicid=' + topicid + '">回复</a>';
        html += '	<a onclick="floatwin(\'open_reply\', this.href, 600, 410, \'600,0\');doane(event);" class="repquote" href="postreply.aspx?topicid=' + topicid + '&postid=' + pid + '&forumpage=1&quote=yes">引用</a>';
        html += '	<a class="editpost" href="editpost.aspx?topicid=' + topicid + '&postid=' + pid + '&forumpage=1&pageid=1">编辑</a>';
        html += '	<a href="delpost.aspx?topicid=' + topicid + '&postid=' + pid + '" onclick="return confirm(\'确定要删除吗?\');" class="delpost" title="删除我的帖子">删除</a>';

    }
    html += '			</div>';
    html += '			</td>';
    html += '			</tr>';
    html += '			</tbody>';
    html += '			<tr class="threadad">';
    html += '			    <td class="postauthor"></td>';
    html += '			    <td class="adcontent"></td>';
    html += '           </tr>';
    divDetailnav.innerHTML = html;
    window.location.href = '#' + pid;
    onloadshowCreditPrompt();

    try {
        if ($('quickpostform') != null)
            document.getElementById("quickpostform").reset();
    } catch (e) {
        alert(e.message);
    }
    if (redirect == true) {
        $("message").innerHTML = "正在跳转到主题列表...";
        window.location = "showforum.aspx?forumid=" + fid;
    }
    delete doc;
    hideWindow('reply');
    secclick = new Array();
}

function getSingleNodeValue(doc, tagname){
    try{
        var oNodes = doc.getElementsByTagName(tagname);
        if (oNodes[0] != null && oNodes[0] != undefined){
            if (oNodes[0].childNodes.length > 1) {
                return oNodes[0].childNodes[1].nodeValue;
            } else {
                return oNodes[0].firstChild.nodeValue;    		
            }
        }
    }
    catch(e){}
    return '';
}
