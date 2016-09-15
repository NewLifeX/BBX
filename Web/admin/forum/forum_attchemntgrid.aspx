<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Page language="c#" Inherits="BBX.Web.Admin.attchemntgrid" Codebehind="forum_attchemntgrid.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>附件批量删除</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />  
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form,'aid','DeleteAttachment');
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<cc1:datagrid id="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid" PageSize="20">
	<Columns>
		<asp:TemplateColumn HeaderText="<input title='选中/取' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<HeaderStyle Width="20px" />
			<ItemTemplate>
				<input id="aid" onclick="checkedEnabledButton(this.form,'aid','DeleteAttachment')" type="checkbox" value="<%# Eval("aid").ToString() %>" name="aid" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="aid" SortExpression="aid" HeaderText="附件id" Visible="false" ></asp:BoundColumn>
		<asp:BoundColumn DataField="attachment" SortExpression="attachment" HeaderText="附件原名"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="文件名">
			<ItemTemplate>
				<a href="<%# Eval("filename").ToString().Trim().ToLower().IndexOf("http://") == -1 ? "../../upload/" + Eval("filename").ToString().Trim().Replace("\\","/") : Eval("filename").ToString().Trim() %>" target="_blank"><%# Eval("filename").ToString().Trim()%></a>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="poster" SortExpression="poster" HeaderText="作者" ></asp:BoundColumn>
		<asp:BoundColumn DataField="topictitle" SortExpression="topictitle" HeaderText="所在主题"></asp:BoundColumn>
		<asp:BoundColumn DataField="filesize" SortExpression="filesize" HeaderText="尺寸[字节]"></asp:BoundColumn>
		<asp:BoundColumn DataField="downloads" SortExpression="downloads" HeaderText="下载次数"></asp:BoundColumn>
	</Columns>
</cc1:datagrid>
<p style="text-align:right;">
	<cc1:Button id="DeleteAttachment" runat="server" Text=" 删除选中的附件 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选附件吗？\n删除后将不能恢复！')) return false;"></cc1:Button>&nbsp;&nbsp;
	<cc1:Button id="DeleteAll" runat="server" Text=" 直接全部删除 " ButtonImgUrl="../images/del.gif" OnClientClick="if(!confirm('你确认要删除所有附件吗？\n删除后将不能恢复！')) return false;"></cc1:Button>
</p>
</form>
<%=footer%>		
</body>
</html>
