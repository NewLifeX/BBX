﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using NewLife.Log;
using NewLife.Web;
using XCode;
using XCode.Configuration;

namespace BBX.Entity
{
    /// <summary>帮助</summary>
    public partial class Help : EntityTree<Help>
    {
        #region 对象操作﻿
        static Help()
        {
            // 注册新的实体树操作
            Setting = new HelpSetting();
        }

        class HelpSetting : EntityTreeSetting<Help>
        {
            /// <summary>已重载。</summary>
            public override string Key { get { return __.ID; } }

            /// <summary>关联父键名，一般是Parent加主键，如ParentID</summary>
            public override string Parent { get { return __.Pid; } }

            public override string Sort { get { return __.Orderby; } }

            public override string Name { get { return __.Title; } }

            public override Boolean BigSort { get { return false; } }
        }

        //protected override string ParentKeyName { get { return _.Pid.Name; } }

        //protected override string SortingKeyName { get { return _.Orderby; } }

        //protected override string NameKeyName { get { return _.Title; } }

        //protected override bool BigSort { get { return false; } }

        /// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew"></param>
        public override void Valid(Boolean isNew)
        {
            // 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
            //if (String.IsNullOrEmpty(Name)) throw new ArgumentNullException(_.Name, _.Name.DisplayName + "无效！");
            //if (!isNew && ID < 1) throw new ArgumentOutOfRangeException(_.ID, _.ID.DisplayName + "必须大于0！");

            // 建议先调用基类方法，基类方法会对唯一索引的数据进行验证
            base.Valid(isNew);

            // 在新插入数据或者修改了指定字段时进行唯一性验证，CheckExist内部抛出参数异常
            //if (isNew || Dirtys[_.Name]) CheckExist(_.Name);

        }

        /// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void InitData()
        {
            base.InitData();

            // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
            // Meta.Count是快速取得表记录数
            if (Meta.Count > 0) return;

            // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
            if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}帮助数据……", typeof(Help).Name);

            Add("用户须知", "", 0, 1);
            Add("论坛常见问题", "", 0, 2);
            Add("个人空间常见问题", "", 0, 3);
            Add("相册常见问题", "", 0, 4);
            Add("我必须要注册吗？", "这取决于管理员如何设置 BBX 论坛的用户组权限选项，您甚至有可能必须在注册成正式用户后后才能浏览帖子。当然，在通常情况下，您至少应该是正式用户才能发新帖和回复已有帖子。请 <a href=\"register.aspx\" target=\"_blank\">点击这里</a> 免费注册成为我们的新用户！<br /><br />强烈建议您注册，这样会得到很多以游客身份无法实现的功能。", 1, 1);
            Add("我如何登录论坛？", "如果您已经注册成为该论坛的会员，那么您只要通过访问页面右上的<a href=\"login.aspx\" target=\"_blank\">登录</a>，进入登陆界面填写正确的用户名和密码（如果您设有安全提问，请选择正确的安全提问并输入对应的答案），点击“提交”即可完成登陆如果您还未注册请点击这里。<br /><br />如果需要保持登录，请选择相应的 Cookie 时间，在此时间范围内您可以不必输入密码而保持上次的登录状态。", 1, 2);
            Add("忘记我的登录密码，怎么办？", "当您忘记了用户登录的密码，您可以通过注册时填写的电子邮箱重新设置一个新的密码。点击登录页面中的 <a href=\"getpassword.aspx\" target=\"_blank\">取回密码</a>，按照要求填写您的个人信息，系统将自动发送重置密码的邮件到您注册时填写的 Email 信箱中。如果您的 Email 已失效或无法收到信件，请与论坛管理员联系。", 1, 3);
            Add("我如何使用个性化头像", "在<a href=\"usercppreference.aspx\" target=\"_blank\">用户中心</a>中的 个人设置  -> 个性设置，可以使用论坛自带的头像或者自定义的头像。", 1, 4);
            Add("我如何修改登录密码", "在<a href=\"usercpnewpassword.aspx\" target=\"_blank\">用户中心</a>中的 个人设置 -> 更改密码，填写“原密码”，“新密码”，“确认新密码”。点击“提交”，即可修改。", 1, 5);
            Add("我如何使用个性化签名和昵称", "在<a href=\"usercpprofile.aspx\" target=\"_blank\">用户中心</a>中的 个人设置 -> 编辑个人档案，有一个“昵称”和“个人签名”的选项，可以在此设置。", 1, 6);
            Add("我如何发表新主题，以及投票", "在论坛版块中，点“发帖”，点击即可进入功能齐全的发帖界面。<br /><br />注意：需要发布投票时请在发帖界面的下方开启投票选项进行设置即可。如发布普通主题，直接点击“发帖”，当然您也可以使用版块下面的“快速发帖”发表新帖(如果此选项打开)。一般论坛都设置为需要登录后才能发帖。", 2, 1);
            Add("我如何发表回复", "回复有分三种：第一、帖子最下方的快速回复； 第二、在您想回复的楼层点击右下方“回复”； 第三、完整回复页面，点击本页“新帖”旁边的“回复”。", 2, 2);
            Add("我如何编辑自己的帖子", "在帖子的右上角，有编辑，回复，报告等选项，点击编辑，就可以对帖子进行编辑。", 2, 3);
            Add("我如何出售购买主题", "<li>出售主题：当您进入发贴界面后，如果您所在的用户组有发买卖贴的权限，在“售价(金钱)”后面填写主题的价格，这样其他用户在查看这个帖子的时候就需要进入交费的过程才可以查看帖子。</li><li>购买主题：浏览你准备购买的帖子，在帖子的相关信息的下面有[查看付款记录] [购买主题] [返回上一页] 等链接，点击“购买主题”进行购买。</li>", 2, 4);
            Add("我如何上传附件", "<li>发表新主题的时候上传附件，步骤为：写完帖子标题和内容后点上传附件右方的浏览，然后在本地选择要上传附件的具体文件名，最后点击发表话题。</li><li>发表回复的时候上传附件，步骤为：写完回复楼主的内容，然后点上传附件右方的浏览，找到需要上传的附件，点击发表回复。</li>", 2, 5);
            Add("我如何实现发帖时图文混排效果", "<li>发表新主题的时候点击上传附件左侧的“[插入]”链接把附件标记插入到帖子中适当的位置即可。</li>", 2, 6);
            Add("我如何使用BBX代码", "<table width=\"99%\" cellpadding=\"2\" cellspacing=\"2\"><tr><th width=\"50%\">BBX代码</th><th width=\"402\">效果</th></tr><tr><td>[b]粗体文字 Abc[/b]</td><td><strong>粗体文字 Abc</strong></td></tr><tr><td>[i]斜体文字 Abc[/i]</td><td><i>斜体文字 Abc</i></td></tr><tr><td>[u]下划线文字 Abc[/u]</td><td><u>下划线文字 Abc</u></td></tr><tr><td>[color=red]红颜色[/color]</td><td><font color=\"red\">红颜色</font></td></tr><tr><td>[size=3]文字大小为 3[/size] </td><td><font size=\"3\">文字大小为 3</font></td></tr><tr><td>[font=仿宋]字体为仿宋[/font] </td><td><font face=\"仿宋\">字体为仿宋</font></td></tr><tr><td>[align=Center]内容居中[/align] </td><td><div align=\"center\">内容居中</div></td></tr><tr><td>[url]http://www.newlifex.com[/url]</td><td><a href=\"http://www.newlifex.com\" target=\"_blank\">http://www.newlifex.com</a>（超级链接）</td></tr><tr><td>[url=http://www.newlifex.com]BBX 论坛[/url]</td><td><a href=\"http://www.newlifex.com\" target=\"_blank\">BBX 论坛</a>（超级链接）</td></tr><tr><td>[email]myname@mydomain.com[/email]</td><td><a href=\"mailto:myname@mydomain.com\">myname@mydomain.com</a>（E-mail链接）</td></tr><tr><td>[email=support@newlifex.com]BBX 技术支持[/email]</td><td><a href=\"mailto:support@newlifex.com\">BBX 技术支持（E-mail链接）</a></td></tr><tr><td>[quote]BBX 是由新生命开发团队开发的论坛软件[/quote] </td><td><div style=\"font-size: 12px\"><br /><br /><div class=\"quote\"><h5>引用:</h5><blockquote>原帖由 <i>admin</i> 于 2006-12-26 08:45 发表<br />BBX Board 是由新生命开发团队开发开发的论坛软件</blockquote></div></td></tr> <tr><td>[code]BBX Board 是由新生命开发团队开发的论坛软件[/code] </td><td><div style=\"font-size: 12px\"><br /><br /><div class=\"blockcode\"><h5>代码:</h5><code id=\"code0\">BBX Board 是由新生命开发团队开发的论坛软件</code></div></td></tr><tr><td>[hide]隐藏内容 Abc[/hide]</td><td>效果:只有当浏览者回复本帖时，才显示其中的内容，否则显示为“<b>**** 隐藏信息 跟帖后才能显示 *****</b>”</td></tr><tr><td>[list][*]列表项 #1[*]列表项 #2[*]列表项 #3[/list]</td><td><ul><li>列表项 ＃1</li><li>列表项 ＃2</li><li>列表项 ＃3 </li></ul></td></tr><tr><td>[img]http://www.newlifex.com/templates/default/images/logo.png[/img] </td><td>帖子内显示为：<img src=\"http://www.newlifex.com/templates/default/images/logo.png\" /></td></tr><tr><td>[img=88,31]http://www.newlifex.com/templates/default/images/logo.png[/img] </td><td>帖子内显示为：<img src=\"http://www.newlifex.com/templates/default/images/logo.png\" /></td> </tr> <tr><td>[fly]飞行的效果[/fly]</td><td><marquee scrollamount=\"3\" behavior=\"alternate\" width=\"90%\">飞行的效果</marquee></td></tr><tr><td>[flash]Flash网页地址 [/flash] </td><td>帖子内嵌入 Flash 动画</td></tr><tr><td>X[sup]2[/sup]</td><td>X<sup>2</sup></td></tr><tr><td>X[sub]2[/sub]</td><td>X<sub>2</sub></td></tr></table>", 2, 7);
            Add("我如何使用短消息功能", "您登录后，点击导航栏上的短消息按钮，即可进入短消息管理。点击[发送短消息]按钮，在\"发送到\"后输入收信人的用户名，填写完标题和内容，点提交(或按 Ctrl+Enter 发送)即可发出短消息。<br /><br />如果要保存到发件箱，以在提交前勾选\"保存到发件箱中\"前的复选框。<ul><li>点击收件箱可打开您的收件箱查看收到的短消息。</li><li>点击发件箱可查看保存在发件箱里的短消息。 </li></ul>", 2, 8);
            Add("我如何查看论坛会员数据", "点击导航栏上面的会员，然后显示的是此论坛的会员数据。注：需要论坛管理员开启允许你查看会员资料才可看到。", 2, 9);
            Add("我如何使用搜索", "点击导航栏上面的搜索，输入搜索的关键字并选择一个范围，就可以检索到您有权限访问论坛中的相关的帖子。", 2, 10);
            Add("我如何使用“我的”功能", "<li>会员必须首先<a href=\"login.aspx\" target=\"_blank\">登录</a>，没有用户名的请先<a href=\"register.aspx\" target=\"_blank\">注册</a>；</li><li>登录之后在论坛的左上方会出现一个“我的”的超级链接，点击这个链接之后就可进入到有关于您的信息。</li>", 2, 11);
            Add("我如何向管理员举报帖子", "打开一个帖子，在帖子的右上角可以看到：\"举报\" | \"树型\" | \"收藏\" | \"编辑\" | \"删除\" |\"评分\" 等等几个按钮，单击“举报”按钮即可完成举报某个帖子的操作。", 2, 12);
            Add("我如何“收藏”帖子", "当你浏览一个帖子时，在它的右上角可以看到：\"举报\" | \"树型\" | \"收藏\" | \"编辑\" | \"删除\" |\"评分\"，点击相对应的文字连接即可完成相关的操作。", 2, 13);
            Add("我如何使用RSS订阅", "在论坛的首页和进入版块的页面的右上角就会出现一个rss订阅的小图标<img src=\"templates/default/images/icon_feed.gif\" border=\"0\">，鼠标点击之后将出现本站点的rss地址，你可以将此rss地址放入到你的rss阅读器中进行订阅。", 2, 14);
            Add("我如何清除Cookies", "介绍3种常用浏览器的Cookies清除方法(注：此方法为清除全部的Cookies,请谨慎使用)<ul><li>Internet Explorer: 工具（选项）内的Internet选项→常规选项卡内，IE6直接可以看到删除Cookies的按钮点击即可，IE7为“浏 览历史记录”选项内的删除点击即可清空Cookies。对于Maxthon,腾讯TT等IE核心浏览器一样适用。 </li><li>FireFox:工具→选项→隐私→Cookies→显示Cookie里可以对Cookie进行对应的删除操作。 </li><li>Opera:工具→首选项→高级→Cookies→管理Cookies即可对Cookies进行删除的操作。</li></ul>", 2, 15);
            Add("我如何使用表情代码", "表情是一些用字符表示的表情符号，如果打开表情功能，BBX 会把一些符号转换成小图像，显示在帖子中，更加美观明了。同时BBX支持表情分类，分页功能。插入表情时只需使用鼠标点击表情即可。", 2, 16);
            Add("我如何开通个人空间", "如果您有权限开通“我的个人空间”，当用户登录论坛以后在论坛首页，在搜索框下方有申请个人空间连接点击提交申请，如果管理员已经开启了手动开通则需要等待管理员来审核通过您的申请", 3, 1);
            Add("我如何在个人空间发表日志", "如果您已经开通“个人空间”，当用户登录论坛以后在论坛用户中心 -> 个人空间 -> 管理文章内可以进行发表和管理日志的操作。", 3, 2);
            Add("我如何在相册中上传图片", "如果您已经开通“相册功能”，当用户登录论坛以后在论坛用户中心 -> 相册 -> 管理相册内可以进行发表和管理相册的操作。", 4, 1);

            if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}帮助数据！", typeof(Help).Name);
        }

        static void Add(String title, String message, Int32 pid, Int32 orderby)
        {
            var entity = new Help();
            entity.Title = title;
            entity.Message = message;
            entity.Pid = pid;
            entity.Orderby = orderby;
            entity.Insert();
        }

        ///// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
        ///// <returns></returns>
        //public override Int32 Insert()
        //{
        //    return base.Insert();
        //}

        ///// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        ///// <returns></returns>
        //protected override Int32 OnInsert()
        //{
        //    return base.OnInsert();
        //}
        #endregion

        #region 扩展属性﻿
        #endregion

        #region 扩展查询﻿
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Help FindByID(Int32 id)
        {
            if (Meta.Count >= 1000)
                return Find(_.ID, id);
            else // 实体缓存。
                return Meta.Cache.Entities.Find(__.ID, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }
        #endregion

        #region 高级查询
        // 以下为自定义高级查询的例子

        ///// <summary>
        ///// 查询满足条件的记录集，分页、排序
        ///// </summary>
        ///// <param name="key">关键字</param>
        ///// <param name="orderClause">排序，不带Order By</param>
        ///// <param name="startRowIndex">开始行，0表示第一行</param>
        ///// <param name="maximumRows">最大返回行数，0表示所有行</param>
        ///// <returns>实体集</returns>
        //[DataObjectMethod(DataObjectMethodType.Select, true)]
        //public static EntityList<Help> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        //{
        //    return FindAll(SearchWhere(key), orderClause, null, startRowIndex, maximumRows);
        //}

        ///// <summary>
        ///// 查询满足条件的记录总数，分页和排序无效，带参数是因为ObjectDataSource要求它跟Search统一
        ///// </summary>
        ///// <param name="key">关键字</param>
        ///// <param name="orderClause">排序，不带Order By</param>
        ///// <param name="startRowIndex">开始行，0表示第一行</param>
        ///// <param name="maximumRows">最大返回行数，0表示所有行</param>
        ///// <returns>记录数</returns>
        //public static Int32 SearchCount(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        //{
        //    return FindCount(SearchWhere(key), null, null, 0, 0);
        //}

        /// <summary>构造搜索条件</summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        private static String SearchWhere(String key)
        {
            // WhereExpression重载&和|运算符，作为And和Or的替代


            // SearchWhereByKeys系列方法用于构建针对字符串字段的模糊搜索
            var exp = SearchWhereByKeys(key);

            // 以下仅为演示，2、3行是同一个意思的不同写法，Field（继承自FieldItem）重载了==、!=、>、<、>=、<=等运算符（第4行）
            //exp &= _.Name == "testName"
            //    & !String.IsNullOrEmpty(key) & _.Name == key
            //    .AndIf(!String.IsNullOrEmpty(key), _.Name == key)
            //    | _.ID > 0;

            return exp;
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        public static List<Help> GetHelpList(Int32 id = 0)
        {
            if (id == 0) return Root.AllChilds;

            var entity = Root.AllChilds.Find(__.ID, id);
            if (entity == null) return Root.AllChilds;

            var list = new EntityList<Help>();
            list.Add(entity);
            list.AddRange(entity.AllChilds);
            return list;
        }

        public static void DelHelp(String ids)
        {
            if (ids.IsNullOrWhiteSpace()) return;

            var list = FindAll(_.ID.In(ids.SplitAsInt()), null, null, 0, 0);
            list.Delete();

            Meta.Cache.Clear("删除");
        }

        public static Boolean UpOrder(String[] ords, String[] ids)
        {
            if (ords == null || ords.Length < 1) return false;

            for (int i = 0; i < ids.Length && i < ords.Length; i++)
            {
                var entity = FindByID(Int32.Parse(ids[i]));
                if (entity != null)
                {
                    entity.Orderby = Int32.Parse(ords[i]);
                    entity.Save();
                }
            }

            Meta.Cache.Clear("排序");

            return true;
        }
        #endregion
    }
}