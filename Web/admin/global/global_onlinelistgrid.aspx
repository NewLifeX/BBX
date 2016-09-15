<%@ Page Language="c#" Inherits="BBX.Web.Admin.onlinelistgrid" Codebehind="global_onlinelistgrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>在线列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<uc1:PageInfo ID="info1" runat="server" Icon="information" Text="<ul><li>本功能用于自定义首页及主题列表页显示的在线会员分组及图例, 只在在线列表功能打开时有效. </li></ul><ul><li>用户组图例处空白为不区分该组用户, 所有未区分的用户组将统一归入第一行的“普通用户”项. </li></ul><ul><li>用户组图例中可直接选择相应的图片或请将您的相应图片文件上传到 站点:images/groupicons 目录中, 并在此窗口中选择相应的图片文件名. </li></ul><ul><li>组头衔最大长度为50个字符</li></ul>" />
<cc1:DataGrid ID="DataGrid1" runat="server" PageSize="10" OnCancelCommand="DataGrid_Cancel" OnEditCommand="DataGrid_Edit"
	OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid" OnUpdateCommand="DataGrid_Update">
	<Columns>
		<asp:BoundColumn DataField="groupid" HeaderText="groupid [递增]" Visible="false"></asp:BoundColumn>
		<asp:BoundColumn DataField="newdisplayorder" HeaderText="显示顺序"></asp:BoundColumn>
		<asp:BoundColumn DataField="title" HeaderText="组头衔"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="用户组图例">
			<itemtemplate>
				<%# Eval("img")%>
			</itemtemplate>
			<edititemtemplate>
				<asp:DropDownList id="imgdropdownlist" runat="server"></asp:DropDownList>
			</edititemtemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="图例显示">
			<itemtemplate>
			   <%# GetImgLink(Eval("img").ToString())%>
			</itemtemplate>
		</asp:TemplateColumn>
	</Columns>
</cc1:DataGrid>
<p style="text-align:right;">
	<button type="button" class="ManagerButton" onclick="javascript:window.location.href='global_uploadonlieninco.aspx';">
		<img src="../images/add.gif" />用户组图例上传
	</button>
</p>
</form>
<%=footer%>
</body>
</html>
