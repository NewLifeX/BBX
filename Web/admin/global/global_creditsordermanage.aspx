<%@ Page Language="c#" Inherits="BBX.Web.Admin.creditsordermanage" Codebehind="global_creditsordermanage.aspx.cs" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="BBX.Common" %>
<%@ Import Namespace="BBX.Forum" %>
<%@ Import Namespace="BBX.Entity" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>积分充值订单查询</title>
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../styles/calendar.css">
    <script src="../js/calendar.js" language="javascript" />
    <script type="text/javascript" src="../js/common.js"></script>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
    <script type="text/javascript">
        function initSearchCondition() {
            $('orderstatus').value = -1;
            $('orderid').value = "";
            $('tradeno').value = "";
            $('buyer').value = "";
            $('submitstartdate').value = "";
            $('submitlastdate').value = "";
            $('confirmstartdate').value = "";
            $('confirmlastdate').value = "";
            $('initcondition').style.display = "none";
        }

        function isShowInitCondition() {
            if ($('orderstatus').value != -1 || $('orderid').value != "" || $('tradeno').value != "" || $('buyer').value != "" || $('submitstartdate').value != "" || $('submitlastdate').value != "" || $('confirmstartdate').value != "" || $('confirmlastdate').value != "") {
                $('initcondition').style.display = "";
            }
        }

        function goPageIndex(index) {
            var parms = 'page='+index;
            if ($('orderstatus').value != -1)
                parms += '&orderstatus=' + $('orderstatus').value;
            if ($('orderid').value != "")
                parms += '&orderid=' + $('orderid').value;
            if ($('tradeno').value = "")
                parms += '&tradeno=' + $('tradeno').value;
            if ($('buyer').value != "")
                parms += '&buyer=' + $('buyer').value;
            if ($('submitstartdate').value != "")
                parms += '&submitstartdate=' + $('submitstartdate').value;
            if ($('submitlastdate').value != "")
                parms += '&submitlastdate=' + $('submitlastdate').value;
            if ($('confirmstartdate').value != "")
                parms += '&confirmstartdate=' + $('confirmstartdate').value;
            if ($('confirmlastdate').value != "")
                parms += '&confirmlastdate=' + $('confirmlastdate').value;
            window.location.href = "global_creditsordermanage.aspx?" + parms + "#bottom";
        }
    </script>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<fieldset>
	<legend style="background: url(../images/icons/icon25.jpg) no-repeat 6px 50%;">积分充值设置</legend>
	<div id="info1" style="text-align:left;border: 1px dotted rgb(219, 221, 211); margin: 10px 0pt; padding: 15px 10px 10px 56px; background: rgb(253, 255, 242) url(../images/hint.gif) no-repeat 20px 15px;clear: both;">
          您可以通过设置下面给出的查询项目来查看您需要了解的订单信息。设置“积分充值功能”，请点击<a href="global_screditset.aspx"style=" font-weight:700; padding-left:10px; text-decoration:underline; color:#FF0000">积分充值设置</a>
	</div>
	<form id="searchchoice" runat="server" action="global_creditsordermanage.aspx#bottom">
	<table width="100%">
		<tr><td class="item_title" colspan="2">订单状态:</td></tr>
	    <tr>
		<td class="vtop rowform">
			<select id="orderstatus" name="orderstatus">
			   <option selected="selected" value="-1">全部状态</option>
			   <option value="0">等待付款</option>
			   <option value="2">交易成功</option>
			</select>
			<script type="text/javascript">
			    $('orderstatus').value=<%=status %>;
			</script>
		</td>
		<td class="vtop">设置关于订单状态的查询条件</td>
	    </tr>
	    <tr><td class="item_title" colspan="2">订单号:</td></tr>
	    <tr>
		<td class="vtop rowform">
		<input id="orderid" name="orderid" type="text" <%if (orderId > 0){%>value="<%=orderId%>"<%}%> />
		</td>
		<td class="vtop">设置关于订单号的查询条件</td>
	    </tr>
	    <tr><td class="item_title" colspan="2">支付宝订单号:</td></tr>
	    <tr>
		<td class="vtop rowform">
		<input id="tradeno" name="tradeno" type="text" value="<%=tradeNo %>"  />
		</td>
		<td class="vtop">设置关于支付宝订单号的查询条件</td>
	    </tr>
	    <tr><td class="item_title" colspan="2">付款用户名(多个用户名间请用半角逗号 "," 分割):</td></tr>
	    <tr>
		<td class="vtop rowform">
		<input id="buyer" name="buyer" type="text" value="<%=buyer %>"  />
		</td>
		<td class="vtop">设置关于付款用户的查询条件</td>
	    </tr>
	    <tr><td class="item_title" colspan="2">订单提交时间范围(yyyy-mm-dd):</td></tr>
	    <tr>
		<td class="vtop rowform">
		从 <input id="submitstartdate" name="submitstartdate" type="text" readonly="readonly" value="<%=submitStartDate %>"  /><img class="calendarimg" align="bottom" onclick="showcalendar(event, $('submitstartdate'))" src="../images/btn_calendar.gif"/> 
		开始<br/>到 <input id="submitlastdate" name="submitlastdate" type="text" readonly="readonly" value="<%=submitLastDate %>"  /><img class="calendarimg" align="bottom" onclick="showcalendar(event, $('submitlastdate'))" src="../images/btn_calendar.gif"/>
		</td>
		<td class="vtop">设置关于订单提交时间的查询条件</td>
	    </tr>
	    <tr><td class="item_title" colspan="2">订单确认时间范围(格式 yyyy-mm-dd):</td></tr>
	    <tr>
		<td class="vtop rowform">
		从 <input id="confirmstartdate" name="confirmstartdate" type="text" readonly="readonly" value="<%=confirmedStartDate %>" /><img class="calendarimg" align="bottom" onclick="showcalendar(event, $('confirmstartdate'))" src="../images/btn_calendar.gif"/> 
		开始<br />到 <input id="confirmlastdate" name="confirmlastdate" type="text" readonly="readonly" value="<%=confirmedLastDate %>" /><img class="calendarimg" align="bottom" onclick="showcalendar(event, $('confirmlastdate'))" src="../images/btn_calendar.gif"/>
		</td>
		<td class="vtop">设置关于订单确认时间的查询条件</td>
	    </tr>
	    <tr>
	    <td>
	       <input id="search" type="submit" value="搜 索"/> &nbsp;&nbsp;&nbsp;<a id="initcondition" style=" display:none;" onclick="initSearchCondition()" href="#">重置查询条件</a>
	    </td>
	    </tr>
	</table>
	<script type="text/javascript">
	    isShowInitCondition();
	</script>
	</form>
</fieldset>
</div>
	<table class="ntcplist">
	  <tbody>
	  <tr class="head">
	      <td><a href="#" name="bottom">订单列表</a></td>
	  </tr>
	  <tr>
	  <td>
	  <table id="orderlist" class="datalist" rules="all" cellspacing="0" border="1" align="center" style=" border-collapse:collapse;">
	    <tbody>
	     <tr class="category">
	        <td nowrap="nowrap" style="border: 1px solid rgb(234, 233, 225);">订单号</td>
	        <td nowrap="nowrap" style="border: 1px solid rgb(234, 233, 225);">支付宝订单号</td>
	        <td nowrap="nowrap" style="border: 1px solid rgb(234, 233, 225);">订单状态</td>
	        <td nowrap="nowrap" style="border: 1px solid rgb(234, 233, 225);">付款用户</td>
	        <td nowrap="nowrap" style="border: 1px solid rgb(234, 233, 225);">所得积分</td>
	        <td nowrap="nowrap" style="border: 1px solid rgb(234, 233, 225);">支付现金</td>
	        <td nowrap="nowrap" style="border: 1px solid rgb(234, 233, 225);">提交时间</td>
	        <td nowrap="nowrap" style="border: 1px solid rgb(234, 233, 225);">确认时间</td>
	     </tr>
	     <%foreach (Order order in orderList)
        {%>
	     <tr class="mouseoutstyle" onmouseout="this.className='mouseoutstyle'" onmouseover="this.className='mouseoverstyle'">
	        <td style="border: 1px solid rgb(234, 233, 225);"><%=order.ID%></td>
	        <td style="border: 1px solid rgb(234, 233, 225);"><%=order.TradeNo%></td>
	        <td style="border: 1px solid rgb(234, 233, 225);"><%=ConvertStatusNoToWord(order.Status)%></td>
	        <td style="border: 1px solid rgb(234, 233, 225);"><%=order.Buyer%></td>
	        <td style="border: 1px solid rgb(234, 233, 225);"><%=ForumUtils.ConvertCreditAndAmountToWord(order.Credit, order.Amount)%></td>
	        <td style="border: 1px solid rgb(234, 233, 225);">¥ <%=order.Price%>元</td>
	        <td style="border: 1px solid rgb(234, 233, 225);"><%=Utility.ToFullString(order.CreatedTime)%></td>
	        <td style="border: 1px solid rgb(234, 233, 225);"><%=Utility.ToFullString(order.ConfirmedTime)%></td>
	     </tr>
	     <%}%>
	    </tbody>
	  </table>
	  </td>
	  </tr>
	  <tr>
	  <td>
	  <div style="margin:30px 0; font-size:30px">
	  <table>
	     <tr>
	        <td><div style="margin-right:20px">共<%=orderCount%>条记录，当前第<%=pageIndex%>页</div></td>
	        <%if (pageIndex > 1){ %><td><a href="#" style="margin-right:10px" onclick="goPageIndex(1)">首页</a></td><td><a href="#" style=" margin-right:10px" onclick="goPageIndex(<%=pageIndex-1 %>)">上一页</a></td><%} %>
	        <%=ShowPageIndex()%>
	        <%if(pageIndex<pageCount){%><td><a style="margin-left:10px" href="#" onclick="goPageIndex(<%=pageIndex+1 %>);">下一页</a><a style="margin-left:10px" href="#" onclick="goPageIndex(<%=pageCount%>)">末页</a></td><%}%>
            <td><div style=" margin-left:10px">跳转到<input id="jumpindex" type="text" style="width:30px; height:18px" />页</div></td>
            <td><button onclick="goPageIndex($('jumpindex').value)">跳转</button></div></td>
	     </tr>
	  </table>
	  </div>
	  </td>
	  </tr>
	  </tbody>
	</table>
<%=footer%>
</body>
</html>
