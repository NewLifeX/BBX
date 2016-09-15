<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="pageinfo.ascx.cs" Inherits="BBX.Web.Admin.pageinfo" %>
<div id="<%=this.ID %>" style="clear: both; border: 1px dotted #DBDDD3; padding: 15px 10px 10px 56px; background: #FDFFF2 url(../images/hint.gif) no-repeat 20px 15px; margin: 10px 0; ">
    <span class="infomessage">
        <a onclick="document.getElementById('<%=this.ID %>').style.display='none';" href="javascript:void(0);"><img alt="关闭" src="../images/off.gif"/></a>
    </span>
    <%=this.Text %>
</div>
<div style="clear:both;"></div>