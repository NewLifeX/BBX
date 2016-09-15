<%@ Page language="c#" Inherits="BBX.Web.Admin.forumcombination" Codebehind="forum_forumcombination.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>forumcombination</title>		
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<uc1:PageInfo id="info1" runat="server" Icon="Information" Text="合并论坛后, 源论坛的帖子全部转入目标论坛, 同时删除源论坛"></uc1:PageInfo>
<uc1:PageInfo id="PageInfo1" runat="server" Icon="Warning" Text="目前的功能要求进行合并的论坛不能有子论坛"></uc1:PageInfo>
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/icon44.jpg) no-repeat 6px 50%;">合并版块</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">源论坛</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:DropDownTreeList id="sourceforumid" runat="server"></cc1:DropDownTreeList>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">目标论坛</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:DropDownTreeList id="targetforumid" runat="server"></cc1:DropDownTreeList>
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<div class="Navbutton">
	<cc1:Button id="SaveCombinationInfo" runat="server" Text=" 提 交 "></cc1:Button>
</div>			
</fieldset>
</div>
</form>		
<%=footer%>
</body>
</html>