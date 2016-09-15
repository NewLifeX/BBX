var colorOptions = new Array('Black', 'Sienna', 'DarkOliveGreen', 'DarkGreen', 'DarkSlateBlue', 'Navy', 'Indigo', 'DarkSlateGray', 'DarkRed', 'DarkOrange',
    'Olive', 'Green', 'Teal', 'Blue', 'SlateGray', 'DimGray', 'Red', 'SandyBrown', 'YellowGreen', 'SeaGreen', 'MediumTurquoise', 'RoyalBlue', 'Purple', 'Gray', 'Magenta', 'Orange',
    'Yellow', 'Lime', 'Cyan', 'DeepSkyBlue', 'DarkOrchid', 'Silver', 'Pink', 'Wheat', 'LemonChiffon', 'PaleGreen', 'PaleTurquoise', 'LightBlue', 'Plum', 'White');

var SimplyEditor = function (instanceName, contentName, cssdir, text, width, rows) {
    this.instanceName = instanceName;
    this.contentName = contentName;
    width = width ? width : 600;
    rows = rows ? rows : 5;
    text = text ? text : '';
    InitEditor(this, instanceName, contentName, cssdir, width, rows, text);

    this.textArea = $(instanceName + 'message');
    this.textValue = $(instanceName + 'message_hidden');
}

function InitEditor(obj, instanceName, contentName, cssdir, width, rows, text) {
    var editorContent = $(contentName);
    var editorCss = document.createElement('LINK');
    editorCss.id = instanceName + '_css';
    editorCss.href = cssdir + '/seditor.css';
    editorCss.rel = 'stylesheet';
    editorCss.type = 'text/css';

    editorContent.appendChild(editorCss);
    var previewContent = document.createElement('DIV');
    previewContent.id = instanceName + 'message_view';
    previewContent.style.display = 'none';
    previewContent.className   = 'preview';

    var editor_tb = document.createElement('DIV');
    editor_tb.className  = 'editor_tb';
    editor_tb.style.width = width + 'px';

    //span
    var preview_span = document.createElement('SPAN');
    preview_span.style.styleFloat = 'right';
    preview_span.style.cssFloat = 'right';

    var preview_a = document.createElement('A');
    preview_a.id = instanceName + '_viewsignature';
    preview_a.href = '###';
    preview_a.innerHTML = '预览';
    preview_a.onclick = function on() {
        obj.Preview();
    }

    preview_span.appendChild(preview_a);
    //span end

    //controlPanel
    var editorControlPanel = document.createElement('DIV');
    var bold = document.createElement('A');
    bold.href = 'javascript:;';
    bold.className  = 'tb_bold';
    bold.title = '粗体';
    bold.onclick = function on() {
        seditor_insertunit(instanceName, '[b]', '[/b]');
        obj.SyncText();
    }
    bold.innerHTML = 'B';

    editorControlPanel.appendChild(bold);

    var forecolor = document.createElement('A');
    forecolor.id = instanceName + 'forecolor';
    forecolor.href = 'javascript:;';
    forecolor.className  = 'tb_color';
    forecolor.title = '颜色';
    forecolor.onclick = function on() {
        showMenu(this.id, true, 0, 2);
    }
    forecolor.innerHTML = 'Color';

    editorControlPanel.appendChild(forecolor);

    var forecolor_menu = document.createElement('DIV');
    forecolor_menu.className  = 'popupmenu_popup tb_color';
    forecolor_menu.id = instanceName + 'forecolor_menu';
    forecolor_menu.style.display = 'none';
    forecolor_menu.style.width = '114px';
    forecolor_menu.style.background = '#FFF';
    forecolor_menu.style.border = '1px solid #CCC';
    forecolor_menu.style.padding = '10px';
    for (var i = 0; i < colorOptions.length; i++) {
        var colorBtn = document.createElement('INPUT');
        colorBtn.type = 'button';
        colorBtn.style.backgroundColor = colorOptions[i];
        colorBtn.onclick = function on() {
            seditor_insertunit(instanceName, '[color=' + this.style.backgroundColor + ']', '[/color]');
            obj.SyncText();
        }
        forecolor_menu.appendChild(colorBtn);
    }

    editorControlPanel.appendChild(forecolor_menu);

    var img = document.createElement('A');
    img.href = 'javascript:;';
    img.title = '图片';
    img.className  = 'tb_img';
    img.onclick = function on() {
        seditor_insertunit(instanceName, '[img]', '[/img]');
        obj.SyncText();
    }
    img.innerHTML = 'Image';

    editorControlPanel.appendChild(img);

    var link = document.createElement('A');
    link.href = 'javascript:;';
    link.title = '链接';
    link.className  = 'tb_link';
    link.onclick = function on() {
        seditor_insertunit(instanceName, '[url]', '[/url]');
        obj.SyncText();
    }
    link.innerHTML = 'Link';

    editorControlPanel.appendChild(link);

//    var code = document.createElement('A');
//    code.href = 'javascript:;';
//    code.title = '代码';
//    code.className = 'tb_code';
//    code.onclick = function on() {
//        seditor_insertunit(instanceName, '[code]', '[/code]');
//    }
//    code.innerHTML = 'Code';

//    editorControlPanel.appendChild(code);
    //controlPanel end

    //editor tb
    editor_tb.appendChild(preview_span);
    editor_tb.appendChild(editorControlPanel);
    //editor tb end

    var messageHidden = document.createElement('TEXTAREA');
    messageHidden.name = instanceName;
    messageHidden.id = instanceName + 'message_hidden';
    messageHidden.style.display = 'none';
    messageHidden.value = text;
    var editorTextArea = document.createElement('TEXTAREA');
    editorTextArea.rows = rows;
    editorTextArea.cols = '80';
    editorTextArea.id = instanceName + 'message';
    editorTextArea.tabindex = '2';
    editorTextArea.className  = 'txtarea';
    editorTextArea.name = instanceName + 'area';
    editorTextArea.style.padding = '0';
    editorTextArea.style.width = width + 'px';

    editorTextArea.value = text;
//    if (is_ie) {
//        editorTextArea.value = text;
//    }
//    else {
//        editorTextArea.innerHTML = text;
//    }
    editorTextArea.onchange = function on() {
        obj.SyncText();
    }

    editorContent.appendChild(previewContent);
    editorContent.appendChild(editor_tb);
    editorContent.appendChild(messageHidden);
    editorContent.appendChild(editorTextArea);
}

SimplyEditor.prototype.GetHTML = function on() {
    if (is_ie) {
        return $(this.instanceName + 'message_hidden').value;
    }
    else
        return $(this.instanceName + 'message_hidden').textContent;
}

SimplyEditor.prototype.Preview = function on() {
    if (mb_strlen($(this.instanceName + 'message_view').innerHTML) == 0) {
        $(this.instanceName + 'message_view').innerHTML = parseubb(this.textArea ? this.textArea.value : '');
        $(this.instanceName + 'message_view').style.display = '';
    }
    else {
        $(this.instanceName + 'message_view').style.display = 'none';
        $(this.instanceName + 'message_view').innerHTML = '';
    }
}

SimplyEditor.prototype.SyncText = function on() {
    if (is_ie) {
        $(this.instanceName + 'message_hidden').value = parseubb(this.textArea.value);
    }
    else {
        $(this.instanceName + 'message_hidden').textContent = parseubb(this.textArea.value);
        $(this.instanceName + 'message_hidden').value = parseubb(this.textArea.value);
    }
}


