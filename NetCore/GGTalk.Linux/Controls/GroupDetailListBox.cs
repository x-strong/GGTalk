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
    [CPF.Design.DesignerLoadStyle("res://GGTalk.Linux.Controls/Stylesheet1.css")]//用于设计的时候加载样式
    internal class GroupDetailListBox : ListBox
    {
        protected override void InitializeComponent()
        {

            WrapPanel panel = new WrapPanel();

            panel.Name = "itemsPanel";
            panel.PresenterFor = this;
            panel.Height = "100%";
            panel.Width = "100%";
            if (IsVirtualizing)
            {
                Children.Add(new WrapPanel { Width = "100%", Height = "100%", Children = { new VirtualizationPresenter<ListBoxItem> { Child = panel, PresenterFor = this } }, });
            }
            else
            {
                Children.Add(new WrapPanel { Width = "100%", Height = "100%", Children = { panel, } });
            }
        }
    }
}
