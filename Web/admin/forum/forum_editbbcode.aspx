<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<%@ Page language="c#" Inherits="BBX.Web.Admin.editbbcode" Codebehind="forum_editbbcode.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title><%=BBX.Common.Utils.ProductName%>代码编辑</title>		
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/icon47.jpg) no-repeat 6px 50%;">编辑<%=BBX.Common.Utils.ProductName%>代码</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">是否生效</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:radiobuttonlist id="available" runat="server">
				<asp:ListItem Value="1">生效</asp:ListItem>
				<asp:ListItem Value="0" Selected="True">不生效</asp:ListItem>
			</cc1:radiobuttonlist>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">图标</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:UpFile id="icon" runat="server" UpFilePath="../../editor/images/" FileType=".jpg|.gif|.png" IsShowTextArea="false" Width="60px"></cc1:UpFile>
		</td>
		<td class="vtop">建议图片宽高为21X20像素</td>
	</tr>
	<tr><td class="item_title" colspan="2">标签</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:textbox id="tag" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验" MaxLength="100" TextMode="MultiLine" Rows="3" Cols="20"   ></cc1:textbox>
		</td>
		<td class="vtop">方括号中的标签代码, 如 [tag] 的标签为 "tag"(不含引号)</td>
	</tr>
	<tr><td class="item_title" colspan="2">替换内容</td></tr>
	<tr>
		<td class="vtop rowform">
			 <uc1:TextareaResize id="replacement" runat="server" controlname="replacement" cols="30" is_replace="true"  HintShowType="down" HintHeight="0"></uc1:TextareaResize>	
		</td>
		<td class="vtop">标签替换为的 html 代码内容, 支持至多三个动态参数
        <table width="300"><tr><td>{1} 代表第一个参数</td><td>{rn} 代表换行回车</td></tr>
        <tr><td>{2} 代表第二个参数</td><td>{nn} 代表两个回车</td></tr>
        <tr><td>{3} 代表第三个参数</td><td>{r} 代表换行</td></tr>
        <tr><td>{RANDOM}代表随机生成的字符串</td><td>{n} 代表回车</td></tr>
        </table>
        </td>
	</tr>
	<tr><td class="item_title" colspan="2">例子</td></tr>
	<tr>
		<td class="vtop rowform">
			 <uc1:TextareaResize id="example" runat="server" controlname="example" cols="30" is_replace="true" maxlength="254"></uc1:TextareaResize>
		</td>
		<td class="vtop">本代码作用的例子</td>
	</tr>
	<tr><td class="item_title" colspan="2">解释</td></tr>
	<tr>
		<td class="vtop rowform">
			 <uc1:TextareaResize id="explanation" runat="server" HintTitle="提示" HintInfo="本代码功能的解释" controlname="explanation" cols="30" is_replace="true"></uc1:TextareaResize>
		</td>
		<td class="vtop">本代码作用的例子</td>
	</tr>
	<tr><td class="item_title" colspan="2">参数个数</td></tr>
	<tr>
		<td class="vtop rowform">
		<cc1:TextBox id="param" runat="server" HintTitle="提示" HintInfo="本代码中使用到的动态参数个数" CanBeNull="必填" RequiredFieldType="数据校验" Text="0" size="3"></cc1:TextBox>
		</td>
		<td class="vtop">本代码中使用到的动态参数个数</td>
	</tr>
	<tr><td class="item_title" colspan="2">参数描述</td></tr>
	<tr>
		<td class="vtop rowform">
		 <uc1:TextareaResize id="paramsdescript" runat="server" controlname="paramsdescript" cols="30" is_replace="true"></uc1:TextareaResize>
		</td>
		<td class="vtop">在插入自定义标签时会弹出录入框,此描述为录入框提示信息,对应参数个数.多于一个参数,请用半角逗号(,)分隔.</td>
	</tr>
	<tr><td class="item_title" colspan="2">嵌套次数</td></tr>
	<tr>
		<td class="vtop rowform">
		<cc1:textbox id="nest" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" size="3"></cc1:textbox>
		</td>
		<td class="vtop">最大解析的代码嵌套次数(深度), 范围从 1～3</td>
	</tr>
	<tr><td class="item_title" colspan="2">params默认值</td></tr>
	<tr>
		<td class="vtop rowform">
		 <uc1:TextareaResize id="paramsdefvalue" runat="server" controlname="paramsdefvalue" cols="30" is_replace="true"></uc1:TextareaResize>
		</td>
		<td class="vtop">在插入自定义标签时会弹出录入框,此默认值为录入框中默认显示的数值,对应参数个数.多于一个参数,请用半角逗号(,)分隔.</td>
	</tr>
</table>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div class="Navbutton">
	<cc1:Button id="UpdateBBCodeInfo" runat="server" Text=" 提 交 "></cc1:Button>&nbsp;&nbsp;
	<cc1:Button id="DeleteBBCode" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" OnClientClick="if(!confirm('你确认要删除所选吗？')) return false;"></cc1:Button>
</div>
</fieldset>
</div>
</form>
<%=footer%>
</body>
</html>