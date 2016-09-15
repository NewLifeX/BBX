<%@ Control Language="c#" Inherits="BBX.Web.Admin.OnlineEditor" Codebehind="onlineeditor.ascx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>

<% string coloroptions = "Black,Sienna,DarkOliveGreen,DarkGreen,DarkSlateBlue,Navy,Indigo,DarkSlateGray,DarkRed,DarkOrange,Olive,Green,Teal,Blue,SlateGray,DimGray,Red,SandyBrown,YellowGreen,SeaGreen,MediumTurquoise,RoyalBlue,Purple,Gray,Magenta,Orange,Yellow,Lime,Cyan,DeepSkyBlue,DarkOrchid,Silver,Pink,Wheat,LemonChiffon,PaleGreen,PaleTurquoise,LightBlue,Plum,White";%>
<div class="preview" style="display: none;" id="<%=ID%>message_view"></div>		
<div class="editor_tb" style="width:600px;">
	<span style="float:right">
	<a onclick="preview('<%=ID%>message_view','<%=ID%>message')" href="###" id="viewsignature">预览</a>		
	</span>
	<div>
		<a href="javascript:;" title="粗体" class="tb_bold" onclick="seditor_insertunit('<%=ID%>', '[b]', '[/b]')">B</a>
		<a href="javascript:;" title="颜色" class="tb_color" id="<%=ID%>forecolor" onclick="showMenu(this.id, true, 0, 2)">Color</a>
		<div class="popupmenu_popup tb_color" id="<%=ID%>forecolor_menu" style="display: none;width:114px;background:#FFF;border:1px solid #CCC;padding:10px;">			
			<%foreach(string colorname in coloroptions.Split(',')){%>
				<input type="button" style="background-color: <%=colorname%>" onclick="seditor_insertunit('<%=ID%>', '[color=<%=colorname%>]', '[/color]')" />
            <%}%>
		</div>
		<a href="javascript:;" title="图片" class="tb_img" onclick="seditor_insertunit('<%=ID%>', '[img]', '[/img]')">Image</a>
		<a href="javascript:;" title="链接" class="tb_link" onclick="seditor_insertunit('<%=ID%>', '[url]', '[/url]')">Link</a>
		<a href="javascript:;" title="代码" class="tb_code" onclick="seditor_insertunit('<%=ID%>', '[code]', '[/code]')">Code</a>
	</div>
</div>
<input type="hidden" name="<%=ID%>message_hidden" id="<%=ID%>message_hidden" value=""/>
<textarea rows="5" cols="80" id="<%=ID%>message" tabindex="2" class="txtarea" name="<%=ID%>message"  style="padding:0;width:600px;"><%=text%></textarea>