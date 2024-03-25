using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using GGTalk.Linux.ViewModels;
using GGTalk.Linux.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux.Controls.Templates
{
    [CPF.Design.DesignerLoadStyle("res://GGTalk.Linux.Controls.Templates/Stylesheet1.css")]//用于设计的时候加载样式
    internal class UserSelectedListBoxItemTemplate : ListBoxItem
    {

        protected override void InitializeComponent()
        {//模板定义
            //Width = "100%";
            Width = "90%";
            Height = 35;
            Background = "#fff";
            Children.Add(
                new Picture
                {
                    IsAntiAlias = true,
                    Width = 25,
                    Height = 25,
                    MarginLeft = 5,
                    Stretch = Stretch.Fill,
                    Bindings = { { nameof(Picture.Source), nameof(UserSelectedModel.HeadImage) } }
                });
            Children.Add(new TextBlock { MarginLeft = 43, MarginTop = 10, Bindings = { { nameof(TextBlock.Text), nameof(UserSelectedModel.Name) } } });
            Children.Add(new Button
            {
                Name="btn_delete",
                PresenterFor=this.Parent,
                Width = 20,
                Height = 20,
                MarginRight = 6,
                CornerRadius = "10,10,10,10",
                Visibility = Visibility.Visible,
                Classes = "CancelButton",
                Content = "X",                
                Commands = { { nameof(Button.Click),nameof(UserSelectedWindow.UserSelectedListBoxItemTemplate_RemoveUserSelectedItemEvent),null,this.DataContext } } 
            });

            Triggers.Add(new Trigger { Property = nameof(IsMouseOver), PropertyConditions = a => (bool)a && !IsSelected, Setters = { { nameof(Background), "229,243,251" } } });


        }

        private void DeleteClick()
        {
            //UserSelectedViewModel model = this.Parent.DataContext as UserSelectedViewModel;
            //model.UserSelectedModels.Remove((UserSelectedModel)this.DataContext);
        }
    }
}
