using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace BBX.Common
{
    public class Thumbnail
    {
        private static void SaveImage(Image image, string savePath, ImageCodecInfo ici)
        {
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
            image.Save(savePath, ici, encoderParameters);
            encoderParameters.Dispose();
        }

        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo[] array = imageEncoders;
            for (int i = 0; i < array.Length; i++)
            {
                ImageCodecInfo imageCodecInfo = array[i];
                if (imageCodecInfo.MimeType == mimeType)
                {
                    return imageCodecInfo;
                }
            }
            return null;
        }

        private static Size ResizeImage(int width, int height, int maxWidth, int maxHeight)
        {
            decimal num = maxWidth;
            decimal d = maxHeight;
            decimal d2 = num / d;
            decimal d3 = width;
            decimal num2 = height;
            int width2;
            int height2;
            if (d3 > num || num2 > d)
            {
                if (d3 / num2 > d2)
                {
                    decimal d4 = d3 / num;
                    width2 = Convert.ToInt32(d3 / d4);
                    height2 = Convert.ToInt32(num2 / d4);
                }
                else
                {
                    decimal d4 = num2 / d;
                    width2 = Convert.ToInt32(d3 / d4);
                    height2 = Convert.ToInt32(num2 / d4);
                }
            }
            else
            {
                width2 = width;
                height2 = height;
            }
            return new Size(width2, height2);
        }

        public static ImageFormat GetFormat(string name)
        {
            string text = name.Substring(name.LastIndexOf(".") + 1);
            string a;
            if ((a = text.ToLower()) != null)
            {
                if (a == "jpg" || a == "jpeg")
                {
                    return ImageFormat.Jpeg;
                }
                if (a == "bmp")
                {
                    return ImageFormat.Bmp;
                }
                if (a == "png")
                {
                    return ImageFormat.Png;
                }
                if (a == "gif")
                {
                    return ImageFormat.Gif;
                }
            }
            return ImageFormat.Jpeg;
        }

        public static void MakeSquareImage(Image image, string newFileName, int newSize)
        {
            int width = image.Width;
            int height = image.Height;
            Bitmap bitmap = new Bitmap(newSize, newSize);
            try
            {
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.InterpolationMode = InterpolationMode.High;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.Clear(Color.Transparent);
                if (width < height)
                {
                    graphics.DrawImage(image, new Rectangle(0, 0, newSize, newSize), new Rectangle(0, (height - width) / 2, width, width), GraphicsUnit.Pixel);
                }
                else
                {
                    graphics.DrawImage(image, new Rectangle(0, 0, newSize, newSize), new Rectangle((width - height) / 2, 0, height, height), GraphicsUnit.Pixel);
                }
                Thumbnail.SaveImage(bitmap, newFileName, Thumbnail.GetCodecInfo("image/" + Thumbnail.GetFormat(newFileName).ToString().ToLower()));
            }
            finally
            {
                image.Dispose();
                bitmap.Dispose();
            }
        }

        public static void MakeSquareImage(string fileName, string newFileName, int newSize)
        {
            Thumbnail.MakeSquareImage(Image.FromFile(fileName), newFileName, newSize);
        }

        public static void MakeRemoteSquareImage(string url, string newFileName, int newSize)
        {
            Stream remoteImage = Thumbnail.GetRemoteImage(url);
            if (remoteImage == null)
            {
                return;
            }
            Image image = Image.FromStream(remoteImage);
            remoteImage.Close();
            Thumbnail.MakeSquareImage(image, newFileName, newSize);
        }

        public static void MakeThumbnailImage(Image original, string newFileName, int maxWidth, int maxHeight)
        {
            Size newSize = Thumbnail.ResizeImage(original.Width, original.Height, maxWidth, maxHeight);
            using (Image image = new Bitmap(original, newSize))
            {
                try
                {
                    image.Save(newFileName, original.RawFormat);
                }
                finally
                {
                    original.Dispose();
                }
            }
        }

        public static void MakeThumbnailImage(string fileName, string newFileName, int maxWidth, int maxHeight)
        {
            Thumbnail.MakeThumbnailImage(Image.FromFile(fileName), newFileName, maxWidth, maxHeight);
        }

        public static bool MakeRemoteThumbnailImage(string url, string newFileName, int maxWidth, int maxHeight)
        {
            Stream remoteImage = Thumbnail.GetRemoteImage(url);
            if (remoteImage == null)
            {
                return false;
            }
            Image image = Image.FromStream(remoteImage);
            remoteImage.Close();
            if (image == null || image.Width == 0 || image.Height == 0)
            {
                return false;
            }
            Thumbnail.MakeThumbnailImage(image, newFileName, maxWidth, maxHeight);
            return true;
        }

        private static Stream GetRemoteImage(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentLength = 0L;
            httpWebRequest.Timeout = 20000;
            Stream result;
            try
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                result = httpWebResponse.GetResponseStream();
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public static string GetKey(int aid, int width, int height)
        {
            return DES.Encode(string.Format("{0},{1},{2}", aid, width, height), Utils.MD5(aid.ToString())).Replace("+", "[");
        }

        public static string GetKey(int aid)
        {
            return Thumbnail.GetKey(aid, 300, 300);
        }
    }
}