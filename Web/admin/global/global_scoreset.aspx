<%@ Page Language="c#" Inherits="BBX.Web.Admin.scoreset" Codebehind="global_scoreset.aspx.cs" %>
<%@ Register TagPrefix="cc2" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc3" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>积分设置</title>
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/common.js"></script>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
	<script type="text/javascript">
	    function creditsTransStatus(status)
	    {
	        document.getElementById("creditstransLayer").style.display = (status == "0" ? "none" : "block");
	    }
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
<script type="text/javascript" src="../../javascript/common.js"></script>
<script language="JavaScript" type="text/javascript" src="../../javascript/ajax.js"></script>
<script type="text/javascript">
    function UpdateCredit(startuid) {
        var formula = $("TabControl1_tabPage51_formula").value;
        var url = "formula=" + encodeChar(formula) + "&start_uid=" + startuid;
        _sendRequest('../global/global_ajaxcall.aspx?opname=updateusercreditbyformula', updateusercredit_callback, false, url);
    }

    function encodeChar(str) {
        return encodeURI(str.replace(/\+/g, '_plus_').replace(/\&/g, '_and_').replace(/\=/g, '_equal_'));
    }

    function updateusercredit_callback(doc) {
        var data = eval(doc);
        if (data[0] == undefined) {
            $('Layer5').innerHTML = '运行错误，系统重新加载当前页面!';
            window.location.href = "global_scoreset.aspx";
        }
        else {
            var updatecount = parseInt(data[0].count);
            if (updatecount == 0) {
                $('Layer5').innerHTML = "<br />用户积分信息更新成功!";
                window.location.href = "global_scoreset.aspx";
            }
            else {
                if (isNaN(updatecount))
                    updatecount = 0;
                count += updatecount;
                $('Layer5').innerHTML = "<br />已更新了" + count + " 名用户积分信息";
                UpdateCredit(data[0].startuid);
            }
        }
    }

    var count; //统计已操作的记录数

    function submit_Click() {
        //        if (!confirm('你确认要更新用户积分吗?')) {
        //            $("SendPM").disabled = false;
        //            return false;
        //        }
        $("Save").disabled = true;
        $('Layer5').innerHTML = "<br />正在更新用户积分...";
        $('success').style.display = "block";
        count = 0;
        UpdateCredit(0);
    }   
</script>
</head>
<body>
<form id="Form1" runat="server">
<cc3:TabControl ID="TabControl1" SelectionMode="Client" runat="server" TabScriptPath="../js/tabstrip.js" Width="660" Height="100%">
<cc3:TabPage Caption="基本信息" ID="tabPage51">	
	<table cellspacing="0" cellpadding="4" width="100%" align="center">
	<tr>
		<td>
			<cc1:DataGrid ID="DataGrid1" runat="server" OnCancelCommand="DataGrid_Cancel" OnEditCommand="DataGrid_Edit" OnUpdateCommand="DataGrid_Update">
				<Columns>
					<asp:BoundColumn DataField="id" SortExpression="id [递增]" Visible="false"></asp:BoundColumn>
					<asp:BoundColumn DataField="name" SortExpression="name" HeaderText="名称" ReadOnly="true"></asp:BoundColumn>
					<asp:BoundColumn DataField="extcredits1" HeaderText="extcredits1"></asp:BoundColumn>
					<asp:BoundColumn DataField="extcredits2" HeaderText="extcredits2"></asp:BoundColumn>
					<asp:BoundColumn DataField="extcredits3" HeaderText="extcredits3"></asp:BoundColumn>
					<asp:BoundColumn DataField="extcredits4" HeaderText="extcredits4"></asp:BoundColumn>
					<asp:BoundColumn DataField="extcredits5" HeaderText="extcredits5"></asp:BoundColumn>
					<asp:BoundColumn DataField="extcredits6" HeaderText="extcredits6"></asp:BoundColumn>
					<asp:BoundColumn DataField="extcredits7" HeaderText="extcredits7"></asp:BoundColumn>
					<asp:BoundColumn DataField="extcredits8" HeaderText="extcredits8"></asp:BoundColumn>
				</Columns>
			</cc1:DataGrid>
		</td>
	</tr>
	</table>
	<div style="border: 1px dotted rgb(219, 221, 211); margin: 10px 0pt; padding: 15px 10px 10px 56px; background: rgb(253, 255, 242) url(../images/hint.gif) no-repeat 20px 15px; clear: both;" id="info1">
    您可以通过设置负值的方式来扣除某一操作的积分值, 各项积分增减允许的范围为-999～+999. 如果为更多的操作设置积分策略, 系统就需要更频繁的更新用户积分, 同时意味着消耗更多的系统资源, 因此请根据实际情况酌情设置
	<table class="table1" cellspacing="0" cellpadding="4" width="100%" align="center">
	<tr>
		<td width="100">兑换比率</td>
		<td>
			兑换比率为单项积分对应一个单位标准积分的值, 例如 extcredits1 的比率为 1.5(相当于 1.5 个单位标准积分)、extcredits2 的比率为
			3(相当于 3 个单位标准积分)、extcredits3 的比率为 15(相当于 15 个单位标准积分), 则 extcredits3 的 1 分相当于 extcredits2
			的 5 分或 extcredits1 的 10 分. 一旦设置兑换比率, 则用户将可以在控制面板中自行兑换各项设置了兑换比率的积分, 如不希望实行积分自由兑换,
			请将其兑换比率设置为 0
		</td>
	</tr>
	<tr>
		<td>积分名称</td>
		<td>该项积分的名称, 如果为空则不启用该项积分显示</td>
	</tr>
	<tr>
		<td>积分单位</td>
		<td>如金币,元等</td>
	</tr>
	<tr>
		<td>发主题</td>
		<td>作者发新主题增加的积分数, 如果该主题被删除, 作者积分也会按此标准相应减少</td>
	</tr>
	<tr>
		<td>回复</td>
		<td>作者发新回复增加的积分数, 如果该回复被删除, 作者积分也会按此标准相应减少</td>
	</tr>
	<tr>
		<td>加精华</td>
		<td>主题被加入精华时单位级别作者增加的积分数(根据精华级别乘以1～3), 如果该主题被移除精华, 作者积分也会按此标准相应减少</td>
	</tr>
	<tr>
		<td>上传附件</td>
		<td>用户每上传一个附件增加的积分数, 如果该附件被删除, 发布者积分也会按此标准相应减少</td>
	</tr>
	<tr>
		<td>下载附件</td>
		<td>用户每下载一个附件扣除的积分数. 注意: 分值为负数时才能扣除相应的积分.如果允许游客组下载附件, 本策略将可能被绕过</td>
	</tr>
	<tr>
		<td>发短消息</td>
		<td>用户每发送一条短消息扣除的积分数.注意: 分值为负数时才能扣除相应的积分.</td>
	</tr>
	<tr>
		<td>搜索</td>
		<td>用户每进行一次帖子搜索或短消息搜索扣除的积分数.注意: 分值为负数时才能扣除相应的积分.</td>
	</tr>
	<tr>
		<td>交易成功</td>
		<td>用户每成功进行一次交易后增加的积分数</td>
	</tr>
	<tr>
		<td>参与投票</td>
		<td>用户每参与一次投票后增加的积分数</td>
	</tr>
	</table>
	</div>
	<div class="ManagerForm">
	<fieldset>
	<legend style="background: url(../images/icons/icon25.jpg) no-repeat 6px 50%;">积分设置</legend>
	<table width="100%">
	<tr><td class="item_title" colspan="2">总积分计算公式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:TextBox ID="formula" runat="server" cols="30" Height="50" TextMode="MultiLine" RequiredFieldType="暂无校验"></cc2:TextBox>
			<div class="countor" style="display:block">
				<cc2:CheckBoxList ID="RefreshUserScore" RepeatLayout="flow" runat="server">
					<asp:ListItem Value="1">根据该公式刷新所有用户总积分</asp:ListItem>
				</cc2:CheckBoxList>
			</div>
		</td>
		<td class="vtop">总积分是衡量用户级别的唯一标准, 您可以在此设定用户的总积分计算公式, 其中 posts 代表发帖数;digestposts 代表精华帖数;oltime 代表用户总在线时间(分钟);extcredits1～extcredits8 分别代表上述 8 个自定义积分. 公式中可使用包括 + - * / () 在内的运算符号, 例如"<i><u>posts*0.5+digestposts*10+oltime*10+extcredits1*2+extcredits8</u></i>"代表总积分为"<i><u>发帖数</u></i>*0.5+<i><u>精华帖数</u></i>*10+<i><u>总在线时间(分钟)</u></i>*10+<i><u>自定义积分1</u></i>*2+<i><u>自定义积分8</u></i>". 注意: 一旦修改积分公式, 将可能导致所有用户的积分和所在会员用户组重新计算, 因此会加重服务器负担, 直至全部用户更新完毕. 其中在线时间,用户可以通过长时间联机刷新而作弊, 请慎用</td>
	</tr>
	<tr><td class="item_title" colspan="2">交易积分设置</td></tr>
	<tr>
		<td class="vtop rowform" style="padding-bottom:10px;">
            <cc2:DropDownList ID="creditstrans" runat="server">
                <asp:ListItem Value="0">关闭</asp:ListItem>
            </cc2:DropDownList>
		</td>
		<td class="vtop" rowspan="3">交易积分是一种可以由用户间自行转让、买卖交易的积分类型, 您可以指定一种积分作为交易积分. 如果不指定交易积分, 则用户间积分交易功能将不能使用. 注意: 交易积分必须是已启用的积分, 一旦确定请尽量不要更改, 否则以往记录及交易可能会产生问题.
		<p style="color:#000; padding-top:10px">如果想要让用户可以通过现金充值论坛积分,请进入<a href="global_screditset.aspx"style=" font-weight:700; padding-left:10px; text-decoration:underline; color:#FF0000">积分充值设置</a></p></td>
	</tr>
	<tr>
		<td class="vtop rowform" style="padding-left:20px;padding-bottom:10px;">
			主题(附件)买卖的积分:<cc2:DropDownList ID="topicattachcreditstrans" runat="server">
									<asp:ListItem Value="0" Selected="True">交易积分</asp:ListItem>
								</cc2:DropDownList>
		</td>
	</tr>
	<tr>
		<td class="vtop rowform" style="padding-left:20px;">
			悬赏主题使用的积分:<cc2:DropDownList ID="bonuscreditstrans" runat="server">
									<asp:ListItem Value="0" Selected="True">交易积分</asp:ListItem>
								</cc2:DropDownList>
		</td>
	</tr>
	<tr><td class="item_title" colspan="2">转账最低余额</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:TextBox ID="transfermincredits" runat="server" RequiredFieldType="数据校验" CanBeNull="必填"></cc2:TextBox>
		</td>
		<td class="vtop">积分转账后要求用户所拥有的余额最小数值. 利用此功能, 您可以设置较大的余额限制, 使积分小于这个数值的用户无法转账;也可以将余额限制设置为负数, 使得转账在限额内可以透支</td>
	</tr>
	<tr><td class="item_title" colspan="2">单主题最高收入</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:TextBox ID="maxincperthread" runat="server" RequiredFieldType="数据校验" CanBeNull="必填"></cc2:TextBox>
		</td>
		<td class="vtop">设置单一主题作者出售所得的最高税后积分收入, 超过此限制后购买者将仍然被扣除相应积分, 但主题作者收益将不再上涨. 本限制只在主题买卖时起作用, 0 为不限制</td>
	</tr>
	<tr><td class="item_title" colspan="2">积分交易税</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:TextBox ID="creditstax" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Size="5" MaxLength="5"></cc2:TextBox>
		</td>
		<td class="vtop">积分交易税(损失率)为用户在利用积分进行转让、兑换、买卖时扣除的税率, 范围为 0～1 之间的浮点数, 例如设置为 0.2, 则用户在转换 100 个单位积分时, 损失掉的积分为 20 个单位, 0 为不损失</td>
	</tr>
	<tr><td class="item_title" colspan="2">兑换最低余额</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:TextBox ID="exchangemincredits" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Size="5" MaxLength="4"></cc2:TextBox>
		</td>
		<td class="vtop">积分兑换后要求用户所拥有的余额最小数值. 利用此功能, 您可以设置较大的余额限制, 使积分小于这个数值的用户无法兑换;也可以将余额限制设置为负数, 使得兑换在限额内可以透支</td>
	</tr>
	<tr><td class="item_title" colspan="2">单主题最高出售时限</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:TextBox ID="maxchargespan" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Size="5" MaxLength="4" ></cc2:TextBox>(单位:小时)
		</td>
		<td class="vtop">设置当主题被作者出售时, 系统允许自主题发布时间起, 其可出售的最长时间. 超过此时间限制后将变为普通主题, 阅读者无需支付积分购买, 作者也将不再获得相应收益, 以小时为单位, 0 为不限制</td>
	</tr>
	<tr><td class="item_title" colspan="2">删帖不减积分时间期限</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="losslessdel" runat="server" Text="5" RequiredFieldType="数据校验"  CanBeNull="必填" Size="5" MaxLength="4"></cc1:TextBox>(单位:天)
		</td>
		<td class="vtop">设置版主或管理员从前台删除发表于多少天以前的帖子时, 不更新用户积分, 可用于清理老帖子而不对作者积分造成损失. 0 为不使用此功能, 始终更新用户积分</td>
	</tr>
	</table>
	<table id="creditstransLayer" runat="server">
	</table>

	</fieldset>
</div>
</cc3:TabPage>
<cc3:TabPage Caption="积分充值" ID="tabPage22">
	<div class="ManagerForm">
	<fieldset>
	<legend style="background: url(../images/icons/icon25.jpg) no-repeat 6px 50%;">积分充值设置</legend>
	<div id="info1" style="text-align:left;border: 1px dotted rgb(219, 221, 211); margin: 10px 0pt; padding: 15px 10px 10px 56px; background: rgb(253, 255, 242) url(../images/hint.gif) no-repeat 20px 15px;clear: both;">
&nbsp;&nbsp;&nbsp;&nbsp;“支付宝”(http://www.alipay.com)是中国领先的网上支付平台，由全球最佳 B2B 公司阿里巴巴公司创建，为 <%=BBX.Common.Utils.ProductName%> 用户提供积分购买及论坛 B2C、C2C 交易平台。您只需进行简单的设置，即可使论坛内容和人气，真成为除广告收入外的重要利润来源，从而实现论坛的规模化经营。
由于涉及现金交易，为避免因操作不当而造成的资金损失，请在开始使用支付宝积分交易功能(不包含支付宝按钮功能)前，务必仔细阅读《用户使用说明书》中有关电子商务的部分，当确认完全理解和接受相关流程及使用方法后再进行相关设置。<br />
<a href="#" onClick="this.style.display='none';document.getElementById('alipayscript').style.display = 'block'">显示全部提示...</a>
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
			<a href="#" onClick="testAlipayaccout();">积分充值订单测试</a><br/>
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
	</fieldset>
</div>
</cc3:TabPage>
</cc3:TabControl>
	<div class="Navbutton">
		<cc1:Button ID="Save" runat="server" Text="提 交"></cc1:Button>
	</div>
<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
</form>
<%=footer%>
</body>
</html>