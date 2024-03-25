using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux.Controls
{
    internal class ElTextBox : TextBox
    {
        protected override void InitializeComponent()
        {
            this.Children.Add(new Border
            {
                BorderStroke = "0",
                Margin = "0",
                Child = new ScrollViewer
                {
                    Width = "100%",
                    Height = "100%",
                    Name = "scrollViewer",
                    PresenterFor = this,
                    Content = TextBoxView,
                    Bindings = {
                        { nameof(ScrollViewer.HorizontalScrollBarVisibility), nameof(TextBox.HScrollBarVisibility), this },
                        { nameof(ScrollViewer.VerticalScrollBarVisibility), nameof(TextBox.VScrollBarVisibility), this }
                    }
                }
            });
            Children.Add(new TextBlock
            {
                Name = "placeholder",
                MarginLeft = 10,
                MarginTop = 5,
                IsHitTestVisible = false,
                Foreground = "192,196,204",
                Bindings =
                {
                    {nameof(TextBlock.Text),nameof(Placeholder),this },
                    {nameof(TextBlock.Visibility),nameof(Text),this,BindingMode.OneWay,(string pl)=>string.IsNullOrEmpty(pl)?Visibility.Visible:Visibility.Collapsed },
                }
            });
        }

        public string Placeholder
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
