<%@ Control Language="c#" Inherits="BBX.Web.Admin.TextareaResize" Codebehind="textarearesize.ascx.cs" %>
<script type="text/javascript">
function zoomtextarea(objname, zoom) 
{
 	zoomsize = zoom ? 10 : -10;
	obj = document.Form1.elements[objname];
	if(obj.rows + zoomsize > 0 && obj.cols + zoomsize * 3 > 0) {
		obj.rows += zoomsize;
		obj.cols += zoomsize * 3;
	}
}
</script>


<% if (this.HintInfo != "") { Response.Write("<span id=\"" + this.ClientID + "\"  onmouseover=\"showhintinfo(this," + this.HintLeftOffSet + "," + this.HintTopOffSet + ",'" + this.HintTitle + "','" + this.HintInfo + "','" + this.HintHeight + "','" + this.HintShowType + "');\" onmouseout=\"hidehintinfo();\">"); } %>

<%if(imagepath==""){%>
<img src="../images/zoomin.gif" onmouseover="this.style.cursor='hand'" onclick="zoomtextarea('<%=controlname%>:posttextarea', 1)" title="扩大" />
<img src="../images/zoomout.gif" onmouseover="this.style.cursor='hand'" onclick="zoomtextarea('<%=controlname%>:posttextarea', 0)" title="缩小" />
<%}else{%>
<img src="<%=imagepath%>/zoomin.gif" onmouseover="this.style.cursor='hand'" onclick="zoomtextarea('<%=controlname%>:posttextarea', 1)" title="扩大" />
<img src="<%=imagepath%>/zoomout.gif" onmouseover="this.style.cursor='hand'" onclick="zoomtextarea('<%=controlname%>:posttextarea', 0)" title="缩小" />
<%}%>
<br>
<textarea rows="5" cols="45" runat="server" id="posttextarea" onfocus="this.className='areamouseover';" onblur="this.className='';" ></textarea>

<% if (this.HintInfo != "") { Response.Write("</span>"); } %>
