<%@ Page language="c#" Inherits="BBX.Web.Admin.ThumbnailRemoteImage" Codebehind="ThumbnailRemoteImage.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
	<title>生成图片缩略图</title>
	<script type="text/javascript" src="../js/common.js"></script>
	<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
	<script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
    <script type="text/javascript">
        window.onerror = function() {
            return true;
        }
    </script>
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<fieldset>
	<legend style="background:url(../images/icons/icon2.jpg) no-repeat 6px 50%;">生成图片缩略图</legend>
	<table width="100%">
	<tr><td class="item_title" colspan="2" runat="server" id="reImgUrl"></td></tr>
	<tr><td class="item_title" colspan="2">图片网址</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="imageUrl" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验" width="400"  MaxLength="2000"></cc1:TextBox>
		</td>
		<td class="vtop">&nbsp;&nbsp;图片的完整url</td>
	</tr>
	<tr><td class="item_title" colspan="2">缩略图最大宽度</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="maxWidth" runat="server" CanBeNull="必填" Text="163" RequiredFieldType="暂无校验" width="50"  MaxLength="50"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">缩略图最大高度</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="maxHeight" runat="server" CanBeNull="必填" Text="130" RequiredFieldType="暂无校验" width="50"  MaxLength="50"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	</table>
	<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
	<div class="Navbutton">
		<cc1:Button id="SaveInfo" runat="server" Text=" 提 交 "></cc1:Button>
	</div>
</fieldset>			
</form>
</div>		
<%=footer%>
</body>
</html>