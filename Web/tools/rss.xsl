<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:dc="http://purl.org/dc/elements/1.1/" version="1.0">
<xsl:output method="xml"  />
<xsl:template match="/">

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
<head>
<title><xsl:value-of select="rss/channel/title"/></title>
<style>

body {
	margin: 0px;
	padding: 0px;
	color: #000000;
	font-family: Arial, Helvetica;
	font-size: 9pt;
	background-color: #ffffff;
	color: #000000;
}

a:LINK {
	color: #666699;
	text-decoration: none;
}

a:ACTIVE {
	color: #ff0000;
}

a:VISITED {
	color: #000066;
	text-decoration: none;
}

a:HOVER {
	text-decoration: underline;
}

#Content {
	padding-top: 12px;
	padding-left: 35px;
	padding-right: 35px;
}

h1,h2,h4 {
	color: #666666;
	font-weight: bold;
	font-family: Arial, Arial, Helvetica;
	margin: 0px;
	font-size: 25pt;
}

h2 {
	font-size: 16pt;
	margin-left: 16px;
}

h4 {
	font-size: 11pt;
	font-family: Arial, Helvetica;
}


#ItemList {
	list-style-type: circle;
	color: #666666;
}

.ItemListItem {
	padding-bottom: 8px;
}

.ItemListItemDetails {
	font-family: Arial, Helvetica;
	font-size: 10pt;
	color: #333333;
}

</style>
</head>
<body xmlns="http://www.w3.org/1999/xhtml">

	<div id="Content">
		    <h3><a><xsl:attribute name="href"><xsl:value-of select="rss/channel/link"/></xsl:attribute><xsl:value-of select="rss/channel/title" /></a></h3>
        	         <div class="ItemListItemDetails">
        	            描述:<xsl:value-of select="rss/channel/description"/> 
        	         </div>
        	         <div class="ItemListItemDetails">
                        版权:<xsl:value-of select="rss/channel/copyright"/>
                     </div>
                     <div class="ItemListItemDetails">
                        日期: <xsl:value-of select="rss/channel/pubDate"/>
                     </div>
                    
           <ol id="ItemList">
			<xsl:for-each select="rss/channel/item">       
				<li class="ItemListItem">
				<h4><a><xsl:attribute name="href"><xsl:value-of select="link"/></xsl:attribute>
                                <xsl:value-of select="title" /></a></h4>
                                 <div class="ItemListItemDetails"> 
                                    内容: <xsl:value-of select="description" />
                                 </div>
                           
                                    类型: <xsl:value-of select="category"/>,
 						            作者: <xsl:value-of select="author" />,
						            时间: <xsl:value-of select="pubdate" />
				</li>
			</xsl:for-each>
		</ol>

	</div>
	
</body>
</html>

</xsl:template>
</xsl:stylesheet>