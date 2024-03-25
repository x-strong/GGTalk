using System;
using System.Collections.Generic;
using System.Text;
using ESBasic;
using System.Windows.Forms;
using TalkBase;


namespace TalkBase.Client.Application
{
    /// <summary>
    /// 用于将聊天窗口合并的容器窗体。
    /// </summary>
    public interface IChatContainerForm
    {
        event CbGeneric<Form ,IUnit> FormCreated;

        void CloseAllForms();
        void CloseForm(string unitID);
        void FlashChatWindow();
        void FocusOnForm(string unitID, bool createNew);
        void OnNewMessage(string unitID);

        List<Form> GetAllForms();
        //string GetCatalog(IUnit unit);
        Form GetExistedForm(string unitID);
        Form GetForm(string unitID);
    }
}
