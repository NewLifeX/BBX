using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Web;
using System.Web.UI;
using BBX.Common;

namespace BBX.Web.UI
{
    public class ImageConverterPage : Page
    {
        public ImageConverterPage()
        {
            string url = DNTRequest.GetString("u").ToLower();
            HttpContext.Current.Response.ContentType = "image/jpeg";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddSeconds(5.0));
            try
            {
                Image computedImage = this.GetComputedImage(url);
                computedImage.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Jpeg);
            }
            catch
            {
            }
            HttpContext.Current.Response.End();
        }

        private Image GetComputedImage(string url)
        {
            return new Bitmap(Utils.GetMapPath(url));
        }

        private string DecodeFrom64(string toDecode)
        {
            byte[] bytes = Convert.FromBase64String(toDecode);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}