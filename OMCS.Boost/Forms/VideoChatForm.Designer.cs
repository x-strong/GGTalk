using OMCS.Boost.Controls;
using OMCS.Passive.Video;

namespace OMCS.Boost.Forms
{
    partial class VideoChatForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoChatForm));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.skinButton_State = new CCWin.SkinControl.SkinButton();
            this.pictureBox_disMic = new System.Windows.Forms.PictureBox();
            this.pictureBox_disCamera = new System.Windows.Forms.PictureBox();
            this.skinLabel_tip = new CCWin.SkinControl.SkinLabel();
            this.skinPanel_tool = new CCWin.SkinControl.SkinPanel();
            this.decibelDisplayer_speaker = new OMCS.Boost.Controls.DecibelDisplayer();
            this.label_P2P = new System.Windows.Forms.Label();
            this.decibelDisplayer_mic = new OMCS.Boost.Controls.DecibelDisplayer();
            this.skinCheckBox_camera = new CCWin.SkinControl.SkinCheckBox();
            this.skinCheckBox_my = new CCWin.SkinControl.SkinCheckBox();
            this.timerLabel1 = new ESBasic.Widget.TimerLabel();
            this.channelQualityDisplayer1 = new OMCS.Boost.Controls.ChannelQualityDisplayer();
            this.skinCheckBox_mic = new CCWin.SkinControl.SkinCheckBox();
            this.skinCheckBox_speaker = new CCWin.SkinControl.SkinCheckBox();
            this.skinLabel_cameraError = new CCWin.SkinControl.SkinLabel();
            this.skinPanel2 = new CCWin.SkinControl.SkinPanel();
            this.dynamicCameraConnector1 = new OMCS.Passive.Video.DynamicCameraConnector();
            this.microphoneConnector1 = new OMCS.Passive.Audio.MicrophoneConnector(this.components);
            this.dynamicCameraConnector_myself = new OMCS.Passive.Video.DynamicCameraConnector();
            this.cameraPanel1 = new OMCS.Windows.CameraPanel();
            this.cameraPanel_myself = new OMCS.Windows.CameraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_disMic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_disCamera)).BeginInit();
            this.skinPanel_tool.SuspendLayout();
            this.cameraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "GVideoTurnOnVideo.png");
            this.imageList1.Images.SetKeyName(1, "GVideoTurnOffVideo.png");
            this.imageList1.Images.SetKeyName(2, "remind_highlight.png");
            this.imageList1.Images.SetKeyName(3, "GVShareVideoCloseSpk_MouseOver.png");
            this.imageList1.Images.SetKeyName(4, "AM_MenuICON.png");
            this.imageList1.Images.SetKeyName(5, "AV_New_Mic_Style3.png");
            // 
            // skinButton_State
            // 
            this.skinButton_State.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skinButton_State.BackColor = System.Drawing.Color.Transparent;
            this.skinButton_State.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.skinButton_State.BackRectangle = new System.Drawing.Rectangle(4, 4, 4, 4);
            this.skinButton_State.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButton_State.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton_State.DownBack = global::OMCS.Boost.Properties.Resources.allbtn_down;
            this.skinButton_State.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton_State.Image = global::OMCS.Boost.Properties.Resources.HDVideoHangs;
            this.skinButton_State.ImageSize = new System.Drawing.Size(23, 20);
            this.skinButton_State.Location = new System.Drawing.Point(430, 379);
            this.skinButton_State.Margin = new System.Windows.Forms.Padding(0);
            this.skinButton_State.MouseBack = global::OMCS.Boost.Properties.Resources.allbtn_highlight;
            this.skinButton_State.Name = "skinButton_State";
            this.skinButton_State.NormlBack = null;
            this.skinButton_State.Palace = true;
            this.skinButton_State.Size = new System.Drawing.Size(29, 23);
            this.skinButton_State.TabIndex = 129;
            this.skinButton_State.Tag = "1";
            this.toolTip1.SetToolTip(this.skinButton_State, "挂断");
            this.skinButton_State.UseHandCursor = false;
            this.skinButton_State.UseVisualStyleBackColor = false;
            this.skinButton_State.Click += new System.EventHandler(this.skinButton_State_Click);
            this.skinButton_State.MouseEnter += new System.EventHandler(this.FocusCurrent);
            // 
            // pictureBox_disMic
            // 
            this.pictureBox_disMic.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox_disMic.BackgroundImage")));
            this.pictureBox_disMic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_disMic.Location = new System.Drawing.Point(415, 12);
            this.pictureBox_disMic.Name = "pictureBox_disMic";
            this.pictureBox_disMic.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_disMic.TabIndex = 1;
            this.pictureBox_disMic.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_disMic, "对方禁用了麦克风");
            this.pictureBox_disMic.Visible = false;
            this.pictureBox_disMic.MouseEnter += new System.EventHandler(this.FocusCurrent);
            // 
            // pictureBox_disCamera
            // 
            this.pictureBox_disCamera.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_disCamera.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_disCamera.Image")));
            this.pictureBox_disCamera.Location = new System.Drawing.Point(206, 129);
            this.pictureBox_disCamera.Name = "pictureBox_disCamera";
            this.pictureBox_disCamera.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_disCamera.TabIndex = 1;
            this.pictureBox_disCamera.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_disCamera, "对方禁用了摄像头");
            this.pictureBox_disCamera.Visible = false;
            this.pictureBox_disCamera.MouseEnter += new System.EventHandler(this.FocusCurrent);
            // 
            // skinLabel_tip
            // 
            this.skinLabel_tip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.skinLabel_tip.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_tip.AutoSize = true;
            this.skinLabel_tip.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel_tip.BorderColor = System.Drawing.Color.White;
            this.skinLabel_tip.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_tip.Location = new System.Drawing.Point(182, 9);
            this.skinLabel_tip.Name = "skinLabel_tip";
            this.skinLabel_tip.Size = new System.Drawing.Size(149, 17);
            this.skinLabel_tip.TabIndex = 4;
            this.skinLabel_tip.Text = "正在等待对方接收邀请 . . .";
            // 
            // skinPanel_tool
            // 
            this.skinPanel_tool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinPanel_tool.BackColor = System.Drawing.Color.Transparent;
            this.skinPanel_tool.Controls.Add(this.decibelDisplayer_speaker);
            this.skinPanel_tool.Controls.Add(this.label_P2P);
            this.skinPanel_tool.Controls.Add(this.decibelDisplayer_mic);
            this.skinPanel_tool.Controls.Add(this.skinCheckBox_camera);
            this.skinPanel_tool.Controls.Add(this.skinCheckBox_my);
            this.skinPanel_tool.Controls.Add(this.timerLabel1);
            this.skinPanel_tool.Controls.Add(this.channelQualityDisplayer1);
            this.skinPanel_tool.Controls.Add(this.skinCheckBox_mic);
            this.skinPanel_tool.Controls.Add(this.skinCheckBox_speaker);
            this.skinPanel_tool.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinPanel_tool.DownBack = null;
            this.skinPanel_tool.Location = new System.Drawing.Point(5, 369);
            this.skinPanel_tool.MouseBack = null;
            this.skinPanel_tool.Name = "skinPanel_tool";
            this.skinPanel_tool.NormlBack = null;
            this.skinPanel_tool.Size = new System.Drawing.Size(419, 45);
            this.skinPanel_tool.TabIndex = 3;
            this.skinPanel_tool.Visible = false;
            // 
            // decibelDisplayer_speaker
            // 
            this.decibelDisplayer_speaker.BackColor = System.Drawing.Color.White;
            this.decibelDisplayer_speaker.Location = new System.Drawing.Point(3, 7);
            this.decibelDisplayer_speaker.Name = "decibelDisplayer_speaker";
            this.decibelDisplayer_speaker.Size = new System.Drawing.Size(20, 10);
            this.decibelDisplayer_speaker.TabIndex = 10;
            this.decibelDisplayer_speaker.ValueVisible = false;
            this.decibelDisplayer_speaker.Working = true;
            this.decibelDisplayer_speaker.MouseClick += new System.Windows.Forms.MouseEventHandler(this.decibelDisplayer_speaker_MouseClick);
            // 
            // label_P2P
            // 
            this.label_P2P.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_P2P.AutoSize = true;
            this.label_P2P.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_P2P.ForeColor = System.Drawing.Color.Brown;
            this.label_P2P.Location = new System.Drawing.Point(346, 25);
            this.label_P2P.Name = "label_P2P";
            this.label_P2P.Size = new System.Drawing.Size(29, 17);
            this.label_P2P.TabIndex = 2;
            this.label_P2P.Text = "P2P";
            this.label_P2P.Visible = false;
            // 
            // decibelDisplayer_mic
            // 
            this.decibelDisplayer_mic.BackColor = System.Drawing.Color.White;
            this.decibelDisplayer_mic.Location = new System.Drawing.Point(3, 28);
            this.decibelDisplayer_mic.Name = "decibelDisplayer_mic";
            this.decibelDisplayer_mic.Size = new System.Drawing.Size(20, 10);
            this.decibelDisplayer_mic.TabIndex = 10;
            this.decibelDisplayer_mic.ValueVisible = false;
            this.decibelDisplayer_mic.Working = true;
            // 
            // skinCheckBox_camera
            // 
            this.skinCheckBox_camera.AutoSize = true;
            this.skinCheckBox_camera.BackColor = System.Drawing.Color.Transparent;
            this.skinCheckBox_camera.Checked = true;
            this.skinCheckBox_camera.CheckState = System.Windows.Forms.CheckState.Checked;
            this.skinCheckBox_camera.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinCheckBox_camera.DownBack = null;
            this.skinCheckBox_camera.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinCheckBox_camera.Location = new System.Drawing.Point(94, 23);
            this.skinCheckBox_camera.MouseBack = null;
            this.skinCheckBox_camera.Name = "skinCheckBox_camera";
            this.skinCheckBox_camera.NormlBack = null;
            this.skinCheckBox_camera.SelectedDownBack = null;
            this.skinCheckBox_camera.SelectedMouseBack = null;
            this.skinCheckBox_camera.SelectedNormlBack = null;
            this.skinCheckBox_camera.Size = new System.Drawing.Size(63, 21);
            this.skinCheckBox_camera.TabIndex = 2;
            this.skinCheckBox_camera.Text = "摄像头";
            this.skinCheckBox_camera.UseVisualStyleBackColor = false;
            this.skinCheckBox_camera.CheckedChanged += new System.EventHandler(this.skinCheckBox_camera_CheckedChanged);
            // 
            // skinCheckBox_my
            // 
            this.skinCheckBox_my.AutoSize = true;
            this.skinCheckBox_my.BackColor = System.Drawing.Color.Transparent;
            this.skinCheckBox_my.Checked = true;
            this.skinCheckBox_my.CheckState = System.Windows.Forms.CheckState.Checked;
            this.skinCheckBox_my.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinCheckBox_my.DownBack = null;
            this.skinCheckBox_my.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinCheckBox_my.Location = new System.Drawing.Point(94, 4);
            this.skinCheckBox_my.MouseBack = null;
            this.skinCheckBox_my.Name = "skinCheckBox_my";
            this.skinCheckBox_my.NormlBack = null;
            this.skinCheckBox_my.SelectedDownBack = null;
            this.skinCheckBox_my.SelectedMouseBack = null;
            this.skinCheckBox_my.SelectedNormlBack = null;
            this.skinCheckBox_my.Size = new System.Drawing.Size(63, 21);
            this.skinCheckBox_my.TabIndex = 2;
            this.skinCheckBox_my.Text = "小窗口";
            this.skinCheckBox_my.UseVisualStyleBackColor = false;
            this.skinCheckBox_my.CheckedChanged += new System.EventHandler(this.skinCheckBox_my_CheckedChanged);
            // 
            // timerLabel1
            // 
            this.timerLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timerLabel1.AutoSize = true;
            this.timerLabel1.BackColor = System.Drawing.Color.Transparent;
            this.timerLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.timerLabel1.Location = new System.Drawing.Point(376, 25);
            this.timerLabel1.Name = "timerLabel1";
            this.timerLabel1.Size = new System.Drawing.Size(39, 17);
            this.timerLabel1.TabIndex = 7;
            this.timerLabel1.Text = "00:00";
            // 
            // channelQualityDisplayer1
            // 
            this.channelQualityDisplayer1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.channelQualityDisplayer1.BackColor = System.Drawing.Color.Transparent;
            this.channelQualityDisplayer1.ColorBadSignal = System.Drawing.Color.Red;
            this.channelQualityDisplayer1.ColorNoSignal = System.Drawing.Color.LightGray;
            this.channelQualityDisplayer1.ColorSignal = System.Drawing.Color.Green;
            this.channelQualityDisplayer1.Location = new System.Drawing.Point(368, 4);
            this.channelQualityDisplayer1.Name = "channelQualityDisplayer1";
            this.channelQualityDisplayer1.Size = new System.Drawing.Size(45, 16);
            this.channelQualityDisplayer1.TabIndex = 6;
            // 
            // skinCheckBox_mic
            // 
            this.skinCheckBox_mic.AutoSize = true;
            this.skinCheckBox_mic.BackColor = System.Drawing.Color.Transparent;
            this.skinCheckBox_mic.Checked = true;
            this.skinCheckBox_mic.CheckState = System.Windows.Forms.CheckState.Checked;
            this.skinCheckBox_mic.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinCheckBox_mic.DownBack = null;
            this.skinCheckBox_mic.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinCheckBox_mic.Location = new System.Drawing.Point(25, 23);
            this.skinCheckBox_mic.MouseBack = null;
            this.skinCheckBox_mic.Name = "skinCheckBox_mic";
            this.skinCheckBox_mic.NormlBack = null;
            this.skinCheckBox_mic.SelectedDownBack = null;
            this.skinCheckBox_mic.SelectedMouseBack = null;
            this.skinCheckBox_mic.SelectedNormlBack = null;
            this.skinCheckBox_mic.Size = new System.Drawing.Size(63, 21);
            this.skinCheckBox_mic.TabIndex = 2;
            this.skinCheckBox_mic.Text = "麦克风";
            this.skinCheckBox_mic.UseVisualStyleBackColor = false;
            this.skinCheckBox_mic.CheckedChanged += new System.EventHandler(this.skinCheckBox_mic_CheckedChanged);
            // 
            // skinCheckBox_speaker
            // 
            this.skinCheckBox_speaker.AutoSize = true;
            this.skinCheckBox_speaker.BackColor = System.Drawing.Color.Transparent;
            this.skinCheckBox_speaker.Checked = true;
            this.skinCheckBox_speaker.CheckState = System.Windows.Forms.CheckState.Checked;
            this.skinCheckBox_speaker.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinCheckBox_speaker.DownBack = null;
            this.skinCheckBox_speaker.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinCheckBox_speaker.Location = new System.Drawing.Point(29, 3);
            this.skinCheckBox_speaker.MouseBack = null;
            this.skinCheckBox_speaker.Name = "skinCheckBox_speaker";
            this.skinCheckBox_speaker.NormlBack = null;
            this.skinCheckBox_speaker.SelectedDownBack = null;
            this.skinCheckBox_speaker.SelectedMouseBack = null;
            this.skinCheckBox_speaker.SelectedNormlBack = null;
            this.skinCheckBox_speaker.Size = new System.Drawing.Size(63, 21);
            this.skinCheckBox_speaker.TabIndex = 2;
            this.skinCheckBox_speaker.Text = "扬声器";
            this.skinCheckBox_speaker.UseVisualStyleBackColor = false;
            this.skinCheckBox_speaker.CheckedChanged += new System.EventHandler(this.skinCheckBox_speaker_CheckedChanged);
            // 
            // skinLabel_cameraError
            // 
            this.skinLabel_cameraError.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.skinLabel_cameraError.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_cameraError.AutoSize = true;
            this.skinLabel_cameraError.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel_cameraError.BorderColor = System.Drawing.Color.White;
            this.skinLabel_cameraError.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_cameraError.Location = new System.Drawing.Point(182, 109);
            this.skinLabel_cameraError.Name = "skinLabel_cameraError";
            this.skinLabel_cameraError.Size = new System.Drawing.Size(90, 17);
            this.skinLabel_cameraError.TabIndex = 4;
            this.skinLabel_cameraError.Text = "DeviceInUsing";
            this.skinLabel_cameraError.Visible = false;
            // 
            // skinPanel2
            // 
            this.skinPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinPanel2.BackColor = System.Drawing.Color.Transparent;
            this.skinPanel2.BackgroundImage = global::OMCS.Boost.Properties.Resources.VideoWaitToAnswer;
            this.skinPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.skinPanel2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinPanel2.DownBack = null;
            this.skinPanel2.Location = new System.Drawing.Point(5, 4);
            this.skinPanel2.MouseBack = null;
            this.skinPanel2.Name = "skinPanel2";
            this.skinPanel2.NormlBack = null;
            this.skinPanel2.Size = new System.Drawing.Size(122, 91);
            this.skinPanel2.TabIndex = 132;
            this.skinPanel2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.skinPanel2_MouseDoubleClick);
            // 
            // dynamicCameraConnector1
            // 
            this.dynamicCameraConnector1.AutoReconnect = false;
            this.dynamicCameraConnector1.DisplayVideoParameters = false;
            this.dynamicCameraConnector1.JustListen = false;
            this.dynamicCameraConnector1.VideoDrawMode = OMCS.Passive.VideoDrawMode.Fill;
            this.dynamicCameraConnector1.OwnerVideoSizeChanged += new ESBasic.CbGeneric<string, System.Drawing.Size>(this.dynamicCameraConnector1_OwnerCameraVideoSizeChanged);
            this.dynamicCameraConnector1.OwnerOutputChanged += new ESBasic.CbGeneric<string>(this.dynamicCameraConnector1_OwnerOutputChanged);
            this.dynamicCameraConnector1.ConnectEnded += new ESBasic.CbGeneric<string, OMCS.Passive.ConnectResult>(this.dynamicCameraConnector1_ConnectEnded);
            // 
            // microphoneConnector1
            // 
            this.microphoneConnector1.AutoReconnect = false;
            this.microphoneConnector1.Mute = false;
            this.microphoneConnector1.SpringReceivedEventWhenMute = true;
            this.microphoneConnector1.OwnerOutputChanged += new ESBasic.CbGeneric<string>(this.microphoneConnector1_OwnerOutputChanged);
            this.microphoneConnector1.ConnectEnded += new ESBasic.CbGeneric<string, OMCS.Passive.ConnectResult>(this.microphoneConnector1_ConnectEnded);
            // 
            // dynamicCameraConnector_myself
            // 
            this.dynamicCameraConnector_myself.AutoReconnect = false;
            this.dynamicCameraConnector_myself.DisplayVideoParameters = false;
            this.dynamicCameraConnector_myself.JustListen = false;
            this.dynamicCameraConnector_myself.VideoDrawMode = OMCS.Passive.VideoDrawMode.Fill;
            // 
            // cameraPanel1
            // 
            this.cameraPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cameraPanel1.BackColor = System.Drawing.Color.White;
            this.cameraPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cameraPanel1.Controls.Add(this.pictureBox_disMic);
            this.cameraPanel1.Controls.Add(this.pictureBox_disCamera);
            this.cameraPanel1.Controls.Add(this.skinLabel_cameraError); 
            this.cameraPanel1.Location = new System.Drawing.Point(5, 31);
            this.cameraPanel1.Name = "cameraPanel1";
            this.cameraPanel1.Size = new System.Drawing.Size(458, 332);
            this.cameraPanel1.TabIndex = 130;
            this.cameraPanel1.Text = "cameraPanel1"; 
            // 
            // cameraPanel_myself
            // 
            this.cameraPanel_myself.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cameraPanel_myself.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch; 
            this.cameraPanel_myself.Location = new System.Drawing.Point(330, 260);
            this.cameraPanel_myself.Name = "cameraPanel_myself";
            this.cameraPanel_myself.Size = new System.Drawing.Size(133, 103);
            this.cameraPanel_myself.TabIndex = 131;
            this.cameraPanel_myself.Text = "cameraPanel2"; 
            // 
            // VideoChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.BorderPalace = ((System.Drawing.Image)(resources.GetObject("$this.BorderPalace")));
            this.ClientSize = new System.Drawing.Size(470, 419);
            this.CloseDownBack = global::OMCS.Boost.Properties.Resources.btn_close_down;
            this.CloseMouseBack = global::OMCS.Boost.Properties.Resources.btn_close_highlight;
            this.CloseNormlBack = global::OMCS.Boost.Properties.Resources.btn_close_disable;
            this.Controls.Add(this.cameraPanel_myself);
            this.Controls.Add(this.skinLabel_tip);
            this.Controls.Add(this.skinButton_State);
            this.Controls.Add(this.skinPanel_tool);
            this.Controls.Add(this.cameraPanel1);
            this.MaxDownBack = global::OMCS.Boost.Properties.Resources.btn_max_down;
            this.MaxMouseBack = global::OMCS.Boost.Properties.Resources.btn_max_highlight;
            this.MaxNormlBack = global::OMCS.Boost.Properties.Resources.btn_max_normal;
            this.MiniDownBack = global::OMCS.Boost.Properties.Resources.btn_mini_down;
            this.MiniMouseBack = global::OMCS.Boost.Properties.Resources.btn_mini_highlight;
            this.MinimumSize = new System.Drawing.Size(470, 400);
            this.MiniNormlBack = global::OMCS.Boost.Properties.Resources.btn_mini_normal;
            this.Name = "VideoChatForm";
            this.RestoreDownBack = global::OMCS.Boost.Properties.Resources.btn_restore_down;
            this.RestoreMouseBack = global::OMCS.Boost.Properties.Resources.btn_restore_highlight;
            this.RestoreNormlBack = global::OMCS.Boost.Properties.Resources.btn_restore_normal;
            this.ShowDrawIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "正在和David视频会话";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_disMic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_disCamera)).EndInit();
            this.skinPanel_tool.ResumeLayout(false);
            this.skinPanel_tool.PerformLayout();
            this.cameraPanel1.ResumeLayout(false);
            this.cameraPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
       
        private OMCS.Passive.Video.DynamicCameraConnector dynamicCameraConnector1;
        private OMCS.Passive.Audio.MicrophoneConnector microphoneConnector1;       
        private System.Windows.Forms.ImageList imageList1;
        private CCWin.SkinControl.SkinCheckBox skinCheckBox_camera;
        private CCWin.SkinControl.SkinCheckBox skinCheckBox_mic;
        private CCWin.SkinControl.SkinCheckBox skinCheckBox_speaker;
        private CCWin.SkinControl.SkinCheckBox skinCheckBox_my;
        private CCWin.SkinControl.SkinPanel skinPanel_tool;
        private CCWin.SkinControl.SkinLabel skinLabel_tip;
        private CCWin.SkinControl.SkinButton skinButton_State;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBox_disCamera;
        private System.Windows.Forms.PictureBox pictureBox_disMic;
        private ChannelQualityDisplayer channelQualityDisplayer1;
        private ESBasic.Widget.TimerLabel timerLabel1;
        private DecibelDisplayer decibelDisplayer_speaker;
        private DecibelDisplayer decibelDisplayer_mic;
        private CCWin.SkinControl.SkinLabel skinLabel_cameraError;
        private System.Windows.Forms.Label label_P2P;
        private CCWin.SkinControl.SkinPanel skinPanel2;
        private Passive.Video.DynamicCameraConnector dynamicCameraConnector_myself;
        private Windows.CameraPanel cameraPanel1;
        private Windows.CameraPanel cameraPanel_myself;
    }
}