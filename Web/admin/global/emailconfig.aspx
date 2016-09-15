<%@ Page language="c#" Inherits="BBX.Web.Admin.emailconfig" Codebehind="emailconfig.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>邮箱设置</title>
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />  
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<fieldset>
	<legend style="background:url(../images/icons/icon41.jpg) no-repeat 6px 50%;">SMTP配置</legend>
	<table width="100%">
	<tr><td class="item_title" colspan="2">SMTP服务器</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="smtp" runat="server" RequiredFieldType="暂无校验" CanBeNull="必填" Width="200"></cc1:TextBox>
		</td>
		<td class="vtop">设置发送邮件的SMTP服务器地址</td>
	</tr>
	<tr><td class="item_title" colspan="2">系统邮箱名称</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="sysemail" runat="server" RequiredFieldType="电子邮箱" CanBeNull="必填" Width="200"></cc1:TextBox>
		</td>
		<td class="vtop">设置发送邮件的邮箱地址</td>
	</tr>
	<tr><td class="item_title" colspan="2">系统邮箱密码</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="password" textmode="password" runat="server" RequiredFieldType="暂无校验" CanBeNull="必填" Width="200"></cc1:TextBox>
		</td>
		<td class="vtop">设置邮箱的密码</td>
	</tr>
	<tr><td class="item_title" colspan="2">SMTP端口</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="port" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" size="7" maxlength="5"></cc1:TextBox>
		</td>
		<td class="vtop">设置SMTP服务器的端口</td>
	</tr>
	<tr><td class="item_title" colspan="2">用户名</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="userName" runat="server" RequiredFieldType="暂无校验" CanBeNull="必填" Width="200"></cc1:TextBox>
		</td>
		<td class="vtop">设置邮箱的用户名</td>
	</tr>
	<tr><td class="item_title" colspan="2">发送邮件程序:</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:dropdownlist id="smtpemail" runat="server" HintInfo="设置发送邮件的程序" DataTextField="key" DataValueField="key"></cc1:dropdownlist>
		</td>
		<td class="vtop">设置发送邮件的程序</td>
	</tr>
	</table>
	<div class="Navbutton">
		<cc1:Button id="SaveEmailInfo" runat="server" Text=" 提 交 " OnClick="SaveEmailInfo_Click"></cc1:Button>
	</div>
</fieldset>
<fieldset>
<legend style="background:url(../images/icons/icon40.jpg) no-repeat 6px 50%;">测试邮件配置</legend>
<table width="100%">
<tr><td class="item_title" colspan="2">EMAIL:</td></tr>
<tr>
	<td class="vtop rowform">
		<cc1:TextBox id="testEmail" runat="server" RequiredFieldType="电子邮箱" CanBeNull="可为空" Width="300"></cc1:TextBox></td>
	</td>
	<td class="vtop"><cc1:Button id="sendTestEmail" runat="server" Text=" 发送测试邮件 " OnClick="sendTestEmail_Click"></cc1:Button>设置要测试的邮箱地址,测试程序将发送一封邮件到测试邮箱中</td>
</tr>
</table>
</fieldset>
<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
</form>
</div>
<% =footer %>
</body>
</html>