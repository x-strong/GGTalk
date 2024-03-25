using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESFramework.Boost.Controls;

namespace GGTalk
{
    public partial class AtMessagePanle : UserControl
    {
        public AtMessagePanle()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.UserPaint, true);//自行绘制            
            this.UpdateStyles();
        }

        public void Initialize(string senderName, Image senderImg, ChatBoxContent content)
        {
            this.skinPictureBox1.Image = senderImg;
            this.skinLabel1.Text = senderName + "：";
            if (content == null || content.Text == null)
            {
                return;
            }
            string pureText = content.Text;
            if (content.NonTextItemDictionary != null && content.NonTextItemDictionary.Count > 0)
            {
                //去掉表情和图片的占位符   
                for (int i = content.NonTextItemPositions.Count - 1; i >= 0; i--)
                {
                    uint pos = content.NonTextItemPositions[i];
                    if (pureText.Length > pos && pureText[(int)pos] == ' ')
                    {
                        pureText = pureText.Remove((int)pos, 1);
                        NonTextItem item = content.NonTextItemDictionary[pos];
                        if (item.ChatBoxElementType == ChatBoxElementType.InnerEmotion)
                        {
                            pureText = pureText.Insert((int)(pos), "[表情]");
                        }
                        else if (item.ChatBoxElementType == ChatBoxElementType.ForeignImage)
                        {
                            pureText = pureText.Insert((int)(pos), "[图片]");
                        }
                    }
                }
            }
            else
            {
                pureText = content.Text;
            }
            this.skinLabel1.Text += pureText;
        }


        private void skinPictureBox_del_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void skinPictureBox2_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBox2.BackgroundImage = global::GGTalk.Properties.Resources.delete_btn_pre;
            this.BackColor = Color.WhiteSmoke;
        }

        private void skinPictureBox2_MouseLeave(object sender, EventArgs e)
        {
            this.pictureBox2.BackgroundImage = global::GGTalk.Properties.Resources.delete_btn_nor;
        }
    }
}
