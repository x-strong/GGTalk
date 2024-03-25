using CPF;
using CPF.Controls;
using CPF.Drawing;
using ESBasic;
using ESBasic.Helpers;
using ESPlus.FileTransceiver;
using GGTalk.Linux;
using GGTalk.Linux.Helpers;
using GGTalk.Linux.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace GGTalk.Linux.Controls
{
    internal class FileTransferItem : Control
    {
        private Button button_offlineSend;
        private Button button_cancel;
        private Button button_receive;
        private TextBlock textBlock_FileName;
        private TextBlock textBlock_speed;
        private TextBlock textBlock_speedTitle;
        private TextBlock textBlock_fileSize;
        private Picture image_send, image_receive, pic_transferType;
        private ProgressBar file_ProgressBar;
        private TransferingProject transmittingFileInfo;
        private DateTime lastDisplaySpeedTime = DateTime.Now;
        private ulong lastTransmittedPreSecond = 0;
        private bool isTransfering = false;
        public bool IsTransfering
        {
            get { return isTransfering; }
            set
            {
                isTransfering = value;
                if (isTransfering)
                {
                    this.button_offlineSend.Visibility = Visibility.Visible;
                }
            }
        }
        private Window parentWindow;

        public FileTransferItem()
        {
            this.InitializeComponent();
        }



        private bool initialized = false;
        protected override void InitializeComponent()
        {
            if (this.initialized) { return; }
            this.initialized = true;
            //模板定义
            Children.Add(
                new Grid
                {
                    Width = 220,
                    Height = 70,
                    Children =
                {
                    new Picture
                    {
                        PresenterFor = this,
                        MarginLeft = 16,
                        Name=nameof(this.pic_transferType),
                        Width = 29,
                        Height = 29,
                        MarginTop = 3,
                        Stretch=Stretch.UniformToFill,
                        Source = CommonOptions.ResourcesCatalog+ "wenjianjia.png",
                    },
                    new Picture
                    {
                        PresenterFor = this,
                        Name="Image_send",
                        Width = 14,
                        Height = 14,
                        MarginLeft = 50,
                        MarginTop = 3,
                        Stretch=Stretch.UniformToFill,
                        Source = CommonOptions.ResourcesCatalog+ "edit-redo1.png",
                    },
                    new Picture
                    {
                        PresenterFor = this,
                        Name="Image_receive",
                        Width = 14,
                        Height = 14,
                        MarginLeft = 50,
                        MarginTop = 3,
                        Stretch=Stretch.UniformToFill,
                        Source = CommonOptions.ResourcesCatalog+ "edit-undo.png",
                    },
                    new TextBlock
                    {
                        PresenterFor = this,
                        Name="TextBlock_FileName",
                        Text = "傲瑞通Linux版本.Zip",
                        TextTrimming=TextTrimming.CharacterEllipsis,
                        Width=140,
                        Height=25,
                        MarginLeft = 70,
                        MarginTop = 3,
                    },
                    new ProgressBar
                    {
                        PresenterFor = this,
                        Name="File_ProgressBar",
                        Width = 140,
                        Height = 10,
                        MarginLeft = 50,
                        MarginTop = 21,
                    },
                    new TextBlock
                    {
                        PresenterFor = this,
                        Name="TextBlock_speedTitle",
                        Text = "速度 :",
                        MarginLeft = 16,
                        MarginTop = 35,
                    },
                    new TextBlock
                    {
                        PresenterFor = this,
                        Name="TextBlock_speed",
                        Text = "142k/s",
                        MarginLeft = 56,
                        MarginTop = 35,
                    },
                    new TextBlock
                    {
                        PresenterFor = this,
                        Name="TextBlock_fileSize",
                        Text = "25.31M",
                        MarginLeft = 150,
                        MarginTop = 35,
                    },
                    new Button
                    {
                        PresenterFor = this,
                        Visibility = Visibility.Hidden,
                        Name="Button_offlineSend",
                        BorderThickness = new Thickness(0,0,0,0),
                        BorderType = BorderType.BorderThickness,
                        Width = 68,
                        Height = 21,
                        Cursor = Cursors.Hand,
                        Content = "转离线发送",
                        MarginLeft = 90,
                        MarginTop = 50,
                        Background = Color.Transparent,
                        Foreground = "#0000ff"
                    },
                    new Button
                    {
                        PresenterFor = this,
                        Name="Button_receive",
                        BorderThickness = new Thickness(0,0,0,0),
                        BorderType = BorderType.BorderThickness,
                        Width = 31,
                        Height = 21,
                        Cursor = Cursors.Hand,
                        Content = "接收",
                        MarginLeft = 130,
                        MarginTop = 50,
                        Background = Color.Transparent,
                        Foreground = "#0000ff"
                    },
                    new Button
                    {
                        PresenterFor = this,
                        Name="Button_cancel",
                        BorderThickness = new Thickness(0,0,0,0),
                        BorderType = BorderType.BorderThickness,
                        Width = 31,
                        Height = 21,
                        Cursor = Cursors.Hand,
                        Content = "取消",
                        MarginLeft = 170,
                        MarginTop = 50,
                        Background = Color.Transparent,
                        Foreground = "#0000ff"
                    }
                }
                });


            this.button_offlineSend = FindPresenterByName<Button>("Button_offlineSend");
            this.button_offlineSend.Click += label_offlineSend_Click;
            this.button_cancel = FindPresenterByName<Button>("Button_cancel");
            this.button_cancel.Click += skinLabel2_Click;
            this.button_receive = FindPresenterByName<Button>("Button_receive");
            this.button_receive.Click += linkLabel_receive_LinkClicked;
            textBlock_speed = FindPresenterByName<TextBlock>("TextBlock_speed");
            textBlock_speedTitle = FindPresenterByName<TextBlock>("TextBlock_speedTitle");
            textBlock_fileSize = FindPresenterByName<TextBlock>("TextBlock_fileSize");
            textBlock_FileName = FindPresenterByName<TextBlock>("TextBlock_FileName");
            image_receive = FindPresenterByName<Picture>("Image_receive");
            image_send = FindPresenterByName<Picture>("Image_send");
            image_receive = FindPresenterByName<Picture>("Image_receive");
            file_ProgressBar = FindPresenterByName<ProgressBar>("File_ProgressBar");
            this.pic_transferType = this.FindPresenterByName<Picture>(nameof(this.pic_transferType));
        }

        /// <summary>
        /// FileCanceled 当点击“取消”按钮时，将触发此事件。
        /// </summary>
        public event CbFileCanceled FileCanceled;
        /// <summary>
        /// 当 点击 “接收”按钮 时，触发
        /// </summary>
        public event CbFileReceived FileReceived;
        /// <summary>
        /// 当点击“拒绝”按钮时，触发
        /// </summary>
        public event CbFileRejected FileRejected;

        /// <summary>
        /// 当点击“转离线发送”按钮时，触发。
        /// </summary>
        public event CbGeneric<TransferingProject, FileTransferItem> ConvertToOfflineFile;

        #region CancelEnabled
        public bool CancelEnabled
        {
            get
            {
                if (this.button_cancel.Visibility == Visibility.Visible)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (CancelEnabled)
                {
                    this.button_cancel.Visibility = Visibility.Visible;
                }
                else
                {
                    this.button_cancel.Visibility = Visibility.Hidden;
                }

            }
        }
        #endregion


        #region Initialize
        public void Initialize(TransferingProject info, bool offlineFile, bool doneAgreed, Window _parentWindow)
        {
            this.transmittingFileInfo = info;
            this.parentWindow = _parentWindow;
            //this.ShowIcon(info.ProjectName, info.IsFolder);

            this.textBlock_FileName.Text = this.transmittingFileInfo.ProjectName;
            this.textBlock_FileName.ToolTip= this.transmittingFileInfo.ProjectName;
            if (info.IsSender)
            {
                this.image_send.Visibility = Visibility.Visible;
                this.image_receive.Visibility = Visibility.Hidden;
                this.button_receive.Visibility = Visibility.Hidden;
                this.button_receive.IsEnabled = false;
                this.button_cancel.Visibility = Visibility.Visible;
                this.button_cancel.IsEnabled = true;
                this.button_offlineSend.Visibility =CommonHelper.GetVisibility(!offlineFile && !ESFramework.NetServer.IsServerUser(this.transmittingFileInfo.DestUserID) && info.OriginStream == null);
            }
            else
            {
                this.image_send.Visibility = Visibility.Hidden;
                this.image_receive.Visibility = Visibility.Visible;
                this.button_receive.Visibility = Visibility.Visible;
                this.button_receive.IsEnabled = true;
                this.button_cancel.Visibility = Visibility.Visible;
                this.button_cancel.IsEnabled = true;
                this.button_offlineSend.Visibility = Visibility.Hidden;
                this.button_cancel.Content = "拒绝";

                if (doneAgreed)
                {
                    this.button_receive.IsEnabled = false;
                    this.button_receive.Visibility = Visibility.Hidden;
                    this.button_offlineSend.Visibility = Visibility.Hidden;
                    this.button_receive.IsEnabled = false;
                    this.button_cancel.Content = "取消";
                }
            }
            this.textBlock_speed.Visibility = Visibility.Hidden;
            this.textBlock_speedTitle.Visibility = Visibility.Hidden;
            this.pic_transferType.Source = info.IsFolder ? GGTalk.Linux.Helpers.FileHelper.FindAssetsBitmap("wenjianjia.png") : GGTalk.Linux.Helpers.FileHelper.GetFileIconBitmap(info.ProjectName);
            string sizeStr = PublicHelper.GetSizeString((ulong)this.transmittingFileInfo.TotalSize);
            this.textBlock_fileSize.Text = sizeStr;
        }

        //private void ShowIcon(string fileName, bool isFolder)
        //{
        //    Image bmp = null;
        //    if (isFolder)
        //    {
        //        bmp = this.imageList1.Images[0];
        //    }
        //    else
        //    {
        //        string[] ary = fileName.Split('.');
        //        if (ary.Length == 1)
        //        {
        //            Icon icon = WindowsHelper.GetSystemIconByFileType(".txt", true);
        //            bmp = icon.ToBitmap();
        //        }
        //        else
        //        {
        //            string extendName = "." + ary[ary.Length - 1].ToLower();
        //            Icon icon = WindowsHelper.GetSystemIconByFileType(extendName, true);
        //            bmp = icon.ToBitmap();
        //        }
        //    }

        //    if (bmp != null)
        //    {
        //        this.pictureBox1.Image = bmp;
        //    }
        //}

        #endregion

        public void CheckZeroSpeed()
        {
            TimeSpan span = DateTime.Now - this.lastDisplaySpeedTime;

            if (span.TotalSeconds >= 1)
            {
                this.SetProgress(this.totalSize, this.lastTransmitted);
            }
        }


        private DateTime lastSetTime = DateTime.Now;
        public void SetProgress(ulong total, ulong transmitted)
        {
            TimeSpan span = DateTime.Now - this.lastSetTime;
            if (span.TotalSeconds < 0.2)
            {
                return;
            }

            this.lastSetTime = DateTime.Now;
            this.SetProgress2(total, transmitted);
        }

        private ulong lastSpeed = 0;
        private bool firstSecond = true; //解决续传时，初始速度非常大的bug
        private ulong totalSize = 1; //解决0速度的问题
        private ulong lastTransmitted = 0;
        private void SetProgress2(ulong total, ulong transmitted)
        {
            this.totalSize = total;
            this.lastTransmitted = transmitted;
            UiSafeInvoker.ActionOnUI(this.ShowProgress);
        }

        private void ShowProgress()
        {
            this.textBlock_speed.Visibility = Visibility.Visible;
            this.textBlock_speedTitle.Visibility = Visibility.Visible;
            this.button_offlineSend.Visibility = Visibility.Hidden;

            this.file_ProgressBar.Maximum = 1000;

            this.file_ProgressBar.Value = (int)(this.lastTransmitted * 1000 / this.totalSize);

            DateTime now = DateTime.Now;
            TimeSpan span = now - this.lastDisplaySpeedTime;

            if (span.TotalSeconds >= 1)
            {
                if (!this.firstSecond)
                {
                    if (lastSpeed == 0)
                    {
                        lastSpeed = (ulong)((this.lastTransmitted - this.lastTransmittedPreSecond) / span.TotalSeconds); ;
                    }

                    ulong transferSpeed = (ulong)((this.lastTransmitted - this.lastTransmittedPreSecond) / span.TotalSeconds);
                    //transferSpeed = (transferSpeed + 7 * this.lastSpeed) / 8;
                    this.lastSpeed = transferSpeed;
                    byte littleNum = 0;
                    if (transferSpeed > 1024 * 1024)
                    {
                        littleNum = 1;
                    }
                    this.textBlock_speed.Text = PublicHelper.GetSizeString((ulong)transferSpeed, littleNum) + "/s";
                    int leftSecs = transferSpeed == 0 ? 10000 : (int)((this.totalSize - this.lastTransmitted) / transferSpeed);
                    int hour = leftSecs / 3600;
                    int min = (leftSecs % 3600) / 60;
                    int sec = ((leftSecs % 3600) % 60) % 60;
                    this.lastDisplaySpeedTime = now;
                }

                this.lastTransmittedPreSecond = this.lastTransmitted;

                if (this.firstSecond)
                {
                    this.firstSecond = false;
                }
            }

        }

        public TransferingProject TransmittingProject
        {
            get
            {
                return this.transmittingFileInfo;
            }
        }


        private void linkLabel_receive_LinkClicked(object sender, RoutedEventArgs e)
        {
            GetPathToSave("保存", this.transmittingFileInfo.ProjectName);
        }


        public async void GetPathToSave(string title, string defaultName)
        {
            //在linux上接收文件夹 启用选择文件夹对话框
            bool needOpenFolderDialog = !RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && this.transmittingFileInfo.IsFolder;
            Task<string> task = needOpenFolderDialog ? GGTalk.Linux.Helpers.FileHelper.OpenFolderDialog(this.parentWindow, title) : GGTalk.Linux.Helpers.FileHelper.SaveFileDialog(this.parentWindow, title, defaultName);
            await task.ConfigureAwait(true);
            string savePath = needOpenFolderDialog ? task.Result  + defaultName : task.Result;
            GlobalResourceManager.Logger.LogWithTime("文件保存路径：" + savePath);
            if (!string.IsNullOrEmpty(savePath))
            {
                if (File.Exists(savePath) || Directory.Exists(savePath))
                {
                    Task<ButtonResult> task1 = MessageBoxEx.ShowQuery(string.Format("{0}已经存在，确定要覆盖它吗？", ESBasic.Helpers.FileHelper.GetFileNameNoPath(savePath)));
                    await task1.ConfigureAwait(true);
                    if (task1.Result != ButtonResult.Yes)
                    {
                        return;
                    }
                }
                UIThreadPoster<string> poster = new UIThreadPoster<string>(this.AgreeReceive, savePath);
                poster.Post();
            }
        }

        private void AgreeReceive(string savePath)
        {
            if (this.FileReceived != null)
            {
                this.button_receive.IsEnabled = false;
                this.button_receive.Visibility = Visibility.Hidden;
                this.button_cancel.Content = "取消";

                this.FileReceived(this, this.transmittingFileInfo.ProjectID, this.transmittingFileInfo.IsSender, savePath);
            }
        }


        private void skinLabel2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.button_cancel.Content.ToString() == "拒绝")
                {
                    if (this.FileRejected != null)
                    {
                        this.FileRejected(this.transmittingFileInfo.ProjectID);
                    }
                }
                else
                {
                    if (this.FileCanceled != null)
                    {
                        this.FileCanceled(this, this.transmittingFileInfo.ProjectID, this.transmittingFileInfo.IsSender);
                    }
                }
            }
            catch (Exception ee)
            {
                //MessageBox.Show(ee.Message, "ESFramework.Boost.Controls");
            }
        }

        private void label_offlineSend_Click(object sender, RoutedEventArgs e)
        {
            if (this.ConvertToOfflineFile != null)
            {
                this.ConvertToOfflineFile(this.transmittingFileInfo, this);
            }
        }

        private void skinLabel_FileName_Click(object sender, RoutedEventArgs e)
        {

        }

        private void skinProgressBar2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    internal delegate void CbFileCanceled(FileTransferItem item, string projectID, bool isSend);
    internal delegate void CbFileReceived(FileTransferItem item, string projectID, bool isSend, string savePath);
    internal delegate void CbFileRejected(string projectID);
}
