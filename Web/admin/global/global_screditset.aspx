<%@ Page Language="c#" Inherits="BBX.Web.Admin.screditset" Codebehind="global_screditset.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>积分充值设置</title>
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/common.js"></script>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
	<script type="text/javascript" reload="1">
	    function customPartnerDisplayStatus(status) {
	        $("custompartnerset").style.display = (status == "0" ? "none" : "");
	    }
	    
	    function initRadioSelect(radioname) {
	        $(radioname).checked = true;
	    }

	    function testAlipayaccout() {
	        var accout = $("alipayaccount").value;
//	        var openpartner = $("usealipaycustompartnerid1").checked == true ? 1 : 0;
	        var openpartner = 1;
	        var partnerid = $("alipaypartnerid").value;
	        var partnerkey = $("alipaypartnercheckkey").value;
	        window.open('global_screditset.aspx?accout=' + accout + '&openpartner=' + openpartner + '&partnerid=' + partnerid + '&partnerkey= ' + partnerkey, '', '', '');
	    }

	    function validator() {
	        var cashtocreditsrate = $('cashtocreditsrate').value;
	        var mincreditsuserbuy = $('mincreditstobuy').value;
	        var maxcreditsuserbuy = $('maxcreditstobuy').value;
	        if (cashtocreditsrate < 0) {
	            alert('现金/积分兑换比率值不能为负数');
	            return false;
	        }
	        if (maxcreditsuserbuy-mincreditsuserbuy<0) {
	            alert('单次购买最大积分数额不能小于最小数额');
	            return false;
	        }
	    }
	</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="saveconfiginfo" runat="server">
<div class="ManagerForm">
	<fieldset>
	<legend style="background: url(../images/icons/icon25.jpg) no-repeat 6px 50%;">积分充值设置</legend>
	<div id="info1" style="text-align:left;border: 1px dotted rgb(219, 221, 211); margin: 10px 0pt; padding: 15px 10px 10px 56px; background: rgb(253, 255, 242) url(../images/hint.gif) no-repeat 20px 15px;clear: both;">
&nbsp;&nbsp;&nbsp;&nbsp;“支付宝”(http://www.alipay.com)是中国领先的网上支付平台，由全球最佳 B2B 公司阿里巴巴公司创建，为 <%=BBX.Common.Utils.ProductName%> 用户提供积分购买及论坛 B2C、C2C 交易平台。您只需进行简单的设置，即可使论坛内容和人气，真成为除广告收入外的重要利润来源，从而实现论坛的规模化经营。
由于涉及现金交易，为避免因操作不当而造成的资金损失，请在开始使用支付宝积分交易功能(不包含支付宝按钮功能)前，务必仔细阅读《用户使用说明书》中有关电子商务的部分，当确认完全理解和接受相关流程及使用方法后再进行相关设置。<br />
<a href="#" onclick="this.style.display='none';document.getElementById('alipayscript').style.display = 'block'">显示全部提示...</a>
<div id="alipayscript" style=" display:none;">
&nbsp;&nbsp;&nbsp;&nbsp;您可以设置允许用户通过现金在线支付的方式，为其交易积分账户充值，用于购买帖子内容、购买用户组权限、积分转账或用户组升级等功能。支付宝积分交易功能，需在“积分设置”中启用交易积分，并同时设置相应的积分策略以满足不同场合的需要。请务必正确设置您的收款支付宝账号，否则将造成用户付款后积分无法实时到账，造成大量需要人工处理的订单信息。
除 <%=BBX.Common.Utils.ProductName%> 官方网站或官方论坛另行通知以外，<%=BBX.Common.Utils.ProductName%> 提供的支付宝支付服务每笔交易收取 1.5% 的手续费。请及时关注相关业务的最新通知，各项政策或流程的变更、调整，以 <%=BBX.Common.Utils.ProductName%> 官方网站或官方论坛提供的信息为准。
您使用支付宝服务是建立在完全自愿的基础上，除 <%=BBX.Common.Utils.ProductName%> 因主观恶意的因素造成的资金损失以外，康盛创想(北京)科技有限公司不对因使用此功能造成的任何损失承担责任。
支付宝业务咨询 Email 为 6688@taobao.com；支付宝客户服务电话为 +86-0571-88156688。</div>
	</div>
	<table width="100%">
	<tr><td class="item_title" colspan="2">收款支付宝账号</td></tr>
	<tr>
		<td class="vtop rowform">
			<input id="alipayaccount" type="text" class="txt" name="alipayaccount" value="<%=configInfo.Alipayaccout%>"/>
		</td>
		<td class="vtop">如果开启兑换或交易功能，请填写真实有效的支付宝账号，用于收取用户以现金兑换交易积分的相关款项。如账号无效或安全码有误，将导致用户支付后无法正确对其积分账户自动充值，或进行正常的交易对其积分账户自动充值，或进行正常的交易。
		<p style="color:#000; padding-top:10px">如果想要查看近期用户充值情况，请进入<a href="global_creditsordermanage.aspx"style=" font-weight:700; padding-left:10px; text-decoration:underline; color:#FF0000">订单查询</a></p></td>
	</tr>
	<tbody id="custompartnerset" style="">
<%--    <script type="text/javascript">
        customPartnerDisplayStatus('<%=configInfo.Usealipaycustompartnerid %>');
    </script>--%>
	<tr><td class="item_title" colspan="2">交易安全校验码 (key)</td></tr>
	<tr>
		<td class="vtop rowform">
			<input id="alipaypartnercheckkey" type="text" class="txt" name="alipaypartnercheckkey" value="<%=configInfo.Alipaypartnercheckkey %>"/>
		</td>
		<td class="vtop">支付宝签约用户可以在此处填写支付宝分配给您的交易安全校验码，此校验码您可以到支付宝官方的商家服务功能处查看<br />(如果您还没有签约，请点击
		<a href="https://www.alipay.com/himalayas/practicality_customer.htm?customer_external_id=C4335344590036426018&market_type=from_agent_contract&pro_codes=21790F5A8C48B687F7F62F29651356BB" target="_blank">这里签约</a>,需要注意的是论坛积分充值需要签约用户支持即时到账业务)</td>
	</tr>
	<tr><td class="item_title" colspan="2">合作者身份 (partnerID)</td></tr>
	<tr>
		<td class="vtop rowform">
			<input id="alipaypartnerid" type="text" class="txt" name="alipaypartnerid" value="<%=configInfo.Alipaypartnerid %>"/>
		</td>
		<td class="vtop">支付宝签约用户请在此处填写支付宝分配给您的合作者身份，签约用户的手续费按照您与支付宝官方的签约协议为准</td>
	</tr>
<%--	<tr><td class="item_title" colspan="2">使用即时到帐接口</td></tr>
	<tr>
		<td class="vtop rowform">
		  <ul>
		    <li><input id="usealipayinstantpay1" type="radio" class="radio" value="1" name="usealipayinstantpay"/>是</li>
		    <li><input id="usealipayinstantpay0" type="radio" class="radio" value="0" name="usealipayinstantpay"/>否</li>
		    <script type="text/javascript" reload="1">
		        initRadioSelect('usealipayinstantpay<%=configInfo.Usealipaycustompartnerid %>');
		    </script>
		  </ul>
		</td>
		<td class="vtop">设置用户每月能够通过在线支付方式购买的交易积分的最大数额，单位为交易积分的单位，0 为不限制</td>
	</tr>--%>
	</tbody>
	<tr><td class="item_title" colspan="2">支付测试</td></tr>
	<tr>
		<td class="vtop rowform">
			<a href="#" onclick="testAlipayaccout();">积分充值订单测试</a><br/>
		</td>
		<td class="vtop">本测试将模拟提交 0.1元 人民币的订单进行测试，如果提交后成功出现付款界面，说明您论坛的支付宝功能可以正常使用(请勿做付款操作,能看见付款界面即可)</td>
	</tr>
	<tr><td class="item_title" colspan="2">现金/积分兑换比率</td></tr>
	<tr>
		<td class="vtop rowform">
			<input id="cashtocreditsrate" type="text" class="txt" name="cashtocreditsrate" value="<%=configInfo.Cashtocreditrate%>"/>
		</td>
		<td class="vtop">设置真实货币现金(以人民币元为单位)与论坛交易积分间的兑换比率，例如设置为 10，则 1 元人民币可以兑换 10 个单位的交易积分。本功能需开启交易积分，并成功进行支付宝收款账号的相关设置后方可使用，如果禁止使用现金与交易积分的兑换功能，请设置为 0</td>
	</tr>
	<tr><td class="item_title" colspan="2">单次购买最小积分数额</td></tr>
	<tr>
		<td class="vtop rowform">
			<input id="mincreditstobuy" type="text" class="txt" name="mincreditstobuy" value="<%=configInfo.Mincreditstobuy %>"/>
		</td>
		<td class="vtop">设置用户一次支付所购买的交易积分的最小数额，单位为交易积分的单位，0 为不限制(该值可能会跟随“现金/积分兑换比率”设置而自动变化以保证每个订单的最小金额为0.1元人民币)</td>
	</tr>
	<tr><td class="item_title" colspan="2">单次购买最大积分数额</td></tr>
	<tr>
		<td class="vtop rowform">
			<input id="maxcreditstobuy" type="text" class="txt" name="maxcreditstobuy" value="<%=configInfo.Maxcreditstobuy %>"/>
		</td>
		<td class="vtop">设置用户一次支付所购买的交易积分的最大数额，单位为交易积分的单位，0 为不限制</td>
	</tr>
	<tr><td class="item_title" colspan="2">用户每日最多可购买积分次数</td></tr>
	<tr>
		<td class="vtop rowform">
			<input id="userbuycreditscountperday" type="text" class="txt" name="userbuycreditscountperday" value="<%=configInfo.Userbuycreditscountperday %>"/>
		</td>
		<td class="vtop">设置用户每月能够通过在线支付方式购买的交易积分的最大数额，单位为交易积分的单位，0 为不限制</td>
	</tr>
	</table>
	<div class="Navbutton">
		<button id="save" type="submit" onclick="return validator();">提 交</button>
	</div>
	</fieldset>
</div>
</form>
<%=footer%>
</body>
</html>