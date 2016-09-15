var PasswordStrength = {
    Level: ["极佳", "一般", "较弱", "太短"],
    LevelValue: [15, 10, 5, 0], //强度值
    Factor: [1, 2, 5], //字符加数,分别为字母，数字，其它
    KindFactor: [0, 0, 10, 20], //密码含几种组成的加数 
    Regex: [/[a-zA-Z]/g, /\d/g, /[^a-zA-Z0-9]/g] //字符正则数字正则其它正则
}

PasswordStrength.StrengthValue = function (pwd) {
    var strengthValue = 0;
    var ComposedKind = 0;
    for (var i = 0; i < this.Regex.length; i++) {
        var chars = pwd.match(this.Regex[i]);
        if (chars != null) {
            strengthValue += chars.length * this.Factor[i];
            ComposedKind++;
        }
    }
    strengthValue += this.KindFactor[ComposedKind];
    return strengthValue;
}

PasswordStrength.StrengthLevel = function (pwd) {
    var value = this.StrengthValue(pwd);
    for (var i = 0; i < this.LevelValue.length; i++) {
        if (value >= this.LevelValue[i])
            return this.Level[i];
    }
}

function checkpasswd(o) {
    var pshowmsg = '密码不得少于6个字符';
    if (o.value.length < 6) {
        setError(o, pshowmsg);
    }
    else {

        var showmsg = PasswordStrength.StrengthLevel(o.value);
        switch (showmsg) {
            case "太短": showmsg += " <img src='" + forumpath + "images/level/1.gif' width='88' height='11' />"; break;
            case "较弱": showmsg += " <img src='" + forumpath + "images/level/2.gif' width='88' height='11' />"; break;
            case "一般": showmsg += " <img src='" + forumpath + "images/level/3.gif' width='88' height='11' />"; break;
            case "极佳": showmsg += " <img src='" + forumpath + "images/level/4.gif' width='88' height='11' />"; break;
        }
        $('passwdpower').style.display = '';
        $('showmsg').innerHTML = showmsg;
        setError(o, "");
    }

}

function checkemail(obj) {
    var strMail = obj.value;
    var str;
    if (strMail.length == 0) {
        setError(obj, "E-mail 地址无效，请提供真实Email");
        return false;
    }
    var objReg = new RegExp("[A-Za-z0-9-_]+@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]")
    var IsRightFmt = objReg.test(strMail)
    var objRegErrChar = new RegExp("[^a-z0-9-._@]", "ig")
    var IsRightChar = (strMail.search(objRegErrChar) == -1)
    var IsRightLength = strMail.length <= 60
    var IsRightPos = (strMail.indexOf("@", 0) != 0 && strMail.indexOf(".", 0) != 0 && strMail.lastIndexOf("@") + 1 != strMail.length && strMail.lastIndexOf(".") + 1 != strMail.length)
    var IsNoDupChar = (strMail.indexOf("@", 0) == strMail.lastIndexOf("@"))
    if (!(IsRightFmt && IsRightChar && IsRightLength && IsRightPos && IsNoDupChar)) {
        str = "E-mail 地址无效，请提供真实Email";
    }
    if (str != '' && str != null && str != undefined) {
        setError(obj, str);
    }
    else {
        setError(obj, "");
    }
}

function htmlEncode(source, display, tabs) {
    function special(source) {
        var result = '';
        for (var i = 0; i < source.length; i++) {
            var c = source.charAt(i);
            if (c < ' ' || c > '~') {
                c = '&#' + c.charCodeAt() + ';';
            }
            result += c;
        }
        return result;
    }

    function format(source) {
        // Use only integer part of tabs, and default to 4
        tabs = (tabs >= 0) ? Math.floor(tabs) : 4;

        // split along line breaks
        var lines = source.split(/\r\n|\r|\n/);

        // expand tabs
        for (var i = 0; i < lines.length; i++) {
            var line = lines[i];
            var newLine = '';
            for (var p = 0; p < line.length; p++) {
                var c = line.charAt(p);
                if (c === '\t') {
                    var spaces = tabs - (newLine.length % tabs);
                    for (var s = 0; s < spaces; s++) {
                        newLine += ' ';
                    }
                }
                else {
                    newLine += c;
                }
            }
            // If a line starts or ends with a space, it evaporates in html
            // unless it's an nbsp.
            newLine = newLine.replace(/(^ )|( $)/g, '&nbsp;');
            lines[i] = newLine;
        }

        // re-join lines
        var result = lines.join('<br />');

        // break up contiguous blocks of spaces with non-breaking spaces
        result = result.replace(/  /g, ' &nbsp;');

        // tada!
        return result;
    }

    var result = source;

    // ampersands (&)
    result = result.replace(/\&/g, '&amp;');

    // less-thans (<)
    result = result.replace(/\</g, '&lt;');

    // greater-thans (>)
    result = result.replace(/\>/g, '&gt;');

    if (display) {
        // format for display
        result = format(result);
    }
    else {
        // Replace quotes if it isn't for display,
        // since it's probably going in an html attribute.
        result = result.replace(new RegExp('"', 'g'), '&quot;');
    }

    // special characters
    result = special(result);

    // tada!
    return result;
}

var profile_username_empty = '用户名不能为空。';
var profile_username_toolong = '用户名超过 20 个字符。';
var profile_username_tooshort = '用户名小于3个字符。';

function checkusername(inputobj) {
    var username = inputobj.value;
    var unlen = username.replace(/[^\x00-\xff]/g, "**").length;

    if (unlen < 3 || unlen > 20) {
        setError(inputobj, unlen < 3 ? (unlen == 0 ? profile_username_empty : profile_username_tooshort) : profile_username_toolong);
        return;
    }
    ajaxRead(forumpath + "tools/ajax.ashx?t=checkusername&username=" + escape(username), "showcheckresult(obj,'" + inputobj.id + "');");
}

function showcheckresult(obj, id) {
    var errorInfo = ["用户名中不允许包含全空格符", "用户名中不允许包含空格", "用户名中不允许包含冒号", "用户名已经使用",
    function (username) {
        var reg = /[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']/;
        return "用户名中存在非法字符 '" + reg.exec(username) + "'";
    },
    function (username) {
        var reg = /^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest/;
        return "用户名中含有非法的字符 '" + reg.exec(username) + "'";
    },
     "用户名被系统禁用"];
    var res = obj.getElementsByTagName('result');
    var result = "";
    if (res[0] != null && res[0] != undefined) {
        if (res[0].childNodes.length > 1) {
            result = res[0].childNodes[1].nodeValue;
        } else {
            result = res[0].firstChild.nodeValue;
        }
    }
    var inputobj = $(id);
    var username = inputobj.value;
    if (result != "0") {
        var msg = "";
        if (typeof errorInfo[result - 1] == "function")
            msg = errorInfo[result - 1](username);
        else
            msg = errorInfo[result - 1];
        setError(inputobj, msg);
    }
    else {
        setError(inputobj, "");
    }
}

function checkdoublepassword(theform) {
    var pw1 = theform.password.value;
    var pw2 = theform.password2.value;
    if (pw2 == '') {
        setError(theform.password2, "确认密码不得少于6个字符");
        return;
    }
    var str;

    if (pw1 != pw2) {
        str = "两次输入的密码不一致";
    }
    if (str != '' && str != null && str != undefined) {
        setError(theform.password2, str);
    }
    else {
        setError(theform.password2, "");
    }
}

function showTipInfo(obj) {
    var id = obj.id;
    var tips = {
        "username": "用户名由 3 到 20 个字符组成", "password": "密码至少需要6字符",
        "password2": "确认密码至少需要6字符", "email": "Email用于密码找回和论坛通知"
    };
    var tipObj = $(id + "_tip");
    tipObj.innerHTML = tips[id];
    tipObj.style.display = '';
    obj.className = obj.className.replace(" error", "");
    $(id + "_error").style.display = 'none';
}

function setError(obj, errorinfo) {
    if (errorinfo) {
        if (obj.className.indexOf("error") == -1)
            obj.className += " error";
    }
    else {
        obj.className = obj.className.replace(" error", "");
    }
    var error = $(obj.id + "_error");
    error.style.display = '';
    error.innerHTML = errorinfo;
    $(obj.id + "_tip").style.display = 'none';
}

function setUsernameFocus() {
    var obj = $("username");
    obj.focus();
    showTipInfo(obj);
}