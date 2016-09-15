<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Page language="c#" Inherits="BBX.Web.Admin.uploadavatar" Codebehind="global_uploadavatar.aspx.cs" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
	<title>uploadavatar</title>			
    <script type="text/javascript" src="../js/common.js"></script>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<uc1:PageInfo ID="info1" runat="server" Icon="information" Text="技巧提示 : 先单击&quot;浏览&quot; 按钮后选取要上传的图片,然后单击&quot;上传&quot; 按钮, 系统会对您上传后的文件进行重命名并保存" />
<fieldset>
<legend style="background:url(../images/icons/icon37.jpg) no-repeat 6px 50%;">上传头像</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">头像图片上传</td></tr>
	<tr>
		<td class="vtop">
			<cc1:UpFile id="url" runat="server" HintTitle="提示" UpFilePath="../../avatars/common/" FileType=".jpg|.gif|.png"  IsShowTextArea="false"></cc1:UpFile>
		</td>
		<td align="left">
			<cc1:Button id="UpdateAvatarCache" runat="server" Text="上传并返回头像列表"  ShowPostDiv="false"></cc1:Button>&nbsp;&nbsp;
			<button type="button" class="ManagerButton" id="Button3" onclick="window.history.back();"><img src="../images/arrow_undo.gif"/> 返 回 </button>
		</td>
	</tr>
</table>
</fieldset>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
</form>
</div>
<%=footer%>
</body>
</html>