<%@ Page language="c#" Inherits="BBX.Web.Admin.editforums" Codebehind="forum_editforums.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>编辑版块</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/tab.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../js/tabstrip.js"></script>
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
function AddTopicType()
{
	typename = $("typename").value;
	typeorder = $("typeorder").value;
	typedescription = $("typedescription").value;
	if(typename == "")
	{
		alert("主题分类名称不能为空！");
		$("typename").focus();
		return false;
	}
	if(!(/^\d+$/.test(typeorder)))
	{
		alert("显示顺序不能为空并且只能为数字！");
		$("typeorder").value = "";
		$("typeorder").focus();
		return false;
	}
	AjaxHelper.Updater('../UserControls/addtopictype','resultmessage','typename='+typename+'&typeorder='+typeorder+'&typedescription='+typedescription,ResultFun);
	BOX_remove('neworedit');
}
function ResultFun()
{
	resultstring = $("resultmessage").innerHTML;
	if(resultstring.indexOf("false") == -1)
	{
	    var maxId = resultstring.replace(" </FORM>", "");
		if(/\s+/.test(maxId))
		{
		    maxId = maxId.substring(1, maxId.length - 4);//过滤空格,已保证主题分类能被识别
		}
		var theDoc = document;
		var typetable = $("TabControl1_tabPage5_TopicTypeDataGrid");
		var tbody = typetable.lastChild;
		lasttr = tbody.lastChild;
		tbody.removeChild(tbody.childNodes[tbody.childNodes.length - 1]);
		rowscount = tbody.childNodes.length - 1;
		
		tr = theDoc.createElement("tr");
		if(window.navigator.appName == "Netscape")
		{                
			tr.setAttribute("onmouseover","this.className='mouseoverstyle'");                    
			tr.setAttribute("onmouseout","this.className='mouseoutstyle'");                    
			tr.setAttribute("style","cursor:hand;");
		}
		else
		{
			tr.onmouseover = "this.className='mouseoverstyle'";
			tr.onmouseout = "this.className='mouseoutstyle'";
			tr.style.cursor = "hand";
		}
		
		td = GetTd();
		td.innerHTML = $("typename").value;
		tr.appendChild(td);
		
		td = GetTd();
		td.innerHTML = $("typedescription").value;
		tr.appendChild(td);
		
		td = GetTd();
		td.innerHTML = "<input type='hidden' name='oldtopictype" + rowscount + "' value='' /><input type='radio' name='type" + rowscount + "' value='-1' />";
		tr.appendChild(td);
		
		td = GetTd();
		td.innerHTML = "<input type='radio' name='type" + rowscount + "' checked value='"+maxId+"," + $("typename").value + ",0|' />";
		tr.appendChild(td);
		
		td = GetTd();
		td.innerHTML = "<input type='radio' name='type" + rowscount + "' value='"+maxId+"," + $("typename").value + ",1|' />";
		tr.appendChild(td);
		
		tbody.appendChild(tr);
		tbody.appendChild(lasttr);
		$("typename").value = "";
		$("typeorder").value = "";
		$("typedescription").value = "";
	}
	else
	{
		alert("数据库中已存在相同的主题分类名称");
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

function forumicotype(status) {
    if(status)
	{
		document.getElementById("uploadforumicon").style.display='';
		document.getElementById("icon_layer").style.display='none';
	}
	else
	{
		document.getElementById("uploadforumicon").style.display='none';
		document.getElementById("icon_layer").style.display='';
	}
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" enctype="multipart/form-data" method="post" runat="server">
<div class="ManagerForm">
<span style="font-size:12px">当前版块为: <b><asp:literal id="forumname" runat="server"></asp:literal></b></span>
<cc2:TabControl id="TabControl1" SelectionMode="Client" runat="server" TabScriptPath="../js/tabstrip.js"  height="100%">
<cc2:TabPage Caption="基本信息" ID="tabPage1" >
<table width="100%">
	<tr><td class="item_title" colspan="2">版块名称</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:TextBox id="name" runat="server" CanBeNull="必填"  IsReplaceInvertedComma="false"   size="30"  MaxLength="49"></cc2:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">是否显示</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="status" runat="server" RepeatColumns="2" HintInfo="设置本版块是否是隐藏版块" >
				<asp:ListItem Value="1" Selected="True">显示</asp:ListItem>
				<asp:ListItem Value="0" >不显示</asp:ListItem>
			</cc2:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>
	<tbody id="templatestyle" runat="server">
	<tr><td class="item_title" colspan="2">模板风格</td></tr>
	<tr>
		<td class="vtop rowform">
		<cc2:DropDownList id="templateid" runat="server" HintInfo="设置本版块使用的模板,将不受系统模板限制"></cc2:DropDownList> <input type="checkbox" name="childForumApplyTemplate" id="childForumApplyTemplate" value="1"  runat="server" />应用到所有子版
		</td>
		<td class="vtop">设置本版块使用的模板,将不受系统模板限制</td>
	</tr>
	</tbody>
	<tr><td class="item_title" colspan="2">显示模式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="colcount" runat="server" RepeatColumns="1">
				<asp:ListItem Value="1">传统模式[默认]</asp:ListItem>
				<asp:ListItem Value="2">子版块横排模式</asp:ListItem>
			</cc2:RadioButtonList>
			<div id="showcolnum" runat="server">
				<cc2:TextBox id="colcountnumber" runat="server" Size="2" Text="4" MaxLength="1"></cc2:TextBox>
			</div>
		</td>
		<td class="vtop">用来设置该论坛(或分类)的子论坛在列表中的显示方式,选择"子版块横排模式",则子分类列表按每行按输入的数字个数出现</td>
	</tr>
	<tr><td class="item_title" colspan="2">版主列表</td></tr>
	<tr>
		<td class="vtop rowform">
			<uc1:TextareaResize id="moderators" runat="server" Width="450px" controlname="TabControl1:tabPage1:moderators" Cols="30" Rows="5"></uc1:TextareaResize>
		</td>
		<td class="vtop">当前版块版主列表，以","进行分割,以','进行分割,如:lisi,zhangsan</td>
	</tr>
	<tr><td class="item_title" colspan="2">已继承的版主</td></tr>
	<tr>
		<td class="vtop rowform">
			<asp:Literal ID="inheritmoderators" runat="server"></asp:Literal>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">URL重写名称</td></tr>
	<tr>
		<td class="vtop rowform">
			<%=root%><cc2:textbox id="rewritename" runat="server" Width="250px" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false"  MaxLength="20" ></cc2:textbox>/<%if (config.Iisurlrewrite == 0){%>list.aspx<%}%>
								        <input type="hidden" id="oldrewritename" runat="server" />
		</td>
		<td class="vtop">设置版块URL重写,会以"http://www.newlifex.com/rewritename/"的形式显示,以字母开头,其后可以是字母或数字,但不可包含<br>"admin,aspx,tools,archive"中任意一个字符串</td>
	</tr>
	<tr><td class="item_title" colspan="2">版块描述</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:textbox id="description" runat="server" width="450" height="100" TextMode="MultiLine" IsReplaceInvertedComma="false"></cc2:textbox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">本版块SEO关键词</td></tr>
	<tr>
		<td class="vtop rowform">
			  <cc2:textbox id="seokeywords" runat="server" RequiredFieldType="暂无校验" width="450" height="100" TextMode="MultiLine" IsReplaceInvertedComma="false"></cc2:textbox>
		</td>
		<td class="vtop">设置本版块的SEO关键词,用于搜索引擎优化,放在meta的keyword标签中,多个关键字间请用半角逗号","隔开</td>
	</tr>
	<tr><td class="item_title" colspan="2">本版块SEO描述</td></tr>
	<tr>
		<td class="vtop rowform">
			  <cc2:textbox id="seodescription" runat="server" RequiredFieldType="暂无校验" width="450" height="100" TextMode="MultiLine" IsReplaceInvertedComma="false"></cc2:textbox>
		</td>
		<td class="vtop">设置本版块的SEO描述,用于搜索引擎优化,放在meta的description标签中,多个说明文字间请用半角逗号","隔开</td>
	</tr>
</table>
</cc2:TabPage>
<cc2:TabPage Caption="高级设置" ID="tabPage2">
<table width="100%">
    <tr><td class="item_title" colspan="2">版块图标</td></tr>
     <%if (!String.IsNullOrEmpty(forumInfo.Icon))
      { %>
	<tr>
		<td class="vtop rowform" style="padding-bottom:10px;">
            <img src="<%=forumInfo.Icon + "?t="+ DateTime.Now.Ticks.ToString()%>" alt="forumicon" />
		</td>
	</tr><%} %>
	<tr>
        <td class="vtop rowform" style="padding-bottom:10px;">
			<input name="logotype" type="radio" value="" checked onClick="forumicotype(0)">
			外链<input name="logotype" type="radio" value="" onClick="forumicotype(1)">上传
		</td>
        <td class="vtop">显示于首页论坛版块名称前，可填写网络地址，和使用下方上传功能上传本地图片。留空为不设置版块图标</td>
	</tr>
	<tr>
        <td class="vtop rowform" style="padding-bottom:10px;">
			 <div id="icon_layer"><cc2:textbox id="icon" runat="server" Width="250px" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false"  MaxLength="253"></cc2:textbox></div>
			 <input id="uploadforumicon" type="file" name="Filedata" onChange="validatefiletype(this);" style="display:none"/>
		</td>
        <td class="vtop">显示于首页论坛版块名称前，可填写网络地址，和使用下方上传功能上传本地图片。留空为不设置版块图标</td>
	</tr>
    <tr><td class="item_title" colspan="2">访问本版块的密码</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:textbox id="password" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false" MaxLength="16" Size="20"></cc2:textbox>
		</td>
		<td class="vtop">设置本版块的密码,留空则本版块不使用密码</td>
	</tr>
	<tr><td class="item_title" colspan="2">发主题积分策略</td></tr>
	<tr>
		<td class="vtop rowform">
			 <a href="#" class="TextButton" onClick="javascript:editcredit('<%=Request.Params["fid"]%>','postcredits');" >编 辑</a>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">发回复积分策略</td></tr>
	<tr>
		<td class="vtop rowform">
			  <a href="#" class="TextButton" onClick="javascript:editcredit('<%=Request.Params["fid"]%>','replycredits');" >编 辑</a>
		</td>
		<td class="vtop"></td>
	</tr>
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
		<td class="vtop">允许在本版块上传的<a href="forum_attchemnttypes.aspx">附件类型</a>,留空为使用用户组设置, 且版块设置优先于用户组设置</td>
	</tr>
	<tr><td class="item_title" colspan="2">本版块规则</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:textbox id="rules" runat="server" HintTitle="提示" HintInfo="支持Html" RequiredFieldType="暂无校验" width="250" height="100" TextMode="MultiLine" IsReplaceInvertedComma="false"></cc2:textbox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">定期自动关闭主题</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="autocloseoption" runat="server" RepeatColumns="1">
				<asp:ListItem Value="0" Selected="True">不自动关闭</asp:ListItem>
				<asp:ListItem Value="1">按发布时间</asp:ListItem>
			</cc2:RadioButtonList>
			<div id="showclose" runat="server">
				<cc2:TextBox id="autocloseday" runat="server" RequiredFieldType="数据校验" Size="4" MaxLength="3"></cc2:TextBox>
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
	<tr><td class="item_title" colspan="2">设置</td></tr>
	<tr>
		<td class="vtop" colspan="2">
			<cc2:CheckBoxList id="setting" runat="server" RepeatColumns="4" >
				<asp:ListItem Value="allowsmilies">允许使用表情符</asp:ListItem>
				<asp:ListItem Value="allowrss">允许RSS和Sitemap</asp:ListItem>
				<asp:ListItem Value="allowbbcode">允许论坛代码</asp:ListItem>
				<asp:ListItem Value="allowimgcode">允许[img]代码</asp:ListItem>
				<asp:ListItem Value="recyclebin">打开主题回收站</asp:ListItem>
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
</table>
</cc2:TabPage>
<cc2:TabPage Caption="权限设定" ID="tabPage3">
	<uc1:PageInfo id="PageInfo1" runat="server" Icon="Information" Text="每个组的权限项不选择为使用用户组设置，且版块设置优先于&lt;a href=&quot;../global/global_editgroup.aspx&quot;&gt;用户组设置&lt;/a&gt;."></uc1:PageInfo>    			
	<table width="100%" id="powerset" align="center" class="table1" cellspacing="0" cellPadding="4"  bgcolor="#C3C7D1" runat="server">	
		<tr>
			<td class="td_alternating_item2" width="1%">&nbsp;</td>
			<td class="td_alternating_item2" width="20%" style="word-wrap: break-word">&nbsp;</td>
			<td class="td_alternating_item2"><input type="checkbox" id="c1" onClick="seleCol('viewperm',this.checked)"/><label for="c1">浏览论坛</label></td>
			<td class="td_alternating_item2"><input type="checkbox" id="c2" onClick="seleCol('postperm',this.checked)"/><label for="c2">发新话题</label></td>
			<td class="td_alternating_item2"><input type="checkbox" id="c3" onClick="seleCol('replyperm',this.checked)"/><label for="c3">发表回复</label></td>
			<td class="td_alternating_item2"><input type="checkbox" id="c4" onClick="seleCol('getattachperm',this.checked)"/><label for="c4">下载/查看附件</label></td>
			<td class="td_alternating_item2"><input type="checkbox" id="c5" onClick="seleCol('postattachperm',this.checked)"/><label for="c5">上传附件</label></td>
		</tr>
	</table>
</cc2:TabPage>
<cc2:TabPage Caption="特殊用户" ID="tabPage4">
	<table width="100%" align="center" class="table1" cellspacing="0" cellPadding="4"  bgcolor="#C3C7D1">
		<tr>
			<td class="category">
				<input title="选中/取消选中 本页所有Case" onClick="Check(this.form,this.checked,'userid')" type="checkbox" name="chkall" id="chkall" />全选/取消全选 &nbsp; 
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
					<%# SpecialUserList.LoadSelectedCheckBox(Eval("uid")+"")%>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn><HeaderStyle Width="15%" /><ItemStyle width="15%"/>
				<HeaderTemplate>
					用户名
				</HeaderTemplate>
				<ItemTemplate>
					<input type="checkbox" onClick="CheckRow(this.form,this.checked,<%# Convert.ToInt32(Eval("id")) + 2%>)" />&nbsp;
					<%# Eval("name")%>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn ><HeaderStyle Width="15%" /><ItemStyle width="15%"/>
				<HeaderTemplate>
					<input type="checkbox" onClick="Check(this.form,this.checked,':viewbyuser')" />&nbsp;浏览论坛
				</HeaderTemplate>
				<ItemTemplate>
					<asp:CheckBox id="viewbyuser" runat="server" Checked='<%# Eval("viewbyuser")%>'></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn><HeaderStyle Width="15%" /><ItemStyle width="15%"/>
				<HeaderTemplate>
					<input type="checkbox" onClick="Check(this.form,this.checked,':postbyuser')" />&nbsp;发新话题
				</HeaderTemplate>
				<ItemTemplate>
					<asp:CheckBox id="postbyuser" runat="server" Checked='<%# Eval("postbyuser")%>'></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn ><HeaderStyle Width="15%" /><ItemStyle width="15%"/>
				<HeaderTemplate>
					<input type="checkbox" onClick="Check(this.form,this.checked,':replybyuser')" />&nbsp;发表回复
				</HeaderTemplate>
				<ItemTemplate>
					<asp:CheckBox id="replybyuser" runat="server" Checked='<%# Eval("replybyuser")%>'></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn ><HeaderStyle Width="15%" /><ItemStyle width="15%"/>
				<HeaderTemplate>
					<input type="checkbox" onClick="Check(this.form,this.checked,':getattachbyuser')" />&nbsp;下载/查看附件
				</HeaderTemplate>
				<ItemTemplate>
					<asp:CheckBox id="getattachbyuser" runat="server" Checked='<%# Eval("getattachbyuser")%>'></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn ><HeaderStyle Width="15%" /><ItemStyle width="15%"/>
				<HeaderTemplate>
					<input type="checkbox" onClick="Check(this.form,this.checked,':postattachbyuser')" />&nbsp;上传附件
				</HeaderTemplate>
				<ItemTemplate>
					<asp:CheckBox id="postattachbyuser" runat="server" Checked='<%# Eval("postattachbyuser")%>'></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>			
		</Columns>
	</cc2:datagrid>
	<uc1:PageInfo id="info1" runat="server" Icon="Information" Text="授予某些用户在本版一些特殊权限,在下面输入框用中输入要给予特殊权限的用户列表,以&quot;,&quot;分隔"></uc1:PageInfo>
	<table width="100%">
	<tr><td class="item_title" colspan="2">增加特殊用户列表</td></tr>
	<tr>
		<td class="vtop rowform">
			  <uc1:TextareaResize id="UserList" runat="server" controlname="TabControl1:tabPage4:UserList" Cols="40" Rows="2"></uc1:TextareaResize>            	
			  &nbsp;&nbsp;<cc2:Button id="BindPower" runat="server" Text=" 增加 "></cc2:Button>
		</td>
		<td class="vtop"></td>
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
		<td class="vtop">如果选择"是",用户将可以在本版块中按照不同的类别浏览主题.注意: 本功能会加重服务器负担</td>
	</tr>
	<tr><td class="item_title" colspan="2">类别前缀</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:RadioButtonList id="topictypeprefix" runat="server">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc2:RadioButtonList>
		</td>
		<td class="vtop">设置是否在主题列表中,给已分类的主题前加上类别的显示</td>
	</tr>
</table>
<br />
<cc2:datagrid id="TopicTypeDataGrid" OnSortCommand="Sort_Grid" PageSize="20" runat="server">
	<Columns>
		<asp:BoundColumn DataField="id" HeaderText="id" Visible="false"></asp:BoundColumn>
		<asp:BoundColumn DataField="name" HeaderText="主题分类"><HeaderStyle Width="15%" /></asp:BoundColumn>
		<asp:BoundColumn DataField="description" HeaderText="描述"><HeaderStyle Width="40%" /></asp:BoundColumn>
		<asp:BoundColumn HeaderText="不使用"><HeaderStyle Width="15%" /></asp:BoundColumn>
		<asp:BoundColumn HeaderText="使用(平板显示)"><HeaderStyle Width="15%" /></asp:BoundColumn>	
		<asp:BoundColumn HeaderText="使用(下拉显示)"><HeaderStyle Width="15%" /></asp:BoundColumn>				
	</Columns>
</cc2:datagrid>
<div class="Navbutton"><button type="button" class="ManagerButton" id="AddNewRec" onclick="BOX_show('neworedit');"><img src="../images/submit.gif"/> 新增主题分类 </button></div>

<div id="resultmessage" style="display:none"></div>
</cc2:TabPage>
<cc2:TabPage Caption="统计信息" ID="tabPage6">
	<asp:Label id="forumsstatic" runat="server" Visible="true"></asp:Label>
	<br />
	<br>
   <img src="../images/dot.gif"><a href="forum_updateforumstatic.aspx">论坛维护</a>
	<br>
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
</table>
</div>
<div class="Navbutton">
	<cc2:Button id="SubmitInfo" runat="server" Text=" 提 交 "></cc2:Button>&nbsp;&nbsp;
	<button onClick="window.location='forum_forumstree.aspx';" id="Button3" class="ManagerButton" type="button"><img src="../images/arrow_undo.gif"/> 返 回 </button>
</div>
<cc2:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc2:Hint>							
<script type="text/javascript">
	function editcredit(fid,fieldname)
	{
		window.location="forum_ScoreStrategy.aspx?fid="+fid+"&fieldname="+fieldname;
    }

    function validatefiletype(obj) {
        if (obj.value == '')
            return false;
        var fileext = obj.value.substr(obj.value.lastIndexOf('.')).toLowerCase();

        var allowext = new Array('jpg', 'png', 'gif', 'bmp', 'jpeg');
        var isallowext = false;

        for (i = 0; i < allowext.length; i++) {
            if (fileext == '.'+ allowext[i]) {
                isallowext = true;
                break;
            }
        }
        if (!isallowext)
            alert('您选择的文件不是合法的图片文件');
    }
</script>	
</div>


<div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:400px;">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/icon45.jpg) no-repeat 6px 50%;">添加主题分类</legend>
<table width="100%">
	<tr><td style="width: 90px;height:35px">主题分类名</td>
		<td>
			 <input name="typename" type="text" id="typename" MaxLength="30" size="30" />
		</td>
	</tr>
	<tr><td style="height:35px">显示顺序</td>
		<td>
			 <input name="typeorder" type="text" maxlength="4" id="typeorder" maxlength="4" size="3" />
		</td>
	</tr>
	<tr><td style="height:35px">描述</td>
		<td>
			 <input name="typedescription" type="text" maxlength="500" id="typedescription" maxlength="500" size="10" />
		</td>
	</tr>
</table>
<div class="Navbutton"><button type="button" class="ManagerButton" id="AddNewRec" onClick="AddTopicType();"><img src="../images/submit.gif"/> 增加 </button>
<button type="button" class="ManagerButton" onClick="BOX_remove('neworedit');"><img src="../images/submit.gif"/> 取消 </button>
</div>

</fieldset>
</div>
</div>

</form>
<div id="setting" />
<%=footer%>	
</body>
</html>