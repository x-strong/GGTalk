using ESBasic;
using OMCS.Passive;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OMCS.Boost.Controls
{
    /// <summary>
    /// 通道质量（信号强度）显示器。
    /// 根据IMultimediaConnector的GetChannelQuality方法的返回值转换为信号强度。
    /// </summary>
    public partial class ChannelQualityDisplayer : UserControl
    {       
        public ChannelQualityDisplayer()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.UserPaint, true);//自行绘制            
            this.UpdateStyles();
        }

        private IMultimediaConnector multimediaConnector;      
        public void Initialize(IMultimediaConnector connector)
        {
            this.multimediaConnector = connector;           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(this.multimediaConnector == null)
            {
                return;
            }

            //获取通道质量（从Owner到当前Guest的通道传输质量）。 取值：0~10。值越大，表示通道质量越好。
            int qulity = this.multimediaConnector.GetChannelQuality();
            this.DisplaySignal(qulity);
        }

        #region DisplaySignal     
        private int lastValue = 0;
        // 取值：0~10。值越大，表示通道质量越好。
        private void DisplaySignal(int current)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<int>(this.DisplaySignal), current);
            }
            else
            {
                this.lastValue = current;

                this.label1.BackColor = this.colorNoSignal;
                this.label2.BackColor = this.colorNoSignal;
                this.label3.BackColor = this.colorNoSignal;
                this.label4.BackColor = this.colorNoSignal;
                this.label5.BackColor = this.colorNoSignal;

                if (current <= 0)
                {
                    return;
                }               

                if (current <= 2)
                {
                    this.label1.BackColor = this.colorBadSignal;
                    return;
                }

                if (current <= 4)
                {
                    this.label1.BackColor = this.colorBadSignal;
                    this.label2.BackColor = this.colorBadSignal;
                    return;
                }

                if (current <= 6)
                {
                    this.label1.BackColor = this.colorSignal;
                    this.label2.BackColor = this.colorSignal;
                    this.label3.BackColor = this.colorSignal;
                    return;
                }

                if (current <= 8)
                {
                    this.label1.BackColor = this.colorSignal;
                    this.label2.BackColor = this.colorSignal;
                    this.label3.BackColor = this.colorSignal;
                    this.label4.BackColor = this.colorSignal;
                    return;
                }

                this.label1.BackColor = this.colorSignal;
                this.label2.BackColor = this.colorSignal;
                this.label3.BackColor = this.colorSignal;
                this.label4.BackColor = this.colorSignal;
                this.label5.BackColor = this.colorSignal;
            }
        } 
        #endregion

        #region ColorSignal
        private Color colorSignal = Color.Green;
        /// <summary>
        /// 有信号部分的指示条的颜色。
        /// </summary>
        public Color ColorSignal
        {
            get { return colorSignal; }
            set
            { 
                colorSignal = value;
                this.DisplaySignal(this.lastValue);
            }
        } 
        #endregion

        #region ColorNoSignal
        private Color colorNoSignal = Color.LightGray;
        /// <summary>
        /// 无信号部分的指示条颜色。
        /// </summary>
        public Color ColorNoSignal
        {
            get { return colorNoSignal; }
            set 
            { 
                colorNoSignal = value;
                this.DisplaySignal(this.lastValue);
            }
        } 
        #endregion

        #region ColorBadSignal
        private Color colorBadSignal = Color.Red;
        /// <summary>
        /// 信号差时，有信号部分的指示条的颜色。
        /// </summary>
        public Color ColorBadSignal
        {
            get { return colorBadSignal; }
            set 
            { 
                colorBadSignal = value;
                this.DisplaySignal(this.lastValue);
            }
        }
        #endregion

        
    }
}


