function bindshopgoodscategory(goodscategorylist, goodsid){
   for(i in cats) {
       if(goodscategorylist.indexOf(','+cats[i].categoryid+",")>=0) {
            document.write(cats[i].name + ' <a href="#" onclick="javascript:$(\'operation\').value=\'removecategory\';$(\'removeshopgoodscategoryid\').value='+cats[i].categoryid+';$(\'removegoodsid\').value='+goodsid+';$(\'shopcategorysubmit\').click();">[移除分类]</a> <br />');
       }
   }
}

function checkgoodsbox(form,objtag) {
    for(var i = 0; i < form.elements.length; i++) {
        var e = form.elements[i];
        if(e.name == "goodsid") {
            e.checked = objtag.checked;
        }
    }
    objtag.checked = !objtag.checked;
}


function selectcategory(obj, value) {
   for(i in obj.options) {
      if(obj.options[i].value==value) {
          obj.options[i].selected=true
      }
   }   
}
