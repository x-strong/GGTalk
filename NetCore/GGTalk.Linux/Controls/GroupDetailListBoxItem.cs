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
    [CPF.Design.DesignerLoadStyle("res://GGTalk.Linux.Controls/Stylesheet1.css")]//用于设计的时候加载样式
    internal class GroupDetailListBoxItem : ListBoxItem
    {
        protected override void InitializeComponent()
        {//模板定义
            //Width = "100%";
            Width = 68;
            Height = 60;
            Background = "#fff";
            Children.Add(
                new Picture
                {
                    MarginTop = 5,
                    Width = 34,
                    Height = 34,
                    Stretch = Stretch.Fill,
                    Cursor = Cursors.Hand,
                    IsAntiAlias = true,
                    Bindings = { { nameof(Picture.Source), nameof(GGUserPlus.HeadImage) },{ nameof(Picture.ToolTip),nameof(GGUserPlus.ID)} }
                });
            Children.Add(new TextBlock { MarginBottom = 0,MaxWidth=60,MaxHeight=20, TextTrimming = TextTrimming.CharacterEllipsis, Bindings = { { nameof(TextBlock.Text), nameof(GGUserPlus.DisplayName) } } });
            Triggers.Add(

                new Trigger { Property = nameof(IsMouseOver), PropertyConditions = a => (bool)a && !IsSelected, Setters = { { nameof(Background), "#eee" } } }

                );


        }
    }
}
