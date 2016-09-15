function checklength(theform) {
	if ($("posteditor_mode").value == '1') { 
		theform.message.value = html2bbcode(getEditorContents()); 
	}
	if (postmaxchars != 0) { 
		message = "系统限制: " + postminchars + " 到 " + postmaxchars + " 字符"; 
	}
	else { 
		message = ""; 
	}
	alert("\n当前长度: " + mb_strlen(theform.message.value) + " 字符\n\n" + message);
}

function previewpost(theform) {
	if(!validate(theform, true)) {
		$('title').focus();
		return;
	}
	$("previewmessage").innerHTML = '<span class="bold"><span class="smalltxt">' + $('title').value + '</span></span><br /><br /><span style="font-size: 12px">' + bbcode2html(theform.message.value) + '</span>';
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
