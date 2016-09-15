<%@ Page Language="c#" Inherits="BBX.Web.Admin.searchengine" Codebehind="global_searchengine.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>搜索引擎优化</title>
    <script type="text/javascript" src="../js/common.js"></script>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<fieldset>
	<legend style="background: url(../images/icons/icon13.jpg) no-repeat 6px 50%;">搜索引擎优化</legend>
	<table width="100%">
	<tr><td class="item_title" colspan="2">启用Archiver</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="archiverstatus" runat="server" RepeatColumns="1" HintTitle="提示" HintShowType="down" HintHeight="80" HintTopFirefoxOffset="60">
				<asp:ListItem Value="0" Selected="True">关闭</asp:ListItem>
				<asp:ListItem Value="1">完全启用</asp:ListItem>
				<asp:ListItem Value="2">启用, 但用户从搜索引擎点击时自动转向动态页面</asp:ListItem>
				<asp:ListItem Value="3">启用, 但用户使用浏览器访问时自动转向动态页面</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"><%=BBX.Common.Utils.ProductName%> Archiver 能够将论坛公开的内容模拟成静态页面, 以便搜索引擎获取其中的内容. 高级使用技巧请参考《用户使用说明书》</td>
	</tr>
	<tr><td class="item_title" colspan="2">标题附加字</td></tr>
	<tr>
		<td class="vtop rowform">
			 <uc1:TextareaResize ID="seotitle" runat="server" cols="33" controlname="seotitle"></uc1:TextareaResize>
		</td>
		<td class="vtop">网页标题通常是搜索引擎关注的重点, 本附加字设置将出现在标题中论坛名称的前面, 如果有多个关键字, 建议用	"|"、","(不含引号) 等符号分隔</td>
	</tr>
	<tr><td class="item_title" colspan="2">Meta Description</td></tr>
	<tr>
		<td class="vtop rowform">
			 <uc1:TextareaResize ID="seodescription" runat="server" cols="33" controlname="seodescription"></uc1:TextareaResize>
		</td>
		<td class="vtop">Description 出现在页面头部的 Meta 标签中, 用于记录本页面的概要与描述</td>
	</tr>
	<tr><td class="item_title" colspan="2">Meta Keywords</td></tr>
	<tr>
		<td class="vtop rowform">
			<uc1:TextareaResize ID="seokeywords" runat="server" cols="33" controlname="seokeywords"></uc1:TextareaResize>
		</td>
		<td class="vtop">Keywords 项出现在页面头部的 Meta 标签中, 用于记录本页面的关键字, 多个关键字间请用半角逗号 "," 隔开</td>
	</tr>
	<tr><td class="item_title" colspan="2">其他头部信息</td></tr>
	<tr>
		<td class="vtop rowform">
			 <uc1:TextareaResize ID="seohead" runat="server" cols="33" controlname="seohead"></uc1:TextareaResize>
		</td>
		<td class="vtop">如需在 &lt;head&gt;&lt;/head&gt; 中添加其他的 html 代码, 可以使用本设置, 否则请留空, 不能使用纯文本</td>
	</tr>
		<tr><td class="item_title" colspan="2">启用 SiteMap</td></tr>
	<tr>
		<td class="vtop rowform">
		<cc1:RadioButtonList id="sitemapstatus" runat="server"  RepeatLayout="flow">
			<asp:ListItem Value="1">是</asp:ListItem>
			<asp:ListItem Value="0">否</asp:ListItem>
		</cc1:RadioButtonList>
		</td>
		<td class="vtop">SiteMap为百度论坛收录协议,是否允许百度收录</td>
	</tr>
	<tr><td class="item_title" colspan="2">SiteMap TTL (单位:小时)</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="sitemapttl" runat="server" CanBeNull="必填" MinimumValue="1" MaximumValue="24" Text="12" Size="5" MaxLength="2"></cc1:TextBox>
		</td>
		<td class="vtop">百度论坛收录协议更新时间, 用于控制百度论坛收录协议更新时间, 时间越短则资料实时性就越高, 但会加重服务器负担, 通常可设置为 1～24 范围内的数值</td>
	</tr>
		<tr><td class="item_title" colspan="2">启用伪静态url</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="aspxrewrite" runat="server"  RepeatLayout="flow" RepeatColumns="3">
				<asp:ListItem Value="1">启用</asp:ListItem>
				<asp:ListItem Value="0" Selected="true">不启用</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">只有启用该设置，伪静态url设置才会生效</td>
	</tr>
    <tr><td class="item_title" colspan="2">伪静态url的扩展名</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="extname" runat="server"  RequiredFieldType="暂无校验" CanBeNull="必填" Text="10" Size="5"></cc1:TextBox>		</td>
		<td class="vtop">此功能会实现网页链接页面的扩展名使用当前的设置!<a href="http://www.newlifex.com/doc/default.aspx?cid=36" target="_blank"><img src="../images/nav/sysinfo.gif" border="0" alt="伪静态url设置帮助" /></a></td>
	</tr>
    <tr><td class="item_title" colspan="2">伪静态url的替换规则</td></tr>
	<tr>
		<td class="vtop rowform">
			<span id="Span1" onMouseOut="hidehintinfo();" 
	onmouseover="showhintinfo(this,0,0,'提示','此处功能会实现网页链接的地址重定向的正则式校验内容,当您修改时请谨用!','50','up');">
				<span id="Span2" style="display:inline-block;border-width:0px;border-style:Dotted;"></span>			</span>		</td>
		<td class="vtop"><a href="#" class="TextButton" onClick="javascript:window.location.href='global_urlgrid.aspx';" >编辑伪静态url替换规则</a></td>
	</tr>
    <tr><td class="item_title" colspan="2">启用IIS的URL重写</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="iisurlrewrite" RepeatLayout="flow" runat="server" HintPosOffSet="40">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0" Selected="true">否</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop">此功能需要在IIS中配置才可生效[相关设置，<a href="http://www.newlifex.com/doc/default.aspx?cid=35" target="_blank">请参见</a>]</td>
	</tr>
	</table>
	<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
	<div class="Navbutton">
		<cc1:Button ID="SaveInfo" runat="server" Text=" 提 交 "></cc1:Button>
	</div>
</fieldset>
</form>
</div>
<%=footer%>
</body>
</html>