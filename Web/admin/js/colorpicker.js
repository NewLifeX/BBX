
var tb_ClientID = ''; //颜色文件框ID
var img_ClientID = ''; //显示选取器的图标ID

var color = "" ;
var SelRGB = color;
var DrRGB = '';
var SelGRAY = '120';

var hexch = new Array('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F');

//转成16进制数
function ToHex(n) {	
	var h, l;

	n = Math.round(n);
	l = n % 16;
	h = Math.floor((n / 16)) % 16;
	return (hexch[h] + hexch[l]);
}

//进行标准颜色值转换            
function DoColor(c, l){
	var r=1, g=2, b=3;

     
 		r = '0x' + c.substring(1, 3);
		g = '0x' + c.substring(3, 5);
		b = '0x' + c.substring(5, 7);

		if(l > 120)
		{
			l = l - 120;
			r = (r * (120 - l) + 255 * l) / 120;
			g = (g * (120 - l) + 255 * l) / 120;
			b = (b * (120 - l) + 255 * l) / 120;
		}
		else
		{
			r = (r * l) / 120;
			g = (g * l) / 120;
			b = (b * l) / 120;
		}
    var aaa='#' + ToHex(r) + ToHex(g) + ToHex(b);
    if(aaa=='#NaNNaNNaN')
    {
		return '#FFFFFF';
    }
    else
    {
        return '#' + ToHex(r) + ToHex(g) + ToHex(b);
    }
}

//显示当前颜色区域(td)
function wc(r, g, b, n, tb_ClientID)
{         
    r = ((r * 16 + r) * 3 * (15 - n) + 0x80 * n) / 15;
    g = ((g * 16 + g) * 3 * (15 - n) + 0x80 * n) / 15;
    b = ((b * 16 + b) * 3 * (15 - n) + 0x80 * n) / 15;

    document.write('<td bgcolor=#' + ToHex(r) + ToHex(g) + ToHex(b) + ' title=\"#' + ToHex(r) + ToHex(g) + ToHex(b) + '\" height=8 width=8 onmouseover=\"ColorTableMouseOver(this)\" onmousedown=\"ColorTableMouseDown(this)\"  onmouseout=\"ColorTableMouseOut(this)\" ></td>');
}

//显示颜色和灰度面板
function WriteColorPanel(tb_ClientID,img_ClientID,leftvalue,topvalue)
{
    
    document.write('<td >');
    document.write('<table border=\"0\" cellPadding=\"0\" cellSpacing=\"0\" id=\"ColorTable_'+tb_ClientID+'\" style=\"cursor:crosshair;\" >');
    var cnum = new Array(1, 0, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0, 0);
    for(i = 0; i < 16; i ++)
    {
        document.write('<TR>');
        for(j = 0; j < 30; j ++)
        {
       	    n1 = j % 5;
       	    n2 = Math.floor(j / 5) * 3;
      	    n3 = n2 + 3;
       	    wc((cnum[n3] * n1 + cnum[n2] * (5 - n1)),
       	    (cnum[n3 + 1] * n1 + cnum[n2 + 1] * (5 - n1)),
       	    (cnum[n3 + 2] * n1 + cnum[n2 + 2] * (5 - n1)), i,tb_ClientID);
        }
        document.write('</tr>');
    }
    document.write('</table></td>');
    
    
    document.write('<td>');
    document.write('<table border=\"0\" cellPadding=\"0\" cellSpacing=\"0\" id=\"GrayTable'+tb_ClientID+'\" style=\"CURSOR: hand;cursor:crosshair;\" >');
    for(i = 255; i >= 0; i -= 8.5)
       document.write('<tr bgcolor=#' + ToHex(i) + ToHex(i) + ToHex(i) + '><td title=' + Math.floor(i * 16 / 17) + ' height=4 width=20 onmouseover=\"GrayTableMouseOver(this)\" onmousedown=\"GrayTableMouseDown(this)\"  onmouseout=\"GrayTableMouseOut(this)\" ></td></tr>');
           
    // alert(tb_ClientID);      
    document.write("<tbody></tbody></table></td>");
}

//鼠标在颜色面板上MouseDown事件处理程序
function ColorTableMouseDown(e)
{
	SelRGB = e.title;
	EndColor();
}

//鼠标在颜色面板上MouseOver事件处理程序
function ColorTableMouseOver(e)
{
 	$('RGB'+tb_ClientID).innerHTML = e.title;
	EndColor();
}

//鼠标在颜色面板上MouseOut事件处理程序
function ColorTableMouseOut(e)
{
  	$('RGB'+tb_ClientID).innerHTML = SelRGB;
	EndColor();
}


//鼠标在灰度面板上MouseDown事件处理程序
function GrayTableMouseDown(e)
{
	SelGRAY = e.title;
	EndColor();
}

//鼠标在灰度面板上MouseOver事件处理程序
function GrayTableMouseOver(e)
{
	$('GRAY'+tb_ClientID).innerHTML = e.title;
	EndColor();
}

//鼠标在灰度面板上MouseOut事件处理程序
function GrayTableMouseOut(e)
{
    //alert(tb_ClientID);
	$('GRAY'+tb_ClientID).innerHTML = SelGRAY;
	EndColor();
}

//隐藏颜色和灰度面板
function HideColorPanel(tb_clientid)
{   
    $('ShowColor'+tb_ClientID).bgColor = $('SelColor'+tb_ClientID).value;
    $('ColorPicker'+tb_clientid).style.display='none';
}

//当鼠标在颜色或灰度面板上操作时，将所选值设置到相应控件或区域
function EndColor()
{
    var i;

    if(DrRGB != SelRGB)
    {
       DrRGB = SelRGB;
       for(i = 0; i <= 30; i ++)
       {
            $('GrayTable'+tb_ClientID).rows[i].bgColor = DoColor(SelRGB, 240 - i * 8);
       }
    }

    $('SelColor'+tb_ClientID).value = DoColor($('RGB'+tb_ClientID).innerHTML, $('GRAY'+tb_ClientID).innerHTML);
    $('ShowColor'+tb_ClientID).bgColor = $('SelColor'+tb_ClientID).value;
    $(tb_ClientID).value = $('SelColor'+tb_ClientID).value;
}


//当点击"确定"按钮时
function ColorPickerOK(tb_clientid ,img_clientid)
{
     var selectcolor=$('SelColor'+tb_clientid).value;
     $(img_ClientID).style.background=selectcolor;
     obj=$(tb_clientid);
     obj.value=selectcolor;
     obj.focus();
     obj.select();
     //if(navigator.appName.indexOf('Explorer') > -1)
     //{
     //    obj.createTextRange().execCommand('Copy');
     //    window.status = '将模板内容复制到剪贴板';
     //    setTimeout(\"window.status=''\", 1800);
     //}
     HideColorPanel(tb_clientid);
}

//在参数值指定的位置显示"颜色"面板,供面板自身设置时(onmouseover)调用
function ShowColorPanel(tb_clientid,img_clientid, leftvalue,topvalue)
{
     tb_ClientID = tb_clientid;
     img_ClientID = img_clientid;
     var p = getposition($(tb_ClientID));
     $('ColorPicker'+tb_ClientID).style.display = 'block';
    
        if(navigator.appName.indexOf('Explorer') > -1)
        {
 			$('ColorPicker'+tb_ClientID).style.left = p['x'] + leftvalue +  'px';
		}
		else
		{
		    $('ColorPicker'+tb_ClientID).style.left = p['x'] + 'px';
		}
        
        if(navigator.appName.indexOf('Explorer') > -1)
        {
            $('ColorPicker'+tb_ClientID).style.top = p['y'] + $(tb_ClientID).offsetHeight + topvalue + 'px';
        }
        else
        {
			$('ColorPicker'+tb_ClientID).style.top = p['y'] + $(tb_ClientID).offsetHeight + 'px';
        }    
}

//在参数值指定的位置显示"颜色"面板(供控件调用)
function IsShowColorPanel(tb_clientid,img_clientid, leftvalue,topvalue)
{
     tb_ClientID = tb_clientid;
     img_ClientID = img_clientid;
     if($('ColorPicker'+tb_ClientID).style.display == 'none')
     {
        var p = getposition($(tb_ClientID));
        $('ColorPicker'+tb_ClientID).style.display = 'block';
        
        if(navigator.appName.indexOf('Explorer') > -1)
        {
 			$('ColorPicker'+tb_ClientID).style.left = p['x'] + leftvalue +  'px';
		}
		else
		{
		    $('ColorPicker'+tb_ClientID).style.left = p['x'] + 'px'; 
		}
        
        if(navigator.appName.indexOf('Explorer') > -1)
        {
            $('ColorPicker'+tb_ClientID).style.top = p['y'] + $(tb_ClientID).offsetHeight + topvalue + 'px';
        }
        else
        {
			$('ColorPicker'+tb_ClientID).style.top = p['y'] + $(tb_ClientID).offsetHeight + 'px';
        }    
     }
     else
     {
        $('ColorPicker'+tb_ClientID).style.display = 'none';
     }
}

//获得指定元素的位置
function getposition(obj)
{
    // alert(topvalue);
     var r = new Array();
     r['x'] = obj.offsetLeft;
     r['y'] = obj.offsetTop;
     while(obj = obj.offsetParent)
     {
 		r['x'] += obj.offsetLeft;
		r['y'] += obj.offsetTop;
     }
     return r;
}

//摘自prototype.js
function $() 
{
    var elements = new Array();
  
    for (var i = 0; i < arguments.length; i++) 
    {
        var element = arguments[i];
        try
        {
		    if (typeof element == 'string')
		    {
		        element = document.getElementById(element) || document.all(element) || document.forms(0).all(element);
		    }
        }
        catch(ex)
        {
		    element = null;
        }

        if (arguments.length == 1) 
        {
            return element;
        }
      
        elements.push(element);
    }
  
    return elements;
}


//初始化选取器中图标的背景色
function InitColorPicker(img_ClientID , selectColor)   
{         
    if (selectColor != null && selectColor != "")
    {
        $(img_ClientID).style.background=selectColor;
    }
}
