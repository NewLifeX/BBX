<%@ Page language="c#" Inherits="BBX.Web.Admin.auditingtopic" Codebehind="forum_auditingtopic.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="AjaxPostInfo" Src="../UserControls/AjaxPostInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>AuditingTopic</title>	
<link href="../styles/calendar.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body >
<form id="Form1" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/icon19.jpg) no-repeat 6px 50%;">搜索帖子</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">所在论坛</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:dropdowntreelist id="forumid" runat="server"></cc1:dropdowntreelist>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">原帖作者</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="poster" runat="server" Width="150px" RequiredFieldType="暂无校验"></cc1:TextBox>
		</td>
		<td class="vtop">多个用户名之间请用半角逗号 "," 分割</td>
	</tr>
	<tr><td class="item_title" colspan="2">标题关键字</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="title" runat="server" Width="150px" RequiredFieldType="暂无校验"></cc1:TextBox>
		</td>
		<td class="vtop">多关键字之间请用半角逗号 "," 分割</td>
	</tr>
	<tr><td class="item_title" colspan="2">删帖管理员</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="moderatorname" runat="server" Width="150px" RequiredFieldType="暂无校验"></cc1:TextBox>
		</td>
		<td class="vtop">多关键字之间请用半角逗号 "," 分割</td>
	</tr>
	<tr><td class="item_title" colspan="2">帖子发表时间范围</td></tr>
	<tr>
		<td class="vtop rowform">
			起始日期:<cc1:Calendar id="postdatetimeStart" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js"></cc1:Calendar><br />
			结束日期:<cc1:Calendar ID="postdatetimeEnd" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js" />
		</td>
		<td class="vtop">格式 yyyy-mm-dd, 不限制请留空</td>
	</tr>
	<tr><td class="item_title" colspan="2">删帖时间范围</td></tr>
	<tr>
		<td class="vtop rowform">
			起始日期:<cc1:Calendar id="deldatetimeStart" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js"></cc1:Calendar><br />
			结束日期:<cc1:Calendar id="deldatetimeEnd" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js"></cc1:Calendar>
		</td>
		<td class="vtop">格式 yyyy-mm-dd, 不限制请留空</td>
	</tr>
</table>
<div class="Navbutton"><cc1:Button id="SearchTopicAudit" runat="server" Text=" 搜索符合条件的被删帖子 "></cc1:Button></div>
</fieldset>
</div>
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/icon46.jpg) no-repeat 6px 50%;">清除回帖站</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">清空多少天以前的回收站主题</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox ID="RecycleDay" runat="server" RequiredFieldType="数据校验" Text="30" Size="5"/>天前
		</td>
		<td class="vtop">0 为清空全部</td>
	</tr>
</table>
<div class="Navbutton"><cc1:Button id="DeleteRecycle" runat="server" Text=" 批量清空回收站 " OnClick="DeleteRecycle_Click" OnClientClick="if(!confirm('你确认要删除指定天数的帖子吗？')) return false;"></cc1:Button></div>
</fieldset>
</div>	
<uc1:AjaxPostInfo id="AjaxPostInfo1" runat="server"></uc1:AjaxPostInfo>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint> 
</form>		
<%=footer%>
</body>
</html>
