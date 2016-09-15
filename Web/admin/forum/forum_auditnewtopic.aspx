<%@ Page Language="c#" Inherits="BBX.Web.Admin.auditnewtopic" CodeBehind="forum_auditnewtopic.aspx.cs" %>

<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="AjaxPostInfo" Src="../UserControls/AjaxPostInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>审核主题</title>
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../js/AjaxHelper.js"></script>
    <script type="text/javascript">
        function LoadInfo(istopic, pid, tid) {
            AjaxHelper.Updater('../UserControls/AjaxPostInfo', 'AjaxPostInfo', 'istopic=' + istopic + '&pid=' + pid + '&tid=' + tid);
            document.getElementById('PostInfo').style.display = "block";
        }

        function Check(form) {
            CheckAll(form);
            checkedEnabledButton(form, 'tid', 'SelectPass', 'SelectDelete');
        }

        function jumppage(pageid) {
            window.location.href = 'forum_auditnewtopic.aspx?pageid=' + pageid;
        }
    </script>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table class="ntcplist">
        <tbody>
            <tr class="head">
                <td>
                    审核主题列表
                </td>
            </tr>
            <tr>
                <td>
                    <table class="datalist" id="topiclist" style="border-collapse: collapse;" border="1"
                        cellspacing="0" rules="all">
                        <tbody>
                            <tr class="category">
                                <td style="border: 1px solid rgb(234, 233, 225); width: 20px;" nowrap="nowrap">
                                    <input title="选中/取消" onclick="Check(this.form)" name="chkall" id="chkall" type="checkbox" />
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    标题
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    作者
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    主题类型
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    主题状态
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    发布时间
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    是否含有附件
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    是否关闭
                                </td>
                            </tr>
                            <%foreach (BBX.Entity.Topic topicInfo in auditTopicList)
                              { %>
                            <tr class="mouseoutstyle" onmouseover="this.className='mouseoverstyle'" onmouseout="this.className='mouseoutstyle'">
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    <input id="tid" onclick="checkedEnabledButton(this.form,'tid','SelectPass','SelectDelete')"
                                        value="<%=topicInfo.ID %>" name="tid" type="checkbox" />
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" align="left">
                                    <a href="javascript:void(0);" onclick="javascript:LoadInfo('true','0','<%=topicInfo.ID %>');">
                                        <%=topicInfo.Title %></a>
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);">
                                    <a href="../../userinfo.aspx?userid=<%=topicInfo.PosterID %>" target="_blank">
                                        <%=topicInfo.Poster %></a>
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);">
                                    <%=GetTopicType(topicInfo.Special.ToString())%>
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);">
                                    <%=GetTopicStatus(topicInfo.DisplayOrder.ToString())%>
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);">
                                    <%=topicInfo.PostDateTime%>
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);">
                                    <img src="../images/<%=topicInfo.Attachment == 0?"cancel":"ok"%>.gif" />
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);">
                                    <img src="../images/<%=topicInfo.Closed == 0?"cancel":"ok"%>.gif" />
                                </td>
                            </tr>
                            <%} %>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    <table class="datagridpage" style="margin-top: 10px">
        <tbody>
            <tr>
                <td>
                    共
                    <%=pageCount%>
                    页, 当前第
                    <%=pageid %>
                    页, 共
                    <%=topicCount%>
                    条记录 &nbsp;&nbsp;<%=ShowPageIndex()%>
                    跳转到<input id="jumpindex" type="text" style="width: 30px; height: 18px" />页
                    <button onclick="jumppage($('jumpindex').value);return false;">
                        跳转</button>
                </td>
            </tr>
        </tbody>
    </table>
    <p style="text-align: right;">
        <cc1:Button id="SelectPass" runat="server" Text=" 通 过 " Enabled="false">
        </cc1:Button>
        &nbsp;&nbsp;
        <cc1:Button id="SelectDelete" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif"
            Enabled="false" OnClientClick="if(!confirm('你确认要删除所选未通过审核主题吗？')) return false;">
        </cc1:Button>
    </p>
    <div id="AjaxPostInfo" style="overflow-y: auto;" valign="top">
        <uc1:AjaxPostInfo id="AjaxPostInfo1" runat="server">
        </uc1:AjaxPostInfo>
    </div>
    <div id="Div1" style="display: none">
        <tr>
            <td bgcolor="#f8f8f8" colspan="2">
                <asp:Literal ID="msg" runat="server" Text="没有等待审核新主题" Visible="False"></asp:Literal>
            </td>
        </tr>
    </div>
    </form>
    <%=footer%>
</body>
</html>
