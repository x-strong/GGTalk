using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGTalk.Controls
{
    /// <summary>
    /// 扁平化右侧窗体模版
    /// </summary>
    public interface IFlatControl
    {
        /// <summary>
        /// 面板上方标题
        /// </summary>
        string ControlTitle { get; }

        /// <summary>
        /// 关闭该面板后 会触发 Disposed 事件
        /// </summary>
        void Close();

        /// <summary>
        /// 点击上方标题、或者右上角... 触发展开更多
        /// </summary>
        void ClickMore();
    }
}
