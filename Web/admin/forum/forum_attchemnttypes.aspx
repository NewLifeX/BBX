<%@ Page Language="c#" Inherits="BBX.Web.Admin.forum_attchemnttypes" Codebehind="forum_attchemnttypes.aspx.cs"%>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>附件类型管理</title>
<link href="../styles/tab.css" type="text/css" rel="stylesheet" />
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script> 
<script type="text/javascript">    
	
	function getCheckboxId(thevalue)
	{
		var labels = document.getElementsByTagName("label");
		for(var i = 0 ; i < labels.length ; i++)
		{
			if(labels[i].childNodes[0].nodeValue.toLowerCase() == thevalue.toLowerCase())
			{
				return labels[i].getAttributeNode("for").value;
			}
		}
		return "";
	}
	
	function enabledCheckbox()
	{
		var i = 1;
		var extname = "";
		while(true)
		{
			var thetype = eval("atttype.type" + i);
			if(thetype == undefined)
				break;
			if(extname == "")
			{
				extname = thetype.extname;
			}
			else
			{
				if(thetype.extname != "")
				{
					extname += "," + thetype.extname;
				}
			}
			i++;
		}
		if(extname != "")
			extname = "," + extname + ",";
		var labels = document.getElementsByTagName("label");
		for(var i = 0 ; i < labels.length ; i++)
		{
			var type = labels[i].firstChild.nodeValue;
			if(extname.indexOf("," + type + ",") == -1)
			{
				var id = getCheckboxId(type);
				document.getElementById(id).disabled = false;
				document.getElementById(id).checked = false;
			}
		}
	}
	
	function disabledCheckbox()
	{        
		var labels = document.getElementsByTagName("label");
		for(var i = 0 ; i < labels.length ; i++)
		{
			var elemid = labels[i].getAttributeNode("for").value;
			document.getElementById(elemid).disabled = true;
			document.getElementById(elemid).checked = false;
		}
	}
	
	function checkedCheckbox(elemid,checked)
	{
		document.getElementById(elemid).disabled = document.getElementById(elemid).parentNode.disabled = !checked;
		document.getElementById(elemid).checked = checked;
	}
	
	function newAttType()
	{
		document.getElementById("typename").value = "";
		document.getElementById("atttypeid").value = "";
		disabledCheckbox();
		enabledCheckbox();
		BOX_show('neworedit');
	}

	function editAttType(typeid)
	{
		var thetype = eval("atttype.type" + typeid);
		extname = thetype.extname.split(",");
		document.getElementById("typename").value = thetype.typename;
		document.getElementById("atttypeid").value = typeid;
		disabledCheckbox();
		enabledCheckbox();
		for(var i = 0 ; i < extname.length ; i++)
		{
			elemid = getCheckboxId(extname[i]);
			if(elemid != "")
			{
				checkedCheckbox(elemid,true);
			}
		}
		BOX_show('neworedit');
	}
	
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form,'typeid','DelRec')
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<div id="TabControl1_Tab" class="tabs"><input type="hidden" value="TabControl1:tabPage51" id="TabControl1" name="TabControl1">
	<ul>		
		<li class="TabSelect" ><a href="../global/global_attach.aspx">附件设置</a></li>
		<li class="TabSelect"><a href="../global/global_ftpsetting.aspx?ftptype=forumattach">远程附件</a></li>
		<li class="CurrentTabSelect" ><a href="forum_attchemnttypes.aspx" class="current" >附件类型</a></li>
		<li class="TabSelect" ><a href="forum_attachtypesgrid.aspx">附件尺寸</a></li>
	</ul>
</div>
<div class="tabarea" id="TabControl1tabarea" style="display: block;">
<cc1:DataGrid ID="DataGrid1" runat="server">
	<Columns>
		<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<HeaderStyle Width="20px" />
			<ItemTemplate>
				<input id="typeid" type="checkbox" onclick="checkedEnabledButton(this.form,'typeid','DelRec')" value="<%# Eval("typeid").ToString() %>"	name="typeid" />
				<%# DataGrid1.LoadSelectedCheckBox(Eval("typeid").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemStyle width="5%" />
			<ItemTemplate>
				<a href="javascript:editAttType(<%# Eval("typeid").ToString() %>);">编辑</a>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="typeid" HeaderText="id [递增]" Visible="false"></asp:BoundColumn>
		<asp:BoundColumn DataField="typename" HeaderText="类型名称"></asp:BoundColumn>
		<asp:BoundColumn DataField="extname" HeaderText="扩展名列表" readonly="true"></asp:BoundColumn>
	</Columns>
</cc1:DataGrid>
<p style="text-align:right;">
	<cc1:Button ID="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选的附件分类吗？')) return false;"></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button2" onclick="newAttType()"><img src="../images/add.gif"/> 添加附件分类 </button>
</p>
<div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
<div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:380px;">
<div class="ManagerForm">
<fieldset>
<legend style="background: url(../images/icons/icon59.jpg) no-repeat 6px 50%;">添加/编辑 附件分类</legend>
<table width="100%">
	<tr><td class="item_title">分类名称</td></tr>
	<tr>
		<td class="vtop">
			<cc2:TextBox ID="typename" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="true" MaxLength="254" Size="30"></cc2:TextBox>
            <input type="hidden" name="atttypeid" id="atttypeid" />
		</td>
	</tr>
	<tr><td class="item_title">分类绑定</td></tr>
	<tr>
		<td class="vtop">
		<cc2:CheckBoxList id="attachextensions" runat="server" RepeatColumns="4"></cc2:CheckBoxList><br/>论坛上允许使用的附件类型,灰色的表示已经加入某一附件分类,可以通过编辑某分类将某一类型从中删除,使其状态成为未分类后再加入其它分类中
		</td>
	</tr>
</table>
<div class="Navbutton">
	<cc1:Button ID="AddNewRec" runat="server" Text=" 提 交 "></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworedit');"><img src="../images/state1.gif"/> 取 消 </button>
</div>
</fieldset>
</div>
</div>
<cc2:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc2:Hint>
</div>
</div>
</form>
<div id="setting" />
<%=footer%>
</body>
</html>