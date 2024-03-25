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
using TalkBase;

namespace GGTalk.Linux.Controls
{
    internal class UserItem : TreeViewItem
    {
        protected override void InitializeComponent()
        {
            //模板定义


            Width = "100%";
            Height = 55;

            Children.Add(new Picture
            {
                MarginLeft = 7.9f,
                Height = 40,
                Width = 40,
                Stretch = Stretch.Fill,
                Bindings =
                {
                    {nameof(Picture.Source),nameof(GGUserPlus.HeadImage) }
                }
            });


            Children.Add(new Picture
            {
                MarginTop = 33.5f,
                MarginLeft = 36.9f,
                Height = 14,
                Width = 14,
                Stretch = Stretch.Fill,
                Bindings =
                {
                    {nameof(Picture.Source),nameof(GGUserPlus.UserStatus),null,BindingMode.OneWay, a =>{
                        UserStatus userStatus= (UserStatus)a ;
                        if(userStatus==UserStatus.Hide&&Program.ResourceCenter.CurrentUserID!=((GGUserPlus)this.DataContext).ID)
                        {
                            userStatus=UserStatus.OffLine;
                        }
                        return CommonHelper.GetUserStatusIco(userStatus);
                    } }
                }
            });

            Children.Add(new TextBlock
            {
                FontSize = 14,
                MarginLeft = 63.5f,
                MarginTop = 7.3f, 
                TextTrimming = TextTrimming.CharacterEllipsis,
                Height = 16, 
                Bindings =
                {
                    {nameof(TextBlock.Text),nameof(GGUserPlus.DisplayName) },
                    {
                        nameof(Width),
                        nameof(ActualSize),
                        1,
                        BindingMode.OneWay,
                        (Size a)=>a.Width/1.4
                    }
                }
            });
            Children.Add(new TextBlock
            {
                Foreground = "#7E7E7E",
                MarginLeft = 62.1f,
                MarginTop = 30f, 
                TextTrimming = TextTrimming.CharacterEllipsis,
                Height = 16,
                ClipToBounds = true, 
                Bindings =
                {
                    {nameof(TextBlock.Text),nameof(GGUserPlus.Signature) },
                    {
                        nameof(Width),
                        nameof(ActualSize),
                        1,
                        BindingMode.OneWay,
                        (Size a)=>a.Width/1.4
                    }
                }
            });
            Commands.Add(nameof(MouseDown),(s, e) => { SingleSelect(); } );
            Triggers.Add(nameof(IsMouseOver), Relation.Me, null, (nameof(Background), "#aaaaaa55"));
            Triggers.Add(nameof(IsSelected), Relation.Me, null, (nameof(Background), "#aaaaaa55"));

        }
    }

}
