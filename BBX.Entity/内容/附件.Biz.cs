/*
 * XCoder v5.1.5002.17097
 * 作者：nnhy/X
 * 时间：2013-09-11 09:31:00
 * 版权：版权所有 (C) 新生命开发团队 2002~2013
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BBX.Common;
using BBX.Config;
using NewLife.Log;
using NewLife.Reflection;
using NewLife.Threading;
using XCode;

namespace BBX.Entity
{
    public enum AttachmentFileType
    {
        FileAttachment,
        ImageAttachment,
        All
    }

    /// <summary>附件</summary>
    public partial class Attachment : Entity<Attachment>
    {
        #region 对象操作﻿
        static Attachment()
        {
            Meta.Factory.AdditionalFields.Add(__.Downloads);

            new TimerX(DeleteAllToday, null, 5000, 3600 * 1000);
        }

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
            //if (isNew || Dirtys[__.Name]) CheckExist(__.Name);

        }

        protected override void OnLoad()
        {
            base.OnLoad();

            this.TrimField();
        }

        /// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        /// <returns></returns>
        protected override Int32 OnInsert()
        {
            var rs = base.OnInsert();

            var myatt = new MyAttachment();
            myatt.CopyFrom(this);
            myatt.ID = ID;
            rs += myatt.Insert();

            return rs;
        }

        protected override int OnUpdate()
        {
            if (ID <= 0) return -1;

            var rs = base.OnUpdate();

            var myatt = MyAttachment.FindByKey(ID);
            if (myatt == null) myatt = new MyAttachment();
            myatt.CopyFrom(this);
            myatt.ID = ID;
            rs += myatt.Save();

            return rs;
        }

        protected override int OnDelete()
        {
            string mapPath = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/");
            var name = (FileName + "").Trim();
            if (!String.IsNullOrEmpty(name) && File.Exists(mapPath + name))
            {
                File.Delete(mapPath + name);
            }

            var myatt = MyAttachment.FindByKey(ID);
            if (myatt != null) myatt.Delete();

            return base.OnDelete();
        }
        #endregion

        #region 扩展属性﻿
        private List<String> hasLoad = new List<String>();
        private Topic _Topic;
        /// <summary>所属主题</summary>
        public Topic Topic
        {
            get
            {
                if (_Topic == null && !Dirtys.ContainsKey("Topic"))
                {
                    _Topic = Topic.FindByID(Tid);
                    Dirtys.Add("Topic", true);
                }
                return _Topic;
            }
            set { _Topic = value; }
        }

        /// <summary>帖子标题</summary>
        public String Title { get { return Topic == null ? null : Topic.Title; } }

        private Int32 _Sys_index;
        /// <summary>属性说明</summary>
        public Int32 Sys_index { get { return _Sys_index; } set { _Sys_index = value; } }

        public Boolean ImgPost { get { return IsImgFilename(FileName); } }

        //private String _Sys_noupload;
        ///// <summary>属性说明</summary>
        //public String Sys_noupload { get { return _Sys_noupload; } set { _Sys_noupload = value; } }

        /// <summary>是否本地文件。远程文件包括http://和ftp://，也可能https://</summary>
        public Boolean IsLocal { get { return String.IsNullOrEmpty(FileName) || !FileName.Contains("://"); } }

        /// <summary>完整路径</summary>
        public String FullFileName
        {
            get
            {
                var name = FileName;
                if (String.IsNullOrEmpty(name)) return name;

                if (name.Contains("://")) return name;

                return UploadPath.CombinePath(FileName).GetFullPath();
            }
        }

        /// <summary>是否压缩包</summary>
        public Boolean IsZip { get { return FileName.EndsWithIgnoreCase("rar", "zip", "7z", "gzip", "gz"); } }

        private Boolean _Isbought;
        /// <summary>是否已经购买。猜测是判断当前用户是否已经购买</summary>
        public Boolean IsBought { get { return _Isbought; } set { _Isbought = value; } }

        private Boolean _AllowRead;
        /// <summary>是否允许读取</summary>
        public Boolean AllowRead { get { return _AllowRead; } set { _AllowRead = value; } }

        private Boolean _Inserted;
        /// <summary>是否已经插入？？？</summary>
        public Boolean Inserted { get { return _Inserted; } set { _Inserted = value; } }

        private Int32 _Getattachperm;
        /// <summary>属性说明</summary>
        public Int32 Getattachperm { get { return _Getattachperm; } set { _Getattachperm = value; } }

        private String _Preview;
        /// <summary>预览</summary>
        public String Preview { get { return _Preview; } set { _Preview = value; } }
        #endregion

        #region 扩展查询﻿
        /// <summary>根据P编号查找</summary>
        /// <param name="pid">P编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Attachment> FindAllByPid(Int32 pid)
        {
            if (Meta.Count >= 1000)
                return FindAll(_.Pid, pid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.Pid, pid);
        }
        public static EntityList<Attachment> FindAllByPids(String pids)
        {
            return FindAll(_.Pid.In(pids.SplitAsInt()), null, null, 0, 0);
        }

        public static Int32 FindCountByPid(Int32 pid)
        {
            if (Meta.Count >= 1000)
                return FindCount(_.Pid, pid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.Pid, pid).Count;
        }

        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Attachment FindByID(Int32 id)
        {
            // 实体缓存
            if (Meta.Count < 1000) return Meta.Cache.Entities.Find(__.ID, id);

            // 单对象缓存
            return Meta.SingleCache[id];
        }

        /// <summary>根据T编号查找</summary>
        /// <param name="tid">T编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Attachment> FindAllByTid(Int32 tid)
        {
            if (Meta.Count >= 1000)
                return FindAll(_.Tid, tid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.Tid, tid);
        }

        public static EntityList<Attachment> FindAllByTids(String tids)
        {
            return FindAll(_.Tid.In(tids.SplitAsInt()), null, null, 0, 0);
        }

        public static Int32 FindCountByTid(Int32 tid)
        {
            if (Meta.Count >= 1000)
                return FindCount(__.Tid, tid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.Tid, tid).Count;
        }

        /// <summary>根据U编号查找</summary>
        /// <param name="uid">U编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Attachment> FindAllByUid(Int32 uid)
        {
            if (Meta.Count >= 1000)
                return FindAll(_.Uid, uid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.Uid, uid);
        }
        #endregion

        #region 高级查询
        //public static EntityList<Attachment> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        //{
        //    return FindAll(SearchWhere(key), orderClause, null, startRowIndex, maximumRows);
        //}

        //public static Int32 SearchCount(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        //{
        //    return FindCount(SearchWhere(key), null, null, 0, 0);
        //}

        public static String SearchWhere(Int32 forumId, Int32 fileSizeMin, Int32 fileSizeMax, Int32 downLoadsMin, Int32 downLoadsMax, Int32 days, string fileName, string description, string poster)
        {
            var exp = new WhereExpression();

            if (forumId > 0) exp &= _.Pid.In(Post.FindSQLByFid(forumId));

            if (fileSizeMin > 0) exp &= _.FileSize >= fileSizeMin;
            if (fileSizeMax > 0) exp &= _.FileSize <= fileSizeMax;

            if (downLoadsMin > 0) exp &= _.Downloads >= downLoadsMin;
            if (downLoadsMax > 0) exp &= _.Downloads <= downLoadsMax;

            if (days > 0) exp &= _.PostDateTime >= DateTime.Now.AddDays(-days);

            if (!fileName.IsNullOrWhiteSpace()) exp &= _.FileName.Contains(fileName);
            if (!description.IsNullOrWhiteSpace()) exp &= _.Description.Contains(description);

            if (!poster.IsNullOrWhiteSpace()) exp &= _.Pid.In(Post.FindSQLByPoster(poster));

            return exp;
        }

        public static EntityList<Attachment> FindAllByIDs(String ids)
        {
            if (String.IsNullOrEmpty(ids)) return new EntityList<Attachment>();

            return FindAll(_.ID.In(ids.SplitAsInt()), null, null, 0, 0);
        }

        public static EntityList<Attachment> SearchByUidAndDays(Int32 uid, Int32 days = 0)
        {
            if (uid <= 0) return new EntityList<Attachment>();

            var exp = _.Uid == uid;
            // 多少天以前
            if (days > 0) exp &= _.PostDateTime < DateTime.Now.AddDays(-days);
            return FindAll(exp, null, null, 0, 0);
        }

        public static EntityList<Attachment> GetAttachList(String condition)
        {
            if (condition != null)
            {
                condition = condition.Trim();
                if (condition.StartsWithIgnoreCase("where"))
                {
                    condition = condition.Substring("where".Length).Trim();
                }
            }

            return FindAll(condition, null, null, 0, 0);
        }
        #endregion

        #region 扩展操作
        static HashSet<String> imgexts = new HashSet<String>(new String[] { "jpg", "jpeg", "gif", "png", "bmp" }, StringComparer.OrdinalIgnoreCase);
        public static Boolean IsImgFilename(String filename)
        {
            var ext = Path.GetExtension(filename + "").TrimStart(".");
            if (String.IsNullOrEmpty(ext)) return false;

            return imgexts.Contains(ext);
        }
        #endregion

        #region 业务
        public static EntityList<Attachment> GetWebSiteAggHotImages(int count, string orderby, string fidlist, int continuous)
        {
            //IF @continuous = 1
            //    BEGIN
            //        IF @fidlist <> ''
            //            SET @fidlist = 'AND [fid] IN('+@fidlist+') '
            //        EXEC('SELECT TOP '+ @count +' [attach].[aid],[attach].[tid],[attach].[filename],[attach].[attachment],[topic].[title] 
            //          FROM [dnt_attachments] AS [attach] LEFT JOIN [dnt_topics] AS [topic] ON [attach].[tid] = [topic].[tid] AND [topic].[displayorder]>=0 WHERE
            //aid = (SELECT MIN(aid) from [dnt_attachments] where [width] > 360 AND [height] > 240 and [tid]=[topic].[tid]) '+ @fidlist +' ORDER BY [attach].['+ @orderby +'] DESC')
            //    END
            //ELSE
            //    BEGIN
            //        IF @fidlist <> ''
            //            SET @fidlist = 'AND [topic].[fid] IN('+@fidlist+') '
            //        EXEC('SELECT TOP '+ @count +' [attach].[tid],[attach].[filename],[attach].[attachment],[topic].[title] 
            //          FROM [dnt_attachments] AS [attach] LEFT JOIN [dnt_topics] AS [topic] ON [attach].[tid] = [topic].[tid] AND [topic].[displayorder]>=0  WHERE [attach].[width] > 360  AND [height] > 240 '+ @fidlist+' ORDER BY ['+ @orderby +'] DESC')
            //    END
            //GO
            var exp = _.Width > 360 & _.Height > 240;
            exp &= _.Tid.In(Topic.FindIDSQLByFIDList(fidlist));
            var order = Meta.Table.FindByName(orderby);
            //EntityList<Attachment> list = null;
            if (continuous == 1)
            {
                //exp.Builder.Append(" Group By " + _.Tid.ColumnName);
                //exp &= _.Tid.GroupBy();

                // 计算每个帖子的最小ID的附件
                //var where = FindSQL(exp, null, String.Format("min({0}) as {0}", _.ID.ColumnName), 0, 0);
                var where = FindSQL(exp & _.Tid.GroupBy(), null, _.ID.Min(), 0, 0);

                //if (!String.IsNullOrEmpty(orderby)) orderby += " Desc";
                return FindAll(_.ID.In(where), order.Desc(), null, 0, count);
            }
            else
            {
                //exp.Builder.Append(" Group By " + _.Tid.ColumnName);
                //exp &= _.Tid.GroupBy();

                //if (!String.IsNullOrEmpty(orderby)) orderby += " Desc";
                //list = FindAll(exp, orderby, null, 0, count);
                return FindAll(exp, order.Desc(), null, 0, count);
            }
            //var dt = list.ToDataTable(false);
            //if (list.Count <= 0) return dt;

            //// 附加上主题标题
            //dt.Columns.Add("title", typeof(String));
            //foreach (DataRow dr in dt.Rows)
            //{
            //    var entity = Topic.FindByID((Int32)dr["tid"]);
            //    if (entity != null) dr["title"] = entity.Title;
            //}

            //return dt;
        }

        public static int GetUploadFileSizeByuserid(int uid)
        {
            //return BBX.Data.Attachments.GetUploadFileSizeByUserId(uid);
            var list = FindAll(_.Uid == uid & _.PostDateTime >= DateTime.Now.Date, _.FileSize.Desc(), _.FileSize.Sum(), 0, 0);
            return list.Count > 0 ? list[0].FileSize : 0;
        }

        private static void DeleteAllToday(Object state)
        {
            // 删除所有人今天未使用的附件
            var list = Attachment.FindAllNoUsed(0, DateTime.MinValue);
            if (list.Count > 0)
            {
                //XTrace.WriteLine("删除所有人今天未使用的附件：{0}", list.ToList().Join(",", e => e.ID + ""));
                XTrace.WriteLine("删除所有人今天未使用的附件：{0}", list.Join("ID", ","));
                list.Delete();
            }
        }

        public static EntityList<Attachment> FindAllNoUsed(Int32 userid, DateTime posttime, AttachmentFileType attachmentType = AttachmentFileType.All)
        {
            var exp = _.Tid == 0 & _.Pid == 0;
            if (userid > 0) exp &= _.Uid == userid;
            if (posttime > DateTime.MinValue) exp &= _.PostDateTime > posttime;
            if (attachmentType < AttachmentFileType.All) exp &= _.IsImage == attachmentType;
            return FindAll(exp, null, null, 0, 100);
        }

        public static EntityList<Attachment> FindAllByUid(Int32 userid, DateTime posttime, AttachmentFileType attachmentType = AttachmentFileType.All)
        {
            var exp = new WhereExpression();
            if (userid > 0) exp &= _.Uid == userid;
            if (posttime > DateTime.MinValue) exp &= _.PostDateTime > posttime;
            if (attachmentType < AttachmentFileType.All) exp &= _.IsImage == attachmentType;
            return FindAll(exp, null, null, 0, 100);
        }

        public static EntityList<Attachment> FindAllByUid(Int32 userid, string attachId)
        {
            //return BBX.Data.Attachments.GetEditPostAttachList(userid, attachId).ToArray();

            //IF OBJECT_ID('dnt_geteditpostattachlist','P') IS NOT NULL
            //DROP PROC [dnt_geteditpostattachlist]
            //GO

            //CREATE PROCEDURE [dnt_geteditpostattachlist]
            //@uid INT,
            //@aidlist VARCHAR(2000)
            //AS
            //BEGIN
            //IF @uid=0
            //SELECT [aid], [uid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads], [width], [height], [attachprice],[isimage]
            //FROM [dnt_attachments] WITH (NOLOCK) WHERE aid in (SELECT [item] FROM [dnt_split](@aidlist, ','))
            //ELSE
            //SELECT [aid], [uid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads], [width], [height], [attachprice],[isimage]
            //FROM [dnt_attachments] WITH (NOLOCK) WHERE aid in (SELECT [item] FROM [dnt_split](@aidlist, ',')) and UID=@uid
            //END
            //GO

            var exp = _.ID.In(attachId.SplitAsInt());
            if (userid > 0) exp &= _.Uid == userid;
            return FindAll(exp, null, null, 0, 0);
        }
        #endregion

        #region 实体转换
        /// <summary>转为另一个实体类对象。快速反射</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Cast<T>() where T : class, new()
        {
            var entity = new T();
            var fs = GetAllFields();
            foreach (var pi in entity.GetType().GetProperties())
            {
                if (fs.Contains(pi.Name)) entity.SetValue(pi, this[pi.Name]);
            }

            return entity;
        }

        protected virtual ICollection<String> GetAllFields()
        {
            return new HashSet<String>(Meta.AllFields.Select(e => e.Name), StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region 设置
        private static String _UploadPath;
        /// <summary>附件上传目录</summary>
        public static String UploadPath
        {
            get
            {
                if (_UploadPath == null) _UploadPath = NewLife.Configuration.Config.GetConfig<String>("BBX.UploadPath", "../Upload/");
                return _UploadPath;
            }
            set { _UploadPath = value; }
        }
        #endregion
    }
}