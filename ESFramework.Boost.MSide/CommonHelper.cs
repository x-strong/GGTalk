using System.Drawing;

namespace ESFramework.Boost
{
    public class CommonHelper
    {
        public static Image CropPicture(Image image, int maxLength = 300)
        {
            if (image.Width < maxLength && image.Height < maxLength)
            {
                return null;
            }
            bool widthMax = image.Width > image.Height;
            int cropWidth, cropHeight;
            if (widthMax)
            {
                cropWidth = maxLength;
                cropHeight = (int)((float)image.Height / image.Width * maxLength);
            }
            else
            {
                cropHeight = maxLength;
                cropWidth = (int)((float)image.Width / image.Height * maxLength);
            }
            Bitmap clone = new Bitmap(cropWidth, cropHeight);
            using (Graphics graphics = Graphics.FromImage(clone))
            {
                RectangleF srcRect = new RectangleF(0, 0, image.Width, image.Height);
                RectangleF dstRect = new RectangleF(0, 0, cropWidth, cropHeight);
                graphics.DrawImage(image, dstRect, srcRect, GraphicsUnit.Pixel);
            }
            return clone;
        }
    }
}
