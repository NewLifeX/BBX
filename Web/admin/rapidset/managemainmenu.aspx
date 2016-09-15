<%@ Page Language="C#" Inherits="BBX.Web.Admin.managemainmenu" Codebehind="managemainmenu.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>主菜单管理</title>
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript">
        function newMenu()
        {
            document.getElementById("opt").innerHTML = "新建主菜单";
            document.getElementById("menuid").value = "0";
            document.getElementById("mode").value = "new";
            document.getElementById("menutitle").value = "";
            document.getElementById("defaulturl").value = "";
            BOX_show('neworeditmainmenu');
        }
        function editMenu(menuid,menutitle,defaulturl)
        {
            document.getElementById("opt").innerHTML = "编辑主菜单";
            document.getElementById("menuid").value = menuid;
            document.getElementById("mode").value = "edit";
            document.getElementById("menutitle").value = menutitle;
            document.getElementById("defaulturl").value = defaulturl;
            BOX_show('neworeditmainmenu');
        }
        function chkSubmit()
        {
            if(document.getElementById("menutitle").value == "")
            {
                alert("主菜单名称不能为空！");
                document.getElementById("menutitle").focus();
                return false;
            }
            if(document.getElementById("defaulturl").value == "")
            {
                if(!confirm("您确认要将默认展现页面地址置空吗？"))
                {
                    document.getElementById("defaulturl").focus();
                    return false;
                }
            }
            document.getElementById("form1").submit();
            return true;
        }
    </script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:PageInfo ID="info1" runat="server" Icon="information" Text="<li>主菜单项必须在其下没有子菜单时才可删除!</li><li>编辑完菜单后必须点击<b>生成菜单</b>按钮才能使编辑生效!</li><li>建议在编辑菜单前先进入<b>备份管理</b>中对当前未编辑的菜单进行备份,如果修改不当还可以从备份中恢复!</li>" />
        <cc1:datagrid id="DataGrid1" runat="server">
           <Columns>
            <asp:BoundColumn DataField="id" HeaderText="序号" readonly="true"></asp:BoundColumn>
            <asp:BoundColumn DataField="title" HeaderText="主菜单名称"></asp:BoundColumn>
            <asp:BoundColumn DataField="defaulturl" HeaderText="默认展现页面地址"><ItemStyle HorizontalAlign="left" /></asp:BoundColumn>
            <asp:BoundColumn DataField="system" HeaderText="系统菜单" readonly="true"></asp:BoundColumn>
              <asp:TemplateColumn HeaderText="操作">
                <ItemTemplate>
                    <a href="javascript:;" onclick="editMenu('<%# Eval("id").ToString() %>','<%# Eval("title").ToString() %>','<%# Eval("defaulturl").ToString() %>');">编辑</a>&nbsp;
                    <%# Eval("delitem").ToString()%>&nbsp;
                    <a href="managesubmenu.aspx?menuid=<%# Eval("id").ToString() %>">管理子菜单</a>
                </ItemTemplate>
              </asp:TemplateColumn>
          </Columns>
        </cc1:datagrid>
        <p style="text-align:right;">
            <button type="button" class="ManagerButton" id="Button2" onclick="newMenu();"><img src="../images/add.gif"/> 新 建 </button>
            <button type="button" class="ManagerButton" id="Button3" onclick="window.location='managemenubackupfile.aspx';"><img src="../images/zip.gif"/>备份管理</button>
            <cc1:Button ID="createMenu" runat="server" Text="生成菜单"></cc1:Button>	        
        </p>
        <div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
        <div id="neworeditmainmenu" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:400px;">
            <div class="ManagerForm">
                <fieldset>
                <legend id="opt" style="background:url(../images/icons/icon53.jpg) no-repeat 6px 50%;">新建主菜单</legend>
                <table cellspacing="0" cellPadding="4" class="tabledatagrid" width="80%">
                    <tr>
                        <td width="30%">
                            主菜单名称:
                            <input type="hidden" id="menuid" name="menuid" value="0" />
                            <input type="hidden" id="mode" name="mode" value="" />
                        </td>
                        <td width="70%"><input id="menutitle"  name="menutitle" type="text" maxlength="8" size="10"class="FormBase" onfocus="this.className='FormFocus';" onblur="this.className='FormBase';" /></td>
                    </tr>
                    <tr>
                        <td>默认展现<br />页面地址:</td>
                        <td><input id="defaulturl" name="defaulturl" type="text" maxlength="100" size="30" class="FormBase" onfocus="this.className='FormFocus';" onblur="this.className='FormBase';" /></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <button type="button" class="ManagerButton" id="AddNewRec" onclick="chkSubmit();"><img src="../images/add.gif"/> 提 交 </button>&nbsp;&nbsp;
                            <button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworeditmainmenu');"><img src="../images/state1.gif"/> 取 消 </button>
                        </td>
                    </tr>
                </table>
                </fieldset>
            </div>
        </div>
    </form>
    <div id="setting" />
    <%=footer%>
</body>
</html>
