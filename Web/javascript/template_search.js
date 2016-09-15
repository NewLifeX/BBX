var keywordtypes = document.getElementsByName('keywordtype');
var keywordtype = '0';
for (var i = 0; i < keywordtypes.length; i++) {
    if (keywordtypes[i].checked) {
        keywordtype = keywordtypes[i].value;
        break;
    }
}
var postoptions = $('postoptions').innerHTML;
var spacepostoptions = $('spacepostoptions').innerHTML;
var albumoptions = $('albumoptions').innerHTML;
$('options_item').parentNode.removeChild($('options_item'));
switch (keywordtype) {//0:论坛主题 1:全文搜索,2:空间日志,3:相册,4:精华,8:按用户搜索
    case '1':
        changeoption('post');
        break;
    case '2':
        changeoption('spacepost');
        break;
    case '3':
        changeoption('album');
        break;
    default:
        changeoption('');
        break;
}

function changeoption(optionname) {
    switch (optionname) {
        case 'spacepost':
            $('options').innerHTML = spacepostoptions;
            break;
        case 'album':
            $('options').innerHTML = albumoptions;
            break;
        default:
            $('options').innerHTML = postoptions;
            break;
    }
    try {
        $('type').value = optionname;
    }
    catch (e) {
    }
}

function checkauthoroption(obj) {
    if (obj.checked) {
        $('divsearchtype').style.display = 'none';
        $('srchtxt').value = "";
        $('srchtxt').disabled = true;
    }
    else {
        $('divsearchtype').style.display = '';
        $('srchtxt').disabled = false;
    }
}
