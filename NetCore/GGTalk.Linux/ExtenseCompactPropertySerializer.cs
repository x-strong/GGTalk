using CPF.Drawing;
using ESBasic;
using ESPlus.Serialization;
using GGTalk.Linux.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GGTalk.Linux
{
    internal class ExtenseCompactPropertySerializer : CompactPropertySerializer
    {
        protected override bool CustomizeExtenseDeserialize(Type type, byte[] buff, ref int offset, out object obj)
        {
            obj = null;

            if (type == typeof(Image)|| type == typeof(Bitmap))
            {
                int len = ByteConverter.Parse<int>(buff, ref offset);
                Image bitmap = null;
                if (len > -1)
                {
                    bool isGif = false;
                    offset += 1;
                    byte[] imgBuff = new byte[len];
                    Buffer.BlockCopy(buff, offset, imgBuff, 0, len);
                    bitmap = BitmapHelper.Convert(imgBuff);
                    offset += len;
                }
                obj = bitmap;
                return true;
            }
            return false;
        }

        protected override bool CustomizeExtenseSerialize(MemoryStream stream, Type type, object obj)
        {

            if (type == typeof(Image) || type == typeof(Bitmap))
            {
                if (obj == null)
                {
                    byte[] buff = ByteConverter.ToBytes<int>(-1);
                    stream.Write(buff, 0, buff.Length);
                }
                else
                {
                    byte[] val2 = BitmapHelper.Bitmap2Byte((Image)obj);
                    byte[] len = ByteConverter.ToBytes<int>(val2.Length);
                    stream.Write(len, 0, len.Length);
                    stream.Write(BitConverter.GetBytes(false), 0, 1);
                    stream.Write(val2, 0, val2.Length);
                }
                return true;
            }
            return false;
        }

    }
}
