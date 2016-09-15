//加载当前用户的评价数据
function loadratedata(startpos,endpos, div_list, span_goodrate)
{
    var goodrate = 0; //好评数
    var sosorate = 0; //中评数
    var badrate = 0; //差评数
    var oneweekcount = 0;//最近一周评价数
    var onemonthcount = 0;//最近一个月评价数
    var sixmonthweekcount = 0;//最近六个月评价数
    var sixmonthagocount = 0;//6个月前评价数
    var ratecount = 0; //评价总数
    var rate_html = '<table cellspacing="0" cellpadding="5"><thead><tr><td>&nbsp;</td><td>最近1周</td><td>最近1个月</td><td>最近6个月</td><td>6个月前</td><td>总计</td></tr></thead><tbody>';
   
    for(i = startpos; i<endpos ;i++) {		
        oneweekcount += usercredit_data[i].oneweek;
        onemonthcount += usercredit_data[i].onemonth;
        sixmonthweekcount += usercredit_data[i].sixmonth;
        sixmonthagocount += usercredit_data[i].sixmonthago;
    
        rate_html += '<tr>';			
        curentratecount = usercredit_data[i].sixmonth +usercredit_data[i].sixmonthago;
        if(usercredit_data[i].ratetype == 1) {
		    rate_html +='<td><img src="templates/' + templatepath + '/images/good.gif" border="0" width="14" height="16" /><font color="red">好评</font></td>';
		    goodrate = curentratecount;
	    }else if(usercredit_data[i].ratetype == 2) {
	        rate_html +='<td><img src="templates/' + templatepath + '/images/soso.gif" border="0" width="14" height="16" /><font color="green">中评</font></td>';
	        sosorate = curentratecount;
	    }else if(usercredit_data[i].ratetype == 3) {
	        rate_html +='<td><img src="templates/' + templatepath + '/images/bad.gif" border="0" width="14" height="16" /><font color="black">差评</black></td>';
	        badrate = curentratecount;
	    }	
	    rate_html +='<td><a href="#" onclick="ajaxgetrate(' + uid + ',' +usercredit_data[i].ratefrom + ', ' + usercredit_data[i].ratetype + ',\'oneweek\');">' + usercredit_data[i].oneweek + '</a></td>';
	    rate_html +='<td><a href="#" onclick="ajaxgetrate(' + uid + ',' +usercredit_data[i].ratefrom + ', ' + usercredit_data[i].ratetype + ',\'onemonth\');">' + usercredit_data[i].onemonth + '</a></td>';
	    rate_html +='<td><a href="#" onclick="ajaxgetrate(' + uid + ',' +usercredit_data[i].ratefrom + ', ' + usercredit_data[i].ratetype + ',\'sixmonth\');">' + usercredit_data[i].sixmonth + '</a></td>';
	    rate_html +='<td><a href="#" onclick="ajaxgetrate(' + uid + ',' +usercredit_data[i].ratefrom + ', ' + usercredit_data[i].ratetype + ',\'sixmonthago\');">' + usercredit_data[i].sixmonthago + '</a></td>';
	    rate_html +='<td>' + curentratecount + '</td>';
	    rate_html +='</tr>';
    }				
    rate_html +='<tr><td>总计</td>';
    
    var ratefrom = 1; //来自卖家
    if(startpos == 0) {
        ratefrom = 2; //来自买家
    }
    rate_html +='<td><a href="#" onclick="ajaxgetrate(' + uid + ', ' + ratefrom + ', 0, \'oneweek\');">' + oneweekcount + '</a></td>';
    rate_html +='<td><a href="#" onclick="ajaxgetrate(' + uid + ', ' + ratefrom + ', 0, \'onemonth\');">' + onemonthcount + '</a></td>';
    rate_html +='<td><a href="#" onclick="ajaxgetrate(' + uid + ', ' + ratefrom + ', 0, \'sixmonth\');">' + sixmonthweekcount + '</a></td>';
    rate_html +='<td><a href="#" onclick="ajaxgetrate(' + uid + ', ' + ratefrom + ', 0, \'sixmonthago\');">' + sixmonthagocount + '</a></td>';

    ratecount = sixmonthweekcount + sixmonthagocount;
    rate_html +='<td>'+ ratecount +'</td>';
    rate_html +='</tr></tbody></table>';
    
    $(div_list).innerHTML = rate_html;

    if(ratecount > 0) { 
        $(span_goodrate).innerHTML = parseFloat((goodrate / ratecount) * 100).toFixed(2) + '%';
    }
    else {
        $(span_goodrate).innerHTML = '0.00%';
    }
}					

   
function gettradecredit(goodsratenum, isseller, span_credit) {
    var raterank = '';
    for(var i in creditrulesjsondata){
        if(creditrulesjsondata[i].lowerlimit <= goodsratenum && creditrulesjsondata[i].upperlimit > goodsratenum) {
            if(isseller) {
                raterank = creditrulesjsondata[i].sellericon;
            }
            else {
                raterank = creditrulesjsondata[i].buyericon;
            }
            break;
        }
    }
    $(span_credit).innerHTML = goodsratenum + (raterank == ''? '': ' <img alt="0" src="templates/' + templatepath + '/images/' + raterank + '" />');
}

        
function ajaxgetrate(uid, uidtype, ratetype, filter, tabname) {
   if(tabname != null && tabname != '') {
       $('fromall').className = '';
       $('fromseller').className = '';
       $('frombuyer').className = '';
       $('toothers').className = '';
       $(tabname).className = 'current';
       $('recentrate').style.display = 'none';
    }
    else {
       var recentrate = '最近';
       switch(filter) {
           case "oneweek" : recentrate += '1周 来自';break;
           case "onemonth" : recentrate += '1个月 来自';break;
           case "sixmonth" : recentrate += '6个月 来自';break;
           case "sixmonthago" : recentrate = '6个月前 来自';break;
       }
       
       if(uidtype == 1) {
          recentrate += '卖家的';
       }
       else {
          recentrate += '买家的';
       }
       
       switch (ratetype) {
           case 1 : recentrate += '好评';break;
           case 2 : recentrate += '中评';break;
           case 3 : recentrate += '差评';break;
           default : recentrate += '评价';break;
       }
    
       $('fromall').className = '';
       $('fromseller').className = '';
       $('frombuyer').className = '';
       $('toothers').className = '';
       $('recentrate').className = 'current';
       $('recentrate').style.display = 'block';
       $('recentrate').innerHTML = recentrate;
    }
    
    $('ratelist_html').innerHTML = '加载数据中...';
    _sendRequest('tools/ajax.ashx?t=ajaxgetgoodsratelist&uid=' + uid + '&uidtype=' + uidtype + '&ratetype=' + ratetype + '&filter=' + filter, function(d){
		try{
		eval('ratelist_callback(' + d + ','+uidtype+')');}catch(e){};
	});
}

function ratelist_callback(data, uidtype) {
   
    var ratelist_html = '<table cellspacing="0" cellpadding="5"><thead><tr><td>&nbsp;</td><td>评价内容</td><td>宝贝名称/评价人</td><td>成交价（元）</td></tr></thead><tbody>';
    if(data.length == 0) {
       ratelist_html += '<tr><td colspan="4" >没有找到相关评价!</td></tr>';
       ratelist_html += '</tbody>';
	   ratelist_html += '</table>';
       $('ratelist_html').innerHTML = ratelist_html;
       return ;
    }
	for(var i in data) {
		ratelist_html += '<tr><td>';
		
		if(data[i].ratetype == 1) {
		    ratelist_html +='<img src="templates/' + templatepath + '/images/good.gif" border="0" width="14" height="16" /><font color="red">好评</font>';
	    }else if(data[i].ratetype == 2) {
	        ratelist_html +='<img src="templates/' + templatepath + '/images/soso.gif" border="0" width="14" height="16" /><font color="green">中评</font>';
	    }else if(data[i].ratetype == 3) {
	        ratelist_html +='<img src="templates/' + templatepath + '/images/bad.gif" border="0" width="14" height="16" /><font color="black">差评</black>';
	    }	
	    
	    ratelist_html += '</td><td><p>' + data[i].message + ' <BR />' + data[i].postdatetime + '</p></td>';//<p>解释: XXXX</p>';
	    ratelist_html += '<td><p><a href="showgoods.aspx?goodsid=' + data[i].goodsid + '">' + data[i].goodstitle + '</a></p>';
	  
	        
	    if(uidtype>=0 && uidtype <3) {
	      
            if(data[i].uidtype == 1) { 
                 ratelist_html += '<p>卖家: ';
            }else{     
                 ratelist_html += '<p>买家: ';
            }
	        if(data[i].uidtype > 0 && data[i].uidtype<3) { //1:卖家 2:买家  3:给他人
	            ratelist_html += '<a href="userinfo.aspx?uid=' + data[i].uid + '">' + data[i].username + '</a></p></td>';
	        }
	        else { //给他人
	            ratelist_html += '<a href="userinfo.aspx?uid=' + data[i].ratetouid + '">' + data[i].ratetousername + '</a></p></td>';
	        }
	    }
	    else {
	        if(uid != data[i].uid) {
	            if(data[i].uidtype == 1) { 
	                 ratelist_html += '<p>卖家: ';
	            }else{     
	                 ratelist_html += '<p>买家: ';
	            }
                 ratelist_html += '<a href="userinfo.aspx?uid=' + data[i].uid + '">' + data[i].username + '</a></p></td>';
	        }
	        else {
	            if(data[i].uidtype == 1) { 
	                 ratelist_html += '<p>买家: ';
	            }else{     
	                 ratelist_html += '<p>卖家: ';
	            }
	            ratelist_html += '<a href="userinfo.aspx?uid=' + data[i].ratetouid + '">' + data[i].ratetousername + '</a></p></td>';
	        }
	    }
	    
	    ratelist_html += '<td>' + data[i].price + '</td>';
    	ratelist_html += '</tr>';
	}
	ratelist_html += '   </tbody>';
	ratelist_html += '	</table>';
	
    $('ratelist_html').innerHTML = ratelist_html;
}



            
                
                    
               
             