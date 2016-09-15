<%@ Page language="c#" Inherits="BBX.Web.Admin.identifymanage" Codebehind="forum_identifymanage.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Import NameSpace="BBX.Common"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
	<title>论坛图标文件列表</title>		
	<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
	<script type="text/javascript" src="../js/common.js"></script>
	<script type="text/javascript" src="../../javascript/common.js"></script>
	<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />  
	<script type="text/javascript">
	function validate()
	{
		var str = ",<%=ViewState["code"]%>";
		var count = 2;
		while(true)
		{
			if(	document.getElementById("id" + count) != null)
			{
				for(var i = count - 1; i >= 1; i--)
				{
					if(!document.getElementById("id" + i).checked) continue;
					if(!document.getElementById("id" + count).checked) continue;
					if(document.getElementById("name" + i).value == document.getElementById("name" + count).value)
					{
						Message("第" + i + "行的代码名称与第" + count + "行相同");
						return false;
					}
				}
				count++;
			}
			else
				break;
		}
		for(var i = 1;;i++)
		{
			if(document.getElementById("id" + i) == null)
			{
				Message("没有要提交的数据!");
				return false;
			}
			if(document.getElementById("id" + i).checked) break;
		}
		for(var i = 1;; i++)
		{
			if(document.getElementById("id" + i) == null)
				return true;
			else
			{
				if(document.getElementById("name" + i).value == "")
				{
					Message("第" + i + "行的代码名称为空！");
					document.getElementById("name" + i).focus();
					return;
				}
				if(document.getElementById("id" + i).checked)
				{
					if(str.indexOf("," + document.getElementById("name" + i).value + ",") >= 0)
					{
						Message("第" + i + "行的代码名称与原有的代码名称相同");
						return false;
					}
				}
			}
		}
	}
	
	function Message(m)
	{
		document.getElementById("success").style.display = 'none';
		document.getElementById("SubmitButton").disabled = false;
		alert(m);
	}
	
	function CheckSelect(form)
	{
		for (var i=0;i<form.elements.length;i++)
		{
			var e = form.elements[i];
			if (e.name != 'chkall' && e.name =='id')
			e.checked = form.chkall.checked;
		}
	}
	
	function checkFileList(form)
	{
		var i = 1;
		while(true)
		{
			if(form.elements["id" + i] == null)
				break;
			form.elements["id" + i].checked = form.cfile.checked;
			i++;
		}
	}
	
	function Check(form)
	{
		CheckSelect(form);
		checkedEnabledButton(form,'id','DelRec');
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div id="append_parent"></div>
<form id="Form1" method="post" runat="server">
<cc1:datagrid id="identifygrid" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" IsFixConlumnControls="true" pagesize="10">
	<Columns>
		<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<HeaderStyle Width="20px" />
			<ItemTemplate>
				<input id="id" type="checkbox" onclick="checkedEnabledButton(this.form,'id','DelRec')" value="<%# Eval("ID")%>" name="id" />
				<%# identifygrid.LoadSelectedCheckBox(Eval("ID")+"")%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="name" HeaderText="名称"><HeaderStyle width="80%" /></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="图片">
			<ItemTemplate>
				<div id="layer<%# Eval("ID")%>_a" onmouseover="showMenu(this.id,false)">
					<%# PicStr(Eval("filename").ToString(),20) %>
				</div>
				<div id="layer<%# Eval("ID")%>_a_menu" style="display:none">
					<%# PicStr(Eval("filename").ToString())%>
				</div>
			</ItemTemplate>
		</asp:TemplateColumn>			
	</Columns>
</cc1:datagrid>
<p style="text-align:right;">
	<cc1:Button id="EditIdentify" runat="server" Text=" 保存鉴定修改 "></cc1:Button>&nbsp;&nbsp;
	<cc1:Button id="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选的鉴定吗？')) return false;"></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" onclick="javascript:window.location.href='forum_addidentify.aspx';"><img src="../images/add.gif" /> 上传鉴定</button>
</p>
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/icon49.jpg) no-repeat 6px 50%;">增加现有鉴定</legend>
<table class="ntcplist" >
<tr>
	<td>
		<table class="datalist" cellspacing="0" cellpadding="3" rules="rows" border="0" width="100%">
			<tr class="category">
			  <td width="8%"><input type="checkbox" name="cfile" onclick="checkFileList(this.form)"></td>
			  <td width="82%" align="left">名称</td>
			  <td width="10%">图片</td>
			</tr>
			<asp:Literal ID="fileinfoList" Runat="server" />
			<tr><td align="center" colspan="3"><br /><cc1:Button id="SubmitButton" runat="server" Text=" 提 交 " ValidateForm="true"></cc1:Button></td></tr>
		</table>
	</td>
</tr>
</table>
</fieldset>
</div>
</form>
<%=footer%>
</body>
</html>