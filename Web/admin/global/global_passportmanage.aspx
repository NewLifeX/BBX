<%@ Page Language="c#" AutoEventWireup="true" CodeBehind="global_passportmanage.aspx.cs" Inherits="BBX.Web.Admin.global_passportmanage" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>整合程序设置</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../js/AjaxHelper.js"></script>
<script type="text/javascript">
function setAllowPassport(thevalue)
{
	var url = "allowpassport=" + thevalue;
	var result = getReturn('global_ajaxcall.aspx?opname=setapp&' + url);
	if(result == "ok")
	{
		$("passportbody").style.display = thevalue == "1" ? "block" : "none";
	}
}

function Check(form)
{
	CheckAll(form);
	checkedEnabledButton(form,'apikey','DelRec');
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="form1" runat="server">
<table width="100%">
	<tr><td class="item_title">启用整合程序</td></tr>
	<tr>
		<td class="vtop">
			<cc1:RadioButtonList ID="allowpassport" runat="server" RepeatColumns="2">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0" Selected="true">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
	</tr>
</table>
<div id="passportbody" runat="server">
	<uc1:PageInfo id="info1" runat="server" Icon="Information" Text="请妥善保管API Key和密钥，否则会带来严重的安全隐患，在真实环境中请务必配置IP地址访问限制"></uc1:PageInfo>
	<cc1:DataGrid ID="DataGrid1" runat="server">
		<Columns>
			<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
				<HeaderStyle Width="20px" />
				<ItemTemplate>
					<input id="apikey" onclick="checkedEnabledButton(this.form,'apikey','DelRec')" type="checkbox" value="<%# Eval("apikey").ToString() %>" name="apikey" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="">
				<ItemTemplate>
					<a href="global_passportsetting.aspx?apikey=<%# Eval("apikey").ToString()%>">编辑</a>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="appname" HeaderText="应用程序名称" />
			<asp:BoundColumn DataField="apptype" HeaderText="应用程序类型" />
			<asp:BoundColumn DataField="callbackurl" HeaderText="返回地址" />
			<asp:BoundColumn DataField="apikey" HeaderText="API Key" />
			<asp:BoundColumn DataField="secret" HeaderText="密钥" />
		</Columns>
	</cc1:DataGrid>
	<p style="text-align:right;">
		<cc1:Button ID="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" OnClick="DelRec_Click" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选的整合程序吗？')) return false;"></cc1:Button>&nbsp;&nbsp;
		<button type="button" class="ManagerButton" onclick="javascript:window.location.href='global_passportsetting.aspx';">
			<img src="../images/add.gif" />添加整合程序设置
		</button>
	</p>
</div>
</form>
<%=footer%>
</body>
</html>