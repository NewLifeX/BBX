var is_ie = document.all ? true : false;
var is_ff = window.addEventListener ? true : false;

//得到控件的绝对位置
function getposition(id) {
	e = document.getElementById(id);
	var t = e.offsetTop;
	var l = e.offsetLeft;
	while (e = e.offsetParent) {
		t += e.offsetTop;
		l += e.offsetLeft;
	}
	var r = new Array();
	r['x'] = l;
	r['y'] = t;
	return r;
}

//debug
document.write('<div id="jsdebug"></div>');
function d(e) {
	s = '';
	for(k in e) {
		t = typeof e[k];
		s += t + ' : <b>' + k + ' :</b> ' + e[k] + '<br />';
	}
	document.getElementById('jsdebug').innerHTML = s;
}

/***********************************************************************************************************************/
var controlid;				//控件 日历数值显示， 绝对位置定位
var currdate 	= null;			//当前初始化时间	默认为本地时间
var startdate 	= null; 		//日期范围 - 开始日期
var enddate 	= null; 		//日期范围 - 截止日期
var yy 			= null; 	//年
var mm 			= null;		//月
var i;					//列
var j;					//行
var currday		= null; 	//今天
var today 		= new Date(); 	//当前时间
today.setHours(0);
today.setMinutes(0);
today.setSeconds(0);
today.setMilliseconds(0);

//	pasedate('2005-1-2') 返回date对象
function parsedate(s){
	if(s == ''){ return false;};
	var reg = new RegExp("[^0-9-]","")
	if(s.search(reg)>=0)
		return today;
	var ss = s.split("-");
	if(ss.length != 3)
		return today;
	if(isNaN(ss[0])||isNaN(ss[1])||isNaN(ss[2]))
		return today;
	return new Date(parseFloat(ss[0]),parseFloat(ss[1])-1,parseFloat(ss[2]));
}

function setdate(d){
	document.getElementById('calendardiv').style.display = 'none';
	controlid.value = yy + "-" + (mm+1) + "-" + d;
}

function myCancelBubble(event) {
	e = event ? event : window.event ;
	if(is_ff) {
		e.stopPropagation();
	} else if(is_ie) {
		e.cancelBubble = true;
	}
}

function initcalendar(){
	//当前时间
	s = '<style>';
	s += '#calendardiv{background-color:#FFFFCC;cursor:default;width:180px;z-index:9999;}';
	s += '#calendardiv a{color:#333333;text-decoration:none;}';
	s += '#calendardiv table{border:1px solid #333333;width:100%}';
	s += '.expire, .expire a{color:#ccc;}';
	s += '.default, .default a{color:#333333}';
	s += '.checked, .checked a{font-weight:bold;}';
	s += '.today{color:#ffcc00}';
	s += '</style>';
	s += '<div id="calendardiv" style="display:none;position:absolute;" onclick="myCancelBubble(event)">';
	s += '<table cellpadding="2" cellspacing="4">';
	s += '<tr><td colspan="7"><table width="100%" style="border:0px" align="center"><tr><td id="prev" align="center"><a href="javascript:drawcalendar(yy-1,mm);" title="上一年"><img src="images/page/first.gif" border="0" width="9" height="8" /></a>&nbsp; &nbsp<a href="javascript:drawcalendar(yy,mm-1);" title="上个月"><img src="images/page/prev.gif" border="0" width="8" height="8" /></a></td><td colspan="5" id="yyyymm" align="center"></td><td id="next" align="center"><a href="javascript:drawcalendar(yy,mm+1);" title="下个月"><img src="images/page/next.gif" border="0" width="8" height="8" /></a>&nbsp &nbsp;<a href="javascript:drawcalendar(yy+1,mm);" title="下一年"><img src="images/page/last.gif" border="0" width="9" height="8" /></a></td></tr></table></td></tr>';
	//s += '<tr><td id="prev"> </td><td colspan="5" id="yyyymm" align="center"></td></tr>';
	s += '<tr><td>日</td><td>一</td><td>二</td><td>三</td><td>四</td><td>五</td><td>六</td></tr>';
	for(i=0; i <6; i++){
		s += "<tr>";
		for(j=1; j<=7; j++)
			s += "<td id=d"+(i*7+j)+" height=\"19\">0</td>";
		s += "</tr>";
	}
	s += '</table>';
	s += '</div>';
	document.write(s);
	currday 	= currdate ? currdate : today;// 默认为本地时间
	//点击隐藏
	document.onclick = function() {
		document.getElementById('calendardiv').style.display = 'none';
	}
}

function showcalendar(event, controlid1, startdate1, enddate1, defday){
	// 判断controlid position
	controlid   = document.getElementById(controlid1);
	startdate   = parsedate(document.getElementById(startdate1).value);
	enddate     = parsedate(document.getElementById(enddate1).value);
	defday		= parsedate(defday);

	var p   = getposition(controlid1);
	document.getElementById('calendardiv').style.display = '';
	document.getElementById('calendardiv').style.left = p['x'] + 'px';
	document.getElementById('calendardiv').style.top  = p['y'] + 26 + 'px';

	myCancelBubble(event);

	drawcalendar(defday.getFullYear(),defday.getMonth());
}

// 刷新日历
function drawcalendar(y, m){
	var x  = new Date(y, m, 1);
	var mv = x.getDay();
	var d = x.getDate();
	var de = null;					// 单元格对象
	yy 	   = x.getFullYear();
	mm 	   = x.getMonth();
	document.getElementById("yyyymm").innerHTML = yy + "." + (mm+1 > 9  ? mm+1 : "0" + (mm+1));
	//将1号以前的单元设置为空
	for(var i=1; i<=mv; i++){
		de = document.getElementById("d"+i);
		de.innerHTML= "";
		de.className= "";
	}

	//开始画当月日历
	while(x.getMonth() == mm){
		de = document.getElementById("d"+(d+mv));
		if((enddate && x.getTime() > enddate.getTime()) || (startdate && x.getTime() < startdate.getTime())) {
			de.className = 'expire';
			de.innerHTML = d;
		}else{
			de.className = 'default';
			de.innerHTML = "<a href=javascript:setdate("+d+");>"+d+"</a>";
		}
		if(x.getTime() == currday.getTime()) {
			de.className = 'checked';
		}
		if(x.getTime() == today.getTime()) {
			de.className = 'today';
		}
		x.setDate(++d);
	}
	// 尾部空格
	while(d + mv <= 42){
		de = document.getElementById("d"+(d+mv));
		de.innerHTML = "";
		de.bgColor = "";
		de.className = "";
		d++;
	}
}

initcalendar();
