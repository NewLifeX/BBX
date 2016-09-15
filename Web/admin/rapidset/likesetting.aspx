<%@ Page Language="c#" Inherits="BBX.Web.Admin.likesetting" Codebehind="likesetting.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>个人喜好设置</title>
    <script type="text/javascript" src="../js/common.js"></script>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <div class="ManagerForm">
        <form id="form1" runat="server">
        <uc1:PageInfo ID="info1" runat="server" Icon="information" Text="设置本提示是否在其它页面中显示." />
            <fieldset>
            <legend style="background:url(../images/icons/icon58.jpg) no-repeat 6px 50%;">个人喜好设置</legend>
                <table class="table1" cellspacing="0" cellpadding="8" width="100%" align="center">
                    <tr><td class="item_title" colspan="2">显示页面帮助信息</td></tr>
                    <tr>
                        <td class="vtop rowform">
                            <cc1:RadioButtonList id="showhelp" runat="server"  RepeatLayout="flow">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Value="0">否</asp:ListItem>
                            </cc1:RadioButtonList>
                        </td>
                        <td class="vtop">选择"是", 则会在页面中显示提示信息</td>
                    </tr>
                    <tr><td class="item_title" colspan="2">显示自动升级提示</td></tr>
                    <tr>
                        <td class="vtop rowform">
                            <cc1:RadioButtonList id="showupgrade" runat="server"  RepeatLayout="flow" HintTitle="提示" 
                                HintInfo="">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Value="0">否</asp:ListItem>
                            </cc1:RadioButtonList>
                        </td>
                        <td class="vtop">选择"是", 则打开自动升级检测.选择"否",则不再提醒自动升级检测.<br />在关闭状态下可通过"快捷操作"中的"在线升级"中检测是否有升级.</td>
                    </tr>
                </table>
            </fieldset>
            <div align="center">
                <cc1:Button ID="saveinfo" runat="server" Text="提 交" OnClick="saveinfo_Click" ></cc1:Button>
            </div>
            <cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
        </form>
    </div>
    <%=footer%>
</body>
</html>
