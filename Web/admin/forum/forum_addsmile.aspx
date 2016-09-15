<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Page language="c#" Inherits="BBX.Web.Admin.addsmile" Codebehind="forum_addsmile.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>添加表情</title>		
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/icon49.jpg) no-repeat 6px 50%;">表情添加</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">显示顺序</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="displayorder" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Text="0" MaxLength="4" Size="4"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">代码</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="code" runat="server" RequiredFieldType="暂无校验" CanBeNull="必填" MaxLength="30"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">上传图片</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:UpFile id="url" IsShowTextArea="false" runat="server" UpFilePath="../../editor/images/smilies/" FileType=".jpg|.gif|.png"></cc1:UpFile>
		</td>
		<td class="vtop">上传的文件扩展名为<b>jpg</b> ,<b>gif</b>,<b>png</b>,文件大小最好不要超过512 KB</td>
	</tr>
</table>
<div class="Navbutton">
<cc1:Button id="AddSmileInfo" runat="server" Text=" 提 交 "></cc1:Button>&nbsp;&nbsp;
<button type="button" class="ManagerButton" id="Button3" onclick="history.go(-1);"><img src="../images/arrow_undo.gif"/> 返 回 </button>
</div>
</fieldset>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
</div>
</form>
<%=footer%>
</body>
</html>