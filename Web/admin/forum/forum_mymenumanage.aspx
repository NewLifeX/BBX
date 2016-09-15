<%@ Page language="c#" Codebehind="forum_mymenumanage.aspx.cs" Inherits="BBX.Web.Admin.mymenumanage" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>我的菜单管理</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet"/>
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet"/>        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet"/>
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
function setexample()
{
	$("ehref").innerHTML = $("ahref").value;
	$("eonclick").innerHTML = $("aonclick").value;
	$("etarget").innerHTML = $("atarget").value;
	$("etext").innerHTML = $("atext").value;
}

function Check(form)
{
	CheckAll(form);
	checkedEnabledButton(form,'menuid','DelRec');
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
	<uc1:PageInfo id="info1" runat="server" Icon="Information" Text="&quot;我的菜单&quot;将管理前台首页中&quot;我的&quot;中的自定义项,可以增加、删除、修改"></uc1:PageInfo>
	<cc1:datagrid id="DataGrid1" runat="server" IsFixConlumnControls="true" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid">
		<Columns>
			<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
				<HeaderStyle Width="20px" />
				<ItemTemplate>
					<input id="menuid" type="checkbox" onclick="checkedEnabledButton(this.form,'menuid','DelRec')" value="<%# Eval("menuid").ToString() %>" name="menuid"/>
					<%# DataGrid1.LoadSelectedCheckBox(Eval("menuid").ToString())%>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="menuorder" HeaderText="序号[递增]"></asp:BoundColumn>				
			<asp:BoundColumn DataField="text"  HeaderText="链接文字"></asp:BoundColumn>
			<asp:BoundColumn DataField="href" HeaderText="链接地址"></asp:BoundColumn>
			<asp:BoundColumn DataField="onclick"  HeaderText="单击事件"></asp:BoundColumn>
			<asp:BoundColumn DataField="target"  HeaderText="目标窗口"></asp:BoundColumn>				
		</Columns>
	</cc1:datagrid>
	<p style="text-align:right;">
		<cc1:Button id="SaveMyMenu" runat="server" Text="保存我的菜单修改"></cc1:Button>&nbsp;&nbsp;
		<button type="button" class="ManagerButton" id="Button2" onclick="BOX_show('neworedit');"><img src="../images/add.gif"/> 建立菜单项 </button>&nbsp;&nbsp;
		<cc1:Button id="DelRec" runat="server" Text=" 删 除 " Enabled="false" ButtonImgUrl="../images/del.gif" OnClientClick="if(!confirm('你确认要删除所选菜单项吗？')) return false;"></cc1:Button>
	</p>
	<div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
	<div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:400px;">
	<div class="ManagerForm">
		<fieldset>
		<legend style="background:url(../images/icons/icon53.jpg) no-repeat 6px 50%;">添加我的菜单项</legend>
		<table cellspacing="0" cellpadding="4" width="100%" align="center">
			<tr>
				<td style="width: 70px;height:35px;">链接文字:</td>
				<td>
					<cc1:TextBox runat="server" ID="atext" />
				</td>	
			</tr>
			<tr>
				<td style="height:35px;">单击事件:</td>
				<td>
					<cc1:TextBox runat="server" ID="aonclick" IsReplaceInvertedComma="false" />
				</td>
			</tr>
			<tr>
				<td style="height:35px;">链接地址:</td>
				<td><cc1:TextBox runat="server" ID="ahref" /></td>
			</tr>
			<tr>
				<td style="height:35px;">目标窗口:</td>
				<td>
					<cc1:TextBox runat="server" ID="atarget" Text="_blank" Size="15" />
					<select onchange="document.getElementById('atarget').value=this.value;setexample();">
						<option value=""></option>
						<option value="_blank" selected="selected">新窗口</option>
						<option value="_parent">父窗口</option>
						<option value="_self">当前窗口</option>
						<option value="_top">顶层窗口</option>
					</select>
				</td>
			</tr>
			<tr>
				<td colspan="2" style="height:35px;">
					&nbsp;&nbsp;代码:&lt;a href="<span id="ehref"></span>" onclick="<span id="eonclick"></span>" 
					target="<span id="etarget">_blank</span>"&gt;<span id="etext"></span>&lt;/a&gt;
				</td>
			</tr>
			<tr>
				<td colspan="2" align="center" style="height:35px;">                        
					<cc1:Button id="addmenu" runat="server" Text=" 增 加 " ButtonImgUrl="../images/add.gif"></cc1:Button>&nbsp;&nbsp;
					<button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworedit');"><img src="../images/state1.gif"/> 取 消 </button>
				</td>
			</tr>
		</table>
		<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
		</fieldset>
	</div>
	</div>
</form>
<div id="setting" />
<%=footer%>
</body>
</html>