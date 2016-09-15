<%@ Control Language="c#" Inherits="BBX.Web.Admin.forumtree" Codebehind="forumstree.ascx.cs" %>
<%if(WithCheckBox){%>

<style type="text/css">
.input1
 {
    border:0px;
	padding-right: 0px; 
	padding-left: 0px;
	padding-bottom: 0px; 
	padding-top: 0px; 
	height:18 px;
	vertical-align:middle;
	margin: 0px;
 }
 
.treetd
{
    border: 0px;
    width: 100%;
    white-space:nowrap; 
  
} 
 
.treeimg
 {
    align:absmiddle;
    vertical-align:middle;
    border:0px;
    height:16px
 }
 
</style>

<script type="text/javascript">
function CheckAllTreeNode(form)
  {
  for (var i=0;i<form.elements.length;i++)
    {
    var e = form.elements[i];
    if (e.name != '<%=this.ClientID%>_chkall')
       e.checked = form.<%=this.ClientID%>_chkall.checked;
    }
}


function CheckAllTreeByName(form,name,noname)
{
  for (var i=0;i<form.elements.length;i++)
    {
	    var e = form.elements[i];
	    if(e.name.indexOf(name)>=0)
		{
           if(noname!="")
           {
              if(e.name.indexOf(noname)>=0) ;
              else
              {
                 e.checked = form.<%=this.ClientID%>_chkall.checked;
              }
              //alert(e.name+' '+noname);
		   }	  
		   else
		   {
		      e.checked = form.<%=this.ClientID%>_chkall.checked;
		   }
	    }
	}
}

function Tree_SelectOneNode(obj)
{
	if( obj.checked == false)
	{
		document.getElementById('<%=this.ClientID%>_CheckAll').checked = obj.checked;
	}
}
</script>
<%}%>

<% if (this.HintInfo != ""){Response.Write("<span id=\"" + this.ClientID + "\"  onmouseover=\"showhintinfo(this," + this.HintLeftOffSet + "," + this.HintTopOffSet + ",'" + this.HintTitle + "','" + this.HintInfo + "','" + this.HintHeight + "','" + this.HintShowType + "');\" onmouseout=\"hidehintinfo();\">");} %>

<asp:Literal id="TreeContent" runat="server"></asp:Literal>

<% if (this.HintInfo != "") { Response.Write("</span>"); } %>