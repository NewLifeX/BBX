<%@ Page Language="c#" Inherits="BBX.Web.Admin.edituser" Codebehind="edituser.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc3" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<%@ Import Namespace="BBX.Common" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>编辑用户组</title>
<link href="../styles/tab.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
    function checkSetting()
    {
        if (document.getElementById('TabControl1_tabPage22_newsletter_0').checked)
        {
            document.getElementById('TabControl1_tabPage22_newsletter_1').disabled = false;
        }
        else
        {			
            document.getElementById('TabControl1_tabPage22_newsletter_1').checked = false;
            document.getElementById('TabControl1_tabPage22_newsletter_1').disabled = true;
        }
    }
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<cc3:TabControl ID="TabControl1" SelectionMode="Client" runat="server" TabScriptPath="../js/tabstrip.js" Width="760" Height="100%">
<cc3:TabPage Caption="常规基本信息" ID="tabPage11">
<table width="100%">
    <tr><td class="item_title" colspan="2">用户名(UID:<%=BBX.Common.DNTRequest.GetString("uid")%>)</td></tr>
    <tr>
        <td class="vtop rowform">
            <cc1:TextBox ID="userName" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验" Enabled="false" IsReplaceInvertedComma="false" MaxLength="20" Size="10"></cc1:TextBox>
            <asp:CheckBox ID="IsEditUserName" runat="server" Text="允许修改用户名"></asp:CheckBox>
        </td>
        <td class="vtop"></td>
    </tr>
    <tr><td class="item_title" colspan="2">头像</td></tr>
    <tr>
        <td class="vtop rowform">
             <img src="../../tools/avatar.aspx?uid=<%=userInfo.ID%>&size=small" onerror="this.onerror=null;this.src='../../images/common/noavatar_small.gif';" />
             <asp:CheckBox ID="delavart" runat="server" Text="删除头像"></asp:CheckBox>
        </td>
        <td class="vtop"></td>
    </tr>
    <tr><td class="item_title">昵称</td><td class="item_title">所属用户组</td></tr>
    <tr>
        <td class="vtop rowform">
              <cc1:TextBox ID="nickname" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false" MaxLength="20" Size="10"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
              <cc1:DropDownList ID="groupid" runat="server"></cc1:DropDownList>
              <cc1:Button ID="ReSendEmail" runat="server" Text="重发验证email" ButtontypeMode="Normal"></cc1:Button>
        </td>
    </tr>
    <tr><td class="item_title">注册IP</td><td class="item_title">清除用户安全提问</td></tr>
    <tr>
        <td class="vtop rowform">
              <cc1:TextBox ID="regip" runat="server" RequiredFieldType="IP地址" MaxLength="15" Size="11"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
            <cc1:RadioButtonList ID="secques" runat="server" RepeatColumns="2">
                <asp:ListItem Value="1">是</asp:ListItem>
                <asp:ListItem Value="0" Selected="true">否</asp:ListItem>
            </cc1:RadioButtonList>
        </td>
    </tr>
    <tr><td class="item_title">发帖数</td><td class="item_title">精华帖数</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:TextBox ID="posts" runat="server" Width="56px" RequiredFieldType="数据校验" CanBeNull="必填" MaxLength="9"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
             <cc1:TextBox ID="digestposts" runat="server" Width="56px" RequiredFieldType="数据校验" CanBeNull="必填" MaxLength="4"></cc1:TextBox>
        </td>
    </tr>
    </table>
    <uc1:PageInfo ID="info1" runat="server" Icon="information" Text="以下为对该用户的管理操作, 操作后无法撤消, 请谨重! " />
    <table width="100%">
    <tr><td class="item_title" colspan="2"></td></tr>
    <tr>
        <td class="vtop" colspan="2">
             <cc1:Button ID="ResetPassWord" runat="server" Text="重设密码 "></cc1:Button>&nbsp;&nbsp;
             <cc1:Button ID="ResetUserPost" runat="server" Text="重建用户发帖数"></cc1:Button>&nbsp;&nbsp;
             <cc1:Button ID="StopTalk" runat="server" Text="禁言该用户" HintInfo="该操作将会把当前用户放入系统 \'禁止发言\' 组中"></cc1:Button>&nbsp;&nbsp;
             <cc1:Button ID="ResetUserDigestPost" runat="server" Text="重建用户精华帖数"></cc1:Button>
        </td>
    </tr>
    <tr><td class="item_title" colspan="2"></td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:Button ID="DelPosts" runat="server" Text="删除该用户帖子" ButtonImgUrl="../images/del.gif" OnClientClick="if(!confirm('你确认要删除该用户所发表帖子吗？\n删除后将不能恢复！')) return false;"></cc1:Button>
        </td>
        <td class="vtop">该操作将会删除该用户发表过的所有帖子</td>
    </tr>
    <tr><td class="item_title" colspan="2"></td></tr>
    <tr>
        <td class="vtop" colspan="2">
            <cc1:Button ID="DelUserInfo" runat="server" Text="删除该用户" ButtonImgUrl="../images/del.gif" OnClientClick="if(!confirm('你确认要删除该用户吗？\n删除后将不能恢复！')) return false;"></cc1:Button><br/><br/>
            <cc1:CheckBoxList ID="deltype" runat="server" RepeatColumns="1" RepeatLayout="flow">
                <asp:ListItem Value="1">保留用户所发帖子</asp:ListItem>
                <asp:ListItem Value="2">保留用户已发送的短消息</asp:ListItem>
            </cc1:CheckBoxList>
        </td>
    </tr>
</table>
</cc3:TabPage>
<cc3:TabPage Caption="用户信息" ID="tabPage22">
<table width="100%">
    <tr><td class="item_title">真实姓名</td><td class="item_title">身份证号</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:TextBox ID="realname" runat="server" RequiredFieldType="暂无校验" MaxLength="10" Size="10"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
              <cc1:TextBox ID="idcard" runat="server" RequiredFieldType="暂无校验" MaxLength="20" Size="20"></cc1:TextBox>
        </td>
    </tr>
    <tr><td class="item_title">移动电话号码</td><td class="item_title">固定电话号码</td></tr>
    <tr>
        <td class="vtop rowform">
              <cc1:TextBox ID="mobile" runat="server" MaxLength="20" Size="20"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
              <cc1:TextBox ID="phone" runat="server" CanBeNull="可为空" MaxLength="20" Size="20"></cc1:TextBox>
        </td>
    </tr>
    <tr><td class="item_title">性别</td><td class="item_title">注册时间</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:RadioButtonList ID="gender" runat="server" RepeatColumns="3">
                <asp:ListItem Value="1">男</asp:ListItem>
                <asp:ListItem Value="2">女</asp:ListItem>
                <asp:ListItem Value="0">保密</asp:ListItem>
            </cc1:RadioButtonList>
        </td>
        <td class="vtop rowform">
               <cc1:TextBox ID="joindate" runat="server" Size="15" RequiredFieldType="日期时间" CanBeNull="必填"></cc1:TextBox>
        </td>
    </tr>
    <tr><td class="item_title">上次登录IP</td><td class="item_title">上次访问时间</td></tr>
    <tr>
        <td class="vtop rowform">
              <cc1:TextBox ID="lastip" runat="server" RequiredFieldType="IP地址" MaxLength="15" Size="10"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
              <cc1:TextBox ID="lastvisit" runat="server" Size="15" RequiredFieldType="日期时间" CanBeNull="必填"></cc1:TextBox>
        </td>
    </tr>
    <tr><td class="item_title">最后活动时间</td><td class="item_title">最后发帖时间</td></tr>
    <tr>
        <td class="vtop rowform">
              <cc1:TextBox ID="lastactivity" runat="server" Size="15" RequiredFieldType="日期时间" CanBeNull="必填"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
            <cc1:TextBox ID="lastpost" runat="server" Size="15" RequiredFieldType="日期时间" CanBeNull="必填"></cc1:TextBox>
        </td>
    </tr>
    <tr><td class="item_title">在线时间</td><td class="item_title">邮件地址</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:TextBox ID="oltime" runat="server" Width="96px" RequiredFieldType="数据校验" MaxLength="8" CanBeNull="必填"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
             <cc1:TextBox ID="email" runat="server" CanBeNull="可为空" RequiredFieldType="电子邮箱" MaxLength="50" Size="20"></cc1:TextBox>
        </td>
    </tr>
    <tr><td class="item_title">生日</td><td class="item_title">签名</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:TextBox ID="bday" runat="server" RequiredFieldType="暂无校验" MaxLength="10" Size="12"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
            <cc1:RadioButtonList ID="sigstatus" runat="server" RepeatColumns="2">
                <asp:ListItem Value="1" Selected>显示</asp:ListItem>
                <asp:ListItem Value="0">不显示</asp:ListItem>
            </cc1:RadioButtonList>
        </td>
    </tr>
    <tr><td class="item_title" colspan="2">每页主题数</td></tr>
    <tr>
        <td class="vtop rowform">
            <cc1:TextBox ID="tpp" runat="server" RequiredFieldType="数据校验" MaxLength="4" CanBeNull="必填"></cc1:TextBox>
        </td>
        <td class="vtop">论坛每页显示的主题数,0为论坛默认设置</td>
    </tr>
    <tr><td class="item_title" colspan="2">每页帖数</td></tr>
    <tr>
        <td class="vtop rowform">
            <cc1:TextBox ID="ppp" runat="server" RequiredFieldType="数据校验" MaxLength="4" CanBeNull="必填"></cc1:TextBox>
        </td>
        <td class="vtop">查看主题时每页显示的帖子数,0为论坛默认设置</td>
    </tr>
    <tr><td class="item_title" colspan="2">风格</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:DropDownList ID="templateid" runat="server"></cc1:DropDownList>
        </td>
        <td class="vtop">该用户查看论坛时使用的模板</td>
    </tr>
    <tr><td class="item_title">是否显示邮箱</td><td class="item_title">是否隐身</td></tr>
    <tr>
        <td class="vtop rowform">
            <cc1:RadioButtonList ID="showemail" runat="server">
                <asp:ListItem Value="1">是</asp:ListItem>
                <asp:ListItem Value="0">否</asp:ListItem>
            </cc1:RadioButtonList>
        </td>
        <td class="vtop rowform">
            <cc1:RadioButtonList ID="invisible" runat="server">
                <asp:ListItem Value="1">是</asp:ListItem>
                <asp:ListItem Value="0">否</asp:ListItem>
            </cc1:RadioButtonList>
        </td>
    </tr>
    <tr><td class="item_title" colspan="2">消息接收设置</td></tr>
    <tr>
        <td class="vtop rowform">
            <cc1:CheckBoxList ID="newsletter" runat="server" RepeatColumns="1">
                <asp:ListItem Value="2" onclick="checkSetting();">接收用户短消息</asp:ListItem>
                <asp:ListItem Value="4" onclick="checkSetting();">显示短消息提示框</asp:ListItem>
            </cc1:CheckBoxList>
        </td>
        <td class="vtop"></td>
    </tr>
    <tr><td class="item_title">网站</td><td class="item_title">QQ号码</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:TextBox ID="website" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false" MaxLength="80" Size="25"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
            <cc1:TextBox ID="qq" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false" MaxLength="12" Size="13"></cc1:TextBox>
        </td>
    </tr>
    <tr><td class="item_title">ICQ号码</td><td class="item_title">Skype帐号</td></tr>
    <tr>
        <td class="vtop rowform">
            <cc1:TextBox ID="icq" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false" MaxLength="12" Size="13"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
             <cc1:TextBox ID="skype" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false" MaxLength="40" Size="20"></cc1:TextBox>
        </td>
    </tr>
    <tr><td class="item_title">MSN Messenger帐号</td><td class="item_title">Yahoo Messenger帐号</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:TextBox ID="msn" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false" MaxLength="40" Size="20"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
            <cc1:TextBox ID="yahoo" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false" MaxLength="40" Size="20"></cc1:TextBox>
        </td>
    </tr>
    <tr><td class="item_title">自定义头衔</td><td class="item_title">来自</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:TextBox ID="customstatus" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false" MaxLength="50" Size="20"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
             <cc1:TextBox ID="location" runat="server" RequiredFieldType="暂无校验" IsReplaceInvertedComma="false" MaxLength="50" Size="20"></cc1:TextBox>
        </td>
    </tr>
    <tr><td class="item_title" colspan="2">自我介绍</td></tr>
    <tr>
        <td class="vtop" colspan="2">
             <cc1:TextBox ID="bio" runat="server" Width="496px" RequiredFieldType="暂无校验" TextMode="MultiLine" Height="92px" IsReplaceInvertedComma="false"></cc1:TextBox>
        </td>
    </tr>
    <tr><td class="item_title" colspan="2">签名</td></tr>
    <tr>
        <td class="vtop" colspan="2">
             <cc1:TextBox ID="signature" runat="server" RequiredFieldType="暂无校验" Width="496px" TextMode="MultiLine" Height="50px" IsReplaceInvertedComma="false"></cc1:TextBox>
        </td>
    </tr>
</table>
</cc3:TabPage>
<cc3:TabPage Caption="积分设置" ID="tabPage33">
<table width="100%">
    <tr><td class="item_title" colspan="2">积分数</td></tr>
    <tr>
        <td class="vtop" colspan="2">
             <asp:Label ID="credits" runat="server" Width="64px" ForeColor="Black"></asp:Label>
             <cc1:Button ID="CalculatorScore" runat="server" Visible="false" Text="以系统标准计算用户积分" ButtontypeMode="Normal"></cc1:Button>
             <span>积分公式（提交后自动计算，上限为999999999）：<asp:Label ID="lblScoreCalFormula" runat="server" Text=""></asp:Label></span>
        </td>
    </tr>
    <tr>
        <td class="item_title"><asp:Literal ID="extcredits1name" runat="server" Text="extcredits1"></asp:Literal></td>
        <td class="item_title"><asp:Literal ID="extcredits2name" runat="server" Text="extcredits2"></asp:Literal></td>
    </tr>
    <tr>
        <td class="vtop rowform">
             <cc1:TextBox ID="extcredits1" runat="server" RequiredFieldType="数据校验" Width="64px" MaxLength="7"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
             <cc1:TextBox ID="extcredits2" runat="server" RequiredFieldType="数据校验" Width="64px" MaxLength="7"></cc1:TextBox>
        </td>
    </tr>
    <tr>
        <td class="item_title"><asp:Literal ID="extcredits3name" runat="server" Text="extcredits3"></asp:Literal></td>
        <td class="item_title"><asp:Literal ID="extcredits4name" runat="server" Text="extcredits4"></asp:Literal></td>
    </tr>
    <tr>
        <td class="vtop rowform">
            <cc1:TextBox ID="extcredits3" runat="server" Width="64px" RequiredFieldType="数据校验" MaxLength="7"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
            <cc1:TextBox ID="extcredits4" runat="server" Width="64px" RequiredFieldType="数据校验" MaxLength="7"></cc1:TextBox>
        </td>
    </tr>
    <tr>
        <td class="item_title"><asp:Literal ID="extcredits5name" runat="server" Text="extcredits5"></asp:Literal></td>
        <td class="item_title"><asp:Literal ID="extcredits6name" runat="server" Text="extcredits6"></asp:Literal></td>
    </tr>
    <tr>
        <td class="vtop rowform">
            <cc1:TextBox ID="extcredits5" runat="server" Width="64px" RequiredFieldType="数据校验" MaxLength="7"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
            <cc1:TextBox ID="extcredits6" runat="server" Width="64px" RequiredFieldType="数据校验" MaxLength="7"></cc1:TextBox>
        </td>
    </tr>
    <tr>
        <td class="item_title"><asp:Literal ID="extcredits7name" runat="server" Text="extcredits7"></asp:Literal></td>
        <td class="item_title"><asp:Literal ID="extcredits8name" runat="server" Text="extcredits8"></asp:Literal></td>
    </tr>
    <tr>
        <td class="vtop rowform">
            <cc1:TextBox ID="extcredits7" runat="server" Width="64px" RequiredFieldType="数据校验" MaxLength="7"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
            <cc1:TextBox ID="extcredits8" runat="server" Width="64px" RequiredFieldType="数据校验" MaxLength="7"></cc1:TextBox>
        </td>
    </tr>
</table>
</cc3:TabPage>
<cc3:TabPage Caption=" 勋 章 " ID="tabPage44">
<table class="ntcplist">
<tr>
<td>
<table class="datalist" id="DataGrid1" style="border-collapse: collapse;" align="center" border="1" cellspacing="0" rules="all">
    <tr class="category">
        <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap" width="40%">勋章图片</td>
        <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap" width="40%">名称</td>
        <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap" width="20%"><input title="选中/取消选中 本页所有Case" onclick="CheckAll(this.form)" type="checkbox" name="chkall" id="chkall" />授予该勋章</td>
    </tr>
    <asp:Repeater ID="medalslist" runat="server">
        <ItemTemplate>
            <tr class="mouseoutstyle" onmouseover="this.className='mouseoverstyle'" onmouseout="this.className='mouseoutstyle'">
                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap"><img src="../../images/medals/<%# Eval("image").ToString()%>" height="25px" /></td>
                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap"><%# Eval("name").ToString()%></td>
                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                    <%# BeGivenMedal(Eval("isgiven").ToString(), Eval("id").ToString())%>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="mouseoutstyle" onmouseover="this.className='mouseoverstyle'" onmouseout="this.className='mouseoutstyle'">
                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap"><img src="../../images/medals/<%# Eval("image").ToString()%>" height="25px"></td>
                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap"><%# Eval("name").ToString()%></td>
                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                    <%# BeGivenMedal(Eval("isgiven").ToString(), Eval("id").ToString())%>
                </td>
            </tr>
        </AlternatingItemTemplate>
    </asp:Repeater>
</table>
</td>
</tr>
</table>
<table width="100%">
<tr>
<td class="panelbox">
<table class="table1" cellspacing="0" cellpadding="4" width="100%" align="center" border="0">
    <tr>
        <td style="width: 130px">授予/收回勋章的理由:</td>
        <td>
            <cc1:TextBox ID="reason" runat="server" HintTitle="提示" HintInfo="如果您修改了用户的勋章资料, 请输入操作理由, 系统将把理由记录在勋章授予记录中, 以供日后查看"
                TextMode="MultiLine" RequiredFieldType="暂无校验" Width="60%" Height="112px"></cc1:TextBox>
            <cc1:Button ID="GivenMedal" runat="server" Text="仅修改勋章设置"></cc1:Button>
        </td>
    </tr>
</table>
</td>
</tr>
</table>
</cc3:TabPage>
</cc3:TabControl>
</td>
</tr>
</table>
<div class="Navbutton">
    <cc1:Button ID="SaveUserInfo" runat="server" Text=" 提 交 "></cc1:Button>&nbsp;&nbsp;
    <button type="button" class="ManagerButton" id="Button3" onclick="window.history.back();"><img src="../images/arrow_undo.gif"/> 返 回 </button>
</div>
<div id="topictypes" style="display: none; width: 100%;">
<tr>
    <td>页面浏览量</td>
    <td><cc1:TextBox ID="pageviews" runat="server" Width="96px" RequiredFieldType="暂无校验"></cc1:TextBox></td>
</tr>
<tr>
    <td class="td2">日期格式</td>
    <td class="td2"><cc1:TextBox ID="smalldatetimeformat" runat="server" Width="208px" RequiredFieldType="暂无校验"></cc1:TextBox></td>
</tr>
<tr>
    <td>时间格式</td>
    <td><cc1:TextBox ID="timeformat" runat="server" Width="208px" RequiredFieldType="暂无校验"></cc1:TextBox></td>
</tr>
<tr>
    <td class="td2">短消息铃声</td>
    <td class="td2"><cc1:TextBox ID="pmsound" runat="server" Width="208px" RequiredFieldType="暂无校验"></cc1:TextBox></td>
</tr>
<tr>
    <td>是否使用特殊权限</td>
    <td>
        <cc1:RadioButtonList ID="accessmasks" runat="server">
            <asp:ListItem Value="1">是</asp:ListItem>
            <asp:ListItem Value="0">否</asp:ListItem>
        </cc1:RadioButtonList>
    </td>
</tr>
<tr>
    <td>组过期时间</td>
    <td><cc1:TextBox ID="groupexpiry" runat="server" Width="192px" RequiredFieldType="暂无校验"></cc1:TextBox></td>
</tr>
<tr>
    <td class="td2">扩展用户组</td>
    <td class="td2">
        <cc1:CheckBoxList ID="extgroupids" runat="server" RepeatColumns="4">
        </cc1:CheckBoxList>
    </td>
</tr>
<tr>
    <td class="td2">是否有新消息</td>
    <td class="td2">
        <cc1:RadioButtonList ID="newpm" runat="server">
            <asp:ListItem Value="1">是</asp:ListItem>
            <asp:ListItem Value="0">否</asp:ListItem>
        </cc1:RadioButtonList></td>
</tr>
</div>
<div style="display: none">
<tr>
    <td>当前用户:<asp:Literal ID="givenusername" runat="server"></asp:Literal></td>
    <td></td>
    <td></td>
</tr>
</div>
<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
</div>
</form>
<%=footer%>
</body>
</html>