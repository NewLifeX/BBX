var postSubmited = false;
var smdiv = new Array();

function AddText(txt) {
    try {
        obj = typeof $('postform').message != 'undefined' ? $('postform').message : $('e_textarea');
    } catch (e) { 
        obj = typeof $('quickpostform').message != 'undefined' ? $('quickpostform').message : $('quickpostmessage');
    }
    selection = document.selection;
    checkFocus();
    if(!isUndefined(obj.selectionStart)) {
        var opn = obj.selectionStart + 0;
        obj.value = obj.value.substr(0, obj.selectionStart) + txt + obj.value.substr(obj.selectionEnd);
    } else if(selection && selection.createRange) {
        var sel = selection.createRange();
        sel.text = txt;
        sel.moveStart('character', -strlen(txt));
    } else {
        obj.value += txt;
    }
}

function checkFocus() {
    var textarea;
    try {
        textarea = typeof $('postform').message != 'undefined' ? $('postform').message : $('e_textarea');
    } catch (e) { 
        textarea = typeof $('quickpostform').message != 'undefined' ? $('quickpostform').message : $('quickpostmessage');
    }
    var obj = typeof wysiwyg == 'undefined' || !wysiwyg ? textarea : editwin;
    if(!obj.hasfocus) {
        obj.focus();
    }
}

function ctlent(event) {
    if(postSubmited == false && (event.ctrlKey && event.keyCode == 13) || (event.altKey && event.keyCode == 83) && $('postsubmit')) {
        if(in_array($('postsubmit').name, ['topicsubmit', 'replysubmit', 'editsubmit']) && !validate($('postform'))) {
            doane(event);
            return;
        }
        postSubmited = true;
        $('postsubmit').disabled = true;
        $('postform').submit();
    }
}

function ctltab(event) {
    if(event.keyCode == 9) {
        doane(event);
    }
}

function ctlentParent(event) {
    var pForm = parent.window.document.getElementById('postform');
    var pSubmit = parent.window.document.getElementById('postsubmit');

    if(postSubmited == false && (event.ctrlKey && event.keyCode == 13) || (event.altKey && event.keyCode == 83) && pSubmit) {
        if (parent.window.validate && !parent.window.validate(pForm))
        {
            doane(event);
            return;
        }
        postSubmited = true;
        pSubmit.disabled = true;
        pForm.submit();
    }
}

function deleteData() {
    if(is_ie) {
        saveData('', 'delete');
    } else if(window.sessionStorage) {
        try {
            sessionStorage.removeItem('BBX');
        } catch(e) {}
    }
}

function insertSmiley(smilieid) {
    checkFocus();
    var src = $('smilie_' + smilieid).src;
    var code = $('smilie_' + smilieid).alt;
    if(typeof wysiwyg != 'undefined' && wysiwyg && allowsmilies && (!$('smileyoff') || $('smileyoff').checked == false)) {
        if(is_moz) {
            applyFormat('InsertImage', false, src);
            
            var smilies = editdoc.body.getElementsByTagName('img');
            for(var i = 0; i < smilies.length; i++) {
                if(smilies[i].src == src && smilies[i].getAttribute('smilieid') < 1) {
                    smilies[i].setAttribute('smilieid', smilieid);
                    smilies[i].setAttribute('border', "0");
                }
            }
        } else {
    insertText('<img src="' + src + '" border="0" smilieid="' + smilieid + '" alt="" onload="w=width;h=height;" onresize="style.width=w;style.height=h;" /> ', false);
        }
    } else {
        code += ' ';
        AddText(code);
    }
    hideMenu();
}

function smileyMenu(ctrl) {
    ctrl.style.cursor = 'pointer';
    if(ctrl.alt) {
        ctrl.pop = ctrl.alt;
        ctrl.alt = '';
    }
    if(ctrl.title) {
        ctrl.lw = ctrl.title;
        ctrl.title = '';
    }
    //if(!smdiv[ctrl.id]) {
        smdiv[ctrl.id] = document.createElement('div');
        smdiv[ctrl.id].id = ctrl.id + '_menu';
        smdiv[ctrl.id].style.display = 'none';
        smdiv[ctrl.id].style.width = '60px';
        smdiv[ctrl.id].style.height = '60px';
        smdiv[ctrl.id].className = 'popupmenu_popup';
        ctrl.parentNode.appendChild(smdiv[ctrl.id]);
    //}
    smdiv[ctrl.id].innerHTML = '<table width="100%" height="100%"><tr><td align="center" valign="middle"><img src="' + ctrl.src + '" border="0" /></td></tr></table>';
    showMenu(ctrl.id, 0, 0, 1, 0);
}



function showsmiles(index, typename, pageindex, seditorKey)
{
    $("s_" + index).className = "current";
    var cIndex = 1;
    for (i in smilies_HASH) {
        if (cIndex != index) {
            $("s_" + cIndex).className = "";
        }
        $("s_" + cIndex).style.display = "";
        cIndex ++;
    }

    var pagesize = (typeof smiliesCount) == 'undefined' ? 12 : smiliesCount;
    var url = (typeof forumurl) == 'undefined' ? '' : forumurl;
    var s = smilies_HASH[typename];
    var pagecount = Math.ceil(s.length/pagesize);
    var inseditor = typeof seditorKey != 'undefined';

    if (isUndefined(pageindex)) {
        pageindex = 1;
    }
    if (pageindex > pagecount) {
        pageindex = pagecount;
    }

    var maxIndex = pageindex*pagesize;
    if (maxIndex > s.length) {
        maxIndex = s.length;
    }
    maxIndex = maxIndex - 1;

    var minIndex = (pageindex-1)*pagesize;

    var html = '<table id="' + index + '_table" cellpadding="0" cellspacing="0" style="clear: both"><tr>';

    var ci = 1;
    for (var id = minIndex; id <= maxIndex; id++) {
        var clickevt = 'insertSmiley(\'' + addslashes(s[id]['code']) + '\');';
        if (inseditor) {
            clickevt = 'seditor_insertunit(\'' + seditorKey + '\', \'' + s[id]['code'] + '\');';
        }

        html += '<td valign="middle"><img style="cursor: pointer;" src="' + url + 'editor/images/smilies/' + s[id]['url'] + '" id="smilie_' + s[id]['code'] + '" alt="' + s[id]['code'] + '" onclick="' + clickevt + '" onmouseover="smilies_preview(\'s\', this, 40)" onmouseout="smilies_preview(\'s\')" title="" border="0" height="20" width="20" /></td>';
        if (ci%colCount == 0) {
            html += '</tr><tr>'
        }
        ci ++;
    }

    html += '<td colspan="' + (colCount - ((ci-1) % colCount)) + '"></td>';
    html += '</tr>';
    html += '</table>';
    $(seditorKey+"showsmilie").innerHTML = html;

    if (pagecount > 1) {
        html = '<div class="p_bar">';
        for (var i = 1; i <= pagecount; i++) {
            if (i == pageindex) {
                html += "<a class=\"p_curpage\">" + i + "</a>";
            }
            else {
                html += "<a class=\"p_num\" href='#smiliyanchor' onclick=\"showsmiles(" + index + ", '" + typename + "', " + i + ", '"+seditorKey+"')\">" + i + "</a>"
            }
        }
        html += '</div>'
        $(seditorKey+"showsmilie_pagenum").innerHTML = html;
    }
    else {
        $(seditorKey+"showsmilie_pagenum").innerHTML = "";
    }
}

function showFirstPageSmilies(firstpagesmilies, defaulttypename, maxcount, seditorKey)
{
    var html = '<table align="center" border="0" cellpadding="3" cellspacing="0" width="90%"><tr>';
    var ci = 1;
    var inseditor = (typeof seditorKey != 'undefined');
    var url = (typeof forumurl) == 'undefined' ? '' : forumurl;
    var s = firstpagesmilies[defaulttypename];
    for (var id = 0; id <= maxcount - 1; id++) {
        if(s[id] == null)
            continue;
        var clickevt = 'insertSmiley(\'' + addslashes(s[id]['code']) + '\');';
        if (inseditor) {
            clickevt = 'seditor_insertunit(\'' + seditorKey + '\', \'' + s[id]['code'] + '\');';
        }
        html += '<td valign="middle"><img style="cursor: pointer;" src="' + url + 'editor/images/smilies/' + s[id]['url'] + '" id=smilie_' + s[id]['code'] + ' alt="' + s[id]['code'] + '" onclick="' + clickevt + '" onmouseover="smilies_preview(\'s\', this, 40)" onmouseout="smilies_preview(\'s\')" title="" border="0" height="20" width="20" /></td>';
        if (ci%4 == 0) {
            html += '</tr><tr>'
        }
        ci ++;
    }
    html += '<td colspan="' + (4 - ((ci-1) % 4)) + '"></td>';
    html += '</tr>';
    html += '</table>';

    $("showsmilie").innerHTML = html;
}

function scrollSmilieTypeBar(bar, scrollwidth)
{
    //bar.scrollLeft += scrollwidth;
    var i = 0;
    if (scrollwidth > 0) {
        var scl = window.setInterval(function(){
            if (i < scrollwidth) {
                bar.scrollLeft += 1;
                i++
            }
            else
                window.clearInterval(scl);
        }, 1);
    }
    else {
        var scl = window.setInterval(function(){
            if (i > scrollwidth) {
                bar.scrollLeft -= 1;
                i--
            }
            else
                window.clearInterval(scl);
        }, 1);
    }
}
function smilies_preview(id, obj, v) {
    if(!obj) {
        $(id + '_preview_table').style.display = 'none';
    } else {
        $(id + '_preview_table').style.display = '';
        $(id + '_preview').innerHTML = '<img src="' + obj.src + '" />';
    }
}

function parseurl(str, mode) {
    str = str.replace(/([^>=\]"'\/]|^)((((https?|ftp):\/\/)|www\.)([\w\-]+\.)*[\w\-\u4e00-\u9fa5]+\.([\.a-zA-Z0-9]+|\u4E2D\u56FD|\u7F51\u7EDC|\u516C\u53F8)((\?|\/|:)+[\w\.\/=\?%\-&~`@':+!]*)+\.(jpg|gif|png|bmp))/ig, mode == 'html' ? '$1<img src="$2" border="0">' : '$1[img]$2[/img]');
    str = str.replace(/([^>=\]"'\/@]|^)((((https?|ftp|gopher|news|telnet|rtsp|mms|callto|bctp|ed2k|thunder|qqdl|synacast):\/\/))([\w\-]+\.)*[:\.@\-\w\u4e00-\u9fa5]+\.([\.a-zA-Z0-9]+|\u4E2D\u56FD|\u7F51\u7EDC|\u516C\u53F8)((\?|\/|:)+[\w\.\/=\?%\-&;~`@':+!#]*)*)/ig, mode == 'html' ? '$1<a href="$2" target="_blank">$2</a>' : '$1[url]$2[/url]'); 
    str = str.replace(/([^\w>=\]"'\/@]|^)((www\.)([\w\-]+\.)*[:\.@\-\w\u4e00-\u9fa5]+\.([\.a-zA-Z0-9]+|\u4E2D\u56FD|\u7F51\u7EDC|\u516C\u53F8)((\?|\/|:)+[\w\.\/=\?%\-&~`@':+!#]*)*)/ig, mode == 'html' ? '$1<a href="$2" target="_blank">$2</a>' : '$1[url]$2[/url]');
    str = str.replace(/([^\w->=\]:"'\.\/]|^)(([\-\.\w]+@[\.\-\w]+(\.\w+)+))/ig, mode == 'html' ? '$1<a href="mailto:$2">$2</a>' : '$1[email]$2[/email]');
    return str;
}

function getData(tagname) {
    if (typeof tagname == 'undefined' || tagname == '')
    {
        tagname = 'BBX';
    }
    var message = '';
    if(is_ie) {
        try {
            textobj.load(tagname);
            var oXMLDoc = textobj.XMLDocument;
            var nodes = oXMLDoc.documentElement.childNodes;
            message = nodes.item(nodes.length - 1).getAttribute('message');
        } catch(e) {}
    } else if(window.sessionStorage) {
        try {
            message = sessionStorage.getItem(tagname);
            if (!message)
                message = "";
        } catch(e) {}
    }
    return message.toString();

}

function setData(data, tagname) {
    if (typeof tagname == 'undefined' || tagname == '')
    {
        tagname = 'BBX';
    }
    if(is_ie) {
        try {
            var oXMLDoc = textobj.XMLDocument;
            var root = oXMLDoc.firstChild;
            if(root.childNodes.length > 0) {
                root.removeChild(root.firstChild);
            }
            var node = oXMLDoc.createNode(1, 'POST', '');
            var oTimeNow = new Date();
            oTimeNow.setHours(oTimeNow.getHours() + 24);
            textobj.expires = oTimeNow.toUTCString();
            node.setAttribute('message', data);
            oXMLDoc.documentElement.appendChild(node);
            textobj.save(tagname);
        } catch(e) {}
    } else if(window.sessionStorage) {
        try {
            sessionStorage.setItem(tagname, data);
        } catch(e) {}
    }

}

var autosaveDatai, autosaveDatatime;
function autosaveData(op) {
    var autosaveInterval = 60;
    obj = $(editorid + '_cmd_autosave');
    if(op) {
        if(op == 2) {
            saveData(wysiwyg ? editdoc.body.innerHTML : textobj.value);
        } else {
            setcookie('disableautosave', '', -2592000);
        }
        autosaveDatatime = autosaveInterval;
        autosaveDatai = setInterval(function() {
            autosaveDatatime--;
            if(autosaveDatatime == 0) {
                saveData(wysiwyg ? editdoc.body.innerHTML : textobj.value);
                autosaveDatatime = autosaveInterval;
            }
            if($('autsavet')) {
                $('autsavet').innerHTML = '(' + autosaveDatatime + '秒' + ')';
            }
        }, 1000);
        obj.onclick = function() { autosaveData(0); }
    } else {
        setcookie('disableautosave', 1, 2592000);
        clearInterval(autosaveDatai);
        $('autsavet').innerHTML = '(已停止)';
        obj.onclick = function() { autosaveData(1); }
    }
}

function setCaretAtEnd() {
    if(typeof wysiwyg != 'undefined' && wysiwyg) {
        editdoc.body.innerHTML += '';
    } else {
        editdoc.value += '';
    }
}

function storeCaret(textEl){
    if(textEl.createTextRange){
        textEl.caretPos = document.selection.createRange().duplicate();
    }
}

if (BROWSER.ie >= 5 || is_moz >= 2) {
    window.onbeforeunload = function () {
        try {
            saveData(wysiwyg && bbinsert ? editdoc.body.innerHTML : textobj.value);
        } catch(e) {}
    };
}

function insertmedia() {
    InFloat = InFloat_Editor;
    if(is_ie) $(editorid + '_mediaurl').pos = getCaret();
    showMenu(editorid + '_popup_media', true, 0, 2);
}

function setmediacode(editorid) {
    checkFocus();
    if(is_ie) setCaret($(editorid + '_mediaurl').pos);
    insertText('[media='+$(editorid + '_mediatype').value+
        ','+$(editorid + '_mediawidth').value+
        ','+$(editorid + '_mediaheight').value+']'+
        $(editorid + '_mediaurl').value+'[/media]');
    $(editorid + '_mediaurl').value = '';
    hideMenu();
}


function setmediatype(editorid) {
    var ext = $(editorid + '_mediaurl').value.lastIndexOf('.') == -1 ? '' : $(editorid + '_mediaurl').value.substr($(editorid + '_mediaurl').value.lastIndexOf('.') + 1, $(editorid + '_mediaurl').value.length).toLowerCase();
    if(ext == 'rmvb') {
        ext = 'rm';
    }
    if($(editorid + '_mediatyperadio_' + ext)) {
        $(editorid + '_mediatyperadio_' + ext).checked = true;
        $(editorid + '_mediatype').value = ext;
    }
}

var divdragstart = new Array();
function divdrag(e, op, obj) {
    if(op == 1) {
        divdragstart = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
        divdragstart[2] = parseInt(obj.style.left);
        divdragstart[3] = parseInt(obj.style.top);
        doane(e);
    } else if(op == 2 && divdragstart[0]) {
        var divdragnow = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
        obj.style.left = (divdragstart[2] + divdragnow[0] - divdragstart[0]) + 'px';
        obj.style.top = (divdragstart[3] + divdragnow[1] - divdragstart[1]) + 'px';
        doane(e);
    } else if(op == 3) {
        divdragstart = [];
        doane(e);
    }
}

function pagescrolls(op) {
    if(!infloat && op.substr(0, 6) == 'credit') {
        window.open('faq.php?action=credits&fid=' + fid);
        return;
    }
    switch(op) {
        case 'credit1':hideMenu();$('moreconf').style.display = 'none';$('extcreditbox1').innerHTML = $('extcreditbox').innerHTML;pagescroll.left();break;
        case 'credit2':$('moreconf').style.display = 'none';$('extcreditbox2').innerHTML = $('extcreditbox').innerHTML;pagescroll.left();break;
        case 'credit3':hideMenu();$('moreconf').style.display = 'none';$('extcreditbox3').innerHTML = $('extcreditbox').innerHTML;pagescroll.left();break;
        case 'return':if(!Editorwin) {hideMenu();$('custominfoarea').style.display=$('more_2').style.display='none';pagescroll.up(1, '$(\'more_1\').style.display=\'\'');}break;
        case 'creditreturn':pagescroll.right(1, '$(\'moreconf\').style.display = \'\';');break;
        case 'swf':hideMenu();$('moreconf').style.display = 'none';swfHandler(3);break;
        case 'swfreturn':$('swfbox').style.display = 'none';if(!Editorwin) {pagescroll.left(1, '$(\'moreconf\').style.display = \'\';');}swfHandler(2);break;
        case 'more':hideMenu();pagescroll.down(1, '$(\'more_1\').style.display=$(\'more_2\').style.display=$(\'custominfoarea\').style.display=\'none\'');break;
        case 'editorreturn':$('more_1').style.display='none';pagescroll.up(1, '$(\'more_2\').style.display=$(\'custominfoarea\').style.display=\'\'');break;
        case 'editor':$('more_1').style.display='none';pagescroll.down(1, '$(\'more_2\').style.display=\'\';$(\'custominfoarea\').style.display=\'\'');break;
    }
}

function switchicon(iconid, obj) {
    $('iconid').value = iconid;
    $('icon_img').src = obj.src;
    hideMenu();
}

var swfuploaded = 0;
function swfHandler(action) {
    if(action == 1) {
        swfuploaded = 1;
    } else if(action == 2) {
        if(Editorwin || !infloat) {
            swfuploadwin();
        } else {
            $('swfbox').style.display = 'none';
            pagescroll.left(1, '$(\'moreconf\').style.display=\'\';');
        }
        if(swfuploaded) {
            swfattachlistupdate(action);
        }
    } else if(action == 3) {
        swfuploaded = 0;
        pagescroll.right(1, '$(\'swfuploadbox\').style.display = $(\'swfbox\').style.display = \'\';');
    }
}

function swfattachlistupdate(action) {
    ajaxget('ajax.php?action=swfattachlist', 'swfattachlist', 'swfattachlist', 'swfattachlist', null, '$(\'uploadlist\').scrollTop=10000');
    attachlist('open');
    $('postform').updateswfattach.value = 1;
}

function appendreply() {
    newpos = fetchOffset($('post_new'));
    document.documentElement.scrollTop = newpos['top'];
    $('post_new').style.display = '';
    $('post_new').id = '';
    div = document.createElement('div');
    div.id = 'post_new';
    div.style.display = 'none';
    div.className = '';
    $('postlistreply').appendChild(div);
    $('postform').replysubmit.disabled = false;
    creditnoticewin();
}

var Editorwin = 0;
function resizeEditorwin() {
    var obj = $('resizeEditorwin');
    floatwin('size_' + editoraction);
    $('editorbox').style.height = $('floatlayout_' + editoraction).style.height = $('floatwin_' + editoraction).style.height;
    if(!Editorwin) {
        obj.className = 'float_min';
        obj.title = obj.innerHTML = '还原大小';
        $('editorbox').style.width = $('floatlayout_' + editoraction).style.width = (parseInt($('floatwin_' + editoraction).style.width) - 10)+ 'px';
        $('editorbox').style.left = '0px';
        $('editorbox').style.top = '0px';
        $('swfuploadbox').style.display = $('custominfoarea').style.display = $('creditlink').style.display = $('morelink').style.display = 'none';
        if(wysiwyg) {
            $('e_iframe').style.height = (parseInt($('floatwin_' + editoraction).style.height) - 150)+ 'px';
        }
        $('e_textarea').style.height = (parseInt($('floatwin_' + editoraction).style.height) - 150)+ 'px';
        attachlist('close');
        Editorwin = 1;
    } else {
        obj.className = 'float_max';
        obj.title = obj.innerHTML = '最大化';
        $('editorbox').style.width = $('floatlayout_' + editoraction).style.width = '600px';
        $('swfuploadbox').style.display = $('custominfoarea').style.display = $('creditlink').style.display = $('morelink').style.display = '';
        if(wysiwyg) {
            $('e_iframe').style.height = '';
        }
        $('e_textarea').style.height = '';
        swfuploadwin();
        Editorwin = 0;
    }
}

function closeEditorwin() {
    if(Editorwin) {
        resizeEditorwin();
    }
    floatwin('close_' + editoraction);
}

function editorwindowopen(url) {
    data = wysiwyg ? editdoc.body.innerHTML : textobj.value;
    saveData(data);
    url += '&cedit=' + (data !== '' ? 'yes' : 'no');
    window.open(url);
}

function swfuploadwin() {
    if(Editorwin) {
        if($('swfuploadbox').style.display == 'none') {
            $('swfuploadbox').className = 'floatbox floatbox1 floatboxswf floatwin swfwin';
            $('swfuploadbox').style.position = 'absolute';
            width = (parseInt($('floatlayout_' + editoraction).style.width) - 604) / 2;
            $('swfuploadbox').style.left = width + 'px';
            $('swfuploadbox').style.display = $('swfclosebtn').style.display = $('swfbox').style.display = '';

        } else {
            $('swfuploadbox').className = 'floatbox floatbox1 floatboxswf';
            $('swfuploadbox').style.position = $('swfuploadbox').style.left = '';
            $('swfuploadbox').style.display = $('swfclosebtn').style.display = 'none';
        }
    } else {
        if(infloat) {
            pagescrolls('swf');
        } else {
            if($('swfuploadbox').style.display == 'none') {
                $('swfuploadbox').style.display = $('swfbox').style.display = $('swfclosebtn').style.display = '';
            } else {
                $('swfuploadbox').style.display = $('swfbox').style.display = $('swfclosebtn').style.display = 'none';
            }
        }
    }
}


function uploadAttach(curId, statusid, prefix) {
    prefix = isUndefined(prefix) ? '' : prefix;
    var nextId = 0;
    for(var i = 0; i < AID - 1; i++) {
        if($(prefix + 'attachform_' + i)) {
            nextId = i;
            if(curId == 0) {
                break;
            } else {
                if(i > curId) {
                    break;
                }
            }
        }
    }
    if(nextId == 0) {
        return;
    }
    CURRENTATTACH = nextId + '|' + prefix;
    if(curId > 0) {
        if(statusid == 0) {
            UPLOADCOMPLETE++;
        } else {
            FAILEDATTACHS += '<br />' + mb_cutstr($(prefix + 'attachnew_' + curId).value.substr($(prefix + 'attachnew_' + curId).value.replace(/\\/g, '/').lastIndexOf('/') + 1), 25) + ': ' + STATUSMSG[statusid];
            UPLOADFAILED++;
        }
        $(prefix + 'cpdel_' + curId).innerHTML = '<img src="' + IMGDIR + '/check_' + (statusid == 0 ? 'right' : 'error') + '.gif" alt="' + STATUSMSG[statusid] + '" />';
        if(nextId == curId || in_array(statusid, [6, 8])) {
            if (prefix == 'img') {
                updateImageList(2);
            }
            else {
                updateAttachListbycount(UPLOADCOMPLETE);
            }
            if(UPLOADFAILED > 0) {
                showDialog('附件上传完成！成功 ' + UPLOADCOMPLETE + ' 个，失败 ' + UPLOADFAILED + ' 个:' + FAILEDATTACHS);
                FAILEDATTACHS = '';
            }
            UPLOADSTATUS = 2;
            for(var i = 0; i < AID - 1; i++) {
                if($(prefix + 'attachform_' + i)) {
                    reAddAttach(prefix, i)
                }
            }
            $(prefix + 'uploadbtn').style.display = '';
            $(prefix + 'uploading').style.display = 'none';
            if(AUTOPOST) {
                hideMenu();
                validate($('postform'));
            } else if(UPLOADFAILED == 0 && (prefix == 'img' || prefix == '')) {
                showDialog('附件上传完成！', 'notice');
            }
            UPLOADFAILED = UPLOADCOMPLETE = 0;
            CURRENTATTACH = '0';
            FAILEDATTACHS = '';
            return;
        }
    } else {
        $(prefix + 'uploadbtn').style.display = 'none';
        $(prefix + 'uploading').style.display = '';
    }
    $(prefix + 'cpdel_' + nextId).innerHTML = '<img src="' + IMGDIR + '/loading.gif" alt="上传中..." />';
    UPLOADSTATUS = 1;
    $(prefix + 'attachform_' + nextId).submit();
}

var postSubmited = false;
var AID = 1;
var UPLOADSTATUS = -1;
var UPLOADFAILED = UPLOADCOMPLETE = AUTOPOST =  0;
var CURRENTATTACH = '0';
var FAILEDATTACHS = '';
var UPLOADWINRECALL = null;
var STATUSMSG = {'-1' : '内部服务器错误', '0' : '上传成功', '1' : '不支持此类扩展名', '2' : '附件大小为 0', '3' : '附件大小超限', '4' : '不支持此类扩展名', '5' : '附件大小超限', '6' : '附件总大小超限', '7' : '图片附件不合法', '8' : '附件文件无法保存', '9' : '没有合法的文件被上传', '10' : '非法操作','11' : '您没有上传权限'};

function checkFocus() {
    var obj = wysiwyg ? editwin : textobj;
    if(!obj.hasfocus) {
        obj.focus();
    }
}

function ctlent(event) {
    if(postSubmited == false && (event.ctrlKey && event.keyCode == 13) || (event.altKey && event.keyCode == 83) && $('postsubmit')) {
        if(in_array($('postsubmit').name, ['topicsubmit', 'replysubmit', 'editsubmit']) && !validate($('postform'))) {
            doane(event);
            return;
        }
        postSubmited = true;
        $('postsubmit').disabled = true;
        $('postform').submit();
    }
    if(event.keyCode == 9) {
        doane(event);
    }
}

function checklength(theform) {
    var message = wysiwyg ? html2bbcode(getEditorContents()) : (!theform.parseurloff.checked ? parseurl(theform.message.value) : theform.message.value);
    showDialog('当前长度: ' + mb_strlen(message) + ' 字节，' + (postmaxchars != 0 ? '系统限制: ' + postminchars + ' 到 ' + postmaxchars + ' 字节。' : ''), 'notice', '字数检查');
}

if(!tradepost) {
    var tradepost = 0;
}

function postsubmit(theform) {
    theform.replysubmit ? theform.replysubmit.disabled = true : (theform.editsubmit ? theform.editsubmit.disabled = true : theform.topicsubmit.disabled = true);
    theform.submit();
}

function evalevent(obj) {
    var script = obj.parentNode.innerHTML;
    var re = /onclick="(.+?)["|>]/ig;
    var matches = re.exec(script);
    if(matches != null) {
        matches[1] = matches[1].replace(/this\./ig, 'obj.');
        eval(matches[1]);
    }
}

function relatekw(subject, message, recall) {
    if(isUndefined(recall)) recall = '';
    if(isUndefined(subject) || subject == -1) subject = $('subject').value;
    if(isUndefined(message) || message == -1) message = getEditorContents();
    subject = (BROWSER.ie && document.charset == 'utf-8' ? encodeURIComponent(subject) : subject);
    message = (BROWSER.ie && document.charset == 'utf-8' ? encodeURIComponent(message) : message);
    message = message.replace(/&/ig, '', message).substr(0, 500);
    ajaxget('forum.php?mod=relatekw&subjectenc=' + subject + '&messageenc=' + message, 'tagselect', '', '', '', recall);
}

function switchicon(iconid, obj) {
    $('iconid').value = iconid;
    $('icon_img').src = obj.src;
    hideMenu();
}

function clearContent() {
    if(wysiwyg) {
        editdoc.body.innerHTML = BROWSER.firefox ? '<br />' : '';
    } else {
        textobj.value = '';
    }
}

function uploadNextAttach() {
    var str = $('attachframe').contentWindow.document.body.innerHTML;
    if(str == '') return;
    var arr = str.split('|');
    var att = CURRENTATTACH.split('|');
    uploadAttach(parseInt(att[0]), arr[0] == 'BBXUPLOAD' ? parseInt(arr[1]) : -1, att[1]);
    if (arr[0] == "BBXUPDATE") {
        if (arr[1] == "0") {
            $("attachname" + arr[3]).innerHTML = arr[2];
            $("attach" + arr[3]).style.display = '';
            $("attachupdate" + arr[3]).style.display = 'none';
            $("attach" + arr[3] + "_opt").innerHTML = '更新';
            $("attach" + arr[3] + "_type").src = arr[4] == -1 ? "images/attachicons/attachment.gif" : "images/attachicons/image.gif";
        }
        else {
            showDialog('更新失败:' + STATUSMSG[arr[1]]);
        }
    }
}


function addAttach(prefix, filetype) {
    var tags, newnode, i;
    prefix = isUndefined(prefix) ? '' : prefix;
    var uploadAttachFiles = document.getElementsByName("uploadattchfile");
    for (var index = 0; index < uploadAttachFiles.length; index++) {
        var id = AID;
        var type = uploadAttachFiles[index].attributes["filetype"].nodeValue == "img" ? "img" : "";
        if (!isUndefined(filetype) && filetype != type)
            continue;
        newnode = uploadAttachFiles[index].firstChild.cloneNode(true);
        tags = newnode.getElementsByTagName('input');
        for (i in tags) {
            if (tags[i].name == 'Filedata') {
                tags[i].id = prefix + type + 'attachnew_' + id;
                tags[i].onchange = function () { insertAttach(prefix, this) };
                tags[i].unselectable = 'on';
            } else if (tags[i].name == 'attachid') {
                tags[i].value = id;
            }
        }
        tags = newnode.getElementsByTagName('form');
        tags[0].name = tags[0].id = prefix + type + 'attachform_' + id;
        $(prefix + type + 'attachbtn').appendChild(newnode);
        newnode = $(prefix + type + 'attachbodyhidden').firstChild.cloneNode(true);
        tags = newnode.getElementsByTagName('input');
        for (i in tags) {
            if (tags[i].name == prefix + type + 'localid') {
                tags[i].value = id;
            }
        }
        tags = newnode.getElementsByTagName('span');
        for (i in tags) {
            var elm = tags[i];
            if (!elm) continue;
            if (elm.id == prefix + type + 'localfile[]') {
                elm.id = prefix + type + 'localfile_' + id;
            } else if (elm.id == prefix + type + 'cpdel[]') {
                elm.id = prefix + type + 'cpdel_' + id;
            } else if (elm.id == prefix + type + 'localno[]') {
                elm.id = prefix + type + 'localno_' + id;
            } else if (elm.id == prefix + type + 'deschidden[]') {
                elm.id = prefix + type + 'deschidden_' + id;
            }
        }
        AID++;
        newnode.style.display = 'none';
        $(prefix + type + 'attachbody').appendChild(newnode);
    }
}

function insertAttach(prefix, uploadinput) {
    var localimgpreview = '';
    //var uploadinput = $(prefix + 'attachnew_' + id);
    var filetype = uploadinput.attributes["filetype"].nodeValue == "img" ? "img" : "";
    var id = uploadinput.id.replace(prefix + filetype + 'attachnew_', "");
    var path = uploadinput.value;
    var extpos = path.lastIndexOf('.');
    var ext = extpos == -1 ? '' : path.substr(extpos + 1, path.length).toLowerCase();
    var re = new RegExp("(^|\\s|,)" + ext + "($|\\s|,)", "ig");
    var localfile = uploadinput.value.substr(uploadinput.value.replace(/\\/g, '/').lastIndexOf('/') + 1);
    var filename = mb_cutstr(localfile, 30);

    if(path == '') {
        return;
    }
    var uplaodextensions=new Array();
    var extensionsarray=extensions.split('|')
    for(i=0;i<extensionsarray.length;i++)
    {
        var t=extensionsarray[i].split(',')
        uplaodextensions.push(t[0]);
    }
    //if(extensions != '' && (re.exec(extensions) == null || ext == '')) {
    if(uplaodextensions != '' && (re.exec(uplaodextensions) == null || ext == '')) {
        //reAddAttach(prefix, id);
        showDialog('对不起，不支持上传此类扩展名的附件');
        return;
    }
    if(filetype == 'img' && imgexts.indexOf(ext) == -1) {
        reAddAttach(prefix, id);
        showDialog('请选择图片文件(' + imgexts + ')');
        return;
    }

    $(prefix + filetype + 'cpdel_' + id).innerHTML = '<a href="###" class="d" onclick="reAddAttach(\'' + prefix + '\', ' + id + ',\'' + filetype + '\')" title="删除">删除</a>';
    $(prefix + filetype + 'localfile_' + id).innerHTML = '<span>' + filename + '</span>';
    $(prefix + filetype + 'attachnew_' + id).style.display = 'none';
    $(prefix + filetype + 'deschidden_' + id).style.display = '';
    $(prefix + filetype + 'deschidden_' + id).title = localfile;
    $(prefix + filetype + 'localno_' + id).parentNode.parentNode.style.display = '';
    addAttach(prefix, filetype);
    UPLOADSTATUS = 0;
//	ATTACHNUM['attachunused']++;
//	updateattachnum('attach');
}

function reAddAttach(prefix, id, filetype) {
    filetype = isUndefined(filetype) ? '' : filetype;
    $(prefix + filetype + 'attachbody').removeChild($(prefix + filetype + 'localno_' + id).parentNode.parentNode);
    $(prefix + filetype + 'attachbtn').removeChild($(prefix + filetype + 'attachnew_' + id).parentNode.parentNode);
    $(prefix + filetype + 'attachbody').innerHTML == '' && addAttach(prefix);
    $('localimgpreview_' + id) ? document.body.removeChild($('localimgpreview_' + id)) : null;
}

function delAttach(id, type) {
    //appendAttachDel(id);
    //$('attach_' + id).style.display = 'none';
    ATTACHNUM['attach' + (type ? 'un' : '') + 'used']--;
    updateattachnum('attach');
    data=id.toString().split(",")
    _sendRequest('tools/ajax.ashx?t=deleteattach&aid=' + data[0] + '&fid=' + data[1], delnouseAttach_callback, false);
}

function delnouseAttach_callback(doc)
{
    var data=eval(doc)
    if (data.length != 0 && data[0].aid) {
        appendAttachDel(data[0].aid);
        if ($('attach_' + data[0].aid))
            $('attach_' + data[0].aid).parentNode.removeChild($('attach_' + data[0].aid));
        //updateAttachList();
    }
    else {
        var obj = $("image_td_" + data[0].erraid);
        if (obj) {
            obj.className = "";
            alert("权限不足，不能删除该附件");
        }
    }
}
function delImgAttach(id, type) {
    appendAttachDel(id);
    if ($('image_td_' + id)) {
        $('image_td_' + id).className = 'imgdeleted';
        $('image_' + id).onclick = null;
        $('image_desc_' + id).disabled = true;
    }
    data = id.toString().split(",")
    _sendRequest('tools/ajax.ashx?t=deleteattach&aid=' + data[0] + '&fid=' + data[1], delnouseAttach_callback, false);
    //delAttach(id, type);
    ATTACHNUM['image' + (type ? 'un' : '') + 'used']--;
    updateattachnum('image');
}

function appendAttachDel(id) {
    var input = document.createElement('input');
    input.type = 'hidden';
    input.name = 'attachdel[]';
    input.value = id;
    $('postbox').appendChild(input);
}

function updateAttach(aid) {
    objupdate = $('attachupdate'+aid);
    obj = $('attach' + aid);
    if(!objupdate.innerHTML) {
        obj.style.display = 'none';
        objupdate.innerHTML = '<input type="file" name="attachupdate[paid' + aid + ']"><a href="javascript:;" onclick="updateAttach(' + aid + ')">取消</a>';
    } else {
        obj.style.display = '';
        objupdate.innerHTML = '';
    }
}

function updateattachnum(type) {
    ATTACHNUM[type + 'used'] = ATTACHNUM[type + 'used'] >= 0 ? ATTACHNUM[type + 'used'] : 0;
    ATTACHNUM[type + 'unused'] = ATTACHNUM[type + 'unused'] >= 0 ? ATTACHNUM[type + 'unused'] : 0;
    var num = ATTACHNUM[type + 'used'] + ATTACHNUM[type + 'unused'];
    if(num) {
        if($(editorid + '_' + type)) {
            $(editorid + '_' + type).title = '包含 ' + num + (type == 'image' ? ' 个图片附件' : ' 个附件');
        }
        if($(editorid + '_' + type + 'n')) {
            $(editorid + '_' + type + 'n').style.display = '';
        }
    } else {
        if($(editorid + '_' + type)) {
            $(editorid + '_' + type).title = type == 'image' ? '图片' : '附件';
        }
        if($(editorid + '_' + type + 'n')) {
            $(editorid + '_' + type + 'n').style.display = 'none';
        }
    }
}

function unusedoption(op, aid) {
    switch (op) {
        case 0: //Ignore and delete unused attachments
            if (confirm('您是否要删除选中的附件?')) {
                for (var i = 0; i < $('unusedform').elements.length; i++) {
                    var e = $('unusedform').elements[i];
                    if (e.name.match('unused')) {
                        if (e.checked) {
                            if ($('image_td_' + e.value)) {
                                RemoveImages(e.value);
                                delImgAttach(e.value, 1);

                            }
                            if ($('attach_' + e.value)) {
                                $('attach_' + e.value).parentNode.removeChild($('attach_' + e.value));
                                delAttach(e.value, 1);
                            }
                        }
                    }
                }
                updateimagelistHTML(unusedimagelist);
                switchTab();
            }
            else return false;
            break;
        case 1: //Use selected attachments and Ignore unselect attachments
            for (var i = 0; i < $('unusedform').elements.length; i++) {
                var e = $('unusedform').elements[i];
                if (e.name.match('unused')) {
                    if (!e.checked) {
                        if ($('image_td_' + e.value)) {
                            RemoveImages(e.value);
                            ATTACHNUM['imageunused']--;
                        }
                        if ($('attach_' + e.value)) {
                            $('attach_' + e.value).parentNode.removeChild($('attach_' + e.value));
                            ATTACHNUM['attachunused']--;
                        }
                    }
                }
            }
            updateimagelistHTML(unusedimagelist);
            switchTab();
            break;
//        case 2:
//            delAttach(aid, 1);
//            break;
//        case 3:
//            delImgAttach(aid, 1);
//            break;
    }
    if(op < 2) {
        hideMenu('fwin_dialog', 'dialog');
        updateattachnum('image');
        updateattachnum('attach');
        //if attachlist has attachment item,show attachlist else show swfupload
        if (ATTACHNUM['attachused'] + ATTACHNUM['attachunused'] <= 0)
            switchAttachbutton('swfupload');
        else
            switchAttachbutton('attachlist');
    } else {
        $('unusedrow' + aid).outerHTML = '';
        if(!ATTACHNUM['imageunused'] && !ATTACHNUM['attachunused']) {
            hideMenu('fwin_dialog', 'dialog');
        }
    }
}
function swfHandler(action, type) {
    if(action == 2) {
        if(type == 'image') {
            updateImageList(action);
        } else {
            updateAttachList(action);
        }
    }
}

function getfileextname(filename)
{
   if(filename.indexOf('.')!=-1)
   {
     var arr=filename.split('.');
     return arr[arr.length-1];
   }
}

function getattachlist_callback(doc) {
    var data = eval(doc);
    html = '';
    hiddenattachidlist = '';
    if (data.length > 0) {
        ATTACHNUM['attachunused'] += data.length;
        updateattachnum('attach');
        html += '<table cellpadding="0" cellspacing="0" border="0" width="100%">';
        html += '<tbody><tr><td colspan="6">以下是你上次上传但没有使用的附件:</td></tr></tbody>'
        for (i = 0; i < data.length; i++) {
            linkurl = '';
            isimage = '';
            filetypeimage = '';
            html += '<tr>';
            hiddenattachidlist += data[i].aid + ',';
            fileext = getfileextname(data[i].attachment);

            var isimgbool = data[i].filetype.indexOf('image') != -1;

            if (isimgbool) {
                linkurl = 'insertAttachimgTag(' + data[i].aid + ')';
                isimage = ' isimage="1"';
                filetypeimage = 'image.gif';
            }
            else {
                linkurl = 'insertAttachTag(' + data[i].aid + ')';
                if (fileext == 'rar' || fileext == 'zip')
                    filetypeimage = 'rar.gif';
                else
                    filetypeimage = 'attachment.gif';
            }

            html += '<tbody id="attach_' + data[i].aid + '"><tr><td class="atnu">';
            html += '<img border="0" alt="" class="vm" src="images/attachicons/' + filetypeimage + '">';
            html += '</td><td class="atna">';
            html += '<a id="attachname' + data[i].aid + '" href="javascript:;" onclick="' + linkurl + '" ' + isimage + ' title="' + data[i].attachment + '">' + mb_cutstr(data[i].attachment, 40) + '</a>';
            html += '<span id="attachupdate' + data[i].aid + '"></span>';
            html += '<input type="hidden" name="attachid" value="' + data[i].aid + '">';
            if (isimgbool)
                html += '<img style="position: absolute; top: -10000px;" cwidth="' + data[i].width + '" id="image_' + data[i].aid + '" src="tools/ajax.ashx?t=image&aid=' + data[i].aid + '&size=300x300&key=' + data[i].attachkey + '&nocache=yes&type=fixnone" id="image_' + data[i].aid + '"/>';
            html += '</td>';
            html += '<td class="atds"><input type="text" value="" class="txt" size="18" name="attachdesc_' + data[i].aid + '"></td>';

            html += '<td class="attv"><select id="readperm_' + data[i].aid + '" onchange="$(\'readperm_hidden_' + data[i].aid + '\').value = this.value;" size="1" style="width:90px">';
            html += '<option value="">不限</option>';
            html += '</select><input type="hidden" id="readperm_hidden_' + data[i].aid + '" name="readperm_' + data[i].aid + '" value="0" class="txt"  size="1"></td>';

            html += '<td class="attp"><input type="text" name="attachprice_' + data[i].aid + '" value="0" size="1"></td>';
            html += '<td class="attc delete_msg"><a onclick="delAttach(' + data[i].aid + ',1)" class="d" href="javascript:;" title="删除">删除</a></td>';
            html += '</tr></tbody>';
        }
        html += '</table>';
        $("attachlist_tablist").innerHTML = html;

        //bind attachments readperm list
        for (var j = 0; j < data.length; j++) {
            getreadpermoption($('readperm_' + data[j].aid));
        }
    }
}

function updateAttachListbycount(attachlistcount,issimpleeditor) {
    var url = 'tools/ajax.ashx?t=getattachlist&file=true&posttime=' + encodeURI($("posttime").value);
    var fun = issimpleeditor ? updateSimpleAttachListbycount_callback : updateAttachListbycount_callback;
    _sendRequest(url, fun, false);
    switchAttachbutton('attachlist');
}

function updateSimpleAttachListbycount_callback(doc) {
    var data = eval(doc);
    if (data.length <= 0) {
        switchAttachbutton('upload');
        return;
    }
    var attachlist = $("e_attachlist");
    var html = '<table cellspacing="0" cellpadding="0" border="0" style="" id="attach_tblheader">' +
            '<tbody><tr>' +
            '<td colspan="3">点击附件文件名添加到帖子内容中</td>' +
            '</tr>';
    var hiddenattachidlist = "";
    for (i = 0; i < data.length; i++) {
        var linkurl = '';
        var isimage = ''
        var filetypeimage = '';
        hiddenattachidlist += data[i].aid + ',';
        fileext = getfileextname(data[i].attachment);

        var isimgbool = data[i].filetype.indexOf('image') != -1;
        if (isimgbool) {
            linkurl = 'seditor_insertunit(\'' + seditorkey + '\', \'\\r\\n[attachimg]' + data[i].aid + '[/attachimg]\',\'\', null, 1); return false';
            isimage = ' isimage="1"';
            filetypeimage = 'image.gif';
        }
        else {
            linkurl = 'seditor_insertunit(\'' + seditorkey + '\', \'\\r\\n[attach]' + data[i].aid + '[/attach]\',\'\', null, 1); return false';
            if (fileext == 'rar' || fileext == 'zip')
                filetypeimage = 'rar.gif';
            else
                filetypeimage = 'attachment.gif';
        }
        html += '<tr id="attach_' + data[i].aid + '">';
        html += '<td class="atnu"><img border="0" alt="" class="vm" src="images/attachicons/' + filetypeimage + '"></td>';
        html += '<td><a id="attachname' + data[i].aid + '" href="javascript:;" onclick="' + linkurl + '" ' + isimage + ' title="' + data[i].attachment + '">' + mb_cutstr(data[i].attachment, 35) + '</a>';
        $("hiddenlayer").innerHTML += '<input type="hidden" value="' + data[i].aid + '" name="attachid">';
        html += '</td>';
        html += '<td class="attc delete_msg"><a onclick="delSimpleAttach(' + data[i].aid + ',1)" class="d" href="javascript:;" title="删除">删除</a></td>';
        html += '</tr>';
    }
    html += '</tbody></table>';
    attachlist.innerHTML = html;
    attachlist.style.display = '';
}

function delSimpleAttach(id, type) {
    data = id.toString().split(",")
    _sendRequest('tools/ajax.ashx?t=deleteattach&aid=' + data[0] + '&fid=' + data[1], delnouseSimpleAttach_callback, false);
}

function delnouseSimpleAttach_callback(doc) {
    var data = eval(doc)
    if (data.length != 0 && data[0].aid) {
        if ($('attach_' + data[0].aid))
            $('attach_' + data[0].aid).parentNode.removeChild($('attach_' + data[0].aid));
        //updateAttachList();
    }
    else {
        var obj = $("image_td_" + data[0].erraid);
        if (obj) {
            obj.className = "";
            alert("权限不足，不能删除该附件");
        }
    }
}

function updateAttachListbycount_callback(doc) {
    var data = eval(doc);
    html = '';
    hiddenattachidlist = '';
    var attachidcache = document.getElementsByName('attachid');

    if (data.length > 0) {
        html += '<table cellpadding="0" cellspacing="0" border="0" width="100%">';
        for (i = 0; i < data.length; i++) {
            //如果上次上传但没有使用的附件列表中已经存在该附件条目，则不重复添加.如果该位置的html改变了层级结构，可能会造成第二个判断无法生效
            if ($('attach_' + data[i].aid) != null && $('attach_' + data[i].aid).parentNode.parentNode.id == 'attachlist_tablist')
                continue;
            linkurl = '';
            isimage = ''
            filetypeimage = '';
            html += '<tr>';
            hiddenattachidlist += data[i].aid + ',';
            fileext = getfileextname(data[i].attachment);

            var isimgbool = data[i].filetype.indexOf('image') != -1;

            if (isimgbool) {
                linkurl = 'insertAttachimgTag(' + data[i].aid + ')';
                isimage = ' isimage="1"';
                filetypeimage = 'image.gif';
            }
            else {
                linkurl = 'insertAttachTag(' + data[i].aid + ')';
                if (fileext == 'rar' || fileext == 'zip')
                    filetypeimage = 'rar.gif';
                else
                    filetypeimage = 'attachment.gif';
            }

            html += '<tbody id="attach_' + data[i].aid + '"><tr><td class="atnu">';
            html += '<img border="0" alt="" class="vm" src="images/attachicons/' + filetypeimage + '">';
            html += '</td><td class="atna">';
            html += '<a id="attachname' + data[i].aid + '" href="javascript:;" onclick="' + linkurl + '" ' + isimage + ' title="' + data[i].attachment + '">' + mb_cutstr(data[i].attachment, 40) + '</a>';
            html += '<span id="attachupdate' + data[i].aid + '"></span>';
            html += '<input type="hidden" name="attachid" value="' + data[i].aid + '">';
            if (isimgbool)
                html += '<img style="position: absolute; top: -10000px;" cwidth="' + data[i].width + '" id="image_' + data[i].aid + '" src="tools/ajax.ashx?t=image&aid=' + data[i].aid + '&size=300x300&key=' + data[i].attachkey + '&nocache=yes&type=fixnone"/>';
            html += '</td>';
            html += '<td class="atds"><input type="text" value="" class="txt" size="18" name="attachdesc_' + data[i].aid + '"></td>';

            html += '<td class="attv"><select id="readperm_' + data[i].aid + '" onchange="$(\'readperm_hidden_' + data[i].aid + '\').value = this.value;"  style="width:90px">';
            html += '<option value="">不限</option>';
            html += '</select><input type="hidden" id="readperm_hidden_' + data[i].aid + '" name="readperm_' + data[i].aid + '" value="0" class="txt"  size="1"></td>';

            html += '<td class="attp"><input type="text" name="attachprice_' + data[i].aid + '" value="0" size="1"></td>';
            html += '<td class="attc delete_msg"><a onclick="delAttach(' + data[i].aid + ',1)" class="d" href="javascript:;" title="删除">删除</a></td>';
            html += '</tr></tbody>';

            var isnewattach = 1;
            for (j = 0; j < attachidcache.length; j++) {
                if (data[i].aid == attachidcache[j].value) {
                    isnewattach = 0;
                    break;
                }
            }
            if (isnewattach) {
                ATTACHNUM['attachunused'] ++;
            }
        }
        html += '</table>';
        $('attachlist_tablist_current').innerHTML = html;
        updateattachnum('attach');

        //bind attachments readperm list
        for (var j = 0; j < data.length; j++) {
            getreadpermoption($('readperm_' + data[j].aid));
        }
    }
}

function updateAttachList(action) {
    var url = 'tools/ajax.ashx?t=getattachlist' + (action ? "&posttime=" + encodeURI($("posttime").value) : "") + "&file=true";
    var fun = action ? updateAttachListbycount_callback : getattachlist_callback;
    _sendRequest(url, fun, false);
    switchAttachbutton('attachlist');
    $('attachlist_tablist').style.display = '';
    $('attach_notice').style.display = '';
}

var unusedimagelist; //last upload unused imagelist

function RemoveImages(aid) {
    var jsonstr = '[';
    for (i = 0; i < unusedimagelist.length; i++) {
        if (unusedimagelist[i].aid != aid) {
            if (jsonstr != '[')
                jsonstr += ',';
            jsonstr += '{\'aid\':' + unusedimagelist[i].aid + ',\'attachment\':\'' + unusedimagelist[i].attachment.replace(/'/g, "\\\'") + '\',\'attachkey\':\'' + unusedimagelist[i].attachkey + '\',\'description\':\'\'}';
        }
    }
    jsonstr += ']';
    unusedimagelist = eval(jsonstr);
}

function updateImageList(action) {
    var url = 'tools/ajax.ashx?t=imagelist&pid=' + pid + (action ? '&posttime=' + encodeURI($("posttime").value) : "") + (!fid ? '' : '&fid=' + fid);
    _sendRequest(url, function (responseText) {
        updateimagelist_callback(responseText, action);
    });
    switchImagebutton('imgattachlist');$('imgattach_notice').style.display = '';
}

function updateimagelist_callback(doc, action) {
    if (!action)
        unusedimagelist = eval(doc);
    var data = eval(doc);
    if (data.length > 0)
        updateimagelistHTML(data, action);
}

//action:null=更新上次上传未使用的图片列表；2：更新本次上传的图片列表；3：更新帖子中已使用的图片列表
function updateimagelistHTML(data, action) {
    var createHiddenInput = function (name, value) {
        var hiddenInput = document.createElement("input");
        hiddenInput.type = "hidden";
        hiddenInput.name = name;
        hiddenInput.value = value;
        return hiddenInput;
    }
    if (data == null || data.length == 0) {
        var unusedimgattachlist = $("unusedimgattachlist");
        unusedimgattachlist.parentNode.removeChild(unusedimgattachlist);
        return;
    }

    var attachidcache = document.getElementsByName('attachid');
    var html = '';
    if (!action)
        html += '<p>以下是你上次上传但没有使用的附件:</p>';
    if (action == 3)
        html += '<table id="uploadattachlist" cellspacing="2" cellpadding="2" class="imgl">';
    else
        html += '<table cellspacing="2" cellpadding="2" class="imgl">';
    for (var item = 0; item < data.length; item++) {
        var description = data[item].description;
        if ((item + 1) % 4 == 1)
            html += '<tr>';
        html += '<td valign="bottom" id="image_td_' + data[item].aid + '" width="25%">';

        var postForm = $("postform");
        postForm.appendChild(createHiddenInput("attachid", data[item].aid));
        postForm.appendChild(createHiddenInput("attachprice_" + data[item].aid, 0));
        postForm.appendChild(createHiddenInput("readperm_" + data[item].aid, 0));
        html += '<a href="javascript:;" title="' + data[item].attachment + '">';
        html += '<img src="tools/ajax.ashx?t=image&aid=' + data[item].aid + '&size=300x300&key=' + data[item].attachkey + '&nocache=yes&type=fixnone" id="image_' + data[item].aid + '" onclick="insertAttachimgTag(\'' + data[item].aid + '\')" width="110" cwidth="300" /></a>';
        html += '<p class="imgf"><a href="javascript:;" onclick="delImgAttach(' + data[item].aid + ',' + (action == 3 ? '0' : '1') + ')" class="del y">删除</a>';
        html += '<input type="text" class="px xg2" value="' + (description == '' ? '描述' : description) + '" onclick="this.style.display=\'none\';$(\'image_desc_' + data[item].aid + '\').style.display=\'\';$(\'image_desc_' + data[item].aid + '\').focus();" />';
        html += '<input type="text" name="attachdesc_' + data[item].aid + '" class="px" style="display: none;" id="image_desc_' + data[item].aid + '" value="' + description + '" />';
        html += '</p></td>';
        if ((item + 1) % 4 == 0)
            html += '</tr>';

        if (action == 2) {
            var isnewattach = 1;
            for (j = 0; j < attachidcache.length; j++) {
                if (data[item].aid == attachidcache[j].value) {
                    isnewattach = 0;
                    break;
                }
            }
            if (isnewattach) {
                ATTACHNUM['imageunused']++;
            }
        }
    }
    if (data.length % 4 != 0) {
        for (i = 0; i < 4 - data.length % 4; i++)
            html += '<td width=\"25%\"></td>';
        html += '</tr>';
    }
    html += '</table>';

    switch (action) {
        case 2:
            $('imgattachlist').innerHTML = html;
            break;
        case 3:
            $('usedimgattachlist').innerHTML = html;
            $('imgattachlist').innerHTML = '';
            break;
        default:
            $('unusedimgattachlist').innerHTML = html;
            ATTACHNUM['imageunused'] = data.length;
            break;
    }
    updateattachnum('image');
}

function switchButton(btn, btns) {
    if(!$(editorid + '_btn_' + btn) || !$(editorid + '_' + btn)) {
        return;
    }
    $(editorid + '_btn_' + btn).style.display = '';
    $(editorid + '_' + btn).style.display = '';
    $(editorid + '_btn_' + btn).className = 'current';
    for(i = 0;i < btns.length;i++) {
        if(btns[i] != btn) {
            if(!$(editorid + '_' + btns[i]) || !$(editorid + '_btn_' + btns[i])) {
                continue;
            }
            $(editorid + '_' + btns[i]).style.display = 'none';
            $(editorid + '_btn_' + btns[i]).className = '';
        }
    }
}

function uploadWindowstart() {
    $('uploading').style.visibility = 'visible';
}

function uploadWindowload() {
    $('uploading').style.visibility = 'hidden';
    var str = $('uploadattachframe').contentWindow.document.body.innerHTML;
    if(str == '') return;
    var arr = str.split('|');
    if(arr[0] == 'BBXUPLOAD' && arr[1] == 0) {
        updateAttachListbycount(1,true);
    } else {
        showDialog('上传失败:' + STATUSMSG[arr[1]]);
    }
}

function uploadWindow(recall, type) {
    var type = isUndefined(type) ? 'image' : type;
    UPLOADWINRECALL = recall;
    showWindow('upload', 'forum.php?mod=misc&action=upload&fid=' + fid + '&type=' + type, 'get', 0, {'cover':1});
}

function updatetradeattach(aid, url, attachurl) {
    $('tradeaid').value = aid;
    $('tradeattach_image').innerHTML = '<img src="' + attachurl + '/' + url + '" class="spimg" />';
}

function updateactivityattach(aid, url, attachurl) {
    $('activityaid').value = aid;
    $('activityattach_image').innerHTML = '<img src="' + attachurl + '/' + url + '" class="spimg" />';
}

function updatesortattach(aid, url, attachurl) {
    $('sortaid').value = aid;
    $('sortattachurl').value = attachurl + '/' + url;
    $('sortattach_image').innerHTML = '<img src="' + attachurl + '/' + url + '" class="spimg" />';
}

function switchpollm(swt) {
    t = $('pollchecked').checked && swt ? 2 : 1;
    var v = '';
    for(var i = 0; i < $('postform').elements.length; i++) {
        var e = $('postform').elements[i];
        if(e.name.match('^polloption')) {
            if(t == 2 && e.tagName == 'INPUT') {
                v += e.value + '\n';
            } else if(t == 1 && e.tagName == 'TEXTAREA') {
                v += e.value;
            }
        }
    }
    if(t == 1) {
        var a = v.split('\n');
        var pcount = 0;
        for(var i = 0; i < $('postform').elements.length; i++) {
            var e = $('postform').elements[i];
            if(e.name.match('^polloption')) {
                pcount++;
                if(e.tagName == 'INPUT') e.value = '';
            }
        }
        for(var i = 0; i < a.length - pcount + 2; i++) {
            addpolloption();
        }
        var ii = 0;
        for(var i = 0; i < $('postform').elements.length; i++) {
            var e = $('postform').elements[i];
            if(e.name.match('^polloption') && e.tagName == 'INPUT' && a[ii]) {
                e.value = a[ii++];
            }
        }
    } else if(t == 2) {
        $('postform').polloptions.value = trim(v);

    }
    $('postform').tpolloption.value = t;
    if(swt) {
        display('pollm_c_1');
        display('pollm_c_2');
    }
}

function loadimgsize(imgurl) {
    var s = new Object();
    s.img = new Image();
    s.img.src = imgurl;
    s.loadCheck = function () {
        if(s.img.complete) {
            $(editorid + '_image_submit').disabled = false;
            $(editorid + '_image_param_2').value = s.img.width ? s.img.width : '';
            $(editorid + '_image_param_3').value = s.img.height ? s.img.height : '';
            $(editorid + '_image_status').innerHTML = '';
        } else {
            $(editorid + '_image_submit').disabled = true;
            $(editorid + '_image_status').innerHTML = ' 验证图片中...';
            setTimeout(function () { s.loadCheck(); }, 100);
        }
    };
    s.loadCheck();
}

function addpolloption() {
    if(curoptions < maxoptions) {
        $('polloption_new').outerHTML = '<p>' + $('polloption_hidden').innerHTML + '</p>' + $('polloption_new').outerHTML;
        curoptions++;
    }
}

function delpolloption(obj) {
    obj.parentNode.parentNode.removeChild(obj.parentNode);
    curoptions--;
}

function showsmiles1(index, typename, pageindex, seditorKey) {
    $("s_" + index).className = "current";
    var cIndex = 1;
    for (i in smilies_HASH) {
        if (cIndex != index) {
            $("s_" + cIndex).className = "";
        }
        $("s_" + cIndex).style.display = "";
        cIndex++;
    }

    var pagesize = (typeof smiliesCount) == 'undefined' ? 12 : smiliesCount;
    var url = (typeof forumurl) == 'undefined' ? '' : forumurl;
    var s = smilies_HASH[typename];
    var pagecount = Math.ceil(s.length / pagesize);
    var inseditor = typeof seditorKey != 'undefined';

    if (isUndefined(pageindex)) {
        pageindex = 1;
    }

    if (pageindex > pagecount) {
        pageindex = pagecount;
    }

    var maxIndex = pageindex * pagesize;
    if (maxIndex > s.length) {
        maxIndex = s.length;
    }

    maxIndex = maxIndex - 1;
    var minIndex = (pageindex - 1) * pagesize;
    var html = '<table id="' + index + '_table" cellpadding="0" cellspacing="0" style="clear: both"><tr>';
    var ci = 1;
    for (var id = minIndex; id <= maxIndex; id++) {
        var clickevt = 'insertSmiley(\'' + addslashes(s[id]['code']) + '\');';
        if (inseditor) {
            clickevt = 'seditor_insertunit(\'' + seditorKey + '\', \'' + s[id]['code'] + '\');';
        }
        html += '<td valign="middle"><img style="cursor: pointer;" src="' + url + 'editor/images/smilies/' + s[id]['url'] + '" id="smilie_' + s[id]['code'] + '" alt="' + s[id]['code'] + '" onclick="' + clickevt + '" onmouseover="smilies_preview(\'s\', this, 40)" onmouseout="smilies_preview(\'s\')" title="" border="0" height="20" width="20" /></td>';
        if (ci % colCount == 0) {
            html += '</tr><tr>'
        }
        ci++;
    }
    html += '<td colspan="' + (colCount - ((ci - 1) % colCount)) + '"></td>';
    html += '</tr>';
    html += '</table>';
    $("showsmilie").innerHTML = html;
    if (pagecount > 1) {
        html = '<div class="p_bar">';
        for (var i = 1; i <= pagecount; i++) {
            if (i == pageindex) {
                html += "<a class=\"p_curpage\">" + i + "</a>";
            }
            else {
                html += "<a class=\"p_num\" href='#smiliyanchor' onclick=\"showsmiles1(" + index + ", '" + typename + "', " + i + ")\">" + i + "</a>"
            }
        }
        html += '</div>'
        $("showsmilie_pagenum").innerHTML = html;
    }
    else {
        $("showsmilie_pagenum").innerHTML = "";
    }
}


function switchAdvanceMode(url, seditorid) {
    var message = jQuery("#message").val();
    if (message != '') {
        var iframe = document.createElement('IFRAME');
        url += (url.indexOf('?') != -1 ? '&' : '?') + 'cedit=yes';

        if (iframe.attachEvent) {
            iframe.attachEvent('onload', function () { location.href = url; });
        } else {
            iframe.onload = function () { location.href = url; };
        }
        iframe.src = forumpath + 'userdatahub.aspx?formname=' + seditorid + 'form';
        iframe.style.display = 'none';
        $(seditorid + 'form').appendChild(iframe);
    }
    else {
        location.href = url;
    }
    return false;
}

function insertAllAttachTag() {
    var attachListObj = $('e_attachlist').getElementsByTagName("tbody");
    for (var i in attachListObj) {
        if (typeof attachListObj[i] == "object") {
            var attach = attachListObj[i];
            var ids = attach.id.split('_');
            if (ids[0] == 'attach') {
                if ($('attachname' + ids[1])) {
                    if (parseInt($('attachname' + ids[1]).getAttribute('isimage'))) {
                        insertAttachimgTag(ids[1]);
                    } else {
                        insertAttachTag(ids[1]);
                    }
                }
            }
        }
    }
    doane();
}

function getreadpermoption(obj, selectvalue) {
    if (!topicreadperm)
        return;
    //var readpermoption = $('topicreadpermclone').options;
    obj.options.length = 0;
    for (var i = 0; i < topicreadperm.length; i++) {
        var optionItem = new Option(topicreadperm[i].grouptitle, topicreadperm[i].readaccess);
        optionItem.title = "阅读权限:" + topicreadperm[i].readaccess;
        obj.options.add(optionItem);
    }
    if (selectvalue && selectvalue > 0)
        obj.value = selectvalue;
}

function switchTab() {
    var tds = $('unusedimgattachlist') ? $('unusedimgattachlist').getElementsByTagName("td") : 0;
    var toggle = true;
    for (var i = 0; i < tds.length; i++) {
        if (tds[i].innerHTML && tds[i].className != 'imgdeleted') {
            toggle = false;
            break;
        }
    }
    if (toggle && !$('usedimgattachlist').childNodes.length)
        switchImagebutton('multi');
    else
        switchImagebutton('imgattachlist');
}

function userdataoption(op, control) {
    if (!op) {
        saveUserdata('forum', '');
    } else {
        loadData(true, control);
        checkFocus();
    }
    display('rstnotice');
    doane();
}