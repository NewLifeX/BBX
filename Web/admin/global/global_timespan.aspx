<%@ Page language="c#" Inherits="BBX.Web.Admin.timespan" Codebehind="global_timespan.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
	<title>baseset</title>
	<script type="text/javascript" src="../js/common.js"></script>
	<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
	<script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<fieldset>
	<legend style="background:url(../images/icons/icon2.jpg) no-repeat 6px 50%;">时间段设置</legend>
	<table width="100%">
	<tr><td class="item_title" colspan="2">禁止访问时间段</td></tr>
	<tr>
		<td class="vtop rowform">
			<uc1:TextareaResize id="visitbanperiods" runat="server" HintShowType="down" cols="35" controlname="visitbanperiods" HintHeight="0"></uc1:TextareaResize>
		</td>
		<td class="vtop">每天该时间段内用户不能访问论坛, 请使用 24 小时时段格式, 每个时间段一行, 如需要也可跨越零点, 留空为不限制. 例如:每日晚 11:25 到次日早 5:05 可设置为: 23:25-5:05, 每日早 9:00 到当日下午 2:30 可设置为: 9:00-14:30.注意: 格式不正确将可能导致意想不到的问题. 所有时间段设置均以论坛系统默认时区为准, 不受用户自定义时区的影响</td>
	</tr>
	<tr><td class="item_title" colspan="2">发帖审核时间段</td></tr>
	<tr>
		<td class="vtop rowform">
			<uc1:TextareaResize id="postmodperiods" runat="server" cols="35"  controlname="postmodperiods"></uc1:TextareaResize>
		</td>
		<td class="vtop">每天该时间段内用户发帖不直接显示, 需经版主或管理员人工审核才能发表, 格式和用法同上</td>
	</tr>
	<tr><td class="item_title" colspan="2">禁止全文搜索时间段</td></tr>
	<tr>
		<td class="vtop rowform">
			<uc1:TextareaResize id="searchbanperiods" runat="server"  cols="35" controlname="searchbanperiods"></uc1:TextareaResize>
		</td>
		<td class="vtop">每天该时间段内用户不能使用全文搜索, 格式和用法同上</td>
	</tr>
	<tr><td class="item_title" colspan="2">禁止发帖时间段</td></tr>
	<tr>
		<td class="vtop rowform">
			 <uc1:TextareaResize id="postbanperiods" runat="server"  cols="35" controlname="postbanperiods"></uc1:TextareaResize>
		</td>
		<td class="vtop">每天该时间段内用户不能发帖, 格式和用法同上</td>
	</tr>
	<tr><td class="item_title" colspan="2">禁止下载附件时间段:</td></tr>
	<tr>
		<td class="vtop rowform">
			  <uc1:TextareaResize id="attachbanperiods" runat="server"  cols="35" controlname="attachbanperiods"></uc1:TextareaResize>
		</td>
		<td class="vtop">每天该时间段内用户不能下载附件, 格式和用法同上</td>
	</tr>
	</table>
	<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
	<div class="Navbutton">
		<cc1:Button id="SaveInfo" runat="server" Text=" 提 交 "></cc1:Button>
	</div>
</fieldset>			
</form>
</div>		
<%=footer%>
</body>
</html>