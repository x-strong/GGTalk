using ESBasic;
using ESPlus.Serialization;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TalkBase
{


    public class ExtenseCompactPropertySerializer: CompactPropertySerializer
    {
        protected override bool CustomizeExtenseDeserialize(Type type, byte[] buff, ref int offset, out object obj)
        {
            obj = null;
            
            if (type == typeof(System.Drawing.Font))
            {
                SimpleFont simpleFont = null;
                int len = BitConverter.ToInt32(buff,offset);
                if (len > -1)
                {
                    simpleFont = CompactPropertySerializer.Default.Deserialize<SimpleFont>(buff, offset);
                    offset += len + 4;
                }
                else {
                    offset += 4;
                }
                obj = simpleFont?.GetFont();
                return true;
            }
            if (type == typeof(Image) || type.IsSubclassOf(typeof(Image)))
            {
                int len = ByteConverter.Parse<int>(buff, ref offset);
                Image img = null;
                if (len > -1)
                {
                    bool isGif = BitConverter.ToBoolean(buff, offset);
                    offset += 1;

                    if (isGif)
                    {
                        img = (Image)ESBasic.Helpers.SerializeHelper.DeserializeBytes(buff, offset, len);
                    }
                    else
                    {
                        byte[] imgBuff = new byte[len];
                        Buffer.BlockCopy(buff, offset, imgBuff, 0, len);
                        img = ESBasic.Helpers.ImageHelper.Convert(imgBuff);
                    }
                    offset += len;
                }
                obj = img;
                return true;
            }
            return false;
        }

        protected override bool CustomizeExtenseSerialize(MemoryStream stream, Type type, object obj)
        {
            if (type == typeof(System.Drawing.Font))
            {
                SimpleFont simpleFont = new SimpleFont((Font)obj);
                byte[] data = CompactPropertySerializer.Default.Serialize(simpleFont);
                stream.Write(data, 0, data.Length);
                return true;
            }
            if (type == typeof(Image) || type.IsSubclassOf(typeof(Image))) //0926
            {
                if (obj == null)
                {
                    byte[] buff = ByteConverter.ToBytes<int>(-1);//长度
                    stream.Write(buff, 0, buff.Length);
                }
                else
                {
                    byte[] val2 = null;
                    bool isGif = false;
                    Image img = (Image)obj;
                    Guid[] guids = img.FrameDimensionsList;
                    FrameDimension fd = new FrameDimension(guids[0]);
                    if (img.GetFrameCount(fd) > 1) //Gif
                    {
                        isGif = true;
                        val2 = ESBasic.Helpers.SerializeHelper.SerializeObject(img);
                    }
                    else
                    {
                        val2 = ESBasic.Helpers.ImageHelper.Convert(img);
                    }

                    byte[] len = ByteConverter.ToBytes<int>(val2.Length);
                    stream.Write(len, 0, len.Length);
                    stream.Write(BitConverter.GetBytes(isGif), 0, 1);
                    stream.Write(val2, 0, val2.Length);
                }
                return true;
            }
            return false;
        }
    }
}
