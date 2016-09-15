<%@ Page Language="c#" Inherits="BBX.Web.Admin.resetpassword" Codebehind="resetpassword.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>添加用户</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background: url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">重置密码</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">用户名</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox ID="userName" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验" MaxLength="30" Size="25" Enabled="False"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">新密码</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox ID="password" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验" TextMode="Password" MaxLength="32" Size="25"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">重复输入密码</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox ID="passwordagain" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验" MaxLength="32" Size="25" TextMode="Password"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<div class="Navbutton"><cc1:Button ID="ResetUserPWs" runat="server" Text=" 提 交 "></cc1:Button></div>
</fieldset>
</div>
</form>
<%=footer%>
</body>
</html>