<%@ Page Language="C#" CodeBehind="forum_option.aspx.cs" Inherits="BBX.Web.Admin.option" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<title>站点功能</title>
	<script type="text/javascript" src="../js/common.js"></script>		
	<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
	<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
	<script type="text/javascript" src="../js/modalpopup.js"></script>
	<meta http-equiv="X-UA-Compatible" content="IE=7" />
	<script type="text/javascript">
		function setStatus(status) {
		document.getElementById("msgforwardlistinfo[0]").style.display = (status) ? "block" : "none";
		//document.getElementById("msgforwardlist").style.display = (status) ? "block" : "none";
		document.getElementById("msgforwardlistinfo[1]").style.display = (status) ? "block" : "none";
		document.getElementById("msgforwardlistinfo[2]").style.display = (status) ? "block" : "none";
	}
		function ratevalveimg(ratelevel)
		{
			var imgelement =  '<img src="../images/agree.gif" border="0" alt="" style="vertical-align:middle;"/>';
			var valveimg = '';
			if(isie())
			{
				imgelement = '<img src="../images/agree.gif" border="0" alt="" />';
			}
			for(i = 0; i < ratelevel; i++) {
				valveimg += imgelement;  
			}            	
			return valveimg;
    }
	</script>
</head>
<body>
<form id="Form1" runat="server">
<div class="ManagerForm">
<fieldset>
	<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">站点功能</legend>
    <table width="100%">
	<tr><td class="item_title" colspan="2">强制跳转到www</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="forcewww" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">选择"是"将把非www访问站点的链接强制转向到www.</td>
	</tr>
	<tr><td class="item_title" colspan="2">使用论坛流量统计</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="statstatus" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="true">是</asp:ListItem>
				<asp:ListItem Value="false">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">选择"是"将打开论坛统计功能,提供详细的论坛访问统计信息.</td>
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
	<tr><td class="item_title" colspan="2">管理记录保留时间</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="maxmodworksmonths" runat="server" CanBeNull="必填" RequiredFieldType="数据校验"  Size="6" MaxLength="4"></cc1:TextBox>(单位:月)
		</td>
		<td class="vtop">系统中保留管理记录的时间,默认为 3 个月,建议在 3～6 个月的范围内取值</td>
	</tr>
	<tr><td class="item_title" colspan="2">管理操作理由是否通知作者</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="reasonpm" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">是否将管理操作的理由短消息通知作者</td>
	</tr>
	<tr><td class="item_title" colspan="2">每页主题数</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="tpp" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Size="6" MaxLength="4"></cc1:TextBox>
		</td>
		<td class="vtop">版块每页显示的主题数</td>
	</tr>
	<tr><td class="item_title" colspan="2">热门话题最低帖数</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="hottopic" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Size="6" MaxLength="4"></cc1:TextBox>
		</td>
		<td class="vtop">超过一定帖子数的话题将显示为热门话题</td>
	</tr>
	<tr><td class="item_title" colspan="2">每页帖子数</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="ppp" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Size="6" MaxLength="4"></cc1:TextBox>
		</td>
		<td class="vtop">看主题时每页帖子数</td>
	</tr>
	<tr><td class="item_title" colspan="2">是否允许使用标签(Tag)功能</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="enabletag" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">选择允许使用标签功能</td>
	</tr>
	<tr><td class="item_title" colspan="2">首页显示热门标签(Tag)数量设置</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="hottagcount" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" MinimumValue="0" MaximumValue="60" Size="6" MaxLength="2"></cc1:TextBox>
		</td>
		<td class="vtop">取值范围为0~60.如果取值为0,则关闭首页热门标签(Tag)的显示</td>
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
	<tr><td class="item_title" colspan="2">启用论坛管理工作日志</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:RadioButtonList id="modworkstatus" runat="server"  RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">论坛管理工作统计可以使管理员了解版主等管理人员的工作状况. 注意: 本功能会轻微加重系统负担</td>
	</tr>
	<tr><td class="item_title" colspan="2">删帖不减积分时间期限</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="losslessdel" runat="server" Text="5" RequiredFieldType="数据校验"  CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox>(单位:天)
		</td>
		<td class="vtop">设置版主或管理员从前台删除发表于多少天以前的帖子时, 不更新用户积分, 可用于清理老帖子而不对作者积分造成损失. 0 为不使用此功能, 始终更新用户积分</td>
	</tr>
	<tr><td class="item_title" colspan="2">编辑帖子附加编辑记录</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="editedby" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">在 60 秒后编辑帖子添加“本帖由 xxx 于 xxxx-xx-xx 编辑”字样. 管理员编辑不受此限制</td>
	</tr>
	<tr><td class="item_title" colspan="2">星星升级阀值</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="starthreshold" runat="server" CanBeNull="必填" RequiredFieldType="数据校验"  Size="6" MaxLength="4"></cc1:TextBox>
		</td>
		<td class="vtop">N 个星星显示为 1 个月亮、N 个月亮显示为 1 个太阳. 默认值为 2, 如设为 0 则取消此项功能, 始终以星星显示</td>
	</tr>
	<tr><td class="item_title" colspan="2">是否允许切换编辑器模式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="allowswitcheditor" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">选择否将禁止用户在论坛代码模式和所见即所得模式之间切换.</td>
	</tr>
	<tbody style="display:none;">
	<tr><td class="item_title" colspan="2">评分等级</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="ratevalveset1" runat="server" CanBeNull="必填" Size="3" MaxLength="3"></cc1:TextBox> <script type="text/javascript">document.write(ratevalveimg(1));</script><br />
		                        <cc1:TextBox id="ratevalveset2" runat="server" CanBeNull="必填" Size="3" MaxLength="3"></cc1:TextBox> <script type="text/javascript">document.write(ratevalveimg(2));</script><br />
		                        <cc1:TextBox id="ratevalveset3" runat="server" CanBeNull="必填" Size="3" MaxLength="3"></cc1:TextBox> <script type="text/javascript">document.write(ratevalveimg(3));</script><br />
		                        <cc1:TextBox id="ratevalveset4" runat="server" CanBeNull="必填" Size="3" MaxLength="3"></cc1:TextBox> <script type="text/javascript">document.write(ratevalveimg(4));</script><br />
		                        <cc1:TextBox id="ratevalveset5" runat="server" CanBeNull="必填" Size="3" MaxLength="3"></cc1:TextBox> <script type="text/javascript">document.write(ratevalveimg(5));</script>
		</td>
		<td class="vtop"></td>
	</tr>
	</tbody>
	<tr><td class="item_title" colspan="2">是否默认回帖选中"发送回复通知"</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="replynotificationstatus" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">选择是否在回复选项中默认选择</td>
	</tr>
	<tr><td class="item_title" colspan="2">是否默认回帖选中"发送邮件给楼主"</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="replyemailstatus" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">选择是否在回复选项中默认选择</td>
	</tr>
	<%--<tr><td class="item_title" colspan="2">是否启用首页快速发主题</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="allowforumindexposts" runat="server" RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">选择是否在首页快速发主题</td>
	</tr>--%>
	<tr><td class="item_title" colspan="2">查看新帖时间(分钟)</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="viewnewtopicminute" runat="server" Size="5" MaxLength="5" MinimumValue="5" MaximumValue="14400"></cc1:TextBox>		</td>
		<td class="vtop">设置多长时间内发布的帖子算是新帖</td>
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
<tr><td class="item_title" colspan="2">启用 RSS</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="rssstatus" runat="server"  RepeatLayout="flow">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">选择"是", 论坛将允许用户使用 RSS 客户端软件接收最新的论坛帖子更新. 注意: 在分论坛很多的情况下, 本功能可能会加重服务器负担</td>
	</tr>
    <tr><td class="item_title" colspan="2">RSS TTL(单位:分钟)</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="rssttl" runat="server"  CanBeNull="必填" MinimumValue="0" Size="5" MaxLength="4"></cc1:TextBox>		</td>
		<td class="vtop">TTL(Time to Live) 是 RSS 2.0 的一项属性, 用于控制订阅内容的自动刷新时间, 时间越短则资料实时性就越高, 但会加重服务器负担, 通常可设置为 30～180 范围内的数值</td>
	</tr>
    	<tr><td class="item_title" colspan="2">是否记录缓存日志</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="cachelog" RepeatLayout="flow" runat="server"  >
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>		</td>
		<td class="vtop">此项功能会记录论坛的缓存日志并在缓存日志中进行显示. 注意: 当此项功能会加重系统负担</td>
	</tr>
    </table>
	<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>	
	<div class="Navbutton">
		<cc1:Button id="SaveInfo" runat="server" Text=" 提 交 "></cc1:Button>
	</div>
</fieldset>
</div>   
</form>
<script type="text/javascript">
	setStatus(document.getElementById("quickforward_0").checked);
</script>
<%=footer%>
</body>
</html>