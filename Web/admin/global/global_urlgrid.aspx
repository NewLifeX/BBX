<%@ Page Language="c#" Inherits="BBX.Web.Admin.urlgrid" Codebehind="global_urlgrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>伪静态url的替换规则</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" runat="server">
	<uc1:PageInfo ID="info1" runat="server" Icon="information" Text="对正则式不太了解的情况下请不要改动, 如果修改建议事先备份论坛目录下的config/urls.config文件" />
	<uc1:PageInfo ID="PageInfo1" runat="server" Icon="warning" Text="[name为只读项]" />
	<cc1:DataGrid ID="DataGrid1" runat="server" OnCancelCommand="DataGrid_Cancel" OnEditCommand="DataGrid_Edit"
		OnUpdateCommand="DataGrid_Update">
		<Columns>
			<asp:TemplateColumn HeaderText="name">
				<itemtemplate>
					<%# Eval("name")%>
				</itemtemplate>
				<edititemtemplate>
					<asp:TextBox id="nametext" runat="server"></asp:TextBox>
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="path" HeaderText="path"></asp:BoundColumn>
			<asp:BoundColumn DataField="pattern" HeaderText="pattern"></asp:BoundColumn>
			<asp:BoundColumn DataField="page" HeaderText="page"></asp:BoundColumn>
			<asp:BoundColumn DataField="querystring" HeaderText="querystring"></asp:BoundColumn>
		</Columns>
	</cc1:DataGrid><br />
	<p style="text-align:right;">
		<button type="button" class="ManagerButton" id="Button3" onclick="window.location='global_searchengine.aspx';"><img src="../images/arrow_undo.gif"/> 返 回 </button>
	</p>
</form>
<%=footer%>
</body>
</html>