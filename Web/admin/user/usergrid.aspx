<%@ Page Language="c#" Inherits="BBX.Web.Admin.usergrid" Codebehind="usergrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>用户列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/calendar.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
    function Check(form,checked)
    {
        CheckByName(form,'uid',checked);
        checkedEnabledButton(form,'uid','StopTalk','DeleteUser')
    }
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background: url(../images/icons/icon32.jpg) no-repeat 6px 50%;">搜索用户</legend>
<asp:Panel ID="searchtable" runat="server" Visible="true">
<table width="100%">
    <tr><td class="item_title">用户名</td><td class="item_title">昵称</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:TextBox ID="Username" runat="server" RequiredFieldType="暂无校验" Width="100"></cc1:TextBox>&nbsp;模糊查找<input id="islike" type="checkbox" value="1" name="cins" runat="server" />
        </td>
        <td class="vtop rowform">
            <cc1:TextBox ID="nickname" runat="server" RequiredFieldType="暂无校验" Width="150"></cc1:TextBox>
        </td>
    </tr>
    <tr><td class="item_title">用户组</td><td class="item_title">用户ID (UID)</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:DropDownList ID="UserGroup" runat="server"></cc1:DropDownList>
        </td>
        <td class="vtop rowform">
            <cc1:TextBox ID="uid" runat="server" RequiredFieldType="暂无校验" Width="100"></cc1:TextBox>&nbsp;
            <asp:RegularExpressionValidator ID="homephone" runat="SERVER" ControlToValidate="uid" ErrorMessage="输入错误" ValidationExpression="^([1-9]*,)*[0-9]*$">
            </asp:RegularExpressionValidator>格式:1,2,3
        </td>
    </tr>

    <tr><td class="item_title">用户积分</td><td class="item_title">注册日期</td></tr>
    <tr>
        <td class="vtop rowform">
             大于或等于:<cc1:TextBox ID="credits_start" runat="server" RequiredFieldType="数据校验" Size="8" MaxLength="9"></cc1:TextBox>&nbsp;&nbsp;
             小于或等于:<cc1:TextBox ID="credits_end" runat="server" RequiredFieldType="数据校验" Size="8" MaxLength="9"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
            从&nbsp;<cc1:Calendar ID="joindateStart" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js"> </cc1:Calendar>
            到&nbsp;<cc1:Calendar ID="joindateEnd" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js"></cc1:Calendar>&nbsp;
            使用日期查找<input id="ispostdatetime" type="checkbox" value="1" name="cins" runat="server" />
        </td>
    </tr>
    <tr><td class="item_title">最后登录IP</td><td class="item_title">用户发帖数</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:TextBox ID="lastip" runat="server" RequiredFieldType="IP地址" Width="150"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
              大于或等于:<cc1:TextBox ID="posts" runat="server" RequiredFieldType="数据校验" Width="80"></cc1:TextBox>
        </td>
    </tr>
    <tr><td class="item_title">用户精华帖数</td><td class="item_title">Email 包含</td></tr>
    <tr>
        <td class="vtop rowform">
              大于或等于:<cc1:TextBox ID="digestposts" runat="server" RequiredFieldType="数据校验" Width="80"></cc1:TextBox>
        </td>
        <td class="vtop rowform">
              <cc1:TextBox ID="email" runat="server" RequiredFieldType="暂无校验" Width="150"></cc1:TextBox>
        </td>
    </tr>
</table>
<div class="Navbutton"> <cc1:Button ID="Search" runat="server" Text="开始搜索"></cc1:Button></div>
</asp:Panel>
<cc1:Button ID="ResetSearchTable" runat="server" Text="重设搜索条件" Visible="False"></cc1:Button>
</fieldset>
</div>
<table width="100%">
<tr>
<td>
<cc1:DataGrid ID="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" align="center">
<Columns>
    <asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form,this.checked)' type='checkbox' name='chkall' id='chkall' />">
        <HeaderStyle Width="20px" />
        <ItemTemplate>
            <%# Eval("ID").ToString() != "1" ? "<input id=\"uid\" onclick=\"checkedEnabledButton(this.form,'uid','StopTalk','DeleteUser')\" type=\"checkbox\" value=\"" + Eval("ID").ToString() + "\"	name=\"uid\">" : ""%>
        </ItemTemplate>
    </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
            <a href="#" onclick="javascript:window.location.href='edituser.aspx?uid=<%# Eval("ID").ToString()%>&condition=<%=ViewState["condition"]==null?"":BBX.Common.Utils.HtmlEncode(ViewState["condition"].ToString().Replace("'","~^").Replace("%","~$"))%>';">编辑</a>
        </ItemTemplate>
    </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
            <a href="#" onclick="javascript:window.location.href='global_givemedals.aspx?uid=<%# Eval("ID").ToString()%>&condition=<%=ViewState["condition"]==null?"":BBX.Common.Utils.HtmlEncode(ViewState["condition"].ToString().Replace("'","~^").Replace("%","~$"))%>';">勋章</a>
        </ItemTemplate>
    </asp:TemplateColumn>
    <asp:BoundColumn DataField="ID" HeaderText="用户ID" Visible="false"></asp:BoundColumn>
    <asp:TemplateColumn HeaderText="用户名">
        <ItemTemplate>
            <a href="../../userinfo.aspx?userid=<%# Eval("ID")%>" target="_blank"><%# Eval("Name")%></a>
        </ItemTemplate>
    </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="头像">
        <ItemTemplate>
            <img src="<%# GetAvatarUrl(Eval("ID").ToString())%>" onerror="this.onerror=null;this.src='../../images/common/noavatar_medium.gif';" width="48px" />
        </ItemTemplate>
    </asp:TemplateColumn>
    <asp:BoundColumn DataField="GroupTitle" HeaderText="所属组"></asp:BoundColumn>
    <asp:BoundColumn DataField="NickName" HeaderText="昵称"></asp:BoundColumn>
    <asp:BoundColumn DataField="posts" HeaderText="发帖数"></asp:BoundColumn>
    <asp:BoundColumn DataField="joindate" HeaderText="注册时间" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
    <asp:BoundColumn DataField="credits" HeaderText="积分"></asp:BoundColumn>
    <asp:BoundColumn DataField="email" HeaderText="邮箱"></asp:BoundColumn>
    <asp:TemplateColumn HeaderText="最后活动/上次访问时间">
        <ItemTemplate>
            <%# Eval("LastActivity")%><br /><%# Eval("LastVisit")%>
        </ItemTemplate>
    </asp:TemplateColumn>
</Columns>
</cc1:DataGrid>
</td>
</tr>
</table>
<div class="Navbutton">
<table style="float:right">
    <tr>
        <td><cc1:Button ID="StopTalk" runat="server" Text=" 禁 言 " designtimedragdrop="247" Enabled="false"></cc1:Button>&nbsp;&nbsp;</td>
        <td><cc1:Button ID="DeleteUser" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false"></cc1:Button>&nbsp;&nbsp;</td>
        <td>
            <cc1:CheckBoxList ID="deltype" runat="server" RepeatColumns="1" RepeatLayout="flow">
                <asp:ListItem Value="1">删除但保留该用户所发帖子</asp:ListItem>
                <asp:ListItem Value="2">删除但保留该用户已发送的短消息</asp:ListItem>
            </cc1:CheckBoxList>
        </td>
    </tr>
</table>              
</div>
</form>
<%=footer%>
</body>
</html>