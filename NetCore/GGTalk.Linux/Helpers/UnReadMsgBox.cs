using ESBasic.ObjectManagement.Managers;
using ESPlus.Serialization;
using GGTalk.Linux.Controls;
using System.Collections.Generic;
using TalkBase;

namespace GGTalk.Linux.Helpers
{
    internal class UnReadMsgBox
    {

        #region ctor

        private UnReadMsgBox()
        {
            this.objectManager = new ObjectManager<string, List<UnReadMsgModel>>();
        }

        #endregion

        #region attr

        private ObjectManager<string, List<UnReadMsgModel>> objectManager;


        private static UnReadMsgBox Single;

        public static UnReadMsgBox Singleton
        {
            get
            {
                if (Single == null)
                {
                    Single = new UnReadMsgBox();
                }
                return Single;
            }
        }

        #endregion

        internal void Add(string destUnitID,string speakerID, byte[] content)
        {
            List<UnReadMsgModel> list = this.objectManager.Get(destUnitID);
            if (list == null) list = new List<UnReadMsgModel>();
            IUnit unit = Program.ResourceCenter.ClientGlobalCache.GetUnit(destUnitID);
            if (unit == null) return;
            UnReadMsgModel model = new UnReadMsgModel(unit, content);
            list.Add(model);
            this.objectManager.Add(destUnitID, list);
            LastWordsRecord lastRecord = new LastWordsRecord(Program.ResourceCenter.CurrentUserID, destUnitID, unit.UnitType == UnitType.Group, content);
            unit.LastWordsRecord = lastRecord;
            CommonHelper.LastWordsChanged(unit,true);
        }

        internal void Remove(string destUnitID)
        {
            this.objectManager.Remove(destUnitID);
        } 

        internal List<UnReadMsgModel> Get(string destUnitID)
        {
            return this.objectManager.Get(destUnitID);
        }


    }

    public class UnReadMsgModel
    {

        internal UnReadMsgModel(IUnit unit, byte[] bytes)
        {
            this.unit = unit;
            this.bytes = bytes;
        }

        internal IUnit unit;

        internal byte[] bytes; 

    }
}
