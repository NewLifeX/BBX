<%@ Page Language="c#" Inherits="BBX.Web.Admin.safecontrol" CodeBehind="global_safecontrol.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>安全与防灌水</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
	function setstatus(obj) {
		if (obj.id == "seccodestatus_select_5") {
			document.getElementById("seccodestatus_select_2").checked = obj.checked;
			document.getElementById("seccodestatus_select_2").disabled = obj.checked;
			document.getElementById("seccodestatus_select_12").checked = obj.checked;
			document.getElementById("seccodestatus_select_12").disabled = obj.checked;
		}

		if (obj.id == "seccodestatus_select_2") {
			document.getElementById("seccodestatus_select_5").checked = obj.checked;
			document.getElementById("seccodestatus_select_5").disabled = obj.checked;
			document.getElementById("seccodestatus_select_12").checked = obj.checked;
			document.getElementById("seccodestatus_select_12").disabled = obj.checked;
		}

		if (obj.id == "seccodestatus_select_6") {
			document.getElementById("seccodestatus_select_3").checked = obj.checked;
			document.getElementById("seccodestatus_select_3").disabled = obj.checked;
		}
		if (obj.id == "seccodestatus_select_12") {
			document.getElementById("seccodestatus_select_2").checked = obj.checked;
			document.getElementById("seccodestatus_select_2").disabled = obj.checked;
			document.getElementById("seccodestatus_select_5").checked = obj.checked;
			document.getElementById("seccodestatus_select_5").disabled = obj.checked;
		}
		
		checkselecedpage();
	}
</script>
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<fieldset>
<legend style="background: url(../images/icons/icon22.jpg) no-repeat 6px 50%;">安全与防灌水</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">验证码显示方式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:DropDownList ID="VerifyImage" runat="server"></cc1:DropDownList>		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">使用验证码的页面列表</td></tr>
	<tr>
		<td colspan="2" class="vtop">
		<div style="display:none">
			<cc1:TextBox ID="seccodestatus" runat="server" HintShowType="down" CssClass="" RequiredFieldType="暂无校验" TextMode="MultiLine" Height="90px" Rows="4" Cols="30">			</cc1:TextBox>
		</div>
	      <table id="seccodestatus_select" width="100%">
	        <tr>
	          <td>
	            <input id="seccodestatus_select_0" type="checkbox" name="seccodestatus_select:0" onClick="checkselecedpage()" value="register" />
	            <label for="seccodestatus_select_0">新用户注册</label>              </td>
			  <td>
			    <input id="seccodestatus_select_5" type="checkbox" name="seccodestatus_select:5" onClick="setstatus(this);" value="showforum" />
			    <label for="seccodestatus_select_5">查看(已设置密码)版块</label>		      </td>
			  <td>
			    <input id="seccodestatus_select_1" type="checkbox" name="seccodestatus_select:1" onClick="checkselecedpage();" value="login" />
			    <label for="seccodestatus_select_1">用户登录</label>		      </td>
		  </tr>
	        <tr>
	          <td>
	            <input id="seccodestatus_select_6" type="checkbox" name="seccodestatus_select:6" onClick="setstatus(this)" value="showtopic" />
	            <label for="seccodestatus_select_6">快速回复(主题页)</label>              </td>
			  <td>
			    <input id="seccodestatus_select_2" type="checkbox" name="seccodestatus_select:2" onClick="setstatus(this);" value="posttopic" />
			    <label for="seccodestatus_select_2">发表主题</label>		      </td>
			  <td>
			    <input id="seccodestatus_select_7" type="checkbox" name="seccodestatus_select:7" onClick="checkselecedpage()" value="usercpprofile" />
			    <label for="seccodestatus_select_7">修改个人密码</label>		      </td>
		  </tr>
	        <tr>
	          <td>
	            <input id="seccodestatus_select_3" type="checkbox" name="seccodestatus_select:3" onClick="checkselecedpage()" value="postreply" />
	            <label for="seccodestatus_select_3">发表回复</label>              </td>
			  <td>
			    <input id="seccodestatus_select_8" type="checkbox" name="seccodestatus_select:8" onClick="checkselecedpage()" value="editpost" />
			    <label for="seccodestatus_select_8">修改帖子</label>		      </td>
			  <td>
			    <input id="seccodestatus_select_4" type="checkbox" name="seccodestatus_select:4" onClick="checkselecedpage()" value="usercppostpm" />
			    <label for="seccodestatus_select_4">发送短消息</label>		      </td>
            </tr>
	        <tr>
	          <td>
	            <input id="seccodestatus_select_9" type="checkbox" name="seccodestatus_select:9" onClick="checkselecedpage()" value="editgoods" />
	            <label for="seccodestatus_select_9">编辑商品</label>              </td>
			  <td>
			    <input id="seccodestatus_select_10" type="checkbox" name="seccodestatus_select:10" onClick="checkselecedpage()" value="showgoods" />
			    <label for="seccodestatus_select_10">显示商品</label>		      </td>
			  <td>
			    <input id="seccodestatus_select_11" type="checkbox" name="seccodestatus_select:11" onClick="checkselecedpage()" value="postgoods" />
			    <label for="seccodestatus_select_11">发送商品</label>		      </td>
		  </tr>
	        <tr>
	          <td>
	            <input id="seccodestatus_select_12" type="checkbox" name="seccodestatus_select:12" onClick="setstatus(this);" value="quicklyposttpoic" />
	            <label for="seccodestatus_select_12">首页快速发帖</label>              </td>
			  <td>		      </td>
			  <td>		      </td>
		  </tr>
              </table>
	      请选取相应的页面复选框, 并在相应页面模板表单中加入{_vcode}校验码子模板, 就可以增加校验码判断功能.		</td>
	  </tr>
	<tr><td class="item_title" colspan="2">用户登录时是否启用安全问题:</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="secques" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1" Selected>是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop">用户登录时是否启用安全问题</td>
	</tr>
	<tr><td class="item_title" colspan="2">发帖灌水预防</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox SetFocusButtonID="SaveInfo" ID="postinterval" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Text="15" MaxLength="5" Size="6"></cc1:TextBox>(单位:秒)		</td>
		<td class="vtop">两次发帖间隔小于此时间, 或两次发送短消息间隔小于此时间的二倍将被禁止, 0 为不限制题</td>
	</tr>
	<tr><td class="item_title" colspan="2">60 秒最大搜索次数</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox SetFocusButtonID="SaveInfo" ID="maxspm" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Text="0" MaxLength="3" Size="4"></cc1:TextBox>		</td>
		<td class="vtop">论坛系统每 60 秒系统响应的最大搜索次数, 0 为不限制. 注意: 如果服务器负担较重, 建议设置为 5, 或在 5~20 范围内取值, 以避免过于频繁的搜索造成数据表被锁</td>
	</tr>
	<tr style="display:none"><td class="item_title" colspan="2">是否使用管理员客户端工具:</td></tr>
	<tr style="display:none">
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="admintools" runat="server" RepeatColumns="1" RepeatLayout="flow" HintPosOffSet="180">
				<asp:ListItem Value="0">不使用</asp:ListItem>
				<asp:ListItem Value="1">仅论坛创始人可用</asp:ListItem>
				<asp:ListItem Value="2">管理员可用</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop">是否使用管理员客户端工具</td>
	</tr>
	
	<tr><td class="item_title" colspan="2">防注册机用户名设置</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox SetFocusButtonID="SaveInfo" id="antispamusername" runat="server" Text="0" MaxLength="10" Size="10"></cc1:TextBox>		</td>
		<td class="vtop">注册页面提交表单中用户名的字符设置，默认是"username"，改变后可以有效防止注册机，例如"user1name"</td>
	</tr>	
	<tr><td class="item_title" colspan="2">防注册机Email设置</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox SetFocusButtonID="SaveInfo" id="antispamemail" runat="server" Text="0" MaxLength="10" Size="10"></cc1:TextBox>		</td>
		<td class="vtop">注册页面提交表单中Email的字符设置，默认是"email"，改变后可以有效防止注册机</td>
	</tr>	
	<tr><td class="item_title" colspan="2">防灌水机标题设置</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox SetFocusButtonID="SaveInfo" id="antispamtitle" runat="server" Text="0" MaxLength="10" Size="10"></cc1:TextBox>		</td>
		<td class="vtop">发帖页面提交表单中标题的字符设置，默认是"title"，改变后可以有效防止灌水机，例如"t1tle"</td>
	</tr>	
	<tr><td class="item_title" colspan="2">防灌水机内容设置</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox SetFocusButtonID="SaveInfo" id="antispammessage" runat="server" Text="0" MaxLength="10" Size="10" ></cc1:TextBox>		</td>
		<td class="vtop">发帖页面提交表单中标题的字符设置，默认是"message"，改变后可以有效防止灌水机，例如"mes5age"</td>
	</tr>
		<tr><td class="item_title" colspan="2">新用户广告强力屏蔽</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="disablepostad" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList><br />
			<div id="postadstatus" runat="server">
				<table width="100%">
				<tr>
				<td>注册分钟:</td>
				<td><cc1:TextBox id="disablepostadregminute" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Size="6" MaxLength="4" HintTitle="提示" 
					HintInfo="用户注册N分钟内进行新用户广告强力屏蔽功能检查,0为不行进该项检查"></cc1:TextBox>(分钟)				 </td>
				 </tr>
				 <tr>
				 <td>发帖数:</td>
				 <td><cc1:TextBox id="disablepostadpostcount" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Size="6" MaxLength="4" HintTitle="提示" 
					HintInfo="用户发帖N帖内进行新用户广告强力屏蔽功能检查,0为不行进该项检查"></cc1:TextBox>(帖)				 </td>
				 </tr>
				 <tr>
				 <td colspan="2">正则式:</td>
				 </tr>
				 <tr>
				 <td colspan="2">
					 <uc1:TextareaResize id="disablepostadregular" runat="server"  cols="35" controlname="disablepostadregular" HintTitle="提示" 
						HintInfo="用于对新用户进行广告屏蔽的正则表达式,每条正则表达式用回车符间隔" HintPosOffSet="160"></uc1:TextareaResize>				 </td>
				 </tr>                            
				</table>
			</div>		</td>
		<td class="vtop">是否启用新用户广告强力屏蔽功能</td>
	</tr>
    </table>
	<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
	<div class="Navbutton">
		<cc1:Button ID="SaveInfo" runat="server" Text=" 提 交 ">
		</cc1:Button>
	</div>
</fieldset>
<script type="text/javascript">
var seccodestatus = '<%=ViewState["Seccodestatus"].ToString().Trim()%>';
loadseccodestatus();
function loadseccodestatus() {
	if (seccodestatus.indexOf('register') >= 0) {
		document.getElementById("seccodestatus_select_0").checked = true;
	}
	if (seccodestatus.indexOf('login') >= 0) {
		document.getElementById("seccodestatus_select_1").checked = true;
	}
	if (seccodestatus.indexOf('posttopic') >= 0) {
		document.getElementById("seccodestatus_select_2").checked = true;
	}
	if (seccodestatus.indexOf('postreply') >= 0) {
		document.getElementById("seccodestatus_select_3").checked = true;
	}
	if (seccodestatus.indexOf('usercppostpm') >= 0) {
		document.getElementById("seccodestatus_select_4").checked = true;
	}
	if (seccodestatus.indexOf('showforum') >= 0) {
		document.getElementById("seccodestatus_select_5").checked = true;
	}
	if (seccodestatus.indexOf('showtopic') >= 0) {
		document.getElementById("seccodestatus_select_6").checked = true;
	}
	if (seccodestatus.indexOf('usercpnewpassword') >= 0) {
		document.getElementById("seccodestatus_select_7").checked = true;
	}
	if (seccodestatus.indexOf('editpost') >= 0) {
		document.getElementById("seccodestatus_select_8").checked = true;
	}
	/*if(seccodestatus.indexOf('website')>=0) 
	{
	document.getElementById("seccodestatus_select_9").checked=true;
	}*/
	if (seccodestatus.indexOf('editgoods') >= 0) {
		document.getElementById("seccodestatus_select_9").checked = true;
	}
	if (seccodestatus.indexOf('showgoods') >= 0) {
		document.getElementById("seccodestatus_select_10").checked = true;
	}
	if (seccodestatus.indexOf('postgoods') >= 0) {
		document.getElementById("seccodestatus_select_11").checked = true;
	}
	if (seccodestatus.indexOf('forumindex') >= 0) {
		document.getElementById("seccodestatus_select_12").checked = true;
	}
}

function checkselecedpage() {
	document.getElementById("seccodestatus").value = '';
	var selectstr = '';
	if (document.getElementById("seccodestatus_select_0").checked) {
		selectstr += 'register.aspx\r\n';
	}
	if (document.getElementById("seccodestatus_select_1").checked) {
		selectstr += 'login.aspx\r\n';
	}
	if (document.getElementById("seccodestatus_select_2").checked) {
		selectstr += 'posttopic.aspx\r\n';
	}
	if (document.getElementById("seccodestatus_select_3").checked) {
		selectstr += 'postreply.aspx\r\n';
	}
	if (document.getElementById("seccodestatus_select_4").checked) {
		selectstr += 'usercppostpm.aspx\r\nusercpshowpm.aspx\r\n';
	}
	if (document.getElementById("seccodestatus_select_5").checked) {
		selectstr += 'showforum.aspx\r\n';
	}
	if (document.getElementById("seccodestatus_select_6").checked) {
		selectstr += 'showtopic.aspx\r\najax.ashx\r\nshowdebate.aspx\r\nshowtree.aspx\r\nshowbonus.aspx\r\n';
	}
	if (document.getElementById("seccodestatus_select_7").checked) {
		selectstr += 'usercpnewpassword.aspx\r\n';
	}
	if (document.getElementById("seccodestatus_select_8").checked) {
		selectstr += 'editpost.aspx\r\n';
	}
	/*if(document.getElementById("seccodestatus_select_9").checked)
	{
	selectstr+='website.aspx\r\n';
	}*/
	if (document.getElementById("seccodestatus_select_9").checked) {
		selectstr += 'editgoods.aspx\r\n';
	}
	if (document.getElementById("seccodestatus_select_10").checked) {
		selectstr += 'showgoods.aspx\r\n';
	}
	if (document.getElementById("seccodestatus_select_11").checked) {
		selectstr += 'postgoods.aspx\r\n';
	}
	if (document.getElementById("seccodestatus_select_12").checked) {
		selectstr += 'forumindex.aspx\r\n';
	}
	document.getElementById("seccodestatus").value = selectstr;
}
</script>
 </form>
</div>
<%=footer%>
</body>
</html>