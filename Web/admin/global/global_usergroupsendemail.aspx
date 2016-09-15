<%@ Page Language="c#" Inherits="BBX.Web.Admin.usergroupsendemail" Codebehind="global_usergroupsendemail.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<%@ Register TagPrefix="cc3" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>用户组邮件群发</title>
<link href="../styles/tab.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../../js/common.js"></script>
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script language="JavaScript" type="text/javascript" src="../../javascript/ajax.js"></script>
<script type="text/javascript">
    function SendEmail(startuid) {
        var groupidlist = "<%=groupidlist%>";
        var topnumber = $("postcountpercircle").value;
        var subject = $("subject").value;
        var body = $("body_posttextarea").value;

        var url = "groupidlist=" + groupidlist + "&topnumber=" + topnumber + "&start_uid=" + startuid + "&subject=" + encodeChar(subject) + "&body=" + encodeChar(body);
        _sendRequest('../global/global_ajaxcall.aspx?opname=usergroupsendemail', sendemail_callback, false, url);
    }

    function encodeChar(str) {
        return encodeURI(str.replace(/\+/g, '_plus_').replace(/\&/g, '_and_').replace(/\=/g, '_equal_'));
    }

    function sendemail_callback(doc) {
        var data = eval(doc);
        if (data[0] == undefined) {
            $('Layer5').innerHTML = '运行错误，系统重新加载当前页面!';
            window.location.href = "global_sendsmtogroup.aspx";
        }
        else {
            var sendcount = parseInt(data[0].count);
            if (sendcount == 0) {
                $('Layer5').innerHTML = "<br />邮件已发送成功!";
                window.location.href = "global_sendsmtogroup.aspx";
            }
            else {
                if (isNaN(sendcount))
                    sendcount = 0;
                count += sendcount;
                $('Layer5').innerHTML = "<br />已发送" + count + " 封邮件";
                SendEmail(data[0].startuid);
            }
        }
    }

    var count; //统计已操作的记录数

    function submit_Click() {
        //        if (!confirm('你确认要发送短消息吗?')) {
        //            $("SendPM").disabled = false;
        //            return false;
        //        }
        $("BatchSendEmail").disabled = true;
        $('Layer5').innerHTML = "<br />正在发送邮件...";
        $('success').style.display = "block";
        count = 0;
        SendEmail(0);
    }   
</script>
<script type="text/javascript">
	function nodeCheckChanged(node)
	{
		var status = "未选取"; 
		if (node.Checked) status = "选取"; 
	}  
</script>
</head>
<body>
<form id="Form1" method="post" runat="server">
<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div class="ManagerForm">
<fieldset>
<legend style="background: url(../images/icons/icon41.jpg) no-repeat 6px 50%;">批量邮件发送</legend>
<table width="100%">	
	<tbody id="user">
	<tr><td class="item_title" colspan="2">接收邮件用户名称</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox ID="usernamelist" runat="server" CanBeNull="可为空" RequiredFieldType="暂无校验" Width="400" Height="40" TextMode="MultiLine"></cc1:TextBox>
		</td>
		<td class="vtop">要发送的用户名列表,以","进行分割</td>
	</tr>
	</tbody>
	<tbody id="usergroup">
	<tr><td class="item_title" colspan="2">接收邮件用户组</td></tr>
	<tr>
		<td class="vtop" colspan="2">
			<input type="checkbox" id="Checkbox1" name="chkall" onclick="javascript:CheckAll(this.form);" />选择全部/取消
			
			<a href = "#" onclick="document.getElementById('flag').value=1;Form1.submit()">[导出选中用户组用户的Email]</a>
			<input type="hidden" name="flag" id="flag" />
			<br />
			<cc1:CheckBoxList ID="Usergroups" runat="server" RepeatColumns="4">
			</cc1:CheckBoxList>
		</td>
	</tr>
	
	</tbody>
	<tr><td class="item_title" colspan="2">每次循环发送消息数</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox ID="postcountpercircle" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Size="10">50</cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">邮件标题</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox ID="subject" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验" Width="400"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">邮件内容</td></tr>
	<tr>
		<td class="vtop rowform">
			<uc1:TextareaResize ID="body" runat="server" controlname="TabControl1:tabPage51:body" rows="18" cols="80"></uc1:TextareaResize>
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<div class="Navbutton">
	<cc1:Button ID="BatchSendEmail" runat="server" Text=" 提 交 "></cc1:Button>
</div>
<asp:Label ID="lblClientSideCheck" runat="server" CssClass="hint">&nbsp;</asp:Label>
<asp:Label ID="lblCheckedNodes" runat="server" CssClass="hint">&nbsp;</asp:Label>
<asp:Label ID="lblServerSideCheck" runat="server" CssClass="hint">&nbsp;</asp:Label>
<script type="text/javascript">
	document.getElementById("lblClientSideCheck").innerText = document.getElementById("lblServerSideCheck").innerText;
</script>
</fieldset>
</div>
</form>
<%=footer%>
</body>
</html>