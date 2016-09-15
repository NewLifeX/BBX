using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Plugin.VerifyImage;

namespace BBX.Web.UI
{
    public class VerifyImagePage : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //}
            //protected override void OnInit(EventArgs e)
            //{
            //    base.OnInit(e);

            var config = GeneralConfigInfo.Current;
            var text = DNTRequest.GetQueryString("bgcolor").Trim();
            var queryInt = DNTRequest.GetQueryInt("textcolor", 1);
            var array = text.SplitAsInt(",");
            var bgcolor = Color.White;
            if (array.Length == 1 && text != string.Empty)
            {
                bgcolor = Utils.ToColor(text);
            }
            else
            {
                if (array.Length == 3)
                {
                    bgcolor = Color.FromArgb(Utils.StrToInt(array[0], 255), Utils.StrToInt(array[1], 255), Utils.StrToInt(array[2], 255));
                }
            }
            var online = Online.UpdateInfo();
            var verifyImageInfo = VerifyImageProvider.Current.GenerateImage(online.VerifyCode, 120, 60, bgcolor, queryInt);
            var image = verifyImageInfo.Image;
            HttpContext.Current.Response.ContentType = verifyImageInfo.ContentType;
            //image.Save(base.Response.OutputStream, verifyImageInfo.ImageFormat);
            var ms = new MemoryStream();
            image.Save(ms, verifyImageInfo.ImageFormat);
            ms.Position = 0;
            ms.CopyTo(Response.OutputStream);
        }
    }
}