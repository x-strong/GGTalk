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

    internal class NoticeTabControl : TabControl
    {
        protected override void InitializeComponent()
        {
            Children.Add(new Grid
            {
                Width = "100%",
                Height = "100%",
                ColumnDefinitions = { new ColumnDefinition { Width="*",
                    Bindings={ {nameof(ColumnDefinition.Width),nameof(TabStripPlacement),this,BindingMode.OneWay,a=> (Dock)a== Dock.Left?GridLength.Auto:GridLength.Star} } },
                    new ColumnDefinition { Width = 0,
                    Bindings={ {nameof(ColumnDefinition.Width),nameof(TabStripPlacement),this,BindingMode.OneWay,
                                a=>{
                                    switch ((Dock)a)
                                    {
                                        case Dock.Left:
                                        return GridLength.Star;
                                        case Dock.Right:
                                        return GridLength.Auto;
                                        default:
                                        return (GridLength)0;
                                    }
                                } } }
                    } },
                RowDefinitions = { new RowDefinition { Height="auto",
                    Bindings={ {nameof(RowDefinition.Height),nameof(TabStripPlacement),this,BindingMode.OneWay,a=> (Dock)a == Dock.Top ? GridLength.Auto : GridLength.Star } } },
                    new RowDefinition {
                    Bindings={ {nameof(RowDefinition.Height),nameof(TabStripPlacement),this,BindingMode.OneWay,
                                a=>{
                                    switch ((Dock)a)
                                    {
                                        case Dock.Top:
                                        return GridLength.Star;
                                        case Dock.Bottom:
                                        return GridLength.Auto;
                                        default:
                                        return (GridLength)0;
                                    }
                                } } }
                    } },
                Children =
                {
                    new Border {
                        Attacheds={
                            { Grid.ColumnIndex, 0
                                ,nameof(TabStripPlacement),this,BindingMode.OneWay,
                                a=> {
                                    var dock=(Dock)a;
                                    return (dock!= Dock.Right)?0:1;
                                }
                            },
                            { Grid.RowIndex,0
                                ,nameof(TabStripPlacement),this,BindingMode.OneWay,
                                a=> {
                                    var dock=(Dock)a;
                                    return (dock!= Dock.Bottom)?0:1;
                                }
                            } },
                        Name="headBorder",
                        BorderFill="#619fd7",
                        Background="#619fd7",
                        BorderThickness=new Thickness(1),
                        BorderType= BorderType.BorderThickness,
                        Width="100%",
                        MarginLeft=0,
                        Padding=new Thickness(0,0,0,0),
                        Bindings={ {nameof(WrapPanel.Width),nameof(TabStripPlacement),this,BindingMode.OneWay,a=>((byte)(Dock)a)%2==0?(FloatField)"auto": (FloatField)"100%" },
                            {nameof(WrapPanel.Height),nameof(TabStripPlacement),this,BindingMode.OneWay,a=>((byte)(Dock)a)%2==0?(FloatField)"100%": (FloatField)"auto" } },
                        Child=
                        new WrapPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Name="headerPanel",
                            PresenterFor=this,
                            MarginLeft=0,
                            Width="100%",
                            Bindings={
                                {nameof(WrapPanel.Orientation),nameof(TabStripPlacement),this,BindingMode.OneWay,a=>((byte)(Dock)a)%2==0?Orientation.Vertical:Orientation.Horizontal },
                                {nameof(WrapPanel.Width),nameof(TabStripPlacement),this,BindingMode.OneWay,a=>((byte)(Dock)a)%2==0?(FloatField)"auto": (FloatField)"100%" },
                                {nameof(WrapPanel.Height),nameof(TabStripPlacement),this,BindingMode.OneWay,a=>((byte)(Dock)a)%2==0?(FloatField)"100%": (FloatField)"auto" }
                            }
                        }
                    },

                    new Border
                    {
                        Name="contentBorder",
                        Attacheds={ { Grid.ColumnIndex, 0,nameof(TabStripPlacement),this,BindingMode.OneWay,
                                a=> {
                                    var dock=(Dock)a;
                                    return (dock!= Dock.Left)?0:1;
                                } },{Grid.RowIndex,1,nameof(TabStripPlacement),this,BindingMode.OneWay,
                                a=> {
                                    var dock=(Dock)a;
                                    return (dock!= Dock.Top)?0:1;
                                } } },
                        Width="100%",
                        Height="100%",
                        BorderFill=null,
                        ClipToBounds=true,
                        //Background="#fff",
                        Child = new Panel{ Name="contentPanel",Width="100%",Height="100%" ,PresenterFor=this},
                    }

                }
            });
        }
    }
}
