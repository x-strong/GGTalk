using System.Windows.Forms;
namespace GGTalk.LikeQQ.Yes
{
    partial class EditGroupControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.skinButton_select = new CCWin.SkinControl.SkinButton();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 35);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(364, 267);
            this.flowLayoutPanel1.TabIndex = 145;
            this.flowLayoutPanel1.SizeChanged += new System.EventHandler(this.flowLayoutPanel1_SizeChanged);
            // 
            // skinButton_select
            // 
            this.skinButton_select.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skinButton_select.BackColor = System.Drawing.Color.Transparent;
            this.skinButton_select.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButton_select.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton_select.DownBack = global::GGTalk.Properties.Resources.button_frame;
            this.skinButton_select.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton_select.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton_select.Location = new System.Drawing.Point(293, 3);
            this.skinButton_select.MouseBack = global::GGTalk.Properties.Resources.button_frame_pre;
            this.skinButton_select.Name = "skinButton_select";
            this.skinButton_select.NormlBack = global::GGTalk.Properties.Resources.button_frame;
            this.skinButton_select.Palace = true;
            this.skinButton_select.Size = new System.Drawing.Size(62, 27);
            this.skinButton_select.TabIndex = 144;
            this.skinButton_select.Text = "添加";
            this.skinButton_select.UseHandCursor = false;
            this.skinButton_select.UseVisualStyleBackColor = false;
            this.skinButton_select.Click += new System.EventHandler(this.skinButton_select_Click);
            // 
            // EditGroupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.skinButton_select);
            this.Name = "EditGroupControl";
            this.Size = new System.Drawing.Size(364, 302);
            this.ResumeLayout(false);

        }

        #endregion
        private CCWin.SkinControl.SkinButton skinButton_select;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}
