<%@ Page Language="c#" AutoEventWireup="true" CodeBehind="global_passportsetting.aspx.cs" Inherits="BBX.Web.Admin.global_passportsetting" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>应用程序设置</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
function validate(theform)
{
	if(document.getElementById("appname").value == "")
	{
		resetpage();
		alert("应用程序名称没有填写!");
		document.getElementById("appname").focus();
		return false;
	}
	if (document.getElementById("asyncmode").value == 2 && document.getElementById("appurl").value == "")
	{
		resetpage();
		alert("应用程序Url地址没有填写!");
		document.getElementById("appurl").focus();
		return false;
	}
	if (document.getElementById("asyncmode").value == 2 && document.getElementById("callbackurl").value == "")
	{
		resetpage();
		alert("登录完成后返回地址没有填写!");
		document.getElementById("callbackurl").focus();
		return false;
    }
	return true;
}
function resetpage()
{
	document.getElementById("success").style.display = "none";
	document.getElementById("savepassportinfo").disabled = false;            
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="form1" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">应用程序设置</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">应用程序名称</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="appname" runat="server"  CanBeNull="必填" Width="250"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">应用程序类型</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="applicationtype" runat="server" RepeatColumns="2">
				<asp:ListItem Value="1" Selected="true">Web</asp:ListItem>
				<asp:ListItem Value="2">桌面</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>

	<tbody id="showurl">
	<tr><td class="item_title" colspan="2">应用程序 Url 地址</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="appurl" runat="server" Width="250"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">登录完成后返回地址</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="callbackurl" runat="server"  Width="250"></cc1:TextBox>
		</td>
		<td class="vtop">通过通行证登录以后返回应用程序的地址(Callback URL)</td>
	</tr>
    <tr><td class="item_title" colspan="2">同步数据模式</td></tr>
    <tr>
        <td>
            <cc1:RadioButtonList id="asyncmode" runat="server" RepeatColumns="3">
                <asp:ListItem Value="1" Selected="true">开启</asp:ListItem>
                <asp:ListItem Value="0">关闭</asp:ListItem>
                <asp:ListItem Value="2">自定义</asp:ListItem>
            </cc1:RadioButtonList>
        </td>
        <td class="vtop"></td>
    </tr>
    <tr id="tr_asyncurl">
	<td colspan="2">
		<table width="100%">
		<tr><td class="item_title" colspan="2">同步数据的 URL 地址</td></tr>
		<tr>
			<td class="vtop rowform"><cc1:TextBox id="asyncurl" runat="server" Width="250"></cc1:TextBox></td>
			<td class="vtop">论坛内发生登录、注册等事件后会向该地址发送消息，以便数据同步</td>
		</tr>
		</table>
	</td>
	</tr>
    <tr id="tr_asynclist" style="display:none;">
	<td colspan="2">
		<table width="100%">
		<tr><td class="item_title" colspan="2">同步数据的事件列表</td></tr>
		<tr>
			<td class="vtop rowform"><cc1:TextBox id="asynclist" runat="server" Wdith="250"></cc1:TextBox></td>
			<td class="vtop">自定义事件名称列表，避免不必要的请求，以逗号分隔。（请点击<a href="http://www.newlifex.com/doc/default.aspx?cid=155" target="_blank">自定义同步数据说明</a>了解事件列表关键字）</td>
		</tr>
		</table>
	</td>
	</tr>
	</tbody>

	<tr><td class="item_title" colspan="2">允许的服务器IP地址</td></tr>
	<tr>
		<td class="vtop rowform">
			 <uc1:TextareaResize id="ipaddresses" runat="server" controlname="ipaddresses" HintPosOffSet="160" ></uc1:TextareaResize>
		</td>
		<td class="vtop">如果你提交的是例如" 10.1.20.1, 10.1.20.3 ",其它地址将被拒绝</td>
	</tr>
</table>
<div class="Navbutton">
	<input type="hidden" id="apikeyhidd" runat="server" />
	<cc1:Button id="savepassportinfo" runat="server" Text=" 提 交 " OnClick="savepassportinfo_Click" ValidateForm="true"></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button3" onclick="window.history.back()"><img src="../images/arrow_undo.gif"/> 返 回 </button>
</div>	
</fieldset>
<cc1:Hint ID="hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
</div>
</form>
<%=footer%>
</body>
</html>