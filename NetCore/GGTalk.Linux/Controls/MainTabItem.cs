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
    internal class MainTabItem : TabItem
    {
        public object PictureSource { get; set; }
        private Picture picture;

        protected override void InitializeComponent()
        {
            //模板定义
            //Width = 91.5;
            //Height = 30;

            Size = SizeField.Fill;  
            Children.Add(new Border
            {
                BorderFill = "#619fd7", 
                Background = "#619fd7",  
                Size = SizeField.Fill,
                BorderThickness = new Thickness(1, 1, 1, 1),
                BorderType = BorderType.BorderThickness,
                Child = new Picture
                {

                    Name=nameof(this.picture),
                    PresenterFor=this,
                    Width = 18,
                    Height = 18, 
                    MarginBottom = 3,
                    Stretch = Stretch.UniformToFill,
                    Source = PictureSource,
                } 
            });
            //Children.Add(new Border
            //{
            //    BorderFill = "#619fd7",
            //    Background = "#619fd7",
            //    MarginTop = -1,
            //    MarginLeft = 0,
            //    Height = 1,
            //    Width = "100%",
            //});

            this.Triggers.Add(new Trigger { Property = nameof(IsSelected), TargetRelation = Relation.Me.Children(a => a is Border), Setters = { { nameof(Border.BorderFill), "#adcdeb" }, { nameof(Border.Background), "#adcdeb" } } });
            //this.Triggers.Add(new Trigger { Property = nameof(IsSelected), TargetRelation = Relation.Me, Setters = { { nameof(Foreground), "#adcdeb" } } });
            this.picture = this.FindPresenterByName<Picture>(nameof(this.picture));
        }

        public void SetPictureSource(object source)
        {
            this.picture.Source = source;
        }
    }
}
