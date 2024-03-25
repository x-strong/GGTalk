using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CPF.Svg;
using CPF.Input;

namespace GGTalk.Linux.Controls
{
    [CPF.Design.DesignerLoadStyle("res://GGTalk.Linux/Stylesheet1.css")]//用于设计的时候加载样式
    internal class Dialog : Control
    {
        public Dialog(Window window, string content)
        {
            this.content = content;
            this.window = window;
            window.Children.Add(mask);
            window.Children.Add(this);
            mask.TransitionValue(a => a.Background, "0,0,0,100", TimeSpan.FromSeconds(0.3), null, AnimateMode.Linear);
            this.TransitionValue(a => a.MarginTop, 100, TimeSpan.FromSeconds(0.3), new PowerEase { }, AnimateMode.EaseOut);
            mask.MouseDown += Mask_MouseDown;
        }

        private void Mask_MouseDown(object sender, MouseButtonEventArgs e)
        {
            window.DragMove();
        }

        string content;
        Window window;

        protected override void InitializeComponent()
        {
            IsAntiAlias = true;
            CornerRadius = "3,3,3,3";
            MarginTop = 0;
            Height = 164.8f;
            Width = 275.2f;
            Background = "#fff";
            ZIndex = 100;
            Children.Add(new TextBlock
            {
                MarginRight = 9f,
                MarginTop = 8.1f,
                Text = "x",
                FontSize = 18,
                Classes = "el-icon,el-icon-close",
                Commands =
                {
                    {
                        nameof(MouseDown),
                        (s,e)=>Close()
                    }
                },
                Triggers =
                {
                    {
                        nameof(IsMouseOver),
                        Relation.Me,
                        null,
                        (nameof(Foreground),"64,158,255")
                    }
                }
            });
            Children.Add(new ElButton
            {
                MarginRight = 86.3f,
                MarginBottom = 10.1f,
                Content = "取消",
                Width = 54.3f,
                Height = 25.9f,
                Commands =
                {
                    {
                        nameof(Button.Click),
                        (s,e)=>Close()
                    }
                }
            });
            Children.Add(new ElButton
            {
                MarginBottom = 10.1f,
                MarginRight = 21.1f,
                Classes = "primary",
                Content = "确认",
                Width = 53.5f,
                Height = 25.9f,
                Commands =
                {
                    {
                        nameof(Button.Click),
                        (s,e)=>Close()
                    }
                }
            });
            Children.Add(new TextBlock
            {
                FontSize = 14f,
                Text = content,
            });
            Children.Add(new TextBlock
            {
                FontSize = 16f,
                MarginLeft = 12.6f,
                MarginTop = 9.6f,
                Text = "提示",
            });
        }

        public void Close()
        {
            //采用过渡属性的写法定义淡出效果
            mask.TransitionValue(a => a.Background, "0,0,0,0", TimeSpan.FromSeconds(0.3), null, AnimateMode.Linear, () =>
            {
                window.Children.Remove(mask);
            });

            this.TransitionValue(a => a.MarginTop, -100, TimeSpan.FromSeconds(0.3), new PowerEase { }, AnimateMode.EaseIn, () =>
            {
                window.Children.Remove(this);
            });
        }

        Control mask = new Control { Size = SizeField.Fill, ZIndex = 100, Background = "#00000000" };
    }
}
