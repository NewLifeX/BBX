<%@ Page Language="c#" Inherits="BBX.Web.Admin.forumlinksgrid" Codebehind="global_forumlinksgrid.aspx.cs"%>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>友情链接列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form,'delid','DelRec');
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
	<cc1:DataGrid ID="DataGrid1" runat="server" IsFixConlumnControls="true" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid">
		<Columns>
			<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
				<HeaderStyle Width="20px" />
				<ItemTemplate>
						<input id="delid" onclick="checkedEnabledButton(this.form,'delid','DelRec')" type="checkbox" value="<%# Eval("id").ToString() %>" name="delid" />
						<%# DataGrid1.LoadSelectedCheckBox(Eval("id").ToString())%>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="id" SortExpression="id" HeaderText="id [递增]" Visible="false" />
			<asp:BoundColumn DataField="displayorder" SortExpression="displayorder" HeaderText="显示顺序" />
			<asp:BoundColumn DataField="name" SortExpression="name" HeaderText="名称" />
			<asp:BoundColumn DataField="url" SortExpression="url" HeaderText="链接地址" />
			<asp:BoundColumn DataField="note" SortExpression="note" HeaderText="说明" />
			<asp:BoundColumn DataField="logo" SortExpression="logo" HeaderText="LOGO" />
			<asp:TemplateColumn HeaderText="图片">
				<headerstyle width="10%" />
				<ItemTemplate>
						<%# LogoStr(Eval("LOGO").ToString())%>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</cc1:DataGrid>
	<p style="text-align:right;">
		<cc1:Button ID="SaveFriend" runat="server" Text="保存友情链接修改"></cc1:Button>&nbsp;&nbsp;
		<button type="button" class="ManagerButton" id="Button2" onclick="BOX_show('neworedit');"><img src="../images/add.gif"/> 新建友情链接 </button>&nbsp;&nbsp;
		<cc1:Button ID="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选的友情链接吗？')) return false;"></cc1:Button>
	</p>
</form>
<div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
<div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:450px;">
<div class="ManagerForm">
<form method="post" name="form2" id="form2" action="global_forumlinksgrid.aspx">
	<fieldset>
		<legend style="background: url(../images/icons/icon4.jpg) no-repeat 6px 50%;">添加友情链接</legend>
		<table cellspacing="0" cellpadding="4" cellspacing="4" width="100%" align="center">
			<tr>
				<td style="width: 70px;height:35px;">名称:</td>
				<td>
					<input name="name" type="text" id="name" maxlength="100" class="txt"/>(必填)
				</td>
			</tr>
			<tr>
				<td style="height:35px;">链接地址:</td>
				<td>
					<input name="url" type="text" id="url" maxlength="100" class="txt" />
				</td>
			</tr>
			<tr>
				<td style="height:35px;">LOGO:</td>
				<td>
					<input name="logo" type="text" title="当LOGO为空时，友情链接以文字方式显示" id="logo" maxlength="100" class="txt"/>
				</td>
			</tr>
			<tr>
				<td style="height:35px;">显示顺序:</td>
				<td>
					<input name="displayorder" type="text" title="当显示顺序为0时, 则不显示当前链接" maxlength="8" size="4" id="displayorder" class="txt" value="1" maxlength="4" />(必填)
				</td>
			</tr>
			<tr>
				<td style="height:35px;">说明:</td>
				<td>
					<input name="note" type="text" title="当说明为空时, 友情链接以横排方式显示" id="note" maxlength="200" class="txt" />
				</td>
			</tr>
		</table>
		<div class="Navbutton">
			<button type="button" class="ManagerButton" id="AddNewRec" onclick="javascript:document.form2.submit();"><img src="../images/add.gif" />提 交</button>&nbsp;&nbsp;&nbsp;&nbsp;
			<button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworedit');"><img src="../images/state1.gif"/> 取 消 </button>
		</div>
	</fieldset>
</form>
</div>
</div>
<div id="setting" />
<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<%=footer%>
</body>
</html>