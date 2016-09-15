<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="shortcutmenu.ascx.cs" Inherits="BBX.Web.Admin.shortcutmenu" %>
<dl>
    <%=shortcutmenustr%>
    <dd><a href="javascript:void(0);" onclick="javascript:top.location.href='../../index.aspx';" onfocus="this.blur();">返回前台</a></dd>
    <dd><a href="javascript:void(0);" onclick="javascript:document.getElementById('main').src='../rapidset/shortcut.aspx';" onfocus="this.blur();" >常用操作</a></dd>
    <dd><a href="javascript:void(0);" onclick="javascript:document.getElementById('main').src='../rapidset/systeminf.aspx';" onfocus="this.blur();" >系统信息</a></dd>
    <dd><a href="javascript:void(0);" onclick="javascript:document.getElementById('main').src='../rapidset/setting.aspx';" onfocus="this.blur();" >快速设置向导</a></dd>
    <dd><a href="javascript:void(0);" onclick="javascript:document.getElementById('main').src='../rapidset/likesetting.aspx';" onfocus="this.blur();" >个人喜好设置</a></dd>
    <dd><a href="javascript:void(0);" onclick="javascript:document.getElementById('main').src='../rapidset/managemainmenu.aspx';" onfocus="this.blur();" >管理功能菜单</a></dd>
    <dd><a href="javascript:void(0);" onclick="javascript:document.getElementById('main').src='../rapidset/manageshortcutmenu.aspx';" onfocus="this.blur();" >管理快捷菜单</a></dd>
    <dd><a href="#" target="_blank" onfocus="this.blur();">帮助</a></dd>    
    <dd><a href="javascript:void(0);" onclick="javascript:top.location.href='../logout.aspx';" onfocus="this.blur();" >退出</a></dd>
</dl>