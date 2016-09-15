<%@ Page language="c#" Codebehind="forum_topictypesgrid.aspx.cs" Inherits="BBX.Web.Admin.topictypesgrid" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>主题分类</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />	
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />	
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
function Check(form)
{
	CheckAll(form);
	checkedEnabledButton(form,'id','delButton');
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background: transparent url(../images/icons/icon32.jpg) no-repeat scroll 6px 50%; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;">搜索主题分类</legend>
<asp:Panel ID="searchtable" runat="server" Visible="true">
<table width="100%">
	<tr>
		<td style="width: 80px">主题分类名:</td>
		<td style="width:150px">
			<cc1:TextBox ID="topictypename" runat="server" RequiredFieldType="暂无校验" Width="100"></cc1:TextBox>
		</td>
		<td><cc1:Button ID="Search" runat="server" Text="开始搜索"></cc1:Button></td>
	</tr>
</table>
</asp:Panel>
<cc1:Button ID="ResetSearchTable" runat="server" Text="重设搜索条件" Visible="False"></cc1:Button>
</fieldset>
</div>
<cc1:datagrid id="DataGrid1" runat="server" IsFixConlumnControls="true" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid" PageSize="10">
	<Columns>
		<asp:TemplateColumn HeaderText="<input title='选中/取消选中' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<HeaderStyle Width="20px" />
			<ItemTemplate>
				<input id="id" onclick="checkedEnabledButton(this.form,'id','delButton')" type="checkbox" value="<%# Eval("id").ToString() %>" name="id" />
				<%# DataGrid1.LoadSelectedCheckBox(Eval("id").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="name" SortExpression="name" HeaderText="主题分类"></asp:BoundColumn>
		<asp:BoundColumn DataField="displayorder" SortExpression="displayorder" HeaderText="显示顺序"></asp:BoundColumn>
		<asp:BoundColumn DataField="description" HeaderText="描述"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="关联的版块">
			<ItemStyle HorizontalAlign="Left"></ItemStyle>
			<ItemTemplate>
				<%# LinkForum(Eval("id").ToString()) %>
			</ItemTemplate>
		</asp:TemplateColumn>					
	</Columns>
</cc1:datagrid>
<p style="text-align:right;">
	<cc1:Button id="SaveTopicType" runat="server" Text="保存主题分类"></cc1:Button>&nbsp;&nbsp;
	<cc1:Button id="delButton" runat="server" Text="删除主题分类" Enabled="false" ButtonImgUrl="../images/del.gif" OnClientClick="if(!confirm('你确认要删除所选主题分类吗？')) return false;"></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button2" onclick="BOX_show('neworedit');"><img src="../images/add.gif"/> 建立主题分类 </button>
</p>
<div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
<div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:400px;">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/icon45.jpg) no-repeat 6px 50%;">添加主题分类</legend>
<table cellspacing="0" cellpadding="4" width="100%" align="center">
	<tr>
		<td style="width: 90px;height:35px">主题分类名:</td>
		<td>
			<cc2:TextBox id="typename" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="true" MaxLength="30"></cc2:TextBox>
		</td>
	</tr>
	<tr>
		<td style="height:35px">显示顺序:</td>
		<td>
			<cc2:TextBox id="displayorder" runat="server" RequiredFieldType="数据校验"  size="3" MaxLength="4" HintInfo="在版块中显示顺序，序号按升序排序" Text="0"></cc2:TextBox>
		</td>
	</tr>
	<tr>
		<td style="height:35px">描述:</td>
		<td>
			<cc2:TextBox id="description" runat="server" RequiredFieldType="暂无校验" MaxLength="500"></cc2:TextBox>
		</td>
	</tr>
	<tr>
		<td align="center" colspan="2" style="height:35px">
			<cc1:Button id="AddNewRec" runat="server" Text=" 提 交 "></cc1:Button>&nbsp;&nbsp;
			<button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworedit');"><img src="../images/state1.gif"/> 取 消 </button>
		</td>
	</tr>
</table>
</fieldset>
</div>
</div>
</form>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div id="setting" />
<%=footer%>
</body>
</html>