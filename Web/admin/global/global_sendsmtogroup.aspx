<%@ Page Language="c#" Inherits="BBX.Web.Admin.sendsmtogroup" CodeBehind="global_sendsmtogroup.aspx.cs" %>

<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="textarea" Src="../UserControls/TextareaResize.ascx" %>
<%@ Register TagPrefix="cc3" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>批量发送短消息</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/tab.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../../js/common.js"></script>
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script language="JavaScript" type="text/javascript" src="../../javascript/ajax.js"></script>
<script type="text/javascript">
    function SendPM(startuid) {
        var groupidlist = "<%=groupidlist%>";
        var topnumber = $("postcountpercircle").value;
        var msgfrom = $("msgfrom").value;
        var msguid = "<%=userid%>";
        var folder = $("folder").value;
        var subject = $("subject").value;
        var postdatetime = $("postdatetime").value;
        var message = $("message_posttextarea").value;

        var url = "groupidlist=" + groupidlist + "&topnumber=" + topnumber + "&start_uid=" + startuid + "&msgfrom=" + encodeChar(msgfrom) + "&msguid=" + msguid + "&folder=" + folder + "&subject=" + encodeChar(subject) + "&postdatetime=" + postdatetime + "&message=" + encodeChar(message);
        _sendRequest('../global/global_ajaxcall.aspx?opname=sendsmtogroup', sendpm_callback, false, url);
    }

    function encodeChar(str) {
        return encodeURI(str.replace(/\+/g, '_plus_').replace(/\&/g, '_and_').replace(/\=/g, '_equal_'));
    }
    
    function sendpm_callback(doc)
	{	
	    var data=eval(doc);
	    if(data[0]==undefined) {
	        $('Layer5').innerHTML = '运行错误，系统重新加载当前页面!';
	       window.location.href = "global_sendsmtogroup.aspx";
	    }
	    else
	    {
	        var sendcount = parseInt(data[0].count);
	        if (sendcount == 0) {
                  $('Layer5').innerHTML = "<br />短消息已发送成功!";
                  window.location.href = "global_sendsmtogroup.aspx";
            }
            else {
                  if (isNaN(sendcount)) 
                      sendcount = 0;
                  count += sendcount;
                  $('Layer5').innerHTML = "<br />已发送" + count + " 条短消息";
                  SendPM(data[0].startuid);
            }	   
	    }
    }

    var count;//统计已操作的记录数

    function submit_Click() {
//        if (!confirm('你确认要发送短消息吗?')) {
//            $("SendPM").disabled = false;
//            return false;
//        }
        $("BatchSendSM").disabled = true;
        $('Layer5').innerHTML = "<br />正在发送短消息...";
        $('success').style.display = "block";
        count = 0;        
        SendPM(0);
   }   
</script>
</head>
<body>
<form id="Form1" method="post" runat="server">
<script type="text/javascript">
	document.getElementById('Layer5').innerHTML = "<br /><table><tr><td valign=top><img border=\"0\" src=\"../images/ajax_loading.gif\"  /></td><td valign=middle style=\"font-size: 14px;\" >程序已启动. 如果用户较多, <br />系统要运行一段时间....</td></tr></table><BR />";
</script>
<div class="ManagerForm">
<fieldset>
<legend style="background: url(../images/icons/icon57.jpg) no-repeat 6px 50%;">批量短消息发送</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">接收短消息的用户组</td></tr>
	<tr>
		<td class="vtop" colspan="2">
			<input type="checkbox" id="chkall" name="chkall" onclick="javascript:CheckAll(this.form);" />选择全部/取消
			<br />
			<cc1:CheckBoxList ID="Usergroups" runat="server" RepeatColumns="4">
			</cc1:CheckBoxList>
		</td>
	</tr>
	<tr><td class="item_title" colspan="2">标题</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox ID="subject" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验" MaxLength="80" Size="60"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">发布者用户名</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox ID="msgfrom" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">文件箱</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:DropDownList ID="folder" runat="server">
				<asp:ListItem Value="0" Selected="True">收件箱</asp:ListItem>
				<asp:ListItem Value="1">发件箱</asp:ListItem>
				<asp:ListItem Value="2">草稿箱</asp:ListItem>
			</cc1:DropDownList>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">发送日期</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox ID="postdatetime" runat="server" CanBeNull="必填" RequiredFieldType="日期" Size="10"></cc1:TextBox>
		</td>
		<td class="vtop">格式:2005-5-5</td>
	</tr>
	<tr><td class="item_title" colspan="2">每次循环发送消息数</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox ID="postcountpercircle" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Size="10">100</cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">短消息内容</td></tr>
	<tr>
		<td class="vtop rowform">
			  <uc1:textarea ID="message" runat="server" controlname="TabControl1:tabPage51:message" Rows="10" cols="80"></uc1:textarea>
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<div class="Navbutton">
	<cc1:Button ID="BatchSendSM" runat="server" Style="align: center" Text=" 提 交 ">
	</cc1:Button>
</div>
</fieldset>
</div>
<asp:Label ID="lblClientSideCheck" runat="server" CssClass="hint">&nbsp;</asp:Label>
<asp:Label ID="lblCheckedNodes" runat="server" CssClass="hint">&nbsp;</asp:Label>
<asp:Label ID="lblServerSideCheck" runat="server" CssClass="hint">&nbsp;</asp:Label>
<script type="text/javascript">
	document.getElementById("lblClientSideCheck").innerText = document.getElementById("lblServerSideCheck").innerText;
</script>
</form>
<%=footer%>
</body>
</html>