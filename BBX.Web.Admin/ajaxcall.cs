using System;
using NewLife;
using NewLife.Reflection;
using System.Data;
using System.Text;
using System.Threading;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class ajaxcall : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                int pertask = DNTRequest.GetInt("pertask", 0);
                int lastnumber = DNTRequest.GetInt("lastnumber", 0);
                int startvalue = DNTRequest.GetInt("startvalue", 0);
                int endvalue = DNTRequest.GetInt("endvalue", 0);
                string text = "";
                string key = base.Request.Params["opname"];
                if (key != null)
                {
                    switch (key)
                    {
                        case "UpdatePostSP":
                            //AdminForumStats.UpdatePostSP(pertask, ref lastnumber);
                            text = lastnumber.ToString();
                            break;
                        case "UpdateMyPost":
                            AdminForumStats.UpdateMyPost(pertask, ref lastnumber);
                            text = lastnumber.ToString();
                            break;
                        case "ReSetFourmTopicAPost":
							//AdminForumStats.ReSetFourmTopicAPost();
							// 清空forums表中的帖子数，以便重新统计
							// 取得每个版块在当前分表中的帖子数，累加并更新forums表中版块帖子数

							//!!! 但是实际上SQL处理有误，每重置一次，数量会不断增加，故不迁移
							text = "-1";
                            break;
                        case "ReSetUserDigestPosts":
                            AdminForumStats.ReSetUserDigestPosts();
                            text = "-1";
                            break;
                        case "ReSetUserPosts":
                            AdminForumStats.ReSetUserPosts(pertask, ref lastnumber);
                            text = lastnumber.ToString();
                            break;
                        case "ReSetTopicPosts":
                            AdminForumStats.ReSetTopicPosts(pertask, ref lastnumber);
                            text = lastnumber.ToString();
                            break;
                        case "ReSetFourmTopicAPost_StartEnd":
                            AdminForumStats.ReSetFourmTopicAPost(startvalue, endvalue);
                            text = "1";
                            break;
                        case "ReSetUserDigestPosts_StartEnd":
                            AdminForumStats.ReSetUserDigestPosts(startvalue, endvalue);
                            text = "1";
                            break;
                        case "ReSetUserPosts_StartEnd":
                            AdminForumStats.ReSetUserPosts(startvalue, endvalue);
                            text = "1";
                            break;
                        case "ReSetTopicPosts_StartEnd":
                            AdminForumStats.ResetLastRepliesInfoOfTopics(startvalue, endvalue);
                            text = "1";
                            break;
                        //case "ftptest":
                        //    {
                        //        var fTPs = new FTPs();
                        //        string str = "";
                        //        text = (fTPs.TestConnect(Request["serveraddress"], DNTRequest.GetInt("serverport", 0), Request["username"], Request["password"], DNTRequest.GetInt("timeout", 0), Request["uploadpath"], ref str) ? "ok" : ("远程附件设置测试出现错误！\n描述：" + str));
                        //        break;
                        //    }
                        case "setapp":
                            {
                                var config = APIConfigInfo.Current;
                                config.Enable = (Request["allowpassport"] == "1");

                                //APIConfigs.SaveConfig(config);
                                config.Save();
                                text = "ok";
                                break;
                            }
                        case "location":
                            {
                                string @string = Request["city"];
                                text = "ok";
								//var dataTable = MallPluginProvider.GetInstance().GetLocationsTable();
								//foreach (DataRow dataRow in dataTable.Rows)
								//{
								//	if (dataRow["country"].ToString() == Request["country"] && dataRow["state"].ToString() == Request["state"] && dataRow["city"].ToString() == @string)
								//	{
								//		text = "<img src='../images/false.gif' title='" + @string + "已经存在!'>";
								//		break;
								//	}
								//}
                                break;
                            }
						//case "goodsinfo":
						//	int goodsid = DNTRequest.GetInt("goodsid", 0);
						//	var goodsInfo = MallPluginProvider.GetInstance().GetGoodsInfo(goodsid);
						//	if (goodsInfo == null)
						//	{
						//		text = "商品不存在!";
						//	}
						//	else
						//	{
						//		text = "<table width='100%'><tr><td>" + UBB.UBBToHTML(new PostpramsInfo
						//		{
						//			Allowhtml = 1,
						//			Showimages = 1,
						//			Sdetail = goodsInfo.Message
						//		}) + "</td></tr></table>";
						//	}
						//	break;
                        case "downloadword":
                            {
                                //var dataTable = BanWords.GetBanWordList();
                                //string text2 = "";
                                //if (dataTable.Rows.Count > 0)
                                //{
                                //    for (int i = 0; i < dataTable.Rows.Count; i++)
                                //    {
                                //        text2 = text2 + dataTable.Rows[i][2].ToString() + "=" + dataTable.Rows[i][3].ToString() + "\r\n";
                                //    }
                                //}
                                var text2 = Word.GetBanWordsStr();
                                string s = "words.txt";
                                HttpContext.Current.Response.Clear();
                                HttpContext.Current.Response.Buffer = false;
                                HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + base.Server.UrlEncode(s));
                                HttpContext.Current.Response.ContentType = "text/plain";
                                HttpContext.Current.Response.Write(text2);
                                HttpContext.Current.Response.End();
                                break;
                            }
                        case "gettopicinfo":
                            {
                                var sb = new StringBuilder();
                                var topicInfo = Topic.FindByID(DNTRequest.GetInt("tid", 0));
                                sb.Append("[");
                                if (topicInfo != null)
                                {
                                    sb.Append(string.Format("{{'tid':{0},'title':'{1}'}}", topicInfo.ID, topicInfo.Title));
                                }
                                HttpContext.Current.Response.Clear();
                                HttpContext.Current.Response.ContentType = "application/json";
                                HttpContext.Current.Response.Expires = 0;
                                HttpContext.Current.Response.Cache.SetNoStore();
                                HttpContext.Current.Response.Write(sb.Append("]").ToString());
                                HttpContext.Current.Response.End();
                                break;
                            }
                        case "DeletePrivateMessages":
                            var rs = ShortMessage.DeletePrivateMessages(Request["isnew"] == "true", Request["postdatetime"].ToInt(), Request["msgfromlist"], Request["lowerupper"] == "true", this.DecodeChar(Request["subject"]), this.DecodeChar(Request["message"]), Request["isupdateusernewpm"] == "true");
                            text = string.Format("[{{'count':'{0}'}}]", rs);
                            Thread.Sleep(4000);
                            break;
                        case "sendsmtogroup":
                            {
                                int int5 = DNTRequest.GetInt("start_uid", 0);
                                text = Users.SendPMByGroupidList(Request["groupidlist"], DNTRequest.GetInt("topnumber", 0), ref int5, Request["msgfrom"], DNTRequest.GetInt("msguid", 1), DNTRequest.GetInt("folder", 0), this.DecodeChar(Request["subject"]), Request["postdatetime"], this.DecodeChar(Request["message"])).ToString();
                                text = string.Format("[{{'startuid':{0},'count':'{1}'}}]", int5, text);
                                Thread.Sleep(4000);
                                break;
                            }
                        case "usergroupsendemail":
                            {
                                int int6 = DNTRequest.GetInt("start_uid", 0);
                                text = Users.SendEmailByGroupidList(Request["groupidlist"], DNTRequest.GetInt("topnumber", 0), ref int6, this.DecodeChar(Request["subject"]), this.DecodeChar(Request["body"])).ToString();
                                text = string.Format("[{{'startuid':{0},'count':'{1}'}}]", int6, text);
                                Thread.Sleep(4000);
                                break;
                            }
                        case "updateusercreditbyformula":
                            {
                                int int7 = DNTRequest.GetInt("start_uid", 0);
                                text = Users.UpdateUserCredits(this.DecodeChar(Request["formula"]), int7).ToString();
                                text = string.Format("[{{'startuid':{0},'count':'{1}'}}]", int7 + 100, text);
                                Thread.Sleep(4000);
                                break;
                            }
                        default:
                            break;
                    }
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Expires = 0;
                HttpContext.Current.Response.Cache.SetNoStore();
                base.Response.Write(text);
                base.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1.0);
                base.Response.Expires = -1;
                base.Response.End();
            }
        }

        public string DecodeChar(string str)
        {
            return str.Replace("_plus_", "+").Replace("_and_", "&").Replace("_equal_", "=");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }
    }
}