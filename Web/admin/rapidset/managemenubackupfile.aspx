<%@ Page Language="C#" Inherits="BBX.Web.Admin.managemenubackupfile" Codebehind="managemenubackupfile.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>无标题页</title>
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript">
        function Check(form)
        {
            CheckAll(form);
            checkedEnabledButton(form,'backupname','Delbackupfile');
        }
    </script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<br />
    <form id="form1" runat="server">
    <cc1:datagrid id="DataGrid1" runat="server">
       <Columns>
        <asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
            <HeaderStyle Width="20px" />
            <ItemTemplate>
                <input id="backupname" onclick="checkedEnabledButton(this.form,'backupname','Delbackupfile')" type="checkbox" value="<%# Eval("backupname").ToString() %>"	name="backupname" />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:BoundColumn DataField="backupdate" HeaderText="备份日期"></asp:BoundColumn>
          <asp:TemplateColumn HeaderText="操作">
            <ItemTemplate>
                <a href="managemenubackupfile.aspx?filename=<%# Eval("backupname").ToString() %>" onclick='return confirm("您确认要将 <%# Eval("backupdate").ToString() %> 的备份菜单恢复吗？\n\t注意：恢复将覆盖当前使用中的菜单！");'>恢复此备份</a>
            </ItemTemplate>
          </asp:TemplateColumn>
      </Columns>
    </cc1:datagrid>
    <p style="text-align:right;">
        <cc1:Button id="backupfile" runat="server" Text="备 份" ButtonImgUrl="../images/zip.gif" OnClick="backupfile_Click"></cc1:Button>&nbsp;&nbsp;
        <cc1:Button id="Delbackupfile" runat="server" Text="删 除" ButtonImgUrl="../images/del.gif" OnClick="Delbackupfile_Click" Enabled="false" ></cc1:Button>&nbsp;&nbsp;
        <button type="button" class="ManagerButton" id="Button3" onclick="window.location='managemainmenu.aspx';"><img src="../images/arrow_undo.gif"/> 返 回</button>
    </p>
    </form>
    <%=footer%>
</body>
</html>
