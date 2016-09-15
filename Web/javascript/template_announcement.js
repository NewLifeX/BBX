	var anndelay = 3000;
	var annst = 0;
	var annstop = 0;
	var annrowcount = 0;
	var anncount = 0;
	var annlis = $('announcementbody').getElementsByTagName("LI");
	var annrows = new Array();
	var annstatus;

	function announcementScroll() {
		if(annstop) {
			annst = setTimeout('announcementScroll()', anndelay);
			return;
		}
		if(!annst) {
			var lasttop = -1;
			for(i = 0;i < annlis.length;i++) {

				if(lasttop != annlis[i].offsetTop) {
					if(lasttop == -1) {
						lasttop = 0;
					}
					annrows[annrowcount] = annlis[i].offsetTop - lasttop;
					annrowcount++;
				}
				lasttop = annlis[i].offsetTop;
			}

			if(annrows.length == 1) {
				$('announcement').onmouseover = $('announcement').onmouseout = null;
			} else {
				annrows[annrowcount] = annrows[1];
				//				$('announcementbody').innerHTML += '<br style="clear:both" />' + $('announcementbody').innerHTML;
				$('announcementbody').innerHTML += $('announcementbody').innerHTML;
				annst = setTimeout('announcementScroll()', anndelay);
			}
			annrowcount = 1;
			return;
		}

		if(annrowcount >= annrows.length) {
			$('announcementbody').scrollTop = 0;
			annrowcount = 1;
			annst = setTimeout('announcementScroll()', anndelay);
		} else {
			anncount = 0;
			announcementScrollnext(annrows[annrowcount]);
		}
	}

	function announcementScrollnext(time) {
		$('announcementbody').scrollTop++;
		anncount++;
		if(anncount != time) {
			annst = setTimeout('announcementScrollnext(' + time + ')', 10);
		} else {
			annrowcount++;
			annst = setTimeout('announcementScroll()', anndelay);
		}
	}
	announcementScroll();