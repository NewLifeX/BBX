function BOX_show(e)
{       //显示
        if(document.getElementById(e)==null){return;}
        BOX_layout(e);
        window.onresize = function(){BOX_layout(e);} //改变窗体重新调整位置
        window.onscroll = function(){BOX_layout(e);} //滚动窗体重新调整位置
}

function BOX_remove(e)
{       //移除
        window.onscroll = null;
        window.onresize = null;
        //document.getElementById('BOX_overlay').style.display="none";
        document.getElementById(e).style.display="none";
}

function BOX_layout(e)
{       //调整位置
        var a = document.getElementById(e);
//        if (document.getElementById('BOX_overlay')==null){ //判断是否新建遮掩层
//            var overlay = document.createElement("div");
//            overlay.setAttribute('id','BOX_overlay');
//            overlay.onclick=function(){BOX_remove(e);};
//            a.parentNode.appendChild(overlay);
//        }
        //取客户端左上坐标，宽，高
        var scrollLeft = (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft);
        var scrollTop = (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop);
        var clientWidth;
       // if (window.innerWidth) {
        //    clientWidth = ((Sys.Browser.agent === Sys.Browser.Safari) ? window.innerWidth : Math.min(window.innerWidth, document.documentElement.clientWidth));
       // } else {
            clientWidth = document.documentElement.clientWidth;
       // }
        var clientHeight;
       // if (window.innerHeight) {
      //      clientHeight = ((Sys.Browser.agent === Sys.Browser.Safari) ? window.innerHeight : Math.min(window.innerHeight, document.documentElement.clientHeight));
       // } else {
            clientHeight = document.documentElement.clientHeight;
       // }
//        var bo = document.getElementById('BOX_overlay');
//        bo.style.left = scrollLeft+'px';
//        bo.style.top = scrollTop+'px';
//        bo.style.width = clientWidth+'px';
//        bo.style.height = clientHeight+'px';      
//        bo.style.display="";
        //Popup窗口定位
        a.style.position = 'absolute';
        a.style.zIndex=101;
        a.style.display="";
        a.style.left = scrollLeft+((clientWidth-a.offsetWidth)/2)+'px';
        a.style.top = scrollTop+((clientHeight-a.offsetHeight)/2)+'px';
}
