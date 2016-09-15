<%@ Page Language="c#" Inherits="BBX.Web.Admin.cachemanage" Codebehind="global_cachemanage.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>缓存管理</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">	       
	var result = 1; 
	var bar = 0;    
	function runstatic()
	{  
	  if(result > -1)
	  {
		 document.getElementById('Layer5').innerHTML =  '<BR /><table><tr><td valign=top><img border=\"0\" src=\"../images/ajax_loading.gif\"  /></td><td valign=middle style=\"font-size: 14px;\" >'+getcachename()+', 请稍等...<BR /></td></tr></table><BR />';
		 document.getElementById('Layer5').style.witdh='350';
		 document.getElementById('success').style.witdh='400';
		 document.getElementById('success').style.display ="block"; 
		 result=getReturn('global_refreshallcache.aspx?opnumber='+result);
		 if(result==null)
		 {
			document.getElementById('Layer5').innerHTML="<br />提交成功, 请稍等...";
			document.getElementById('success').style.display = "block";
			count(); 
			document.getElementById('Form1').submit();
		 }
	  }
	  else
	  {
		 document.getElementById('Layer5').innerHTML="<br />提交成功, 请稍等...";
		 document.getElementById('success').style.display = "block";
		 count(); 
		 document.getElementById('Form1').submit();
	  }
   }
   
	function getcachename()
	{
	   var cachename='';
	   switch(result)
	   {
			case '1': cachename='正在重设管理组信息';break;
			case '2': cachename='正在重设用户组信息';break;
			case '3': cachename='正在重设版主信息';break;
			case '4': cachename='正在重设指定时间内的公告列表';break;
			case '5': cachename='正在重设第一条公告';break;
			case '6': cachename='正在重设版块下拉列表';break;
			case '7': cachename='正在重设表情';break;
			case '8': cachename='正在重设主题图标';break;
			case '9': cachename='正在重设自定义标签';break;
			case '10': cachename='正在重设论坛基本设置';break;
			case '11': cachename='正在重设论坛积分';break;
			case '12': cachename='正在重设地址对照表';break;
			case '13': cachename='正在重设论坛统计信息';break;
			case '14': cachename='正在重设系统允许的附件类型和大小';break;
			case '15': cachename='正在重设模板列表的下拉框html';break;
			case '16': cachename='正在重设在线用户列表图例';break;
			case '17': cachename='正在重设友情链接列表';break;
			case '18': cachename='正在重设脏字过滤列表';break;
			case '19': cachename='正在重设论坛列表';break;
			
			/*case '20': cachename='正在重设指定版块RSS';break;
			case '21': cachename='正在重设论坛整体RSS';break;
			case '22': cachename='正在重设模板列表';break;
			case '23': cachename='正在重设用户表扩展字段';break;
			case '24': cachename='正在重设勋章列表';break;
			case '25': cachename='正在重设数据链接串和表前缀';break;
			case '26': cachename='正在重设最后帖子表';break;
			case '27': cachename='正在重设帖子列表';break;
			case '28': cachename='正在重设广告列表';break;
			case '29': cachename='正在重设用户上一次执行搜索操作时间';break;
			case '30': cachename='正在重设用户一分钟内搜索次数';break;
			case '31': cachename='正在重设用户头象列表';break;
			case '32': cachename='正在重设干扰码字符串';break;
			case '33': cachename='正在重设魔力列表';break;
			case '34': cachename='正在重设兑换比率可交易积分策略';break;
			case '35': cachename='正在重设当前帖子表相关信息';break;
			case '36': cachename='正在重设全部版块精华主题列表';break;
			case '37': cachename='正在重设全部版块热帖主题列表';break;
			case '38': cachename='正在重设最近主题列表';break;
			case '40': cachename='正在重设在线表';break;
			case '41': cachename='正在重设导航弹出菜单';break;
			default : cachename='正在重设系统缓存';break; */
			
			case '20': cachename='正在重设在线用户信息';break;
			case '21': cachename='正在重设论坛整体RSS及指定版块RSS';break;
			case '22': cachename='正在重设论坛整体RSS';break;
			case '23': cachename='正在重设模板ID列表';break;
			case '24': cachename='正在重设有效用户表扩展字段';break;
			case '25': cachename='正在重设勋章列表';break;
			case '26': cachename='正在重设数据链接串和表前缀';break;
			case '27': cachename='正在重设帖子列表';break;
			case '28': cachename='正在重设最后帖子表';break;
			case '29': cachename='正在重设广告列表';break;
			case '30': cachename='正在重设用户上一次执行搜索操作时间';break;
			case '31': cachename='正在重设用户一分钟内搜索次数';break;
			case '32': cachename='正在重设用户头象列表';break;
			case '33': cachename='正在重设干扰码字符串';break;
			case '34': cachename='正在重设魔力列表';break;
			case '35': cachename='正在重设兑换比率可交易积分策略';break;
			case '36': cachename='正在重设当前帖子表相关信息';break;
			case '37': cachename='正在重设全部版块精华主题列表';break;
			case '38': cachename='正在重设全部版块热帖主题列表';break;
			case '39': cachename='正在重设最近主题列表';break;
			case '40': cachename='正在重设BaseConfig';break;
			case '41': cachename='正在重设在线用户表';break;
			case '42': cachename='正在重设导航弹出菜单';break;                
			default : cachename='正在重设系统缓存';break; 

	   }
	   return cachename;//+' ,请稍等...';
   }
	 
   function clearflag()
   {
		bar=0;
		document.getElementById('Layer5').innerHTML="<br />提交成功, 请稍等...";
		document.getElementById('success').style.display = "block";
		count(); 
   }
	
   function count()
   { 
		bar=bar+2;
		if (bar<99) {setTimeout("count()",100);} 
		else { document.getElementById('success').style.display ="none"; } 
   }
   
   function run()
   {
	  bar=0;
	  document.getElementById('Layer5').innerHTML="<br />正在处理数据, 请稍等...";
	  document.getElementById('success').style.display = "block";
	  setInterval('runstatic()',2000); //每次提交时间为2秒
   }
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<table cellspacing="0" cellpadding="4" width="100%" align="center">
<tr>
	<td  class="panelbox" width="50%" align="left">
		<table width="100%">
			<tr>
				<td>
					<button type="button" class="ManagerButton" onclick="javascript:run();"><img src="../images/submit.gif" />更新所有缓存</button>
					<cc1:Button ID="ResetAllCache" runat="server" Text="<B>更新所有缓存</B>" ButtonImgUrl="../images/cache_resetall.gif" Visible="false" />
				</td>
			</tr>
			<tr>                    
				<td><cc1:Button ID="ResetMGinf" runat="server" Text="重新设置管理组信息" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetForumInf" runat="server" Text="重新设置版主信息" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>                    
				<td><cc1:Button ID="ResetFirstAnnounce" runat="server" Text="重新设置第一条公告" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetSmiles" runat="server" Text="重新设置表情" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetThemeIcon" runat="server" Text="重新设置主题图标" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ReSetDigestTopicList" runat="server" Text="重新设置版块精华主题列表" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetForumsStaticInf" runat="server" Text="重新设置论坛统计信息" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetTemplateDropDown" runat="server" Text="重新设置模板列表的下拉框html" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetLink" runat="server" Text="重新设置友情链接列表" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetForumList" runat="server" Text="重新设置论坛列表" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetRssAll" runat="server" Text="重新设置论坛整体RSS" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetValidUserExtField" runat="server" Text="重新设置有效的用户表扩展字段" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetMedalList" runat="server" Text="重新设置勋章列表" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ReSetStatisticsSearchtime" runat="server" Text="重新设置用户上一次执行搜索操作的时间" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ReSetCommonAvatarList" runat="server" Text="重新设置用户头象列表" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ReSetMagicList" runat="server" Text="重新设置魔力列表" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ReSetScoreset" runat="server" Text="重新设置论坛积分设置" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>                   
			<tr>
				<td><cc1:Button ID="ReSetAlbumCategory" runat="server" Text="重新设置相册分类" ButtonImgUrl="../images/cache_reset.gif" OnClick="ReSetAlbumCategory_Click" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ReSetNavPopupMenu" runat="server" Text="重设导航弹出菜单" ButtonImgUrl="../images/cache_reset.gif" OnClick="ReSetNavPopupMenu_Click" /></td>
			</tr>
		</table>
	</td>
	<td  class="panelbox" width="50%" align="right">
		<table width="100%">                    
			<tr>
				<td><cc1:Button ID="ReSetPostTableInfo" runat="server" Text="重新设置当前帖子表相关信息" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetUGinf" runat="server" Text="重新设置用户组信息" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetAnnonceList" runat="server" Text="重新设置指定时间内的公告列表" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetForumDropList" runat="server" Text="重新设置版块下拉列表" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetAddressRefer" runat="server" Text="重新设置地址对照表" DESIGNTIMEDRAGDROP="54" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetFlag" runat="server" Text="重新设置自定义标签" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ReSetHotTopicList" runat="server" Text="重新设置版块热帖主题列表" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetAttachSize" runat="server" Text="重新设置系统允许的附件类型和大小" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetOnlineInco" runat="server" Text="重新设置在线用户列表图例" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetWord" runat="server" Text="重新设置脏字过滤列表" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetRss" runat="server" Text="重新设置论坛RSS" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetTemplateIDList" runat="server" Text="重新设置论坛模板id列表" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ResetOnlineUserInfo" runat="server" Text="重新设置在线用户信息" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ReSetAdsList" runat="server" Text="重新广告列表" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ReSetStatisticsSearchcount" runat="server" Text="重新设置用户在一分钟内搜索的次数" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ReSetJammer" runat="server" Text="重新设置干扰码字符串" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ReSetScorePaySet" runat="server" Text="重新设置兑换比率的可交易积分策略" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ReSetAggregation" runat="server" Text="重新设置聚合" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
			<tr>
				<td><cc1:Button ID="ReSetTag" runat="server" Text="重新设置标签(Tag)" ButtonImgUrl="../images/cache_reset.gif" /></td>
			</tr>
		</table>
	</td>
</tr>
<tr>
	<td colspan="2" class="panelbox">
		&nbsp;&nbsp;<cc1:Button ID="ResetRssByFid" runat="server" Text="重新设置指定版块RSS" ButtonImgUrl="../images/cache_reset.gif" />
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 请指定版块ID(如rss-2.aspx,ID 为 2)
		<cc1:TextBox ID="txtRssfid" runat="server" Width="40px" RequiredFieldType="数据校验" MaxLength="4"></cc1:TextBox><br /><br />
	</td>
</tr>
<tr>
	<td colspan="2" class="panelbox">
		&nbsp;&nbsp;<cc1:Button ID="ReSetTopiclistByFid" runat="server" Text="重新设置相应的主题列表" ButtonImgUrl="../images/cache_reset.gif" />
		&nbsp;&nbsp;请指定版块ID(如showforum-1.aspx,ID 为 1)
		<cc1:TextBox ID="txtTopiclistFid" runat="server" Width="40px" RequiredFieldType="数据校验" MaxLength="4"></cc1:TextBox>
	</td>
</tr>
<tr>
	<td><cc1:Button ID="ResetForumBaseSet" runat="server" Text="重新设置论坛基本设置" ButtonImgUrl="../images/cache_reset.gif" Visible="false" /></td>
</tr>        
<tr>
	<td><cc1:Button ID="ReSetRecentTopicList" runat="server" Text="重新设置最近主题列表" ButtonImgUrl="../images/cache_reset.gif" Visible="false" /></td>
</tr> 
</table>
</form>
</div>
<%=footer%>
</body>
</html>