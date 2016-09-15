<%@ Control Language="c#" Inherits="BBX.Web.Admin.ajaxtopicinfo" Codebehind="ajaxtopicinfo.ascx.cs" AutoEventWireup="false"%>
<br />

<div style="width:100%" align=center>
        <table class="table1" cellspacing="0" cellpadding="4" width="100%" align="center">
        <tr>
		<td colspan="2">
		<table class="ntcplist" >
            <tr class="head">
              <td>&nbsp;&nbsp;候选主题列表</td>
            </tr>
            <tr>
            <td>
	          <table class="datalist" cellspacing="0" rules="all" border="1" id="DataGrid1" style="border-collapse:collapse;">
                  <tr class="category">
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">选择</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">帖子标题</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">作者</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">所属版块</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">发布日期</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">回复数</td>
                  </tr>
                  <%
                      foreach (BBX.Entity.Topic tp in topics)
                    {
                        if (tp.Title.Length > 30)
                        {
                            tp.Title = tp.Title.Substring(0, 30) + "...";
                        }
                   %>
                  <tr class="datagridItem" onmouseover="this.className='mouseoverstyle'" onmouseout="this.className='mouseoutstyle'" style="cursor:hand;">
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">
			        <input id="tid<%=tp.ID %>" onclick="javascript:insertElement('tid',this.value,$('title'+this.value).innerHTML,this.checked)" type="checkbox" value="<%=tp.ID %>" name="topicid"></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;" align="left"><a href="../../showtopic.aspx?topicid=<%=tp.ID %>" target="_blank"><span id="title<%=tp.ID %>"><%=tp.Title %></span></a></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;">
                    <%if(tp.PosterID == -1){ %>
                        <%=tp.Poster %>
                    <%}else{ %>
                        <a href="../../userinfo.aspx?userid=<%=tp.PosterID %>" target="_blank"><%=tp.Poster %></a>
                    <%} %>
                    </td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><a href="../../showforum.aspx?forumid=<%=tp.Fid %>" target="_blank"><%=tp.ForumName %></a></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><%=tp.PostDateTime %></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><%=tp.Replies %></td>
                  </tr>
                  <%} %>
                  <tr class="datagridPager">
	                <td align="left" valign="bottom" colspan="6" style="border-width:0px;"><%=pagelink %></td>
                  </tr>
              </table></td>
            </tr>
          </TABLE>
        </td>
        </tr>
        </table>
</div>
