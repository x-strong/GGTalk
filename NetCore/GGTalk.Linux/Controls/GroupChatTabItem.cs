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
    internal class GroupChatTabItem : TabItem
    {
        private Image source;
        public GroupChatTabItem(Image ico)
        {
            this.source = ico;
        }

        protected override void InitializeComponent()
        {
            //模板定义
            Width = 60;
            Children.Add(new Border
            {
                Background = "#619fd7",
                BorderFill = null,
                MarginLeft = -1,
                MarginBottom=-1,
                Width = 60,
                Child =
                new ContentControl
                {
                    MarginBottom = 5,
                    MarginLeft = 30,
                    MarginRight = 5,
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
            });
            Children.Add(new Picture
            {
                Source = this.source,
                Width = 18,
                Height = 18,
                MarginLeft = 8,
                Stretch = Stretch.Fill
            });
        }
    }
}
