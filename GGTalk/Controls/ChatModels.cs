using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GGTalk.Controls
{

    /// <summary>
    /// 名片实体
    /// </summary>
    public class PersonCardModel
    {

        /// <summary>
        /// 卡片ID
        /// </summary>
        public int CardID { get; set; }

        /// <summary>
        /// 名片头像
        /// </summary>
        public Image CardImage { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

    }
}
