<%@ Page language="c#" Inherits="BBX.Web.Admin.forumhot" %>
<%@ Register TagPrefix="cc1" Namespace="BBX.Control" Assembly="BBX.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
    <head>
        <title>插件轮换图片管理</title>
        <script type="text/javascript" src="../../javascript/common.js"></script>
        <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
        <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
        <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
        <meta http-equiv="X-UA-Compatible" content="IE=7" />
        <script type="text/javascript" src="../js/common.js"></script>
        <script type="text/javascript" src="../js/modalpopup.js"></script>
        <script type="text/javascript">
            function setliststatus(status) {
                document.getElementById("forumhotlayer").style.display = (status) ? "" : "none";
            }

            function isNumber(str) {
                return (/^\d+$/.test(str));
            }
        </script>
    </head>
    <body>
        <%if (action == ""){%>
            <form action="forum_forumhot.aspx?action=setenabled" method="post">
                <uc1:PageInfo ID="info1" runat="server" Icon="information" Text="<li>论坛热点信息,可以配置是否开启和调用条件!</li>" />
                <div class="ManagerForm">
                    <fieldset>
                        <legend style="background: url(../images/icons/icon18.jpg) no-repeat 6px 50%;">论坛热点设置</legend>
                        <table width="100%" border="0">
                            <tr>
                                <td class="item_title">是否开启论坛热点功能</td>
                            </tr>
                            <tr>
                                <td class="vtop rowform">
		                            <input name="enabled" type="radio" value="1" onclick="setliststatus(1);" <%if(forumHotConfigInfo.Enable){%>checked<%}%>/>是
      	                            <input name="enabled" type="radio" value="0" onclick="setliststatus(0);" <%if(!forumHotConfigInfo.Enable){%>checked<%}%>/>否
	                            </td>
                            </tr>
                        </table>
                        <div class="Navbutton">
	                        <button class="ManagerButton" type="submit"><img alt="" src="../images/submit.gif"/>提 交</button>
                        </div>
                    </fieldset>
                </div>
                <div class="ManagerForm ntcplist" id="forumhotlayer" <%if(!forumHotConfigInfo.Enable){%>style="display:none"<%}%>>
                    <table class="datalist">
	                    <tbody>
		                    <tr class="category">
			                    <td style="border:1px solid #EAE9E1">ID</td>
			                    <td style="border:1px solid #EAE9E1">名称</td>
			                    <td style="border:1px solid #EAE9E1">操作</td>
		                        <td style="border:1px solid #EAE9E1">是否开启</td>
		                    </tr>
		                    <%foreach (BBX.Config.ForumHotItemInfo item in forumHotConfigInfo.ForumHotCollection)
                        {%>
		                        <%if(item.Id==6){%>
		                            <tr>
			                            <td colspan="4" class="category" style="border:1px solid #EAE9E1;text-align:left;padding-left:10px;font-weight:700;background:#F2F2F2;">轮显图片配置</td>
		                            </tr>
		                            <tr onmouseout="this.className='mouseoutstyle'" onmouseover="this.className='mouseoverstyle'" class="mouseoutstyle">
			                            <td style="border:1px solid #EAE9E1"><%=item.Id%></td>
			                            <td style="border:1px solid #EAE9E1;text-align:left;padding-left:10px;"><%=item.Name%></td>
			                            <td style="border:1px solid #EAE9E1"><a href="forum_forumhot.aspx?action=edit&id=<%=item.Id%>">编辑</a></td>
		                                <td style="border:1px solid #EAE9E1"><img alt="" src="../images/state2.gif"/></td>
		                            </tr>	
		                        <%}else{%>
		                            <tr onmouseout="this.className='mouseoutstyle'" onmouseover="this.className='mouseoverstyle'" class="mouseoutstyle">
			                            <td style="border:1px solid #EAE9E1"><%=item.Id%></td>
			                            <td style="border:1px solid #EAE9E1;text-align:left;padding-left:10px;"><%=item.Name%></td>
			                            <td style="border:1px solid #EAE9E1"><a href="forum_forumhot.aspx?action=edit&id=<%=item.Id%>">编辑</a></td>
		                                <td style="border:1px solid #EAE9E1">
			                            <%if (item.Enabled == 1){%>
			                                <img alt="" src="../images/state2.gif"/>
			                            <%}else{%>
			                                <img alt="" src="../images/state3.gif"/>
			                            <%}%>
			                            </td>
		                            </tr>
		                        <%}%>
		                    <%}%>  
	                    </tbody>
	                </table>
                </div>
            </form>
        <%} %>

        <%if(action=="edit"){%>
            <form action="forum_forumhot.aspx?action=editsave&id=<%=forumHotItem.Id%>" method="post"  name="form" id="form">
                <div class="ManagerForm">
                    <fieldset>
                        <legend style="background: url(&quot;../images/icons/legendimg.jpg&quot;) no-repeat scroll 6px 50% transparent;">编辑热点配置</legend>
                        <table width="100%">
                            <tbody>
                                <tr><td colspan="2" class="item_title">热点名称</td></tr>
	                            <tr>
		                            <td class="vtop rowform">
			                            <input type="text" size="30" onblur="this.className='txt';" onfocus="this.className='txt_focus';" class="txt" id="forumhotitemname" value="<%=forumHotItem.Name%>" name="forumhotitemname" />
		                            </td>
		                            <td class="vtop">热点标签的名称</td>
	                            </tr>
                                <tr><td colspan="2" class="item_title"><%if (forumHotItem.Id != 6){%>是否开启<%}else{%>是否开启从一个主题提取一张图片<%}%></td></tr>
	                            <tr>
		                            <td class="vtop rowform">
			                            <input name="itemenabled" type="radio" value="1"  <%if(forumHotItem.Enabled==1){%>checked<%}%> />启用
			                            <input name="itemenabled" type="radio" value="0"  <%if(forumHotItem.Enabled==0){%>checked<%}%> />关闭
		                            </td>
		                            <td class="vtop">可配置该项目是否显示</td>
	                            </tr>
                                <%if (forumHotItem.Id != 6)
                                  {%>
                                <tr><td colspan="2" class="item_title">调用选项</td></tr>
	                            <tr>
		                            <td class="vtop rowform">
			                            <select name="datatype" id="datatype" onchange="bindpage(this.value);">
				                            <option value="topics" <%if(forumHotItem.Datatype=="topics"){ %>selected<% }%>>帖子排行</option>
				                            <option value="forums" <%if(forumHotItem.Datatype=="forums"){ %>selected<% }%>>版块排行</option>
				                            <option value="users" <%if(forumHotItem.Datatype=="users"){ %>selected<% }%>>用户排行</option>
			                            </select>
		                            </td>
		                            <td class="vtop">选择数据类型</td>
	                            </tr>
                                <%}
                                  else
                                  { %>
                                  <tr style="display:none"><td colspan="2" class="item_title"><input type="hidden" id="datatype" name="datatype" value="pictures" /></td></tr>
                                  <%} %>
                                <tr id="sorttype_title"><td colspan="2" class="item_title">排序方式</td></tr>
                                <tr id="sorttype_content">
		                            <td class="vtop rowform">
			                            <select name="sorttype" id="sorttype_select" onchange="if($('datatype').value=='users'){users_setdatatimetype(this);}">
				                            <option value="0">0</option>
			                            </select>
		                            </td>
		                            <td class="vtop">选择调用数据的排序规则</td>
	                            </tr>

                                <tr id="forumlist_title"><td colspan="2" class="item_title">版块列表</td></tr>
                                <tr id="forumlist_content">
		                            <td class="vtop rowform">
			                            <select name="forumlist" id="forumlist_select" size="10" multiple="multiple">
                                        <%foreach (BBX.Entity.IXForum forum in BBX.Forum.Forums.GetForumList())
                                          {%>
                                          <%if (forum.Layer > 0) {%>
				                            <option value="<%=forum.Fid %>" <%if (BBX.Common.Utils.InArray(forum.Fid.ToString(), forumHotItem.Forumlist)){%>selected<%} %>><%=forum.Name.Replace("'", "\\'")%></option>
                                            <%}else{ %>
                                            <optgroup label="--<%=forum.Name.Replace("'", "\\'")%>"></optgroup>
                                            <%} %>
                                        <%} %>
			                            </select>
                                        <div style="margin-top:10px">
                                            <a href="###" onclick="selectcontrols(false);">清空选择</a> 
                                            <a href="###" onclick="selectcontrols(true);">选择全部</a> 
                                            <a href="###" onclick="selectvisibles();">选择游客可见</a>
                                        </div>
                                        <script type="text/javascript">
                                            function selectcontrols(setvalue) {
                                                for (i = 0; i < $('forumlist_select').options.length; i++) {
                                                    $('forumlist_select').options[i].selected = setvalue;
                                                }
                                            }

                                            function selectvisibles() {
                                                selectcontrols(false);
                                                var visibleforums = '<%=BBX.Forum.Forums.GetVisibleForum()%>';
                                                var forumidlist = visibleforums.split(",");
                                                for (i = 0; i < $('forumlist_select').options.length; i++) {
                                                    for (j = 0; j < forumidlist.length; j++) {
                                                        if ($('forumlist_select').options[i].value == forumidlist[j]) {
                                                            $('forumlist_select').options[i].selected = true;
                                                        }
                                                    }
                                                }
                                            }
                                            if ('<%= forumHotItem.Forumlist%>' == '')
                                                selectvisibles();
                                        </script>
		                            </td>
		                            <td class="vtop">选择论坛数据的来源版块,按住Ctrl或者Shift可多选,不选择将获取所有游客可见版块的信息</td>
	                            </tr>

                                <tr id="datacount_title"><td colspan="2" class="item_title">调用条数</td></tr>
                                <tr id="datacount_content">
                                    <td class="vtop rowform">
		                                <select name="datacount" id="datacount_select">
			                                <option value="8">8</option>
		                                </select>
		                            </td>
		                            <td class="vtop">显示数据的条数</td>
	                            </tr>

                                <tr id="datatimetype_title"><td colspan="2" class="item_title">获取信息时间范围</td></tr>
	                            <tr id="datatimetype_content">
		                            <td class="vtop rowform">
                                        <select id="datatimetype_select" name="datatimetype">
                                            <option value="0">0</option>
                                        </select>
		                            </td>
		                            <td class="vtop">设置程序获取一定时间范围以内的数据</td>
	                            </tr>

                                <tr id="cachetimeout_title"><td colspan="2" class="item_title">调用间隔时间（秒）</td></tr>
	                            <tr id="cachetimeout_content">
		                            <td class="vtop rowform">
			                            <input type="text" size="10" onblur="this.className='txt';" onfocus="this.className='txt_focus';" class="txt" id="cachetime" value="<%=forumHotItem.Cachetimeout%>" name="cachetime"/>
		                            </td>
		                            <td class="vtop">调用间隔时间，程序一段时间将缓存数据，过期后再次调用</td>
	                            </tr>

                                <tr id="forumlength_title"><td colspan="2" class="item_title">版块名称长度</td></tr>
	                            <tr id="forumlength_content">
		                            <td class="vtop rowform">
			                            <input type="text" size="10" onblur="this.className='txt';" onfocus="this.className='txt_focus';" class="txt" id="forumnamelength" value="<%=forumHotItem.Forumnamelength%>" name="forumnamelength"/>
		                            </td>
		                            <td class="vtop">主题所在的相关版块名称的长短，超过将截取</td>
	                            </tr>

                                <tr id="titlelength_title"><td colspan="2" class="item_title">标题长度</td></tr>
	                            <tr id="titlelength_content">
		                            <td class="vtop rowform">
			                            <input type="text" size="10" onblur="this.className='txt';" onfocus="this.className='txt_focus';" class="txt" id="topictitlelength" value="<%=forumHotItem.Topictitlelength%>" name="topictitlelength"/>
		                            </td>
		                            <td class="vtop">主题名称的长短，超过将截取</td>
	                            </tr>
                            </tbody>
                        </table>
                    </fieldset>
                    <div class="Navbutton">
	                    <span><button class="ManagerButton" type="button"  onclick="validate();"><img alt="" src="../images/submit.gif" />提 交</button>
                        <button class="ManagerButton" type="button" onclick="javascript:window.location.href='forum_forumhot.aspx'"><img alt="" src="../images/submit.gif"/>返 回</button></span>
                    </div>
                </div>
            </form>
            <script type="text/javascript">
                var topics_sorttype = [{ value: 'Views', text: '浏览量' }, { value: 'LastPost', text: '最后回复时间' }, { value: 'PostDateTime', text: '最新主题' },
                { value: 'Digest', text: '精华主题' }, { value: 'Replies', text: '回复数' }, { value: 'Rate', text: '评分数'}];
                var topics_datacount = [{ value: 7, text: '7' }, { value: 13, text: '13'}];
                var topics_datatimetype = [{ value: 'All', text: '全部' }, { value: 'Day', text: '一天' }, { value: 'ThreeDays', text: '三天' }, { value: 'FiveDays', text: '五天' },
                { value: 'Week', text: '一周' }, { value: 'Month', text: '一个月' }, { value: 'SixMonth', text: '六个月' }, { value: 'Year', text: '一年'}];

                var forums_sorttype = [{ value: 'posts', text: '帖子总数' }, { value: 'topics', text: '主题数' }, { value: 'today', text: '今日发帖数' },
                 { value: 'thismonth', text: '30天发帖数'}];
                var forums_datacount = [{ value: 10, text: '10' }, { value: 20, text: '20'}];

                var users_sorttype = [{ value: 'credits', text: '积分' }, { value: 'posts', text: '发帖数' }, { value: 'digestposts', text: '精华帖数' },
                 { value: 'lastactivity', text: '最后访问时间' }, { value: 'joindate', text: '加入时间'}];


                 var users_datacount = [{ value: 10, text: '10' }, { value: 20, text: '20'}];
                 var users_datatimetype = [{ value: 'posts', text: '全部' }, { value: 'today', text: '1天' }, { value: 'thisweek', text: '7天' }, { value: 'thismonth', text: '30天'}];

                 var pictures_sorttype = [{ value: 'aid', text: '最新图片' }, { value: 'downloads', text: '最多浏览'}];
                 var pictures_datacount = [{ value: 3, text: '3' }, { value: 5, text: '5'}];


                function setdisplay(ctrlname, status) {
                    $(ctrlname + '_title').style.display = status;
                    $(ctrlname + '_content').style.display = status;
                }

                function setselectoptions(ctrlname, items, selectvalue) {
                    if (!$(ctrlname + '_select'))
                        return;
                    if (!items)
                        items = [{ value: 0, text: 0}];
                    var selectoption = $(ctrlname + '_select').options;
                    selectoption.length = 0;

                    var isselected = 0;
                    for (i = 0; i < items.length; i++) {
                        var optionItem = document.createElement('OPTION');
                        optionItem.value = items[i].value;
                        optionItem.text = items[i].text;
                        if (selectvalue && selectvalue == items[i].value) {
                            isselected = 1;
                        }
                        selectoption.add(optionItem);
                    }

                    if (selectvalue && isselected == 1)
                        $(ctrlname + '_select').value = selectvalue;
                }

                function validate() {
                    if ($('forumhotitemname').value == "") {
                        alert('热点名称不能为空');
                        return false;
                    }

                    if ($('datatype').value == "") {
                        alert('数据来源必须选择');
                        return false;
                    }

                    if (!isNumber($('cachetime').value)) {
                        alert('间隔时间必须为数字');
                        return false;
                    }

                    if ($('datatype').value != "users" || $('datatype').value != "pictures") {
                        if (!isNumber($('forumnamelength').value)) {
                            alert('版块名称长度必须为数字');
                            return false;
                        } 
                    }

                    if ($('datatype').value != "users" || $('datatype').value != "forums") {
                        if (!isNumber($('topictitlelength').value)) {
                            alert('标题长度必须为数字');
                            return false;
                        } 
                    }
                    $('form').submit();
                }

                function users_setdatatimetype(obj) {
                    if (obj.value == 'posts')
                        setdisplay('datatimetype', '');
                    else
                        setdisplay('datatimetype', 'none');
                }

                function bindpage(action) {
                    //set all controls show
                    setdisplay('sorttype', '');
                    setdisplay('forumlist', '');
                    setdisplay('datacount', '');
                    setdisplay('datatimetype', '');
                    setdisplay('forumlength', '');
                    setdisplay('titlelength', '');

                    switch (action) {
                        case "topics":
                            setselectoptions('sorttype', topics_sorttype, '<%=forumHotItem.Sorttype%>');
                            setselectoptions('datacount', topics_datacount, '<%=forumHotItem.Dataitemcount%>');
                            setselectoptions('datatimetype', topics_datatimetype, '<%=forumHotItem.Datatimetype%>');
                            break;
                        case "forums":
                            setselectoptions('sorttype', forums_sorttype, '<%=forumHotItem.Sorttype%>');
                            setselectoptions('datacount', forums_datacount, '<%=forumHotItem.Dataitemcount%>');
                            setdisplay('forumlist', 'none');
                            setdisplay('datatimetype', 'none');
                            setdisplay('titlelength', 'none');
                            break;
                        case "users":
                            var itemsorttype = '<%=forumHotItem.Sorttype%>';
                            if (itemsorttype == "thismonth" || itemsorttype == "thisweek" || itemsorttype == "today")
                                itemsorttype = "posts";
                            setselectoptions('sorttype', users_sorttype, itemsorttype);
                            setselectoptions('datacount', users_datacount, '<%=forumHotItem.Dataitemcount%>');
                            setselectoptions('datatimetype', users_datatimetype, '<%=forumHotItem.Datatimetype%>');
                            setdisplay('forumlist', 'none');
                            setdisplay('forumlength', 'none');
                            setdisplay('titlelength', 'none');

                            if ($('sorttype_select').value != 'posts') {
                                setdisplay('datatimetype', 'none');
                            }
                            break;
                        case "pictures":
                            setselectoptions('sorttype', pictures_sorttype, '<%=forumHotItem.Sorttype%>');
                            setselectoptions('datacount', pictures_datacount, '<%=forumHotItem.Dataitemcount%>');
                            setdisplay('datatimetype', 'none');
                            setdisplay('forumlength', 'none');
                            break;
                    }
                }
                bindpage('<%=forumHotItem.Datatype%>');
            </script>
        <%}%>
    </body>
</html>