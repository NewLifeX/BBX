using System;
using System.Web.UI.HtmlControls;
using BBX.Cache;
using BBX.Control;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class cachemanage : AdminPage
    {
        protected HtmlForm Form1;
        protected Button ResetAllCache;
        protected Button ResetMGinf;
        protected Button ResetForumInf;
        protected Button ResetFirstAnnounce;
        protected Button ResetSmiles;
        protected Button ResetThemeIcon;
        protected Button ReSetDigestTopicList;
        protected Button ResetForumsStaticInf;
        protected Button ResetTemplateDropDown;
        protected Button ResetLink;
        protected Button ResetForumList;
        protected Button ResetRssAll;
        protected Button ResetValidUserExtField;
        protected Button ResetMedalList;
        protected Button ReSetStatisticsSearchtime;
        protected Button ReSetCommonAvatarList;
        protected Button ReSetMagicList;
        protected Button ReSetScoreset;
        protected Button ReSetAlbumCategory;
        protected Button ReSetPostTableInfo;
        protected Button ResetUGinf;
        protected Button ResetAnnonceList;
        protected Button ResetForumDropList;
        protected Button ResetAddressRefer;
        protected Button ResetFlag;
        protected Button ReSetHotTopicList;
        protected Button ResetAttachSize;
        protected Button ResetOnlineInco;
        protected Button ResetWord;
        protected Button ResetRss;
        protected Button ResetTemplateIDList;
        protected Button ResetOnlineUserInfo;
        protected Button ReSetAdsList;
        protected Button ReSetStatisticsSearchcount;
        protected Button ReSetJammer;
        protected Button ReSetScorePaySet;
        protected Button ReSetAggregation;
        protected Button ResetRssByFid;
        protected TextBox txtRssfid;
        protected Button ReSetTopiclistByFid;
        protected TextBox txtTopiclistFid;
        protected Button ResetForumBaseSet;
        protected Button ReSetRecentTopicList;
        protected Button ReSetTag;

        private void ReSetDigestTopicList_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetDigestTopicList(16);
                this.SubmitReturnInf();
            }
        }

        private void ReSetHotTopicList_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetHotTopicList(16, 30);
                this.SubmitReturnInf();
            }
        }

        private void ReSetAdsList_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetAdsList();
                this.SubmitReturnInf();
            }
        }

        private void ReSetStatisticsSearchtime_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetStatisticsSearchtime();
                this.SubmitReturnInf();
            }
        }

        private void ReSetStatisticsSearchcount_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetStatisticsSearchcount();
                this.SubmitReturnInf();
            }
        }

        private void ReSetCommonAvatarList_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetCommonAvatarList();
                this.SubmitReturnInf();
            }
        }

        private void ReSetJammer_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetJammer();
                this.SubmitReturnInf();
            }
        }

        private void ReSetMagicList_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetMagicList();
                this.SubmitReturnInf();
            }
        }

        private void ReSetScorePaySet_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetScorePaySet();
                this.SubmitReturnInf();
            }
        }

        private void ReSetPostTableInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //Caches.ReSetPostTableInfo();
                this.SubmitReturnInf();
            }
        }

        private void ReSetTopiclistByFid_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (String.IsNullOrEmpty(this.txtTopiclistFid.Text))
                {
                    base.RegisterStartupScript("", "<script>alert('重新设置相应主题列表的版块参数无效!');window.location.href='global_cachemanage.aspx';</script>");
                    return;
                }
                Caches.ReSetTopiclistByFid(this.txtTopiclistFid.Text);
                this.SubmitReturnInf();
            }
        }

        private void ResetMGinf_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //Caches.ReSetAdminGroupList();
                this.SubmitReturnInf();
            }
        }

        private void ResetUGinf_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //Caches.ReSetUserGroupList();
                this.SubmitReturnInf();
            }
        }

        private void ResetForumInf_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetModeratorList();
                this.SubmitReturnInf();
            }
        }

        private void ResetAnnonceList_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetAnnouncementList();
                this.SubmitReturnInf();
            }
        }

        private void ResetFirstAnnounce_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetSimplifiedAnnouncementList();
                this.SubmitReturnInf();
            }
        }

        private void ResetForumDropList_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetForumListBoxOptions();
                this.SubmitReturnInf();
            }
        }

        private void ResetSmiles_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetSmiliesList();
                this.SubmitReturnInf();
            }
        }

        private void ResetThemeIcon_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetIconsList();
                this.SubmitReturnInf();
            }
        }

        private void ResetForumBaseSet_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetConfig();
                this.SubmitReturnInf();
            }
        }

        private void ResetAddressRefer_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetSiteUrls();
                this.SubmitReturnInf();
            }
        }

        private void ResetForumsStaticInf_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetStatistics();
                this.SubmitReturnInf();
            }
        }

        private void ResetAllCache_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetAllCache();
                this.SubmitReturnInf();
            }
        }

        private void ReSetScoreset_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetScoreset();
                this.SubmitReturnInf();
            }
        }

        private void ResetAttachSize_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetAttachmentTypeArray();
                this.SubmitReturnInf();
            }
        }

        private void ResetTemplateDropDown_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetTemplateListBoxOptionsCache();
                this.SubmitReturnInf();
            }
        }

        private void ResetOnlineInco_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //Caches.ReSetOnlineGroupIconList();
                this.SubmitReturnInf();
            }
        }

        private void ResetLink_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetForumLinkList();
                this.SubmitReturnInf();
            }
        }

        private void ResetWord_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetBanWordList();
                this.SubmitReturnInf();
            }
        }

        private void ResetForumList_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetForumList();
                this.SubmitReturnInf();
            }
        }

        private void ResetRss_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetRss();
                this.SubmitReturnInf();
            }
        }

        private void ResetOnlineUserInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetOnlineUserTable();
                this.SubmitReturnInf();
            }
        }

        private void ResetRssByFid_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (String.IsNullOrEmpty(this.txtRssfid.Text))
                {
                    base.RegisterStartupScript("", "<script>alert('重新设置指定版块RSS的版块参数无效!');window.location.href='global_cachemanage.aspx';</script>");
                    return;
                }
                Caches.ReSetForumRssXml(this.txtRssfid.Text.ToInt());
                this.SubmitReturnInf();
            }
        }

        private void ResetRssAll_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetRssXml();
                this.SubmitReturnInf();
            }
        }

        private void ResetTemplateIDList_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetValidTemplateIDList();
                this.SubmitReturnInf();
            }
        }

        private void ResetValidUserExtField_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetValidScoreName();
                this.SubmitReturnInf();
            }
        }

        private void ResetFlag_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetCustomEditButtonList();
                this.SubmitReturnInf();
            }
        }

        private void ResetMedalList_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //Caches.ReSetMedalsList();
                this.SubmitReturnInf();
            }
        }

        private void ReSetAggregation_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //AggregationFacade.BaseAggregation.ClearAllDataBind();
                this.SubmitReturnInf();
            }
        }

        protected void ReSetNavPopupMenu_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetNavPopupMenu();
            }
        }

        private void SubmitReturnInf()
        {
            if (base.CheckCookie())
            {
                base.RegisterStartupScript("PAGE", "window.location.href='global_cachemanage.aspx';");
            }
        }

        private void ReSetTag_Click(object sender, EventArgs e)
        {
            XCache.Remove("/Forum/Tag/Hot-" + this.config.Hottagcount);
        }

        protected void ReSetAlbumCategory_Click(object sender, EventArgs e)
        {
            //if (base.CheckCookie())
            //{
            //    Caches.ResetAlbumCategory();
            //    this.SubmitReturnInf();
            //}
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.ResetMGinf.Click += new EventHandler(this.ResetMGinf_Click);
            this.ResetUGinf.Click += new EventHandler(this.ResetUGinf_Click);
            this.ResetForumInf.Click += new EventHandler(this.ResetForumInf_Click);
            this.ResetAnnonceList.Click += new EventHandler(this.ResetAnnonceList_Click);
            this.ResetFirstAnnounce.Click += new EventHandler(this.ResetFirstAnnounce_Click);
            this.ResetForumDropList.Click += new EventHandler(this.ResetForumDropList_Click);
            this.ResetSmiles.Click += new EventHandler(this.ResetSmiles_Click);
            this.ResetThemeIcon.Click += new EventHandler(this.ResetThemeIcon_Click);
            this.ResetForumBaseSet.Click += new EventHandler(this.ResetForumBaseSet_Click);
            this.ReSetScoreset.Click += new EventHandler(this.ReSetScoreset_Click);
            this.ResetAddressRefer.Click += new EventHandler(this.ResetAddressRefer_Click);
            this.ResetForumsStaticInf.Click += new EventHandler(this.ResetForumsStaticInf_Click);
            this.ResetAttachSize.Click += new EventHandler(this.ResetAttachSize_Click);
            this.ResetTemplateDropDown.Click += new EventHandler(this.ResetTemplateDropDown_Click);
            this.ResetOnlineInco.Click += new EventHandler(this.ResetOnlineInco_Click);
            this.ResetLink.Click += new EventHandler(this.ResetLink_Click);
            this.ResetWord.Click += new EventHandler(this.ResetWord_Click);
            this.ResetForumList.Click += new EventHandler(this.ResetForumList_Click);
            this.ResetRss.Click += new EventHandler(this.ResetRss_Click);
            this.ResetRssByFid.Click += new EventHandler(this.ResetRssByFid_Click);
            this.ResetRssAll.Click += new EventHandler(this.ResetRssAll_Click);
            this.ResetTemplateIDList.Click += new EventHandler(this.ResetTemplateIDList_Click);
            this.ResetValidUserExtField.Click += new EventHandler(this.ResetValidUserExtField_Click);
            this.ResetOnlineUserInfo.Click += new EventHandler(this.ResetOnlineUserInfo_Click);
            this.ResetAllCache.Click += new EventHandler(this.ResetAllCache_Click);
            this.ResetFlag.Click += new EventHandler(this.ResetFlag_Click);
            this.ResetMedalList.Click += new EventHandler(this.ResetMedalList_Click);
            this.ReSetAdsList.Click += new EventHandler(this.ReSetAdsList_Click);
            this.ReSetStatisticsSearchtime.Click += new EventHandler(this.ReSetStatisticsSearchtime_Click);
            this.ReSetStatisticsSearchcount.Click += new EventHandler(this.ReSetStatisticsSearchcount_Click);
            this.ReSetCommonAvatarList.Click += new EventHandler(this.ReSetCommonAvatarList_Click);
            this.ReSetJammer.Click += new EventHandler(this.ReSetJammer_Click);
            this.ReSetMagicList.Click += new EventHandler(this.ReSetMagicList_Click);
            this.ReSetScorePaySet.Click += new EventHandler(this.ReSetScorePaySet_Click);
            this.ReSetPostTableInfo.Click += new EventHandler(this.ReSetPostTableInfo_Click);
            this.ReSetTopiclistByFid.Click += new EventHandler(this.ReSetTopiclistByFid_Click);
            this.ReSetDigestTopicList.Click += new EventHandler(this.ReSetDigestTopicList_Click);
            this.ReSetHotTopicList.Click += new EventHandler(this.ReSetHotTopicList_Click);
            this.ReSetAggregation.Click += new EventHandler(this.ReSetAggregation_Click);
            this.ReSetTag.Click += new EventHandler(this.ReSetTag_Click);
        }
    }
}