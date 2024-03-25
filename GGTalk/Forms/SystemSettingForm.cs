using CCWin;
using OMCS.Passive;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GGTalk
{
    /// <summary>
    /// 系统设置。
    /// </summary>
    public partial class SystemSettingForm : BaseForm
    {
        public SystemSettingForm()
        {
            InitializeComponent();
            SendMessage(this.btnClose.Handle, BCM_SETSHIELD, 0, (IntPtr)1);

            this.skinRadioButton_hide.Checked = !SystemSettings.Singleton.ExitWhenCloseMainForm;
            this.skinCheckBox_autoRun.Checked = SystemSettings.Singleton.AutoRun;
            this.skinCheckBox_autoLogin.Checked = SystemSettings.Singleton.AutoLogin;
            this.skinCheckBox_ring.Checked = SystemSettings.Singleton.PlayAudio4Message;
            this.skinCheckBox_lastWords.Checked = SystemSettings.Singleton.LoadLastWordsWhenChatFormOpened;

            List<CameraInformation> cameraList = OMCS.Windows.Camera.GetCameras();
            this.skinComboBox_camera.DataSource = cameraList;
            if (SystemSettings.Singleton.WebcamIndex < cameraList.Count)
            {
                this.skinComboBox_camera.SelectedIndex = SystemSettings.Singleton.WebcamIndex;
            }
            else
            {
                SystemSettings.Singleton.WebcamIndex = 0;
                if (cameraList.Count > 0)
                {
                    this.skinComboBox_camera.SelectedIndex = 0;
                }
            }

            List<MicrophoneInformation> micList = OMCS.Windows.SoundDevice.GetMicrophones();
            this.skinComboBox_mic.DataSource = micList;
            if (SystemSettings.Singleton.MicrophoneIndex < micList.Count)
            {
                this.skinComboBox_mic.SelectedIndex = SystemSettings.Singleton.MicrophoneIndex;
            }
            else
            {
                SystemSettings.Singleton.MicrophoneIndex = 0;
                if (micList.Count > 0)
                {
                    this.skinComboBox_mic.SelectedIndex = 0;
                }
            }

            List<SpeakerInformation> speakerList = OMCS.Windows.SoundDevice.GetSpeakers();
            this.skinComboBox_speaker.DataSource = speakerList;
            if (SystemSettings.Singleton.MicrophoneIndex < speakerList.Count)
            {
                this.skinComboBox_speaker.SelectedIndex = SystemSettings.Singleton.SpeakerIndex;
            }
            else
            {
                SystemSettings.Singleton.SpeakerIndex = 0;
                if (micList.Count > 0)
                {
                    this.skinComboBox_speaker.SelectedIndex = 0;
                }
            }

            SystemSettings.Singleton.Save();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                bool autoStartChanged = (this.skinCheckBox_autoRun.Checked != SystemSettings.Singleton.AutoRun);
                SystemSettings.Singleton.AutoRun = this.skinCheckBox_autoRun.Checked;
                SystemSettings.Singleton.WebcamIndex = this.skinComboBox_camera.SelectedIndex;
                SystemSettings.Singleton.MicrophoneIndex = this.skinComboBox_mic.SelectedIndex;
                SystemSettings.Singleton.SpeakerIndex = this.skinComboBox_speaker.SelectedIndex;
                SystemSettings.Singleton.ExitWhenCloseMainForm = !this.skinRadioButton_hide.Checked;
                SystemSettings.Singleton.AutoLogin = this.skinCheckBox_autoLogin.Checked;
                SystemSettings.Singleton.PlayAudio4Message = this.skinCheckBox_ring.Checked;
                SystemSettings.Singleton.LoadLastWordsWhenChatFormOpened = this.skinCheckBox_lastWords.Checked;
                MultimediaManagerFactory.GetSingleton().CameraDeviceIndex = SystemSettings.Singleton.WebcamIndex;
                MultimediaManagerFactory.GetSingleton().MicrophoneDeviceIndex = SystemSettings.Singleton.MicrophoneIndex;
                MultimediaManagerFactory.GetSingleton().SpeakerIndex = SystemSettings.Singleton.SpeakerIndex;
                SystemSettings.Singleton.Save();

                if (autoStartChanged)
                {
                    string name = "GGTalk.exe";
                    string args = string.Format("{0} {1}", name, SystemSettings.Singleton.AutoRun.ToString());
                    Process.Start(AppDomain.CurrentDomain.BaseDirectory + "AutoStart.exe", args);
                }

                //操作注册表，需要使用管理员身份启动程序。
                //ESBasic.Helpers.WindowsHelper.RunWhenStart_usingReg(this.skinCheckBox_autoRun.Checked, "GG2013", Application.ExecutablePath);
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message);
            }
        }

        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, IntPtr lParam);
        public const UInt32 BCM_SETSHIELD = 0x160C;

    }
}
