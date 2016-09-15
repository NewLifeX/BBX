<%@ Page language="c#" Inherits="BBX.Web.Admin.forumbatchset" Codebehind="forum_forumbatchset.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="ForumsTree" Src="../UserControls/forumstree.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>forumbatchset</title>		
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<table width="100%">
	<tr>
		<td class="vtop" colspan="2">
			<div style="OVERFLOW: auto;HEIGHT:95%">										
				<uc1:ForumsTree id="Forumtree1" runat="server"  PageName="forumbatchset"  WithCheckBox="true"></uc1:ForumsTree>
			</div>
		</td>
	</tr>
	<tr><td class="item_title" colspan="2"><b>源论坛:</b> <%=forumInfo.Name%></td></tr>
	<tr>
		<td class="vtop" colspan="2">
			<input onclick="CheckByName(this.form,'set','setting:')" type="checkbox" name="chkall" id="chkall" />选择复制所有设置/取消
		</td>
	</tr>
	<tr><td class="item_title" colspan="2"><input id="setpassword" type="checkbox" checked value="1" name="setpassword" runat="server" />访问本论坛的密码</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox ID="password" runat="server" RequiredFieldType="暂无校验" Width="150px" />
		</td>
		<td class="vtop">留空为不需密码</td>
	</tr>
	<tr><td class="item_title" colspan="2"> <input id="setattachextensions" type="checkbox" checked value="1" name="setattachextensions" runat="server" />论坛上传附件类型</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:checkboxlist id="attachextensions" runat="server" RepeatColumns="3"></cc1:checkboxlist>
		</td>
		<td class="vtop">允许在本论坛上传的附件类型,留空为使用用户组及系统默认设置</td>
	</tr>
	<tr><td class="item_title" colspan="2"></td></tr>
	<tr>
		<td class="vtop"  colspan="2">
			 <input id="setpostcredits" type="checkbox" checked value="1" name="settopicscore" runat="server" />应用发主题积分策略
		</td>
	</tr>
	<tr><td class="item_title" colspan="2"></td></tr>
	<tr>
		<td class="vtop"  colspan="2">
			<input id="setreplycredits" type="checkbox" checked value="1" name="setpostscore" runat="server" />应用发回复积分策略
		</td>
	</tr>
	<tr><td class="item_title" colspan="2"><input id="setsetting" type="checkbox" checked value="1" name="setsetting" runat="server" />设置</td></tr>
	<tr>
		<td class="vtop"  colspan="2">
			<cc2:CheckBoxList id="setting" runat="server" RepeatColumns="4" >
				<asp:ListItem Value="allowsmilies">允许使用表情符</asp:ListItem>
				<asp:ListItem Value="allowrss">允许RSS</asp:ListItem>
				<asp:ListItem Value="allowbbcode">允许论坛代码</asp:ListItem>
				<asp:ListItem Value="allowimgcode">允许[img]代码</asp:ListItem>
				<asp:ListItem Value="recyclebin">打开回收站</asp:ListItem>
				<asp:ListItem Value="modnewposts">发帖需要审核</asp:ListItem>
                <asp:ListItem Value="modnewtopics">发主题需要审核</asp:ListItem>
				<asp:ListItem Value="jammer">帖子中添加干扰码</asp:ListItem>
				<asp:ListItem Value="disablewatermark">禁止附件自动水印</asp:ListItem>
				<asp:ListItem Value="inheritedmod">继承上级论坛或分类的版主设定</asp:ListItem>
				<asp:ListItem Value="allowthumbnail">主题列表中显示缩略图</asp:ListItem>
				<asp:ListItem Value="allowtags">允许标签</asp:ListItem>									        
				<asp:ListItem Value="allowpostpoll">允许发投票</asp:ListItem>
				<asp:ListItem Value="allowdebate">允许辩论</asp:ListItem>
				<asp:ListItem Value="allowbonus">允许悬赏</asp:ListItem>
				<asp:ListItem Value="alloweditrules">允许版主编辑版规</asp:ListItem>						        
			</cc2:CheckBoxList>
		</td>
	</tr>
	<tr><td class="item_title" colspan="2"><input id="setviewperm" type="checkbox" checked value="1" name="setviewperm" runat="server" />浏览权限设定</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:checkboxlist id="viewperm" runat="server" RepeatColumns="4" ></cc1:checkboxlist>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2"><input id="setpostperm" type="checkbox" checked value="1" name="setpostperm" runat="server" />发主题权限设定</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:checkboxlist id="postperm" runat="server" RepeatColumns="4" ></cc1:checkboxlist>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2"><input id="setreplyperm" type="checkbox" checked value="1" name="setreplyperm" runat="server" />发回复权限设定</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:checkboxlist id="replyperm" runat="server" RepeatColumns="4" ></cc1:checkboxlist>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2"><input id="setgetattachperm" type="checkbox" checked value="1" name="setgetattachperm" runat="server" />下载附件权限设定</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:checkboxlist id="getattachperm" runat="server" RepeatColumns="4" ></cc1:checkboxlist>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2"><input id="setpostattachperm" type="checkbox" checked value="1" name="setpostattachperm" runat="server" />上传附件权限设定</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:checkboxlist id="postattachperm" runat="server" RepeatColumns="4" ></cc1:checkboxlist>
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div class="Navbutton">
	<cc1:Button id="SubmitBatchSet" runat="server" ButtontypeMode="WithImage" Text=" 提 交 " XpBGImgFilePath="../images/"
	ApplyDefaultStyle="false" ButtonImgUrl="../images/ok.gif"></cc1:Button>
</div>
</form>
</div>
<%=footer%>
</body>
</html>