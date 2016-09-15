<%@ Page Language="c#" Inherits="BBX.Web.Admin.runsql" Codebehind="global_runsql.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title></title>
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
	function showalert(info)
	{
		document.getElementById("alerterrorinfo").innerHTML = info;
		document.getElementById("alertdialog").style.display = 'block';
	}
	function closealert()
	{
		document.getElementById("alertdialog").style.display = 'none';
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<div id="alertdialog" class="PopUpModel" style="position: absolute; z-index: 101; left: 40%; top: 40%; display: none;">
	<div class="ctrl_title">
		<a onclick="closealert();" href="javascript:void(0);"><img border="0" src="../images/close.gif" /></a>警告
	</div>
	<div id="searchresult">
		<table cellpadding="4" cellspacing="4">
			<tr>
				<td>
					<span id="alerterrorinfo"></span>
				</td>
			</tr>
		</table>
		<br />
	</div>
</div>
<form id="Form1" method="post" runat="server">
<uc1:PageInfo ID="info1" runat="server" Icon="information" Text="如果要一次运行多条SQL语句,语句之间请使用 --/* SQL Separator */-- 进行分割即可." />
<fieldset>
<legend style="background: url(../images/icons/icon38.jpg) no-repeat 6px 50%;">执行SQL语句</legend>
<table width="100%">
	<tr>
		<td class="vtop">
			<uc1:TextareaResize ID="sqlstring" runat="server" controlname="sqlstring" cols="80" rows="10" is_replace="false"></uc1:TextareaResize>
		</td>
	</tr>
</table>
<div class="Navbutton"><cc1:Button ID="RunSqlString" runat="server" Text=" 执行SQL语句 " Enabled="false"></cc1:Button></div>
</fieldset>
</form>
<script type="text/javascript">            
	document.getElementById("sqlstring_posttextarea").onkeyup = function(){ document.getElementById("RunSqlString").disabled = (document.getElementById("sqlstring_posttextarea").value == ""); }
</script>
<%=footer%>
</div>
</body>
</html>