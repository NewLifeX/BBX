<%@ Page Language="C#" Inherits="BBX.Web.Admin.manageshortcutmenu" Codebehind="manageshortcutmenu.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>快捷菜单管理</title>
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet">
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet">
    <script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:PageInfo ID="info1" runat="server" Icon="information" Text="添加方式：请点击功能页面上的“<img style=&quot;vertical-align:middle&quot; src=&quot;../images/addmenu.gif&quot; align=&quot;absmiddle&quot;> 加入收藏”链接进行收藏." />
    <cc1:datagrid id="DataGrid1" runat="server" OnDeleteCommand="DataGrid1_DeleteCommand" OnItemDataBound="DataGrid1_ItemDataBound">
      <Columns>
        <asp:BoundColumn DataField="local" HeaderText="位置"></asp:BoundColumn>
        <asp:ButtonColumn CommandName="Delete" HeaderText="操作" Text="删除"></asp:ButtonColumn>
      </Columns>
    </cc1:datagrid>
    </form>
    <%=footer%>
</body>
</html>
