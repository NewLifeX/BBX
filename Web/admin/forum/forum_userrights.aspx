<%@ Page Language="C#" CodeBehind="forum_userrights.aspx.cs" Inherits="BBX.Web.Admin.forum_userrights" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>用户权限</title>
<script type="text/javascript" src="../js/common.js"></script>		
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="form1" runat="server">
<div class="ManagerForm">
	<fieldset>
	<legend style="background:url(../images/icons/icon42.jpg) no-repeat 6px 50%;">用户权限</legend>
	<table width="100%">
	<tr><td class="item_title" colspan="2">允许重复评分</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="dupkarmarate" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">选择"是"将允许用户对一个帖子进行多次评分, 默认为"否"</td>
	</tr>
    <tr><td class="item_title" colspan="2">编辑帖子时间限制</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="edittimelimit" runat="server" Text="5" RequiredFieldType="数据校验" CanBeNull="必填" Size="7" MaxLength="7"></cc1:TextBox>(单位:分钟)
		</td>
		<td class="vtop">帖子作者发帖后超过此时间限制将不能再编辑帖, 版主和管理员不受此限制, 0 为不限制, -1为不允许帖子作者编辑</td>
	</tr>
    <tr><td class="item_title" colspan="2">删除帖子时间限制</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="deletetimelimit" runat="server" Text="5" RequiredFieldType="数据校验" CanBeNull="必填" Size="7" MaxLength="7"></cc1:TextBox>(单位:分钟)
		</td>
		<td class="vtop">帖子作者发帖后超过此时间限制将不能再删除, 版主和管理员不受此限制, 0 为不限制, -1为不允许帖子作者删除</td>
	</tr>
	<tr><td class="item_title" colspan="2">最大允许的上传附件数</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox ID="maxattachments" runat="server" CanBeNull="必填" HintInfo="最大允许的上传附件数" HintTitle="提示" MaxLength="5" RequiredFieldType="数据校验" Size="6" />
			<asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" ControlToValidate="maxattachments" ErrorMessage="请输入正整数或者零" ValidationExpression="^[0-9]*$">
			</asp:RegularExpressionValidator>
		</td>
		<td class="vtop">最大允许的上传附件数</td>
	</tr>
	<tr><td class="item_title" colspan="2">评分时间限制</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="karmaratelimit" runat="server" Text="10" RequiredFieldType="数据校验"  CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox>(单位:小时)
			 <asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" ControlToValidate="karmaratelimit" ErrorMessage="请输入正整数或者零" ValidationExpression="^[0-9]*$">
             </asp:RegularExpressionValidator>
		</td>
		<td class="vtop">帖子发表后超过此时间限制其他用户将不能对此帖评分, 版主和管理员不受此限制, 0 为不限制</td>
	</tr>
	<tr><td class="item_title" colspan="2">收藏夹容量</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxfavorites" runat="server" CanBeNull="必填"  RequiredFieldType="数据校验" Text="" Size="8" MaxLength="7"></cc1:TextBox>
			<asp:RegularExpressionValidator id="RegularExpressionValidator4" runat="server" ControlToValidate="maxfavorites" 
			ErrorMessage="请输入正整数或者零" ValidationExpression="^[0-9]*$">
			</asp:RegularExpressionValidator>
		</td>
		<td class="vtop">允许收藏的最大板块 / 主题数, 默认为100</td>
	</tr>
	<tr><td class="item_title" colspan="2">投票最大选项数</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="maxpolloptions" runat="server" CanBeNull="必填"  RequiredFieldType="数据校验" Text=""  Size="8" MaxLength="7"></cc1:TextBox>
			<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="maxpolloptions" ErrorMessage="请输入正整数或者零" ValidationExpression="^[0-9]*$">
			</asp:RegularExpressionValidator>
		</td>
		<td class="vtop">设定发布投票包含的最大选项数</td>
	</tr>
	<tr><td class="item_title" colspan="2">帖子最小字数</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="minpostsize" runat="server" CanBeNull="必填"  RequiredFieldType="数据校验" Text=""  Size="8"  MaxLength="7"></cc1:TextBox>(单位:字节)
			<select onchange="document.getElementById('minpostsize').value=this.value">
				<option value="">请选择</option>
				<option value="51200">50K</option>
				<option value="102400">100K</option>
				<option value="153600">150K</option>
				<option value="204800">200K</option>
				<option value="256000">250K</option>
				<option value="307200">300K</option>
				<option value="358400">350K</option>
				<option value="409600">400K</option>
				<option value="512000">500K</option>
				<option value="614400">600K</option>
				<option value="716800">700K</option>
				<option value="819200">800K</option>
				<option value="921600">900K</option>
				<option value="1024000">1M</option>
				<option value="2048000">2M</option>
				<option value="4096000">4M</option>								
			</select>
		</td>
		<td class="vtop">管理组成员可通过"发帖不受限制"设置而不受影响, 0 为不限制</td>
	</tr>
	<tr><td class="item_title" colspan="2">帖子最大字数</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxpostsize" runat="server" CanBeNull="必填"  RequiredFieldType="数据校验" Text=""  Size="8" MaxLength="7"></cc1:TextBox>(单位:字节)
			<select onchange="document.getElementById('maxpostsize').value=this.value">
				<option value="">请选择</option>
				<option value="51200">50K</option>
				<option value="102400">100K</option>
				<option value="153600">150K</option>
				<option value="204800">200K</option>
				<option value="256000">250K</option>
				<option value="307200">300K</option>
				<option value="358400">350K</option>
				<option value="409600">400K</option>
				<option value="512000">500K</option>
				<option value="614400">600K</option>
				<option value="716800">700K</option>
				<option value="819200">800K</option>
				<option value="921600">900K</option>
				<option value="1024000">1M</option>
				<option value="2048000">2M</option>
				<option value="4096000">4M</option>								
			</select>
		</td>
		<td class="vtop">管理组成员可通过"发帖不受限制"设置而不受影响</td>
	</tr>
	<tr><td class="item_title" colspan="2">主题查看页面显示管理操作否</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="moderactions" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">显示</asp:ListItem>
				<asp:ListItem Value="0">不显示</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">是否在主题查看页面显示管理操作</td>
	</tr>
	</table>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div class="Navbutton">
<cc1:Button id="SaveInfo" runat="server" Text=" 提 交 "></cc1:Button>
</div>
</fieldset>	
</div>	
</form>
<%=footer%>
</body>
</html>