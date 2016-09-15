function displayGender(gender) {
    var gendername = '女';
    gender = parseInt(gender);
    switch (gender) {
        case 0:
            gendername = '保密';
            break;
        case 1:
            gendername = '男';
            break;
        case 2:
            gendername = '女';
            break;
    }
    return gendername;
}
function replyToFloor(floor, poster, postid) {
    if ($('quickposttitle')) {
        $('quickpostform').postid.value = postid;
        $('quickposttitle').value = '回复 ' + floor + '# ' + poster + ' 的帖子';
        $('validatemessage').innerHTML = $('quickposttitle').value;
        $('validatemessage').className = '';
        $('quickpostmessage').focus();
    }
}

function nospace(username) {
    alert('抱歉, 用户 ' + username + ' 尚未开通个人空间');
}

function fastdalert(s,editorkey) {
    $(editorkey+'validatemessage').className = s ? 'onerror' : '';
    $(editorkey+'validatemessage').innerHTML = s;
}


function fastvalidate(theform, editorkey, previewpost, switcheditormode) {
    fastdalert("", editorkey);
    var originalmessage = $(editorkey + "message").value;
    var message = !theform.parseurloff.checked ? parseurl(originalmessage) : originalmessage;

    if (message == "") {
        fastdalert("请完成标题或内容栏。", editorkey);
        $(editorkey + "message").focus();
//        try {
//            theform.message.focus();
//        } catch (e) {
//            $(editorkey+"message").focus();
//        }
        try { $(editorkey+"submit").disabled = false; } catch (e) { }
        return false;
    } else if ($(editorkey+"title").value.length > 60) {
        fastdalert("您的标题超过 60 个字符的限制。",editorkey);
        $(editorkey+"title").focus();
        try { $(editorkey+"submit").disabled = false; } catch (e) { }
        return false;
    }

    if ($('debateopinion') && $('debateopinion').value == 0) {
        fastdalert('请选择您在辩论中的观点',editorkey);
        return false;
    }

    if (!disablepostctrl && ((postminchars != 0 && mb_strlen(message) < postminchars) || (postmaxchars != 0 && mb_strlen(message) > postmaxchars))) {
        fastdalert("您的帖子长度不符合要求。\n\n当前长度: " + mb_strlen(message) + " 字节\n系统限制: " + postminchars + " 到 " + postmaxchars + " 字节",editorkey);
        return false;
    }

    if (!switcheditormode && !previewpost) {
        try { $(editorkey+"submit").disabled = true; } catch (e) { }
    }
    $(editorkey + "message").value = message;
//    try {
//        theform.message.value = message;
//    } catch (e) {
//        $(editorkey+"message").value = message;
//    }
    return true;
}

function ShowStars(n, t) {
    var s = '';
    for (var i = 3; i > 0; i--) {
        level = parseInt(n / Math.pow(t, i - 1));
        n = n % Math.pow(t, i - 1);
        for (var j = 0; j < level; j++) {
            s += '<img src="' + imagedir + '/star_level' + i + '.gif" />';
        }
    }
    document.write(s);
}

function copycode(obj) {
    //if(is_ie && obj.style.display != 'none') {
    //	var rng = document.body.createTextRange();
    //rng.moveToElementText(obj);
    //	rng.scrollIntoView();
    //	rng.select();
    //	rng.execCommand("Copy");
    //	rng.collapse(false);
    //}
    setcopy(is_ie ? obj.innerText.replace(/\r\n\r\n/g, '\r\n') : obj.textContent, '复制完成');

}

function signature(obj) {
    if (obj.style.maxHeightIE != '') {
        var height = (obj.scrollHeight > parseInt(obj.style.maxHeightIE)) ? obj.style.maxHeightIE : obj.scrollHeight;
        if (obj.innerHTML.indexOf('<IMG ') == -1) {
            obj.style.maxHeightIE = '';
        }
        return height;
    }
}

function imgzoom(o) {
    if (event.ctrlKey) {
        var zoom = parseInt(o.style.zoom, 10) || 100;
        zoom -= event.wheelDelta / 12;
        if (zoom > 0) {
            o.style.zoom = zoom + '%';
        }
        return false;
    } else {
        return true;
    }
}

function printinpostad(index) {
    try {
        if (inpostad) {
            document.write("<div class=\"line category\"><div style='float: left;'>[广告]&nbsp;</div><div style='text-align:left;'>");
            var tempstr = inpostad[index];
            var ad = tempstr.split("\\r\\n");
            for (var i = 0; i < ad.length; i++) {
                document.writeln(ad[i]);
            }
            document.write("</div>");
            document.write("</div>");
        }
    } catch (e) {
    }
}

function showrate(pid, aspxrewrite, ratetimes, tid, templatepath, showtype) {
    //	var rr = $("rate_" + pid + "_real");
    //	var rf = $("rate_" + pid + "_fake");

    //	if (rr.style.display == "none") {
    //		rr.style.display = "";
    //		rf.style.display = "none";
    //	} else {
    //		rr.style.display = "none";
    //		rf.style.display = "";
    //	}
    var ratediv = $("rate_" + pid);
    var ratetablediv = $("ratetable_" + pid);
    if (ratediv.innerHTML == "") {
        ratediv.innerHTML = "请稍侯..."
        var action = "tools/ajax.ashx?t=ratelist";

        var oXmlHttp = createXMLHttp();
        oXmlHttp.open("post", action, true);
        oXmlHttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        oXmlHttp.onreadystatechange = function () {
            if (oXmlHttp.readyState == 4) {
                if (oXmlHttp.status == 200) {
                    var xml;
                    if (oXmlHttp.responseXML == null && oXmlHttp.responseText != null) {
                        var parser=new DOMParser();
                        xml = parser.parseFromString(oXmlHttp.responseText, "text/xml");
                    }
                    else {
                        xml = oXmlHttp.responseXML;
                    }


                    outputRatelog(xml, pid, aspxrewrite, templatepath, showtype);
                    //bind current post;
                } else {
                    alert("An error occurred: " + oXmlHttp.statusText);
                }
            }
        };
        oXmlHttp.send("pid=" + pid + "&ratetimes=" + ratetimes + "&tid=" + tid);
    }
}

function outputRatelog(doc, pid, aspxrewrite, templatepath, showtype) {
    var err = doc.getElementsByTagName('error');

    var ratediv = $("rate_" + pid);
    var ratetablediv = $("ratetable_" + pid);
    if (err[0] != null && err[0] != undefined) {
        if (err[0].childNodes.length > 1) {
            alert(err[0].childNodes[1].nodeValue);
        } else {
            //alert(err[0].firstChild.nodeValue);
            ratediv.innerHTML = "该帖评分记录已被删除";
            ratediv.style.marginLeft = "0"
        }
        return;
    }
    //    var ratediv = $("rate_" + pid);

    var html = "";
    var tablehtml = "";
    var ratelist = doc.getElementsByTagName('ratelog');
    var previoursuid = "";
    if (showtype == 0) {
        for (var i = 0; i < ratelist.length; i++) {
            var uid = getSpecificNodeValue(doc, "uid", i);
            var nextuid = getSpecificNodeValue(doc, "uid", i + 1);
            var username = getSpecificNodeValue(doc, "username", i);
            var extcreditsname = getSpecificNodeValue(doc, "extcreditsname", i);
            var extcreditsunit = getSpecificNodeValue(doc, "extcreditsunit", i);
            var postdatetime = getSpecificNodeValue(doc, "postdatetime", i);
            var score = getSpecificNodeValue(doc, "score", i);
            var reason = getSpecificNodeValue(doc, "reason", i);
            var avatarurl = aspxrewrite == 1 ? "userinfo-" + uid + ".aspx" : "userinfo.aspx?userid=" + uid;
            var r = r ? r : Math.random();
            
            if (previoursuid != uid) {
                html += "<li><div style='position: absolute; z-index: 999; clip: rect(auto, auto, auto, auto); left: 421px; top: 5343px; display: none;' ";
                html += "class='attach_popup' id='" + r + "_rate_" + uid + "_menu'>";
                html += "<p class='cornerlayger'>" + reason;
            }
            html += "<em>&nbsp;&nbsp;" + extcreditsname + " " + score + " " + extcreditsunit + "</em>";
            if (nextuid != uid) {
                html += "</p><p class='minicorner'/></div>";
                html += "<p class='rateavatar' onmouseover='showMenu({\"ctrlid\":this.id, \"pos\":\"12\"})' id='" + r + "_rate_" + uid + "'>";
                html += "<a target='_blank' href='" + avatarurl + "'><img width='48' height='48' onerror='this.onerror=null;this.src=\"" + forumpath + "images/common/noavatar_small.gif\"' ";
                html += "src='tools/avatar.aspx?uid=" + uid + "&size=small'/></a></p>";
                html += "<p><a target='_blank' href='" + avatarurl + "'>" + username + "</a></p></li>";
            }
            if (nextuid != uid)
                r = Math.random();
            previoursuid = uid;
        }
    }
    else if (showtype == 1) {
        tablehtml += "<table cellspacing=\"0\" cellpadding=\"0\" class=\"list\">";
        tablehtml += "<thead><tr><td class=\"user\" width=\"80\" style=\"padding-left:4px\">用户名</td><td width=\"120\">时间</td><td width=\"240\">积分</td><td>理由</td></tr></thead>";
        for (var i = 0; i < ratelist.length; i++) {
            var uid = getSpecificNodeValue(doc, "uid", i);
            var nextuid = getSpecificNodeValue(doc, "uid", i + 1);
            var username = getSpecificNodeValue(doc, "username", i);
            var extcreditsname = getSpecificNodeValue(doc, "extcreditsname", i);
            var extcreditsunit = getSpecificNodeValue(doc, "extcreditsunit", i);
            var postdatetime = getSpecificNodeValue(doc, "postdatetime", i);
            var score = getSpecificNodeValue(doc, "score", i);
            var reason = getSpecificNodeValue(doc, "reason", i);
            var avatarurl = aspxrewrite == 1 ? "userinfo-" + uid + ".aspx" : "userinfo.aspx?userid=" + uid;
            if (previoursuid != uid) {
                tablehtml += "<tbody><tr>";
                tablehtml += "<td class=\"user\"  width=\"80\" style=\"padding-left:4px\"><a target='_blank' href=\"" + avatarurl + "\" class='xg2'>" + username + "</a></td>";
                tablehtml += "<td width=\"120\">" + postdatetime + "</td>";
                tablehtml += "<td width=\"240\">";
            }
            if (score.substr(0, 1) == '+')
                tablehtml += "<span class=\"xi1\">" + extcreditsname + score + extcreditsunit + "</span>&nbsp;&nbsp;";
            if (score.substr(0, 1) == '-')
                tablehtml += "<span class=\"xi2\">" + extcreditsname + score + extcreditsunit + "</span>&nbsp;&nbsp;";
            if (nextuid != uid) {
                tablehtml += "</td><td>" + reason + "</td>";
                tablehtml += "</tr></tbody>";
            }
            previoursuid = uid;
        }
        tablehtml += "</table>";
    }
    else {
        var avatarcount = ratelist.length < 7 ? ratelist.length : 7;
        for (var i = 0; i < avatarcount; i++) {
            var uid = getSpecificNodeValue(doc, "uid", i);
            var nextuid = getSpecificNodeValue(doc, "uid", i + 1);
            var username = getSpecificNodeValue(doc, "username", i);
            var extcreditsname = getSpecificNodeValue(doc, "extcreditsname", i);
            var extcreditsunit = getSpecificNodeValue(doc, "extcreditsunit", i);
            var postdatetime = getSpecificNodeValue(doc, "postdatetime", i);
            var score = getSpecificNodeValue(doc, "score", i);
            var reason = getSpecificNodeValue(doc, "reason", i);
            var avatarurl = aspxrewrite == 1 ? "userinfo-" + uid + ".aspx" : "userinfo.aspx?userid=" + uid;
            var r = r ? r : Math.random();
            if (previoursuid != uid) {
                html += "<li><div style='position: absolute; z-index: 999; clip: rect(auto, auto, auto, auto); left: 421px; top: 5343px; display: none;' ";
                html += "class='attach_popup' id='" + r + "_rate_" + uid + "_menu'>";
                html += "<p class='cornerlayger'>" + reason;
            }
            html += "<em>&nbsp;&nbsp;" + extcreditsname + " " + score + " " + extcreditsunit + "</em>";
            if (nextuid != uid) {
                html += "</p><p class='minicorner'/></div>";
                html += "<p class='rateavatar' onmouseover='showMenu(this.id,false,2)' id='" + r + "_rate_" + uid + "'>";
                html += "<a target='_blank' href='" + avatarurl + "'><img width='48' height='48' onerror='this.onerror=null;this.src=\"" + forumpath + "images/common/noavatar_small.gif\"' ";
                html += "src='tools/avatar.aspx?uid=" + uid + "&size=small'/></a></p>";
                html += "<p><a target='_blank' href='" + avatarurl + "'>" + username + "</a></p></li>";
            }
            else if(avatarcount < ratelist.length){
                avatarcount++;
            }
            if (nextuid != uid)
                r = Math.random();
            previoursuid = uid;
        }
        previoursuid = "";
        if (avatarcount < ratelist.length) {
            tablehtml += "<table cellspacing=\"0\" cellpadding=\"0\" class=\"list\">";
            tablehtml += "<thead><tr><td class=\"user\" width=\"80\" style=\"padding-left:4px\">用户名</td><td width=\"120\">时间</td><td width=\"240\">积分</td><td>理由</td></tr></thead>";
        }
        for (var i = avatarcount; i < ratelist.length; i++) {
            var uid = getSpecificNodeValue(doc, "uid", i);
            var nextuid = getSpecificNodeValue(doc, "uid", i + 1);
            var username = getSpecificNodeValue(doc, "username", i);
            var extcreditsname = getSpecificNodeValue(doc, "extcreditsname", i);
            var extcreditsunit = getSpecificNodeValue(doc, "extcreditsunit", i);
            var postdatetime = getSpecificNodeValue(doc, "postdatetime", i);
            var score = getSpecificNodeValue(doc, "score", i);
            var reason = getSpecificNodeValue(doc, "reason", i);
            var avatarurl = aspxrewrite == 1 ? "userinfo-" + uid + ".aspx" : "userinfo.aspx?userid=" + uid;
            if (previoursuid != uid) {
                tablehtml += "<tbody><tr>";
                tablehtml += "<td class=\"user\" width=\"80\" style=\"padding-left:4px\"><a href=\"" + avatarurl + "\">" + username + "</a></td>";
                tablehtml += "<td width=\"120\">" + postdatetime + "</td>";
                tablehtml += "<td width=\"240\">";
            }
            tablehtml += extcreditsname + "<span class=\"lightlink\">" + score + "</span>" + extcreditsunit + "&nbsp;&nbsp;";
            if (nextuid != uid) {
                tablehtml += "</td><td>" + reason + "</td>";
                tablehtml += "</tr></tbody>";
            }
            previoursuid = uid;
        }
        tablehtml += "</table>";
    }
    if (ratelist.length == 0) {
        html = "";
        tablehtml = "";
    }
    
    ratediv.innerHTML = html;
    ratetablediv.innerHTML = tablehtml;


    if (ratediv != null && ratediv.innerHTML == "")
        removeElement(ratediv);
    if (ratetablediv != null && ratetablediv.innerHTML == "")
        removeElement(ratetablediv);
}

function removeElement(_element) {
    var parentElement = _element.parentNode;
    if (parentElement) {
        parentElement.removeChild(_element);
    }
}

var msgwidth = 0;
function thumbImg(obj, method) {
    if (!obj) {
        return;
    }
    obj.onload = null;
    file = obj.src;
    zw = obj.offsetWidth;
    zh = obj.offsetHeight;
    if (!zw) {
        if (!obj.id) {
            obj.id = 'img_' + Math.random();
        }
        setTimeout("thumbImg($('" + obj.id + "'), " + method + ")", 100);
        return;
    }
    zr = zw / zh;
    method = !method ? 0 : 1;
    if (method) {
        fixw = obj.getAttribute('_width');
        fixh = obj.getAttribute('_height');
        if (zw > fixw) {
            zw = fixw;
            zh = zw / zr;
        }
        if (zh > fixh) {
            zh = fixh;
            zw = zh * zr;
        }
    } else {
        fixw = 600; //!contentwidth || contentwidth < 100 ? 600 : contentwidth;
        if (zw > fixw) {
            zw = fixw;
            zh = zw / zr;
            obj.style.cursor = 'pointer';
            if (!obj.onclick) {
                obj.onclick = function() {
                    zoom(obj, obj.src);
                }
            }
        }
    }
    obj.width = zw;
    obj.height = zh;
}


function attachimg(obj, action) {
    if (action == 'load') {
        if (is_ie && is_ie < 7) {
            var objinfo = fetchOffset(obj);
            msgwidth = document.body.clientWidth - objinfo['left'] - 81;
        } else {
            if (!msgwidth) {
                var re = /postcontent|msgborder/i;
                var testobj = obj;
                while ((testobj = testobj.parentNode) != null) {
                    var matches = re.exec(testobj.className);
                    if (matches != null) {
                        msgwidth = 900;
                        break;
                    }
                };
                if (msgwidth < 1) {
                    msgwidth = window.screen.width;
                }
            }
        };
        if (obj.width > msgwidth) {
            obj.resized = true;
            obj.width = msgwidth;
            obj.style.cursor = 'pointer';
        } else if (obj.width < 600) {
            var newsrc = obj.getAttribute('newsrc');
            if (newsrc == null || newsrc == '')
                obj.onclick = null;
        }
    } else if (action == 'mouseover') {
        if (obj.resized)
            obj.style.cursor = 'pointer';
    }
}

function attachimginfo(obj, infoobj, show, event) {
    if (!$(infoobj))
        return;
    objinfo = fetchOffset(obj);
    if (show) {
        $(infoobj).style.left = objinfo['left'] + 'px';
        $(infoobj).style.top = obj.offsetHeight < 40 ? (objinfo['top'] + obj.offsetHeight) + 'px' : objinfo['top'] + 'px';
        $(infoobj).style.display = '';
    } else {
        if (is_ie) {
            $(infoobj).style.display = 'none';
            return;
        } else {
            var mousex = document.body.scrollLeft + event.clientX;
            var mousey = document.documentElement.scrollTop + event.clientY;
            if (mousex < objinfo['left'] || mousex > objinfo['left'] + objinfo['width'] || mousey < objinfo['top'] || mousey > objinfo['top'] + objinfo['height']) {
                $(infoobj).style.display = 'none';
            }
        }
    }
}

var zoomobj = Array(); var zoomadjust; var zoomstatus = 1;
function zoom(obj, zimg) {
    if (!zoomstatus) {
        window.open(zimg, '', '');
        return;
    }
    if (!zimg) {
        zimg = obj.src;
    }
    if (!$('zoomimglayer_bg')) {
        div = document.createElement('div'); div.id = 'zoomimglayer_bg';
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
        div = document.createElement('div'); div.id = 'zoomimglayer';
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
    if (op == 1) {
        zoomclick = 1;
        zoomdragstart = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
        zoomdragstart[2] = parseInt($('zoomimglayer').style.left);
        zoomdragstart[3] = parseInt($('zoomimglayer').style.top);
        doane(e);
    } else if (op == 2 && zoomdragstart[0]) {
        zoomclick = 0;
        var zoomdragnow = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
        $('zoomimglayer').style.left = (zoomdragstart[2] + zoomdragnow[0] - zoomdragstart[0]) + 'px';
        $('zoomimglayer').style.top = (zoomdragstart[3] + zoomdragnow[1] - zoomdragstart[1]) + 'px';
        doane(e);
    } else if (op == 3) {
        if (zoomclick) zoomclose();
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
    if (h > clientHeight) {
        h = clientHeight;
        w = h * r;
    }
    var l = (document.body.clientWidth - w) / 2;
    var t = h < clientHeight ? (clientHeight - h) / 2 : 0;
    t += +scrollTop;
    zoomobj['x'] = (l - zoomobj['srcinfo']['left']) / 5;
    zoomobj['y'] = (t - zoomobj['srcinfo']['top']) / 5;
    zoomobj['w'] = (w - zoomobj['srcobj'].width) / 5;
    zoomobj['h'] = (h - zoomobj['srcobj'].height) / 5;
    $('zoomimglayer').style.filter = '';
    $('zoomimglayer').innerHTML = '';
    setTimeout('zoomST(1)', 5);
}

function zoomST(c) {
    if ($('zoomimglayer').style.display == '') {
        $('zoomimglayer').style.left = (parseInt($('zoomimglayer').style.left) + zoomobj['x']) + 'px';
        $('zoomimglayer').style.top = (parseInt($('zoomimglayer').style.top) + zoomobj['y']) + 'px';
        $('zoomimglayer').style.width = (parseInt($('zoomimglayer').style.width) + zoomobj['w']) + 'px';
        $('zoomimglayer').style.height = (parseInt($('zoomimglayer').style.height) + zoomobj['h']) + 'px';
        var opacity = c * 20;
        $('zoomimglayer').style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=' + opacity + ',finishOpacity=100,style=0)';
        $('zoomimglayer').style.opacity = opacity / 100;
        c++;
        if (c <= 5) {
            setTimeout('zoomST(' + c + ')', 5);
        } else {
            zoomadjust = 1;
            $('zoomimglayer').style.filter = '';
            $('zoomimglayer_bg').style.display = '';
            $('zoomimglayer').innerHTML = '<table cellspacing="0" cellpadding="2"><tr><td style="text-align: right"><span class="left">鼠标滚轮缩放图片</span> <a href="' + zoomobj['zimg'] + '" target="_blank"><img src="images/common/newwindow.gif" border="0" style="vertical-align: middle" title="在新窗口打开" /></a> <a href="###" onclick="zoomimgadjust(event, 1)"><img src="images/common/resize.gif" border="0" style="vertical-align: middle" title="实际大小" /></a> <a href="###" onclick="zoomclose()"><img style="vertical-align: middle" src="images/common/close.gif" title="关闭" /></a>&nbsp;</td></tr><tr><td align="center" id="zoomimgbox"><img id="zoomimg" style="cursor: move; margin: 5px;" src="' + zoomobj['zimg'] + '" width="' + $('zoomimglayer').style.width + '" height="' + $('zoomimglayer').style.height + '"></td></tr></table>';
            $('zoomimglayer').style.overflow = 'visible';
            $('zoomimglayer').style.width = $('zoomimglayer').style.height = 'auto';
            if (is_ie) {
                $('zoomimglayer').onmousewheel = zoomimgadjust;
            } else {
                $('zoomimglayer').addEventListener("DOMMouseScroll", zoomimgadjust, false);
            }
            $('zoomimgbox').onmousedown = function(event) { try { zoomdrag(event, 1); } catch (e) { } };
            $('zoomimgbox').onmousemove = function(event) { try { zoomdrag(event, 2); } catch (e) { } };
            $('zoomimgbox').onmouseup = function(event) { try { zoomdrag(event, 3); } catch (e) { } };
        }
    }
}

function zoomimgadjust(e, a) {
    if (!a) {
        if (!e) e = window.event;
        if (e.altKey || e.shiftKey || e.ctrlKey) return;
        var l = parseInt($('zoomimglayer').style.left);
        var t = parseInt($('zoomimglayer').style.top);
        if (e.wheelDelta <= 0 || e.detail > 0) {
            if ($('zoomimg').width <= 200 || $('zoomimg').height <= 200) {
                doane(e); return;
            }
            $('zoomimg').width -= zoomobj['zimginfo'][0] / 10;
            $('zoomimg').height -= zoomobj['zimginfo'][1] / 10;
            l += zoomobj['zimginfo'][0] / 20;
            t += zoomobj['zimginfo'][1] / 20;
        } else {
            if ($('zoomimg').width >= zoomobj['zimginfo'][0]) {
                zoomimgadjust(e, 1); return;
            }
            $('zoomimg').width += zoomobj['zimginfo'][0] / 10;
            $('zoomimg').height += zoomobj['zimginfo'][1] / 10;
            l -= zoomobj['zimginfo'][0] / 20;
            t -= zoomobj['zimginfo'][1] / 20;
        }
    } else {
        var clientHeight = document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight;
        var scrollTop = document.body.scrollTop ? document.body.scrollTop : document.documentElement.scrollTop;
        $('zoomimg').width = zoomobj['zimginfo'][0]; $('zoomimg').height = zoomobj['zimginfo'][1];
        var l = (document.body.clientWidth - $('zoomimg').clientWidth) / 2; l = l > 0 ? l : 0;
        var t = (clientHeight - $('zoomimg').clientHeight) / 2 + scrollTop; t = t > 0 ? t : 0;
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

function setIdentify(identify) {
    //identify.style.left = ((document.body.clientWidth - $('wrap').clientWidth)/2 + $('wrap').clientWidth - 400) + 'px';
    identify.style.top = identify.offsetTop + 25 + 'px';
    identify.style.left = $('wrap').offsetWidth / 2 + 300 + 'px';
}

var tags = new Array();
if (typeof (closedtags) == 'undefined') {
    var closedtags;
}
if (typeof (colorfultags) == 'undefined') {
    var colorfultags;
}
function getTopicTags(topicid) {
    _sendRequest('tools/ajax.ashx?t=gettopictags&topicid=' + topicid + '&r=1' + Math.random(), function(responseText) {
        if (responseText) {
            var topic_tags = eval('(' + responseText + ')');
            if (topic_tags.length > 0) {
                var html = '<div class="threadtags">';
                for (var i in topic_tags) {
                    html += '<a href="';
                    if (aspxrewrite == 1) {
                        html += 'topictag-' + topic_tags[i].tagid + '.aspx"';
                    } else {
                        html += 'tags.aspx?t=topic&tagid=' + topic_tags[i].tagid + '"';
                    }

                    if (colorfultags && colorfultags[topic_tags[i].tagid]) {
                        html += ' style="color:#' + colorfultags[topic_tags[i].tagid].color + ';"';
                    }
                    html += '>' + topic_tags[i].tagname + '</a>&nbsp;';
                    tags[tags.length] = topic_tags[i];
                }
                html += "</div>";
                $('topictag').innerHTML = html;
            }
        }
        parsetag();
    });
}

function isexisttaginarray(tagarray, tag) {
    if (tag) {
        for (var i = 0; i < tagarray.length; i++) {
            if (tagarray[i] && tagarray[i].tagid == tag.tagid)
                return true;
        }
    }
    return false;
}

function parsetag() {
    var tagfindarray = new Array();
    var str = $('firstpost').innerHTML.replace(/(^|>)([^<]+)(?=<(?!\/script)|$)/ig, function ($1, $2, $3, $4) {
        for (i in tags) {
            if (tags[i] && !in_array(tags[i].tagid, closedtags) && !isexisttaginarray(tagfindarray, tags[i]) && ($3.indexOf(tags[i].tagname) != -1)
                //修复标签中含有">"或"<"号时匹配错误的问题，
                //如<IMG onload="if(this.width>750) this.width = 750;
                && ($3.indexOf('src=') == -1) && ($3.indexOf('href=') == -1)) {
                $3 = $3.replace(tags[i].tagname, '<h_ ' + i + '>');
                tagfindarray[i] = tags[i];
                tags[i] = '';
            }
        }
        return $2 + $3;
    });

    $('firstpost').innerHTML = str.replace(/<h_ (\d+)>/ig, function ($1, $2) {
        var temp_html = '<span href=\"tools/ajax.ashx?t=topicswithsametag&tagid=' + tagfindarray[$2].tagid + '\" onclick=\"tagshow(event)\" class=\"t_tag\"';
        if (colorfultags && colorfultags[tagfindarray[$2].tagid]) {
            temp_html += ' style=\"color: #' + colorfultags[tagfindarray[$2].tagid].color + '\"';
        }
        temp_html += '>' + tagfindarray[$2].tagname + '</span>';
        return temp_html;
    });
}

function tagshow(event) {
    var obj = is_ie ? event.srcElement : event.target;
    obj.id = !obj.id ? 'tag_' + Math.random() : obj.id;
    ajaxmenu(event, obj.id, 0, '', 1, 3, 0);
    obj.onclick = null;
}

function shrinkUserColumn() {
    var postauthor = getCSSRule(".mainbox td.postauthor");
    var authorhidden = getCSSRule(".authorhidden");
    /*
    if (!postauthor){
    postauthor = addCSSRule(".mainbox td.postauthor");
    }
    */
    //alert(postauthor.style.width);
    if (postauthor.style.width == "0px") {
        postauthor.style.width = "180px";
        postauthor.style.padding = "5px";

        authorhidden.style.display = 'none';
        setcookie("showtopic_shrink", "", 0);
    } else {
        postauthor.style.width = "0px";
        postauthor.style.padding = "0px";

        authorhidden.style.display = '';
        setcookie("showtopic_shrink", "yes", 0);
    }
}


function BOX_show(e, aid) {       //显示
    if ($(e) == null) { return; }
    BOX_layout(e);
    window.onresize = function() { BOX_layout(e); } //改变窗体重新调整位置
    window.onscroll = function() { BOX_layout(e); } //滚动窗体重新调整位置

    if (e == 'attachpaymentlog') {
        loadattachpaymentlog(aid);
    } else if (e == 'buyattach') {
        loadbuyattach(aid);
    }

}

function BOX_remove(e) {       //移除
    window.onscroll = null;
    window.onresize = null;
    //$('BOX_overlay').style.display = "none";
    $(e).style.display = "none";
}



function BOX_layout(e) {       //调整位置
    var a = $(e);
    //    if ($('BOX_overlay') == null) { //判断是否新建遮掩层
    //        var overlay = document.createElement("div");
    //        overlay.setAttribute('id', 'BOX_overlay');
    //        overlay.onclick = function() { BOX_remove(e); };
    //        a.parentNode.appendChild(overlay);
    //    }
    //取客户端左上坐标，宽，高
    var scrollLeft = (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft);
    var scrollTop = (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop);
    var clientWidth = document.documentElement.clientWidth;

    var clientHeight = document.documentElement.clientHeight;

    //    var bo = $('BOX_overlay');
    //    bo.style.left = scrollLeft + 'px';
    //    bo.style.top = scrollTop + 'px';
    //    bo.style.width = clientWidth + 'px';
    //    bo.style.height = clientHeight + 'px';
    //    bo.style.display = "";
    //Popup窗口定位
    a.style.position = 'absolute';
    a.style.zIndex = 101;
    a.style.display = "";
    a.style.left = scrollLeft + ((clientWidth - a.offsetWidth) / 2) + 'px';
    a.style.top = scrollTop + ((clientHeight - a.offsetHeight) / 2) + 'px';

}

function loadattachpaymentlog(aid) {
    _sendRequest('tools/ajax.ashx?t=getattachpaymentlog&aid=' + aid, function(d) {
        try {
            eval('attachpaymentlog_callback(' + d + ')');
        }
        catch (e) {
            var zeroattach_html = '<table cellspacing="0" cellpadding="0" summary="购买" style="width:300px;margin:10px;"><tbody>';
            zeroattach_html += '<tr><td >当前附件未曾被购买!<br /><br /></td></tr>';
            zeroattach_html += '</tbody></table>';
            showDialog(zeroattach_html, "info", "购买附件");
        }
    });
}

function attachpaymentlog_callback(data) {
    var attachpaymentlog_html = '<table summary="记录列表" cellspacing="0" cellpadding="0" style="width:300px;margin:10px;">';
    attachpaymentlog_html += '<tr><th>用户名</th><th>时间</th><th>' + attachtransname + '</th></tr><tbody>';
    if (data.length == 0)
        attachpaymentlog_html += '<tr><td colspan="3" style="text-align:center;">当前附件未曾被购买!</td></tr>';
    else {
        for (var i in data) {
            attachpaymentlog_html += '<tr><td><a href="userinfo.aspx?userid=' + data[i].uid + '">' + data[i].username + '</a></td>';
            attachpaymentlog_html += '<td>' + convertdate(data[i].postdatetime) + '</td>';
            attachpaymentlog_html += '<td>' + data[i].amount + '</td></tr>';
        }
    }
    attachpaymentlog_html += '</tbody></table>';
    showDialog(attachpaymentlog_html, "info", "购买记录");
}

function loadbuyattach(aid) {
    _sendRequest('tools/ajax.ashx?t=checkuserextcredit&aid=' + aid, function(d) {
        try {
            return eval('loadbuyattach_callback(' + d + ')');
        } catch (e) { alert(e.message); };
    });
}

function loadbuyattach_callback(data) {
    var buyattach_html = '';
    if (!data[0].haserror) {
        var extname = data[0].extname != '' ? '(' + data[0].extname + ')' : '';
        buyattach_html += '<table cellspacing="0" cellpadding="0" summary="购买" style="width:300px;margin:0 10px;"><tbody>';
        buyattach_html += '<tr><th width="150" height="24"><label for="keyword">作者:</th><td><a href="userinfo.aspx?userid=' + data[0].posterid + '" target="_blank">' + data[0].poster + '</a></td></tr>';
        buyattach_html += '<tr><th width="150" height="24"><label for="keyword">附件:</th><td>' + data[0].attachname + '</td></tr>';
        buyattach_html += '<tr><th width="150" height="24"><label for="keyword">售价' + extname + ':</th><td>' + data[0].attachprice + '</td></tr>';
        buyattach_html += '<tr><th width="150" height="24"><label for="keyword">作者所得' + extname + ':</th><td>' + data[0].attachprice + '</td></tr>';
        buyattach_html += '<tr><th width="150" height="24"><label for="keyword">购买后余额' + extname + ':</th><td>' + data[0].leavemoney + '</td></tr>';
        buyattach_html += '<tr><th width="150" height="60" style="text-align:center;"></th><th style="text-align:center;"><button class="pn pnc" value="true"onclick="javascript:confirmbuyattach(' + data[0].aid + ');"><strong>购买</strong></button></th></tr>';
        buyattach_html += '</tbody></table>';
    }
    else {
        buyattach_html += '<table cellspacing="0" cellpadding="0" summary="购买" style="width:300px;margin:10px;"><tbody>';
        buyattach_html += '<tr><td >' +  data[0].errormsg + '<br /><br /></td></tr>';
        buyattach_html += '<tr><td style="text-align:center;"><button class="pn pnc" value="true" id="fwin_dialog_submit"><strong>关闭</strong></button></td></tr>';
        buyattach_html += '</tbody></table>';
    }
    showDialog(buyattach_html, "info", "购买附件"); 
}

function confirmbuyattach(aid) {
    var buyattach_html = '<div class="floatwinmask" style="padding:7px;">';
    buyattach_html += '<div class="floatwin" id="floatwin">';
    buyattach_html += '<h3 class="float_ctrl" style="margin:0"><span><a title="关闭" class="float_close" href="javascript:void(0);" onclick="javascript:BOX_remove(\'buyattach\');" style="margin:7px;"></a></span></h3>';
    buyattach_html += '<div style="text-align:center;">正在提交数据...</div></div></div>';
    //    $('fwin_dialog').innerHTML = buyattach_html;
    showDialog(buyattach_html, "info2", "购买附件"); 
    _sendRequest('tools/ajax.ashx?t=confirmbuyattach&aid=' + aid, function(d) {
        try {
            eval('confirmbuyattach_callback(' + d + ')');
        } catch (e) { };
    });
}

function confirmbuyattach_callback(data) {
    var buyattach_html = '<div class="floatwin" id="floatwin" style="width: 500px; height: 310px; z-index: 999;">';
    buyattach_html += '<h3><span><a title="关闭" class="float_close" href="javascript:void(0);" onclick="javascript:BOX_remove(\'buyattach\');"></a></span> &raquo; 系统提示</h3>';
    if (!data[0].haserror) {
        window.location.hash = '#attach' + data[0].aid;
        if (!is_ie) {
            window.location.reload();
        } else {
            document.execCommand('Refresh'); //ie刷新
        }
    }
    else {
        buyattach_html += '<div style="padding-top:80px; text-align:center;">' + data[0].errormsg + '</div>';
        //$('buyattach').innerHTML = buyattach_html;
        showDialog(buyattach_html, "info3", "购买附件"); 
    }
}

var authort;
function showauthor(obj, id) {
    authort = setTimeout("$('" + id + "').style.display = 'block';", 500);
    if (!$(id).onmouseover) {
        $(id + '_ma').innerHTML = $(id + '_a').innerHTML;
        $(id).onmouseover = function() {
            $(id).style.display = 'block';
        }
        $(id).onmouseout = function() {
            $(id).style.display = 'none';
        }
    }
    if (!obj.onmouseout) {
        obj.onmouseout = function() {
            clearTimeout(authort);
        }
    }
}

function modthreads(optgroup, operation) {
    var operation = !operation ? '' : operation;
    //var windowWidth = 250;
    //var windowHeight = 220;
    /* switch (operation) {
        case "split":
            windowHeight = 300;
            break;
        case "identify":
            windowHeight = 230;
            break;
        default:
            windowHeight = 220;
    } */
    //floatwin('open_mods', '', windowWidth, windowHeight);
    hideWindow('mods');
    $('moderate').optgroup.value = optgroup;
    $('moderate').operat.value = operation;
    //$('floatwin_mods').innerHTML = '';
    //ajaxpost('moderate', 'floatwin_mods', '');
    


    

    showWindow('mods', 'moderate', 'post', 0);

    if(BROWSER.ie) {

        doane(event);

    }	
}

function getElementsByName(tag, name) {
    var returns = document.getElementsByName(name);
    if (returns.length > 0) return returns;
    returns = new Array();
    var e = document.getElementsByTagName(tag);
    for (var i = 0; i < e.length; i++) {
        if (e[i].getAttribute("name") == name) {
            returns[returns.length] = e[i];
        }
    }
    return returns;
}


function getuserips() {
    var t = getElementsByName("em", "traveler_ip");
    if (t.length == 0) return;

    var iplist = new Array();
    var pidlist = new Array();
    //for(var i=0;i<t.length;i++)
    for (var i = 0; i < t.length; i++) {
        iplist.push(t[i].innerHTML);
        pidlist.push(t[i].id.split("_")[2]);
    }
    _sendRequest('tools/ajax.ashx?t=getiplist', backIpvalues_callback, false, 'iplist=' + iplist.join(",") + '&pidlist=' + pidlist.join(","));
}
function backIpvalues_callback(doc) {

    var data = eval(doc);
    for (var i = 0; i < data.length; i++) {
        var d = data[i].split("|");
        $('traveler_ip_' + d[0]).style.display = '';
        $('traveler_ip_' + d[0]).innerHTML = d[1].split(" ")[0] + '网友';
    }
}

function fontZoom(id, zoomOut) {
    var message = $(id);
    if (zoomOut && message.className == 't_bigfont')
        return;
    if (!zoomOut && message.className == 't_msgfont')
        return;
    message.className = zoomOut ? 't_bigfont' : 't_msgfont';
    var fontCollention = message.getElementsByTagName("font")
    for (var i = 0; i < fontCollention.length; i++) {
        var node = fontCollention[i];
        var size = parseInt(node.getAttribute("size"));
        if (zoomOut) {
            node.setAttribute("oldsize", size);
            size = size == 7 ? size : ++size;
        }
        else {
            size = node.getAttribute("oldsize");
        }
        node.setAttribute("size", size);
    }
}

function loadImg(obj) {
    obj.setAttribute('src', '/images/common/loading_wide.gif');
    var cache = document.createElement("IMG");
    cache.setAttribute('src', obj.getAttribute('newsrc'));
    cache.setAttribute('onload', LoadImgComplete(obj, cache));
    $('imgcache').appendChild(cache);
}

function LoadImgComplete(obj, cache) {
    setTimeout(function on() {
        var img = new Image();
        img.width = 0;
        img.src = cache.src;
        var imgwidth = cache.width > 0 ? cache.width : img.width;

        if (BROWSER.ie) {
            if (!cache.complete && img.width == 28 && img.height == 30) {
                LoadImgError(obj, cache);
                return;
            }
        }
        else {
            if (imgwidth <= 0) {
                LoadImgError(obj, cache);
                return;
            }
        }
        obj.setAttribute('src', cache.getAttribute('src'));
        obj.setAttribute('newsrc', null);
        obj.setAttribute('alt', '');
        obj.onclick = imgwidth >= 600 ? function on() { zoom(obj); } : null;
        var imgattachid = parseInt(obj.getAttribute('imageid'));

        var inpost = obj.getAttribute('inpost') == '1';
        obj.onmouseover = inpost ? function on() { attachimg(obj, 'mouseover'); attachimginfo(obj, 'attach_' + imgattachid, 1); } : function on() { attachimg(obj, 'mouseover'); };
    }
    , 800);
}

function LoadImgError(obj, cache) {
        obj.setAttribute('src', '/images/common/imgerror.png');
        obj.onmouseover = null;
        obj.setAttribute('alt', '点击重新加载');
}

function loadAllImg() {
    var div = $('postsContainer');
    var imgattach = document.getElementsByTagName("IMG");

    for (i = 0; i < imgattach.length; i++) {
        var newsrc = imgattach[i].getAttribute('newsrc');
        if (newsrc != null && newsrc != '')
            loadImg(imgattach[i]);
    }
}

function copypostlayer(tid, pid, forumpath) {
    var host = window.location.host;
    var path = 'http://' + host + forumpath + 'showtopic.aspx?topicid=' + tid + '&postid=' + pid + '#' + pid;
    setcopy(path, '已经复制到剪贴板');
}