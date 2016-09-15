<%@ Page Language="C#" Codebehind="global_helplist.aspx.cs" Inherits="BBX.Web.Admin.helplist" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/calendar.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript">
        function selectSubItem(form,checked,subid)
        {
            for (var i=0;i<form.elements.length;i++)
            {
                var e = form.elements[i];
                if (e.id == "id" + subid)
                   e.checked = checked;
            }
            checkedEnabledButton(form,'id','DelRec');
        }
        
        function Check(form)
        {
            CheckAll(form);
            checkedEnabledButton(form,'id','DelRec');
        }
    </script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <form id="form1" runat="server" action="">
        <div class="ManagerForm">
            <fieldset>
                <legend style="background: url(../images/icons/icon34.jpg) no-repeat 6px 50%;">帮助管理</legend>
                <div class="Navbutton">
                    <table cellspacing="1" cellpadding="4" align="center" class="table1">
                        <tbody>
                            <tr>
                                <td>
                                    <input title="选中/取消选中 本页所有Case" onclick="Check(this.form)" type="checkbox" name="chkall" id="chkall" />全选/取消全选 &nbsp;&nbsp;
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <table width="100%" border="0">
                    <tr>
                        <td width="25%" align="center">标题</td>
                        <td width="37%" align="center">排序号</td>
                        <td width="25%" align="center">编辑</td>
                    </tr>
                    <%if (helpInfoList != null)
                      {
                          foreach (BBX.Entity.Help hi in helpInfoList)
                          {
                              string kg = "", id;
                              if (hi.Pid == 0)
                              {
                                  id = hi.ID.ToString();
                              }
                              else
                              {
                                  id = hi.Pid.ToString();
                                  kg = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                              }
                    %>
                    <tr onmouseover="this.style.background='#ECFFE6'; " onmouseout="this.style.background=''; this.style.borderColor=''">
                        <td align="left">
                            <%=kg%>
                            <input type="checkbox" name="id" value="<%=hi.ID%>" id="id<%=id%>" 
                            onclick="<%if (hi.Pid == 0){%>selectSubItem(this.form,checked,<%=id%>)<%}else{%>checkedEnabledButton(form,'id','DelRec')<%}%>" />
                            <a href="../../help.aspx?hid=<%=hi.ID%>" target="_blank"><%=hi.Title%></a>
                        </td>
                        <td align="center">
                            <input type="text" size="3" maxlength="4" name="orderbyid" value="<%=hi.Orderby %>"
                                class="FormBase" onfocus="this.className='FormFocus';" onblur="this.className='FormBase';" />
                            <input name="hidid" type="hidden" value="<%=hi.ID%>" /></td>
                        <td align="center">
                            <a href="global_edithelp.aspx?id=<%=hi.ID%>">编辑</a>
                        </td>
                    </tr>
                        <%}%>
                    <%}
                      else
                      {%>
                    <tr>
                        <td>没有数据</td>
                    </tr>
                    <%} %>
                </table>
            </fieldset>
            <p style="text-align:right;">
                <cc1:Button ID="Orderby" runat="server" Text=" 确 定 " ButtonImgUrl="../images/submit.gif"></cc1:Button>&nbsp;&nbsp;
                <cc1:Button ID="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选的帮助项吗？')) return false;"></cc1:Button>&nbsp;&nbsp;
                <button type="button" class="ManagerButton" onclick="javascript:window.location.href='global_addhelpclass.aspx';">
                    <img src="../images/add.gif" />添加分类
                </button>&nbsp;&nbsp;
                <button type="button" class="ManagerButton" onclick="javascript:window.location.href='global_addhelp.aspx';">
                    <img src="../images/add.gif" />添加帮助
                </button>
            </p>
    </form>
    <%=footer%>
</body>
</html>
