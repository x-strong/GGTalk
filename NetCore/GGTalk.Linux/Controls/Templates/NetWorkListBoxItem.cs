using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using GGTalk.Linux.Helpers;
using GGTalk.Linux.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux.Controls
{
    [CPF.Design.DesignerLoadStyle("res://GGTalk.Linux.Controls.Templates/Stylesheet1.css")]//用于设计的时候加载样式
    internal class NetWorkListBoxItem : ListBoxItem
    {
        protected override void InitializeComponent()
        {//模板定义
            //Width = "100%";
            Width = 80;
            Height = 75;
            Background = "#fff";
            Children.Add(
                new Picture
                {
                    MarginTop = 0,
                    Width = 34,
                    Height = 34,
                    Stretch = Stretch.Fill,
                    ToolTip = "",                    
                    Cursor = Cursors.Hand,
                    IsAntiAlias = true,
                    Bindings = { { nameof(Picture.Source),nameof(FileOrDirectoryTag.FileIco)},  { nameof(ToolTip), nameof(FileOrDirectoryTag.ToolTip)  } }
                });
            Children.Add(new TextBlock { MaxWidth=60, MaxHeight= 40,MarginTop=40, TextTrimming=TextTrimming.CharacterEllipsis, Bindings = { { nameof(TextBlock.Text), nameof(FileOrDirectoryTag.Name) } } });

            Triggers.Add(

                new Trigger { Property = nameof(IsMouseOver), PropertyConditions = a => (bool)a && !IsSelected, Setters = { { nameof(Background), "#eee" } } }

                );
            Triggers.Add(new Trigger { Property = nameof(IsSelected), Setters = { { nameof(Background), "#f0f0f0" } } });
            Commands.Add(nameof(DoubleClick), nameof(FolderBrowserWindow.FileBoxItem_DoubleClicked), null, (FileOrDirectoryTag)this.DataContext);
        }
    }
}
