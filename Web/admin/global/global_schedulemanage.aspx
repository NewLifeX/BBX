<%@ Page Language="c#" CodeBehind="global_schedulemanage.aspx.cs" Inherits="BBX.Web.Admin.global_schedulemanage" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>整合程序设置</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="form1" runat="server">
<uc1:PageInfo id="info1" runat="server" Icon="information"
Text="计划任务分为系统级和非系统级两种.系统级计划任务不可禁用;添加的计划任务均为非系统级,可以设置开启或禁用."></uc1:PageInfo>
<uc1:PageInfo id="PageInfo1" runat="server" Icon="warning"
Text="请勿随意添加计划任务,此功能适用于开发人员。"></uc1:PageInfo>
<div  id="passportbody" runat="server">
<table class="ntcplist" >
<tr class="head">
<td>计划任务列表</td>
</tr>
<tr>
<td>
	<asp:DataGrid ID="DataGrid1" runat="server" 
		onitemcommand="DataGrid1_ItemCommand">
		<Columns>
			<asp:BoundColumn DataField="key" HeaderText="计划任务名称"></asp:BoundColumn>
			<asp:BoundColumn DataField="exetime" HeaderText="执行方式"></asp:BoundColumn>
			<asp:BoundColumn DataField="lastexecute" HeaderText="上次执行时间"></asp:BoundColumn>
			<asp:BoundColumn DataField="enable" HeaderText="状态"></asp:BoundColumn>
			<asp:BoundColumn DataField="issystemevent" HeaderText="级别"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="">
				<ItemTemplate>
						<a href="global_schedulesetting.aspx?keyid=<%# Eval("key").ToString()%>">编辑</a>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="">
				<ItemTemplate>
						<asp:LinkButton runat="server" ID="button" CommandName="exec" CommandArgument='<%# Eval("key").ToString()%>'>立即执行</asp:LinkButton>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
	</td></tr></TABLE><BR />
	<p style="text-align:right;">
		<button type="button" class="ManagerButton" onclick="javascript:window.location.href='global_schedulesetting.aspx';">
			<img src="../images/add.gif" />添加计划任务
		</button>
	</p>
</div>
</form>
<%=footer%>
</body>
</html>