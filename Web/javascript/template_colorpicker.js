var color = "" ;
var SelRGB = color;
var DrRGB = '';
var SelGRAY = '120';

var hexch = new Array('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F');

function ToHex(n) {	
	var h, l;

	n = Math.round(n);
	l = n % 16;
	h = Math.floor((n / 16)) % 16;
	return (hexch[h] + hexch[l]);
}

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



function EndColor(){

	var i;

	if(DrRGB != SelRGB){
		DrRGB = SelRGB;
		for(i = 0; i <= 30; i ++)
		{
		   $("GrayTable").rows[i].bgColor = DoColor(SelRGB, 240 - i * 8);
		}
	}

	$('SelColor').value = DoColor($('RGB').innerHTML, $('GRAY').innerHTML);
	$('ShowColor').bgColor = $('SelColor').value;
	//$('highlight_color').value = $('SelColor').value;
}

function ColorTableMouseDown(e)
{
	if ($('highlight_colorselect'))
	{	
		$('highlight_colorselect').selectedIndex=0;
		$('highlight_colorselect').style.background='#FFFFFF';
	}
   // $('s_bgcolor').style.background=e.title;
	SelRGB = e.title;
	EndColor();
}

function ColorTableMouseOver(e)
{
 	$('RGB').innerHTML = e.title;
	EndColor();
}

function ColorTableMouseOut(e)
{
	$('RGB').innerHTML = SelRGB;
	EndColor();
}



function GrayTableMouseDown(e)
{
	if ($('highlight_colorselect'))
	{
		$('highlight_colorselect').selectedIndex=0;
		$('highlight_colorselect').style.background='#FFFFFF';
	}	
	//$('s_bgcolor').style.background=$('SelColor').value;
	SelGRAY = e.title;
	EndColor();
}

function GrayTableMouseOver(e)
{
	$('GRAY').innerHTML = e.title;
	EndColor();
}

function GrayTableMouseOut(e)
{
	$('GRAY').innerHTML = SelGRAY;
	EndColor();
}

function ColorPickerOK(objid, buttonid)
{
    var selectcolor=$('SelColor').value;
	objid = objid ? objid : 'highlight_color';
	buttonid = buttonid ? buttonid : 's_bgcolor';
    $(objid).value=selectcolor;
    obj=$(objid);
    $(buttonid).style.background=selectcolor;
	obj.focus();
	obj.select();
	HideColorPanel();
}

function HideColorPanel()
{  
    $('ColorPicker').style.display = 'none';
}



function ShowColorPanel(obj, color_textbox_id)
{ 
	
   	var p = getposition($(color_textbox_id ? color_textbox_id : 'highlight_color'));
	$('ColorPicker').style.display = 'block';
	$('ColorPicker').style.left = p['x']+'px';
	$('ColorPicker').style.top = (p['y'] + 20)+'px';
}


function IsShowColorPanel(obj, color_textbox_id)
{
	if($('ColorPicker').style.display == 'none')
	{
		var p = getposition($(color_textbox_id ? color_textbox_id : 'highlight_color'));
		$('ColorPicker').style.display = 'block';
		$('ColorPicker').style.left = p['x']+'px';
		$('ColorPicker').style.top = (p['y'] + 20)+'px';
	}
	else
	{
		$('ColorPicker').style.display = 'none';
	}
}
	
function getposition(obj) {
	if (typeof obj != "object")
	{
		obj = $(obj);
	}
	var r = new Array();
	r['x'] = obj.offsetLeft;
	r['y'] = obj.offsetTop;
	while(obj = obj.offsetParent) {
		r['x'] += obj.offsetLeft;
		r['y'] += obj.offsetTop;
	}
	return r;
}


function selectoptioncolor(obj)
{
      $('highlight_color').value=obj.value;
      $('s_bgcolor').style.background=obj.value;
      if($('highlight_colorselect'))
      {
          $('highlight_colorselect').style.background=obj.value;
      }
      $('highlight_color').focus();
}

 