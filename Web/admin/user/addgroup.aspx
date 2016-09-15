<%@ Page Language="C#" AutoEventWireup="true" Inherits="BBX.Web.Admin.addgroup" Codebehind="addgroup.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>添加用户组</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="form1" runat="server">
<div class="ManagerForm">
<fieldset>
	<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">添加用户组</legend>
	<table class="table1" cellspacing="0" cellpadding="8" width="100%" align="center" >
		<tr align="center">
			<td><button type="button" class="ManagerButton" onclick="window.location='addusergroup.aspx';"><img src="../images/add.gif" /> 添加积分用户组</button></td>
			<td><button type="button" class="ManagerButton" onclick="window.location='addadminusergroup.aspx';"><img src="../images/add.gif" /> 添加管理组</button></td>
			<td><button type="button" class="ManagerButton" onclick="window.location='addusergroupspecial.aspx';"><img src="../images/add.gif" /> 添加特殊组</button></td>
		</tr>
	</table>
</fieldset>
</div>
</form>
<%=footer%>
</body>
</html>