<%@ Page language="c#" Inherits="BBX.Web.Admin.forum_tagmanage" Codebehind="forum_tagmanage.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>标签管理</title>	
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />		
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
function Check(form)
{
	CheckAll(form);
	checkedEnabledButton(form,'tagid','DisableRec');
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/icon32.jpg) no-repeat 6px 50%;">标签查询</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">标签名称</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="tagname" runat="server" Width="150px" onBlur></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">主题数介于</td></tr>
	<tr>
		<td class="vtop" colspan="2">
			<cc1:TextBox id="txtfrom" runat="server" Width="150px"></cc1:TextBox>--<cc1:TextBox id="txtend" runat="server" Width="150px"></cc1:TextBox>
		</td>
	</tr>
	<tr><td class="item_title" colspan="2"></td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="radstatus" runat="server" RepeatColumns = "3" RepeatDirection="Vertical">
				<asp:ListItem Value="0"  Selected="True">全部</asp:ListItem>
				<asp:ListItem Value="1">锁定</asp:ListItem>
				<asp:ListItem Value="2">开放</asp:ListItem>
			</cc1:RadioButtonList><br />
		</td>
		<td class="vtop">请选择</td>
	</tr>
</table>
<div class="Navbutton"><cc1:Button id="searchtag" runat="server" Text="搜索标签" OnClick="searchtag_Click"/></div>
</fieldset>
</div>
<cc1:datagrid id="DataGrid1" runat="server" IsFixConlumnControls="true" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid" PageSize="15">
<Columns>
	<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
		<HeaderStyle Width="20px" />
		<ItemTemplate>
			<input id="tagid" type="checkbox" onclick="checkedEnabledButton(this.form,'tagid','DisableRec')" value="<%# Eval("ID") %>" name="tagid"/>
		</ItemTemplate>
	</asp:TemplateColumn>
	<asp:TemplateColumn HeaderText="标签名称">
		<itemtemplate>
			<a href="../../topictag-<%# Eval("ID") %>.aspx" target="_blank"><%# Eval("Name") %></a> | <%# Eval("fcount") %>
			<%# DataGrid1.LoadSelectedCheckBox(Eval("ID")+"") %>
		</itemtemplate>
	</asp:TemplateColumn>
	<asp:BoundColumn DataField="orderid" HeaderText="显示顺序"></asp:BoundColumn>
	<asp:BoundColumn DataField="color" HeaderText="颜色"></asp:BoundColumn>
</Columns>
</cc1:datagrid>
<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<p style="text-align:right;">
<cc1:Button ID="savetags" runat="server" Text="保存标签修改" OnClick="savetags_Click"></cc1:Button>&nbsp;&nbsp;
<cc1:Button id="DisableRec" runat="server" Text=" 禁 用 " Enabled="false" ButtonImgUrl="../images/del.gif" OnClick="DisableRec_Click"></cc1:Button>
</p>
</form>
<%=footer%>
</body>
</html>