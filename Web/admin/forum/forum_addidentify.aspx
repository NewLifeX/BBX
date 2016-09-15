<%@ Page language="c#" Inherits="BBX.Web.Admin.addidentify" Codebehind="forum_addidentify.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>鉴定添加</title>		
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
<legend style="background:url(../images/icons/icon49.jpg) no-repeat 6px 50%;">鉴定添加</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">名称</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="name" runat="server" CanBeNull="必填" Text="" MaxLength="50" Size="20"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">上传鉴定大图片</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:UpFile id="uploadfile" IsShowTextArea=false runat="server" UpFilePath="../../images/identify/" FileType=".jpg|.gif|.png"></cc1:UpFile>
		</td>
		<td class="vtop">上传的文件扩展名为<b>jpg</b> ,<b>gif</b>,<b>png</b>,文件大小最好不要超过512 KB，此图将显示在鉴定的帖子内</td>
	</tr>
    <tr><td class="item_title" colspan="2">上传鉴定小图片</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:UpFile id="uploadfilesmall" IsShowTextArea=false runat="server" UpFilePath="../../images/identify/" FileType=".jpg|.gif|.png"></cc1:UpFile>
		</td>
		<td class="vtop">上传的文件扩展名为<b>jpg</b> ,<b>gif</b>,<b>png</b>,文件大小最好不要超过512 KB，此图将显示在版块列表中</td>
	</tr>
</table>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div class="Navbutton">
	<cc1:Button id="AddIdentifyInfo" runat="server" Text=" 提 交 "></cc1:Button>
</div>
</fieldset>
</div>
</form>
<%=footer%>
</body>
</html>