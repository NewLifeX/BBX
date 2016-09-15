<%@ Page language="c#" Inherits="BBX.Web.Admin.templatetree" Codebehind="global_templatetree.aspx.cs" %>
<%@ Register TagPrefix="cc3" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head id="Head1">
<title>生成模板</title>
<link rev="stylesheet" media="all" href="../styles/default.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/tab.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">	
	/*function getCookie(name)//取cookies函数        
	{
		var arr = document.cookie.match(new RegExp("(^| )"+name+"=([^;]*)(;|$)"));
		if(arr != null) 
			return unescape(arr[2]); 
		return null;
	}*/

	function nodeCheckChanged(node)
	{
		var status = "未选取"; 
		if (node.Checked) status = "选取"; 
	}  
	
	function checkedEnabledButton1()
	{
		for (var i = 0; i < arguments[0].elements.length; i++)
		{
			var e = arguments[0].elements[i];
			if (e.type == "checkbox" && e.checked)
			{
				for(var j = 1; j < arguments.length; j++)
				{
					document.getElementById(arguments[j]).disabled = false;
				}
				return;
			}
		}
		for(var j = 1; j < arguments.length; j++)
		{
			document.getElementById(arguments[j]).disabled = true;
		}
	}
	
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton1(form,'TabControl1:tabPage22:CreateTemplate','TabControl1:tabPage22:DeleteTemplateFile')
	}
	
	/*function getTemplateList()
	{
		var commontemplate = getCookie("commontemplate");
		if(commontemplate == null) return;
		var tempstr = "";
		var filelist = commontemplate.split(",");
		for(var i = 0 ; i < filelist.length ; i++)
		{
			if(filelist[i].indexOf(".config") != -1)
			{
				tempstr += "<img src='../images/config.gif' />" + filelist[i] + "&nbsp;";
			}
			else
			{
				tempstr += "<img src='../images/htm.gif' />" + filelist[i] + "&nbsp;";
			}
		}
		return tempstr;
	}*/
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" runat="server">
	<%if(Request.Params["templateid"]=="1"){%>
	<uc1:PageInfo ID="info1" runat="server" Icon="information" Text="<ul><li>您正在修改默认模板,为了扩充其他模板的方便,强烈建议您不要对默认模板的内容进行修改. </li></ul><ul><li>点击相应的模板文件进行编辑</li></ul>" />
	<%}else{%>
	<uc1:PageInfo ID="PageInfo1" runat="server" Icon="information" Text="点击相应的模板文件进行编辑" />
	<%}%>
	&nbsp;&nbsp; <b>当前模板: <%=Request.Params["templatename"]%></b><br /><br />
	<table class="table1" cellspacing="0" cellpadding="4" width="100%" border="0">
		<tr>
			<td width="3"></td>
			<td>
				<!--常生成的模板:<span id="templatelist"></span>-->
				<cc3:TabControl id="TabControl1" SelectionMode="Client" runat="server" TabScriptPath="../js/tabstrip.js" width="760" height="100%">
					<cc3:TabPage Caption="模板文件" ID="tabPage22">
						<div style="OVERFLOW: auto;HEIGHT: 400px">
							<cc2:CheckBoxList id="TreeView1" runat="server" RepeatColumns="3"></cc2:CheckBoxList>
						</div>
						<br />
						<p style="text-align:right;">
							<input type="checkbox" id="chkall" name="chkall" onclick="Check(this.form);" />选择全部 &nbsp;&nbsp;
							<cc2:Button id="CreateTemplate" runat="server" Text=" 按选中的模板文件生成页面 " OnClick="CreateTemplate_Click"></cc2:Button>&nbsp;&nbsp;
							<cc2:Button id="DeleteTemplateFile" runat="server" Text="删除指定的模板文件" ButtonImgUrl="../images/del.gif" OnClick="DeleteTemplateFile_Click" OnClientClick="if(!confirm('你确认要删除所选模板文件吗？\n删除后将不能恢复！')) return false;"></cc2:Button>&nbsp;&nbsp;
							<button type="button" class="ManagerButton" onclick="window.location.href='global_templatevariable.aspx?templateid=<%=Request.Params["templateid"]%>&templatename=<%=Request.Params["templatename"]%>&path=<%=Request.Params["path"]%>'">模板变量列表</button>&nbsp;&nbsp;
							<button type="button" class="ManagerButton" id="Button3" onclick="window.location='global_templatesgrid.aspx';"><img src="../images/arrow_undo.gif"/> 返 回 </button>
						</p>
					</cc3:TabPage>
					<cc3:TabPage Caption="其它文件" ID="tabPage33">
						<div style="OVERFLOW: auto;HEIGHT: 400px" align="left">
							<asp:Repeater id="TreeView2" runat="server">
								<ItemTemplate>
										<%# Eval("filename")+""%><br />
								</ItemTemplate>
							</asp:Repeater>                    				
						</div>						
					</cc3:TabPage>
				</cc3:TabControl>
			</td>
		</tr>
	</table>
	<br />			
	<asp:label id="lblClientSideCheck" runat="server" CssClass="hint">&nbsp;</asp:label>
	<asp:label id="lblCheckedNodes" runat="server" CssClass="hint">&nbsp;</asp:label>
	<asp:label id="lblServerSideCheck" runat="server" CssClass="hint">&nbsp;</asp:label>
	<script type="text/javascript">
		  document.getElementById("lblClientSideCheck").innerText = document.getElementById("lblServerSideCheck").innerText;
		  //document.getElementById("templatelist").innerHTML = getTemplateList();
	</script>			
	<br />
</form>
<%=footer%>
</body>
</html>