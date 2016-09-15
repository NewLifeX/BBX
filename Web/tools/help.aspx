<%@ Page AutoEventWireup="false" Inherits="BBX.Web.UI.HelpPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>使用帮助 - <%=forumtitle%></title>
<script language="javascript">
function expandoptions(id)
{
	var a = document.getElementById(id);
	if(a.style.display=='')
	{
		a.style.display='none';
	}
	else
	{
		a.style.display='';
	}
}
</script>
<style type="text/css">
<!--
a {
	font-family: Tahoma, Verdana;
	font-size: 12px;
	color: #003366;
	text-decoration: none;
}
body {
	font-family: Tahoma, Verdana;
	font-size: 12px;
	color: #333333;
	text-decoration: none;
}
h1 {
	font-family: Tahoma, Verdana;
	font-size: 14px;
	color: #333333;
	text-decoration: none;
	font-weight: bold;
}
-->
</style>
</head>

<body>
<div style="border:0px; background-color:#ffffff; width:98%; margin:auto">
<h1><%=forumtitle%>使用帮助</h1>
</div>
<br />
<div style="border:#CCCCCC solid 1px; background-color:#ffffff; width:98%; margin:auto">
<div style="border:0px; background-color:#F3F3F3; width:100%; margin:auto; padding:6px"><a href="###" onclick="expandoptions('item1');">用户须知</a></div>
<div id="item1" style="border:0px; background-color:#ffffff; width:100%; margin:auto; padding:6px; display:none">
  <div>
      <ul>
        <li><a href="#1-1">我必须要注册吗？</a> </li>
        <li><a href="#1-2"><%=BBX.Common.Utils.ProductName%> 论坛使用 Cookies 吗？</a> </li>
        <li><a href="#1-3">如何使用签名？</a> </li>
        <li><a href="#1-4">如何使用个性化的头像？</a> </li>
        <li><a href="#1-5">如果我遗忘了密码，我该怎么办？</a> </li>
        <li><a href="#1-6">什么是“短消息”？</a> </li>
      </ul>

  </div>
  <br />
  <div>
    <div><a name="#1-1" id="#1-1"></a><strong>我必须要注册吗？</strong></div>
    <div>    这取决于管理员如何设置 <%=BBX.Common.Utils.ProductName%> 论坛的用户组权限选项，您甚至有可能必须在注册成正式用户后后才能浏览帖子。当然，在通常情况下，您至少应该是正式用户才能发新帖和回复已有帖子。请 <a href="../register.aspx">点击这里</a> 免费注册成为我们的新用户！<br />
        <br />
            
      强烈建议您注册，这样会得到很多以游客身份无法实现的功能。 </div>
  </div>
  <br />
  <div>
    <div><a name="#1-2" id="#1-2"></a><strong><%=BBX.Common.Utils.ProductName%> 论坛使用 Cookies 吗？</strong></div>
    <div>    <%=BBX.Common.Utils.ProductName%> 采用 Cookie   的双重方式保存用户信息，以确保在各种环境，包括 Cookie 完全无法使用的情况下您都能正常使用论坛各项功能。但 Cookies   的使用仍然可以为您带来一系列的方便和好处，因此我们强烈建议您在正常情况下不要禁止 Cookie 的应用，<%=BBX.Common.Utils.ProductName%>   的安全设计将全力保证您的资料安全。<br />
        <br />
          
      在登录页面中，您可以选择 Cookie 记录时间，在该时间范围内您打开浏览器访问论坛将始终保持您上一次访问时的登录状态，而不必每次都输入密码。但出于安全考虑，如果您在公共计算机访问论坛，建议选择“浏览器进程”，或在离开公共计算机前选择“退出”以杜绝资料被非法使用的可能。 </div>
  </div>
  <br />
  <div>
    <div><a name="#1-3" id="#1-3"></a><strong>如何使用签名？</strong></div>
    <div>      签名是加在您发表的帖子下面的小段文字，注册之后，您就可以设置自己的个性签名了。 </div>
  </div>
  <br />
  <div>
    <div><a name="#1-4" id="#1-4"></a><strong>如何使用个性化的头像？</strong></div>
    <div>    控制面板中，有一处“头像”选项。头像是显示在您用户名下面的小图像，使用头像可能需要一定的权限，否则将不会显示出来。详情请查询本论坛的级别设定，一般头像图像宽度应控制在 150 像素以下，以免影响界面美观。 </div>
  </div>
  <br />
  <div>
    <div><a name="#1-5" id="#1-5"></a><strong>如果我遗忘了密码，我该怎么办？</strong></div>
    <div>    <%=BBX.Common.Utils.ProductName%> 提供发送取回密码链接到 Email 的服务，点击登录页面中的取回密码功能，可以为您把取回密码的方法发送到注册时填写的 Email   信箱中。如果您的 Email 已失效或无法收到信件，请与论坛管理员联系。 </div>
  </div>
  <br />
  <div>
    <div><a name="#1-6" id="#1-6"></a><strong>什么是“短消息”？</strong></div>
    <div>      “短消息”是论坛注册用户间交流的工具，信息只有发件和收件人可以看到，收到信息后系统会出现铃声和相应提示，您可以通过短消息功能与同一论坛上的其他用户保持私人联系。收件箱或控制面板中提供了短消息的收发服务。 </div>
  </div>
</div>
</div>

<br />

<div style="border:#CCCCCC solid 1px; background-color:#ffffff; width:98%; margin:auto">
<div style="border:0px; background-color:#F3F3F3; width:100%; margin:auto; padding:6px"><a href="###" onclick="expandoptions('item2');">论坛使用</a></div>
<div id="item2" style="border:0px; background-color:#ffffff; width:100%; margin:auto; padding:6px; display:none">
  <div>
      <ul>
        <li><a href="#2-1">在哪里可以登录？</a> </li>
        <li><a href="#2-2">在哪里可以退出？</a> </li>
        <li><a href="#2-3">我要搜索论坛，应该怎么做？</a> </li>
        <li><a href="#2-4">怎样给其他人发送“短消息”？</a> </li>
        <li><a href="#2-5">怎样看到全部的会员？</a> </li>
      </ul>
  </div>
  <br />
  <div>
    <div><a name="#2-1" id="#2-1"></a><strong>在哪里可以登录？</strong></div>
    <div>      如果您尚未登录，点击左上角的“登录”，输入用户名和密码，确定即可。如果需要保持登录，请选择相应的 Cookie   时间，在此时间范围内您可以不必输入密码而保持上次的登录状态。 </div>
  </div>
  <br />
  <div>
    <div><a name="#2-2" id="#2-2"></a><strong>在哪里可以退出？</strong></div>
    <div>    如果您已经登录，点击左上角的“退出”，系统会清除 Cookie，退出登录状态。 </div>
  </div>
  <br />
  <div>
    <div><a name="#2-3" id="#2-3"></a><strong>我要搜索论坛，应该怎么做？</strong></div>
    <div>    点击上面的搜索，输入搜索的关键字并选择一个范围，就可以检索到您有权限访问论坛中的相关的帖子。 </div>
  </div>
  <br />
  <div>
    <div><a name="#2-4" id="#2-4"></a><strong>怎样给其他人发送“短消息”？</strong></div>
    <div>    如果您已登录，菜单上会显示出短消息项，点击后弹出短消息窗口，通过类似发送邮件一样的填写，点“发送”，消息就被发到对方收件箱中了。当他/她访问论坛的主要页面时，系统都会提示他/她收信息。 </div>
  </div>
  <br />
  <div>
    <div><a name="#2-5" id="#2-5"></a><strong>怎样看到全部的会员？</strong></div>
    <div>    当管理员设置可用此项功能时，您可以通过点击会员查看所有的会员及其资料，并可实现会员资料的检索和排序输出。 </div>
  </div>
</div>
</div>

<br />

<div style="border:#CCCCCC solid 1px; background-color:#ffffff; width:98%; margin:auto">
<div style="border:0px; background-color:#F3F3F3; width:100%; margin:auto; padding:6px"><a href="###" onclick="expandoptions('item3');">读写帖子和收发短消息</a></div>
<div id="item3" style="border:0px; background-color:#ffffff; width:100%; margin:auto; padding:6px; display:none">
  <div>
      <ul>
        <li><a href="#3-1">如何发布新帖子？</a> </li>
        <li><a href="#3-2">如何回复帖子？</a> </li>
        <li><a href="#3-3">我能够删除主题吗？</a> </li>
        <li><a href="#3-4">怎样编辑自己发表的帖子？</a> </li>
        <li><a href="#3-5">我可不可以上传附件？</a> </li>
        <li><a href="#3-6">什么是表情？</a> </li>
        <li><a href="#3-7">该怎样发起一个投票？</a> </li>
      </ul>
  </div>
  <br />
  <div>
    <div><a name="#3-1" id="#3-1"></a><strong>如何发布新帖子？</strong></div>
    <div>      在论坛版块中，点“发新帖”即可进入功能齐全的发帖界面。当然您也可以使用版块下面的“快速发帖”发表新帖(如果此选项打开)。注意，一般论坛都设置为需要登录后才能发帖。 </div>
  </div>
  <br />
  <div>
    <div><a name="#3-2" id="#3-2"></a><strong>如何回复帖子？</strong></div>
    <div>      在浏览帖子时，点“回复帖子”即可进入功能齐全的回复界面。当然您也可以使用版块下面的“快速回复”发表回复(如果此选项打开)。注意，一般论坛都设置为需要登录后才能回复。 </div>
  </div>
  <br />
  <div>
    <div><a name="#3-3" id="#3-3"></a><strong>我能够删除主题吗？</strong></div>
    <div>      浏览帖子时可以按下面的“编辑帖子”，对于您自己发表的帖子，可以很容易的编辑和删除。但当这帖是整个主题的起始帖时，则会删除该主题和全部回复。 </div>
  </div>
  <br />
  <div>
    <div><a name="#3-4" id="#3-4"></a><strong>怎样编辑自己发表的帖子？</strong></div>
    <div>      和上面一样，用“编辑帖子”就可以编辑自己发表的帖子。如果管理员通过论坛设置将这个功能屏蔽掉则不再可以进行此操作。 </div>
  </div>
  <br />
  <div>
    <div><a name="#3-5" id="#3-5"></a><strong>我可不可以上传附件？</strong></div>
    <div>      可以。您可以在任何支持上传附件的版块中，通过发新帖、或者回复的方式上传附件（只要您的权限足够）。附件不能超过系统限定尺寸，且在可用类型的范围内才能上传。 </div>
  </div>
  <br />
  <div>
    <div><a name="#3-6" id="#3-6"></a><strong>什么是表情？</strong></div>
    <div>    Smilies 是一些用字符表示的表情符号，如果打开 Smilies 功能，<%=BBX.Common.Utils.ProductName%>   会把一些符号转换成小图像，显示在帖子中，更加美观明了。</div>
  </div>
  <br />
  <div>
    <div><a name="#3-7" id="#3-7"></a><strong>该怎样发起一个投票？</strong></div>
    <div>      您可以像发帖一样在版块中发起投票。每行输入一个可能的选项（最多10个），您可以通过阅读这个投票帖选出自己的答案，每人只能投票一次，之后将不能再对您的选择做出更改。<br />
        <br />
            
      管理员拥有随时关闭和修改投票选项的权力。 </div>
  </div>
</div>
</div>

<br />

<div style="border:#CCCCCC solid 1px; background-color:#ffffff; width:98%; margin:auto">
<div style="border:0px; background-color:#F3F3F3; width:100%; margin:auto; padding:6px"><a href="###" onclick="expandoptions('item4');">相关法规</a></div>
<div id="item4" style="border:0px; background-color:#ffffff; width:100%; margin:auto; padding:6px; display:none">
  <div><strong>《互联网电子公告服务管理规定》</strong><br />
    <br />
    第一条   为了加强对互联网电子公告服务(以下简称电子公告服务)的管理，规范电子公告信息发布行为，维护国家安全和社会稳定，保障公民、法人和其他组织的合法权益，根据《互联网信息服务管理办法》的规定，制定本规定。<br />
    第二条   在中华人民共和国境内开展电子公告服务和利用电子公告发布信息，适用本规定。<br />
    　　
    本规定所称电子公告服务，是指在互联网上以电子布告牌、电子白板、电子论坛、网络聊天室、留言板等交互形式为上网用户提供信息发布条件的行为。<br />
    第三条   电子公告服务提供者开展服务活动，应当遵守法律、法规，加强行业自律，接受信息产业部及省、自治区、直辖市电信管理机构和其他有关主管部门依法实施的监督检查。<br />
    第四条   上网用户使用电子公告服务系统，应当遵守法律、法规，并对所发布的信息负责。<br />
    第五条   从事互联网信息服务，拟开展电子公告服务的，应当在向省、自治区、直辖市电信管理机构或者信息产业部申请经营性互联网信息服务许可或者办理非经营性互联网信息服务备案时，提出专项申请或者专项备案。<br />
    　　
    省、自治区、直辖市电信管理机构或者信息产业部经审查符合条件的，应当在规定时间内连同互联网信息服务一并予以批准或者备案，并在经营许可证或备案文件中专项注明；不符合条件的，不予批准或者不予备案，书面通知申请人并说明理由。<br />
    第六条   开展电子公告服务，除应当符合《互联网信息服务管理办法》规定的条件外，还应当具备下列条件：<br />
    　　
    (一)有确定的电子公告服务类别和栏目；<br />
    　　
    (二)有完善的电子公告服务规则；<br />
    　　
    (三)有电子公告服务安全保障措施，包括上网用户登记程序、上网用户信息安全管理制度、技术保障设施；<br />
    　　
    (四)有相应的专业管理人员和技术人员，能够对电子公告服务实施有效管理。<br />
    第七条   已取得经营许可或者已履行备案手续的互联网信息服务提供者，拟开展电子公告服务的，应当向原许可或者备案机关提出专项申请或者专项备案。<br />
    　　
    省、自治区、直辖市电信管理机构或者信息产业部，应当自收到专项申请或者专项备案材料之日起60日内进行审查完毕。经审查符合条件的，予以批准或者备案，并在经营许可证或备案文件中专项注明；不符合条件的，不予批准或者不予备案，书面通知申请人并说明理由。<br />
    第八条   未经专项批准或者专项备案手续，任何单位或者个人不得擅自开展电子公告服务。<br />
    第九条   任何人不得在电子公告服务系统中发布含有下列内容之一的信息：<br />
    　　
    (一)反对宪法所确定的基本原则的；<br />
    　　
    (二)危害国家安全，泄露国家秘密，颠覆国家政权，破坏国家统一的；<br />
    　　
    (三)损害国家荣誉和利益的；<br />
    　　
    (四)煽动民族仇恨、民族歧视，破坏民族团结的；<br />
    　　
    (五)破坏国家宗教政策，宣扬邪教和封建迷信的；<br />
    　　
    (六)散布谣言，扰乱社会秩序，破坏社会稳定的；<br />
    　　
    (七)散布淫秽、色情、赌博、暴力、凶杀、恐怖或者教唆犯罪的；<br />
    　　
    (八)侮辱或者诽谤他人，侵害他人合法权益的；<br />
    　　
    (九)含有法律、行政法规禁止的其他内容的；<br />
    第十条   电子公告服务提供者应当在电子公告服务系统的显著位置刊载经营许可证编号或者备案编号、电子公告服务规则，并提示上网用户发布信息需要承担的法律责任。<br />
    第十一条   电子公告服务提供者应当按照经批准或者备案的类别和栏目提供服务，不得超出类别或者另设栏目提供服务。<br />
    第十二条   电子公告服务提供者应当对上网用户的个人信息保密，未经上网用户同意不得向他人泄露，但法律另有规定的除外。<br />
    第十三条   电子公告服务提供者发现其电子公告服务系统中出现明显属于本办法第九条所列的信息内容之一的，应当立即删除，保存有关记录，并向国家有关机关报告。<br />
    第十四条   电子公告服务提供者应当记录在电子公告服务系统中发布的信息内容及其发布时间、互联网地址或者域名。记录备份应当保存60日，并在国家有关机关依法查询时，予以提供。<br />
    第十五条   互联网接入服务提供者应当记录上网用户的上网时间、用户帐号、互联网地址或者域名、主叫电话号码等信息，记录备份应保存60日，并在国家有关机关依法查询时，予以提供。<br />
    第十六条   违反本规定第八条、第十一条的规定，擅自开展电子公告服务或者超出经批准或者备案的类别、栏目提供电子公告服务的，依据《互联网信息服务管理办法》第十九条的规定处罚。<br />
    第十七条   在电子公告服务系统中发布本规定第九条规定的信息内容之一的，依据《互联网信息服务管理办法》第二十条的规定处罚。<br />
    第十八条   违反本规定第十条的规定，未刊载经营许可证编号或者备案编号、未刊载电子公告服务规则或者未向上网用户作发布信息需要承担法律责任提示的，依据《互联网信息服务管理办法》第二十二条的规定处罚。<br />
    第十九条   违反本规定第十二条的规定，未经上网用户同意，向他人非法泄露上网用户个人信息的，由省、自治区、直辖市电信管理机构责令改正；给上网用户造成损害或者损失的，依法承担法律责任。<br />
    第二十条   未履行本规定第十三条、第十四条、第十五条规定的义务的，依据《互联网信息服务管理办法》第二十一条、第二十三条的规定处罚。<br />
    第二十一条   在本规定施行以前已开展电子公告服务的，应当自本规定施行之日起60日内，按照本规定办理专项申请或者专项备案手续。<br />
    第二十二条   本规定自发布之日起施行。<br />
  </div>
</div>
</div>
<br /><br /><br />
</body>
</html>
