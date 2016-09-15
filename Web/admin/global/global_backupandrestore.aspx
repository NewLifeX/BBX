<%@ Page Language="c#" Inherits="BBX.Web.Admin.backupandrestore" Codebehind="global_backupandrestore.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>备份恢复数据库</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/calendar.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form,'id','DeleteBackup');
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<br />
<cc2:DataGrid ID="Grid1" runat="server" ColumnSpan="4">
	<Columns>
		<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<headerstyle width="5%" />
			<itemtemplate>
				<input id="id" onclick="checkedEnabledButton(this.form,'id','DeleteBackup')" type="checkbox" value="<%# Eval("id").ToString() %>" name="id" />
			</itemtemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="id" HeaderText="id [递增不重复]" Visible="false"></asp:BoundColumn>
		<asp:BoundColumn DataField="filename" HeaderText="文件名">
			<headerstyle width="25%" />
		</asp:BoundColumn>
		<asp:BoundColumn DataField="createtime" HeaderText="创建日期">
			<headerstyle width="20%" />
		</asp:BoundColumn>
		<asp:BoundColumn DataField="fullname" HeaderText="文件路径">
			<headerstyle width="30%" />
		</asp:BoundColumn>
	</Columns>
</cc2:DataGrid>
<p style="text-align:right;">
	<cc1:Button ID="Restore" runat="server" Text="开始数据库恢复" Visible="false"></cc1:Button>
	<button type="button" class="ManagerButton" id="Button2" onclick="BOX_show('neworedit');"><img src="../images/add.gif"/> 建立备份 </button>&nbsp;&nbsp;
	<cc1:Button ID="DeleteBackup" runat="server" Text="删除备份" ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选的数据库备份吗？')) return false;"></cc1:Button>
</p>
<div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
<div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:400px;">
<fieldset>
	<legend style="background: url(../images/icons/icon39.jpg) no-repeat 6px 50%;">数据库备份</legend>

	<table cellspacing="0" cellpadding="4" width="100%" align="center">
		<tr>
			<td style="width: 70px;height:35px;">服务器名称:</td>
			<td>
				<cc1:TextBox ID="ServerName" runat="server" RequiredFieldType="暂无校验" CanBeNull="必填" Text=""></cc1:TextBox>
			</td>
		</tr>
		<tr>
			<td style="height:35px;">用户名:</td>
			<td>
				<cc1:TextBox ID="UserName" runat="server" RequiredFieldType="暂无校验" CanBeNull="必填" Text=""></cc1:TextBox>
			</td>
		</tr>
		<tr>
			<td style="height:35px;">备份的名称:</td>
			<td>
				<cc1:TextBox ID="backupname" runat="server" HintTitle="提示" HintInfo="系统会自动将扩展名定义为.config"
					RequiredFieldType="暂无校验" CanBeNull="可为空" Width="150" Text=""></cc1:TextBox>空格会被"_"代替
			</td>
		</tr>
		<tr>
			<td style="height:35px;">数据库名称:</td>
			<td>
				<cc1:TextBox ID="strDbName" runat="server" RequiredFieldType="暂无校验" CanBeNull="必填" Text=""></cc1:TextBox>
			</td>
		</tr>
		<tr>
			<td style="height:35px;">密码:</td>
			<td>
				<cc1:TextBox ID="Password" runat="server" RequiredFieldType="暂无校验" Text="" TextMode="Password"></cc1:TextBox>
			</td>
		</tr>
		<tr>
			<td colspan="2" align="center" style="height:35px;">
				<cc1:Button ID="BackUP" runat="server" Text="开始备份数据库"></cc1:Button>&nbsp;&nbsp;
				<button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworedit');"><img src="../images/state1.gif"/> 取 消 </button>
			</td>
		</tr>
	</table>
</fieldset>
</div>
</form>
</div>
<div id="setting" />
<%=footer%>
</body>
</html>