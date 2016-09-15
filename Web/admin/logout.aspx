<%@ Page language="c#" Inherits="BBX.Web.Admin.logout" Codebehind="logout.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
		<title>Logout</title>
		<link href="styles/default.css" rev="stylesheet" rel="stylesheet" type="text/css" media="all">
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<script type="text/javascript">
    if(top.location!=self.location)
    {
	    top.location.href = "logout.aspx";
    }
</script>
	<body>
<br /><br />
<div style="width:100%" align="center">
<div align="center" style="width:600px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;"><img src="images/hint.gif" border="0" alt="提示:" align="absmiddle" width="11" height="13" /> &nbsp; 
您已成功退出系统设置
</div></div>
</body>
</html>
