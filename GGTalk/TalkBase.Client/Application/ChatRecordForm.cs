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
using System.Configuration;
using ESBasic.Security;
using ESPlus.Rapid;
using CCWin.SkinControl;
using ESBasic;
using ESFramework.Boost.Controls;
using TalkBase;
using TalkBase.Client.Core;

namespace TalkBase.Client.Application
{
    /// <summary>
    /// 根据UserID获取Name的接口。
    /// </summary>
    public interface IUserNameGetter
    {
        string GetUserName(string userID);
    }

    /// <summary>
    /// 聊天记录查询窗口。
    /// </summary>
    public partial class ChatRecordForm : BaseForm
    {
        private IResourceCenter resourceCenter;       
        private int totalPageCount = 1;
        private int currentPageIndex = -1;
        private int pageSize = 25; 
        private Parameter<string,string> my; //ID - Name
        private Parameter<string, string> friend;
        private Parameter<string, string> group;
        private bool isGroupChat = false;
        private IUserNameGetter userNameGetter;

        public ChatRecordForm(IResourceCenter center, Parameter<string, string> _my, Parameter<string, string> _friend, Dictionary<uint, Image> emotionDictionary)
        {
            InitializeComponent();

            this.chatBox_history.Initialize(emotionDictionary);
            this.isGroupChat = false;
            this.my = _my;
            this.friend = _friend;
            this.Text += " - " + this.friend.Arg2;
            this.resourceCenter = center;           
        }

        public ChatRecordForm(IResourceCenter center, Parameter<string, string> gr, Parameter<string, string> _my, IUserNameGetter getter, Dictionary<uint, Image> emotionDictionary)
        {
            InitializeComponent();

            this.chatBox_history.Initialize(emotionDictionary);
            this.isGroupChat = true;
            this.group = gr;
            this.my = _my;
            this.userNameGetter = getter;
            this.Text = "群消息记录 - " + gr.Arg2;
            this.resourceCenter = center;        
        }

        #region ServerRecordEnabled
        public bool ServerRecordEnabled
        {
            get
            {
                return this.skinRadioButton_Server.Visible;
            }
            set
            {
                this.skinRadioButton1.Checked = true;
                this.skinRadioButton_Server.Visible = false;
                this.skinRadioButton1.Visible = false;
            }
        } 
        #endregion

        private IChatRecordGetter CurrentChatRecordGetter
        {
            get
            {
                if (this.skinRadioButton_Server.Checked)
                {
                    return this.resourceCenter.RemoteChatRecordGetter;
                }

                return this.resourceCenter.LocalChatRecordPersister;
            }
        }

        private void MessageRecordForm_Shown(object sender, EventArgs e)
        {
            this.skinComboBox1.SelectedIndex = 0;            
        }

        private void ShowRecord(int pageIndex)
        {
            this.ShowRecord(pageIndex, true);
        }

        private void ShowRecord(int pageIndex ,bool allowCache)
        {
            if (pageIndex != int.MaxValue)
            {
                if (pageIndex + 1 > this.totalPageCount)
                {
                    pageIndex = this.totalPageCount - 1;
                }

                if (pageIndex < 0)
                {
                    pageIndex = 0;
                }
                if (this.currentPageIndex == pageIndex && allowCache)
                {
                    return;
                }
            }

            this.Cursor = Cursors.WaitCursor;
            try
            {
                ChatRecordTimeScope timeScope = ChatRecordTimeScope.All;
                DateTime now = DateTime.Now ;
                if (this.skinComboBox1.SelectedIndex == 0) //一周
                {
                    timeScope = ChatRecordTimeScope.RecentWeek;
                }
                else if (this.skinComboBox1.SelectedIndex == 1)//一月
                {
                    timeScope = ChatRecordTimeScope.RecentMonth;
                }
                else if (this.skinComboBox1.SelectedIndex == 2)//三月
                {
                    timeScope = ChatRecordTimeScope.Recent3Month;
                }
                else //全部
                {
                }

                
                ChatRecordPage page = null;
                if (this.isGroupChat)
                {
                    page = this.CurrentChatRecordGetter.GetGroupChatRecordPage(timeScope, this.group.Arg1, this.pageSize, pageIndex);
                }
                else
                {
                    page = this.CurrentChatRecordGetter.GetChatRecordPage(timeScope, my.Arg1, friend.Arg1, this.pageSize, pageIndex);
                }
                this.chatBox_history.Clear(); 

                if (page == null || page.Content.Count == 0)
                {
                    MessageBoxEx.Show("没有消息记录！");                   
                    return;
                }

                
                
                for (int i = 0; i < page.Content.Count; i++)
                {
                    ChatMessageRecord record = page.Content[i];
                    byte[] msg = record.Content;
                    if (this.skinRadioButton_Server.Checked)
                    {
                        if (this.resourceCenter.DesEncryption != null)
                        {
                            msg = this.resourceCenter.DesEncryption.Decrypt(msg);
                        }
                    }
                    ChatBoxContent content = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(msg, 0);

                    if (this.isGroupChat)
                    {
                        if (record.SpeakerID == this.my.Arg1)
                        {
                            this.AppendChatBoxContent(record.OccureTime, string.Format("{0}({1})", this.my.Arg2, record.SpeakerID), content, Color.Green);
                        }
                        else
                        {
                            string name = this.userNameGetter.GetUserName(record.SpeakerID) ?? record.SpeakerID;                            
                            this.AppendChatBoxContent(record.OccureTime, string.Format("{0}({1})", name, record.SpeakerID), content, Color.Blue);
                        }
                    }
                    else
                    {
                        if (record.SpeakerID == this.my.Arg1)
                        {
                            this.AppendChatBoxContent(record.OccureTime, this.my.Arg2, content, Color.Green);
                        }
                        else
                        {
                            string name = this.friend.Arg2;//content.NonTextItemDictionary.Count > 0 ? "系统" : this.friend.Arg2;
                            this.AppendChatBoxContent(record.OccureTime, name, content, Color.Blue);
                        }
                    }                    
                }

                //this.chatBox_history.SelectionStart = 0;
                //this.chatBox_history.ScrollToCaret();

                int pageCount = page.TotalCount / this.pageSize;
                if (page.TotalCount % this.pageSize > 0)
                {
                    ++pageCount;
                }
                this.currentPageIndex = page.PageIndex;
                this.totalPageCount = pageCount;
                this.SetPageIndexButtonEnabled();



            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void SetPageIndexButtonEnabled()
        {
            this.toolStripButton1.Enabled = true;
            this.toolStripButton2.Enabled = true;
            this.toolStripButton3.Enabled = true;
            this.toolStripButton4.Enabled = true;
            if (this.currentPageIndex == 0) {
                this.toolStripButton3.Enabled = false;
                this.toolStripButton4.Enabled = false;
            }
            if (this.currentPageIndex + 1 == this.totalPageCount)
            {
                this.toolStripButton1.Enabled = false;
                this.toolStripButton2.Enabled = false;
            }
        }

        #region AppendMessage
        private void AppendChatBoxContent(DateTime showTime, string userName, ChatBoxContent content, Color color)
        {
            this.chatBox_history.AppendRichText(string.Format("{0}  {1}", userName, showTime), new Font(this.Font, FontStyle.Regular), color);
            this.chatBox_history.AppendText("    ");
            this.chatBox_history.AppendChatBoxContent(content);
            this.chatBox_history.Select(this.chatBox_history.Text.Length, 0);
            this.chatBox_history.ScrollToCaret();
        }
        #endregion

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.ShowRecord(this.totalPageCount - 1);
        }       

        private void skinButton_last_Click(object sender, EventArgs e)
        {
            this.ShowRecord(0);
        }

        private void skinButton_pre_Click(object sender, EventArgs e)
        {            
            this.ShowRecord(this.currentPageIndex + 1);
        }

        private void skinButton_next_Click(object sender, EventArgs e)
        {
            this.ShowRecord(this.currentPageIndex - 1);
        }       

        

        private void skinRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.ShowRecord(0, false);
        }


        private void skinComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ShowRecord(0, false);
        }      
    }
}
