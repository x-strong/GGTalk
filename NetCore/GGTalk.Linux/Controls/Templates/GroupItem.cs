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
    internal class GroupItem : TreeViewItem
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
                Source = CommonOptions.ResourcesCatalog + "Group1.png"
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
                    {nameof(TextBlock.Text),nameof(GGGroup.DisplayName) },
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
                MarginRight = 40,
                Height = 16,
                Width = 200,
                MaxWidth = 200,
                TextTrimming = TextTrimming.CharacterEllipsis,
                ClipToBounds = true,
                Text = "TextBlock2231313112311",
                Bindings =
                {
                    {nameof(TextBlock.Text),nameof(GGGroup.MemberCount),null,BindingMode.OneWay, x=>x+"人"}
                }
            });
            Children.Add(new TextBlock
            {
                Foreground = "#7E7E7E",
                MarginLeft = 95f,
                MarginTop = 30f,
                MarginRight = 40,
                Width = 200,
                MaxWidth = 200,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Height = 16,
                ClipToBounds = true,
                Text = "TextBlock2231313112311",
                Bindings =
                {
                    {nameof(TextBlock.Text),nameof(GGGroup.CreateTime),null,BindingMode.OneWay,x=>((DateTime)x).ToShortDateString()  }
                }
            });
            Triggers.Add(nameof(IsMouseOver), Relation.Me, null, (nameof(Background), "#aaaaaa55"));
            Triggers.Add(nameof(IsSelected), Relation.Me, null, (nameof(Background), "#aaaaaa55"));

        }
    }

}
