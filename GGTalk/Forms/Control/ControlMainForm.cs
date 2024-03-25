using ESBasic;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;

namespace GGTalk
{
    public partial class ControlMainForm : BaseForm
    {
        private ResourceCenter<GGUser, GGGroup> resourceCenter;

        public ControlMainForm(ResourceCenter<GGUser, GGGroup> center)
        {
            InitializeComponent();
            
            this.resourceCenter = center;
            this.addUserBox1.Initialize(this.resourceCenter.RapidPassiveEngine);
            this.updateUserStateBox1.Initialize(this.resourceCenter);
        }


    }
}
