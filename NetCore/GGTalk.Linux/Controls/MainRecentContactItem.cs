using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using GGTalk.Linux.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux.Controls
{
    internal class MainRecentContactItem : TreeViewItem
    {
        protected override void InitializeComponent()
        {//模板定义
            if (DesignMode)
            {
                Width = 300; ;
                Background = "#fff";
            }
            else
            {
                Width = "100%";
            }
            Height = 55;

            Children.Add(new Picture
            {
                MarginLeft = 7.9f,
                Height = 40,
                Width = 40,
                Stretch = Stretch.Fill,
                Bindings =
                {
                    {nameof(Picture.Source),nameof(RecentContact.HeadImagePath) }
                }
            });

            Children.Add(new TextBlock
            {
                FontSize = 14,
                MarginLeft = 63.5f,
                MarginTop = 7.3f,
                Text = "TextBlock1",
                MarginRight = 40,
                Bindings =
                {
                    {nameof(TextBlock.Text),nameof(RecentContact.NickName) }
                }
            });

            Children.Add(new TextBlock
            {
                Foreground = "#7E7E7E",
                MarginLeft = 62.1f,
                MarginTop = 30f,
                MarginRight = 40,
                Height = 16,
                ClipToBounds = true,
                Text = "TextBlock2231313112311",
                Bindings =
                {
                    {nameof(TextBlock.Text),nameof(RecentContact.Sender) }
                }
            });
            Children.Add(new TextBlock
            {
                Foreground = "#7E7E7E",
                MarginLeft = 110,
                MarginTop = 30f,
                MarginRight = 40,
                Height = 16,
                ClipToBounds = true,
                Text = "TextBlock2231313112311",
                Bindings =
                {
                    {nameof(TextBlock.Text),nameof(RecentContact.Message) }
                }
            });
            Triggers.Add(nameof(IsMouseOver), Relation.Me, null, (nameof(Background), "#aaaaaa55"));
            Triggers.Add(nameof(IsSelected), Relation.Me, null, (nameof(Background), "#aaaaaa55"));

        }
    }
}
