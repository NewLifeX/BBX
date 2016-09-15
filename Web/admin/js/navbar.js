var menu, titles, submenus, arrows, bypixels; //定义指定的菜单数组变量
var heights = new Array();
var speed = 25;  //加载菜单项的速度  
var n = navigator.userAgent;

//if (/Opera/.test(n)) {
//    bypixels = 2;
//}
//else if (/Firefox/.test(n)) {
//    bypixels = 3;
//}
//else if (/MSIE/.test(n)) {
//    bypixels = 2;
//}

if (/Firefox/.test(n)) {
    bypixels = 3;
}
else {
    bypixels = 2;
}


//展开所有的菜单项
function slash_expandall() {
    if (typeof menu != "undefined") {
        for (i = 0; i < Math.max(titles.length, submenus.length); i++) {
            titles[i].className = "CurrentItem";
            arrows[i].src = imgpath + "dropdown.gif";
            submenus[i].style.display = "";
            submenus[i].style.height = heights[i] + "px";
        }
    }
}

//收缩所有菜单项
function slash_contractall() {
    if (typeof menu != "undefined") {
        if (expandall == false) {
            for (i = 0; i < Math.max(titles.length, submenus.length); i++) {
                currents[i].className = "NoSelect";
                arrows[i].src = imgpath + "dropdown.gif";
                submenus[i].style.display = "none";
                submenus[i].style.height = 0 + "px";
                submenusli[i].style.display = "none";
            }
        }
    }
}

//初始化函数
function init(showmenuid, showmenuidlist) {

    menu = getElementsByClassName("NavManagerMenu", "div", document)[0];
    titles = getElementsByClassName("CurrentItem", "li", menu);
    arrows = getElementsByClassName("arrow", "img", menu);
    submenus = getElementsByClassName("Submenu1", "div", menu);
    submenusli = getElementsByClassName("Submenu", "li", menu);
    currents = getElementsByClassName("current", "div", menu);

    for (i = 0; i < Math.max(titles.length, submenus.length); i++) {
        heights[i] = submenus[i].offsetHeight;
        submenus[i].style.height = submenus[i].offsetHeight + "px";

        submenus[i].style.display = "none";
        currents[i].style.display = "none";
        titles[i].style.display = "none";
        currents[i].className = "NoSelect";
        submenusli[i].style.display = "none";
        /*alert(i); */
    }

    //当存在初始菜单项id串参数时
    if ((showmenuid != null) && (showmenuid != "")) {
        i = 0;
        var menuidarray = showmenuidlist.split(',')
        for (i = 0; i < menuidarray.length; i++) {
            for (j = 0; j < titles.length; j++) {
                if (titles[j].getAttribute("forid") == menuidarray[i]) {
                    titles[j].style.display = "block";
                    if (menuidarray[i] != showmenuid) {
                        submenus[j].style.display = "none";
                        currents[j].style.display = "block";
                        arrows[j].src = imgpath + "dropdown.gif";
                        submenusli[j].style.display = "none";
                    }
                    else {
                        submenus[j].style.display = "block";
                        currents[j].style.display = "block";
                        arrows[j].src = imgpath + "dropup.gif";
                        currents[j].className = "current";
                        submenusli[j].style.display = "block";
                    }
                    break;
                }
            }
        }
    }
    window_load();
}

//存储菜单项状态
function restore() {
    if (getcookie("menu") != null) {
        var hidden = getcookie("menu").split(",");
        for (var i in hidden) {
            titles[hidden[i]].className = "CurrentItem";
            submenus[hidden[i]].style.height = "0px";
            submenus[hidden[i]].style.display = "none";
            arrows[hidden[i]].src = imgpath + "dropup.gif";
        }
    }
}


//定向到指定的菜单项进行相应操作
function gomenu(e) {
    e = e ? e : window.event;
    var ce = (e.target) ? e.target : e.srcElement;
    var sm;

    //找到当前菜单项在数组中的位置,用于下面显示或隐藏判断
    for (var i in currents) {
        if ((arrows[i] == ce) || (currents[i] == ce.parentNode) || (currents[i] == ce.parentNode.parentNode)) {
            sm = i;
            break;
        }
    }

    if (sm >= 0) {
        //当前菜单项是展示状态时
        if (parseInt(submenus[sm].style.height) > parseInt(heights[sm])) {
            hidemenu(sm);
        }
        else {
            //当前菜单项是展示状态时
            if (submenus[sm].style.display == "block") {
                hidemenu(sm);
            }
            else {
                //显示指定的菜单项
                slash_contractall();
                currents[sm].className = "current";
                showmenu(sm);
            }
        }
    }
}


//隐藏指定的菜单元素
function hidemenu(sm) {
    var nr = submenus[sm].getElementsByTagName("a").length * bypixels + speed;
    submenus[sm].style.height = (parseInt(submenus[sm].style.height) - nr) + "px";

    var to = setTimeout("hidemenu(" + sm + ")", 5);
    if (parseInt(submenus[sm].style.height) <= nr) {
        clearTimeout(to);
        submenus[sm].style.display = "none";
        submenus[sm].style.height = 0 + "px";
        arrows[sm].src = imgpath + "dropdown.gif";
        submenusli[sm].style.display = "none";
        currents[sm].className = "NoSelect";
    }
}

//显示指定的菜单元素
function showmenu(sm) {
    var nr = submenus[sm].getElementsByTagName("a").length * bypixels + speed;
    submenus[sm].style.display = "block";
    submenus[sm].style.height = (parseInt(submenus[sm].style.height) + nr) + "px";
    var to = setTimeout("showmenu(" + sm + ")", 5);
    if (parseInt(submenus[sm].style.height) > (parseInt(heights[sm]) - nr)) {
        clearTimeout(to);
        submenus[sm].style.height = heights[sm] + "px";
        arrows[sm].src = imgpath + "dropup.gif";
        submenusli[sm].style.display = "block";
    }
}

//保存菜单元素
function store() {
    var hidden = new Array();
    for (var i in titles) {
        if (titles[i].className == "CurrentItem") {
            hidden.push(i);
        }
    }
    putcookie("menu", hidden.join(","), 5);
}


//获取指定样式的元素
function getElementsByClassName(strClassName, strTagName, oElm) {
    var arrElements = (strTagName == "*" && document.all) ? document.all : oElm.getElementsByTagName(strTagName);
    var arrReturnElements = new Array();
    strClassName = strClassName.replace(/\-/g, "\\-");
    var oRegExp = new RegExp("(^|\\s)" + strClassName + "(\\s|$)");
    var oElement;
    for (var i = 0; i < arrElements.length; i++) {
        oElement = arrElements[i];
        if (oRegExp.test(oElement.className)) {
            arrReturnElements.push(oElement);
        }
    }
    return (arrReturnElements)
}

function getElements(strTagName, oElm) {
    var arrElements = (strTagName == "*" && document.all) ? document.all : oElm.getElementsByTagName(strTagName);
    return arrElements;
}

function putcookie(c_name, value, expiredays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + expiredays);
    document.cookie = c_name + "=" + escape(value) + ((expiredays == null) ? "" : ";expires=" + exdate);
}

function getcookie(c_name) {
    if (document.cookie.length > 0) {
        var c_start = document.cookie.indexOf(c_name + "=");
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1;
            var c_end = document.cookie.indexOf(";", c_start);
            if (c_end == -1)
                c_end = document.cookie.length;
            return unescape(document.cookie.substring(c_start, c_end));
        }
    }
    return null;
}

