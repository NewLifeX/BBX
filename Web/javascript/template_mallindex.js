var nogoodsinfo = '暂无商品信息!';
var noshopinfo = '暂无店铺信息!';
/*******************************************************加载商品分类信息*********************************************************/

function loadcategory(aspxrewrite) {
   var outputhtml = '';
   count_i = 0;
   for(var i in cats) {
        if(count_i >9){ //获取10个1级分类
		      break;
		}
		
        if(cats[i].layer == 0){
           outputhtml += '<dl><dt>'+cats[i].name+'</dt><dd>';
           count_j = 0;
		   for(var j in cats) {
		       if(count_j > 6){ //提取7个子分类
		            outputhtml += '...';
		            count_j = 0;
		            break;
		       }
		       if(cats[i].id == cats[j].pid){
		          outputhtml += cats[j].name + ' ';
                  count_j++; 
		       }
		   }				
		   outputhtml += '</dd></dl>';

		   count_i++;
		   
		   if(i%2==0 && i>0) {
	         outputhtml +='<div style="clear:both;"></div>';
           } 
       }
   }
   $('categorylist').innerHTML = outputhtml;
}

//加载指定商品分类的子分类信息
function loadsubcategory(categoryid) {
   var outputhtml = '<em>分类:</em>';
   count_i = 0;
   for(var i in cats) {
        if(cats[i].pid == categoryid) {
            if(count_i > 5){ //获取5个1级分类
		        break;
		    }
		    if (aspxrewrite == 1) {
		        outputhtml += '<a href="showgoodslist-'+cats[i].id+'.aspx">'+cats[i].name+'</a>';
		    }
		    else {
                outputhtml += '<a href="showgoodslist.aspx?categoryid='+cats[i].id+'">'+cats[i].name+'</a>';
            }
            count_i++;
		}
   }
   
   if (aspxrewrite == 1) {
       outputhtml += '<a href="showgoodslist-'+categoryid+'.aspx" class="more">更多</a>&gt;&gt;';
   }
   else {
       outputhtml += '<a href="showgoodslist.aspx?categoryid='+categoryid+'" class="more">更多</a>&gt;&gt;';
   }
   $('goodscategory_'+categoryid).innerHTML = outputhtml;
}

//加载商品分类TAB信息
function loadgoodscategorytab(categoryid,tabid) {
    var outputhtml = '';
    count_i = 0;
    for(var i in cats) {
        if(cats[i].pid == categoryid) {
            if(count_i > 4){ //获取5个1级分类
		          break;
		    }
		    
		    if(cats[i].id == tabid || tabid == 0) { //当为选中TAB或为初始值(0)时
		        outputhtml += '<li class="cur">'+cats[i].name+'</li>';
		        ajaxgetfocusgoodlist(categoryid, cats[i].id);
		        tabid = cats[i].id;
		    }
		    else {
		        outputhtml += '<li><a href="javascript:;" onclick="javascript:loadgoodscategorytab('+categoryid+','+cats[i].id+');ajaxgetfocusgoodlist('+categoryid+', '+cats[i].id+');">'+cats[i].name+'</a></li>';
		    }
		}
   }
   $('tab_goodscategory_'+categoryid).innerHTML = outputhtml;
}
   
//设定字符串指定长度   
function cutstring(str,len)
{
    var strlen = 0;
    var s = "";
    for(var i = 0;i < str.length;i++) {
        if(str.charCodeAt(i) > 128) {
            strlen += 2;
        }
        else {
            strlen++;
        }
        s += str.charAt(i);
        if(strlen >= len) {
            return s + "...";
        }
    }
    return s;
}

/*******************************************************ajax加载商品信息*********************************************************/
//获取热门商品信息
function ajaxgethotgoods(days, categoryid, count, div_id) {
    $(div_id).innerHTML = '加载数据中...';
    _sendRequest('tools/ajax.ashx?t=gethotgoods&days=' + days + '&categoryid=' + categoryid + '&count=' + count, function(d){
		try{
		eval('hotgoods_callback(' + d + ',\'' + div_id + '\')');}catch(e){};
	});
}

//设置热门商品显示信息
function hotgoods_callback(data, div_id) {
    if(data.length<=0) {
        $(div_id).innerHTML = nogoodsinfo;
        return;
    }
    
    var goods_html = '';
    
    for(var i in data) {
        goods_html += '<dl><dt>';
        if (data[i].goodspic == '') {
            goods_html += '<img width="49" height="49" src="templates/'+templatepath+'/images/NoPhoto.jpg" onerror="this.onerror=null;this.src=\''+data[i].goodspic+'\';"  title="'+data[i].title+'">';
        }
        else{
            goods_html += '<img width="49" height="49" src="upload/'+data[i].goodspic+'" onerror="this.onerror=null;this.src=\''+data[i].goodspic+'\';"  title="'+data[i].title+'">';
        }
	    goods_html += '</dt><dd class="title"><a href="#">'+data[i].title+'</a></dd>';
	    if (aspxrewrite == 1) {
	        goods_html += '<dd>商家:<a href="userinfo-'+data[i].selleruid+'.aspx">'+data[i].seller+'</a></dd>';
	    }
	    else {
	        goods_html += '<dd>商家:<a href="userinfo.aspx?userid='+data[i].selleruid+'">'+data[i].seller+'</a></dd>';
	    }
	    goods_html += '<dd>价格:<em>'+data[i].price+'</em>元</dd>';
        goods_html += '</dl>';  
    }
    $(div_id).innerHTML = goods_html;
}

//获取指定分类的商品信息
function ajaxgetgoodslist(categoryid) {
    $('goodsinfo_li_' + categoryid).innerHTML = '加载数据中...';
    _sendRequest('tools/ajax.ashx?t=getgoodslist&categoryid=' + categoryid + '&topnumber=12' , function(d){
		try{
		eval('goods_callback(' + d + ',\'goodsinfo_li_' + categoryid + '\')');}catch(e){};
	});
}

//设置指定分类的商品信息
function goods_callback(data, div_id) {
    if(data.length<=0) {
        $(div_id).innerHTML = nogoodsinfo;
        return;
    }
    var goods_html = '';
    
    for(var i in data) {
        goods_html += '<li>';
        if (aspxrewrite == 1) {
	        goods_html += '<a href="showgoods-'+data[i].goodsid+'.aspx">';
	    }
	    else {
	        goods_html += '<a href="showgoods.aspx?goodsid='+data[i].goodsid+'">';
	    }
        
        if (data[i].goodspic == '') {
            goods_html += '<img width="49" height="49" src="templates/'+templatepath+'/images/NoPhoto.jpg" onerror="this.onerror=null;this.src=\''+data[i].goodspic+'\';"  title="'+data[i].title+'"></a>';
        }
        else{
            goods_html += '<img width="49" height="49" src="upload/'+data[i].goodspic+'" onerror="this.onerror=null;this.src=\''+data[i].goodspic+'\';"  title="'+data[i].title+'"></a>';
        }
        if (aspxrewrite == 1) {
	        goods_html += '<h4><a href="showgoods-'+data[i].goodsid+'.aspx">'+cutstring(data[i].title,32)+'</a></h4>';
	    }
	    else {
	        goods_html += '<h4><a href="showgoods.aspx?goodsid='+data[i].goodsid+'">'+cutstring(data[i].title,32)+'</a></h4>';
	    }
	    goods_html += '<p>市场价:<strike>'+data[i].costprice+'</strike>元</p><p class="price">现价:'+data[i].price+'元</p></li>';
    }
    $(div_id).innerHTML = goods_html;
}


//获取人气商品信息
function ajaxgetfocusgoodlist(parentid, categoryid) {
    $('hotgoodsinfo_' + parentid).innerHTML = '加载数据中...';
    _sendRequest('tools/ajax.ashx?t=getgoodslist&order=1&categoryid=' + categoryid + '&topnumber=9' , function(d){
	    try{
	    eval('focusgoodslist_callback(' + d + ',\'hotgoodsinfo_' + parentid + '\')');}catch(e){};
    });
//    $('hotgoodsinfo_' + parentid).innerHTML = '加载数据中...';
//    _sendRequest('tools/ajax.ashx?t=gethotgoods&days=' + 365 + '&categoryid=' + categoryid + '&count=9', function(d){
//		try{
//		eval('focusgoodslist_callback(' + d + ',\'hotgoodsinfo_' + parentid + '\')');}catch(e){};
//	});
}

//设置人气商品显示信息
function focusgoodslist_callback(data, div_id) {
    if(data.length<=0) {
        $(div_id).innerHTML = nogoodsinfo;
        return;
    }
    var goods_html = '';
    
    for(var i in data) {
        goods_html += '<li><cite>'+(parseInt(i)+1)+'</cite>';
        if (aspxrewrite == 1) {
	        goods_html += '<a href="showgoods-'+data[i].goodsid+'.aspx">';
	    }
	    else {
	        goods_html += '<a href="showgoods.aspx?goodsid='+data[i].goodsid+'">';
	    }
	    goods_html += cutstring(data[i].title,25) + '</a></li>';
    }
    $(div_id).innerHTML = goods_html;
}
/*******************************************************ajax加载店铺信息*********************************************************/

function ajaxgetshop(shoptype, div_id) {
    $(div_id).innerHTML = '加载数据中...';
    var shop_type = shoptype=='hotshop' ? 1 : 2;
    _sendRequest('tools/ajax.ashx?t=getshopinfo&shoptype=' +  shop_type, function(d){
		try{
		eval('getshops_callback(' + d + ',\'' + div_id + '\')');}catch(e){};
	});
}

function getshops_callback(data, div_id) {
    if(data.length<=0) {
        $(div_id).innerHTML = noshopinfo;
        return;
    }
    var shops_html = '';
    
    for(var i in data) {
        shops_html += '<dl><dt>';
        if (data[i].logo == '') {
            shops_html += '<img width="49" height="49" src="templates/'+templatepath+'/images/NoPhoto.jpg" onerror="this.onerror=null;this.src=\''+data[i].logo+'\';"  title="'+data[i].title+'">';
        }
        else{
            shops_html += '<img width="49" height="49" src="upload/'+data[i].logo+'" onerror="this.onerror=null;this.src=\''+data[i].logo+'\';"  title="'+data[i].title+'">';
        }
        shops_html += '</dt>';
		shops_html += '<dd class="title"><a href="shop.aspx?shopid='+data[i].shopid+'">'+data[i].shopname+'</a></dd>';
		if (aspxrewrite == 1) {
		    shops_html += '<dd>商家:<a href="userinfo-'+data[i].uid+'.aspx">'+data[i].username+'</a></dd>';
		}
		else {
	        shops_html += '<dd>商家:<a href="userinfo.aspx?userid='+data[i].uid+'">'+data[i].username+'</a></dd>';
	    }
	    shops_html += '<dd><a href="shop.aspx?shopid='+data[i].shopid+'">进去逛逛:)</a></dd>';  
        shops_html += '</dl>';  
    }
    $(div_id).innerHTML = shops_html;
}

function tabselect(objid, tabname){
    if(objid == 'hotgoods_day_h2') {
        if (tabname == 'onemonth') {
            $(objid).innerHTML = '<strong>本月热销</strong><a href="javascript:;" onclick="javascript:tabselect(\''+objid+'\',\'oneweek\');ajaxgethotgoods(7,0,4,\'hotgoods_days\');">本周热销</a>';
        }
        else {
            $(objid).innerHTML = '<a href="javascript:;" onclick="javascript:tabselect(\''+objid+'\',\'onemonth\');ajaxgethotgoods(30,0,4,\'hotgoods_days\');">本月热销</a><strong>本周热销</strong>';
        }
    }
    
    if(objid == 'hotshop_h2') {
       if (tabname == 'hotshop') {
            $(objid).innerHTML = '<strong>热门店铺</strong><a href="javascript:;" onclick="javascript:tabselect(\''+objid+'\',\'newshop\');ajaxgetshop(\'newshop\',\'hotshop_div\');">新开店铺</a>';
        }
        else {
            $(objid).innerHTML = '<a href="javascript:;" onclick="javascript:tabselect(\''+objid+'\',\'hotshop\');ajaxgetshop(\'hotshop\',\'hotshop_div\');">热门店铺</a><strong>新开店铺</strong>';
        } 
    }
}


function init() {
    ajaxgethotgoods(30,0,3,'hotgoods_days');
    ajaxgetshop('hotshop','hotshop_div');
}

