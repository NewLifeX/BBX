using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using BBX.Entity;

namespace BBX.Plugin.VerifyImage.JpegImage
{
    public class VerifyImage : IVerifyImage
    {
        private static byte[] randb = new byte[4];
        private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
        private static Matrix m = new Matrix();

        private static Font[] fonts = new Font[]
        {
            new Font(new FontFamily("Times New Roman"), (float)(16 + Next(3)), FontStyle.Regular),
            new Font(new FontFamily("Georgia"), (float)(16 + Next(3)), FontStyle.Italic),
            new Font(new FontFamily("Arial"), (float)(16 + Next(3)), FontStyle.Regular),
            new Font(new FontFamily("Comic Sans MS"), (float)(16 + Next(3)), FontStyle.Italic)
        };

        private static int Next(int max)
        {
            rand.GetBytes(randb);
            int num = BitConverter.ToInt32(randb, 0);
            num %= max + 1;
            if (num < 0)
            {
                num = -num;
            }
            return num;
        }

        private static int Next(int min, int max)
        {
            return Next(max - min) + min;
        }

        public VerifyImageInfo GenerateImage(string code, int width, int height, Color bgcolor, int textcolor)
        {
            var verifyImageInfo = new VerifyImageInfo();
            verifyImageInfo.ImageFormat = ImageFormat.Jpeg;
            verifyImageInfo.ContentType = "image/pjpeg";
            width = 120;
            height = 40;
            var image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.Clear(bgcolor);
            int num = (textcolor == 2) ? 60 : 0;
            Pen pen = new Pen(Color.FromArgb(Next(50) + num, Next(50) + num, Next(50) + num), 1f);
            var solidBrush = new SolidBrush(Color.FromArgb(Next(100), Next(100), Next(100)));
            for (int i = 0; i < 3; i++)
            {
                g.DrawArc(pen, Next(20) - 10, Next(20) - 10, Next(width) + 10, Next(height) + 10, Next(-100, 100), Next(-200, 200));
            }
            var charbmp = new Bitmap(40, 40);
            var g2 = Graphics.FromImage(charbmp);
            var num2 = -18f;
            for (int j = 0; j < code.Length; j++)
            {
                m.Reset();
                m.RotateAt((float)(Next(50) - 25), new PointF((float)(Next(3) + 7), (float)(Next(3) + 7)));
                g2.Clear(Color.Transparent);
                g2.Transform = m;
                solidBrush.Color = Color.Black;
                num2 = num2 + 18f + (float)Next(5);
                PointF point = new PointF(num2, 2f);
                g2.DrawString(code[j].ToString(), fonts[Next(fonts.Length - 1)], solidBrush, new PointF(0f, 0f));
                g2.ResetTransform();
                g.DrawImage(charbmp, point);
            }
            solidBrush.Dispose();
            g.Dispose();
            g2.Dispose();
            verifyImageInfo.Image = image;
            return verifyImageInfo;
        }
    }
}