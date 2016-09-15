<%@ Page language="c#" Inherits="BBX.Web.Admin.updateforumstatic" Codebehind="forum_updateforumstatic.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html >
<head>
<title>updateforumstatic</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" /> 
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">	
var lastnumber=0;
var currentnum=0;
 
function runstatic(opname,pertask)
{  
  if(lastnumber>-1)
  { 
	 currentnum=lastnumber/1+pertask/1;    // alert(currentnum);

	 var result;
	 switch (opname)      
	 {
		case "ReSetFourmTopicAPost":     result="\r\n重建论坛帖数";break
		case "ReSetUserDigestPosts":     result = "\r\n重建所有用户的精华贴数"; break
		case "ReSetUserPosts":           result = "\r\n重建分表" + lastnumber + " 的用户发帖数"; break
		case "ReSetTopicPosts": result = "\r\n重建分表" + lastnumber + " 的主题最后回复等信息"; break
		case "UpdatePostSP": result = "\r\n重建分表" + lastnumber + " 的存储过程"; break
		case "UpdateMyPost": result = "\r\n更新分表" + lastnumber + " 中我的帖子"; break
	 }
	 
	 document.getElementById('Layer5').innerHTML ="<br /><table><tr><td valign=top><img border=\"0\" src=\"../images/ajax_loading.gif\"  /></td><td valign=middle style=\"font-size: 14px;\" >"+result+"<BR /></td></tr></table><BR />";
	 document.getElementById('success').style.display ="block"; 
	
	 lastnumber=getReturn('../global/global_ajaxcall.aspx?opname='+opname+'&lastnumber='+lastnumber+'&pertask='+pertask);
	 if(lastnumber==null)
	 {
		document.getElementById('Layer5').innerHTML="<br />操作成功执行";
		document.getElementById('success').style.display = "block";
		count(); 
		document.getElementById('Form1').submit();
	 }
	}
  else
  {
	 document.getElementById('Layer5').innerHTML="<br />操作成功执行";
	 document.getElementById('success').style.display = "block";
	 count(); 
	 document.getElementById('Form1').submit();
  }
}

function clearflag()
{
	 bar=0;
	 document.getElementById('Layer5').innerHTML="<br />操作成功执行";
	 document.getElementById('success').style.display = "block";
	 count(); 
}


var bar=0;
function count()
{ 
		bar=bar+4;
		if (bar<99) {setTimeout("count()",100);} 
		else { document.getElementById('success').style.display ="none"; } 
}

function run(opname,pertask)
{
  if(pertask=="")
  {   
	  alert('每个循环更新数量不能为空!');return;
  }
  lastnumber=1;
  currentnum=0;
  bar=0;
  
  document.getElementById('Layer5').innerHTML="<br /><table><tr><td valign=top><img border=\"0\" src=\"../images/ajax_loading.gif\"  /></td><td valign=middle style=\"font-size: 14px;\" >正在处理数据, 请稍等...<BR /></td></tr></table><BR />";
  document.getElementById('success').style.display = "block";
  //runstatic(opname,pertask);
  setInterval('runstatic("'+opname+'",'+pertask+')',1000); //每次提交时间为1秒
}


var result=0;
function run2(opname,startvalue,endvalue)
{

  document.getElementById('Layer5').innerHTML="<br /><table><tr><td valign=top><img border=\"0\" src=\"../images/ajax_loading.gif\"  /></td><td valign=middle style=\"font-size: 14px;\" >正在处理数据, 请稍等...<BR /></td></tr></table><BR />";
  document.getElementById('success').style.display = "block";
  //alert(opname); 
  pageurl='../global/global_ajaxcall.aspx?opname='+opname+'&startvalue='+startvalue+'&endvalue='+endvalue;
  setTimeout('getforumdata("'+pageurl+'")',1000); //每次提交时间为5秒
}


function getforumdata(pageurl)
{
  result=getReturn(pageurl);
  if((result>0)||(result==null))
  {
		bar=0;
		document.getElementById('Layer5').innerHTML="<br />操作成功执行";
		document.getElementById('success').style.display = "block";
		count();
		document.getElementById('Form1').submit();
  }
  result=0;
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" runat="server">
<div style="width:98%;margin:0 auto;">
<table cellspacing="0" cellpadding="4" width="100%" align="center">
<tr>
	<td class="panelbox">
		<table width="100%">
			<tr>
				<td style="width:260px">重建论坛全部帖数:</td>
				<td style="width:260px">
					<!--每个循环更新数量:&nbsp;&nbsp;&nbsp;&nbsp;
					<cc1:TextBox id="pertask1" runat="server" Text="15" RequiredFieldType="暂无校验" size="5"></cc1:TextBox>-->
				</td>
				<td>
					<span id="ReSetFourmTopicAPost_id"  onmouseover="showhintinfo(this,0,0,'','重建论坛全部帖数','50','up');" onmouseout="hidehintinfo();">
						<span>
							<button type="button" id="ReSetFourmTopicAPost_id" class="ManagerButton" onclick="javascript:run('ReSetFourmTopicAPost','1');">
							<img src="../images/submit.gif" />提 交</button>
						</span>
					</span>
				</td>
			</tr>
			<tr>
				<td style="width:260px">重建所有版块的主题数:</td>
				<td style="width:260px"></td>
				<td><cc1:Button id="UpdateCurTopics" runat="server" Text="提 交" HintInfo="如果版块内主题数缺少或分页不准, 可执行此操作"></cc1:Button></td>
			</tr>
			<tr>
				<td style="width:260px">重建所有版块今日发帖数:</td>
				<td style="width:260px"></td>
				<td><cc1:Button id="ResetTodayPosts" runat="server" Text="提 交" HintInfo="如果版块内今日发帖数缺少或不准, 可执行此操作"></cc1:Button></td>
			</tr>
			<tr>
				<td>重建全部用户精华帖数:</td>
				<td>
					<!--每个循环更新数量:&nbsp;&nbsp;&nbsp;&nbsp;
					<cc1:TextBox id="pertask2" runat="server" Text="1000" RequiredFieldType="暂无校验" size="5"></cc1:TextBox>-->
				</td>
				<td>
					<span id="ReSetUserDigestPosts_id"  onmouseover="showhintinfo(this,0,0,'','重建全部用户精华帖数','50','up');" onmouseout="hidehintinfo();">
						<span>
							<button id="ReSetUserDigestPosts_id" type="button" class="ManagerButton" onclick="javascript:run('ReSetUserDigestPosts','1');">
								<img src="../images/submit.gif" />提 交
							</button>
						</span>
					</span>
				</td>
			</tr>
			<tr>
				<td>重建全部用户发帖数:</td>
				<td>
					每个循环更新的分表个数:&nbsp;&nbsp;&nbsp;&nbsp;
					<cc1:TextBox id="pertask3" runat="server" Text="1" RequiredFieldType="暂无校验" size="5"></cc1:TextBox>
				</td>
				<td>
					<span id="ReSetUserPosts_id"  onmouseover="showhintinfo(this,0,0,'','重建全部用户发帖数','50','up');" onmouseout="hidehintinfo();">
						<span>
							<button id="ReSetUserPosts_id" type="button" class="ManagerButton" onclick="javascript:run('ReSetUserPosts',pertask3.value);">
								<img src="../images/submit.gif" />提 交
							</button>
						</span>
					</span>
				</td>
			</tr>
			<tr>
				<td>重建全部主题的最后回复等信息:</td>
				<td>
					每个循环更新的分表个数:&nbsp;&nbsp;
					<cc1:TextBox id="pertask4" runat="server" Text="1" RequiredFieldType="暂无校验" size="5"></cc1:TextBox>
				</td>
				<td>
					<span id="ReSetTopicPosts_id"  onmouseover="showhintinfo(this,0,0,'','重建全部主题最后回复等信息','50','up');" onmouseout="hidehintinfo();">
						<span>
							<button id="ReSetTopicPosts_id" type="button" class="ManagerButton" onclick="javascript:run('ReSetTopicPosts',pertask4.value);" >
								<img src="../images/submit.gif" />提 交
							</button>
						</span>
					</span>	
				</td>
			</tr>
		</table>
	</td>
</tr>
</table>	
<hr style="border-top:0; border-bottom:1px solid #ccc;display:none;" size="1" />
<table cellspacing="0" cellpadding="4" width="100%" align="center" style="display:none">
	<tr>
		<td class="panelbox">
			<table width="100%">
				<tr>
					<td style="width:260px">重建指定论坛区间帖数:</td>
					<td style="width:260px">
						开始版块FID:
						<cc1:TextBox id="startfid" runat="server" Text="1" RequiredFieldType="数据校验" size="10"></cc1:TextBox><br />
						结束版块FID:
						<cc1:TextBox id="endfid" runat="server" Text="20" RequiredFieldType="数据校验" size="10"></cc1:TextBox>
					</td>
					<td>
						<span id="ReSetFourmTopicAPost_StartEnd_id"  onmouseover="showhintinfo(this,0,0,'','重建指定论坛区间帖数','50','up');" onmouseout="hidehintinfo();">
							<span>
								<button id="ReSetFourmTopicAPost_StartEnd_id" type="button" class="ManagerButton" 
								onclick="javascript:run2('ReSetFourmTopicAPost_StartEnd',startfid.value,endfid.value);">
									<img src="../images/submit.gif" />提 交
								</button>
							</span>
						</span>
					</td>
				</tr>
				<tr>
					<td>重建指定用户区间精华帖数:</td>
					<td>
						开始用户UID:
						<cc1:TextBox id="startuid_digest" runat="server" Text="1" RequiredFieldType="暂无校验" size="10"></cc1:TextBox><br />
						结束用户UID:
						<cc1:TextBox id="enduid_digest" runat="server" Text="20" RequiredFieldType="暂无校验" size="10"></cc1:TextBox>
					</td>
					<td>
						<span id="ReSetUserDigestPosts_StartEnd_id"  onmouseover="showhintinfo(this,0,0,'','重建指定用户区间精华帖数','50','up');" onmouseout="hidehintinfo();">
							<span>
								<button id="ReSetUserDigestPosts_StartEnd_id" type="button" class="ManagerButton" 
								onclick="javascript:run2('ReSetUserDigestPosts_StartEnd',startuid_digest.value,enduid_digest.value);">
									<img src="../images/submit.gif" />提 交
								</button>
							</span>
						</span>
					</td>
				</tr>
				<tr>
					<td>重建指定用户区间发帖数:</td>
					<td>
						开始用户UID:
						<cc1:TextBox id="startuid_post" runat="server" Text="1" RequiredFieldType="暂无校验" size="10"></cc1:TextBox><br />
						结束用户UID:
						<cc1:TextBox id="enduid_post" runat="server" Text="20" RequiredFieldType="暂无校验" size="10"></cc1:TextBox>
					</td>
					<td>
						<span id="ReSetUserPosts_StartEnd_id"  onmouseover="showhintinfo(this,0,0,'','重建指定用户区间发帖数','50','up');" onmouseout="hidehintinfo();">
							<span>
								<button id="ReSetUserPosts_StartEnd_id" type="button" class="ManagerButton" 
								onclick="javascript:run2('ReSetUserPosts_StartEnd',startuid_post.value,enduid_post.value);">
									<img src="../images/submit.gif" />提 交
								</button>
							</span>
						</span>
					</td>
				</tr>
				<tr>
					<td>重建主题回复信息:</td>
					<td>
						从分表:
						<cc1:TextBox id="starttid" runat="server" Text="" RequiredFieldType="暂无校验" size="10"></cc1:TextBox><br />
						到分表:
						<cc1:TextBox id="endtid" runat="server" Text="" RequiredFieldType="暂无校验" size="10"></cc1:TextBox>
					</td>
					<td>
						<span id="ReSetTopicPosts_StartEnd_id"  onmouseover="showhintinfo(this,0,0,'','重建指定主题区间帖数','50','up');" onmouseout="hidehintinfo();">
							<span>
								<button id="ReSetTopicPosts_StartEnd_id" type="button" class="ManagerButton" 
								onclick="javascript:run2('ReSetTopicPosts_StartEnd',starttid.value,endtid.value);">
									<img src="../images/submit.gif" />提 交
								</button>
							</span>
						</span>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>		
<hr style="border-top:0; border-bottom:1px solid #ccc;" size="1" />
<table cellspacing="0" cellpadding="4" width="100%" align="center">
	<tr>
		<td class="panelbox">
			<table width="100%">
				<tr>
					<td style="width:540px">清理移动标记:</td>
					<td><cc1:Button ID="SubmitClearFlag" runat="server" HintInfo="清理移动标记" Text="提 交" /></td>
				</tr>
				<tr>
					<td>重建论坛统计(表)数据:</td>
					<td><cc1:Button ID="ReSetStatistic" runat="server" HintInfo="重建论坛统计(表)数据" Text="提 交" /></td>
				</tr>
				<tr>
					<td>系统调整论坛版块:</td>
					<td><cc1:Button ID="SysteAutoSet" runat="server" HintInfo="系统调整论坛版块,对论坛版块表中的链接, 子版数等相关内容进行调整. " Text="提 交" /></td>
				</tr>
				<asp:Panel ID="UpdateStoreProcPanel" Visible="true" runat="server">
				<tr>
					<td>更新分表存储过程:</td>
					<%--<td><cc1:Button ID="UpdatePostSP" runat="server" HintInfo="更新分表存储过程" Text="提 交" /></td>--%>
					<td>
					    <span id="UpdatePostSP"  onmouseover="showhintinfo(this,0,0,'','更新分表存储过程','50','up');" onmouseout="hidehintinfo();">
						<span>
							<button id="UpdatePostSP" type="button" class="ManagerButton" onclick="javascript:run('UpdatePostSP','1');">
								<img src="../images/submit.gif" />提 交
							</button>
						</span>
					</span>
					</td>
				</tr>
				</asp:Panel>
				<tr>
					<td>为未建立全文索引的帖子表建立全文索引:</td>
					<td>
						<cc1:Button id="CreateFullTextIndex" runat="server" Text="提 交" 
						HintInfo="为未建立全文索引的帖子表建立全文索引,同时为已建全文索引的帖子表进行完全填充,操作时间与帖子数据量多少有关. 操作的结果要参见SqlServer相应数据库的“全文目录”的填充进度.">
						</cc1:Button>
					</td>
				</tr>
				<%--<tr>
					<td>更新所有版块的当前帖数:</td>
					<td><cc1:Button id="UpdateCurTopics" runat="server" Text="提 交" HintInfo="如果版块内主题数缺少或分页不准, 可执行此操作"></cc1:Button></td>
				</tr>--%>
				<tr>
					<td>更新所有版块最后发帖:</td>
					<td><cc1:Button ID="UpdateForumLastPost" runat="server" HintInfo="更新版块最后发帖" Text="提 交" /></td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<hr style="border-top:0; border-bottom:1px solid #ccc;" size="1" />
<table cellspacing="0" cellpadding="4" width="100%" align="center">
	<tr>
		<td class="panelbox">
			<table width="100%">
				<tr>
					<td style="width:540px">更新我的主题:</td>
					<td><cc1:Button ID="UpdateMyTopic" runat="server" HintInfo="更新我的主题" Text="提 交" /></td>
				</tr>
				<tr>
					<td>更新我的帖子:</td>
					<%--<td><cc1:Button ID="UpdateMyPost" runat="server" HintInfo="更新我的帖子" Text="提 交" /></td>--%>
					<td>
					    <span id="UpdateMyPost"  onmouseover="showhintinfo(this,0,0,'','更新我的帖子','50','up');" onmouseout="hidehintinfo();">
						<span>
							<button id="UpdateMyPost" type="button" class="ManagerButton" onclick="javascript:run('UpdateMyPost','1');">
								<img src="../images/submit.gif" />提 交
							</button>
						</span>
					</span>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
  </div>
</form>
<%=footer%>	
</body>
</html>