using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux
{
    [CPF.Design.DesignerLoadStyle("res://GGTalk.Linux/Stylesheet1.css")]//用于设计的时候加载样式
    internal class NoticeTabItem : TabItem
    {
        protected override void InitializeComponent()
        {
            //模板定义
            Width = 110;
            Height = 30;
            Children.Add(new Border
            {
                Background = "#619fd7",
                BorderFill = null,
                MarginLeft = -1,
                MarginBottom = -1,
                Width = 111,
                Height = 30,
                Child =
                new ContentControl
                {
                    MarginBottom = 5,
                    MarginTop = 5,
                    Bindings =
                    {
                        {
                            nameof(Content),
                            nameof(Header),
                            this
                        },
                        {
                            nameof(ContentTemplate),
                            nameof(HeaderTemplate),
                            this
                        }
                    }
                },
                Triggers ={
                            new Trigger { Property = nameof(IsMouseOver), Setters = { { nameof(Background), "232,242,252" } } },
                }
            });


        }
    }
}
