window.onload = getMsg;
window.onresize = resizeDiv;

//短信提示使用
var divTop,divLeft,divWidth,divHeight,docHeight,docWidth,objTimer,i = 0;
function getMsg()
{
	var media_ad = document.getElementById("silverlightmediaad");
	try{
		divTop = parseInt(media_ad.style.top,10);
		divLeft = parseInt(media_ad.style.left,10);
		divHeight = parseInt(media_ad.offsetHeight,10);
		divWidth = parseInt(media_ad.offsetWidth,10);
		docWidth = document.body.clientWidth;
		docHeight = document.body.clientHeight;
		media_ad.style.top = parseInt(document.body.scrollTop,10) + docHeight + 10 + "px";//  divHeight
		media_ad.style.left = parseInt(document.body.scrollLeft,10) + docWidth - divWidth + "px";
		media_ad.style.visibility = "visible";
		objTimer = window.setInterval("moveDiv()",10)
	}
	catch(e){}
}

function resizeDiv()
{
	var media_ad = document.getElementById("silverlightmediaad");

	i+=1;
	//if(i > 1000) closeDiv() //想不用自动消失由用户来自己关闭，可以屏蔽这句，也可以调整数字来实现停留时间，目前是10秒
	try{
		divHeight = parseInt(media_ad.offsetHeight,10);
		divWidth = parseInt(media_ad.offsetWidth,10);
		docWidth = document.body.clientWidth;
		docHeight = document.body.clientHeight;
		media_ad.style.top = docHeight - divHeight + parseInt(document.body.scrollTop,10) + "px";
		media_ad.style.left = docWidth - divWidth + parseInt(document.body.scrollLeft,10) + "px";
	}
	catch(e){}
}

function moveDiv()
{
	var media_ad = document.getElementById("silverlightmediaad");
	try
	{
		if(parseInt(media_ad.style.top,10) <= (docHeight - divHeight + parseInt(document.body.scrollTop,10)))
		{
			window.clearInterval(objTimer);
			objTimer = window.setInterval("resizeDiv()",1)
		}
		divTop = parseInt(media_ad.style.top,10);
		media_ad.style.top = divTop - 10 + 'px';
	}
	catch(e){}
}
function closeDiv()
{
	document.getElementById('silverlightmediaad').style.visibility='hidden';
	if(objTimer) 
		window.clearInterval(objTimer)
}


function printMediaAD(pagename, forumid)
{
	var html = '<div id="silverlightmediaad" style="overflow:hidden; zoom:1; BORDER-RIGHT: 1px solid #455690; BORDER-TOP: 1px solid #a6b4cf; Z-INDEX:99999; LEFT: 0px; VISIBILITY: hidden; BORDER-LEFT: 1px solid #a6b4cf; WIDTH: 580px; BORDER-BOTTOM: 1px solid #455690; POSITION: absolute; TOP: 0px; BACKGROUND-COLOR: #c9d3f3">';

	html += '		<table style="BORDER-TOP: #ffffff 1px solid; BORDER-LEFT: #ffffff 1px solid" cellSpacing="0" cellPadding="0" width="100%" bgColor="#AFDCF3" border="0">';
	html += '			<tbody>';
	html += '				<tr bgColor="#6699cc">';
	html += '					<td style="font-size: 12px; color: #0f2c8c" width="30" height="24"></td>';
	html += '					<td style="font-weight: normal; font-size: 12px; color: #ffffff; padding-left: 4px; padding-top: 4px" vAlign="center" width="100%">广告：</td>';
	html += '					<td style="padding-right: 2px; padding-top: 2px" vAlign="center" align="right" width="19">';
	html += '						<font color="#FFFFFF">';
	html += '						<span title="关闭" style="CURSOR: hand;font-size:12px;font-weight:bold;margin-right:4px" onclick="closeDiv()" >×</span></font>';
	html += '					</td>';
	html += '				</tr>';
	html += '				<tr>';
	html += '					<td style="padding:5px; padding-right:8px;" colSpan="3">';
	html += '						<div style="BORDER-RIGHT: #b9c9ef 1px solid; BORDER-TOP: #728eb8 1px solid; FONT-SIZE: 12px; BORDER-LEFT: #728eb8 1px solid; WIDTH: 100%; COLOR: #1f336b; BORDER-BOTTOM: #b9c9ef 1px solid;">';
	html += '							<iframe src="silverlight/ad.htm?forumid=' + forumid + '&pagename=' + pagename + '" width=100% height=320 border="0" scrolling="no" marginheight="0" marginwidth="0" frameborder="0" ></iframe>';
	html += '						</div>';
	html += '					</td>';
	html += '				</tr>';
	html += '			</tbody>';
	html += '		</table>';
	html += '	</div>';

	document.write(html);
}
