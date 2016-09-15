<%@ Page language="c#" Inherits="BBX.Web.Admin.templatevariable" Codebehind="global_templatevariable.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>模板变量</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />     
<script type="text/javascript" src="../js/common.js"></script>
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
<b>当前模板: <%=Request.Params["templatename"]%></b>    
<cc1:datagrid id="DataGrid1" runat="server" IsFixConlumnControls="true" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid">
	<Columns>
		<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<HeaderStyle Width="20px" />
			<ItemTemplate>
					<input id="delid" onclick="checkedEnabledButton(this.form,'delid','DelRec')" type="checkbox" value="<%# Eval("id").ToString() %>" name="delid" />
					<%# DataGrid1.LoadSelectedCheckBox(Eval("id").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="id" SortExpression="id" HeaderText="ID [递增]" Visible="false" ></asp:BoundColumn>
		<asp:BoundColumn DataField="variablename"  SortExpression="variablename" HeaderText="变量名称"></asp:BoundColumn>
		<asp:BoundColumn DataField="variablevalue" SortExpression="variablevalue" HeaderText="变量值"></asp:BoundColumn>					
	</Columns>
</cc1:datagrid>
<p style="text-align:right;">
	<cc1:Button ID="SaveVar" runat="server" Text="保存变量修改"></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button2" onclick="BOX_show('neworedit');"><img src="../images/add.gif"/> 新建变量 </button>&nbsp;&nbsp;
	<cc1:Button ID="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false"></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" onclick="javascript:window.location.href='global_templatetree.aspx?templateid=<%=Request.Params["templageid"]%>&path=<%=Request.Params["path"]%>&templatename=<%=Request.Params["templatename"]%>';"><img src="../images/arrow_undo.gif" /> 返 回 </button>
</p>
<div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
<div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:350px;">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">添加变量</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">变量名称</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:TextBox id="variablename" runat="server" RequiredFieldType="暂无校验" width="200"></cc2:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">变量值</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:TextBox id="variablevalue" runat="server" RequiredFieldType="暂无校验" width="200"></cc2:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<div class="Navbutton">
	<cc1:Button id="AddNewRec" runat="server" Text=" 提 交 "></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworedit');"><img src="../images/state1.gif"/> 取 消 </button>
</div>
</fieldset>
</div>
</div>
</form>
<div id="setting" />
<%=footer%>
</body>
</html>
