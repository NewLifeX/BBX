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
