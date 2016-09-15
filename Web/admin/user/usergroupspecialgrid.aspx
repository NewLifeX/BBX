<%@ Page Language="c#" Inherits="BBX.Web.Admin.usergroupspecialgrid" Codebehind="usergroupspecialgrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>特殊组列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div>
	<uc1:PageInfo ID="info1" runat="server" Icon="information" Text="增加特殊用户组方法有两种:<br />方法1: 进入<a href=&quot;addusergroupspecial.aspx&quot;>特殊用户组添加</a>, 增加一个新的用户组,同时编辑该组的其他设置. <br />方法2: 点击下面的相关特殊用户组记录上的 &quot;添加&quot; 链接,系统会用相关特殊用户组的信息作为模板初始化&quot;添加表单&quot;,同时编辑该组的其他设置." />
	<cc1:DataGrid ID="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid">
		<Columns>
			<asp:TemplateColumn HeaderText="">
				<itemtemplate>
					<a href="editusergroupspecial.aspx?groupid=<%# Eval("groupid").ToString()%>">编辑</a>
				</itemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="">
				<itemtemplate>
					<a href="addusergroupspecial.aspx?groupid=<%# Eval("groupid").ToString()%>">添加</a>
				</itemtemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="groupid" SortExpression="groupid" HeaderText="用户组ID" Visible="false"></asp:BoundColumn>
			<asp:BoundColumn DataField="grouptitle" SortExpression="grouptitle" HeaderText="名称"></asp:BoundColumn>
			<asp:BoundColumn DataField="stars" SortExpression="stars" HeaderText="星星数目"></asp:BoundColumn>
			<asp:BoundColumn DataField="readaccess" SortExpression="readaccess" HeaderText="阅读权限"></asp:BoundColumn>
			<asp:BoundColumn DataField="maxprice" SortExpression="maxprice" HeaderText="主题(附件)最高售价"></asp:BoundColumn>
			<asp:BoundColumn DataField="maxpmnum" SortExpression="maxpmnum" HeaderText="短消息最多条数"></asp:BoundColumn>
			<asp:BoundColumn DataField="maxsigsize" SortExpression="maxsigsize" HeaderText="签名最多字节"></asp:BoundColumn>
			<asp:BoundColumn DataField="maxattachsize" SortExpression="maxattachsize" HeaderText="附件最大尺寸 [单位:字节]"></asp:BoundColumn>
		</Columns>
	</cc1:DataGrid>
	<p style="text-align:right;">
		<button type="button" class="ManagerButton" id="Button3" onclick="window.history.back();"><img src="../images/arrow_undo.gif"/> 返 回 </button>
	</p>
</div>
</form>
<%=footer%>
</body>
</html>