using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GGTalk
{
    /// <summary>
    /// 快捷回复管理。
    /// </summary>
    public partial class QuickAnswerForm : BaseForm
    {
        #region AnswerList
        private List<string> answerList = new List<string>();
        public List<string> AnswerList
        {
            get { return answerList; }
        }

        private List<StringBag> BindList
        {
            get
            {
                List<StringBag> bindList = new List<StringBag>();
                foreach (string val in this.answerList)
                {
                    bindList.Add(new StringBag(val));
                } 
                return bindList;
            }
        }
        #endregion

        public QuickAnswerForm(List<string> list)
        {
            InitializeComponent();
            this.answerList = list;
            this.xDataGridView1.DataSource = this.BindList;
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            string answer = this.textBox1.Text.Trim();
            if (string.IsNullOrEmpty(answer))
            {
                return ;
            }

            this.answerList.Add(answer);
            this.xDataGridView1.DataSource = null;
            this.xDataGridView1.DataSource = this.BindList;
            this.textBox1.Clear();
            this.textBox1.Focus();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.answerList.Count > this.xDataGridView1.SelectedRowIndex)
            {
                this.answerList.RemoveAt(this.xDataGridView1.SelectedRowIndex);
                this.xDataGridView1.DataSource = null;
                this.xDataGridView1.DataSource = this.BindList;
            }
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void xDataGridView1_ItemDoubleClicked(object obj)
        {
            StringBag bag = obj as StringBag;
            if (bag == null)
            {
                return;
            }
            this.textBox1.Text = bag.Target;
            this.answerList.RemoveAt(this.xDataGridView1.SelectedRowIndex);
            this.xDataGridView1.DataSource = null;
            this.xDataGridView1.DataSource = this.BindList;
            this.textBox1.Focus();
        }
    }

    internal class StringBag
    {
        public StringBag() { }
        public StringBag(string val)
        {
            this.target = val;
        }

        private string target;
        public string Target
        {
            get { return target; }
            set { target = value; }
        }
    }
}
