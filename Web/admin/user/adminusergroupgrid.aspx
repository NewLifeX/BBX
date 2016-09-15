<%@ Page Language="c#" Inherits="BBX.Web.Admin.adminusergroupgrid" Codebehind="adminusergroupgrid.aspx.cs" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>系统组列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
	<uc1:PageInfo id="info1" runat="server" Icon="warning"
	Text="<ul><li>{forumproductname} 管理组包括管理员、超级版主、版主以及关联了这三个组的用户组. </ul><ul><li>增加管理组方法有两种:<br />方法1:<ul><li>进入<a href=&quot;addadminusergroup.aspx&quot;>管理组添加</a>, 增加一个新的管理组;<li>并在管理权限设置页中选择一个 &quot;关联管理组 &quot;　,同时编辑该组的其他设置. </ul></ul><ul>方法2:<ul><li>点击下面的相关管理组记录上的 &quot;添加&quot; 链接,系统会用相关管理组的信息作为模板初始化&quot;添加表单&quot;,同时编辑该组的其他设置. </ul></ul>"></uc1:PageInfo>
	<cc1:DataGrid ID="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid">
		<Columns>
			<asp:TemplateColumn HeaderText="">
				<itemtemplate>
						<a href="editadminusergroup.aspx?groupid=<%# Eval("ID").ToString()%>">编辑</a>
					</itemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="">
				<ItemTemplate>
						<a href="addadminusergroup.aspx?groupid=<%# Eval("ID").ToString()%>">添加</a>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="ID" SortExpression="ID" HeaderText="用户组ID" Visible="false"></asp:BoundColumn>
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
		<button type="button" class="ManagerButton" id="Button3" onclick="window.location='editgroup.aspx';"><img src="../images/arrow_undo.gif"/> 返 回 </button>           
	</p>
</form>
<%=footer%>
</body>
</html>