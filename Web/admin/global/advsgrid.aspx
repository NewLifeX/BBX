<%@ Page Language="c#" Inherits="BBX.Web.Admin.advsgrid" Codebehind="advsgrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>广告列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
    function Check(form)
    {
        CheckAll(form);
        checkedEnabledButton(form,'advid','DelAds','SetAvailable','SetUnAvailable')
    }
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="advtypes">
<h4>广告类型</h4>
<a href="advsgrid.aspx?type=" <%if(type<0){%>class="currenttype"<%}%>>全部</a>
<%for(int i=0;i<advtypes.Length;i++)
  {%>
     <a href="advsgrid.aspx?type=<%=i%>" <%if(type==i){%>class="currenttype"<%}%>><%=advtypes[i]%></a>
  <%} %>
</div>
<cc1:DataGrid ID="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid">
    <Columns>
        <asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
            <HeaderStyle Width="20px" />
            <ItemTemplate>
                    <input id="advid" onClick="checkedEnabledButton(this.form,'advid','DelAds','SetAvailable','SetUnAvailable')" type="checkbox" value="<%# Eval("ID").ToString() %>" name="advid" />							</asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="">
            <ItemTemplate>
                    <a href="global_editadvs.aspx?advid=<%# Eval("ID").ToString()%>">编辑</a>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:BoundColumn DataField="ID" SortExpression="ID" HeaderText="广告id [递增]"
            Visible="false"></asp:BoundColumn>
        <asp:TemplateColumn HeaderText="是否有效" SortExpression="available">
            <ItemTemplate>
                    <%# BoolStr(Eval("available").ToString())%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:BoundColumn DataField="title" SortExpression="title" HeaderText="广告标题"></asp:BoundColumn>
        <asp:BoundColumn DataField="displayorder" SortExpression="displayorder" HeaderText="显示顺序">
        </asp:BoundColumn>
        <asp:TemplateColumn HeaderText="广告类型" SortExpression="type">
            <ItemTemplate>
                    <a href="advsgrid.aspx?type=<%#Eval("type").ToString()%>"><%# GetAdType(Eval("type").ToString())%></a>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="广告投放范围" SortExpression="targets">
            <ItemTemplate>
                    <%# TargetsType(Eval("targets").ToString())%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="样式" SortExpression="parameters">
            <ItemTemplate>
                    <%# ParameterType(Eval("parameters").ToString())%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="生效时间" SortExpression="starttime">
            <ItemTemplate>
                    <%# Eval("starttime").ToString()== "1900-1-1 0:00:00" ? "" : Convert.ToDateTime(Eval("starttime").ToString()).ToShortDateString()%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="结束时间" SortExpression="endtime">
            <ItemTemplate>
                    <%# Eval("endtime").ToString()== "2555-1-1 0:00:00" ? "" : Convert.ToDateTime(Eval("endtime").ToString()).ToShortDateString()%>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</cc1:DataGrid>
<p style="text-align:right;">
    <cc1:Button ID="DelAds" runat="server" Text="删 除" ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选的广告吗？')) return false;"></cc1:Button>&nbsp;&nbsp;
    <cc1:Button ID="SetAvailable" runat="server" Text="置为有效" Enabled="false"></cc1:Button>&nbsp;&nbsp;
    <cc1:Button ID="SetUnAvailable" runat="server" Text="置为无效" ButtonImgUrl="../images/invalidation.gif" Enabled="false"></cc1:Button>&nbsp;&nbsp;
    <button type="button" class="ManagerButton" onClick="javascript:window.location.href='global_addadvs.aspx';">
        <img src="../images/add.gif" />添加广告
    </button>
</p>
</form>
<%=footer%>
</body>
</html>