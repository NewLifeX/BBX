//-------------------------首页快速发帖功能------------------------//

 


Array.prototype.remove=function(dx)
{
    if(isNaN(dx)||dx>this.length){return false;}
    for(var i=0,n=0;i<this.length;i++)
    {
        if(this[i]!=this[dx])
        {
            this[n++]=this[i]
        }
    }
    this.length-=1
}

function resetroot()
{
	$('pathlist').innerHTML='<a href=\"javascript:void(0)\" onclick=\"getajaxforums(0);\">首页</a>';
}




function getajaxforums(fid,forumname,otype,parentid,applytopictype,topictypeselectoptions,topictypesidlist,postbytopictype)
{
	_sendRequest('tools/ajax.ashx?t=getajaxforums&fid='+fid, getajaxforums_callback, false);
	if(parentid!=0 && parentid!=undefined)
	{

		if(applytopictype==1 && topictypeselectoptions!='')
		{  
		  var str='';
		   var arr=topictypeselectoptions.split('|');
		   var arrid=topictypesidlist.split('|');
		   str+="<ul>";
		   for(var i=0;i<arr.length;i++)
		   {
		     if(parseInt(arrid[i])!=0)
			 {
				str+='<li>';
				str+='<a href=\"javascript:void(0)\" onClick=\"choosetopictypes(\''+arr[i]+'\','+arrid[i]+')\">'+arr[i]+'</a>';
				str+='</li>';
			 }
		   }
		   str+="</ul>";
		if(postbytopictype==1 && $('topicstypes').innerHTML.indexOf('必选')==-1)
		$('topicstypes').innerHTML+='(必选)';
		 
		var Hiddentypeid=document.createElement("INPUT");
		Hiddentypeid.id='typeid';
		Hiddentypeid.name='typeid';
		Hiddentypeid.type='hidden';
		$('postform').appendChild(Hiddentypeid);
		$('topicstypes_menu').innerHTML=str;
		$('topicstypescontainer').style.display='';
		}
		
	  $('postsubmit').removeAttribute("disabled");
		}
	else
	{
		$('postsubmit').setAttribute("disabled","disabled");
		}
	//if(forumname!=undefined)
	if(fid==0 && otype==undefined)
	{
	   resetroot();
	  // for(var i=0;i<pathlist.length;i++)
	   //{
		//pathlist.remove(i);
	   //}
	   pathlist=new Array();
	}
	if(parseInt(fid)!=0 && otype=="add")
	{
		pathlist["push"]({'forumname':'<a href=\"javascript:void(0)\" onclick=\"getajaxforums('+fid+',\''+forumname+'\',\'remove\','+parentid+');chooseforums(\''+forumname+'\','+fid+')\">'+forumname+'</a>','fid':fid});
		resetroot();
		for(var i=0;i<pathlist.length;i++)
		{
			$('pathlist').innerHTML +='&raquo;' + pathlist[i].forumname;
		}
	}
	else
	{
	  if(otype=="remove")
	  {
		 for(var i=0;i<pathlist.length;i++)
		  {
			 if(pathlist[i].fid==fid)
			 {
			   for(j=(i+1);j<=pathlist.length-1;j++)
			   {
				pathlist.remove(j);
			   }
			   break;
			 }
		  }

		resetroot();
		  for(var i=0;i<pathlist.length;i++)
		  {
			$('pathlist').innerHTML +='&raquo;' + pathlist[i].forumname;
		  }
		}
	}
}

function getajaxforums_callback(doc)
{
  var htmlstr='';
  var data=eval(doc)

  for (var ix=0; ix<data.length;ix++)
  { 
  var topictypeslist='';
  var topictypesidlist='';
    var re = /\<option value=\"(\d+)\"\>([^\<\>]+?)\<\/option\>/ig;
    var str=data[ix].topictypeselectoptions;
	while(matches = re.exec(str))
	{
	 topictypesidlist+=matches[1]+'|';
	 topictypeslist+=matches[2]+'|';
	}

	  
	  htmlstr+='<a href=\"javascript:void(0)\" onclick=\"getajaxforums('+data[ix].fid+',\''+data[ix].forumname+'\',\'add\','+data[ix].parentid+','+data[ix].applytopictype+',\''+topictypeslist+'\',\''+topictypesidlist+'\','+data[ix].postbytopictype+');chooseforums(\''+data[ix].forumname+'\','+data[ix].fid+')\">'+data[ix].forumname+'</a>';
  }
 $("forumtreelist").innerHTML=htmlstr;
}


function chooseforums(forumname,fid,bytopictype,topictype)
{
$('loginorreg').innerHTML=lastpostforumhtml;
$('userselectforum').innerHTML = forumname;
$('forumname').value=forumname;
$('forumid').value=fid;
postbytopictype=bytopictype;
topictypes=topictype;

}

function choosetopictypes(typename,typeid)
{
$('topicstypes').innerHTML=typename;
$('typeid').value=typeid;
}

function divtotextarea(type)
{
	if(type==1)
	{
  addqicktitleattribute('remove');
	$('quicktitletext').innerHTML ='<input type=\"text\" maxlength=\"60\" value=\"'+$('quicktitletext').innerHTML+'\" size=\"120\" id=\"titlehidden\" name=\"titlehidden\" tabindex=\"1\" onblur=\"divtotextarea(0)\" class=\"txt\"/>';
	}
	else
	{
    allowedittitle=false;
   addqicktitleattribute('add');



	if($('titlehidden'))
	{
	$('title').value=$('titlehidden').value;
	$('quicktitletext').innerHTML=$('titlehidden').value;
	}
	}
}


function changequicktitletext(t)
{
	if(allowedittitle)
	{
	$('quicktitletext').innerHTML=t.value.substring(0,60);
	$('title').value=t.value;
	}
}

function checkquicmessage(event)
{
	//|| $('quicktitletext').innerHTML.length>20
	if(in_array(event.keyCode,[32,188,190,229]))
	{
		if($('quicktitletext').innerHTML!='' &&  $('quicktitletext').innerHTML!=null)
		allowedittitle=false;
	}
}

	//document.onclick=function()
	//{
		//if(ismeesageopen)
		//{
		//testareafocus($('message'));
		//}
	//}
	//}
function addqicktitleattribute(reoradd)
{
	if(reoradd=='add')
	{
		if(is_ie)
		{
		   $('quicktitletext').setAttribute("onclick",function(){divtotextarea(1)});
		}
		else
		{
		  $('quicktitletext').setAttribute("onclick","divtotextarea(1)");
		}
	}
	else
	{

		 if(is_ie)
		 {
		$('quicktitletext').setAttribute("onclick",function(){});
		 }
		 else
		 {
			$('quicktitletext').removeAttribute("onclick");
		 }
	}
}

function textareafocus(t,type)
{

	if(type==1)
	{;

    $('vcodediv').style.display='';
  t.style.height=60+'px';
	}
	else
	{

  t.style.height=20+'px';
		}
}

function validateforumid(forum)
{
	if(forum.forumid.value=='')
	{
		alert("请选择板块");
		try{$("postsubmit").disabled = false;}catch(e){}
		return false;

		}
}

function inittopictype()
{
$('topicstypes').innerHTML='请选择主题分类';
$('topicstypes_menu').innerHTML='';
$('topicstypescontainer').style.display='none';
if($('typeid'))
{
$('postform').removeChild($('typeid'));
}
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


function ajaxposttopic(postbytopictype,topictypes)
{
    if($('title'))
	{
		$('title').value=$('message').value!=''?$('message').value.substr(0,20):'';
	}
	else
	{
		var Hiddentitle=document.createElement("INPUT");
		Hiddentitle.id='title';
		Hiddentitle.name='title';
		Hiddentitle.type='hidden';
		Hiddentitle.value=$('message').value!=''?$('message').value.substr(0,20):'';
		$('postform').appendChild(Hiddentitle);
	}
	
    if(postbytopictype==1 && topictypes!='')
	{
	    if($('typeid'))
		{
		    $('typeid').value=topictypes.split(',')[0];
		}
		else
		{
			var Hiddentypeid=document.createElement("INPUT");
			Hiddentypeid.id='typeid';
			Hiddentypeid.name='typeid';
			Hiddentypeid.type='hidden';
			Hiddentypeid.value=topictypes.split(',')[0];
			$('postform').appendChild(Hiddentypeid);
		}
	}
	//sendRequest_commentdebates('tools/ajax.ashx?tid=' + tid + '&t=addcommentdebates',messageid);			
	_sendRequest('posttopic.aspx?infloat=1&fromindex=1', ajaxpostttopic_callback, true, getRequestBody($('postform')));
	
}	



	
function ajaxpostttopic_callback(doc)
{    
     var t=getSingleNodeValue(doc,'root').toString().replace(/<br \/>/g," ");
	 if (!disablepostctrl && ((postminchars != 0 && mb_strlen($('message').value) < postminchars) || (postmaxchars != 0 && mb_strlen($('message').value) > postmaxchars))) 
	{
        ajaxinnerhtml($('loginorreg'),"<img src=\""+imagedir+"/check_error.gif\"/>您的帖子长度不符合要求。\n\n当前长度: " + mb_strlen($('message').value) + " 字节\n系统限制: " + postminchars + " 到 " + postmaxchars + " 字节");
        setTimeout("$('loginorreg').innerHTML=lastpostforumhtml;",3000);       
	    return;
    }
	 
	 if(t.indexOf('<p')!=-1)
	 {
	 
	  ajaxinnerhtml($('loginorreg'),t.replace("<p>","<p><img src=\""+imagedir+"/check_error.gif\"/>"));
	  setTimeout("$('loginorreg').innerHTML=lastpostforumhtml;",3000);
	 }
	 else
	 {
		 if($('vcodetext'))
		 {
		  $('vcodetext').value='';
		 }
	     $('message').value='';
		 
		  var currenturl=aspxrewrite==1?'showforum-'+$('forumid').value+'.aspx':'showforum.aspx?forumid='+$('forumid').value;
		  ajaxinnerhtml($('loginorreg'),'<img src=\'' + imagedir + '/data_valid.gif\'/>发表主题成功');
		  lastpostforumhtml='您将要在<a id="userselectforum" href="javascript:void(0)" onmouseover="showMenu(this.id)" class="drop">'+$('forumname').value+'<\/a>发帖'
		  var lastpostforumhtmlnew =lastpostforumhtml+'          <a href=\''+currenturl+'\'>进入此板块</a>'
		 //setTimeout("$('loginorreg').innerHTML=lastpostforumhtml;",1000);
		 $('message').style.height=20+'px';
		 setTimeout(function(){$('loginorreg').innerHTML=lastpostforumhtmlnew;},1000)
	 }
	 $('title').parentNode.removeChild($('title'));
	 if($('typeid'))
	 {
	 $('typeid').parentNode.removeChild($('typeid'));
	 }
	 return;
}

document.onclick = function(e)
{
   if($('fastpost'))
   {
		var event = window.event || e;
		//var ele=event.scrElement || event.target;
		var ele=event.srcElement ? event.srcElement : event.target

		var arr=new Array();
		var allelements=$('fastpost').getElementsByTagName('*');
		var d=new Date();
		for(var i=0;i<allelements.length;i++)
		{
		  if(allelements[i].id=='')
			allelements[i].id=d.getUTCHours().toString()+d.getUTCMinutes().toString()+d.getUTCSeconds().toString()+d.getUTCMilliseconds().toString()+i.toString();

		  arr.push(allelements[i].id);
		}	
		 
		if(!in_array(ele.id,arr))	 
		{	 
		$('message').style.height=20+'px';
		}  
	}
}