<%@ Page language="c#" Inherits="BBX.Web.Admin.addusergroup" Codebehind="addusergroup.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc3" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<%@ Register TagPrefix="uc1" TagName="UserGroupPowerSetting" Src="../UserControls/usergrouppowersetting.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>添加用户组</title>
<link href="../styles/tab.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>		
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />      
<link href="../styles/colorpicker.css" rel="stylesheet" type="text/css" />  
<link href="../styles/modelpopup.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
	function validate(theform)
	{
		if(document.getElementById("TabControl1_tabPage51_groupTitle").value == "")
		{
			resetPage();
			alert("用户组名称不能为空");
			document.getElementById("TabControl1_tabPage51_groupTitle").focus();
			return false;
		}
		var creditshigher = document.getElementById("TabControl1_tabPage51_creditshigher").value;
		var creditslower = document.getElementById("TabControl1_tabPage51_creditslower").value;
		if(!isNumber(creditshigher))
		{
			resetPage();
			alert("积分下限为非数字");
			document.getElementById("TabControl1_tabPage51_creditshigher").focus();
			document.getElementById("TabControl1_tabPage51_creditshigher").value = "";
			return false;
		}
		if(!isNumber(creditslower))
		{
			resetPage();
			alert("积分上限为非数字");
			document.getElementById("TabControl1_tabPage51_creditslower").focus();
			document.getElementById("TabControl1_tabPage51_creditslower").value = "";
			return false;
		}
		if (parseInt(creditshigher) >= parseInt(creditslower))
		{
			resetPage();
			alert("积分下限必须小于积分上限");
			document.getElementById("TabControl1_tabPage51_creditshigher").focus();
			return;
		}		        
		return validatebonusprice();
	}
	function resetPage()
	{
		document.getElementById('success').style.display = 'none'
		document.getElementById("AddUserGroupInf").disabled = false;
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" runat="server">
<uc1:PageInfo id="info1" runat="server" Icon="warning" Text="增加用户组时积分的上限和下限必须在已有的某个用户组积分上下限范围之内. <br />例如: 新手上路(已有)的积分上下限分别是50和0 ,那么要添加的用户组的积分上下限必须在(50≥上下限 >0) 或 (50>上下限≥0)之间. 而当要添加跨越多个用户组积分上下限区间的用户组时, 系统将视为无效."></uc1:PageInfo>		
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<cc3:TabControl id="TabControl1" SelectionMode="Client" runat="server" TabScriptPath="../js/tabstrip.js" width="660" height="100%">
<cc3:TabPage Caption="基本信息" ID="tabPage51">	
<table width="100%">
	<tr><td class="item_title" colspan="2">用户组名称</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="groupTitle" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验" width="180"  MaxLength="50"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">组名称颜色</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:ColorPicker id="color" runat="server" ReadOnly="True" LeftOffSet="-23" TopOffSet="-123"></cc1:ColorPicker>
		</td>
		<td class="vtop">用户组名称的显示颜色</td>
	</tr>
	<tr><td class="item_title" colspan="2">积分下限</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="creditshigher" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Text="0" size="10"  MaxLength="9"></cc1:TextBox>
		</td>
		<td class="vtop">所属该用户组用户的最低积分数</td>
	</tr>
	<tr><td class="item_title" colspan="2">积分上限</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="creditslower" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Text="0" size="10"  MaxLength="9"></cc1:TextBox>
		</td>
		<td class="vtop">所属该用户组用户的最高积分数</td>
	</tr>
	<tr><td class="item_title" colspan="2">星星数</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="stars" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Text="0" size="5"  MaxLength="4"></cc1:TextBox>
			<asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" ControlToValidate="stars" ErrorMessage="请输入正整数或者零" ValidationExpression="^[0-9]*$">
            </asp:RegularExpressionValidator>
		</td>
		<td class="vtop">该用户组显示的星星数</td>
	</tr>
	<tr><td class="item_title" colspan="2">阅读权限</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="readaccess" runat="server"  CanBeNull="必填" RequiredFieldType="数据校验" Text="0" size="5"  MaxLength="4"></cc1:TextBox> <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="readaccess" ErrorMessage="请输入整数" ValidationExpression="^[-]?\d+\d*$">
            </asp:RegularExpressionValidator>
		</td>
		<td class="vtop">设置用户浏览帖子或附件的权限级别,范围 0~255,0 为禁止用户浏览任何帖子或附件.当用户的阅读权限小于帖子或附件的阅读权限许可(默认时为 1)时,用户将不能阅读该帖子或下载该附件</td>
	</tr>
	<tr><td class="item_title" colspan="2">短消息最多条数</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxpmnum" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Text="0" size="5"  MaxLength="4"></cc1:TextBox>
			<asp:RegularExpressionValidator id="RegularExpressionValidator4" runat="server" ControlToValidate="maxpmnum" ErrorMessage="请输入正整数或者零" ValidationExpression="^[0-9]*$">
            </asp:RegularExpressionValidator>
		</td>
		<td class="vtop">设置用户短消息最大可保存的消息数目,0 为禁止使用短消息</td>
	</tr>
	<tr><td class="item_title" colspan="2">签名最多字节</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxsigsize" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Text="0" size="5"  MaxLength="4"></cc1:TextBox>
			<asp:RegularExpressionValidator id="RegularExpressionValidator5" runat="server" ControlToValidate="maxsigsize" ErrorMessage="请输入正整数或者零" ValidationExpression="^[0-9]*$">
            </asp:RegularExpressionValidator>
		</td>
		<td class="vtop">设置用户签名最大字节数,0 为不允许用户使用签名</td>
	</tr>
	<tr><td class="item_title" colspan="2">主题(附件)最高售价</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxprice" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Text="0" size="5" MaxLength="4"></cc1:TextBox>
			<asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" ControlToValidate="maxprice" ErrorMessage="请输入正整数或者零" ValidationExpression="^[0-9]*$">
            </asp:RegularExpressionValidator>
		</td>
		<td class="vtop">主题(附件)出售使得作者可以将自己发表的主题(附件)隐藏起来,只有当浏览者向作者支付相应的交易积分后才能查看主题(附件)内容.此处设置用户出售主题(附件)时允许设置的最高价格,0 为不允许用户出售.</td>
	</tr>
	<tr><td class="item_title" colspan="2">论坛每天允许上传附件总字节数</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxsizeperday" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Text="0" Size="10"  MaxLength="9"></cc1:TextBox>(单位:字节)
			<select onchange="document.getElementById('TabControl1_tabPage51_maxsizeperday').value=this.value">
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
				<option value="6144000">6M</option>
				<option value="8192000">8M</option>
				<option value="10240000">10M</option>
				<option value="12288000">12M</option>
				<option value="14336000">14M</option>
				<option value="16384000">16M</option>
				<option value="18432000">18M</option>
				<option value="20480000">20M</option>
				<option value="22528000">22M</option>
				<option value="24576000">24M</option>
				<option value="26624000">26M</option>
				<option value="28672000">28M</option>
				<option value="30720000">30M</option>
			</select>
		</td>
		<td class="vtop">设置用户每 24 小时可以上传的附件总字节数.注意: 本功能会加重服务器负担,建议仅在必要时使用.</td>
	</tr>
	<tr><td class="item_title" colspan="2">上传单个附件允许的最大字节数</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxattachsize" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Text="0" Size="10"  MaxLength="9"></cc1:TextBox>(单位:字节)
			<select onchange="document.getElementById('TabControl1_tabPage51_maxattachsize').value=this.value">
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
		<td class="vtop">设置上传单个附件允许最大字节数</td>
	</tr>
	<tr><td class="item_title" colspan="2">允许附件类型</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:CheckBoxList id="attachextensions" runat="server" HintHeight="80" RepeatColumns="3"></cc1:CheckBoxList>
		</td>
		<td class="vtop">如果要允许所有附件类型, 则不要点选右侧任何附件类型, 且具体版块设置优先于用户组设置</td>
	</tr>
</table>
</cc3:TabPage>
<cc3:TabPage Caption="权限信息" ID="tabPage22">
	<uc1:UserGroupPowerSetting ID="usergrouppowersetting" runat="server" />
</cc3:TabPage>
</cc3:TabControl>
<div class="Navbutton">
	<cc1:Button id="AddUserGroupInf" runat="server" ValidateForm="true" Text=" 提 交 "></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button3" onclick="window.history.back();"><img src="../images/arrow_undo.gif"/> 返 回 </button>
</div>
<div id="topictypes" style="display:none;width:100%;"> 
<tr><td class="item_title" colspan="2">用户组头像</td></tr>
<tr>
	<td class="vtop rowform">
			<cc1:TextBox id="groupavatar" runat="server" RequiredFieldType="暂无校验" width="80%"></cc1:TextBox>
	</td>
	<td class="vtop"></td>
</tr>
</div>
</form>
</div>
<%=footer%>
</body>
</html>