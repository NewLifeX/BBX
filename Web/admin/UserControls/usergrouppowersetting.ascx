<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="usergrouppowersetting.ascx.cs" Inherits="BBX.Web.Admin.usergrouppowersetting" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<script type="text/javascript">
    function bonusPriceSet(checked)
    {
        document.getElementById("minbonusprice").disabled = !checked;
        document.getElementById("maxbonusprice").disabled = !checked;
    }
    function validatebonusprice()
    {
        if(document.getElementById("<%=usergroupright.ClientID%>_16").checked)
        {
            var min = document.getElementById("minbonusprice").value;
            var max = document.getElementById("maxbonusprice").value;
            if(!isNumber(min))
            {
                resetPage();
                alert("最低悬赏价格非数字");
                document.getElementById("minbonusprice").focus();
                document.getElementById("minbonusprice").value = "";
                return false;
            }
            if(!isNumber(max))
            {
                resetPage();
                alert("最高悬赏价格非数字");
                document.getElementById("maxbonusprice").focus();
                document.getElementById("maxbonusprice").value = "";
                return false;
            }
            if(parseInt(min) < 1)
            {
                resetPage();
                alert("最低悬赏价格必须大于0");
                document.getElementById("minbonusprice").focus();
                document.getElementById("minbonusprice").value = "";
                return false;
            }
            if(parseInt(max) > 32767)
            {
                resetPage();
                alert("最高悬赏价格必须小于32767");
                document.getElementById("maxbonusprice").focus();
                document.getElementById("maxbonusprice").value = "";
                return false;
            }
            if (parseInt(max) <= parseInt(min))
            {
                resetPage();
                alert("最高悬赏价格必须高于最低悬赏价格");
                document.getElementById("minbonusprice").focus();
                return;
            }
        }
        else
        {
            document.getElementById("minbonusprice").value = 0;
            document.getElementById("maxbonusprice").value = 0;
        }
        return true;
    }
</script>
<table width="100%">
<tr><td class="item_title" colspan="2">操作理由短消息通知作者</td></tr>
<tr>
    <td class="vtop rowform">
        <cc1:RadioButtonList ID="reasonpm" runat="server" RepeatColumns="1" Width="96%" Font-Size="12px">
            <asp:ListItem Value="0" Selected>不强制</asp:ListItem>
            <asp:ListItem Value="1">强制输入理由</asp:ListItem>
            <asp:ListItem Value="2">强制通知作者</asp:ListItem>
            <asp:ListItem Value="3">强制输入理由和通知作者</asp:ListItem>
        </cc1:RadioButtonList>
    </td>
    <td class="vtop"></td>
</tr>
<tr><td class="item_title" colspan="2">是否允许搜索</td></tr>
<tr>
    <td class="vtop rowform">
        <cc1:RadioButtonList ID="allowsearch" runat="server" RepeatColumns="1" Width="96%" Font-Size="12px">
            <asp:ListItem Value="0" Selected>不允许</asp:ListItem>
            <asp:ListItem Value="1">允许搜索标题或全文</asp:ListItem>
            <asp:ListItem Value="2">仅允许搜索标题</asp:ListItem>
        </cc1:RadioButtonList>
    </td>
    <td class="vtop"></td>
</tr>
<tr><td class="item_title" colspan="2">其它权限</td></tr>
<tr>
    <td class="vtop" colspan="2">
        <cc1:CheckBoxList ID="usergroupright" runat="server" RepeatColumns="3" Font-Size="12px">
            <asp:ListItem Value="allowvisit">是否允许访问论坛</asp:ListItem>
            <asp:ListItem Value="allowpost">是否允许发帖</asp:ListItem>
            <asp:ListItem Value="allowreply">是否允许回复</asp:ListItem>
            <asp:ListItem Value="allowpostpoll">是否允许发起投票</asp:ListItem>
            <asp:ListItem Value="allowvote">是否允许参与投票</asp:ListItem>
            <asp:ListItem Value="allowpostattach">是否发布附件</asp:ListItem>
            <asp:ListItem Value="allowgetattach">是否允许下载附件</asp:ListItem>
            <asp:ListItem Value="allowsetreadperm">是否允许设置主题阅读积分权限</asp:ListItem>
            <asp:ListItem Value="allowsetattachperm">是否允许设置附件阅读积分限制</asp:ListItem>
            <asp:ListItem Value="allowhidecode">是否允许使用hide代码</asp:ListItem>
            <asp:ListItem Value="allowcusbbcode">是否允许使用UBB代码</asp:ListItem>
            <asp:ListItem Value="allowsigbbcode">签名是否支持UBB代码</asp:ListItem>
            <asp:ListItem Value="allowsigimgcode">签名是否支持图片代码</asp:ListItem>
            <asp:ListItem Value="allowviewpro">是否允许查看用户资料</asp:ListItem>
            <asp:ListItem Value="disableperiodctrl">是否不受时间段限制</asp:ListItem>
            <asp:ListItem Value="allowdebate">是否允许辩论</asp:ListItem>
            <asp:ListItem Value="allowbonus">是否允许悬赏</asp:ListItem>
            <asp:ListItem Value="allowviewstats">是否允许查看统计数据</asp:ListItem>
            <asp:ListItem Value="allowdiggs">是否允许辩论支持</asp:ListItem>
            <asp:ListItem Value="allowhtmltitle">是否允许HTML标题</asp:ListItem>
            <asp:ListItem Value="allowhtml">是否允许HTML</asp:ListItem>
            <asp:ListItem Value="modnewtopics">发主题是否需要审核</asp:ListItem>
            <asp:ListItem Value="modnewposts">发回复是否需要审核</asp:ListItem>
            <asp:ListItem Value="ignoreseccode">是否允许忽略检测验证码</asp:ListItem>
            <asp:ListItem Value="allowtrad">是否允许交易</asp:ListItem>
        </cc1:CheckBoxList>
        <asp:Literal ID="outscript" runat="server" />
    </td>
</tr>
</table>