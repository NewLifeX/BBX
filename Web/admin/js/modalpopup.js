//显示
function BOX_show(e) 
{   
    if(document.getElementById(e)==null){return;}
    BOX_layout(e);
    window.onresize = function(){BOX_layout(e);} //改变窗体重新调整位置
    window.onscroll = function(){BOX_layout(e);} //滚动窗体重新调整位置
}

//移除
function BOX_remove(e)
{   
    document.getElementById('BOX_overlay').style.display="none";
    document.getElementById(e).style.display="none";
    
    window.onscroll = null;
    window.onresize = null;
}

//调整布局
function BOX_layout(e)
{
    var a = document.getElementById(e);
    
    //判断是否新建遮掩层
    if (document.getElementById('BOX_overlay')==null)
    { 
        var overlay = document.createElement("div");
        overlay.setAttribute('id','BOX_overlay');
        a.parentNode.appendChild(overlay);
    }
    //alert(a.style.width + "," + a.style.height);
    //alert("clientWidth:" + window.parent.innerWidth + ",clientHeight:" + window.parent.innerHeight);
    //取客户端左上坐标，宽，高
    var scrollLeft = (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft);
    var scrollTop = (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop);
    var clientWidth = document.documentElement.clientWidth;
    var clientHeight = document.documentElement.clientHeight;
    var bo = document.getElementById('BOX_overlay');
    bo.style.left = scrollLeft+'px';
    bo.style.top = scrollTop+'px';
    bo.style.width = clientWidth+'px';
    bo.style.height = clientHeight+'px';
    bo.style.display="";
    //Popup窗口定位
    a.style.position = 'absolute';
    a.style.zIndex=101;
    a.style.display="";
    //a.style.left = "40%";
    //a.style.top = "40%";
    //a.style.left = ((document.documentElement.clientWidth - 780) / 2) + "px";
    //a.style.top = "-10px";
    
    document.getElementById('setting').style.display="block";
    a.style.left = ((document.documentElement.clientWidth - a.clientWidth) / 2) + "px";
    var top = (parseInt((document.documentElement.clientHeight - a.clientHeight - 43) / 2));
    top = parseInt((document.documentElement.clientHeight - a.clientHeight - 43) / 2);  
    if(top < 0)
        top = 0;
    top += document.documentElement.scrollTop;
    a.style.top = top + "px";
}




