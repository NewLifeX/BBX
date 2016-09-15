<%@ Page language="c#" Inherits="BBX.Web.Admin.editmall" Codebehind="forum_editmall.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>

<html>
<head>
<title>编辑商城</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/tab.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../js/tabstrip.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript" src="../js/AjaxHelper.js"></script>
<style type="text/css">
.td_alternating_item1{font-size: 12px;}
.td_alternating_item2{font-size: 12px;background-color: #F5F7F8;}
</style>
<script type="text/javascript">
function editcredit(fid,fieldname)
{
	window.location="forum_ScoreStrategy.aspx?fid="+fid+"&fieldname="+fieldname;
}
function Check(form,bCheck,findstring)
{
	for (var i=0;i<form.elements.length;i++)
	{
	var e = form.elements[i];
	if (e.name.indexOf(findstring) >= 0)
		e.checked = bCheck;
	}
}
function CheckRow(form,bCheck,rowId)
{
	for (var i=0;i<form.elements.length;i++)
	{
	var e = form.elements[i];
	if (e.name.indexOf(rowId + ":viewbyuser") >= 0 || e.name.indexOf(rowId + ":postbyuser") >= 0
		 || e.name.indexOf(rowId + ":replybyuser") >= 0 || e.name.indexOf(rowId + ":getattachbyuser") >= 0
		  || e.name.indexOf(rowId + ":postattachbyuser") >= 0)
		e.checked = bCheck;
	}
}

function GetTd()
{
		td = document.createElement("td");
		td.setAttribute("nowrap","nowrap");
		td.style.borderColor = "#EAE9E1";
		td.style.borderWidth = "1px";
		td.style.borderStyle = "solid";
		return td;
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<span style="font-size:12px">当前商城版块为: <b><asp:literal id="forumname" runat="server"></asp:literal></b></span>
<cc2:TabControl id="TabControl1" SelectionMode="Client" runat="server" TabScriptPath="../js/tabstrip.js"  height="100%">
<cc2:TabPage Caption="基本信息" ID="tabPage1" >
<table width="100%">
	<tr><td class="item_title" colspan="2">交易版名称</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:TextBox id="name" runat="server" CanBeNull="必填"  IsReplaceInvertedComma="false"   size="30"  MaxLength="49"></cc2:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">已继承的版主</td></tr>
	<tr>
		<td class="vtop rowform">
			<asp:Literal ID="inheritmoderators" runat="server"></asp:Literal>
		</td>
		<td class="vtop"></td>
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
	<!--
	<tr><td class="item_title" colspan="2">显示模式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="colcount" runat="server" RepeatColumns="1">
				<asp:ListItem Value="1">传统模式[默认]</asp:ListItem>
				<asp:ListItem Value="2">子版块横排模式</asp:ListItem>
			</cc2:RadioButtonList>
			<div id="showcolnum" runat="server"><cc2:TextBox id="colcountnumber" runat="server" Size="2" Text="4" MaxLength="1"></cc2:TextBox></div>
		</td>
		<td class="vtop">用来设置该论坛(或分类)的子论坛在列表中的显示方式,选择"子版块横排模式",则子分类列表按每行按输入的数字个数出现</td>
	</tr>
	-->
	<tr><td class="item_title" colspan="2">版主列表</td></tr>
	<tr>
		<td class="vtop rowform">
			 <uc1:TextareaResize id="moderators" runat="server" controlname="TabControl1:tabPage1:moderators" Cols="30" Rows="5"></uc1:TextareaResize>
		</td>
		<td class="vtop">当前版块版主列表，以','进行分割,如:lisi,zhangsan</td>
	</tr>
	<tr><td class="item_title" colspan="2">交易版描述</td></tr>
	<tr>
		<td class="vtop rowform">
			  <uc1:TextareaResize id="description" runat="server" controlname="TabControl1:tabPage1:description" Cols="30" Rows="5"></uc1:TextareaResize>
		</td>
		<td class="vtop"></td>
	</tr>
	<tbody id="templatestyle" runat="server">
	<tr><td class="item_title" colspan="2">模板风格</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:DropDownList id="templateid" runat="server" HintInfo="设置本版块使用的模板,将不受系统模板限制"></cc2:DropDownList>
		<td class="vtop"></td>
	</tr>
	</tbody>
</table>
</cc2:TabPage>
<cc2:TabPage Caption="高级设置" ID="tabPage2">
<table width="100%">
	<tr><td class="item_title" colspan="2">访问本版块的密码</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:textbox id="password" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false" MaxLength="16" Size="20"></cc2:textbox>
		</td>
		<td class="vtop">设置本版块的密码,留空则本版块不使用密码</td>
	</tr>
	<tr><td class="item_title" colspan="2">交易版图标</td></tr>
	<tr>
		<td class="vtop rowform">
			  <cc2:textbox id="icon" runat="server" Width="250px" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false"  MaxLength="253"></cc2:textbox>
		</td>
		<td class="vtop">显示于首页论坛列表等</td>
	</tr>
	 <!--<tr><td class="item_title" colspan="2">发主题积分策略</td></tr>
	<tr>
		<td class="vtop rowform">
			  <a href="#" class="TextButton" onclick="javascript:editcredit('<%=Request.Params["fid"]%>','postcredits');" >编 辑</a>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">发回复积分策略</td></tr>
	<tr>
		<td class="vtop rowform">
			   <a href="#" class="TextButton" onclick="javascript:editcredit('<%=Request.Params["fid"]%>','replycredits');" >编 辑</a>
		</td>
		<td class="vtop"></td>
	</tr>
	-->
	<tr><td class="item_title" colspan="2">指向外部链接地址</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:textbox id="redirect" runat="server" Width="250px" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false"  MaxLength="253"></cc2:textbox>
		</td>
		<td class="vtop">设置版块为一个链接，当点击本版块是将跳转到指定的地址上</td>
	</tr>
	<tr><td class="item_title" colspan="2">允许的附件类型</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:CheckBoxList id="attachextensions" runat="server" RepeatColumns="4"></cc2:CheckBoxList>
		</td>
		<td class="vtop">允许在本版块上传的附件类型,留空为使用用户组设置, 且版块设置优先于用户组设置</td>
	</tr>
	<tr><td class="item_title" colspan="2">本版规则</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:textbox id="rules" runat="server" RequiredFieldType="暂无校验" width="250" height="100" TextMode="MultiLine" IsReplaceInvertedComma="false"></cc2:textbox>
		</td>
		<td class="vtop">支持Html</td>
	</tr>
	<!--
	<tr><td class="item_title" colspan="2">定期自动关闭主题:</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="autocloseoption" runat="server" RepeatColumns="1">
				<asp:ListItem Value="0" Selected="True">不自动关闭</asp:ListItem>
				<asp:ListItem Value="1">按发布时间</asp:ListItem>
			</cc2:RadioButtonList>
			<div id="showclose" runat="server">
				<cc2:TextBox id="autocloseday" runat="server" Size="4" MaxLength="3"></cc2:TextBox>
				<font style="font-size:12px">天自动关闭</font>	
			</div>
		</td>
		<td class="vtop">设置主题关闭方式</td>
	</tr>
	<tr><td class="item_title" colspan="2">只允许发布特殊类型主题</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="allowspecialonly" runat="server" RepeatColumns="2">
						                    <asp:ListItem Value="1">是</asp:ListItem>
						                    <asp:ListItem Value="0" Selected="True">否</asp:ListItem>
						                </cc2:RadioButtonList>
		</td>
		<td class="vtop">设置本版是否只允许发布特殊类型主题</td>
	</tr>
	-->
	<tr><td class="item_title" colspan="2">设置</td></tr>
	<tr>
		<td class="vtop" colspan="2">
			 <cc2:CheckBoxList id="setting" runat="server" RepeatColumns="4" >
				<asp:ListItem Value="allowsmilies">允许使用表情符</asp:ListItem>
				<asp:ListItem Value="allowrss">允许RSS</asp:ListItem>
				<asp:ListItem Value="allowbbcode">允许论坛代码</asp:ListItem>
				<asp:ListItem Value="allowimgcode">允许[img]代码</asp:ListItem>
				<asp:ListItem Value="recyclebin">打开回收站</asp:ListItem>
				<asp:ListItem Value="modnewposts">发帖需要审核</asp:ListItem>
				<asp:ListItem Value="disablewatermark">禁止附件自动水印</asp:ListItem>
				<asp:ListItem Value="inheritedmod">继承上级论坛或分类的版主设定</asp:ListItem>
				<asp:ListItem Value="allowthumbnail">主题列表中显示缩略图</asp:ListItem>
				<asp:ListItem Value="allowtags">允许标签</asp:ListItem>						        
			</cc2:CheckBoxList>
		</td>
	</tr>
</table>
</cc2:TabPage>
<cc2:TabPage Caption="权限设定" ID="tabPage3">
<uc1:PageInfo id="PageInfo1" runat="server" Icon="Information" Text="每个组的权限项不选择为使用用户组设置，且版块设置优先于用户组设置."></uc1:PageInfo>    			
<table width="100%" id="powerset" align="center" class="table1" cellspacing="0" cellPadding="4"  bgcolor="#C3C7D1" runat="server">	
	<tr>
		<td class="td_alternating_item2" width="1%">&nbsp;</td>
		<td class="td_alternating_item2" width="20%" style="word-wrap: break-word">&nbsp;</td>
		<td class="td_alternating_item2"><input type="checkbox" id="c1" onclick="seleCol('viewperm',this.checked)"/><label for="c1">浏览/交易商品</label></td>
		<td class="td_alternating_item2"><input type="checkbox" id="c2" onclick="seleCol('postperm',this.checked)"/><label for="c2">发布商品</label></td>
		<td class="td_alternating_item2"><input type="checkbox" id="c3" onclick="seleCol('replyperm',this.checked)"/><label for="c3">发表留言</label></td>
		<td class="td_alternating_item2"><input type="checkbox" id="c4" onclick="seleCol('getattachperm',this.checked)"/><label for="c4">下载/查看附件</label></td>
		<td class="td_alternating_item2"><input type="checkbox" id="c5" onclick="seleCol('postattachperm',this.checked)"/><label for="c5">上传附件</label></td>
	</tr>
</table>
</cc2:TabPage>
<cc2:TabPage Caption="特殊用户" ID="tabPage4">
<table width="100%" align="center" class="table1" cellspacing="0" cellPadding="4"  bgcolor="#C3C7D1">
<tr>
	<td class="category">
		<input title="选中/取消选中 本页所有Case" onclick="Check(this.form,this.checked,'userid')" type="checkbox" name="chkall" id="chkall" />全选/取消全选 &nbsp; 
		<cc2:Button id="DelButton" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif"></cc2:Button>
	</td>
</tr>
</table>
<cc2:datagrid id="SpecialUserList"  PageSize="25"  runat="server" Width="100%" ColumnSpan="7">
	<Columns>
		<asp:TemplateColumn HeaderText="选择">
		<HeaderStyle Width="10%" /><ItemStyle width="10%"/>
			<ItemTemplate>
				<asp:CheckBox id="userid" runat="server"></asp:CheckBox>
				<%# SpecialUserList.LoadSelectedCheckBox(Eval("uid").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn><HeaderStyle Width="15%" /><ItemStyle width="15%"/>
			<HeaderTemplate>
				用户名
			</HeaderTemplate>
			<ItemTemplate>
				<input type="checkbox" onclick="CheckRow(this.form,this.checked,<%# Convert.ToInt32(Eval("id")) + 2%>)" />&nbsp;
				<%# (Eval("name"))%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn ><HeaderStyle Width="15%" /><ItemStyle width="15%"/>
			<HeaderTemplate>
				<input type="checkbox" onclick="Check(this.form,this.checked,':viewbyuser')" />&nbsp;浏览论坛
			</HeaderTemplate>
			<ItemTemplate>
				<asp:CheckBox id="viewbyuser" runat="server" Checked='<%# (Eval("viewbyuser"))%>'></asp:CheckBox>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn><HeaderStyle Width="15%" /><ItemStyle width="15%"/>
			<HeaderTemplate>
				<input type="checkbox" onclick="Check(this.form,this.checked,':postbyuser')" />&nbsp;发布商品
			</HeaderTemplate>
			<ItemTemplate>
				<asp:CheckBox id="postbyuser" runat="server" Checked='<%# (Eval("postbyuser"))%>'></asp:CheckBox>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn ><HeaderStyle Width="15%" /><ItemStyle width="15%"/>
			<HeaderTemplate>
				<input type="checkbox" onclick="Check(this.form,this.checked,':replybyuser')" />&nbsp;发表留言
			</HeaderTemplate>
			<ItemTemplate>
				<asp:CheckBox id="replybyuser" runat="server" Checked='<%# (Eval("replybyuser"))%>'></asp:CheckBox>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn ><HeaderStyle Width="15%" /><ItemStyle width="15%"/>
			<HeaderTemplate>
				<input type="checkbox" onclick="Check(this.form,this.checked,':getattachbyuser')" />&nbsp;下载/查看附件
			</HeaderTemplate>
			<ItemTemplate>
				<asp:CheckBox id="getattachbyuser" runat="server" Checked='<%# (Eval("getattachbyuser"))%>'></asp:CheckBox>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn ><HeaderStyle Width="15%" /><ItemStyle width="15%"/>
			<HeaderTemplate>
				<input type="checkbox" onclick="Check(this.form,this.checked,':postattachbyuser')" />&nbsp;上传附件
			</HeaderTemplate>
			<ItemTemplate>
				<asp:CheckBox id="postattachbyuser" runat="server" Checked='<%# (Eval("postattachbyuser"))%>'></asp:CheckBox>
			</ItemTemplate>
		</asp:TemplateColumn>			
	</Columns>
</cc2:datagrid>
<br />
<uc1:PageInfo id="info1" runat="server" Icon="Information" Text="授予某些用户在本版块一些特殊权限,在下面输入框用中输入要给予特殊权限的用户列表,以&quot;,&quot;分隔"></uc1:PageInfo>
<table width="100%">
	<tr><td class="item_title" colspan="2">增加特殊用户列表</td></tr>
	<tr>
		<td class="vtop" colspan="2">
			 <uc1:TextareaResize id="UserList" runat="server" controlname="TabControl1:tabPage4:UserList" Cols="40" Rows="2"></uc1:TextareaResize>            	
			 &nbsp;&nbsp;<cc2:Button id="BindPower" runat="server" Text=" 增加 "></cc2:Button>
		</td>
	</tr>
</table>					
</cc2:TabPage>
<cc2:TabPage Caption="主题分类" ID="tabPage5">
<table width="100%">
	<tr><td class="item_title" colspan="2">启用主题分类</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="applytopictype" runat="server">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc2:RadioButtonList>
		</td>
		<td class="vtop">设置是否在本版块启用主题分类功能,您需要同时设定相应的分类选项,才能启用本功能</td>
	</tr>
	<tr><td class="item_title" colspan="2">发帖必须归类</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="postbytopictype" runat="server">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc2:RadioButtonList>
		</td>
		<td class="vtop">如果选择"是",作者发新主题时,必须选择主题对应的类别才能发表.本功能必须"启用主题分类"后才可使用</td>
	</tr>
	<tr><td class="item_title" colspan="2">允许按类别浏览</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="viewbytopictype" runat="server">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc2:RadioButtonList>
		</td>
		<td class="vtop">如果选择"是",用户将可以在本论坛中按照不同的类别浏览主题.注意: 本功能必须"启用主题分类"后才可使用并会加重服务器负担</td>
	</tr>
	<tr><td class="item_title" colspan="2">类别前缀</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="topictypeprefix" runat="server" HintTitle="提示" HintInfo="">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc2:RadioButtonList>
		</td>
		<td class="vtop">设置是否在主题列表中,给已分类的主题前加上类别的显示.注意: 本功能必须"启用主题分类"后才可使用</td>
	</tr>
</table>
<br />
<cc2:datagrid id="TopicTypeDataGrid" OnSortCommand="Sort_Grid" PageSize="10" runat="server">
<Columns>
	<asp:BoundColumn DataField="id" HeaderText="id" Visible="false"></asp:BoundColumn>
	<asp:BoundColumn DataField="name" HeaderText="主题分类"><HeaderStyle Width="15%" /></asp:BoundColumn>
	<asp:BoundColumn DataField="description" HeaderText="描述"><HeaderStyle Width="40%" /></asp:BoundColumn>
	<asp:BoundColumn HeaderText="不使用"><HeaderStyle Width="15%" /></asp:BoundColumn>
	<asp:BoundColumn HeaderText="使用(平板显示)"><HeaderStyle Width="15%" /></asp:BoundColumn>	
	<asp:BoundColumn HeaderText="使用(下拉显示)"><HeaderStyle Width="15%" /></asp:BoundColumn>				
</Columns>
</cc2:datagrid>
<table width="100%">
	<tr><td class="item_title" colspan="2">主题分类名</td></tr>
	<tr>
		<td class="vtop rowform">
			<input name="typename" type="text" maxlength="200" id="typename" maxlength="200" size="10" />
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">显示顺序</td></tr>
	<tr>
		<td class="vtop rowform">
			<input name="typeorder" type="text" maxlength="4" id="typeorder" maxlength="4" size="3" />
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">描述</td></tr>
	<tr>
		<td class="vtop rowform">
			<input name="typedescription" type="text" maxlength="500" id="typedescription" maxlength="500" size="10" />
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<div class="Navbutton"><button type="button" class="ManagerButton" id="AddNewRec" onclick="AddTopicType();"><img src="../images/submit.gif"/> 新增主题分类 </button></div>
<div id="resultmessage" style="display:none"></div>
</cc2:TabPage>
<cc2:TabPage Caption="统计信息" ID="tabPage6">
	<asp:Label id="forumsstatic" runat="server" Visible="true"></asp:Label>
	<br />
	<br /><cc2:Button ID="RunForumStatic" runat="server" ButtontypeMode="Normal" Text="统计最新信息" />
	<%=runforumsstatic%>
</cc2:TabPage>	
</cc2:TabControl>
<div id="topictypes" style="display:none;width:100%;">
<table width="100%">
	<tr><td class="item_title" colspan="2">显示顺序</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:TextBox id="displayorder" runat="server"  CanBeNull="必填" RequiredFieldType="数据校验"></cc2:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">主题分类</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:textbox id="topictypes" runat="server" RequiredFieldType="暂无校验" width="370" height="50" TextMode="MultiLine"></cc2:textbox>
		</td>
		<td class="vtop"></td>
	</tr>
 <table>
</div>
<div class="Navbutton">
<cc2:Button id="SubmitInfo" runat="server" Text=" 提 交 "></cc2:Button>&nbsp;&nbsp;
<button onclick="window.location='forum_forumstree.aspx';" id="Button3" class="ManagerButton" type="button"><img src="../images/arrow_undo.gif"/> 返 回 </button>
</div>
<cc2:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc2:Hint>							
<script type="text/javascript">
function editcredit(fid,fieldname)
{
window.location="forum_ScoreStrategy.aspx?fid="+fid+"&fieldname="+fieldname;
}
</script>					
</form>
<%=footer%>	
</body>
</html>