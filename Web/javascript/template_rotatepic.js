window.System = function(){this.setHashCode();}

System.debug=false; //false
System._codebase={};
try
{
  if (window!=parent && parent.System && parent.System._codebase)
    System._codebase = parent.System._codebase;
  else if ("undefined"!=typeof opener&&opener.System&&opener.System._codebase)
    System._codebase = opener.System._codebase;
  else if ("undefined"!=typeof dialogArguments && dialogArguments.System)
    System._codebase = dialogArguments.System._codebase;
}
catch(ex){}

System.MISSING_ARGUMENT="Missing argument";
System.ARGUMENT_PARSE_ERROR="The argument cannot be parsed";
System.NOT_SUPPORTED_XMLHTTP="Your browser do not support XMLHttp";
System.FILE_NOT_FOUND="File not found";
System.MISCODING="Maybe file encoding is not ANSI or UTF-8";
System.NAMESPACE_ERROR=" nonstandard namespace";

System.hashCounter=0;
System.currentVersion="20070228_2";
var t=document.getElementsByTagName("SCRIPT");
t=(System.scriptElement=t[t.length-1]).src.replace(/\\/g, "/");
System.extend=function(d,s){for(var i in s)d[i]=s[i];return d;};
System.path=(t.lastIndexOf("/")<0)?".":t.substring(0, t.lastIndexOf("/"));
System.resourcePath=System.path +"/System/_resource";
System.getUniqueId=function(){return "mz_"+(System.hashCounter++).toString(36);};
System.toHashCode=function(e)
{
  if("undefined"!=typeof e.hashCode) return e.hashCode;
  return e.hashCode=System.getUniqueId();
};
System.supportsXmlHttp=function()
{
  return "object"==typeof(System._xmlHttp||(System._xmlHttp=new XMLHttpRequest()));
};
System._getPrototype=function(namespace, argu)
{
  if("undefined"==typeof System._prototypes[namespace])return new System();
  for(var a=[], i=0; i<argu.length; i++) a[i]="argu["+ i +"]";
  return eval("new (System._prototypes['"+namespace+"'])("+a.join(",")+")");
};
System.ie=navigator.userAgent.indexOf("MSIE")>0 && !window.opera;
System.ns=navigator.vendor=="Netscape";
System.alert=function(msg){if(System.debug)alert(msg);};
System._parseResponseText=function(s)
{
  if(null==s||"\uFFFD"==s.charAt(0)){System.alert(System.MISCODING);return "";}
  if("\xef"==s.charAt(0))s=s.substr(3); //for firefox
  return s.replace(/(^|\n)\s*\/\/+\s*((Using|Import|Include)\((\"|\'))/g,"$1$2");
};

if(!window.XMLHttpRequest && window.ActiveXObject)
{
  window.XMLHttpRequest = function()
  {
    var msxmls=['MSXML3','MSXML2','Microsoft'];
    for(var i=0;i<msxmls.length;i++)
      try{return new ActiveXObject(msxmls[i]+'.XMLHTTP')} catch(ex){}
    System._xmlHttp="mz"; throw new Error(System.NOT_SUPPORTED_XMLHTTP);
  }
}
System.load = function(namespace, path)
{
  try
  {
    if(System.supportsXmlHttp()){path=System._mapPath(namespace, path);
    var x=System._xmlHttp; x.open("GET",path,false); x.send(null);
    if (x.readyState==4){if(x.status==0||/^file\:/i.test(path))
      return System._parseResponseText(x.responseText);
    else if(x.status==200)return System._parseResponseText(x.responseText);
    else if(x.status==404)System.alert(namespace+"\n"+System.FILE_NOT_FOUND);
    else throw new Error(x.status +": "+ x.statusText);}}
    else System.alert(System.NOT_SUPPORTED_XMLHTTP);
  }
  catch(ex){System.alert(namespace+"\n"+ex.message);}return "";
};
System._exist = function(namespace, path)
{

  if("undefined"==typeof System._existences[namespace]) return false;
  return System._existences[namespace]==System._mapPath(namespace,path);
};
System._mapPath = function(namespace, path)
{
  if("string"==typeof path && path.length>3) return path;
  var p=System.path +"/"+ namespace.replace(/\./g,"/") +".js";
  return p +(("undefined"==typeof path||path) ? "" : "?t="+ Math.random());
};

window.Using = function(namespace, path, rename)
{
  var N=namespace, C=N.substr(N.lastIndexOf(".")+1), code=N+".";
  if(System._exist(N,path)){window[C]=System._prototypes[N];return}
  if(!/((^|\.)[\w\$]+)+$/.test(N))throw new Error(N+System.NAMESPACE_ERROR);

  for(var s,e,i=code.indexOf("."); i>-1; i=code.indexOf(".", i+1)){
  e=code.substring(0,i);s=(e.indexOf(".")==-1)?"window[\""+e+"\"]":e;
  if(e&&"undefined"==typeof(s)){eval(s+"=function(){return "
    +"System._getPrototype(\""+e+"\", arguments)}");}}

  if("string"!=typeof System._codebase[N]&&(code=System.load(N,path)))
    System._codebase[N]=code+";\r\nSystem._prototypes['"+
    N+"']=window['"+(rename||C)+"']="+C;code="";
  System._existences[N]=System._mapPath(N, path);

  if("string"==typeof (s=System._codebase[N]))try{(new Function(s))()}
  catch(e){System.alert("Syntax error on load "+ N +"\n"+ e.message);}
  //alert(System._codebase[namespace])
};
window.Import=function(namespace,path,rename){Using(namespace,path,rename)};
window.Instance=function(hashCode){return System._instances[hashCode]};
window.Include=function(namespace, path)
{
  var N=namespace,code; if(System._exist(N, path)) return;
  if(!/((^|\.)[\w\$]+)+$/.test(N))throw new Error(N+System.NAMESPACE_ERROR);

  if("string"!=typeof System._codebase[N])if(System.supportsXmlHttp()
    && (code=System.load(N, path))) System._codebase[N]=code;
  System._existences[N]=System._mapPath(N, path);

  var B=("string"==typeof(System._codebase[N]));try{
  if(window.execScript&&B)window.execScript(System._codebase[N]);else
  {
    var s=document.createElement("SCRIPT");s.type="text/javascript";
    if(B)s.innerHTML="eval(System._codebase['"+N+"']);";
    else s.src=System._existences[N]=System._mapPath(N,path);
    document.getElementsByTagName("HEAD")[0].appendChild(s);
    setTimeout(function(){s.parentNode.removeChild(s)},99);}
  }catch(B){System.alert("Syntax error on include "+N+"\n"+B.message);}
};
Function.READ=1;Function.WRITE=2;Function.READ_WRITE=3;
Function.prototype.addProperty=function(name,initValue,r_w)
{
  var capital=name.charAt(0).toUpperCase()+name.substr(1);
  r_w=r_w||Function.READ_WRITE; name="_"+name; var p=this.prototype;
  if("undefined"!=typeof initValue) p[name]=initValue;
  if(r_w&Function.READ) p["get"+ capital]=function(){return this[name];};
  if(r_w&Function.WRITE) p["set"+ capital]=function(v){this[name]=v;};
};
Function.prototype.Extends=function(SuperClass,ClassName)
{
  var op=this.prototype, i, p=this.prototype=new SuperClass();
  if(ClassName)p._className=ClassName; for(i in op)p[i]=op[i];
  if(p.hashCode)delete System._instances[p.hashCode];return p;
};
System._instances={};
System._prototypes=
{
  "System":System,
  "System.Object":System,
  "System.Event":System.Event
};
System._existences=
{
  "System":System._mapPath("System"),
  "System.Event":System._mapPath("System.Event"),
  "System.Object":System._mapPath("System.Object")
};
t=System.Extends(Object, "System"); System.Object = System;
t.decontrol=function(){var t;if(t=this.hashCode)delete System._instances[t]};
t.addEventListeners=function(type, handle)
{
  if("function"!=typeof handle)
    throw new Error(this+" addEventListener: "+handle+" is not a function");
  if(!this._listeners) this._listeners={};
  var id=System.toHashCode(handle), t=this._listeners; 
  if("object"!=typeof t[type]) t[type]={}; t[type][id]=handle;
};
t.removeEventListener=function(type, handle)
{
  if(!this._listeners)this._listeners={};var t=this._listeners;
  if(!t[type]) return; var id=System.toHashCode(handle);
  if( t[type][id])delete t[type][id];if(t[type])delete t[type];
};
t.dispatchEvent=function(evt)
{
  if(!this._listeners)this._listeners={};
  var i, t =this._listeners, p =evt.type;
  evt.target=evt.srcElement=evt.target||evt.srcElement||this;
  evt.currentTarget=this; if(this[p])this[p](evt);
  if("object"==typeof t[p]) for(i in t[p]) t[p][i].call(null, evt);
  delete evt.target;delete evt.currentTarget;delete evt.srcElement;
  return evt.returnValue;
};
t.setHashCode=function()
{
  System._instances[(this.hashCode=System.getUniqueId())]=this;
};
t.getHashCode=function()
{
  if(!this.hashCode)this.setHashCode(); return this.hashCode;
};
t.toString=function(){return "[object "+(this._className||"Object")+"]";};
System.getType=function(e)
{
  if("object"!=typeof(e))return typeof(e);
  if("[object Object]"==e)return "object";
  if(/\[object\s+([^\s\]]+)\]/.test(e))return RegExp.$1;
  else return "object";
};
System.Event=function(type){this.type=type;};
t=System.Event.Extends(System,"System.Event");
t.returnValue=true;t.cancelBubble=false;
t.target=t.currentTarget=t.srcElement=null;
t.stopPropagation=function(){this.cancelBubble=true;};
t.preventDefault =function(){this.returnValue=false;};

if(System.ie && !System.debug) Include("System.Plugins.IE");//IE UserData
if(window.opera) Include("System.Plugins.Opera"); //Opera support
//Include("System.Global");

//Include("Csdn.Common");


/***  Global.js  ***/

/*-=< base function >=-*/
//HTMLElement.getElementById extend
if(document && !document.getElementById){document.getElementById=function(id){
if(document.all) return document.all(id); return null;}}


/*-=< HTMLElement >=-*/
if(typeof(HTMLElement)!="undefined" && !window.opera)
{
  var t=HTMLElement.prototype;
  t.contains=function(e){do if(e==this)return true;while(e=e.parentNode);return false;};
  t.__defineGetter__("outerHTML",function()
  {
    var a=this.attributes, str="<"+this.tagName, i=0;for(;i<a.length;i++)
    if(a[i].specified) str+=" "+a[i].name+'="'+a[i].value+'"';
    if(!this.canHaveChildren) return str+" />";
    return str+">"+this.innerHTML+"</"+this.tagName+">";
  });
  t.__defineSetter__("outerHTML",function(s)
  {
    var r = this.ownerDocument.createRange();
    r.setStartBefore(this);
    r = r.createContextualFragment(s);
    this.parentNode.replaceChild(r, this);
    return s;
  });
  t.__defineGetter__("canHaveChildren",function()
  {
    switch(this.tagName.toLowerCase())
    {
      case "area": case "base":  case "basefont":
      case "col":  case "frame": case "hr":
      case "img":  case "br":    case "input":
      case "link": case "meta":  case "isindex":
      case "param":return false;
    } return true;
  });
  t.__defineGetter__("currentStyle", function()
  {
    return this.ownerDocument.defaultView.getComputedStyle(this,null);
  });
  t.__defineGetter__("children",function()
  {
    for(var a=[],j=0,n,i=0; i<this.childNodes.length; i++){
    n=this.childNodes[i];if(n.nodeType==1){a[j++]=n;if(n.name){
    if(!a[n.name])a[n.name]=[]; a[n.name][a[n.name].length]=n;}
    if(n.id) a[n.id]=n;}}return a;
  });
  t.insertAdjacentHTML=function(where, html)
  {
    var e=this.ownerDocument.createRange();
    e.setStartBefore(this);
    e=e.createContextualFragment(html);
    switch (where)
    {
      case 'beforeBegin': this.parentNode.insertBefore(e, this);break;
      case 'afterBegin': this.insertBefore(e, this.firstChild); break;
      case 'beforeEnd': this.appendChild(e); break;
      case 'afterEnd':
        if(!this.nextSibling) this.parentNode.appendChild(e);
        else this.parentNode.insertBefore(e, this.nextSibling); break;
    }
  };
};
if(!window.attachEvent && window.addEventListener)
{
  Window = {
	attachEvent:function(){},
	detachEvent:function(){}
  };
  Window.attachEvent = HTMLDocument.prototype.attachEvent=
  t.attachEvent=function(en, func, cancelBubble)
  {
    var cb = cancelBubble ? true : false;
    this.addEventListener(en.toLowerCase().substr(2), func, cb);
  };
  Window.detachEvent = HTMLDocument.prototype.detachEvent=
  t.detachEvent=function(en, func, cancelBubble)
  {
    var cb = cancelBuble ? true : false;
    this.removeEventListener(en.toLowerCase().substr(2), func, cb);
  };
}
if(typeof Event!="undefined" && !System.ie && !window.opera)
{
  var t=Event.prototype;
  t.__defineSetter__("returnValue", function(b){if(!b)this.preventDefault();  return b;});
  t.__defineSetter__("cancelBubble",function(b){if(b) this.stopPropagation(); return b;});
  t.__defineGetter__("offsetX", function(){return this.layerX;});
  t.__defineGetter__("offsetY", function(){return this.layerY;});
  t.__defineGetter__("srcElement", function(){var n=this.target; while (n.nodeType!=1)n=n.parentNode;return n;}); 
}

/*-=< Function >=-*/
//apply and call
if(typeof(Function.prototype.apply)!="function")
{
  Function.prototype.apply = function(obj, argu)
  {
    if(obj) obj.constructor.prototype.___caller = this;
    for(var a=[], i=0; i<argu.length; i++) a[i] = "argu["+ i +"]";
    var t = eval((obj ? "obj.___caller" : "this") +"("+ a.join(",") +");");
    if(obj) delete obj.constructor.prototype.___caller; return t;};
    Function.prototype.call = function(obj){
    for(var a=[], i=1; i<arguments.length; i++) a[i-1]=arguments[i];
    return this.apply(obj, a);
  }; 
}

/*-=< Array >=-*/
var t = Array.prototype;
//[extended method] push  insert new item
if(typeof(t.push)!="function")
{
  t.push = function()
  {
    for (var i=0; i<arguments.length; i++)
      this[this.length] = arguments[i];
    return this.length; 
  };
}
//[extended method] shift  delete the first item
if(typeof(t.shift)!="function")
{
  t.shift = function()
  {
    var mm = null;
    if(this.length>0)
    {
      mm = this[0]; for(var i=1; i<this.length; i++)
      this[i-1]=this[i]; this.length=this.length -1;
    }
    return mm;
  };
}
//[extended method] unique  Delete repeated item
t.unique = function()
{
  for(var a={}, i=0; i<this.length; i++)
  {
    if(typeof(a[this[i]])=="undefined") a[this[i]] = 1;
  }
  this.length=0;
  for(i in a) this[this.length] = i; return this;
};
//[extended method] indexOf
if(typeof(t.indexOf)!="function")
{
  t.indexOf=function(item, start)
  {
    start=start||0; if(start<0)start=Math.max(0,this.length+start);
    for(var i=start;i<this.length;i++){if(this[i]===item)return i;}
    return -1;
  };
}
t.include=function(e){return this.indexOf(e)!=-1};
t.remove=function(e)
{
  for(var i=0,n=this.length,a=[]; i<n; i++) if(this[i]!=e) a[a.length]=this[i];
  return a;
}

/*-=< Date >=-*/
//datetime format
Date.prototype.format = function(format)
{
  var o = {
    "M+" : this.getMonth()+1, //month
    "d+" : this.getDate(),    //day
    "h+" : this.getHours(),   //hour
    "m+" : this.getMinutes(), //minute
    "s+" : this.getSeconds(), //second
    "q+" : Math.floor((this.getMonth()+3)/3),  //quarter
    "S" : this.getMilliseconds() //millisecond
  }
  if(/(y+)/.test(format)) format=format.replace(RegExp.$1,
    (this.getFullYear()+"").substr(4 - RegExp.$1.length));
  for(var k in o)if(new RegExp("("+ k +")").test(format))
    format = format.replace(RegExp.$1,
      RegExp.$1.length==1 ? o[k] : 
        ("00"+ o[k]).substr((""+ o[k]).length));
  return format;
};

/*-=< Number >=-*/
if(typeof(Number.prototype.toFixed)!="function")
{
    Number.prototype.toFixed = function(d)
    {
        var s=this+"";if(!d)d=0;
        if(s.indexOf(".")==-1)s+=".";s+=new Array(d+1).join("0");
        if (new RegExp("^(-|\\+)?(\\d+(\\.\\d{0,"+ (d+1) +"})?)\\d*$").test(s))
        {
            var s="0"+ RegExp.$2, pm=RegExp.$1, a=RegExp.$3.length, b=true;
            if (a==d+2){a=s.match(/\d/g); if (parseInt(a[a.length-1])>4)
            {
                for(var i=a.length-2; i>=0; i--) {a[i] = parseInt(a[i])+1;
                if(a[i]==10){a[i]=0; b=i!=1;} else break;}
            }
            s=a.join("").replace(new RegExp("(\\d+)(\\d{"+d+"})\\d$"),"$1.$2");
        }if(b)s=s.substr(1);return (pm+s).replace(/\.$/, "");} return this+"";
    };
}

/*-=< Global >=-*/
if("undefined"==typeof(encodeURIComponent))encodeURIComponent=function(s){return escape(s);}

/*-=< String >=-*/
var t=String.prototype;
t.trim=function(){return this.replace(/(^[\s\t\xa0\u3000]+)|([\u3000\xa0\s\t]+$)/g, "")};
t.capitalize=function(){return this.charAt(0).toUpperCase() + this.substr(1);};
t.getByteLength=function(){return this.replace(/[^\x00-\xff]/g, "mm").length;};
t.getAttribute = function(attribute)
{
  if(new RegExp("(^|;)\\s*"+attribute+"\\s*:\\s*([^;]*)\\s*(;|$)","i").test(this))
  return RegExp.$2.replace(/%3B/gi,";").replace(/%25/g,"%"); return null;
};
t.setAttribute = function(attribute, value)
{
  value=(""+value).replace(/%/g,"%25").replace(/;/g,"%3B").replace(/\r|\n/g,"");
  return (attribute +":"+ value +";" + this);
};
t.deleteAttribute = function(attribute)
{
  return this.replace(new RegExp("\\b\\s*"+attribute+"\\s*:\\s*([^;]*)\\s*(;|$)","gi"),"");
};
t.getQueryString = function(name)
{
  var reg = new RegExp("(^|&|\\?)"+ name +"=([^&]*)(&|$)"), r;
  if (r=this.match(reg)) return unescape(r[2]); return null;
};
t.sub = function(n)
{
  var r = /[^\x00-\xff]/g;
  if(this.replace(r, "mm").length <= n) return this;
  n = n - 3;
  var m = Math.floor(n/2);
  for(var i=m; i<this.length; i++)
  {
    if(this.substr(0, i).replace(r, "mm").length>=n)
    {
      return this.substr(0, i) +"...";
    }
  }
  return this;
};
t.format=function()
{
  if(arguments.length==0) return this;
  for(var s=this, i=0; i<arguments.length; i++)
    s=s.replace(new RegExp("\\{"+i+"\\}","g"), arguments[i]);
  return s;
};
String.format=function(str)
{
  if("string"!=typeof(str)||arguments.length<=1) return str;
  for(var i=1; i<arguments.length; i++)
    str=str.replace(new RegExp("\\{"+(i-1)+"\\}","g"), arguments[i]);
  return str;
};

/*-=< Meizz Class >=-*/
//NameSpace: System.MzBrowser
System.MzBrowser=window["MzBrowser"]={};(function()
{
  if(MzBrowser.platform) return;
  var ua = window.navigator.userAgent;
  MzBrowser.platform = window.navigator.platform;

  MzBrowser.firefox = ua.indexOf("Firefox")>0;
  MzBrowser.opera = typeof(window.opera)=="object";
  MzBrowser.ie = !MzBrowser.opera && ua.indexOf("MSIE")>0;
  MzBrowser.mozilla = window.navigator.product == "Gecko";
  MzBrowser.netscape= window.navigator.vendor=="Netscape";
  MzBrowser.safari  = ua.indexOf("safari")>-1&&window.Dom;

  if(MzBrowser.firefox) var re = /Firefox(\s|\/)(\d+(\.\d+)?)/;
  else if(MzBrowser.ie) var re = /MSIE( )(\d+(\.\d+)?)/;
  else if(MzBrowser.opera) var re = /Opera(\s|\/)(\d+(\.\d+)?)/;
  else if(MzBrowser.netscape) var re = /Netscape(\s|\/)(\d+(\.\d+)?)/;
  else if(MzBrowser.mozilla) var re = /rv(\:)(\d+(\.\d+)?)/;

  if("undefined"!=typeof(re)&&re.test(ua))
    MzBrowser.version = parseFloat(RegExp.$2);
})();
//alert(MzBrowser.version);

/*-=< Extend >=-*/
System.loadCssFile=function(cssPath, uniqueId)
{
  if(/\w+\.\w+(\?|$)/.test(cssPath))
  {
    if(!("string"==typeof uniqueId && uniqueId!=""))
    uniqueId = "MzCss_"+ cssPath.replace(/\W/g, "");
    if(document.getElementById(uniqueId)) return;

    var link  = document.createElement("LINK");
    link.href = cssPath;
    link.id   = uniqueId;
    link.type = "text/css";
    link.rel  = "Stylesheet";
    uniqueId  = document.getElementsByTagName("HEAD")[0];
    uniqueId.insertBefore(link, uniqueId.firstChild);
  }
};
System.zIndexBase=
{
   "MzForm": 52000 //foused 56000-60000
  ,"dragLayer": 65000
};
System.disabledList={};

var t=window.MzElement=System.MzElement={};
var $=t.check=function()
{
  for(var i=0, a=[]; i<arguments.length; i++)
  {
    var e=arguments[i]; a[i]=null
    if("object"==typeof(e) && e.tagName && e!=window) a[i]=e;
    if("string"==typeof(e) &&(e=document.getElementById(e)))a[i]=e;
  }
  return a.length<2 ? a[0] : a;
};
t.hide=function()
{
  for (var e=null, n=arguments.length, i=0; i<n; i++)
  if(e=MzElement.check(arguments[i])) e.style.display="none";
};
t.show=function()
{
  for (var e=null, n=arguments.length, i=0; i<n; i++)
  if(e=MzElement.check(arguments[i])) e.style.display="";
};
t.remove=function()
{
  for (var e=null, n=arguments.length, i=0; i<n; i++)
  if(e=MzElement.check(arguments[i])) e.parentNode.removeChild(e);
};
t.realOffset=function(o)
{
  if(!o) return null; var e=o, x=y=l=t=0, doc=MzElement.body();
  do{l+=e.offsetLeft||0; t+=e.offsetTop||0; e=e.offsetParent;}while(e);
  do{x+=o.scrollLeft||0; y+=o.scrollTop||0; o=o.parentNode;}while(o);
  return {"x":l-x+doc.scrollLeft, "y":t-y+doc.scrollTop};
};
t.searchByTagName=function(e, TAG)
{
  do if(e.tagName==TAG.toUpperCase())return e;
  while (e=e.parentNode); return null;
}
t.body=function()
{
  var W, H, SL, ST;
  var w=window, d=document, dd=d.documentElement;

  if(w.innerWidth) W=w.innerWidth;
  else if(dd&&dd.clientWidth) W=dd.clientWidth;
  else if(d.body) W=d.body.clientWidth;

  if(w.innerHeight) H=w.innerHeight;
  else if(dd&&dd.clientHeight) H=dd.clientHeight; 
  else if(d.body) H=d.body.clientHeight;

  if(w.pageXOffset) SL=w.pageXOffset;
  else if(dd&&dd.scrollLeft) SL=dd.scrollLeft;
  else if(d.body) SL=d.body.scrollLeft;

  if(w.pageYOffset) ST=w.pageYOffset;
  else if(dd&&dd.scrollTop) ST=dd.scrollTop;
  else if(d.body) ST=d.body.scrollTop;

  return {"scrollTop":ST,"scrollLeft":SL,"clientWidth":W,"clientHeight":H};
}
t.hasClassName=function(element, className)
{
  if(!(element=MzElement.check(element))) return;
  return element.className.split(" ").include(className);
};
t.addClassName=function(element, className)
{
  if(!(element=MzElement.check(element))) return;
  var a=element.className.split(" ");
  if(!a.include(className))a=a.concat(className);
  element.className = a.join(" ").trim(); a=null;
};
t.removeClassName=function(element, className)
{
  if(!(element=MzElement.check(element))) return;
  var r=new RegExp("(^| )"+ className +"( |$)", "g");
  element.className=element.className.replace(r, "$2");
};t=null;



/*** MzDataProvider.js ***/

/*---------------------------------------------------------------------------*\
|  Subject:    DataProvider for Meizz WebControl
|  NameSpace:  System.Data.MzDataProvider
|  Author:     meizz
|  Created:    2005-12-28
|  Version:    2007-02-07
\*---------------------------------------------------------------------------*/

//text,description,icon,url,target,JSData,XMLData,ULData
function MzDataProvider(){System.call(this);}
t=MzDataProvider.Extends(System, "MzDataProvider");
t.__="\x0f";
t.rootId="-1";
t.dividerEncoding=t.divider="_";
t.indexes=t.jsDataPath=t.xmlDataPath="";

t.appendIndexes=function(s){this.indexes += this.__+ s +this.__;}
t.getUniqueId=function(){return "MzDataProvider"+(MzDataProvider.nodeCounter++).toString(36);};
MzDataProvider.nodeCounter=0;

t.nodePrototype=window.MzDataNode = function()
{
  this.index= (MzDataProvider.nodeCounter++).toString(36);
  this.childNodes=[];
};
t=MzDataNode.Extends(System, "MzDataNode");
t.text = t.path = t.sourceIndex="";
t.isLoaded = t.hasChild= false;
t.parentNode = t.$$caller = null;  //instance of System.Data.MzDataProvider

//public
MzDataProvider.prototype.setDivider=function(d)
{
  this.divider=d; for(var a="", i=0; i<d.length; i++)
  a+=("\'^{[(\\-|+.*?)]}$\"".indexOf(d.charAt(i))>-1?"\\":"")+d.charAt(i);
  this.dividerEncoding = a;
};
MzDataProvider.prototype.setAsyncJsDataPath=function(path)
{
  if(path.length>0) this.jsDataPath = path.replace(/[\\\/]*$/, "/");
}
MzDataProvider.prototype.setAsyncXmlDataPath=function(path)
{
  if(path.length>0) this.xmlDataPath = path.replace(/[\\\/]*$/, "/");
}
MzDataProvider.prototype.render=function(){this.dataInit();};
//private: initialize data node
MzDataProvider.prototype.dataInit = function()
{
  if(this._dataInitialized) return;
  if("object"!=typeof(this.nodes)) this.nodes={};
  if("object"!=typeof(this.dataSource)) this.dataSource={};
  if("object"!=typeof(this.indexMapping)) this.indexMapping={};

  var _=this.__, d=this.dividerEncoding, a=[], i;

  for(i in this.dataSource)a[a.length]=i;this.appendIndexes(a.join(_));
  this.dataSource.length=(this.dataSource.length||0)+ a.length;

  a=(MzDataProvider.nodeCounter++).toString(36);
  var node=this.nodes[a]=this.rootNode = new this.nodePrototype; //this.imaginaryNode
  node.$$caller=this;node.index=a;node.virgin=this.rootId=="-1";

  if(node.virgin)
  {
    node.id=node.path="-1";
    node.loadChildNodes();
    node.hasChildNodes();
  }
  else
  {
    a=new RegExp("([^"+_+d+"]+)"+ d +"("+ this.rootId +")("+_+"|$)");
    if(a.test(this.indexes))
    {
      a=RegExp.$1 + this.divider + this.rootId;
      node.childNodes[0]=node.DTO(this.nodePrototype, a);
      node.isLoaded=true; node.hasChild=true;
    }
  }
  this._dataInitialized=true;
};
//public: append data onafterload
MzDataProvider.prototype.appendData = function(data, override) //param data: json
{
  if("object"!=typeof this.dataSource) this.dataSource={}; var a=[],i;
  for(i in data)if(!this.dataSource[i]){this.dataSource[i]=data[i];a[a.length]=i}
  if(this._dataInitialized) this.appendIndexes(a.join(this.__));
  this.dataSource.length=(this.dataSource.length||0)+a.length;data=null;a=null;
};
//public: getNode (has Builded) by sourceId
MzDataProvider.prototype.getNodeById = function(id)
{
  if(id==this.rootId&&this.rootNode.virgin) return this.rootNode;
  var _=this.__, d = this.dividerEncoding;
  var reg=new RegExp("([^"+_+ d +"]+"+ d + id +")("+_+"|$)");
  if(reg.test(this.indexes)){var s=this.indexMapping[RegExp.$1];
  //alert(s);
  if("string"==typeof(s)) return this.nodes[s.getAttribute("index_"+this.hashCode)];
  //if(reg.test(this.indexes)){var s=this.dataSource[RegExp.$1];
  //if("string"==typeof(s)) return this.nodes[s.getAttribute("index_"+this.hashCode)];
  //else if("object"==typeof(s)) return this.nodes[s["index_"+this.hashCode]];
  else{System.alert("The node isn't initialized!"); return null;}}
  alert("sourceId="+ id +" is nonexistent!"); return null;
};
//public: asynchronous get childNodes from JS File
MzDataProvider.prototype.loadJsData = function(JsFileUrl)
{
  var js; try{if(js = System.load("",JsFileUrl)){var d=eval(js);
  if("object"!=d && "object"==typeof(data) && null!=data)d=data;
  this.appendData(d); data=d=null;}}catch(e){}
};
//public: asynchronous get childNodes from Json File
MzDataProvider.prototype.loadJsonData = function(JsonFileUrl)
{
  MzDataProvider.instance=this;
  if("undefined"==typeof(MzJson))Using("System.Net.MzJson");
  var e=new MzJson();  e.cache = /\.js$/i.test(JsonFileUrl);
  e.request(JsonFileUrl); e=null;
};
window.MzJsonDataLoad=function(json)
{
  if(MzDataProvider.instance) MzDataProvider.instance.appendData(json);
  MzDataProvider.instance=null;
}
//public: asynchronous get childNodes from XML File
MzDataProvider.prototype.loadXmlData = function(url, parentId, mapping)
{
  if(System.supportsXmlHttp())
  {
    //Using("System.Xml.MzXmlDocument");
    if("undefined"==typeof parentId) parentId=this.rootId;
    var x=new MzXmlDocument(); x.async=false; x.load(url);
    if(x.readyState==4)
    {
      if(!x.documentElement)
        alert("xmlDoc.documentElement = null, Please update your browser");
      this._loadXmlNodeData(x.documentElement, parentId, mapping);
    }
  }
};
//public: asynchronous get childNodes from XML String
MzDataProvider.prototype.loadXmlDataString = function(xmlString, parentId, mapping)
{
  if(System.supportsXmlHttp())
  {
    //Using("System.Xml.MzXmlDocument");
    if("undefined"==typeof parentId) parentId=this.rootId;
    var x=new MzXmlDocument(); x.loadXML(xmlString);
    this._loadXmlNodeData(x.documentElement, parentId, mapping);
  }
};
MzDataProvider.prototype._loadXmlNodeData = function(xmlNode, parentId, mapping)
{
  if(!(xmlNode && xmlNode.hasChildNodes())) return;
  for(var k,id,i=0,data={},n=xmlNode.childNodes; i<n.length; i++)
  {
    if(n[i].nodeType==1){id=n[i].getAttribute("id")||this.getUniqueId();
    if(n[i].hasChildNodes()){for(k=0,nic=n[i].childNodes;k<nic.length;k++)
    {
      if(nic[k].nodeType==1){this._loadXmlNodeData(n[i], id, mapping);break;}}
    }
    for(var k=0,s="",a=n[i].attributes; k<a.length; k++)
    {
      if(mapping) s=s.setAttribute(mapping[a[k].name.toLowerCase()]||a[k].name, a[k].value);
      else s=s.setAttribute(a[k].name, a[k].value);
    }
    if(!s.getAttribute("text")) s="text:;"+ s;
    a=parentId + this.divider + id; data[a]=s;}
  }
  this.appendData(data);
};

//public
MzDataProvider.prototype.loadUlData=function(HtmlUL, parentId)
{
  if("undefined"==typeof parentId) parentId=this.rootId; var ul;
  if("string"==typeof HtmlUL&&(ul=document.getElementById(HtmlUL)));
  else if("object"==typeof HtmlUL&&(ul=HtmlUL.tagName)&&
    "UL OL".indexOf(ul.toUpperCase())>-1) ul=HtmlUL;
  if("object"==typeof ul)
  {
    var data={}; for(var i=0, n=ul.childNodes; i<n.length; i++)
    {
      if(n[i].nodeType==1 && n[i].tagName=="LI")
      {
        var id=n[i].getAttribute("sourceid")||this.getUniqueId(),txt="",link="";
        for(var k=0; k<n[i].childNodes.length; k++)
        {
          var node=n[i].childNodes[k];
          if(node.nodeType==3) txt += node.nodeValue;
          if(node.nodeType==1)
          {
            switch(node.tagName)
            {
              case "UL":
              case "OL": this.loadUlData(node, id); break;
              case "A" : if(!link) link=node; break;
            }
          }
        }
        var str="";
        if(link)
        {
          str=str.setAttribute("target", link.target);
          str=str.setAttribute("url", link.href);
          str=str.setAttribute("text", link.innerHTML);
        }
        else str = str.setAttribute("text", txt);
        var a=n[i].attributes; //<li>.attributes
        for(var k=0; k<a.length; k++)
        {
          if(a[k].specified && a[k].name!="style")
            str = str.setAttribute(a[k].name, a[k].value);
        }
        a=parentId + this.divider + id;
        data[a]=str;
      }
    }
    this.appendData(data);
  }
}
//public: check node has child
MzDataNode.prototype.hasChildNodes = function()
{
  var $=this.$$caller;
  this.hasChild=$.indexes.indexOf($.__+ this.id + $.divider)>-1
  ||(this.sourceIndex&&(this.get("JSData")!=null||this.get("XMLData")!=null
  || this.get("ULData")!=null)); return this.hasChild;
};
//public: get node attribute
MzDataNode.prototype.get = function(attribName)
{
  if("undefined"!=typeof(this[attribName]))return this[attribName]; else
  {
    var s=this.$$caller.dataSource[this.sourceIndex];
    if("string"==typeof(s)) return s.getAttribute(attribName);
    else if("object"==typeof(s)) return s[attribName];
  }
};
//public: set node attribute
MzDataNode.prototype.set = function(attribName, value)
{
  if("undefined"!=typeof(this[attribName])) this[attribName]=value; else
  {
    var s=this.$$caller.dataSource[this.sourceIndex];
    if("string"==typeof(s))
      this.$$caller.dataSource[this.sourceIndex]=s.setAttribute(attribName,value);
    else if("object"==typeof(s)) s[attribName]=value;
  }
};
//private: load all node's node and init
MzDataNode.prototype.loadChildNodes = function(DataNodeClass)
{
  var $=this.$$caller,r=$.dividerEncoding,_=$.__, i, cs;
  var tcn=this.childNodes;tcn.length=0;if(this.sourceIndex){
  if((i=this.get("JSData"))) $.loadJsData((/^\w+\.js(\s|\?|$)/i.test(i)?$.jsDataPath:"")+i);
  if((i=this.get("ULData"))) $.loadUlData(i, this.id);
  if((i=this.get("XMLData")))$.loadXmlData((/^\w+\.xml(\s|\?|$)/i.test(i)?$.xmlDataPath:"")+i,this.id);}
  var reg=new RegExp(_ + this.id + r +"[^"+ _ + r +"]+", "g"); 
  if((cs=$.indexes.match(reg))){for(i=0;i<cs.length;i++){
    tcn[tcn.length]=this.DTO(DataNodeClass, cs[i].substr(_.length));}}
  this.isLoaded = true;
};
MzDataNode.prototype.DTO=function(DataNodeClass, sourceIndex)
{
  var C=DataNodeClass||MzDataNode,$=this.$$caller,d=$.divider,n=new C,s;
  n.$$caller=this.$$caller; s=$.dataSource[n.sourceIndex=sourceIndex];
  n.id=sourceIndex.substr(sourceIndex.indexOf(d)+d.length);
  n.hasChildNodes(); n.parentNode=this; $.nodes[n.index]=n;
  n.path=this.path+d+n.id;
  if("string"!=typeof($.indexMapping[n.sourceIndex])) $.indexMapping[n.sourceIndex]="";
  $.indexMapping[n.sourceIndex]=$.indexMapping[n.sourceIndex].setAttribute("index_"+ $.hashCode,n.index);
  n.set("index_"+ $.hashCode,n.index);
  if("string"==typeof(s)) n.text=s.getAttribute("text");
  else if("object"==typeof(s)) n.text=s.text; return n;
};

String.prototype.getAttribute = function(attribute)
{
  if(new RegExp("(^|;)\\s*"+attribute+"\\s*:\\s*([^;]*)\\s*(;|$)","i").test(this))
  return RegExp.$2.replace(/%3B/gi,";").replace(/%25/g,"%"); return null;
};
String.prototype.setAttribute = function(attribute, value)
{
  value=(""+value).replace(/%/g,"%25").replace(/;/g,"%3B").replace(/\r|\n/g,"");
  return (attribute +":"+ value +";" + this);
};
String.prototype.deleteAttribute = function(attribute)
{
  return this.replace(new RegExp("\\b\\s*"+attribute+"\\s*:\\s*([^;]*)\\s*(;|$)","gi"),"");
};


/*** MzEffect.js ***/

/*---------------------------------------------------------------------------*\
|  Subject:    Html Element effect base
|  NameSpace:  System.Web.Forms.MzEffect
|  Author:     meizz
|  Created:    2006-07-07
|  Version:    2007-04-13
\*---------------------------------------------------------------------------*/

/*/Interface
Required: initialize() render() finish() restore()
abort() end() cancel() dispose()
op.onbeforestart() op.onbeforeupdate() op.onafterupdate() op.onafterfinish()
/*/
//op{interval, duration, overlap, trend, continual}
function MzEffect()
{
  System.call(this);this.readyState=1;
  this.element=MzElement.check(arguments[0]);
  if(!this.element) return; var t;

  this.terminative=false;  //true for terminate effect

  var op=this.options=System.extend({
     "interval": 10   //milliseconds
    ,"duration": 800  //milliseconds
    ,"overlap":  false
    ,"continual": true
    ,"trend": "strengthen" //strengthen|weaken true|false
  },
    arguments[1]||{}
  );
  op["trend"]= ("boolean"==typeof(op["trend"]) && op["trend"]) ||
    ("string"==typeof(op["trend"])&&op["trend"].toLowerCase()=="strengthen");

  //prevent repeated effect
  if(!op.overlap){this.attribName="att_"+this._className.replace(/\W/g, "_");
  if(t=this.element.getAttribute(this.attribName))if(t=Instance(t))t.cancel();
  this.element.setAttribute(this.attribName, this.hashCode, 0);}

  this.beginTime = new Date().getTime();
  this.endTime = this.beginTime + op.duration;
  if(this.initialize) this.initialize();

  /*_onbeforestart*/ MzEffect.attachEvent(this, "_onbeforestart");
  /*onbeforestart*/  MzEffect.attachEvent(this, "onbeforestart");
  /*onbeforeupdate*/ MzEffect.attachEvent(this, "onbeforeupdate");
  /*onafterupdate*/  MzEffect.attachEvent(this, "onafterupdate");
  /*onafterfinish*/  MzEffect.attachEvent(this, "onafterfinish");

  this.dispatchEvent(new System.Event("_onbeforestart"));
  this.dispatchEvent(new System.Event("onbeforestart"));
  this.readyState=2;if(op.continual) this._process();else
  if(!this.restore){this.restore=function(){};this.dispose();}
};
t=MzEffect.Extends(System, "MzEffect");

t._process=function()
{
  var now = new Date().getTime(), me = this, op=this.options;
  if (now>=this.endTime)
  {
    if(this.render) this.render((op.trend==true ? 1 : 0));
    if(this.finish) this.finish(); this.readyState=4;
    this.dispatchEvent(new System.Event("onafterfinish"));
    this.dispose(); return;
  } if(op){

  var schedule = Math.sqrt((now-this.beginTime)/op.duration);
  if(op.trend!=true) schedule = Math.pow((this.endTime-now)/op.duration, 2);
  this.schedule = schedule;

  this.dispatchEvent(new System.Event("onbeforeupdate"));
  if(this.render)this.render(schedule);
  this.dispatchEvent(new System.Event("onafterupdate"));
  if(!this.terminative)

  this.timer=setTimeout(function(){me._process()},op.interval); else
  try{this.element.removeAttribute(this.attribName);}catch(ex){}}
  me.readyState=3; 
};
t.abort=function(){if(this.timer)clearTimeout(this.timer);};
t.end=function()
{
  if(this.readyState==4) return;
  if(this.timer) clearTimeout(this.timer);
  this.endTime = this.beginTime; this._process();
}
t.cancel=function()
{
  this.endTime = this.beginTime;
  if(this.timer)  clearTimeout(this.timer);
  if(this.restore) this.restore();
  this.dispose();
};
t.dispose=function()
{
  if(this.element)this.element.removeAttribute(this.attribName);
  //System.prototype.dispose.call(this);
  if(System.ie) CollectGarbage();
};
MzEffect.attachEvent=function(T, type)
{
  //if(T.options&&typeof(T.options[type])=="function")
  //  T.addEventListener(type, function(e){T.options[type](e)});
};
MzEffect.formatColor=function(color)
{
  if(/^\#[\da-z]{6}$/i.test(color)) return color;
  else if(color.indexOf("rgb(")==0)
  {
    var cs=color.substring(4, color.length-1).split(",");
    for(var i=0, color="#"; i<cs.length; i++)
    {
      var s = parseInt(cs[i]).toString(16);
      color+= ("00"+ s).substr(s.length);
    }
    return color;
  }
  return "";
};


/*--====== core effect ======--*/
//op{interval, duration, trend}
MzEffect.Opacity=function(element, op)
{
  op = System.extend({interval: 20}, op||{});
  op.simple = (System.ie && MzBrowser.version<5.5)
    || (MzBrowser.opera  && MzBrowser.version<8.5);
  MzEffect.apply(this, [element, op]);
};
MzEffect.Opacity.Extends(MzEffect, "MzEffect.Opacity").initialize=function()
{
  var op=this.options, obj=this.element, me=this;
  this.render=function(schedule)
  {
    schedule = me.schedule || schedule;
    if(op.simple){me.endTime-=op.duration; return;}
    if(!System.ie)
    {
      obj.style.opacity = schedule;
      obj.style.MozOpacity = schedule;
      obj.style.KHTMLOpacity = schedule;
    }
    else obj.style.filter = "alpha(opacity:"+Math.round(schedule*100)+")";
  };
  if (System.ie)op.originalFilter = obj.style.filter; else
  {
    op.originalOpacity = obj.style.opacity;
    op.originalMozOpacity = obj.style.MozOpacity;
    op.originalKHTMLOpacity = obj.style.KHTMLOpacity;
  }
  this.restore=function()
  {
    if(System.ie) obj.style.filter = op.originalFilter; else
    {
      obj.style.opacity = op.originalOpacity;
      obj.style.MozOpacity = op.originalMozOpacity;
      obj.style.KHTMLOpacity = op.originalKHTMLOpacity;
    }
  };
};

//op{interval, duration, trend}
MzEffect.MoveBy=function(element, x, y, op)
{
  if("undefined"==typeof x || "undefined"==typeof y) return;
  this.offsetX=parseFloat(x);
  this.offsetY=parseFloat(y);
  op=System.extend({duration: 400}, op||{});
  MzEffect.apply(this, [element, op]);
};
MzEffect.MoveBy.Extends(MzEffect, "MzEffect.MoveBy").initialize=function()
{
  var me=this, obj=me.element, op=me.options;
  op.originalTop  = obj.style.top;
  op.originalLeft = obj.style.left;
  op.originalPosition = obj.style.position;
  me.originalY = parseFloat(obj.style.top  || '0');
  me.originalX = parseFloat(obj.style.left || '0');
  if(obj.style.position == "") obj.style.position = "relative";

  this.render=function(schedule)
  {
    schedule = me.schedule || schedule;
    obj.style.left = (me.offsetX * schedule + me.originalX) +"px";
    obj.style.top  = (me.offsetY * schedule + me.originalY) +"px";
  };
  this.setPosition=function(x, y)
  {
    obj.style.top  = y +"px";
    obj.style.left = x +"px";
  };
  this.restore=function()
  {
    obj.style.top  = op.originalTop;
    obj.style.left = op.originalLeft;
    obj.style.position = op.originalPosition;
  };
};

//op{interval, duration, trend}
MzEffect.MoveTo=function(element, op)
{
  if(!op&&("undefined"==typeof op.x||"undefined"==typeof op.y)) return;
  this.x=parseFloat(op.x);this.y=parseFloat(op.y);
  op=System.extend({duration: 300}, op||{});
  MzEffect.apply(this, [element, op]);
};
MzEffect.MoveTo.Extends(MzEffect, "MzEffect.MoveTo").initialize=function()
{
  var me=this, obj=me.element, op=me.options;
  op.originalTop  = obj.style.top;
  op.originalLeft = obj.style.left;
  op.originalPosition = obj.style.position;
  me.originalY = parseFloat(obj.style.top  || '0');
  me.originalX = parseFloat(obj.style.left || '0');
  me.offsetX = me.x - me.originalX;
  me.offsetY = me.y - me.originalY;
  if((obj.currentStyle && obj.currentStyle.position== "static")
    || obj.style.position == "") obj.style.position = "relative";

  this.render=function(schedule)
  {
    schedule = me.schedule || schedule;
    obj.style.left = (me.offsetX * schedule + me.originalX) +"px";
    obj.style.top  = (me.offsetY * schedule + me.originalY) +"px";
  };
  this.setPosition=function(x, y)
  {
    obj.style.top  = y +"px";
    obj.style.left = x +"px";
  };
  this.restore=function()
  {
    obj.style.top  = op.originalTop;
    obj.style.left = op.originalLeft;
    obj.style.position = op.originalPosition;
  };
};

//op{interval, duration, trend, color, beginColor, endColor, finalColor}
MzEffect.Highlight=function(element, op){MzEffect.apply(this, arguments);};
MzEffect.Highlight.Extends(MzEffect, "MzEffect.Highlight").initialize=function()
{
  var op=this.options, obj=this.element, endColor="#FFFFFF";
  var backColor = (obj.currentStyle||obj.style).backgroundColor;
  op.originalBgColor = obj.style.backgroundColor;
  op.originalColor = obj.style.color;
  if(backColor) endColor = MzEffect.formatColor(backColor);
  op.beginColor = op.beginColor || "#FFFF00";
  op.endColor   = op.endColor || endColor;
  op.finalColor = op.finalColor || obj.style.backgroundColor;

  this.colors_base=[
    parseInt(op.beginColor.substring(1,3),16),
    parseInt(op.beginColor.substring(3,5),16),
    parseInt(op.beginColor.substr(5),16)];
  this.colors_var=[
    parseInt(op.endColor.substring(1,3),16)-this.colors_base[0],
    parseInt(op.endColor.substring(3,5),16)-this.colors_base[1],
    parseInt(op.endColor.substr(5),16)-this.colors_base[2]];

  this.finish=function()
  {
    if(op.color) obj.style.color = op.color;
    obj.style.backgroundColor = op.finalColor;
  };
  function n2h(s){s=parseInt(s).toString(16);return ("00"+ s).substr(s.length)}
  this.render=function(schedule)
  {
    schedule = this.schedule || schedule;
    var colors=[
      n2h(Math.round(this.colors_base[0]+(this.colors_var[0]*schedule))),
      n2h(Math.round(this.colors_base[1]+(this.colors_var[1]*schedule))),
      n2h(Math.round(this.colors_base[2]+(this.colors_var[2]*schedule)))];
    obj.style.backgroundColor = "#"+ colors.join("");
  };
  this.restore=function()
  {
    obj.style.color = op.originalColor;
    obj.style.backgroundColor = op.originalBgColor;
  };
};

MzEffect.Curtain=function(element, op)
{
  op = System.extend(
  {
    direction: "",
    scaleContent: false
  }, op||{});
  MzEffect.apply(this, [element, op]);
}
MzEffect.Curtain.Extends(MzEffect, "MzEffect.Mask").initialize=function()
{
  var me=this, op=me.options, obj=me.element;
  op.direction = op.direction.toLowerCase();

  op.originalWidth  = obj.style.width;
  op.originalHeight = obj.style.height;
  op.originalOverflow = obj.style.overflow;
  op.originalPosition = obj.style.position;
  op.originalVisibility = obj.style.visibility;
  op.originalMarginTop = (obj.currentStyle || obj.style).marginTop;

  op.originalOffsetWidth  = obj.offsetWidth;
  op.originalOffsetHeight = obj.offsetHeight;

  obj.style.overflow = "hidden";

  this.restore=function()
  {
    obj.style.width = op.originalWidth;
    obj.style.height = op.originalHeight;
    obj.style.overflow = op.originalOverflow;
    obj.style.position = op.originalPosition;
    obj.style.marginTop = op.originalMarginTop;
    obj.style.visibility = op.originalVisibility;
  };
  this.finish=function(){this.restore();};
  this.render=function(schedule){};
};

//op{interval, duration, trend, direction, opacity}
MzEffect.Mask=function(element, op)
{
  op=System.extend({direction: "box"}, op||{});
  MzEffect.apply(this, [element, op]);
};
MzEffect.Mask.Extends(MzEffect, "MzEffect.Mask").initialize=function()
{
  var me=this, op=me.options, obj=me.element;
  if(!op.trend && obj.style.display=="none"){me.endTime-=op.duration; return;}
  op.direction = op.direction.toLowerCase();
  if(op.direction=="random")
  {
    var a=["box","center-up-down", "center-right-left", "top", "bottom",
      "left", "right", "top-right", "top-left", "right-top", "left-top"];
    var n=Math.floor(Math.random() * a.length);
    op.direction = (n>=a.length) ? "box" : a[n];
  }

  op.originalTop  = obj.style.top;
  op.originalLeft = obj.style.left;
  op.originalClip = obj.style.clip;
  op.originalWidth= obj.style.width;
  op.originalHeight= obj.style.height;
  op.originalPosition = obj.style.position;
  if (op.trend){op.originalVisibility = obj.style.visibility;
  obj.style.visibility = "hidden"; MzElement.show(obj);}
  op._offsetWidth  = obj.offsetWidth;
  op._offsetHeight = obj.offsetHeight;
  if(System.ie || window.opera)
  {
    obj.style.width = op._offsetWidth +"px";
    obj.style.height= op._offsetHeight+"px";
  }
  else
  {
    obj.style.width = (op._offsetWidth
      - parseInt((obj.currentStyle || obj.style).borderLeftWidth||0)
      - parseInt((obj.currentStyle || obj.style).borderRightWidth||0)) +"px";
    obj.style.height= (op._offsetHeight
      - parseInt((obj.currentStyle || obj.style).borderTopWidth||0)
      - parseInt((obj.currentStyle || obj.style).borderBottomWidth||0)) +"px";
  }
  if (op.trend) obj.style.visibility = op.originalVisibility;

  //for opacity 20070114
  if (System.ie)op.originalFilter = obj.style.filter; else
  {
    op.originalOpacity = obj.style.opacity;
    op.originalMozOpacity = obj.style.MozOpacity;
    op.originalKHTMLOpacity = obj.style.KHTMLOpacity;
  }


  if((obj.currentStyle || obj.style).position!="absolute")
  {
    obj.style.position = "absolute";
    var input = document.createElement("INPUT");
    input.type="text";   input.style.border="none";
    input.readOnly=true; input.style.fontSize="1px";
    input.style.margin=input.style.padding=0;
    input.id = "MzEffectMaskStuffing"+ this.hashCode;
    input.style.backgroundColor = "transparent";//"red";
    var a=(obj.currentStyle||obj.style).marginLeft ||0;a=a=="auto"?0:parseInt(a);
    var b=(obj.currentStyle||obj.style).marginRight||0;b=b=="auto"?0:parseInt(b);
    input.style.width = (op._offsetWidth  + a + b-(System.ie?2:0)) +"px";
    a=(obj.currentStyle||obj.style).marginTop   ||0;a=a=="auto"?0:parseInt(a);
    b=(obj.currentStyle||obj.style).marginBottom||0;b=b=="auto"?0:parseInt(b);
    input.style.height= (op._offsetHeight + a + b-(System.ie?2:0)) +"px";
    obj.parentNode.insertBefore(input, obj.nextSibling);
  }

  this.restore=function()
  {
    obj.style.clip  = op.originalClip||"rect(auto, auto, auto, auto)";
    obj.style.top   = op.originalTop;
    obj.style.left  = op.originalLeft;
    obj.style.width = op.originalWidth;
    obj.style.height= op.originalHeight;
    if((obj.currentStyle || obj.style).position!="absolute")
      MzElement.remove("MzEffectMaskStuffing"+ this.hashCode);
    obj.style.position = op.originalPosition;
    if(System.ie) obj.style.filter = op.originalFilter; else
    {
      obj.style.opacity = op.originalOpacity;
      obj.style.MozOpacity = op.originalMozOpacity;
      obj.style.KHTMLOpacity = op.originalKHTMLOpacity;
    }
  };
  this.finish=function(){if(!op.trend)MzElement.hide(obj); me.restore();};
  this.render=function(schedule)
  {
    schedule = me.schedule || schedule;
    if(op.opacity)
    {
      if(!System.ie){obj.style.opacity = schedule;
        obj.style.MozOpacity = schedule; obj.style.KHTMLOpacity = schedule;}
      else obj.style.filter = "alpha(opacity:"+Math.round(schedule*100)+")";
    }
    switch(op.direction)
    {
      case "box":
        var halfW = Math.ceil(op._offsetWidth /2);
        var halfH = Math.ceil(op._offsetHeight/2);
        var right = Math.ceil(halfW * schedule);
        var bottom =Math.ceil(halfH * schedule);
        obj.style.clip = "rect("+ (halfH-bottom) +"px "+ (halfW+right) +"px "+
          (halfH+bottom) +"px "+ (halfW-right) +"px)";
        break;
      case "center-up-down":
      case "center-down-up":
      case "center-top-bottom":
      case "center-bottom-top":
        var halfH = Math.ceil(op._offsetHeight/2);
        var bottom =Math.ceil(halfH * schedule);
        obj.style.clip = "rect("+ (halfH-bottom) +"px "+
          op._offsetWidth +"px "+ (halfH+bottom) +"px 0)";
        break;
      case "center-right-left":
      case "center-left-right":
        var halfW = Math.ceil(op._offsetWidth /2);
        var right = Math.ceil(halfW * schedule);
        obj.style.clip = "rect(0 "+ (halfW+right) +"px "+
          op._offsetHeight +"px "+ (halfW-right) +"px)";
        break;
      case "top":
      case "top-bottom":
        var bottom =op._offsetHeight * schedule +"px";
        obj.style.clip="rect(0 "+ op._offsetWidth +"px "+ bottom +" 0)";
        break;
      case "bottom":
      case "bottom-top":
        var top = op._offsetHeight * (1 - schedule) +"px";
        obj.style.clip = "rect("+ top +" "+ op._offsetWidth +"px "+
          op._offsetHeight +"px 0)";
        break;
      case "left":
      case "left-right":
        var right = op._offsetWidth  * schedule +"px";
        obj.style.clip="rect(0 "+ right +" "+ op._offsetHeight +"px 0)";
        break;
      case "right":
      case "right-left":
        var left =op._offsetWidth  * (1 - schedule) +"px";
        obj.style.clip = "rect(0 "+ op._offsetWidth +"px "+
          op._offsetHeight +"px "+ left +")";
        break;
      case "top-right":
      case "left-bottom":
        var right = op._offsetWidth  * schedule +"px";
        var bottom =op._offsetHeight * schedule +"px";
        obj.style.clip = "rect(0 "+ right +" "+ bottom +" 0)";
        break;
      case "top-left":
      case "right-bottom":
        var bottom = op._offsetHeight * schedule +"px";
        var left =op._offsetWidth  * (1 - schedule) +"px";
        obj.style.clip = "rect(0 "+ op._offsetWidth +"px "+
          bottom +" "+ left +")";
        break;
      case "right-top":
      case "bottom-left":
        var top = op._offsetHeight * (1 - schedule) +"px";
        var left =op._offsetWidth  * (1 - schedule) +"px";
        obj.style.clip = "rect("+ top +" "+ op._offsetWidth +"px "+
          op._offsetHeight +"px "+ left +")";
        break;
      case "left-top":
      case "bottom-right":
        var top = op._offsetHeight * (1 - schedule) +"px";
        var right=op._offsetWidth  * schedule +"px";
        obj.style.clip = "rect("+ top  +" "+ right +" "+
          op._offsetHeight +"px 0)";
        break;
    }
  };
};


//op{interval, duration, trend}
MzEffect.Combo=function(effects, element, op)
{
  this.effects=effects||[];
  if(this.effects.length==0) return;
  MzEffect.apply(this,[element,op]);
};
t=MzEffect.Combo.Extends(MzEffect, "MzEffect.Combo");
t.render=function(schedule)
{
  schedule = this.schedule || schedule;
  for(var i=0; i<this.effects.length; i++)
  {
    var e = this.effects[i];
    if (e.timer) clearTimeout(e.timer);
    e.render(schedule);
  }
};
t.finish=function()
{
  for(var i=0; i<this.effects.length; i++)
  {
    if(this.effects[i].finish) this.effects[i].finish();
    this.effects[i].dispose();
  }
};





/*--====== meizz effects ======--*/

MzEffect.Shake=function(element){MzEffect.call(this, element)};
MzEffect.Shake.Extends(MzEffect, "MzEffect.Shake").initialize=function()
{
  var me=this, obj=me.element, os=obj.style;
  var top =os.top, left=os.left, position=os.position;
  this.restore=function(){os.top=top; os.left=left; os.position=position;}

  this._process=function()
  {
    var a=[0, 0];
    if(me._scheduledIndex>=me._scheduledArray.length)
    {
      this._process=function()
      {
        me.restore();
        me.dispose();
        delete me._scheduledIndex;
        delete me._scheduledArray;
      };
    }
    else
    {
      a=me._scheduledArray[me._scheduledIndex];
      me._scheduledIndex++;
    }
    MzEffect.moveBy(obj, a[0], a[1],{
      duration: 40,
      interval: 20,
      onafterfinish: function(e){me._process();}});
  };
};
MzEffect.Shake.prototype._scheduledIndex=0;
MzEffect.Shake.prototype._scheduledArray=
[
   [20, -5]
  ,[-40,-5]
  ,[40, 10]
  ,[-40, 5]
  ,[40, -9]
  ,[-35, 8]
  ,[35, -7]
  ,[-30, 6]
  ,[20, -5]
  ,[-15, 4]
  ,[10, -3]
  ,[-8,  2]
  ,[6,  -1]
  ,[-5,  0]
  ,[3,   0]
  ,[-1,  0]
];


MzEffect.Fadein=function(element, op)
{
  var obj=MzElement.check(element);
  if(!obj) return; if(System.ie 
    &&!obj.style.width
    &&!obj.style.height
    &&obj.style.position!="absolute"
    &&obj.currentStyle
    &&obj.currentStyle.position!="absolute"
    &&obj.currentStyle.width=="auto"
    &&obj.currentStyle.height=="auto")
  { MzElement.show(obj); return;}
  op=System.extend({"_onbeforestart": function(e)
  {
    MzElement.show(e.target.element);e.target.render(0.01);
  }}, op||{});
  obj=new MzEffect.Opacity(element, op);
  obj.addEventListener("onafterfinish",function(e){e.target.restore();});
  return obj;
};

MzEffect.Fadeout=function(element, op)
{
  var obj=MzElement.check(element);
  if(!obj) return; if(System.ie 
    &&!obj.style.width
    &&!obj.style.height
    &&obj.style.position!="absolute"
    &&obj.currentStyle
    &&obj.currentStyle.position!="absolute"
    &&obj.currentStyle.width=="auto"
    &&obj.currentStyle.height=="auto")
  { MzElement.hide(obj); return;}
  op=System.extend({"trend": "weaken"}, op||{});
  obj=new MzEffect.Opacity(element, op);
  obj.addEventListener("onafterfinish",function(e)
  {
    MzElement.hide(e.target.element); e.target.restore();
  });
  return obj;
};

MzEffect.Appear=function(element, op){return new MzEffect.Fadein(element, op);}
MzEffect.Fade  =function(element, op){return new MzEffect.Fadeout(element, op);}


//op{loop, interval, dynamic}
MzEffect.Pulsate=function(element, op)
{
  this.element=MzElement.check(element); if(!this.element) return;
  op=this.options=System.extend({loop:5,interval:360,dynamic:true},op||{});
  if(!op.dynamic || op.loop<0 || op.loop>5) op.interval += 160;

  var a=this.attribName = "att_"+ this._className.replace(/\W/g, "_");
  if((t=this.element.getAttribute(a)) && (t=Instance(t))){
  System.extend(t.options, this.options); t.times=0; return t;}

  System.call(this);
  MzEffect.attachEvent(this, "onbeforestart");
  MzEffect.attachEvent(this, "onafterfinish");
  this.element.setAttribute(this.attribName, this.hashCode, 0);
  this.timer=null; this.initialize();
  this.dispatchEvent(new System.Event("onbeforestart"));
  this.render();
};
t=MzEffect.Pulsate.Extends(System, "MzEffect.Pulsate");

t.times=0;
t.status=true;
t.initialize=function()
{
  var me=this, op=me.options, obj=me.element;
  op.visibility=obj.style.visibility;

  this.render=function()
  {
    if(me.terminative) return me.stop();
    if(op.loop>-1 && me.times>=op.loop) return me.stop();
    if(me.status)
    {
      if(!op.dynamic || op.loop<0 || op.loop>5)
      {
        obj.style.visibility="hidden";
        me.timer = setTimeout(me.render, op.interval);
      }
      else
      {
        MzEffect.fadeout(obj, {
          interval: 40,
          duration: 160,
          onafterfinish: function(e)
          {
            obj.style.visibility="hidden";
            e.target.restore();
            me.timer = setTimeout(me.render, op.interval);
          }
        });
      }
    }
    else
    {
      if(!op.dynamic || op.loop<0 || op.loop>5)
      {
        obj.style.visibility="";
        me.timer = setTimeout(me.render, op.interval);
      }
      else
      {
        MzEffect.fadein(obj,{
          interval: 40,
          duration: 160,
          onbeforestart: function(e){obj.style.visibility=""; me.render(0.01);},
          onafterfinish: function(e){me.restore(); me.timer=setTimeout(me.render, op.interval);}
        });
      }
    }
    me.status = !me.status; if(me.status) me.times++;
  };

  this.stop=function()
  {
    obj.style.visibility=op.visibility;
    if(me.finish) me.finish();
    me.dispatchEvent(new System.Event("onbeforestart"));
    clearTimeout(me.timer);
    me.dispose();
  }
  this.dispose=function()
  {
    if(me.element) me.element.removeAttribute(me.attribName);
    System.prototype.dispose.call(this);
  };
};
MzEffect.Pulsate.stop=function(element)
{
  if(element=MzElement.check(element))
  {
    var t = "att_"+ "MzEffect.Pulsate".replace(/\W/g, "_");
    if(t=element.getAttribute(t))if(t=Instance(t))t.terminative=true;
  }
}


//op{interval, duration}
MzEffect.BlindDown=function(element, op){MzEffect.apply(this, arguments);};
MzEffect.BlindDown.Extends(MzEffect, "MzEffect.Mask").initialize=function()
{
  var op=this.options, obj=this.element;
  op.originalHeight = obj.style.height;
  op.originalOverflow = obj.style.overflow;
  op.visibility = obj.style.visibility;
  obj.style.visibility="hidden"; MzElement.show(obj);
  op._offsetHeight = obj.offsetHeight;
  if(!System.ie||(System.ie&&MzBrowser.version<7)){
  var t; if(t=(obj.currentStyle||obj.style).paddingTop) t=parseFloat(t);
  if(typeof(t)=="number") op._offsetHeight -= t;
  var b; if(b=(obj.currentStyle||obj.style).paddingBottom) b=parseFloat(b);
  if(typeof(b)=="number") op._offsetHeight -= b;}
  obj.style.height = "1px"; obj.style.overflow = "hidden";
  setTimeout(function(){obj.style.visibility = op.visibility;}, op.interval);

  this.finish=this.restore=function()
  {
    obj.style.overflow = op.originalOverflow;
    obj.style.height = op.originalHeight;
  };
  this.render=function(schedule)
  {
    schedule = this.schedule || schedule;
    obj.style.height = parseInt(schedule * op._offsetHeight) +"px";
  };
};

//op{interval, duration}
MzEffect.BlindUp=function(element, op)
{
  op = System.extend({trend: "weaken"}, op||{});
  MzEffect.apply(this, [element, op]);
};
MzEffect.BlindUp.Extends(MzEffect, "MzEffect.Mask").initialize=function()
{
  var op=this.options, obj=this.element;
  op.originalHeight = obj.style.height;
  op.originalOverflow = obj.style.overflow;
  op._offsetHeight = obj.offsetHeight;
  if(!System.ie||(System.ie&&MzBrowser.version<7)){
  var t; if(t=(obj.currentStyle||obj.style).paddingTop) t=parseFloat(t);
  if(typeof(t)=="number") op._offsetHeight -= t;
  var b; if(b=(obj.currentStyle||obj.style).paddingBottom) b=parseFloat(b);
  if(typeof(b)=="number") op._offsetHeight -= b;}
  obj.style.overflow = "hidden";

  this.finish=function()
  {
    MzElement.hide(obj);
    obj.style.overflow = op.originalOverflow;
    obj.style.height = op.originalHeight;
  };
  this.restore=function()
  {
    obj.style.overflow = op.originalOverflow;
    obj.style.height = op.originalHeight;
  };
  this.render=function(schedule)
  {
    schedule = this.schedule || schedule;
    if(schedule<=0.05) MzElement.hide(obj);
    obj.style.height = parseInt(schedule * op._offsetHeight) +"px";
  };
};

//op{interval, duration}
MzEffect.SlideUp=function(element, op)
{
  return MzEffect.collapse(element, System.extend(
  {
    onafterupdate: function(e){e.target.element.scrollTop=e.target.options.offsetHeight;}
  }, op||{}))
};

//op{interval, duration}
MzEffect.SlideDown=function(element, op)
{
  return MzEffect.expand(element, System.extend(
  {
    onafterupdate: function(e){e.target.element.scrollTop=e.target.options.offsetHeight;}
  }, op||{}))
};

//op{interval, duration}
MzEffect.Fall=function(){MzEffect.Curtain.apply(this, arguments);};
MzEffect.Fall.Extends(MzEffect.Curtain).initialize=function()
{
  MzEffect.Curtain.prototype.initialize.call(this);
  var me=this, op=me.options, obj=me.element;

  this.finish=function(){this.hide(9);};
  this.hide=function(n)
  {
    if(n<0){MzElement.hide(obj); me.restore(); return;}
    obj.style.height = "1px"; obj.style.visibility = "hidden";
    obj.style.marginTop=(n/10)*op.originalOffsetHeight + parseFloat(op.originalMarginTop||0) +"px";
    this.timer=setTimeout(function(){me.hide(--n);}, 10);
  }
  this.render=function(schedule)
  {
    schedule = Math.ceil(op.originalOffsetHeight * (this.schedule || schedule));
    obj.style.marginTop = (schedule + parseFloat(op.originalMarginTop||0)) +"px";
    obj.style.height= (op.originalOffsetHeight - schedule) +"px";
  };
};

//op{interval, duration}
MzEffect.Rise=function(){MzEffect.Curtain.apply(this, arguments);};
MzEffect.Rise.Extends(MzEffect.Curtain).initialize=function()
{
  MzEffect.Curtain.prototype.initialize.call(this);
  var me=this, op=me.options, obj=me.element;

  obj.style.height="1px"; MzElement.show(obj);
  op.originalOffsetHeight = obj.offsetHeight;
  obj.style.marginTop = op.originalOffsetHeight + parseFloat(op.originalMarginTop||0);

  this.render=function(schedule)
  {
    schedule = Math.ceil(op.originalOffsetHeight * (this.schedule || schedule));
    obj.style.height = schedule +"px";
    obj.style.marginTop = (op.originalOffsetHeight - schedule + parseFloat(op.originalMarginTop||0)) +"px";
  };
};


//2007-04-13
//[static method]
//op{interval, duration, trigger}
MzEffect.fold=function(element, op)
{
  element=MzElement.check(element); if(!element) return;
  op=System.extend({"interval":20, "duration":600},op||{});

  if(element.style.display=="none")
  {
    var ef=MzEffect.expand(element, {"interval":op.interval, "duration":op.duration});
    if(e=MzElement.check(op.trigger)) MzElement.addClassName(e, "expand");
  }
  else
  {
    var ef=MzEffect.collapse(element, {"interval":op.interval, "duration":op.duration});
    if(e=MzElement.check(op.trigger)) MzElement.removeClassName(e, "expand");
  }
  return ef;
};


//[static method]
MzEffect.opacity  =function(element, op){return new MzEffect.Opacity(element, op);};
MzEffect.moveBy   =function(element, x, y, op){return new MzEffect.MoveBy(element, x, y, op);};
MzEffect.moveTo   =function(element, op){return new MzEffect.MoveTo(element, op);};
MzEffect.highlight=function(element, op){return new MzEffect.Highlight(element, op);};
MzEffect.mask     =function(element, op){return new MzEffect.Mask(element, op);};

MzEffect.combo    =function(element, op){};
MzEffect.shake    =function(element, op){return new MzEffect.Shake(element, op);};
MzEffect.fadein   =function(element, op){return new MzEffect.Fadein(element, op);};
MzEffect.fadeout  =function(element, op){return new MzEffect.Fadeout(element, op);};
MzEffect.pulsate  =function(element, op){return new MzEffect.Pulsate(element, op);};
MzEffect.expand   =function(element, op){return new MzEffect.BlindDown(element, op);};
MzEffect.collapse =function(element, op){return new MzEffect.BlindUp(element, op);};
MzEffect.remove   =function(element, op){return new MzEffect.Fadeout(element, System.extend(op||{}, {onafterfinish: function(){MzElement.remove(element)}}));};


/*** MzBehavior.js ***/

/*---------------------------------------------------------------------------*\
|  Subject:    Html Element behavior base
|  NameSpace:  System.Web.Forms.MzBehavior
|  Author:     meizz
|  Created:    2006-08-05
|  Version:    2007-05-21
\*---------------------------------------------------------------------------*/
//Using("System.Web.Forms.MzEffect");

//op{interval, duration, dynamic}
function MzBehavior()
{
  this.element=MzElement.check(arguments[0]);
  if(!this.element) return; System.call(this);

  this.options=System.extend({
    interval: 20,   //milliseconds
    duration: 360,  //milliseconds
    dynamic:  true
  },arguments[1]||{}); var t;

  //prevent repeated behavior
  this.attributeName = "att_"+ this._className.replace(/\W/g, "_");
  if(t=this.element.getAttribute(this.attributeName)) return;
  this.element.setAttribute(this.attributeName, this.hashCode, 0);

  if("function"==typeof(this.initialize)) this.initialize();
}
MzBehavior.Extends(System, "MzBehavior");


//op{color, beginColor, backgroundColor, backgroundImage}
MzBehavior.Highlight=function(obj,op)
{
  op=System.extend({backgroundColor: "#D4D0C8"}, op||{});
  MzBehavior.apply(this, [obj, op]);
};
t=MzBehavior.Highlight.Extends(MzBehavior, "MzBehavior.Highlight");

t.initialize=function()
{
  var me=this, op=me.options, obj=me.element;
  op._color = obj.style.color;
  op._backgroundColor = obj.style.backgroundColor;
  op._backgroundImage = obj.style.backgroundImage; if(!op.beginColor){
  op.beginColor=(obj.currentStyle||obj.style).backgroundColor;
  if(!op.beginColor || op.beginColor=="transparent")op.beginColor="#FFFFFF";}
  op.beginColor = MzEffect.formatColor(op.beginColor);

  this.mouseover=function()
  {
    if(me.outer) return;
    if(op.color) obj.style.color=op.color;
    if(op.dynamic)
    {
      MzEffect.highlight(obj, {
        interval:   op.interval,
        duration:   op.duration,
        beginColor: op.beginColor,
        endColor:   op.backgroundColor,
        finalColor: op.backgroundColor,
        onafterfinish: function(e)
        {
          if(op.color) obj.style.color=op.color;
          if(op.backgroundImage) obj.style.backgroundImage=op.backgroundImage;
        }});
    }
    else
    {
      if(op.color)
      obj.style.color=op.color;if(op.backgroundImage)
      obj.style.backgroundImage = op.backgroundImage;
      obj.style.backgroundColor = op.backgroundColor;
    }
    me.outer=true;
  };
  this.mouseout=function()
  {
    if(me.inner) return; obj.style.color=op._color;
    if(op.dynamic)
    {
      MzEffect.highlight(obj, {
        interval:   op.interval,
        duration:   op.duration,
        beginColor: op.backgroundColor,
        endColor:   op.beginColor,
        finalColor: op.beginColor,
        onafterfinish: function(e)
        {
          obj.style.color=op._color;if(op.backgroundImage)
          obj.style.backgroundImage = op._backgroundImage;
          obj.style.backgroundColor = op._backgroundColor;
        }});
    }
    else
    {
      obj.style.color=op._color;if(op.backgroundImage)
      obj.style.backgroundImage = op._backgroundImage;
      obj.style.backgroundColor = op._backgroundColor;
    }
    me.outer=false;
  };
  this.mouseoverHandler=function()
  {
    clearTimeout(me.timer); me.inner=true;
    setTimeout(me.mouseover, 1);
  };
  this.mouseoutHandler =function()
  {
    me.outer=!(me.inner=false);
    me.timer=setTimeout(me.mouseout, 1);
  };
  this.restore=function()
  {
    this.inner=false; this.mouseout();
    obj.removeAttribute(me.attributeName);
    obj.detachEvent("onmouseover",this.mouseoverHandler);
    obj.detachEvent("onmouseout", this.mouseoutHandler);
  };
  obj.attachEvent("onmouseover", this.mouseoverHandler);
  obj.attachEvent("onmouseout",  this.mouseoutHandler);
};









//op{dark,light,color,backgroundColor,backgroundImage,condition(e)}
MzBehavior.Emboss=function(element, op)
{
  op = System.extend({
    dark: "#808080",
    light:"#F5F5F5",
    borderWidth: 1,
    continual: false,
    condition: function(e){return true;}
  },op||{});

  MzBehavior.apply(this, [element, op]);
};
t=MzBehavior.Emboss.Extends(MzBehavior, "MzBehavior.Emboss");
t.initialize=function()
{
  var me=this, obj=this.element; op=this.options, bw = op.borderWidth;
  var borderStyle = bw==1 ? "solid " : "outset ";
  op._color = obj.style.color;
  op._backgroundColor = obj.style.backgroundColor;
  op._backgroundImage = obj.style.backgroundImage;

  this.especial=false;
  /MSIE (\d+(\.\d+)?)/.test(navigator.userAgent);
  if(System.ie)this.especial=parseFloat(RegExp.$1)<7;
  this.especial = this.especial || window.opera;

  if(this.especial)
  {
    var es = obj.currentStyle || obj.style;
    var PT = ((op._PT=es.paddingTop)   =="auto"?0:parseInt(op._PT))+bw;
    var PL = ((op._PL=es.paddingLeft)  =="auto"?0:parseInt(op._PL))+bw;
    var PR = ((op._PR=es.paddingRight) =="auto"?0:parseInt(op._PR))+bw;
    var PB = ((op._PB=es.paddingBottom)=="auto"?0:parseInt(op._PB))+bw;

    var es = obj.style;
    this._pristine=function()
    {
      es.paddingTop =PT +"px"; es.paddingBottom=PB +"px";
      es.paddingLeft=PL +"px"; es.paddingRight =PR +"px";
      es.border="none";   es.color=op._color;
      es.backgroundColor=op._backgroundColor;
      es.backgroundImage=op._backgroundImage;
    };
    this.mouseover=function()
    {
      if(!op.condition(me)){me._pristine(); return;}
      es.borderTop = es.borderLeft    = borderStyle + bw +"px "+ op.light;
      es.borderRight = es.borderBottom= borderStyle + bw +"px "+ op.dark;
      es.paddingTop=(PT-bw) +"px"; es.paddingBottom=(PB-bw) +"px";
      es.paddingLeft=(PL-bw) +"px"; es.paddingRight=(PR-bw) +"px";
      if(op.color) es.color=op.color;
      if(op.backgroundColor) es.backgroundColor=op.backgroundColor;
      if(op.backgroundImage) es.backgroundImage=op.backgroundImage;
    };
    this.mousedown=function()
    {
      if(!op.condition(me)){me._pristine(); return;}
      es.borderTop = es.borderLeft = "inset "+ bw +"px "+ op.dark;
      es.borderRight=es.borderBottom="inset "+ bw +"px "+ op.light;
      es.paddingTop=(PT-bw) +"px"; es.paddingBottom=(PB-bw) +"px";
      es.paddingLeft=(PL-bw) +"px"; es.paddingRight=(PR-bw) +"px";
      if(op.color) es.color=op.color;
      if(op.backgroundColor) es.backgroundColor=op.backgroundColor;
      if(op.backgroundImage) es.backgroundImage=op.backgroundImage;
    };
  }
  else
  {
    var es=obj.style;
    op._BT=es.borderTop;   op._BL=es.borderLeft;
    op._BR=es.borderRight; op._BB=es.borderBottom;
    es.borderTop = es.borderLeft = "solid "+ bw +"px transparent";
    es.borderRight = es.borderBottom = "solid "+ bw +"px transparent";

    this._pristine=function()
    {
      es.borderTop = es.borderLeft = "solid "+ bw +"px transparent";
      es.borderRight = es.borderBottom = "solid "+ bw +"px transparent";
      es.color=op._color;
      es.backgroundColor=op._backgroundColor;
      es.backgroundImage=op._backgroundImage;
    };
    this.mouseover=function()
    {
      if(!op.condition(me)){me._pristine(); return;}
      es.borderTop = es.borderLeft     = borderStyle + bw +"px "+ op.light;
      es.borderRight = es.borderBottom = borderStyle + bw +"px "+ op.dark;
      if(op.color) es.color=op.color;
      if(op.backgroundColor) es.backgroundColor=op.backgroundColor;
      if(op.backgroundImage) es.backgroundImage=op.backgroundImage;
    };
    this.mousedown=function()
    {
      if(!op.condition(me)){me._pristine(); return;}
      es.borderTop = es.borderLeft = "inset "+ bw +"px "+ op.dark;
      es.borderRight = es.borderBottom = "inset "+ bw +"px "+ op.light;
      if(op.color) es.color=op.color;
      if(op.backgroundColor) es.backgroundColor=op.backgroundColor;
      if(op.backgroundImage) es.backgroundImage=op.backgroundImage;
    };
  }

  this._pristine();
  obj.attachEvent("onmouseover", this.mouseover);
  obj.attachEvent("onmouseout",  this._pristine);
  obj.attachEvent("onmousedown", this.mousedown);
  obj.attachEvent("onclick",     this.mouseover);

  this.restore=function()
  {
    if(this.especial)
    {
      es.paddingTop=op._PT; es.paddingBottom=op._PB;
      es.paddingLeft=op._PL; es.paddingRight=op._PR;
    }
    else
    {
      es.borderTop=op._BT; es.borderBottom=op._BB;
      es.borderLeft=op._BL; es.borderRight=op._BR;
    }
    obj.removeAttribute(me.attributeName);
    obj.detachEvent("onmouseover", this.mouseover);
    obj.detachEvent("onmouseout",  this._pristine);
    obj.detachEvent("onmousedown", this.mousedown);
    obj.detachEvent("onclick",     this.mouseover);
  };
};





//2006-11-29
//op{interval,duration,direction,dynamic,continual,binding,width,height,increased,controls,selectedClassName}
MzBehavior.Rotate=function(element, op)
{
  op = System.extend(
  {
    interval: 3000,
    duration: 2000,
    continual: true,
    direction: "random",
    binding: "onmouseover"
  },op||{});
  MzBehavior.apply(this, [element, op]);
};
MzBehavior.Rotate.Extends(MzBehavior, "MzBehavior.Rotate");

MzBehavior.Rotate.prototype.initialize=function()
{
  var me=this, obj=me.element, op=me.options;
  me.interval = op.duration + op.interval;
  me.nodes = obj.children;
  me.timer = null;
  me.activeIndex = 1;
  me.currentIndex = 0;
  obj.style.position = "relative"; //20061208

  if (me.nodes.length<=1) return; n=op.controls;
  for(var i=1; i<me.nodes.length; i++) MzElement.hide(me.nodes[i]);
  if("undefined"!=typeof(n)&& n.length&&"object"==typeof(n[0])&&n[0].tagName)
  {
    System.call(me);
    for(var i=0;i<n.length;i++)if(n[i].tagName)
    {
      var f=new Function("Instance('"+ me.hashCode +"').focus("+ i +")");
      n[i].onclick = f; if(op.binding) n[i][op.binding] = f;
    }
    n[me.currentIndex].className=op.selectedClassName||"selected";
  }
  if(this.options.continual) me.timer=
  setTimeout(function(){me.change();}, me.interval);
  setTimeout(function()
  {
    var w = parseFloat(op.width);
    var h = parseFloat(op.height);
    op.width = op.width ? (isNaN(w) ? "" : w) : "";
    op.height= op.height? (isNaN(h) ? "" : h) : "";
    w=parseFloat(obj.currentStyle.width);
    h=parseFloat(obj.currentStyle.height);
    op.width = op.width || (isNaN(w) ? "" : w);
    op.height= op.height|| (isNaN(h) ? "" : h);
    op.width = op.width || me.nodes[0].offsetWidth;
    op.height= op.height|| me.nodes[0].offsetHeight;
    obj.style.width = op.width +"px";
    obj.style.height= op.height+"px";
    obj.style.overflow = "hidden";
  }, 1);
};

MzBehavior.Rotate.prototype.change=function()
{
  if (this.dispatchEvent(new System.Event("onchange"))) this.mask();
  var me = this, op=me.options, n=op.controls;
  if("undefined"!=typeof(n)&& n.length&&"object"==typeof(n[0])&&n[0].tagName)
  {
    for(var i=0; i<n.length; i++) n[i].className = "";
    n[me.currentIndex].className= op.selectedClassName || "selected";
  }
  if(this.options.continual) this.timer=
  setTimeout(function(){me.change();}, me.interval);
};

MzBehavior.Rotate.prototype.mask=function()
{
  var me=this, L=me.nodes.length, I=me.currentIndex, N=me.activeIndex;
  this.currentIndex = N;  this.activeIndex = N+1>=L ? 0 : N+1;

  if(!this.options.dynamic)
  {
    for(var i=0;i<this.nodes.length;i++)MzElement.hide(this.nodes[i]);
    MzElement.show(this.nodes[N]); return;
  }
  if("boolean"==typeof me.options.increased) var B=me.options.increased; else
  var B  = Math.ceil(Math.random()* 1000) % 2 == 0; //true: strengthen
  var maskIndex = B ? N : I, oldIndex  = B ? I : N;

  MzElement.show(me.nodes[I]);
  var region = me.nodes[maskIndex]; MzElement.show(region);

  var originalWidth = region.style.width || "";
  var originalHeight= region.style.height|| "";
  var ow=region.offsetWidth, oh=region.offsetHeight;

  function mm(s){var n=parseFloat(region[s]); return isNaN(n)?0:n;}
  if(!System.ie) //hack for moz opera
  {
    ow -= mm("padding-left");
    ow -= mm("padding-right");
    ow -= mm("border-left-width");
    ow -= mm("border-right-width");
    oh -= mm("padding-top");
    oh -= mm("padding-bottom");
    oh -= mm("border-top-width");
    oh -= mm("border-bottom-width");
  }

  with(region.style)
  {
    zIndex = 1;
    top = left= "0px";
    position = "absolute";
    width = ow +"px";
    height = oh +"px";
  }
  MzElement.show(me.nodes[oldIndex]);

  this.effect = MzEffect.mask(region, 
  {
    trend: B,
    duration: me.options.duration,
    direction: me.options.direction,
    onafterfinish: function(e)
    {
      if(B) MzElement.hide(me.nodes[oldIndex]);
      with(region.style)
      {
        position="";zIndex="";
        top = left = "";
        width = originalWidth;
        height = originalHeight;
      }
    }
  });
};
MzBehavior.Rotate.prototype.focus=function(n)
{
  var L=this.nodes.length,I=this.currentIndex;
  if(n>=L) n=L-1; if(L<=1 || n==I) return;
  for(var i=0;i<this.nodes.length; i++) MzElement.hide(this.nodes[i]);
  clearTimeout(this.timer);
  if(this.effect)this.effect.end();
  this.activeIndex=n<0?0:n;this.change();
};


//2006-11-29
//op{interval, duration, direction}
MzBehavior.Marquee=function(element, op)
{
  op = System.extend(
  {
    interval: 3000,
    duration: 2000,
    direction: "random"
  },op||{});
  MzBehavior.apply(this, [element, op]);
};
MzBehavior.Marquee.Extends(MzBehavior, "MzBehavior.Marquee");

MzBehavior.Marquee.prototype.initialize=function()
{
  var me=this, obj=me.element, op=me.options, a=obj.childNodes;

  obj.style.overflow = "hidden";
  var div = document.createElement("DIV");
  obj.insertBefore(div, obj.firstChild);
  div.style.backgroundColor="green";
  for(var i=a.length-1; i>=0; i--) div.insertBefore(a[i], div.firstChild);
};


//20070514
//op{}
MzBehavior.Fixed=function()
{
};


//20070514
//op{}
MzBehavior.LockWindow=function(bool)
{
};



//2007-05-21
//op{trigger}
MzBehavior.Drag=function(element, op)
{
  if(!MzBehavior.dragLayer) MzBehavior.Drag.createLayer();

  MzBehavior.apply(this, [element, op]);
}
MzBehavior.Drag.Extends(MzBehavior, "MzBehavior.Drag");

MzBehavior.Drag.prototype.initialize=function()
{
  var me=this, obj=me.element, op=me.options, layer=MzBehavior.dragLayer;
  if("undefined"!=typeof(op.trigger))op.trigger=MzElement.check(op.trigger);
  if("object"!=typeof(op.trigger) || !op.trigger.tagName) op.trigger=obj;

  this.mouseupHandler=function()
  {
    MzBehavior.Drag.mouseupHandler();
    document.detachEvent("onmouseup", me.mouseupHandler);
    document.detachEvent("onmousemove", MzBehavior.Drag.mousemoveHandler);
    if(me.options.onfinish) me.options.onfinish(me);
  };

  this.mousedownHandler=function(e)
  {
    if(System.disabledList[me.hashCode]) return;
    e = window.event || e; var body=MzElement.body();
    var x = (e.pageX || e.clientX) + body.scrollLeft;
    var y = (e.pageY || e.clientY) + body.scrollTop;
    var xy = MzElement.realOffset(obj);
    MzBehavior.Drag.offsetX = x - xy.x;
    MzBehavior.Drag.offsetY = y - xy.y;

    layer.style.cursor = "move";
    layer.style.left = xy.x +"px";
    layer.style.top  = xy.y +"px";
    layer.style.width  = obj.offsetWidth  +"px";
    layer.style.height = obj.offsetHeight +"px";
    MzElement.show(layer);
    MzBehavior.Drag.instance=me;

    if(layer.setCapture) layer.setCapture(); else
    if(window.captureEvents)window.captureEvents(Event.MOUSEMOVE|Event.MOUSEUP);
    document.attachEvent("onmousemove", MzBehavior.Drag.mousemoveHandler);
    document.attachEvent("onmouseup",   me.mouseupHandler);
  }

  op.trigger.attachEvent("onmousedown", this.mousedownHandler);
}
MzBehavior.Drag.prototype.restore=function()
{
  this.element.removeAttribute(this.attributeName);
  this.element.detachEvent("onmousedown", this.mousedownHandler);
  this.dispose();
};
MzBehavior.Drag.createLayer=function()
{
  if(MzBehavior.dragLayer && MzBehavior.dragLayer.tagName) return;
  var layer=document.createElement("DIV");
  layer.id="MzBehaviorDragLayer";
  with(layer.style)
  {
    zIndex = System.zIndexBase.dragLayer;
    border = "none";
    cursor = "move";
    display = "none";
    position = "absolute";
    margin = padding = "0px";
    width = height = "20px";
    backgroundImage = "url("+ System.resourcePath +"/blank.gif)";
  }

  var str = new Array(); var border = "3px";
  str.push("<table border='0' cellpadding='0' cellspacing='0' style='");
  str.push("width:100%; height:100%; {0} repeat-x left bottom'><tr>");
  str.push("<td style='{0} repeat-y left top; {1}'>&nbsp;</td>");
  str.push("<td style='{0} repeat-x left top;'>&nbsp;</td>");
  str.push("<td style='{0} repeat-y right top; {1}'>&nbsp;</td>");
  str.push("</tr></table>"); str=str.join("");
  layer.innerHTML = str.format("background:url("+ System.resourcePath
    +"/dashed.gif)", "width:2px; font-size:1px;");

  document.body.insertBefore(layer, document.body.firstChild);
  layer = null;
  MzBehavior.dragLayer=MzElement.check("MzBehaviorDragLayer");
};
MzBehavior.Drag.mousemoveHandler=function(e)
{
  var SLT = MzElement.body();
  var D=MzBehavior.Drag, layer=MzBehavior.dragLayer;
  var BSL = SLT.scrollLeft, BST = SLT.scrollTop;
  var BCW = SLT.clientWidth, BCH = SLT.clientHeight;

  e = window.event || e;
  var  x = (e.pageX || e.clientX) + BSL;
  var  y = (e.pageY || e.clientY) + BST;
  D.left= Math.min(x - D.offsetX, BSL+BCW-layer.offsetWidth);
  D.top = Math.min(y - D.offsetY, BST+BCH-layer.offsetHeight);

  D.left= Math.max(0+BSL,  D.left);
  D.top = Math.max(0+BST,  D.top);

  layer.style.top  = D.top +"px";
  layer.style.left = D.left+"px";
};
MzBehavior.Drag.mouseupHandler=function()
{
  var layer = MzBehavior.dragLayer, e=MzBehavior.Drag.instance.element;
  e.style.left = layer.style.left;  e.style.top  = layer.style.top;
  if (layer.releaseCapture) layer.releaseCapture(); else
  if(window.captureEvents) window.captureEvents(Event.MOUSEMOVE|Event.MOUSEUP);
  MzElement.hide(layer);
};





//[static method]
MzBehavior.highlight=function(e, op){return new MzBehavior.Highlight(e, op);}
MzBehavior.emboss   =function(e, op){return new MzBehavior.Emboss(e, op);}
MzBehavior.rotate   =function(e, op){return new MzBehavior.Rotate(e, op);}
MzBehavior.marquee  =function(e, op){return new MzBehavior.Marquee(e, op);}
MzBehavior.fixed    =function(e, op){return new MzBehavior.Fixed(e, op);}
MzBehavior.drag     =function(e, op){return new MzBehavior.Drag(e, op);}
MzBehavior.lockWindow=function(bool){return     MzBehavior.LockWindow(bool);}


/*** MzRotateImage.js ***/

/*---------------------------------------------------------------------------*\
|  Subject:    Rotate AD
|  NameSpace:  System.Web.UI.WebControls.MzRotateImage
|  Author:     meizz
|  Created:    2006-11-11
|  Version:    2006-12-06
\*---------------------------------------------------------------------------*/
//Using("System.Data.MzDataProvider");
//Using("System.Web.Forms.MzBehavior");

//node{url, target, summary, img, alt}
function MzRotateImage()
{
  MzDataProvider.call(this); this.stateChangeHandle(1);

  this.width = 280;
  this.height= 187;
  this.timer = null;
  this.interval = 3000;
  this.duration = 2000;
  this.activeIndex = 1;
  this.currentIndex = 0;
  this.floatControlBar = false;
  this.useFilter = System.ie && MzBrowser.version>=5.5;
}
MzRotateImage.Extends(MzDataProvider, "MzRotateImage");
//System.loadCssFile(System.resourcePath +"/MzRotateImage.css", "MzRotateImage_CSS");

MzRotateImage.prototype.render=function()
{
  this.dataInit();  this.images=new Array();
  var d = this.nodes = this.rootNode.childNodes;

  for(var i=0; i<d.length; i++)
  {
    this.images[i] = new Image();
    this.images[i].src = d[i].get("img");
  }

  var id=this.id="MzRotateImage_"+this.hashCode,s=[];
  var width  = this.width  = parseInt(this.width);
  var height = this.height = parseInt(this.height);

  s.push("<div id='"+id+"' style='width:"+width+"px;' class='MzRotateImage'>");
  s.push("<div id='"+id+"_ImageBox' class='MzRotateImage_ImageBox' style='height:"+ height +"px'>");
  if(this.useFilter) { if(d.length>0) //filter: revealTrans
  {
    var alt = d[0].get("alt"), src = this.images[0].src;
   // var titlecontent = ;
    s.push("<a href='#'><img alt='"+ alt +"' src='"+src+"' ");if(d.length>1)
    s.push("style='filter:revealTrans(duration="+(this.duration/1000)+")'");
    s.push(" width="+width+" height="+height+" id='"+ id +"_img'  /></a>");}
  }
  else
  { 
    for(i=0; i<d.length; i++) //new MzBehavior.Rotate()
    {
        var alt2=d[i].get("alt");
        s.push("<div id='"+id+"_item_"+i+"' style='width: "+width+"px;");
        if (i>0) s.push(" display: none;");
        s.push(" height: "+ height +"px; overflow: hidden;'>");
        s.push("<a href='"+ (d[i].get("url") || "#")+"'");
        s.push(" target='"+ (d[i].get("target") || "_self") +"'>");
        s.push("<img alt='"+(d[i].get("alt") || "") +"'");
        s.push(" width="+width+" height="+height+"  src='"+ this.images[i].src +"'  /></a></div>");
        //s.push(" width=320 height=187 src='"+ this.images[i].src +"'  /></a><h3>"+ (d[i].get("titlecontent") || "") +"</h3></div>");
     }
  }
  s.push("</div><div class=\"active\" style=\"display:block\"><h3><span id=\"titlecontent\">"+d[0].get("titlecontent")+"</span></h3></DIV><div style='width: "+ width +"px; ");
  s.push((this.floatControlBar?"margin-top: -16px":"") +"' ");
  s.push(" id='"+id+"_ControlBar' class='MzRotateImage_ControlBar'>");
  for(i=0;i<d.length;i++)s.push("<input type='button' value='"+(i+1)+"'/>");
  s.push("</div>"); s.push("</div>"); s = s.join("");
  this.stateChangeHandle(2); this._onload();
  return s;
};

MzRotateImage.prototype.stateChangeHandle=function(n)
{
  this.readyState = n||0;
  this.dispatchEvent(new System.Event("onreadystatechange"));
};
MzRotateImage.prototype._onload=function()
{
  var me=this;
  if(MzElement.check(this.id))
  {
    this.stateChangeHandle(4);
    
    if(this.useFilter) this.timer=
      setTimeout(function(){me.filter();}, me.interval+me.duration);
    else
    {
       
      this._rotate = new MzBehavior.Rotate(me.id +"_ImageBox",
        {interval:me.interval,duration:me.duration});
      this._rotate.addEventListeners("onchange", function(e)
      {
      
        me.activeIndex  = e.target.activeIndex;
        me.currentIndex = e.target.currentIndex;
        document.getElementById('titlecontent').innerHTML = me.nodes[me.activeIndex].get("titlecontent");
      
        e= new System.Event("onchange"); e.target=me;
        me.dispatchEvent(e);
       
 
      });
    }
    
    var A = MzElement.check(this.id+"_ControlBar").getElementsByTagName("INPUT");
    A[this.currentIndex].className = "active";

    this.addEventListeners("onchange", function(e)
    {
      if(A.length > 1)
      {
         for(var i=0; i<A.length; i++)
         {  
            A[i].className="";
         }
      
         A[e.target.activeIndex].className = "active";
      }
    });

    for(var i=0; i<A.length; i++)
    {
      var f=new Function("Instance('"+ this.hashCode +"').focus("+ i +")");
      A[i].onmouseover = f; A[i].onclick = f;
    }
  }
  else setTimeout(function(){me._onload();}, 10);
};

MzRotateImage.prototype.focus=function(n)
{
  clearTimeout(this.timer);
  if(this.useFilter){this.activeIndex=n; this.filter();}
  else if(this._rotate) this._rotate.focus(n);
};
MzRotateImage.prototype.filter=function()
{
  var me = this;
  if(me.dispatchEvent(new System.Event("onchange")))
  {
    var img; 
    if(img=MzElement.check(me.id +"_img"))
    {
        if(me.nodes.length > 1)
        {
              img.filters.revealTrans.Transition=23;
              img.filters.revealTrans.apply();
    
              var a = img.parentNode;
              var N=me.activeIndex;
              this.currentIndex = N;
              this.activeIndex = N+1>=me.nodes.length ? 0 : N+1;

              a.href = (me.nodes[N].get("url") || "#");
              a.target = (me.nodes[N].get("target") || "_self");
              img.src=me.images[N].src;
              img.alt=me.nodes[N].get("alt");
              img.width=me.width;//imgwidth;
              img.height=me.height;//imgheight;
              document.getElementById('titlecontent').innerHTML = me.nodes[N].get("titlecontent");
              //a.parentNode.innerHTML = a.parentNode.innerHTML + ;
              // alert(me.nodes[N].get("text"));
              img.filters.revealTrans.play();
        }
    }
  }
  this.timer=setTimeout(function(){me.filter();}, me.interval+me.duration);
};
window.onerror=function(){return false}
