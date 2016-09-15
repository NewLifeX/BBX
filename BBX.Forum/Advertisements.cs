using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Discuz.Cache;
using Discuz.Common;

using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class Advertisements
    {
        private static AdShowInfo[] GetAdsTable(string selectstr)
        {
            var cacheService = DNTCache.Current;
            DataTable dataTable = cacheService.RetrieveObject(CacheKeys.FORUM_ADVERTISEMENTS) as DataTable;
            if (dataTable == null)
            {
                dataTable = Advertisenments.GetAdsTable();
                cacheService.AddObject(CacheKeys.FORUM_ADVERTISEMENTS, dataTable);
            }
            DataRow[] array = dataTable.Select(selectstr);
            AdShowInfo[] array2 = new AdShowInfo[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array2[i] = new AdShowInfo();
                array2[i].Advid = Utils.StrToInt(array[i]["advid"].ToString(), 0);
                array2[i].Displayorder = Utils.StrToInt(array[i]["displayorder"].ToString(), 0);
                array2[i].Code = array[i]["code"].ToString().Trim();
                array2[i].Parameters = array[i]["parameters"].ToString().Trim();
            }
            return array2;
        }

        public static string GetSelectStr(string pagename, int fid, AdType adtype)
        {
            string text = Convert.ToInt16(adtype).ToString();
            string text2 = "";
            if (!Utils.StrIsNullOrEmpty(pagename) && pagename == "indexad")
            {
                text2 = ",首页,";
            }
            else
            {
                if (fid > 0)
                {
                    object obj = text2;
                    text2 = obj + "," + fid + ",";
                }
            }
            if (text2 == "")
            {
                if (Convert.ToInt16(adtype) > 10)
                {
                    text2 = "type='" + text + "'";
                }
                else
                {
                    text2 = "type='" + text + "'  AND [targets] Like '%全部%'";
                }
            }
            else
            {
                text2 = "type='" + text + "'  AND ([targets] Like '%" + text2 + "%' OR [targets] Like '%全部%')";
            }
            return text2;
        }

        public static AdShowInfo[] GetHeaderAdList(string pagename, int forumid)
        {
            return GetAdsTable(GetSelectStr(pagename, forumid, AdType.HeaderAd));
        }

        public static string GetOneHeaderAd(string pagename, int forumid)
        {
            string result = "";
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr(pagename, forumid, AdType.HeaderAd));
            if (adsTable.Length > 0)
            {
                int num = new Random().Next(0, adsTable.Length);
                result = adsTable[num].Code;
            }
            return result;
        }

        public static AdShowInfo[] GetFooterAdList(string pagename, int forumid)
        {
            return GetAdsTable(GetSelectStr(pagename, forumid, AdType.FooterAd));
        }

        private static string GetRandomAds(string selectstr)
        {
            string result = "";
            AdShowInfo[] adsTable = GetAdsTable(selectstr);
            if (adsTable.Length > 0)
            {
                int num = new Random().Next(0, adsTable.Length);
                result = adsTable[num].Code;
            }
            return result;
        }

        public static string GetOneFooterAd(string pagename, int forumid)
        {
            return Advertisements.GetRandomAds(GetSelectStr(pagename, forumid, AdType.FooterAd));
        }

        public static List<string> GetPageAd(string pagename, int forumid)
        {
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr(pagename, forumid, AdType.PageAd));
            if (adsTable.Length < 1)
            {
                return new List<string>();
            }
            List<string> list = new List<string>();
            AdShowInfo[] array = adsTable;
            for (int i = 0; i < array.Length; i++)
            {
                AdShowInfo adShowInfo = array[i];
                list.Add(adShowInfo.Code);
            }
            return list;
        }

        public static AdShowInfo[] GetPageWordAdList(string pagename, int forumid)
        {
            return GetAdsTable(GetSelectStr(pagename, forumid, AdType.PageWordAd));
        }

        public static string[] GetPageWordAd(string pagename, int forumid)
        {
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr(pagename, forumid, AdType.PageWordAd));
            if (adsTable.Length < 1)
            {
                return new string[0];
            }
            List<string> list = new List<string>();
            AdShowInfo[] array = adsTable;
            for (int i = 0; i < array.Length; i++)
            {
                AdShowInfo adShowInfo = array[i];
                list.Add(adShowInfo.Code);
            }
            return list.ToArray();
        }

        public static AdShowInfo[] GetInPostAdList(string pagename, int forumid)
        {
            return GetAdsTable(GetSelectStr(pagename, forumid, AdType.InPostAd));
        }

        public static int GetInPostAdCount(string pagename, int forumid)
        {
            return GetAdsTable(GetSelectStr(pagename, forumid, AdType.InPostAd)).Length;
        }

        public static string GetInPostAd(string pagename, int forumid, string templatepath, int count)
        {
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr(pagename, forumid, AdType.InPostAd));
            StringBuilder stringBuilder = new StringBuilder();
            if (adsTable.Length > 0)
            {
                stringBuilder.Append("<div style=\"display: none;\" id=\"ad_none\">\r\n");
                stringBuilder.Append(GetAdShowInfo(adsTable, count, 0));
                stringBuilder.Append(GetAdShowInfo(adsTable, count, 1));
                stringBuilder.Append(GetAdShowInfo(adsTable, count, 2));
                stringBuilder.Append("</div><script type='text/javascript' src='javascript/template_inforumad.js'></script>\r\n");
            }
            return stringBuilder.ToString();
        }

        public static string GetAdShowInfo(AdShowInfo[] adshowArray, int count, int inPostAdType)
        {
            string text = "";
            Random random = new Random();
            for (int i = 1; i <= count; i++)
            {
                List<AdShowInfo> list = new List<AdShowInfo>();
                for (int j = 0; j < adshowArray.Length; j++)
                {
                    AdShowInfo adShowInfo = adshowArray[j];
                    string[] array = Utils.SplitString(adShowInfo.Parameters.Trim(), "|", 9);
                    if (Utils.StrToInt(array[7], -1) == inPostAdType && (Utils.InArray(i.ToString(), array[8], ",") || array[8] == "0"))
                    {
                        list.Add(adShowInfo);
                    }
                }
                if (list.Count > 0)
                {
                    switch (inPostAdType)
                    {
                        case 0:
                            text += string.Format("<div class=\"ad_textlink1\" id=\"ad_thread1_{0}_none\">{1}</div>\r\n", i, list[random.Next(0, list.Count)].Code);
                            break;

                        case 1:
                            text += string.Format("<div class=\"ad_textlink2\" id=\"ad_thread2_{0}_none\">{1}</div>\r\n", i, list[random.Next(0, list.Count)].Code);
                            break;

                        default:
                            text += string.Format("<div class=\"ad_pip\" id=\"ad_thread3_{0}_none\">{1}</div>\r\n", i, list[random.Next(0, list.Count)].Code);
                            break;
                    }
                }
            }
            return text;
        }

        public static AdShowInfo[] GetFloatAdList(string pagename, int forumid)
        {
            return GetAdsTable(GetSelectStr(pagename, forumid, AdType.FloatAd));
        }

        public static string GetFloatAd(string pagename, int forumid)
        {
            string[] array = new string[0];
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr(pagename, forumid, AdType.FloatAd));
            string adsMsg = GetAdsMsg(adsTable, ref array);
            if (Utils.StrIsNullOrEmpty(adsMsg))
            {
                return "";
            }
            return "<script type='text/javascript'>theFloaters.addItem('floatAdv',10,'(document.body.clientHeight>document.documentElement.clientHeight ? document.documentElement.clientHeight :document.body.clientHeight)-" + ((array[3] == "") ? "0" : array[3]) + "-40','" + adsMsg + "');</script>";
        }

        private static string GetAdsMsg(AdShowInfo[] adshowArray, ref string[] parameter)
        {
            if (adshowArray.Length == 0)
            {
                return "";
            }
            int num = 0;
            if (adshowArray.Length > 1)
            {
                num = new Random().Next(0, adshowArray.Length);
            }
            if (Utils.StrIsNullOrEmpty(adshowArray[num].Parameters))
            {
                return "";
            }
            parameter = adshowArray[num].Parameters.Split('|');
            string result;
            if (parameter[0].ToLower() == "flash")
            {
                result = string.Format("<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0\" width=\"{0}\" height=\"{1}\"><param name=\"movie\" value=\"{2}\" /><param name=\"quality\" value=\"high\" /><param name=\"wmode\" value=\"opaque\">{3}<embed src=\"{2}\" wmode=\"opaque\" quality=\"high\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\" width=\"{0}\" height=\"{1}\"></embed></object>", new object[]
				{
					parameter[2],
					parameter[3],
					parameter[1],
					adshowArray[num].Code
				});
            }
            else
            {
                result = adshowArray[num].Code;
            }
            return result;
        }

        public static string GetDoubleAd(string pagename, int forumid)
        {
            string[] array = new string[0];
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr(pagename, forumid, AdType.DoubleAd));
            string adsMsg = GetAdsMsg(adsTable, ref array);
            if (Utils.StrIsNullOrEmpty(adsMsg)) return "";

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("<script type=\"text/javascript\">", new object[0]);
            stringBuilder.AppendFormat("\ntheFloaters.addItem('coupleBannerAdv1','(document.body.clientWidth>document.documentElement.clientWidth ? document.documentElement.clientWidth :document.body.clientWidth )-" + ((array[2] == "") ? "0" : array[2]) + "-90',10,'" + adsMsg + "<br /><img src=\"images/common/advclose.gif\" onMouseOver=\"this.style.cursor=\\'hand\\'\" onClick=\"closeBanner();\">');", new object[0]);
            stringBuilder.AppendFormat("\ntheFloaters.addItem('coupleBannerAdv2',10,10,'" + adsMsg + "<br /><img src=\"images/common/advclose.gif\" onMouseOver=\"this.style.cursor=\\'hand\\'\" onClick=\"closeBanner();\">');", new object[0]);
            stringBuilder.AppendFormat("\n</script>", new object[0]);
            return stringBuilder.ToString();
        }

        public static string GetMediaAd(string templatepath, string pagename, int forumid)
        {
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr(pagename, forumid, AdType.MediaAd));
            if (adsTable.Length <= 0)
            {
                return "";
            }
            return string.Format(adsTable[0].Code, templatepath, pagename, forumid);
        }

        public static string[] GetMediaAdParams(string pagename, int forumid)
        {
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr(pagename, forumid, AdType.MediaAd));
            if (adsTable.Length > 0)
            {
                return adsTable[0].Parameters.Split('|');
            }
            return new string[0];
        }

        public static string GetOnePostLeaderboardAD(string pagename, int forumid)
        {
            string result = "";
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr(pagename, forumid, AdType.PostLeaderboardAd));
            if (adsTable.Length > 0)
            {
                int num = new Random().Next(0, adsTable.Length);
                result = adsTable[num].Code;
            }
            return result;
        }

        public static string GetInForumAd(string pagename, int forumid, List<IndexPageForumInfo> topforum, string templatepath)
        {
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr(pagename, forumid, AdType.InForumAd));
            StringBuilder stringBuilder = new StringBuilder();
            if (adsTable.Length > 0)
            {
                new Random();
                stringBuilder.Append("<div style=\"display: none\" id=\"ad_none\">\r\n");
                int num = 0;
                while (num < topforum.Count && num < adsTable.Length)
                {
                    stringBuilder.AppendFormat("<div class=\"ad_column\" id=\"ad_intercat_{0}_none\">{1}</div>\r\n", topforum[num].Fid, adsTable[num].Code);
                    num++;
                }
                stringBuilder.Append("</div><script type='text/javascript' src='javascript/template_inforumad.js'></script>\r\n");
            }
            return stringBuilder.ToString();
        }

        public static string GetInPostAdXMLByFloor(string pagename, int forumid, string templatepath, int floor)
        {
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr(pagename, forumid, AdType.InPostAd));
            StringBuilder stringBuilder = new StringBuilder();
            if (adsTable.Length > 0)
            {
                stringBuilder.Append(GetAdShowInfoXMLByFloor(adsTable, floor, 0));
                stringBuilder.Append(GetAdShowInfoXMLByFloor(adsTable, floor, 1));
                stringBuilder.Append(GetAdShowInfoXMLByFloor(adsTable, floor, 2));
            }
            return stringBuilder.ToString();
        }

        public static string GetAdShowInfoXMLByFloor(AdShowInfo[] adshowArray, int floor, int inPostAdType)
        {
            string result = "";
            Random random = new Random();
            List<AdShowInfo> list = new List<AdShowInfo>();
            for (int i = 0; i < adshowArray.Length; i++)
            {
                AdShowInfo adShowInfo = adshowArray[i];
                string[] array = Utils.SplitString(adShowInfo.Parameters.ToString().Trim(), "|", 9);
                if (Utils.StrToInt(array[7], -1) == inPostAdType && (Utils.InArray(floor.ToString(), array[8], ",") || array[8] == "0"))
                {
                    list.Add(adShowInfo);
                }
            }
            if (list.Count > 0)
            {
                AdShowInfo adShowInfo2 = list[random.Next(0, list.Count)];
                result = string.Format("<ad_thread{0}><![CDATA[{1}]]></ad_thread{0}>", inPostAdType, adShowInfo2.Code);
            }
            return result;
        }

        public static string GetQuickEditorAD(string pagename, int forumid)
        {
            string result = "";
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr(pagename, forumid, AdType.QuickEditorAd));
            if (adsTable.Length > 0)
            {
                int num = new Random().Next(0, adsTable.Length);
                result = adsTable[num].Code;
            }
            return result;
        }

        public static string[] GetQuickEditorBgAd(string pagename, int forumid)
        {
            string[] array = new string[]
			{
				"",
				""
			};
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr(pagename, forumid, AdType.QuickEditorBgAd));
            if (adsTable.Length > 0)
            {
                int num = new Random().Next(0, adsTable.Length);
                array[1] = adsTable[num].Parameters.Split('|')[1];
                array[0] = adsTable[num].Parameters.Split('|')[4];
            }
            return array;
        }

        public static void CreateAd(int available, string type, int displayorder, string title, string targets, string parameters, string code, string startTime, string endTime)
        {
            targets = ((targets.IndexOf("全部") >= 0) ? ",全部," : ("," + targets + ","));
            Advertisenments.CreateAd(available, type, displayorder, title, targets, parameters, code, (startTime.IndexOf("1900") >= 0) ? "1900-1-1" : startTime, (endTime.IndexOf("1900") >= 0) ? "2555-1-1" : endTime);
            DNTCache.Current.RemoveObject(CacheKeys.FORUM_ADVERTISEMENTS);
        }

        public static DataTable GetAdvertisements(int type)
        {
            if (type < 0)
            {
                return Advertisenments.GetAdvertisements();
            }
            return Advertisenments.GetAdvertisements(type);
        }

        public static void DeleteAdvertisementList(string advIdList)
        {
            if (Utils.IsNumericList(advIdList))
            {
                Advertisenments.DeleteAdvertisementList(advIdList);
                DNTCache.Current.RemoveObject(CacheKeys.FORUM_ADVERTISEMENTS);
            }
        }

        public static int UpdateAdvertisementAvailable(string aidList, int available)
        {
            if (Utils.IsNumericList(aidList) && (available == 0 || available == 1))
            {
                int result = Advertisenments.UpdateAdvertisementAvailable(aidList, available);
                DNTCache.Current.RemoveObject(CacheKeys.FORUM_ADVERTISEMENTS);
                return result;
            }
            return 0;
        }

        public static void UpdateAdvertisement(int adId, int available, string type, int displayorder, string title, string targets, string parameters, string code, string startTime, string endTime)
        {
            if (adId > 0)
            {
                startTime = ((startTime.IndexOf("1900") >= 0) ? "1900-1-1" : startTime);
                endTime = ((endTime.IndexOf("1900") >= 0) ? "2555-1-1" : endTime);
                targets = ((targets.IndexOf("全部") >= 0) ? ",全部," : ("," + targets + ","));
                Advertisenments.UpdateAdvertisement(adId, available, type, displayorder, title, targets, parameters, code, startTime, endTime);
                DNTCache.Current.RemoveObject(CacheKeys.FORUM_ADVERTISEMENTS);
            }
        }

        public static DataTable GetAdvertisement(int aid)
        {
            if (aid <= 0)
            {
                return new DataTable();
            }
            return Advertisenments.GetAdvertisement(aid);
        }

        public static string GetWebSiteAd(AdType adType)
        {
            string result = "";
            AdShowInfo[] adsTable = GetAdsTable(GetSelectStr("", 0, adType));
            if (adsTable.Length > 0)
            {
                int num = new Random().Next(0, adsTable.Length);
                result = adsTable[num].Code;
            }
            return result;
        }
    }
}