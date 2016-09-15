using System;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Common;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class UBB
    {
        private static string IMG_SIGN_SIGNATURE = "<img src=\"$1\" border=\"0\" />";
        private static string IMG_SIGN = "<img src=\"$1\" border=\"0\" onload=\"thumbImg(this)\"{0} />";
        private static string IMG_ERROR = " onerror=\"this.onerror=null;this.src='/images/common/imgerror2.png';\"";

        // 不能使用编译项，否则CPU一路高歌 http://www.newlifex.com/showtopic-979.aspx
        private static RegexOptions options = RegexOptions.IgnoreCase;// | RegexOptions.Compiled;
        public static Regex[] rs = new Regex[20];

        static UBB()
        {
            rs[0] = new Regex("\\s*\\[code\\]([\\s\\S]+?)\\[\\/code\\]\\s*", options);
            rs[1] = new Regex("(\\[upload=([^\\]]{1,4})(,.*?\\.[^\\]]{1,4})?\\])(.*?)(\\[\\/upload\\])", options);
            rs[2] = new Regex("viewfile.asp\\?id=(\\d{1,})", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            rs[3] = new Regex("(\\[uploadimage\\])(.*?)(\\[\\/uploadimage\\])", options);
            rs[4] = new Regex("viewfile.asp\\?id=(\\d{1,})", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            rs[5] = new Regex("(\\[uploadfile\\])(.*?)(\\[\\/uploadfile\\])", options);
            rs[6] = new Regex("viewfile.asp\\?id=(\\d{1,})", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            rs[7] = new Regex("(\\[upload\\])(.*?)(\\[\\/upload\\])", options);
            rs[8] = new Regex("viewfile.asp\\?id=(\\d{1,})", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            rs[9] = new Regex("(\\r\\n((&nbsp;)|\u3000| )+)(?<正文>\\S+)", options);
            rs[10] = new Regex("\\s*\\[hide\\][\\n\\r]*([\\s\\S]+?)[\\n\\r]*\\[\\/hide\\]\\s*", RegexOptions.IgnoreCase);
            rs[11] = new Regex("\\[table(?:=(\\d{1,4}%?)(?:,([\\(\\)%,#\\w ]+))?)?\\]\\s*([\\s\\S]+?)\\s*\\[\\/table\\]", options);
            rs[12] = new Regex("\\[media=(\\w{1,4}),(\\d{1,4}),(\\d{1,4})(,(\\d))?\\]\\s*([^\\[\\<\\r\\n]+?)\\s*\\[\\/media\\]", options);
            rs[13] = new Regex("\\[attach\\](\\d+)(\\[/attach\\])*", options);
            rs[14] = new Regex("\\[attachimg\\](\\d+)(\\[/attachimg\\])*", options);
            rs[15] = new Regex("\\s*\\[free\\][\\n\\r]*([\\s\\S]+?)[\\n\\r]*\\[\\/free\\]\\s*", RegexOptions.IgnoreCase);
            rs[16] = new Regex("\\s*\\[hide=(\\d+?)\\][\\n\\r]*([\\s\\S]+?)[\\n\\r]*\\[\\/hide\\]\\s*", RegexOptions.IgnoreCase);
            rs[17] = new Regex("\\[audio(=(\\d))?\\]\\s*([^\\[\\<\\r\\n]+?)\\s*\\[\\/audio\\]", options);
            rs[18] = new Regex("\\[p=(\\d{1,4}), (\\d{1,4}), (left|center|right)\\]\\s*([^\\r\\n]+?)\\s*\\[\\/p\\]", options);
            rs[19] = new Regex("\\[flash(=(\\d{1,4}),(\\d{1,4}))?\\]\\s*([^\\[\\<\\r\\n]+?)\\s*\\[\\/flash\\]", options);
        }

        public static string UBBToHTML(PostpramsInfo pi)
        {
            string text = pi.Sdetail;
            var stringBuilder = new StringBuilder();
            int num = -1;
            string text2 = pi.Pid.ToString();
            if (pi.BBCode)
            {
                Match match = rs[0].Match(text);
                while (match.Success)
                {
                    string newValue = Parsecode(match.Groups[1].ToString(), text2, ref num, pi.Allowhtml, ref stringBuilder);
                    text = text.Replace(match.Groups[0].ToString(), newValue);
                    match = match.NextMatch();
                }
            }
            if (pi.BBCode)
            {
                text = HideDetail(text, pi.Hide, pi.Usercredits);
            }
            text = Regex.Replace(text, "\\[smilie\\](.+?)\\[\\/smilie\\]", "$1", options);
            if (pi.Smileyoff == 0 && pi.Smiliesinfo != null)
            {
                text = ParseSmilies(text, pi.Smiliesinfo, pi.Smiliesmax);
            }
            text = Regex.Replace(text, "\\[smilie\\](.+?)\\[\\/smilie\\]", "<img src=\"$1\" />", options);
            if (pi.BBCode)
            {
                if (text.ToLower().Contains("[free]") || text.ToLower().Contains("[/free]"))
                {
                    Match match = rs[15].Match(text);
                    while (match.Success)
                    {
                        text = text.Replace(match.Groups[0].ToString(), "<br /><div class=\"msgheader\">免费内容:</div><div class=\"msgborder\">" + match.Groups[1].ToString() + "</div><br />");
                        match = match.NextMatch();
                    }
                }
                text = ParseBold(text);
                text = Regex.Replace(text, "\\[sup(?:\\s*)\\]", "<sup>", options);
                text = Regex.Replace(text, "\\[sub(?:\\s*)\\]", "<sub>", options);
                text = Regex.Replace(text, "\\[/sup(?:\\s*)\\]", "</sup>", options);
                text = Regex.Replace(text, "\\[/sub(?:\\s*)\\]", "</sub>", options);
                text = Regex.Replace(text, "((\\r\\n)*\\[p\\])(.*?)((\\r\\n)*\\[\\/p\\])", "<p>$3</p>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                text = ParseUrl(text);
                text = Regex.Replace(text, "\\[email(?:\\s*)\\](.*?)\\[\\/email\\]", "<a href=\"mailto:$1\" target=\"_blank\">$1</a>", options);
                text = Regex.Replace(text, "\\[email=(.[^\\[]*)(?:\\s*)\\](.*?)\\[\\/email(?:\\s*)\\]", "<a href=\"mailto:$1\" target=\"_blank\">$2</a>", options);
                text = ParseFont(text);
                text = Regex.Replace(text, "\\[indent(?:\\s*)\\]", "<blockquote>", options);
                text = Regex.Replace(text, "\\[/indent(?:\\s*)\\]", "</blockquote>", options);
                text = Regex.Replace(text, "\\[simpletag(?:\\s*)\\]", "<blockquote>", options);
                text = Regex.Replace(text, "\\[/simpletag(?:\\s*)\\]", "</blockquote>", options);
                text = Regex.Replace(text, "\\[list\\]", "<ul>", options);
                text = Regex.Replace(text, "\\[list=1\\]", "<ul type=1 class=\"litype_1\">", options);
                text = Regex.Replace(text, "\\[list=a\\]", "<ul type=1 class=\"litype_2\">", options);
                text = Regex.Replace(text, "\\[list=A\\]", "<ul type=1 class=\"litype_3\">", options);
                text = Regex.Replace(text, "\\[\\*\\]", "<li>", options);
                text = Regex.Replace(text, "\\[/list\\]", "</ul>", options);
                text = ParseTable(text);
                text = Regex.Replace(text, "(\\[SHADOW=)(\\d*?),(#*\\w*?),(\\d*?)\\]([\\s]||[\\s\\S]+?)(\\[\\/SHADOW\\])", "<table width='$2'  style='filter:SHADOW(COLOR=$3, STRENGTH=$4)'>$5</table>", options);
                text = Regex.Replace(text, "(\\[glow=)(\\d*?),(#*\\w*?),(\\d*?)\\]([\\s]||[\\s\\S]+?)(\\[\\/glow\\])", "<table width='$2'  style='filter:GLOW(COLOR=$3, STRENGTH=$4)'>$5</table>", options);
                text = Regex.Replace(text, "\\[center\\]([\\s]||[\\s\\S]+?)\\[\\/center\\]", "<center>$1</center>", options);
                MatchCollection matchCollection = rs[12].Matches(text);
                foreach (Match match2 in matchCollection)
                {
                    text = text.Replace(match2.Groups[0].Value, ParseMedia(match2.Groups[1].Value, Utils.StrToInt(match2.Groups[2].Value, 64), Utils.StrToInt(match2.Groups[3].Value, 48), match2.Groups[4].Value == "1", match2.Groups[6].Value));
                }
                matchCollection = rs[17].Matches(text);
                foreach (Match match3 in matchCollection)
                {
                    text = text.Replace(match3.Groups[0].Value, ParseAudio(match3.Groups[2].Value, match3.Groups[3].Value));
                }
                matchCollection = rs[18].Matches(text);
                foreach (Match match4 in matchCollection)
                {
                    text = text.Replace(match4.Groups[0].Value, ParseP(match4.Groups[1].Value, match4.Groups[2].Value, match4.Groups[3].Value, match4.Groups[4].Value));
                }
                text = text.Replace("[p=30, 2, left][/p]", "<p style=\"line-height: 30px; text-indent: 2em; text-align: left;\"></p>");
                matchCollection = rs[19].Matches(text);
                foreach (Match match5 in matchCollection)
                {
                    text = text.Replace(match5.Groups[0].Value, ParseFlash(match5.Groups[2].Value, match5.Groups[3].Value, match5.Groups[4].Value));
                }
                if (pi.Customeditorbuttoninfo != null)
                {
                    text = ReplaceCustomTag(text, pi.Customeditorbuttoninfo);
                }
                int num2 = text.ToLower().IndexOf("[quote]");
                int num3 = 0;
                while (num2 >= 0 && text.ToLower().IndexOf("[/quote]") >= 0 && num3 < 3)
                {
                    num3++;
                    text = Regex.Replace(text, "\\[quote\\]([\\s\\S]+?)\\[/quote\\]", "<table style=\"width: auto;\"><tr><td style=\"border:none;\"><div class=\"quote\"><blockquote>$1</blockquote></div></td></tr></table>", options);
                    num2 = text.ToLower().IndexOf("[quote]", num2 + 7);
                }
                text = Regex.Replace(text, "\\[area=([\\s\\S]+?)\\]([\\s\\S]+?)\\[/area\\]", "<div class=\"msgheader\">$1</div><div class=\"msgborder\">$2</div>", options);
                text = Regex.Replace(text, "\\[area\\]([\\s\\S]+?)\\[/area\\]", "<br /><br /><div class=\"msgheader\"></div><div class=\"msgborder\">$1</div>", options);
                if (pi.Bbcodemode == 1)
                {
                    string format = "<p><img alt=\"\" src=\"{0}\" border=\"0\" /><span class=\"bold\">附件</span>: <a href=\"{1}\" target=\"_blank\">{2}</a> </p>";
                    string arg = "images/attachicons/attachment.gif";
                    Match match = rs[1].Match(text);
                    while (match.Success)
                    {
                        Match match6 = rs[2].Match(match.Groups[4].ToString().ToLower());
                        if (match6.Success)
                        {
                            text = text.Replace(match.Groups[0].ToString(), "[attach]" + match6.Groups[1] + "[/attach]");
                        }
                        else
                        {
                            string text3 = match.Groups[4].ToString().ToLower().Replace("viewfile.asp?id", "attachment.aspx?attachmentid");
                            if (text3.IndexOf("attachment.aspx?attachmentid") == -1)
                            {
                                text3 = BaseConfigs.GetForumPath + "upload/" + text3;
                            }
                            else
                            {
                                text3 = BaseConfigs.GetForumPath + text3;
                            }
                            if ("rar,zip".IndexOf(match.Groups[2].ToString().ToLower()) != -1)
                            {
                                arg = "images/attachicons/rar.gif";
                            }
                            if ("jpg,jpeg,gif,bmp,png".IndexOf(match.Groups[2].ToString().ToLower()) != -1)
                            {
                                if (pi.Showimages == 1)
                                {
                                    text = text.Replace(match.Groups[0].ToString(), "<img src=\"" + text3 + "\" border=\"0\" onload=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.alt='点击在新窗口浏览图片\\nCTRL+Mouse 滚轮可放大/缩小';}\" onmouseover=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.style.cursor='hand'; this.alt='点击在新窗口浏览图片\\nCTRL+Mouse 滚轮可放大/缩小';}\" onclick=\"if(!this.resized) {return true;} else {window.open(this.src);}\" onmousewheel=\"return imgzoom(this);\" />");
                                }
                                else
                                {
                                    string text4 = text3;
                                    if (text4.LastIndexOf("/") > 0)
                                    {
                                        text4 = Utils.CutString(text4, text4.LastIndexOf("/"));
                                    }
                                    text4 = string.Format(format, arg, text3);
                                    text = text.Replace(match.Groups[0].ToString(), text4);
                                }
                            }
                            else
                            {
                                string text4 = text3;
                                if (text4.LastIndexOf("/") > 0)
                                {
                                    text4 = Utils.CutString(text4, text4.LastIndexOf("/"));
                                }
                                text4 = string.Format(format, arg, text3, text4);
                                text = text.Replace(match.Groups[0].ToString(), text4);
                            }
                        }
                        match = match.NextMatch();
                    }
                    text = Regex.Replace(text, "\\[uploadimage\\](\\d{1,})\\[/uploadimage\\]", "[attach]$1[/attach]", options);
                    match = rs[3].Match(text);
                    while (match.Success)
                    {
                        Match match7 = rs[4].Match(match.Groups[2].ToString().ToLower());
                        if (match7.Success)
                        {
                            text = text.Replace(match.Groups[0].ToString(), "[attach]" + match7.Groups[1] + "[/attach]");
                        }
                        else
                        {
                            string text5 = match.Groups[2].ToString().ToLower().Replace("viewfile.asp?id", "attachment.aspx?attachmentid");
                            if (text5.IndexOf("attachment.aspx?attachmentid") == -1)
                            {
                                text5 = BaseConfigs.GetForumPath + "upload/" + text5;
                            }
                            else
                            {
                                text5 = BaseConfigs.GetForumPath + text5;
                            }
                            if (pi.Showimages == 1)
                            {
                                text = text.Replace(match.Groups[0].ToString(), "<img src=\"" + text5 + "\" border=\"0\" onload=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.alt='点击在新窗口浏览图片\\nCTRL+Mouse 滚轮可放大/缩小';}\" onmouseover=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.style.cursor='hand'; this.alt='点击在新窗口浏览图片\\nCTRL+Mouse 滚轮可放大/缩小';}\" onclick=\"if(!this.resized) {return true;} else {window.open(this.src);}\" onmousewheel=\"return imgzoom(this);\" />");
                            }
                            else
                            {
                                string text4 = text5;
                                if (text4.LastIndexOf("/") > 0)
                                {
                                    text4 = Utils.CutString(text4, text4.LastIndexOf("/"));
                                }
                                text4 = string.Format(format, arg, text5, text4);
                                text = text.Replace(match.Groups[0].ToString(), text4);
                            }
                        }
                        match = match.NextMatch();
                    }
                    text = Regex.Replace(text, "\\[uploadfile\\](\\d{1,})\\[/uploadfile\\]", "[attach]$1[/attach]", options);
                    match = rs[5].Match(text);
                    while (match.Success)
                    {
                        Match match8 = rs[6].Match(match.Groups[2].ToString().ToLower());
                        if (match8.Success)
                        {
                            text = text.Replace(match.Groups[0].ToString(), "[attach]" + match8.Groups[1] + "[/attach]");
                        }
                        else
                        {
                            string text4 = match.Groups[2].ToString().ToLower().Replace("viewfile.asp?id", "attachment.aspx?attachmentid");
                            if (text4.IndexOf("attachment.aspx?attachmentid") == -1)
                            {
                                text4 = BaseConfigs.GetForumPath + "upload/" + text4;
                            }
                            else
                            {
                                text4 = BaseConfigs.GetForumPath + text4;
                            }
                            if (text4.LastIndexOf("/") > 0)
                            {
                                text4 = Utils.CutString(text4, text4.LastIndexOf("/"));
                            }
                            text4 = string.Format(format, arg, BaseConfigs.GetForumPath + match.Groups[2].ToString().ToLower().Replace("viewfile.asp?id", "attachment.aspx?attachmentid"), text4);
                            text = text.Replace(match.Groups[0].ToString(), text4);
                        }
                        match = match.NextMatch();
                    }
                    text = Regex.Replace(text, "\\[upload\\](\\d{1,})\\[/upload\\]", "[attach]$1[/attach]", options);
                    match = rs[7].Match(text);
                    while (match.Success)
                    {
                        Match match9 = rs[8].Match(match.Groups[2].ToString().ToLower());
                        if (match9.Success)
                        {
                            text = text.Replace(match.Groups[0].ToString(), "[attach]" + match9.Groups[1] + "[/attach]");
                        }
                        else
                        {
                            string text4 = BaseConfigs.GetForumPath + match.Groups[2].ToString().ToLower().Replace("viewfile.asp?id", "attachment.aspx?attachmentid");
                            if (text4.IndexOf("attachment.aspx?attachmentid") == -1)
                            {
                                text4 = BaseConfigs.GetForumPath + "upload/" + text4;
                            }
                            else
                            {
                                text4 = BaseConfigs.GetForumPath + text4;
                            }
                            if (text4.LastIndexOf("/") > 0)
                            {
                                text4 = Utils.CutString(text4, text4.LastIndexOf("/"));
                            }
                            text4 = string.Format(format, arg, BaseConfigs.GetForumPath + match.Groups[2].ToString().ToLower().Replace("viewfile.asp?id", "attachment.aspx?attachmentid"), text4);
                            text = text.Replace(match.Groups[0].ToString(), text4);
                        }
                        match = match.NextMatch();
                    }
                }
            }
            if (pi.Parseurloff == 0)
            {
                text = Regex.Replace(text, "^((tencent|ed2k|thunder|vagaa):\\/\\/[\\[\\]\\|A-Za-z0-9\\.\\/=\\?%\\-&_~`@':+!]+)", "<a target=\"_blank\" href=\"$1\">$1</a>", options);
                text = Regex.Replace(text, "((tencent|ed2k|thunder|vagaa):\\/\\/[\\[\\]\\|A-Za-z0-9\\.\\/=\\?%\\-&_~`@':+!]+)$", "<a target=\"_blank\" href=\"$1\">$1</a>", options);
                text = Regex.Replace(text, "[^>=\\]\"]((tencent|ed2k|thunder|vagaa):\\/\\/[\\[\\]\\|A-Za-z0-9\\.\\/=\\?%\\-&_~`@':+!]+)", "<a target=\"_blank\" href=\"$1\">$1</a>", options);
            }
            if (pi.Showimages == 1)
            {
                text = ParseImg(text, pi.Signature);
            }
            num = 0;
            text = Regex.Replace(text, "\\[r/\\]", "\r", options);
            text = Regex.Replace(text, "\\[n/\\]", "\n", options);
            text = text.Replace("\r\n", "<br/>");
            text = text.Replace("\r", "");
            text = text.Replace("\n\n", "<br/><br/>");
            text = text.Replace("\n", "<br/>");
            string[] array = Utils.SplitString(stringBuilder.ToString(), "<>");
            for (int i = 0; i < array.Length; i++)
            {
                string newValue2 = array[i];
                text = text.Replace("[\tBBX_CODE_" + text2 + "_" + num.ToString() + "\t]", newValue2);
                num++;
            }
            if (pi.Allowhtml == 0)
            {
                text = text.Replace("{rn}", "\r\n");
                text = text.Replace("{nn}", "\n\n");
                text = text.Replace("{r}", "\r");
                text = text.Replace("{n}", "\n");
            }
            text = text.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
            text = text.Replace("  ", "&nbsp;&nbsp;");
            text = Regex.Replace(text, "\\[hr\\]", "<hr/>", options);
            return text;
        }

        private static string ParseImg(string sDetail, int Signature)
        {
            if (String.IsNullOrEmpty(sDetail)) return sDetail;

            if (Signature == 1)
            {
                sDetail = Regex.Replace(sDetail, "\\[img\\]\\s*([^\\[\\<\\r\\n]+?)\\s*\\[\\/img\\]", IMG_SIGN_SIGNATURE, options);
            }
            else
            {
                sDetail = Regex.Replace(sDetail, "\\[img\\]\\s*([^\\[\\<\\r\\n]+?)\\s*\\[\\/img\\]", string.Format(IMG_SIGN, sDetail.Contains("<blockquote>") ? IMG_ERROR : ""), options);
            }
            sDetail = Regex.Replace(sDetail, "\\[img=(\\d{1,4})[x|\\,](\\d{1,4})\\]\\s*([^\\[\\<\\r\\n]+?)\\s*\\[\\/img\\]", "<img src=\"$3\" width=\"$1\" height=\"$2\" border=\"0\" onload=\"thumbImg(this)\" />", options).Replace("width=\"0\"", "").Replace("height=\"0\"", "");
            sDetail = Regex.Replace(sDetail, "\\[image\\]([\\s\\S]+?)\\[/image\\]", "<img src=\"$1\" border=\"0\" />", options);
            return sDetail;
        }

        private static string ParseBold(string sDetail)
        {
            if (String.IsNullOrEmpty(sDetail)) return sDetail;

            sDetail = Regex.Replace(sDetail, "\\[b(?:\\s*)\\]", "<b>", options);
            sDetail = Regex.Replace(sDetail, "\\[i(?:\\s*)\\]", "<i>", options);
            sDetail = Regex.Replace(sDetail, "\\[u(?:\\s*)\\]", "<u>", options);
            sDetail = Regex.Replace(sDetail, "\\[/b(?:\\s*)\\]", "</b>", options);
            sDetail = Regex.Replace(sDetail, "\\[/i(?:\\s*)\\]", "</i>", options);
            sDetail = Regex.Replace(sDetail, "\\[/u(?:\\s*)\\]", "</u>", options);
            return sDetail;
        }

        private static string ParseFont(string sDetail)
        {
            if (String.IsNullOrEmpty(sDetail)) return sDetail;

            sDetail = Regex.Replace(sDetail, "\\[color=([^\\[\\<]+?)\\]", "<font color=\"$1\">", options);
            sDetail = Regex.Replace(sDetail, "\\[size=(\\d+?)\\]", "<font size=\"$1\">", options);
            sDetail = Regex.Replace(sDetail, "\\[size=(\\d+(\\.\\d+)?(px|pt|in|cm|mm|pc|em|ex|%)+?)\\]", "<font style=\"font-size: $1\">", options);
            sDetail = Regex.Replace(sDetail, "\\[font=([^\\[\\<]+?)\\]", "<font face=\"$1\">", options);
            sDetail = Regex.Replace(sDetail, "\\[align=([^\\[\\<]+?)\\]", "<p align=\"$1\">", options);
            sDetail = Regex.Replace(sDetail, "\\[float=(left|right)\\]", "<br style=\"clear: both\"><span style=\"float: $1;\">", options);
            sDetail = Regex.Replace(sDetail, "\\[/color(?:\\s*)\\]", "</font>", options);
            sDetail = Regex.Replace(sDetail, "\\[/size(?:\\s*)\\]", "</font>", options);
            sDetail = Regex.Replace(sDetail, "\\[/font(?:\\s*)\\]", "</font>", options);
            sDetail = Regex.Replace(sDetail, "\\[/align(?:\\s*)\\]", "</p>", options);
            sDetail = Regex.Replace(sDetail, "\\[/float(?:\\s*)\\]", "</span>", options);
            return sDetail;
        }

        public static string ParseUrl(string sDetail)
        {
            if (String.IsNullOrEmpty(sDetail)) return sDetail;

            sDetail = Regex.Replace(sDetail, "\\[url(?:\\s*)\\](www\\.(.*?))\\[/url(?:\\s*)\\]", "<a href=\"http://$1\" target=\"_blank\">$1</a>", options);
            sDetail = Regex.Replace(sDetail, "\\[url(?:\\s*)\\]\\s*(([^\\[\"']+?))\\s*\\[\\/url(?:\\s*)\\]", "<a href=\"$1\" target=\"_blank\">$1</a>", options);
            sDetail = Regex.Replace(sDetail, "\\[url=www.([^\\[\"']+?)(?:\\s*)\\]([\\s\\S]+?)\\[/url(?:\\s*)\\]", "<a href=\"http://www.$1\" target=\"_blank\">$2</a>", options);
            sDetail = Regex.Replace(sDetail, "\\[url=(([^\\[\"']+?))(?:\\s*)\\]([\\s\\S]+?)\\[/url(?:\\s*)\\]", "<a href=\"$1\" target=\"_blank\">$3</a>", options);
            return sDetail;
        }

        public static string ParseSimpleUBB(string sDetail)
        {
            if (String.IsNullOrEmpty(sDetail)) return sDetail;

            sDetail = ParseImg(sDetail, 0);
            sDetail = ParseFont(sDetail);
            sDetail = ParseBold(sDetail);
            sDetail = ParseUrl(sDetail);
            return sDetail;
        }

        public static string HideDetail(string str, int hide, int usercredit)
        {
            if (String.IsNullOrEmpty(str)) return str;
            if (hide == 0) return str;

            int num = str.ToLower().IndexOf("[hide");
            while (num >= 0 && str.ToLower().IndexOf("[/hide]") >= 0)
            {
                Match match = rs[10].Match(str);
                while (match.Success)
                {
                    if (hide == 1)
                    {
                        str = str.Replace(match.Groups[0].ToString(), "<div class=\"hide\"><div class=\"hidestyle\">***** 该内容需会员回复才可浏览 *****</div></div>");
                    }
                    else
                    {
                        str = str.Replace(match.Groups[0].ToString(), "<div class=\"hide\"><div class=\"hidestyle\">以下内容会员跟帖回复才能看到</div><div class=\"hidetext\"><br />==============================<br /><br />" + match.Groups[1].ToString() + "<br /><br />==============================</div></div>");
                    }
                    match = match.NextMatch();
                }
                match = rs[16].Match(str);
                while (match.Success)
                {
                    int num2 = match.Groups[1].ToInt();
                    if (hide != -2 && usercredit < num2)
                    {
                        str = str.Replace(match.Groups[0].ToString(), "<div class=\"hide\"><div class=\"hidestyle\">***** 该内容需浏览者积分高于" + num2 + " 才可浏览 ***** </div></div>");
                    }
                    else
                    {
                        str = str.Replace(match.Groups[0].ToString(), "<div class=\"hide\"><div class=\"hidestyle\">以下内容只有在浏览者积分高于 " + num2 + " 时才显示</div><div class=\"hidetext\"><br />==============================<br /><br />" + match.Groups[2].ToString() + "<br /><br />==============================</div></div>");
                    }
                    match = match.NextMatch();
                }
                if (num + 7 > str.Length)
                {
                    num = str.ToLower().IndexOf("[table", str.Length);
                }
                else
                {
                    num = str.ToLower().IndexOf("[table", num + 7);
                }
            }
            return str;
        }

        private static string ParseSmilies(string sDetail, Smilie[] smiliesinfo, int smiliesmax)
        {
            if (String.IsNullOrEmpty(sDetail)) return sDetail;
            if (smiliesinfo == null) return sDetail;

            string format = "[smilie]{0}editor/images/smilies/{1}[/smilie]";
            for (int i = 0; i < Smilies.regexSmile.Length; i++)
            {
                if (smiliesmax > 0)
                {
                    sDetail = Smilies.regexSmile[i].Replace(sDetail, string.Format(format, BaseConfigs.GetForumPath, smiliesinfo[i].Url), smiliesmax);
                }
                else
                {
                    sDetail = Smilies.regexSmile[i].Replace(sDetail, string.Format(format, BaseConfigs.GetForumPath, smiliesinfo[i].Url));
                }
            }
            return sDetail;
        }

        private static string ReplaceCustomTag(string sDetail, CustomEditorButtonInfo[] customeditorbuttoninfo)
        {
            if (String.IsNullOrEmpty(sDetail)) return sDetail;
            if (customeditorbuttoninfo == null) return sDetail;

            for (int i = 0; i < Editors.regexCustomTag.Length; i++)
            {
                string replacement = customeditorbuttoninfo[i].Replacement;
                int @params = customeditorbuttoninfo[i].Params;
                for (int j = 0; j < customeditorbuttoninfo[i].Nest; j++)
                {
                    Match match = Editors.regexCustomTag[i].Match(sDetail);
                    while (match.Success)
                    {
                        string text = replacement.Replace("{1}", match.Groups[2].ToString());
                        if (@params > 1)
                        {
                            for (int k = 2; k <= @params; k++)
                            {
                                if (match.Groups.Count > k)
                                {
                                    text = text.Replace("{" + k + "}", match.Groups[k + 1].ToString());
                                }
                            }
                        }
                        sDetail = sDetail.Replace(match.Groups[0].ToString(), text);
                        sDetail = sDetail.Replace("{RANDOM}", Guid.NewGuid().ToString());
                        match = match.NextMatch();
                    }
                }
            }
            return sDetail;
        }

        private static string ParseTable(string str)
        {
            if (String.IsNullOrEmpty(str)) return str;

            int num = str.ToLower().IndexOf("[table");
            while (num >= 0 && str.ToLower().IndexOf("[/table]") >= 0)
            {
                Match match = rs[11].Match(str);
                while (match.Success)
                {
                    string text = match.Groups[1].ToString();
                    text = (Utils.CutString(text, text.Length - 1, text.Length).Equals("%") ? ((Utils.StrToInt(Utils.CutString(text, 0, text.Length - 1), 100) <= 98) ? text : "98%") : ((Utils.StrToInt(text, 560) <= 560) ? text : "560"));
                    string text2 = match.Groups[2].ToString();
                    string text3 = "<table class=\"t_table\" cellspacing=\"1\" cellpadding=\"4\" style=\"";
                    text3 += String.IsNullOrEmpty(text) ? "" : ("width:" + text + ";");
                    text3 += ("".Equals(text2) ? "" : ("background: " + text2 + ";"));
                    text3 += "\">";
                    text = match.Groups[3].ToString();
                    text = Regex.Replace(text, "\\[td=(\\d{1,2}),(\\d{1,2})(,(\\d{1,4}%?))?\\]", "<td colspan=\"$1\" rowspan=\"$2\" width=\"$4\" class=\"t_table\">", options);
                    text = Regex.Replace(text, "\\[td\\]", "<td>", options);
                    text = Regex.Replace(text, "\\[td=(\\d{1,4}%?)\\]", "<td width=\"$1\">", options);
                    text = Regex.Replace(text, "\\[tr\\]", "<tr>", options);
                    text = Regex.Replace(text, "\\[tr(?:=([\\(\\)\\s%,#\\w]+))\\]", "<tr style=\"background-color:$1\">", options);
                    text = Regex.Replace(text, "\\[td\\]", "<td>", options);
                    text = Regex.Replace(text, "\\[\\/td\\]", "</td>", options);
                    text = Regex.Replace(text, "\\[\\/tr\\](\\r\\n)?", "</tr>", options);
                    text = Regex.Replace(text, "\\<td\\>\\<\\/td\\>", "<td>&nbsp;</td>", options);
                    text3 += text;
                    text3 += "</table>";
                    str = str.Replace(match.Groups[0].ToString(), text3);
                    match = match.NextMatch();
                }
                num = str.ToLower().IndexOf("[table", num + 7);
            }
            return str;
        }
        private static string Parsecode(string text, string prefix, ref int pcodecount, int allowhtml, ref StringBuilder builder)
        {
            text = Regex.Replace(text, "^[\\n\\r]*([\\s\\S]+?)[\\n\\r]*$", "$1", options);
            if (builder.Length > 0) builder.Append("<>");

            //builder.Append("<div class=\"blockcode\"><div id=\"code" + prefix + "_" + pcodecount.ToString() + "\"><ol>");
            //string[] array = Utils.SplitString(text, "\r\n");
            //for (int i = 0; i < array.Length; i++)
            //{
            //    string str = array[i];
            //    if (allowhtml == 0)
            //    {
            //        builder.Append("<li>" + str + "<br/></li>{rn}");
            //    }
            //    else
            //    {
            //        builder.Append("<li>" + str + "<br/></li>\r\n");
            //    }
            //}
            //builder.Append("</ol></div><em onclick=\"copycode($('code" + prefix + "_" + pcodecount.ToString() + "'));\">复制代码</em></div>");

            builder.Append("<pre><code class=\"language\">" + text + "</code></pre>");

            pcodecount++;
            text = "[\tBBX_CODE_" + prefix + "_" + pcodecount.ToString() + "\t]";
            return text;
        }

        public static string ParseMedia(string type, int width, int height, bool autostart, string url)
        {
            string text = ParseFlv(url, width, height);
            if (text != string.Empty) return text;

            url = url.Replace("\\\\", "\\").Replace("<", string.Empty).Replace(">", string.Empty);
            switch (type)
            {
                case "mp3":
                case "wma":
                case "ra":
                case "ram":
                case "wav":
                case "mid":
                    return ParseAudio(autostart ? "1" : "0", url);
                case "rm":
                case "rmvb":
                case "rtsp":
                    {
                        Random random = new Random(3);
                        string text2 = "media_" + random.Next();
                        return string.Format("<object classid=\"clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA\" width=\"{0}\" height=\"{1}\"><param name=\"autostart\" value=\"{2}\" /><param name=\"src\" value=\"{3}\" /><param name=\"controls\" value=\"imagewindow\" /><param name=\"console\" value=\"{4}_\" /><embed src=\"{3}\" type=\"audio/x-pn-realaudio-plugin\" controls=\"IMAGEWINDOW\" console=\"{4}_\" width=\"{0}\" height=\"{1}\"></embed></object><br /><object classid=\"clsid:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA\" width=\"{0}\" height=\"32\"><param name=\"src\" value=\"{3}\" /><param name=\"controls\" value=\"controlpanel\" /><param name=\"console\" value=\"{4}_\" /><embed src=\"{3}\" type=\"audio/x-pn-realaudio-plugin\" controls=\"ControlPanel\" {5} console=\"{4}_\" width=\"{0}\" height=\"32\"></embed></object>", new object[]
                {
                    width,
                    height,
                    autostart ? 1 : 0,
                    url,
                    text2,
                    autostart ? "autostart=\"true\"" : string.Empty
                });
                    }
                case "flv":
                    return string.Format("<script type=\"text/javascript\" reload=\"1\">document.write(AC_FL_RunContent('width', '{0}', 'height', '{1}', 'allowNetworking', 'internal', 'allowScriptAccess', 'never', 'src', '{2}images/common/flvplayer.swf', 'flashvars', 'file={3}', 'quality', 'high', 'wmode', 'transparent', 'allowfullscreen', 'true'));</script>", new object[]
                {
                    width,
                    height,
                    BaseConfigs.GetForumPath,
                    Utils.UrlEncode(url)
                });
                case "swf":
                    return string.Format("<script type=\"text/javascript\" reload=\"1\">document.write(AC_FL_RunContent('width', '{0}', 'height', '{1}', 'allowNetworking', 'internal', 'allowScriptAccess', 'never', 'src', '{2}', 'quality', 'high', 'bgcolor', '#ffffff', 'wmode', 'transparent', 'allowfullscreen', 'true'));</script>", width, height, url);
                case "asf":
                case "asx":
                case "wmv":
                case "mms":
                case "avi":
                case "mpg":
                case "mpeg":
                    return string.Format("<object classid=\"clsid:6BF52A52-394A-11d3-B153-00C04F79FAA6\" width=\"{0}\" height=\"{1}\"><param name=\"invokeURLs\" value=\"0\"><param name=\"autostart\" value=\"{2}\" /><param name=\"url\" value=\"{3}\" /><embed src=\"{3}\" autostart=\"{2}\" type=\"application/x-mplayer2\" width=\"{0}\" height=\"{1}\"></embed></object>", new object[]
                {
                    width,
                    height,
                    autostart,
                    url
                });
                case "mov":
                    return string.Format("<object classid=\"clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B\" width=\"{0}\" height=\"{1}\"><param name=\"autostart\" value=\"{2}\" /><param name=\"src\" value=\"{3}\" /><embed src=\"{3}\" autostart=\"{2}\" type=\"video/quicktime\" controller=\"true\" width=\"{0}\" height=\"{1}\"></embed></object>", new object[]
                {
                    width,
                    height,
                    autostart ? "" : "fale",
                    url
                });
            }
            return string.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", url);
        }

        private static string ReplaceString(string[] r, string t, string s)
        {
            for (int i = 0; i < r.Length; i++)
            {
                string oldValue = r[i];
                s.Replace(oldValue, t);
            }
            return s;
        }

        private static string ParseFlv(string url, int width, int height)
        {
            string text = url.ToLower();
            string text2 = "";
            if (text != ReplaceString(new string[]
            {
                "player.youku.com/player.php/sid/",
                "tudou.com/v/",
                "player.ku6.com/refer/"
            }, "", text))
            {
                text2 = url;
            }
            else if (text.Contains("v.youku.com/v_show/"))
                text2 = GetFlvUrl(url, "http:\\/\\/v.youku.com\\/v_show\\/id_([^\\/]+)(.html|)", "http://player.youku.com/player.php/sid/{0}/v.swf");
            else if (text.Contains("tudou.com/programs/view/"))
                text2 = GetFlvUrl(url, "http:\\/\\/(www.)?tudou.com\\/programs\\/view\\/([^\\/]+)", "http://www.tudou.com/v/{0}", 2);
            else if (text.Contains("v.ku6.com/show/"))
                text2 = GetFlvUrl(url, "http:\\/\\/v.ku6.com\\/show\\/([^\\/]+).html", "http://player.ku6.com/refer/{0}/v.swf");
            else if (text.Contains("v.ku6.com/special/show_"))
                text2 = GetFlvUrl(url, "http:\\/\\/v.ku6.com\\/special\\/show_\\d+\\/([^\\/]+).html", "http://player.ku6.com/refer/{0}/v.swf");
            else if (text.Contains("www.youtube.com/watch?"))
                text2 = GetFlvUrl(url, "http:\\/\\/www.youtube.com\\/watch\\?v=([^\\/&]+)&?", "http://www.youtube.com/v/{0}&hl=zh_CN&fs=1");
            else if (text.Contains("tv.mofile.com/"))
                text2 = GetFlvUrl(url, "http:\\/\\/tv.mofile.com\\/([^\\/]+)", "http://tv.mofile.com/cn/xplayer.swf?v={0}");
            else if (text.Contains("v.mofile.com/show/"))
                text2 = GetFlvUrl(url, "http:\\/\\/v.mofile.com\\/show\\/([^\\/]+).shtml", "http://tv.mofile.com/cn/xplayer.swf?v={0}");
            else if (text.Contains("you.video.sina.com.cn/b/"))
                text2 = GetFlvUrl(url, "http:\\/\\/you.video.sina.com.cn\\/b\\/(\\d+)-(\\d+).html", "http://vhead.blog.sina.com.cn/player/outer_player.swf?vid={0}");
            else if (text.Contains("http://v.blog.sohu.com/u/"))
                text2 = GetFlvUrl(url, "http:\\/\\/v.blog.sohu.com\\/u\\/[^\\/]+\\/(\\d+)", "http://v.blog.sohu.com/fo/v4/{0}");
            else if (text.Contains("http://www.56.com"))
            {
                MatchCollection matchCollection;
                if ((matchCollection = Regex.Matches(url, "http:\\/\\/www.56.com\\/\\S+\\/play_album-aid-(\\d+)_vid-(.+?).html", RegexOptions.IgnoreCase)).Count > 0)
                {
                    text2 = string.Format("http://player.56.com/v_{0}.swf", matchCollection[0].Groups[2].Value);
                }
                else if ((matchCollection = Regex.Matches(url, "http:\\/\\/www.56.com\\/\\S+\\/([^\\/]+).html", RegexOptions.IgnoreCase)).Count > 0)
                {
                    text2 = string.Format("http://player.56.com/{0}.swf", matchCollection[0].Groups[1].Value);
                }
            }

            if (!string.IsNullOrEmpty(text2) && width != 0 && height != 0)
            {
                return string.Format("<script type=\"text/javascript\" reload=\"1\">document.write(AC_FL_RunContent('width', '{0}', 'height', '{1}', 'allowNetworking', 'internal', 'allowScriptAccess', 'never', 'src', '{2}', 'quality', 'high', 'bgcolor', '#ffffff', 'wmode', 'transparent', 'allowfullscreen', 'true'));</script>", width, height, text2);
            }
            return "";
        }

        private static string GetFlvUrl(string url, string reg, string flvFormat)
        {
            return GetFlvUrl(url, reg, flvFormat, 1);
        }

        private static string GetFlvUrl(string url, string reg, string flvFormat, int groupIndex)
        {
            string result = "";
            var ms = Regex.Matches(url, reg, RegexOptions.IgnoreCase);
            if (ms.Count > 0)
            {
                result = string.Format(flvFormat, ms[0].Groups[groupIndex].Value);
            }
            return result;
        }

        public static string ParseAudio(string autostart, string url)
        {
            return string.Format("<object width=\"400\" height=\"64\" classid=\"clsid:6BF52A52-394A-11d3-B153-00C04F79FAA6\"><param value=\"0\" name=\"invokeURLs\"><param value=\"{0}\" name=\"autostart\"><param value=\"{1}\" name=\"url\"><embed width=\"400\" height=\"64\" type=\"application/x-mplayer2\" autostart=\"{0}\" src=\"{1}\"></object>", (autostart != "") ? "1" : "0", url);
        }
        public static string ParseP(string lineHeight, string textIndent, string textAlign, string content)
        {
            return string.Format("<p style=\"line-height: {0}px; text-indent: {1}em; text-align: {2};\">{3}</p>", new object[]
            {
                lineHeight,
                textIndent,
                textAlign,
                content
            });
        }
        public static string ParseFlash(string flashWidth, string flashHeight, string flashUrl)
        {
            flashWidth = ((String.IsNullOrEmpty(flashWidth)) ? "550" : flashWidth);
            flashHeight = ((String.IsNullOrEmpty(flashHeight)) ? "400" : flashHeight);
            string text = "swf_" + Guid.NewGuid();
            if (Utils.GetFileExtName(flashUrl) != ".flv")
            {
                return string.Format("<span id=\"{0}\"></span><script type=\"text/javascript\" reload=\"1\">$('{0}').innerHTML=AC_FL_RunContent('width', '{1}', 'height', '{2}', 'allowNetworking', 'internal', 'allowScriptAccess', 'none', 'src', '{3}', 'quality', 'high', 'bgcolor', '#ffffff', 'wmode', 'transparent', 'allowfullscreen', 'true');</script>", new object[]
                {
                    text,
                    flashWidth,
                    flashHeight,
                    flashUrl
                });
            }
            return string.Format("<span id=\"{0}\"></span><script type=\"text/javascript\" reload=\"1\">$('{0}').innerHTML=AC_FL_RunContent('width', '{1}', 'height', '{2}', 'allowNetworking', 'internal', 'allowScriptAccess', 'none', 'src', '{3}images/common/flvplayer.swf', 'flashvars', 'file={4}', 'quality', 'high', 'wmode', 'transparent', 'allowfullscreen', 'true');</script>", new object[]
            {
                text,
                flashWidth,
                flashHeight,
                BaseConfigs.GetForumPath,
                Utils.UrlEncode(flashUrl)
            });
        }
        public static string ClearUBB(string sDetail)
        {
            return Regex.Replace(sDetail, "\\[[^\\]]*?\\]", string.Empty, RegexOptions.IgnoreCase);
        }
        public static string ClearBR(string sDetail)
        {
            return Regex.Replace(sDetail, "[\\r\\n]", string.Empty, RegexOptions.IgnoreCase);
        }
        public static string ClearAttachUBB(string sDetail)
        {
            return sDetail;
        }
    }
}
