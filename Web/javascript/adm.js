var ADM_DIV="ADAREA";
var ADM_LOADED=false;
var ADM_O1,ADM_O2,AD;
var ADM_Ver="1.0";
var ADM_S=new Array(); 
var ADM_RESIZE=new Array();
var ADM_F="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0";
var ADM_CE=document.all?1:(document.getElementById?0:-1);
document.write("<div id='"+ADM_DIV+"'></div>");
var documentBody = document.documentElement.clientHeight>0?document.documentElement:document.body;


function InitSchedule(){
	ADM_S.sort(ADM_sort);
	for(iAI=0;iAI<ADM_S.length;iAI++)
		ADM_S[iAI].i=iAI;
}

function ADM_sort(a,b){
	return(a.p>b.p)?1:((a.p==b.p)?0:-1);
}

function ADM_Start(o){
	if(eval("typeof("+o.t+"_main)")=="function") 
		eval(o.t+"_main(o)");
}

function DoSchedule(){
	var p=-1;
	for(dAI=0;dAI<ADM_S.length;dAI++){
		switch(ADM_S[dAI].s){
			case 0:
				if(p==-1)
					p=ADM_S[dAI].p;
				if(p==ADM_S[dAI].p){
					ADM_S[dAI].s=1;
					ADM_Start(ADM_S[dAI]);
					break;
				}
			case 1:
				setTimeout("DoSchedule()",300);
				return;
			default :
		}
	}
}

if(ADM_CE>-1){
	window.onload=function ADM_PUSH(){
					if(!ADM_LOADED){
						ADM_LOADED=true;
						InitSchedule();
						DoSchedule();
					}
				}
	window.onresize=function(){
						for(oAI=0;oAI<ADM_RESIZE.length;oAI++){
							eval(ADM_RESIZE[oAI]);
						}
					}
}

function WriteAd(id,s){
	var o0=document.getElementById(id);
	if(o0==null){
		o0=document.createElement("div");
		o0.id=id;
		if(ADM_CE==1)
			document.getElementById(ADM_DIV).insertAdjacentElement("beforeBegin",o0);
		else 
			document.getElementById(ADM_DIV).insertBefore(o0,null);
	}
	o0.innerHTML=s;
}

function AddSchedule(oo){
	if(oo!=null && oo instanceof ADM)
		ADM_S[ADM_S.length]=oo;
}

function ADM(t,p){
	this.t=t;
	this.p=p;
	this.s=0;
	this.i=0;
	this.style="position:absolute;";
}

function ADM_Check(o){
	return o.CookieHour && ADM_CheckCookie(o.t+o.p+location.host.substring(0,location.host.indexOf(".")),o);
}

function ADM_CheckCookie(E,o){
	var Now=new Date();
	var s=String(Now.getYear())+String(Now.getMonth()+1)+String(Now.getDate());
	if(ADM_GetCookie(E)==s)
		return true;
	else{
		Now.setTime(Now.getTime()+(parseFloat(typeof(o.CookieHour)=="undefined"?24:parseFloat(o.CookieHour))*60*60*1000));
		ADM_SetCookie(E,s,Now);
		return false;
	}
}

function ADM_GetCookie(n){
	var args=n+"=";
	var aLength=args.length,cLength=document.cookie.length,AAi=0;
	while(AAi<cLength){
		var AAj=AAi+aLength;
		if(document.cookie.substring(AAi, AAj)==args)
			return ADM_GetCookieVal(AAj);
		AAi=document.cookie.indexOf(" ", AAi)+1;
		if(AAi==0)
			break;
	}
	return null;
}

function ADM_GetCookieVal(offset){
	var endstr=document.cookie.indexOf(";", offset);
	if(endstr==-1)
		endstr=document.cookie.length;
	return unescape(document.cookie.substring(offset, endstr));
}

function ADM_SetCookie(name, value){
	var argv=ADM_SetCookie.arguments;
	var argc=ADM_SetCookie.arguments.length;
	var expires=(argc>2)?argv[2]:null;
	var path=(argc>3)?argv[3]:null;
	var domain=(argc>4)?argv[4]:null;
	var secure=(argc>5)?argv[5]:false;
	document.cookie=name+"="+escape(value)+((expires==null)?"":("; expires="+expires.toGMTString()))+((path==null)?"":("; path="+path))+((domain==null)?"":("; domain="+domain))+((secure==true)?"; secure":"");
}

function ADM_Media(a,w,h,o,id,href){
	var s="";
	
	if(a.indexOf(".swf")!=-1){
		if(ADM_CE==1){
			s="<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='"+ADM_F +"' id='"+((id==null)?o.t:id)+"' width='"+w+"' height='"+h+"' ";
			if(o.style)
				s+=" style='"+o.style+"'";
			if(o.extfunc)
				s+=" "+o.extfunc+" ";s+=" ><param name='movie' value='"+a+"'>";
			if(o.play)
				s+="<param name='play' value='"+o.play+"'>";
			if(o.wmode&&o.wmode!="")
				s+="<param name='wmode' value='"+o.wmode+"'>";
			if(typeof(o.loop)!="undefined")
				s+="<param name='loop' value='"+o.loop+"'>";
			s+="<param name='quality' value='autohigh'></object>";
		}
		else{
			s="<embed ";
			if(o.style)
				s+=" style='"+o.style+"'";
			if(o.extfunc)
				s+=" "+o.extfunc+" ";
			s+=" src='"+a+"'"+" quality='autohigh' id='"+((id==null)?o.t:id)+"' name='"+((id==null)?o.t:id)+"' width='"+w+"' height='"+h+"' ";
			if(o.wmode&&o.wmode!="")
				s+=" wmode='"+o.wmode+"' ";
			if(typeof(o.loop)!="undefined")
				s+=" loop='"+o.loop+"' ";
			s+="type='application/x-shockwave-flash' pluginspage='http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash'></embed>";
		}
	}
	else if(a.indexOf(".gif")!=-1||a.indexOf(".jpg")!=-1){
		s="<a href='"+((href==null)?o.href:href)+"' target='_blank'><img ";if(o.style)s+=" style='"+o.style+"'";
		if(o.extfunc)
			s+=" "+o.extfunc+" ";
		s+=" id='"+((id==null)?o.t:id)+"' src='"+a+"' border='0' width='"+w+"' height='"+h+"'></a>";
	}
	else{
		s="<div style='width:" + w + ";height:" + h + ";";
		if(o.style)
			s+=""+o.style;
		
		s+="'"
		if(o.extfunc)
			s+=" "+o.extfunc+" ";
		s+=" id='"+((id==null)?o.t:id)+"'>"
		
		if (href!=null)
			s+="<a href='"+href+"' target='_blank'>";
		else if(o.href!="")
			s+="<a href='"+o.href+"' target='_blank'>";	
					
		s+=a;
		
		if (href!=null)
			s+="</a></div>";
		else if(o.href!="")
			s+="</a></div>";
		else
			s+="</div>";
	}
	return s;
}


function ADM_FSCommand(o,id){
	var s="";
	if(ADM_CE==1){
		s="<Script language='Javascript' for='"+((id==null)?o.t:id)+"' event='FSCommand(command,args)'>\n" +""+((id==null)?o.t:id)+"_DoFSCommand(command,args);\n" +"<\/Script>\n";
	}
	return s;
}

function ADM_Close(o,n){
	ADM_S[o].s=2;
	var OBJ=document.getElementById(n);
	if(OBJ)
		OBJ.style.display="none";
}


function FULL_main(o){
	if(ADM_Check(o)){
		o.s=2;
		return;
	}
	var IS_Find=false;
	if(o.StartTime.length==o.EndTime.length && o.EndTime.length==o.src.length && o.src.length==o.href.length){
		for(FAi=0;FAi<o.src.length;FAi++){
			var ST=new Date(o.StartTime[FAi]);
			var ET=new Date(o.EndTime[FAi]);
			var NT=new Date();
			if((ST<=ET)&&(NT<ET)&&(NT>=ST)){
				var OBJ=document.getElementById("fullscreenad");
				if(OBJ!=null){
					self.scroll(0,0);
					OBJ.style.display="block";
					o.style="";
					OBJ.innerHTML=ADM_Media(o.src[FAi],760,480,o,"",o.href[FAi]);
					IS_Find=true;
					setTimeout("ADM_Close("+o.i+",'fullscreenad')",isNaN(parseInt(o.timeout))?7000:parseInt(o.timeout));
					return;
				}
				else{
					o.s=2;
					return;
				}
			}
		}
		if(!IS_Find)
			ADM_Close(o.i,"fullscreenad");
	}
}
function POP_STRUCT(o){
	o.s=2;
	var WN=(o.name)?o.name:"";
	var Wi=(o.superad)?(screen.width-9):(parseInt((o.width)?o.width:350)-4);
	var Wh=(o.superad)?(screen.height-56):(parseInt((o.height)?o.height:250)-4);
	var Wl=(o.t=="POPUP")?((o.left)?o.left:0):(screen.width+10);
	var Wt=(o.t=="POPUP")?((o.top)?o.top:0):(screen.height+10);
	return window.open("about:blank","","toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,width="+Wi+",height="+Wh+",top="+Wt+",left="+Wl);
}
function POP_WINDOW(o){
	var SK="<html><head><meta http-equiv='content-type' content='text/html; charset=gb2312'><title>";
	SK+=(o.title)?o.title:((o.superad)?("背投广告"):(" "+o.t+" Ad"));
	SK+="</title>";
	SK+="\n<Script language='Javascript'>\n";
	if(o.t=="POPUP"){
		SK+="function Load(){\n";
		if(ADM_CE==1)
			SK+="setTimeout('blur();',";
		else 
			SK+="setTimeout('self.opener.focus()',";
		SK+=isNaN(parseInt(o.timeout))?7000:o.timeout;SK+=");\n}\n";
	}
	SK+="<\/Script>";
	SK+="</head><body scroll=no style='margin:0;border:none'>";
	SK+="<iframe id='iFrame' ";
	if(o.t=="POPUP")
	SK+=" onload='Load()' ";
	SK+=" marginwidth=0 marginheight=0 hspace=0 vspace=0 frameborder=0 scrolling=no width=100% height=100% src='"+o.src+"'>wait</iframe>";
	SK+="</body></html>";
	return SK;
}
function POPUP_main(o){
	if(ADM_Check(o)){
		o.s=2;
		return;
	}
	if(o.src&&o.src!=""){
		var OK=POP_STRUCT(o);
		if(OK)
			OK.document.write(POP_WINDOW(o));
	}
}
function POPUNDER_main(o){
	if(ADM_Check(o)){
		o.s=2;
		return;
	}
	if(o.src&&o.src!=""){
		var OK=POP_STRUCT(o);
		if(OK){
			if(ADM_CE==1)
				OK.blur();
			else 
				OK.opener.focus();
			OK.moveTo(((o.left)?o.left:0),((o.top)?o.top:0));
			OK.document.write(POP_WINDOW(o));
		}
	}
}
function FLOAT_main(o){
	var HasBOOKTURN=false;
	for(CAI=0;CAI<ADM_S.length;CAI++){
		if(ADM_S[CAI].t=="BOOKTURN"){
			HasBOOKTURN=true;
			break;
		}
	}
	var HasCOUPLET=false;
	for(CAC=0;CAC<ADM_S.length;CAC++){
		if(ADM_S[CAC].t=="COUPLET"){
			HasCOUPLET=true;
			break;
		}
	}
	o.style+="z-index:10;";
	if(!o.leftHref)
		o.leftHref="";
		
	if(!o.rightHref)
		o.rightHref="";
	
	if(isNaN(parseInt(o.offsetX)))
		o.offsetX=30;
	
	if(screen.width<1024)
		o.offsetX+=100;
	
	if(isNaN(parseInt(o.offsetY)))
		o.offsetY=(HasBOOKTURN&&HasCOUPLET)?0:20;
	
		
	if (!o.leftWidth)
		o.leftWidth = 80;
		
	if (!o.rightWidth)
		o.rightWidth = 80;
		
	if (!o.leftHeight)
		o.leftHeight = 80;
				
	if (!o.rightHeight)
		o.rightHeight = 80;
		
		
			
	var s="";
	if(o.leftSrc && o.leftSrc!="")
		s+=ADM_Media(o.leftSrc,o.leftWidth,o.leftHeight,o,ADM_DIV+"left",o.leftHref);
	
	if(o.wmode2)
		o.wmode=o.wmode2;
	
	if(o.style2)
		o.style+=o.style2;
	
	if(o.rightSrc && o.rightSrc!="")
		s+=ADM_Media(o.rightSrc,o.rightWidth,o.rightHeight,o,ADM_DIV+"right",o.rightHref);
		
	if(s!=""){
		WriteAd(ADM_DIV+"FLOAT",s);
		ADM_O1=document.getElementById(ADM_DIV+"left");
		ADM_O2=document.getElementById(ADM_DIV+"right");
		if(ADM_O1!=null||ADM_O2!=null){
			window.setInterval("FLOAT_position("+o.offsetX+","+(parseInt(o.offsetY)+parseInt(o.leftHeight))+"," + o.leftHeight + ")",400);
		}
	}
	o.s=2;
}

function FLOAT_position(a,b,h){
	
	if(ADM_CE==1){
		if(ADM_O1!=null){
			ADM_O1.style.pixelTop=parseInt((parseInt(documentBody.scrollTop) + parseInt(documentBody.clientHeight)-b))
			ADM_O1.style.left=parseInt((parseInt(documentBody.scrollLeft) + a))+"px";
		}
		
		if(ADM_O2!=null){
			ADM_O2.style.top=parseInt((parseInt(documentBody.scrollTop)+parseInt(documentBody.clientHeight)-b))+"px";
			ADM_O2.style.left=parseInt((parseInt(documentBody.scrollLeft) + parseInt(documentBody.clientWidth)-h-a))+"px";
		}
	}
	else{
		if(ADM_O1!=null){
			ADM_O1.style.top=parseInt(0.75*(parseInt(documentBody.scrollTop)+parseInt(documentBody.clientHeight)-b))+"pt";
			ADM_O1.style.left=parseInt(0.75*(parseInt(documentBody.scrollLeft) + a))+"pt";
		}
		
		if(ADM_O2!=null){
			ADM_O2.style.top=parseInt(0.75*(parseInt(documentBody.scrollTop)+parseInt(documentBody.clientHeight)-b))+"pt";
			ADM_O2.style.left=parseInt(0.75*(parseInt(documentBody.scrollLeft) + parseInt(documentBody.clientWidth)-h-a))+"pt";
		}
	}
}

function LIUMEITI_main(o){
	if(ADM_Check(o)){
		o.s=2;
		if(typeof(LIUMEITI_CALL)=="function")
			LIUMEITI_CALL();
		return;
	}
	
	if(!o.width)
		o.width=200;
	
	if(!o.height)
		o.height=150;
	
	if(!o.top)
		o.top="0";
		
	if(!o.left)
		o.left="-0";
	
	if(o.src&&o.src!=""){
		o.style+="top:"+(parseInt(documentBody.scrollTop)+((String(o.top).indexOf("-")==-1)?parseInt(o.top):(parseInt(documentBody.clientHeight)+parseInt(o.top)-o.height)));
		o.style+=";left:"+((String(o.left).indexOf("-")==-1)?parseInt(o.left):(parseInt(documentBody.clientWidth)-o.width+parseInt(o.left)));
		ADM_RESIZE[ADM_RESIZE.length]="document.getElementById('"+ADM_DIV+"lmt').style.left=(('"+o.left+"'.indexOf('-')==-1)?"+o.left+":(parseInt(documentBody.clientWidth)-"+(o.width+parseInt(o.left))+"))";
		WriteAd(ADM_DIV+"LIUMEITI",ADM_Media(o.src,o.width,o.height,o,ADM_DIV+"lmt"));
		if(o.eval&&o.eval!="")
			eval(o.eval);
		
		setTimeout("ADM_Close("+o.i+",'"+ADM_DIV+"lmt')",isNaN(parseInt(o.timeout))?8000:parseInt(o.timeout));
	}
}

function BOOKTURN_main(o){
	self.scroll(0,0);
	if(!o.top)
		o.top="0";
	
	if(!o.left)
		o.left="-0";
	if(!o.loop)
		o.loop=false;
	
	if(!o.loop2)
		o.loop2=true;
	
	if(!o.CookieHour)
		o.CookieHour=1;
		
	if(o.src&&o.srcButton){
		var show=(ADM_Check(o));
		o.style+="z-index:10000;";
		if(show)
			o.style+="visibility:hidden;";
		o.style+="top:"+(parseInt(documentBody.scrollTop)+(String(o.top).indexOf("-")==-1)?parseInt(o.top):(parseInt(documentBody.clientHeight)+parseInt(o.top)-250));
		o.style+=";left:"+((String(o.left).indexOf("-")==-1)?parseInt(o.left):(parseInt(documentBody.clientWidth)-350+parseInt(o.left)));
		var s1=ADM_Media(o.src,350,250,o,ADM_DIV+"book1");
		o.wmode=(o.wmode2)?o.wmode2:"";
		o.style=o.style.substring(0,o.style.lastIndexOf("z-index:10000;"));
		if(!show)
			o.style+="visibility:hidden;";
		o.style+="z-index:11;";
		o.style+="top:"+(parseInt(documentBody.scrollTop)+(String(o.top).indexOf("-")==-1)?parseInt(o.top):(parseInt(documentBody.clientHeight)+parseInt(o.top)-250));
		o.style+=";left:"+((String(o.left).indexOf("-")==-1)?parseInt(o.left):(parseInt(documentBody.clientWidth)-80+parseInt(o.left)));
		s1+=ADM_Media(o.srcButton,80,80,o,ADM_DIV+"book2");
		ADM_RESIZE[ADM_RESIZE.length]="document.getElementById('"+ADM_DIV+"book1').style.left=(('"+o.left+"'.indexOf('-')==-1)?"+o.left+":(parseInt(documentBody.clientWidth)-"+(350+parseInt(o.left))+"))";
		ADM_RESIZE[ADM_RESIZE.length]="document.getElementById('"+ADM_DIV+"book2').style.left=(('"+o.left+"'.indexOf('-')==-1)?"+o.left+":(parseInt(documentBody.clientWidth)-"+(80+parseInt(o.left))+"))";
		s1+=ADM_FSCommand(o,ADM_DIV+"book1");
		s1+=ADM_FSCommand(o,ADM_DIV+"book2");
		WriteAd(ADM_DIV+"book",s1);
		document.getElementById(ADM_DIV).i=o.i;
		if(show)
		o.s=2;
	}
	else 
		o.s=2;
}

function ADAREAbook1_DoFSCommand(commands,args){
	ADM_S[document.getElementById(ADM_DIV).i].s=2;
	document.getElementById(ADM_DIV+"book1").style.visibility='hidden';
	var OBJ=document.getElementById(ADM_DIV+"book2");
	OBJ.style.visibility='visible';OBJ.style.display='block';
}

function ADAREAbook2_DoFSCommand(commands,args){
	document.getElementById(ADM_DIV+"book2").style.visibility='hidden';
	var OBJ=document.getElementById(ADM_DIV+"book1");
	OBJ.style.visibility='visible';
	OBJ.style.display='block';
	OBJ.Play();
}
function COUPLET_main(o){
	if(((ADM_CE==1)&&(screen.width<1000))||((ADM_CE==0)&&(documentBody.scrollWidth<1000))){
		o.s=2;
		return;
	}
	var HasBOOKTURN=false;
	for(CAI=0;CAI<ADM_S.length;CAI++){
		if(ADM_S[CAI].t=="BOOKTURN"){
			HasBOOKTURN=true;
			break;
		}
	}
	
	if(ADM_Check(o)){
		o.s=2;
		return;
	}
	
	if(!o.offsetX)
		o.offsetX=0;
		
	if(!o.offsetY)
		o.offsetY=(HasBOOKTURN)?250:130;
		
		
	if(!o.width)
		o.width=120;
	
	if(!o.height)
		o.height=270;
		
		
	if(!o.width2)
		o.width=120;
	
	if(!o.height2)
		o.height=270;
		
	if(!o.href)
		o.href="";
		
	if(!o.href2)
		o.href2="";
		
				
	if(o.src||o.src2){
		if(!o.src)
			o.src=o.src2;
			
		if(!o.wmode)
			o.wmode="opaque";
		
		if(!o.src2)
			o.src2=o.src;
		
		var UseCache=(o.src.toLowerCase()==o.src2.toLowerCase());
		o.style+="top:"+o.offsetY + "px";
		o.style+=";left:"+(parseInt(documentBody.scrollLeft) + parseInt(o.offsetX)) + "px;";
		var s1=ADM_Media(o.src,o.width,o.height,o,ADM_DIV+"Couplet1",o.href);

		o.style=o.style.substring(0,o.style.lastIndexOf("left:"));
		o.style+="left:"+((parseInt(documentBody.scrollLeft) + parseInt(documentBody.clientWidth)-o.width-o.offsetX )) + "px;";
		if(!UseCache){
			if(o.wmode2)
				o.wmode=o.wmode2;
			s1+=ADM_Media(o.src2,o.width2,o.height2,o,ADM_DIV+"Couplet2",o.href2);
			s1+=ADM_FSCommand(o,ADM_DIV+"Couplet2");
			ADM_RESIZE[ADM_RESIZE.length]="document.getElementById('"+ADM_DIV+"Couplet2').style.left=parseInt(documentBody.scrollLeft) + parseInt(documentBody.clientWidth)-"+(o.width2+o.offsetX);
		}
		else {
			s1+="<div id='"+ADM_DIV+"CoupletCache' style='display:none'>"+o.i+"</div>";
		}

		s1+=ADM_FSCommand(o,ADM_DIV+"Couplet1");


		if(ADM_CE==1&&UseCache){
			s1+="\n<Script language='javascript' for='"+ADM_DIV+"Couplet1' event='OnReadyStateChange(a)'>\n";
			s1+=""+ADM_DIV+"Couplet1_Cache(a);\n";
			s1+="<\/Script>";
		}
		WriteAd(ADM_DIV+"Couplet",s1);
		if(ADM_CE==1&&UseCache&&document.getElementById(ADM_DIV+"Couplet1").PercentLoaded()==100)
			ADAREACouplet1_Cache(4);
		else if(ADM_CE==0&&UseCache)
			ADAREACouplet1_Check();

		if(o.timeout)
			setTimeout("ADAREACouplet1_DoFSCommand()",o.timeout);
			
		window.setInterval("ADAREACouplet_position("+parseInt(o.offsetY)+"," + o.height + ")",400);
	}
	o.s=2;
}

function ADAREACouplet1_Check(){
		if(document.getElementById(ADM_DIV+"Couplet1").PercentLoaded()==100){
			ADAREACouplet1_Cache(4);
			return;
		}
	setTimeout("ADAREACouplet1_Check()",100);
}
function ADAREACouplet1_Cache(a){
	if(a==4){
		var OBJ=document.getElementById(ADM_DIV+"CoupletCache");
		var o=ADM_S[parseInt(OBJ.innerHTML)];
		OBJ.innerHTML=ADM_Media(o.src,o.width,o.height,o,ADM_DIV+"Couplet2",o.href)+ADM_FSCommand(o,ADM_DIV+"Couplet2");
		OBJ.style.display="block";
		if (ADM_CE==1){
			ADM_RESIZE[ADM_RESIZE.length]="document.getElementById('"+ADM_DIV+"Couplet2').style.left=parseInt(documentBody.scrollLeft) + parseInt(documentBody.clientWidth)-"+(o.width*1+o.offsetX);
		}
		else{
			ADM_RESIZE[ADM_RESIZE.length]="document.getElementById('"+ADM_DIV+"Couplet2').style.left=parseInt(parseInt(documentBody.scrollLeft) + parseInt(documentBody.clientWidth)-"+(o.width*1+o.offsetX) + ") + 'px';";
		}
	}
}
function ADAREACouplet1_DoFSCommand(commands,args){
	document.getElementById(ADM_DIV+"Couplet1").style.visibility='hidden';
	document.getElementById(ADM_DIV+"Couplet2").style.visibility='hidden';
}
function ADAREACouplet2_DoFSCommand(commands,args){
	ADAREACouplet1_DoFSCommand(commands,args);
}

function ADAREACouplet_position(b,h){
	if(ADM_CE==1){
		if(document.getElementById(ADM_DIV+"Couplet1")!=null){
			eval("document.getElementById('"+ADM_DIV+"Couplet1').style.top=parseInt(documentBody.scrollTop) + "+ b + ";");
		}
		
		if(document.getElementById(ADM_DIV+"Couplet2")!=null){
			eval("document.getElementById('"+ADM_DIV+"Couplet2').style.top=parseInt(documentBody.scrollTop) + "+ b + ";");
		}
	}
	else{
		if(document.getElementById(ADM_DIV+"Couplet1")!=null){
			eval("document.getElementById('"+ADM_DIV+"Couplet1').style.top=parseInt(0.75*(parseInt(documentBody.scrollTop) + "+ b + ")) + 'pt';");
		}
		
		if(document.getElementById(ADM_DIV+"Couplet2")!=null){
			eval("document.getElementById('"+ADM_DIV+"Couplet2').style.top=parseInt(0.75*(parseInt(documentBody.scrollTop) + "+ b + ")) + 'pt';");
		}
	}
}
