using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using BBX.Plugin.Preview;
using NewLife;

namespace BBX.Web.UI
{
    public class AttachUploadPage : PageBase
    {
        public int forumid = DNTRequest.GetInt("forumid", 0);

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            var action = Request["action"];
            var operation = Request["operation"];
            if (action != "swfupload" && ForumUtils.IsCrossSitePost()) return;

            bool flag = true;
            if (operation == "upload")
            {
                var uid = Request["uid"].ToInt(-1);

                var online = uid > 0 ? Online.FindByUserID(uid) : Online.Current;
                if (online != null)
                {
                    string b = DES.Encode(online.ID + "," + online.UserName, online.Password.Substring(0, 10)).Replace("+", "[");
                    if (DNTRequest.GetString("hash") == b)
                    {
                        this.userid = online.UserID;
                        this.usergroupinfo = online.Group;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
            }

            var userInfo = Users.GetUserInfo(this.userid);
            var forumInfo = Forums.GetForumInfo(this.forumid);
            // 文件大小
            int size = this.userid > 0 ? Attachment.GetUploadFileSizeByuserid(this.userid) : 0;
            // 今天还可以上传多大的文件
            int size2 = this.usergroupinfo.MaxSizeperday - size;
            //string allowAttachmentType = Attachments.GetAllowAttachmentType(this.usergroupinfo, forumInfo);
            string attachmentTypeArray = AttachType.GetAttachmentTypeArray(usergroupinfo, forumInfo);
            string attachmentTypeString = AttachType.GetAttachmentTypeString(usergroupinfo, forumInfo);
            // 是否图片
            var isimg = Request["type"].EqualIgnoreCase("image");
            if (action == "swfupload" && operation == "config")
            {
                this.GetConfig(this.userid, attachmentTypeString, size2, isimg);
                return;
            }
            int aid = DNTRequest.GetInt("aid", 0);
            string text2 = "";
            //var sb = new StringBuilder();
            if (!(flag & UserAuthority.PostAttachAuthority(forumInfo, this.usergroupinfo, this.userid, ref text2)))
            {
                this.ResponseXML("BBXUPLOAD|11|0|-1");
                return;
            }
            if (size2 <= 0)
            {
                this.ResponseXML("BBXUPLOAD|3|0|-1");
                return;
            }
            string errMsg = "";
            Attachment[] atts = null;
            try
            {
                atts = SaveRequestFiles(this.forumid, this.config.Maxattachments, this.usergroupinfo.MaxSizeperday, this.usergroupinfo.MaxAttachSize, size, attachmentTypeArray, forumInfo.DisableWatermark ? 0 : this.config.Watermarkstatus, this.config, "Filedata", isimg);
                if (atts.Length == 0)
                {
                    ResponseXML("0");
                    return;
                }
            }
            catch (XException ex)
            {
                errMsg = ex.Message;

                var code = this.GetNoUploadCode(errMsg);
                if (aid > 0)
                    ResponseXML("BBXUPDATE|{0}|{1}|{2}|{3}", code, null, aid, 0);
                else if (action != "swfupload")
                    ResponseXML("BBXUPLOAD|{0}|{1}|{2}", code, 0, 0);
                else
                    ResponseXML((code != 0) ? "error" : "0");
                return;
            }

            //var array2 = atts;
            //for (int i = 0; i < array2.Length; i++)
            //{
            //    var attachmentInfo = array2[i];
            //    text3 = (string.IsNullOrEmpty(attachmentInfo.Sys_noupload) ? text3 : attachmentInfo.Sys_noupload);
            //    attachmentInfo.Uid = this.userid;
            //}
            foreach (var item in atts)
            {
                //if (!item.Sys_noupload.IsNullOrWhiteSpace()) text3 = item.Sys_noupload;
                item.Uid = userid;
            }
            if (aid <= 0)
            {
                Attachments.CreateAttachments(atts);
            }
            else
            {
                if (string.IsNullOrEmpty(errMsg))
                {
                    //var att = Attachments.GetAttachmentInfo(@int);
                    var att = Attachment.FindByID(aid);
                    if (att == null || (userInfo.AdminID <= 0 && att.Uid != this.userid)) return;

                    att.PostDateTime = atts[0].PostDateTime;
                    att.FileName = atts[0].FileName;
                    att.Description = atts[0].Description;
                    att.FileType = atts[0].FileType;
                    att.FileSize = (Int32)atts[0].FileSize;
                    att.Name = atts[0].Name;
                    att.Width = atts[0].Width;
                    att.Height = atts[0].Height;
                    att.IsImage = atts[0].IsImage;
                    //Attachments.UpdateAttachment(att);
                    att.Save();
                }
            }
            //var sb2 = new StringBuilder();
            int isimage = atts[0].FileType.StartsWith("image") ? 0 : -1;
            int noUploadCode = this.GetNoUploadCode(errMsg);
            if (aid > 0)
                ResponseXML("BBXUPDATE|{0}|{1}|{2}|{3}", noUploadCode, atts[0].Name, aid, isimage);
            else if (action != "swfupload")
                ResponseXML("BBXUPLOAD|{0}|{1}|{2}", noUploadCode, atts[0].ID, isimage);
            else
                ResponseXML((noUploadCode != 0) ? "error" : atts[0].ID + "");

            //this.ResponseXML(sb2.ToString());
        }

        private void GetConfig(int uid, string allowFormats, int maxupload, bool isImage)
        {
            if (isImage)
            {
                if (string.IsNullOrEmpty(allowFormats))
                {
                    allowFormats = "*.jpg,*.gif,*.png,*.jpeg";
                }
                else
                {
                    string[] array = allowFormats.Split(',');
                    allowFormats = "";
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string text = array2[i];
                        if (Utils.InArray(text, "jpg,gif,png,jpeg"))
                        {
                            allowFormats = allowFormats + "*." + text + ",";
                        }
                    }
                    allowFormats = allowFormats.TrimEnd(',');
                }
            }
            else
            {
                if (string.IsNullOrEmpty(allowFormats))
                {
                    allowFormats = "*.*";
                }
                else
                {
                    allowFormats = "*." + allowFormats.Replace(",", ",*.");
                }
            }
            string text2 = DES.Encode(oluserinfo.ID + "," + oluserinfo.UserName, this.oluserinfo.Password.Substring(0, 10)).Replace("+", "[");
            string format = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<parameter>\r\n<allowsExtend><extend depict=\"{0}\">{1}</extend></allowsExtend>\r\n<language>\r\n<okbtn>确定</okbtn>\r\n<ctnbtn>继续</ctnbtn>\r\n<fileName>文件名</fileName>\r\n<size>文件大小</size>\r\n<stat>上传进度</stat>\r\n<browser>浏览</browser>\r\n<delete>删除</delete>\r\n<return>返回</return>\r\n<upload>上传</upload>\r\n<okTitle>上传完成</okTitle>\r\n<okMsg>文件上传完成</okMsg>\r\n<uploadTitle>正在上传</uploadTitle>\r\n<uploadMsg1>总共有</uploadMsg1>\r\n<uploadMsg2>个文件等待上传,正在上传第</uploadMsg2>\r\n<uploadMsg3>个文件</uploadMsg3>\r\n<bigFile>文件过大</bigFile>\r\n<uploaderror>上传失败</uploaderror>\r\n</language>\r\n<config><userid>{2}</userid><hash>{3}</hash><maxupload>{4}</maxupload></config>\r\n</parameter>";
            this.ResponseXML(string.Format(format, new object[]
            {
                isImage ? "All Image File" : "All Support Formats",
                allowFormats,
                uid,
                text2,
                maxupload
            }));
        }

        private int GetNoUploadCode(string message)
        {
            if (string.IsNullOrEmpty(message)) return 0;

            if (message == "文件格式无效") return 1;

            if (message == "文件大于今天允许上传的字节数" || message == "文件大于该类型附件允许的字节数" || message == "文件大于单个文件允许上传的字节数") return 3;

            if (message.IndexOf("图片宽度") > 0 || message.IndexOf("图片高度") > 0) return 7;

            return -1;
        }

        private void ResponseXML(String xmlnode, params Object[] args)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "text/html";
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.Response.Write(xmlnode.F(args));
            HttpContext.Current.Response.End();
        }

        public Attachment[] SaveRequestFiles(int forumid, int MaxAllowFileCount, int MaxSizePerDay, int MaxFileSize, int TodayUploadedSize, string AllowFileType, int watermarkstatus, GeneralConfigInfo config, string filekey, bool isImage)
        {
            string[] array = Utils.SplitString(AllowFileType, "|");
            string[] types = new string[array.Length];
            int[] sizes = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                types[i] = Utils.CutString(array[i], 0, array[i].LastIndexOf(","));
                sizes[i] = Utils.CutString(array[i], array[i].LastIndexOf(",") + 1).ToInt();
            }

            var keys = Request.Files.Keys;
            var count = Request.Files.Count;
            var num = 0;
            for (int i = 0; i < count; i++)
            {
                if (!Request.Files[i].FileName.IsNullOrWhiteSpace() && keys[i] == filekey) num++;
            }
            if (num > MaxAllowFileCount) return new Attachment[0];

            var list = new List<Attachment>();
            num = 0;
            var random = new Random((int)DateTime.Now.Ticks);
            for (int k = 0; k < count; k++)
            {
                var hfile = Request.Files[k];
                if (hfile.FileName.IsNullOrWhiteSpace() || keys[k] != filekey) continue;

                string fileName = Path.GetFileName(hfile.FileName);
                //string ext = Utils.CutString(fileName, fileName.LastIndexOf(".") + 1).ToLower();
                var ext = Path.GetExtension(fileName).TrimStart(".");
                string contentType = hfile.ContentType;
                int contentLength = hfile.ContentLength;
                var file = "";
                if (contentType.EqualIgnoreCase("application/octet-stream")) contentType = GetContentType(ext);

                var att = new Attachment();
                if (Attachment.IsImgFilename(fileName) && !contentType.StartsWith("image") || !ValidateImage(contentType, hfile.InputStream))
                    throw new XException("文件格式无效");


                int inArrayID = Utils.GetInArrayID(ext, types);
                if (inArrayID < 0) throw new XException("文件格式无效");

                if (MaxSizePerDay - TodayUploadedSize < contentLength)
                    throw new XException("文件大于今天允许上传的字节数");

                if (contentLength > sizes[inArrayID])
                    throw new XException("文件大于该类型附件允许的字节数");

                if (MaxFileSize < contentLength)
                    throw new XException("文件大于单个文件允许上传的字节数");

                TodayUploadedSize += contentLength;
                var mapPath = Attachment.UploadPath.GetFullPath();
                var attachmentPath = GetAttachmentPath(forumid, config, ext);
                file = string.Format("{0}{1}{2}.{3}", Environment.TickCount & 0x7FFFFFFF, k, random.Next(1000, 9999), ext);
                file = attachmentPath.CombinePath(file);
                var fullfile = mapPath.CombinePath(file).EnsureDirectory();

                att.FileSize = contentLength;
                if (ext.EqualIgnoreCase("bmp", "jpg", "jpeg", "png") && contentType.StartsWith("image"))
                    SaveImage(att, hfile, fullfile, watermarkstatus != 0);
                else
                    hfile.SaveAs(fullfile);

                try
                {
                    var instance = PreviewProvider.GetInstance(ext);
                    if (instance != null) instance.OnSaved(fullfile);
                }
                catch { }

                att.FileName = file;
                att.Description = "";
                att.FileType = contentType;
                att.Name = fileName;
                att.Downloads = 0;
                att.PostDateTime = DateTime.Now;
                att.Sys_index = k;
                att.IsImage = isImage;

                list.Add(att);

                num++;
            }
            return list.ToArray();
        }

        void SaveImage(Attachment att, HttpPostedFile hfile, String fullfile, Boolean watermarkstatus)
        {
            var image = Image.FromStream(hfile.InputStream);
            if (config.Attachimgmaxwidth > 0 && image.Width > config.Attachimgmaxwidth)
                throw new XException("图片宽度为" + image.Width + ", 系统允许的最大宽度为" + config.Attachimgmaxwidth);
            if (config.Attachimgmaxheight > 0 && image.Height > config.Attachimgmaxheight)
                throw new XException("图片高度为" + image.Width + ", 系统允许的最大高度为" + config.Attachimgmaxheight);

            att.Width = image.Width;
            att.Height = image.Height;
            //if (!String.IsNullOrEmpty(att.Sys_noupload)) return;

            if (!watermarkstatus)
            {
                //if (ftp.Allowupload == 1 && ftp.Reservelocalattach == 0)
                //{
                //    hfile.SaveAs(mapPath + str);
                //}
                //else
                {
                    hfile.SaveAs(fullfile);
                }
                //att.FileSize = contentLength;
            }
            else
            {
                if (config.Watermarktype == 1 && File.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath + "watermark/" + config.Watermarkpic)))
                {
                    //if (ftp.Allowupload == 1 && ftp.Reservelocalattach == 0)
                    //{
                    //    AddImageSignPic(image, mapPath + str, Utils.GetMapPath(BaseConfigs.GetForumPath + "watermark/" + config.Watermarkpic), config.Watermarkstatus, config.Attachimgquality, config.Watermarktransparency);
                    //}
                    //else
                    {
                        AddImageSignPic(image, fullfile, Utils.GetMapPath(BaseConfigs.GetForumPath + "watermark/" + config.Watermarkpic), config.Watermarkstatus, config.Attachimgquality, config.Watermarktransparency);
                    }
                }
                else
                {
                    string text4 = config.Watermarktext.Replace("{1}", config.Forumtitle);
                    text4 = text4.Replace("{2}", "http://" + DNTRequest.GetCurrentFullHost() + "/");
                    text4 = text4.Replace("{3}", DateTime.Now.ToString("yyyy-MM-dd"));
                    text4 = text4.Replace("{4}", DateTime.Now.ToString("HH:mm:ss"));
                    //if (ftp.Allowupload == 1 && ftp.Reservelocalattach == 0)
                    //{
                    //    AddImageSignText(image, mapPath + str, text4, config.Watermarkstatus, config.Attachimgquality, config.Watermarkfontname, config.Watermarkfontsize);
                    //}
                    //else
                    {
                        AddImageSignText(image, fullfile, text4, config.Watermarkstatus, config.Attachimgquality, config.Watermarkfontname, config.Watermarkfontsize);
                    }
                }
                //if (ftp.Allowupload == 1 && ftp.Reservelocalattach == 0)
                //{
                //    att.Filesize = new FileInfo(mapPath + str).Length;
                //}
                //else
                {
                    att.FileSize = (Int32)new FileInfo(fullfile).Length;
                }
            }
        }

        private static bool ValidateImage(string fileType, Stream inputStream)
        {
            if (inputStream.Length == 0L) return false;

            if (!fileType.StartsWith("image")) return true;

            try
            {
                var image = Image.FromStream(inputStream);
                image.Dispose();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static string GetContentType(string fileextname)
        {
            switch (fileextname.TrimStart("."))
            {
                case "jpeg":
                    return "image/jpeg";
                case "jpg":
                    return "image/jpeg";
                case "js":
                    return "application/x-javascript";
                case "jsp":
                    return "text/html";
                case "gif":
                    return "image/gif";
                case "htm":
                    return "text/html";
                case "html":
                    return "text/html";
                case "asf":
                    return "video/x-ms-asf";
                case "avi":
                    return "video/avi";
                case "bmp":
                    return "application/x-bmp";
                case "asp":
                    return "text/asp";
                case "wma":
                    return "audio/x-ms-wma";
                case "wav":
                    return "audio/wav";
                case "wmv":
                    return "video/x-ms-wmv";
                case "ra":
                    return "audio/vnd.rn-realaudio";
                case "ram":
                    return "audio/x-pn-realaudio";
                case "rm":
                    return "application/vnd.rn-realmedia";
                case "rmvb":
                    return "application/vnd.rn-realmedia-vbr";
                case "xhtml":
                    return "text/html";
                case "png":
                    return "image/png";
                case "ppt":
                    return "application/x-ppt";
                case "tif":
                    return "image/tiff";
                case "tiff":
                    return "image/tiff";
                case "xls":
                    return "application/x-xls";
                case "xlw":
                    return "application/x-xlw";
                case "xml":
                    return "text/xml";
                case "xpl":
                    return "audio/scpls";
                case "swf":
                    return "application/x-shockwave-flash";
                case "torrent":
                    return "application/x-bittorrent";
                case "dll":
                    return "application/x-msdownload";
                case "asa":
                    return "text/asa";
                case "asx":
                    return "video/x-ms-asf";
                case "au":
                    return "audio/basic";
                case "css":
                    return "text/css";
                case "doc":
                    return "application/msword";
                case "exe":
                    return "application/x-msdownload";
                case "mp1":
                    return "audio/mp1";
                case "mp2":
                    return "audio/mp2";
                case "mp2v":
                    return "video/mpeg";
                case "mp3":
                    return "audio/mp3";
                case "mp4":
                    return "video/mpeg4";
                case "mpa":
                    return "video/x-mpg";
                case "mpd":
                    return "application/vnd.ms-project";
                case "mpe":
                    return "video/x-mpeg";
                case "mpeg":
                    return "video/mpg";
                case "mpg":
                    return "video/mpg";
                case "mpga":
                    return "audio/rn-mpeg";
                case "mpp":
                    return "application/vnd.ms-project";
                case "mps":
                    return "video/x-mpeg";
                case "mpt":
                    return "application/vnd.ms-project";
                case "mpv":
                    return "video/mpg";
                case "mpv2":
                    return "video/mpeg";
                case "wml":
                    return "text/vnd.wap.wml";
                case "wsdl":
                    return "text/xml";
                case "xsd":
                    return "text/xml";
                case "xsl":
                    return "text/xml";
                case "xslt":
                    return "text/xml";
                case "htc":
                    return "text/x-component";
                case "mdb":
                    return "application/msaccess";
                case "zip":
                    return "application/zip";
                case "rar":
                    return "application/x-rar-compressed";
                case "*":
                    return "application/octet-stream";
                case "001":
                    return "application/x-001";
                case "301":
                    return "application/x-301";
                case "323":
                    return "text/h323";
                case "906":
                    return "application/x-906";
                case "907":
                    return "drawing/907";
                case "a11":
                    return "application/x-a11";
                case "acp":
                    return "audio/x-mei-aac";
                case "ai":
                    return "application/postscript";
                case "aif":
                    return "audio/aiff";
                case "aifc":
                    return "audio/aiff";
                case "aiff":
                    return "audio/aiff";
                case "anv":
                    return "application/x-anv";
                case "awf":
                    return "application/vnd.adobe.workflow";
                case "biz":
                    return "text/xml";
                case "bot":
                    return "application/x-bot";
                case "c4t":
                    return "application/x-c4t";
                case "c90":
                    return "application/x-c90";
                case "cal":
                    return "application/x-cals";
                case "cat":
                    return "application/vnd.ms-pki.seccat";
                case "cdf":
                    return "application/x-netcdf";
                case "cdr":
                    return "application/x-cdr";
                case "cel":
                    return "application/x-cel";
                case "cer":
                    return "application/x-x509-ca-cert";
                case "cg4":
                    return "application/x-g4";
                case "cgm":
                    return "application/x-cgm";
                case "cit":
                    return "application/x-cit";
                case "class":
                    return "java/*";
                case "cml":
                    return "text/xml";
                case "cmp":
                    return "application/x-cmp";
                case "cmx":
                    return "application/x-cmx";
                case "cot":
                    return "application/x-cot";
                case "crl":
                    return "application/pkix-crl";
                case "crt":
                    return "application/x-x509-ca-cert";
                case "csi":
                    return "application/x-csi";
                case "cut":
                    return "application/x-cut";
                case "dbf":
                    return "application/x-dbf";
                case "dbm":
                    return "application/x-dbm";
                case "dbx":
                    return "application/x-dbx";
                case "dcd":
                    return "text/xml";
                case "dcx":
                    return "application/x-dcx";
                case "der":
                    return "application/x-x509-ca-cert";
                case "dgn":
                    return "application/x-dgn";
                case "dib":
                    return "application/x-dib";
                case "dot":
                    return "application/msword";
                case "drw":
                    return "application/x-drw";
                case "dtd":
                    return "text/xml";
                case "dwf":
                    return "application/x-dwf";
                case "dwg":
                    return "application/x-dwg";
                case "dxb":
                    return "application/x-dxb";
                case "dxf":
                    return "application/x-dxf";
                case "edn":
                    return "application/vnd.adobe.edn";
                case "emf":
                    return "application/x-emf";
                case "eml":
                    return "message/rfc822";
                case "ent":
                    return "text/xml";
                case "epi":
                    return "application/x-epi";
                case "eps":
                    return "application/x-ps";
                case "etd":
                    return "application/x-ebx";
                case "fax":
                    return "image/fax";
                case "fdf":
                    return "application/vnd.fdf";
                case "fif":
                    return "application/fractals";
                case "fo":
                    return "text/xml";
                case "frm":
                    return "application/x-frm";
                case "g4":
                    return "application/x-g4";
                case "gbr":
                    return "application/x-gbr";
                case "gcd":
                    return "application/x-gcd";
                case "gl2":
                    return "application/x-gl2";
                case "gp4":
                    return "application/x-gp4";
                case "hgl":
                    return "application/x-hgl";
                case "hmr":
                    return "application/x-hmr";
                case "hpg":
                    return "application/x-hpgl";
                case "hpl":
                    return "application/x-hpl";
                case "hqx":
                    return "application/mac-binhex40";
                case "hrf":
                    return "application/x-hrf";
                case "hta":
                    return "application/hta";
                case "htt":
                    return "text/webviewhtml";
                case "htx":
                    return "text/html";
                case "icb":
                    return "application/x-icb";
                case "ico":
                    return "application/x-ico";
                case "iff":
                    return "application/x-iff";
                case "ig4":
                    return "application/x-g4";
                case "igs":
                    return "application/x-igs";
                case "iii":
                    return "application/x-iphone";
                case "img":
                    return "application/x-img";
                case "ins":
                    return "application/x-internet-signup";
                case "isp":
                    return "application/x-internet-signup";
                case "IVF":
                    return "video/x-ivf";
                case "java":
                    return "java/*";
                case "jfif":
                    return "image/jpeg";
                case "jpe":
                    return "application/x-jpe";
                case "la1":
                    return "audio/x-liquid-file";
                case "lar":
                    return "application/x-laplayer-reg";
                case "latex":
                    return "application/x-latex";
                case "lavs":
                    return "audio/x-liquid-secure";
                case "lbm":
                    return "application/x-lbm";
                case "lmsff":
                    return "audio/x-la-lms";
                case "ls":
                    return "application/x-javascript";
                case "ltr":
                    return "application/x-ltr";
                case "m1v":
                    return "video/x-mpeg";
                case "m2v":
                    return "video/x-mpeg";
                case "m3u":
                    return "audio/mpegurl";
                case "m4e":
                    return "video/mpeg4";
                case "mac":
                    return "application/x-mac";
                case "man":
                    return "application/x-troff-man";
                case "math":
                    return "text/xml";
                case "mfp":
                    return "application/x-shockwave-flash";
                case "mht":
                    return "message/rfc822";
                case "mhtml":
                    return "message/rfc822";
                case "mi":
                    return "application/x-mi";
                case "mid":
                    return "audio/mid";
                case "midi":
                    return "audio/mid";
                case "mil":
                    return "application/x-mil";
                case "mml":
                    return "text/xml";
                case "mnd":
                    return "audio/x-musicnet-download";
                case "mns":
                    return "audio/x-musicnet-stream";
                case "mocha":
                    return "application/x-javascript";
                case "movie":
                    return "video/x-sgi-movie";
                case "mpw":
                    return "application/vnd.ms-project";
                case "mpx":
                    return "application/vnd.ms-project";
                case "mtx":
                    return "text/xml";
                case "mxp":
                    return "application/x-mmxp";
                case "net":
                    return "image/pnetvue";
                case "nrf":
                    return "application/x-nrf";
                case "nws":
                    return "message/rfc822";
                case "odc":
                    return "text/x-ms-odc";
                case "out":
                    return "application/x-out";
                case "p10":
                    return "application/pkcs10";
                case "p12":
                    return "application/x-pkcs12";
                case "p7b":
                    return "application/x-pkcs7-certificates";
                case "p7c":
                    return "application/pkcs7-mime";
                case "p7m":
                    return "application/pkcs7-mime";
                case "p7r":
                    return "application/x-pkcs7-certreqresp";
                case "p7s":
                    return "application/pkcs7-signature";
                case "pc5":
                    return "application/x-pc5";
                case "pci":
                    return "application/x-pci";
                case "pcl":
                    return "application/x-pcl";
                case "pcx":
                    return "application/x-pcx";
                case "pdf":
                    return "application/pdf";
                case "pdx":
                    return "application/vnd.adobe.pdx";
                case "pfx":
                    return "application/x-pkcs12";
                case "pgl":
                    return "application/x-pgl";
                case "pic":
                    return "application/x-pic";
                case "pko":
                    return "application/vnd.ms-pki.pko";
                case "pl":
                    return "application/x-perl";
                case "plg":
                    return "text/html";
                case "pls":
                    return "audio/scpls";
                case "plt":
                    return "application/x-plt";
                case "pot":
                    return "application/vnd.ms-powerpoint";
                case "ppa":
                    return "application/vnd.ms-powerpoint";
                case "ppm":
                    return "application/x-ppm";
                case "pps":
                    return "application/vnd.ms-powerpoint";
                case "pr":
                    return "application/x-pr";
                case "prf":
                    return "application/pics-rules";
                case "prn":
                    return "application/x-prn";
                case "prt":
                    return "application/x-prt";
                case "ps":
                    return "application/x-ps";
                case "ptn":
                    return "application/x-ptn";
                case "pwz":
                    return "application/vnd.ms-powerpoint";
                case "r3t":
                    return "text/vnd.rn-realtext3d";
                case "ras":
                    return "application/x-ras";
                case "rat":
                    return "application/rat-file";
                case "rdf":
                    return "text/xml";
                case "rec":
                    return "application/vnd.rn-recording";
                case "red":
                    return "application/x-red";
                case "rgb":
                    return "application/x-rgb";
                case "rjs":
                    return "application/vnd.rn-realsystem-rjs";
                case "rjt":
                    return "application/vnd.rn-realsystem-rjt";
                case "rlc":
                    return "application/x-rlc";
                case "rle":
                    return "application/x-rle";
                case "rmf":
                    return "application/vnd.adobe.rmf";
                case "rmi":
                    return "audio/mid";
                case "rmj":
                    return "application/vnd.rn-realsystem-rmj";
                case "rmm":
                    return "audio/x-pn-realaudio";
                case "rmp":
                    return "application/vnd.rn-rn_music_package";
                case "rms":
                    return "application/vnd.rn-realmedia-secure";
                case "rmx":
                    return "application/vnd.rn-realsystem-rmx";
                case "rnx":
                    return "application/vnd.rn-realplayer";
                case "rp":
                    return "image/vnd.rn-realpix";
                case "rpm":
                    return "audio/x-pn-realaudio-plugin";
                case "rsml":
                    return "application/vnd.rn-rsml";
                case "rt":
                    return "text/vnd.rn-realtext";
                case "rtf":
                    return "application/msword";
                case "rv":
                    return "video/vnd.rn-realvideo";
                case "sam":
                    return "application/x-sam";
                case "sat":
                    return "application/x-sat";
                case "sdp":
                    return "application/sdp";
                case "sdw":
                    return "application/x-sdw";
                case "sit":
                    return "application/x-stuffit";
                case "slb":
                    return "application/x-slb";
                case "sld":
                    return "application/x-sld";
                case "slk":
                    return "drawing/x-slk";
                case "smi":
                    return "application/smil";
                case "smil":
                    return "application/smil";
                case "smk":
                    return "application/x-smk";
                case "snd":
                    return "audio/basic";
                case "sol":
                    return "text/plain";
                case "sor":
                    return "text/plain";
                case "spc":
                    return "application/x-pkcs7-certificates";
                case "spl":
                    return "application/futuresplash";
                case "spp":
                    return "text/xml";
                case "ssm":
                    return "application/streamingmedia";
                case "sst":
                    return "application/vnd.ms-pki.certstore";
                case "stl":
                    return "application/vnd.ms-pki.stl";
                case "stm":
                    return "text/html";
                case "sty":
                    return "application/x-sty";
                case "svg":
                    return "text/xml";
                case "tdf":
                    return "application/x-tdf";
                case "tg4":
                    return "application/x-tg4";
                case "tga":
                    return "application/x-tga";
                case "tld":
                    return "text/xml";
                case "top":
                    return "drawing/x-top";
                case "tsd":
                    return "text/xml";
                case "txt":
                    return "text/plain";
                case "uin":
                    return "application/x-icq";
                case "uls":
                    return "text/iuls";
                case "vcf":
                    return "text/x-vcard";
                case "vda":
                    return "application/x-vda";
                case "vdx":
                    return "application/vnd.visio";
                case "vml":
                    return "text/xml";
                case "vpg":
                    return "application/x-vpeg005";
                case "vsd":
                    return "application/vnd.visio";
                case "vss":
                    return "application/vnd.visio";
                case "vst":
                    return "application/vnd.visio";
                case "vsw":
                    return "application/vnd.visio";
                case "vsx":
                    return "application/vnd.visio";
                case "vtx":
                    return "application/vnd.visio";
                case "vxml":
                    return "text/xml";
                case "wax":
                    return "audio/x-ms-wax";
                case "wb1":
                    return "application/x-wb1";
                case "wb2":
                    return "application/x-wb2";
                case "wb3":
                    return "application/x-wb3";
                case "wbmp":
                    return "image/vnd.wap.wbmp";
                case "wiz":
                    return "application/msword";
                case "wk3":
                    return "application/x-wk3";
                case "wk4":
                    return "application/x-wk4";
                case "wkq":
                    return "application/x-wkq";
                case "wks":
                    return "application/x-wks";
                case "wm":
                    return "video/x-ms-wm";
                case "wmd":
                    return "application/x-ms-wmd";
                case "wmf":
                    return "application/x-wmf";
                case "wmx":
                    return "video/x-ms-wmx";
                case "wmz":
                    return "application/x-ms-wmz";
                case "wp6":
                    return "application/x-wp6";
                case "wpd":
                    return "application/x-wpd";
                case "wpg":
                    return "application/x-wpg";
                case "wpl":
                    return "application/vnd.ms-wpl";
                case "wq1":
                    return "application/x-wq1";
                case "wr1":
                    return "application/x-wr1";
                case "wri":
                    return "application/x-wri";
                case "wrk":
                    return "application/x-wrk";
                case "ws":
                    return "application/x-ws";
                case "ws2":
                    return "application/x-ws";
                case "wsc":
                    return "text/scriptlet";
                case "wvx":
                    return "video/x-ms-wvx";
                case "xdp":
                    return "application/vnd.adobe.xdp";
                case "xdr":
                    return "text/xml";
                case "xfd":
                    return "application/vnd.adobe.xfd";
                case "xfdf":
                    return "application/vnd.adobe.xfdf";
                case "xq":
                    return "text/xml";
                case "xql":
                    return "text/xml";
                case "xquery":
                    return "text/xml";
                case "xwd":
                    return "application/x-xwd";
                case "x_b":
                    return "application/x-x_b";
                case "x_t":
                    return "application/x-x_t";
            }
            return "application/octet-stream";
        }

        private static string GetAttachmentPath(int forumid, GeneralConfigInfo config, string fileExtName)
        {
            var sep = Path.DirectorySeparatorChar;
            switch (config.Attachsave)
            {
                case 1:
                    return DateTime.Now.ToString("yyyy-MM-dd").Replace('-', sep) + sep + forumid;
                case 2:
                    return forumid + "" + sep;
                case 3:
                    return fileExtName + sep;
                default:
                    return DateTime.Now.ToString("yyyy-MM-dd").Replace('-', sep);
            }
            //var sb = new StringBuilder("");
            //if (config.Attachsave == 1)
            //{
            //    sb.Append(DateTime.Now.ToString("yyyy"));
            //    sb.Append(Path.DirectorySeparatorChar);
            //    sb.Append(DateTime.Now.ToString("MM"));
            //    sb.Append(Path.DirectorySeparatorChar);
            //    sb.Append(DateTime.Now.ToString("dd"));
            //    sb.Append(Path.DirectorySeparatorChar);
            //    sb.Append(forumid.ToString());
            //    sb.Append(Path.DirectorySeparatorChar);
            //}
            //else
            //{
            //    if (config.Attachsave == 2)
            //    {
            //        sb.Append(forumid);
            //        sb.Append(Path.DirectorySeparatorChar);
            //    }
            //    else
            //    {
            //        if (config.Attachsave == 3)
            //        {
            //            sb.Append(fileExtName);
            //            sb.Append(Path.DirectorySeparatorChar);
            //        }
            //        else
            //        {
            //            sb.Append(DateTime.Now.ToString("yyyy"));
            //            sb.Append(Path.DirectorySeparatorChar);
            //            sb.Append(DateTime.Now.ToString("MM"));
            //            sb.Append(Path.DirectorySeparatorChar);
            //            sb.Append(DateTime.Now.ToString("dd"));
            //            sb.Append(Path.DirectorySeparatorChar);
            //        }
            //    }
            //}
            //return sb.ToString();
        }

        public static void AddImageSignPic(Image img, string filename, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
        {
            img = Process(img);
            var graphics = Graphics.FromImage(img);
            var image = new Bitmap(watermarkFilename);
            if (image.Height >= img.Height || image.Width >= img.Width) return;

            var atts = new ImageAttributes();
            var map = new ColorMap[]
            {
                new ColorMap
                {
                    OldColor = Color.FromArgb(255, 0, 255, 0),
                    NewColor = Color.FromArgb(0, 0, 0, 0)
                }
            };
            atts.SetRemapTable(map, ColorAdjustType.Bitmap);
            float num = 0.5f;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
            {
                num = (float)watermarkTransparency / 10f;
            }
            float[][] array = new float[5][];
            //float[][] arg_B2_0 = array;
            //int arg_B2_1 = 0;
            float[] array2 = new float[5];
            array2[0] = 1f;
            array[0] = array2;
            //float[][] arg_C9_0 = array;
            //int arg_C9_1 = 1;
            float[] array3 = new float[5];
            array3[1] = 1f;
            array[1] = array3;
            //float[][] arg_E0_0 = array;
            //int arg_E0_1 = 2;
            float[] array4 = new float[5];
            array4[2] = 1f;
            array[2] = array4;
            //float[][] arg_F4_0 = array;
            //int arg_F4_1 = 3;
            float[] array5 = new float[5];
            array5[3] = num;
            array[3] = array5;
            array[4] = new float[] { 0f, 0f, 0f, 0f, 1f };
            float[][] newColorMatrix = array;

            var newColorMatrix2 = new ColorMatrix(newColorMatrix);
            atts.SetColorMatrix(newColorMatrix2, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            int x = 0;
            int y = 0;
            switch (watermarkStatus)
            {
                case 1:
                    x = (int)((float)img.Width * 0.01f);
                    y = (int)((float)img.Height * 0.01f);
                    break;

                case 2:
                    x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
                    y = (int)((float)img.Height * 0.01f);
                    break;

                case 3:
                    x = (int)((float)img.Width * 0.99f - (float)image.Width);
                    y = (int)((float)img.Height * 0.01f);
                    break;

                case 4:
                    x = (int)((float)img.Width * 0.01f);
                    y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
                    break;

                case 5:
                    x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
                    y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
                    break;

                case 6:
                    x = (int)((float)img.Width * 0.99f - (float)image.Width);
                    y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
                    break;

                case 7:
                    x = (int)((float)img.Width * 0.01f);
                    y = (int)((float)img.Height * 0.99f - (float)image.Height);
                    break;

                case 8:
                    x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
                    y = (int)((float)img.Height * 0.99f - (float)image.Height);
                    break;

                case 9:
                    x = (int)((float)img.Width * 0.99f - (float)image.Width);
                    y = (int)((float)img.Height * 0.99f - (float)image.Height);
                    break;
            }
            graphics.DrawImage(image, new Rectangle(x, y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, atts);
            //var imageEncoders = ImageCodecInfo.GetImageEncoders();
            //ImageCodecInfo imageCodecInfo = null;
            ////ImageCodecInfo[] array6 = imageEncoders;
            //for (int i = 0; i < imageEncoders.Length; i++)
            //{
            //    var imageCodecInfo2 = imageEncoders[i];
            //    if (imageCodecInfo2.MimeType.IndexOf("jpeg") > -1)
            //    {
            //        imageCodecInfo = imageCodecInfo2;
            //    }
            //}
            var imageCodecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(e => e.MimeType.Contains("jpeg"));
            var encoderParameters = new EncoderParameters();
            //long[] array7 = new long[1];
            if (quality < 0 || quality > 100) quality = 80;

            //array7[0] = (long)quality;
            var encoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, new Int64[] { quality });
            encoderParameters.Param[0] = encoderParameter;
            if (imageCodecInfo != null)
                img.Save(filename, imageCodecInfo, encoderParameters);
            else
                img.Save(filename);

            graphics.Dispose();
            img.Dispose();
            image.Dispose();
            atts.Dispose();
        }

        public static void AddImageSignText(Image img, string filename, string watermarkText, int watermarkStatus, int quality, string fontname, int fontsize)
        {
            img = Process(img);
            var graphics = Graphics.FromImage(img);
            var font = new Font(fontname, (float)fontsize, FontStyle.Regular, GraphicsUnit.Pixel);
            var sizeF = graphics.MeasureString(watermarkText, font);
            float num = 0f;
            float num2 = 0f;
            switch (watermarkStatus)
            {
                case 1:
                    num = (float)img.Width * 0.01f;
                    num2 = (float)img.Height * 0.01f;
                    break;

                case 2:
                    num = (float)img.Width * 0.5f - sizeF.Width / 2f;
                    num2 = (float)img.Height * 0.01f;
                    break;

                case 3:
                    num = (float)img.Width * 0.99f - sizeF.Width;
                    num2 = (float)img.Height * 0.01f;
                    break;

                case 4:
                    num = (float)img.Width * 0.01f;
                    num2 = (float)img.Height * 0.5f - sizeF.Height / 2f;
                    break;

                case 5:
                    num = (float)img.Width * 0.5f - sizeF.Width / 2f;
                    num2 = (float)img.Height * 0.5f - sizeF.Height / 2f;
                    break;

                case 6:
                    num = (float)img.Width * 0.99f - sizeF.Width;
                    num2 = (float)img.Height * 0.5f - sizeF.Height / 2f;
                    break;

                case 7:
                    num = (float)img.Width * 0.01f;
                    num2 = (float)img.Height * 0.99f - sizeF.Height;
                    break;

                case 8:
                    num = (float)img.Width * 0.5f - sizeF.Width / 2f;
                    num2 = (float)img.Height * 0.99f - sizeF.Height;
                    break;

                case 9:
                    num = (float)img.Width * 0.99f - sizeF.Width;
                    num2 = (float)img.Height * 0.99f - sizeF.Height;
                    break;
            }
            graphics.DrawString(watermarkText, font, new SolidBrush(Color.White), num + 1f, num2 + 1f);
            graphics.DrawString(watermarkText, font, new SolidBrush(Color.Black), num, num2);
            //ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            //ImageCodecInfo imageCodecInfo = null;
            //ImageCodecInfo[] array = imageEncoders;
            //for (int i = 0; i < array.Length; i++)
            //{
            //    ImageCodecInfo imageCodecInfo2 = array[i];
            //    if (imageCodecInfo2.MimeType.IndexOf("jpeg") > -1)
            //    {
            //        imageCodecInfo = imageCodecInfo2;
            //    }
            //}
            var imageCodecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(e => e.MimeType.Contains("jpeg"));
            var encoderParameters = new EncoderParameters();
            long[] array2 = new long[1];
            if (quality < 0 || quality > 100) quality = 80;

            //array2[0] = (long)quality;
            var encoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, new Int64[] { quality });
            encoderParameters.Param[0] = encoderParameter;
            if (imageCodecInfo != null)
                img.Save(filename, imageCodecInfo, encoderParameters);
            else
                img.Save(filename);

            graphics.Dispose();
            img.Dispose();
        }

        static Image Process(Image img)
        {
            if (!IsPixelFormatIndexed(img.PixelFormat)) return img;

            var bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(img, 0, 0);
            }

            return bmp;
        }

        /// <summary>会产生graphics异常的PixelFormat</summary>
        private static PixelFormat[] indexedPixelFormats = { PixelFormat.Undefined, PixelFormat.DontCare,PixelFormat.Format16bppArgb1555, PixelFormat.Format1bppIndexed, PixelFormat.Format4bppIndexed,PixelFormat.Format8bppIndexed
    };

        /// <summary>判断图片的PixelFormat 是否在 引发异常的 PixelFormat 之中</summary>
        /// <param name="imgPixelFormat">原图片的PixelFormat</param>
        /// <returns></returns>
        private static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            foreach (PixelFormat pf in indexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }

            return false;
        }
    }
}