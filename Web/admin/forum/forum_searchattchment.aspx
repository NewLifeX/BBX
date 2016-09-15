<%@ Page language="c#" Inherits="BBX.Web.Admin.searchattchment" Codebehind="forum_searchattchment.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>searchattchment</title>		
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
<legend style="background:url(../images/icons/icon19.jpg) no-repeat 6px 50%;">搜索附件</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">所在论坛</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:dropdowntreelist id="forumid" runat="server"></cc1:dropdowntreelist>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title">附件尺寸小于</td><td class="item_title">附件尺寸大于</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="filesizemin" runat="server" RequiredFieldType="数据校验" Size="10"></cc1:TextBox>(单位:字节)
			<select onchange="document.getElementById('filesizemin').value=this.value">
				<option value="">请选择</option>
				<option value="51200">50K</option>
				<option value="102400">100K</option>
				<option value="153600">150K</option>
				<option value="204800">200K</option>
				<option value="256000">250K</option>
				<option value="307200">300K</option>
				<option value="358400">350K</option>
				<option value="409600">400K</option>
				<option value="512000">500K</option>
				<option value="614400">600K</option>
				<option value="716800">700K</option>
				<option value="819200">800K</option>
				<option value="921600">900K</option>
				<option value="1024000">1M</option>
				<option value="2048000">2M</option>
				<option value="4096000">4M</option>								
			</select>
		</td>
		<td class="vtop rowform">
			<cc1:TextBox ID="filesizemax" runat="server" RequiredFieldType="数据校验" Size="10" />(单位:字节)
			<select onchange="document.getElementById('filesizemax').value=this.value">
				<option value="">请选择</option>
				<option value="51200">50K</option>
				<option value="102400">100K</option>
				<option value="153600">150K</option>
				<option value="204800">200K</option>
				<option value="256000">250K</option>
				<option value="307200">300K</option>
				<option value="358400">350K</option>
				<option value="409600">400K</option>
				<option value="512000">500K</option>
				<option value="614400">600K</option>
				<option value="716800">700K</option>
				<option value="819200">800K</option>
				<option value="921600">900K</option>
				<option value="1024000">1M</option>
				<option value="2048000">2M</option>
				<option value="4096000">4M</option>									
			</select>
		</td>
	</tr>
	<tr><td class="item_title">被下载次数小于</td><td class="item_title">被下载次数大于</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="downloadsmin" runat="server" Size="6" RequiredFieldType="数据校验"></cc1:TextBox>
		</td>
		<td class="vtop rowform">
			 <cc1:TextBox id="downloadsmax" runat="server" RequiredFieldType="数据校验" Size="6" />
		</td>
	</tr>
	<tr><td class="item_title">存储文件名</td><td class="item_title">发表于多少天前</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="filename" runat="server" RequiredFieldType="暂无校验" Width="200"></cc1:TextBox>
		</td>
		<td class="vtop rowform">
			 <cc1:TextBox id="postdatetime" runat="server" Size="4" RequiredFieldType="数据校验"></cc1:TextBox>
		</td>
	</tr>
	<tr><td class="item_title" colspan="2">作者</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="poster" runat="server" RequiredFieldType="暂无校验" Width="100"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">描述关键字</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="description" runat="server" RequiredFieldType="暂无校验" Width="200"></cc1:TextBox>
		</td>
		<td class="vtop">多关键字中间请用半角逗号 "," 分割</td>
	</tr>
</table>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div class="Navbutton"><cc1:Button id="SaveSearchCondition" runat="server" Text="搜索附件" ButtonImgUrl="../images/search.gif"></cc1:Button></div>
</fieldset>
</form>
</div>
<%=footer%>
</body>
</html>