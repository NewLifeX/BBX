<%@ Page Language="c#" Inherits="BBX.Web.Admin.sharelistgrid" Codebehind="global_sharelistgrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>分享列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<table class="ntcplist">
<tbody>
<tr class="head"><td>分享列表</td></tr>
<tr>
<td>
<table width="100%" border="1" cellpadding="0" cellspacing="0" class="datalist">
  <tr class="category">
    <td style="border:1px solid #EAE9E1;">排序</td>
    <td style="border:1px solid #EAE9E1;">可用</td>
    <td style="border:1px solid #EAE9E1;">分享名称</td>
	<td style="border:1px solid #EAE9E1;">分享图标</td>
  </tr>
  <%foreach (string site in list){
  string[]  infoarray = site.Split('|');
  %>
  <tr>
    <td style="border:1px solid #EAE9E1;"><input name="newdisplayorder" type="text" value="<%=infoarray[0]%>" size="1"></td>
	<td style="border:1px solid #EAE9E1;"><input name="sharedisable" type="checkbox" value="<%=infoarray[1]%>" <%if (infoarray[3] == "1"){%>checked<%}%>></td>
	<td style="border:1px solid #EAE9E1;"><%=infoarray[2]%><input name="title" type="hidden" value="<%=infoarray[2]%>"><input name="site" type="hidden" value="<%=infoarray[1]%>"></td>
    <td style="border:1px solid #EAE9E1;"><img src="../../images/share/<%=infoarray[1]%>.gif"></td>
  </tr>
  <%}%>
  
</table>
</td>
</tr>
</tbody>
</table>
<div class="Navbutton">
	<cc1:Button ID="UpdateShare" runat="server" Text=" 提 交 "></cc1:Button>&nbsp;&nbsp;
</div>
</form>
<%=footer%>
</body>
</html>
