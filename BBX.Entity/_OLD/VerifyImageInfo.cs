using System.Drawing;
using System.Drawing.Imaging;

namespace BBX.Entity
{
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