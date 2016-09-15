var Ajaxs = new Array();
var AjaxStacks = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
var attackevasive = 1;

function createXMLHttp() {
	if(window.XMLHttpRequest){
		return new XMLHttpRequest();
	} else if(window.ActiveXObject){
		return new ActiveXObject("Microsoft.XMLHTTP");
	} 
	throw new Error("XMLHttp object could be created.");
}

function _sendRequest(url,func,isxml,postdata)
{
	var xhr=createXMLHttp();
	if(!postdata)postdata=null;
	xhr.open(postdata?"POST":"GET",url,true);
	if (postdata)
	{
		xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	}
	if(func){
		xhr.onreadystatechange=function(){
			if(xhr.readyState==4){
				func(isxml&&xhr.responseXML?xhr.responseXML:xhr.responseText)
			}
		}
	}
	if (postdata === true)
	{
		postdata = '';
	}
	xhr.send(postdata)
}
function ajaxRead(file,fun){
	var xmlObj = createXMLHttp();

	xmlObj.onreadystatechange = function(){
		if(xmlObj.readyState == 4){
			if (xmlObj.status ==200){
				obj = xmlObj.responseXML;
				eval(fun);
			}
			else{
				alert("读取文件出错,错误号为 [" + xmlObj.status  + "]");
			}
		}
	}
	xmlObj.open ('GET', file, true);
	xmlObj.send (null);
}

function getRequestBody(oForm) {
	var aParams = new Array();
	for (var i=0 ; i < oForm.elements.length; i++) {
		/*
		if (oForm.elements[i].type == "checkbox" && oForm.elements[i].checked == false)
		{
			continue;
		}
		*/
		var sParam = encodeURIComponent(oForm.elements[i].name);
		sParam += "=";
		sParam += encodeURIComponent(oForm.elements[i].value);
		aParams.push(sParam);
	}
	return aParams.join("&");
}


function getSpecificNodeValue(doc, tagname, index)
{
	try{
		var oNodes = doc.getElementsByTagName(tagname);
		if (oNodes[index] != null && oNodes[index] != undefined)
		{
			if (oNodes[index].childNodes.length > 1) {
				return oNodes[index].childNodes[1].nodeValue;
			} else {
				return oNodes[index].firstChild.nodeValue;    		
			}
		}
	}
	catch(e){}
	return '';
}

function getSingleNodeValue(doc, tagname)
{
	try{
		var oNodes = doc.getElementsByTagName(tagname);
		if (oNodes[0] != null && oNodes[0] != undefined)
		{
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



function appendscript(src, text, reload) {
	var id = hash(src + text);
	if(!reload && in_array(id, evalscripts)) return;

	if(reload && $(id)) {

		$(id).parentNode.removeChild($(id));

	}

	evalscripts.push(id);

	var scriptNode = document.createElement("script");

	scriptNode.type = "text/javascript";

	scriptNode.id = id;

	try {

		if(src) {

			scriptNode.src = src;

		} else if(text){

			scriptNode.text = text;

		}

		$('append_parent').appendChild(scriptNode);

	} catch(e) {}

}

function hash(string, length) {

	var length = length ? length : 32;

	var start = 0;

	var i = 0;

	var result = '';

	filllen = length - string.length % length;

	for(i = 0; i < filllen; i++){

		string += "0";

	}

	while(start < string.length) {

		result = stringxor(result, string.substr(start, length));

		start += length;

	}

	return result;

}

function stringxor(s1, s2) {

	var s = '';

	var hash = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';

	var max = Math.max(s1.length, s2.length);

	for(var i=0; i<max; i++) {

		var k = s1.charCodeAt(i) ^ s2.charCodeAt(i);

		s += hash.charAt(k % 52);

	}

	return s;

}

function ajaxmenu(e, ctrlid, timeout, func, cache, duration, ismenu, divclass, optionclass) {
	showloading();

	if(jsmenu['active'][0] && jsmenu['active'][0].ctrlkey == ctrlid) {
		doane(e);
		return;
	} else if(is_ie && is_ie < 7 && document.readyState.toLowerCase() != 'complete') {
		return;
	}

	if(isUndefined(timeout)) timeout = 3000;
	if(isUndefined(func)) func = '';
	if(isUndefined(cache)) cache = 1;
	if(isUndefined(divclass)) divclass = 'popupmenu_popup';
	if(isUndefined(optionclass)) optionclass = 'popupmenu_option';
	if(isUndefined(ismenu)) ismenu = 1;
	if(isUndefined(duration)) duration = timeout > 0 ? 0 : 3;
	var div = $(ctrlid + '_menu');

	if(cache && div) {
		showMenu(ctrlid, e.type == 'click', 0, duration, timeout);
		if(func) setTimeout(func + '(' + ctrlid + ')', timeout);
		doane(e);
	} else {
		if(!div) {
			div = document.createElement('div');
			div.ctrlid = ctrlid;
			div.id = ctrlid + '_menu';
			div.style.display = 'none';
			div.className = divclass;
			$('append_parent').appendChild(div);
		}

		var x = new Ajax();
		var href = !isUndefined($(ctrlid).href) ? $(ctrlid).href : $(ctrlid).attributes['href'].value;
		x.div = div;
		x.etype = e.type;
		x.get(href + '&inajax=1&ajaxmenuid='+ctrlid+'_menu', function(s) {
			evaled = false;
			if(s.indexOf('ajaxerror') != -1) {
				evalscript(s);
				evaled = true;
				if(!cache && duration != 3 && x.div.id) setTimeout('document.body.removeChild($(\'' + x.div.id + '\'))', timeout);
			}

			if(!evaled && (typeof ajaxerror == 'undefined' || !ajaxerror)) {
				if(x.div) x.div.innerHTML = '<div class="' + optionclass + '">' + s + '</div>';
				showMenu(ctrlid, x.etype == 'click', 0, duration, timeout);
				if(func) setTimeout(func + '("' + ctrlid + '")', timeout);
			}
			if(!evaled) evalscript(s);
			ajaxerror = null;
			showloading('none');
		});
		doane(e);
	}
}

function showloading(display, wating) {
	var display = display ? display : 'block';
	var wating = wating ? wating : 'Loading...';
	$('ajaxwaitid').innerHTML = wating;
	$('ajaxwaitid').style.display = display;
	$('ajaxwaitid').style.left = ((document.documentElement.clientWidth - $('ajaxwaitid').offsetWidth)/ 2) + 'px';
	$('ajaxwaitid').style.top = document.documentElement.scrollTop + 'px';
}

var ajaxpostHandle = 0;

function ajaxupdateevents(obj, tagName) {

	tagName = tagName ? tagName : 'A';

	var objs = obj.getElementsByTagName(tagName);

	for(k in objs) {

		var o = objs[k];

		ajaxupdateevent(o);

	}

}

function ajaxupdateevent(o) {

	if(typeof o == 'object' && o.getAttribute) {

		if(o.getAttribute('ajaxtarget')) {

			if(!o.id) o.id = Math.random();

			var ajaxevent = o.getAttribute('ajaxevent') ? o.getAttribute('ajaxevent') : 'click';

			var ajaxurl = o.getAttribute('ajaxurl') ? o.getAttribute('ajaxurl') : o.href;

			_attachEvent(o, ajaxevent, newfunction('ajaxget', ajaxurl, o.getAttribute('ajaxtarget'), o.getAttribute('ajaxwaitid'), o.getAttribute('ajaxloading'), o.getAttribute('ajaxdisplay')));

			if(o.getAttribute('ajaxfunc')) {

				o.getAttribute('ajaxfunc').match(/(\w+)\((.+?)\)/);

				_attachEvent(o, ajaxevent, newfunction(RegExp.$1, RegExp.$2));

			}

		}

	}

}

function newfunction(func){

	var args = new Array();

	for(var i=1; i<arguments.length; i++) args.push(arguments[i]);

	return function(event){

		doane(event);

		window[func].apply(window, args);

		return false;

	}
}