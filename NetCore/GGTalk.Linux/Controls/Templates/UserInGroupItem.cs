using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using GGTalk.Linux;
using GGTalk.Linux.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux.Controls.Templates
{
    [CPF.Design.DesignerLoadStyle("res://GGTalk.Linux.Controls.Templates/Stylesheet1.css")]//用于设计的时候加载样式
    internal class UserInGroupItem : TreeViewItem
    {
        protected override void InitializeComponent()
        {//模板定义
            if (DesignMode)
            {
                Width = 300; ;
                Background = "#fff";
            }
            else
            {
                Width = "100%";
            }
            Height = 28;

            var menuMember = new ContextMenu
            {
                Items = new UIElement[]
                {
                    //new MenuItem
                    //{
                    //    Header = "禁言",
                    //    Items = new MenuItem[]
                    //    {
                    //        new MenuItem
                    //        {
                    //            IsCheckable = true,
                    //            Header = "1天"
                    //        },
                    //        new MenuItem
                    //        {
                    //            IsCheckable = true,
                    //            Header = "10天"
                    //        },
                    //        new MenuItem
                    //        {
                    //            IsCheckable = true,
                    //            Header = "永久"
                    //        }
                    //    }
                    //},
                    //new MenuItem
                    //{
                    //    Header = "解除禁言",
                    //},
                    new MenuItem
                    {
                        Header = "添加好友",
                        Commands={ { nameof(MouseDown),(s,e)=> { this.AddFriend_Click(s); } } }
                    },
                    //new MenuItem
                    //{
                    //    Header = "@TA",
                    //},
                }
            };

            ContextMenu = menuMember;

            Children.Add(new Picture
            {
                MarginLeft = 8f,
                Height = 20,
                Width = 20,
                Stretch = Stretch.Fill,
                Bindings =
                {
                    {nameof(Picture.Source),nameof(GGUserPlus.HeadImage) }
                }
            });

            Children.Add(new TextBlock
            {
                FontSize = 14,
                MarginLeft = 34,
                MarginTop = 4f,
                Bindings =
                {
                    {nameof(TextBlock.Text),nameof(GGUserPlus.DisplayName) }
                }
            });

            Triggers.Add(nameof(IsMouseOver), Relation.Me, null, (nameof(Background), "#aaaaaa55"));
            Triggers.Add(nameof(IsSelected), Relation.Me, null, (nameof(Background), "#aaaaaa55"));

        }

        #region 右键菜单
        private void AddFriend_Click(object sender)
        {
            GGUserPlus model = this.DataContext as GGUserPlus;
            if (model == null) { return; }
            CommonBusinessMethod.AddFriend((Window)this.Root, Program.ResourceCenter, model.ID);
        }

        #endregion
    }
}
