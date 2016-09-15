<%@ Page Language="c#" Inherits="BBX.Web.Admin.AddUser" Codebehind="AddUser.aspx.cs" %>

<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>添加用户</title>
    <script type="text/javascript" src="../js/common.js"></script>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
    <script type="text/javascript">
        function IsValidPost() {
            var admingroup = "";
            var groupid = document.getElementById("groupid");
            if ((groupid.value == "1") || (groupid.value == "2") || (groupid.value == "3")) {
                admingroup = groupid.options[groupid.value].innerHTML;
            }
            if (groupid.value == "0") {
                alert('您未选择所属用户组');
                return false;
            }
            if (admingroup != "")
                if (confirm('您是要添加 "' + admingroup + '" 组的用户吗?')) {
                    return true;
                }
                else {
                    return false;
                }
            return true;
        }
    </script>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <div class="ManagerForm">
        <form id="Form1" method="post" runat="server">
            <fieldset>
                <legend style="background: url(../images/icons/icon9.jpg) no-repeat 6px 50%;">添加用户</legend>
                <table width="100%">
                    <tr>
                        <td class="item_title" colspan="2">用户名</td>
                    </tr>
                    <tr>
                        <td class="vtop rowform">
                            <cc1:TextBox ID="userName" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验" MaxLength="20" Size="30"></cc1:TextBox>
                        </td>
                        <td class="vtop"></td>
                    </tr>
                    <tr>
                        <td class="item_title" colspan="2">密码</td>
                    </tr>
                    <tr>
                        <td class="vtop rowform">
                            <cc1:TextBox ID="password" runat="server" CanBeNull="必填" MaxLength="32" RequiredFieldType="暂无校验" Size="30"></cc1:TextBox>
                        </td>
                        <td class="vtop"></td>
                    </tr>
                    <tr>
                        <td class="item_title" colspan="2">E-mail</td>
                    </tr>
                    <tr>
                        <td class="vtop rowform" colspan="2">
                            <cc1:TextBox ID="email" runat="server" CanBeNull="必填" RequiredFieldType="电子邮箱" Width="200" MaxLength="50" Size="60"></cc1:TextBox>&nbsp;&nbsp;<input id="sendemail" type="checkbox" runat="server" checked="checked" />发送邮件到上述邮箱中
                        </td>
                    </tr>
                    <tr>
                        <td class="item_title">所属用户组</td>
                        <td class="item_title">积分设置</td>
                    </tr>
                    <tr>
                        <td class="vtop rowform">
                            <cc1:DropDownList ID="groupid" runat="server"></cc1:DropDownList>
                        </td>
                        <td class="vtop rowform">
                            <cc1:TextBox ID="credits" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" MaxLength="8" Size="10"></cc1:TextBox>设置用户的初始积分
                        </td>
                    </tr>
                    <tr>
                        <td class="item_title">真实姓名</td>
                        <td class="item_title">身份证号</td>
                    </tr>
                    <tr>
                        <td class="vtop rowform">
                            <cc1:TextBox ID="realname" runat="server" RequiredFieldType="暂无校验" MaxLength="10" Size="10"></cc1:TextBox>
                        </td>
                        <td class="vtop rowform">
                            <cc1:TextBox ID="idcard" runat="server" MaxLength="20" Size="20"></cc1:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="item_title">移动电话号码</td>
                        <td class="item_title">固定电话号码</td>
                    </tr>
                    <tr>
                        <td class="vtop rowform">
                            <cc1:TextBox ID="mobile" runat="server" MaxLength="20" Size="20"></cc1:TextBox>
                        </td>
                        <td class="vtop rowform">
                            <cc1:TextBox ID="phone" runat="server" CanBeNull="可为空" MaxLength="20" Size="20"></cc1:TextBox>
                        </td>
                    </tr>
                </table>
                <cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
                <div class="Navbutton">
                    <cc1:Button ID="AddUserInfo" runat="server" Text=" 提 交 "></cc1:Button>
                </div>
            </fieldset>
        </form>
    </div>
    <%=footer%>
</body>
</html>
