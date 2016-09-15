<%@ Page language="c#" Inherits="BBX.Web.Admin.ForumDataCall" Codebehind="global_forumdatacall.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>论坛数据调用</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="<%=config.GetJqueryUrl()%>"></script>
<script type="text/javascript">
    function runCode(obj) {
        var code = obj.value; 
        var newwin = window.open('', '', '');
        newwin.opener = null;
        newwin.document.write(code);
        newwin.document.close();
    }
    function getParam() {
        if ($("#url").val() == "") {
            alert("您的网址未输入！");
            $("#url").focus(); 
            return;
        }
        var url = "";
        var form = $("#forumdatacall").get(0);
        for (var i = 0; i < form.elements.length; i++) {
            if (form.elements[i].value != "" && form.elements[i].id.indexOf("st_") == 0) {
                if ((form.elements[i].type == "text" && form.elements[i].value != form.elements[i].defaultValue) ||
                     (form.elements[i].type == "select-one" && form.elements[i].selectedIndex != 0) ||
                     (form.elements[i].type == "radio" && form.elements[i].checked && form.elements[i].value != 0)) {
                    if (form.elements[i].name != 'st_length')
                        url += form.elements[i].name + "=" + form.elements[i].value + "&";
                    else
                        url += "length=" + form.elements[i].value + "&";
                }
            }
        }
        if (url != "")
            url = "?" + url; 
        $("#scripttextarea").val("<script type=\"text/javascript\" src=\"" + $("#url").val() + "/tools/showtopics.aspx" + url.substring(0, url.length - 1) + "\"></sc" + "ript>");
    }
</script>
<style>
.item_title{text-align:right;width:140px;padding-right:18px;}
label{top:3px;}
</style>
</head>
<body>
<form id="forumdatacall">
<div class="ManagerForm">
<fieldset>
<legend style="background: url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">论坛数据调用</legend>
<table width="100%">
	<tr>
		<td class="item_title">输出的最大记录数:</td>
		<td>
			<input type="text" name="count" id="st_count" value="10" size="4" class="txt" />
		</td>
	</tr>
	<tr>
		<td class="item_title">访问量下限:</td>
		<td>
			<input type="text" name="views" id="st_views" value="-1" size="4"  class="txt"  />
		</td>
	</tr>
	<tr>
		<td class="item_title">时间类型:</td>
		<td>
			<select name="time" id="st_time">
				<option value="0">不限制</option>
				<option value="1">一天内</option>
				<option value="2">一周内</option>
				<option value="3">一月内</option>
			</select>
		</td>
	</tr>
	<tr>
		<td class="item_title">排序类型:</td>
		<td>
			<select name="order" id="st_order">
				<option value="0">按序号倒序</option>
				<option value="1">按访问量倒序</option>
				<option value="2">按最后回复倒序</option>
			</select>
		</td>
	</tr>
	<tr>
		<td class="item_title">版块ids:</td>
		<td>
			<input type="text" name="fid" id="st_fid" value="0" size="4"  class="txt" />(支持多版块调用，例如：1,2,3，0表示除密码和隐藏版块的所有版块)
		</td>
	</tr>
	<tr>
		<td class="item_title">是否精华:</td>
		<td>
			<label><input type="radio" name="digest" id="st_digest_1" value="1" />是</label><label><input type="radio" name="digest" id="st_digest_1" value="0" checked="checked" />否</label>
		</td>
	</tr>
	<tr>
		<td class="item_title">模板序号:</td>
		<td>
			<input type="text" name="template" id="st_template" value="0" size="4"  class="txt" />
		</td>
	</tr>
	<tr>
		<td class="item_title">编码:</td>
		<td>
			<input type="text" name="encoding" id="st_encoding" value="utf-8" size="8"  class="txt" />
		</td>
	</tr>
	<tr>
		<td class="item_title">缓存时间:</td>
		<td>
			<input type="text" name="cachetime" id="st_cachetime" value="20" size="4"  class="txt" />
		</td>
	</tr>
	<tr>
		<td class="item_title">标题长度:</td>
		<td>
			<input type="text" name="st_length" id="st_length" value="20" size="4"  class="txt" />
		</td>
	</tr>
	<tr>
		<td class="item_title">调用带有图片附件的帖子:</td>
		<td>
			<label><input type="radio" name="onlyimg" id="st_onlyimg" value="1" />是</label>&nbsp;&nbsp;<label><input type="radio" name="onlyimg" id="st_onlyimg1" value="0" checked="checked" />否</label>
		</td>
	</tr>
	<tr>
		<td class="item_title">图片显示类型:</td>
		<td>
			<input type="radio" name="type" id="st_type" value="0" checked="checked" />默认</label>&nbsp;&nbsp;
			<label><input type="radio" name="type" id="st_type1" value="1" />正方形</label>&nbsp;&nbsp;
			<label><input type="radio" name="type" id="st_type2" value="2" />原比例</label>
		</td>
	</tr>
	<tr>
		<td class="item_title">图片最大边长:</td>
		<td>
			<input type="text" name="imgsize" id="st_imgsize" value="-1" size="4" class="txt"  />
		</td>
	</tr>
	<tr>
		<td class="item_title">日志或相册调用:</td>
		<td>
			<select name="agg" id="st_agg">
				<option value="0"></option>
				<option value="1">更新的个人空间</option>
				<option value="2">推荐的个人空间</option>
				<option value="3">最新日志</option>
				<option value="4">推荐日志</option>
				<option value="5">推荐相册</option>
			</select>
		</td>
	</tr>
	<tr>
		<td class="item_title">您的网址:</td>
		<td>
			<input type="text" name="url" id="url" value="http://www.newlifex.com" class="txt"  />
		</td>
	</tr>
	<tr>
		<td class="item_title">&nbsp;</td>
		<td class="vtop" colspan="2" style="padding:12px 0;">
			<a href="javascript:getParam();">生成JS调用</a>&nbsp;&nbsp;
			<a href="http://www.newlifex.com/doc/default.aspx?cid=38" target="_blank">参数详解</a>
		</td>
	</tr>
	<tr>
		<td class="item_title">&nbsp;</td>
		<td class="vtop" colspan="2">
			<textarea name="scripttextarea" id="scripttextarea" dragover="true" onclick="this.focus();this.select()" cols="80" rows="5"></textarea>
		</td>
	</tr>
</table>
<div class="Navbutton" style="padding-left:160px;">
	<span><button onclick="runCode(this.offsetParent.getElementsByTagName('textarea')[0])" id="forumdatacall" class="ManagerButton" type="button"><img src="../images/submit.gif"> 运行代码</button></span>
</div>
</div>
</form>
<% =footer %>
</body>
</html>