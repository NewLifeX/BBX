<%@ Page Language="c#" Inherits="BBX.Web.Admin.setting" Codebehind="setting.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>firststep</title>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/tab.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <form id="Form1" runat="server">
        <uc1:PageInfo id="info1" runat="server" Icon="information"
        Text="如果您不熟悉论坛的设置, 可以通过&quot;快速设置向导&quot;帮助您快速的设置论坛"></uc1:PageInfo>
        <uc1:PageInfo id="info2" runat="server" Icon="warning"
        Text="如果您已经运行快速设置向导, 或者自行配置过论坛, 则执行此次&quot;快速设置向导&quot;将覆盖您已经设置的相关参数"></uc1:PageInfo>
        <br />
        <table class="table1" cellspacing="0" cellpadding="4" width="100%" border="0">
            <tr>
                <td width="1px"></td>
                <td>
                    <cc2:TabControl ID="TabControl1" SelectionMode="Client" runat="server" TabScriptPath="../js/tabstrip.js" Height="100%">
                        <cc2:TabPage Caption="请选择您的论坛类型" ID="tabPage51">
                        <table cellspacing="0" cellpadding="4" width="100%" align="center">
                            <tr>
                                <td  class="panelbox" width="50%" align="left">
                                    <table width="100%">
                                        <tr>
                                            <td style="width:100px">论坛规模:</td>
                                            <td>
                                                <cc1:RadioButtonList ID="size" runat="server" RepeatColumns="1">
                                                    <asp:ListItem Value="1">小型 (在线人数 <500)</asp:ListItem>
                                                    <asp:ListItem Value="2" Selected="True">中型 (在线人数 < 5000)</asp:ListItem>
                                                    <asp:ListItem Value="3">大型 (在线人数 < 50000)</asp:ListItem>
                                                </cc1:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>安全等级:</td>
                                            <td>
                                                <cc1:RadioButtonList ID="safe" runat="server" RepeatColumns="1">
                                                    <asp:ListItem Value="1">高 (用户部分操作受到比较多的限制)</asp:ListItem>
                                                    <asp:ListItem Value="2">中 (用户部分操作受到一定限制)</asp:ListItem>
                                                    <asp:ListItem Value="3" Selected="True">低 (本身系统安全已经很高)</asp:ListItem>
                                                </cc1:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td  class="panelbox" width="50%" align="right">
                                    <table width="100%">
                                        <tr>
                                            <td>功能偏好:</td>
                                            <td>
                                                <cc1:RadioButtonList ID="func" runat="server" RepeatColumns="1">
                                                    <asp:ListItem Value="1">简洁 (关闭娱乐和部分功能, 速度将会有所提升)</asp:ListItem>
                                                    <asp:ListItem Value="2">一般 (满足大多数用户需求,速度功能比较平衡)</asp:ListItem>
                                                    <asp:ListItem Value="3" Selected="True">丰富 (启用所有娱乐功能, 速度将会有所下降)</asp:ListItem>
                                                </cc1:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        </cc2:TabPage>
                        <cc2:TabPage Caption="设置论坛基本信息" ID="tabPage22">
                        <table cellspacing="0" cellpadding="4" width="100%" align="center">
                            <tr>
                                <td  class="panelbox" width="50%" align="left">
                                    <table width="100%">
                                        <tr>
                                            <td style="width:100px">论坛名称:</td>
                                            <td><cc1:TextBox ID="forumtitle" runat="server" Width="250" RequiredFieldType="暂无校验" HintInfo="论坛名称, 将显示在导航条和标题中"></cc1:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>论坛URL地址:</td>
                                            <td><cc1:TextBox ID="forumurl" runat="server" Width="250" RequiredFieldType="暂无校验"></cc1:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                                <td  class="panelbox" width="50%" align="left">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 100px">网站名称:</td>
                                            <td><cc1:TextBox ID="webtitle" runat="server" Width="250" RequiredFieldType="暂无校验" HintInfo="网站名称, 将显示在页面底部的联系方式处"></cc1:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>网站URL地址:</td>
                                            <td><cc1:TextBox ID="weburl" runat="server" Width="250" RequiredFieldType="暂无校验" HintInfo="网站 URL, 将作为链接显示在页面底部"></cc1:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        
                        
                            <table id="Table1" class="table1" cellspacing="0" cellpadding="4" width="100%" align="center" bgcolor="#C3C7D1">
                                <tr>
                                </tr>
                            </table>
                        </cc2:TabPage>
                    </cc2:TabControl>
                </td>
            </tr>
        </table>
        <br />
        <cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
        &nbsp; &nbsp;<cc1:Button ID="submitsetting" runat="server" Text=" 提 交 "></cc1:Button>
        <%=footer%>
    </form>
</body>
</html>
