<%@ Page language="c#" Inherits="BBX.Web.Admin.forumsmove" Codebehind="forum_forumsmove.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>移动版块</title>		
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
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">移动版块</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">源版块</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:DropDownTreeList id="sourceforumid" runat="server"></cc1:DropDownTreeList>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">移动方式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="movetype" runat="server" RepeatColumns="1">
				<asp:ListItem Value="0" >调整顺序到目标版块前</asp:ListItem>
				<asp:ListItem Value="1" Selected="True">作为目标版块的子版块</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">目标版块</td></tr>
	<tr>
		<td class="vtop" colspan="2">
			<cc1:ListBoxTreeList id="targetforumid" runat="server"></cc1:ListBoxTreeList>
		</td>
	</tr>
</table>
<div class="Navbutton">
	<cc1:Button id="SaveMoveInfo" runat="server" Text=" 提 交 "/>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" onclick="javascript:window.location.href='forum_forumstree.aspx';"><img src="../images/arrow_undo.gif" /> 返 回 </button>
</div>
</fieldset>
</form>
</div>
<%=footer%>
</body>
</html>