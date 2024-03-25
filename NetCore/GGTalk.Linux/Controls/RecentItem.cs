using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using GGTalk.Linux;
using GGTalk.Linux.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux.Controls
{
    internal class RecentItem : TreeViewItem
    {
        protected override void InitializeComponent()
        {
            Width = "100%";
            Height = 55; 
            Children.Add(new Picture
            {
                MarginLeft = 7.9f,
                Height = 40,
                Width = 40,
                Stretch = Stretch.Fill,
                Bindings =
                {
                    {
                        nameof(Picture.Source),
                        nameof(RecentContactModel.HeadImg)
                    }
                }
            });
            Children.Add(new Picture
            {
                MarginLeft = 37.9f,
                MarginBottom=8,
                Height = 10,
                Width = 10,
                Stretch = Stretch.Fill,
                Source=CommonOptions.ResourcesCatalog+ "newMsg.png",
                Bindings =
                {
                    {
                        nameof(Visibility),
                        nameof(RecentContactModel.IsNewMsg),
                        null,
                        BindingMode.OneWay,
                        b =>
                        {
                            return CommonHelper.GetVisibility((bool)b);
                        }
                    }
                }
            });
            Children.Add(new TextBlock
            {
                FontSize = 14,
                MarginLeft = 63.5f,
                MarginTop = 7.3f, 
                TextTrimming = TextTrimming.CharacterEllipsis,
                Height = 16, 
                Bindings =
                {
                    {
                        nameof(TextBlock.Text),
                        nameof(RecentContactModel.DisplayName)
                    },
                    {
                        nameof(Width),
                        nameof(ActualSize),
                        1,
                        BindingMode.OneWay,
                        (Size a)=>a.Width/1.4
                    }
                }
            });
            Children.Add(new TextBlock
            { 
                Foreground = "#7E7E7E",
                MarginLeft = 62.1f,
                MarginTop = 30f,
                MarginRight = 5,
                Height = 16,
                ClipToBounds = true,
                TextTrimming =TextTrimming.CharacterEllipsis,
                Text = "TextBlock",
                Bindings =
                {
                    {
                        nameof(TextBlock.Text),
                        nameof(RecentContactModel.LastWords)
                    }
                }
            });
            Commands.Add(nameof(MouseDown), (s, e) =>
            {
                SingleSelect();
            });
            Triggers.Add(nameof(IsMouseOver), Relation.Me, null, (nameof(Background), "#aaaaaa55"));
            Triggers.Add(nameof(IsSelected), Relation.Me, null, (nameof(Background), "#aaaaaa55"));
        }
    }
}
