<%@ Page language="c#" Inherits="BBX.Web.Admin.searchpost" Codebehind="forum_searchpost.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc4" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>searchpost</title>		
<link href="../styles/calendar.css" type="text/css" rel="stylesheet" />
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
<legend style="background:url(../images/icons/icon19.jpg) no-repeat 6px 50%;">搜索帖子</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">所在论坛</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:dropdowntreelist id="forumid" runat="server"></cc1:dropdowntreelist>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">发表时间范围</td></tr>
	<tr>
		<td class="vtop rowform">
			开始日期:<cc4:Calendar id="postdatetimeStart" runat="server" ReadOnly="True" ScriptPath="../js/calendar.js"></cc4:Calendar><br />
			结束日期:<cc4:Calendar id="postdatetimeEnd" runat="server" ReadOnly="True" ScriptPath="../js/calendar.js"></cc4:Calendar>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">发帖用户名</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="poster" runat="server" RequiredFieldType="暂无校验" width="200"></cc1:TextBox> 
			&nbsp;<input id="lowerupper" type="checkbox" CHECKED value="1" name="lowerupper" runat="server">  不区分大小写
		</td>
		<td class="vtop">多用户名中间请用半角逗号 "," 分割</td>
	</tr>
	<tr><td class="item_title" colspan="2">发帖 IP</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="Ip" runat="server" RequiredFieldType="暂无校验" Width="150"></cc1:TextBox>
		</td>
		<td class="vtop">通配符 "*" 如 "127.0.*.*", 慎用!</td>
	</tr>
	<tr><td class="item_title" colspan="2">内容关键字</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="message" runat="server" RequiredFieldType="暂无校验"  width="200"></cc1:TextBox>
		</td>
		<td class="vtop">多关键字中间请用半角逗号 "," 分割</td>
	</tr>
</table>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div class="Navbutton"><cc1:Button id="SaveConditionInf" runat="server" Text="搜索符合条件的帖子" ButtonImgUrl="../images/search.gif"></cc1:Button></div>
</fieldset>
</form>
</div>	
<%=footer%>
</body>
</html>