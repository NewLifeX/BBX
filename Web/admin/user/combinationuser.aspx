<%@ Page Language="c#" Inherits="BBX.Web.Admin.combinationuser" Codebehind="combinationuser.aspx.cs" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>添加用户</title>
    <script type="text/javascript" src="../js/common.js"></script>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
 <form id="Form1" method="post" runat="server">
<uc1:PageInfo ID="info1" runat="server" Icon="information" Text="原用户的帖子、积分全部转入目标用户, 同时删除原用户" />
<div class="ManagerForm">
<fieldset>
<legend style="background: url(../images/icons/icon12.jpg) no-repeat 6px 50%;">合并用户</legend>
<table width="100%">
    <tr><td class="item_title" colspan="2">原用户名1</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:TextBox ID="username1" runat="server" RequiredFieldType="暂无校验" Width="200" IsReplaceInvertedComma="false"></cc1:TextBox>
        </td>
        <td class="vtop"></td>
    </tr>
    <tr><td class="item_title" colspan="2">原用户名2</td></tr>
    <tr>
        <td class="vtop rowform">
            <cc1:TextBox ID="username2" runat="server" RequiredFieldType="暂无校验" Width="200" IsReplaceInvertedComma="false"></cc1:TextBox>
        </td>
        <td class="vtop"></td>
    </tr>
    <tr><td class="item_title" colspan="2">原用户名3</td></tr>
    <tr>
        <td class="vtop rowform">
            <cc1:TextBox ID="username3" runat="server" RequiredFieldType="暂无校验" Width="200" IsReplaceInvertedComma="false"></cc1:TextBox>
        </td>
        <td class="vtop"></td>
    </tr>
    <tr><td class="item_title" colspan="2">合并到目标用户</td></tr>
    <tr>
        <td class="vtop rowform">
             <cc1:TextBox ID="targetusername" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验" Width="200" IsReplaceInvertedComma="false"></cc1:TextBox>
        </td>
        <td class="vtop"></td>
    </tr>
</table>
<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div class="Navbutton">
    <cc1:Button ID="CombinationUserInfo" runat="server" Text=" 提 交 "></cc1:Button>
</div>
</fieldset>
</div>
</form>
<%=footer%>
</body>
</html>