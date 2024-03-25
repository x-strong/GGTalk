using CPF.Drawing;

namespace GGTalk.Linux.Controls.ChatRender4Dll
{
    internal class ImgModel
    {

        internal ImgModel(string guid,Image sourceImg, Image thumbnailImg)
        {
            this.guid = guid;
            this.sourceImg = sourceImg;
            this.thumbnailImg = thumbnailImg;
        }

        internal Image GetShowImage()
        {
            Image image = this.thumbnailImg != null ? this.thumbnailImg : this.sourceImg;
            return image;
        }

        internal string guid;

        internal Image sourceImg;

        internal Image thumbnailImg;   
    }
}
