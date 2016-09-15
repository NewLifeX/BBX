<%@ Page Language="c#" Inherits="BBX.Web.Admin.attach" Codebehind="global_attach.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>baseset</title>
<link href="../styles/tab.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<div id="TabControl1_Tab" class="tabs"><input type="hidden" value="TabControl1:tabPage51" id="TabControl1" name="TabControl1">
	<ul>
		<li class="CurrentTabSelect"><a class="current" href="global_attach.aspx">附件设置</a></li>
		<li class="TabSelect" ><a href="global_ftpsetting.aspx?ftptype=forumattach">远程附件</a></li>
		<li class="TabSelect" ><a href="../forum/forum_attchemnttypes.aspx">附件分类</a></li>
		<li class="TabSelect" ><a href="../forum/forum_attachtypesgrid.aspx">附件类型</a></li>
	</ul>
</div>
<div class="tabarea" id="TabControl1tabarea" style="display: block;">
	<table width="100%">
		<%--<tr><td class="item_title" colspan="2">帖子中显示图片附件</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:RadioButtonList ID="attachimgpost" runat="server" RepeatLayout="flow">
					<asp:ListItem Value="1">是</asp:ListItem>
					<asp:ListItem Value="0">否</asp:ListItem>
				</cc1:RadioButtonList>
			</td>
			<td class="vtop">在帖子中直接将图片或动画附件显示出来, 而不需要点击附件链接</td>
		</tr>--%>
		<tr><td class="item_title" colspan="2">附件保存方式</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:RadioButtonList ID="attachsave" runat="server" RepeatColumns="1" RepeatLayout="flow">
					<asp:ListItem Value="0">按年/月/日存入不同目录</asp:ListItem>
					<asp:ListItem Value="1">按年/月/日/论坛存入不同目录</asp:ListItem>
					<asp:ListItem Value="2">按版块存入不同目录 [不推荐]</asp:ListItem>
					<asp:ListItem Value="3">按文件类型存入不同目录 [不推荐]</asp:ListItem>
				</cc1:RadioButtonList>
			</td>
			<td class="vtop">本设置只影响新上传的附件, 设置更改之前的附件仍存放在原来位置.</td>
		</tr>
		<tr><td class="item_title" colspan="2">图片附件地址显示开关</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:RadioButtonList ID="showattachmentpath" runat="server" RepeatLayout="flow">
					<asp:ListItem Value="1">显示</asp:ListItem>
					<asp:ListItem Value="0">不显示</asp:ListItem>
				</cc1:RadioButtonList>
			</td>
			<td class="vtop">如果选择是, 则系统会以真实路径显示图片.如果选择否, 则以程序路径显示</td>
		</tr>
		<tr><td class="item_title" colspan="2">图片最大高度</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:TextBox ID="attachimgmaxheight" runat="server" Size="7" CanBeNull="必填" RequiredFieldType="数据校验" Text="0" MaxLength="5"></cc1:TextBox>(单位:像素)
			</td>
			<td class="vtop">0为不受限制.本设置只适用于bmp/png/jpeg格式图片</td>
		</tr>
		<tr><td class="item_title" colspan="2">JPG图片质量</td></tr>
		<tr>
			<td class="vtop rowform">
				 <cc1:TextBox ID="attachimgquality" runat="server" Size="5" CanBeNull="必填" RequiredFieldType="数据校验" Text="80" MaxLength="3"></cc1:TextBox>
			</td>
			<td class="vtop">本设置只适用于加水印的jpeg格式图片.取值范围 0-100, 0质量最低, 100质量最高, 默认80</td>
		</tr>
		<tr><td class="item_title" colspan="2">图片最大宽度</td></tr>
		<tr>
			<td class="vtop rowform">
				 <cc1:TextBox ID="attachimgmaxwidth" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Size="7" Text="0" MaxLength="5"></cc1:TextBox>(单位:像素)
			</td>
			<td class="vtop">0为不受限制.本设置只适用于bmp/png/jpeg格式图片</td>
		</tr>
		<tr><td class="item_title" colspan="2">下载附件来路检查</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:RadioButtonList ID="attachrefcheck" runat="server" HintShowType="down" HintTopOffSet="25" RepeatLayout="flow">
					<asp:ListItem Value="1">是</asp:ListItem>
					<asp:ListItem Value="0">否</asp:ListItem>
				</cc1:RadioButtonList>
			</td>
			<td class="vtop">选择"是"将检查下载附件的来路, 来自其他网站或论坛的下载请求将被禁止. 注意: 本功能在开启"帖子中显示图片附件"时,会加重服务器负担</td>
		</tr>
		<tr><td class="item_title" colspan="2">图片附件水印类型</td></tr>
		<tr>
			<td class="vtop rowform">
				 <cc1:RadioButtonList ID="watermarktype" runat="server" RepeatLayout="flow">
					<asp:ListItem Value="0">文字</asp:ListItem>
					<asp:ListItem Value="1">图片</asp:ListItem>
				</cc1:RadioButtonList>
			</td>
			<td class="vtop"></td>
		</tr>
		<tr><td class="item_title" colspan="2">选择水印位置</td></tr>
		<tr>
			<td class="vtop rowform">
				 <asp:Literal ID="position" runat="server"></asp:Literal>
			</td>
			<td class="vtop">请在此选择水印添加的位置(共 9 个位置可选).添加水印暂不支持动画 GIF 格式. 附加的水印图片在下面的使用的 图片水印文件 中指定.</td>
		</tr>
		<tr><td class="item_title" colspan="2">文字型水印的内容</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:TextBox ID="watermarktext" runat="server" RequiredFieldType="暂无校验" CanBeNull="必填" IsReplaceInvertedComma="false"></cc1:TextBox>
			</td>
			<td class="vtop">可以使用替换变量: {1}表示论坛标题 {2}表示论坛地址 {3}表示当前日期 {4}表示当前时间.例如: {3} {4}上传于{1} {2}</td>
		</tr>
		<tr><td class="item_title" colspan="2">图片附件文字水印字体</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:DropDownList ID="watermarkfontname" runat="server"></cc1:DropDownList>
			</td>
			<td class="vtop"></td>
		</tr> 
		<tr><td class="item_title" colspan="2">图片型水印文件</td></tr>
		<tr>
			<td class="vtop rowform">
				 <cc1:TextBox ID="watermarkpic" runat="server" Width="200px" RequiredFieldType="暂无校验" CanBeNull="必填" IsReplaceInvertedComma="false"></cc1:TextBox>
			</td>
			<td class="vtop">附加的水印图片需存放到论坛目录的watermark子目录下.注意:如果图片不存在系统将使用文字类型的水印.</td>
		</tr>
		<tr><td class="item_title" colspan="2">图片附件文字水印大小</td></tr>
		<tr>
			<td class="vtop rowform">
				<cc1:TextBox ID="watermarkfontsize" runat="server" Size="7" CanBeNull="必填" RequiredFieldType="数据校验" MaxLength="5"></cc1:TextBox>(单位:像素)
			</td>
			<td class="vtop"></td>
		</tr>
		<tr><td class="item_title" colspan="2">图片水印透明度</td></tr>
		<tr>
			<td class="vtop rowform">
				 <cc1:TextBox ID="watermarktransparency" runat="server" RequiredFieldType="数据校验" MaxLength="2" CanBeNull="必填" Size="5"></cc1:TextBox>
			</td>
			<td class="vtop">取值范围1--10 (10为不透明).</td>
		</tr>
	</table>
	<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
	<div class="Navbutton"><cc1:Button ID="SaveInfo" runat="server" Text=" 提 交 "></cc1:Button></div>
	</div>
</div>
</form>
<%=footer%>
</body>
</html>