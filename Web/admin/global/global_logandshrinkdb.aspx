<%@ Page Language="c#" Inherits="BBX.Web.Admin.logandshrinkdb" Codebehind="global_logandshrinkdb.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>logandshrinkdb</title>
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
<legend style="background: url(../images/icons/icon26.jpg) no-repeat 6px 50%;">日志管理</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">数据库名称</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox ID="strDbName" runat="server" Text="" CanBeNull="必填" Width="100px" RequiredFieldType="暂无校验" Enabled="false"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">要收缩的大小范围</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox ID="size" runat="server" Text="0" Width="100px" RequiredFieldType="数据校验"></cc1:TextBox>单位: M (兆字节)
		</td>
		<td class="vtop">此值仅供程序压缩时进行参考</td>
	</tr>
</table>
<div class="Navbutton"><cc1:Button ID="ClearLog" runat="server" Text="清空日志"></cc1:Button>&nbsp;<cc1:Button ID="ShrinkDB" runat="server" Text="收缩数据库"></cc1:Button></div>
</fieldset>
<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
</form>
</div>
<%=footer%>
</body>
</html>