if (typeof (closedtags)=='undefined')
{
	var closedtags;
}
if (typeof (colorfultags)=='undefined')
{
	var colorfultags;
}
function forumhottag_callback(data)
{
	var hottag_html = '';
	for (var i in data)
	{
		if (!in_array(data[i].tagid, closedtags))
		{
		    if(aspxrewrite == 1) {
			     hottag_html += '<li><a href="topictag-' + data[i].tagid + '.aspx" target="_blank"';
			}
			else {
			     hottag_html += '<li><a href="tags.aspx?t=topic&tagid=' + data[i].tagid + '" target="_blank"';
			}    
			
			if (colorfultags && colorfultags[data[i].tagid])
			{
				 hottag_html += ' style="color: #' + colorfultags[data[i].tagid].color + '"';
			}
			hottag_html +='>' + data[i].tagname + '</a><em>(' + data[i].fcount + ')</em></li>';
		}		
	}
	$('forumhottags').innerHTML = hottag_html;
}

function spacehottag_callback(data)
{
	var hottag_html = '';
	for (var i in data)
	{
		if (!in_array(data[i].tagid, closedtags))
		{
		    if(aspxrewrite == 1) {
			     hottag_html += '<li><a href="spacetag-' + data[i].tagid + '.aspx" target="_blank"';
			}
			else {
			     hottag_html += '<li><a href="tags.aspx?t=spacepost&tagid=' + data[i].tagid + '" target="_blank"';
			}    
			
			if (colorfultags && colorfultags[data[i].tagid])
			{
				 hottag_html += ' style="color: #' + colorfultags[data[i].tagid].color + '"';
			}
			hottag_html +='>' + data[i].tagname + '</a>(' + data[i].scount + ')</li>';
		}		
	}
	$('spacehottags').innerHTML = hottag_html;
}

function photohottag_callback(data)
{
	var hottag_html = '';
	for (var i in data)
	{
		if (!in_array(data[i].tagid, closedtags))
		{
		    if(aspxrewrite == 1) {
			     hottag_html += '<li><a href="phototag-' + data[i].tagid + '.aspx" target="_blank"';
			}
			else {
			     hottag_html += '<li><a href="tags.aspx?t=photo&tagid=' + data[i].tagid + '" target="_blank"';
			}    
			
			if (colorfultags && colorfultags[data[i].tagid])
			{
				 hottag_html += ' style="color: #' + colorfultags[data[i].tagid].color + '"';
			}
			hottag_html +='>' + data[i].tagname + '</a>(' + data[i].pcount + ')</li>';
		}		
	}
	$('photohottags').innerHTML = hottag_html;
}


function mallhottag_callback(data)
{
	var hottag_html = '';
	for (var i in data)
	{
		if (!in_array(data[i].tagid, closedtags))
		{
		    if(aspxrewrite == 1) {
			     hottag_html += '<li><a href="malltag-' + data[i].tagid + '.aspx" target="_blank"';
			}
			else {
			     hottag_html += '<li><a href="tags.aspx?t=mall&tagid=' + data[i].tagid + '" target="_blank"';
			}    
			
			if (colorfultags && colorfultags[data[i].tagid])
			{
				 hottag_html += ' style="color: #' + colorfultags[data[i].tagid].color + '"';
			}
			hottag_html +='>' + data[i].tagname + '</a>(' + data[i].pcount + ')</li>';
		}		
	}
	$('mallhottags').innerHTML = hottag_html;
}


function getajaxforumhottags()
{
	_sendRequest('tools/ajax.ashx?t=getforumhottags', function(d){
		try{
		eval("(" + d + ")");}catch(e){};
	});
}

function getajaxspacehottags()
{
	_sendRequest('tools/ajax.ashx?t=getspacehottags', function(d){
		try{
		eval("(" + d + ")");}catch(e){};
	});
}

function getajaxphotohottags()
{
	_sendRequest('tools/ajax.ashx?t=getphotohottags', function(d){
		try{
		eval("(" + d + ")");}catch(e){};
	});
}

function getajaxmallhottags()
{
	_sendRequest('tools/ajax.ashx?t=getmallhottags', function(d){
		try{
		eval("(" + d + ")");}catch(e){};
	});
}
