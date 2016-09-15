<%@ Page Language="C#" AutoEventWireup="true" Inherits="BBX.Web.Admin.editgroup" Codebehind="editgroup.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>编辑用户组</title>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="ManagerForm">
        <fieldset>
        <legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">编辑用户组</legend>
        <table class="table1" cellspacing="0" cellpadding="8" width="100%" align="center" >
            <tr align="center">
                <td><button type="button" class="ManagerButton" onclick="window.location='usergroupgrid.aspx';"><img src="../images/submit.gif" /> 编辑积分用户组</button></td>
                <td><button type="button" class="ManagerButton" onclick="window.location='adminusergroupgrid.aspx';"><img src="../images/submit.gif" /> 编辑管理组</button></td>
                <td><button type="button" class="ManagerButton" onclick="window.location='sysadminusergroupgrid.aspx';"><img src="../images/submit.gif" /> 编辑系统组</button></td>
                <td><button type="button" class="ManagerButton" onclick="window.location='usergroupspecialgrid.aspx';"><img src="../images/submit.gif" /> 编辑特殊组</button></td>
            </tr>
        </table>
        </fieldset>
    </div>
    </form>
    <%=footer%>
</body>
</html>
