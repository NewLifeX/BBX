<%@ Page language="c#" Inherits="BBX.Web.Admin.topicsgrid" Codebehind="forum_topicsgrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>主题列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />		
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
function check(browser)
{ 
   document.forms[0].operation.value=browser;
}

function CheckAll(form)
  {
  for (var i=0;i<form.elements.length;i++)
	{
	var e = form.elements[i];
	if (e.name == 'tid')
	   e.checked = form.chkall.checked;
	}
}

function SH_SelectOne(obj)
{
	if( obj.checked == false)
	{
		document.getElementById('chkall').checked = obj.chcked;
		
	}
}

function Check(form)
{
	CheckAll(form);
	checkedEnabledButton(form,'tid','SetTopicInfo')
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">操作主题</legend>
<table cellspacing="0" cellpadding="4" width="100%" align="center">
	<tr>
		<td class="panelbox" colspan="2">
			<table width="100%">
				<tr>
					<td style="width:100px"><input type="radio" name="operation" value="moveforum" onClick="check(this.value)" checked />批量移动到论坛</td>
					<td><cc2:dropdowntreelist id="forumid" runat="server"></cc2:dropdowntreelist></td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td class="panelbox" align="left" width="50%">
			<table width="100%">
				<tr>
					<td style="width:100px"><input type="radio" name="operation" value="delete" onClick="check(this.value)" />批量删除</td>
					<td><input type="checkbox" name="nodeletepostnum" value="1" checked id="nodeletepostnum" runat="server" />删帖不减用户发帖数和积分</td>
				</tr>
				<tr>
					<td><input type="radio" name="operation" value="displayorder" onclick="check(this.value)" />批量置顶</td>
					<td>
						<input type="radio" name="displayorder_level" value="0" checked onclick="check(this.value)" />取消置顶 <br />
						<input type="radio" name="displayorder_level" value="1" onclick="check(this.value)" />
						<img src="../images/star.gif" width="16" height="16" /> <br /> 
						<input type="radio" name="displayorder_level" value="2" onclick="check(this.value)" />
						<img src="../images/star.gif" width="16" height="16" />
						<img src="../images/star.gif" width="16" height="16" /><br />
						<input type="radio" name="displayorder_level" value="3" onclick="check(this.value)" />
						<img src="../images/star.gif" width="16" height="16" />
						<img src="../images/star.gif" width="16" height="16" />
						<img src="../images/star.gif" width="16" height="16" />
					</td>
				</tr>
			</table>
		</td>
		<td class="panelbox" align="right" width="50%">
			<table width="100%">
				<tr>
					<td style="width:110px"><input type="radio" name="operation" value="deleteattach" onClick="check(this.value)" />删除主题中的附件</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td><input type="radio" name="operation" value="adddigest" onclick="check(this.value)" />批量设置精华</td>
					<td>
						<input type="radio" name="digest_level" value="0" checked /> 取消精华 <br />
						<input type="radio" name="digest_level" value="1" />
						<img src="../images/star.gif" width="16" height="16" /><br />
						<input type="radio" name="digest_level" value="2" />
						<img src="../images/star.gif" width="16" height="16" />
						<img src="../images/star.gif" width="16" height="16" /><br />
						<input type="radio" name="digest_level" value="3" /> 
						<img src="../images/star.gif" width="16" height="16" />
						<img src="../images/star.gif" width="16" height="16" />
						<img src="../images/star.gif" width="16" height="16" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td align="center" colspan="2"><cc1:Button id="SetTopicInfo" runat="server" Text=" 提 交 " Enabled="false"></cc1:Button></td>
	</tr>
</table>
</fieldset>
</div>				
<cc1:datagrid id="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid" PageSize="15">
	<Columns>
		<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<HeaderStyle Width="20px" />
			<ItemTemplate>
				<input id="tid" onclick="checkedEnabledButton(this.form,'tid','SetTopicInfo')" type="checkbox" value="<%# Eval("tid").ToString() %>" name="tid" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="tid" SortExpression="tid" HeaderText="帖子ID" Visible="false" ></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="标题">
			<ItemTemplate>
				<a href="../../showtopic.aspx?topicid=<%# Eval("tid").ToString() %>" target="_blank">
					<%# Eval("title").ToString() %>
				</a>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="发帖人">
			<itemtemplate>
				<%# (Eval("posterid").ToString() != "-1") ? "<a href='../../userinfo.aspx?userid=" + Eval("posterid").ToString() + "' target='_blank'>" + Eval("poster").ToString() + "</a>" : Eval("poster").ToString()%>
			</itemtemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="postdatetime" SortExpression="postdatetime" HeaderText="发布日期" ></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="最后回复人">
			<itemtemplate>
				<%# (Eval("lastposterid").ToString() != "-1") ? "<a href='../../userinfo.aspx?userid=" + Eval("lastposterid").ToString() + "' target='_blank'>" + Eval("lastposter").ToString() + "</a>" : Eval("lastposter").ToString()%>
			</itemtemplate>
		</asp:TemplateColumn>				
		<asp:TemplateColumn HeaderText="回帖数">
			<ItemTemplate>
				<%# GetPostLink(Eval("tid").ToString(),Eval("replies").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>					
		<asp:BoundColumn DataField="views" SortExpression="views" HeaderText="查看数"></asp:BoundColumn>
		<asp:BoundColumn DataField="digest" SortExpression="digest" HeaderText="精华帖" ></asp:BoundColumn>
		<asp:BoundColumn DataField="displayorder" SortExpression="displayorder" HeaderText="显示顺序"></asp:BoundColumn>
		<asp:BoundColumn DataField="price" SortExpression="price" HeaderText="价格"></asp:BoundColumn>					
		<asp:TemplateColumn HeaderText="关闭">
			<ItemTemplate>
				<%# BoolStr(Eval("closed").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</cc1:datagrid>
<div id="topictypes" style="display:none;width:100%;">
	<tr>
		<td class="td2"><input type="radio" name="operation" value="movetype" onClick="check(this.value)">
			批量移动到分类</td>
		<td class="td2"><cc2:dropdownlist id="typeid" runat="server"></cc2:dropdownlist></td>
	</tr>
</div>
</form>
<%=footer%>
</body>
</html>