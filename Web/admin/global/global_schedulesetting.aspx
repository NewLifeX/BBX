<%@ Page Language="c#" AutoEventWireup="true" CodeBehind="global_schedulesetting.aspx.cs" Inherits="BBX.Web.Admin.global_schedulesetting" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<%@ Register TagPrefix="uc2" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>计划任务设置</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
function changetimespan(thevalue)
{
	if(thevalue == "type1")
	{
		document.getElementById("hour").disabled = false;
		document.getElementById("minute").disabled = false;
		document.getElementById("timeserval").disabled = true;
	}
	else
	{
		document.getElementById("hour").disabled = true;
		document.getElementById("minute").disabled = true;
		document.getElementById("timeserval").disabled = false;
	}
}
function validate(theform)
{
	return true;
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="form1" runat="server">
<div class="ManagerForm">
<uc2:PageInfo id="PageInfo1" runat="server" Icon="warning" Text="请勿随意添加计划任务,此功能适用于开发人员。"></uc2:PageInfo>
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">计划任务名称</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">用户名</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="key" runat="server"  CanBeNull="必填" Width="250"></cc1:TextBox><input type="hidden" name="oldkey" id="oldkey" runat="server" />
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">计划任务类型</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="scheduletype" runat="server"  CanBeNull="必填" Width="400"></cc1:TextBox>
		</td>
		<td class="vtop">例如:BBX.Forum.ScheduledEvents.TagsEvent, BBX.Forum</td>
	</tr>
	<tr><td class="item_title" colspan="2">执行方式</td></tr>
	<tr>
		<td class="vtop rowform">
			<asp:RadioButton ID="type1" GroupName="type" runat="server" Checked="true" />&nbsp;定时执行&nbsp;&nbsp;
			<cc1:DropDownList ID="hour" runat="server"/>&nbsp;时&nbsp;<cc1:DropDownList ID="minute" runat="server" />&nbsp;分&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			<asp:RadioButton ID="type2" GroupName="type" runat="server" />&nbsp;周期执行&nbsp;&nbsp;
			<cc1:TextBox ID="timeserval" runat="server" Width="40" ></cc1:TextBox>&nbsp;分钟
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">启用</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="eventenable" runat="server">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<div class="Navbutton">
	<input type="hidden" id="apikeyhidd" runat="server" />
	<cc1:Button id="savepassportinfo" runat="server" Text=" 提 交 " OnClick="savepassportinfo_Click" ValidateForm="true"></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button3" onclick="window.location='global_schedulemanage.aspx';"><img src="../images/arrow_undo.gif"/> 返 回 </button>
</div>	
</fieldset>
<cc1:Hint ID="hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
</div>
</form>
<%=footer%>
</body>
</html>