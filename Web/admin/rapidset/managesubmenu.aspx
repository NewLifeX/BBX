<%@ Page language="c#" Inherits="BBX.Web.Admin.managesubmenu" Codebehind="managesubmenu.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
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
            function newMenu()
            {
                document.getElementById("opt").innerHTML = "新建子菜单";
                document.getElementById("submenuid").value = "0";
                document.getElementById("menutitle").value = "";
                BOX_show('neworeditsubmenu');
            }
            function editMenu(menuid,menutitle)
            {
                document.getElementById("opt").innerHTML = "编辑子菜单";
                document.getElementById("submenuid").value = menuid;
                document.getElementById("menutitle").value = menutitle;
                BOX_show('neworeditsubmenu');
            }
            function chkSubmit()
            {
                if(document.getElementById("menutitle").value == "")
                {
                    alert("子菜单名称不能为空！");
                    document.getElementById("menutitle").focus();
                    return false;
                }
                document.getElementById("Form1").submit();
                return true;
            }
     </script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <form id="Form1" runat="server">
    <uc1:PageInfo ID="info1" runat="server" Icon="information" Text="子菜单中必须在其下没有子菜单项时才可删除!" />
    <cc1:datagrid id="DataGrid1" runat="server">
                <Columns>
                    <asp:BoundColumn DataField="menutitle" HeaderText="子菜单名称"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="操作"><HeaderStyle Width="30%" />
                        <ItemTemplate>
                            <a href="javascript:;" onclick="editMenu('<%# Eval("id").ToString() %>','<%# Eval("menutitle").ToString() %>');">编辑</a>&nbsp;
                            <%# Eval("delitem").ToString()%>&nbsp;
                            <a href="managesubmenuitem.aspx?menuid=<%# Eval("menuid").ToString() %>&submenuid=<%# Eval("submenuid").ToString() %>&pagename=<%# Server.UrlEncode(Eval("menutitle").ToString()) %>">管理子菜单项</a>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </cc1:datagrid>
    <p style="text-align:right;"><button type="button" class="ManagerButton" id="Button2" onclick="newMenu();"><img src="../images/add.gif"/> 新 建 </button>&nbsp;
            <button type="button" class="ManagerButton" id="Button3" onclick="window.location='managemainmenu.aspx';"><img src="../images/arrow_undo.gif"/> 返 回 </button></p>
    <div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
        <div id="neworeditsubmenu" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:400px;">
        <div class="ManagerForm">
            <fieldset>
            <legend id="opt" style="background:url(../images/icons/icon53.jpg) no-repeat 6px 50%;">新建主菜单</legend>
            <table cellspacing="0" cellPadding="4" class="tabledatagrid" width="80%">
                        <tr>
                            <td style="width:90px;height:35px;">子菜单名称:<input type="hidden" id="submenuid" name="submenuid" value="0" /></td>
                            <td><input id="menutitle"  name="menutitle" type="text" maxlength="8" size="10"class="FormBase" onfocus="this.className='FormFocus';" onblur="this.className='FormBase';" /></td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" style="height:35px;">
                                <button type="button" class="ManagerButton" id="AddNewRec" onclick="chkSubmit();"><img src="../images/add.gif"/> 提 交 </button>&nbsp;&nbsp;
                                <button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworeditsubmenu');"><img src="../images/state1.gif"/> 取 消 </button>
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
