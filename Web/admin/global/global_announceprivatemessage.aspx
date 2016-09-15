<%@ Page Language="c#" Inherits="BBX.Web.Admin.announceprivatemessage" Codebehind="announceprivatemessage.aspx.cs" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>过滤词列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form,'id','DelRec')
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
					<input id="id" onclick="checkedEnabledButton(this.form,'id','DelRec')" type="checkbox" value="<%# Eval("pmid").ToString() %>"	name="id" />
					<%# DataGrid1.LoadSelectedCheckBox(Eval("pmid").ToString())%>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="pmid" SortExpression="pmid" HeaderText="pmid" Visible="false" />
			<asp:BoundColumn DataField="subject" SortExpression="subject" HeaderText="消息标题" readonly="true"/>
			<asp:BoundColumn DataField="message" SortExpression="message" HeaderText="消息内容" readonly="true"/>
			<asp:BoundColumn DataField="postdatetime" SortExpression="postdatetime" HeaderText="发布日期" readonly="true" />
		</Columns>
	</cc1:DataGrid>
	<p style="text-align:right;">
		<button type="button" class="ManagerButton" id="Button2" onclick="BOX_show('neworedit');"><img src="../images/add.gif"/> 新建公共消息 </button>&nbsp;&nbsp;
		<cc1:Button ID="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选的公共消息吗？')) return false;"></cc1:Button>
	</p>
	<div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
	<div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:350px;">
		<div class="ManagerForm">
			<fieldset>
				<legend style="background: url(../images/icons/icon48.jpg) no-repeat 6px 50%;">新建公共消息</legend>
				<table cellspacing="0" cellpadding="4" width="100%" align="center">
					<tr>
						<td style="width: 80px;height:35px;">消息标题:</td>
						<td>
							<cc2:TextBox ID="subject" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="true" MaxLength="254" Size="30"></cc2:TextBox>
						</td>
					</tr>
					<tr>
						<td style="height:100px;">消息内容:</td>
						<td>
							<cc2:TextBox ID="message" runat="server" RequiredFieldType="暂无校验" TextMode="MultiLine" Rows="4" Cols="33"></cc2:TextBox>
						</td>
					</tr>
					<tr>
						<td class="panelbox" colspan="2" align="center" style="height:35px;">
							<cc1:Button ID="AddNewRec" runat="server" Text=" 提 交 "></cc1:Button>&nbsp;&nbsp;
							<button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworedit');"><img src="../images/state1.gif"/> 取 消 </button>
						</td>
					</tr>
				</table>
			</fieldset>
		</div>
	</div>
	<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
</form>
<div id="setting" />
<%=footer%>
</body>
</html>
