<%@ Page language="c#" Inherits="BBX.Web.Admin.moderatormanagelog" Codebehind="forum_moderatormanagelog.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc4" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>管理日志</title>
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
		checkedEnabledButton(form,'id','DelRec');
	}
	
	function DeleteMode2SetStatus()
	{
		document.getElementById("DelRec").disabled = 
			!(document.getElementById("radio2").checked && document.getElementById("deleteNum").value != "");
	}
	
	function DeleteMode3SetStatus()
	{
		document.getElementById("DelRec").disabled = 
			!(document.getElementById("radio3").checked && document.getElementById("deleteFrom_deleteFrom").value != "");
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">		
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/icon19.jpg) no-repeat 6px 50%;">搜索日志</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">发生时间范围</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc4:Calendar id="postdatetimeStart" runat="server" ReadOnly="True" ScriptPath="../js/calendar.js"></cc4:Calendar> -
			<cc4:Calendar id="postdatetimeEnd" runat="server" ReadOnly="True" ScriptPath="../js/calendar.js"></cc4:Calendar>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">搜索内容</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:TextBox id="others" runat="server" RequiredFieldType="暂无校验" width="200"></cc2:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">管理员名称</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:TextBox id="Username" runat="server" RequiredFieldType="暂无校验" width="200"></cc2:TextBox>
		</td>
		<td class="vtop">多用户名中间请用半角逗号 "," 分割</td>
	</tr>
</table>
<div class="Navbutton"><cc1:Button ID="SearchLog" runat="server" Text="开始搜索" ButtonImgUrl="../images/search.gif"></cc1:Button></div>
</fieldset>
</div>		
<cc1:datagrid id="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" PageSize="15" >
	<Columns>						
		<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<HeaderStyle Width="20px" />
			<ItemTemplate>
				<input id="id" type="checkbox" onclick="checkedEnabledButton(this.form,'id','DelRec')" value='<%# Eval("id").ToString() %>' name="id" />
			</ItemTemplate>
		</asp:TemplateColumn>					
		<asp:BoundColumn DataField="id" HeaderText="id [递增]" Visible="false" ></asp:BoundColumn>
		<asp:BoundColumn DataField="moderatorname" HeaderText="操作者" ></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="用户组">
			<ItemTemplate>
				<%# GroupName(Eval("groupid").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="ip" HeaderText="IP地址" ></asp:BoundColumn>
		<asp:BoundColumn DataField="postdatetime" HeaderText="时间" ></asp:BoundColumn>
		<asp:BoundColumn DataField="fname" HeaderText="版块" ></asp:BoundColumn>
		<asp:BoundColumn DataField="title" HeaderText="主题" ></asp:BoundColumn>				
		<asp:BoundColumn DataField="actions"  HeaderText="动作" ></asp:BoundColumn>
		<asp:BoundColumn DataField="reason"  HeaderText="原因" ></asp:BoundColumn>				
	</Columns>
</cc1:datagrid>
<div class="Navbutton">
	<input type="radio" name="deleteMode" checked="checked" onclick="changeDeleteModeState(1,this.form)" value="chkall" id="radio1" />
	<input title="选中/取消" type="checkbox" name="chkall1" id="chkall1" onclick='document.getElementById("chkall").click()' />全选/取消全选
	&nbsp;&nbsp;<input type="radio" name="deleteMode" onclick="changeDeleteModeState(2,this.form)" value="deleteNum" id="radio2" />
	保留最新<cc2:TextBox id="deleteNum" runat="server" RequiredFieldType="暂无校验" size="5" MaxLength="5" Enabled="true" onkeyup="DeleteMode2SetStatus()"></cc2:TextBox>条记录
	&nbsp;&nbsp;<input type="radio" name="deleteMode" onclick="changeDeleteModeState(3,this.form)" value="deleteFrom" id="radio3" />
	删除<cc4:Calendar id="deleteFrom" runat="server" ReadOnly="True" ScriptPath="../js/calendar.js" Enabled="False"></cc4:Calendar>之前的记录
	&nbsp;&nbsp;<cc1:Button id="DelRec" runat="server" Text=" 删 除 "   ButtonImgUrl="../images/del.gif" Enabled="false" 
	 OnClientClick="if(!confirm('你确认要删除所选的管理日志记录吗？')) return false;"></cc1:Button>
</div>
<script type="text/javascript">
	document.getElementById("deleteFrom_deleteFrom").onkeyup = DeleteMode3SetStatus;
	document.getElementById("deleteFrom_deleteFrom").onchange = DeleteMode3SetStatus;
</script>
</form>
<%=footer%>
</body>
</html>