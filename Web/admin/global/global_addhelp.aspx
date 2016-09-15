<%@ Page Language="C#"  CodeBehind="global_addhelp.aspx.cs" Inherits="BBX.Web.Admin.addhelp" %>
<%@ Register TagPrefix="uc1" TagName="OnlineEditor" Src="../UserControls/OnlineEditor.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>无标题页</title>
<link href="../styles/default.css" rel="stylesheet" type="text/css" id="css" />
<link href="../styles/editor.css" rel="stylesheet" type="text/css" id="Link1" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<link href="../../templates/default/seditor.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../javascript/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
<script type="text/javascript">
	function validate()
	{
		$('helpmessage_hidden').value=parseubb($('helpmessage').value)
		return true;
	}
</script>
</head>
<body>
<div class="ManagerForm">
<form id="Form1" runat="server" method="post"  onsubmit="return validate();">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">添加帮助</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">帮助标题</td></tr>
	<tr>
		<td class="vtop rowform">
			  <cc1:TextBox id="title" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验"  maxlength="249" size="60"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">帮助分类</td></tr>
	<tr>
		<td class="vtop rowform">
			   <cc1:DropDownList id="type" runat="server"></cc1:DropDownList>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">帮助内容</td></tr>
	<tr>
		<td class="vtop" colspan="2">
        <uc1:OnlineEditor ID="help" runat="server" controlname="help" postminchars="0" postmaxchars="200"></uc1:OnlineEditor>			 
			 
		</td>
	</tr>
</table>
<div style="display:none">
	<tr><td class="item_title" colspan="2">发布者用户名</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="poster" runat="server" RequiredFieldType="暂无校验" CanBeNull="必填" MaxLength="20" Enabled="false"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
</div>
<div class="Navbutton">
	<cc1:Button id="Addhelp" runat="server" Text=" 提 交 "></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button3" onclick="window.history.back();"><img src="../images/arrow_undo.gif"/> 返 回 </button>
</div>
</fieldset>
</form>
</div>
<%=footer%>
</body>
</html>