<%@ Page Language="c#" Inherits="BBX.Web.Admin.uiandshowstyle" CodeBehind="global_uiandshowstyle.aspx.cs" %>
<%@ Import Namespace="BBX.Common"%>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>界面与显示方式设置</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
	function LoadImage(index) {
		document.getElementById("preview").src = images[index];
	}
	function setStatus(status) {
		document.getElementById("msgforwardlistinfo[0]").style.display = (status) ? "block" : "none";
		document.getElementById("msgforwardlistinfo[1]").style.display = (status) ? "block" : "none";
		document.getElementById("msgforwardlistinfo[2]").style.display = (status) ? "block" : "none";
	}
	
    function openShare(status) {
        document.getElementById("opensharelayer").style.display = (status) ? "" : "none";
    }
	
	function openforumhot(status) {
        document.getElementById("forumhotlayer").style.display = (status) ? "" : "none";
    }
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<fieldset>
<legend style="background: url(../images/icons/icon18.jpg) no-repeat 6px 50%;">界面与显示方式设置</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">默认论坛风格</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:dropdownlist id="templateid" runat="server"></cc1:dropdownlist>		</td>
		<td class="vtop">论坛默认的界面风格, 游客和使用默认风格的会员将以此风格显示</td>
	</tr>
	<tr><td class="item_title" colspan="2">选择模板预览</td></tr>
	<tr>
		<td class="vtop rowform">
			 <img id="preview" runat="server" alt="选择模板预览" src="../../templates/default/about.png" />		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">浏览自动生成</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:radiobuttonlist id="browsecreatetemplate" runat="server" RepeatLayout="Flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:radiobuttonlist>		</td>
		<td class="vtop">设置当页面模板不存在,用户浏览时是否自动生成.不推荐使用</td>
	</tr>
	<tr><td class="item_title" colspan="2">显示风格下拉菜单</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:radiobuttonlist id="stylejump" runat="server" RepeatLayout="Flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:radiobuttonlist>		</td>
		<td class="vtop">设置是否在论坛底部显示可用的论坛风格下拉菜单, 用户可以通过此菜单切换不同的论坛风格</td>
	</tr>
	<tr>
	  <td class="item_title" colspan="2"> 显示最近访问版块数量</td>
	</tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="visitedforums" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Size="6" MaxLength="4"></cc1:TextBox>		</td>
		<td class="vtop">设置在版块列表和帖子浏览中, 显示的最近访问过的版块下拉列表数量, 建议设置为 30 以内, 0 为关闭此功能</td>
	</tr>
	<tr><td class="item_title" colspan="2">最大签名高度</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="maxsigrows" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Size="6" MaxLength="4"></cc1:TextBox>(单位:行)		</td>
		<td class="vtop">设置帖子中允许显示签名的最大高度, 0 为不限制</td>
	</tr>
	<tr><td class="item_title" colspan="2">是否显示签名</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:radiobuttonlist id="showsignatures" runat="server" RepeatLayout="Flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:radiobuttonlist>		</td>
		<td class="vtop">是否在帖子中显示会员签名</td>
	</tr>
	<tr><td class="item_title" colspan="2">是否显示图片</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:radiobuttonlist id="showimages" runat="server" RepeatLayout="Flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:radiobuttonlist>		</td>
		<td class="vtop">是否在帖子中显示图片(包括上传的附件图片和 [img] 代码图片)</td>
	</tr>
	<tr><td class="item_title" colspan="2">显示可点击表情符</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="smileyinsert" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop">发帖页面包含表情符快捷工具, 点击图标即可插入表情符</td>
	</tr>
	<tr><td class="item_title" colspan="2">帖子中显示作者状态</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:RadioButtonList id="showauthorstatusinpost" runat="server" RepeatColumns="1">
				<asp:ListItem Value="1" selected="true">简单判断作者在线状态并显示(推荐)</asp:ListItem>
				<asp:ListItem Value="2">精确判断作者在线状态并显示</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop">浏览帖子时显示作者在线状态, 如果在线用户数量较多时, 启用"精确判断作者在线状态"功能会加重服务器负担, 此时建议使用"简单判断作者在线状态"</td>
	</tr>
	<tr><td class="item_title" colspan="2">显示论坛跳转菜单</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="forumjump" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop">选择"是"将在列表页面下部显示快捷跳转菜单. 只有在本设置启用时JS菜单中的论坛跳转设置才有效. 注意: 当分论坛很多时, 本功能会严重加重服务器负担</td>
	</tr>
	<tr><td class="item_title" colspan="2">查看新帖时间(分钟)</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="viewnewtopicminute" runat="server" Size="5" MaxLength="5" MinimumValue="5" MaximumValue="14400"></cc1:TextBox>		</td>
		<td class="vtop">设置多长时间内发布的帖子算是新帖</td>
	</tr>
	<tr><td class="item_title" colspan="2">是否开启左右分栏</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:radiobuttonlist id="isframeshow" runat="server" RepeatLayout="Flow" RepeatColumns="1" HintTopOffSet="-100">
				<asp:ListItem Value="0">关闭</asp:ListItem>
				<asp:ListItem Value="1">开启，默认为平板模式</asp:ListItem>
				<asp:ListItem Value="2">开启，默认为分栏模式</asp:ListItem>
			</cc1:radiobuttonlist>		</td>
		<td class="vtop">开启此功能后，论坛的头部会出现切换的链接(平板模式/分栏模式)</td>
	</tr>
	<tr><td class="item_title" colspan="2">帖子评分列表展示类型</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:radiobuttonlist id="ratelisttype" runat="server" RepeatLayout="Flow" RepeatColumns="1" HintTopOffSet="-100">
				<asp:ListItem Value="0">仅头像</asp:ListItem>
				<asp:ListItem Value="1">仅列表</asp:ListItem>
				<asp:ListItem Value="2">头像和列表混排</asp:ListItem>
			</cc1:radiobuttonlist>		</td>
		<td class="vtop">设置帖子中的评分列表展示类型，头像和列表混排时，只有在评分会员超过7个人时，列表才会出现</td>
	</tr>
	<tr><td class="item_title" colspan="2"> 显示帖子评分的数量</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="showratecount" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Size="6" MaxLength="4"></cc1:TextBox>		</td>
		<td class="vtop">设置在浏览帖子时,有多少条评分是被直接显示的,其他的将通过查看所有评分展现</td>
	</tr>
	<tr><td class="item_title" colspan="2">自定义楼号</td></tr>
	<tr>
		<td class="vtop rowform">
			<uc1:TextareaResize id="postnocustom" runat="server" cols="45" controlname="postnocustom" HintPosOffSet="160"></uc1:TextareaResize>		</td>
		<td class="vtop">用户可以自己定义楼号的显示方式,比如1楼可以称为"楼主",2楼可以称为"沙发",3楼可以称为"板凳",4楼可以称为"地板"等.每一个自定义楼号占一行.未定义的楼号按默认方式显示</td>
	</tr>
	<tr><td class="item_title" colspan="2">开启直接/快速跳转</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:RadioButtonList id="quickforward" runat="server" RepeatLayout="flow" HintPosOffSet="80">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop">对于论坛中的某些成功的操作不显示提示信息,直接跳转到下一个页面,例如发帖,回复等等,可以节省用户等待跳转的时间</td>
	</tr>
	<tr><td class="item_title" colspan="2"><span id="msgforwardlistinfo[2]">直接/快速跳转的信息</span></td></tr>
	<tr>
		<td class="vtop rowform">
		<span id="msgforwardlistinfo[0]">
			 <uc1:TextareaResize id="msgforwardlist" runat="server" controlname="msgforwardlist" HintPosOffSet="160"></uc1:TextareaResize>
		</span>		</td>
		<td class="vtop"><span id="msgforwardlistinfo[1]">当开启直接/快速跳转以后,以下信息将会直接跳转.每行填写一个信息的关键字<br/>
             如果您对信息的关键字不了解,可以<a href="http://www.newlifex.com/doc/Default.aspx?cid=153" target="_blank">点击这里查看</a></span></td>
	</tr>
	<tr><td class="item_title" colspan="2">版主显示方式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:radiobuttonlist id="moddisplay" runat="server" RepeatLayout="Flow">
				<asp:ListItem Value="1">下拉菜单</asp:ListItem>
				<asp:ListItem Value="0">平面显示</asp:ListItem>
			</cc1:radiobuttonlist>		</td>
		<td class="vtop">首页论坛列表中版主显示方式</td>
	</tr>
	<tr><td class="item_title" colspan="2">是否显示头像</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:radiobuttonlist id="showavatars" runat="server" RepeatLayout="Flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:radiobuttonlist>		</td>
		<td class="vtop">是否在帖子中显示会员头像</td>
	</tr>
	<tr><td class="item_title" colspan="2">启用浮动窗口</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:CheckBoxList id="allowfloatwin" runat="server" RepeatColumns="3">
				<asp:ListItem Value="login">登录</asp:ListItem>
				<asp:ListItem Value="register">注册</asp:ListItem>
				<asp:ListItem Value="sendpm">发短消息</asp:ListItem>
				<asp:ListItem Value="newthread">发帖</asp:ListItem>
				<asp:ListItem Value="reply">回帖</asp:ListItem>
			</cc1:CheckBoxList>
		</td>
		<td class="vtop">需要开启浮动窗口的页面</td>
	</tr>
	<tr><td class="item_title" colspan="2">帖子中同一表情符出现的最大次数</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="smiliesmax" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Width="60px" size="6" MaxLength="4"></cc1:TextBox>		</td>
		<td class="vtop">设置在帖子中出现同一表情的次数,默认值为5</td>
	</tr>
	<tr><td class="item_title" colspan="2">显示在线用户</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="whosonlinestatus" runat="server" RepeatColumns="1">
				<asp:ListItem Value="0">不显示</asp:ListItem>
				<asp:ListItem Value="1">仅在首页显示</asp:ListItem>
				<asp:ListItem Value="2">仅在分论坛显示</asp:ListItem>
				<asp:ListItem Value="3">在首页和分论坛显示</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop">在首页和论坛列表页显示在线会员列表(最大在线超过 500 人系统将自动缩略显示在线列表)</td>
	</tr>
	<tr><td class="item_title" colspan="2">在线列表是否隐藏游客</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="whosonlinecontact" runat="server" RepeatLayout="Flow">
				<asp:ListItem Value="true">是</asp:ListItem>
				<asp:ListItem Value="false">否</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop">在线列表是否隐藏游客</td>
	</tr>
	<tr><td class="item_title" colspan="2">最多显示在线人数</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxonlinelist" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox>		</td>
		<td class="vtop">此设置只有在显示在线用户启用时才有效. 设置为0则为不限制</td>
	</tr>
	<tr><td class="item_title" colspan="2">无动作离线时间</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="onlinetimeout" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Size="5" Text="10" MaxLength="4"></cc1:TextBox>(单位:分钟)		</td>
		<td class="vtop">多久无动作视为离线, 默认为10</td>
	</tr>
	
	<tr><td class="item_title" colspan="2">是否开启帖子分享功能</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="openshare" runat="server" RepeatLayout="flow" HintPosOffSet="80">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop"><div id="opensharelayer">[<a href="global_sharelistgrid.aspx">排序分享的网站</a>]</div></td>
	</tr>
	
	<tr><td class="item_title" colspan="2">首页是否显示版块前有无新帖图标</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="shownewposticon" runat="server" RepeatLayout="flow" HintPosOffSet="80">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop">首页是否显示版块前有无新帖图标</td>
	</tr>
	<tr><td class="item_title" colspan="2">允许查看会员列表</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="memliststatus" runat="server" RepeatLayout="Flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">允许查看会员列表</td>
	</tr>
	<tr><td class="item_title" colspan="2">首页类型</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="Indexpage" RepeatLayout="flow" runat="server">
				<asp:ListItem Value="0">论坛首页</asp:ListItem>
				<asp:ListItem Value="1">聚合首页</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>
		<tr><td class="item_title" colspan="2">是否显示组别</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="userstatusby" runat="server"  RepeatLayout="flow">
				<asp:ListItem Value="1" Selected>是</asp:ListItem>	
				<asp:ListItem Value="0">否</asp:ListItem>					
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">浏览帖子时是否显示用户所在的组</td>
	</tr>
	<tr><td class="item_title" colspan="2">每页主题数</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="tpp" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Size="6" MaxLength="4"></cc1:TextBox>
		</td>
		<td class="vtop">版块每页显示的主题数</td>
	</tr>
	<tr><td class="item_title" colspan="2">每页帖子数</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="ppp" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Size="6" MaxLength="4"></cc1:TextBox>
		</td>
		<td class="vtop">看主题时每页帖子数</td>
	</tr>
	<tr><td class="item_title" colspan="2">快速发帖</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:radiobuttonlist id="fastpost" runat="server" RepeatColumns="1">
				<asp:ListItem Value="0">不显示</asp:ListItem>
				<asp:ListItem Value="1">只显示快速发表主题</asp:ListItem>
				<asp:ListItem Value="2">只显示快速发表回复</asp:ListItem>
				<asp:ListItem Value="3">同时显示快速发表主题和回复</asp:ListItem>
			</cc1:radiobuttonlist>
		</td>
		<td class="vtop">浏览论坛和帖子页面底部显示快速发帖表单</td>
	</tr>
	<tr><td class="item_title" colspan="2">主题查看页面显示管理操作否</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="moderactions" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">显示</asp:ListItem>
				<asp:ListItem Value="0">不显示</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">是否在主题查看页面显示管理操作</td>
	</tr>
    <tr><td class="item_title" colspan="2">是否显示切换宽窄屏按钮</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="allowchangewidth" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">显示</asp:ListItem>
				<asp:ListItem Value="0">不显示</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">设置“我的中心”下拉列表中是否显示“切换到宽/窄屏”(聚合首页除外)</td>
	</tr>
    <tr><td class="item_title" colspan="2">页面默认显示的宽窄样式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="showwidthmode" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="0">宽屏显示</asp:ListItem>
				<asp:ListItem Value="1">窄屏显示</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">设置页面默认显示的宽窄样式(聚合首页除外)</td>
	</tr>
    <tr><td class="item_title" colspan="2">显示程序运行信息</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="debug" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop">选择"是"将在页脚处显示程序运行时间</td>
	</tr>
    <tr><td class="item_title" colspan="2">开启人性化时间格式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="datediff" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1" selected="true">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop">选择“是”，站点中的时间将显示以“n分钟前”、“昨天”、“n天前”等形式显示</td>
	</tr>

	<tr><td class="item_title" colspan="2">设置用户信息显示的位置</td></td>
	<tr><td colspan="2" class="ntcplist">
		<table width="100%" class="datalist postitem">
		  <tr class="category">
			<td>&nbsp;</td>
			<td>帖内左侧</td>
			<td>作者头像菜单</td>
		  </tr>
		  <tr>
			<td>UID</td>
			<td>
			  <input type="checkbox" name="postleft" value="uid" <%if (Utils.InArray("uid",postleftarray)){%>checked<%}%>> </td>
			<td>
			<input name="userface" type="checkbox" value="uid" <%if (Utils.InArray("uid",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
			<tr>
			<td>生日</td>
			<td>
			  <input type="checkbox" name="postleft" value="bday" <%if (Utils.InArray("bday",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="bday" <%if (Utils.InArray("bday",userfacearray)){%>checked<%}%>></td>
		  </tr>
		  <tr>
			<td>帖子</td>
			<td>
			  <input type="checkbox" name="postleft" value="posts" <%if (Utils.InArray("posts",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="posts" <%if (Utils.InArray("posts",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <tr>
			<td>精华</td>
			<td>
			  <input type="checkbox" name="postleft" value="digestposts" <%if (Utils.InArray("digestposts",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="digestposts" <%if (Utils.InArray("digestposts",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <tr>
			<td>积分</td>
			<td>
			  <input type="checkbox" name="postleft" value="credits" <%if (Utils.InArray("credits",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="credits" <%if (Utils.InArray("credits",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <%if (score[1]!=""){%>
		  <tr>
			<td><%=score[1]%></td>
			<td><input type="checkbox" name="postleft" value="extcredits1" <%if (Utils.InArray("extcredits1",postleftarray)){%>checked<%}%>>	</td>
			<td>
			<input type="checkbox" name="userface" value="extcredits1" <%if (Utils.InArray("extcredits1",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <%}%>
		  <%if (score[2]!=""){%>
		  <tr>
			<td><%=score[2]%></td>
			<td>
			  <input type="checkbox" name="postleft" value="extcredits2" <%if (Utils.InArray("extcredits2",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="extcredits2" <%if (Utils.InArray("extcredits2",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <%}%>
		  <%if (score[3]!=""){%>
		  <tr>
			<td><%=score[3]%></td>
			<td>
			  <input type="checkbox" name="postleft" value="extcredits3" <%if (Utils.InArray("extcredits3",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="extcredits3" <%if (Utils.InArray("extcredits3",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <%}%>
		  <%if (score[4]!=""){%>
		  <tr>
			<td><%=score[4]%></td>
			<td>
			  <input type="checkbox" name="postleft" value="extcredits4" <%if (Utils.InArray("extcredits4",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="extcredits4" <%if (Utils.InArray("extcredits4",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <%}%>
		  <%if (score[5]!=""){%>
		  <tr>
			<td><%=score[5]%></td>
			<td>
			  <input type="checkbox" name="postleft" value="extcredits5" <%if (Utils.InArray("extcredits5",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="extcredits5" <%if (Utils.InArray("extcredits5",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <%}%>
		  <%if (score[6]!=""){%>
		  <tr>
			<td><%=score[6]%></td>
			<td>
			  <input type="checkbox" name="postleft" value="extcredits6" <%if (Utils.InArray("extcredits6",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="extcredits6" <%if (Utils.InArray("extcredits6",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <%}%>
		  <%if (score[7]!=""){%>
		  <tr>
			<td><%=score[7]%></td>
			<td>
			  <input type="checkbox" name="postleft" value="extcredits7" <%if (Utils.InArray("extcredits7",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="extcredits7" <%if (Utils.InArray("extcredits7",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <%}%>
		  
			<%if (score[8]!=""){%>
		  <tr>
			<td><%=score[8]%></td>
			<td>
			  <input type="checkbox" name="postleft" value="extcredits8"  <%if (Utils.InArray("extcredits8",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="extcredits8" <%if (Utils.InArray("extcredits8",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <%}%>
		  <tr>
			<td>性别</td>
			<td>
			  <input type="checkbox" name="postleft" value="gender" <%if (Utils.InArray("gender",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="gender" <%if (Utils.InArray("gender",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <tr>
			<td>来自</td>
			<td>
			  <input type="checkbox" name="postleft" value="location" <%if (Utils.InArray("location",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="location" <%if (Utils.InArray("location",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <tr>
			<td>在线时间</td>
			<td>
			  <input type="checkbox" name="postleft" value="oltime" <%if (Utils.InArray("oltime",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="oltime" <%if (Utils.InArray("oltime",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <tr>
			<td>注册时间</td>
			<td>
			  <input type="checkbox" name="postleft" value="joindate" <%if (Utils.InArray("joindate",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="joindate" <%if (Utils.InArray("joindate",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		  <tr>
			<td>最后登录</td>
			<td>
			  <input type="checkbox" name="postleft" value="lastvisit" <%if (Utils.InArray("lastvisit",postleftarray)){%>checked<%}%>>	 </td>
			<td>
			<input type="checkbox" name="userface" value="lastvisit" <%if (Utils.InArray("lastvisit",userfacearray)){%>checked<%}%>>	</td>
		  </tr>
		</table>
	</td></tr>
</table>
<div class="Navbutton">
    <cc1:Button id="SaveInfo" runat="server" Text=" 提 交 "></cc1:Button>
</div>
</fieldset>
<script type="text/javascript">
	setStatus(document.getElementById("quickforward_0").checked);
</script>
</form>
</div>
<%=footer%>
</body>
</html>