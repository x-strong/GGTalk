using CPF.Controls;
using CPF.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux.Views
{
    internal class BaseWindow :Window
    {
        public BaseWindow()
        {
            this.Initialized += BaseWindow_Initialized;
            this.Icon = GlobalResourceManager.Png64;
        }

        private void BaseWindow_Initialized(object sender, EventArgs e)
        {
            List<TextBox> buttons= new List<TextBox>(this.Find<TextBox>()) ;
            //界面中第一个Textbox聚焦到最后一个字符后
            if (buttons != null && buttons.Count > 0)
            {
                buttons[0].FocusLastIndex();
            }
        }
    }
}
