﻿using ESBasic;
using OMCS.Passive;
using OMCS.Passive.MultiChat;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OMCS.Boost.MultiChat
{
    /// <summary>
    /// 视频聊天控件。由MultiVideoChatContainer使用。
    /// </summary>
    public partial class SpeakerVideoPanel : UserControl, ISpeakerPanel
    {
        private IChatUnit chatUnit;
        private bool isMySelf = false;
        /// <summary>
        /// 该事件用于支持双击小视频窗口，全屏显示该成员的图像。
        /// </summary>
        public event CbGeneric<SpeakerVideoPanel,IChatUnit> VideoDoubleClicked;

        public SpeakerVideoPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 是否开启用于控制发言的CheckBox。
        /// </summary>
        public bool SpeakControlEnabled
        {
            get
            {
                return this.checkBox_allowSpeak.Visible;
            }
            set
            {
                this.checkBox_allowSpeak.Visible = value;
            }
        }

        public string MemberID
        {
            get
            {
                if (this.chatUnit == null)
                {
                    return null;
                }

                return this.chatUnit.MemberID;
            }
        }

        /// <summary>
        /// 初始化成员视频显示控件。
        /// </summary>       
        public void Initialize(IChatUnit unit ,bool myself , string showName)
        {
            this.Height = 200;// this.toolStrip1.Height + 180;
            this.Width = 234;
            this.pictureBox_Camera.Size = new Size(32, 32);
            this.pictureBox_Mic.Size = new Size(24, 24);
            

            this.chatUnit = unit;
            this.isMySelf = myself;
            this.toolStripLabel_displayName.Text = showName;
            this.decibelDisplayer1.Visible = !myself;            

            //初始化麦克风连接器
            this.chatUnit.MicrophoneConnector.Mute = myself;
            this.chatUnit.MicrophoneConnector.SpringReceivedEventWhenMute = myself;
            this.chatUnit.MicrophoneConnector.ConnectEnded += new CbGeneric<string, ConnectResult>(MicrophoneConnector_ConnectEnded);
            this.chatUnit.MicrophoneConnector.OwnerOutputChanged += new CbGeneric<string>(MicrophoneConnector_OwnerOutputChanged);
            this.chatUnit.MicrophoneConnector.AudioDataReceived += new CbGeneric<string,byte[]>(MicrophoneConnector_AudioDataReceived);
            this.chatUnit.MicrophoneConnector.BeginConnect(unit.MemberID);

            //初始化摄像头连接器
            this.chatUnit.DynamicCameraConnector.SetViewer(this.cameraPanel1);
            this.chatUnit.DynamicCameraConnector.ConnectEnded += new CbGeneric<string, ConnectResult>(DynamicCameraConnector_ConnectEnded);
            this.chatUnit.DynamicCameraConnector.OwnerOutputChanged += new CbGeneric<string>(DynamicCameraConnector_OwnerOutputChanged);
            this.chatUnit.DynamicCameraConnector.Disconnected += new CbGeneric<string, ConnectorDisconnectedType>(DynamicCameraConnector_Disconnected);
            this.chatUnit.DynamicCameraConnector.BeginConnect(unit.MemberID);
        }

        void DynamicCameraConnector_Disconnected(string ownerID ,ConnectorDisconnectedType disconnectedType)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string ,ConnectorDisconnectedType>(this.DynamicCameraConnector_Disconnected),  ownerID , disconnectedType);
            }
            else
            {
                this.label_tip.Text = disconnectedType.ToString();
                this.label_tip.Visible = true;
            }
        }

        //好友启用或禁用摄像头
        void DynamicCameraConnector_OwnerOutputChanged(string ownerID)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string>(this.DynamicCameraConnector_OwnerOutputChanged) , ownerID);
            }
            else
            {
                this.ShowCameraState();
            }
        }

        private ConnectResult connectCameraResult;
        //摄像头连接器尝试连接的结果
        void DynamicCameraConnector_ConnectEnded(string ownerID ,ConnectResult res)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string ,ConnectResult>(this.DynamicCameraConnector_ConnectEnded), ownerID, res);
            }
            else
            {
                if (res == ConnectResult.Succeed)
                {
                    if (this.chatUnit.DynamicCameraConnector.OwnerMachineType == Passive.Video.MachineType.Android || this.chatUnit.DynamicCameraConnector.OwnerMachineType == Passive.Video.MachineType.IOS)
                    {
                        this.chatUnit.DynamicCameraConnector.VideoDrawMode = VideoDrawMode.Scale;
                    }
                }
                this.label_tip.Visible = false;
                this.connectCameraResult = res;
                this.ShowCameraState();
            }           
        }

        /// <summary>
        /// 综合显示摄像头的状态。
        /// </summary>
        private void ShowCameraState()
        {            
            if (this.connectCameraResult != OMCS.Passive.ConnectResult.Succeed)
            {
                this.pictureBox_Camera.BackgroundImage = null;
                this.pictureBox_Camera.BackgroundImage = Properties.Resources.cameraFail; //this.imageList2.Images[2];
                this.pictureBox_Camera.Visible = true;
                this.toolTip1.SetToolTip(this.pictureBox_Camera, this.connectCameraResult.ToString());
            }
            else
            {
                this.pictureBox_Camera.Visible = !this.chatUnit.DynamicCameraConnector.OwnerOutput;                
                if (!this.chatUnit.DynamicCameraConnector.OwnerOutput)
                {
                    this.pictureBox_Camera.BackgroundImage = Properties.Resources.cameraDis;//this.imageList2.Images[1];
                    this.toolTip1.SetToolTip(this.pictureBox_Camera, "摄像头被主人禁用！");
                    return;
                }     
            }
        }

        //将接收到的声音数据交给分贝显示器显示
        void MicrophoneConnector_AudioDataReceived(string ownerID,byte[] data)
        {
            this.decibelDisplayer1.DisplayAudioData(data);
        }

        //好友启用或禁用麦克风
        void MicrophoneConnector_OwnerOutputChanged(string ownerID)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string>(this.MicrophoneConnector_OwnerOutputChanged));
            }
            else
            {
                this.ShowMicState();
            }
        }

        private ConnectResult connectMicResult;
        //麦克风连接器尝试连接的结果
        void MicrophoneConnector_ConnectEnded(string ownerID, ConnectResult res)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string, ConnectResult>(this.MicrophoneConnector_ConnectEnded), ownerID, res);
            }
            else
            {
                this.connectMicResult = res;
                this.ShowMicState();
            }
        }

        /// <summary>
        /// 综合显示麦克风的状态。
        /// </summary>
        private void ShowMicState()
        {            
            if (this.connectMicResult != OMCS.Passive.ConnectResult.Succeed)
            {
                this.checkBox_allowSpeak.Checked = false;
                this.pictureBox_Mic.Visible = true;
                this.toolTip1.SetToolTip(this.pictureBox_Mic, this.connectMicResult.ToString());
            }
            else
            {
                this.checkBox_allowSpeak.Checked = this.chatUnit.MicrophoneConnector.OwnerOutput;
                this.decibelDisplayer1.Working = false;
                this.pictureBox_Mic.Visible = !this.chatUnit.MicrophoneConnector.OwnerOutput;
                this.decibelDisplayer1.Visible = this.chatUnit.MicrophoneConnector.OwnerOutput && !this.isMySelf;
                if (!this.chatUnit.MicrophoneConnector.OwnerOutput)
                {
                    this.pictureBox_Mic.BackgroundImage = Properties.Resources.micFail; //this.imageList1.Images[2];
                    this.toolTip1.SetToolTip(this.pictureBox_Mic, "麦克风被主人禁用！");
                    return;
                }

                this.pictureBox_Mic.Visible = !isMySelf;
                if (this.chatUnit.MicrophoneConnector.Mute)
                {
                    this.pictureBox_Mic.BackgroundImage = Properties.Resources.micDis; //this.imageList1.Images[1];
                    this.toolTip1.SetToolTip(this.pictureBox_Mic, "静音");
                }
                else
                {
                    this.pictureBox_Mic.Visible = false;
                    this.pictureBox_Mic.BackgroundImage = Properties.Resources.micNor;// this.imageList1.Images[0];
                    this.toolTip1.SetToolTip(this.pictureBox_Mic, "正常");
                    this.decibelDisplayer1.Working = true;
                }
            }

        }
       
        /// <summary>
        /// 展开或收起视频面板。
        /// </summary>       
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Height > this.toolStrip1.Height)
                {
                    this.toolStripButton1.Text = "展开";
                    this.toolStripButton1.Image = OMCS.Boost.Properties.Resources.Hor;
                    //this.chatUnit.DynamicCameraConnector.SetViewer(null);
                    this.Height = this.toolStrip1.Height;
                }
                else
                {
                    this.toolStripButton1.Text = "收起";
                    this.toolStripButton1.Image = OMCS.Boost.Properties.Resources.Ver;
                    this.Height = 200;// this.toolStrip1.Height + 180;
                    this.Width = 234;
                    this.chatUnit.DynamicCameraConnector.SetViewer(this.cameraPanel1);
                }
            }
            catch (Exception ee)
            {               
                MessageBox.Show(ee.Message);
            }
        }

        private void pictureBox_Mic_Click(object sender, EventArgs e)
        {

        }

        private void checkBox_allowSpeak_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.chatUnit.MicrophoneConnector.Connected)
            {
                return;
            }
           
            this.chatUnit.MicrophoneConnector.ChangeOwnerOutput(this.checkBox_allowSpeak.Checked);
        }

        private void skinPanel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.VideoDoubleClicked != null)
            {
                this.VideoDoubleClicked(this,this.chatUnit);
            }
        }

        public void ResetViewer()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.chatUnit.DynamicCameraConnector.SetViewer(this.cameraPanel1);
        }
    }
}
