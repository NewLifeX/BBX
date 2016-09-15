<%@ Page Language="c#" Inherits="BBX.Web.Admin.editorset" CodeBehind="global_editorset.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>编辑器设置</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server" name="Form1">
<fieldset>
	<legend style="background: url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">编辑器设置</legend>
	<table width="100%">

	<tr><td class="item_title" colspan="2"><%=BBX.Common.Utils.ProductName%>代码模式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="bbcodemode" runat="server" RepeatColumns="1" RepeatLayout="flow">
				<asp:ListItem Value="0"  Selected=true>标准论坛代码</asp:ListItem>
				<asp:ListItem Value="1" >动网UBB代码兼容模式</asp:ListItem>								
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">注意: 动网UBB兼容模式只适用于从动网论坛转换而来的论坛数据.</td>
	</tr>
		<tr><td class="item_title" colspan="2">默认的编辑器模式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="defaulteditormode" runat="server" RepeatColumns="1">
				<asp:ListItem Value="0">论坛代码编辑器</asp:ListItem>
				<asp:ListItem Value="1">可视化编辑器</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">默认的编辑器模式</td>
	</tr>
	<tr><td class="item_title" colspan="2">是否允许切换编辑器模式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="allowswitcheditor" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">选择否将禁止用户在 <%=BBX.Common.Utils.ProductName%> 代码模式和所见即所得模式之间切换.</td>
	</tr>
    <tr><td class="item_title" colspan="2">批量上传方式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="swfupload" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1" Selected=true>Flash批量上传</asp:ListItem>
				<asp:ListItem Value="0">Silverlight批量上传</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">发帖批量上传文件中使用的上传方式</td>
	</tr>
	</table>
</fieldset>

<div class="Navbutton">
	<cc1:Button ID="SaveInfo" runat="server" Text="提 交"></cc1:Button>
</div>
</form>
</div>
<%=footer%>
</body>
</html>
