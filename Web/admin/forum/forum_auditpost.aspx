<%@ Page Language="c#" Inherits="BBX.Web.Admin.auditpost" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="AjaxPostInfo" Src="../UserControls/AjaxPostInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>审核帖子</title>
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
            checkedEnabledButton(form, 'pid', 'SelectPass', 'SelectDelete');
        }

        function jumppage(pageid) {
            window.location.href = 'forum_auditpost.aspx?pageid=' + pageid;
        }
    </script>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <%--<table width="100%">
        <tr>
            <td class="item_title" colspan="2">
                当前帖子表
            </td>
        </tr>
        <tr>
            <td class="vtop rowform">
                <cc1:DropDownList id="postlist" runat="server">
                </cc1:DropDownList>
            </td>
            <td class="vtop">
            </td>
        </tr>
    </table>--%>
    <table class="ntcplist">
        <tbody>
            <tr class="head">
                <td>
                    审核帖子列表
                </td>
            </tr>
            <tr>
                <td>
                    <table class="datalist" id="postdatagrid" style="border-collapse: collapse;" border="1"
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
                                    发布日期
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    发帖人
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    帖子状态
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    附件数
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    评分分数
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    评分次数
                                </td>
                            </tr>
                            <%foreach (BBX.Entity.Post postInfo in auditPostList)
                              {%>
                            <tr class="mouseoutstyle" onmouseover="this.className='mouseoverstyle'" onmouseout="this.className='mouseoutstyle'"
                                style="">
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap">
                                    <input id="pid" onclick="checkedEnabledButton(this.form,'pid','SelectPass','SelectDelete')"
                                        value="<%=postInfo.ID%>|<%=postInfo.Tid%>" name="pid" type="checkbox" />
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);" align="left">
                                    <a href="javascript:void(0);" onclick="javascript:LoadInfo('false','<%=postInfo.ID%>','<%=postInfo.Tid%>');">
                                        (无标题)</a>
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);">
                                    <%=postInfo.PostDateTime %>
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);">
                                    <a href="../../userinfo.aspx?userid=<%=postInfo.PosterID%>" target="_blank">
                                        <%=postInfo.Poster%></a>
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);">
                                    未审核
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);">
                                    <%=postInfo.Attachment%>
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);">
                                    <%=postInfo.Rate %>
                                </td>
                                <td style="border: 1px solid rgb(234, 233, 225);">
                                    <%=postInfo.RateTimes %>
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
                    <%=postCount %>
                    条记录 &nbsp;&nbsp;
                    <%=ShowPageIndex()%>
                    跳转到<input id="jumpindex" type="text" style="width: 30px; height: 18px" />页
                    <button onclick="jumppage($('jumpindex').value);return false;">
                        跳转</button>
                </td>
            </tr>
        </tbody>
    </table>
    <p style="text-align: right;">
        <cc1:Button id="SelectPass" runat="server" Text=" 通 过 " Enabled="false">
        </cc1:Button>&nbsp;&nbsp;
        <cc1:Button id="SelectDelete" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif"
            Enabled="false" OnClientClick="if(!confirm('你确认要删除所选未审核帖子吗？')) return false;">
        </cc1:Button>
    </p>
    <div id="AjaxPostInfo" style="overflow-y: auto;" valign="top">
        <uc1:AjaxPostInfo id="AjaxPostInfo1" runat="server">
        </uc1:AjaxPostInfo>
    </div>
    <div id="div1" style="display: none">
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
