<%@ Page Language="c#" Inherits="BBX.Web.Admin.announcegrid" Codebehind="global_announcegrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc4" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>公告列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/calendar.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form,'id','DelRec');
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
	<div style="display: none">
		<table id="Table1" width="100%" cellspacing="1" cellpadding="4">
			<tr>
				<td colspan="2" class="td1"><b>公告搜索</b></td>
			</tr>
			<tr>
				<td width="50%" class="td2">作者:</td>
				<td class="td2">
					<cc2:TextBox ID="poster" runat="server" RequiredFieldType="暂无校验" Width="300"></cc2:TextBox>
				</td>
			</tr>
			<tr>
				<td class="td1">标题:</td>
				<td class="td1">
					<cc2:TextBox ID="title" runat="server" RequiredFieldType="暂无校验" Width="300"></cc2:TextBox></td>
			</tr>
			<tr>
				<td class="td2">发布日期:</td>
				<td class="td2">
					从<cc4:Calendar ID="postdatetimeStart" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js"></cc4:Calendar>
					到<cc4:Calendar ID="postdatetimeEnd" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js"></cc4:Calendar>
					<br />
				</td>
			</tr>
			<tr>
				<td class="td1" align="right"></td>
				<td class="td1">
					<cc1:Button ID="Search" runat="server" Text="开始搜索" ButtonImgUrl="../images/search.gif"></cc1:Button>
				</td>
			</tr>
		</table>
	</div>
	<cc1:DataGrid ID="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid">
		<Columns>
			<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
				<HeaderStyle Width="20px" />
				<ItemTemplate>
						<input id="id" onclick="checkedEnabledButton(this.form,'id','DelRec')" type="checkbox" value="<%# Eval("id").ToString() %>" name="id" />
					</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="">
				<ItemTemplate>
						<a href="global_editannounce.aspx?id=<%# Eval("id").ToString()%>">编辑</a>
					</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="ID" SortExpression="id" HeaderText="公告ID [递增]" Visible="false" />
			<asp:BoundColumn DataField="poster" SortExpression="poster" HeaderText="发布者用户名" />
			<asp:BoundColumn DataField="title" SortExpression="title" HeaderText="公告标题" ItemStyle-HorizontalAlign="Left"/>
			<asp:BoundColumn DataField="displayorder" SortExpression="displayorder" HeaderText="显示顺序" />
			<asp:BoundColumn DataField="starttime" SortExpression="starttime" HeaderText="起始时间" />
			<asp:BoundColumn DataField="endtime" SortExpression="endtime" HeaderText="结束时间" />
		</Columns>
	</cc1:DataGrid>
	<p style="text-align:right;">
		<button type="button" class="ManagerButton" onclick="javascript:window.location.href='global_addannounce.aspx';">
			<img src="../images/add.gif" />添加公告
		</button>&nbsp;&nbsp;
		<cc1:Button ID="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选的公告吗？')) return false;"></cc1:Button>
	</p>
</form>
<%=footer%>
</body>
</html>