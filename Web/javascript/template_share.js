/*
javascript
*/

var share={
	floatwin : function(list){			
		//floatwin('open_forward', -1, 350, 140);
		//showWindow('mods','');
		
//		if(BROWSER.ie) {
//			showPrompt(null, null, '<span>' + this.show(list) + '</span>', 1500);
//		} else {
//		showDialog(this.show(list), 'info');
	//		}
	    showDialog(this.show(list), 'info');

		//$('floatwin_forward_title').innerHTML = '转发';
		//$('floatwin_forward_content').innerHTML = this.show(list);
		//$('floatwin_forward_content').style.margin='15px';
		//$('floatwin_forward_content').style.textAlign='left';
	},
	forward : function(c){
		(location.href = c) || (window.location = c);
	},
	show :function (list){
		var sharesite = list.split(",");
		var html = '';
		html += '<div class="c cl share">';
		for (var i= 0 ;i< sharesite.length ; i++)
		{
			var s = sharesite[i].split("|");  

			if (parseInt(s[3]) == 1)
			{
			  html +='<a href="javascript:void(0);" onclick="javascript:share.'+s[1]+'();return false;" Style="background:URL('+forumpath+'images/share/'+s[1]+'.gif) no-repeat 0 50%;margin-right:10px;white-Space:nowrap;padding:0px 0 4px 20px;float:left;display:inline;">'+s[2]+'</a>';
			}
		}
		html +='</div>'
		return html;
	},
	renren : function (){
		var c = "http://share.xiaonei.com/share/buttonshare.do?link=" + encodeURIComponent(location.href) + "&title=" + encodeURIComponent(topictitle);
		if (!window.open(c, "xiaonei", "toolbar=0,resizable=1,scrollbars=yes,status=1,width=626,height=436")) 
		//(location.href = c) || (window.location = c);
		this.forward(c);
	},
	kaixin001 : function (){
		var c = "http://www.kaixin001.com/repaste/share.php?rtitle=" + encodeURIComponent(topictitle) + "&rurl=" + encodeURIComponent(location.href) + "&rcontent=" + encodeURIComponent(topictitle);
		//(location.href = c) || (window.location = c);
		if (!window.open(c, "kaixin001", "location = yes,menubar=yes,resizable=1,scrollbars=yes,status=1,titlebar=yes,toolbar=yes,scrollbars=yes")) 
		this.forward(c);
	},
	douban : function (){
		var c = "http://www.douban.com/recommend/?url=" + encodeURIComponent(location.href) + "&title=" + encodeURIComponent(topictitle);
		if (!window.open(c, "douban", "toolbar=0,resizable=1,scrollbars=yes,status=1,width=450,height=330")) 
		//(location.href = c) || (window.location = c);
		this.forward(c);
	},
	sohu : function ()
	{
		var c = "http://bai.sohu.com/share/blank/addbutton.do?link=" + location.href + "&title=" + topictitle;
	    if (!window.open(c, "sohu", "location = yes,menubar=yes,resizable=1,scrollbars=yes,status=1,titlebar=yes,toolbar=yes,scrollbars=yes")) 
        this.forward(c);
	},
	sina : function (){
        var c="http://v.t.sina.com.cn/share/share.php?url=" + encodeURIComponent(location.href) + "&title="+encodeURIComponent(topictitle)+"&source="+encodeURIComponent(forumtitle)+"&sourceUrl="+encodeURIComponent(document.domain)+"&content=gb2312";
		var pic=$('firstpost').getElementsByTagName('img');
		if(pic.length!=0){
			for(var j=0; j<pic.length;j++) {
			//pic[j].src.indexOf("http://")!=-1 || 
			  var getpic=is_ie?typeof pic[j].onload!="undefined":pic[j].getAttribute('onload')=='thumbImg(this)'
			  if(pic[j].src.indexOf("attachment.aspx?")!=-1 || getpic)
			  {
			  c+='&pic='+encodeURIComponent(pic[j].src);
			  break
			  }
			}
		}
//		else
//		{
//			  var pics=document.getElementsByTagName('img');
//			  for(var i=0; i<pics.length;i++)
//			  {
//				  if(pics[i].src.indexOf("attachment.aspx?")!=-1)
//				  {
//					  c+='&pic='+encodeURIComponent(pics[i].src);
//					  break;
//				  }
//			  }
//	    }
		if (!window.open(c, "sina", "toolbar=0,resizable=1,scrollbars=yes,status=1,width=626,height=436")) 
		//(location.href = c) || (window.location = c);
		this.forward(c);
	},
	qq : function (){
	//noui 是否开启界面
		var c = "http://shuqian.qq.com/post?title=" + encodeURIComponent(topictitle) + "&uri=" + encodeURIComponent(location.href) + "&jumpback=2&noui=1";
		if (!window.open(c, "qq", "location = yes,menubar=yes,resizable=1,scrollbars=yes,status=1,titlebar=yes,toolbar=yes,scrollbars=yes")) 
		this.forward(c);
	},
    google: function(){
		var c="http://www.google.com/bookmarks/mark?op=add&bkmk=" + encodeURIComponent(location.href) + "&title=" + encodeURIComponent(topictitle);
	    if (!window.open(c, "google", "location = yes,menubar=yes,resizable=1,scrollbars=yes,status=1,titlebar=yes,toolbar=yes,scrollbars=yes")) 
		this.forward(c);
	},
    vivi :function(){
		var c="http://vivi.sina.com.cn/collect/icollect.php?title=" + encodeURIComponent(topictitle) + "&url=" + encodeURIComponent(location.href);
		if (!window.open(c, "vivi", "location = yes,menubar=yes,resizable=1,scrollbars=yes,status=1,titlebar=yes,toolbar=yes,scrollbars=yes")) 
		this.forward(c);
	},
    live:function(){
		var c="https://skydrive.live.com/sharefavorite.aspx/.SharedFavorites??url=&title=" + encodeURIComponent(topictitle) + "&jump=1&wa=wsignin1.0"
		if (!window.open(c, "live", "location = yes,menubar=yes,resizable=1,scrollbars=yes,status=1,titlebar=yes,toolbar=yes,scrollbars=yes")) 
		this.forward(c);
	},
	baidu : function(){
	    var c="http://cang.baidu.com/do/add?it=" + encodeURIComponent(topictitle) + "&iu=" + encodeURIComponent(location.href) + "&fr=ien#nw=1";
		if (!window.open(c, "baidu", "scrollbars=no,width=600,height=450,left=75,top=20,status=no,resizable=yes")) 
		this.forward(c);
	},
    favorite:function(){
		if (window.sidebar) { 
			window.sidebar.addPanel(topictitle,location.href,""); 
		} else if(document.all) {
			window.external.AddFavorite(location.href,topictitle);
		} else if(window.opera && window.print) {
			return true;
		}
	}	
}