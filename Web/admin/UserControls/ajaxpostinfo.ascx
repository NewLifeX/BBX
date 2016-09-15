<%@ Control Language="c#" Inherits="BBX.Web.Admin.ajaxpostinfo" Codebehind="ajaxpostinfo.ascx.cs" %>
<div id=PostInfo style="display:none;position:relative;border:1px dotted #DBDDD3; background-color:#FDFFF2; margin:4px;width:99%;valign:middle">
<table><tr><td><img border="0" src="../images/ajax_loading.gif" /></td><td valign=middle>正在载入中,请稍等.....</td></tr></table></div>
<br />
<%if(isexist){%>
<table cellspacing="0" cellPadding="4" width="99%" align="center"  valign="middle"  style="border:solid 1px #EAE9E1;" class="datalist"><tr class="head"><td>
 标题: <b><%=title%></b></td></tr><tr><td> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=message%></td></tr></table>
<%}%>
<SCRIPT LANGUAGE="JavaScript">
document.getElementById('PostInfo').style.display = "none";
</SCRIPT>
