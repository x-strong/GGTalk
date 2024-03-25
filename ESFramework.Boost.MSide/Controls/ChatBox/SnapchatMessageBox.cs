using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace ESFramework.Boost.Controls
{
    public partial class SnapchatMessageBox : UserControl
    {
        private object snapchatMessage;
        public object Message { get { return this.snapchatMessage; } }

        private string messageID;
        public string MessageID { get { return this.messageID; } }

        private int startIndex;
        public int StartIndex {  get { return this.startIndex; } set { this.startIndex = value; } }
        private int endIndex;
        public int EndIndex { get { return this.endIndex; } }
        public SnapchatMessageBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化阅后自焚控件
        /// </summary>
        /// <param name="message">阅后自焚消息</param>
        /// <param name="messageID">消息ID</param>
        public void Initialze(object message, string messageID)
        {
            this.snapchatMessage = message;
            this.messageID = messageID;
        }

        /// <summary>
        /// 设置该Box在ChatBox中的位置
        /// </summary>
        /// <param name="startIndex">在ChatBox开始行数索引</param>
        /// <param name="endIndex">在ChatBox结束行数索引</param>
        public void SetPostion4InChatBox(int startIndex, int endIndex)
        {
            this.startIndex = startIndex;
            this.endIndex = endIndex;
        }
    }
}
