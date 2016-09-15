function ismaxlen(o) {
     var nMaxLen=o.getAttribute? parseInt(o.getAttribute('maxlength')):'';
     if(o.getAttribute && o.value.length>nMaxLen){
          o.value=o.value.substring(0,nMaxLen)
     }
}


/******************************************图片预览******************************************/

function PhotoView(path ,previewImage)
{
		if (path != "") {
			var patn = /\.jpg$|\.jpeg$|\.gif$|\.png$/i;
			if (!patn.test(path))
			{
				clearFileInput($("upfileshoppic"));
				previewImage.innerHTML = "暂无图片";
				alert("店铺LOGO只允许jpg、jpeg、gif或png格式的图片!");
				return;
			}
			if(document.all) //IE执行
			{
                insertImage(path, previewImage);
            }			
		}
}

function clearFileInput(file) {
		var form = document.createElement('form');
		document.body.appendChild(form);
		var pos = file.nextSibling;
		form.appendChild(file);
		form.reset();
		pos.parentNode.insertBefore(file, pos);
		document.body.removeChild(form);
}
	
function insertImage(path, previewImage) {
    var localimgpreview = '';
    var ext = path.lastIndexOf('.') == -1 ? '' : path.substr(path.lastIndexOf('.') + 1, path.length).toLowerCase();
    var re = new RegExp("(^|\\s|,)" + ext + "($|\\s|,)", "ig");
    var localfile = $("upfileshoppic").value.substr($("upfileshoppic").value.replace(/\\/g, '/').lastIndexOf('/') + 1);
    if(path == '') {
        return;
    }
    previewImage.innerHTML = "<img style=\"filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=\'scale\',src=\'" + path +"\');width:75;height:75;\" src=\"images/common/none.gif\" border=\"0\" alt=\"\" />";
}



/*******************************************************城市信息*********************************************************/
//数据文件详见javascript/locations.js

$("locus_1").onchange = function(e) {
    var length = 0;
    for(var i in locations) {
        if(locations[i].state == $("locus_1").value) {
            $("locus_2").options[length] = new Option(locations[i].city, locations[i].lid);
            length++;
        }
    }
    $("locus_2").options.length = length; 
}

function initstate() {
    $("locus_1").options.length = states.length+1; 
    $("locus_1").options[0] = new Option("----请选择省份----","-1");
    i = 1;
    for(var state in states) {
        $("locus_1").options[i] = new Option(states[i-1].state, states[i-1].state);
        i++;
    }
}

initstate();


