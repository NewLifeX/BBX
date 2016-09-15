var BROWSER = {};
var USERAGENT = navigator.userAgent.toLowerCase();
browserVersion({'ie':'msie','firefox':'','chrome':'','opera':'','safari':'','mozilla':'','webkit':'','maxthon':'','qq':'qqbrowser'});
if(BROWSER.safari) {
    BROWSER.firefox = true;
}
BROWSER.opera = BROWSER.opera ? opera.version() : 0;
if(BROWSER.ie) {

    document.documentElement.addBehavior("#default#userdata");

}
function browserVersion(types) {
    var other = 1;
    for(i in types) {
        var v = types[i] ? types[i] : i;
        if(USERAGENT.indexOf(v) != -1) {
            var re = new RegExp(v + '(\\/|\\s)([\\d\\.]+)', 'ig');
            var matches = re.exec(USERAGENT);
            var ver = matches != null ? matches[2] : 0;
            other = ver !== 0 && v != 'mozilla' ? 0 : other;
        }else {
            var ver = 0;
        }
        eval('BROWSER.' + i + '= ver');
    }
    BROWSER.other = other;
}

var lang = new Array();
var userAgent = navigator.userAgent.toLowerCase();
var is_opera = userAgent.indexOf('opera') != -1 && opera.version();
var is_moz = (navigator.product == 'Gecko') && userAgent.substr(userAgent.indexOf('firefox') + 8, 3);
var is_ie = (userAgent.indexOf('msie') != -1 && !is_opera) && userAgent.substr(userAgent.indexOf('msie') + 5, 3);
var is_mac = userAgent.indexOf('mac') != -1;
var ajaxdebug = 0;
var codecount = '-1';
var codehtml = new Array();
var charset='utf-8';
//FixPrototypeForGecko
var cookiepath = typeof forumpath == 'undefined' ? '' : forumpath;
if(is_moz && window.HTMLElement) {
    HTMLElement.prototype.__defineSetter__('outerHTML', function(sHTML) {
            var r = this.ownerDocument.createRange();
        r.setStartBefore(this);
        var df = r.createContextualFragment(sHTML);
        this.parentNode.replaceChild(df,this);
        return sHTML;
    });

    HTMLElement.prototype.__defineGetter__('outerHTML', function() {
        var attr;
        var attrs = this.attributes;
        var str = '<' + this.tagName.toLowerCase();
        for(var i = 0;i < attrs.length;i++){
            attr = attrs[i];
            if(attr.specified)
            str += ' ' + attr.name + '="' + attr.value + '"';
        }
        if(!this.canHaveChildren) {
            return str + '>';
        }
        return str + '>' + this.innerHTML + '</' + this.tagName.toLowerCase() + '>';
        });

    HTMLElement.prototype.__defineGetter__('canHaveChildren', function() {
        switch(this.tagName.toLowerCase()) {
            case 'area':case 'base':case 'basefont':case 'col':case 'frame':case 'hr':case 'img':case 'br':case 'input':case 'isindex':case 'link':case 'meta':case 'param':
            return false;
            }
        return true;
    });
    HTMLElement.prototype.click = function(){
        var evt = this.ownerDocument.createEvent('MouseEvents');
        evt.initMouseEvent('click', true, true, this.ownerDocument.defaultView, 1, 0, 0, 0, 0, false, false, false, false, 0, null);
        this.dispatchEvent(evt);
    }
}

/* Array.prototype.push = function(value) {
    this[this.length] = value;
    return this.length;
} */

function $(id) {
    return document.getElementById(id);
}

function checkall(form, prefix, checkall) {
    var checkall = checkall ? checkall : 'chkall';
    var count = 0;
    for(var i = 0; i < form.elements.length; i++) {
        var e = form.elements[i];
        if (e.name && e.name != checkall && (!prefix || (prefix && e.name.match(prefix)) && !e.getAttribute("topictype"))) {
            e.checked = form.elements[checkall].checked;
            if(e.checked) {
                count++;
            }
        }
    }
    return count;
}

/* function doane(event) {
    e = event ? event : window.event;
    if(is_ie) {
        e.returnValue = false;
        e.cancelBubble = true;
    } else if(e) {
        e.stopPropagation();
        e.preventDefault();
    }
} */
function doane(event) {

    e = event ? event : window.event;

    if(!e) e = getEvent();

    if(e && BROWSER.ie) {

        e.returnValue = false;

        e.cancelBubble = true;

    } else if(e) {

        e.stopPropagation();

        e.preventDefault();

    }

}
function getEvent() {

    if(document.all) return window.event;

    func = getEvent.caller;

    while(func != null) {

        var arg0 = func.arguments[0];

        if (arg0) {

            if((arg0.constructor  == Event || arg0.constructor == MouseEvent) || (typeof(arg0) == "object" && arg0.preventDefault && arg0.stopPropagation)) {

                return arg0;

            }

        }

        func=func.caller;

    }

    return null;

}


function fetchCheckbox(cbn) {
    return $(cbn) && $(cbn).checked == true ? 1 : 0;
}

function getcookie(name) {
    var cookie_start = document.cookie.indexOf(name);
    var cookie_end = document.cookie.indexOf(";", cookie_start);
    return cookie_start == -1 ? '' : unescape(document.cookie.substring(cookie_start + name.length + 1, (cookie_end > cookie_start ? cookie_end : document.cookie.length)));
}

imggroup = new Array();
function thumbImg(obj, method) {
    if(!obj) {
        return;
    }
    obj.onload = null;
    file = obj.src;
    zw = obj.offsetWidth;
    zh = obj.offsetHeight;
    if(zw < 2) {
        if(!obj.id) {
            obj.id = 'img_' + Math.random();
        }
        setTimeout("thumbImg($('" + obj.id + "'), " + method + ")", 100);
        return;
    }
    zr = zw / zh;
    method = !method ? 0 : 1;
    if(method) {
        fixw = obj.getAttribute('_width');
        fixh = obj.getAttribute('_height');
        if(zw > fixw) {
            zw = fixw;
            zh = zw / zr;
        }
        if(zh > fixh) {
            zh = fixh;
            zw = zh * zr;
        }
    } else {
        var widthary = typeof imagemaxwidth=='undefined'?[]:imagemaxwidth.split('%');
        if(widthary.length > 1) {
            fixw = $('wrap').clientWidth - 200;
            if(widthary[0]) {				
                fixw = fixw * widthary[0] / 100;				
            } else if(widthary[1]) {
                fixw = fixw < widthary[1] ? fixw : widthary[1];					
            }
        } else {
            fixw = widthary[0];
        }
        if(zw > fixw) {
            zw = fixw;
            zh = zw / zr;
            obj.style.cursor = 'pointer';
            if(!obj.onclick) {
                obj.onclick = function() {
                    zoom(obj, obj.src);
                }
            }
        }
    }
    obj.width = zw;
    obj.height = zh;
}

function imgzoom() {}
function attachimg() {}

function in_array(needle, haystack) {
    if(typeof needle == 'string' || typeof needle == 'number') {
        for(var i in haystack) {
            if(haystack[i] == needle) {
                    return true;
            }
        }
    }
    return false;
}

var clipboardswfdata;
function setcopy(text, alertmsg){
    if (is_ie) {
        clipboardData.setData('Text', text);
        if (alertmsg) {
            alert(alertmsg);
        }
    } else {
        var msg = '<div style="width: 200px; text-align: center; text-decoration:underline;">点此复制到剪贴板</div>' +
        AC_FL_RunContent('id', 'clipboardswf', 'name', 'clipboardswf', 'devicefont', 'false', 'width', '200', 'height', '40', 'src', 'images/common/clipboard.swf', 'menu', 'false', 'allowScriptAccess', 'sameDomain', 'swLiveConnect', 'true', 'wmode', 'transparent', 'style', 'margin-top:-20px') + '</div>';
        showDialog(msg, 'info');
        text = text.replace(/[\xA0]/g, ' ');
        clipboardswfdata = text;
    }
}

function getClipboardData() {
    window.document.clipboardswf.SetVariable('str', clipboardswfdata);
}

function dconfirm(msg, script, width, height) {
    floatwin('open_confirm', -1, !width ? 300 : width, !height ? 110 : height);
    $('floatwin_confirm_title').innerHTML = '提示信息';
    $('floatwin_confirm_content').innerHTML = msg + '<br /><button onclick="' + script + ';floatwin(\'close_confirm\')">&nbsp;是&nbsp;</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button onclick="floatwin(\'close_confirm\')">&nbsp;否&nbsp;</button>';
}

function dnotice(msg, script, width, height) {
    script = !script ? '' : script;
    floatwin('open_confirm', -1, !width ? 400 : width, !height ? 110 : height);
    $('floatwin_confirm_title').innerHTML = '提示信息';
    $('floatwin_confirm_content').innerHTML = msg + (script ? '<br /><button onclick="' + script + ';floatwin(\'close_confirm\')">确定</button>' : '');
}

function setcopy_gettext() {
    window.document.clipboardswf.SetVariable('str', clipboardswfdata)
}

function isUndefined(variable) {
    return typeof variable == 'undefined' ? true : false;
}

function mb_strlen(str) {
    var len = 0;
    for(var i = 0; i < str.length; i++) {
        len += str.charCodeAt(i) < 0 || str.charCodeAt(i) > 255 ? (charset == 'utf-8' ? 3 : 2) : 1;
    }
    return len;
}

function mb_cutstr(str, maxlen, dot) {
    var len = 0;
    var ret = '';
    var dot = !dot ? '...' : '';
    maxlen = maxlen - dot.length;
    for(var i = 0; i < str.length; i++) {
        len += str.charCodeAt(i) < 0 || str.charCodeAt(i) > 255 ? (charset == 'utf-8' ? 3 : 2) : 1;
        if(len > maxlen) {
            ret += dot;
            break;
        }
        ret += str.substr(i, 1);
    }
    return ret;
}

function setcookie(cookieName, cookieValue, seconds, path, domain, secure) {
    var expires = new Date();
    expires.setTime(expires.getTime() + seconds * 1000);
    domain = !domain ? cookiedomain : domain;
    path = !path ? cookiepath : path;
    document.cookie = escape(cookieName) + '=' + escape(cookieValue)
        + (expires ? '; expires=' + expires.toGMTString() : '')
        + (path ? '; path=' + path : '/')
        + (domain ? '; domain=' + domain : '')
        + (secure ? '; secure' : '');
}

function strlen(str) {
    return (is_ie && str.indexOf('\n') != -1) ? str.replace(/\r?\n/g, '_').length : str.length;
}

function updatestring(str1, str2, clear) {
    str2 = '_' + str2 + '_';
    return clear ? str1.replace(str2, '') : (str1.indexOf(str2) == -1 ? str1 + str2 : str1);
}

function toggle_collapse(objname, noimg, complex, lang) {
    var obj = $(objname);
    if(obj) {
        obj.style.display = obj.style.display == '' ? 'none' : '';
        var collapsed = getcookie('bbx_collapse');
        collapsed = updatestring(collapsed, objname, !obj.style.display);
        setcookie('bbx_collapse', collapsed, (collapsed ? 2592000 : -2592000));
    }
    if(!noimg) {
        var img = $(objname + '_img');
        if(img.tagName != 'IMG') {
            if(img.className.indexOf('_yes') == -1) {
                img.className = img.className.replace(/_no/, '_yes');
                if(lang) {
                    img.innerHTML = lang[0];
                }
            } else {
                img.className = img.className.replace(/_yes/, '_no');
                if(lang) {
                    img.innerHTML = lang[1];
                }
            }
        } else {
            img.src = img.src.indexOf('_yes.gif') == -1 ? img.src.replace(/_no\.gif/, '_yes\.gif') : img.src.replace(/_yes\.gif/, '_no\.gif');
        }
        img.blur();
    }
    if(complex) {
        var objc = $(objname + '_c');
        objc.className = objc.className == 'c_header' ? 'c_header closenode' : 'c_header';
    }

}

function sidebar_collapse(lang) {
    if(lang[0]) {
        toggle_collapse('sidebar', null, null, lang);
        $('wrap').className = $('wrap').className == 'wrap with_side s_clear' ? 'wrap s_clear' : 'wrap with_side s_clear';
    } else {
        var collapsed = getcookie('bbx_collapse');
        collapsed = updatestring(collapsed, 'sidebar', 1);
        setcookie('bbx_collapse', collapsed, (collapsed ? 2592000 : -2592000));
        location.reload();
    }
}

function trim(str) {
    return (str + '').replace(/(\s+)$/g, '').replace(/^\s+/g, '');
}

function _attachEvent(obj, evt, func, eventobj) {
    eventobj = !eventobj ? obj : eventobj;
    if(obj.addEventListener) {
        obj.addEventListener(evt, func, false);
    } else if(eventobj.attachEvent) {
        obj.attachEvent("on" + evt, func);
    }
}

var cssloaded= new Array();
function loadcss(cssname) {
    if(!cssloaded[cssname]) {
        css = document.createElement('link');
        css.type = 'text/css';
        css.rel = 'stylesheet';
        css.href = 'forumdata/cache/style_' + STYLEID + '_' + cssname + '.css?' + VERHASH;
        var headNode = document.getElementsByTagName("head")[0];
        headNode.appendChild(css);
        cssloaded[cssname] = 1;
    }
}

var jsmenu = new Array();
var ctrlobjclassName;
jsmenu['active'] = new Array();
jsmenu['timer'] = new Array();
jsmenu['iframe'] = new Array();


var JSMENU = [];
JSMENU['active'] = [];
JSMENU['timer'] = [];
JSMENU['drag'] = [];
JSMENU['layer'] = 0;
JSMENU['zIndex'] = {'win':200,'menu':300,'prompt':400,'dialog':500};
JSMENU['float'] = '';

function initCtrl(ctrlobj, click, duration, timeout, layer) {
    if(ctrlobj && !ctrlobj.initialized) {
        ctrlobj.initialized = true;
        ctrlobj.unselectable = true;

        ctrlobj.outfunc = typeof ctrlobj.onmouseout == 'function' ? ctrlobj.onmouseout : null;
        ctrlobj.onmouseout = function() {
            if(this.outfunc) this.outfunc();
            if(duration < 3 && !jsmenu['timer'][ctrlobj.id]) jsmenu['timer'][ctrlobj.id] = setTimeout('hideMenu(' + layer + ')', timeout);
        }

        ctrlobj.overfunc = typeof ctrlobj.onmouseover == 'function' ? ctrlobj.onmouseover : null;
        ctrlobj.onmouseover = function(e) {
            doane(e);
            if(this.overfunc) this.overfunc();
            if(click) {
                clearTimeout(jsmenu['timer'][this.id]);
                jsmenu['timer'][this.id] = null;
            } else {
                for(var id in jsmenu['timer']) {
                    if(jsmenu['timer'][id]) {
                        clearTimeout(jsmenu['timer'][id]);
                        jsmenu['timer'][id] = null;
                    }
                }
            }
        }
    }
}

function initMenu(ctrlid, menuobj, duration, timeout, layer, drag) {
    if(menuobj && !menuobj.initialized) {
        menuobj.initialized = true;
        menuobj.ctrlkey = ctrlid;
        menuobj.onclick = ebygum;
        menuobj.style.position = 'absolute';
        if(duration < 3) {
            if(duration > 1) {
                menuobj.onmouseover = function() {
                    clearTimeout(jsmenu['timer'][ctrlid]);
                    jsmenu['timer'][ctrlid] = null;
                }
            }
            if(duration != 1) {
                menuobj.onmouseout = function() {
                    jsmenu['timer'][ctrlid] = setTimeout('hideMenu(' + layer + ')', timeout);
                }
            }
        }
        menuobj.style.zIndex = 999 + layer;
        if (ctrlid.indexOf("calendarexp") != -1)
            menuobj.style.zIndex = 10003;
        if(drag) {
            menuobj.onmousedown = function(event) {try{menudrag(menuobj, event, 1);}catch(e){}};
            menuobj.onmousemove = function(event) {try{menudrag(menuobj, event, 2);}catch(e){}};
            menuobj.onmouseup = function(event) {try{menudrag(menuobj, event, 3);}catch(e){}};
        }
    }
}

var menudragstart = new Array();
function menudrag(menuobj, e, op) {
    if(op == 1) {
        if(in_array(is_ie ? event.srcElement.tagName : e.target.tagName, ['TEXTAREA', 'INPUT', 'BUTTON', 'SELECT'])) {
            return;
        }
        menudragstart = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
        menudragstart[2] = parseInt(menuobj.style.left);
        menudragstart[3] = parseInt(menuobj.style.top);
        doane(e);
    } else if(op == 2 && menudragstart[0]) {
        var menudragnow = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
        menuobj.style.left = (menudragstart[2] + menudragnow[0] - menudragstart[0]) + 'px';
        menuobj.style.top = (menudragstart[3] + menudragnow[1] - menudragstart[1]) + 'px';
        doane(e);
    } else if(op == 3) {
        menudragstart = [];
        doane(e);
    }
}


function showMenu(v) {
    var ctrlid = isUndefined(v['ctrlid']) ? v : v['ctrlid'];
    var showid = isUndefined(v['showid']) ? ctrlid : v['showid'];
    var menuid = isUndefined(v['menuid']) ? showid + '_menu' : v['menuid'];
    var ctrlObj = $(ctrlid);
    var menuObj = $(menuid);
    if(!menuObj) return;
    var mtype = isUndefined(v['mtype']) ? 'menu' : v['mtype'];
    var evt = isUndefined(v['evt']) ? 'mouseover' : v['evt'];
    var pos = isUndefined(v['pos']) ? '43' : v['pos'];
    var layer = isUndefined(v['layer']) ? 1 : v['layer'];
    var duration = isUndefined(v['duration']) ? 2 : v['duration'];
    var timeout = isUndefined(v['timeout']) ? 250 : v['timeout'];
    var maxh = isUndefined(v['maxh']) ? 600 : v['maxh'];
    var cache = isUndefined(v['cache']) ? 1 : v['cache'];
    var drag = isUndefined(v['drag']) ? '' : v['drag'];
    var dragobj = drag && $(drag) ? $(drag) : menuObj;
    var fade = isUndefined(v['fade']) ? 0 : v['fade'];
    var cover = isUndefined(v['cover']) ? 0 : v['cover'];
    var zindex = isUndefined(v['zindex']) ? JSMENU['zIndex']['menu'] : v['zindex'];
    zindex = cover ? zindex + 200 : zindex;
    if(typeof JSMENU['active'][layer] == 'undefined') {
        JSMENU['active'][layer] = [];
    }

    if(evt == 'click' && in_array(menuid, JSMENU['active'][layer]) && mtype != 'win') {
        hideMenu(menuid, mtype);
        return;
    }
    if(mtype == 'menu') {
        hideMenu(layer, mtype);
    }

    if(ctrlObj) {
        //note 初始化ctrlObj
        if(!ctrlObj.initialized) {
            ctrlObj.initialized = true;
            ctrlObj.unselectable = true;

            ctrlObj.outfunc = typeof ctrlObj.onmouseout == 'function' ? ctrlObj.onmouseout : null;
            ctrlObj.onmouseout = function() {
                if(this.outfunc) this.outfunc();
                if(duration < 3 && !JSMENU['timer'][menuid]) JSMENU['timer'][menuid] = setTimeout('hideMenu(\'' + menuid + '\', \'' + mtype + '\')', timeout);
            };

            ctrlObj.overfunc = typeof ctrlObj.onmouseover == 'function' ? ctrlObj.onmouseover : null;
            ctrlObj.onmouseover = function(e) {
                doane(e);
                if(this.overfunc) this.overfunc();
                if(evt == 'click') {
                    clearTimeout(JSMENU['timer'][menuid]);
                    JSMENU['timer'][menuid] = null;
                } else {
                    for(var i in JSMENU['timer']) {
                        if(JSMENU['timer'][i]) {
                            clearTimeout(JSMENU['timer'][i]);
                            JSMENU['timer'][i] = null;
                        }
                    }
                }
            };
        }
    }

    var dragMenu = function(menuObj, e, op) {
        e = e ? e : window.event;
        if(op == 1) {
            if(in_array(BROWSER.ie ? e.srcElement.tagName : e.target.tagName, ['TEXTAREA', 'INPUT', 'BUTTON', 'SELECT'])) {
                return;
            }
            JSMENU['drag'] = [e.clientX, e.clientY];
            JSMENU['drag'][2] = parseInt(menuObj.style.left);
            JSMENU['drag'][3] = parseInt(menuObj.style.top);
            document.onmousemove = function(e) {try{dragMenu(menuObj, e, 2);}catch(err){}};
            document.onmouseup = function(e) {try{dragMenu(menuObj, e, 3);}catch(err){}};
            doane(e);
        }else if(op == 2 && JSMENU['drag'][0]) {
            var menudragnow = [e.clientX, e.clientY];
            menuObj.style.left = (JSMENU['drag'][2] + menudragnow[0] - JSMENU['drag'][0]) + 'px';
            menuObj.style.top = (JSMENU['drag'][3] + menudragnow[1] - JSMENU['drag'][1]) + 'px';
            doane(e);
        }else if(op == 3) {
            JSMENU['drag'] = [];
            document.onmousemove = null;
            document.onmouseup = null;
        }
    };

    //note 初始化menuObj
    if(!menuObj.initialized) {
        menuObj.initialized = true;
        menuObj.ctrlkey = ctrlid;
        menuObj.mtype = mtype;
        menuObj.layer = layer;
        menuObj.cover = cover;
        if(ctrlObj && ctrlObj.getAttribute('fwin')) {menuObj.scrolly = true;}
        menuObj.style.position = 'absolute';
        menuObj.style.zIndex = zindex + layer;
        menuObj.onclick = function(e) {
            if(!e || BROWSER.ie) {
                window.event.cancelBubble = true;
                return window.event;
            } else {
                e.stopPropagation();
                return e;
            }
        };
        if(duration < 3) {
            if(duration > 1) {
                menuObj.onmouseover = function() {
                    clearTimeout(JSMENU['timer'][menuid]);
                    JSMENU['timer'][menuid] = null;
                };
            }
            if(duration != 1) {
                menuObj.onmouseout = function() {
                    JSMENU['timer'][menuid] = setTimeout('hideMenu(\'' + menuid + '\', \'' + mtype + '\')', timeout);
                };
            }
        }
        if(cover) {
            var coverObj = document.createElement('div');
            coverObj.id = menuid + '_cover';
            coverObj.style.position = 'absolute';
            coverObj.style.zIndex = menuObj.style.zIndex - 1;
            coverObj.style.left = coverObj.style.top = '0px';
            coverObj.style.width = '100%';
            coverObj.style.height = document.body.offsetHeight + 'px';
            coverObj.style.backgroundColor = '#000';
            coverObj.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=50)';
            coverObj.style.opacity = 0.5;
            $('append_parent').appendChild(coverObj);
            _attachEvent(window, 'load', function () {
                coverObj.style.height = document.body.offsetHeight + 'px';
            }, document);
        }
    }
    if(drag) {
        dragobj.style.cursor = 'move';
        dragobj.onmousedown = function(event) {try{dragMenu(menuObj, event, 1);}catch(e){}};
    }
    menuObj.style.display = '';
    if(cover) $(menuid + '_cover').style.display = '';
    if(fade) {
        var O = 0;
        var fadeIn = function(O) {
            if(O == 100) {
                clearTimeout(fadeInTimer);
                return;
            }
            menuObj.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=' + O + ')';
            menuObj.style.opacity = O / 100;
            O += 10;
            var fadeInTimer = setTimeout(function () {
                fadeIn(O);
            }, 50);
        };
        fadeIn(O);
        menuObj.fade = true;
    } else {
        menuObj.fade = false;
    }
    if(pos != '*') {
        setMenuPosition(showid, menuid, pos);
    }
    if(maxh && menuObj.scrollHeight > maxh) {
        menuObj.style.height = maxh + 'px';
        if(BROWSER.opera) {
            menuObj.style.overflow = 'auto';
        } else {
            menuObj.style.overflowY = 'auto';
        }
    }

    if(!duration) {
        setTimeout('hideMenu(\'' + menuid + '\', \'' + mtype + '\')', timeout);
    }

    if(!in_array(menuid, JSMENU['active'][layer])) JSMENU['active'][layer].push(menuid);
    menuObj.cache = cache;
    if(layer > JSMENU['layer']) {
        JSMENU['layer'] = layer;
    }
}

function setMenuPosition(showid, menuid, pos) {
    var showObj = $(showid);
    var menuObj = menuid ? $(menuid) : $(showid + '_menu');
    if(isUndefined(pos)) pos = '43';
    var basePoint = parseInt(pos.substr(0, 1));
    var direction = parseInt(pos.substr(1, 1));
    var sxy = 0, sx = 0, sy = 0, sw = 0, sh = 0, ml = 0, mt = 0, mw = 0, mcw = 0, mh = 0, mch = 0, bpl = 0, bpt = 0;
    if(!menuObj || (basePoint > 0 && !showObj)) return;
    if(showObj) {
        sxy = fetchOffset(showObj);
        sx = sxy['left'];
        sy = sxy['top'];
        sw = showObj.offsetWidth;
        sh = showObj.offsetHeight;
    }
    mw = menuObj.offsetWidth;
    mcw = menuObj.clientWidth;
    mh = menuObj.offsetHeight;
    mch = menuObj.clientHeight;

    switch(basePoint) {
        case 1:
            bpl = sx;
            bpt = sy;
            break;
        case 2:
            bpl = sx + sw;
            bpt = sy;
            break;
        case 3:
            bpl = sx + sw;
            bpt = sy + sh;
            break;
        case 4:
            bpl = sx;
            bpt = sy + sh;
            break;
    }
    switch(direction) {
        case 0:
            menuObj.style.left = (document.body.clientWidth - menuObj.clientWidth) / 2 + 'px';
            mt = (document.documentElement.clientHeight - menuObj.clientHeight) / 2;
            break;
        case 1:
            ml = bpl - mw;
            mt = bpt - mh;
            break;
        case 2:
            ml = bpl;
            mt = bpt - mh;
            break;
        case 3:
            ml = bpl;
            mt = bpt;
            break;
        case 4:
            ml = bpl - mw;
            mt = bpt;
            break;
    }
    var scrollTop = Math.max(document.documentElement.scrollTop, document.body.scrollTop);
    var scrollLeft = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft);
    if(in_array(direction, [1, 4]) && ml < 0) {
        ml = bpl;
        if(in_array(basePoint, [1, 4])) ml += sw;
    } else if(ml + mw > scrollLeft + document.body.clientWidth && sx >= mw) {
        ml = bpl - mw;
        if(in_array(basePoint, [2, 3])) ml -= sw;
    }
    if(in_array(direction, [1, 2]) && mt < 0) {
        mt = bpt;
        if(in_array(basePoint, [1, 2])) mt += sh;
    } else if(mt + mh > scrollTop + document.documentElement.clientHeight && sy >= mh) {
        mt = bpt - mh;
        if(in_array(basePoint, [3, 4])) mt -= sh;
    }
    if(pos == '210') {
        ml += 69 - sw / 2;
        mt -= 5;
        if(showObj.tagName == 'TEXTAREA') {
            ml -= sw / 2;
            mt += sh / 2;
        }
    }
    if(direction == 0 || menuObj.scrolly) {
        if(BROWSER.ie && BROWSER.ie < 7) {
            if(direction == 0) {mt += scrollTop;}
        } else {
            if(menuObj.scrolly) mt -= scrollTop;
            menuObj.style.position = 'fixed';
        }
    }
    if(ml) menuObj.style.left = ml + 'px';
    if(mt) menuObj.style.top = mt + 'px';
    if((menuObj.id == "infloatquickposttopicsmilies_menu" || 
        menuObj.id == "smilies_menu")
        && BROWSER.ie && BROWSER.ie < 7)
    {
        //menuObj.style.left = parseInt(menuObj.style.left) - 425 + "px";
        //menuObj.style.top = parseInt(menuObj.style.top) - 300 + "px";
        //alert("left=" + menuObj.style.left + ",top=" + menuObj.style.top);
        if($("smilies")){
        menuObj.style.left = parseInt($("smilies").offsetLeft) + 25 + "px";
        menuObj.style.top = parseInt($("smilies").offsetTop) + 120 + "px";
        }
        if($('infloatquickposttopicsmilies')){
        menuObj.style.left = parseInt($("infloatquickposttopicsmilies").offsetLeft) + 25 + "px";
        menuObj.style.top = parseInt($("infloatquickposttopicsmilies").offsetTop) + 120 + "px";
        }
    }
    
    if((menuObj.id == "infloatquickposttopicforecolor_menu" || 
    menuObj.id == "forecolor_menu")
    && BROWSER.ie && BROWSER.ie < 7)
    {
        //menuObj.style.left = parseInt(menuObj.style.left) - 425 + "px";
        //menuObj.style.top = parseInt(menuObj.style.top) - 300 + "px";
        //alert("left=" + menuObj.style.left + ",top=" + menuObj.style.top);
        if($("forecolor")){
        menuObj.style.left = parseInt($("forecolor").offsetLeft) + 25 + "px";
        menuObj.style.top = parseInt($("forecolor").offsetTop) + 120 + "px";
        }
        if($('infloatquickposttopicforecolor')){
        menuObj.style.left = parseInt($("infloatquickposttopicforecolor").offsetLeft) + 25 + "px";
        menuObj.style.top = parseInt($("infloatquickposttopicforecolor").offsetTop) + 120 + "px";
        }
    }
    
    
    
    if(direction == 0 && BROWSER.ie && !document.documentElement.clientHeight) {
        menuObj.style.position = 'absolute';
        menuObj.style.top = (document.body.clientHeight - menuObj.clientHeight) / 2 + 'px';
    }
    if(menuObj.style.clip && !BROWSER.opera) {
        menuObj.style.clip = 'rect(auto, auto, auto, auto)';
    }
}

/**
* 隐藏菜单
* attr	菜单属性 空字串:隐藏所有菜单 字串:隐藏指定menuid的菜单 数字:隐藏某一层菜单
* mtype	菜单类型 参见showMenu函数
*/
function hideMenu(attr, mtype) {
    attr = isUndefined(attr) ? '' : attr;
    mtype = isUndefined(mtype) ? 'menu' : mtype;
    if(attr == '') {
        for(var i = 1; i <= JSMENU['layer']; i++) {
            hideMenu(i, mtype);
        }
        return;
    } else if(typeof attr == 'number') {
        for(var j in JSMENU['active'][attr]) {
            hideMenu(JSMENU['active'][attr][j], mtype);
        }
        return;
    }else if(typeof attr == 'string') {
        var menuObj = $(attr);
        if(!menuObj || (mtype && menuObj.mtype != mtype)) return;
        clearTimeout(JSMENU['timer'][attr]);
        var hide = function () {
            if (menuObj.cache) {
                menuObj.style.display = 'none';
                if (menuObj.cover) $(attr + '_cover').style.display = 'none';
            } else {
                menuObj.parentNode.removeChild(menuObj);
                if (menuObj.cover) $(attr + '_cover').parentNode.removeChild($(attr + '_cover'));
            }
            var tmp = [];
            for (var k in JSMENU['active'][menuObj.layer]) {
                if (attr != JSMENU['active'][menuObj.layer][k]) tmp.push(JSMENU['active'][menuObj.layer][k]);
            }
            JSMENU['active'][menuObj.layer] = tmp;
        };
        if(menuObj.fade) {
            var O = 100;
            var fadeOut = function(O) {
                if(O == 0) {
                    clearTimeout(fadeOutTimer);
                    hide();
                    return;
                }
                menuObj.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=' + O + ')';
                menuObj.style.opacity = O / 100;
                O -= 10;
                var fadeOutTimer = setTimeout(function () {
                    fadeOut(O);
                }, 50);
            };
            fadeOut(O);
        } else {
            hide();
        }
    }
}

function saveData(ignoreempty, form, titlename, contentname) {
    var ignoreempty = isUndefined(ignoreempty) ? 0 : ignoreempty;
        if (!isUndefined(form)) {
            var obj = $(form);
        }
        else {
            var obj = $('postform') && (($('fwin_newthread') && $('fwin_newthread').style.display == '') || ($('fwin_reply') && $('fwin_reply').style.display == '')) ? $('postform') : ($('quickpostform') ? $('quickpostform') : $('postform'));
        }
    if (!obj) return;
    if (typeof isfirstpost != 'undefined') {
        if (typeof wysiwyg != 'undefined' && wysiwyg == 1) {
            var messageisnull = trim(html2bbcode(editdoc.body.innerHTML)) === '';
        } else {
            var msg = $('postform').message;
            var messageisnull = !msg || msg.value === '';
        }

        if (isfirstpost && (messageisnull && $('postform').title.value === '')) {
            return;
        }
        if (!isfirstpost && messageisnull) {
            return;
        }
    }
    var data = subject = message = '';
    for (var i = 0; i < obj.elements.length; i++) {
        var el = obj.elements[i];
        if (el.name != '' && (el.tagName == 'SELECT' || el.tagName == 'TEXTAREA' || el.tagName == 'INPUT' && (el.type == 'text' || el.type == 'checkbox' || el.type == 'radio' || el.type == 'hidden' || el.type == 'select')) && el.name.substr(0, 6) != 'attach') {
            var elvalue = el.value;
            if (el.name == titlename) {
                subject = trim(elvalue);
            } else if (el.name == contentname) {
                if (typeof wysiwyg != 'undefined' && wysiwyg == 1) {
                    elvalue = html2bbcode(editdoc.body.innerHTML);
                }
                message = trim(elvalue);
            }

            if ((el.type == 'checkbox' || el.type == 'radio') && !el.checked) {
                continue;
            } else if (el.tagName == 'SELECT') {
                elvalue = el.value;
            } else if (el.type == 'hidden') {
                if (el.id) {
                    eval('var check = typeof ' + el.id + '_upload == \'function\'');
                    if (check) {
                        elvalue = elvalue;
                        if ($(el.id + '_url')) {
                            elvalue += String.fromCharCode(1) + $(el.id + '_url').value;
                        }
                    } else {
                        continue;
                    }
                } else {
                    continue;
                }
            }
            if (trim(elvalue)) {
                data += el.name + String.fromCharCode(9) + el.tagName + String.fromCharCode(9) + el.type + String.fromCharCode(9) + elvalue + String.fromCharCode(9, 9);
            }
        }
    }
    if (!subject && !message && !ignoreempty) {
        return;
    }
    saveUserdata('forum', data);
}


//function saveData(ignoreempty, form, titlename, contentname) {
//    var ignoreempty = isUndefined(ignoreempty) ? 0 : ignoreempty;
//    if (!isUndefined(form)) {
//        var obj = $(form);
//    }
//    else {
//        var obj = $('postform') && (($('fwin_newthread') && $('fwin_newthread').style.display == '') || ($('fwin_reply') && $('fwin_reply').style.display == '')) ? $('postform') : ($('quickpostform') ? $('quickpostform') : $('postform'));
//    }

//    if (!obj) return;
//    var data = subject = message = '';
//    for (var i = 0; i < obj.elements.length; i++) {
//        var el = obj.elements[i];
//        if (el.name != '' && (el.tagName == 'SELECT' || el.tagName == 'TEXTAREA' || el.tagName == 'INPUT' && (el.type == 'text' || el.type == 'checkbox' || el.type == 'radio')) && el.name.substr(0, 6) != 'attach') {
//            var elvalue = el.value;
//            if (el.name == titlename) {
//                subject = trim(elvalue);
//            } else if (el.name == contentname) {
//                if (typeof wysiwyg != 'undefined' && wysiwyg == 1) {
//                    elvalue = html2bbcode(editdoc.body.innerHTML);
//                }
//                message = trim(elvalue);
//            }
//            if ((el.type == 'checkbox' || el.type == 'radio') && !el.checked) {
//                continue;
//            } else if (el.tagName == 'SELECT') {
//                elvalue = el.value;
//            }else if (el.type == 'hidden') {
//                if (el.id) {
//                    eval('var check = typeof ' + el.id + '_upload == \'function\'');
//                    if (check) {
//                        elvalue = elvalue;
//                        if ($(el.id + '_url')) {
//                            elvalue += String.fromCharCode(1) + $(el.id + '_url').value;
//                        }
//                    } else {
//                        continue;
//                    }
//                } else {
//                    continue;
//                }

//            }
//            if (trim(elvalue)) {
//                data += el.name + String.fromCharCode(9) + el.tagName + String.fromCharCode(9) + el.type + String.fromCharCode(9) + elvalue + String.fromCharCode(9, 9);
//            }
//        }
//    }

//    if (!subject && !message && !ignoreempty) {
//        return;
//    }
//    saveUserdata('forum', data);
//}

function fetchOffset(obj, mode) {
    var left_offset = 0, top_offset = 0, mode = !mode ? 0 : mode;

    if(obj.getBoundingClientRect && !mode) {
        var rect = obj.getBoundingClientRect();
        var scrollTop = Math.max(document.documentElement.scrollTop, document.body.scrollTop);
        var scrollLeft = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft);
        if(document.documentElement.dir == 'rtl') {
            scrollLeft = scrollLeft + document.documentElement.clientWidth - document.documentElement.scrollWidth;
        }
        left_offset = rect.left + scrollLeft - document.documentElement.clientLeft;
        top_offset = rect.top + scrollTop - document.documentElement.clientTop;
    }
    if(left_offset <= 0 || top_offset <= 0) {
        left_offset = obj.offsetLeft;
        top_offset = obj.offsetTop;
        while((obj = obj.offsetParent) != null) {
            position = getCurrentStyle(obj, 'position', 'position');
            if(position == 'relative') {
                continue;
            }
            left_offset += obj.offsetLeft;
            top_offset += obj.offsetTop;
        }
    }
    return {'left' : left_offset, 'top' : top_offset};
}

function getCurrentStyle(obj, cssproperty, csspropertyNS) {
    if (obj.style[cssproperty]) {
        return obj.style[cssproperty];
    }
    if (obj.currentStyle) {
        return obj.currentStyle[cssproperty];
    } else if (document.defaultView.getComputedStyle(obj, null)) {
        var currentStyle = document.defaultView.getComputedStyle(obj, null);
        var value = currentStyle.getPropertyValue(csspropertyNS);
        if (!value) {
            value = currentStyle[cssproperty];
        }
        return value;
    } else if (window.getComputedStyle) {
        var currentStyle = window.getComputedStyle(obj, "");
        return currentStyle.getPropertyValue(csspropertyNS);
    }
} 

function ebygum(eventobj) {
    if(!eventobj || is_ie) {
        window.event.cancelBubble = true;
        return window.event;
    } else {
        if(eventobj.target.type == 'submit') {
            eventobj.target.form.submit();
        }
        eventobj.stopPropagation();
        return eventobj;
    }
}

function menuoption_onclick_function(e) {
    this.clickfunc();
    hideMenu();
}

function menuoption_onclick_link(e) {
    choose(e, this);
}

function menuoption_onmouseover(e) {
    this.className = 'popupmenu_highlight';
}

function menuoption_onmouseout(e) {
    this.className = 'popupmenu_option';
}

function choose(e, obj) {
    var links = obj.getElementsByTagName('a');
    if(links[0]) {
        if(is_ie) {
            links[0].click();
            window.event.cancelBubble = true;
        } else {
            if(e.shiftKey) {
                window.open(links[0].href);
                e.stopPropagation();
                e.preventDefault();
            } else {
                window.location = links[0].href;
                e.stopPropagation();
                e.preventDefault();
            }
        }
        hideMenu();
    }
}

var Ajaxs = new Array();
var AjaxStacks = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
var attackevasive = isUndefined(attackevasive) ? 0 : attackevasive;
function Ajax(recvType, waitId) {

    for(var stackId = 0; stackId < AjaxStacks.length && AjaxStacks[stackId] != 0; stackId++);
    AjaxStacks[stackId] = 1;

    var aj = new Object();

    aj.loading = '加载中...';//public
    aj.recvType = recvType ? recvType : 'XML';//public
    aj.waitId = waitId ? $(waitId) : null;//public

    aj.resultHandle = null;//private
    aj.sendString = '';//private
    aj.targetUrl = '';//private
    aj.stackId = 0;
    aj.stackId = stackId;

    aj.setLoading = function(loading) {
        if(typeof loading !== 'undefined' && loading !== null) aj.loading = loading;
    }

    aj.setRecvType = function(recvtype) {
        aj.recvType = recvtype;
    }

    aj.setWaitId = function(waitid) {
        aj.waitId = typeof waitid == 'object' ? waitid : $(waitid);
    }

    aj.createXMLHttpRequest = function() {
        var request = false;
        if(window.XMLHttpRequest) {
            request = new XMLHttpRequest();
            if(request.overrideMimeType) {
                request.overrideMimeType('text/xml');
            }
        } else if(window.ActiveXObject) {
            var versions = ['Microsoft.XMLHTTP', 'MSXML.XMLHTTP', 'Microsoft.XMLHTTP', 'Msxml2.XMLHTTP.7.0', 'Msxml2.XMLHTTP.6.0', 'Msxml2.XMLHTTP.5.0', 'Msxml2.XMLHTTP.4.0', 'MSXML2.XMLHTTP.3.0', 'MSXML2.XMLHTTP'];
            for(var i=0; i<versions.length; i++) {
                try {
                    request = new ActiveXObject(versions[i]);
                    if(request) {
                        return request;
                    }
                } catch(e) {}
            }
        }
        return request;
    }

    aj.XMLHttpRequest = aj.createXMLHttpRequest();
    aj.showLoading = function() {
        if(aj.waitId && (aj.XMLHttpRequest.readyState != 4 || aj.XMLHttpRequest.status != 200)) {
            aj.waitId.style.display = '';
            aj.waitId.innerHTML = '<span><img src="' + IMGDIR + '/loading.gif"> ' + aj.loading + '</span>';
        }
    }

    aj.processHandle = function() {
        if(aj.XMLHttpRequest.readyState == 4 && aj.XMLHttpRequest.status == 200) {
            for(k in Ajaxs) {
                if(Ajaxs[k] == aj.targetUrl) {
                    Ajaxs[k] = null;
                }
            }
            if(aj.waitId) {
                aj.waitId.style.display = 'none';
            }
            if(aj.recvType == 'HTML') {
                aj.resultHandle(aj.XMLHttpRequest.responseText, aj);
            } else if(aj.recvType == 'XML') {
                if(aj.XMLHttpRequest.responseXML.lastChild) {
                    aj.resultHandle(aj.XMLHttpRequest.responseXML.lastChild.firstChild.nodeValue, aj);
                } else {
                    if(ajaxdebug) {
                        var error = mb_cutstr(aj.XMLHttpRequest.responseText.replace(/\r?\n/g, '\\n').replace(/"/g, '\\\"'), 200);
                        aj.resultHandle('<root>ajaxerror<script type="text/javascript" reload="1">alert(\'Ajax Error: \\n' + error + '\');</script></root>', aj);
                    }
                }
            }
            AjaxStacks[aj.stackId] = 0;
        }
    }

    aj.get = function(targetUrl, resultHandle) {

        setTimeout(function(){aj.showLoading()}, 250);
        if(in_array(targetUrl, Ajaxs)) {
            return false;
        } else {
            Ajaxs.push(targetUrl);
        }
        aj.targetUrl = targetUrl;
        aj.XMLHttpRequest.onreadystatechange = aj.processHandle;
        aj.resultHandle = resultHandle;
        var delay = attackevasive & 1 ? (aj.stackId + 1) * 1001 : 100;
        if(window.XMLHttpRequest) {
            setTimeout(function(){
            aj.XMLHttpRequest.open('GET', aj.targetUrl);
            aj.XMLHttpRequest.send(null);}, delay);
        } else {
            setTimeout(function(){
            aj.XMLHttpRequest.open("GET", targetUrl, true);
            aj.XMLHttpRequest.send();}, delay);
        }

    }
    aj.post = function(targetUrl, sendString, resultHandle) {
        setTimeout(function(){aj.showLoading()}, 250);
        if(in_array(targetUrl, Ajaxs)) {
            return false;
        } else {
            Ajaxs.push(targetUrl);
        }
        aj.targetUrl = targetUrl;
        aj.sendString = sendString;
        aj.XMLHttpRequest.onreadystatechange = aj.processHandle;
        aj.resultHandle = resultHandle;
        aj.XMLHttpRequest.open('POST', targetUrl);
        aj.XMLHttpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        aj.XMLHttpRequest.send(aj.sendString);
    }
    return aj;
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

function display(id) {
    $(id).style.display = $(id).style.display == '' ? 'none' : '';
}

function display_opacity(id, n) {
    if(!$(id)) {
        return;
    }
    if(n >= 0) {
        n -= 10;
        $(id).style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=' + n + ')';
        $(id).style.opacity = n / 100;
        setTimeout('display_opacity(\'' + id + '\',' + n + ')', 50);
    } else {
        $(id).style.display = 'none';
        $(id).style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=100)';
        $(id).style.opacity = 1;
    }
}

var evalscripts = new Array();
function evalscript(s) {
    if(s.indexOf('<script') == -1) return s;
    var p = /<script[^\>]*?>([^\x00]*?)<\/script>/ig;
    var arr = new Array();
    while(arr = p.exec(s)) {
        var p1 = /<script[^\>]*?src=\"([^\>]*?)\"[^\>]*?(reload=\"1\")?(?:charset=\"([\w\-]+?)\")?><\/script>/i;
        var arr1 = new Array();
        arr1 = p1.exec(arr[0]);
        if(arr1) {
            appendscript(arr1[1], '', arr1[2], arr1[3]);
        } else {
            p1 = /<script(.*?)>([^\x00]+?)<\/script>/i;
            arr1 = p1.exec(arr[0]);
            appendscript('', arr1[2], arr1[1].indexOf('reload=') != -1);
        }
    }
    return s;
}

function appendscript(src, text, reload, charset) {
    var id = hash(src + text);
    if(!reload && in_array(id, evalscripts)) return;
    if(reload && $(id)) {
        $(id).parentNode.removeChild($(id));
    }

    evalscripts.push(id);
    var scriptNode = document.createElement("script");
    scriptNode.type = "text/javascript";
    scriptNode.id = id;
    scriptNode.charset = charset ? charset : (is_moz ? document.characterSet : document.charset);
    try {
        if(src) {
            scriptNode.src = src;
        } else if(text){
            scriptNode.text = text;
        }
        $('append_parent').appendChild(scriptNode);
    } catch(e) {}
}

function stripscript(s) {
    return s.replace(/<script.*?>.*?<\/script>/ig, '');
}

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

/*
 *@ url: 需求请求的 url
 *@ id : 显示的 id
 *@ waitid: 等待的 id，默认为显示的 id，如果 waitid 为空字符串，则不显示 loading...， 如果为 null，则在 showid 区域显示
 *@ linkid: 是哪个链接触发的该 ajax 请求，该对象的属性(如 ajaxdisplay)保存了一些 ajax 请求过程需要的数据。
*/
/* function ajaxget(url, showid, waitid, loading, display, recall) {
    waitid = typeof waitid == 'undefined' || waitid === null ? showid : waitid;
    var x = new Ajax();
    x.setLoading(loading);
    x.setWaitId(waitid);
    x.display = typeof display == 'undefined' || display == null ? '' : display;
    x.showId = $(showid);
    if(x.showId) x.showId.orgdisplay = typeof x.showId.orgdisplay === 'undefined' ? x.showId.style.display : x.showId.orgdisplay;

    if(url.substr(strlen(url) - 1) == '#') {
        url = url.substr(0, strlen(url) - 1);
        x.autogoto = 1;
    }

    var url = url + '&inajax=1&ajaxtarget=' + showid;
    x.get(url, function(s, x) {
        evaled = false;
        if(s.indexOf('ajaxerror') != -1) {
            evalscript(s);
            evaled = true;
        }
        if(!evaled && (typeof ajaxerror == 'undefined' || !ajaxerror)) {
            if(x.showId) {
                x.showId.style.display = x.showId.orgdisplay;
                x.showId.style.display = x.display;
                x.showId.orgdisplay = x.showId.style.display;
                ajaxinnerhtml(x.showId, s);
                ajaxupdateevents(x.showId);
                if(x.autogoto) scroll(0, x.showId.offsetTop);
            }
        }

        if(!evaled)evalscript(s);
        ajaxerror = null;
        if(recall) {eval(recall);}
    });
}
 */
 
function ajaxget(url, showid, waitid, loading, display, recall) {
    
    waitid = typeof waitid == 'undefined' || waitid === null ? showid : waitid;

    var x = new Ajax();

    x.setLoading(loading);

    x.setWaitId(waitid);

    x.display = typeof display == 'undefined' || display == null ? '' : display;

    x.showId = $(showid);

    if(x.showId) x.showId.orgdisplay = typeof x.showId.orgdisplay === 'undefined' ? x.showId.style.display : x.showId.orgdisplay;



    if(url.substr(strlen(url) - 1) == '#') {

        url = url.substr(0, strlen(url) - 1);

        x.autogoto = 1;

    }



    var url = url + '&inajax=1&ajaxtarget=' + showid;

    x.get(url, function(s, x) {

        var evaled = false;

        if(s.indexOf('ajaxerror') != -1) {

            evalscript(s);

            evaled = true;

        }

        if(!evaled && (typeof ajaxerror == 'undefined' || !ajaxerror)) {

            if(x.showId) {

                x.showId.style.display = x.showId.orgdisplay;

                x.showId.style.display = x.display;

                x.showId.orgdisplay = x.showId.style.display;

                ajaxinnerhtml(x.showId, s);

                ajaxupdateevents(x.showId);

                if(x.autogoto) scroll(0, x.showId.offsetTop);

            }

        }



        ajaxerror = null;

        if(typeof recall == 'function') {

            recall();

        } else {

            eval(recall);

        }

        if(!evaled) evalscript(s);

    });

}
 
 
 
var ajaxpostHandle = 0;
/* function ajaxpost(formid, showid, waitid, showidclass, submitbtn) {
    showloading();
    var waitid = typeof waitid == 'undefined' || waitid === null ? showid : (waitid !== '' ? waitid : '');
    var showidclass = !showidclass ? '' : showidclass;

    if(ajaxpostHandle != 0) {
        return false;
    }
    var ajaxframeid = 'ajaxframe';
    var ajaxframe = $(ajaxframeid);
    if(ajaxframe == null) {
        if (is_ie && !is_opera) {
            ajaxframe = document.createElement("<iframe name='" + ajaxframeid + "' id='" + ajaxframeid + "'></iframe>");
        } else {
            ajaxframe = document.createElement("iframe");
            ajaxframe.name = ajaxframeid;
            ajaxframe.id = ajaxframeid;
        }
        ajaxframe.style.display = 'none';
        $('append_parent').appendChild(ajaxframe);

    }
    $(formid).target = ajaxframeid;
    ajaxpostHandle = [showid, ajaxframeid, formid, $(formid).target, showidclass, submitbtn];
    if(ajaxframe.attachEvent) {
        ajaxframe.detachEvent ('onload', ajaxpost_load);
        ajaxframe.attachEvent('onload', ajaxpost_load);
    } else {
        document.removeEventListener('load', ajaxpost_load, true);
        ajaxframe.addEventListener('load', ajaxpost_load, false);
    }
    $(formid).action += '&inajax=1';
    $(formid).submit();
    return false;
} */
function ajaxpost(formid, showid, waitid, showidclass, submitbtn, recall) {
    var waitid = typeof waitid == 'undefined' || waitid === null ? showid : (waitid !== '' ? waitid : '');
    var showidclass = !showidclass ? '' : showidclass;
    var ajaxframeid = 'ajaxframe';
    var ajaxframe = $(ajaxframeid);
    var formtarget = $(formid).target;

    var handleResult = function () {
        var s = '';
        var evaled = false;

        showloading('none');
        try {
            if (BROWSER.ie && BROWSER.ie != "9.0") {
                if (typeof ($(ajaxframeid).contentWindow.document.XMLDocument) != 'undefined') {
                    s = $(ajaxframeid).contentWindow.document.XMLDocument.text;
                } else {
                    s = $(ajaxframeid).contentWindow.document.documentElement.innerText.replace(/\- /g, "");
                    var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
                    xmlDoc.async = "false";
                    xmlDoc.loadXML(s);
                    root = xmlDoc.documentElement;
                    s = root.text;
                }

            } else {
                if (BROWSER.safari > 0) {
                    s = $(ajaxframeid).contentWindow.document.documentElement.firstChild.wholeText;
                } else {
                    s = $(ajaxframeid).contentWindow.document.documentElement.firstChild.nodeValue;
                }
            }
        } catch (e) {
            s = '内部错误，无法显示此内容';
        }

        if (s != '' && s.indexOf('ajaxerror') != -1) {
            evalscript(s);
            evaled = true;
        }
        if (showidclass) {
            $(showid).className = showidclass;
        }
        if (submitbtn) {
            submitbtn.disabled = false;
        }
        if (!evaled && (typeof ajaxerror == 'undefined' || !ajaxerror)) {
            ajaxinnerhtml($(showid), s);
        }
        ajaxerror = null;
        if ($(formid)) $(formid).target = formtarget;
        if (typeof recall == 'function') {
            recall();
        } else {
            eval(recall);
        }
        if (!evaled) evalscript(s);
        ajaxframe.loading = 0;
        $('append_parent').removeChild(ajaxframe.parentNode);
    };
    if(!ajaxframe) {
        var div = document.createElement('div');
        div.style.display = 'none';
        div.innerHTML = '<iframe name="' + ajaxframeid + '" id="' + ajaxframeid + '" loading="1"></iframe>';
        $('append_parent').appendChild(div);
        ajaxframe = $(ajaxframeid);
    } else if(ajaxframe.loading) {
        return false;
    }

    _attachEvent(ajaxframe, 'load', handleResult);

    showloading();
    $(formid).target = ajaxframeid;
    var action = $(formid).getAttribute('action');
    action = hostconvert(action);
    $(formid).action = action.replace(/\&inajax\=1/g, '')+'&inajax=1';
    $(formid).submit();
    if(submitbtn) {
        submitbtn.disabled = true;
    }
    doane();
    return false;
}
function hostconvert(url) {
    //if(!url.match(/^https?:\/\//)) url = SITEURL + url;
    if(!url.match(/^https?:\/\//)) url = url;
    var url_host = getHost(url);
    var cur_host = getHost().toLowerCase();
    if(url_host && cur_host != url_host) {
        url = url.replace(url_host, cur_host);
    }
    return url;
}

function getHost(url) {
    var host = "null";
    if(typeof url == "undefined"|| null == url) {
        url = window.location.href;
    }
    var regex = /.*\:\/\/([^\/]*).*/;
    var match = url.match(regex);
    if(typeof match != "undefined" && null != match) {
        host = match[1];
    }
    return host;
}


function ajaxpost_load() {
    showloading('none');
    var s = '';
    try
    {
        if(is_ie) {
            s = $(ajaxpostHandle[1]).contentWindow.document.XMLDocument.text;
        } else {
            s = $(ajaxpostHandle[1]).contentWindow.document.documentElement.firstChild.nodeValue;
        }
    } catch(e) {
        if(ajaxdebug) {
            var error = mb_cutstr($(ajaxpostHandle[1]).contentWindow.document.body.innerText.replace(/\r?\n/g, '\\n').replace(/"/g, '\\\"'), 200);
            s = '<root>ajaxerror<script type="text/javascript" reload="1">alert(\'Ajax Error: \\n' + error + '\');</script></root>';
        }
    }
    evaled = false;
    if(s != '' && s.indexOf('ajaxerror') != -1) {
        evalscript(s);
        evaled = true;
    }
    if(ajaxpostHandle[4]) {
        $(ajaxpostHandle[0]).className = ajaxpostHandle[4];
        if(ajaxpostHandle[5]) {
            ajaxpostHandle[5].disabled = false;
        }
    }
    if(!evaled && (typeof ajaxerror == 'undefined' || !ajaxerror)) {
        ajaxinnerhtml($(ajaxpostHandle[0]), s);
        if(!evaled)evalscript(s);
        setMenuPosition($(ajaxpostHandle[0]).ctrlid, 0);
        setTimeout("hideMenu()", 3000);
    }
    ajaxerror = null;
    if($(ajaxpostHandle[2])) {
        $(ajaxpostHandle[2]).target = ajaxpostHandle[3];
    }
    ajaxpostHandle = 0;
}

function ajaxmenu(e, ctrlid, timeout, func, cache, duration, ismenu, divclass, optionclass) {
    showloading();
    if(jsmenu['active'][0] && jsmenu['active'][0].ctrlkey == ctrlid) {
        hideMenu();
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
        showMenu(ctrlid, e.type == 'click', 0, duration, timeout, 0, ctrlid, 400, 1);
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
        x.optionclass = optionclass;
        x.duration = duration;
        x.timeout = timeout;
        x.get(href + '&inajax=1&ajaxmenuid='+ctrlid+'_menu', function(s) {
            evaled = false;
            if(s.indexOf('ajaxerror') != -1) {
                evalscript(s);
                evaled = true;
                if(!cache && duration != 3 && x.div.id) setTimeout('$("append_parent").removeChild($(\'' + x.div.id + '\'))', timeout);
            }
            if(!evaled && (typeof ajaxerror == 'undefined' || !ajaxerror)) {
                if(x.div) x.div.innerHTML = '<div class="' + x.optionclass + '">' + s + '</div>';
                showMenu(ctrlid, x.etype == 'click', 0, x.duration, x.timeout, 0, ctrlid, 400, 1);
                if(func) setTimeout(func + '("' + ctrlid + '")', x.timeout);
            }
            if(!evaled) evalscript(s);
            ajaxerror = null;
            showloading('none');
        });
        doane(e);
    }
}

//得到一个定长的hash值， 依赖于 stringxor()
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

function showloading(display, waiting) {
    var display = display ? display : 'block';
    var waiting = waiting ? waiting : '页面加载中...';
    $('ajaxwaitid').innerHTML = waiting;
    $('ajaxwaitid').style.display = display;
}
/*
function ajaxinnerhtml(showid, s) {
    if (showid.tagName != 'TBODY')
    {
        if (showid.tagName == "EM" && s.indexOf("script") != -1)
        {
            showid.className = "";
            showid.innerHTML += s;
        }
        else
            showid.innerHTML = s;
    } else {
        while(showid.firstChild) {
            showid.firstChild.parentNode.removeChild(showid.firstChild);
        }
        var div1 = document.createElement('DIV');
        div1.id = showid.id+'_div';
        div1.innerHTML = '<table><tbody id="'+showid.id+'_tbody">'+s+'</tbody></table>';
        $('append_parent').appendChild(div1);
        var trs = div1.getElementsByTagName('TR');
        var l = trs.length;
        for(var i=0; i<l; i++) {
            showid.appendChild(trs[0]);
        }
        var inputs = div1.getElementsByTagName('INPUT');
        var l = inputs.length;
        for(var i=0; i<l; i++) {
            showid.appendChild(inputs[0]);
        }
        div1.parentNode.removeChild(div1);
    }
}
*/

function ajaxinnerhtml(showid, s) {
    if(showid.tagName != 'TBODY') {
        showid.innerHTML = s;
    } else {
        while(showid.firstChild) {
            showid.firstChild.parentNode.removeChild(showid.firstChild);
        }
        var div1 = document.createElement('DIV');
        div1.id = showid.id+'_div';
        div1.innerHTML = '<table><tbody id="'+showid.id+'_tbody">'+s+'</tbody></table>';
        $('append_parent').appendChild(div1);
        var trs = div1.getElementsByTagName('TR');
        var l = trs.length;
        for(var i=0; i<l; i++) {
            showid.appendChild(trs[0]);
        }
        var inputs = div1.getElementsByTagName('INPUT');
        var l = inputs.length;
        for(var i=0; i<l; i++) {
            showid.appendChild(inputs[0]);
        }
        div1.parentNode.removeChild(div1);
    }
}


function AC_GetArgs(args, classid, mimeType) {
    var ret = new Object();
    ret.embedAttrs = new Object();
    ret.params = new Object();
    ret.objAttrs = new Object();
    for (var i = 0; i < args.length; i = i + 2){
        var currArg = args[i].toLowerCase();
        switch (currArg){
            case "classid":break;
            case "pluginspage":ret.embedAttrs[args[i]] = 'http://www.macromedia.com/go/getflashplayer';break;
            case "src":ret.embedAttrs[args[i]] = args[i+1];ret.params["movie"] = args[i+1];break;
            case "codebase":ret.objAttrs[args[i]] = 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0';break;
            case "onafterupdate":case "onbeforeupdate":case "onblur":case "oncellchange":case "onclick":case "ondblclick":case "ondrag":case "ondragend":
            case "ondragenter":case "ondragleave":case "ondragover":case "ondrop":case "onfinish":case "onfocus":case "onhelp":case "onmousedown":
            case "onmouseup":case "onmouseover":case "onmousemove":case "onmouseout":case "onkeypress":case "onkeydown":case "onkeyup":case "onload":
            case "onlosecapture":case "onpropertychange":case "onreadystatechange":case "onrowsdelete":case "onrowenter":case "onrowexit":case "onrowsinserted":case "onstart":
            case "onscroll":case "onbeforeeditfocus":case "onactivate":case "onbeforedeactivate":case "ondeactivate":case "type":
            case "id":ret.objAttrs[args[i]] = args[i+1];break;
            case "width":case "height":case "align":case "vspace": case "hspace":case "class":case "title":case "accesskey":case "name":
            case "tabindex":ret.embedAttrs[args[i]] = ret.objAttrs[args[i]] = args[i+1];break;
            default:ret.embedAttrs[args[i]] = ret.params[args[i]] = args[i+1];
        }
    }
    ret.objAttrs["classid"] = classid;
    if(mimeType) {
        ret.embedAttrs["type"] = mimeType;
    }
    return ret;
}

/*function AC_FL_RunContent() {
    var ret = AC_GetArgs(arguments, "clsid:d27cdb6e-ae6d-11cf-96b8-444553540000", "application/x-shockwave-flash");
    var str = '';
    if(is_ie && !is_opera) {
        str += '<object ';
        for (var i in ret.objAttrs) {
            str += i + '="' + ret.objAttrs[i] + '" ';
        }
        str += '>';
        for (var i in ret.params) {
            str += '<param name="' + i + '" value="' + ret.params[i] + '" /> ';
        }
        str += '</object>';
    } else {
        str += '<embed ';
        for (var i in ret.embedAttrs) {
            str += i + '="' + ret.embedAttrs[i] + '" ';
        }
        str += '></embed>';
    }
    return str;
}*/
function AC_FL_RunContent() {
    var str = '';
    if (AC_DetectFlashVer(9, 0, 124)) {
        var ret = AC_GetArgs(arguments, "clsid:d27cdb6e-ae6d-11cf-96b8-444553540000", "application/x-shockwave-flash");
        if (BROWSER.ie && !BROWSER.opera) {
            str += '<object ';
            for (var i in ret.objAttrs) {
                str += i + '="' + ret.objAttrs[i] + '" ';
            }
            str += '>';
            for (var i in ret.params) {
                str += '<param name="' + i + '" value="' + ret.params[i] + '" /> ';
            }
            str += '</object>';
        } else {
            str += '<embed ';
            for (var i in ret.embedAttrs) {
                str += i + '="' + ret.embedAttrs[i] + '" ';
            }
            str += '></embed>';
        }
    } else {
        str = '此内容需要 Adobe Flash Player 9.0.124 或更高版本<br /><a href="http://www.adobe.com/go/getflashplayer/" target="_blank"><img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="下载 Flash Player" /></a>';
    }
    return str;
}

function AC_DetectFlashVer(reqMajorVer, reqMinorVer, reqRevision) {
    var versionStr = -1;
    if (navigator.plugins != null && navigator.plugins.length > 0 && (navigator.plugins["Shockwave Flash 2.0"] || navigator.plugins["Shockwave Flash"])) {
        var swVer2 = navigator.plugins["Shockwave Flash 2.0"] ? " 2.0" : "";
        var flashDescription = navigator.plugins["Shockwave Flash" + swVer2].description;
        var descArray = flashDescription.split(" ");
        var tempArrayMajor = descArray[2].split(".");
        var versionMajor = tempArrayMajor[0];
        var versionMinor = tempArrayMajor[1];
        var versionRevision = descArray[3];
        if (versionRevision == "") {
            versionRevision = descArray[4];
        }
        if (versionRevision[0] == "d") {
            versionRevision = versionRevision.substring(1);
        } else if (versionRevision[0] == "r") {
            versionRevision = versionRevision.substring(1);
            if (versionRevision.indexOf("d") > 0) {
                versionRevision = versionRevision.substring(0, versionRevision.indexOf("d"));
            }
        }
        versionStr = versionMajor + "." + versionMinor + "." + versionRevision;
    } else if (BROWSER.ie && !BROWSER.opera) {
        try {
            var axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.7");
            versionStr = axo.GetVariable("$version");
        } catch (e) { }
    }
    if (versionStr == -1) {
        return false;
    } else if (versionStr != 0) {
        if (BROWSER.ie && !BROWSER.opera) {
            tempArray = versionStr.split(" ");
            tempString = tempArray[1];
            versionArray = tempString.split(",");
        } else {
            versionArray = versionStr.split(".");
        }
        var versionMajor = versionArray[0];
        var versionMinor = versionArray[1];
        var versionRevision = versionArray[2];
        return versionMajor > parseFloat(reqMajorVer) || (versionMajor == parseFloat(reqMajorVer)) && (versionMinor > parseFloat(reqMinorVer) || versionMinor == parseFloat(reqMinorVer) && versionRevision >= parseFloat(reqRevision));
    }
}

//PageScroll
function pagescroll_class(obj, pagewidth, pageheight) {
    this.ctrlobj = $(obj);
    this.speed = 2;
    this.pagewidth = pagewidth;
    this.times = 1;
    this.pageheight = pageheight;
    this.running = 0;
    this.defaultleft = 0;
    this.defaulttop = 0;
    this.script = '';
    this.start = function(times) {
        if(this.running) return 0;
        this.times = !times ? 1 : times;
        this.scrollpx = 0;
        return this.running = 1;
    }
    this.left = function(times, script) {
        if(!this.start(times)) return;
        this.stepv = -(this.step = this.pagewidth * this.times / this.speed);
        this.script = !script ? '' : script;
        setTimeout('pagescroll.h()', 1);
    }
    this.right = function(times, script) {
        if(!this.start(times)) return;
        this.stepv = this.step = this.pagewidth * this.times / this.speed;
        this.script = !script ? '' : script;
        setTimeout('pagescroll.h()', 1);
    }
    this.up = function(times, script) {
        if(!this.start(times)) return;
        this.stepv = -(this.step = this.pageheight * this.times / this.speed);
        this.script = !script ? '' : script;
        setTimeout('pagescroll.v()', 1);
    }
    this.down = function(times, script) {
        if(!this.start(times)) return;
        this.stepv = this.step = this.pageheight * this.times / this.speed;
        this.script = !script ? '' : script;
        setTimeout('pagescroll.v()', 1);
    }
    this.h = function() {
        if(this.scrollpx <= this.pagewidth * this.times) {
            this.scrollpx += Math.abs(this.stepv);
            patch = this.scrollpx > this.pagewidth * this.times ? this.scrollpx - this.pagewidth * this.times : 0;
            patch = patch > 0 && this.stepv < 0 ? -patch : patch;
            oldscrollLeft = this.ctrlobj.scrollLeft;
            this.ctrlobj.scrollLeft = this.ctrlobj.scrollLeft + this.stepv - patch;
            if(oldscrollLeft != this.ctrlobj.scrollLeft) {
                setTimeout('pagescroll.h()', 1);
                return;
            }
        }
        if(this.script) {
            eval(this.script);
        }
        this.running = 0;
    }
    this.v = function() {
        if(this.scrollpx <= this.pageheight * this.times) {
            this.scrollpx += Math.abs(this.stepv);
            patch = this.scrollpx > this.pageheight * this.times ? this.scrollpx - this.pageheight * this.times : 0;
            patch = patch > 0 && this.stepv < 0 ? -patch : patch;
            oldscrollTop = this.ctrlobj.scrollTop;
            this.ctrlobj.scrollTop = this.ctrlobj.scrollTop + this.stepv - patch;
            if(oldscrollTop != this.ctrlobj.scrollTop) {
                setTimeout('pagescroll.v()', 1);
                return;
            }
        }
        if(this.script) {
            eval(this.script);
        }
        this.running = 0;
    }
    this.init = function() {
        this.ctrlobj.scrollLeft = this.defaultleft;
        this.ctrlobj.scrollTop = this.defaulttop;
    }

}

//LiSelect
var selectopen = null;
var hiddencheckstatus = 0;
function loadselect(id, showinput, pageobj, pos, method) {
    var obj = $(id);
    var objname = $(id).name;
    var objoffset = fetchOffset(obj);
    objoffset['width'] = is_ie ? (obj.offsetWidth ? obj.offsetWidth : parseInt(obj.currentStyle.width)) : obj.offsetWidth;
    objoffset['height'] = is_ie ? (obj.offsetHeight ? obj.offsetHeight : parseInt(obj.currentStyle.height)) : obj.offsetHeight;
    pageobj = !pageobj ? '' : pageobj;
    showinput = !showinput ? 0 : showinput;
    pos = !pos ? 0 : 1;
    method = !method ? 0 : 1;
    var maxlength = 0;
    var defaultopt = '', defaultv = '';
    var lis = '<ul onfocus="loadselect_keyinit(event, 1)" onblur="loadselect_keyinit(event, 2)" class="newselect" id="' + objname + '_selectmenu" style="' + (!pos ? 'z-index:999;position: absolute; width: ' + objoffset['width'] + 'px;' : '') + 'display: none">';
    for(var i = 0;i < obj.options.length;i++){
        lis += '<li ' + (obj.options[i].selected ? 'class="current" ' : '') + 'k_id="' + id + '" k_value="' + obj.options[i].value + '" onclick="loadselect_liset(\'' + objname + '\', ' + showinput + ', \'' + id + '\',' + (showinput ? 'this.innerHTML' : '\'' + obj.options[i].value + '\'') + ',this.innerHTML, ' + i + ')">' + obj.options[i].innerHTML + '</li>';
        maxlength = obj.options[i].value.length > maxlength ? obj.options[i].value.length : maxlength;
        if(obj.options[i].selected) {
            defaultopt = obj.options[i].innerHTML;
            defaultv = obj.options[i].value;
            if($(objname)) {
                $(objname).setAttribute('selecti', i);
            }
        }
    }
    lis += '</ul>';
    if(showinput) {
        inp = '<input autocomplete="off" class="newselect" id="' + objname + '_selectinput" onclick="loadselect_viewmenu(this, \'' + objname + '\', 0, \'' + pageobj + '\');doane(event)" onchange="loadselect_inputset(\'' + id + '\', this.value);loadselect_viewmenu(this, \'' + objname + '\', 0, \'' + pageobj + '\')" value="' + defaultopt + '" style="width: ' + objoffset['width'] + 'px;height: ' + objoffset['height'] + 'px;" tabindex="1" />';
    } else {
        inp = '<a href="javascript:;" hidefocus="true" class="loadselect" id="' + objname + '_selectinput"' + (!obj.disabled ? ' onfocus="loadselect_keyinit(event, 1)" onblur="loadselect_keyinit(event, 2)" onmouseover="this.focus()" onmouseout="this.blur()" onkeyup="loadselect_key(this, event, \'' + objname + '\', \'' + pageobj + '\')" onclick="loadselect_viewmenu(this, \'' + objname + '\', 0, \'' + pageobj + '\');doane(event)"' : '') + ' tabindex="1">' + defaultopt + '</a>';
    }
    obj.options.length = 0;
    if(defaultopt) {
        obj.options[0]= showinput ? new Option('', defaultopt) : new Option('', defaultv);
    }
    obj.style.width = objoffset['width'] + 'px';
    obj.style.display = 'none';
    if(!method) {
        obj.outerHTML += inp + lis;
    } else {
        if(showinput) {
            var inpobj = document.createElement("input");
        } else {
            var inpobj = document.createElement("a");
        }
        obj.parentNode.appendChild(inpobj);
        inpobj.outerHTML = inp;
        var lisobj = document.createElement("ul");
        obj.parentNode.appendChild(lisobj);
        lisobj.outerHTML = lis;
    }
}

function loadselect_keyinit(e, a) {
    if(a == 1) {
        if(document.attachEvent) {
            document.body.attachEvent('onkeydown', loadselect_keyhandle);
        } else {
            document.body.addEventListener('keydown', loadselect_keyhandle, false);
        }
    } else {
        if(document.attachEvent) {
            document.body.detachEvent('onkeydown', loadselect_keyhandle);
        } else {
            document.body.removeEventListener('keydown', loadselect_keyhandle, false);
        }
    }
}

function loadselect_keyhandle(e) {
    e = is_ie ? event : e;
    if(e.keyCode == 40 || e.keyCode == 38) doane(e);
}

function loadselect_key(ctrlobj, e, objname, pageobj) {
    value = e.keyCode;
    if(value == 40 || value == 38) {
        if($(objname + '_selectmenu').style.display == 'none') {
            loadselect_viewmenu(ctrlobj, objname, 0, pageobj);
        } else {
            lis = $(objname + '_selectmenu').getElementsByTagName('LI');
            selecti = $(objname).getAttribute('selecti');
            lis[selecti].className = '';
            if(value == 40) {
                selecti = parseInt(selecti) + 1;
            } else if(value == 38) {
                selecti = parseInt(selecti) - 1;
            }
            if(selecti < 0) {
                selecti = lis.length - 1
            } else if(selecti > lis.length - 1) {
                selecti = 0;
            }
            lis[selecti].className = 'current';
            $(objname).setAttribute('selecti', selecti);
            lis[selecti].parentNode.scrollTop = lis[selecti].offsetTop;
        }
    } else if(value == 13) {
        lis = $(objname + '_selectmenu').getElementsByTagName('LI');
        for(i = 0;i < lis.length;i++) {
            if(lis[i].className == 'current') {
                loadselect_liset(objname, 0, lis[i].getAttribute('k_id'), lis[i].getAttribute('k_value'), lis[i].innerHTML, i);
                break;
            }
        }
    }
}

function loadselect_viewmenu(ctrlobj, objname, hidden, pageobj) {
    if(!selectopen) {
        if(document.attachEvent) {
            document.body.attachEvent('onclick', loadselect_hiddencheck);
        } else {
            document.body.addEventListener('click', loadselect_hiddencheck, false);
        }
    }
    var hidden = !hidden ? 0 : 1;
    if($(objname + '_selectmenu').style.display == '' || hidden) {
        $(objname + '_selectmenu').style.display = 'none';
    } else {
        if($(selectopen)) {
            $(selectopen).style.display = 'none';
        }
        var objoffset = fetchOffset(ctrlobj);
        if(pageobj) {
            var InFloate = pageobj.split('_');
            objoffset['left'] -= $(pageobj).scrollLeft + parseInt(floatwinhandle[InFloate[1] + '_1']);
            objoffset['top'] -= $(pageobj).scrollTop + parseInt(floatwinhandle[InFloate[1] + '_2']);
        }
        objoffset['height'] = ctrlobj.offsetHeight;
        $(objname + '_selectmenu').style.display = '';
        selectopen = objname + '_selectmenu';
    }
    hiddencheckstatus = 1;
}

function loadselect_hiddencheck() {
    if(hiddencheckstatus) {
        if($(selectopen)) {
            $(selectopen).style.display = 'none';
        }
        hiddencheckstatus = 0;
    }
}

function loadselect_liset(objname, showinput, obj, v, opt, selecti) {
    var change = 1;
    if(showinput) {
        if($(objname + '_selectinput').value != opt) {
            $(objname + '_selectinput').value = opt;
        } else {
            change = 0;
        }
    } else {
        if($(objname + '_selectinput').innerHTML != opt) {
            $(objname + '_selectinput').innerHTML = opt;
        } else {
            change = 0;
        }
    }
    lis = $(objname + '_selectmenu').getElementsByTagName('LI');
    lis[$(objname).getAttribute('selecti')].className = '';
    lis[selecti].className = 'current';
    $(objname).setAttribute('selecti', selecti);
    $(objname + '_selectmenu').style.display='none';
    if(change) {
        obj = $(obj);
        obj.options.length=0;
        obj.options[0]=new Option('', v);
        eval(obj.getAttribute('change'));
    }
}

function loadselect_inputset(obj, v) {
    obj = $(obj);
    obj.options.length=0;
    obj.options[0]=new Option('', v);
    eval(obj.getAttribute('change'));
}

//DetectCapsLock
var detectobj;
function detectcapslock(e, obj) {
    detectobj = obj;
    valueCapsLock = e.keyCode ? e.keyCode : e.which;
    valueShift = e.shiftKey ? e.shiftKey : (valueCapsLock == 16 ? true : false);
    detectobj.className = (valueCapsLock >= 65 && valueCapsLock <= 90 && !valueShift || valueCapsLock >= 97 && valueCapsLock <= 122 && valueShift) ? 'capslock txt' : 'txt';
    if(is_ie) {
        event.srcElement.onblur = detectcapslock_cleardetectobj;
    } else {
        e.target.onblur = detectcapslock_cleardetectobj;
    }
}

function detectcapslock_cleardetectobj() {
    detectobj.className = 'txt';
}

//FloatWin
var hiddenobj = new Array();
var floatwinhandle = new Array();
var floatscripthandle = new Array();
var floattabs = new Array();
var floatwins = new Array();
var InFloat = '';
var floatwinreset = 0;
var floatwinopened = 0;
//var allowfloatwin = 1;
var STYLEID = '1';
var VERHASH = 'Pvt'
function floatwin(action, script, w, h, scrollpos) {
    var floatonly = !floatonly ? 0 : 1;
    var actione = action.split('_');
    action = actione[0];
    if(script && script != -1)
        script += script.indexOf("?") != -1 ? "&stamp=" + Math.random() : "?stamp=" + Math.random();
    if((!allowfloatwin || allowfloatwin == 0) && action == 'open' && in_array(actione[1], ['register','login','newthread','reply','edit']) && w >= 600) {
        location.href = script;
        return;
    }
    var handlekey = actione[1];
    var layerid = 'floatwin_' + handlekey;
    if(is_ie) {
        var objs = $('wrap').getElementsByTagName("OBJECT");
    } else {
        var objs = $('wrap').getElementsByTagName("EMBED");
    }
    if(action == 'open') {
        loadcss('float');
        floatwinhandle[handlekey + '_0'] = layerid;
        if(!floatwinopened) {
            $('wrap').onkeydown = floatwin_wrapkeyhandle;
            for(i = 0;i < objs.length; i ++) {
                if(objs[i].style.visibility != 'hidden') {
                    objs[i].setAttribute("oldvisibility", objs[i].style.visibility);
                    objs[i].style.visibility = 'hidden';
                }
            }
        }
        scrollpos = !scrollpos ? '' : 'floatwin_scroll(\'' + scrollpos + '\');';
        var clientWidth = document.body.clientWidth;
        var clientHeight = document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight;
        var scrollTop = document.body.scrollTop ? document.body.scrollTop : document.documentElement.scrollTop;
        if(script && script != -1) {
            if(script.lastIndexOf('/') != -1) {
                script = script.substr(script.lastIndexOf('/') + 1);
            }
            var scriptfile = script.split('?');
            scriptfile = scriptfile[0];
            if(in_array(scriptfile,['posttopic.aspx','postreply.aspx','editpost.aspx']))
            {
            scriptfile='post.aspx';
            }
            if(floatwinreset || floatscripthandle[scriptfile] && floatscripthandle[scriptfile][0] != script) {
                if(!isUndefined(floatscripthandle[scriptfile])) {
                    $('append_parent').removeChild($(floatscripthandle[scriptfile][1]));
                    $('append_parent').removeChild($(floatscripthandle[scriptfile][1] + '_mask'));
                }
                floatwinreset = 0;
            }
            floatscripthandle[scriptfile] = [script, layerid];
        }
        if(!$(layerid)) {
            floattabs[layerid] = new Array();
            div = document.createElement('div');
            div.className = 'floatwin';
            div.id = layerid;
            div.style.width = w + 'px';
            div.style.height = h + 'px';
            div.style.left = floatwinhandle[handlekey + '_1'] = ((clientWidth - w) / 2) + 'px';
            div.style.position = 'absolute';
            div.style.zIndex = '10002';
            div.onkeydown = floatwin_keyhandle;
            $('append_parent').appendChild(div);
            $(layerid).style.display = '';
            $(layerid).style.top = floatwinhandle[handlekey + '_2'] = ((clientHeight - h) / 2 + scrollTop) + 'px';
            $(layerid).innerHTML = '<div><h3 class="float_ctrl"><em><img src="' + IMGDIR + '/loading.gif"> 加载中...</em><span><a href="javascript:;" class="float_close" onclick="floatwinreset = 1;floatwin(\'close_' + handlekey + '\');">&nbsp</a></span></h3></div>';
            divmask = document.createElement('div');
            divmask.className = 'floatwinmask';
            divmask.id = layerid + '_mask';
            divmask.style.width = (parseInt($(layerid).style.width) + 14) + 'px';
            divmask.style.height = (parseInt($(layerid).style.height) + 14) + 'px';
            divmask.style.left = (parseInt($(layerid).style.left) - 6) + 'px';
            divmask.style.top = (parseInt($(layerid).style.top) - 6) + 'px';
            divmask.style.position = 'absolute';
            divmask.style.zIndex = '10001';
            divmask.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=90,finishOpacity=100,style=0)';
            divmask.style.opacity = 0.9;
            $('append_parent').appendChild(divmask);
            if(script && script != -1) {
                script += (script.search(/\?/) > 0 ? '&' : '?') + 'infloat=1&handlekey=' + handlekey;
                try {
                    ajaxget(rooturl+script, layerid, '', '', '', scrollpos);
                } catch(e) {
                    setTimeout("ajaxget('" + (rooturl+script) + "', '" + layerid + "', '', '', '', '" + scrollpos + "')", 1000);
                }
            } else if(script == -1) {
                $(layerid).innerHTML = '<div><h3 class="float_ctrl"><em id="' + layerid + '_title"></em><span><a href="javascript:;" class="float_close" onclick="floatwinreset = 1;floatwin(\'close_' + handlekey + '\');">&nbsp</a></span></h3></div><div id="' + layerid + '_content"></div>';
                $(layerid).style.zIndex = '1099';
                $(layerid + '_mask').style.zIndex = '1098';
            }
        } else {
            $(layerid).style.width = w + 'px';
            $(layerid).style.height = h + 'px';
            $(layerid).style.display = '';
            $(layerid).style.top = floatwinhandle[handlekey + '_2'] = ((clientHeight - h) / 2 + scrollTop) + 'px';
            $(layerid + '_mask').style.width = (parseInt($(layerid).style.width) + 14) + 'px';
            $(layerid + '_mask').style.height = (parseInt($(layerid).style.height) + 14) + 'px';
            $(layerid + '_mask').style.display = '';
            $(layerid + '_mask').style.top = (parseInt($(layerid).style.top) - 6) + 'px';
        }
        floatwins[floatwinopened] = handlekey;
        floatwinopened++;
        
    } else if(action == 'close' && floatwinhandle[handlekey + '_0']) {
        floatwinopened--;
        for(i = 0;i < floatwins.length; i++) {
            if(handlekey == floatwins[i]) {
                floatwins[i] = null;
            }
        }
        if(!floatwinopened) {
            for(i = 0;i < objs.length; i ++) {
                if(objs[i].attributes['oldvisibility']) {
                    objs[i].style.visibility = objs[i].attributes['oldvisibility'].nodeValue;
                    objs[i].removeAttribute('oldvisibility');
                }
            }
            $('wrap').onkeydown = null;
        }
        hiddenobj = new Array();
        $(layerid + '_mask').style.display = 'none';
        $(layerid).style.display = 'none';
    } else if(action == 'size' && floatwinhandle[handlekey + '_0']) {
        if(!floatwinhandle[handlekey + '_3']) {
            var clientWidth = document.body.clientWidth;
            var clientHeight = document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight;
            var w = clientWidth > 800 ? clientWidth * 0.9 : 800;
            var h = clientHeight * 0.9;
            floatwinhandle[handlekey + '_3'] = $(layerid).style.left;
            floatwinhandle[handlekey + '_4'] = $(layerid).style.top;
            floatwinhandle[handlekey + '_5'] = $(layerid).style.width;
            floatwinhandle[handlekey + '_6'] = $(layerid).style.height;
            $(layerid).style.left = floatwinhandle[handlekey + '_1'] = ((clientWidth - w) / 2) + 'px';
            $(layerid).style.top = floatwinhandle[handlekey + '_2'] = ((document.documentElement.clientHeight - h) / 2 + document.documentElement.scrollTop) + 'px';
            $(layerid).style.width = w + 'px';
            $(layerid).style.height = h + 'px';
        } else {
            $(layerid).style.left = floatwinhandle[handlekey + '_1'] = floatwinhandle[handlekey + '_3'];
            $(layerid).style.top = floatwinhandle[handlekey + '_2'] = floatwinhandle[handlekey + '_4'];
            $(layerid).style.width = floatwinhandle[handlekey + '_5'];
            $(layerid).style.height = floatwinhandle[handlekey + '_6'];
            floatwinhandle[handlekey + '_3'] = '';
        }
        $(layerid + '_mask').style.width = (parseInt($(layerid).style.width) + 14) + 'px';
        $(layerid + '_mask').style.height = (parseInt($(layerid).style.height) + 14) + 'px';
        $(layerid + '_mask').style.left = (parseInt($(layerid).style.left) - 6) + 'px';
        $(layerid + '_mask').style.top = (parseInt($(layerid).style.top) - 6) + 'px';
    }
}

function floatwin_scroll(pos) {
    var pose = pos.split(',');
    try {
        pagescroll.defaultleft = pose[0];
        pagescroll.defaulttop = pose[1];
        pagescroll.init();
    } catch(e) {}
}

function floatwin_wrapkeyhandle(e) {
    e = is_ie ? event : e;
    if(e.keyCode == 9) {
        doane(e);
    } else if(e.keyCode == 27) {
        for(i = floatwins.length - 1;i >= 0; i--) {
            floatwin('close_' + floatwins[i]);
        }
    }
}

function floatwin_keyhandle(e) {
    e = is_ie ? event : e;
    if(e.keyCode == 9) {
        doane(e);
        var obj = is_ie ? e.srcElement : e.target;
        var srcobj = obj;
        j = 0;
        while(obj.className.indexOf('floatbox') == -1) {
            obj = obj.parentNode;
        }
        obj.id = obj.id ? obj.id : 'floatbox_' + Math.random();
        if(!floattabs[obj.id]) {
            floattabs[obj.id] = new Array();
            var alls = obj.getElementsByTagName("*");
            for(i = 0;i < alls.length;i++) {
                if(alls[i].getAttribute('tabindex') == 1) {
                    floattabs[obj.id][j] = alls[i];
                    j++;
                }
            }
        }
        if(floattabs[obj.id].length > 0) {
            for(i = 0;i < floattabs[obj.id].length;i++) {
                if(srcobj == floattabs[obj.id][i]) {
                    j = e.shiftKey ? i - 1 : i + 1;break;
                }
            }
            if(j < 0) {
                j = floattabs[obj.id].length - 1;
            }
            if(j > floattabs[obj.id].length - 1) {
                j = 0;
            }
            do{
                focusok = 1;
                try{ floattabs[obj.id][j].focus(); } catch(e) {
                    focusok = 0;
                }
                if(!focusok) {
                    j = e.shiftKey ? j - 1 : j + 1;
                    if(j < 0) {
                        j = floattabs[obj.id].length - 1;
                    }
                    if(j > floattabs[obj.id].length - 1) {
                        j = 0;
                    }
                }
            } while(!focusok);
        }
    }
}

//ShowSelect
function showselect(obj, inpid, t, rettype) {
    if(!obj.id) {
        var t = !t ? 0 : t;
        var rettype = !rettype ? 0 : rettype;
        obj.id = 'calendarexp_' + Math.random();
        div = document.createElement('div');
        div.id = obj.id + '_menu';
        div.style.display = 'none';
        div.className = 'showselect_menu';
        if($(InFloat) != null) {
            $(InFloat).appendChild(div);
        } else {
            $('append_parent').appendChild(div);
        }
        s = '';
        if(!t) {
            s += showselect_row(inpid, '一天', 1, 0, rettype);
            s += showselect_row(inpid, '一周', 7, 0, rettype);
            s += showselect_row(inpid, '一个月', 30, 0, rettype);
            s += showselect_row(inpid, '三个月', 90, 0, rettype);
            s += showselect_row(inpid, '自定义', -2);
        } else {
            if($(t)) {
                var lis = $(t).getElementsByTagName('LI');
                for(i = 0;i < lis.length;i++) {
                    s += '<a href="javascript:;" onclick="$(\'' + inpid + '\').value = this.innerHTML">' + lis[i].innerHTML + '</a><br />';
                }
                s += showselect_row(inpid, '自定义', -1);
            } else {
                s += '<a href="javascript:;" onclick="$(\'' + inpid + '\').value = \'0\'">永久</a><br />';
                s += showselect_row(inpid, '7 天', 7, 1, rettype);
                s += showselect_row(inpid, '14 天', 14, 1, rettype);
                s += showselect_row(inpid, '一个月', 30, 1, rettype);
                s += showselect_row(inpid, '三个月', 90, 1, rettype);
                s += showselect_row(inpid, '半年', 182, 1, rettype);
                s += showselect_row(inpid, '一年', 365, 1, rettype);
                s += showselect_row(inpid, '自定义', -1);
            }
        }
        $(div.id).innerHTML = s;
    }
    if (inpid.indexOf("reason") != -1)
        div.style.width = "63px";
    showMenu(obj.id);
    if(is_ie && is_ie < 7) {
        doane(event);
    }
}

function showselect_row(inpid, s, v, notime, rettype) {
    if(v >= 0) {
        if(!rettype) {
            var notime = !notime ? 0 : 1;
            t = today.getTime();
            t += 86400000 * v;
            d = new Date();
            d.setTime(t);
            return '<a href="javascript:;" onclick="$(\'' + inpid + '\').value = \'' + d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate() + (!notime ? ' ' + d.getHours() + ':' + d.getMinutes() : '') + '\'">' + s + '</a><br />';
        } else {
            return '<a href="javascript:;" onclick="$(\'' + inpid + '\').value = \'' + v + '\'">' + s + '</a><br />';
        }
    } else if(v == -1) {
        return '<a href="javascript:;" onclick="$(\'' + inpid + '\').focus()">' + s + '</a><br />';
    } else if(v == -2) {
        return '<a href="javascript:;" onclick="$(\'' + inpid + '\').onclick()">' + s + '</a><br />';
    }
}

//Smilies
function smilies_show(id, smcols, method, seditorkey) {
//showMenu({'ctrlid':'e_sml_menu','evt':'click','timeout':250,'duration':3 ,'drag':in_array(tag, ['attach', 'image']) ? 'e_sml' + '_ctrl' : 1});
    /* if(seditorkey && !$(seditorkey + 'smilies_menu')) {
        var div = document.createElement("div");
        div.id = seditorkey + 'smilies_menu';
        div.style.display = 'none';
        div.className = 'smilieslist';
        $('append_parent').appendChild(div);
        var div = document.createElement("div");
        div.id = id;
        div.style.overflow = 'hidden';
        $(seditorkey + 'smilies_menu').appendChild(div);
    }
    if(typeof smilies_type == 'undefined') {
        var scriptNode = document.createElement("script");
        scriptNode.type = "text/javascript";
        scriptNode.charset = charset ? charset : (is_moz ? document.characterSet : document.charset);
        scriptNode.src = 'forumdata/cache/smilies_var.js?' + VERHASH;
        $('append_parent').appendChild(scriptNode);
        if(is_ie) {
            scriptNode.onreadystatechange = function() {
                smilies_onload(id, smcols, method, seditorkey);
            }
        } else {
            scriptNode.onload = function() {
                smilies_onload(id, smcols, method, seditorkey);
            }
        }
    } else {
        smilies_onload(id, smcols, method, seditorkey);
    } */
}

var currentstype = null;
function smilies_onload(id, smcols, method, seditorkey) {
    seditorkey = !seditorkey ? '' : seditorkey;
    smile = getcookie('smile').split('D');
    if(typeof smilies_type == 'object') {
        if(smile[0] && smilies_array[smile[0]]) {
            currentstype = smile[0];
        } else {
            for(i in smilies_array) {
                currentstype = i; break;
            }
        }
        smiliestype = '<div class="smiliesgroup" style="margin-right: 0"><ul>';
        for(i in smilies_type) {
            if(smilies_type[i][0]) {
                smiliestype += '<li><a href="javascript:;" hidefocus="true" ' + (currentstype == i ? 'class="current"' : '') + ' id="stype_'+method+'_'+i+'" onclick="smilies_switch(\'' + id + '\', \'' + smcols + '\', '+i+', 1, ' + method + ', \'' + seditorkey + '\');if(currentstype) {$(\'stype_'+method+'_\'+currentstype).className=\'\';}this.className=\'current\';currentstype='+i+';">'+smilies_type[i][0]+'</a></li>';
            }
        }
        smiliestype += '</ul></div>';
        $(id).innerHTML = smiliestype + '<div style="clear: both" class="float_typeid" id="' + id + '_data"></div><table class="smilieslist_table" id="' + id + '_preview_table" style="display: none"><tr><td class="smilieslist_preview" id="' + id + '_preview"></td></tr></table><div style="clear: both" class="smilieslist_page" id="' + id + '_page"></div>';
        smilies_switch(id, smcols, currentstype, smile[1], method, seditorkey);
    }
}

function smilies_switch(id, smcols, type, page, method, seditorkey) {
    page = page ? page : 1;
    if(!smilies_array[type] || !smilies_array[type][page]) return;
    setcookie('smile', type + 'D' + page, 31536000);
    smiliesdata = '<table id="' + id + '_table" cellpadding="0" cellspacing="0" style="clear: both"><tr>';
    j = k = 0;
    img = new Array();
    for(i in smilies_array[type][page]) {
        if(j >= smcols) {
            smiliesdata += '<tr>';
            j = 0;
        }
        s = smilies_array[type][page][i];
        smilieimg = 'images/smilies/' + smilies_type[type][1] + '/' + s[2];
        img[k] = new Image();
        img[k].src = smilieimg;
        smiliesdata += s && s[0] ? '<td onmouseover="smilies_preview(\'' + id + '\', this, ' + s[5] + ')" onmouseout="smilies_preview(\'' + id + '\')" onclick="' + (method ? 'insertSmiley(' + s[0] + ')': 'seditor_insertunit(\'' + seditorkey + '\', \'' + s[1].replace(/'/, '\\\'') + '\')') +
            '"><img id="smilie_' + s[0] + '" width="' + s[3] +'" height="' + s[4] +'" src="' + smilieimg + '" alt="' + s[1] + '" />' : '<td>';
        j++;k++;
    }
    smiliesdata += '</table>';
    smiliespage = '';
    if(smilies_array[type].length > 2) {
        prevpage = ((prevpage = parseInt(page) - 1) < 1) ? smilies_array[type].length - 1 : prevpage;
        nextpage = ((nextpage = parseInt(page) + 1) == smilies_array[type].length) ? 1 : nextpage;
        smiliespage = '<div class="pags_act"><a href="javascript:;" onclick="smilies_switch(\'' + id + '\', \'' + smcols + '\', ' + type + ', ' + prevpage + ', ' + method + ', \'' + seditorkey + '\')">上页</a>' +
            '<a href="javascript:;" onclick="smilies_switch(\'' + id + '\', \'' + smcols + '\', ' + type + ', ' + nextpage + ', ' + method + ', \'' + seditorkey + '\')">下页</a></div>' +
            page + '/' + (smilies_array[type].length - 1);
    }
    $(id + '_data').innerHTML = smiliesdata;
    $(id + '_page').innerHTML = smiliespage;
}

function smilies_preview(id, obj, v) {
    if(!obj) {
        $(id + '_preview_table').style.display = 'none';
    } else {
        $(id + '_preview_table').style.display = '';
        $(id + '_preview').innerHTML = '<img width="' + v + '" src="' + obj.childNodes[0].src + '" />';
    }
}

//SEditor
function seditor_ctlent(event, script,editorkey) {
    if(postSubmited == false && (event.ctrlKey && event.keyCode == 13) || (event.altKey && event.keyCode == 83) && $(editorkey+'submit'))
    {
        if (eval(script))
        {
            postSubmited = true;
            $(editorkey+'submit').disabled = true;
            $(editorkey+'form').submit(); 
        }
    }
}

function parseurl(str, mode, parsecode) {
    if(!parsecode) str= str.replace(/\s*\[code\]([\s\S]+?)\[\/code\]\s*/ig, function($1, $2) {return codetag($2);});
    str = str.replace(/([^>=\]"'\/]|^)((((https?|ftp):\/\/)|www\.)([\w\-]+\.)*[\w\-\u4e00-\u9fa5]+\.([\.a-zA-Z0-9]+|\u4E2D\u56FD|\u7F51\u7EDC|\u516C\u53F8)((\?|\/|:)+[\w\.\/=\?%\-&~`@':+!]*)+\.(jpg|gif|png|bmp))/ig, mode == 'html' ? '$1<img src="$2" border="0">' : '$1[img]$2[/img]');
    str = str.replace(/([^>=\]"'\/@]|^)((((https?|ftp|gopher|news|telnet|rtsp|mms|callto|bctp|ed2k|thunder|synacast):\/\/))([\w\-]+\.)*[:\.@\-\w\u4e00-\u9fa5]+\.([\.a-zA-Z0-9]+|\u4E2D\u56FD|\u7F51\u7EDC|\u516C\u53F8)((\?|\/|:)+[\w\.\/=\?%\-&~`@':+!#]*)*)/ig, mode == 'html' ? '$1<a href="$2" target="_blank">$2</a>' : '$1[url]$2[/url]');
    str = str.replace(/([^\w>=\]"'\/@]|^)((www\.)([\w\-]+\.)*[:\.@\-\w\u4e00-\u9fa5]+\.([\.a-zA-Z0-9]+|\u4E2D\u56FD|\u7F51\u7EDC|\u516C\u53F8)((\?|\/|:)+[\w\.\/=\?%\-&~`@':+!#]*)*)/ig, mode == 'html' ? '$1<a href="$2" target="_blank">$2</a>' : '$1[url]$2[/url]');
    str = str.replace(/([^\w->=\]:"'\.\/]|^)(([\-\.\w]+@[\.\-\w]+(\.\w+)+))/ig, mode == 'html' ? '$1<a href="mailto:$2">$2</a>' : '$1[email]$2[/email]');
    if(!parsecode) {
        for(var i = 0; i <= codecount; i++) {
            str = str.replace("[\tBBX_CODE_" + i + "\t]", codehtml[i]);
        }
    }
    return str;
}

/* function codetag(text) {
    codecount++;
    if(typeof wysiwyg != 'undefined' && wysiwyg) text = text.replace(/<br[^\>]*>/ig, '\n').replace(/<(\/|)[A-Za-z].*?>/ig, '');
    codehtml[codecount] = '[code]' + text + '[/code]';
    return '[\tBBX_CODE_' + codecount + '\t]';
} */


function codetag(text) {
    BBXCODE['num']++;
    if(typeof wysiwyg != 'undefined' && wysiwyg) text = text.replace(/<br[^\>]*>/ig, '\n').replace(/<(\/|)[A-Za-z].*?>/ig, '');
    BBXCODE['html'][BBXCODE['num']] = '[code]' + text + '[/code]';
    return '[\tBBX_CODE_' + BBXCODE['num'] + '\t]';
}

function seditor_insertunit(key, text, textend, moveend) {
    $(key + 'message').focus();
    textend = isUndefined(textend) ? '' : textend;
    moveend = isUndefined(textend) ? 0 : moveend;
    startlen = strlen(text);
    endlen = strlen(textend);
    if (!isUndefined($(key + 'message').selectionStart)) {
        var opn = $(key + 'message').selectionStart + 0;
        if (textend != '') {
            text = text + $(key + 'message').value.substring($(key + 'message').selectionStart, $(key + 'message').selectionEnd) + textend;
        }
        $(key + 'message').value = $(key + 'message').value.substr(0, $(key + 'message').selectionStart) + text + $(key + 'message').value.substr($(key + 'message').selectionEnd);
        if (!moveend) {
            $(key + 'message').selectionStart = opn + strlen(text) - endlen;
            $(key + 'message').selectionEnd = opn + strlen(text) - endlen;
        }
    } else if (document.selection && document.selection.createRange) {
        var sel = document.selection.createRange();
        if (textend != '') {
            text = text + sel.text + textend;
        }
        sel.text = text.replace(/\r?\n/g, '\r\n');
        if (!moveend) {
            sel.moveStart('character', -endlen);
            sel.moveEnd('character', -endlen);
        }
        sel.select();
    } else {
        $(key + 'message').value += text;
    }
    hideMenu(2);
    if (BROWSER.ie) {
        doane();
    }
}

function pmchecknew() {
    ajaxget('pm.php?checknewpm=' + Math.random(), 'pm_ntc', 'ajaxwaitid');
}

function pmviewnew() {
    if(!$('pm_ntc_menu')) {
        var div = document.createElement("div");
        div.id = 'pm_ntc_menu';
        div.style.display = 'none';
        $('append_parent').appendChild(div);
        div.innerHTML = '<div id="pm_ntc_view"></div>';
    }
    showMenu('pm_ntc');
    if($('pm_ntc_view').innerHTML == '') {
        ajaxget('pm.php?action=viewnew', 'pm_ntc_view', 'ajaxwaitid');
    }
}

function creditnoticewin() {
    if(!(creditnoticedata = getcookie('bbx_creditnotice'))) {
        return;
    }
    if(getcookie('bbx_creditnoticedisable')) {
        return;
    }
    creditnoticearray = creditnoticedata.split('D');
    if(creditnoticearray[9] != bbx_uid) {
        return;
    }
    creditnames = creditnotice.split(',');
    creditinfo = new Array();
    for(i in creditnames) {
        var e = creditnames[i].split('|');
        creditinfo[e[0]] = [e[1], e[2]];
    }
    s = '';
    for(i = 1;i <= 8;i++) {
        if(creditnoticearray[i] != 0 && creditinfo[i]) {
            s += '<span>' + creditinfo[i][0] + (creditnoticearray[i] > 0 ? '<em>+' : '<em class="desc">') + creditnoticearray[i] + '</em>' + creditinfo[i][1] + '</span>';
        }
    }
    setcookie('bbx_creditnotice', '', -2592000);
    if(s) {
        noticewin(s, 2000, 1);
    }
}

function noticewin(s, t, c) {
    c = !c ? '' : c;
    s = !c ? '<span style="font-style: normal;">'+s+'</span>' : s;
    s = '<table cellspacing="0" cellpadding="0" class="popupcredit"><tr><td class="pc_l">&nbsp;</td><td class="pc_c"><div class="pc_inner">' + s +
        (c ? '<a class="pc_btn" href="javascript:;" onclick="display(\'ntcwin\');setcookie(\'bbx_creditnoticedisable\', 1, 31536000);" title="不要再提示我"><img src="' + IMGDIR + '/popupcredit_btn.gif" alt="不要再提示我" /></a>' : '') +
        '</td><td class="pc_r">&nbsp;</td></tr></table>';
    if(!$('ntcwin')) {
        var div = document.createElement("div");
        div.id = 'ntcwin';
        div.style.display = 'none';
        div.style.position = 'absolute';
        div.style.zIndex = '100000';
        $('append_parent').appendChild(div);
    }
    $('ntcwin').innerHTML = s;
    $('ntcwin').style.display = '';
    $('ntcwin').style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=0)';
    $('ntcwin').style.opacity = 0;
    pbegin = document.documentElement.scrollTop + (document.documentElement.clientHeight / 2);
    pend = document.documentElement.scrollTop + (document.documentElement.clientHeight / 5);
    setTimeout(function () {noticewin_show(pbegin, pend, 0, t)}, 10);
    $('ntcwin').style.left = ((document.documentElement.clientWidth - $('ntcwin').clientWidth) / 2) + 'px';
    $('ntcwin').style.top = pbegin + 'px';
}

function noticewin_show(b, e, a, t) {
    step = (b - e) / 10;
    newp = (parseInt($('ntcwin').style.top) - step);
    if(newp > e) {
        $('ntcwin').style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=' + a + ')';
        $('ntcwin').style.opacity = a / 100;
        $('ntcwin').style.top = newp + 'px';
        setTimeout(function () {noticewin_show(b, e, a += 10, t)}, 10);
    } else {
        $('ntcwin').style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=100)';
        $('ntcwin').style.opacity = 1;
        setTimeout('display_opacity(\'ntcwin\', 100)', t);
    }
}

function showimmestatus(imme) {
    var lang = {'Online':'MSN 在线','Busy':'MSN 忙碌','Away':'MSN 离开','Offline':'MSN 脱机'};
    $('imme_status_' + imme.id.substr(0, imme.id.indexOf('@'))).innerHTML = lang[imme.statusText];
}

var bbx_uid = isUndefined(bbx_uid) ? 0 : bbx_uid;
var creditnotice = isUndefined(creditnotice) ? '' : creditnotice;
var cookiedomain = isUndefined(cookiedomain) ? '' : cookiedomain;


if(typeof IN_ADMINCP == 'undefined') {
    if(bbx_uid && !getcookie('checkpm')) {
        _attachEvent(window, 'load', pmchecknew, document);
    }
    if(creditnotice != '' && getcookie('bbx_creditnotice') && !getcookie('bbx_creditnoticedisable')) {
        _attachEvent(window, 'load', function(){creditnoticewin()}, document);
    }
}

function scrollHiddenDiv(div, scrollwidth)
{
    div.scrollLeft += scrollwidth;
}

function findtags(parentobj, tag) {
    if(!isUndefined(parentobj.getElementsByTagName)) {
        return parentobj.getElementsByTagName(tag);
    } else if(parentobj.all && parentobj.all.tags) {
        return parentobj.all.tags(tag);
    } else {
        return null;
    }
}

function getQueryString(queryname) {
    var qKeys = {};
    var re = /[?&]([^=]+)(?:=([^&]*))?/g;
    var matchInfo;
    while(matchInfo = re.exec(location.search)){
        qKeys[matchInfo[1]] = matchInfo[2];
    }
    return typeof(qKeys[queryname])=='undefined'?'':qKeys[queryname];
}

function getUserid()
{
    // cookies are separated by semicolons
    var aCookie = document.cookie.split("; ");
    for (var i=0; i < aCookie.length; i++)
    {
        // a name alue pair (a crumb) is separated by an equal sign
        var aCrumb = aCookie[i].split("=");
        for(var j = 0; j < aCrumb.length; j++)
        {
            if ("userid" == aCrumb[j])
            {
                return aCrumb[j+1].split("&")[0];
            }
        }
    }
    // a cookie with the requested name does not exist
    return null;
}

function getCSSRule(ruleName, deleteFlag) {               // Return requested style obejct
   ruleName=ruleName.toLowerCase();                       // Convert test string to lower case.
   if (document.styleSheets) {                            // If browser can play with stylesheets
      for (var i=0; i<document.styleSheets.length; i++) { // For each stylesheet
         var styleSheet=document.styleSheets[i];          // Get the current Stylesheet
         var ii=0;                                        // Initialize subCounter.
         var cssRule=false;                               // Initialize cssRule. 
         do {                                             // For each rule in stylesheet
            if (styleSheet.cssRules) {                    // Browser uses cssRules?
               cssRule = styleSheet.cssRules[ii];         // Yes --Mozilla Style
            } else {                                      // Browser usses rules?
               cssRule = styleSheet.rules[ii];            // Yes IE style. 
            }                                             // End IE check.
            if (cssRule)  {                               // If we found a rule...
               if (cssRule.selectorText.toLowerCase()==ruleName) { //  match ruleName?
                  if (deleteFlag=='delete') {             // Yes.  Are we deleteing?
                     if (styleSheet.cssRules) {           // Yes, deleting...
                        styleSheet.deleteRule(ii);        // Delete rule, Moz Style
                     } else {                             // Still deleting.
                        styleSheet.removeRule(ii);        // Delete rule IE style.
                     }                                    // End IE check.
                     return true;                         // return true, class deleted.
                  } else {                                // found and not deleting.
                     return cssRule;                      // return the style object.
                  }                                       // End delete Check
               }                                          // End found rule name
            }                                             // end found cssRule
            ii++;                                         // Increment sub-counter
         } while (cssRule)                                // end While loop
      }                                                   // end For loop
   }                                                      // end styleSheet ability check
   return false;                                          // we found NOTHING!
}                                                         // end getCSSRule 

function killCSSRule(ruleName) {                          // Delete a CSS rule   
   return getCSSRule(ruleName,'delete');                  // just call getCSSRule w/delete flag.
}                                                         // end killCSSRule

function addCSSRule(ruleName) {                           // Create a new css rule
   if (document.styleSheets) {                            // Can browser do styleSheets?
      if (!getCSSRule(ruleName)) {                        // if rule doesn't exist...
         if (document.styleSheets[0].addRule) {           // Browser is IE?
            document.styleSheets[0].addRule(ruleName, null,0);      // Yes, add IE style
         } else {                                         // Browser is IE?
            document.styleSheets[0].insertRule(ruleName+' { }', 0); // Yes, add Moz style.
         }                                                // End browser check
      }                                                   // End already exist check.
   }                                                      // End browser ability check.
   return getCSSRule(ruleName);                           // return rule we just created.

}

function AC_FL_RunContent()
{
    var ret = AC_GetArgs(arguments, "clsid:d27cdb6e-ae6d-11cf-96b8-444553540000", "application/x-shockwave-flash");
    var str = '';
    if (is_ie && !is_opera)
    {
        str += '<object ';
        for (var i in ret.objAttrs)
        {
            str += i + '="' + ret.objAttrs[i] + '" ';
        }
        str += '>';
        for (var i in ret.params)
        {
            str += '<param name="' + i + '" value="' + ret.params[i] + '" /> ';
        }
        str += '</object>';
    } else
    {
        str += '<embed ';
        for (var i in ret.embedAttrs)
        {
            str += i + '="' + ret.embedAttrs[i] + '" ';
        }
        str += '></embed>';
    }
    return str;
}

function AC_GetArgs(args, classid, mimeType)
{
    var ret = new Object();
    ret.embedAttrs = new Object();
    ret.params = new Object();
    ret.objAttrs = new Object();
    for (var i = 0; i < args.length; i = i + 2)
    {
        var currArg = args[i].toLowerCase();
        switch (currArg)
        {
            case "classid": break;
            case "pluginspage": ret.embedAttrs[args[i]] = 'http://www.macromedia.com/go/getflashplayer'; break;
            case "src": ret.embedAttrs[args[i]] = args[i + 1]; ret.params["movie"] = args[i + 1]; break;
            case "codebase": ret.objAttrs[args[i]] = 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0'; break;
            case "onafterupdate": case "onbeforeupdate": case "onblur": case "oncellchange": case "onclick": case "ondblclick": case "ondrag": case "ondragend":
            case "ondragenter": case "ondragleave": case "ondragover": case "ondrop": case "onfinish": case "onfocus": case "onhelp": case "onmousedown":
            case "onmouseup": case "onmouseover": case "onmousemove": case "onmouseout": case "onkeypress": case "onkeydown": case "onkeyup": case "onload":
            case "onlosecapture": case "onpropertychange": case "onreadystatechange": case "onrowsdelete": case "onrowenter": case "onrowexit": case "onrowsinserted": case "onstart":
            case "onscroll": case "onbeforeeditfocus": case "onactivate": case "onbeforedeactivate": case "ondeactivate": case "type":
            case "id": ret.objAttrs[args[i]] = args[i + 1]; break;
            case "width": case "height": case "align": case "vspace": case "hspace": case "class": case "title": case "accesskey": case "name":
            case "tabindex": ret.embedAttrs[args[i]] = ret.objAttrs[args[i]] = args[i + 1]; break;
            default: ret.embedAttrs[args[i]] = ret.params[args[i]] = args[i + 1];
        }
    }
    ret.objAttrs["classid"] = classid;
    if (mimeType)
    {
        ret.embedAttrs["type"] = mimeType;
    }
    return ret;
}
 
 
 function newSetMenuPosition(showid, menuid, pos) {
    var showObj = $(showid);
    var menuObj = menuid ? $(menuid) : $(showid + '_menu');
    if(isUndefined(pos)) pos = '43';
    var basePoint = parseInt(pos.substr(0, 1));
    var direction = parseInt(pos.substr(1, 1));
    var sxy = sx = sy = sw = sh = ml = mt = mw = mcw = mh = mch = bpl = bpt = 0;

    if(!menuObj || (basePoint > 0 && !showObj)) return;
    if(showObj) {
        sxy = fetchOffset(showObj);
        sx = sxy['left'];
        sy = sxy['top'];
        sw = showObj.offsetWidth;
        sh = showObj.offsetHeight;
    }
    mw = menuObj.offsetWidth;
    mcw = menuObj.clientWidth;
    mh = menuObj.offsetHeight;
    mch = menuObj.clientHeight;

    switch(basePoint) {
        case 1:
            bpl = sx;
            bpt = sy;
            break;
        case 2:
            bpl = sx + sw;
            bpt = sy;
            break;
        case 3:
            bpl = sx + sw;
            bpt = sy + sh;
            break;
        case 4:
            bpl = sx;
            bpt = sy + sh;
            break;
    }
    switch(direction) {
        case 0:
            menuObj.style.left = (document.body.clientWidth - menuObj.clientWidth) / 2 + 'px';
            mt = (document.documentElement.clientHeight - menuObj.clientHeight) / 2;
            break;
        case 1:
            ml = bpl - mw;
            mt = bpt - mh;
            break;
        case 2:
            ml = bpl;
            mt = bpt - mh;
            break;
        case 3:
            ml = bpl;
            mt = bpt;
            break;
        case 4:
            ml = bpl - mw;
            mt = bpt;
            break;
    }
    if(in_array(direction, [1, 4]) && ml < 0) {
        ml = bpl;
        if(in_array(basePoint, [1, 4])) ml += sw;
    } else if(ml + mw > document.documentElement.scrollLeft + document.body.clientWidth && sx >= mw) {
        ml = bpl - mw;
        if(in_array(basePoint, [2, 3])) ml -= sw;
    }
    if(in_array(direction, [1, 2]) && mt < 0) {
        mt = bpt;
        if(in_array(basePoint, [1, 2])) mt += sh;
    } else if(mt + mh > document.documentElement.scrollTop + document.documentElement.clientHeight && sy >= mh) {
        mt = bpt - mh;
        if(in_array(basePoint, [3, 4])) mt -= sh;
    }
    if(pos == '210') {
        ml += 69 - sw / 2;
        mt -= 5;
        if(showObj.tagName == 'TEXTAREA') {
            ml -= sw / 2;
            mt += sh / 2;
        }
    }
    if(direction == 0 || menuObj.scrolly) {
        if(BROWSER.ie && BROWSER.ie < 7) {
            if(direction == 0) mt += Math.max(document.documentElement.scrollTop, document.body.scrollTop);
        } else {
            if(menuObj.scrolly) mt -= Math.max(document.documentElement.scrollTop, document.body.scrollTop);
            menuObj.style.position = 'fixed';
        }
    }
    if(ml) menuObj.style.left = ml + 'px';
    if(mt) menuObj.style.top = mt + 'px';
    if(direction == 0 && BROWSER.ie && !document.documentElement.clientHeight) {
        menuObj.style.position = 'absolute';
        menuObj.style.top = (document.body.clientHeight - menuObj.clientHeight) / 2 + 'px';
    }
    if(menuObj.style.clip && !BROWSER.opera) {
        menuObj.style.clip = 'rect(auto, auto, auto, auto)';
    }
}

 
 
 function newHideMenu(attr, mtype) {
    attr = isUndefined(attr) ? '' : attr;
    mtype = isUndefined(mtype) ? 'menu' : mtype;
    if(attr == '') {
        for(var i = 1; i <= JSMENU['layer']; i++) {
            newHideMenu(i, mtype);
        }
        return;
    } else if(typeof attr == 'number') {
        for(var j in JSMENU['active'][attr]) {
            newHideMenu(JSMENU['active'][attr][j], mtype);
        }
        return;
    } else if(typeof attr == 'string') {
        var menuObj = $(attr);
        if(!menuObj || (mtype && menuObj.mtype != mtype)) return;
        clearTimeout(JSMENU['timer'][attr]);
        var hide = function() {
            if(menuObj.cache) {
                menuObj.style.display = 'none';
                if(menuObj.cover) $(attr + '_cover').style.display = 'none';
            } else {
                menuObj.parentNode.removeChild(menuObj);
                if(menuObj.cover) $(attr + '_cover').parentNode.removeChild($(attr + '_cover'));
            }
            var tmp = [];
            for(var k in JSMENU['active'][menuObj.layer]) {
                if(attr != JSMENU['active'][menuObj.layer][k]) tmp.push(JSMENU['active'][menuObj.layer][k]);
            }
            JSMENU['active'][menuObj.layer] = tmp;
        };
        if(menuObj.fade) {
            var O = 100;
            var fadeOut = function(O) {
                if(O == 0) {
                    clearTimeout(fadeOutTimer);
                    hide();
                    return;
                }
                menuObj.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=' + O + ')';
                menuObj.style.opacity = O / 100;
                O -= 10;
                var fadeOutTimer = setTimeout(function () {
                    fadeOut(O);
                }, 50);
            };
            fadeOut(O);
        } else {
            hide();
        }
    }
}

 
 
 function newShowMenu(v) {
    var ctrlid = isUndefined(v['ctrlid']) ? v : v['ctrlid'];
    var showid = isUndefined(v['showid']) ? ctrlid : v['showid'];
    var menuid = isUndefined(v['menuid']) ? showid + '_menu' : v['menuid'];
    var ctrlObj = $(ctrlid);
    var menuObj = $(menuid);
    if(!menuObj) return;
    var mtype = isUndefined(v['mtype']) ? 'menu' : v['mtype'];
    var evt = isUndefined(v['evt']) ? 'mouseover' : v['evt'];
    var pos = isUndefined(v['pos']) ? '43' : v['pos'];
    var layer = isUndefined(v['layer']) ? 1 : v['layer'];
    var duration = isUndefined(v['duration']) ? 2 : v['duration'];
    var timeout = isUndefined(v['timeout']) ? 250 : v['timeout'];
    var maxh = isUndefined(v['maxh']) ? 500 : v['maxh'];
    var cache = isUndefined(v['cache']) ? 1 : v['cache'];
    var drag = isUndefined(v['drag']) ? '' : v['drag'];
    var dragobj = drag && $(drag) ? $(drag) : menuObj;
    var fade = isUndefined(v['fade']) ? 0 : v['fade'];
    var cover = isUndefined(v['cover']) ? 0 : v['cover'];
    var zindex = isUndefined(v['zindex']) ? JSMENU['zIndex']['menu'] : v['zindex'];
    if(typeof JSMENU['active'][layer] == 'undefined') {
        JSMENU['active'][layer] = [];
    }

    if(evt == 'click' && in_array(menuid, JSMENU['active'][layer]) && mtype != 'win') {
        newHideMenu(menuid, mtype);
        return;
    }
    if(mtype == 'menu') {
        newHideMenu(layer, mtype);
    }

    if(ctrlObj) {
        if(!ctrlObj.initialized) {
            ctrlObj.initialized = true;
            ctrlObj.unselectable = true;

            ctrlObj.outfunc = typeof ctrlObj.onmouseout == 'function' ? ctrlObj.onmouseout : null;
            ctrlObj.onmouseout = function() {
                if(this.outfunc) this.outfunc();
                if(duration < 3 && !JSMENU['timer'][menuid]) JSMENU['timer'][menuid] = setTimeout('newHideMenu(\'' + menuid + '\', \'' + mtype + '\')', timeout);
            };

            ctrlObj.overfunc = typeof ctrlObj.onmouseover == 'function' ? ctrlObj.onmouseover : null;
            ctrlObj.onmouseover = function(e) {
                doane(e);
                if(this.overfunc) this.overfunc();
                if(evt == 'click') {
                    clearTimeout(JSMENU['timer'][menuid]);
                    JSMENU['timer'][menuid] = null;
                } else {
                    for(var i in JSMENU['timer']) {
                        if(JSMENU['timer'][i]) {
                            clearTimeout(JSMENU['timer'][i]);
                            JSMENU['timer'][i] = null;
                        }
                    }
                }
            };
        }
    }

    var dragMenu = function(menuObj, e, op) {
        e = e ? e : window.event;
        if(op == 1) {
            if(in_array(BROWSER.ie ? e.srcElement.tagName : e.target.tagName, ['TEXTAREA', 'INPUT', 'BUTTON', 'SELECT'])) {
                return;
            }
            JSMENU['drag'] = [e.clientX, e.clientY];
            JSMENU['drag'][2] = parseInt(menuObj.style.left);
            JSMENU['drag'][3] = parseInt(menuObj.style.top);
            document.onmousemove = function(e) {try{dragMenu(menuObj, e, 2);}catch(err){}};
            document.onmouseup = function(e) {try{dragMenu(menuObj, e, 3);}catch(err){}};
            doane(e);
        } else if(op == 2 && JSMENU['drag'][0]) {
            var menudragnow = [e.clientX, e.clientY];
            menuObj.style.left = (JSMENU['drag'][2] + menudragnow[0] - JSMENU['drag'][0]) + 'px';
            menuObj.style.top = (JSMENU['drag'][3] + menudragnow[1] - JSMENU['drag'][1]) + 'px';
            doane(e);
        } else if(op == 3) {
            JSMENU['drag'] = [];
            document.onmousemove = null;
            document.onmouseup = null;
        }
    };

    if(!menuObj.initialized) {
        menuObj.initialized = true;
        menuObj.ctrlkey = ctrlid;
        menuObj.mtype = mtype;
        menuObj.layer = layer;
        menuObj.cover = cover;
        if(ctrlObj && ctrlObj.getAttribute('fwin')) {menuObj.scrolly = true;}
        menuObj.style.position = 'absolute';
        menuObj.style.zIndex = zindex + layer;
        menuObj.onclick = function(e) {
            if(!e || BROWSER.ie) {
                window.event.cancelBubble = true;
                return window.event;
            } else {
                e.stopPropagation();
                return e;
            }
        };
        if(duration < 3) {
            if(duration > 1) {
                menuObj.onmouseover = function() {
                    clearTimeout(JSMENU['timer'][menuid]);
                    JSMENU['timer'][menuid] = null;
                };
            }
            if(duration != 1) {
                menuObj.onmouseout = function() {
                    JSMENU['timer'][menuid] = setTimeout('newHideMenu(\'' + menuid + '\', \'' + mtype + '\')', timeout);
                };
            }
        }
        if(drag) {
            dragobj.style.cursor = 'move';
            dragobj.onmousedown = function(event) {try{dragMenu(menuObj, event, 1);}catch(e){}};
        }
        if(cover) {
            var coverObj = document.createElement('div');
            coverObj.id = menuid + '_cover';
            coverObj.style.position = 'absolute';
            coverObj.style.zIndex = menuObj.style.zIndex - 1;
            coverObj.style.left = coverObj.style.top = '0px';
            coverObj.style.width = '100%';
            coverObj.style.height = document.body.scrollHeight + 'px';
            coverObj.style.backgroundColor = '#000';
            coverObj.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=50)';
            coverObj.style.opacity = 0.5;
            $('append_parent').appendChild(coverObj);
        }
    }
    menuObj.style.display = '';
    if(cover) $(menuid + '_cover').style.display = '';
    if(fade) {
        var O = 0;
        var fadeIn = function(O) {
            if(O == 100) {
                clearTimeout(fadeInTimer);
                return;
            }
            menuObj.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=' + O + ')';
            menuObj.style.opacity = O / 100;
            O += 10;
            var fadeInTimer = setTimeout(function () {
                fadeIn(O);
            }, 50);
        };
        fadeIn(O);
        menuObj.fade = true;
    } else {
        menuObj.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=100)';
        menuObj.style.opacity = 1;
        menuObj.fade = false;
    }
    newSetMenuPosition(showid, menuid, pos);
    if(maxh && menuObj.scrollHeight > maxh) {
        menuObj.style.height = maxh + 'px';
        if(BROWSER.opera) {
            menuObj.style.overflow = 'auto';
        } else {
            menuObj.style.overflowY = 'auto';
        }
    }

    if(!duration) {
        setTimeout('newHideMenu(\'' + menuid + '\', \'' + mtype + '\')', timeout);
    }

    if(!in_array(menuid, JSMENU['active'][layer])) JSMENU['active'][layer].push(menuid);
    menuObj.cache = cache;
    if(layer > JSMENU['layer']) {
        JSMENU['layer'] = layer;
    }
}
 
 function showPrompt(ctrlid, evt, msg, timeout, negligible) {
    var menuid = ctrlid ? ctrlid + '_pmenu' : 'ntcwin';
    var duration = timeout ? 0 : 3;
    if(!$(menuid)) {
        var div = document.createElement('div');
        div.id = menuid;
        div.className = ctrlid ? 'promptmenu up' : 'ntcwin';
        div.style.display = 'none';
        $('append_parent').appendChild(div);
        if(ctrlid) {
            msg = '<div id="' + ctrlid + '_prompt" class="promptcontent"><ul><li>' + msg + '</li></ul></div>';
        } else {
            msg = negligible ? msg : '<span style="font-style: normal;">' + msg + '</span>';
            msg = '<table cellspacing="0" cellpadding="0" class="popupcredit"><tr><td class="pc_l">&nbsp;</td><td class="pc_c"><div class="pc_inner">' + msg +
                (negligible ? '<a class="pc_btn" href="javascript:;" onclick="display(\'ntcwin\');setcookie(\'bbx_creditnoticedisable\', 1, 31536000);" title="不要再提示我"><img src="' + IMGDIR + '/popupcredit_btn.gif" alt="不要再提示我" /></a>' : '') +
                '</td><td class="pc_r">&nbsp;</td></tr></table>';
        }
        div.innerHTML = msg;
    }
    if(ctrlid) {
        if($(ctrlid).evt !== false) {
            var prompting = function() {
                newShowMenu({'mtype':'prompt','ctrlid':ctrlid,'evt':evt,'menuid':menuid,'pos':'210'});
            };
            if(evt == 'click') {
                $(ctrlid).onclick = prompting;
            } else {
                $(ctrlid).onmouseover = prompting;
            }
        }
        newShowMenu({'mtype':'prompt','ctrlid':ctrlid,'evt':evt,'menuid':menuid,'pos':'210','duration':duration,'timeout':timeout,'fade':1,'zindex':JSMENU['zIndex']['prompt']});
        $(ctrlid).unselectable = false;
    } else {
        newShowMenu({'mtype':'prompt','pos':'00','menuid':menuid,'duration':duration,'timeout':timeout,'fade':1,'zindex':JSMENU['zIndex']['prompt']});
    }
}

function showCreditPrompt() {
    var notice = getcookie('bbx_creditnotice').split(',');
/* 	if(notice.length < 2 || notice[9] != bbx_uid || getcookie('bbx_creditnoticedisable')) {
        return;
    } */
    var creditnames = creditnotice.split(',');
    var creditinfo = [];
    var s = '';
    var e;
    for(var i in creditnames) {
        e = creditnames[i].split('|');
        creditinfo[e[0]] = [e[1], e[2]];
    }
    for(i = 1; i <= 8; i++) {
        if(notice[i] != 0 && creditinfo[i]) {
            s += '<span>' + creditinfo[i][0] + (notice[i] > 0 ? '<em>+' : '<em class="desc">') + notice[i] + '</em>' + creditinfo[i][1] + '</span>';
        }
    }
    s && showPrompt(null, null, s, 2000, 1);
    setcookie('bbx_creditnotice', '', -2592000);
}

/* function showWindow(k, url, mode, cache) {
    mode = isUndefined(mode) ? 'get' : mode;
    cache = isUndefined(cache) ? 1 : cache;
    var menuid = 'fwin_' + k;
    var menuObj = $(menuid);

    if(disallowfloat && disallowfloat.indexOf(k) != -1) {
        if(BROWSER.ie) url += (url.indexOf('?') != -1 ?  '&' : '?') + 'referer=' + escape(location.href);
        location.href = url;
        return;
    }

    var fetchContent = function() {
        if(mode == 'get') {
            menuObj.url = url;
            url += (url.search(/\?/) > 0 ? '&' : '?') + 'infloat=yes&handlekey=' + k;
            ajaxget(url, 'fwin_content_' + k, null, '', '', function() {initMenu();show();});
        } else if(mode == 'post') {
            menuObj.act = $(url).action;
            ajaxpost(url, 'fwin_content_' + k, '', '', '', function() {initMenu();show();});
        }
        showDialog('', 'info', '<img src="' + IMGDIR + '/loading.gif"> 加载中...');
    };
    var initMenu = function() {
        var objs = menuObj.getElementsByTagName('*');
        for(var i = 0; i < objs.length; i++) {
            if(objs[i].id) {
                objs[i].setAttribute('fwin', k);
            }
            if(objs[i].className == 'float_ctrl') {
                if(!objs[i].id) objs[i].id = 'fctrl_' + k;
                drag = objs[i].id;
            }
        }
    };
    var show = function() {
        hideMenu('fwin_dialog', 'dialog');
        showMenu({'mtype':'win','menuid':menuid,'duration':3,'pos':'00','zindex':JSMENU['zIndex']['win'],'drag':typeof drag == 'undefined' ? '' : drag,'cache':cache});
    };

    if(!menuObj) {
        menuObj = document.createElement('div');
        menuObj.id = menuid;
        menuObj.className = 'fwinmask';
        menuObj.style.display = 'none';
        $('append_parent').appendChild(menuObj);
        menuObj.innerHTML = '<table cellpadding="0" cellspacing="0" class="fwin"><tr><td class="t_l"></td><td class="t_c"></td><td class="t_r"></td></tr><tr><td class="m_l"></td><td class="m_c" id="fwin_content_' + k + '">'
            + '</td><td class="m_r"></td></tr><tr><td class="b_l"></td><td class="b_c"></td><td class="b_r"></td></tr></table>';
        fetchContent();
    } else if((mode == 'get' && url != menuObj.url) || (mode == 'post' && $(url).action != menuObj.act)) {
        fetchContent();
    } else {
        show();
    }
    doane();
} */

function showWindow(k, url, mode, cache, menuv) {
    mode = isUndefined(mode) ? 'get' : mode;
    cache = isUndefined(cache) ? 1 : cache;
    var menuid = 'fwin_' + k;
    var menuObj = $(menuid);
    var drag = null;
    var loadingst = null;

    hideWindow('reply');
    hideWindow('newthread')
    if(disallowfloat && disallowfloat.indexOf(k) != -1) {
        if(BROWSER.ie) url += (url.indexOf('?') != -1 ?  '&' : '?') + 'referer=' + escape(location.href);
//		location.href = url;
//		doane();
        return;
    }

    var fetchContent = function () {
        if (mode == 'get') {
            menuObj.url = url;
            url += (url.search(/\?/) > 0 ? '&' : '?') + 'infloat=1&handlekey=' + k;
            url += cache == -1 ? '&t=' + (+new Date()) : '';
            ajaxget(url, 'fwin_content_' + k, null, '', '', function () { initMenu(); show(); });


        } else if (mode == 'post') {
            menuObj.act = $(url).action;
            ajaxpost(url, 'fwin_content_' + k, '', '', '', function () { initMenu(); show(); });
        }
        if (parseInt(BROWSER.ie) != 6) {
            loadingst = setTimeout(function () { showDialog('', 'info', '<img src="' + IMGDIR + '/loading.gif"> 请稍候...') }, 500);
        }
    };
    var initMenu = function() {
    
        clearTimeout(loadingst);
        var objs = menuObj.getElementsByTagName('*');
        var fctrlidinit = false;
        for(var i = 0; i < objs.length; i++) {
            if(objs[i].id) {
                objs[i].setAttribute('fwin', k);
            }
            if(objs[i].className == 'flb' && !fctrlidinit) {
                if(!objs[i].id) objs[i].id = 'fctrl_' + k;
                drag = objs[i].id;
                fctrlidinit = true;
            }
        }
    };
    var show = function () {
        hideMenu('fwin_dialog', 'dialog');
        v = { 'mtype': 'win', 'menuid': menuid, 'duration': 3, 'pos': '00', 'zindex': JSMENU['zIndex']['win'], 'drag': typeof drag == null ? '' : drag, 'cache': cache };
        for (k in menuv) {
            v[k] = menuv[k];
        }
        showMenu(v);
    };

    if(!menuObj) {
        menuObj = document.createElement('div');
        menuObj.id = menuid;
        menuObj.className = 'fwinmask';
        menuObj.style.display = 'none';
        $('append_parent').appendChild(menuObj);
        menuObj.innerHTML = '<table cellpadding="0" cellspacing="0" class="fwin"><tr><td class="t_l"></td><td class="t_c" ondblclick="hideWindow(\'' + k + '\')"></td><td class="t_r"></td></tr><tr><td class="m_l" ondblclick="hideWindow(\'' + k + '\')">&nbsp;&nbsp;</td><td class="m_c" id="fwin_content_' + k + '">'
            + '</td><td class="m_r" ondblclick="hideWindow(\'' + k + '\')"></td></tr><tr><td class="b_l"></td><td class="b_c" ondblclick="hideWindow(\'' + k + '\')"></td><td class="b_r"></td></tr></table>';
        if (mode == 'html') {
            $('fwin_content_' + k).innerHTML = url;
            initMenu();
            show();
        } else {
            fetchContent();
        }
    } else if((mode == 'get' && (url != menuObj.url || cache != 1)) || (mode == 'post' && $(url).action != menuObj.act)) {
        fetchContent();
    } else {
        show();
    }
    doane();
}

function hideWindow(k, all, clear) {

    all = isUndefined(all) ? 1 : all;

    clear = isUndefined(clear) ? 1 : clear;

    hideMenu('fwin_' + k, 'win');

    if(clear && $('fwin_' + k)) {

        $('append_parent').removeChild($('fwin_' + k));

    }

    if(all) {

        hideMenu();

    }

    hideMenu('', 'prompt');

}

function onloadshowCreditPrompt()
{
    if(typeof IN_ADMINCP == 'undefined') {
        if(creditnotice != '' && getcookie('bbx_creditnotice') && !getcookie('bbx_creditnoticedisable')) {
            try
            {
            _attachEvent(window, 'load', showCreditPrompt, document) || showCreditPrompt(); 
            }
            catch(e)
            {
            showCreditPrompt();
            }
        }
    }
}

function saveUserdata(name, data) {
    if (BROWSER.ie) {
        with (document.documentElement) {
            setAttribute("value", data);
            save('BBX_' + name);
        }
    } else if (window.sessionStorage) {
        sessionStorage.setItem('BBX_' + name, data);
    }
}

function showColorBox(ctrlid, layer, k) {
    if(!$(ctrlid + '_menu')) {
        var menu = document.createElement('div');
        menu.id = ctrlid + '_menu';
        menu.className = 'p_pop colorbox';
        menu.unselectable = true;
        menu.style.display = 'none';
        var coloroptions = ['Black', 'Sienna', 'DarkOliveGreen', 'DarkGreen', 'DarkSlateBlue', 'Navy', 'Indigo', 'DarkSlateGray', 'DarkRed', 'DarkOrange', 'Olive', 'Green', 'Teal', 'Blue', 'SlateGray', 'DimGray', 'Red', 'SandyBrown', 'YellowGreen', 'SeaGreen', 'MediumTurquoise', 'RoyalBlue', 'Purple', 'Gray', 'Magenta', 'Orange', 'Yellow', 'Lime', 'Cyan', 'DeepSkyBlue', 'DarkOrchid', 'Silver', 'Pink', 'Wheat', 'LemonChiffon', 'PaleGreen', 'PaleTurquoise', 'LightBlue', 'Plum', 'White'];
        var colortexts = ['黑色', '赭色', '暗橄榄绿色', '暗绿色', '暗灰蓝色', '海军色', '靛青色', '墨绿色', '暗红色', '暗桔黄色', '橄榄色', '绿色', '水鸭色', '蓝色', '灰石色', '暗灰色', '红色', '沙褐色', '黄绿色', '海绿色', '间绿宝石', '皇家蓝', '紫色', '灰色', '红紫色', '橙色', '黄色', '酸橙色', '青色', '深天蓝色', '暗紫色', '银色', '粉色', '浅黄色', '柠檬绸色', '苍绿色', '苍宝石绿', '亮蓝色', '洋李色', '白色'];
        var str = '';
        for(var i = 0; i < 40; i++) {
            str += '<input type="button" style="background-color: ' + coloroptions[i] + '"' + (typeof setEditorTip == 'function' ? ' onmouseover="setEditorTip(\'' + colortexts[i] + '\')" onmouseout="setEditorTip(\'\')"' : '') + ' onclick="'
            + (typeof wysiwyg == 'undefined' ? 'seditor_insertunit(\'' + k + '\', \'[color=' + coloroptions[i] + ']\', \'[/color]\')' : (ctrlid == editorid + '_tbl_param_4' ? '$(\'' + ctrlid + '\').value=\'' + coloroptions[i] + '\';hideMenu(2)' : 'bbxcode(\'forecolor\', \'' + coloroptions[i] + '\')'))
            + '" title="' + colortexts[i] + '" />' + (i < 39 && (i + 1) % 8 == 0 ? '<br />' : '');
        }
        menu.innerHTML = str;
        $('append_parent').appendChild(menu);
    }
    showMenu({'ctrlid':ctrlid,'evt':'click','layer':layer});
}

//function loadData(quiet, contentname) {

////    var evalevent = function (obj) {
////        var script = obj.parentNode.innerHTML;
////        var re = /onclick="(.+?)["|>]/ig;
////        var matches = re.exec(script);
////        if (matches != null) {
////            matches[1] = matches[1].replace(/this\./ig, 'obj.');
////            eval(matches[1]);
////        }
////    };

//    var data = '';
//    data = loadUserdata('forum');
//    var formobj = !formobj ? $('postform') : formobj;

//    if (in_array((data = trim(data)), ['', 'null', 'false', null, false])) {
//        if (!quiet) {
//            showDialog('没有可以恢复的数据！', 'info');
//        }
//        return;
//    }

//    if (!quiet && !confirm('此操作将覆盖当前帖子内容，确定要恢复数据吗？')) {
//        return;
//    }

//    var data = data.split(/\x09\x09/);
//    for (var i = 0; i < formobj.elements.length; i++) {
//        var el = formobj.elements[i];
//        if (el.name != '' && (el.tagName == 'SELECT' || el.tagName == 'TEXTAREA' || el.tagName == 'INPUT' && (el.type == 'text' || el.type == 'checkbox' || el.type == 'radio' || el.type == 'hidden'))) {
//            for (var j = 0; j < data.length; j++) {
//                var ele = data[j].split(/\x09/);
//                if (ele[0] == el.name) {
//                    elvalue = !isUndefined(ele[3]) ? ele[3] : '';
//                    if (ele[1] == 'INPUT') {
//                        if (ele[2] == 'text') {
//                            el.value = elvalue;
//                        } else if ((ele[2] == 'checkbox' || ele[2] == 'radio') && ele[3] == el.value) {
//                            el.checked = true;
//                            evalevent(el);
//                        } else if (ele[2] == 'hidden') {
//                            eval('var check = typeof ' + el.id + '_upload == \'function\'');
//                            if (check) {
//                                var v = elvalue.split(/\x01/);
//                                el.value = v[0];
//                                if (el.value) {
//                                    if ($(el.id + '_url') && v[1]) {
//                                        $(el.id + '_url').value = v[1];
//                                    }
//                                    eval(el.id + '_upload(\'' + v[0] + '\', \'' + v[1] + '\')');
//                                    if ($('unused' + v[0])) {
//                                        var attachtype = $('unused' + v[0]).parentNode.parentNode.parentNode.parentNode.id.substr(11);
//                                        $('unused' + v[0]).parentNode.parentNode.outerHTML = '';
//                                        $('unusednum_' + attachtype).innerHTML = parseInt($('unusednum_' + attachtype).innerHTML) - 1;
//                                        if ($('unusednum_' + attachtype).innerHTML == 0 && $('attachnotice_' + attachtype)) {
//                                            $('attachnotice_' + attachtype).style.display = 'none';
//                                        }
//                                    }
//                                }
//                            }

//                        }
//                    } else if (ele[1] == 'TEXTAREA') {
//                        if (ele[0] == 'message') {
//                            if (!wysiwyg) {
//                                textobj.value = elvalue;
//                            } else {
//                                editdoc.body.innerHTML = bbcode2html(elvalue);
//                            }
//                        } else {
//                            el.value = elvalue;
//                        }
//                    } else if (ele[1] == 'SELECT') {
//                        if ($(el.id + '_ctrl_menu')) {
//                            var lis = $(el.id + '_ctrl_menu').getElementsByTagName('li');
//                            for (var k = 0; k < lis.length; k++) {
//                                if (ele[3] == lis[k].k_value) {
//                                    lis[k].onclick();
//                                    break;
//                                }
//                            }
//                        } else {
//                            for (var k = 0; k < el.options.length; k++) {
//                                if (ele[3] == el.options[k].value) {
//                                    el.options[k].selected = true;
//                                    break;
//                                }
//                            }
//                        }
//                    }
//                    break;
//                }
//            }
//        }
//    }
//    if ($('rstnotice')) {
//        $('rstnotice').style.display = 'none';
//    }
//    extraCheckall();
//}

function loadData(quiet, contentname) {
    var data = '';
    data = loadUserdata('forum');
    if (in_array((data = trim(data)), ['', 'null', 'false', null, false])) {
        if (!quiet)
            showDialog('没有可以恢复的数据！');
        return;
    }

    if (!quiet && !confirm('此操作将覆盖当前帖子内容，确定要恢复数据吗？'))
        return;

    var data = data.split(/\x09\x09/);
    for (var i = 0; i < $('postform').elements.length; i++) {
        var el = $('postform').elements[i];
        if (el.name != '' && (el.tagName == 'SELECT' || el.tagName == 'TEXTAREA' || el.tagName == 'INPUT' && (el.type == 'text' || el.type == 'checkbox' || el.type == 'radio'))) {
            for (var j = 0; j < data.length; j++) {
                var ele = data[j].split(/\x09/);
                if (ele[0] == el.name) {
                    elvalue = !isUndefined(ele[3]) ? ele[3] : '';
                    if (ele[1] == 'INPUT') {
                        if (ele[2] == 'text') {
                            el.value = elvalue;
                        } else if ((ele[2] == 'checkbox' || ele[2] == 'radio') && ele[3] == el.value) {
                            el.checked = true;
                            evalevent(el);
                        }
                    } else if (ele[1] == 'TEXTAREA') {
                        if (ele[0] == contentname) {
                            if (wysiwyg) {
                                editdoc.body.innerHTML = bbcode2html(elvalue);
                            }
                            textobj.value = elvalue;
                        } else {
                            el.value = elvalue;
                        }
                    } else if (ele[1] == 'SELECT') {
                        if ($(el.id + '_ctrl_menu')) {
                            var lis = $(el.id + '_ctrl_menu').getElementsByTagName('li');
                            for (var k = 0; k < lis.length; k++) {
                                if (ele[3] == lis[k].k_value) {
                                    lis[k].onclick();
                                    break;
                                }
                            }
                        } else {
                            for (var k = 0; k < el.options.length; k++) {
                                if (ele[3] == el.options[k].value) {
                                    el.options[k].selected = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
    }
}

function loadUserdata(name) {
    if (BROWSER.ie) {
        with (document.documentElement) {
            load('BBX_' + name);
            return getAttribute("value");
        }
    }
    else if (window.sessionStorage) {
        return sessionStorage.getItem('BBX_' + name);
    }
}

function showDialog(msg, mode, t, func, cover, funccancel, leftmsg) {
    cover = isUndefined(cover) ? (mode == 'info' ? 0 : 1) : cover;
    leftmsg = isUndefined(leftmsg) ? '' : leftmsg;
    mode = in_array(mode, ['confirm', 'notice', 'info']) ? mode : 'alert';
    var menuid = 'fwin_dialog';
    var menuObj = $(menuid);

    if(menuObj) hideMenu('fwin_dialog', 'dialog');
    menuObj = document.createElement('div');
    menuObj.style.display = 'none';
    menuObj.className = 'fwinmask';
    menuObj.id = menuid;
    $('append_parent').appendChild(menuObj);
    var s = '<table cellpadding="0" cellspacing="0" class="fwin"><tr><td class="t_l"></td><td class="t_c"></td><td class="t_r"></td></tr><tr><td class="m_l">&nbsp;&nbsp;</td><td class="m_c"><h3 class="flb"><em>';
    s += t ? t : '提示信息';
    s += '</em><span><a href="javascript:;" id="fwin_dialog_close" class="flbc" onclick="hideMenu(\'' + menuid + '\', \'dialog\')" title="关闭">关闭</a></span></h3>';
    if(mode == 'info') {
        s += msg ? msg : '';
    } else {
        s += '<div class="c altw"><div class="' + (mode == 'alert' ? 'alert_error' : 'alert_info') + '"><p>' + msg + '</p></div></div>';
        s += '<p class="o pns">' + (leftmsg ? '<span class="z xg1">' + leftmsg + '</span>' : '') + '<button id="fwin_dialog_submit" value="true" class="pn pnc"><strong>确定</strong></button>';
        s += mode == 'confirm' ? '<button id="fwin_dialog_cancel" value="true" class="pn" onclick="hideMenu(\'' + menuid + '\', \'dialog\')"><strong>取消</strong></button>' : '';
        s += '</p>';
    }
    s += '</td><td class="m_r"></td></tr><tr><td class="b_l"></td><td class="b_c"></td><td class="b_r"></td></tr></table>';
    menuObj.innerHTML = s;
    if($('fwin_dialog_submit')) $('fwin_dialog_submit').onclick = function() {
        if(typeof func == 'function') func();
        else eval(func);
        hideMenu(menuid, 'dialog');
    };
    if ($('fwin_dialog_cancel')) {
        $('fwin_dialog_cancel').onclick = function () {
            if (typeof funccancel == 'function') funccancel();
            else eval(funccancel);
            hideMenu(menuid, 'dialog');
        };
        $('fwin_dialog_close').onclick = $('fwin_dialog_cancel').onclick;
    }
    else {
        $('fwin_dialog_close').onclick = function () {
            if (typeof funccancel == 'function') funccancel();
            else eval(funccancel);
            hideMenu(menuid, 'dialog');
        };
    }
    showMenu({'mtype':'dialog','menuid':menuid,'duration':3,'pos':'00','zindex':JSMENU['zIndex']['dialog'],'cache':0,'cover':cover});
}


function lsShowmore() {
    var objpos = fetchOffset($('ls_password'));
    var objh = $('ls_password').offsetHeight;
    $('ls_more').style.position = 'absolute';
    $('ls_more').style.left = objpos['left'] + 'px';
    $('ls_more').style.top = (objpos['top'] + objh) + 'px';
    $('ls_more').style.display = '';
}
_attachEvent(window, 'load', onloadshowCreditPrompt, document);


function simulateSelect(selectId, widthvalue, ignorewhitespace) {
    var selectObj = $(selectId);
    if (!selectObj) return;
    if (BROWSER.other) {
        if (selectObj.getAttribute('change')) {
            selectObj.onchange = function() { eval(selectObj.getAttribute('change')); }
        }
        return;
    }
    var widthvalue = widthvalue ? widthvalue : 70;
    var defaultopt = selectObj.options[0] ? selectObj.options[0].innerHTML : '';
    var defaultv = '';
    var menuObj = document.createElement('div');
    var ul = document.createElement('ul');
    var handleKeyDown = function(e) {
        e = BROWSER.ie ? event : e;
        if (e.keyCode == 40 || e.keyCode == 38) doane(e);
    };
    var selectwidth = (selectObj.getAttribute('width', i) ? selectObj.getAttribute('width', i) : widthvalue) + 'px';
    var tabindex = selectObj.getAttribute('tabindex', i) ? selectObj.getAttribute('tabindex', i) : 1;

    for (var i = 0; i < selectObj.options.length; i++) {
        var li = document.createElement('li');
        li.innerHTML = selectObj.options[i].innerHTML;
        li.k_id = i;
        li.k_value = selectObj.options[i].value;
        if (selectObj.options[i].selected) {
            var selecttext = "";
            if (ignorewhitespace) {
                selecttext = selectObj.options[i].innerHTML.replace(/(^\s*)|(\s*$)|&nbsp;/g, "");
            }
            else
                selecttext = selectObj.options[i].innerHTML;
            defaultopt = selecttext;
            defaultv = selectObj.options[i].value;
            li.className = 'current';
            selectObj.setAttribute('selecti', i);
        }
        li.onclick = function () {
            var selecttext = "";
            if (ignorewhitespace) 
                selecttext = this.innerHTML.replace(/(^\s*)|(\s*$)|&nbsp;/g, "");
            else
                selecttext = this.innerHTML;
            if ($(selectId + '_ctrl').innerHTML != selecttext) {
                var lis = menuObj.getElementsByTagName('li');
                lis[$(selectId).getAttribute('selecti')].className = '';
                this.className = 'current';
                $(selectId + '_ctrl').innerHTML = selecttext;
                $(selectId).setAttribute('selecti', this.k_id);
                $(selectId).options.length = 0;
                $(selectId).options[0] = new Option('', this.k_value);
                eval(selectObj.getAttribute('change'));
            }
            hideMenu(menuObj.id);
            return false;
        };
        ul.appendChild(li);
    }

    selectObj.options.length = 0;
    selectObj.options[0] = new Option('', defaultv);
    selectObj.style.display = 'none';
    selectObj.outerHTML += '<a href="javascript:;" id="' + selectId + '_ctrl" style="width:' + selectwidth + '" tabindex="' + tabindex + '">' + defaultopt + '</a>';

    menuObj.id = selectId + '_ctrl_menu';
    menuObj.className = 'sltm';
    menuObj.style.display = 'none';
    menuObj.style.width = selectwidth;
    menuObj.appendChild(ul);
    $('append_parent').appendChild(menuObj);

    $(selectId + '_ctrl').onclick = function(e) {
        $(selectId + '_ctrl_menu').style.width = selectwidth;
        showMenu({ 'ctrlid': (selectId == 'loginfield' ? 'account' : selectId + '_ctrl'), 'menuid': selectId + '_ctrl_menu', 'evt': 'click', 'pos': '43' });
        doane(e);
    };
    $(selectId + '_ctrl').onfocus = menuObj.onfocus = function() {
        _attachEvent(document.body, 'keydown', handleKeyDown);
    };
    $(selectId + '_ctrl').onblur = menuObj.onblur = function() {
        _detachEvent(document.body, 'keydown', handleKeyDown);
    };
    $(selectId + '_ctrl').onkeyup = function(e) {
        e = e ? e : window.event;
        value = e.keyCode;
        if (value == 40 || value == 38) {
            if (menuObj.style.display == 'none') {
                $(selectId + '_ctrl').onclick();
            } else {
                lis = menuObj.getElementsByTagName('li');
                selecti = selectObj.getAttribute('selecti');
                lis[selecti].className = '';
                if (value == 40) {
                    selecti = parseInt(selecti) + 1;
                } else if (value == 38) {
                    selecti = parseInt(selecti) - 1;
                }
                if (selecti < 0) {
                    selecti = lis.length - 1
                } else if (selecti > lis.length - 1) {
                    selecti = 0;
                }
                lis[selecti].className = 'current';
                selectObj.setAttribute('selecti', selecti);
                lis[selecti].parentNode.scrollTop = lis[selecti].offsetTop;
            }
        } else if (value == 13) {
            var lis = menuObj.getElementsByTagName('li');
            lis[selectObj.getAttribute('selecti')].onclick();
        } else if (value == 27) {
            hideMenu(menuObj.id);
        }
    };
}

function _detachEvent(obj, evt, func, eventobj) {
    eventobj = !eventobj ? obj : eventobj;
    if (obj.removeEventListener) {
        obj.removeEventListener(evt, func, false);
    } else if (eventobj.detachEvent) {
        obj.detachEvent('on' + evt, func);
    }
}

function preg_replace(search, replace, str, regswitch) {
    var regswitch = !regswitch ? 'ig' : regswitch;
    var len = search.length;
    for(var i = 0; i < len; i++) {
        re = new RegExp(search[i], regswitch);
        str = str.replace(re, typeof replace == 'string' ? replace : (replace[i] ? replace[i] : replace[0]));
    }
    return str;
}

function preview(previewid, messageid) {
    if (mb_strlen($(previewid).innerHTML) == 0) {
        $(previewid).innerHTML = parseubb($(messageid).value);
        $(previewid).style.display = '';
    }
    else {
        $(previewid).style.display = 'none'
        $(previewid).innerHTML = '';
    }
}

function parseubb(str) {
    str = trim(str);
    if (str == '') {
        return '';
    }

    str = str.replace(/\s*\[code\]([\s\S]+?)\[\/code\]\s*/ig, function ($1, $2) { return parsecode($2); });
    str = str.replace(/\[url\]\s*(www.|https?:\/\/|ftp:\/\/|gopher:\/\/|news:\/\/|telnet:\/\/|rtsp:\/\/|mms:\/\/|callto:\/\/|bctp:\/\/|ed2k:\/\/){1}([^\[\"']+?)\s*\[\/url\]/ig, function ($1, $2, $3) { return cuturl($2 + $3); });
    str = str.replace(/\[url=www.([^\[\"']+?)\](.+?)\[\/url\]/ig, '<a href="http://www.$1" target="_blank">$2</a>');
    str = str.replace(/\[url=(https?|ftp|gopher|news|telnet|rtsp|mms|callto|bctp|ed2k){1}:\/\/([^\[\"']+?)\]([\s\S]+?)\[\/url\]/ig, '<a href="$1://$2" target="_blank">$3</a>');
    str = str.replace(/\[email\](.*?)\[\/email\]/ig, '<a href="mailto:$1">$1</a>');
    str = str.replace(/\[email=(.[^\[]*)\](.*?)\[\/email\]/ig, '<a href="mailto:$1" target="_blank">$2</a>');
    str = str.replace(/\[color=([^\[\<]+?)\]/ig, '<font color="$1">');
    str = str.replace(/\[size=(\d+?)\]/ig, '<font size="$1">');
    str = str.replace(/\[size=(\d+(\.\d+)?(px|pt|in|cm|mm|pc|em|ex|%)+?)\]/ig, '<font style="font-size: $1">');
    str = str.replace(/\[font=([^\[\<]+?)\]/ig, '<font face="$1">');
    str = str.replace(/\[align=([^\[\<]+?)\]/ig, '<p align="$1">');
    str = str.replace(/\[float=([^\[\<]+?)\]/ig, '<br style="clear: both"><span style="float: $1;">');

    re = /\[table(?:=(\d{1,4}%?)(?:,([\(\)%,#\w ]+))?)?\]\s*([\s\S]+?)\s*\[\/table\]/ig;
    for (i = 0; i < 4; i++) {
        str = str.replace(re, function ($1, $2, $3, $4) { return parsetable($2, $3, $4); });
    }

    str = preg_replace([
            '\\\[\\\/color\\\]', '\\\[\\\/size\\\]', '\\\[\\\/font\\\]', '\\\[\\\/align\\\]', '\\\[b\\\]', '\\\[\\\/b\\\]',
            '\\\[i\\\]', '\\\[\\\/i\\\]', '\\\[u\\\]', '\\\[\\\/u\\\]', '\\\[list\\\]', '\\\[list=1\\\]', '\\\[list=a\\\]',
            '\\\[list=A\\\]', '\\\[\\\*\\\]', '\\\[\\\/list\\\]', '\\\[indent\\\]', '\\\[\\\/indent\\\]', '\\\[\\\/float\\\]'
            ], [
            '</font>', '</font>', '</font>', '</p>', '<b>', '</b>', '<i>',
            '</i>', '<u>', '</u>', '<ul>', '<ul type=1 class="litype_1">', '<ul type=a class="litype_2">',
            '<ul type=A class="litype_3">', '<li>', '</ul>', '<blockquote>', '</blockquote>', '</span>'
            ], str, 'g');

    str = str.replace(/\[localimg=(\d{1,4}),(\d{1,4})\](\d+)\[\/localimg\]/ig, function ($1, $2, $3, $4) { if ($('attachnew_' + $4)) { var src = $('attachnew_' + $4).value; if (src != '') return '<img style="filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=\'scale\',src=\'' + src + '\');width:' + $2 + ';height=' + $3 + '" src=\'images/common/none.gif\' border="0" aid="attach_' + $4 + '" alt="" />'; } });
    str = str.replace(/\[img\]\s*([^\[\<\r\n]+?)\s*\[\/img\]/ig, '<img src="$1" border="0" alt="" />');
    //str = str.replace(/\[attachimg\](\d+)\[\/attachimg\]/ig, function ($1, $2) {eval('var attachimg = $(\'preview_' + $2 + '\')');return '<img src="' + attachimg.src + '" border="0" aid="attachimg_' + $2 + '" width="' + attachimg.clientWidth + '" alt="" />';});
    //str = str.replace(/\[attachimg\](\d+)\[\/attachimg\]/ig, '<img src="attachment.aspx?attachmentid=$1" border="0" aid="attachimg_$1" alt="" />');
    str = str.replace(/\[img=(\d{1,4})[x|\,](\d{1,4})\]\s*([^\[\<\r\n]+?)\s*\[\/img\]/ig, '<img width="$1" height="$2" src="$3" border="0" alt="" />');


    for (var i = 0; i <= codecount; i++) {
        str = str.replace("[\tBBX_CODE_" + i + "\t]", codehtml[i]);
    }
    str = preg_replace(['\t', '   ', '  ', '(\r\n|\n|\r)'], ['&nbsp; &nbsp; &nbsp; &nbsp; ', '&nbsp; &nbsp;', '&nbsp;&nbsp;', '<br />'], str);
    return str;
}

function cuturl(url) {
    var length = 65;
    var urllink = '<a href="' + (url.toLowerCase().substr(0, 4) == 'www.' ? 'http://' + url : url) + '" target="_blank">';
    if (url.length > length)
        url = url.substr(0, parseInt(length * 0.5)) + ' ... ' + url.substr(url.length - parseInt(length * 0.3));
    urllink += url + '</a>';
    return urllink;
}


function setScrollToTop(id) {
    window.scroll(0, 0);
    $(id).style.display == '' ? 'none' : '';
}

window.onscroll = function () {
    if ($('scrolltop') != null) {
        if (document.documentElement.scrollTop == 0 && document.body.scrollTop == 0)
            $('scrolltop').style.display = 'none';
        else {
            if (BROWSER.ie && BROWSER.ie < 7) {
                var viewPortHeight = parseInt(document.documentElement.clientHeight);
                var scrollHeight = parseInt(document.body.getBoundingClientRect().top);
                $('scrolltop').style.top = viewPortHeight - scrollHeight - 60 + 'px';
            }
            if ($('wp') != null)
                $('scrolltop').style.left = (parseInt($('wp').clientWidth) + parseInt(fetchOffset($('wp'))['left'])) + 6 + 'px';
            $('scrolltop').style.display = '';
        }
    }
}

function seditor_menu(seditorkey, tag) {
    var sel = false;
    if (!isUndefined($(seditorkey + 'message').selectionStart)) {
        sel = $(seditorkey + 'message').selectionEnd - $(seditorkey + 'message').selectionStart;
    } else if (document.selection && document.selection.createRange) {
        var sel = document.selection.createRange();
        sel = sel.text ? true : false;
    }
    if (sel) {
        seditor_insertunit(seditorkey, '[' + tag + ']', '[/' + tag + ']');
        return;
    }
    var ctrlid = seditorkey + tag;
    var menuid = ctrlid + '_menu';
    if (!$(menuid)) {
        switch (tag) {
            case 'url':
                str = '<div class="pbt">请输入链接地址:</div><div class="pbt"><input type="text" id="' + ctrlid + '_param_1" sautocomplete="off" style="width: 98%" value="http://" class="txt" />' +
                    '</div><div class="pbt">请输入链接文字:</div><div class="pbt"><input type="text" id="' + ctrlid + '_param_2" style="width: 98%" value="" class="txt" /></div>';
                submitstr = "$('" + ctrlid + "_param_2').value !== '' ? seditor_insertunit('" + seditorkey + "', '[url='+$('" + ctrlid + "_param_1').value+']'+$('" + ctrlid + "_param_2').value, '[/url]', null, 1) : seditor_insertunit('" + seditorkey + "', '[url]'+$('" + ctrlid + "_param_1').value, '[/url]', null, 1);hideMenu();";
                break;
            case 'code':
            case 'quote':
                var tagl = { 'quote': '请输入要插入的引用', 'code': '请输入要插入的代码' };
                str = tagl[tag] + ':<br /><textarea id="' + ctrlid + '_param_1" style="width: 98%" cols="50" rows="5" class="txtarea"></textarea>';
                submitstr = "seditor_insertunit('" + seditorkey + "', '[" + tag + "]'+$('" + ctrlid + "_param_1').value, '[/" + tag + "]', null, 1);hideMenu();";
                break;
            case 'img':
                submitstr = "seditor_insertunit('" + seditorkey + "', '[img' + ($('" + ctrlid + "_param_2').value !== '' && $('" + ctrlid + "_param_3').value !== '' ? '='+$('" + ctrlid + "_param_2').value+','+$('" + ctrlid + "_param_2').value : '')+']'+$('" + ctrlid + "_param_1').value, '[/img]', null, 1);hideMenu();";
                str = '<ul style="cursor: move;" class="imguptype" id="e_image_ctrl">' +
                  '<li><a href="javascript:;" hidefocus="true" id="e_btn_attachlist" onclick="switchAttachbutton(\'attachlist\');">已上传列表</a></li>' +
                  '<li><a class="current" href="javascript:;" hidefocus="true" id="e_btn_upload" onclick="switchAttachbutton(\'upload\');">上传附件和图片</a></li>' +
                  '<li><a href="javascript:;" hidefocus="true" id="e_btn_www" onclick="switchAttachbutton(\'www\');">网络图片</a></li>' +
                  '</ul>';
                str += '<div id="e_attachlist" class="upfl hasfsl" style="display:none;">未上传附件和图片，<a href="javascript:;" onclick="switchAttachbutton(\'upload\');" class="xg2">点击这里</a>上传</div>';
                str += '<div id="e_upload" class="filebtn" style="display:none;">' +
                    '<form enctype="multipart/form-data" action="' + rooturl + 'tools/attachupload.aspx?forumid=' + $("fid").value + '" target="uploadattachframe" autocomplete="off" method="post" class="ptm pbm" id="uploadform" fwin="upload">' +
                    '<div class="filebtn"><input type="file" onchange="uploadWindowstart();$(\'uploadform\').submit()" size="1" class="pf cur1" id="filedata" name="Filedata" fwin="upload" multiple="multiple">' +
                    '<button class="pn pnc" type="button"><strong>浏览</strong></button></div>' +
                    '</form></div><p style="visibility:hidden;text-align: center;" id="uploading"><img style="vertical-align: middle;" src="images/common/uploading.gif"> 上传中，请稍候</p>' +
                    '<iframe style="display: none;" onload="uploadWindowload();" id="uploadattachframe" name="uploadattachframe" fwin="upload"></iframe>';
                str += '<div id="e_www"><form onsubmit="' + submitstr + ';return false;" autocomplete="off"><div class="pbt">请输入图片地址:</div><div class="pbt"><input type="text" id="' + ctrlid + '_param_1" style="width: 98%" value="http://" class="txt" onchange="loadimgsize(this.value, \'' + seditorkey + '\',\'' + tag + '\')" /></div>' +
                    '<div class="pbt"><p class="mtm">宽(可选): <input type="text" id="' + ctrlid + '_param_2" style="width: 15%" value="" class="txt" /> &nbsp;' +
                    '高(可选): <input type="text" id="' + ctrlid + '_param_3" style="width: 15%" value="" class="txt" /></p></div><div class="pns mtn"><button type="submit" id="' + ctrlid + '_submit" class="pn pnc"><strong>提交</strong></button><button type="button" onClick="hideMenu()" class="pn"><em>取消</em></button></div></form></div>';
                break;
        }
        var menu = document.createElement('div');
        menu.id = menuid;
        menu.style.display = 'none';
        menu.className = 'popupmenu_popup upf';
        menu.style.width = '320px';
        $('append_parent').appendChild(menu);
        //menu.innerHTML = '<span class="y" style="margin-top:-4px;"><a onclick="hideMenu()" class="flbc" href="javascript:;">关闭</a></span><div class="p_opt cl"><form onsubmit="' + submitstr + ';return false;" autocomplete="off"><div>' + str + '</div><div class="pns mtn"><button type="submit" id="' + ctrlid + '_submit" class="pn pnc"><strong>提交</strong></button><button type="button" onClick="hideMenu()" class="pn"><em>取消</em></button></div></form></div>';
        menu.innerHTML = '<span class="y" style="margin-top:-4px;"><a onclick="hideMenu()" class="flbc" href="javascript:;">关闭</a></span><div class="p_opt cl">' + str + '</div>';
        
    }
    showMenu({ 'ctrlid': ctrlid, 'evt': 'click', 'duration': 3, 'cache': 0, 'drag': 1 });
    if(tag == "img")
        updateAttachListbycount(1, true);
} 

function switchAttachbutton(btn) {
    var btns = ['www','attachlist','upload'];
    switchButton(btn, btns);
} 

function widthauto(obj,cssdir) {
    if (!$('css_widthauto') || $('css_widthauto').disabled) {//切换到窄版
        if (!$('css_widthauto')) {//创建元素
            var widthcss = document.createElement('link');
            widthcss.type = 'text/css';
            widthcss.rel = 'stylesheet';
            widthcss.href = cssdir + '/widthauto.css';
            widthcss.id = "css_widthauto";
            var headNode = document.getElementsByTagName("head")[0];
            headNode.appendChild(widthcss);
        }
        else {
            $('css_widthauto').disabled = false;
        }
        setcookie('allowchangewidth', 0, 86400 * 30);
        obj.innerHTML = '切换到宽版';
    } else {//切换到宽版
        $('css_widthauto').disabled = true;
        setcookie('allowchangewidth', 1, 86400 * 30);
        obj.innerHTML = '切换到窄版';
    }
    hideMenu(obj.id, "menu");
    window.setTimeout("setMenuPosition('pm_ntc', 'pm_ntc_menu', '43')", 100);
}

function htmlspecialchars(str) {
        return preg_replace(['&', '<', '>', '"'], ['&amp;', '&lt;', '&gt;', '&quot;'], str);
}

/**
* 显示Tip信息
* ctrlobj 弹出提示信息的对象
*/
function showTip(ctrlobj) {
 if(!ctrlobj.id) {
 ctrlobj.id = 'tip_' + Math.random();
 }
 menuid = ctrlobj.id + '_menu';
 if(!$(menuid)) {
 var div = document.createElement('div');
 div.id = ctrlobj.id + '_menu';
 div.className = 'prmm up';
 div.style.display = 'none';
 div.innerHTML = '<div class="prmc"><ul><li>' + ctrlobj.getAttribute('tip') + '</li></ul></div>';
 $('append_parent').appendChild(div);
 }
 showMenu({'mtype':'prompt','ctrlid':ctrlobj.id,'pos':'210','duration':2,'timeout':250,'zindex':JSMENU['zIndex']['prompt']});
} 

function closenotice(pmcount){
    setcookie('shownotice', 0, 86400 * 30);
    setcookie('newpms', pmcount, 86400 * 30);
}

function fastsubmit(seditorid) {
    var message = $(seditorid + 'message').value;
    if (message != '') {
        var iframe = document.createElement('IFRAME');
        iframe.src = forumpath + 'userdatahub.aspx?formname=' + seditorid + 'form';
        iframe.style.display = 'none';
        if (iframe.addEventListener) iframe.addEventListener("load", function () { window.document.getElementById(seditorid + 'form').submit(); }, false);
        else if (iframe.attachEvent) iframe.attachEvent("onload", function () { window.document.getElementById(seditorid + 'form').submit(); });
        else iframe.onload = function () { window.document.getElementById(seditorid + 'form').submit(); };
        $('append_parent').appendChild(iframe);
    }
    else {
        $(seditorid + 'form').submit();
    }
}

//检查标题长度
function checkLength(which, maxChars) {
    which.onkeyup = function on() {
        if (which.value.length > maxChars) {
            which.value = which.value.substring(0, maxChars);
            alert("标题已超过" + maxChars + "个字符，超出部分将自动截取");
        }
        else
            document.getElementById("chLeft").innerHTML = maxChars - which.value.length;
    }
    which.onblur = function on() {
        if (which.value.length > maxChars) {
            which.value = which.value.substring(0, maxChars);
            alert("标题已超过" + maxChars + "个字符,超出部分将自动截取");
        }
        else
            document.getElementById("chLeft").innerHTML = maxChars - which.value.length;
    }
}                                                                 