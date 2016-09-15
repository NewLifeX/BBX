<%@ Page Language="C#" CodeBehind="global_navigationmanage.aspx.cs" Inherits="BBX.Web.Admin.global_navigationmanage" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>导航菜单管理</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
	function newMenu()
	{
		$("opt").innerHTML = "新建导航菜单";
		$("menuid").value = "0";
		$("mode").value = "new";
		$("name").value = "";
		$("title").value = "";
		$("url").value = "";
		$("target").options[0].selected = true;
		$("menutype").value = "0";
		$("available").options[1].selected = true;
		$("displayorder").value = "";
		$("level").options[0].selected = true;
		BOX_show('neworeditmainmenu');
	}
	function editMenu(menuid)
	{
		$("opt").innerHTML = "编辑导航菜单";
		for(var i = 0; i < nav.length; i++)
		{
			if(nav[i]["id"] == menuid)
			{
				$("menuid").value = nav[i]["id"];
				$("mode").value = "edit";                    
				$("name").value = nav[i]["name"];
				$("title").value = nav[i]["title"];
				$("url").value = nav[i]["url"];
				$("target").options[nav[i]["target"]].selected = true;
				$("menutype").value = nav[i]["type"];
				$("available").options[nav[i]["available"]].selected = true;
				$("displayorder").value = nav[i]["displayorder"];
				$("level").options[nav[i]["level"]].selected = true;
				BOX_show('neworeditmainmenu');
				return;
			}
		}
		alert("菜单不存在！");            
	}
	function chkSubmit()
	{
		if($("name").value == "")
		{
			alert("菜单名称不能为空！");
			$("name").focus();
			return false;
		}
		var url = $("url").value.toLowerCase();
		if (url == "")
		{
			if (!confirm("您确认要将链接地址置空吗？"))
			{
				$("url").focus();
				return false;
			}
			else
			{
				$("url").value = "#";
			}
		}
		else
		{
			if ($("menutype").value != "0" && url.indexOf("://") == -1)
			{
				if (url.indexOf("javascript:") == -1 && url != "#")
				{
					if (!confirm("您的链接地址不包含“http://”，您确认要保持这样的链接吗？"))
					{
						$("url").focus();
						return false;
					}
				}
			}
		}
		if($("displayorder").value == "")
		{
			$("displayorder").value = "0";
		}
		else if(!/^\d+$/.test($("displayorder").value))
		{
			alert("序号必须是数字！");
			$("displayorder").value = "";
			$("displayorder").focus();
			return false;
		}
		$("form1").submit();
		return true;
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="form1" runat="server">
	<uc1:PageInfo ID="info1" runat="server" Icon="information" Text="<li>主菜单项必须在其下没有子菜单时才可删除!</li>" />
	<cc1:datagrid id="DataGrid1" runat="server" IsFixConlumnControls="true" OnItemDataBound="DataGrid1_ItemDataBound">
	   <Columns>
		<asp:BoundColumn DataField="displayorder" HeaderText="显示序号"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="菜单名称">
			<ItemTemplate>
			    <input id="keyid" type="hidden" value="<%# Eval("id").ToString() %>" name="keyid"/>
				<a href="<%# GetLink(Eval("url").ToString()) %>" target="_blank" title="<%# Eval("title").ToString() %>"><%# Eval("name").ToString() %></a>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="url" HeaderText="页面地址"><ItemStyle HorizontalAlign="left" /></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="系统菜单">
			<ItemTemplate>
				<%# Eval("type").ToString() == "0" ? "系统" : "自定义" %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="展开目标">
			<ItemTemplate>
				<%# Eval("target").ToString() == "0" ? "本窗口" : "新窗口"%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="是否可用">
			<ItemTemplate>
				<%# Eval("available").ToString() == "0" ? "不可用" : "可用"%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="可见度">
			<ItemTemplate>
				<%# GetLevel(Eval("level").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="操作">
			<ItemTemplate>
				<a href="javascript:;" onclick="editMenu('<%# Eval("id").ToString() %>');">编辑</a>&nbsp;
				<%# GetSubNavMenuManage(Eval("id").ToString(), Eval("type").ToString())%>&nbsp;
				<%# GetDeleteLink(Eval("id").ToString(),Eval("type").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>
	  </Columns>
	</cc1:datagrid>
	<p style="text-align:right;">
	    <cc1:Button id="saveNav" runat="server" Text="保存" OnClick="saveNav_Click"></cc1:Button>
		<button type="button" class="ManagerButton" id="Button2" onclick="newMenu();"><img src="../images/add.gif"/> 新 建 </button>&nbsp;
		<button type="button" class="ManagerButton" id="returnbutton" onclick="window.location=location.href.replace(location.search,'');" runat="server"><img src="../images/arrow_undo.gif"/>返回上级菜单</button>
	</p>
	<div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
	<div id="neworeditmainmenu" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:400px;">
		<div class="ManagerForm">
			<fieldset>
			<legend id="opt" style="background:url(../images/icons/icon53.jpg) no-repeat 6px 50%;">新建导航菜单</legend>
			<table cellspacing="0" cellPadding="4" class="tabledatagrid" width="80%">
				<tr>
					<td width="30%" height="30px">
						菜单名称:
						<input type="hidden" id="menuid" name="menuid" value="0" />
						<input type="hidden" id="mode" name="mode" value="" />
						<input type="hidden" id="menutype" name="menutype" value="" />
					</td>
					<td width="70%"><input id="name"  name="name" type="text" maxlength="50" size="30"class="FormBase" onfocus="this.className='FormFocus';" onblur="this.className='FormBase';" /></td>
				</tr>
				<tr>
					<td height="30px">菜单提示:</td>
					<td><input id="title"  name="title" type="text" maxlength="255" size="30"class="FormBase" onfocus="this.className='FormFocus';" onblur="this.className='FormBase';" /></td>
				</tr>
				<tr>
					<td height="30px">链接地址:</td>
					<td><input id="url" name="url" type="text" maxlength="255" size="30" class="FormBase" onfocus="this.className='FormFocus';" onblur="this.className='FormBase';" /></td>
				</tr>
				<tr>
					<td height="30px">展开目标:</td>
					<td>
						<select name="target" id="target">
							<option value="0">本窗口</option>
							<option value="1">新窗口</option>
						</select>
					</td>
				</tr>
				<tr>
					<td height="30px">是否可用:</td>
					<td>
						<select name="available" id="available">
							<option value="0">不可用</option>
							<option value="1" selected="selected">可用</option>
						</select>
					</td>
				</tr>
				<tr>
					<td height="30px">显示序号:</td>
					<td><input id="displayorder" name="displayorder" type="text" maxlength="6" size="6"class="FormBase" onfocus="this.className='FormFocus';" onblur="this.className='FormBase';" /></td>
				</tr>
				<tr>
					<td height="30px">可见度:</td>
					<td>
						<select name="level" id="level">
							<option value="0" selected="selected">游客</option>
							<option value="1">会员</option>
							<option value="2">版主</option>
							<option value="3">管理员</option>
						</select>
					</td>
				</tr>
				<tr>
					<td colspan="2" height="30px" align="center">
						<button type="button" class="ManagerButton" id="AddNewRec" onclick="chkSubmit();"><img src="../images/add.gif"/> 提 交 </button>&nbsp;&nbsp;
						<button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworeditmainmenu');"><img src="../images/state1.gif"/> 取 消 </button>
					</td>
				</tr>
			</table>
			</fieldset>
		</div>
	</div>
</form>
<div id="setting" />
<%=footer%>
</body>
</html>