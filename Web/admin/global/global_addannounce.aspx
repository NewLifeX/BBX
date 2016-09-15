<%@ Register TagPrefix="uc1" TagName="OnlineEditor" Src="../UserControls/OnlineEditor.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Page language="c#" Inherits="BBX.Web.Admin.addannounce" Codebehind="global_addannounce.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>添加公告</title>
<link href="../styles/default.css" rel="stylesheet" type="text/css" id="css" />
<link href="../styles/editor.css" rel="stylesheet" type="text/css" id="Link1" />
<script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../../javascript/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
	<link href="../../templates/default/seditor.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
	function validate(theform)
	{
		if(document.getElementById("title").value == "")
		{
			alert("公告标题不能为空");
			document.getElementById("title").focus();
			return false;
		}
		$('announcemessage_hidden').value=parseubb($('announcemessage').value)

		return true;
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body >
<div class="ManagerForm">
<form id="Form1" runat="server" onsubmit="return validate(this);">
<fieldset>
<legend style="background:url(../images/icons/icon33.jpg) no-repeat 6px 50%;">添加公告</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">显示顺序</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox ID="displayorder" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Text="0" MaxLength="6" Size="3"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">公告标题</td></tr>
	<tr>
		<td class="vtop rowform">
			  <cc1:TextBox ID="title" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验" MaxLength="249" Size="60"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">起始时间</td></tr>
	<tr>
		<td class="vtop rowform">
			  <cc1:TextBox ID="starttime" runat="server" CanBeNull="必填" RequiredFieldType="日期时间" Width="200"></cc1:TextBox>
		</td>
		<td class="vtop">格式:2005-5-5 13:22:02</td>
	</tr>
	<tr><td class="item_title" colspan="2">结束时间</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox ID="endtime" runat="server" CanBeNull="必填" RequiredFieldType="日期时间" Width="200"></cc1:TextBox>
		</td>
		<td class="vtop">格式:2005-5-5 13:22:02</td>
	</tr>
	<tr><td class="item_title" colspan="2">公告内容:</td></tr>
	<tr>
		<td class="vtop" colspan="2">
        <uc1:OnlineEditor ID="announce" runat="server" controlname="announce" postminchars="0" postmaxchars="200"></uc1:OnlineEditor>
		</td>
	</tr>
</table>
<div style="display:none">
	<tr><td class="item_title" colspan="2">发布者用户名</td></tr>
	<tr>
		<td class="vtop" colspan="2">
			  <cc1:TextBox id="poster" runat="server" RequiredFieldType="暂无校验" CanBeNull="必填" MaxLength="20" Enabled="false"></cc1:TextBox>
		</td>
	</tr>
</div>
<div class="Navbutton">
	<cc1:Button id="AddAnnounceInfo" runat="server" Text=" 提 交 " ValidateForm="true"></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button3" onclick="window.history.back();"><img src="../images/arrow_undo.gif"/> 返 回 </button>
</div>
</fieldset>
</form>
</div>
<%=footer%>
</body>
</html>