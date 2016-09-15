<%@ Page language="c#" Inherits="BBX.Web.Admin.addforums" Codebehind="forum_addforums.aspx.cs"%>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc3" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc2" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<html>
<head>
<title>添加版块</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />		
<link href="../styles/tab.css" type="text/css" rel="stylesheet" />
<style type="text/css">
.td_alternating_item1{font-size: 12px;}
.td_alternating_item2{font-size: 12px;background-color: #F5F7F8;}
</style>
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
    function setColDisplayer(display) {
        var str = display ? 'none' : '';

        document.getElementById('c2').style.display = str;
        document.getElementById('c3').style.display = str;
        document.getElementById('c4').style.display = str;
        document.getElementById('c5').style.display = str;

        setSeleColDisplay('postperm', display);
        setSeleColDisplay('replyperm', display);
        setSeleColDisplay('getattachperm', display);
        setSeleColDisplay('postattachperm', display);
    }

    function validatefiletype(obj) {
        if (obj.value == '')
            return false;
        var fileext = obj.value.substr(obj.value.lastIndexOf('.')).toLowerCase();

        var allowext = new Array('jpg', 'png', 'gif', 'bmp', 'jpeg');
        var isallowext = false;

        for (i = 0; i < allowext.length; i++) {
            if (fileext == '.' + allowext[i]) {
                isallowext = true;
                break;
            }
        }
        if (!isallowext)
            alert('您选择的文件不是合法的图片文件');
    }
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" enctype="multipart/form-data" method="post" runat="server">
<cc3:TabControl id="TabControl1" SelectionMode="Client" runat="server" TabScriptPath="../js/tabstrip.js" height="100%">
<cc3:TabPage Caption="基本信息" ID="tabPage51">
<table width="100%">
	<tr><td class="item_title" colspan="2">论坛名称</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:TextBox id="name" runat="server" CanBeNull="必填" IsReplaceInvertedComma="false" size="20"  MaxLength="49"></cc2:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">显示模式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="colcount" runat="server" AutoPostBack="false" RepeatColumns="1">
				<asp:ListItem Value="1">传统模式[默认]</asp:ListItem>
				<asp:ListItem Value="2">子版块横排模式</asp:ListItem>
			</cc2:RadioButtonList>
			<div id="showcolnum" runat="server">
				<cc2:TextBox id="colcountnumber" runat="server" Size="2" Text="4" MaxLength="1"></cc2:TextBox>
			</div>
		</td>
		<td class="vtop">用来设置该论坛(或分类)的子论坛在列表中的显示方式</td>
	</tr>
	<tr><td class="item_title" colspan="2">是否显示</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="status" runat="server" RepeatColumns="2">
				<asp:ListItem Value="1" Selected="True">显示</asp:ListItem>
				<asp:ListItem Value="0" >不显示</asp:ListItem>
			</cc2:RadioButtonList>
		</td>
		<td class="vtop">设置本版块是否是隐藏版块</td>
	</tr>
	<tr><td class="item_title" colspan="2">所属类别</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="addtype" runat="server" RepeatColumns="2">
				<asp:ListItem Value="0" >论坛分类</asp:ListItem>
				<asp:ListItem Value="1" Selected="True">论坛版块</asp:ListItem>
			</cc2:RadioButtonList>
			<div id="showtargetforum" runat="server">
				<cc2:DropDownTreeList id="targetforumid" runat="server" Visible="true"></cc2:DropDownTreeList>
			</div>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">版主列表</td></tr>
	<tr>
		<td class="vtop rowform">
			  <uc1:TextareaResize id="moderators" runat="server" controlname="TabControl1:tabPage51:moderators"></uc1:TextareaResize>
		</td>
		<td class="vtop">当前版块版主列表，以","进行分割,以','进行分割,如:lisi,zhangsan</td>
	</tr>
	<tr><td class="item_title" colspan="2">URL重写名称</td></tr>
	<tr>
		<td class="vtop rowform">
			 <%=root%><cc2:textbox id="rewritename" runat="server" Width="250px" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false"  MaxLength="20"></cc2:textbox>/<%if (config.Iisurlrewrite == 0){%>list.aspx<%}%>
			<input type="hidden" id="oldrewritename" runat="server" />
		</td>
		<td class="vtop">设置版块URL重写,会以"http://www.newlifex.com/rewritename/"的形式显示,以字母开头,其后可以是字母或数字,但不可包含"admin,aspx,tools,archive"中任意一个字符串</td>
	</tr>
	<tr><td class="item_title" colspan="2">版块描述</td></tr>
	<tr>
		<td class="vtop rowform">
			  <cc2:textbox id="description" runat="server" TextMode="MultiLine" cols="45" IsReplaceInvertedComma="false"></cc2:textbox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">本版块SEO关键词</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:textbox id="seokeywords" runat="server" RequiredFieldType="暂无校验" cols="45" TextMode="MultiLine" IsReplaceInvertedComma="false"></cc2:textbox>
		</td>
		<td class="vtop">设置本版块的SEO关键词,用于搜索引擎优化,放在meta的keyword标签中,多个关键字间请用半角逗号","隔开</td>
	</tr>
	<tr><td class="item_title" colspan="2">本版块SEO描述</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:textbox id="seodescription" runat="server" RequiredFieldType="暂无校验" cols="45" TextMode="MultiLine" IsReplaceInvertedComma="false"></cc2:textbox>
		</td>
		<td class="vtop">设置本版块的SEO描述,用于搜索引擎优化,放在meta的description标签中,多个说明文字间请用半角逗号","隔开</td>
	</tr>
</table>
</cc3:TabPage>
<cc3:TabPage Caption="高级设置" ID="tabPage22">
<table width="100%">
    <tr><td class="item_title" colspan="2">论坛图标</td></tr>
	<tr>
		<td  style="padding-bottom:10px;" class="vtop rowform">
			 <cc2:textbox id="icon" runat="server" Width="250px" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false"  MaxLength="253"></cc2:textbox>
		</td>
		<td class="vtop">显示于首页论坛版块名称前，可填写网络地址，和使用下方上传功能上传本地图片。留空为不设置版块图标</td>
	</tr>
    <tr>
       <td class="vtop rowform">
            <input id="uploadforumicon" type="file" name="Filedata" onchange="validatefiletype(this);" />
		</td>
		<td class="vtop">选择本地图片文件作为版块的图标</td>
	</tr>
	<tr><td class="item_title" colspan="2">访问本论坛的密码</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:textbox id="password" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false" MaxLength="16" Size="20"></cc2:textbox>
		</td>
		<td class="vtop">设置本版块的密码,留空则本版块不使用密码</td>
	</tr>
	<tr><td class="item_title" colspan="2">指向外部链接的地址</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:textbox id="redirect" runat="server" Width="250px" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false"  MaxLength="253"></cc2:textbox>
		</td>
		<td class="vtop">设置版块为一个链接，当点击本版块是将跳转到指定的地址上</td>
	</tr>
	<tr><td class="item_title" colspan="2">本版规则</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:textbox id="rules" runat="server" RequiredFieldType="暂无校验" width="250" height="100" TextMode="MultiLine" IsReplaceInvertedComma="false" ></cc2:textbox>
		</td>
		<td class="vtop">支持Html</td>
	</tr>
	<tr><td class="item_title" colspan="2">允许的附件类型</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:CheckBoxList id="attachextensions" runat="server" RepeatColumns="4"></cc2:CheckBoxList>
		</td>
		<td class="vtop">允许在本论坛上传的附件类型,留空为使用用户组设置, 且版块设置优先于用户组设置</td>
	</tr> 
	<tr><td class="item_title" colspan="2">定期自动关闭主题</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="autocloseoption" runat="server" RepeatColumns="1">
				<asp:ListItem Value="0" Selected="True">不自动关闭</asp:ListItem>
				<asp:ListItem Value="1">按发布时间</asp:ListItem>
			</cc2:RadioButtonList>
			<div id="showclose" runat="server">
				<cc2:TextBox id="autocloseday" runat="server" RequiredFieldType="数据校验" Size="4" MaxLength="3"></cc2:TextBox>天自动关闭	
			</div>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">只允许发布特殊类型主题</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="allowspecialonly" runat="server" RepeatColumns="2" HintInfo="设置本版是否只允许发布特殊类型主题">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0" Selected="True">否</asp:ListItem>
			</cc2:RadioButtonList>
		</td>
		<td class="vtop">设置本版是否只允许发布特殊类型主题</td>
	</tr> 
	<tr><td class="item_title" colspan="2">设置</td></tr>
	<tr>
		<td class="vtop" colspan="2">
			<cc2:CheckBoxList id="setting" runat="server" RepeatColumns="4" >
				<asp:ListItem Value="allowsmilies" selected="true">允许使用表情符</asp:ListItem>
				<asp:ListItem Value="allowrss" selected="true">允许RSS</asp:ListItem>
				<asp:ListItem Value="allowbbcode" selected="true">允许论坛代码</asp:ListItem>
				<asp:ListItem Value="allowimgcode" selected="true">允许[img]代码</asp:ListItem>
				<asp:ListItem Value="recyclebin">打开主题回收站</asp:ListItem>
				<asp:ListItem Value="modnewposts">发帖需要审核</asp:ListItem>
                <asp:ListItem Value="modnewtopics">发主题需要审核</asp:ListItem>
				<asp:ListItem Value="jammer">帖子中添加干扰码</asp:ListItem>
				<asp:ListItem Value="disablewatermark">禁止附件自动水印</asp:ListItem>
				<asp:ListItem Value="inheritedmod">继承上级论坛或分类的版主设定</asp:ListItem>
				<asp:ListItem Value="allowthumbnail">主题列表中显示缩略图</asp:ListItem>
				<asp:ListItem Value="allowtags" selected="true">允许标签</asp:ListItem>
				<asp:ListItem Value="allowpostpoll" selected="true">允许发投票</asp:ListItem>
				<asp:ListItem Value="allowdebate">允许辩论</asp:ListItem>
				<asp:ListItem Value="allowbonus">允许悬赏</asp:ListItem>
				<asp:ListItem Value="alloweditrules" selected="true">允许版主编辑版规</asp:ListItem>
			</cc2:CheckBoxList>
		</td>
	</tr>
</table>
</cc3:TabPage>
<cc3:TabPage Caption="权限设定" ID="tabPage33">
	<uc2:PageInfo id="info1" runat="server" Icon="Information" Text="每个组的权限项不选择时,权限为使用本版块用户的用户组权限设置,且版块权限设置优先于用户组权限设置."></uc2:PageInfo>    			
	<table width="100%" id="powerset" align="center" class="table1" cellspacing="0" cellPadding="4"  bgcolor="#C3C7D1" runat="server">	
		<tr>
			<td class="td_alternating_item2">全选</td>
			<td class="td_alternating_item2"><input type="checkbox" id="c1" onclick="seleCol('viewperm',this.checked)"/><label for="c1">浏览论坛</label></td>
			<td class="td_alternating_item2"><input type="checkbox" id="c2" onclick="seleCol('postperm',this.checked)"/><label for="c2">发新话题</label></td>
			<td class="td_alternating_item2"><input type="checkbox" id="c3" onclick="seleCol('replyperm',this.checked)"/><label for="c3">发表回复</label></td>
			<td class="td_alternating_item2"><input type="checkbox" id="c4" onclick="seleCol('getattachperm',this.checked)"/><label for="c4">下载/查看附件</label></td>
			<td class="td_alternating_item2"><input type="checkbox" id="c5" onclick="seleCol('postattachperm',this.checked)"/><label for="c5">上传附件</label></td>
		</tr>
	</table>
	</cc3:TabPage>
</cc3:TabControl>
<div id="topictypes" style="display:none;width:100%;">
<table>
	<tr><td class="item_title" colspan="2">主题分类</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:textbox id="topictypes" runat="server" RequiredFieldType="暂无校验" width="370" height="50" TextMode="MultiLine"></cc2:textbox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">模板风格</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:DropDownList id="templateid" runat="server"></cc2:DropDownList>
		</td>
		<td class="vtop"></td>
	</tr>
</table>
</div>
<div class="Navbutton">
	<cc2:Button id="SubmitAdd" runat="server" Text=" 添 加 "></cc2:Button>&nbsp;&nbsp;
	<button onclick="window.location='forum_forumstree.aspx';" id="Button3" class="ManagerButton" type="button"><img src="../images/arrow_undo.gif"/> 返 回 </button>
</div>
<cc2:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc2:Hint>
</form>
<%=footer%>
</div>
</body>
</html>