<%@ Page Language="c#" Inherits="BBX.Web.Admin.givemedals" Codebehind="global_givemedals.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>givemedals</title>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
    <script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <div class="ManagerForm">
            <fieldset>
                <legend style="background: url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">授予/收回勋章</legend>
                <table class="ntcplist">
                <tr class="head">
                <td>当前用户:<asp:Literal ID="givenusername" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                <td>
                <table class="datalist" id="DataGrid1" style="border-collapse: collapse;" align="center" border="1" cellspacing="0" rules="all">
                    <tr class="category">
                        <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap" width="40%">勋章图片</td>
                        <td nowrap="nowrap" style="border: 1px solid rgb(234, 233, 225);" width="40%">名称</td>
                        <td nowrap="nowrap" style="border: 1px solid rgb(234, 233, 225);" width="20%"><input title="选中/取消该勋章" onclick="CheckAll(this.form)" type="checkbox" name="chkall" id="chkall" /></td>
                    </tr>
                    <asp:Repeater ID="medallist" runat="server">
                        <ItemTemplate>
                            <tr class="mouseoutstyle" onmouseover="this.className='mouseoverstyle'" onmouseout="this.className='mouseoutstyle'">
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap"><img src="../../images/medals/<%# Eval("image").ToString()%>" height="25px" /></td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap"><%# Eval("name").ToString()%></td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap"><%# BeGivenMedal(Eval("isgiven").ToString(),Eval("medalid").ToString())%></td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="mouseoutstyle" onmouseover="this.className='mouseoverstyle'" onmouseout="this.className='mouseoutstyle'">
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap"><img src="../../images/medals/<%# Eval("image").ToString()%>" height="25px" /></td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap"><%# Eval("name").ToString()%></td>
                                <td style="border: 1px solid rgb(234, 233, 225);" nowrap="nowrap"><%# BeGivenMedal(Eval("isgiven").ToString(),Eval("medalid").ToString())%></td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>
                </td>
                </tr>
                </table>
                
                <table width="100%">
                <tr>
                <td class="panelbox">
                <table cellspacing="0" cellpadding="4" width="100%" align="center" border="0">
                    <tr>
                        <td style="width: 130px">授予/收回勋章的理由:</td>
                        <td>
                            <cc1:TextBox ID="reason" runat="server" HintTitle="提示" HintInfo="如果您修改了用户的勋章资料, 请输入操作理由, 系统将把理由记录在勋章授予记录中, 以供日后查看"
                                TextMode="MultiLine" RequiredFieldType="暂无校验" Width="80%" Height="112px"></cc1:TextBox>
                        </td>
                    </tr>
                </table>
                </td>
                </tr>
                </table>
            </fieldset>
            <cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
            <div class="Navbutton">
                <cc1:Button ID="GivenMedal" runat="server" Text="提 交"></cc1:Button>&nbsp;&nbsp;
                <button type="button" class="ManagerButton" id="Button3" onclick="window.history.back();"><img src="../images/arrow_undo.gif"/> 返 回 </button>
            </div>
        </div>
    </form>
    <%=footer%>
</body>
</html>
