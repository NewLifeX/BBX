<%@ Page Language="c#" Inherits="BBX.Web.Admin.restoresetupinf" Codebehind="global_restoresetupinf.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>restoresetupinf</title>
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body topmargin="0" leftmargin="0">
    <form id="Form1" method="post" runat="server">
        <table width="100%" border="1">
            <tr>
                <td class="td1">说明:此项功能仅在您想将整个论坛设置中的所有信息恢复到初始化安装状态下时才可使用.</td>
            </tr>
            <tr>
                <td class="td1">
                    <cc1:Button ID="RestoreInf" runat="server" Text="开始恢复"></cc1:Button>
                </td>
            </tr>
        </table>
    </form>
    <%=footer%>
</body>
</html>
