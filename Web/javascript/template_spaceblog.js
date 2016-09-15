function InsertHTML(content)
{
        if(content.indexOf('.jpeg')>0||content.indexOf('.jpg')>0||content.indexOf('.gif')>0||content.indexOf('.png')>0)
        {
            content = "<img src=\""+content+"\" border='0' /><br />\r\n";
        }
        else if(content.indexOf('.swf')>0)
        {
            content = "<embed src=\""+content+"\" width=\"100\" height=\"100\"  type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" play=\"true\" loop=\"true\" menu=\"true\"></embed>";
        }
        else
        {
            content = "<a href='" + content + "'>" + content + "</a>";
        }
        dntEditor.InsertText( content ) ;
}

function checkBlog()
{
//    PrepareSave();
    if(document.getElementById('title').value == "") 
    {
        alert('文章标题不能为空');
        document.getElementById('title').focus();
        return false;
    }
    if (document.getElementById('blogtexteditor').value == "")
    {
        alert('文章内容不能为空');
        return false;
    }
    return true;
}
function getposition(obj)
{
    var r = new Array();
    r['x'] = obj.offsetLeft;
    r['y'] = obj.offsetTop;
    while(obj = obj.offsetParent) 
    {
        r['x'] += obj.offsetLeft;
        if(navigator.appName.indexOf('Explorer') > -1)
        {
            r['y'] += obj.offsetTop;
        }
        else
        {
            r['y'] += obj.offsetTop;
        }
    }
    return r;
}

function hideimage(imgid)
{
    document.getElementById(imgid).style.display = "none";
}

function showimage(imgid)
{
    var p = getposition(document.getElementById("span"+imgid));
    document.getElementById(imgid).style.display = 'block';
    document.getElementById(imgid).style.left = p['x']+'px';
    document.getElementById(imgid).style.top = (p['y']+20)+'px';
    document.getElementById(imgid).style.display="block";
}

