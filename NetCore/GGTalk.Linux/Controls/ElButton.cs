using System;
using System.Collections.Generic;
using System.Text;
using CPF;
using CPF.Drawing;
using CPF.Controls;
using CPF.Shapes;
using CPF.Styling;
using CPF.Animation;

namespace GGTalk.Linux.Controls
{
    internal class ElButton : Button
    {
        protected override void InitializeComponent()
        {//模板定义

            Children.Add(new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    new TextBlock
                    {
                        Classes="el-icon",
                        Bindings =
                        {
                            {nameof(Visibility),nameof(IconPostion),this,BindingMode.OneWay,(IconPostion ip)=>ip!= IconPostion.Left?Visibility.Collapsed:Visibility.Visible },
                            {nameof(Text),nameof(Text),this },
                        }
                    },
                    new Border
                    {
                        Name = "contentPresenter",
                        Height = "100%",
                        Width = "100%",
                        BorderFill = null,
                        PresenterFor = this
                    },
                    new TextBlock
                    {
                        Classes="el-icon",
                        Bindings =
                        {
                            {nameof(Visibility),nameof(IconPostion),this,BindingMode.OneWay,(IconPostion ip)=>ip!= IconPostion.Right?Visibility.Collapsed:Visibility.Visible },
                            {nameof(Text),nameof(Text),this },
                        }
                    },
                }
            });
        }

        public IconPostion IconPostion
        {
            get { return GetValue<IconPostion>(); }
            set { SetValue(value); }
        }
        /// <summary>
        /// 为了css那边设置Icon
        /// </summary>
        public string Text
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }

    public enum IconPostion
    {
        None,
        Left,
        Right
    }
}
