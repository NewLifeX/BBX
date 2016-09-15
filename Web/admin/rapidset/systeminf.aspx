<%@ Page language="c#" Inherits="BBX.Web.Admin.systeminf" Codebehind="systeminf.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
    <head>
        <title>系统信息</title>
        <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet">
        <script language="javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
    <body>
        <div align="left">
            <form id="Form1" runat="server">
                
                <br /><br /> &nbsp; &nbsp; <img src="../images/hint.gif" border="0" alt="提示:" width="11" height="13" /> <b>.NET 服务器相关信息</b>
                <hr style="height:1px; width:600; color:#CCCCCC; background:#CCCCCC; border: 0; " align="left"/>
                <table cellspacing="0" cellpadding="4" width="100%" align="center">
                <tr>
                    <td  class="panelbox" width="50%" align="left">
                        <table width="100%">
                            <tr>
                                <td style="width:120px"><b>服务器名称:</b></td>
                                <td><asp:label ID="servername" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>服务器IP地址:</b></td>
                                <td><asp:label ID="serverip" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>服务器IIS版本:</b></td>
                                <td><asp:label ID="serversoft" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>HTTPS:</b></td>
                                <td><asp:label ID="serverhttps" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>服务端脚本执行超时:</b></td>
                                <td><asp:label ID="serverout" runat="server" />秒</td>
                            </tr>
                        </table>
                    </td>
                    <td  class="panelbox" width="50%" align="right">
                        <table width="100%">
                            <tr>
                                <td style="width:120px"><b>服务器操作系统:</b></td>
                                <td><asp:label ID="serverms" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>服务器域名:</b></td>
                                <td><asp:label ID="server_name" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>.NET解释引擎版本:</b></td>
                                <td><asp:label ID="servernet" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>HTTP访问端口:</b></td>
                                <td><asp:label ID="serverport" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>服务器当前时间:</b></td>
                                <td><asp:label ID="servertime" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="panelbox" colspan="2">
                        <table width="100%">
                            <tr>
                                <td style="width:120px"><b>执行文件绝对路径:</b></td>
                                <td><asp:label ID="servernpath" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>虚拟目录绝对路径:</b></td>
                                <td><asp:label ID="serverppath" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </table>
            
                <br /><br /> &nbsp; &nbsp; <img src="../images/hint.gif" border="0" alt="提示:" width="11" height="13" /> <b>浏览者相关信息</b>
                <hr style="height:1px; width:600; color:#CCCCCC; background:#CCCCCC; border: 0; " align="left"/>
                <table cellspacing="0" cellpadding="4" width="100%" align="center">
                <tr>
                    <td  class="panelbox" width="50%" align="left">
                        <table width="100%">
                            <tr>
                                <td style="width:120px"><b>浏览者ip地址:</b></td>
                                <td><asp:label ID="cip" runat="server"></asp:label></td>
                            </tr>
                            <tr>
                                <td><b>浏览器:</b></td>
                                <td><asp:label ID="ie" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>JavaScript:</b></td>
                                <td><asp:label ID="javas" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>JavaApplets:</b></td>
                                <td><asp:label ID="javaa" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>语言:</b></td>
                                <td><asp:label ID="cl" runat="server"></asp:label></td>
                            </tr>
                        </table>
                    </td>
                    <td  class="panelbox" width="50%" align="right">
                        <table width="100%">
                            <tr>
                                <td style="width:120px"><b>浏览者操作系统:</b></td>
                                <td><asp:label ID="ms" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>浏览器版本:</b></td>
                                <td><asp:label ID="vi" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>VBScript:</b></td>
                                <td><asp:label ID="vbs" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>Cookies:</b></td>
                                <td><asp:label ID="cookies" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>Frames(分栏):</b></td>
                                <td><asp:label ID="frames" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </table>
                <br /><br /> &nbsp; &nbsp; <img src="../images/hint.gif" border="0" alt="提示:" width="11" height="13" /> <b>执行效率相关情况</b>
                <hr style="height:1px; width:600; color:#CCCCCC; background:#CCCCCC; border: 0; " align="left"/>
                <table cellspacing="0" cellpadding="4" width="100%" align="center">
                <tr>
                    <td  class="panelbox" width="50%" align="left">
                        <table width="100%">
                            <tr>
                                <td style="width:120px"><b>本页执行时间:</b></td>
                                <td><asp:label ID="runtime" runat="server" />毫秒</td>
                            </tr>
                        </table>
                    </td>
                    <td  class="panelbox" width="50%" align="right">
                        <table width="100%">
                            <tr>
                                <td style="width:120px"><b>5000万次加<br />法循环测试:</b></td>
                                <td>
                                    <cc1:Button id="for5000" runat="server" Text="测试"></cc1:Button>&nbsp;
                                    <asp:label ID="l5000" runat="server"></asp:label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </table>
            </form>
            <%=footer%>
        </div>
    </body>
</html>
