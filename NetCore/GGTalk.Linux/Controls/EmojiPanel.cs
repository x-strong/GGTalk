using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using GGTalk;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux.Controls
{
    [CPF.Design.DesignerLoadStyle("res://GGTalk.Linux.Controls/Stylesheet1.css")]//用于设计的时候加载样式
    internal class EmojiPanel : Control
    {

        public event Action<int, Image> EmotionClicked;
        public WrapPanel wrapPanel { set; get; }

        protected override void InitializeComponent()
        {//模板定义
            Children.Add(new ScrollViewer
            {
                Focusable = true,
                PresenterFor = this,
                Name = "ScrollViewerEmoji",
                Width = 346,
                Height = 160,
                Content = new WrapPanel
                {
                    BorderThickness = new Thickness(2, 2, 2, 2),
                    BorderType = BorderType.BorderThickness,
                    BorderFill = "#eee",
                    PresenterFor = this,
                    Name = nameof(this.wrapPanel),
                    Width = 326,
                    Height = 340,
                    Background = "#fff"
                }
            });
            this.wrapPanel = FindPresenterByName<WrapPanel>(nameof(this.wrapPanel));


            //加载表情
            for (int  i=0; i< GlobalResourceManager.EmotionList.Count;i++)
            {
                Thickness tk = new Thickness(3, 0, 0, 0);
                Picture p = new Picture();
                p.Source = GlobalResourceManager.EmotionList[i];
                p.Width = 25;
                p.Height = 25;
                p.Stretch = Stretch.Fill;
                p.MarginTop = 5;
                p.MarginLeft = 10;
                p.Tag = i;
                p.MouseDown += this.P_MouseDown;
                this.wrapPanel.Children.Add(p);
            }
        }

        //点击事件
        public void P_MouseDown(object sender, CPF.Input.MouseButtonEventArgs e)
        {
            Picture pic = (Picture)sender;
            Image bm = (Image)pic.Source;

            if (this.EmotionClicked != null)
            {
                this.EmotionClicked(Convert.ToInt32(pic.Tag), bm);
            }
        }
    }
}
