<%@ Page Language="C#" AutoEventWireup="true" Inherits="BBX.Web.Admin.top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理后台 - <%=BBX.Common.Utils.ProductName%> - Powered by <%=BBX.Common.Utils.ProductName%></title>
    <meta name="keywords" content="ASP.net,论坛" />
    <meta name="description" content="<%=BBX.Common.Utils.ProductName%>,论坛,asp.net" />
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
    <script type="text/javascript" src="../xml/navmenu.js?t=<%=DateTime.Now.Ticks%>"></script>
    <script type="text/javascript">
        function switchshortcutmenu(e) {
            setTimeout(function on() { top.mainFrame.setshorcutmenu(e); }, 200);
        }
    </script>
    <style type="text/css">
        body {
            margin: 0;
        }
    </style>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <div id="MenuTab">
        <ul id="MainMenu">
            <li class="MenuDrop">
                <div class="MenuDropOut" onmouseover="this.className='MenuDropOn'" onmouseout="this.className='MenuDropOut'">
                    <a href="javascript:void(0);" onmouseover="switchshortcutmenu('block');" onmouseout="switchshortcutmenu('none');" style="font-size: 14px; font-weight: 700;">常用功能</a>
                </div>
            </li>
        </ul>
        <div class="SearchBar">
            <input type="hidden" id="searchtype" value="function" />
            <dl>
                <dt><span id="searcheoption" onclick="javascript:top.mainFrame.setseachmenu('block');" style="cursor: hand">功能</span>
                    <button type="button" class="OutButton" onmouseover="this.className='OverButton'" onmouseout="this.className='OutButton'" onclick="javascript:top.mainFrame.setseachmenu('block');" onfocus="this.blur();"></button>
                </dt>
                <dd>
                    <input id="searchinfo" onkeydown="if(event.keyCode==13){document.getElementById('dosearch').focus();}" type="text" /></dd>
                <dd>
                    <button type="button" id="dosearch" class="SubmitButton" alt="点击搜索" onclick="javascript:top.mainFrame.Search(document.getElementById('searchinfo').value,document.getElementById('searchtype').value);"></button>
                </dd>
            </dl>
        </div>
    </div>
    <div id="setting" style="display: none;">
    </div>
    <script type="text/javascript">

        function LoadMainMenu() {
            var menuText = "";
            for (var i = 0; i < toptabmenu.length; i++) {
                var mod = (i % 7) + 1;
                menuText += "<li id='NtTab" + (i + 1) + "' ><div id='NtDiv" + (i + 1) + "'  Class='Currenttab" + mod + "'>";
                if (toptabmenu[i]["defaulturl"].indexOf("http://") == 0) {
                    menuText += "<a id='NtA" + (i + 1) + "' href='" + toptabmenu[i]["defaulturl"] + "' target='_blank' class='CurrentHoverTab" + mod + "' onfocus='this.blur();'>" + toptabmenu[i]["title"] + "</a>";
                }
                else {
                    menuText += "<a href='#' id='NtA" + (i + 1) + "' class='CurrentHoverTab" + mod + "'";
                    menuText += " onclick=\"javscript:locationurl('" + toptabmenu[i]["mainmenulist"].split(",")[0] + "','" + toptabmenu[i]["id"] + "','" + toptabmenu[i]["mainmenulist"];
                    menuText += "','" + toptabmenu[i]["defaulturl"] + "');\" onfocus='this.blur();'>" + toptabmenu[i]["title"] + "</a>";
                }
                menuText += "</div></li>";
            }
            document.getElementById("MainMenu").innerHTML += menuText;
        }
        LoadMainMenu();

        function setseachtype(searchtype) {
            document.getElementById("searchtype").value = searchtype;
            switch (searchtype) {
                case 'function': document.getElementById("searcheoption").innerHTML = '功能'; break;
                case 'user': document.getElementById("searcheoption").innerHTML = '用户'; break;
            }
        }

        function locationurl(showmenuid, toptabmenuid, mainmenulist, defaulturl) {
            var menucount = toptabmenu.length;
            j = menucount;

            /*得新进行实始化设置*/
            for (i = 1; i <= menucount; i++) {
                document.getElementById("NtDiv" + i).className = "tab" + (i % 7);
                document.getElementById("NtTab" + i).style.zIndex = j;
                document.getElementById("NtA" + i).className = "";
                j--;
            }


            /*设置当前点中的菜单项样式*/
            document.getElementById("NtA" + toptabmenuid).className = 'CurrentHoverTab' + (toptabmenuid % 7);
            document.getElementById('NtDiv' + toptabmenuid).className = 'Currenttab' + (toptabmenuid % 7);
            document.getElementById('NtTab' + toptabmenuid).style.zIndex = menucount + 1;

            //alert(defaulturl);
            top.mainFrame.location.href = '../framepage/managerbody.aspx?showmenuid=' + showmenuid + '&toptabmenuid=' + toptabmenuid + '&mainmenulist=' + mainmenulist + '&defaulturl=' + defaulturl;

        }

        document.getElementById("searchtype").value = 'function';
        var pagename = "<%=pagename %>";
        var targeturl = false;

        if (pagename != "") {
            var subitem;
            var menuitem;
            var toptabitem;

            try {
                for (i = 0; i < submenu.length; i++) {
                    if (pagename == submenu[i].link) {
                        subitem = submenu[i];
                        break;
                    }
                }

                for (i = 0; i < mainmenu.length; i++) {
                    if (subitem.menuparentid == mainmenu[i].menuid) {
                        menuitem = mainmenu[i];
                        break;
                    }
                }

                for (i = 0; i < toptabmenu.length; i++) {
                    if (toptabmenu[i].mainmenuidlist.indexOf(menuitem.menuid) > -1) {
                        toptabitem = toptabmenu[i];
                        break;
                    }
                }
                if (toptabitem != undefined)
                    targeturl = true;
            } catch (e) {

            }
        }

        if (!targeturl) {
            locationurl(toptabmenu[0]["mainmenulist"].split(",")[0], toptabmenu[0]["id"], toptabmenu[0]["mainmenulist"], toptabmenu[0]["defaulturl"]);
        } else {
            locationurl(menuitem.id, toptabitem.id, toptabitem.mainmenulist, pagename);
        }
    </script>
</body>
</html>
