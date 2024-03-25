using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using GGTalk.Linux;
using GGTalk.Linux.Controls;
using GGTalk.Linux.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGTalk.Linux.Views
{

    internal class MessageBoxManager
    {
        public static IMsgBoxWindow<ButtonResult> GetMessageBoxWindow(string title, string msg, ButtonEnum _buttonEnum = ButtonEnum.Ok)
        {
            return new MessageBoxWindow(title, msg, _buttonEnum);
        }
    }
    internal class MessageBoxWindow : Window, IMsgBoxWindow<ButtonResult>
    {

        private Button btn_cancel, btn_ok;
        private TextBlock textBlock;
        private ButtonEnum buttonEnum = ButtonEnum.Ok;
        public ButtonResult ButtonResult = ButtonResult.None;
        private string title = "提示", content = "";

        public MessageBoxWindow(string title, string msg, ButtonEnum _buttonEnum = ButtonEnum.Ok)
        {
            this.Title = title;
            this.content = msg;
            this.buttonEnum = _buttonEnum;
            this.MouseDown += MessageBoxWindow_MouseDown;
        }

        private void Initialize()
        {
            this.TopMost = true;
            this.textBlock = this.FindPresenterByName<TextBlock>("textBlock");
            this.textBlock.Text = this.content;
            this.btn_cancel = this.FindPresenterByName<ElButton>("btn_cancel");
            this.btn_ok = this.FindPresenterByName<ElButton>("btn_ok");            
            this.SetButtonStyle();
        }
        private void SetButtonStyle()
        {
            switch (this.buttonEnum)
            {
                case ButtonEnum.Ok:
                    this.btn_cancel.Visibility = Visibility.Hidden;
                    break;
                case ButtonEnum.YesNo:
                    this.btn_cancel.Content = "否";
                    this.btn_ok.Content = "是";
                    break;
                case ButtonEnum.OkCancel:
                    break;
                case ButtonEnum.OkAbort:
                    this.btn_cancel.Content = "中止";
                    break;
                case ButtonEnum.YesNoCancel:
                    this.btn_cancel.Visibility = Visibility.Hidden;
                    this.btn_ok.Content = "是";
                    break;
                case ButtonEnum.YesNoAbort:
                    this.btn_cancel.Content = "否";
                    this.btn_ok.Content = "是";
                    break;
            }
        }
        private void MessageBoxWindow_MouseDown(object sender, CPF.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            IsAntiAlias = true;
            CornerRadius = "3,3,3,3";
            Height = 170f;
            Width = 350f;
            Background = "#fff";
            ZIndex = 100;            
            Panel panel= new Panel()
            {
                Width = "100%",
                Height = "100%",
                Children = {
                    new ElButton
                    {
                        Name = "btn_cancel",
                        PresenterFor = this,
                        MarginRight = 86.3f,
                        MarginBottom = 10.1f,
                        Content = "取消",
                        Width = 54.3f,
                        Height = 25.9f,
                        Commands =
                        {
                            {
                                nameof(Button.Click),
                                (s,e)=>Btn_cancel_Click()
                            }
                        }
                    }, new ElButton
                    {
                        Name = "btn_ok",
                        PresenterFor = this,
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
                                (s,e)=>Btn_ok_Click()
                            }
                        }
                    }, new TextBlock
                    {
                        Name = "textBlock",
                        PresenterFor = this,
                        MarginLeft =10,
                        MarginTop=10,                        
                        MaxWidth = 330f,
                        FontSize = 14f,
                        Text = "12343",
                    }
            }
            };
            Children.Add(new WindowFrame(this, panel)
            {
                MaximizeBox = false,
                MinimizeBox = false
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            if (!DesignMode)//设计模式下不执行
            {
                this.Initialize();
            }
        }

        private void Btn_cancel_Click()
        {
            switch (this.buttonEnum)
            {
                case ButtonEnum.Ok:

                    break;
                case ButtonEnum.YesNo:
                    this.ButtonResult = ButtonResult.No;
                    break;
                case ButtonEnum.OkCancel:
                    this.ButtonResult = ButtonResult.Cancel;
                    break;
                case ButtonEnum.OkAbort:
                    this.ButtonResult = ButtonResult.Abort;
                    break;
                case ButtonEnum.YesNoCancel:
                    this.ButtonResult = ButtonResult.Cancel;
                    break;
                case ButtonEnum.YesNoAbort:
                    this.ButtonResult = ButtonResult.No;
                    break;
            }
            this.Close();
        }

        private void Btn_ok_Click()
        {
            switch (this.buttonEnum)
            {
                case ButtonEnum.Ok:
                    this.ButtonResult = ButtonResult.Ok;
                    break;
                case ButtonEnum.YesNo:
                    this.ButtonResult = ButtonResult.Yes;
                    break;
                case ButtonEnum.OkCancel:
                    this.ButtonResult = ButtonResult.Ok;
                    break;
                case ButtonEnum.OkAbort:
                    this.ButtonResult = ButtonResult.Ok;
                    break;
                case ButtonEnum.YesNoCancel:
                    this.ButtonResult = ButtonResult.Yes;
                    break;
                case ButtonEnum.YesNoAbort:
                    this.ButtonResult = ButtonResult.Yes;
                    break;
            }
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        TaskCompletionSource<ButtonResult> tcs = new TaskCompletionSource<ButtonResult>();
        Task<ButtonResult> IMsgBoxWindow<ButtonResult>.Show()
        {
            
            this.Closed += delegate
            {
                tcs.TrySetResult(this.ButtonResult);
            };
            //this.Position = this.GetPostion(null);
            this.Show();
            return tcs.Task;
        }

        Task<ButtonResult> IMsgBoxWindow<ButtonResult>.ShowDialog(Window ownerWindow)
        {
            //TaskCompletionSource<ButtonResult> tcs = new TaskCompletionSource<ButtonResult>();
            this.Closed += delegate
            {
                tcs.TrySetResult(this.ButtonResult);
            };
            //this.Position = this.GetPostion(ownerWindow);
            this.ShowDialog(ownerWindow);
            return tcs.Task;
        }

        private PixelPoint GetPostion(Window ownerWindow)
        {
            if (ownerWindow == null)
            {
                Point point = Screen.Bounds.Center;
                return new PixelPoint((int)(point.X - this.Width.Value / 2),(int) (point.Y -this.Height.Value / 2));
            }
            int offsetX = (int)(ownerWindow.Width.Value - this.Width.Value) / 2;
            int offsetY = (int)(ownerWindow.Height.Value - this.Height.Value) / 2;
            return new PixelPoint(ownerWindow.Position.X + offsetX, ownerWindow.Position.Y + offsetY);
        }
    }






    internal enum ButtonEnum
    {
        Ok = 0,
        YesNo = 1,
        OkCancel = 2,
        OkAbort = 3,
        YesNoCancel = 4,
        YesNoAbort = 5
    }

    [Flags]
    internal enum ButtonResult
    {
        Ok = 0,
        Yes = 1,
        No = 2,
        Abort = 3,
        Cancel = 4,
        None = 5
    }

    internal interface IMsgBoxWindow<T>
    {
        Task<T> Show();
        Task<T> ShowDialog(Window ownerWindow);
    }
}
