<%@ Page Language="c#" Inherits="BBX.Web.Admin.siteset" CodeBehind="global_siteset.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>站点信息</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
	function setStatus(status) {
	    document.getElementById("isclosedforum").style.display = (status) ? "block" : "none";
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server" name="Form1">
<fieldset>
	<legend style="background: url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">站点信息</legend>
	<table width="100%">
	<tr><td class="item_title" colspan="2">网站名称</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="webtitle" runat="server" RequiredFieldType="暂无校验"></cc1:TextBox>
		</td>
		<td class="vtop">网站名称, 将显示在页面底部的联系方式处</td>
	</tr>
	<tr><td class="item_title" colspan="2">网站URL地址</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="weburl" runat="server" RequiredFieldType="网页地址"></cc1:TextBox>
		</td>
		<td class="vtop">网站 URL, 将作为链接显示在页面底部</td>
	</tr>
	<tr><td class="item_title" colspan="2">论坛名称</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="forumtitle" runat="server" RequiredFieldType="暂无校验"></cc1:TextBox>
		</td>
		<td class="vtop">论坛名称, 将显示在导航条和标题中</td>
	</tr>
	<tr><td class="item_title" colspan="2">显示授权信息链接</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="licensed" runat="server"  RepeatLayout="Flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">选择"是"将在页脚显示商业授权用户链接, 链接将指向 <%=BBX.Common.Utils.ProductName%> 官方网站, 用户可通过此链接验证其所使用的 <%=BBX.Common.Utils.ProductName%> 是否经过商业授权</td>
	</tr>
	<tr><td class="item_title" colspan="2">网站备案信息代码</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="icp" runat="server"  RequiredFieldType="暂无校验"></cc1:TextBox>
		</td>
		<td class="vtop">页面底部可以显示 ICP 备案信息,如果网站已备案,在此输入您的授权码,它将显示在页面底部,如果没有请留空</td>
	</tr>
	<tr><td class="item_title" colspan="2">显示程序运行时间</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="debug" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">选择"是"将在页脚处显示程序运行时间</td>
	</tr>
	<tr><td class="item_title" colspan="2">统计代码设置</td></tr>
	<tr>
		<td class="vtop rowform">
			<uc1:TextareaResize id="Statcode" runat="server"  cols="45" controlname="Linktext" HintPosOffSet="160"></uc1:TextareaResize>
		</td>
		<td class="vtop">用户可以自己添加的统计代码</td>
	</tr>
	<tr><td class="item_title" colspan="2">外部链接</td></tr>
	<tr>
		<td class="vtop rowform">
			<uc1:TextareaResize id="Linktext" runat="server"  cols="45" controlname="Linktext" HintPosOffSet="160"></uc1:TextareaResize>
		</td>
		<td class="vtop">用户可以自己添加的外部链接html字符串，例如&lt;a href='/download/'&gt;下载频道&lt;/a&gt;</td>
	</tr>

	</table>
</fieldset>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<fieldset>
	<legend style="background: url(../images/icons/icon23.jpg) no-repeat 6px 50%;">模块设置</legend>
	<table width="100%">
	<tr><td class="item_title" colspan="2">是否关闭论坛</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="closed" runat="server" RepeatLayout="flow" HintPosOffSet="80">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">暂时将论坛关闭(包括论坛、个人空间、相册等), 其他人无法访问, 但不影响管理员访问</td>
	</tr>
	<tbody id="isclosedforum">
	<tr><td class="item_title" colspan="2">关闭的原因</td></tr>
	<tr>
		<td class="vtop rowform">
			<uc1:TextareaResize id="closedreason" runat="server" controlname="closedreason" HintPosOffSet="160"></uc1:TextareaResize>
		</td>
		<td class="vtop">论坛关闭时出现的提示信息</td>
	</tr>
	</tbody>
	</table>
</fieldset>
<div class="Navbutton">
	<cc1:Button ID="SaveInfo" runat="server" Text="提 交"></cc1:Button>
</div>
<script type="text/javascript">
	setStatus(document.getElementById("closed_0").checked);
</script>
</form>
</div>
<%=footer%>
</body>
</html>
