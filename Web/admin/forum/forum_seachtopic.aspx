<%@ Page language="c#" Inherits="BBX.Web.Admin.seachtopic" Codebehind="forum_seachtopic.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc4" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>seachcondition</title>
<link href="../styles/calendar.css" type="text/css" rel="stylesheet" />		
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<fieldset>
<legend style="background:url(../images/icons/icon19.jpg) no-repeat 6px 50%;">搜索主题</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">所在论坛</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:dropdowntreelist id="forumid" runat="server"></cc1:dropdowntreelist>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title">被浏览次数小于</td><td class="item_title">被浏览次数大于</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:textbox id="viewsmin" runat="server" RequiredFieldType="数据校验" Size="5"></cc1:textbox>
		</td>
		<td class="vtop rowform">
			<cc1:textbox id="viewsmax" runat="server" RequiredFieldType="数据校验" Size="5"></cc1:textbox>
		</td>
	</tr>
	<tr><td class="item_title">被回复次数小于</td><td class="item_title">被回复次数大于</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:textbox id="repliesmin" runat="server" RequiredFieldType="数据校验" Size="5"></cc1:textbox>
		</td>
		<td class="vtop rowform">
			<cc1:textbox id="repliesmax" runat="server" RequiredFieldType="数据校验" Size="5"></cc1:textbox>
		</td>
	</tr>
	<tr><td class="item_title">所需阅读权限高于</td><td class="item_title">多少天内无新回复</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:textbox id="rate" runat="server" RequiredFieldType="数据校验" Size="5"></cc1:textbox>
		</td>
		<td class="vtop rowform">
			<cc1:textbox id="lastpost" runat="server" RequiredFieldType="数据校验" Size="5"></cc1:textbox>
		</td>
	</tr>
	<tr><td class="item_title" colspan="2">发表时间范围</td></tr>
	<tr>
		<td class="vtop rowform">
			起始日期:<cc4:Calendar id="postdatetimeStart" runat="server" ReadOnly="True" ScriptPath="../js/calendar.js"></cc4:Calendar><br />
			结束日期:<cc4:Calendar id="postdatetimeEnd" runat="server" ReadOnly="True" ScriptPath="../js/calendar.js"></cc4:Calendar>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">标题关键字</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="keyword" runat="server" RequiredFieldType="暂无校验" Width="100"></cc1:TextBox>
		</td>
		<td class="vtop">多关键字中间请用半角逗号"," 分割</td>
	</tr>
	<tr><td class="item_title" colspan="2">主题作者</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="poster" runat="server" RequiredFieldType="暂无校验" Width="100"></cc1:TextBox>&nbsp;
			<input id="lowerupper" type="checkbox" value="1" name="cins" runat="server" checked="checked" />不区分大小写
		</td>
		<td class="vtop">多用户名中间请用半角逗号 "," 分割</td>
	</tr>
	<tr><td class="item_title" colspan="2">是否包含精华帖</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:radiobuttonlist id="digest" runat="server" RepeatColumns="3">
				<asp:Listitem Value="0" selected="true">无限制</asp:Listitem>
				<asp:Listitem Value="1">包含且仅包含</asp:Listitem>
				<asp:Listitem Value="20">不包含</asp:Listitem>
			</cc1:radiobuttonlist>
		</td>
		<td class="vtop">多用户名中间请用半角逗号 "," 分割</td>
	</tr>
	<tr><td class="item_title" colspan="2">是否包含置顶帖</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:radiobuttonlist id="displayorder" runat="server" RepeatColumns="3">
				<asp:Listitem Value="0" selected="true">无限制</asp:Listitem>
				<asp:Listitem Value="1">包含且仅包含</asp:Listitem>
				<asp:Listitem Value="20">不包含</asp:Listitem>
			</cc1:radiobuttonlist>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">是否包含附件</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:radiobuttonlist id="attachment" runat="server" RepeatColumns="3">
				<asp:Listitem Value="0" selected="true">无限制</asp:Listitem>
				<asp:Listitem Value="1">包含且仅包含</asp:Listitem>
				<asp:Listitem Value="20">不包含</asp:Listitem>
			</cc1:radiobuttonlist>
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div class="Navbutton"><cc1:Button id="SaveSearchCondition" runat="server" Text="搜索符合条件主题" ButtonImgUrl="../images/search.gif"></cc1:Button></div>
</fieldset>
<div id="topictypes" style="display:none;">
	<tr><td class="item_title" colspan="2">所在分类</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:dropdownlist id="typeid" runat="server"></cc1:dropdownlist>
		</td>
		<td class="vtop"></td>
	</tr>
</div>
</form>
</div>
<%=footer%>
</body>
</html>