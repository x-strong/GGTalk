using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GGTalk.Controls
{
    public partial class SkinPanel2 : SkinPanel
    {
        private Image Normal_BackgroundImage { get; set; }

        /// <summary>
        /// 鼠标停放在控件上发生
        /// </summary>
        public Image MouseBack_BackgroundImage { get; set; }

        /// <summary>
        /// 点击控件时发生
        /// </summary>
        public Image MouseEnter_BackgroundImage { get; set; }


        private Color Normal_BackgroundColor = Color.Transparent;

        /// <summary>
        /// 鼠标停放在控件上发生
        /// </summary>
        public Color MouseBack_BackgroundColor { get; set; }

        /// <summary>
        /// 点击控件时发生
        /// </summary>
        public Color MouseEnter_BackgroundColor { get; set; }

        //是否启用鼠标停放在控件上改变控件状态（背景色、背景图） 默认为true
        private bool mouseHoverChangedEnable = true;


        private string tips = string.Empty;
        /// <summary>
        /// Tip说明
        /// </summary>
        public string Tips { get { return this.tips; } set { this.tips = value; this.toolTip.SetToolTip(this, value); } }

        ToolTip toolTip = new ToolTip();
        public SkinPanel2():base()
        {
            this.Normal_BackgroundImage = this.BackgroundImage;
            this.MouseHover += SkinPanel2_MouseHover;
            this.MouseDown += SkinPanel2_MouseDown;
            this.MouseLeave += SkinPanel2_MouseLeave;
        }

        private bool isFirstBackgroundImage = true;
        protected override void OnBackgroundImageChanged(EventArgs e)
        {
            base.OnBackgroundImageChanged(e);
            if (isFirstBackgroundImage)
            {
                isFirstBackgroundImage = false;
                this.Normal_BackgroundImage = this.BackgroundImage;
            }
        }


        private bool isFirstBackgroundColor = true;
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            if (isFirstBackgroundColor)
            {
                isFirstBackgroundColor = false;
                this.Normal_BackgroundColor = this.BackColor;
            }
        }


        private void SkinPanel2_MouseLeave(object sender, EventArgs e)
        {            
            this.BackgroundImage = this.Normal_BackgroundImage;
            this.BackColor = this.Normal_BackgroundColor;
        }

        private void SkinPanel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.MouseEnter_BackgroundImage != null)
            {
                this.BackgroundImage = this.MouseEnter_BackgroundImage;
            }
            if (this.MouseEnter_BackgroundColor != null)
            {
                this.BackColor = this.MouseEnter_BackgroundColor;
            }
        }

        private void SkinPanel2_MouseHover(object sender, EventArgs e)
        {
            if (!this.mouseHoverChangedEnable) { return; }
            if (this.MouseBack_BackgroundImage != null)
            {
                this.BackgroundImage = this.MouseBack_BackgroundImage;
            }
            if (this.MouseBack_BackgroundColor != null)
            {
                this.BackColor = this.MouseBack_BackgroundColor;
            }
        }

        /// <summary>
        /// 设置默认正常状态的背景图片
        /// </summary>
        /// <param name="normalBackgroundImage">正常状态下显示的背景图</param>
        /// <param name="_mouseHoverChangedEnable">鼠标移到控件上是否改变控件状态（背景色、背景图）</param>
        public void SetBackgroundImage(Image normalBackgroundImage,bool _mouseHoverChangedEnable)
        {
            this.mouseHoverChangedEnable = _mouseHoverChangedEnable;
            this.Normal_BackgroundImage = normalBackgroundImage;
            this.BackgroundImage = normalBackgroundImage;
        }

        /// <summary>
        /// 设置正常状态下的背景颜色
        /// </summary>
        /// <param name="normalBackgroundColor"></param>
        public void SetBackgroundColor(Color normalBackgroundColor)
        {
            this.Normal_BackgroundColor = normalBackgroundColor;
            this.BackColor = normalBackgroundColor;
        }
    }
}
