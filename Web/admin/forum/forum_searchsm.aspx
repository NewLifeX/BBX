<%@ Page language="c#" Inherits="BBX.Web.Admin.searchsm" Codebehind="forum_searchsm.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<html>
<head>
<title>清理短消息</title>		
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script language="JavaScript" type="text/javascript" src="../../javascript/ajax.js"></script>
<script type="text/javascript">
    function DeletePM() {
        var isnew = $("isnew").checked;
        var postdatetime = $("postdatetime").value;
        var msgfromlist = $("msgfromlist").value;
        var lowerupper = $("lowerupper").checked;
        var subject = $("subject").value;
        var message = $("message").value;
        var isupdateusernewpm = $("isupdateusernewpm").checked;

        var url = "isnew=" + isnew + "&postdatetime=" + postdatetime + "&msgfromlist=" + msgfromlist + "&lowerupper=" + lowerupper + "&subject=" + encodeChar(subject) + "&message=" + encodeChar(message) + "&isupdateusernewpm=" + isupdateusernewpm;
        _sendRequest('../global/global_ajaxcall.aspx?opname=DeletePrivateMessages', deletepm_callback, false, url);
    }

    function encodeChar(str) {
        return encodeURI(str.replace(/\+/g, '_plus_').replace(/\&/g, '_and_').replace(/\=/g, '_equal_'));
    }
    
    function deletepm_callback(doc) {
        var data = eval(doc);
        if (data[0] == undefined) {
            $('Layer5').innerHTML = '运行错误，系统重新加载当前页面!';
            window.location.href = "forum_searchsm.aspx";
        }
        else {
            var deletecount = parseInt(data[0].count);
            if (deletecount == 0) {
                $('Layer5').innerHTML = "<br />短消息已清理完毕!";
                window.location.href = "forum_searchsm.aspx";
            }
            else {
                if (isNaN(deletecount))
                    deletecount = 0;
                count += deletecount;
                $('Layer5').innerHTML = "<br />已清理" + count + " 条短消息";
                DeletePM();
            }
        }
    }

    var count;//统计已操作的记录数
    
    function submit_Click() {
     
        if (!confirm('你确认要清理短消息吗?')) {
            return false;
        }
        $("DeletePM").disabled = true;
        $('Layer5').innerHTML = "<br />正在清理短消息...";
        $('success').style.display = "block";
        count = 0;
        DeletePM(0);   
    }
</script>

</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<fieldset>
<legend style="background:url(../images/icons/icon51.jpg) no-repeat 6px 50%;">清理短消息</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2"></td></tr>
	<tr>
		<td class="vtop rowform">
			 <input type="checkbox" name="isnew" value="1" id="isnew" checked runat="server" /> 不删除未读信息
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">删除多少天以前的短消息</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="postdatetime" runat="server" RequiredFieldType="数据校验" Width="40"></cc1:TextBox>
		</td>
		<td class="vtop">不限制时间请输入</td>
	</tr>
	<tr><td class="item_title" colspan="2">按发信用户名清理</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="msgfromlist" runat="server" RequiredFieldType="暂无校验" Width="200"></cc1:TextBox> &nbsp; 
			&nbsp;<input type="checkbox" name="lowerupper" value="1" id="lowerupper" runat="server"> 不区分大小写
		</td>
		<td class="vtop">多用户名中间请用半角逗号 "," 分割</td>
	</tr>
	<tr><td class="item_title" colspan="2">按关键字搜索主题</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="subject" runat="server" RequiredFieldType="暂无校验" Width="200"></cc1:TextBox>
		</td>
		<td class="vtop">关键字中间用","分割</td>
	</tr>
	<tr><td class="item_title" colspan="2">按关键字搜索全文</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="message" runat="server" RequiredFieldType="暂无校验" Width="200"></cc1:TextBox>
		</td>
		<td class="vtop">关键字中间用","分割</td>
	</tr>
</table>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div class="Navbutton">
<button id="DeletePM" type="button" class="ManagerButton" onclick="javascript:submit_Click();"><img src="../images/submit.gif" />删除短消息</button>
<input type="checkbox" name="isupdateusernewpm" value="1" id="isupdateusernewpm" checked runat="server" />同时更新收件人新短消息数</div>
</div>
</fieldset>
</form>	
<script type="text/javascript" src="../js/AjaxHelper.js"></script>


<%=footer%>
</div>
</body>
</html>