(function($){
	$(document).ready(function(){
		/*ÂÖ»»Í¼ÇÐ»»js*/
		$('.paging').show().animate({opacity:0.65},0);
		$('.paging a:first').addClass('active');
		$('.paging a:last').css('margin-right','6px');
		$('.paging span').html($('.image_reel li:first img').attr('alt'));

		var imageWidth = $('.window').width();
		var imageSum = $('.image_reel li').size();
		var imageReelWidth = imageWidth*imageSum;
		$('.image_reel ul').css({'width':imageReelWidth});
		rotate=function(){
			var triggerId = $active.attr('rel')-1;
			var image_reelPosition = triggerId*imageWidth;			
			var imgTitle = $('.image_reel li img').eq(triggerId).attr('alt');
			$('.paging span').html(imgTitle);
			$('.paging a').removeClass('active');
			$active.addClass('active');
			$('.image_reel ul').animate({left:-image_reelPosition},500);
		};
		rotateSwitch = function() {
			play = setInterval(function(){
				$active = $('.paging a.active').next();
				if ($active.length === 0) {
					$active = $('.paging a:first');
				}
				rotate();
			},7000);
		};
		rotateSwitch();
		$('.image_reel a').hover(function(){
			clearInterval(play);
		},function(){
			rotateSwitch();
		});
		$('.paging a').click(function(){
			$active = $(this);
			clearInterval(play);
			rotate();
			rotateSwitch();
			return false;
		})
	});
})(jQuery);