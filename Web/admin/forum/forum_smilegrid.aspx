<%@ Page language="c#" Inherits="BBX.Web.Admin.smilegrid" Codebehind="forum_smilegrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<%@ Import NameSpace="BBX.Common"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>论坛图标文件列表</title>		
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />		
<script type="text/javascript" src="../js/common.js"></script>
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
				if(document.getElementById("code" + i).value == document.getElementById("code" + count).value)
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
			if(document.getElementById("code" + i).value == "")
			{
				Message("第" + i + "行的代码名称为空！");
				document.getElementById("code" + i).focus();
				return;
			}
			if(document.getElementById("id" + i).checked)
			{
				if(str.indexOf("," + document.getElementById("code" + i).value + ",") >= 0)
				{
					Message("第" + i + "行的代码名称与原有的代码名称相同");
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
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server"><br>
<uc1:PageInfo id="info1" runat="server" Icon="Information"
Text="显示顺序须为整数, 代码和文件名最大长度将为30. "></uc1:PageInfo>

<cc1:datagrid id="smilesgrid" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid" IsFixConlumnControls="true" pagesize="10">
	<Columns>				
		<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='CheckSelect(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<HeaderStyle Width="20px" />
			<ItemTemplate>
				<input id="id" onclick="checkedEnabledButton(this.form,'id','DelRec')" type="checkbox" value="<%# Eval("id").ToString()%>" name="id" />
				<%# smilesgrid.LoadSelectedCheckBox(Eval("id").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="ID" SortExpression="id" HeaderText="图标id" Visible="false" ></asp:BoundColumn>
		<asp:BoundColumn DataField="type" SortExpression="type" HeaderText="类型" Visible="false"></asp:BoundColumn>
		<asp:BoundColumn DataField="code" SortExpression="code" HeaderText="代码" ><headerstyle width="20%"/></asp:BoundColumn>
		<asp:BoundColumn DataField="displayorder" SortExpression="displayorder" HeaderText="显示顺序"><headerstyle width="20%"/></asp:BoundColumn>
		<asp:BoundColumn DataField="url" SortExpression="url" HeaderText="文件名"><headerstyle width="40%"/></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="图片">
			<ItemTemplate>
				<asp:Label id=Label4 runat="server" Text='<%# PicStr(Eval("url").ToString()) %>'>
				</asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</cc1:datagrid>
<p style="text-align:right;">
	<cc1:Button id="EditSmile" runat="server" Text="保存表情信息修改"></cc1:Button>&nbsp;&nbsp;
	<cc1:Button id="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选的表情码吗？')) return false;"></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" onclick="javascript:window.location.href='forum_addsmile.aspx?typeid=<%=DNTRequest.GetInt("typeid",0)%>';">
		<img src="../images/add.gif" /> 上传表情
	</button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button3" onclick="window.location='forum_smiliemanage.aspx';"><img src="../images/arrow_undo.gif"/> 返 回 </button>
</p>
<div class="ManagerForm">
<fieldset>
	<legend style="background:url(../images/icons/icon49.jpg) no-repeat 6px 50%;">增加现有表情</legend>
	<table class="ntcplist" >
	<tr>
	<td>
	<table class="datalist" cellspacing="0" rules="all" border="1" id="Table1" style="border-collapse:collapse;">
		<tr class="category">
		  <td width="5%" nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">
			<input type="checkbox" name="cfile" onclick="checkFileList(this.form)"/>
		  </td>
		  <td width="25%" nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">代码</td>
		  <td width="25%" nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">显示顺序</td>
		  <td width="35%" nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">文件名</td>
		  <td width="10%" nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">图片</td>
		</tr>
		<asp:Literal ID="fileinfoList" Runat="server" />
		<tr><td align="center" colspan="5"><br /><cc1:Button id="SubmitButton" runat="server" Text=" 提 交 " ValidateForm="true"></cc1:Button></td></tr>
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