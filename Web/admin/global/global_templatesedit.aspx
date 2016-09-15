<%@ Page Language="c#" Inherits="BBX.Web.Admin.templatesedit" Codebehind="global_templatesedit.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>templatesedit</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/colorpicker.css" type="text/css" rel="stylesheet" />
<style type="text/css">
	.img
	{
		border:0px;
		align:bottom;
		position:relative;
		top:0.5%;
	}	
</style>
<script type="text/javascript">
	var n = 0;
	function displayHTML(obj) {
		window.open(obj, 'popup', 'toolbar = no, status = no, scrollbars=yes');
	}	
		
	function HighlightAll(obj) {
		obj.focus();
		obj.select();
		if (document.getElementById("templatenew_posttextarea")) {
			obj.createTextRange().execCommand("Copy");
			window.status = '将模板内容复制到剪贴板';
			setTimeout("window.status=''", 1800);
		}
	}

	function findInPage(obj, str) {
	try
	{
		var txt, i, found;
		if (str == "") {
			return false;
		}
		if (document.layers) {
			if (!obj.find(str)) {
				while(obj.find(str, false, true)) {
					n++;
				}
			} else {
				n++;
			}
			if (n == 0) {
				alert("未找到指定字串. ");
			}
		}
		if (document.getElementById("templatenew_posttextarea")) {
			txt = obj.createTextRange();
			for (i = 0; i <= n && (found = txt.findText(str)) != false; i++) {
				txt.moveStart('character', 1);
				txt.moveEnd('textedit');
			}
			if (found) {
				txt.moveStart('character', -1);
				txt.findText(str);
				txt.select();
				txt.scrollIntoView();
				n++;
			} else {
				if (n > 0) {
					n = 0;
					findInPage(str);
				} else {
					alert("未找到指定字串. ");
				}
			}
		}
		return false;
	}
	catch(error)
	{
		alert("已经到达文件尾！");
	}
	}
</script>
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<table width="100%">
	<tr><td class="item_title" colspan="2">编辑文件</td></tr>
	<tr>
		<td class="vtop rowform">
			 <b><%=filename%></b>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">获取颜色</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:ColorPicker ID="ColorPicker1" runat="server" ReadOnly="True"></cc1:ColorPicker>
			<uc1:TextareaResize ID="templatenew" runat="server" controlname="templatenew" is_replace="false" rows="20" cols="120"></uc1:TextareaResize>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr>
		<td class="vtop rowform">
			<input name="search" type="text" accesskey="t" size="20" onchange="n=0;" />&nbsp;
			<button type="button" class="ManagerButton" accesskey="f" onclick="findInPage(this.form.templatenew_posttextarea, this.form.search.value)">
				<img src="../images/search.gif" />搜索
			</button>&nbsp;&nbsp;
			<button type="button" class="ManagerButton" accesskey="c" onclick="HighlightAll(this.form.templatenew_posttextarea,'')">
				<img src="../images/topictype.gif" />复制
			</button>&nbsp;&nbsp;
			<%if(filename.IndexOf(".config") == -1){ %>
			<button type="button" class="ManagerButton" accesskey="p" onclick="displayHTML('<%=filenamefullpath%>')">
				<img src="../images/cache_resetall.gif" />预览
			</button>
			<%} %>
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<p style="text-align:right;width:80%;">
	<cc1:Button ID="SavaTemplateInfo" runat="server" Text=" 提 交 "></cc1:Button>&nbsp;&nbsp;
	<button class="ManagerButton" type="button" onclick="javascript:window.location.href='global_templatetree.aspx?templateid=<%=Request.Params["templageid"]%>&path=<%=Request.Params["path"].Split('\\')[0]%>&templatename=<%=Request.Params["templatename"]%>';">
		<img src="../images/arrow_undo.gif" /> 返 回 
	</button>
</p>
</form>
</div>
<%=footer%>
</body>
</html>