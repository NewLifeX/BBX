var items;
var adNum=0;
var theTimer;
var bannerPhoto=new Array();
var bannerPhotolink=new Array();

function shownavbar(albumid, curphotoid){
	var b = bindPhotoNav;
	var c="tools/ajax.ashx?t=album&albumid=" + albumid;
	var f = "cache/album/" + (Math.floor(albumid / 1000) + 1) + "/" + albumid + "_json.txt";
	_sendRequest(f,function(d){var e={};try{e=eval("("+d+")")}catch(f){e={};
	
	_sendRequest(c,function(d){var e={};try{e=eval("("+d+")")}catch(f){e={}}var h=e?e:null;b(h, curphotoid);e=null;b=null},false,true);
	return;
	
	}var h=e?e:null;b(h, curphotoid);e=null;b=null},false,null)
}

function showTags(photoid){
	var f = "cache/photo/" + (Math.floor(photoid / 1000) + 1) + "/" + photoid + "_tags.txt";
	var c="tools/ajax.ashx?t=getphototags&photoid=" + photoid;
	_sendRequest(f,function(d){var e={};try{e=eval("("+d+")")}catch(f){e={};
	_sendRequest(c,function(d){var e={};try{e=eval("("+d+")")}catch(f){e={}}var h=e?e:null;bindTags(h);e=null;bindTags=null},false,true);
	return;
	
	}var h=e?e:null;bindTags(h);e=null;bindTags=null},false,null)
}

function bindTags(obj) {
	var tagitems = obj;
	var tagcontainer = $('tagcontainer');
	var html = "";
	for (var i in tagitems) {
		html += "<li><a href='phototag-" + tagitems[i].tagid + ".aspx'>" + tagitems[i].tagname + "</a></li>";
	}
	tagcontainer.innerHTML = html;
}

function bindPhotoNav(obj, curphotoid){
	items = obj.items;
	var length = items.length;
	var photoindex = 1;
	var photonav = $("photonav");
	if (items.length < 1)
	{
		photonav.innerHTML = "暂时无法显示导航条";
		return;
	}

	var html = "";
	html += "<table cellpadding='1'><tr>";
	for (var i in items)
	{
		var border = "";
		var size = 88;
		if (items[i].photoid == curphotoid)
		{
			photoindex = new Number(i) + 1;
			border = " class=\"activeimg\"";
			size = 76;
		}
		html += "<td><a href='showphoto.aspx?photoid=" + items[i].photoid + "'><div" + border + "><img title='" + items[i].title + "' alt='" + items[i].title + "' src='" + items[i].square + "' onerror=\"this.onerror=null;this.src='templates/" + templatepath + "/images/errorphoto.gif';\" width='" + size + "' height='" + size + "' border='0' /></div></a></td>";
	
	}
	html += "</tr></table>";
	photonav.innerHTML = html;

	_attachEvent(window, "load", function(){
		//scrolling
		if (photoindex > 5) {
			$('photonav').scrollLeft = (photoindex-5) * 90;
		}

		//disable prevImg
		if (photoindex == 1) {
			$("prevImg").style.display = 'none';
			if (1 == length) {
				$("nextImg").style.display = 'none';
				$("nextImgOnPhoto").href = 'showphoto.aspx?photoid=' + items[0].photoid;
			}
			else{
				$("nextImgOnPhoto").href = 'showphoto.aspx?photoid=' + items[photoindex].photoid;
				$("nextImg").href = 'showphoto.aspx?photoid=' + items[photoindex].photoid;
			}
		}

		//disable nextImg
		else if (photoindex == length) {
			$("nextImg").style.display = 'none';
			$("nextImgOnPhoto").href = 'showphoto.aspx?photoid=' + items[0].photoid;
			$("prevImg").href = 'showphoto.aspx?photoid=' + items[photoindex - 2].photoid;
		}
		else {
			$("prevImg").href = 'showphoto.aspx?photoid=' + items[photoindex - 2].photoid;
			$("nextImg").href = 'showphoto.aspx?photoid=' + items[photoindex].photoid;
			$("nextImgOnPhoto").href = 'showphoto.aspx?photoid=' + items[photoindex].photoid;
		}
		$("photoIndex").innerHTML = '<em>' + photoindex + '</em>/' + length;
	});
	
}
function resizePhoto(img){
	if (img.width > 685)
	{
		img.width = 685;
	}
}

function stopPhoto(){
	document.getElementById("stopPhotobutton").style.display="none";
	clearTimeout(theTimer);
}

function pptPhoto(){
   for(var i=0;i<items.length-1;i++)
   {
       bannerPhoto[i]=items[i].square.replace("_square","");
       bannerPhotolink[i]=items[i].square.replace("_square","");
   }

   var preloadedimages=new Array();
   for (i=1;i<bannerPhoto.length;i++){
      preloadedimages[i]=new Image();
      preloadedimages[i].src=bannerPhoto[i];
   }

	jump2url();
}

function setTransition(){
   if (document.all){
      bannerPhotoADrotator.filters.revealTrans.Transition=Math.floor(Math.random()*23);
      bannerPhotoADrotator.filters.revealTrans.apply();
   }
}

function playTransition(){
   if (document.all)
      bannerPhotoADrotator.filters.revealTrans.play()
}

function nextPhoto(){
   if(adNum<bannerPhoto.length-1)adNum++ ;
      else adNum=0;
   setTransition();
   document.images.bannerPhotoADrotator.src=bannerPhoto[adNum];
   playTransition();
   theTimer=setTimeout("nextPhoto()", 3000);
}

function jump2url(){
   document.getElementById("stopPhotobutton").style.display="";

   jumpUrl=bannerPhotolink[adNum];
   
   nextPhoto();

}


