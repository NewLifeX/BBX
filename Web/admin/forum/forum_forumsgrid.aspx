<%@ Page language="c#" Inherits="BBX.Web.Admin.forumsgrid" Codebehind="forum_forumsgrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>手动调整版块</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />		
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
	<uc1:PageInfo id="info1" runat="server" Icon="warning"
	 Text="该功能为手动修改版块数据, 这将对论坛版块表中的版块名称, 子版数等相关内容进行调整. 操作务必谨慎!"></uc1:PageInfo>
	<cc1:datagrid id="DataGrid1" runat="server" IsFixConlumnControls="true" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid" >
		<Columns>
			<asp:TemplateColumn HeaderText="版块ID">
				<ItemTemplate>
					<%# Eval("fid").ToString()%>
					<%# DataGrid1.LoadSelectedCheckBox(Eval("fid").ToString())%>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="fid" HeaderText="版块ID" Visible="false" ></asp:BoundColumn>
			<asp:BoundColumn DataField="name" HeaderText="版块名称"></asp:BoundColumn>
			<asp:BoundColumn DataField="layer" HeaderText="显示层数" ReadOnly="true"></asp:BoundColumn>
			<asp:BoundColumn DataField="parentidlist" HeaderText="父列表" ReadOnly="true"></asp:BoundColumn>
			<asp:BoundColumn DataField="subforumcount" HeaderText="子版数" ></asp:BoundColumn>
			<asp:BoundColumn DataField="displayorder" HeaderText="显示顺序"></asp:BoundColumn>
			<asp:BoundColumn DataField="status" HeaderText="状态" ReadOnly="true"></asp:BoundColumn>
			<asp:BoundColumn DataField="topics" HeaderText="主题总数" ReadOnly="true"></asp:BoundColumn>
			<asp:BoundColumn DataField="posts" HeaderText="回帖总数" ReadOnly="true"></asp:BoundColumn>
			<asp:BoundColumn DataField="todayposts" HeaderText="今天回帖数" ReadOnly="true"></asp:BoundColumn>
			<asp:BoundColumn DataField="lastposter" HeaderText="最后回帖人" ReadOnly="true"></asp:BoundColumn>			
		</Columns>
	</cc1:datagrid>
	<br />
	<p style="text-align:right;">
		<cc1:Button id="SaveForum" runat="server" Text="保存版块修改"></cc1:Button>
	</p>
</form>
<%=footer%>
</body>
</html>