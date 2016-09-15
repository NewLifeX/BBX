<%@ Page language="c#" Inherits="BBX.Web.Admin.addmedal" Codebehind="global_addmedal.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>勋章添加</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" /> 
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">勋章添加</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">名称</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="name" runat="server" RequiredFieldType="暂无校验" CanBeNull="必填" Width="80%"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">是否有效</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="available" runat="server">
				<asp:ListItem Value="1" Selected="True">有效</asp:ListItem>
				<asp:ListItem Value="0">无效</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">勋章图片上传</td></tr>
	<tr>
		<td class="vtop" colspan="2">
			<cc1:UpFile id="image" runat="server" UpFilePath="../../images/medals" FileType=".jpg|.gif|.png" ShowPostDiv="false"></cc1:UpFile>
		</td>
	</tr>
</table>
<div class="Navbutton"><cc1:Button id="AddMedalInfo" runat="server" Text=" 提 交 " OnClick="AddMedalInfo_Click"></cc1:Button></div>
</fieldset>
</div>
</form>
<%=footer%>
</body>
</html>