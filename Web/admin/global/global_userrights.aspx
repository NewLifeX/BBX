<%@ Page Language="c#" Inherits="BBX.Web.Admin.userrights" Codebehind="global_userrights.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>registerandvisit</title>
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
	<legend style="background: url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">用户权限</legend>
	<table class="table1" cellspacing="0" cellpadding="4" width="100%" align="center">
		<tr></tr>
	</table>
</fieldset>
<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images/hint.gif"></cc1:Hint>
<div align="center">
	<cc1:Button ID="SaveInfo" runat="server" Text=" 提 交 "></cc1:Button>
</div>
</form>
</div>
<%=footer%>
</body>
</html>
