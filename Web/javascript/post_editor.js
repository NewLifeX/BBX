function floatalert(s) {
	if(!infloat) {
		alert(s);
	} else {
		$('returnmessage').className = 'onerror';
		$('returnmessage').innerHTML = s;
		messagehandle();
	}
}
function validate(theform, previewpost) {
    if (typeof (titleEditor) != 'undefined' && titleEditor != null) {
        var htmltitle = titleEditor.GetHTML();
        if (htmltitle != '') {
            $("title").value = htmltitle.replace(/<[^>]*>/ig, '').toString().substr(0, 60);
        }
    }
    //switchEditor(0);
    //switchEditor(1);
    //switchEditor(0);

    var originalmessage = $("e_textarea").value
    var message = bbinsert && wysiwyg ? html2bbcode(getEditorContents()) : (!theform.parseurloff.checked ? parseurl(originalmessage) : originalmessage);
    if (($('postsubmit').name != 'replysubmit' && !($('postsubmit').name == 'editsubmit' && !isfirstpost) && $("title").value == "") || message.replace(unescape("%0A"), "") == "") {
        floatalert(lang['post_title_and_message_isnull']);
        if (special != 2) {
            try { $("title").focus(); } catch (e) { };
        }
        return false;
    } else if ($("title").value.length > 60) {
        floatalert(lang['post_title_toolong']);
        try { $("title").focus(); } catch (e) { };
        return false;
    }


    if (in_array($('postsubmit').name, ['topicsubmit', 'editsubmit'])) {
        if ((theform.postbytopictype) && (theform.typeid) && (theform.postbytopictype.value == "1") && (theform.typeid.value == "0")) {
            floatalert(lang['post_type_isnull']);
            //theform.typeid.focus();
            return false;
        }
        if ($("createpoll") || $("updatepoll")) {
            var pollcount = 0;
            var polls = document.getElementsByName("pollitemid");
            var polloptionids = document.getElementsByName("optionid");
            var displayorders = document.getElementsByName("displayorder");

            for (var i = 0; i < polls.length; i++) {
                if (polls[i].value != "") {
                    pollcount++;
                    if ($("PollItemname").value == "") {
                        $("PollItemname").value = polls[i].value;
                        if ($("updatepoll")) {
                            $("PollOptionID").value = (polloptionids[i].value == '' ? '0' : polloptionids[i].value);
                            $("PollOptionDisplayOrder").value = displayorders[i].value;
                        }
                    }
                    else {
                        $("PollItemname").value = $("PollItemname").value + "\r\n" + polls[i].value;

                        if ($("updatepoll")) {
                            $("PollOptionID").value = $("PollOptionID").value + "\r\n" + (polloptionids[i].value == '' ? '0' : polloptionids[i].value);
                            $("PollOptionDisplayOrder").value = $("PollOptionDisplayOrder").value + "\r\n" + displayorders[i].value;
                        }
                    }
                }
            }

            if (pollcount < 2) {
                floatalert("投票项不得少于2个");
                $("PollItemname").value = "";
                return false;
            }

            if (pollcount > maxpolloptions) {
                floatalert("系统设置为投票项不得多于" + maxpolloptions + "个");
                $("PollItemname").value = "";
                return false;
            }
        }

        /*
        if(theform.typeid && (theform.typeid.options && theform.typeid.options[theform.typeid.selectedIndex].value == 0) && typerequired) {
        alert(lang['post_type_isnull']);
        theform.typeid.focus();
        return false;
        }
        if(special == 2 && !tradefirst) {
        if(theform.item_name.value == "") {
        alert(lang['post_trade_goodsname_null']);
        theform.item_name.focus();
        return false;
        } else if(theform.item_price.value == "") {
        alert(lang['post_trade_price_null']);
        theform.item_price.focus();
        return false;
        }
        } else if(special == 3 && isfirstpost) {
        if(theform.rewardprice.value == "") {
        alert(lang['post_reward_credits_null']);
        theform.rewardprice.focus();
        return false;
        }
        } else if(special == 4 && isfirstpost) {
        if(theform.activityclass.value == "") {
        alert(lang['post_activity_sort_null']);
        theform.activityclass.focus();
        return false;
        } else if($('starttimefrom_0').value == "" && $('starttimefrom_1').value == "") {
        alert(lang['post_activity_fromtime_null']);
        return false;
        } else if(theform.activityplace.value == "") {
        alert(lang['post_activity_addr_null']);
        theform.activityplace.focus();
        return false;
        }
        }
        */
    }

    if (!disablepostctrl && ((postminchars != 0 && mb_strlen(message) < postminchars) || (postmaxchars != 0 && mb_strlen(message) > postmaxchars))) {
        floatalert(lang['post_message_length_invalid'] + '\n\n' + lang['post_curlength'] + ': ' + mb_strlen(message) + ' ' + lang['bytes'] + '\n' + lang['board_allowed'] + ': ' + postminchars + ' ' + lang['lento'] + ' ' + postmaxchars + ' ' + lang['bytes']);
        $("PollItemname").value = "";
        return false;
    }
    try {
        $("e_textarea").value = message;
    } catch (e) {
        $("e_textarea").value = message;
    }

    //商品信息检查
    if ($('amount') != null) {
        if (!validategoods()) {//该方法的具体实现详见javascript目录下的template_postgoods.js或template_eidtgoods.js文件
            return false;
        }
    }
    /*else{
    if(in_array($('postsubmit').name, ['topicsubmit', 'replysubmit'])){
    //seccheck(theform, seccodecheck, secqaacheck, previewpost);
    }    
    }*/


    /* 	if(previewpost || $('postsubmit').name == 'editsubmit') {
    $("title").disabled = false;
    //alert($("title").disabled);
		
    return true;
    } */

    //if(in_array($('postsubmit').name, ['topicsubmit'])) {
    //	if ($("title").value == getcookie("dnt_title")) {
    //		floatalert("请勿重复发帖,稍后再试");
    //		return false;
    //	}
    //}

    var items = document.createElement("DIV");
    var edittablist = document.createElement("DIV");

    var inputarray = $('attachlist_edittablist').getElementsByTagName('INPUT');

    items.appendChild($('attachlist_tablist_current'));
    items.appendChild($('attachlist_tablist'));
    for (i = 0; i < inputarray.length; i++) {
        edittablist.innerHTML += '<INPUT type=\"text\" name=\"' + inputarray[i].name + '\" value=\"' + inputarray[i].value + '\"></input>';
    }
    items.appendChild(edittablist);
    items.style.display = 'none';
    $('postbox').appendChild(items);
    if (!infloat) {
        $("title").disabled = false;
        return true;

    } else {
        messagehandle();
        $('postform').action = postaction;
        var originalmessage = $("e_textarea") ? $("e_textarea").value : $("e_textarea").value
        var message = wysiwyg ? html2bbcode(getEditorContents()) : (!theform.parseurloff.checked ? parseurl(originalmessage) : originalmessage);
        try {
            $("e_textarea").value = message;
        } catch (e) {
            $("e_textarea").value = message;
        }
        ajaxpost('postform', 'returnmessage', 'returnmessage', 'onerror');
        return false;
    }
}

function seccheck(theform, seccodecheck, secqaacheck, previewpost) {
	if(!previewpost && (seccodecheck || secqaacheck)) {
		var url = 'ajax.php?inajax=1&action=';
		if(seccodecheck) {
			var x = new Ajax();
			x.get(url + 'checkseccode&seccodeverify=' + $('seccodeverify').value, function(s) {
				if(s != 'succeed') {
					alert(s);
					$('seccodeverify').focus();
				} else if(secqaacheck) {
					checksecqaa(url, theform);
				} else {
					postsubmit(theform);
				}
			});
		} else if(secqaacheck) {
			checksecqaa(url, theform);
		}
	} else {
		postsubmit(theform, previewpost);
	}
}

function checksecqaa(url, theform) {
	var x = new Ajax();
	var secanswer = $('secanswer').value;
	secanswer = is_ie && document.charset == 'utf-8' ? encodeURIComponent(secanswer) : secanswer;
	x.get(url + 'checksecanswer&secanswer=' + secanswer, function(s) {
		if(s != 'succeed') {
			alert(s);
			$('secanswer').focus();
		} else {
			postsubmit(theform);
		}
	});
}

function postsubmit(theform, previewpost) {
	if(!previewpost) {
		theform.replysubmit ? theform.replysubmit.disabled = true : theform.topicsubmit.disabled = true;
		theform.submit();
	}
}

function previewpost(){
	if(!validate($('postform'), true)) {
		try{$('title').focus();}catch(e){}
		return;
	}
	var originalmessage = $('postform').message ? $('postform').message.value : $("e_textarea").value
	$("previewmessage").innerHTML = '<span class="bold"><span class="smalltxt">' + $('title').value + '</span></span><br /><br /><span style="font-size: {MSGFONTSIZE}">' + bbcode2html(originalmessage) + '</span>';
	$("previewtable").style.display = '';
	window.scroll(0, 0);
}

function clearcontent() {
	if(wysiwyg && bbinsert) {
		editdoc.body.innerHTML = is_moz ? '<br />' : '';
	} else {
		textobj.value = '';
	}
}

function resizeEditor(change) {
	var editorbox = bbinsert ? editbox : textobj;
	var newheight = parseInt(editorbox.style.height, 10) + change;
	if(newheight >= 100) {
		editorbox.style.height = newheight + 'px';
	}
}

function encodeURL(a) {
	return window.encodeURIComponent?encodeURIComponent(a):escape(a)
}

function relatekw(title, message) {
	if (typeof title=='undefined')
	{
		title = $('title').value;
	}
	if (typeof message=='undefined')
	{
		message = getEditorContents();
	}		
	title = encodeURL(title);
	message = message.replace(/&/ig, '', message);
	message = encodeURL(message);
	_sendRequest('tools/ajax.ashx?t=relatekw', relatedkw, true, 'titleenc=' + title + '&contentenc=' + message);
}

function relatedkw(obj) {
	if (obj == null || typeof(obj) != "object" || obj.firstChild == null)
	{
		return;
	}
	if (is_ie)
	{
		evalscript(obj.text);
	}
	else
	{
		evalscript(obj.firstChild.firstChild.nodeValue);
	}
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