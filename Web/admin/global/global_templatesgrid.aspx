<%@ Page Language="c#" Inherits="BBX.Web.Admin.templatesgrid" Codebehind="global_templatesgrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>模板列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../../javascript/common.js"></script>
<script type="text/javascript">
	function BatchCreateTemplate(form)
	{
		var pathnamelist = "";
		for (var i = 0; i < form.elements.length; i++)
		{
			var e = form.elements[i];
			if (e.type == "checkbox")
			{
				if (e.checked && e.value != "")
				{
					e.checked = false;
					if(e.name == "chkall")
						continue;
					var tempname = "temp" + e.value;
					if (pathnamelist == "")
					{
						pathnamelist = document.getElementsByName(tempname)[0].value;
					}
					else
					{
						pathnamelist += "," + document.getElementsByName(tempname)[0].value;
					}
				}
			}
		}
		checkedEnabledButton(form, 'templateid', 'IntoDB', 'DelRec', 'DelTemplates');
		if (pathnamelist != "")
			CreateTemplate(pathnamelist);
	}
	
	/*function CreateTemplate(pathnamelist)
	{
		if (confirm("生成" + pathnamelist + "下所有模板的操作非常耗时,确认要继续吗?"))
		{
			var pathnamearry = pathnamelist.split(",");
			for (var i = 0; i < pathnamearry.length; i++)
			{
				aa(pathnamearry[i]);
				document.getElementById('success').style.display = 'block';
				document.getElementById('Layer5').innerHTML = '<br /><table><tr><td valign=top><img border=0 src=../images/ajax_loading.gif  /></td><td valign=middle style=font-size:14px;>正在生成' + pathnamearry[i] + '文件夹下的模板, <br />请稍等...<br /></td></tr></table><br />';
				getReturn('../rapidset/createtemplate.aspx?type=template&templatepath=' + pathnamearry[i] + '&random=1' + Math.random()););
			}
			document.getElementById('success').style.display = 'none';
		}
	}*/

	function CreateTemplate(pathname)
	{
		if (confirm("生成" + pathname + "下所有模板的操作非常耗时,确认要继续吗？"))
		{
			document.getElementById('success').style.display = 'block';
			//document.getElementById('Layer5').style.width='400px';
			document.getElementById('Layer5').innerHTML = '<BR /><table><tr><td valign=top><img border=0 src=../images/ajax_loading.gif  /></td><td valign=middle style=font-size:14px;>正在生成' + pathname + '文件夹下的模板, <BR />请稍等...<BR /></td></tr></table><BR />';
			window.location = "?createtemplate=" + pathname;
		}
	}
	
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form, 'templateid', 'IntoDB', 'DelRec',  'DelTemplates');
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
	<uc1:PageInfo ID="info1" runat="server" Icon="information" Text="<ul><li>模板入库是将模板置为可用状态，让用户可以在前台使用此模板 </li><li>模板出库是将可用状态的模板置为不可用状态，用户在前台将无法继续使用此模板。注意模板出库并非将模板做物理性删除，如果以后想再次使用此模板，可以将其再次入库</li><li>删除模板是将模板做物理性删除，此操作将不可恢复，请慎重使用！(列表的第一项是系统初始化模板,系统不允许删除)</li></ul>" />
	<uc1:PageInfo ID="PageInfo1" runat="server" Icon="information" Text="模板是存放在论坛根下templates文件夹中，模板是以文件夹形式存放，一套模板放在一个文件夹中。模板文件是以htm或config为扩展名，扩展名htm可以下载，扩展名config不可下载。" />
	<cc1:DataGrid ID="DataGrid1" runat="server">
		<Columns>
			<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
				<HeaderStyle Width="20px" />
				<ItemTemplate>
						<input id="templateid" onclick="checkedEnabledButton(this.form,'templateid','IntoDB','DelRec','DelTemplates')" type="checkbox" 
						value="<%# Eval("ID") %>"	name="templateid" />
						<input type="hidden" name="temp<%# Eval("ID") %>" 
						value="<%# Eval("Directory") %>" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="ID" HeaderText="ID [递增]" Visible="false"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="模板名称">
				<itemstyle horizontalalign="Left" />
				<ItemTemplate>
						&nbsp;<span id="<%# Eval("Name") %>" onmouseover="showMenu(this.id, 0, 0, 1, 0);" style="font-weight:bold">
							<%# Eval("Name") %>&nbsp;
							<img src="../images/eye.gif" style="vertical-align:middle" />
						</span>
						<div id="<%# Eval("Name") %>_menu" style="display:none">
							<img src="../../templates/<%# Eval("Name") %>/about.png" onerror="this.src='../../images/common/none.gif'" />
						</div>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="directory" HeaderText="存放路径"></asp:BoundColumn>
			<asp:BoundColumn DataField="copyright" HeaderText="版权"></asp:BoundColumn>
			<asp:BoundColumn DataField="author" HeaderText="作者"></asp:BoundColumn>
			<asp:BoundColumn DataField="createdate" HeaderText="创建日期" ReadOnly="true"></asp:BoundColumn>
			<asp:BoundColumn DataField="ver" HeaderText="模板版本"></asp:BoundColumn>
			<asp:BoundColumn DataField="fordntver" HeaderText="论坛版本" ReadOnly="true"></asp:BoundColumn>
			<asp:BoundColumn DataField="Width" HeaderText="宽度" ReadOnly="true"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="已入库">
				<ItemTemplate>
					<asp:Label id=Label1 runat="server" Text='<%# Eval("Enable") %>'></asp:Label>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="" HeaderStyle-Width="60px">
				<ItemTemplate>
					<asp:Label id=Label2 runat="server" Text='<%# CreateStr(Eval("Name")+"", Eval("Directory")+"",Eval("ID")+"") %>'></asp:Label>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</cc1:DataGrid><br />
	<p style="text-align:right;">
		<cc1:Button ID="IntoDB" runat="server" Text=" 入 库 " Enabled="false" ScriptContent="document.getElementById('Layer5').innerHTML='<BR /><table><tr><td valign=top><img border=0 src=../images/ajax_loading.gif  /></td><td valign=middle style=font-size:14px;>选中模板正在入库, 请稍等...<BR /></td></tr></table><BR />';">
		</cc1:Button>&nbsp;&nbsp;
		<cc1:Button ID="DelRec" runat="server" Text=" 出 库 " Enabled="false" ButtonImgUrl="../images/del.gif"
			ScriptContent="document.getElementById('Layer5').innerHTML='<BR /><table><tr><td valign=top><img border=0 src=../images/ajax_loading.gif  /></td><td valign=middle style=font-size:14px;>选中模板正在出库, 请稍等...<BR /></td></tr></table><BR />';">
		</cc1:Button>&nbsp;&nbsp;
		<!--<span><button type="button" class="ManagerButton" id="CreateTemplates" disabled="true" onclick="BatchCreateTemplate(this.form)"><img src="../images/submit.gif"/> 批量生成 </button></span>&nbsp;&nbsp;-->
		<cc1:Button ID="DelTemplates" runat="server" Text=" 删 除 " Enabled="false" ButtonImgUrl="../images/state1.gif"
			OnClick="DelTemplates_Click" OnClientClick="if(!confirm('你确认要删除所选模板吗？\n删除后将不能恢复！')) return false;" ScriptContent="document.getElementById('Layer5').innerHTML='<BR /><table><tr><td valign=top><img border=0 src=../images/ajax_loading.gif  /></td><td valign=middle style=font-size:14px;>正在删除选中模板, 请稍等...<BR /></td></tr></table><BR />';">
		</cc1:Button>
	</p>
</form>
<%=footer%>
</body>
</html>