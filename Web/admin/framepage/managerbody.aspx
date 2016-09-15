<%@ Page Language="C#" Inherits="BBX.Web.Admin.managerbody" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>ASP.net|论坛 管理后台 - <%=BBX.Common.Utils.ProductName%> - Powered by <%=BBX.Common.Utils.ProductName%></title>
    <meta name="keywords" content="ASP.net,论坛" />
    <meta name="description" content="<%=BBX.Common.Utils.ProductName%>,论坛,asp.net" />
    <link href="../styles/dntmanager.css" rel="stylesheet" type="text/css" />
    <link href="../styles/modelpopup.css" rel="stylesheet" type="text/css" />
    <link href="../styles/nav.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
    <script type="text/javascript" id="jsfile" src="../xml/navmenu.js"></script>
    <script type="text/javascript">var imgpath = '../images/'; var expandall = false;</script>
    <script type="text/javascript" src="../js/Navbar.js"></script>
    <script type="text/javascript" src="../../javascript/common.js"></script>
    <script type="text/javascript" src="../js/AjaxHelper.js"></script>
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript">
        function window_load() { loadMainFrame(); }
        function getParam(paramName) {
            var paramList = location.search.replace("?", "").split("&");
            for (var i = 0 ; i < paramList.length ; i++) {
                if (paramList[i].split("=")[0] == paramName)
                    return paramList[i].substring(paramList[i].indexOf("=") + 1, paramList[i].length);
            }
            return "";
        }

        function resizediv_onClick() {
            if ($("resizediv").className == "collapse") {
                top.document.getElementsByTagName('FRAMESET')[1].cols = "7,*";
                $("resizediv").className = "expand";
            }
            else {
                top.document.getElementsByTagName('FRAMESET')[1].cols = "150,*";
                $("resizediv").className = "collapse";
            }

        }

        function SetMenuItemFocus(obj) {
            menuitemcollection = document.getElementsByName("menuitem")
            for (i = 0; i < menuitemcollection.length; i++) {
                menuitemcollection[i].className = "";
            }
            obj.className = "currentitem";
        }

        var toptabmenuid = getParam("toptabmenuid");
        if (toptabmenuid != "") {
            document.write("<body class=\"NtTab" + (toptabmenuid % 7) + "\" onload=\"window_load();\">");
        }
        else {
            document.write("<body class=\"NtTab1\" onload=\"window_load();\">");
        }

        //常用功能显示标识
        var shortcutflag = 0;

        function setshorcutmenu(setoption) {
            if (setoption == 'block')
                shortcutflag++;
            else
                shortcutflag--;

            //当setoption为none且shortcutflag<=0时才能关闭shortcutmenu
            if (setoption == 'block' || (setoption == 'none' && shortcutflag <= 0))
                $('shortcutmenu').style.display = setoption;
        }

        function LoadJs() {
            var e = document.getElementById("jsfile"); if (e) e.parentNode.removeChild(e);
            var script = document.createElement("SCRIPT"); script.defer = true;
            script.type = "text/javascript"; script.src = "../xml/navmenu.js?r=" + Math.random(); script.id = "jsfile";
            document.getElementsByTagName("HEAD")[0].appendChild(script);
        }

        function ShortcutMenuContent() {
            var shortcutmenustr = "";
            for (var i = 0 ; i < shortcut.length ; i++) {
                shortcutmenustr += "<dt><a href='#' onclick=\"resetindexmenu('" + shortcut[i]["showmenuid"] + "','";
                shortcutmenustr += shortcut[i]["toptabmenuid"] + "','" + shortcut[i]["mainmenulist"];
                shortcutmenustr += "','" + shortcut[i]["link"] + "');\">";
                shortcutmenustr += shortcut[i]["menutitle"] + "</a></dt>";
            }
            if (shortcutmenustr != "")
                shortcutmenustr += "<hr class='line' />";
            shortcutmenustr += '<dd><a href="javascript:void(0);" onclick="javascript:top.location.href=\'../../index.aspx\';" onfocus="this.blur();">返回前台</a></dd>';
            shortcutmenustr += '<dd><a href="javascript:void(0);" onclick="javascript:document.getElementById(\'main\').src=\'../rapidset/shortcut.aspx\';" onfocus="this.blur();" >常用操作</a></dd>';
            shortcutmenustr += '<dd><a href="javascript:void(0);" onclick="javascript:document.getElementById(\'main\').src=\'../rapidset/systeminf.aspx\';" onfocus="this.blur();" >系统信息</a></dd>';
            shortcutmenustr += '<dd><a href="javascript:void(0);" onclick="javascript:document.getElementById(\'main\').src=\'../rapidset/setting.aspx\';" onfocus="this.blur();" >快速设置向导</a></dd>';
            shortcutmenustr += '<dd><a href="javascript:void(0);" onclick="javascript:document.getElementById(\'main\').src=\'../rapidset/likesetting.aspx\';" onfocus="this.blur();" >个人喜好设置</a></dd>';
            shortcutmenustr += '<dd><a href="javascript:void(0);" onclick="javascript:document.getElementById(\'main\').src=\'../rapidset/managemainmenu.aspx\';" onfocus="this.blur();" >管理功能菜单</a></dd>';
            shortcutmenustr += '<dd><a href="javascript:void(0);" onclick="javascript:document.getElementById(\'main\').src=\'../rapidset/manageshortcutmenu.aspx\';" onfocus="this.blur();" >管理快捷菜单</a></dd>';
            shortcutmenustr += '<dd><a href="javascript:void(0);" onclick="javascript:document.getElementById(\'main\').src=\'../rapidset/onlineupgrade.aspx\';" onfocus="this.blur();" >在线升级</a></dd>';
            shortcutmenustr += '<dd><a href="#" target="_blank" onfocus="this.blur();">帮助</a></dd>';
            shortcutmenustr += '<dd><a href="javascript:void(0);" onclick="javascript:top.location.href=\'../logout.aspx\';" onfocus="this.blur();" >退出</a></dd>';
            $("shortcutmenucontent").innerHTML = shortcutmenustr;
        }

        function LoadShortcutMenu() {
            LoadJs();
            setTimeout("ShortcutMenuContent()", 3000);
        }

        function Search(searchinf, searchtype) {
            $('searchresult').innerHTML = '<table width=\"260\" height=\"75\"><tr><td><img border=\"0\" src=\"../images/ajax_loading.gif\" /></td><td valign=middle>正在搜索数据, 请稍候......</td></tr></table>';
            AjaxProxyUrl = new String("../ajax.aspx");
            switch (searchtype) {
                case 'function':
                    {
                        AjaxHelper.Updater('../UserControls/searchfunction', 'searchresult', 'searchinf=' + searchinf); break
                    }
                case 'user':
                    {
                        AjaxHelper.Updater('../UserControls/searchuser', 'searchresult', 'searchinf=' + searchinf); break;
                    }
            }
            $("titlebar").innerHTML = "搜索结果";
            BOX_show('PopUpModel');
            window.parent.frames[0].BOX_show('setting');
        }

        function resetindexmenu(showmenu, toptabmenuid, mainmenulist, link) {
            //window.parent.frames[0].BOX_remove('setting');
            top.topFrame.locationurl(showmenu, toptabmenuid, mainmenulist, link);
        }

        function hidemodelbox(boxid) {
            BOX_remove('setting');
            window.parent.frames[0].BOX_remove('setting');
        }
    </script>
    <style type="text/css">
        .collapse {
            background-position: center center;
            background-image: url(images/collapse.gif);
            width: 6px;
            background-repeat: no-repeat;
            position: absolute;
            height: 50px;
            background-color: aliceblue;
        }

        .expand {
            background-position: center center;
            background-image: url(images/expand.gif);
            width: 6px;
            background-repeat: no-repeat;
            position: absolute;
            height: 50px;
            background-color: aliceblue;
        }

        .pluginitemhidd {
            display: none;
        }

        .pluginitemshow {
            display: block;
        }

        body {
            margin: 0;
        }
    </style>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <div class="NavBody">
        <form id="Form1" method="post">
            <div id="ManagerBodyMenu">
                <div class="InnerBodyMenu">
                    <!--菜单开始-->
                    <div class="NavManagerMenu" id="NavManagerMenu"></div>
                    <!--菜单结束-->
                </div>
            </div>
            <!--显示快捷菜单-->
            <div id="shortcutmenu" class="DropWindow" style="position: absolute; display: none; top: 0px; left: 0px;" onmouseout="setshorcutmenu('none')" onmouseover="setshorcutmenu('block')">
                <dl id="shortcutmenucontent">
                </dl>
            </div>
            <!--显示快捷菜单-->

            <!--显示搜索选项层-->
            <div id="searchoption" class="popupmenu-new" style="position: absolute; display: none; top: -8px; right: 154px; width: 50px;" onmouseover="setseachmenu('block')" onmouseout="setseachmenu('none')">
                <div id="popupmenuitemlast" class="popupmenu-item-last" style="padding: 0px;">
                    <div id="popupmenuitem" class="popupmenu-item" style="padding: 3.5px 0px 4px 0px;">
                        <div>&nbsp;&nbsp;<a href="javascript:void(0);" onclick="javascript:top.topFrame.setseachtype('function');setseachmenu('none');" onfocus="this.blur();">功能</a></div>
                        <div>&nbsp;&nbsp;<a href="javascript:void(0);" onclick="javascript:top.topFrame.setseachtype('user');setseachmenu('none');" onfocus="this.blur();">用户</a></div>
                    </div>
                </div>
            </div>

            <script type="text/javascript">
                function setseachmenu(setoption) {
                    $('searchoption').style.display = setoption;
                }
                cancelbubble($('searchoption'));
            </script>
            <!--显示搜索选项层-->


            <!--显示搜索结果-->
            <div id="setting" style="display: none;">
                <div id="PopUpModel" style="display: none;" class="PopUpModel">
                    <div class="ctrl_title"><a href="javascript:void(0);" onclick="hidemodelbox('setting');">
                        <img src="../images/close.gif" border="0" /></a><span id="titlebar"></span></div>
                    <div id="searchresult"></div>
                </div>
            </div>
            <!--显示搜索结果-->

        </form>
        <div id="ManagerBody">
            <div class="InnerBody">
                <div id="NavManager">
                    <iframe id="main" frameborder="0" scrolling="auto" src="about:blank" width="100%" height="600px" style="z-index: 2;"></iframe>
                    <script type="text/javascript">
                        function loadMainFrame() {
                            if (getParam("defaulturl") != "")
                                $("main").src = "../" + getParam("defaulturl");
                        }
                    </script>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function setscreendiv() {
            var clientHeight = 768;
            var bo = $('NavManagerMenu');
            var iframe = $('main');
            if (navigator.userAgent.toLowerCase().indexOf('opera') != -1) {
                clientHeight = document.documentElement.clientHeight + 190;
            }
            else {
                clientHeight = document.documentElement.clientHeight - 25;
            }

            if (navigator.userAgent.toLowerCase().indexOf('msie') != -1) {
                bo.style.height = clientHeight + 'px';
            }
            else {
                bo.style.minHeight = clientHeight + 'px';
            }

            var clientHeight = 768;

            if (navigator.userAgent.toLowerCase().indexOf('opera') != -1) {
                clientHeight = document.documentElement.clientHeight + 186;
            }
            else {
                clientHeight = document.documentElement.clientHeight - 27;
            }
            iframe.style.height = clientHeight + 'px';

            var navmanagermenu = $('NavManagerMenu');
            navmanagermenu.style.height = clientHeight + 2 + 'px';
            navmanagermenu.style.minHeight = clientHeight + 2 + 'px';

            window.onresize = function () { setscreendiv(); }
            window.onscroll = function () { setscreendiv(); }
        }

        function LoadMenuItem() {
            var menuText = "<ul>";
            for (var i = 0 ; i < toptabmenu.length ; i++) {
                var mainMenu = toptabmenu[i]["mainmenuidlist"].split(",");
                for (var j = 0 ; j < mainMenu.length ; j++) {
                    for (var k = 0 ; k < mainmenu.length ; k++) {
                        if (mainmenu[k]["menuid"] == mainMenu[j]) {
                            //增加菜单大项
                            menuText += createMainMenu(mainmenu[k]["id"], mainmenu[k]["menutitle"], ("," + toptabmenu[i]["mainmenulist"] + ",").indexOf("," + getParam("showmenuid") + ",") == -1);
                            //增加菜单子项
                            menuText += createSubMenu(mainmenu[k]["menuid"], mainmenu[k]["id"], getParam("showmenuid") != mainmenu[k]["id"]);
                            break;
                        }
                    }
                }
            }
            menuText += "</ul>";
            $("NavManagerMenu").innerHTML = menuText;
        }

        function createMainMenu(mId, menuText, isHidden) {
            var mainMenuItem = "";
            mainMenuItem += '<li Class="CurrentItem"' + (isHidden ? " style='display:none'" : "") + ' forid="' + mId + '">';
            mainMenuItem += '<div class="current" onmousedown="gomenu(event)">';
            mainMenuItem += '<cite>&nbsp;&nbsp;&nbsp;<a href="#" style="font-weight:bold;" onfocus="this.blur();">' + menuText + '</a></cite>';
            mainMenuItem += '<span class="title" id="top" ><img src="../images/dropdown.gif" class="arrow" style="z-Index:-1;"/></span>';
            mainMenuItem += '</div></li>';
            return mainMenuItem;
        }

        function createSubMenu(mainMenuId, parentId, isHidden) {
            var subMenuItem = "";
            subMenuItem += '<li class="Submenu"><div class="Submenu1"><table><tbody>';
            for (var i = 0 ; i < submenu.length ; i++) {
                if (submenu[i]["menuparentid"] == mainMenuId) {
                    subMenuItem += '<tr><td>';
                    subMenuItem += '<a id="menuitem' + (parentId - 1) + '" name="menuitem" href="javascript:void(0);"';
                    if (submenu[i]["link"] == getParam("defaulturl"))
                        subMenuItem += " class='currentitem'";
                    subMenuItem += ' onclick="javascript:document.getElementById(\'' + submenu[i]["frameid"] + '\').src=\'../' + submenu[i]["link"] + '\';';
                    subMenuItem += 'SetMenuItemFocus(this);"  onfocus="this.blur();">' + submenu[i]["menutitle"] + '</a>';
                    subMenuItem += '</tr></td>';
                }
            }
            subMenuItem += '</tbody></table></div></li>';
            return subMenuItem;
        }

        function resetEscAndF5(e) {
            e = e ? e : window.event;
            actualCode = e.keyCode ? e.keyCode : e.charCode;
            if (actualCode == 27) {
                if ($('setting').style.display == 'none') {
                    showNavigation();
                }
                else {
                    hideNavigation();
                }
            }
            if (actualCode == 116 && parent.main) {
                parent.main.location.reload();
                if (document.all) {
                    e.keyCode = 0;
                    e.returnValue = false;
                }
                else {
                    e.cancelBubble = true;
                    e.preventDefault();
                }
            }
        }

        function _attachEvent(obj, evt, func) {
            if (obj.addEventListener) {
                obj.addEventListener(evt, func, false);
            }
            else if (obj.attachEvent) {
                obj.attachEvent("on" + evt, func);
            }
        }

        function showSubMenu(id) {
            showMenu(id, false);
            $(id + "_menu").style.position = "absolute";
            $(id + "_menu").style.top = "0px";
            $(id + "_menu").style.left = (parseInt($(id + "_menu").style.left.replace("px", "")) - 290) + "px";
        }

        function gotoURL(mainmenuid, toptabmenuid, mainmenulist, url) {
            window.parent.frames[0].BOX_remove('setting');
            resetindexmenu(mainmenuid, toptabmenuid, mainmenulist, url);
        }

        function gotoShortcut() {
            BOX_remove('setting');
            window.parent.frames[0].BOX_remove('setting');
            document.getElementById("main").src = "../rapidset/manageshortcutmenu.aspx";
        }

        function getElementsByClass(node, searchClass, tag) {
            var classElements = new Array();
            var els = node.getElementsByTagName(tag);
            var elsLen = els.length;
            var pattern = new RegExp("\\b" + searchClass + "\\b");
            for (i = 0, j = 0; i < elsLen; i++) {
                if (pattern.test(els[i].className)) {
                    classElements[j] = els[i];
                    j++;
                }
            }
            return classElements;
        }

        function showPluginMenu(menuid) {
            var el = getElementsByClass(document, 'pluginitemshow', 'li');
            for (var i = 0 ; i < el.length ; i++)
                el[i].className = "pluginitemhidd";
            $("item" + menuid).className = "pluginitemshow";
        }

        function showNavigation() {
            $("titlebar").innerHTML = "导航菜单";
            var menutext = "<table width='100%'><tr><td align='left' valign='top' width='83%'>";
            menutext += "<table width='100%'><tr><td colspan='" + toptabmenu.length + "'>&nbsp;&nbsp;<img src='../images/navigation.gif' style='vertical-align: middle;'>&nbsp;按 “ ESC ” 键展开 / 关闭此菜单</td></tr><tr>";
            for (var i = 0 ; i < toptabmenu.length ; i++) {
                menutext += "<td valign='top'><h2 style='text-indent:3px;'>" + toptabmenu[i]["title"] + "</h2><dl>";
                var isplugin = toptabmenu[i]["system"] == "2" ? true : false;
                for (var j = 0; j < mainmenu.length ; j++) {
                    if (("," + toptabmenu[i]["mainmenuidlist"] + ",").indexOf("," + mainmenu[j]["menuid"] + ",") != -1) {
                        if (!isplugin) {
                            menutext += "<dt>" + mainmenu[j]["menutitle"] + "</dt>";
                            menutext += "<dd><ul style='margin-left:2px;'>";
                        }
                        else {
                            menutext += "<li style='text-indent:3px;cursor:pointer' onclick='showPluginMenu(" + j + ");'>" + mainmenu[j]["menutitle"] + "</li>";
                            menutext += "<li id='item" + j + "' class='pluginitemhidd'><ul style='margin-left:12px;'>";
                        }
                        for (var k = 0 ; k < submenu.length ; k++) {
                            if (mainmenu[j]["menuid"] == submenu[k]["menuparentid"]) {
                                menutext += "<li>&rsaquo;<a href='#' onclick='gotoURL(\"" + mainmenu[j]["id"] + "\",\"" + toptabmenu[i]["id"] + "\",\"" + toptabmenu[i]["mainmenulist"] + "\",\"" + submenu[k]["link"] + "\")'>" + submenu[k]["menutitle"] + "</a></li>";
                            }
                        }
                        menutext += "</ul></dd>";
                    }
                }
                menutext += "</dl></td>";
            }
            menutext += "</tr></table></td><td width='17%' align='left' valign='top'>";
            menutext += "<table width='100%'><tr><td align='center'><img src='../images/favorite.gif' style='vertical-align: middle;'>&nbsp;<b>快捷菜单</b>(" + shortcut.length + "/15)[<a href='#' onclick='gotoShortcut();'>管理</a>]</td></tr><tr><td align='center'><ul>";
            if (shortcut.length == 0) {
                menutext += "<li>暂无收藏</li>";
            }
            else {
                for (var l = 0 ; l < shortcut.length ; l++) {
                    menutext += "<li><a href='#' onclick='gotoURL(\"" + shortcut[l]["showmenuid"] + "\",\"" + shortcut[l]["toptabmenuid"] + "\",\"" + shortcut[l]["mainmenulist"] + "\",\"" + shortcut[l]["link"] + "\");'>" + shortcut[l]["menutitle"] + "</a></li>";
                }
            }
            menutext += "</ul></td></tr></table>";
            menutext += "</td></tr></table>";
            $("searchresult").innerHTML = menutext;
            BOX_show('PopUpModel');
            window.parent.frames[0].BOX_show('setting');
        }

        function hideNavigation() {
            BOX_remove('setting');
            window.parent.frames[0].BOX_remove('setting');
        }

        setscreendiv();

        window.onresize = function () { setscreendiv(); }
        window.onscroll = function () { setscreendiv(); }

        var mainmenulist = getParam("mainmenulist");
        var showmenuid = getParam("showmenuid");

        if (mainmenulist == "") {
            mainmenulist = '1,2';
            showmenuid = '1';
        }

        LoadMenuItem();
        window.onload = function () { init(showmenuid, mainmenulist); }
        LoadShortcutMenu();
        setscreendiv();
        _attachEvent(document.documentElement, 'keydown', resetEscAndF5);
    </script>
</body>
</html>
