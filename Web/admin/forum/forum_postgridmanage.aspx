<%@ Page language="c#" Inherits="BBX.Web.Admin.postgridmanage" Codebehind="forum_postgridmanage.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>postgrid</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" /> 
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
function Check(form)
{
	CheckAll(form);
	checkedEnabledButton(form,'pid','SetPostInfo')
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server"><br />
	<cc1:datagrid id="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid" PageSize="15">
		<Columns>
			<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
				<HeaderStyle Width="20px" />
				<ItemTemplate>
					<input id="pid" onclick="checkedEnabledButton(this.form,'pid','SetPostInfo')" type="checkbox" 
					value="<%# Eval("pid").ToString() %>|<%# Eval("tid").ToString() %>" name="pid" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="pid"  SortExpression="pid" HeaderText="帖子ID" Visible="false" ></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="标题">
				<ItemTemplate>
					<a href="../../showtopic.aspx?topicid=<%# Eval("tid").ToString() %>&postid=<%# Eval("pid").ToString() %>" target="_blank">
						<%# Eval("title").ToString() %>
					</a>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="postdatetime" SortExpression="postdatetime" HeaderText="发布日期" ></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="发帖人">
				<itemtemplate>
					<%# (Eval("posterid").ToString() != "-1") ? "<a href='../../userinfo.aspx?userid=" + Eval("posterid").ToString() + "' target='_blank'>" + Eval("poster").ToString() + "</a>" : Eval("poster").ToString()%>
				</itemtemplate>
			</asp:TemplateColumn>				
			<asp:TemplateColumn HeaderText="是否通过审批">
				<ItemTemplate>
					<%# Invisible(Eval("invisible").ToString())%>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="attachment" SortExpression="attachment" HeaderText="附件数"></asp:BoundColumn>
			<asp:BoundColumn DataField="rate" SortExpression="rate" HeaderText="评分分数"></asp:BoundColumn>
			<asp:BoundColumn DataField="ratetimes" SortExpression="ratetimes" HeaderText="评分次数"></asp:BoundColumn>
		</Columns>
	</cc1:datagrid>
	<p style="text-align:right;">
		<cc1:Button id="SetPostInfo" runat="server" Text=" 删除 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选帖子吗？')) return false;"></cc1:Button> 
	</p>
</form>
<%=footer%>
</body>
</html>