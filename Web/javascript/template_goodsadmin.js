/****************************************商品类型层显示**************************************/

function boxShow(e)
{       //显示
        if($(e)==null){return;}
        boxLayout(e);
        window.onresize = function(){boxLayout(e);} //改变窗体重新调整位置
        window.onscroll = function(){boxLayout(e);} //滚动窗体重新调整位置
}

function boxRemove(e)
{       //移除
        window.onscroll = null;
        window.onresize = null;
        document.getElementById('BOX_overlay').style.display="none";
        document.getElementById(e).style.display="none";
}

function boxLayout(e)
{       //调整位置
        var a = $(e);
        if ($('BOX_overlay')==null){ //判断是否新建遮掩层
            var overlay = $("div");
            overlay.setAttribute('id','BOX_overlay');
            overlay.onclick=function(){BOX_remove(e);};
            a.parentNode.appendChild(overlay);
        }
        //取客户端左上坐标，宽，高
        var scrollLeft = (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft);
        var scrollTop = (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop);
        var clientWidth = document.documentElement.clientWidth;

        var clientHeight = document.documentElement.clientHeight;
        var bo = $('BOX_overlay');
        bo.style.left = scrollLeft+'px';
        bo.style.top = scrollTop+'px';
        bo.style.width = clientWidth+'px';
        bo.style.height = clientHeight+'px';      
        bo.style.display="";
        //Popup窗口定位
        a.style.position = 'absolute';
        a.style.zIndex=101;
        a.style.display="";
        a.style.left = scrollLeft+((clientWidth-a.offsetWidth)/2)+'px';
        a.style.top = scrollTop+((clientHeight-a.offsetHeight)/2)+'px';
}


/********************************************商品类型绑定*****************************************/
//所有的类目,结构说明:
// id:商品类型id,
// pid:父ID(关联categoryid), 
// layer:级别, 
// pidlist:父ID串, 
// name:商品类型名称,
// child:有无子结点]
//详情见javascript/goodscategories.js

//临时商品类型值
var tempgoodstypeinfo = "";
var tempgoodstypeid ="";

//分级数(默认是4级分类)
var categorylevelsum = 4;
//categorylevel元素名称
var categorylevelname = "categorylevel";
 
function clickCategory(e) {
    if (!e)
        var e = window.event;
    var cate = (e.target) ? e.target : e.srcElement;

    var categorylevel = $(categorylevelname+cate.layer);
        
    //设置当前分类列表中的选择状态    
    if(categorylevel.childNodes.length > 1) {
       for(var i = 0; i< categorylevel.childNodes.length ; i++) {
          if(categorylevel.childNodes[i].hasnode) { 
             categorylevel.childNodes[i].className =  "isfather";
          }
          else {
             categorylevel.childNodes[i].className =  "";
          }
       }
    }
  
    if(cate.layer < 3) {
        categorylevel = $(categorylevelname+(cate.layer+1));
        //加载新的子分类
        loadCategory(categorylevel,cate.id) ;
    }
    
    if(cate.hasnode) {
        cate.className = "isfather selected";
        categorylevelnum = (cate.layer+2);
    }
    else {
        cate.className = "selected";
        categorylevelnum = (cate.layer+1);
    }
    //清空当前级别下面的所有UL元素
    clearCategoryLevel(categorylevelnum);
    
    tempgoodstypeinfo = cate.innerHTML;
    tempgoodstypeid = cate.id;
}


//清空当前级别下面的所有UL元素
function clearCategoryLevel(startnum)
{
    for(; startnum< categorylevelsum ;startnum++) {
        categorylevel = $(categorylevelname+startnum);
        categorylevel.innerHTML = "";
        categorylevel.className = "blank";
    }
}

function loadCategory(obj, parentid, selectid)
{
    obj.className = "";
    obj.innerHTML = "";
   
    for(var i in  cats) {
        //找出以pid为父节点的所有类型
        if(parentid == cats[i].pid) {
            li = document.createElement('li');
            
            if(selectid != null && cats[i].id == selectid) {
                //alert(selectid);
                if(cats[i].child) {   //判断有无子结点
                    li.className = "isfather selected";
                }
                else {
                    li.className = "selected";
                }
            }
            else{   
                if(cats[i].child) {   //判断有无子结点
                    li.className = "isfather";
                }
                else {
                    li.className = "";
                }
            }
           
            li.onclick = clickCategory;
            li.hasnode = cats[i].child;
            li.id = cats[i].id;
            li.layer = cats[i].layer;
            if(cats[i].fid>0) //binded forumid
            {
                li.innerHTML = cats[i].name;//获取类型名称                
            }
            else
            {
                li.innerHTML = cats[i].name + "(*)";//获取类型名称
            }
            obj.appendChild(li);
        }
    }
}

//提交选择的分类
function submitCategory()
{
    if(enablemall == 2 || (tempgoodstypeid != "" && tempgoodstypeinfo.indexOf('(*)')<0))
    {
        $("goodstypeinfo").innerHTML = tempgoodstypeinfo;
        $("goodscategoryid").value = tempgoodstypeid;
        boxRemove('editcategoryinfo');
    }
    else
    {
        alert('请选取已绑定版块(不带*号)的商品分类');
    }
}

//设置已选择的当前分类
function setCategory(objid)
{
    var categoryid = $(objid).value;
    
    if(categoryid == "-1") {
        loadCategory($(categorylevelname+"0"), 0);
        return ;
    }
   
    if(categoryid!=null && categoryid!="") {
        var categroyinfo = "";
        for(var i in cats) {
            if(cats[i].id ==  categoryid) {
                categroyinfo = cats[i];
                break;
            }
        }
        
        //清空所有UL元素
        clearCategoryLevel(0);
        
        //判断pidlist字段是否为空
        //if(categroyinfo.pidlist != "" && categroyinfo.pidlist != "0") {alert(1);
            var curparentidlist = "0," +categroyinfo.pidlist + "," +categoryid;
            curparentidlist = curparentidlist.split(",");
            for( i = 0 ; i<= categroyinfo.layer; i++){
                loadCategory($(categorylevelname+i), curparentidlist[i],curparentidlist[i+1]);
            }
        //}
    }
}

/******************************************图片预览******************************************/

function PhotoView(path ,previewImage)
{
		if (path != "") {
			var patn = /\.jpg$|\.jpeg$|\.gif$/i;
			if (!patn.test(path))
			{
				clearFileInput($("upfilegoodspic"));
				previewImage.innerHTML = "暂无图片";
				alert("相册只允许jpg、jpeg、gif或png格式的图片!");
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
    var localfile = $("upfilegoodspic").value.substr($("upfilegoodspic").value.replace(/\\/g, '/').lastIndexOf('/') + 1);
    if(path == '') {
        return;
    }
    previewImage.innerHTML = "<img style=\"filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=\'scale\',src=\'" + path +"\');width:75;height:75;\" src=\"images/common/none.gif\" border=\"0\" alt=\"\" />";
}
