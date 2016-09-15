<%@ Page Language="c#" Inherits="BBX.Web.Admin.wordgrid" Codebehind="global_wordgrid.aspx.cs" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>过滤词列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form,'id','DelRec')
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<uc1:PageInfo id="info2" runat="server" Icon="warning" Text="替换前的内容可以使用限定符 {x} 以限定相邻两字符间可忽略的文字，x 是忽略字符的个数。如 'a{1}s{2}s '(不含引号) 可以过滤 'ass' 也可过滤 'axsxs' 和 'axsxxs' 等等。<BR />查询的内容和替换的内容的最大长度为254个字符"></uc1:PageInfo>
<uc1:PageInfo id="PageInfo1" runat="server" Icon="warning" Text="如需禁止发布包含某个词语的文字，而不是替换过滤，请将其对应的替换内容设置为{BANNED}即可；如需当用户发布包含某个词语的文字时，自动标记为需要人工审核，而不直接显示或替换过滤，请将其对应的替换内容设置为{MOD}即可。<font color=red>设置 '{BANNED}' 或 '{MOD}' 请务必使用大写字母！</font>"></uc1:PageInfo>
<uc1:PageInfo id="info1" runat="server" Icon="information" Text="查询的内容和替换的内容为必填项, 为不影响程序效率, 请不要设置过多不需要的过滤内容"></uc1:PageInfo>

屏蔽特殊字符:<br />
某些广告发布者会将关键字用各种特殊字符拆开分割，造成过滤程序无法正确识别，<br />
如果将特殊字符填写在下面的文本框当中，则过滤程序会在过滤之前，首先屏蔽文本框内的字符，让不良关键词无所遁形！<br />
填写方法:直接填写即可，字符直接无需任何标识风格。比如“*&^%$#”(不包括双引号)<br /><br />

<cc1:TextBox id="antipamreplacement" IsReplaceInvertedComma="false" runat="server" HintShowType="down" CssClass="" RequiredFieldType="暂无校验" TextMode="MultiLine" Height="90px" Rows="4" Cols="50"  HintTitle="提示" HintInfo="屏蔽特殊字符" HintPosOffSet="160"></cc1:TextBox><br /><br />
<cc1:Button ID="saveantipamreplacement" runat="server" Text="保 存" ></cc1:Button><br /><br />


批量添加:<br />
每行一组过滤词语，不良词语和替换词语之间使用“=”进行分割;<br />
如果只是想将某个词语直接替换成 **,则只输入词语即可;<br />
<a href="http://www.newlifex.com">下载官方提供的最新词库文件</a>
<br /><a href="###" id="hitmessage" onclick="document.getElementById('messageinfo').style.display='';document.getElementById('hitmessage').style.display='none';">显示全部提示...</a>
<div id="messageinfo" style="display:none">
例如:<br />toobad<br />nobad<br />badword=good<br />sexword={BANNED}<br />审核={MOD}
</div>
<br />
  <cc1:TextBox id="badwords" runat="server" HintShowType="down" CssClass="" RequiredFieldType="暂无校验" TextMode="MultiLine" Height="90px" Rows="4" Cols="50"  HintTitle="提示" HintInfo="禁止发布的词语" HintPosOffSet="160"></cc1:TextBox>
  <br /><br />
  <cc1:RadioButtonList id="radfilter" runat="server"  RepeatColumns="1" RepeatLayout="flow">
	<asp:ListItem Value="0">清空当前词表后导入新词语，此操作不可恢复，建议首先 <a href="global_ajaxcall.aspx?opname=downloadword"><b>导出词表</b></a> 做好备份</asp:ListItem>
	<asp:ListItem Value="1">使用新的设置覆盖已经存在的词语</asp:ListItem>
	<asp:ListItem Value="2" Selected="true">不导入已经存在的词语</asp:ListItem>
 </cc1:RadioButtonList> 
 <br />
<cc1:Button ID="addbadwords" runat="server" Text="提 交" ></cc1:Button><br /><br />
<cc1:DataGrid ID="DataGrid1" runat="server" IsFixConlumnControls="true" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid">
	<Columns>
		<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<HeaderStyle Width="20px" />
			<ItemTemplate>
				<input id="id" onclick="checkedEnabledButton(this.form,'id','DelRec')" type="checkbox" value="<%# Eval("id").ToString() %>"	name="id" />
				<%# DataGrid1.LoadSelectedCheckBox(Eval("id").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="id" SortExpression="id" HeaderText="id [递增]" Visible="false" />
		<asp:BoundColumn DataField="admin" SortExpression="admin" HeaderText="提交人" readonly="true" />
		<asp:BoundColumn DataField="find" SortExpression="find" HeaderText="查询的内容" />
		<asp:BoundColumn DataField="replacement" SortExpression="replacement" HeaderText="替换的内容" />
	</Columns>
</cc1:DataGrid>
<p style="text-align:right;">
	<cc1:Button ID="SaveWord" runat="server" Text="保存过滤词修改"></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button2" onclick="BOX_show('neworedit');"><img src="../images/add.gif"/> 新建过滤词 </button>&nbsp;&nbsp;
	<cc1:Button ID="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选的词语过滤吗？')) return false;"></cc1:Button>
</p>
 <div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
<div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:350px;">
<div class="ManagerForm">
<fieldset>
<legend style="background: url(../images/icons/icon48.jpg) no-repeat 6px 50%;">添加过滤词</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">过滤内容</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:TextBox ID="find" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="true" MaxLength="254" Size="30"></cc2:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">替换为</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:TextBox ID="replacement" runat="server" RequiredFieldType="暂无校验" MaxLength="254" Size="30"></cc2:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<div class="Navbutton">
	<cc1:Button ID="AddNewRec" runat="server" Text=" 提 交 "></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworedit');"><img src="../images/state1.gif"/> 取 消 </button>
</div>
</fieldset>
</div>
</div>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
</form>
<div id="setting" />
<%=footer%>
</body>
</html>