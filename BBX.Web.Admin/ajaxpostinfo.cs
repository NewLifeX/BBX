using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class ajaxpostinfo : UserControl
    {
        public string title = "";
        public string message = "";
        public bool isexist;
        protected internal GeneralConfigInfo config = GeneralConfigInfo.Current;

        public ajaxpostinfo()
        {
            if (DNTRequest.GetInt("tid", 0) == 0)
            {
                return;
            }
            //DataTable postInfo = Posts.GetPostInfo(Request["istopic"] == "true", DNTRequest.GetInt("tid", 0), DNTRequest.GetInt("pid", 0));
            var istopic = Request["istopic"].ToBoolean();
            var tid = Request["tid"].ToInt();
            var pid = Request["pid"].ToInt();
            var pi = istopic ? Post.FindByTid(tid) : Post.FindByID(pid);
            this.GetPostInfo(pi);
            //postInfo.Dispose();
        }

        public void GetPostInfo(Post pi)
        {
            if (pi != null)
            {
                this.isexist = true;
                var ppi = new PostpramsInfo();
                ppi.Fid = pi.Fid;
                ppi.Tid = pi.Tid;
                ppi.Pid = pi.ID;
                ppi.Jammer = 0;
                ppi.Attachimgpost = this.config.Attachimgpost;
                ppi.Showattachmentpath = 1;
                ppi.Showimages = 1;
                ppi.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
                ppi.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
                ppi.Smiliesmax = this.config.Smiliesmax;
                ppi.Bbcodemode = this.config.Bbcodemode;
                ppi.Smileyoff = pi.SmileyOff;
                ppi.BBCode = pi.BBCodeOff == 0;
                ppi.Parseurloff = pi.ParseUrlOff;
                ppi.Allowhtml = pi.HtmlOn;
                ppi.Sdetail = pi.Message;
                this.message = pi.Message;
                if (ppi.Jammer == 1) this.message = ForumUtils.AddJammer(this.message);

                ppi.Sdetail = this.message;
                if (!ppi.Ubbmode)
                    this.message = UBB.UBBToHTML(ppi);
                else
                    this.message = Utils.HtmlEncode(this.message);

                if (pi.Attachment == 1 || new Regex("\\[attach\\](\\d+?)\\[\\/attach\\]", RegexOptions.IgnoreCase).IsMatch(this.message))
                {
                    var list = Attachment.FindAllByPid(pi.ID);
                    //list.Columns.Add("attachimgpost", typeof(Int16));
                    foreach (var att in list)
                    {
                        if (this.message.IndexOf("[attach]" + att.ID + "[/attach]") != -1)
                        {
                            string text;
                            if (att.FileSize > 1024)
                                text = Math.Round((Double)att.FileSize / 1024, 2) + " K";
                            else
                                text = att.FileSize + " B";

                            string newValue;
                            if (att.ImgPost)
                            {
                                att["attachimgpost"] = 1;
                                if (ppi.Showattachmentpath == 1)
                                {
                                    newValue = "<img src=\"../../upload/" + att.FileName + "\" onload=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.alt='点击在新窗口浏览图片 CTRL+鼠标滚轮可放大/缩小';}\" onmouseover=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.style.cursor='hand'; this.alt='点击在新窗口浏览图片 CTRL+Mouse 滚轮可放大/缩小';}\" onclick=\"if(!this.resized) { return true; } else { window.open(this.src); }\" onmousewheel=\"return imgzoom(this);\" />";
                                }
                                else
                                {
                                    newValue = "<img src=\"../../attachment.aspx?attachmentid=" + att.ID + "\" onload=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.alt='点击在新窗口浏览图片 CTRL+鼠标滚轮可放大/缩小';}\" onmouseover=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.style.cursor='hand'; this.alt='点击在新窗口浏览图片 CTRL+Mouse 滚轮可放大/缩小';}\" onclick=\"if(!this.resized) { return true; } else { window.open(this.src); }\" onmousewheel=\"return imgzoom(this);\" />";
                                }
                            }
                            else
                            {
                                att["attachimgpost"] = 0;
                                newValue = "<p><img alt=\"\" src=\"../../images/attachicons/attachment.gif\" border=\"0\" /><span class=\"bold\">附件</span>: <a href=\"../../attachment.aspx?attachmentid=" + att["aid"].ToString() + "\" target=\"_blank\">" + att["attachment"].ToString().Trim() + "</a> (" + att["postdatetime"].ToString() + ", " + text + ")<br />该附件被下载次数 " + att["downloads"].ToString() + "</p>";
                            }
                            this.message = this.message.Replace("[attach]" + att.ID + "[/attach]", newValue);
                        }
                    }
                    //list.Dispose();
                }
                this.title = Utils.RemoveHtml(pi.Title);
            }
        }
    }
}