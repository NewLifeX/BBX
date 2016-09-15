using System.Drawing;
using System.Drawing.Imaging;
using BBX.Entity;

namespace BBX.Plugin.VerifyImage
{
    public interface IVerifyImage
    {
        VerifyImageInfo GenerateImage(string code, int width, int height, Color bgcolor, int textcolor);
    }

    public class VerifyImageInfo
    {

        private Bitmap image;
        public Bitmap Image { get { return image; } set { image = value; } }

        private string contentType = "image/jpeg";
        public string ContentType { get { return contentType; } set { contentType = value; } }

        private ImageFormat imageFormat = ImageFormat.Jpeg;
        public ImageFormat ImageFormat { get { return imageFormat; } set { imageFormat = value; } }
    }
}