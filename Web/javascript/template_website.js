function tabselect(obj, forumid) { //显示指定内容(obj)元素
    switch(obj.id) {
       case 'li_hotforum': {   //热门版块
                   $('hotforum').style.display = '';
                   $('bbsmessage').style.display = 'none';
                   $('li_bbsmessage').className = '';
                 //  $('hottags').style.display = 'none';
                   //$('li_hottags').className = '';break ;  
				   break;
             }      
       case 'li_bbsmessage': { //论坛信息
                   $('bbsmessage').style.display = '';
                   $('hotforum').style.display = 'none';
                   $('li_hotforum').className = '';
                  // $('hottags').style.display = 'none';
                  // $('li_hottags').className = '';break ;
				   break;
             }  
       case 'li_hottags': { //论坛信息
                   $('hottags').style.display = '';
                   $('hotforum').style.display = 'none';
                   $('li_hotforum').className = '';
                   $('bbsmessage').style.display = 'none';
                   $('li_bbsmessage').className = '';break ;               
             } 
       case 'li_forum_'+forumid+'_topic': {//最热主题
                   $('forum_'+forumid+'_topic').style.display = '';
                   $('forum_'+forumid+'_reply').style.display = 'none';
                   $('li_forum_'+forumid+'_reply').className = '';
                   $('forum_'+forumid+'_digest').style.display = 'none';
                   $('li_forum_'+forumid+'_digest').className = '';break ;
             }  
       case 'li_forum_'+forumid+'_reply': {//最新回复
                   $('forum_'+forumid+'_reply').style.display = '';
                   $('forum_'+forumid+'_topic').style.display = 'none';
                   $('li_forum_'+forumid+'_topic').className = '';
                   $('forum_'+forumid+'_digest').style.display = 'none';
                   $('li_forum_'+forumid+'_digest').className = '';break ;
             }  
       case 'li_forum_'+forumid+'_digest': {//最新回复
                   $('forum_'+forumid+'_digest').style.display = '';
                   $('forum_'+forumid+'_reply').style.display = 'none';
                   $('li_forum_'+forumid+'_reply').className = '';
                   $('forum_'+forumid+'_topic').style.display = 'none';
                   $('li_forum_'+forumid+'_topic').className = '';break ;
             }  
       case  'li_album': { //热门相册
                   $('albumlist').style.display = '';
                   $('photolist').style.display = 'none';
                   $('li_photo').className = '';break ;
             }         
       case  'li_photo': {  //热门相片
                   $('photolist').style.display = '';
                   $('albumlist').style.display = 'none';
                   $('li_album').className = '';break ;
             }  
       case 'li_spacecomment': { //最新评论
                   $('spacecommentlist').style.display = '';
                   $('spacelist').style.display = 'none';
                   $('li_space').className = '';break ;
             }  
       case  'li_space': {//最新评论
                   $('spacelist').style.display = '';
                   $('spacecommentlist').style.display = 'none';
                   $('li_spacecomment').className = '';break ;
             }  
       case 'li_hot_goods': {//热门商品
                   $('hot_goodslist').style.display = '';
                   $('old_goodslist').style.display = 'none';
                   $('li_old_goods').className = '';break ;
             }  
       case 'li_old_goods': {//二手商品
                   $('old_goodslist').style.display = '';
                   $('hot_goodslist').style.display = 'none';
                   $('li_hot_goods').className = '';break ;
             }  
       case 'li_postcount_month': {
                   $('postcount_month').style.display = '';
                   $('postcount_week').style.display = 'none';
                   $('li_postcount_week').className = '';
                   $('postcount_day').style.display = 'none';
                   $('li_postcount_day').className = '';break ;
             }  
       case 'li_postcount_week': {
                   $('postcount_week').style.display = '';
                   $('postcount_month').style.display = 'none';
                   $('li_postcount_month').className = '';
                   $('postcount_day').style.display = 'none';
                   $('li_postcount_day').className = '';break ;
             }  
       case 'li_postcount_day': {
                   $('postcount_day').style.display = '';
                   $('postcount_month').style.display = 'none';
                   $('li_postcount_month').className = '';
                   $('postcount_week').style.display = 'none';
                   $('li_postcount_week').className = '';break ;
             }  
       default : return ;
    }
    obj.className = 'current'; //更新选定的TAB的样式
}

function convertdate(strdate)
{
	strdate = strdate.replace(/-/ig,'/');
	var d = new Date(strdate);
	var now = new Date();
	var result;

	if (d.getYear() == now.getYear() && d.getMonth() == now.getMonth())
	{
		var xday = now.getDate() - d.getDate();

		switch (xday)
		{
			case 0:
				result = "今天 " + d.getHours() + " : " + d.getMinutes();
				break;
			case 1:
				result = "昨天 " + d.getHours() + " : " + d.getMinutes();
				break;
			case 2:
				result = "前天 " + d.getHours() + " : " + d.getMinutes();
				break;
			default:
				result = d.format("MM.dd hh:mm");
				break;		
		}
	}
	else
	{
		result = d.format("MM.dd hh:mm");
	}
	
	return result;
}

function showtopicinfo(forumid, index) {
   var no_pic_path = 'templates/' + templatepath +'/images/gather/slide_pic.jpg';
   if(reco_topic.length == 0 )
   {
            return '<a href="" target="_blank"><img src="' + no_pic_path + '"/></a>';
	   }
   
   for(var i in reco_topic) {
       if (reco_topic[i].fid == forumid || index == i) { 
            return '<a href="'+ (aspxrewrite == 1 ? 'showtopic-'+reco_topic[i].tid+ '.aspx':'showtopic.aspx?topicid='+reco_topic[i].tid) +'" target="_blank"><img onload="imgautosize(this, 280,150);" src="' + (reco_topic[i].img.indexOf('http')>=0 ? reco_topic[i].img : 'upload/' + reco_topic[i].img) + '" alt="' + reco_topic[i].title + '" onerror="this.onerror=null;this.src=\'' + no_pic_path + '\';"/><h4>'+ reco_topic[i].title +'</h4></a>';
       }
   }
   return '';
   //return '<img width="237" height="130" src="'+no_pic_path+'" alt="暂无推荐"/><h4>暂无推荐</h4>';
   
}

/*
方法名称: imgautosize 
方法说明: 按高/宽(宽/高)比例显示图片方法
参数说明:
          imgobj : img 元素对象
          maxwidth 设置图片宽度界限
          maxheight 设置图片高度界限
*/
function imgautosize(imgobj, maxwidth, maxheight)
{
    var heightwidthrate = imgobj.offsetHeight / imgobj.offsetWidth;//设置高宽比
    var widthheightrate = imgobj.offsetWidth / imgobj.offsetHeight;//设置宽高比
   
    if(is_ie && imgobj.readyState != 'complete') {  //确保图片完全加载
        //alert(imgobj.offsetHeight+ ' '+imgobj.fileSize);
        return false;
    }
        
    if(imgobj.offsetHeight > maxheight){
        imgobj.height = maxheight;
        imgobj.width = maxheight * widthheightrate;
    }
    if(imgobj.offsetWidth > maxwidth){
        imgobj.width = maxwidth;
        imgobj.height = maxwidth * heightwidthrate;
    }
}
