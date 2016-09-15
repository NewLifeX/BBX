<%@ Page Language="C#" CodeBehind="forum_smiliemanage.aspx.cs" Inherits="BBX.Web.Admin.smiliemanage" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>论坛表情管理</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />			
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" /> 
<script type="text/javascript">
	function validate()
	{
		var str = ",<%=ViewState["dir"]%>";
		var count = 2;
		while(true)
		{
			if(	document.getElementById("id" + count) != null)
			{
				for(var i = count - 1; i >= 1; i--)
				{
					if(!document.getElementById("id" + i).checked) continue;
					if(!document.getElementById("id" + count).checked) continue;
					if(document.getElementById("group" + i).value == document.getElementById("group" + count).value)
					{
						Message("第" + i + "行的表情组名与第" + count + "行相同");
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
				if(document.getElementById("group" + i).value == "")
				{
					Message("第" + i + "行的表情组名为空！");
					document.getElementById("group" + i).focus();
					return;
				}
				if(str.indexOf("," + document.getElementById("group" + i).value + ",") >= 0)
				{
					Message("第" + i + "行的表情组名与原有的表情组名相同");
					return false;
				}
				if(isNaNEx(document.getElementById("order" + i).value))
				{
					Message("第" + i + "行为非零数字");
					return false;
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
	
	function isNaNEx(str)
	{
		return !(/^\d+$/.test(str));
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
	function CheckAllForm(form)
	{
		for (var i=0;i<form.elements.length;i++)
		{
		var e = form.elements[i];
		if (e.name == 'id')
		   e.checked = form.chkall.checked;
		}
	}
	
	function Check(form)
	{
		CheckAllForm(form);
		checkedEnabledButton(form,'id','DelRec');
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>	
<form id="Form1" method="post" runat="server">
<uc1:PageInfo id="info1" runat="server" Icon="Information" Text="<ul><li>把表情图片文件夹上传到 editor/images/smilies 目录，就可以在最下方的列表中看见添加选项。</li><li>请至少保留一组表情！</li></ul>"></uc1:PageInfo>
<cc1:datagrid id="smilesgrid" runat="server" IsFixConlumnControls="true">
	<Columns>
		<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<HeaderStyle Width="20px" />
			<ItemTemplate>
				<input id="id" type="checkbox" onclick="checkedEnabledButton(this.form,'id','DelRec')" value="<%# Eval("id").ToString() %>" name="id" />
				<%# smilesgrid.LoadSelectedCheckBox(Eval("id").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="ID" SortExpression="id" HeaderText="Id" Visible="false"></asp:BoundColumn>
		<asp:BoundColumn DataField="code" SortExpression="code" HeaderText="表情组名称"></asp:BoundColumn>
		<asp:BoundColumn DataField="displayorder" SortExpression="displayorder" HeaderText="显示顺序" ></asp:BoundColumn>
		<asp:BoundColumn DataField="url" SortExpression="url" HeaderText="图片路径" Readonly="true"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="操作">
			<ItemTemplate>
				<a href="forum_smilegrid.aspx?typeid=<%# Eval("id").ToString() %>">管理此表情分类</a>
			</ItemTemplate>
		</asp:TemplateColumn>		
	</Columns>
</cc1:datagrid><br />
<p style="text-align:right;">
	<cc1:Button id="SaveSmiles" runat="server" Text="保存表情修改"></cc1:Button>&nbsp;&nbsp;
	<cc1:Button id="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" OnClick="DelRec_Click" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选的表情分类吗？')) return false;"></cc1:Button>
</p>
<div class="ManagerForm">
<fieldset>
	<legend style="background:url(../images/icons/icon49.jpg) no-repeat 6px 50%;">增加现有表情组</legend>
	<table class="ntcplist" >
	<tr>
	<td>
	<table class="datalist" cellspacing="0" rules="all" border="1" id="Table1" style="border-collapse:collapse;">
		<tr class="category">
		  <td nowrap="nowrap" style="border: 1px solid rgb(234, 233, 225); width: 20px;">
			<input type="checkbox" name="cfile" onclick="checkFileList(this.form)" />
		   </td>
		  <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">表情组名称</td>
		  <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">显示顺序</td>
		  <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">图片路径</td>
		</tr>
		<asp:Literal ID="dirinfoList" Runat="server" />
		<tr>
			<td colspan="4" align="center" style="padding:10px 0;">
				<cc1:Button id="SubmitButton" runat="server" Text=" 提 交 " ValidateForm="true"></cc1:Button>
			</td>
		</tr>
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