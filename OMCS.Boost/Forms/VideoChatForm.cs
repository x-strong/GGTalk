using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCWin;
using CCWin.Win32;
using CCWin.Win32.Const;
using System.Diagnostics;
using ESBasic;
using OMCS.Passive;
using ESBasic.Threading.Timers;
using ESBasic.Loggers;

namespace OMCS.Boost.Forms
{
    /// <summary>
    /// 视频聊天窗口。
    /// </summary>
    public partial class VideoChatForm : CCSkinMain, IVideoChatForm
    {
        private HungUpCauseType hungUpCauseType = HungUpCauseType.ActiveHungUp;
        private bool activeHungUp = true;
        private IMultimediaManager multimediaManager;       
        private bool isWaitingAnswer ;
        private CallbackTimer<Size> switchCameraSzieCallbackTimer = new CallbackTimer<Size>();
        private IAgileLogger agileLogger;

        /// <summary>
        /// 当自己挂断视频或与对方设备的连接中断时，触发此事件。
        /// </summary>
        public event CbGeneric<HungUpCauseType> HungUpVideo;

        public event CbGeneric ConnectorDisconnected;

        private string currentLoginID;
        private string friendLoginID;
        public VideoChatForm(string currentLoginID,  string friendName, bool waitingAnswer, IAgileLogger logger)
        {
            InitializeComponent();           

            this.skinLabel_tip.Location = new Point(this.cameraPanel1.Location.X + 3, this.cameraPanel1.Location.Y + this.cameraPanel1.Height + 20);
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.UserPaint, true);//自行绘制           

            this.CanResize = true;            

            this.agileLogger = logger;
            this.currentLoginID = currentLoginID;
            this.isWaitingAnswer = waitingAnswer ;
            this.Text = string.Format("正在和{0}视频会话" ,friendName);
            if (!this.isWaitingAnswer)
            {
                this.skinLabel_tip.Text = "正在连接 . . .";
            }
        }       
        
        public void Initialize(IMultimediaManager mgr, string destLoginID)
        {
            this.friendLoginID=destLoginID;
            this.multimediaManager = mgr;
            this.multimediaManager.CameraEncodeQuality = 6;
            this.multimediaManager.AudioCaptured += new CbGeneric<byte[]>(multimediaManager_AudioCaptured);
            this.microphoneConnector1.AudioDataReceived += new CbGeneric<string,byte[]>(microphoneConnector1_AudioDataReceived);
            this.microphoneConnector1.Disconnected += new CbGeneric<string, ConnectorDisconnectedType>(microphoneConnector1_Disconnected);
            this.dynamicCameraConnector_myself.Disconnected += new CbGeneric<string, ConnectorDisconnectedType>(cameraConnector1_Disconnected);
            this.dynamicCameraConnector1.ConnectEnded += DynamicCameraConnector1_ConnectEnded;
            this.skinCheckBox_camera.Checked = this.multimediaManager.OutputVideo;//如果是ConnectOnlyWhenNeed，则每次都会重新创建multimediaManager
            this.skinCheckBox_mic.Checked = this.multimediaManager.OutputAudio; //如果是ConnectOnlyWhenNeed，则每次都会重新创建multimediaManager
            if (!this.isWaitingAnswer) //同意视频，开始连接
            {
                this.OnAgree(this.friendLoginID);
            }

            this.channelQualityDisplayer1.Initialize(this.microphoneConnector1);
            this.switchCameraSzieCallbackTimer.DetectSpanInSecs = 1;
            this.switchCameraSzieCallbackTimer.Start();
        }

        private void DynamicCameraConnector1_ConnectEnded(string obj1, ConnectResult connectResult)
        {
            if (connectResult == ConnectResult.Succeed)
            {
                if (this.dynamicCameraConnector1.OwnerMachineType == Passive.Video.MachineType.Android || this.dynamicCameraConnector1.OwnerMachineType == Passive.Video.MachineType.IOS)
                {
                    this.dynamicCameraConnector1.VideoDrawMode = VideoDrawMode.Scale;
                }
            }
        }

        void cameraConnector1_Disconnected(string ownerID, ConnectorDisconnectedType type)
        {
            if (type == ConnectorDisconnectedType.GuestActiveDisconnect)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string ,ConnectorDisconnectedType>(this.cameraConnector1_Disconnected),  ownerID, type);
            }
            else
            {
                this.CheckConnectorState();
            }
        }

        void microphoneConnector1_Disconnected(string ownerID, ConnectorDisconnectedType type)
        {
            if (type == ConnectorDisconnectedType.GuestActiveDisconnect)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string, ConnectorDisconnectedType>(this.microphoneConnector1_Disconnected), ownerID, type);
            }
            else
            {
                this.decibelDisplayer_speaker.Error = true;
                this.CheckConnectorState();
            }
        }

        private void CheckConnectorState()
        {
            if((!this.dynamicCameraConnector1.Connected ) && (!this.microphoneConnector1.Connected))
            {
                this.hungUpCauseType = HungUpCauseType.ConnectorDisconnected;
                this.Close();
            }
        }       

        void microphoneConnector1_AudioDataReceived(string ownerID, byte[] data)
        {
            this.decibelDisplayer_speaker.DisplayAudioData(data);
        }

        void multimediaManager_AudioCaptured(byte[] obj)
        {
            this.decibelDisplayer_mic.DisplayAudioData(obj);
        }       

        private void FocusCurrent(object sender, EventArgs e)
        {
            this.Focus();
        }

        /// <summary>
        /// 对方同意视频会话
        /// </summary>
        public void OnAgree(string destLoginID)
        {
            try
            {
                this.friendLoginID = destLoginID;
                this.skinLabel_tip.Text = "正在连接 . . .";                
                this.dynamicCameraConnector1.SetViewer(this.cameraPanel1);
                this.dynamicCameraConnector_myself.SetViewer(this.cameraPanel_myself);
                this.dynamicCameraConnector_myself.BeginConnect(this.currentLoginID);
                this.dynamicCameraConnector1.BeginConnect(this.friendLoginID);
                this.microphoneConnector1.BeginConnect(this.friendLoginID);                
                this.decibelDisplayer_mic.Working = true;
                this.decibelDisplayer_speaker.Working = true;
            }
            catch (Exception ee)
            {
            }
        }

        public void OnReject()
        {
            this.hungUpCauseType = HungUpCauseType.DestReject;
        }

        /// <summary>
        /// 对方挂断视频
        /// </summary>
        public void OnHungUpVideo()
        {
            //在网络差时，摄像头设备还未感受到对方的断开，导致摄像头仍然在工作
            this.multimediaManager.DisconnectGuest(this.friendLoginID, OMCS.MultimediaDeviceType.Camera, false);
            this.multimediaManager.DisconnectGuest(this.friendLoginID, OMCS.MultimediaDeviceType.Microphone, false);

            this.Visible = false;
            this.activeHungUp = false;
            this.Close();
        }
      

        private void VideoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.multimediaManager == null)
                {
                    return;
                }

                this.decibelDisplayer_mic.Working = false;
                this.decibelDisplayer_speaker.Working = false;
                this.switchCameraSzieCallbackTimer.Stop();
                this.dynamicCameraConnector_myself.Disconnect();
                this.dynamicCameraConnector1.Disconnect();
                this.microphoneConnector1.Disconnect();
                this.timerLabel1.Stop();

                if (this.activeHungUp)
                {
                    if (this.HungUpVideo != null)
                    {
                        this.HungUpVideo(this.hungUpCauseType);
                    }
                }
                this.ConnectorDisconnected?.BeginInvoke(null,null);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void dynamicCameraConnector1_ConnectEnded(string ownerID, ConnectResult res)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string ,ConnectResult>(this.dynamicCameraConnector1_ConnectEnded),  ownerID, res);
            }
            else
            {
                try
                {
                    this.skinPanel_tool.Visible = true;
                    this.skinLabel_tip.Visible = false;
                    this.timerLabel1.Start();

                    if (res != ConnectResult.Succeed)
                    {                      
                        this.skinLabel_cameraError.Text = res.ToString();
                        this.skinLabel_cameraError.Visible = true;
                    }
                }
                catch (Exception ee)
                {
                    this.agileLogger.Log(ee, "dynamicCameraConnector1_ConnectEnded", ESBasic.Loggers.ErrorLevel.Standard);
                    MessageBox.Show(ee.Message + " - " + ee.StackTrace);
                }
            }
        }

        private string microphoneConnectorConnectError = null;
        private void microphoneConnector1_ConnectEnded(string ownerID ,ConnectResult res)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string ,ConnectResult>(this.microphoneConnector1_ConnectEnded),  ownerID, res);
            }
            else
            {
                this.microphoneConnectorConnectError = null;
                if (res != ConnectResult.Succeed)
                {
                    this.microphoneConnectorConnectError = OMCS.Boost.EnumDescriptor.GetDescription(res);                       
                    this.decibelDisplayer_speaker.Working = false;
                    this.decibelDisplayer_speaker.Error = true;                      
                }

                bool p2p = this.multimediaManager.IsP2PChannelExist(this.friendLoginID);
                this.label_P2P.Visible = p2p;
            }
        }  

        private void skinCheckBox_camera_CheckedChanged(object sender, EventArgs e)
        {            
            this.multimediaManager.OutputVideo = this.skinCheckBox_camera.Checked;
        }

        private void skinCheckBox_mic_CheckedChanged(object sender, EventArgs e)
        {         
            this.multimediaManager.OutputAudio = this.skinCheckBox_mic.Checked;
            this.decibelDisplayer_mic.Working = this.skinCheckBox_mic.Checked;
        }

        private void skinCheckBox_speaker_CheckedChanged(object sender, EventArgs e)
        {           
            this.microphoneConnector1.Mute = !this.skinCheckBox_speaker.Checked;
            //this.decibelDisplayer_speaker.Working = this.skinCheckBox_speaker.Checked;
        }

        private void skinCheckBox_my_CheckedChanged(object sender, EventArgs e)
        {          
            this.cameraPanel_myself.Visible = this.skinCheckBox_my.Checked;
        }

        private void skinButton_State_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dynamicCameraConnector1_OwnerOutputChanged(string ownerID)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string>(this.dynamicCameraConnector1_OwnerOutputChanged) ,  ownerID);
            }
            else
            {
                this.pictureBox_disCamera.Visible = !this.dynamicCameraConnector1.OwnerOutput;               
            }
        }

        private void microphoneConnector1_OwnerOutputChanged(string ownerID)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string>(this.microphoneConnector1_OwnerOutputChanged));
            }
            else
            {
                this.pictureBox_disMic.Visible = !this.microphoneConnector1.OwnerOutput;
            }
        }        
       

        private void dynamicCameraConnector1_OwnerCameraVideoSizeChanged(string ownerID, Size obj)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string, Size>(this.dynamicCameraConnector1_OwnerCameraVideoSizeChanged),  ownerID, obj);
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }       

        private void decibelDisplayer_speaker_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.microphoneConnectorConnectError != null)
            {
                MessageBox.Show(this.microphoneConnectorConnectError);
            }
        }

        #region 双击视频窗口，切换自己和对方的图像
        private bool myselfLittle = true;
        private void skinPanel2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.myselfLittle = !this.myselfLittle;
            if (myselfLittle)
            {
                this.dynamicCameraConnector1.SetViewer(this.cameraPanel1);
                this.dynamicCameraConnector_myself.SetViewer(this.cameraPanel_myself);
            }
            else
            {
                this.dynamicCameraConnector1.SetViewer(this.cameraPanel_myself);
                this.dynamicCameraConnector_myself.SetViewer(this.cameraPanel1);
            }
        }    
        #endregion             
    }

   
}
