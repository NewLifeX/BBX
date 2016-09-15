
//隐藏指定元素位置下方的"下拉列表框",该函数主题解决ie6以前(包括ie6)的下拉列表框遮挡显示层的问题
function HideOverSels(objID)
{
    var sels = document.getElementsByTagName('select'); 
  
    for (var i = 0; i < sels.length; i++) 
    {
         if (Obj1OverObj2(document.getElementById(objID), sels[i]))
         {
            
            sels[i].style.visibility = 'hidden';  
         }
         else
         {
            sels[i].style.visibility = 'visible';
         }
    }
}

function getLeftPosition(Obj) 
{
    try
    {
        for (var sumLeft=0;Obj!=document.body;sumLeft+=Obj.offsetLeft,Obj=Obj.offsetParent);
        return sumLeft;
    }
    catch(e)
    {}
}

function getTopPosition(Obj) 
{
    try
    {
        for (var sumTop=0;Obj!=document.body;sumTop+=Obj.offsetTop,Obj=Obj.offsetParent);
        return sumTop;
    }
    catch(e)
    {}
}

//判断obj1是否遮挡了obj2
function Obj1OverObj2(obj1, obj2)
{ 
  var result = true; 
  
  var obj1Left = getLeftPosition(obj1) - document.body.scrollLeft; 
  var obj1Top = getTopPosition(obj1)  - document.body.scrollTop; 
  var obj1Right = obj1Left + obj1.offsetWidth; 
  var obj1Bottom = obj1Top + obj1.offsetHeight;
  var obj2Left = getLeftPosition(obj2) - document.body.scrollLeft; 
  var obj2Top = getTopPosition(obj2) - document.body.scrollTop; 
  var obj2Right = obj2Left + obj2.offsetWidth; 
  var obj2Bottom = obj2Top + obj2.offsetHeight;
 

  if (obj1Right <= obj2Left || obj1Bottom <= obj2Top || obj1Left >= obj2Right || obj1Top >= obj2Bottom) 
  {
     result = false; 
  }
    
  return result; 
}


