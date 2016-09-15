var attachments = new Array();
var attachimgurl = new Array();
function delAttach(id) {
	$('attachbody').removeChild($('localno_' + id).parentNode.parentNode);
	$('attachbtn').removeChild($('attachnew_' + id).parentNode);
	$('attachbody').innerHTML == '' && addAttach();
	$('localimgpreview_' + id) ? document.body.removeChild($('localimgpreview_' + id)) : null;

	countNum2Upload();

	if ($('localimgpreview_' + id + '_menu') )
		{document.body.removeChild($('localimgpreview_' + id + '_menu'));}
}

function delSWFAttach(id) {
	$('swfattach_' + id).style.display = 'none';
	$('delswfattach_' + id).checked = true;
}

function delEditAttach(id) {
	$('attach_' + id).style.display = 'none';
	$('delattach_' + id).checked = true;
}

/* function addAttach() {
    if ($('attachbody').childNodes.length + ($('attachuploaded') ? $('attachuploaded').childNodes.length : 0) > maxattachments - 1){
		countNum2Upload();
		//num2upload++;
		return;
	}
	var id = aid;
	var tags, newnode, i;

	newnode = $('attachbtnhidden').firstChild.cloneNode(true);
	tags = newnode.getElementsByTagName('input');
	for(i in tags) {
		if(tags[i].name == 'postfile') {
			tags[i].id = 'attachnew_' + id;
			tags[i].onchange = function() {insertAttach(id)};
			tags[i].unselectable = 'on';
		}
	}
	$('attachbtn').appendChild(newnode);

    newnode = $('attachbodyhidden').firstChild.cloneNode(true);
	tags = newnode.getElementsByTagName('input');
	for(i in tags) {
		if(tags[i].name == 'localid') {
			tags[i].value = id;
		}
	}
	tags = newnode.getElementsByTagName('span');
	for(i in tags) {
		if(tags[i].id == 'localfile[]') {
			tags[i].id = 'localfile_' + id;
		} else if(tags[i].id == 'cpadd[]') {
			tags[i].id = 'cpadd_' + id;
		} else if(tags[i].id == 'cpdel[]') {
			tags[i].id = 'cpdel_' + id;
		} else if(tags[i].id == 'localno[]') {
			tags[i].id = 'localno_' + id;
		} else if(tags[i].id == 'deschidden[]') {
			tags[i].id = 'deschidden_' + id;
		}
	}
	aid++;
	newnode.style.display = 'none';
	$('attachbody').appendChild(newnode);
	$('uploadlist').scrollTop = 10000;

		if (caninsertalbum) {
		tags = findtags(newnode, 'select');
		for(i in tags) {
			if(tags[i].name == 'albums') {
				tags[i].id = 'albums' + id;
				$('albums' + id).style.display='';
			}
		}
	}

	countNum2Upload();
} */


function insertAttach(id) {
	$('attachinfo').style.display='none';
	var localimgpreview = '';
	var path = $('attachnew_' + id).value;
	var extpos = path.lastIndexOf('.');
	var ext = extpos == -1 ? '' : path.substr(extpos + 1, path.length).toLowerCase();
	var re = new RegExp("(^|\\s|,)" + ext + "($|\\s|,)", "ig");
	var localfile = $('attachnew_' + id).value.substr($('attachnew_' + id).value.replace(/\\/g, '/').lastIndexOf('/') + 1);
	var filename = mb_cutstr(localfile, 30);

	if(path == '') {
		return;
	}
	if(extensions != '' && (re.exec(extensions) == null || ext == '')) {
		alert('对不起，不支持上传此类扩展名的附件。');
		return;
	}
	attachexts[id] = is_ie && in_array(ext, ['gif', 'jpeg', 'jpg', 'png', 'bmp']) ? 2 : 1;

	if(attachexts[id] == 2) {
		previewfile = '';
		$('img_hidden').alt = id;
		$('img_hidden').filters.item("DXImageTransform.Microsoft.AlphaImageLoader").sizingMethod = 'image';
		try {
			$('attachnew_' + id).select();
			previewfile = document.selection.createRange().text;
			$('img_hidden').filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = previewfile;
		} catch (e) {
			alert('无效的图片文件。');
			delAttach(id);
			return;
		}
		if(previewfile) {
			var wh = {'w' : $('img_hidden').offsetWidth, 'h' : $('img_hidden').offsetHeight};
			var aid = $('img_hidden').alt;
			if(wh['w'] >= 180 || wh['h'] >= 150) {
				wh = attachthumbImg(wh['w'], wh['h'], 180, 150);
			}
			attachwh[id] = wh;
			$('img_hidden').style.width = wh['w'];
			$('img_hidden').style.height = wh['h'];
			$('img_hidden').filters.item("DXImageTransform.Microsoft.AlphaImageLoader").sizingMethod = 'scale';
			div = document.createElement('div');
			div.id = 'localimgpreview_' + id;
			div.style.display = 'none';
			document.body.appendChild(div);
			div.innerHTML = '<img style="filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=\'scale\',src=\''+previewfile+'\');width:'+wh['w']+';height:'+wh['h']+'" src=\'images/common/none.gif\' border="0" aid="attach_'+ aid +'" alt="" />';
		}
	}

	$('cpdel_' + id).innerHTML = '<a href="###" class="deloption" onclick="delAttach(' + id + ')">删除</a>';
	$('cpadd_' + id).innerHTML = '<a href="###" title="点击这里将本附件插入帖子内容中当前光标的位置"' + 'onclick="insertAttachtext(' + id + ');return false;">插入</a>';
	$('localfile_' + id).innerHTML = '<span' + (attachexts[id] == 2 ? ' onmouseover="showpreview(this, \'localimgpreview_' + id + '\')" ' : '') + '>' + filename + '</span>';
	$('attachnew_' + id).style.display = 'none';
	$('deschidden_' + id).style.display = '';
	$('deschidden_' + id).title = localfile;
	$('localno_' + id).parentNode.parentNode.style.display = '';
	addAttach();
	attachlist('open');
}
function attachlist(op) {
	if(!op) {
		op = textobj.className == 'autosave' ? 'close' : 'open';
	}
	if(op == 'open') {
		textobj.className = 'autosave';
		if(editbox) {
			editbox.className = 'autosave';
		}
		$('attachlist').style.display = '';
		if(Editorwin) {
			if(wysiwyg) {
				$('e_iframe').style.height = (parseInt($('floatwin_' + editoraction).style.height) - 329)+ 'px';
			}
			$('e_textarea').style.height = (parseInt($('floatwin_' + editoraction).style.height) - 329)+ 'px';
		}
	} else {
		textobj.className = 'autosave max';
		if(editbox) {
			editbox.className = 'autosave max';
		}
		$('attachlist').style.display = 'none';
		if(Editorwin) {
			if(wysiwyg) {
				$('e_iframe').style.height = (parseInt($('floatwin_' + editoraction).style.height) - 150)+ 'px';
			}
			$('e_textarea').style.height = (parseInt($('floatwin_' + editoraction).style.height) - 150)+ 'px';
		}
	}
}

lastshowpreview = null;
function showpreview(ctrlobj, showid) {
	if(lastshowpreview) {
		lastshowpreview.id = '';
	}
	if(!ctrlobj.onmouseout) {
		 ctrlobj.onmouseout = function() { hideMenu(); }
	}
	ctrlobj.id = 'imgpreview';
	lastshowpreview = ctrlobj;
	$('imgpreview_menu').innerHTML = '<table width="100%" height="100%"><tr><td align="center" valign="middle">' + $(showid).innerHTML + '</td></tr></table>';
	InFloat='floatlayout_' + editoraction;
	showMenu('imgpreview', false, 2, 1, 0);
	$('imgpreview_menu').style.top = (parseInt($('imgpreview_menu').style.top) - $('uploadlist').scrollTop) + 'px';
}

function attachpreview(obj, preview, width, height) {
	if(is_ie) {
		$(preview + '_hidden').filters.item("DXImageTransform.Microsoft.AlphaImageLoader").sizingMethod = 'image';
		try {
			$(preview + '_hidden').filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = obj.value;
		} catch (e) {
			alert('无效的图片文件。');
			return;
		}
		var wh = {'w' : $(preview + '_hidden').offsetWidth, 'h' : $(preview + '_hidden').offsetHeight};
		var aid = $(preview + '_hidden').alt;
		if(wh['w'] >= width || wh['h'] >= height) {
			wh = attachthumbImg(wh['w'], wh['h'], width, height);
		}
		$(preview + '_hidden').style.width = wh['w']
		$(preview + '_hidden').style.height = wh['h'];
		$(preview + '_hidden').filters.item("DXImageTransform.Microsoft.AlphaImageLoader").sizingMethod = 'scale';
		$(preview).style.width = 'auto';
		$(preview).innerHTML = '<img style="filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=\'scale\',src=\'' + obj.value+'\');width:'+wh['w']+';height:'+wh['h']+'" src=\'images/common/none.gif\' border="0" alt="" />';
	}
}

function insertAttachtext(id, iserr) {
    if(!attachexts[id]) {
        return;
    }
    if(attachexts[id] == 2) {
        bbinsert && wysiwyg && iserr == false ? insertText($('localimgpreview_' + id + '_menu').innerHTML, false) : AddText('[localimg=' + attachwh[id]['w'] + ',' + attachwh[id]['h'] + ']' + id + '[/localimg]\r\n\r\n');
    } else {
        bbinsert && wysiwyg ? insertText('[local]' + id + '[/local]', false) : AddText('[local]' + id + '[/local]');
    }
}

function attachthumbImg(w, h, twidth, theight) {
	twidth = !twidth ? thumbwidth : twidth;
	theight = !theight ? thumbheight : theight;
	var x_ratio = twidth / w;
	var y_ratio = theight / h;
	var wh = new Array();
	if((x_ratio * h) < theight) {
		wh['h'] = Math.ceil(x_ratio * h);
		wh['w'] = twidth;
	} else {
		wh['w'] = Math.ceil(y_ratio * w);
		wh['h'] = theight;
	}
	return wh;
}

function attachupdate(aid, ctrlobj) {
	objupdate = $('attachupdate'+aid);
	obj = $('attach'+aid);
	if (!obj.style.display) {
	    obj.style.display = 'none';
	    objupdate.style.display = '';
		ctrlobj.innerHTML = '取消';
	} else {
		obj.style.display = '';
		objupdate.style.display = 'none';
		ctrlobj.innerHTML = '更新';
	}
}

function insertAttachTag(aid) {
	if(wysiwyg) {
		insertText('\r\n[attach]' + aid + '[/attach]', false);
	} else {
		AddText('\r\n[attach]' + aid + '[/attach]');
	}
}

function thumbImg(w, h) {
    var x_ratio = thumbwidth / w;
    var y_ratio = thumbheight / h;
    var wh = new Array();

    if((x_ratio * h) < thumbheight) {
        wh['h'] = Math.ceil(x_ratio * h);
        wh['w'] = thumbwidth;
    } else {
        wh['w'] = Math.ceil(y_ratio * w);
        wh['h'] = thumbheight;
    }
    return wh;
}

function addAttachUploaded(attaches) {
	var num = 1;
	for (i in attaches){
		if (num  > num2upload)
			break;
		try {
			addSingleAttachUploaded(attaches[i]);
			num++;
		} catch (e) { }
	}
	$('attachuploadednote').style.display = 'none';
	if (attaches.length >= num2upload && num2upload > 0)
		delAttach(aid - 1) ;
	countNum2Upload();
	

	switchAttachbutton('attachlist');
	updateAttachList();
}

function countNum2Upload(){
	var empty = ($('attach_' + (aid-1)) && $('attach_' + (aid-1)).value=='') ? 1 : 0
	//var locallength = ($('attachbody').childNodes.length) - empty;
	var locallength = ($('attachbody').childNodes.length-1) - empty;
	var uploadedlength = $('attachuploaded')!=null ? $('attachuploaded').childNodes.length : 0;
	num2upload = maxattachments - locallength - uploadedlength;
	if (num2upload == 0 && $('batchupload'))
		$('batchupload').style.display = 'none';
    if (num2upload > 0 && $('batchupload'))
		$('batchupload').style.display = '';
	if (num2upload < 0)
	    num2upload = 0;
	
	$('num2upload').innerHTML = num2upload;
	if ($('num2upload2'))
	    $('num2upload2').innerHTML = num2upload;

	if (attaches.length > num2upload)
		$("maxattachnote").style.display="";
}

function addSingleAttachUploaded(attach){
	var newnode = $('attachuploadedhidden').firstChild.cloneNode(true);
    var id = attach.aid;
	var path = attach.attachment;
    var ext = path.lastIndexOf('.') == -1 ? '' : path.substr(path.lastIndexOf('.') + 1, path.length).toLowerCase();
	var isimg = in_array(ext, ['gif', 'jpg', 'jpeg', 'png', 'bmp']) ? 2 : 1;

    var tags;
    tags = findtags(newnode, 'input');
    for(i in tags) {
        if(tags[i].name == 'attachid') {
            tags[i].value = id;
        }
    }
    tags = findtags(newnode, 'span');
    for(i in tags) {
        if(tags[i].id == 'attachfile[]') {
            tags[i].id = 'attachfile_' + id;
		} else if(tags[i].id == 'sl_cpadd[]') {
			tags[i].id = 'sl_cpadd_' + id;
		} else if(tags[i].id == 'sl_cpdel[]') {
			tags[i].id = 'sl_cpdel_' + id;
		} else if(tags[i].id == 'sl_localno[]') {
			tags[i].id = 'sl_localno_' + id;
		} else if(tags[i].id == 'sl_deschidden[]') {
			tags[i].id = 'sl_deschidden_' + id;
		}
    }
	if (caninsertalbum) {
		tags = findtags(newnode, 'select');
		for(i in tags) {
			if(tags[i].name == 'albums') {
				tags[i].id = 'albums' + id;
			}
		}
	}
    auid++;
    $('attachuploaded').appendChild(newnode);
	$('uploadlist').scrollTop = 10000;

	var err = false;

	$('sl_cpdel_' + id).innerHTML = '<a href="###" class="deloption" onclick="delUploadedAttach(' + id + ')">删除</a>';
	$('sl_cpadd_' + id).innerHTML = '<a href="###" title=",当前光标的位置"' + 'onclick="insertUploadedAttach' + (isimg == 2 ? 'img' : '') + 'Tag(' + id + ');return false;">插入</a>';
	$('attachfile_' + id).innerHTML = isimg == 2 ? '<span id="attachimgpreview_' + id + '" onmouseover="showpreview(this, \'attachimgpreview_' + id + '\');"> <span class="smalltxt">[' + id + ']</span> <a href="###attachment" onclick="insertUploadedAttachimgTag(' + id + ');return false;">' + attach.attachment + '</a></span>' 
	+ '<div class="popupmenu_popup" id="attachimgpreview_' + id + '" style="display: none;"><img style="max-width: 180px" id="attachimgpreview_' + id + '_image" src="'+rooturl+'attachment.aspx?attachmentid=' + id + '" onerror="this.onerror=null;this.src=\''+rooturl+'attachment.aspx?attachmentid=' + id + '\';" /></div>'	: '<span>' + filename + '</span>';
	$('sl_deschidden_' + id).style.display = '';
	$('sl_deschidden_' + id).title = attach.attachment;
	$('sl_localno_' + id).parentNode.parentNode.style.display = '';



}

function insertUploadedAttachTag(aid) {
	if (bbinsert && wysiwyg) {
		insertText('[attach]' + aid + '[/attach]', false);
	} else {
		AddText('[attach]' + aid + '[/attach]');
	}
}
function insertUploadedAttachimgTag(aid) {
	if (bbinsert && wysiwyg) {
		var attachimgurl = 'attachment.aspx?attachmentid=' + aid;
		insertText('<img src="' + attachimgurl + '" border="0" aid="attachimg_' + aid + '" alt="" /><br /><br />', false);
	} else {
		AddText('[attachimg]' + aid + '[/attachimg]\r\n\r\n');
	}
}

function delUploadedAttach(id) {
    $('attachuploaded').removeChild($('sl_cpdel_' + id).parentNode.parentNode);
	countNum2Upload();
    if (num2upload > 0 && (!$("attach_" + (aid-1)) || $("attach_" + (aid-1)).value != ""))
	    addAttach();
	if ($('attachimgpreview_' + id + '_menu') )
		document.body.removeChild($('attachimgpreview_' + id + '_menu'));
}

//获取silverlight插件已经上传的附件列表  //sl上传完返回
function getAttachmentList(sender, args) {
     var attachment = args.AttchmentList;
     if (isUndefined(attachment) || attachment == '[]') {
		 if (infloat == 1) {
				pagescrolls('swfreturn');return false;
		 }
		 else{swfuploadwin();return;}

       
     }
     var attachmentList = eval("(" + attachment + ")");

     addAttachUploaded(attachmentList);     
        if (infloat == 1) {
			pagescrolls('swfreturn');return false;
        }
		else
		{swfuploadwin();}
 }

function onLoad(plugin, userContext, sender) {
     //只读属性,标识 Silverlight 插件是否已经加载。
     //if (sender.getHost().IsLoaded) {
         $("MultiUploadFile").content.JavaScriptObject.UploadAttchmentList = getAttachmentList;         
    // }
 }
// function initneweditor() {
//     var editorform = $('testform');
//     var editorsubmit = $('testsubmit');
//     if (wysiwyg) {
//         newEditor(1, bbcode2html(textobj.value));
//     } else {
//         newEditor(0, textobj.value);
//     }
//     if (getQueryString('cedit') == 'yes') {
//         loadData(true);
//     }
// }