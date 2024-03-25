using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkBase.Client;

namespace GGTalk
{
    /// <summary>
    /// 唯一Form管理
    /// </summary>
    internal class UniqueFormManager
    {
        private static UniqueFormManager instance;
        private UniqueFormManager() { }
        public static UniqueFormManager GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UniqueFormManager();
                }
                return instance;
            }
        }

        private UpdateUserInfoForm updateUserInfoForm=null;
        public UpdateUserInfoForm OnNewUpdateUserInfoForm(ResourceCenter<GGUser, GGGroup> center)
        {
            if (this.updateUserInfoForm == null)
            {
                this.updateUserInfoForm = new UpdateUserInfoForm(center);
                this.updateUserInfoForm.FormClosed += UpdateUserInfoForm_FormClosed;
            }
            return this.updateUserInfoForm;
        }

        private void UpdateUserInfoForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            this.updateUserInfoForm = null;
        }
    }
}
