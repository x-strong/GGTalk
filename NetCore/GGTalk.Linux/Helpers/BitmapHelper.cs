using CPF.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace GGTalk.Linux.Helpers
{
    internal class BitmapHelper
    {
        public static Image Convert(byte[] bytes)
        {
            Bitmap img = null;
            if (bytes != null && bytes.Length > 0)
            {
                MemoryStream ms = new MemoryStream(bytes);
                img = new Bitmap(ms);
            }
            return img;
        }

        //public static Bitmap ConvertToGrey(Bitmap bitmap)
        //{
        //    double width, height;
        //    height = bitmap.Height;
        //    width = bitmap.Width;
        //    System.Drawing.Graphics
        //    Bitmap bmpGrayscale = Bitmap.createBitmap(width, height, Bitmap.Config.RGB_565);
        //    Canvas c = new Canvas(bmpGrayscale);
        //    Paint paint = new Paint();
        //    ColorMatrix cm = new ColorMatrix();
        //    //Set the matrix to affect the saturation of colors. 
        //    //A value of 0 maps the color to gray-scale.
        //    cm.setSaturation(0);
        //    ColorMatrixColorFilter f = new ColorMatrixColorFilter(cm);
        //    paint.setColorFilter(f);
        //    c.drawBitmap(bmpOriginal, 0, 0, paint);
        //    return bmpGrayscale;
        //}


        public static byte[] Bitmap2Byte(Image bitmap)
        {
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    //bitmap.Save(stream);
            //    byte[] data = new byte[stream.Length];
            //    stream.Seek(0, SeekOrigin.Begin);
            //    stream.Read(data, 0, (int)stream.Length);
            //    return data;
            //}

            using (Stream stream = bitmap.SaveToStream(CPF.Drawing.ImageFormat.Png))
            {
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, (int)stream.Length);
                return data;
            }
        }

        /// <summary>
        /// 指定最大长度，获取图片对尺寸 （超过最大长度，以最长边的长度 为最大长度值）
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static Size GetBitmapMaxSize(Image bitmap,int maxSize)
        {
            if (bitmap == null) { new Size(0, 0); }
            if (bitmap.Width <= maxSize && bitmap.Height <= maxSize)
            {
                return new Size(bitmap.Width, bitmap.Height); 
            }
            float width=0f, height = 0f;
            if (bitmap.Width > bitmap.Height)
            {
                width = maxSize;
                height = maxSize * bitmap.Height / bitmap.Width;
            }
            else {
                height = maxSize;
                width= maxSize * bitmap.Width / bitmap.Height;
            }
            return new Size(width, height);
        }


        public static byte[] ImageToByteArray(System.Drawing.Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // 将Image对象保存到MemoryStream中
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                // 将MemoryStream转换为byte[]数组
                return ms.ToArray();
            }
        }

        public static Stream ImageToStream(Image image)
        {
            return image.SaveToStream(CPF.Drawing.ImageFormat.Jpeg); 
        }
    }
}
