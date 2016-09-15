<%@ Page language="c#" Inherits="BBX.Web.Admin.syslogin"  EnableViewstate ="false" Codebehind="syslogin.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<HTML>
<HEAD>
<title>管理员控制台登录</title>
<link href="styles/dntmanager.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="js/common.js"></script>
<script type="text/javascript">
if(top.location!=self.location)
{
	top.location.href = "syslogin.aspx";
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<BODY style="background:#f4f6f7;">
	<FORM id="Form1" method="post" runat="server">
	 <div id="LoginBar">
		<ul>
		   <span class="td1">
				<asp:literal id="Msg" runat="server"></asp:literal>
			    <br />
				<br />
				</span>
			<li class="LoginTop"><img src="images/login-top.gif"/></li>
			<li class="FormNav">
				<dl>
					<dt><label>用户名:</label><cc1:textbox id="UserName" runat="server" RequiredFieldType="暂无校验" Text="z" size="25" ></cc1:textbox></dt>
					<dd><label>密　码:</label><cc1:textbox id="PassWord" runat="server" RequiredFieldType="暂无校验" Text="" TextMode="Password" size="20"></cc1:textbox>
					<dd><label>验证码:</label><input class="txt" id="vcode" onkeydown="if(event.keyCode==13)  document.getElementById('login').focus();" type="text" size="6" name="vcode" autocomplete="off"> <img id="vcodeimg" style="cursor:hand" onclick="this.src='../tools/VerifyImagePage.aspx?time=' + Math.random()" title="点击刷新验证码" align="absMiddle" src="" />
					<script type="text/javascript">
                        document.getElementById('vcodeimg').src='../tools/VerifyImagePage.aspx?id=<%=olid.ToString()%>&time=' + Math.random();
                        document.getElementById('vcode').value = "";
					</script></dd>
													
					<dd><input id="login" type="submit" value="" style="width:60px; height:26px; border:0; background:url(images/button.gif) no-repeat left top; cursor:pointer; margin-left:65px;"></dd>
					
				</dl>
			</li>
			<li><img src="images/login-bottom.gif"/></li>
		</ul>
	 </div>
	 </FORM>
	<div id="copyright"><%=footer%></div>
 </BODY>
</HTML>