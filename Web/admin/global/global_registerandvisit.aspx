<%@ Page Language="c#" Inherits="BBX.Web.Admin.registerandvisit" Codebehind="global_registerandvisit.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<%@ Register TagPrefix="uc1" TagName="OnlineEditor" Src="../UserControls/OnlineEditor.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>registerandvisit</title>
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../../javascript/common.js"></script>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
    <link href="../styles/default.css" rel="stylesheet" type="text/css" id="css" />
    <link href="../styles/editor.css" rel="stylesheet" type="text/css" id="Link1" />
    <script type="text/javascript" src="../../javascript/post.js"></script>
    <script type="text/javascript"  src="../../javascript/template_showtopic.js"></script>
    <script type="text/javascript"  src="../../javascript/ajax.js"></script>
	<link href="../../templates/default/seditor.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <script type="text/javascript">
        function setStatus() {
            var i = false;
            if (document.getElementById("regstatus_1").checked || document.getElementById("regstatus_2").checked)
                i = true;
            document.getElementById("invitation").style.display = (i) ? "block" : "none";
        }
		function bindhtml(){
		    $('invitationvisitordescriptionmessage_hidden').value = parseubb($('invitationvisitordescriptionmessage').value);
		    $('invitationuserdescriptionmessage_hidden').value = parseubb($('invitationuserdescriptionmessage').value);
		    $('invitationemailmodelmessage_hidden').value = parseubb($('invitationemailmodelmessage').value);
		    $('verifyemailtempmessage_hidden').value = parseubb($('verifyemailtempmessage').value);
		}
		
    </script>
</head>
<body>
<div class="ManagerForm" style="margin:4px 30px 10px;">
	<form id="Form1" method="post" runat="server" onsubmit="bindhtml()">
	<fieldset>
	<legend style="background: url(../images/icons/icon21.jpg) no-repeat 6px 50%;">用户注册设置</legend>
	<table width="100%">
		<tr><td class="item_title" colspan="2">新用户注册设置</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:RadioButtonList ID="regstatus" runat="server" RepeatColumns=1 RepeatLayout="flow">
					<asp:ListItem Value="1">允许新用户注册</asp:ListItem>
					<asp:ListItem Value="2">允许常规注册及邀请链接式注册</asp:ListItem>
					<asp:ListItem Value="3">关闭注册，仅通过邀请码注册</asp:ListItem>
					<asp:ListItem Value="0">不允许任何新用户注册</asp:ListItem>
				</cc1:RadioButtonList>
			</td>
			<td class="vtop">“邀请链接式注册”是站点推广的很有效的手段，对已注册用户给予论坛积分奖励，让其通过系统生成的推广链接邀请其他用户访问，并引导其注册，每注册成功一位，
			给予该链接所有用户奖励积分，关于奖励积分设置，请到<a href="global_scoreset.aspx">积分设置</a>处设置；“仅通过邀请码注册”是控制论坛注册人数，或有定向性的允许访客注册的解决方案，该方案开启后，访客必须拥有
			邀请码才可以注册成功，邀请码由已注册用户在invite.aspx页面购买；不允许新用户注册选项不会影响已注册用户。</td>
		</tr>
		<tr><td class="item_title" colspan="2">用户资料中是否必须填写实名选项</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:RadioButtonList ID="realnamesystem" runat="server" RepeatLayout="flow">
					<asp:ListItem Value="1">是</asp:ListItem>
					<asp:ListItem Value="0">否</asp:ListItem>
				</cc1:RadioButtonList>
			</td>
			<td class="vtop">选择"是"填写实名将出现在必填内容区,否则将出现在选填内容区</td>
		</tr>
		<tr><td class="item_title" colspan="2">用户信息保留关键字</td></tr>
		<tr>
			<td class="vtop rowform">
				<uc1:TextareaResize ID="censoruser" runat="server" cols="30" HintTopOffSet="-110" controlname="censoruser" >
				</uc1:TextareaResize>
			</td>
			<td class="vtop">用户在其用户信息(如用户名、昵称、自定义头衔等)中无法使用这些关键字. 每个关键字一行, 可使用通配符 "*" 如 "*版主*"(不含引号)</td>
		</tr>
		<tr><td class="item_title" colspan="2">新用户注册验证</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:RadioButtonList ID="regverify" runat="server" RepeatColumns="3" RepeatLayout="flow">
					<asp:ListItem Value="0" Selected="True">无</asp:ListItem>
					<asp:ListItem Value="1">Email验证</asp:ListItem>
					<asp:ListItem Value="2">人工审核</asp:ListItem>
				</cc1:RadioButtonList>
			</td>
			<td class="vtop">选择"无"用户可直接注册成功;选择"Email 验证"将向用户注册 Email 发送一封验证邮件以确认邮箱的有效性;选择"人工审核"将由管理员人工逐个确定是否允许新用户注册</td>
		</tr>
		<tr><td class="item_title" colspan="2">Email 允许地址</td></tr>
		<tr>
			<td class="vtop rowform">
				<uc1:TextareaResize ID="accessemail" runat="server" cols="30" controlname="accessemail"></uc1:TextareaResize>
			</td>
			<td class="vtop">只允许某些域名结尾的邮箱注册, 每行一个域名, 例如 @hotmail.com.注意:此项开启时, 下面的"Email 禁止地址"项设置无效</td>
		</tr>
		<tr><td class="item_title" colspan="2">Email 禁止地址</td></tr>
		<tr>
			<td class="vtop rowform">
				<uc1:TextareaResize ID="censoremail" runat="server" cols="30" controlname="censoremail">
				</uc1:TextareaResize>
			</td>
			<td class="vtop">由于一些大型邮件服务提供商会过滤论坛程序发送的有效邮件, 您可以要求新用户不得以某些域名结尾的邮箱注册, 每行一个域名, 例如 @hotmail.com</td>
		</tr>
		<tr><td class="item_title" colspan="2">IP注册间隔限制</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:TextBox ID="regctrl" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox>(单位:小时)
				<asp:RegularExpressionValidator ID="mycheck" runat="SERVER" ControlToValidate="regctrl"
					ErrorMessage="请输入正整数" ValidationExpression="^[0-9]*$">
				</asp:RegularExpressionValidator>
			</td>
			<td class="vtop">同一 IP 在本时间间隔内将只能注册一个帐号, 限制对自修改后的新注册用户生效, 0 为不限制</td>
		</tr>
        <tr><td class="item_title" colspan="2">Email验证请求信息有效期</td></tr>
        <tr>
            <td class="vtop rowform">
                <cc1:TextBox ID="verifyregisterexpired" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox>(单位:天)
				<asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="SERVER" ControlToValidate="verifyregisterexpired"
					ErrorMessage="请输入正整数" ValidationExpression="^[0-9]*$">
				</asp:RegularExpressionValidator>
            </td>
            <td class="vtop">开启Email验证后,设置发送给用户的验证注册链接的有效期限,0为不过期</td>
        </tr>
        <tr><td class="item_title" colspan="2">Email验证请求信息邮件内容模板</td></tr>
        <tr>
            <td class="vtop" colspan="2">
                <uc1:OnlineEditor ID="verifyemailtemp" runat="server" controlname="verifyemailtemp" postminchars="0" postmaxchars="200"></uc1:OnlineEditor>
	        </td>
        </tr>
        <tr><td class="vtop" colspan="2">发送给用户的含有注册链接的邮件内容模板,变量说明,{0}请求用户的Email用户名,{1}注册链接(该变量要求必须在内容当中)</td></tr>
		<tr><td class="item_title" colspan="2">新手见习期限</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:TextBox ID="newbiespan" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox>(单位:分钟)
				<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="SERVER" ControlToValidate="newbiespan"
					ErrorMessage="请输入正整数" ValidationExpression="^[0-9]*$">
				</asp:RegularExpressionValidator>
			</td>
			<td class="vtop">新注册用户在本期限内将无法发帖, 不影响版主和管理员, 0 为不限制</td>
		</tr>
		<tr><td class="item_title" colspan="2">允许同一 Email注册不同用户</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:RadioButtonList ID="doublee" runat="server" RepeatLayout="flow">
					<asp:ListItem Value="1">是</asp:ListItem>
					<asp:ListItem Value="0">否</asp:ListItem>
				</cc1:RadioButtonList>
			</td>
			<td class="vtop">选择"否" ,一个 Email 地址只能注册一个用户名</td>
		</tr>
		<tr><td class="item_title" colspan="2">是否允许Email登录</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:RadioButtonList ID="emaillogin" runat="server" RepeatLayout="flow">
					<asp:ListItem Value="1">是</asp:ListItem>
					<asp:ListItem Value="0">否</asp:ListItem>
				</cc1:RadioButtonList>
			</td>
			<td class="vtop">启用后，Email和用户名都可进行登录</td>
		</tr>
		<tr><td class="item_title" colspan="2">特殊 IP 注册限制</td></tr>
		<tr>
			<td class="vtop rowform">
				 <uc1:TextareaResize ID="ipregctrl" runat="server" cols="30" controlname="ipregctrl"></uc1:TextareaResize>
			</td>
			<td class="vtop">当用户处于本列表中的 IP 地址时, 每 72 小时将至多只允许注册一个帐号. 每个 IP 一行, 例如 "192.168.*.*"(不含引号) 可匹配 192.168.0.0~192.168.255.255范围内的所有地址, 留空为不设置</td>
		</tr>
		<tr><td class="item_title" colspan="2">注册许可协议</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:RadioButtonList ID="rules" runat="server" RepeatLayout="flow">
					<asp:ListItem Value="1">是</asp:ListItem>
					<asp:ListItem Value="0">否</asp:ListItem>
				</cc1:RadioButtonList>
			</td>
			<td class="vtop">新用户注册时显示许可协议</td>
		</tr>
		<tr><td class="item_title" colspan="2">许可协议内容</td></tr>
		<tr>
			<td class="vtop rowform">
				<uc1:TextareaResize ID="rulestxt" runat="server" label="" controlname="rulestxt" cols="30"></uc1:TextareaResize>
			</td>
			<td class="vtop">注册许可协议的详细内容</td>
		</tr>
		<tr><td class="item_title" colspan="2">发送欢迎短消息</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:RadioButtonList ID="welcomemsg" runat="server" RepeatLayout="flow">
					<asp:ListItem Value="1">是</asp:ListItem>
					<asp:ListItem Value="0">否</asp:ListItem>
				</cc1:RadioButtonList>
			</td>
			<td class="vtop">选择"是"将自动向新注册用户发送一条欢迎短消息</td>
		</tr>
		<tr><td class="item_title" colspan="2">欢迎短消息内容</td></tr>
		<tr>
			<td class="vtop rowform">
				 <uc1:TextareaResize ID="welcomemsgtxt" runat="server" cols="30" HintTopOffSet="-50" controlname="welcomemsgtxt"></uc1:TextareaResize>
			</td>
			<td class="vtop">系统发送的欢迎短消息的内容</td>
		</tr>
			<tr><td class="item_title" colspan="2">密码模式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="passwordmode" runat="server" RepeatLayout="flow" RepeatColumns="3">
			<asp:ListItem Value="0"  Selected=true>默认</asp:ListItem>
			<asp:ListItem Value="1">动网兼容模式</asp:ListItem>
			<asp:ListItem Value="2">第三方模式</asp:ListItem>
		</cc1:RadioButtonList>
		</td>
		<td class="vtop">注意: 动网兼容模式只适用于从动网论坛(或LeadBBS和雪人论坛等)转换而来的论坛用户数据.非从第三方转换的论坛请勿使用第三方模式[<a href="http://www.newlifex.com/doc/default.aspx?cid=71" target="_blank">详细介绍</a>]</td>
	</tr>
	<tr><td class="item_title" colspan="2">身份验证Cookie域</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="CookieDomain" runat="server" Text="" Size="30" RequiredFieldType="暂无校验"></cc1:TextBox>
		</td>
		<td class="vtop">如需所有子域共享此Cookie, 例如:<br />要让www.abc.com 与 bbs.abc.com共享论坛Cookie,则请设置此处为 .abc.com</td>
	</tr>
        </table>
	</fieldset>
	<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
	<fieldset>
		<legend style="background: url(../images/icons/icon17.jpg) no-repeat 6px 50%;">访问控制</legend>
		<table width="100%">
		<tr><td class="item_title" colspan="2">隐藏无权访问的论坛</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:RadioButtonList ID="hideprivate" runat="server" RepeatLayout="Flow">
					<asp:ListItem Value="1">是</asp:ListItem>
					<asp:ListItem Value="0">否</asp:ListItem>
				</cc1:RadioButtonList>
			</td>
			<td class="vtop">不在列表中显示当前用户无权访问的论坛</td>
		</tr>
		<tr><td class="item_title" colspan="2">IP 访问列表</td></tr>
		<tr>
			<td class="vtop rowform">
				<uc1:TextareaResize ID="ipaccess" runat="server" cols="30" controlname="ipaccess"></uc1:TextareaResize>
			</td>
			<td class="vtop">只有当用户处于本列表中的 IP 地址时才可以访问本论坛, 列表以外的地址访问将视为 IP 被禁止, 仅适用于诸如企业、学校内部论坛等极个别场合. 本功能对管理员没有特例, 如果管理员不在此列表范围内将同样不能登录, 请务必慎重使用本功能. 每个 IP 一行, 例如 "192.168.*.*"(不含引号) 可匹配 192.168.0.0~192.168.255.255 范围内的所有地址, 留空为所有 IP 除明确禁止的以外均可访问</td>
		</tr>
		<tr><td class="item_title" colspan="2">IP 禁止访问列表</td></tr>
		<tr>
			<td class="vtop rowform">
				<uc1:TextareaResize ID="ipdenyaccess" runat="server" cols="30" controlname="ipdenyaccess"></uc1:TextareaResize>
			</td>
			<td class="vtop">当用户处于本列表中的 IP 地址时将禁止访问本论坛. 每个 IP 一行, 例如 "192.168.*.*"(不含引号) 可匹配 192.168.0.0~192.168.255.255 范围内的所有地址</td>
		</tr>
		<tr><td class="item_title" colspan="2">管理员后台IP访问列表</td></tr>
		<tr>
			<td class="vtop rowform">
				<uc1:TextareaResize ID="adminipaccess" runat="server" cols="30" controlname="adminipaccess"></uc1:TextareaResize>
			</td>
			<td class="vtop">只有当管理员(超级版主及版主不在此列)处于本列表中的 IP 地址时才可以访问论坛系统设置, 列表以外的地址访问将无法访问, 但仍可访问论坛前端用户界面, 请务必慎重使用本功能. 每个 IP 一行, 例如 "192.168.*.*"(不含引号) 可匹配 192.168.0.0~192.168.255.255 范围内的所有地址, 留空为所有 IP 除明确禁止的以外均可访问系统设置</td>
		</tr>
		</table>
	</fieldset>
	<fieldset id="invitation">
	    <legend style="background: url(../images/icons/icon21.jpg) no-repeat 6px 50%;">邀请注册控制</legend>
	    <table width="100%">
	     <tr><td class="item_title" colspan="2">邀请链接和邀请码使用期限</td></tr>
	     <tr>
	         <td class="vtop rowform">
	            <cc1:TextBox ID="invitecodeexpiretime" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox>天
				<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="SERVER" ControlToValidate="invitecodeexpiretime"
					ErrorMessage="请输入正整数" ValidationExpression="^[0-9]*$">
				</asp:RegularExpressionValidator>
	         </td>
	         <td class="vtop">设置生成的邀请链接和邀请码最多可以使用多少天，过期后将不能继续使用(0为不设置过期时间，该设置无法影响已生成的邀请链接或码)</td>
	     </tr>
	     <tr><td class="item_title" colspan="2">邀请链接的兑换积分及格线设置</td></tr>
	     <tr>
	         <td class="vtop rowform">
	            <cc1:TextBox ID="addextcreditsline" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox>人
				<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="SERVER" ControlToValidate="addextcreditsline"
					ErrorMessage="请输入正整数" ValidationExpression="^[0-9]*$">
				</asp:RegularExpressionValidator>
	         </td>
	         <td class="vtop">设置邀请链接被成功使用注册的人次达到多少人时才能允许兑换，如果设置为5，则该链接成功邀请到5个人注册之后才能兑换到相应积分。0为不限制</td>
	     </tr>
	     <tr><td class="item_title" colspan="2">邀请链接或码最大使用次数</td></tr>
	     <tr>
	         <td class="vtop rowform">
	            <cc1:TextBox ID="invitecodemaxcount" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox>次
				<asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="SERVER" ControlToValidate="invitecodemaxcount"
					ErrorMessage="请输入正整数" ValidationExpression="^[0-9]*$">
				</asp:RegularExpressionValidator>
	         </td>
	         <td class="vtop">设置邀请链接/码使用次数上限，对于邀请链接来说，该设置可以限制用户邀请链接的成功使用次数，对于邀请码，该设置可以影响到用户购买的邀请码的使用次数。(对邀请链接功能，0为无限制，对于邀请码功能，0为可使用1次，该设置无法影响已生成的邀请码)</td>
	     </tr>
	     <tr><td class="item_title" colspan="2">用户最多拥有邀请码数量</td></tr>
	     <tr>
	         <td class="vtop rowform">
	            <cc1:TextBox ID="invitecodeusermaxbuy" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox>个
				<asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="SERVER" ControlToValidate="invitecodeusermaxbuy"
					ErrorMessage="请输入正整数" ValidationExpression="^[0-9]*$">
				</asp:RegularExpressionValidator>
	         </td>
	         <td class="vtop">设置用户最多可以购买到多少个邀请码（0为不允许用户购买邀请码，该设置方式可用于在不关闭邀请码功能的前提下消耗用户已有邀请码）</td>
	     </tr>
	     <tr><td class="item_title" colspan="2">用户每天可以申请到的邀请链接数量</td></tr>
	     <tr>
	         <td class="vtop rowform">
	            <cc1:TextBox ID="invitecodeusercreateperday" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox>个
				<asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="SERVER" ControlToValidate="invitecodeusercreateperday"
					ErrorMessage="请输入正整数" ValidationExpression="^[0-9]*$">
				</asp:RegularExpressionValidator>
	         </td>
	         <td class="vtop">设置用户每天可以申请到的邀请链接数量,主要用于防范用户恶意刷积分。(0为不限制)</td>
	     </tr>
	     <tr><td class="item_title" colspan="2">邀请码价格设置</td></tr>
	     <tr>
	         <td class="vtop rowform">
	         <%=extCreditsName[0]%> <cc1:TextBox ID="invitecodeprice0" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox><%if (extCreditsName[0] != ""){%>&nbsp; <%=extCreditsUnits[1]%><br /><%}%>
	         <%=extCreditsName[1]%> <cc1:TextBox ID="invitecodeprice1" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox><%if (extCreditsName[1] != ""){%>&nbsp; <%=extCreditsUnits[2]%><br /><%}%>
	         <%=extCreditsName[2]%> <cc1:TextBox ID="invitecodeprice2" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox><%if (extCreditsName[2] != ""){%>&nbsp; <%=extCreditsUnits[3]%><br /><%}%>
	         <%=extCreditsName[3]%> <cc1:TextBox ID="invitecodeprice3" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox><%if (extCreditsName[3] != ""){%>&nbsp; <%=extCreditsUnits[4]%><br /><%}%>
	         <%=extCreditsName[4]%> <cc1:TextBox ID="invitecodeprice4" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox><%if (extCreditsName[4] != ""){%>&nbsp; <%=extCreditsUnits[5]%><br /><%}%>
	         <%=extCreditsName[5]%> <cc1:TextBox ID="invitecodeprice5" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox><%if (extCreditsName[5] != ""){%>&nbsp; <%=extCreditsUnits[6]%><br /><%}%>
	         <%=extCreditsName[6]%> <cc1:TextBox ID="invitecodeprice6" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox><%if (extCreditsName[6] != ""){%>&nbsp; <%=extCreditsUnits[7]%><br /><%}%>
	         <%=extCreditsName[7]%> <cc1:TextBox ID="invitecodeprice7" runat="server" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox><%if (extCreditsName[7] != ""){%>&nbsp; <%=extCreditsUnits[8]%><br /><%}%>
	         </td>
	         <td class="vtop">邀请码注册功能，用户获取邀请码需要支付的扩展积分。</td>
	     </tr>
	     <tr><td class="item_title" colspan="2">邀请功能说明(针对注册用户)</td></tr>
	     <tr>
	         <td class="vtop" colspan="2">
<uc1:OnlineEditor ID="invitationuserdescription" runat="server" controlname="invitationuserdescription" postminchars="0" postmaxchars="200"></uc1:OnlineEditor>			 
			 </td>
		 </tr>
		 <tr>
	         <td class="vtop" colspan="2">当注册用户访问invite页面，需要让注册用户了解的一些事项填写</td>
	     </tr>
	     <tr><td class="item_title" colspan="2">邀请功能说明(针对游客)</td></tr>
	     <tr>
	         <td class="vtop" colspan="2">
 <uc1:OnlineEditor ID="invitationvisitordescription" runat="server" controlname="invitationvisitordescription" postminchars="0" postmaxchars="200"></uc1:OnlineEditor>
	         </td>
		 </tr>
		 <tr>
	         <td class="vtop" colspan="2">当游客通过邀请链接或者自己访问invite页面，需要让游客了解的一些事项填写</td>
	     </tr>
	     <tr><td class="item_title" colspan="2">E-mail推荐信模板</td></tr>
	     <tr>
	         <td class="vtop" colspan="2">
<uc1:OnlineEditor ID="invitationemailmodel" runat="server" controlname="invitationemailmodel" postminchars="0" postmaxchars="200"></uc1:OnlineEditor>
	         </td>
	     </tr>
	     <tr>
	     <td class="vtop" colspan="2">注册用户通过站内邮件推荐功能推荐给其好友的邮件内容定制。内容可用变量说明：{0}：收件人Email地址；{1}：发送人用户id；{2}：发送人用户名；{3}：邀请链接；{4}：论坛名称；{5}：邀请附言（当该参数不被使用时，前台页面不会出现邀请附言的填写项目）;{6}获取论坛URL地址 ; {7}预览小头像 ; {8}预览中头像 ; {9}预览大头像</td>
	     </tr>
	    </table>
	</fieldset>
	<div class="Navbutton">
		<cc1:Button ID="SaveInfo" runat="server" Text=" 提 交 " ValidateForm="true"></cc1:Button>
	</div>
	<script type="text/javascript">
	    setStatus();
    </script>
	</form>
    </div>
    <%=footer%>
</body>
</html>