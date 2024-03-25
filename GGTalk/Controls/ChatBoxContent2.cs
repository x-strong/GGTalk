using System;
using System.Collections.Generic;
using System.Drawing;

namespace GGTalk.Controls
{
    public enum ChatBoxContextMenuMode
    {
        None = 0,
        ForInput,
        ForOutput
    }






    public class RichTextBag
    {
        public RichTextBag() { }
        public RichTextBag(string content, Font f, Color c)
        {
            this.TextContent = content;
            this.Font = f;
            this.Color = c;
        }

        public string TextContent;
        public Font Font;
        public Color Color;
    }

    public class DrawTextAttribute
    {
        public string Text { get; set; }
        public Font font { get; set; }
        public Brush brush { get; set; }

        public RectangleF rectangleF { get; set; }
    }

    public class FileInfoPara
    {
        public string ProjectID;
        public string FileName;
        public string FileSizeStr;
        public Bitmap FileTypeImage;
    }
}
