<%@ Page Language="c#" Inherits="BBX.Web.Admin.medalgrid" Codebehind="global_medalgrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>勋章列表管理</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form,'medalid','Available','UnAvailable')
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
	<uc1:PageInfo ID="info1" runat="server" Icon="information" Text="请将您的勋章图片文件上传到 &quot;论坛/images/medals&quot; 目录下, 并对相应的勋章进行编辑即可" />
	<cc1:DataGrid ID="DataGrid1" runat="server" IsFixConlumnControls="true" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid">
		<Columns>
			<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
				<HeaderStyle Width="20px" />
				<ItemTemplate>
						<input id="medalid" onclick="checkedEnabledButton(this.form,'medalid','Available','UnAvailable')" type="checkbox" 
						value="<%# Eval("ID") %>" name="medalid" />
						<%# DataGrid1.LoadSelectedCheckBox(Eval("ID")+"")%>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="ID" SortExpression="ID" HeaderText="勋章ID" Visible="false" />
			<asp:BoundColumn DataField="name" SortExpression="name" HeaderText="名称" />
			<asp:BoundColumn DataField="image" SortExpression="image" HeaderText="文件名" />
			<asp:TemplateColumn HeaderText="是否有效" SortExpression="available">
				<ItemTemplate>
					   <%# (Boolean)Eval("Available")?"<img src=../images/ok.gif>" : "<img src=../images/cancel.gif>" %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="图片">
				<ItemTemplate>
					   <%# PicStr(Eval("Image")+"")%>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</cc1:DataGrid>
	<p style="text-align:right;">
		<cc1:Button ID="SaveMedal" runat="server" Text="保存勋章修改"></cc1:Button>&nbsp;
		<cc1:Button ID="Available" runat="server" Text="置为有效" Enabled="false"></cc1:Button>&nbsp;&nbsp;
		<cc1:Button ID="UnAvailable" runat="server" Text="置为无效" ButtonImgUrl="../images/invalidation.gif" Enabled="false"></cc1:Button>&nbsp;&nbsp;
		<cc1:Button ID="ImportMedal" runat="server" Text="导入已上传的勋章" ButtonImgUrl="../images/add.gif" />
	</p>
</form>
<%=footer%>
</body>
</html>