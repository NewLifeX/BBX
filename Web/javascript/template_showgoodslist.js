
/*******************************************************城市信息*********************************************************/
//数据文件详见javascript/locations.js

function initstate() {

    $("locus_1").onchange = function(e) {
        var length = 0;
        for(var i in locations) {
            if(locations[i].state == $("locus_1").value) {
                $("locus_2").options[length] = new Option(locations[i].city, locations[i].lid);
                length++;
            }
        }
        $("locus_2").options.length = length; 
    }
    
    var locus_1 = '';//省市
    var locus_2 = '';//城市
    if(locus != null && locus != '') {
        locus_1 = locus.split(',')[0];
        locus_2 = locus.split(',')[1];
    }
    
    $("locus_1").options.length = states.length+1; 
    $("locus_1").options[0] = new Option("----请选择省份----","-1");
    i = 1;
    for(var state in states) {        
        $("locus_1").options[i] = new Option(states[i-1].state, states[i-1].state);
        //当有选中的省市信息时
        if(locus_1 != '' && states[i-1].state == locus_1) {
            $("locus_1").options[i].selected='selected';
        }
        i++;
    }
    //当有选中的城市信息时
    if(locus_2 != '') {
        for(var i in locations) {
            if(locations[i].state == locus_1) {
                $("locus_2").options[i] = new Option(locations[i].city, locations[i].city);
                if(locations[i].city == locus_2) {
                    $("locus_2").options[i].selected='selected';
                }
            }
            i++;
        }
    }

}




function loadcategory(aspxrewrite) {
   var subcategories = $("categories");
   for(var i in cats) {
        li = document.createElement('li');
        li.id = cats[i].id;
        if(aspxrewrite==1) {
            li.innerHTML = '<a href="showgoodslist-' + cats[i].id + '.aspx">' + cats[i].name + '</a><em>(' +cats[i].gcount + ')</em>';
        }
        else {
            li.innerHTML = '<a href="showgoodslist.aspx?categoryid=' + cats[i].id + '">' + cats[i].name + '</a><em>(' +cats[i].gcount + ')</em>';
        }
        subcategories.appendChild(li);
   }
}
