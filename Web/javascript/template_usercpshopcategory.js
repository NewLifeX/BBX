var treeID = null;

function tree(treeID,treeNodes,funName){
var treeType = 0;
var layer = 0;
var parentidlist = '';
var title = '';
var lastDiv = null;

this.id = treeID;
this.drag = false;
this.dragObj = null;
this.oldClass = '';
this.treeType = 0;
this.layer = 0;
this.parentidlist = '';
this.title = '';
this.lastDiv = null

this.getPos=function(el,sProp){
	var iPos = 0;
	while (el!=null) {
		iPos+=el['offset' + sProp];
		el = el.offsetParent;
	}
	return iPos;
}

this.getETarget = function(e) {
	if (!e){
	    return null;
	}
	if (!e.srcElement && !e.target){
	    return null;
	}
	obj = e.srcElement ? e.srcElement : e.target;
	if (obj == null){
	    return null
	};
	
	while (obj.getAttribute('treetype') == null && obj.tagName != 'BODY' && obj.tagName != 'HTML'){
		obj = obj.parentNode;
		if(obj == null){
		    break;
		}
	}
	return obj;
}		

//鼠标按下
this.onmousedown = function(e){

if (!e){return false;}
		
		if (this.dragObj){
			this.dragObj.className = 'treenode_noselected';
		}
		
		this.dragObj = this.getETarget(e);
		if (!this.dragObj){return;}
		
		if (!this.dragObj.getAttribute('treetype')){return;}
		
		var mX = e.x ? e.x : e.pageX;
		var mY = e.y ? e.y : e.pageY;
		
		this.drag = true;
		this.dragObj.className = 'treenode_selected';
		this.oldClass = 'treenode_selected';
		
		var innerHTML = this.dragObj.innerHTML;
		
		innerHTML = innerHTML.substring(0, innerHTML.toLowerCase().indexOf('maxlength') +50);
       
		this.ShowMove(mX, mY, innerHTML);
		
		treeID = this;
		//document.onmousedown = function(){return false};
		document.onmousemove = function(e){
			if (tree==null){
				return;
			}
			treeID.document_onmousemove(e);
		}
		
};
		
this.onmouseup = function(e){this.document_onmouseup(e)};
this.onmouseover = function(e){
	if (this.drag){
		this.oldClass = this.getETarget(e).className;
		if (this.getETarget(e).getAttribute('treeType')==1){
			this.getETarget(e).className = 'treenode_over';
		}
		else{
			this.getETarget(e).className = 'treenode_0_over';				
		}
	}
	var mX = e.x ? e.x : e.pageX;
	var mY = e.y ? e.y : e.pageY;
};

this.onmouseout = function(e){
	if (this.drag){
		this.getETarget(e).className = this.oldClass;
		if (this.dragObj.id == this.getETarget(e).id){
			this.findObj(this.id + '_treenode_move').style.display = 'block';
		}
	}
	
};

this.init = function(){
	var outputhtml = '\n<div id="' + this.id + '_control" class="contral">';
	outputhtml += '\n<div id = "' + this.id + '_treenode_move" class="treenode_move"></div>';
	outputhtml += '\n<div id = "' + this.id + '_line"></div>';
	outputhtml += '\n<div class="treenode_0" style="display:none;" id = "' + this.id + '_treenode_0" onmouseup="' + this.id + '.onmouseup(event);" onmouseover="' + this.id + '.onmouseover(event)" onmouseout="' + this.id + '.onmouseout(event)"></div>';
	outputhtml += '\n<div class="treenode_1" style="display:none;" id = "' + this.id + '_treenode" onmousedown="' + this.id + '.onmousedown(event);" onmouseup="' + this.id + '.onmouseup(event);" onmouseover="' + this.id + '.onmouseover(event)" onmouseout="' + this.id + '.onmouseout(event)"></div>';
	for(i=0;i<treeNodes.length;i++){

		outputhtml += '\n<div class="treenode_0_noselected" id = "' + this.id + '_treenode' + i + '_0" index="' + i + '" treetype="0" ';
		outputhtml += 'onmouseup="' + this.id + '.onmouseup(event);" onmouseover="' + this.id + '.onmouseover(event)" onmouseout="' + this.id + '.onmouseout(event)">';
    	outputhtml += '</div>';
		outputhtml += '\n<div class="treenode_noselected" id = "' + this.id + '_treenode' + i + '" index="' + i + '" treetype="1" onmousedown="' + this.id + '.onmousedown(event);" onmouseup="' + this.id + '.onmouseup(event);" onmouseover="' + this.id + '.onmouseover(event)" onmouseout="' + this.id + '.onmouseout(event)">';
		if(treeNodes[i].parentid ==0) {
		     outputhtml += '<img src="admin/images/exp.gif">'; 
		}
		else {
		     for(j = 0; j < parseInt(treeNodes[i].layer); j++) {
		        outputhtml += '&nbsp;&nbsp;&nbsp;&nbsp;';
		     }
		     outputhtml += '<img src="admin/images/cal_nextMonth.gif"> ';
		}
		outputhtml += '<input type="text" value="'+treeNodes[i].name+'" size="20" id="categoryname_'+ treeNodes[i].categoryid +'" onChange="$(\'editcategoryname\').value=this.value;" onfocus="$(\'editcategoryname\').value=this.value;" maxlength="50">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="#" class="TopicButton" type="button" onclick="javascript:$(\'operation\').value=\'edit\';$(\'categoryid\').value='+ treeNodes[i].categoryid +';$(\'shopcategorysubmit\').click();"><img src="admin/images/submit.gif"/>确定修改</a><a href="#" class="TopicButton" type="button" onclick="javascript:if(confirm(\'您要删除该项吗?\')){$(\'operation\').value=\'delete\';$(\'categoryid\').value='+ treeNodes[i].categoryid +';$(\'shopcategorysubmit\').click();}"><img src="admin/images/del.gif" />删除</a>';
		if(treeNodes[i].childcount == 0) {
		    outputhtml +='<a href="#" class="TopicButton" type="button" onclick="javascript:window.location=\'usercpshopgoodsmanage.aspx?item=shopcategory&shopgoodscategoryid='+treeNodes[i].categoryid+'\';">商品列表<img src="admin/images/selector.gif" /></a></div>';
		}
		else {
		    outputhtml +='</div>';
		}
	}
	outputhtml += '\n</div>';
	
    $('treecategory').innerHTML = outputhtml;
}


	

this.findObj = function(objname){
	if (document.getElementById(objname)){
		return document.getElementById(objname);
	}
	else if(document.getElementsByName(objname)){
		return document.getElementsByName(objname)
	}
	else{
		return null;
	}
}
		
this.HideMove = function(){
	if (this.findObj(this.id + '_treenode_move').style.display!='none'){
		this.findObj(this.id + '_treenode_move').style.display='none';
	}
}

this.ShowMove = function(mX ,mY,innerHTML){
    
	this.findObj(this.id + '_treenode_move').innerHTML = innerHTML;
	this.findObj(this.id + '_treenode_move').style.left =(mX + 10) + 'px';
	this.findObj(this.id + '_treenode_move').style.top = (mY) + 'px';
}

this.SetMove = function(e,mX,mY){
	this.findObj(this.id + '_treenode_move').style.left =(mX + 10) + 'px';
	this.findObj(this.id + '_treenode_move').style.top = (mY) + 'px';			
}


this.document_onmouseup = function(e){
	if (this.drag){
		this.drag = false;
		try{
			this.getETarget(e).className = this.oldClass;
			
			index = this.getETarget(e).getAttribute('index');
			eval(funName+ '(treeNodes[index],treeNodes[this.dragObj.getAttribute("index")],this.getETarget(e).getAttribute("treetype"))');
		}
		catch(e){}
		
		this.HideMove();
		treeID = null;
		
		document.onmousedown = function(){return true};
		document.onmousemove = function(){null;};
	}

}
document.onmouseup = new Function(this.id + '.document_onmouseup();');

this.document_onmousemove = function(e){
	e= e ? e : window.event;
	var mX = e.x ? e.x : e.pageX;
	var mY = e.y ? e.y : e.pageY;
	this.SetMove(e,mX,mY);
	
}

    this.document_onselectstart = function(){
    return !this.drag;
}
	
    document.onselectstart = new Function('return ' + this.id + '.document_onselectstart ();');
    
	this.init();
}	
	 
	
function reSetTree(targetnode,sourcenode,treetype){

    if(targetnode.categoryid!=sourcenode.categoryid) { 
       var message='您是否要将分类"'+sourcenode.name+'"称动到分类"'+targetnode.name+'"';
       if(treetype==1) {
           message+='下吗 ?';
       }
       else {
	       message+='之前吗 ?';
       }
	  
       if(confirm(message)) {
          var objnparentidlist=','+targetnode.parentidlist+',';
          if(objnparentidlist.indexOf(','+sourcenode.categoryid+',')<0) {
               $('targetcategoryid').value = targetnode.categoryid;
               $('categoryid').value = sourcenode.categoryid;
               $('isaschildnode').value = treetype;
               $('operation').value = "move";
               $('shopcategorysubmit').click();
          }
          else {
             alert('不能将当前分类移动到其子分类');
          }
       }
    }
}

//function initcategory() {
//    $('selectcategoryid').options.length = cats.length; 
//    for(var i in cats) {
//        spacechar = '';
//        for(j = 0; j < cats[i].layer; j++) {
//            spacechar = spacechar + ' ';
//        }
//        $("selectcategoryid").options[i] = new Option(spacechar + cats[i].name, cats[i].categoryid);
//    }
//}
